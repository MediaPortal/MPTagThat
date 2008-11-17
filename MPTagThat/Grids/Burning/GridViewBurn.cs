using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MPTagThat.Core;
using MPTagThat.Core.Burning;
using MPTagThat.Core.MediaChangeMonitor;

using Un4seen.Bass;
using Un4seen.Bass.AddOn.Enc;

namespace MPTagThat.GridView
{
  public partial class GridViewBurn : UserControl
  {
    #region Variables
    private delegate void ThreadSafeAddTracksDelegate(TrackData track);
    private delegate void ThreadSafeAddMediaInfoDelegate();
    private delegate void ThreadSafeBurnAudioDelegate(BurnStatus eBurnStatus, int eTrack, int ePercentage);
    private delegate void ThreadSafeBurnStatusDelegate(string message);
    private delegate void ThreadSafeBurnAudioFailedDelegate(BurnResult eBurnResult, ProjectType eProjectType);
    private delegate void ThreadSafeMediaInsertedDelegate(string DriveLetter);
    private delegate void ThreadSafeMediaRemovedDelegate(string DriveLetter);

    private Thread threadBurn = null;
    private Thread threadMediaInfo = null;

    private Main _main;

    private SortableBindingList<TrackData> bindingList = new SortableBindingList<TrackData>();
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log;

    private GridViewColumnsBurn gridColumns;
    private string tmpBurnDirectory = String.Format(@"{0}\MPTagThat\TmpBurn", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
    private IBurnManager burnManager;
    private MediaInfo mediainfo = null;

    private IMediaChangeMonitor mediaChangeMonitor;

    private Rectangle DragDropRectangle;
    private int DragDropSourceIndex;
    private int DragDropCurrentIndex = -1;
    private int DragDropTargetIndex;
    #endregion

    #region Properties
    public Color BackGroundColor
    {
      set
      {
        this.BackColor = value;
        this.panelTop.BackColor = value;
      }
    }

    public bool Burning
    {
      get
      {
        if (threadBurn == null)
          return false;

        if (threadBurn.ThreadState == ThreadState.Running)
          return true;
        else
          return false;
      }
    }

    public DataGridView View
    {
      get { return dataGridViewBurn; }
    }
    #endregion

    #region ctor
    public GridViewBurn(Main main)
    {
      _main = main;

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      InitializeComponent();

      log = ServiceScope.Get<ILogger>();
      burnManager = ServiceScope.Get<IBurnManager>();
      mediaChangeMonitor = ServiceScope.Get<IMediaChangeMonitor>();
      mediaChangeMonitor.MediaInserted += new MediaInsertedEvent(mediaChangeMonitor_MediaInserted);
      mediaChangeMonitor.MediaRemoved += new MediaRemovedEvent(mediaChangeMonitor_MediaRemoved);

      // Create the Temp Directory for the burner
      if (!System.IO.Directory.Exists(tmpBurnDirectory))
        System.IO.Directory.CreateDirectory(tmpBurnDirectory);

      // Load the Settings
      gridColumns = new GridViewColumnsBurn();

      dataGridViewBurn.AutoGenerateColumns = false;
      dataGridViewBurn.DataSource = bindingList;

      // Now Setup the columns, we want to display
      CreateColumns();

      CreateContextMenu();

      List<Burner> burners = burnManager.GetDrives();
      // Try 5 times to get burners. might be a problem with the Burner Service not ready yet
      int i = 0;
      while (burners.Count == 0 && i < 4)
      {
        burners = burnManager.GetDrives();
        i++;
      }

      foreach (Burner burner in burners)
      {
        if (burner.DriveFeatures.WriteCDR)
        {
          string deviceName = string.Format("{0} {1}", burner.DeviceVendor, burner.DeviceName);
          Telerik.WinControls.UI.RadComboBoxItem ri = new Telerik.WinControls.UI.RadComboBoxItem(deviceName, burner);
          main.MainRibbon.BurnerCombo.Items.Add(ri);
        }
      }


      lbBurningStatus.Text = localisation.ToString("Burning", "DragAndDrop"); // "Use Drag & Drop to order the tracks for burning";
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds a Track to the Burn Gridview
    /// </summary>
    /// <param name="trackPath"></param>
    /// <param name="file"></param>
    public void AddToBurner(TrackData track)
    {
      AddTrack(track);
    }

    /// <summary>
    /// Burns the files in the Grid to an Audio CD
    /// </summary>
    public void BurnAudioCD()
    {
      lbBurningStatus.Text = localisation.ToString("Burning", "Decoding"); ;

      if (threadBurn == null)
      {
        threadBurn = new Thread(new ThreadStart(BurningThread));
        threadBurn.Name = "Burning";
      }

      if (threadBurn.ThreadState != ThreadState.Running)
      {
        threadBurn = new Thread(new ThreadStart(BurningThread));
        threadBurn.Start();
      }
    }

    /// <summary>
    /// Cancel the Burning Process
    /// </summary>
    public void BurnAudioCDCancel()
    {
      lbBurningStatus.Text = localisation.ToString("Burning", "Cancelled"); ;

      if (threadBurn != null)
      {
        threadBurn.Abort();
      }
    }

    public void SetMediaInfo()
    {
      if (threadMediaInfo == null)
      {
        threadMediaInfo = new Thread(new ThreadStart(SetMediaInfoThread));
        threadMediaInfo.Name = "MediaInfo";
      }

      if (threadMediaInfo.ThreadState != ThreadState.Running)
      {
        threadMediaInfo = new Thread(new ThreadStart(SetMediaInfoThread));
        threadMediaInfo.Start();
      }
    }

    public void SetActiveBurner(Burner burner)
    {
      burnManager.SetActiveBurner(burner);
      SetMediaInfo();
    }
    #endregion

    #region Private Methods
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
      foreach (DataGridViewColumn col in dataGridViewBurn.Columns)
      {
        col.HeaderText = localisation.ToString("column_header", col.Name);
      }
    }
    #endregion

    #region Decoding / Burning
    private void BurningThread()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      List<string> outFiles = new List<string>();
      bool bError = false;

      foreach (DataGridViewRow row in dataGridViewBurn.Rows)
      {
        TrackData track = bindingList[row.Index];
        string outFile = String.Format(@"{0}\{1}.wav", tmpBurnDirectory, System.IO.Path.GetFileNameWithoutExtension(track.FullFileName));
        int stream = Bass.BASS_StreamCreateFile(track.FullFileName, 0, 0, BASSFlag.BASS_STREAM_DECODE);
        if (stream == 0)
        {
          bError = true;
          log.Error("Error creating stream for {0}.", track.FullFileName);
          continue;
        }

        log.Info("Decoding to WAV: {0}", track.FullFileName);
        BassEnc.BASS_Encode_Start(stream, outFile, BASSEncode.BASS_ENCODE_PCM, null, IntPtr.Zero);

        long pos = 0;
        long chanLength = Bass.BASS_ChannelGetLength(stream);

        byte[] encBuffer = new byte[20000]; // our encoding buffer
        while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
        {
          // getting sample data will automatically feed the encoder
          int len = Bass.BASS_ChannelGetData(stream, encBuffer, encBuffer.Length);
          pos = Bass.BASS_ChannelGetPosition(stream);
          double percentComplete = (double)pos / (double)chanLength * 100.0;
          dataGridViewBurn.Rows[row.Index].Cells[0].Value = (int)percentComplete;
        }
        outFiles.Add(outFile);
        dataGridViewBurn.Rows[row.Index].Cells[0].Value = 100;
        BassEnc.BASS_Encode_Stop(stream);
        Bass.BASS_StreamFree(stream);
      }

      burnManager.BurningFailed += new BurningError(burnManager_BurningFailed);
      burnManager.BurnProgressUpdate += new BurnProgress(burnManager_BurnProgressUpdate);
      if (!bError && MediaAllowsBurning())
      {
        foreach (DataGridViewRow row in dataGridViewBurn.Rows)
          dataGridViewBurn.Rows[row.Index].Cells[0].Value = 0;

        BurnResult result = burnManager.BurnAudioCd(outFiles);
        if (result == BurnResult.Successful)
        {
          log.Info("Burning of Audio CD was successful");
        }
        else
        {
          log.Error("Burning of Audio CD failed");
        }
      }

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    void burnManager_BurnProgressUpdate(BurnStatus eBurnStatus, int eTrack, int ePercentage)
    {
      if (lbBurningStatus.InvokeRequired)
      {
        ThreadSafeBurnAudioDelegate d = new ThreadSafeBurnAudioDelegate(burnManager_BurnProgressUpdate);
        lbBurningStatus.Invoke(d, new object[] { eBurnStatus, eTrack, ePercentage });
        return;
      }

      // Sometimes we don't get correct percentages, so set the previous entry to 100%
      if (eTrack > 1)
        dataGridViewBurn.Rows[eTrack - 2].Cells[0].Value = 100;

      if (eTrack > -1)
        dataGridViewBurn.Rows[eTrack - 1].Cells[0].Value = ePercentage <= 100 ? ePercentage : 100;
      else
      {
        switch (eBurnStatus)
        {
          case BurnStatus.Checking:
            lbBurningStatus.Text = localisation.ToString("Burning", "Checking");
            break;

          case BurnStatus.Blanking:
            lbBurningStatus.Text = localisation.ToString("Burning", "Erasing"); 
            break;

          case BurnStatus.LeadIn:
            lbBurningStatus.Text = localisation.ToString("Burning", "Leaadin"); 
            break;

          case BurnStatus.LeadOut:
            dataGridViewBurn.Rows[dataGridViewBurn.Rows.Count - 1].Cells[0].Value = 100;
            lbBurningStatus.Text = localisation.ToString("Burning", "Leadout"); 
            break;

          case BurnStatus.Burning:
            lbBurningStatus.Text = localisation.ToString("Burning", "Burning");
            break;

          case BurnStatus.Finished:
            lbBurningStatus.Text = localisation.ToString("Burning", "Finished");
            break;

        }
      }
    }

    void burnManager_BurningFailed(BurnResult eBurnResult, ProjectType eProjectType)
    {
      if (lbBurningStatus.InvokeRequired)
      {
        ThreadSafeBurnAudioFailedDelegate d = new ThreadSafeBurnAudioFailedDelegate(burnManager_BurningFailed);
        lbBurningStatus.Invoke(d, new object[] { eBurnResult, eProjectType });
        return;
      }

      lbBurningStatus.Text = string.Format(localisation.ToString("Burning", "Failed"), Enum.GetName(typeof(BurnResult), eBurnResult));
      log.Error("Burning Failed: {0}", Enum.GetName(typeof(BurnResult), eBurnResult));
    }

    /// <summary>
    /// Handle the Changing of Media, before burning starts
    /// </summary>
    private bool MediaAllowsBurning()
    {
      bool diskChangeNeeded = true;
      while (diskChangeNeeded)
      {
        if (mediainfo == null)
          SetBurningStatus(localisation.ToString("Burning", "NoMedia"));
        else if (mediainfo.DiskStatus == BlankStatus.complete)
          SetBurningStatus(localisation.ToString("Burning", "FinishedMedia"));
        else if (mediainfo.DiskStatus == BlankStatus.empty)
          return true;

        Thread.Sleep(2000);
      }
      return false;
    }

    private void SetBurningStatus(string message)
    {
      if (lbBurningStatus.InvokeRequired)
      {
        ThreadSafeBurnStatusDelegate d = new ThreadSafeBurnStatusDelegate(SetBurningStatus);
        lbBurningStatus.Invoke(d, new object[] { message });
        return;
      }

      lbBurningStatus.Text = message;
    }

    /// <summary>
    /// Cleanup of the directory
    /// </summary>
    private void CleanupBurnDirectory()
    {
      try
      {
        System.IO.Directory.Delete(tmpBurnDirectory, true);
      }
      catch (Exception)
      { }
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
        dataGridViewBurn.Columns.Add(Util.FormatGridColumn(column));
      }

      // Add a dummy column and set the property of the last column to fill
      DataGridViewColumn col = new DataGridViewTextBoxColumn();
      col.Name = "dummy";
      col.HeaderText = "";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 5;
      dataGridViewBurn.Columns.Add(col);
      dataGridViewBurn.Columns[dataGridViewBurn.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    /// <summary>
    /// Save the settings
    /// </summary>
    private void SaveSettings()
    {
      // Save the Width of the Columns
      int i = 0;
      foreach (DataGridViewColumn column in dataGridViewBurn.Columns)
      {
        // Don't save the dummy column
        if (i == dataGridViewBurn.Columns.Count - 1)
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
      rmitems[0].Text = localisation.ToString("Burning", "ClearList");
      rmitems[0].Click += new System.EventHandler(dataGridViewBurn_ClearList);
      rmitems[0].DefaultItem = true;
      this.dataGridViewBurn.ContextMenu = new ContextMenu(rmitems);
    }
    #endregion


    /// <summary>
    /// Retrieve media info in a Thread
    /// </summary>
    private void SetMediaInfoThread()
    {
      if (lbMediaInfo.InvokeRequired)
      {
        ThreadSafeAddMediaInfoDelegate d = new ThreadSafeAddMediaInfoDelegate(SetMediaInfoThread);
        lbMediaInfo.Invoke(d);
        return;
      }

      lbMediaInfo.Text = localisation.ToString("Burning", "Retrieving");

      mediainfo = burnManager.GetMediaInfo();
      if (mediainfo != null)
        lbMediaInfo.Text = String.Format(localisation.ToString("Burning", "MediaInfo"), mediainfo.HumanMediaString, ConvertSizeToMinute(mediainfo.Size), mediainfo.DiskStatus);

      CalculateTotalTime();
    }

    /// <summary>
    /// Adds a Track to the data grid
    /// </summary>
    /// <param name="track"></param>
    private void AddTrack(TrackData track)
    {
      if (track == null)
        return;


      if (dataGridViewBurn.InvokeRequired)
      {
        ThreadSafeAddTracksDelegate d = new ThreadSafeAddTracksDelegate(AddTrack);
        dataGridViewBurn.Invoke(d, new object[] { track });
        return;
      }

      bindingList.Add(track);
    }

    private void CalculateTotalTime()
    {
      long totalTime = 0;
      foreach (TrackData track in bindingList)
      {
        totalTime += track.File.Properties.Duration.Ticks;
      }
      DateTime dt = new DateTime(totalTime);
      int minutes = dt.Hour * 60 + dt.Minute;
      lbUsed.Text = String.Format(localisation.ToString("Burning", "Used"), minutes.ToString().PadLeft(2, '0'), dt.Second.ToString().PadLeft(2, '0'), bindingList.Count);
      if (mediainfo != null)
      {
        if (minutes > ConvertSizeToMinute(mediainfo.Size))
          lbUsed.Text += localisation.ToString("Burning", "Capacity");
      }
    }

    private int ConvertSizeToMinute(long size)
    {
      // The size is deliverd in sectors and we have 75 sectors per second of Audio
      return (int)Math.Round((double)size / 75.0 / 60.0);
    }
    #endregion

    #region Event Handler
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

    private void dataGridViewBurn_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      CalculateTotalTime();
    }

    private void dataGridViewBurn_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      CalculateTotalTime();
    }

    void mediaChangeMonitor_MediaRemoved(string eDriveLetter)
    {
      if (lbMediaInfo.InvokeRequired)
      {
        ThreadSafeMediaRemovedDelegate d = new ThreadSafeMediaRemovedDelegate(mediaChangeMonitor_MediaRemoved);
        lbMediaInfo.Invoke(d, new object[] { eDriveLetter });
        return;
      }

      mediainfo = null;
      lbMediaInfo.Text = localisation.ToString("Burning", "NoMedia");
    }

    void mediaChangeMonitor_MediaInserted(string eDriveLetter)
    {
      if (lbMediaInfo.InvokeRequired)
      {
        ThreadSafeMediaInsertedDelegate d = new ThreadSafeMediaInsertedDelegate(mediaChangeMonitor_MediaInserted);
        lbMediaInfo.Invoke(d, new object[] { eDriveLetter });
        return;
      }

      SetMediaInfo();
    }

    #region Drag & Drop
    private void OnMouseDown(object sender, MouseEventArgs e)
    {
      //stores values for drag/drop operations if necessary
      if (dataGridViewBurn.AllowDrop)
      {
        int selectedRow = dataGridViewBurn.HitTest(e.X, e.Y).RowIndex;
        if (selectedRow > -1)
        {
          Size DragSize = SystemInformation.DragSize;
          DragDropRectangle = new Rectangle(new Point(e.X - (DragSize.Width / 2), e.Y - (DragSize.Height / 2)), DragSize);
          DragDropSourceIndex = selectedRow;
        }
      }
      else
      {
        DragDropRectangle = Rectangle.Empty;
      }

      // Show Context Menu on Right Mouse Click
      if (e.Button == MouseButtons.Right)
        this.dataGridViewBurn.ContextMenu.Show(dataGridViewBurn, new Point(e.X, e.Y));

      base.OnMouseDown(e);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      if (dataGridViewBurn.AllowDrop)
      {
        if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
        {
          if (DragDropRectangle != Rectangle.Empty && !DragDropRectangle.Contains(e.X, e.Y))
          {
            DragDropEffects DropEffect = dataGridViewBurn.DoDragDrop(dataGridViewBurn.Rows[DragDropSourceIndex], DragDropEffects.Move);
          }
        }
      }
      base.OnMouseMove(e);
    }

    private void OnDragOver(object sender, DragEventArgs e)
    {
      //runs while the drag/drop is in progress
      if (dataGridViewBurn.AllowDrop)
      {
        e.Effect = DragDropEffects.Move;
        int CurRow = dataGridViewBurn.HitTest(dataGridViewBurn.PointToClient(new Point(e.X, e.Y)).X, dataGridViewBurn.PointToClient(new Point(e.X, e.Y)).Y).RowIndex;
        if (DragDropCurrentIndex != CurRow)
        {
          DragDropCurrentIndex = CurRow;
          dataGridViewBurn.Invalidate(); //repaint
        }
      }
      base.OnDragOver(e);
    }

    private void OnDragDrop(object sender, DragEventArgs drgevent)
    {
      //runs after a drag/drop operation for column/row has completed
      if (dataGridViewBurn.AllowDrop)
      {
        if (drgevent.Effect == DragDropEffects.Move)
        {
          Point ClientPoint = dataGridViewBurn.PointToClient(new Point(drgevent.X, drgevent.Y));

          DragDropTargetIndex = dataGridViewBurn.HitTest(ClientPoint.X, ClientPoint.Y).RowIndex;
          if (DragDropTargetIndex > -1 && DragDropCurrentIndex < dataGridViewBurn.RowCount - 1)
          {
            DragDropCurrentIndex = -1;
            TrackData track = bindingList[DragDropSourceIndex];
            bindingList.RemoveAt(DragDropSourceIndex);

            if (DragDropTargetIndex > DragDropSourceIndex)
              DragDropTargetIndex--;

            bindingList.Insert(DragDropTargetIndex, track);

            dataGridViewBurn.ClearSelection();
            dataGridViewBurn.Rows[DragDropTargetIndex].Selected = true;
          }
        }
      }
      base.OnDragDrop(drgevent);
    }

    private void OnCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (DragDropCurrentIndex > -1)
      {
        if (e.RowIndex == DragDropCurrentIndex && DragDropCurrentIndex < dataGridViewBurn.RowCount - 1)
        {
          //if this cell is in the same row as the mouse cursor
          Pen p = new Pen(Color.Red, 1);
          e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top - 1, e.CellBounds.Right, e.CellBounds.Top - 1);
        }
      }
    }
    #endregion

    /// <summary>
    /// Handle Key input on the Grid
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      switch (keyData)
      {
        case Keys.Delete:
          for (int i = dataGridViewBurn.Rows.Count - 1; i > -1; i--)
          {
            if (!dataGridViewBurn.Rows[i].Selected)
              continue;

            bindingList.RemoveAt(i);
          }
          return true;
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// Context Menu entry has been selected
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void dataGridViewBurn_ClearList(object o, System.EventArgs e)
    {
      dataGridViewBurn.Rows.Clear();
    }
    #endregion

  }
}