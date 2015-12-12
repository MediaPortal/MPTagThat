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
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Elegant.Ui;
using Microsoft.VisualBasic.FileIO;
using MPTagThat.Core;
using MPTagThat.Dialogues;
using MPTagThat.Player;
using TagLib;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;
using Picture = MPTagThat.Core.Common.Picture;
using TextBox = System.Windows.Forms.TextBox;

#endregion

namespace MPTagThat.GridView
{
  public partial class GridViewTracks : UserControl
  {
    #region Variables

    private readonly Cursor _numberingCursor = Util.CreateCursorFromResource("CursorNumbering", 0, 0);
    private readonly GridViewColumns gridColumns;

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly IThemeManager theme = ServiceScope.Get<IThemeManager>();
    private bool _actionCopy;

    private BackgroundWorker _bgWorker;
    private Rectangle _dragBoxFromMouseDown;

    private string[] _filterFileExtensions;
    private string _filterFileMask = "*.*";

    private FindResult _findResult;
    private bool _itemsChanged;
    private Main _main;
    private List<FileInfo> _nonMusicFiles;
    private bool _progressCancelled;
    private Point _screenOffset;
    private bool _waitCursorActive;

    // Get Properties to be able to sort on column heading 
    private readonly PropertyDescriptorCollection _propColl = TypeDescriptor.GetProperties(new TrackData());

    public delegate void CommandThreadEnd(object sender, EventArgs args);
    public event CommandThreadEnd CommandThreadEnded;

    #region Nested type: ThreadSafeAddErrorDelegate

    private delegate void ThreadSafeAddErrorDelegate(string file, string message);

    #endregion

    #region Nested type: ThreadSafeAddTracksDelegate

    private delegate void ThreadSafeAddTracksDelegate(TrackData track);

    #endregion

    #region Nested type: ThreadSafeGridDelegate

    private delegate void ThreadSafeGridDelegate();

    #endregion

    #region Nested type: ThreadSafeGridDelegate1

    private delegate void ThreadSafeGridDelegate1(object sender, DoWorkEventArgs e);

    #endregion

    #endregion

    #region Properties

    /// <summary>
    ///   Returns the GridView
    /// </summary>
    public DataGridView View
    {
      get { return tracksGrid; }
    }

    /// <summary>
    ///   Returns the Selected Track
    /// </summary>
    public TrackData SelectedTrack
    {
      get { return Options.Songlist[tracksGrid.CurrentRow.Index]; }
    }

    /// <summary>
    ///   Do we have any changes pending?
    /// </summary>
    public bool Changed
    {
      get { return _itemsChanged; }
      set { _itemsChanged = value; }
    }

    public FindResult ResultFind
    {
      set { _findResult = value; }
    }

    /// <summary>
    /// Returns the instance of the mainform
    /// </summary>
    public Main MainForm
    {
      get { return _main; }
    }

    /// <summary>
    /// Returns the NonMusic File Section
    /// </summary>
    public List<FileInfo> NonMusicFiles
    {
      get { return _nonMusicFiles; }
    }

    #endregion

    #region Constructor

    public GridViewTracks()
    {
      // Activates double buffering 
      this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();

      InitializeComponent();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      // Load the Settings
      gridColumns = new GridViewColumns();

      // Setup Dataview Grid
      tracksGrid.AutoGenerateColumns = false;
      tracksGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable; // Handle Copy 

      // Setup Event Handler
      tracksGrid.ColumnWidthChanged += tracksGrid_ColumnWidthChanged;
      tracksGrid.CurrentCellDirtyStateChanged += tracksGrid_CurrentCellDirtyStateChanged;
      tracksGrid.DataError += tracksGrid_DataError;
      tracksGrid.CellEndEdit += tracksGrid_CellEndEdit;
      tracksGrid.CellValueChanged += tracksGrid_CellValueChanged;
      tracksGrid.CellPainting += tracksGrid_CellPainting;
      tracksGrid.EditingControlShowing += tracksGrid_EditingControlShowing;
      tracksGrid.ColumnHeaderMouseClick += tracksGrid_ColumnHeaderMouseClick;
      tracksGrid.MouseDown += tracksGrid_MouseDown;
      tracksGrid.MouseUp += tracksGrid_MouseUp;
      tracksGrid.MouseMove += tracksGrid_MouseMove;
      tracksGrid.QueryContinueDrag += tracksGrid_QueryContinueDrag;
      tracksGrid.MouseEnter += tracksGrid_MouseEnter;

      // establish Event Handlers for Virtual Mode hangling of the grid
      tracksGrid.CellValueNeeded += tracksGrid_CellValueNeeded;
      tracksGrid.CellValuePushed += tracksGrid_CellValuePushed;

      // Now Setup the columns, we want to display
      CreateColumns();

      LocaliseScreen();
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Set Main Ref to Main
    ///   Can't do it in constructir due to problems with Designer
    /// </summary>
    /// <param name = "main"></param>
    public void SetMainRef(Main main)
    {
      _main = main;

      // Register for ProgressBar Events
      ApplicationCommands.ProgressCancel.Executed += ProgressCancel_Executed;
      _main.ProgressCancelHovering += new Main.ProgressCancelHover(ProgressCancel_Hover);
      _main.ProgressCancelLeaving += new Main.ProgressCancelLeave(ProgressCancel_Leave);
    }

    #region Status Column

    /// <summary>
    /// Clears the Status column
    /// </summary>
    /// <param name="row"></param>
    public void ClearStatusColumn(int rowIndex)
    {
      Options.Songlist[rowIndex].Status = -1;
    }

    #endregion

    #region Command Execution

    public void ExecuteCommand(string command)
    {
      object[] parameter = {};
      ExecuteCommand(command, parameter, true);
    }

    public void ExecuteCommand(string command, object parameters, bool runAsync)
    {
      log.Trace(">>>");
      log.Debug("Invoking Command: {0}", command);

      object[] parameter = { command , parameters};

      if (runAsync)
      {
        if (_bgWorker == null)
        {
          _bgWorker = new BackgroundWorker();
          _bgWorker.DoWork += ExecuteCommandThread;
        }

        if (!_bgWorker.IsBusy)
        {
          _bgWorker.RunWorkerAsync(parameter);
        }
      }
      else
      {
        ExecuteCommandThread(this, new DoWorkEventArgs(parameter));
      }
      log.Trace("<<<");
    }

    private void ExecuteCommandThread(object sender, DoWorkEventArgs e)
    {
      log.Trace(">>>");

      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeGridDelegate1 d = ExecuteCommandThread;
        tracksGrid.Invoke(d, new[] { sender, e });
        return;
      }

      // Get the command object
      object[] parameters = e.Argument as object[];
      Commands.Command commandObj = Commands.Command.Create(parameters);
      if (commandObj == null)
      {
        return;
      }

      // Extract the command name, since we might need it for specific selections afterwards
      var command = (string)parameters[0];

      var commandParmObj = (object[]) parameters[1];
      var commandParm = commandParmObj.GetLength(0) > 0 ? (string) commandParmObj[0] : "";
      
      // Set a reference to the Track Grid
      commandObj.TracksGrid = this;
      
      int count = 0;
      int trackCount = tracksGrid.SelectedRows.Count;
      SetProgressBar(trackCount);

      // If the command needs Preprocessing, then first loop over all tracks
      if (commandObj.NeedsPreprocessing)
      {
        foreach (DataGridViewRow row in tracksGrid.Rows)
        {
          if (!row.Selected && command != "SaveAll")
          {
            continue;
          }

          TrackData track = Options.Songlist[row.Index];
          commandObj.PreProcess(track);
        }
      }
      
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        ClearStatusColumn(row.Index);

        if (!row.Selected && command != "SaveAll")
        {
          continue;
        }

        count++;
        try
        {
          Application.DoEvents();

          if (command != "SaveAll" || commandParm == "true")
          {
            _main.progressBar1.Value += 1;
            if (_progressCancelled)
            {
              commandObj.ProgressCancelled = true;
              ResetProgressBar();
              return;
            }
          }

          TrackData track = Options.Songlist[row.Index];
          if (command == "SaveAll")
          {
            track.Status = -1;
          }

          if (commandObj.Execute(ref track, row.Index))
          {
            SetBackgroundColorChanged(row.Index);
            track.Changed = true;
            Options.Songlist[row.Index] = track;
            _itemsChanged = true;
          }
          if (commandObj.ProgressCancelled)
          {
            break;
          }
        }
        catch (Exception ex)
        {
          Options.Songlist[row.Index].Status = 2;
          AddErrorMessage(row, ex.Message);
        }
      }

      // Do Command Post Processing
      _itemsChanged = commandObj.PostProcess();

      Util.SendProgress("");
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      if (CommandThreadEnded != null && commandObj.NeedsCallback)
      {
        CommandThreadEnded(this, new EventArgs());
      }

      commandObj.Dispose();

      ResetProgressBar();
      log.Trace("<<<");
    }

    #endregion
    
    #region Numbering

    public void AutoNumber()
    {
      log.Trace(">>>");
      int numberValue = _main.AutoNumber;
      if (numberValue == -1)
        return;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];

        // Get the Number of tracks, so that we don't loose it
        string[] tracks = track.Track.Split('/');
        string numTracks = "0";
        if (tracks.Length > 1)
        {
          numTracks = tracks[1];
        }

        track.Track = string.Format("{0}/{1}", numberValue, numTracks);
        SetBackgroundColorChanged(row.Index);
        track.Changed = true;
        Options.Songlist[row.Index] = track;
        _itemsChanged = true;
        numberValue++;
      }
      _main.AutoNumber = numberValue;
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      log.Trace("<<<");
    }

    #endregion

    #region Delete Tags / Delete Files

    /// <summary>
    ///   Remove the Tags
    /// </summary>
    /// <param name = "type"></param>
    public void DeleteTags(TagTypes type)
    {
      log.Trace(">>>");
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];
        track.TagsRemoved.Add(type);
        track = Track.ClearTag(track);

        SetBackgroundColorChanged(row.Index);
        track.Changed = true;
        Options.Songlist[row.Index] = track;
        _itemsChanged = true;
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      log.Trace("<<<");
    }

    /// <summary>
    ///   The Del key has been pressed. Send the selected files to the recycle bin
    /// </summary>
    public void DeleteTracks()
    {
      log.Trace(">>>");

      if (tracksGrid.SelectedRows.Count > 0)
      {
        DialogResult result = MessageBox.Show(localisation.ToString("message", "DeleteConfirm"),
                                              localisation.ToString("message", "DeleteConfirmHeader"),
                                              MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        if (result == DialogResult.Cancel)
        {
          log.Trace("<<<");
          return;
        }
      }

      for (int i = tracksGrid.Rows.Count - 1; i >= 0; i--)
      {
        DataGridViewRow row = tracksGrid.Rows[i];
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];
        try
        {
          FileSystem.DeleteFile(track.FullFileName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin,
                                UICancelOption.ThrowException);

          // Remove the file from the binding list
          Options.Songlist.RemoveAt(row.Index);
        }
        catch (OperationCanceledException) // User pressed No on delete. Do nothing
        { }
        catch (Exception ex)
        {
          log.Error("Error deleting file: {0} Exception: {1}", track.FullFileName, ex.Message);
          Options.Songlist[row.Index].Status = 2;
          AddErrorMessage(row, ex.Message);
        }
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();

      log.Trace("<<<");
    }

    #endregion

    #region Misc Methods

    /// <summary>
    ///   Discards any changes
    /// </summary>
    public void DiscardChanges()
    {
      _itemsChanged = false;
    }

    /// <summary>
    ///   Checks for Pending Changes
    /// </summary>
    public void CheckForChanges()
    {
      if (Changed)
      {
        DialogResult result = MessageBox.Show(localisation.ToString("message", "Save_Changes"),
                                              localisation.ToString("message", "Save_Changes_Title"),
                                              MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
          object[] parm = {"true"};
          ExecuteCommand("SaveAll", parm, false);
        }
        else
          DiscardChanges();
      }
    }

    /// <summary>
    ///   Checks, if we have something selected
    /// </summary>
    /// <returns></returns>
    public bool CheckSelections(bool selectAll)
    {
      if (tracksGrid.SelectedRows.Count == 0)
      {
        // If no Rows are selected, select ALL of them and do the necessary action
        if (selectAll)
          tracksGrid.SelectAll();
        else
        {
          MessageBox.Show(localisation.ToString("message", "NoSelection"),
                          localisation.ToString("message", "NoSelectionHeader"), MessageBoxButtons.OK,
                          MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    /// <summary>
    ///   Sets the Color for changed Items
    /// </summary>
    /// <param name = "index"></param>
    public void SetBackgroundColorChanged(int index)
    {
      tracksGrid.Rows[index].Tag = "Changed";

      tracksGrid.Rows[index].DefaultCellStyle.BackColor =
        ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedBackColor;
      tracksGrid.Rows[index].DefaultCellStyle.ForeColor =
        ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedForeColor;
    }

    /// <summary>
    /// Sets the Grid Color based on the Line
    /// </summary>
    /// <param name="index"></param>
    public void SetGridRowColors(int index)
    {
      if (index % 2 == 0)
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
     }
      else
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.AlternatingRowBackColor;
      }
    }

    /// <summary>
    ///   Sets the Color for Tracks, that contain errors found by mp3val
    /// </summary>
    /// <param name = "index"></param>
    public void SetColorMP3Errors(int index, Util.MP3Error error)
    {
      if (error == Util.MP3Error.Fixable || error == Util.MP3Error.Fixed)
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.FixableErrorBackColor;
        tracksGrid.Rows[index].DefaultCellStyle.ForeColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.FixableErrorForeColor;
      }
      else if (error == Util.MP3Error.NonFixable)
      {
        tracksGrid.Rows[index].DefaultCellStyle.BackColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.NonFixableErrorBackColor;
        tracksGrid.Rows[index].DefaultCellStyle.ForeColor =
          ServiceScope.Get<IThemeManager>().CurrentTheme.NonFixableErrorForeColor;
      }

      if (error == Util.MP3Error.Fixed)
      {
        Options.Songlist[index].Status = 4;
      }
    }

    /// <summary>
    ///   Adds an Error Message to the Status Column
    /// </summary>
    /// <param name = "row"></param>
    /// <param name = "message"></param>
    public void AddErrorMessage(DataGridViewRow row, string message)
    {
      row.Cells[0].ToolTipText = message;
    }

    #endregion

    #region Script Handling

    /// <summary>
    ///   Executes a script on all selected rows
    /// </summary>
    /// <param name = "scriptFile"></param>
    public void ExecuteScript(string scriptFile)
    {
      log.Trace(">>>");
      Assembly assembly = ServiceScope.Get<IScriptManager>().Load(scriptFile);

      try
      {
        if (assembly != null)
        {
          
          int trackCount = tracksGrid.SelectedRows.Count;
          SetProgressBar(trackCount);

          IScript script = (IScript)assembly.CreateInstance("Script");

          foreach (DataGridViewRow row in tracksGrid.Rows)
          {
            if (!row.Selected)
            {
              continue;
            }

            Application.DoEvents();
            _main.progressBar1.Value += 1;
            if (_progressCancelled)
            {
              ResetProgressBar();
              Util.SendProgress("");
              return;
            }

            TrackData track = Options.Songlist[row.Index];
            Util.SendProgress(string.Format("Running script on {0}", track.FullFileName));
            script.Invoke(track);

            if (track.Changed)
            {
              Options.Songlist[row.Index] = track;
              SetBackgroundColorChanged(row.Index);
              _itemsChanged = true;
            }
          }
          ResetProgressBar();
          Util.SendProgress("");
        }
      }
      catch (Exception ex)
      {
        log.Error("Script Execution failed: {0}", ex.Message);
        MessageBox.Show(localisation.ToString("message", "Script_Compile_Failed"),
                        localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
      }
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      _main.TagEditForm.FillForm();

      log.Trace("<<<");
    }

    #endregion

    #region Folder Scanning

    public void FolderScan()
    {
      log.Trace(">>>");
      tracksGrid.Rows.Clear();
      Options.Songlist.Clear();
      _nonMusicFiles = new List<FileInfo>();
      _main.MiscInfoPanel.ClearNonMusicFiles();
      _main.MiscInfoPanel.ActivateNonMusicTab();
      GC.Collect();

      string selectedFolder = _main.CurrentDirectory;
      if (!Directory.Exists(selectedFolder))
        return;

      _main.FolderScanning = true;

      // Get File Filter Settings
      _filterFileExtensions = _main.TreeView.ActiveFilter.FileFilter.Split('|');
      _filterFileMask = _main.TreeView.ActiveFilter.FileMask.Trim() == ""
                          ? "*"
                          : _main.TreeView.ActiveFilter.FileMask.Trim();

      SetProgressBar(1);

      // Change the style to Marquee, since we really don't know how much files we will get
      _main.progressBar1.Style = ProgressBarStyle.Marquee;
      _main.progressBar1.MarqueeAnimationSpeed = 10;

      int count = 1;
      int nonMusicCount = 0;

      tracksGrid.SuspendLayout();
      tracksGrid.Hide();
      try
      {
        foreach (FileInfo fi in GetFiles(new DirectoryInfo(selectedFolder), _main.TreeView.ScanFolderRecursive))
        {
          Application.DoEvents();

          if (_progressCancelled)
          {
            break;
          }
          try
          {
            if (Util.IsAudio(fi.FullName))
            {
              Util.SendProgress(string.Format("Reading file {0}", fi.FullName));
              log.Trace("Retrieving file: {0}", fi.FullName);
              // Read the Tag
              TrackData track = Track.Create(fi.FullName);
              if (track != null)
              {
                if (ApplyTagFilter(track))
                {
                  AddTrack(track);
                  tracksGrid.Rows.Add(); // Add a row to the grid. Virtualmode will handle the filling of cells
                  count++;
                }
              }
            }
            else
            {
              _nonMusicFiles.Add(fi);
              nonMusicCount++;
            }
          }
          catch (PathTooLongException)
          {
            log.Warn("FolderScan: Ignoring track {0} - path too long!", fi.FullName);
            continue;
          }
          catch (System.UnauthorizedAccessException exUna)
          {
            log.Warn("Could not access file or folder: {0}. {1}", exUna.Message, fi.FullName);
          }
          catch (Exception ex)
          {
            log.Error("Caugth error processing files: {0} {1}", ex.Message, fi.FullName);
          }

          if (count > 20)
          {
            tracksGrid.Show();
            tracksGrid.ResumeLayout();
          }

          _main.ToolStripStatusScan.Text = string.Format(localisation.ToString("main", "toolStripLabelScan"), count);
        }
      }
      catch (OutOfMemoryException)
      {
        GC.Collect();
        MessageBox.Show(localisation.ToString("message", "OutOfMemory"), localisation.ToString("message", "Error_Title"),
                        MessageBoxButtons.OK);
        log.Error("Folderscan: Running out of memory. Scanning aborted.");
      }
      finally
      {
        tracksGrid.Show();
        tracksGrid.ResumeLayout();
      }

      Util.SendProgress("");
      log.Info("FolderScan: Scanned {0} files. Found {1} audio files", nonMusicCount + count, count);

      _main.MiscInfoPanel.AddNonMusicFiles(_nonMusicFiles);

      _main.ToolStripStatusScan.Text = "";
      

      ResetProgressBar();
      _main.progressBar1.Style = ProgressBarStyle.Continuous;

      // Display Status Information
      try
      {
        _main.ToolStripStatusFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"), count, 0);
      }
      catch (InvalidOperationException) { }

      // unselect the first row, which would be selected automatically by the grid
      // And set the background color of the rating cell, as it isn't reset by the grid
      try
      {
        if (tracksGrid.Rows.Count > 0)
        {
          _main.TagEditForm.ClearForm();
          tracksGrid.Rows[0].Selected = false;
        }
      }
      catch (ArgumentOutOfRangeException) { }

      // If MP3 Validation is turned on, set the color
      if (Options.MainSettings.MP3Validate)
      {
        ChangeErrorRowColor();
      }

      _main.FolderScanning = false;

      log.Trace("<<<");
    }

    /// <summary>
    ///   Read a Folder and return the files
    /// </summary>
    /// <param name = "folder"></param>
    /// <param name = "foundFiles"></param>
    private IEnumerable<FileInfo> GetFiles(DirectoryInfo dirInfo, bool recursive)
    {
      Queue<DirectoryInfo> directories = new Queue<DirectoryInfo>();
      directories.Enqueue(dirInfo);
      Queue<FileInfo> files = new Queue<FileInfo>();
      while (files.Count > 0 || directories.Count > 0)
      {
        if (files.Count > 0)
        {
          yield return files.Dequeue();
        }
        try
        {
          if (directories.Count > 0)
          {
            DirectoryInfo dir = directories.Dequeue();

            if (recursive)
            {
              DirectoryInfo[] newDirectories = dir.GetDirectories();
              foreach (DirectoryInfo di in newDirectories)
              {
                directories.Enqueue(di);
              }
            }

            foreach (string extension in _filterFileExtensions)
            {
              string searchFilter = string.Format("{0}.{1}", _filterFileMask, extension);
              FileInfo[] newFiles = dir.GetFiles(searchFilter);
              foreach (FileInfo file in newFiles)
              {
                files.Enqueue(file);
              }
            }
          }
        }
        catch (UnauthorizedAccessException ex)
        {
          log.ErrorException(ex.Message, ex);
        }
      }
    }

    #endregion

    #region Database Scanning / Update

    public void DatabaseScan()
    {
      if (_main.CurrentDirectory == null)
      {
        return;
      }

      string[] searchString = _main.CurrentDirectory.Split('\\');

      // Get out, if we are on the Top Level only
      if (searchString.GetLength(0) == 1)
      {
        return;
      }

      List<string> songs = new List<string>();
      string sql = FormatSQL(searchString);

      string connection = string.Format(@"Data Source={0}", Options.MainSettings.MediaPortalDatabase);
      try
      {
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = CommandType.Text;
          cmd.CommandText = sql;
          log.Debug("Database Scan: Executing sql: {0}", sql);
          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              songs.Add(reader.GetString(0));
            }
          }
        }
        conn.Close();
      }
      catch (Exception ex)
      {
        log.Error("Database Scan: Error executing sql: {0}", ex.Message);
      }

      log.Debug("Database Scan: Query returned {0} songs", songs.Count);
      AddDatabaseSongsToGrid(songs);
    }

    public void AddDatabaseSongsToGrid(List<string> songs)
    {
      // Clear the list and free up resources
      tracksGrid.Rows.Clear();
      Options.Songlist.Clear();
      GC.Collect();

      SetProgressBar(songs.Count);

      // Get File Filter Settings
      _filterFileExtensions = _main.TreeView.ActiveFilter.FileFilter.Split('|');
      _filterFileMask = _main.TreeView.ActiveFilter.FileMask.Trim() == ""
                          ? "*"
                          : _main.TreeView.ActiveFilter.FileMask.Trim();

      int count = 1;
      foreach (string song in songs)
      {
        Application.DoEvents();
        _main.progressBar1.Value += 1;
        if (_progressCancelled)
        {
          break;
        }

        if (ApplyFileFilter(song))
        {
          TrackData track = Track.Create(song);
          if (ApplyTagFilter(track))
          {
            AddTrack(track);
            tracksGrid.Rows.Add(); // Add a row to the grid. Virtualmode will handle the filling of cells
          }
        }
        count++;
      }

      _main.FolderScanning = false;
      ResetProgressBar();

      // Display Status Information
      try
      {
        _main.ToolStripStatusFiles.Text = string.Format(localisation.ToString("main", "toolStripLabelFiles"), count, 0);
      }
      catch (InvalidOperationException) { }

      // unselect the first row, which would be selected automatically by the grid
      // And set the background color of the rating cell, as it isn't reset by the grid
      try
      {
        if (tracksGrid.Rows.Count > 0)
        {
          _main.TagEditForm.ClearForm();
          tracksGrid.Rows[0].Selected = false;
        }
      }
      catch (ArgumentOutOfRangeException) { }


      // If MP3 Validation is turned on, set the color
      if (Options.MainSettings.MP3Validate)
      {
        ChangeErrorRowColor();
      }
    }

    private string FormatSQL(string[] searchString)
    {
      string sql = "select strPath from tracks where {0} order by {1}";

      string whereClause = "";
      string orderByClause = "";
      switch (searchString[0])
      {
        case "artist":
          whereClause = string.Format("strArtist like '%| {0}%'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "strAlbum, iTrack";
          if (searchString.GetLength(0) > 2)
          {
            whereClause += string.Format(" AND strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[2]));
            orderByClause = "iTrack";
          }
          break;

        case "albumartist":
          whereClause = string.Format("strAlbumArtist like '%| {0}%'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "strAlbum, iTrack";
          if (searchString.GetLength(0) > 2)
          {
            whereClause += string.Format(" AND strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[2]));
            orderByClause = "iTrack";
          }
          break;

        case "album":
          whereClause = string.Format("strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "iTrack";
          break;

        case "genre":
          whereClause = string.Format("strGenre like '%| {0}%'", Util.RemoveInvalidChars(searchString[1]));
          orderByClause = "strArtist, strAlbum, iTrack";
          if (searchString.GetLength(0) > 2)
          {
            whereClause += string.Format(" AND strArtist like '%{0}%'", Util.RemoveInvalidChars(searchString[2]));
            orderByClause = "strAlbum, iTrack";
          }
          if (searchString.GetLength(0) > 3)
          {
            whereClause += string.Format(" AND strAlbum like '{0}'", Util.RemoveInvalidChars(searchString[3]));
            orderByClause = "iTrack";
          }
          break;
      }

      sql = string.Format(sql, whereClause, orderByClause);

      return sql;
    }

    public void UpdateMusicDatabase(TrackData track)
    {
      string db = Options.MainSettings.MediaPortalDatabase;

      if (!System.IO.File.Exists(db))
      {
        return;
      }

      string[] tracks = track.Track.Split('/');
      int trackNumber = 0;
      if (tracks[0] != "")
      {
        trackNumber = Convert.ToInt32(tracks[0]);
      }
      int trackTotal = 0;
      if (tracks.Length > 1)
      {
        trackTotal = Convert.ToInt32(tracks[1]);
      }

      string[] discs = track.Disc.Split('/');
      int discNumber = 0;
      if (discs[0] != "")
      {
        discNumber = Convert.ToInt32(discs[0]);
      }
      int discTotal = 0;
      if (discs.Length > 1)
      {
        discTotal = Convert.ToInt32(discs[1]);
      }

      string originalFileName = Path.GetFileName(track.FullFileName);
      string newFileName = "";
      newFileName = track.FullFileName;

      /*
      if (originalFileName != track.FileName)
      {
        string ext = Path.GetExtension(track.File.Name);
        newFileName = Path.Combine(Path.GetDirectoryName(track.File.Name),
                                   String.Format("{0}{1}", Path.GetFileNameWithoutExtension(track.FileName), ext));
      }
      else
      {
        newFileName = track.FullFileName;
      }
       */


      string sql = String.Format(
        @"update tracks 
              set strArtist = '{0}', strAlbumArtist = '{1}', strAlbum = '{2}', 
              strGenre = '{3}', strTitle = '{4}', iTrack = {5}, iNumTracks = {6}, 
              iYear = {7}, iRating = {8}, iDisc = {9}, iNumDisc = {10}, strLyrics = '{11}', strPath = '{12}'
            where strPath = '{13}'",
        Util.RemoveInvalidChars(FormatMultipleEntry(track.Artist)),
        Util.RemoveInvalidChars(FormatMultipleEntry(track.AlbumArtist)), Util.RemoveInvalidChars(track.Album),
        Util.RemoveInvalidChars(FormatMultipleEntry(track.Genre)), Util.RemoveInvalidChars(track.Title), trackNumber,
        trackTotal,
        track.Year, track.Rating, discNumber, discTotal,
        Util.RemoveInvalidChars(track.Lyrics), Util.RemoveInvalidChars(newFileName),
        Util.RemoveInvalidChars(track.FullFileName)
        );


      string connection = string.Format(@"Data Source={0}", db);
      try
      {
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = CommandType.Text;
          cmd.CommandText = sql;
          int result = cmd.ExecuteNonQuery();
        }
        conn.Close();
      }
      catch (Exception ex)
      {
        log.Error("Database Update: Error executing sql: {0}", ex.Message);
      }
    }

    /// <summary>
    ///   Multiple Entry fields need to be formatted to contain a | at the end to be able to search correct
    /// </summary>
    /// <param name = "str"></param>
    /// <returns>Formatted string</returns>
    private string FormatMultipleEntry(string str)
    {
      string[] strSplit = str.Split(new[] { ';', '|' });
      // Can't use a simple String.Join as i need to trim all the elements 
      string strJoin = "| ";
      foreach (string strTmp in strSplit)
      {
        string s = strTmp.Trim();
        strJoin += String.Format("{0} | ", s.Trim());
      }
      return strJoin;
    }

    #endregion

    #endregion

    #region Private Methods

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
      foreach (DataGridViewColumn col in tracksGrid.Columns)
      {
        col.HeaderText = localisation.ToString("column_header", col.Name);
      }
      contextMenu.Items[0].Text = localisation.ToString("contextmenu", "Copy");
      contextMenu.Items[1].Text = localisation.ToString("contextmenu", "Cut");
      contextMenu.Items[2].Text = localisation.ToString("contextmenu", "Paste");
      contextMenu.Items[3].Text = localisation.ToString("contextmenu", "Delete");
      contextMenu.Items[5].Text = localisation.ToString("contextmenu", "SelectAll");
      contextMenu.Items[7].Text = localisation.ToString("contextmenu", "CoverLookup");
      contextMenu.Items[8].Text = localisation.ToString("contextmenu", "AddBurner");
      contextMenu.Items[9].Text = localisation.ToString("contextmenu", "AddConverter");
      contextMenu.Items[10].Text = localisation.ToString("contextmenu", "AddPlaylist");
      contextMenu.Items[12].Text = localisation.ToString("contextmenu", "SavePlaylist");
      contextMenu.Items[13].Text = localisation.ToString("contextmenu", "CreateFolderThumb");
    }

    #endregion

    #region Miscellaneous Methods

    /// <summary>
    ///   Create the Columns of the Grid based on the users setting
    /// </summary>
    private void CreateColumns()
    {
      log.Trace(">>>");

      // Now create the columns 
      foreach (GridViewColumn column in gridColumns.Settings.Columns)
      {
        tracksGrid.Columns.Add(Util.FormatGridColumn(column));
      }

      // Add a dummy column and set the property of the last column to fill
      DataGridViewColumn col = new DataGridViewTextBoxColumn();
      col.Name = "dummy";
      col.HeaderText = "";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 5;
      tracksGrid.Columns.Add(col);
      tracksGrid.Columns[tracksGrid.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

      log.Trace("<<<");
    }

    /// <summary>
    ///   Save the settings
    /// </summary>
    private void SaveSettings()
    {
      // Save the Width of the Columns
      int i = 0;
      foreach (DataGridViewColumn column in tracksGrid.Columns)
      {
        // Don't save the dummy column
        if (i == tracksGrid.Columns.Count - 1)
          break;

        gridColumns.SaveColumnSettings(column, i);
        i++;
      }
      gridColumns.SaveSettings();
    }

    /// <summary>
    ///   Adds a Track to the data grid
    /// </summary>
    /// <param name = "track"></param>
    private void AddTrack(TrackData track)
    {
      if (track == null)
        return;


      if (tracksGrid.InvokeRequired)
      {
        ThreadSafeAddTracksDelegate d = AddTrack;
        tracksGrid.Invoke(d, new object[] { track });
        return;
      }

      // Validate the MP3 File
      if (Options.MainSettings.MP3Validate && track.IsMp3)
      {
        string strError = "";
        track.MP3ValidationError = MP3Val.ValidateMp3File(track.FullFileName, out strError);
        track.MP3ValidationErrorText = strError;
      }

      Options.Songlist.Add(track);
    }

    private void ShowForm(Form f)
    {
      int x = _main.ClientSize.Width / 2 - f.Width / 2;
      int y = _main.ClientSize.Height / 2 - f.Height / 2;
      Point clientLocation = _main.Location;
      x += clientLocation.X;
      y += clientLocation.Y;
      f.Location = new Point(x, y);
      f.Show();
    }

    private void ChangeErrorRowColor()
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        TrackData track = Options.Songlist[row.Index];

        if (track.MP3ValidationError == Util.MP3Error.NoError)
        {
          continue;
        }

        SetColorMP3Errors(row.Index, track.MP3ValidationError);
        tracksGrid.Rows[row.Index].Cells[0].ToolTipText = track.MP3ValidationErrorText;
        track.Status = 3;
      }
    }

    /// <summary>
    ///   Sets the maximum value of Progressbar
    /// </summary>
    /// <param name = "maxCount"></param>
    public void SetProgressBar(int maxCount)
    {
      _main.progressBar1.Maximum = maxCount == 0 ? 100 : maxCount;
      _main.progressBar1.Value = 0;
      _progressCancelled = false;
      SetWaitCursor();
    }

    /// <summary>
    ///   Reset the Progressbar to Initiaövfalue
    /// </summary>
    private void ResetProgressBar()
    {
      _main.progressBar1.Value = 0;
      _progressCancelled = false;
      ResetWaitCursor();
    }

    /// <summary>
    ///   Sets the WaitCursor during various operations
    /// </summary>
    private void SetWaitCursor()
    {
      _main.Cursor = Cursors.WaitCursor;
      tracksGrid.Cursor = Cursors.WaitCursor;
      _waitCursorActive = true;
    }

    /// <summary>
    ///   Resets the WaitCursor to the default
    /// </summary>
    private void ResetWaitCursor()
    {
      _main.Cursor = Cursors.Default;
      tracksGrid.Cursor = Cursors.Default;
      _waitCursorActive = false;
    }

    #endregion

    #region Filter

    private bool ApplyFileFilter(string fileName)
    {
      foreach (string extension in _filterFileExtensions)
      {
        if (extension != "*.*" && Path.GetExtension(fileName) != extension)
          continue;

        if (Regex.IsMatch(Path.GetFileNameWithoutExtension(fileName), MakeRegexp(_filterFileMask)))
        {
          return true;
        }
      }
      return false;
    }

    private bool ApplyTagFilter(TrackData track)
    {
      if (!_main.TreeView.ActiveFilter.UseTagFilter)
      {
        return true;
      }

      List<TreeViewTagFilter> filter = _main.TreeView.ActiveFilter.TagFilter;
      List<bool> results = new List<bool>(filter.Count);


      bool numericCompare = false; // For year, bpm, etc.
      int index = 0;
      int searchNumber = 0;
      string searchstring = "";
      foreach (TreeViewTagFilter tagfilter in filter)
      {
        switch (tagfilter.Field)
        {
          case "artist":
            searchstring = track.Artist;
            break;

          case "albumartist":
            searchstring = track.AlbumArtist;
            break;

          case "album":
            searchstring = track.Album;
            break;

          case "title":
            searchstring = track.Title;
            break;

          case "genre":
            searchstring = track.Genre;
            break;

          case "comment":
            searchstring = track.Comment;
            break;

          case "composer":
            searchstring = track.Composer;
            break;

          case "conductor":
            searchstring = track.Conductor;
            break;

          // Tags with are checked for existence or non-existence
          case "picture":
            searchstring = track.Pictures.Count > 0 ? "True" : "False";
            break;

          case "lyrics":
            searchstring = track.Lyrics != null ? "True" : "False";
            break;

          // Numeric Tags
          case "track":
            numericCompare = true;
            searchNumber = (int)track.TrackNumber;
            break;

          case "numtracks":
            numericCompare = true;
            searchNumber = (int)track.TrackCount;
            break;

          case "disc":
            numericCompare = true;
            searchNumber = (int)track.DiscNumber;
            break;

          case "numdiscs":
            numericCompare = true;
            searchNumber = (int)track.DiscCount;
            break;

          case "year":
            numericCompare = true;
            searchNumber = track.Year;
            break;

          case "rating":
            numericCompare = true;
            searchNumber = track.Rating;
            break;

          case "bpm":
            numericCompare = true;
            searchNumber = track.BPM;
            break;

          case "bitrate":
            numericCompare = true;
            searchNumber = Convert.ToInt16(track.BitRate);
            break;

          case "samplerate":
            numericCompare = true;
            searchNumber = Convert.ToInt16(track.SampleRate);
            break;

          case "channels":
            numericCompare = true;
            searchNumber = Convert.ToInt16(track.Channels);
            break;
        }

        // Now check the filter value, if we have a negation
        string searchValue = tagfilter.FilterValue;
        bool negation = false;
        if (searchValue.Length > 0 && searchValue[0] == '!')
        {
          searchValue = searchValue.Substring(1);
          negation = true;
        }

        bool match = false;
        if (numericCompare)
        {
          try
          {
            searchValue = searchValue.Trim();
            if (searchValue == "")
              searchValue = "0";

            if (searchValue.StartsWith("<="))
            {
              searchValue = searchValue.Substring(2);
              match = searchNumber <= Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith(">="))
            {
              searchValue = searchValue.Substring(2);
              match = searchNumber >= Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith("<"))
            {
              searchValue = searchValue.Substring(1);
              match = searchNumber < Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith(">"))
            {
              searchValue = searchValue.Substring(1);
              match = searchNumber > Int32.Parse(searchValue);
            }
            else if (searchValue.StartsWith("="))
            {
              searchValue = searchValue.Substring(1);
              match = searchNumber == Int32.Parse(searchValue);
            }
            else
            {
              match = searchNumber == Int32.Parse(searchValue);
            }
          }
          catch (FormatException)
          {
            match = false;
          }
        }
        else
        {
          match = Regex.IsMatch(searchstring, MakeRegexp(searchValue));
        }

        if (negation)
        {
          match = !match;
        }

        results.Insert(index, match);
        index++;
      }


      index = 0;
      bool ok = false;
      foreach (bool result in results)
      {
        if (index + 1 == results.Count)
        {
          if (index == 0)
          {
            ok = result;
          }
          break;
        }

        bool nextresult = results[index + 1];

        if (filter[index].FilterOperator == "and")
        {
          ok = result && nextresult;
        }
        else
        {
          ok = result || nextresult;
        }

        if (!ok)
        {
          break;
        }
      }

      return ok;
    }

    private string MakeRegexp(string searchValue)
    {
      if (searchValue == "")
        return "^$";

      // Now replace furter meta chars with their backslashed version
      searchValue = searchValue.Replace(@"\", @"\\").Replace("|", @"\|").Replace("(", @"\(").Replace(")", @"\)");
      searchValue = searchValue.Replace(@"{", @"\{").Replace("[", @"\[").Replace("^", @"\^").Replace("$", @"\$");
      searchValue = searchValue.Replace(@"+", @"\+").Replace("*", @"\*").Replace(".", @"\.");
      searchValue = searchValue.Replace(@"<", @"\<").Replace(">", @"\<");

      if (searchValue.Substring(0, 2) == @"\*")
      {
        searchValue = ".*" + searchValue.Substring(2);
      }
      else
      {
        searchValue = "^" + searchValue;
      }

      if (searchValue.Substring(searchValue.Length - 2) == @"\*")
      {
        searchValue = searchValue.Substring(0, searchValue.Length - 2) + ".*";
      }

      // Replace any wildcards that the user inserted by the correct reg exp syntax
      searchValue = searchValue.Replace("?", ".");

      return searchValue;
    }

    #endregion

    #endregion

    #region EventHandler

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
            tracksGrid.BackgroundColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            break;
          }

        case "setwaitcursor":
        {
          SetWaitCursor();
          break;
        }

        case "resetwaitcursor":
        {
          ResetWaitCursor();
          break;
        }
      }
    }

    /// <summary>
    ///   Handles changes in the column width
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      // On startup we get sometimes an exception
      try
      {
        if (tracksGrid.Rows.Count > 0)
          tracksGrid.InvalidateRow(e.Column.Index);
      }
      catch (Exception) { }
    }

    /// <summary>
    ///   Handles editing of data columns
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      // For combo box and check box cells, commit any value change as soon
      // as it is made rather than waiting for the focus to leave the cell.
      if (!tracksGrid.CurrentCell.OwningColumn.GetType().Equals(typeof(DataGridViewTextBoxColumn)))
      {
        tracksGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
      }
    }

    /// <summary>
    ///   Only allow valid values to be entered.
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      if (e.Exception == null) return;

      // If the user-specified value is invalid, cancel the change 
      // and display the error icon in the row header.
      if ((e.Context & DataGridViewDataErrorContexts.Commit) != 0 &&
          (typeof(FormatException).IsAssignableFrom(e.Exception.GetType()) ||
           typeof(ArgumentException).IsAssignableFrom(e.Exception.GetType())))
      {
        tracksGrid.Rows[e.RowIndex].ErrorText = localisation.ToString("message", "DataEntryError");
        e.Cancel = true;
      }
      else
      {
        // Rethrow any exceptions that aren't related to the user input.
        e.ThrowException = true;
      }
    }

    /// <summary>
    ///   We're leaving a Cell after edit
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      // Ensure that the error icon in the row header is hidden.
      tracksGrid.Rows[e.RowIndex].ErrorText = "";
    }

    /// <summary>
    ///   Value of Cell has changed
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      // When changing the Status or the Header Text, ignore the Cell Changed event
      if (e.ColumnIndex == 0 || e.RowIndex == -1)
        return;

      _itemsChanged = true;
      SetBackgroundColorChanged(e.RowIndex);
      TrackData track = Options.Songlist[e.RowIndex];
      _main.TagEditForm.FillForm();
      Options.Songlist[e.RowIndex] = track;
      track.Changed = true;
    }

    /// <summary>
    ///   Handle Right Mouse Click to show Column Config Dialogue
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      // On right click on Header show config dialogue
      if (e.Button == MouseButtons.Right)
      {
        ColumnSelect dialog = new ColumnSelect(this);
        dialog.ShowDialog();
        return;
      }

      if (e.ColumnIndex == 0) // Don't Sort the Status Column
      {
        return;
      }

      // We clicked on the column, so let's sort it
      DataGridViewColumn newColumn = tracksGrid.Columns[e.ColumnIndex];
      ListSortDirection direction;

      // We store the Sortoder in the Columns Tag
      object currentSortOrder = newColumn.Tag;
      if (currentSortOrder != null)
      {
        // Sort the same column again, reversing the SortOrder.
        if ((int)currentSortOrder == 0)
        {
          direction = ListSortDirection.Descending;
        }
        else
        {
          direction = ListSortDirection.Ascending;
        }
      }
      else
      {
        direction = ListSortDirection.Ascending;
        // Clear the Glyph for all 
        foreach (DataGridViewColumn col in tracksGrid.Columns)
        {
          col.Tag = null;
          col.HeaderCell.SortGlyphDirection = SortOrder.None;
        }
      }

      // Sort the selected column.

      PropertyDescriptor prop = _propColl.Find(newColumn.Name, false);
      Options.Songlist.Sort(prop, direction);

      newColumn.HeaderCell.SortGlyphDirection =
          direction == ListSortDirection.Ascending ?
          SortOrder.Ascending : SortOrder.Descending;
      newColumn.Tag = (int)direction;

      tracksGrid_Sorted();
      tracksGrid.Update();
      tracksGrid.Refresh();
    }

    /// <summary>
    ///   THe Grid has been sorted. We need to change the Background Color of the changed Rows
    /// </summary>
    private void tracksGrid_Sorted()
    {
      int i = 0;
      // Set the Color for changed rows again
      foreach (TrackData track in Options.Songlist)
      {
        if (track.Changed)
        {
          SetBackgroundColorChanged(i);
        }
        else
        {
          SetGridRowColors(i);
        }
        i++;
      }
    }

    /// <summary>
    ///   We want to get Control, when editing a Cell, so that we can control the input
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      DataGridView view = sender as DataGridView;
      string colName = view.CurrentCell.OwningColumn.Name;
      if (colName == "Track" || colName == "Disc")
      {
        TextBox txtbox = e.Control as TextBox;
        if (txtbox != null)
        {
          txtbox.KeyPress += txtbox_KeyPress;
        }
      }
    }

    /// <summary>
    ///   Allow only Digits and the slash when enetering data for Track or Disc
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void txtbox_KeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;

      if (!Char.IsDigit(keyChar) // 0 - 9
          &&
          keyChar != 8 // backspace
          &&
          keyChar != 13 // enter
          &&
          keyChar != '/'
        )
      {
        //  Do not display the keystroke
        e.Handled = true;
      }
    }

    /// <summary>
    ///   Handle Left Mouse Click for Numbering on Click
    ///   Handle Right Mouse Click to open the context Menu in the Grid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        Point mouse = tracksGrid.PointToClient(Cursor.Position);
        DataGridView.HitTestInfo selectedRow = tracksGrid.HitTest(mouse.X, mouse.Y);

        if (selectedRow.Type != DataGridViewHitTestType.ColumnHeader)
          contextMenu.Show(tracksGrid, new Point(e.X, e.Y));
      }
      else
      {
        // Handle Numbering on Click
        if (_main.NumberingOnClick)
        {
          Point mouse = tracksGrid.PointToClient(Cursor.Position);
          DataGridView.HitTestInfo selectedRow = tracksGrid.HitTest(mouse.X, mouse.Y);

          if (selectedRow.Type != DataGridViewHitTestType.ColumnHeader)
          {
            int numberValue = _main.AutoNumber;
            if (numberValue == -1)
              return;

            if (selectedRow.RowIndex == -1)
              return;

            TrackData track = Options.Songlist[selectedRow.RowIndex];

            // Get the Number of tracks, so that we don't loose it
            string[] tracks = track.Track.Split('/');
            string numTracks = "0";
            if (tracks.Length > 1)
            {
              numTracks = tracks[1];
            }

            track.Track = string.Format("{0}/{1}", numberValue, numTracks);
            SetBackgroundColorChanged(selectedRow.RowIndex);
            track.Changed = true;
            Options.Songlist[selectedRow.RowIndex] = track;
            _itemsChanged = true;
            _main.AutoNumber = numberValue + 1;
            tracksGrid.Refresh();
            tracksGrid.Parent.Refresh();
          }
        }
      }
    }

    /// <summary>
    ///   Mouse is over Trackgrid
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseEnter(object sender, EventArgs e)
    {
      // Numbering on Click enabled
      if (_main.NumberingOnClick)
        tracksGrid.Cursor = _numberingCursor;
      else
      {
        if (_waitCursorActive)
        {
          tracksGrid.Cursor = Cursors.WaitCursor;
        }
        else
        {
          tracksGrid.Cursor = Cursors.Default;
        }
      }
    }

    private void tracksGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (_findResult == null)
      {
        e.Handled = false;
        return;
      }

      if ((e.ColumnIndex == _findResult.Column) && (e.RowIndex == _findResult.Row))
      {
        // Draw Merged Cell
        Graphics g = e.Graphics;
        bool selected = ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);
        Color fcolor = (selected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor);
        Color bcolor = (selected ? e.CellStyle.SelectionBackColor : e.CellStyle.BackColor);
        Font font = e.CellStyle.Font;

        // Split up the cell content into 3 pieces
        string cellContent = e.Value == null ? "" : e.Value.ToString();
        ;
        string[] results = new string[3];
        results[0] = cellContent.Substring(0, _findResult.StartPos);
        results[1] = cellContent.Substring(_findResult.StartPos, _findResult.Length);
        results[2] = cellContent.Substring(_findResult.StartPos + _findResult.Length);

        int x = e.CellBounds.Left + e.CellStyle.Padding.Left;
        int y = e.CellBounds.Top + e.CellStyle.Padding.Top + 4;

        for (int i = 0; i < 3; i++)
        {
          Size proposedSize = new Size(int.MaxValue, int.MaxValue);

          int width;
          Size size;
          Color color;
          Color backColor;
          TextFormatFlags textFlags;
          if (i == 0)
          {
            size = TextRenderer.MeasureText(e.Graphics, results[i], font, proposedSize);
            width = size.Width - e.CellStyle.Padding.Left;
            color = fcolor;
            backColor = bcolor;
            textFlags = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.EndEllipsis;
          }
          else if (i == 1)
          {
            size = TextRenderer.MeasureText(e.Graphics, results[i], font, proposedSize, TextFormatFlags.NoPadding);
            width = size.Width;
            color = theme.CurrentTheme.FindReplaceForeColor;
            backColor = theme.CurrentTheme.FindReplaceBackColor;
            textFlags = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding;
          }
          else
          {
            size = TextRenderer.MeasureText(e.Graphics, results[i], font, proposedSize);
            width = size.Width - e.CellStyle.Padding.Right;
            color = fcolor;
            backColor = bcolor;
            textFlags = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.EndEllipsis;
          }

          int height = size.Height + (e.CellStyle.Padding.Top + e.CellStyle.Padding.Bottom);
          Rectangle rect = new Rectangle(x, y - 4, width, e.CellBounds.Height - 2);
          g.FillRectangle(new SolidBrush(backColor), rect);
          TextRenderer.DrawText(e.Graphics, results[i], font, new Rectangle(x, y, width, height), color,
                                textFlags);
          x += width;
        }

        e.Handled = true;
        return;
      }
      e.Handled = false;
    }

    /// <summary>
    ///   The Progress Cancel has been fired from the Statusbar Button
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void ProgressCancel_Executed(object sender, CommandExecutedEventArgs e)
    {
      _progressCancelled = true;
      ResetWaitCursor();
    }

    /// <summary>
    ///   We're hovering over the Progress Cancel button.
    ///   If the Wait Cursor is active, change it to Default
    /// </summary>
    private void ProgressCancel_Hover(object sender, EventArgs e)
    {
      if (_waitCursorActive)
      {
        _main.Cursor = Cursors.Default;
      }
    }

    /// <summary>
    ///   We are leaving the Button again. If WaitCursor is active, we should set it back again
    /// </summary>
    private void ProgressCancel_Leave(object sender, EventArgs e)
    {
      if (_waitCursorActive)
      {
        _main.Cursor = Cursors.WaitCursor;
      }
    }

    #region Virtual Mode Handling

    /// <summary>
    /// This method is invoked, whenever the Grid decides that it needs a cell value.
    /// We get the required row from from the Songlist and retrieve the cell value using reflection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      // Don't handle empty folders
      if (Options.Songlist.Count == 0)
      {
        return;
      }

      if (e.RowIndex == Options.Songlist.Count)
      {
        return;
      }

      TrackData track = Options.Songlist[e.RowIndex];

	    if (track == null)
	    {
		    return;
	    }

      // Handle the status column
      if (e.ColumnIndex == 0)
      {
        if (track.Changed)
        {
          e.Value = Properties.Resources.Warning;
          return;
        }

        switch (track.Status)
        {
          case -1:
            return;

          case 0:
            e.Value = Properties.Resources.Complete_OK;
            break;

          case 1:
            e.Value = Properties.Resources.Warning;
            break;

          case 2:
            e.Value = Properties.Resources.CriticalError;
            break;

          case 3:
            e.Value = Properties.Resources.ribbon_BrokenSong_16x;
            break;

          case 4:
            e.Value = Properties.Resources.ribbon_FixedSong_16x;
            break;
        }

        return;
      }

      string colName = tracksGrid.Columns[e.ColumnIndex].Name;
      if (colName == "dummy")
      {
        return;
      }

      // Get the row from the bindinglist and set the required cell
      e.Value = track.GetType().InvokeMember(colName, BindingFlags.GetField | BindingFlags.GetProperty, null, track,
                                                 new object[] { });
    }
    
    /// <summary>
    /// A cell value has changed. Push it back into the Songlist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tracksGrid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      // Don't handle the Status column
      if (e.ColumnIndex == 0)
      {
        return;
      }

      Options.Songlist[e.RowIndex].GetType().InvokeMember(tracksGrid.Columns[e.ColumnIndex].Name,
                                      BindingFlags.SetProperty | BindingFlags.SetField, null, Options.Songlist[e.RowIndex],
                                      new object[] { e.Value });
    }

    #endregion

    #region Drag & Drop

    /// <summary>
    ///   Handle Drag and Drop Operation
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left && tracksGrid.SelectedRows.Count > 0)
      {
        // Remember the point where the mouse down occurred. The DragSize indicates
        // the size that the mouse can move before a drag event should be started.                
        Size dragSize = SystemInformation.DragSize;

        // Create a rectangle using the DragSize, with the mouse position being
        // at the center of the rectangle.
        _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)),
                                              dragSize);
      }
      else
        _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    ///   The mouse moves. Do a Drag & Drop if necessary
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        // If the mouse moves outside the rectangle, start the drag.
        if (_dragBoxFromMouseDown != Rectangle.Empty &&
            !_dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          // The screenOffset is used to account for any desktop bands 
          // that may be at the top or left side of the screen when 
          // determining when to cancel the drag drop operation.
          _screenOffset = SystemInformation.WorkingArea.Location;

          List<TrackData> selectedRows = new List<TrackData>();
          foreach (DataGridViewRow row in tracksGrid.Rows)
          {
            if (!row.Selected)
              continue;

            TrackData track = Options.Songlist[row.Index];
            selectedRows.Add(track);
          }
          tracksGrid.DoDragDrop(selectedRows, DragDropEffects.Copy | DragDropEffects.Move);
        }
      }
    }

    /// <summary>
    ///   The Mouse has been released
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_MouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    ///   Determines, if Drag and drop should continue
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void tracksGrid_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      DataGridView dg = sender as DataGridView;

      if (dg != null)
      {
        Form f = dg.FindForm();
        // Cancel the drag if the mouse moves off the form. The screenOffset
        // takes into account any desktop bands that may be at the top or left
        // side of the screen.
        if (((MousePosition.X - _screenOffset.X) < f.DesktopBounds.Left) ||
            ((MousePosition.X - _screenOffset.X) > f.DesktopBounds.Right + _main.Player.PlayListForm.Width) ||
            ((MousePosition.Y - _screenOffset.Y) < f.DesktopBounds.Top) ||
            ((MousePosition.Y - _screenOffset.Y) > f.DesktopBounds.Bottom))
        {
          e.Action = DragAction.Cancel;
        }
      }
    }

    #endregion

    #region Context Menu Handler

    /// <summary>
    ///   Add to Burner
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_AddToBurner(object o, EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        _main.BurnGridView.AddToBurner(track);
      }
    }

    /// <summary>
    ///   Add to Converter Grid
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_AddToConvert(object o, EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        _main.ConvertGridView.AddToConvert(track);
      }
    }

    /// <summary>
    ///   Add to Playlist
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_AddToPlayList(object o, EventArgs e)
    {
      // get current playlist count
      int playlistCount = _main.Player.PlayList.Count;
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        PlayListData playListItem = new PlayListData();
        playListItem.FileName = track.FullFileName;
        playListItem.Title = track.Title;
        playListItem.Artist = track.Artist;
        playListItem.Album = track.Album;
        playListItem.Duration = track.Duration.Substring(3, 5); // Just get Minutes and seconds
        _main.Player.PlayList.Add(playListItem);
      }

      // Start playing the songs just added, if player is idle
      if (!_main.Player.IsPlaying && _main.Player.PlayList.Count > playlistCount)
      {
        int index = playlistCount;
        if (index == -1)
        {
          index = 0;
        }
        _main.Player.Play(index);
      }
    }

    /// <summary>
    ///   Save as Playlist
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_SaveAsPlayList(object o, EventArgs e)
    {
      SortableBindingList<PlayListData> playList = new SortableBindingList<PlayListData>();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        PlayListData playListItem = new PlayListData();
        playListItem.FileName = track.FullFileName;
        playListItem.Title = track.Title;
        playListItem.Artist = track.Artist;
        playListItem.Album = track.Album;
        playListItem.Duration = track.Duration.Substring(3, 5); // Just get Minutes and seconds
        playList.Add(playListItem);
      }

      SaveFileDialog sFD = new SaveFileDialog();
      sFD.InitialDirectory = _main.CurrentDirectory;
      sFD.Filter = "M3U Format (*.m3u)|*.m3u|PLS Format (*.pls)|*.pls";
      if (sFD.ShowDialog() == DialogResult.OK)
      {
        IPlayListIO saver = PlayListFactory.CreateIO(sFD.FileName);
        saver.Save(playList, sFD.FileName, true);
      }
    }

    /// <summary>
    ///   Create a Folder Thumb out of the selected Track Picture
    /// </summary>
    /// <param name = "o"></param>
    /// <param name = "e"></param>
    public void tracksGrid_CreateFolderThumb(object o, EventArgs e)
    {
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
        {
          continue;
        }

        TrackData track = Options.Songlist[row.Index];
        Util.SavePicture(track);
      }
    }

    public void tracksGrid_Copy(object o, EventArgs e)
    {
      contextMenu.Items[2].Enabled = true; // Enable the Paste Menu item
      Options.CopyPasteBuffer.Clear();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        Options.CopyPasteBuffer.Add(track);
      }
      _actionCopy = true;
    }

    public void tracksGrid_Cut(object o, EventArgs e)
    {
      contextMenu.Items[2].Enabled = true; // Enable the Paste Menu item
      Options.CopyPasteBuffer.Clear();
      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = Options.Songlist[row.Index];
        Options.CopyPasteBuffer.Add(track);
      }
      _actionCopy = false;
    }

    public void tracksGrid_Paste(object o, EventArgs e)
    {
      contextMenu.Items[2].Enabled = false; // Disable the Paste Menu item
      if (Options.CopyPasteBuffer.Count == 0)
      {
        log.Debug("Copy Paste: No files in Copy Buffer");
        return;
      }


      foreach (TrackData track in Options.CopyPasteBuffer)
      {
        string targetFile = Path.Combine(_main.CurrentDirectory, track.FileName);
        try
        {
          if (_actionCopy)
          {
            log.Debug("TracksGrid: Copying file {0} to {1}", track.FullFileName, targetFile);
            FileSystem.CopyFile(track.FullFileName, targetFile, UIOption.AllDialogs, UICancelOption.DoNothing);
          }
          else
          {
            log.Debug("TracksGrid: Moving file {0} to {1}", track.FullFileName, targetFile);
            FileSystem.MoveFile(track.FullFileName, targetFile, UIOption.AllDialogs, UICancelOption.DoNothing);
          }
        }
        catch (Exception ex)
        {
          log.Error("TracksGrid: Error copying / moving file {0}. {1}", track.FullFileName, ex.Message);
        }
      }
      _main.RefreshTrackList();
    }


    private void tracksGrid_Delete(object sender, EventArgs e)
    {
      DeleteTracks();
    }

    private void tracksGrid_SelectAll(object sender, EventArgs e)
    {
      tracksGrid.SelectAll();
    }


    private void lookupTitlleOnGoogleImagesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (tracksGrid.SelectedRows.Count == 0)
      {
        return;
      }

      TrackData track = Options.Songlist[tracksGrid.SelectedRows[0].Index];
      string songString = track.Artist + " " + track.Title;
      songString = songString.Replace(" ", "+");

      string url = "https://www.google.com/search?tbm=isch&q=" + songString;
      System.Diagnostics.Process.Start(url);
    }

    #endregion

    #endregion
  }
}