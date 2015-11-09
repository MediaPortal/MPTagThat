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
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat.GridView
{
  public partial class GridViewConvert : UserControl
  {
    #region Variables

    private readonly Main _main;

    private readonly SortableBindingList<ConversionData> _bindingList = new SortableBindingList<ConversionData>();
    private readonly GridViewColumnsConvert _gridColumns;
    private readonly ILocalisation _localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger _log = ServiceScope.Get<ILogger>().GetLogger;
    private Thread _threadConvert;
    private bool _conversionActive = false;
    private bool _abortProcessing;

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
        if (_threadConvert == null)
          return false;

        if (_threadConvert.ThreadState == ThreadState.Running)
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

      _abortProcessing = false;

      // Listen to Messages
      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      IMessageQueue queueMessageEncoding = ServiceScope.Get<IMessageBroker>().GetOrCreate("encoding");
      queueMessageEncoding.OnMessageReceive += OnMessageReceiveEncoding;

      // Load the Settings
      _gridColumns = new GridViewColumnsConvert();

      dataGridViewConvert.AutoGenerateColumns = false;
      dataGridViewConvert.DataSource = _bindingList;

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
      _threadConvert = new Thread(ConversionThread);
      _threadConvert.Name = "Conversion";
      _threadConvert.Priority = ThreadPriority.Highest;
      _threadConvert = new Thread(ConversionThread);
      _threadConvert.Start();
    }

    /// <summary>
    ///   Cancel the Conversion Process
    /// </summary>
    public void ConvertFilesCancel()
    {
      _abortProcessing = true;
      _conversionActive = false;
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
      _bindingList.Add(convdata);
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

      _log.Trace(">>>");
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
        MessageBox.Show(_localisation.ToString("Conversion", "ErrorDirectory"), _localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
        _log.Error("Error creating Conversion output directory: {0}. {1}", rootFolder, ex.Message);
        return;
      }

      string encoder = null;

      if (_main.EncoderCombo.SelectedItem != null)
      {
        var item = _main.EncoderCombo.SelectedItem as Item;
        if (item != null)
          encoder = (string)item.Value;
      }

      if (encoder == null)
        return;

      foreach (DataGridViewRow row in dataGridViewConvert.Rows)
      {
        // Reset the Status field to 0
        row.Cells[0].Value = 0;
      }

      int maxThreads;
      int maxComplThreads;
      ThreadPool.GetAvailableThreads(out maxThreads, out maxComplThreads);

      ThreadPool.SetMaxThreads(Environment.ProcessorCount, maxComplThreads);

      foreach (DataGridViewRow row in dataGridViewConvert.Rows)
      {
        ThreadPool.QueueUserWorkItem((f =>
        {
          if (_abortProcessing)
            return;

          _conversionActive = true;

          var conversionData = _bindingList[row.Index];
          conversionData.RootFolder = rootFolder;
          conversionData.Encoder = encoder;

          object[] parameters = { "Convert", conversionData };
          Commands.Command commandObj = Commands.Command.Create(parameters);
          if (commandObj == null)
          {
            _conversionActive = false;
            return;
          }

          var track = conversionData.Track;
          commandObj.Execute(ref track, row.Index);

          _conversionActive = false;
        }
        ));
      }

      Options.MainSettings.LastConversionEncoderUsed = encoder;
      _log.Trace("<<<");
    }

    #endregion

    #region Gridlayout

    /// <summary>
    ///   Create the Columns of the Grid based on the users setting
    /// </summary>
    private void CreateColumns()
    {
      // Now create the columns 
      foreach (GridViewColumn column in _gridColumns.Settings.Columns)
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

        _gridColumns.SaveColumnSettings(column, i);
        i++;
      }
      _gridColumns.SaveSettings();
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
    private void LanguageChanged()
    {
      LocaliseScreen();
    }

    private void LocaliseScreen()
    {
      // Update the column Headings
      foreach (DataGridViewColumn col in dataGridViewConvert.Columns)
      {
        col.HeaderText = _localisation.ToString("column_header", col.Name);
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
      if (!_conversionActive)
      {
        return; // Prevent collision with Rip
      }

      if (InvokeRequired)
      {
        ThreadSafeMessageDelegate d = OnMessageReceiveEncoding;
        Invoke(d, new object[] { message });
        return;
      }

      string action = message.MessageData["action"] as string;
      int rowIndex = (int)message.MessageData["rowindex"];

      if (action != null)
        switch (action.ToLower())
        {
          case "progress":
            double percentComplete = (double)message.MessageData["percent"];
            dataGridViewConvert.Rows[rowIndex].Cells[0].Value = (int)percentComplete;
            break;

          case "newfilename":
            _bindingList[rowIndex].NewFileName = (string)message.MessageData["filename"];
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

      if (action != null)
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