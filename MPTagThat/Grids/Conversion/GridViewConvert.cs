#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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
using System.Threading;
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
    private int _currentRow = -1;
    private Thread threadConvert;

    #region Nested type: ThreadSafeAddErrorDelegate

    private delegate void ThreadSafeAddErrorDelegate(string file, string message);

    #endregion

    #region Nested type: ThreadSafeRefreshDelegate

    private delegate void ThreadSafeRefreshDelegate(string fileName);

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
        threadConvert.Name = "Ripping";
      }

      if (threadConvert.ThreadState != ThreadState.Running)
      {
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
        _currentRow = -1;
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

      ConversionData convdata = new ConversionData();
      convdata.Track = track;

      bindingList.Add(convdata);
    }

    #endregion

    #region Private Methods

    #region Conversion

    private void ConversionThread()
    {
      if (_main.MainRibbon.InvokeRequired)
      {
        ThreadSafeConvertDelegate d = ConversionThread;
        _main.MainRibbon.Invoke(d);
        return;
      }

      log.Trace(">>>");
      string rootFolder = _main.MainRibbon.EncoderOutputDirectory;
      if (string.IsNullOrEmpty(rootFolder))
      {
        rootFolder = Options.MainSettings.RipTargetFolder;
      }

      string encoder = null;

      List<Item> encoders = (List<Item>)_main.MainRibbon.EncoderCombo.DataSource;
      if (_main.MainRibbon.EncoderCombo.SelectedItem != null)
      {
        encoder = (string)encoders[_main.MainRibbon.EncoderCombo.SelectedIndex].Value;
      }

      if (encoder == null)
        return;

      try
      {
        if (!Directory.Exists(rootFolder) && !string.IsNullOrEmpty(rootFolder))
          Directory.CreateDirectory(rootFolder);
      }
      catch (Exception ex)
      {
        _main.ErrorGridView.Rows.Add("", localisation.ToString("Conversion", "ErrorDirectory"));
        log.Error("Error creating Conversion output directory: {0}. {1}", rootFolder, ex.Message);
        return;
      }

      foreach (DataGridViewRow row in dataGridViewConvert.Rows)
      {
        // Reset the Status field to 0
        row.Cells[0].Value = 0;
      }

      _currentRow = -1;
      foreach (DataGridViewRow row in dataGridViewConvert.Rows)
      {
        _currentRow = row.Index;

        ConversionData track = bindingList[_currentRow];

        string inputFile = track.Track.FullFileName;
        string outFile = Util.ReplaceParametersWithTrackValues(Options.MainSettings.RipFileNameFormat, track.Track);
        outFile = Path.Combine(rootFolder, outFile);
        string directoryName = Path.GetDirectoryName(outFile);

        // Now check the validity of the directory
        if (!Directory.Exists(directoryName))
        {
          try
          {
            Directory.CreateDirectory(directoryName);
          }
          catch (Exception e1)
          {
            log.Error("Error creating folder: {0} {1]", directoryName, e1.Message);
            row.Cells[0].Value = localisation.ToString("message", "Error");
            AddErrorMessage(directoryName,
                            String.Format("{0}: {1}", localisation.ToString("message", "Error"), e1.Message));
            continue; // Process next row
          }
        }

        outFile = audioEncoder.SetEncoder(encoder, outFile);
        UpdateNewFileName(outFile);

        if (inputFile == outFile)
        {
          AddErrorMessage(inputFile, localisation.ToString("Conversion", "SameFile"));
          log.Error("No conversion for {0}. Output would overwrite input", inputFile);
          continue;
        }

        int stream = Bass.BASS_StreamCreateFile(inputFile, 0, 0, BASSFlag.BASS_STREAM_DECODE);
        if (stream == 0)
        {
          AddErrorMessage(inputFile, localisation.ToString("Conversion", "OpenFileError"));
          log.Error("Error creating stream for file {0}. Error: {1}", inputFile,
                    Enum.GetName(typeof (BASSError), Bass.BASS_ErrorGetCode()));
          continue;
        }

        log.Info("Convert file {0} -> {1}", inputFile, outFile);

        if (audioEncoder.StartEncoding(stream) != BASSError.BASS_OK)
        {
          AddErrorMessage(inputFile, localisation.ToString("Conversion", "EncodingFileError"));
          log.Error("Error starting Encoder for File {0}. Error: {1}", inputFile,
                    Enum.GetName(typeof (BASSError), Bass.BASS_ErrorGetCode()));
          Bass.BASS_StreamFree(stream);
          continue;
        }

        dataGridViewConvert.Rows[_currentRow].Cells[0].Value = 100;

        Bass.BASS_StreamFree(stream);

        try
        {
          // Now Tag the encoded File
          File tagInFile = File.Create(inputFile);
          File tagOutFile = File.Create(outFile);
          tagOutFile.Tag.AlbumArtists = tagInFile.Tag.AlbumArtists;
          tagOutFile.Tag.Album = tagInFile.Tag.Album;
          tagOutFile.Tag.Genres = tagInFile.Tag.Genres;
          tagOutFile.Tag.Year = tagInFile.Tag.Year;
          tagOutFile.Tag.Performers = tagInFile.Tag.Performers;
          tagOutFile.Tag.Track = tagInFile.Tag.Track;
          tagOutFile.Tag.TrackCount = tagInFile.Tag.TrackCount;
          tagOutFile.Tag.Title = tagInFile.Tag.Title;
          tagOutFile.Tag.Comment = tagInFile.Tag.Comment;
          tagOutFile.Tag.Composers = tagInFile.Tag.Composers;
          tagOutFile.Tag.Conductor = tagInFile.Tag.Conductor;
          tagOutFile.Tag.Copyright = tagInFile.Tag.Copyright;
          tagOutFile.Tag.Disc = tagInFile.Tag.Disc;
          tagOutFile.Tag.DiscCount = tagInFile.Tag.DiscCount;
          tagOutFile.Tag.Lyrics = tagInFile.Tag.Lyrics;
          tagOutFile.Tag.Pictures = tagInFile.Tag.Pictures;
          tagOutFile = Util.FormatID3Tag(tagOutFile);
          tagOutFile.Save();
        }
        catch (Exception ex)
        {
          log.Error("Error tagging encoded file {0}. Error: {1}", outFile, ex.Message);
        }
      }
      Options.MainSettings.LastConversionEncoderUsed = encoder;
      _currentRow = -1;

      log.Trace("<<<");
    }

    private void UpdateNewFileName(string fileName)
    {
      if (dataGridViewConvert.InvokeRequired)
      {
        ThreadSafeRefreshDelegate d = UpdateNewFileName;
        dataGridViewConvert.Invoke(d, new object[] {fileName});
        return;
      }

      bindingList[_currentRow].NewFileName = fileName;
      dataGridViewConvert.Refresh();
    }

    /// <summary>
    ///   Adds an Error Message to the Message Grid
    /// </summary>
    /// <param name = "file"></param>
    /// <param name = "message"></param>
    public void AddErrorMessage(string file, string message)
    {
      if (_main.ErrorGridView.InvokeRequired)
      {
        ThreadSafeAddErrorDelegate d = AddErrorMessage;
        _main.ErrorGridView.Invoke(d, new object[] {file, message});
        return;
      }

      _main.ErrorGridView.Rows.Add(file, message);
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
      if (_currentRow < 0)
        return;

      double percentComplete = (double)message.MessageData["progress"];
      dataGridViewConvert.Rows[_currentRow].Cells[0].Value = (int)percentComplete;
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