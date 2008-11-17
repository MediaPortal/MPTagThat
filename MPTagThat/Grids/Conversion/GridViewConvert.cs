using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MPTagThat.Core;
using MPTagThat.Core.AudioEncoder;
using MPTagThat.Core.Burning;
using MPTagThat.Core.MediaChangeMonitor;

using Un4seen.Bass;
using TagLib;

namespace MPTagThat.GridView
{
  public partial class GridViewConvert : UserControl
  {
    #region Variables
    private delegate void ThreadSafeRefreshDelegate(string fileName);
    private delegate void ThreadSafeAddErrorDelegate(string file, string message);
    private Thread threadConvert = null;

    private Main _main;
    private int _currentRow = -1;

    private SortableBindingList<ConversionData> bindingList = new SortableBindingList<ConversionData>();
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log;
    private IAudioEncoder audioEncoder;

    private GridViewColumnsConvert gridColumns;
    #endregion

    #region Properties
    public Color BackGroundColor
    {
      set
      {
        this.BackColor = value;
      }
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
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      IMessageQueue queueMessageEncoding = ServiceScope.Get<IMessageBroker>().GetOrCreate("encoding");
      queueMessageEncoding.OnMessageReceive += new MessageReceivedHandler(OnMessageReceiveEncoding);

      log = ServiceScope.Get<ILogger>();
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
    /// Converts the selected files in the Grid
    /// </summary>
    public void ConvertFiles()
    {
      if (threadConvert == null)
      {
        threadConvert = new Thread(new ThreadStart(ConversionThread));
        threadConvert.Name = "Ripping";
      }

      if (threadConvert.ThreadState != ThreadState.Running)
      {
        threadConvert = new Thread(new ThreadStart(ConversionThread));
        threadConvert.Start();
      }
    }

    /// <summary>
    /// Cancel the Ripping Process
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
    /// Add the Track to the Conversion Grid
    /// </summary>
    /// <param name="track"></param>
    public void AddToConvert(TrackData track)
    {
      if (track == null)
        return;

      ConversionData convdata = new ConversionData();
      convdata.FileName = track.FileName;

      bindingList.Add(convdata);
    }
    #endregion

    #region Private Methods
    #region Conversion
    private void ConversionThread()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      string outputDir = _main.MainRibbon.EncoderOutputDirectory;
      bool OutputAndCurrentDirEqual = (outputDir == _main.CurrentDirectory);
      string encoder = null;

      List<Item> encoders = (List<Item>)_main.MainRibbon.EncoderCombo.DataSource;
      if (_main.MainRibbon.EncoderCombo.SelectedItem != null)
      {
        encoder = encoders[_main.MainRibbon.EncoderCombo.SelectedIndex].Value;
      }

      if (encoder == null)
        return;

      try
      {
        if (!System.IO.Directory.Exists(outputDir) && !string.IsNullOrEmpty(outputDir))
          System.IO.Directory.CreateDirectory(outputDir);
      }
      catch (Exception ex)
      {
        _main.ErrorGridView.Rows.Add("", localisation.ToString("Conversion","ErrorDirectory"));
        log.Error("Error creating Output Directory directory: {0}. {1}", outputDir, ex.Message);
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

        string inputFile = System.IO.Path.Combine(_main.CurrentDirectory, bindingList[_currentRow].FileName);
        string outFile = System.IO.Path.GetFileNameWithoutExtension(bindingList[_currentRow].FileName);
        outFile = System.IO.Path.Combine(outputDir, outFile);
        outFile = audioEncoder.SetEncoder(encoder, outFile);
        UpdateNewFileName(System.IO.Path.GetFileName(outFile));

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
          log.Error("Error creating stream for file {0}. Error: {1}", inputFile, Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode()));
          continue;
        }

        log.Info("Convert file {0} -> {1}", inputFile, outFile);

        if (audioEncoder.StartEncoding(stream) != BASSError.BASS_OK)
        {
          AddErrorMessage(inputFile, localisation.ToString("Conversion", "EncodingFileError"));
          log.Error("Error starting Encoder for File {0}. Error: {1}", inputFile, Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode()));
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

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    private void UpdateNewFileName(string fileName)
    {
      if (dataGridViewConvert.InvokeRequired)
      {
        ThreadSafeRefreshDelegate d = new ThreadSafeRefreshDelegate(UpdateNewFileName);
        dataGridViewConvert.Invoke(d, new object[] { fileName });
        return;
      }

      bindingList[_currentRow].NewFileName = System.IO.Path.GetFileName(fileName);
      dataGridViewConvert.Refresh();
    }

    /// <summary>
    /// Adds an Error Message to the Message Grid
    /// </summary>
    /// <param name="file"></param>
    /// <param name="message"></param>
    public void AddErrorMessage(string file, string message)
    {
      if (_main.ErrorGridView.InvokeRequired)
      {
        ThreadSafeAddErrorDelegate d = new ThreadSafeAddErrorDelegate(AddErrorMessage);
        _main.ErrorGridView.Invoke(d, new object[] { file, message });
        return;
      }

      _main.ErrorGridView.Rows.Add(file, message);
    }
    #endregion

    #region Gridlayout
    /// <summary>
    /// Create the Columns of the Grid based on the users setting
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
      dataGridViewConvert.Columns[dataGridViewConvert.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    /// <summary>
    /// Save the settings
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
    /// Create Context Menu
    /// </summary>
    private void CreateContextMenu()
    {
      // Build the Context Menu for the Grid
      MenuItem[] rmitems = new MenuItem[1];
      rmitems[0] = new MenuItem();
      rmitems[0].Text = "Clear List";
      rmitems[0].Click += new System.EventHandler(dataGridViewConvert_ClearList);
      rmitems[0].DefaultItem = true;
      this.dataGridViewConvert.ContextMenu = new ContextMenu(rmitems);
    }
    #endregion

    #region Localisation
    /// <summary>
    /// Language Change event has been fired. Apply the new language
    /// </summary>
    /// <param name="language"></param>
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
    /// Handle Messages from the Audio Encoder
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceiveEncoding(QueueMessage message)
    {
      if (_currentRow < 0)
        return;

      double percentComplete = (double)message.MessageData["progress"];
      dataGridViewConvert.Rows[_currentRow].Cells[0].Value = (int)percentComplete;
    }

    /// <summary>
    /// Handle Messages
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "languagechanged":
          LanguageChanged();
          this.Refresh();
          break;
      }
    }

    /// <summary>
    /// Handle Right Mouse Click to open the context Menu in the Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dataGridViewConvert_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        this.dataGridViewConvert.ContextMenu.Show(dataGridViewConvert, new Point(e.X, e.Y));
    }

    /// <summary>
    /// Context Menu entry has been selected
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void dataGridViewConvert_ClearList(object o, System.EventArgs e)
    {
      dataGridViewConvert.Rows.Clear();
    }
    #endregion
  }
}
