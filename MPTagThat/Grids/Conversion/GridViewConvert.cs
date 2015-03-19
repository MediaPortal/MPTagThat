#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion
#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.AudioEncoder;
using Un4seen.Bass;
using File = TagLib.File;

#endregion

namespace MPTagThat.GridView
{
  public partial class GridViewConvert : UserControl
  {
    #region Variables

    private readonly Main _main;
    private readonly IAudioEncoder audioEncoder;

    private readonly SortableBindingList<ConversionData> bindingList = new SortableBindingList<ConversionData>();
    private readonly GridViewColumnsConvert gridColumns;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private Thread threadConvert;

    #region Nested type: ThreadSafeMessageDelegate

    private delegate void ThreadSafeMessageDelegate(QueueMessage message);

    #endregion

    #region Nested type: ThreadSafeConvertDelegate

    private delegate void ThreadSafeConvertDelegate();

    #endregion

    #endregion

    #region Properties

    public Color BackGroundColor
    {
      set { BackColor = value; }
    }

    public bool Converting
    {
      get
      {
        if (threadConvert == null)
          return false;

        if (threadConvert.ThreadState == ThreadState.Running)
          return true;
        else
          return false;
      }
    }

    public DataGridView View
    {
      get { return dataGridViewConvert; }
    }

    #endregion

    #region ctor

    public GridViewConvert(Main main)
    {
      _main = main;

      InitializeComponent();

      // Listen to Messages
      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      IMessageQueue queueMessageEncoding = ServiceScope.Get<IMessageBroker>().GetOrCreate("encoding");
      queueMessageEncoding.OnMessageReceive += OnMessageReceiveEncoding;

      audioEncoder = ServiceScope.Get<IAudioEncoder>();

      // Load the Settings
      gridColumns = new GridViewColumnsConvert();

      dataGridViewConvert.AutoGenerateColumns = false;
      dataGridViewConvert.DataSource = bindingList;

      // Now Setup the columns, we want to display
      CreateColumns();

      CreateContextMenu();
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Converts the selected files in the Grid
    /// </summary>
    public void ConvertFiles()
    {
      if (threadConvert == null)
      {
        threadConvert = new Thread(ConversionThread);
        threadConvert.Name = "Conversion";
      }

      if (threadConvert.ThreadState != ThreadState.Running)
      {
        threadConvert.Priority = ThreadPriority.Highest;
        threadConvert = new Thread(ConversionThread);
        threadConvert.Start();
      }
    }

    /// <summary>
    ///   Cancel the Ripping Process
    /// </summary>
    public void ConvertFilesCancel()
    {
      if (threadConvert != null)
      {
        threadConvert.Abort();
      }
    }

    /// <summary>
    ///   Add the Track to the Conversion Grid
    /// </summary>
    /// <param name = "track"></param>
    public void AddToConvert(TrackData track)
    {
      if (track == null)
        return;

      ConversionData convdata = new ConversionData {Track = track};
      bindingList.Add(convdata);
    }

    #endregion

    #region Private Methods

    #region Conversion

    private void ConversionThread()
    {
      if (_main.InvokeRequired)
      {
        ThreadSafeConvertDelegate d = ConversionThread;
        _main.Invoke(d);
        return;
      }

      log.Trace(">>>");
      string rootFolder = _main.EncoderOutputDirectory;
      if (string.IsNullOrEmpty(rootFolder))
      {
        rootFolder = Options.MainSettings.RipTargetFolder;
      }

      try
      {
        if (!Directory.Exists(rootFolder) && !string.IsNullOrEmpty(rootFolder))
          Directory.CreateDirectory(rootFolder);
      }
      catch (Exception ex)
      {
        MessageBox.Show(localisation.ToString("Conversion", "ErrorDirectory"), localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
        log.Error("Error creating Conversion output directory: {0}. {1}", rootFolder, ex.Message);
        return;
      }

      string encoder = null;

      if (_main.EncoderCombo.SelectedItem != null)
      {
        encoder = (string)(_main.EncoderCombo.SelectedItem as Item).Value;
      }

      if (encoder == null)
        return;

      foreach (DataGridViewRow row in dataGridViewConvert.Rows)
      {
        // Reset the Status field to 0
        row.Cells[0].Value = 0;
      }

      /*
      ParallelOptions po = new ParallelOptions();
      po.MaxDegreeOfParallelism = Environment.ProcessorCount;

      Parallel.ForEach(dataGridViewConvert.Rows.OfType<DataGridViewRow>(), po, row =>
      {
        var conversionData = bindingList[row.Index];
        conversionData.RootFolder = rootFolder;
        conversionData.Encoder = encoder;

        object[] parameters = { "Convert", conversionData };
        Commands.Command commandObj = Commands.Command.Create(parameters);
        if (commandObj == null)
        {
          return;
        }

        var track = conversionData.Track;
        commandObj.Execute(ref track, row.Index);
      }
      );
      */
      
      int maxThreads = 0;
      int maxComplThreads = 0;
      ThreadPool.GetAvailableThreads(out maxThreads, out maxComplThreads);

      ThreadPool.SetMaxThreads(Environment.ProcessorCount, maxComplThreads);

      foreach (DataGridViewRow row in dataGridViewConvert.Rows)
      {
        ThreadPool.QueueUserWorkItem((f =>
        {
          var conversionData = bindingList[row.Index];
          conversionData.RootFolder = rootFolder;
          conversionData.Encoder = encoder;

          object[] parameters = { "Convert", conversionData };
          Commands.Command commandObj = Commands.Command.Create(parameters);
          if (commandObj == null)
          {
            return;
          }

          var track = conversionData.Track;
          commandObj.Execute(ref track, row.Index);
        }
        ));
      }



      /*
      foreach (DataGridViewRow row in dataGridViewConvert.Rows)
      {
        var conversionData = bindingList[row.Index];
        conversionData.RootFolder = rootFolder;
        conversionData.Encoder = encoder;

        object[] parameters = { "Convert", conversionData };
        Commands.Command commandObj = Commands.Command.Create(parameters);
        if (commandObj == null)
        {
          return;
        }

        var track = conversionData.Track;
        commandObj.Execute(ref track, row.Index);
      }
      */

      Options.MainSettings.LastConversionEncoderUsed = encoder;
      log.Trace("<<<");
    }

    private IEnumerable<DataGridViewRow> GetRows()
    {
      return dataGridViewConvert.Rows.Cast<DataGridViewRow>();
    }

    #endregion

    #region Gridlayout

    /// <summary>
    ///   Create the Columns of the Grid based on the users setting
    /// </summary>
    private void CreateColumns()
    {
      // Now create the columns 
      foreach (GridViewColumn column in gridColumns.Settings.Columns)
      {
        dataGridViewConvert.Columns.Add(Util.FormatGridColumn(column));
      }

      // Add a dummy column and set the property of the last column to fill
      DataGridViewColumn col = new DataGridViewTextBoxColumn();
      col.Name = "dummy";
      col.HeaderText = "";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 5;
      dataGridViewConvert.Columns.Add(col);
      dataGridViewConvert.Columns[dataGridViewConvert.Columns.Count - 1].AutoSizeMode =
        DataGridViewAutoSizeColumnMode.Fill;
    }

    /// <summary>
    ///   Save the settings
    /// </summary>
    private void SaveSettings()
    {
      // Save the Width of the Columns
      int i = 0;
      foreach (DataGridViewColumn column in dataGridViewConvert.Columns)
      {
        // Don't save the dummy column
        if (i == dataGridViewConvert.Columns.Count - 1)
          break;

        gridColumns.SaveColumnSettings(column, i);
        i++;
      }
      gridColumns.SaveSettings();
    }

    /// <summary>
    ///   Create Context Menu
    /// </summary>
    private void CreateContextMenu()
    {
      // Build the Context Menu for the Grid
      MenuItem[] rmitems = new MenuItem[1];
      rmitems[0] = new MenuItem();
      rmitems[0].Text = "Clear List";
      rmitems[0].Click += dataGridViewConvert_ClearList;
      rmitems[0].DefaultItem = true;
      dataGridViewConvert.ContextMenu = new ContextMenu(rmitems);
    }

    #endregion

    #region Localisation

    /// <summary>
    ///   Language Change event has been fired. Apply the new language
    /// </summary>
    /// <param name = "language"></param>
    private void LanguageChanged()
    {
      LocaliseScreen();
    }

    private void LocaliseScreen()
    {
      // Update the column Headings
      foreach (DataGridViewColumn col in dataGridViewConvert.Columns)
      {
        col.HeaderText = localisation.ToString("column_header", col.Name);
      }
    }

    #endregion

    #endregion

    #region Event Handler

    /// <summary>
    ///   Handle Messages from the Audio Encoder
    /// </summary>
    /// <param name = "message"></param>
    private void OnMessageReceiveEncoding(QueueMessage message)
    {
      if (InvokeRequired)
      {
        ThreadSafeMessageDelegate d = OnMessageReceiveEncoding;
        Invoke(d, new[] { message });
        return;
      }

      string action = message.MessageData["action"] as string;
      int rowIndex = (int)message.MessageData["rowindex"];

      switch (action.ToLower())
      {
        case "progress":
          double percentComplete = (double)message.MessageData["percent"];
          dataGridViewConvert.Rows[rowIndex].Cells[0].Value = (int)percentComplete;
          break;

        case "newfilename":
          bindingList[rowIndex].NewFileName = (string)message.MessageData["filename"];
          dataGridViewConvert.Refresh();
          break;

        case "error":
          {
            dataGridViewConvert.Rows[rowIndex].Cells[0].Value = (string)message.MessageData["error"];
            dataGridViewConvert.Rows[rowIndex].Cells[0].ToolTipText = (string)message.MessageData["tooltip"];
            break;
          }
      }

      dataGridViewConvert.Update();
      Application.DoEvents();
    }

    /// <summary>
    ///   Handle Messages
    /// </summary>
    /// <param name = "message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "languagechanged":
          LanguageChanged();
          Refresh();
          break;

        case "themechanged":
          {
            dataGridViewConvert.BackgroundColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            break;
          }
      }
    }

    /// <summary>
    ///   Handle Right Mouse Click to open the context Menu in the Grid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void dataGridViewConvert_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        dataGridViewConvert.ContextMenu.Show(dataGridViewConvert, new Point(e.X, e.Y));
    }

    /// <summary>
    ///   Context Menu entry has been selected
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    private void dataGridViewConvert_ClearList(object o, EventArgs e)
    {
      dataGridViewConvert.Rows.Clear();
    }

    #endregion
  }
}