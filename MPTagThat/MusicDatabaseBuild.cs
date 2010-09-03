using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading;
using TagLib;

using MPTagThat.Core;

namespace MPTagThat
{
  public class MusicDatabaseBuild
  {
    #region Variables
    private ILogger log = ServiceScope.Get<ILogger>();
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private string _databaseName;
    private SQLiteConnection conn = null;

    private Thread fileThread = null;
    private Thread scanThread = null;

    List<FileInfo> files = new List<FileInfo>();
    private string _folder;
    private int _processCount = 0;
    private int _audioFiles = 0;
    private bool _scanHasRun = false;
    private bool _abortScan = false;

    private TimeSpan _ts;
    private DateTime _startTime;
    private string _trackPerSecSummary = "";
    #endregion

    #region Properties
    public bool ScanActive
    {
      get
      {
        if (scanThread == null)
          return false;

        if (scanThread.ThreadState == ThreadState.Running)
          return true;

        return false;
      }
    }

    public bool AbortScan
    {
      set { _abortScan = value; }
    }
    #endregion

    #region Public Methods
    #region Database Create
    public void CreateMusicDatabase(string databaseName)
    {
      _databaseName = databaseName;

      OpenConnection();

      string sql = @"CREATE TABLE tracks ( " +
        // Unique id Autoincremented
          "idTrack integer primary key autoincrement, " +
        // Full Path of the file. 
          "strPath text, " +
        // Artist
          "strArtist text, strAlbumArtist text, " +
        // Album (How to handle Various Artist Albums)
          "strAlbum text, " +
        // Genre (multiple genres)
          "strGenre text, " +
        // Song
          "strTitle text, iTrack integer, iNumTracks integer, iYear integer, " +
          "iRating integer, iDisc integer, iNumDisc integer, " +
          "strLyrics text)";

      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxpath_strPath ON tracks(strPath ASC)";
      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxartist_strArtist ON tracks(strArtist ASC)";
      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxalbum_strAlbumArtist ON tracks(strAlbumArtist ASC)";
      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxalbum_strAlbum ON tracks(strAlbum ASC)";
      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxgenre_strGenre ON tracks(strGenre ASC)";
      ExecuteDirectSQL(sql);

      sql = "CREATE TABLE artist ( idArtist integer primary key autoincrement, strArtist text)";
      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxartisttable_strArtist ON artist(strArtist ASC)";
      ExecuteDirectSQL(sql);

      sql = "CREATE TABLE albumartist ( idAlbumArtist integer primary key autoincrement, strAlbumArtist text)";
      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxalbumartisttable_strAlbumArtist ON albumartist(strAlbumArtist ASC)";
      ExecuteDirectSQL(sql);

      sql = "CREATE TABLE genre ( idGenre integer primary key autoincrement, strGenre text)";
      ExecuteDirectSQL(sql);

      sql = "CREATE INDEX idxgenretable_strGenre ON genre(strGenre ASC)";
      ExecuteDirectSQL(sql);

      CloseConnection();
    }
    #endregion

    #region Database build
    /// <summary>
    /// Start the threads to fill the music database
    /// </summary>
    /// <param name="folder"></param>
    public void FillMusicDatabase(string folder, string databaseName)
    {
      _folder = folder;
      _databaseName = databaseName;

      if (fileThread == null)
      {
        fileThread = new Thread(new ThreadStart(GetFilesThread));
        fileThread.Name = "Files";
      }

      if (fileThread.ThreadState != ThreadState.Running)
      {
        fileThread = new Thread(new ThreadStart(GetFilesThread));
        fileThread.Start();
      }

      if (scanThread == null)
      {
        scanThread = new Thread(new ThreadStart(UpdateDBThread));
        scanThread.Name = "Scan";
      }

      if (scanThread.ThreadState != ThreadState.Running)
      {
        scanThread = new Thread(new ThreadStart(UpdateDBThread));
        scanThread.Start();
      }
    }

    /// <summary>
    /// Get Files
    /// </summary>
    private void GetFilesThread()
    {
      log.Debug("Database Update: Starting Get File thread");
      _startTime = DateTime.Now;
      files.Clear();
      GetFiles(_folder, ref files, true);
      log.Debug("Database Update: Get File thread ended. found {0} files", files.Count);
    }

    /// <summary>
    /// Retrieve the folder recursive
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="foundFiles"></param>
    /// <param name="recursive"></param>
    void GetFiles(string folder, ref List<FileInfo> foundFiles, bool recursive)
    {
      try
      {
        if (recursive)
        {
          string[] subFolders = Directory.GetDirectories(folder);
          for (int i = 0; i < subFolders.Length; ++i)
          {
            GetFiles(subFolders[i], ref foundFiles, recursive);
          }
        }

        FileInfo[] files = new DirectoryInfo(folder).GetFiles();
        foundFiles.AddRange(files);
      }
      catch (Exception ex)
      {
        log.Error(ex);
      }
    }


    private void UpdateDBThread()
    {
      log.Debug("Database Update: Starting Database update thread");
      _processCount = 0;
      _audioFiles = 0;

      OpenConnection();
      while ((fileThread.ThreadState == ThreadState.Running || _processCount < files.Count) && !_abortScan)
      {
        if (_processCount == files.Count || files.Count == 0)
        {
          continue;
        }
        string fileName = files[_processCount].FullName;
        if (Util.IsAudio(fileName))
        {
          _audioFiles++;
          try
          {
            TagLib.ByteVector.UseBrokenLatin1Behavior = true;
            TagLib.File file = TagLib.File.Create(fileName);
            AddSong(file);
          }
          catch (CorruptFileException)
          {
            log.Warn("FolderScan: Ignoring track {0} - Corrupt File!", fileName);
          }
          catch (UnsupportedFormatException)
          {
            log.Warn("FolderScan: Ignoring track {0} - Unsupported format!", fileName);
          }
        }
        _processCount++;
      }

      CloseConnection();
      _scanHasRun = true;

      DateTime stopTime = DateTime.Now;
      _ts = stopTime - _startTime;
      float fSecsPerTrack = ((float)_ts.TotalSeconds / (float)files.Count);

      _trackPerSecSummary = "";
      if (files.Count > 0)
      {
        _trackPerSecSummary = string.Format(localisation.ToString("Settings", "DBScanTrackSummary"), fSecsPerTrack);
      }

      log.Info(
        "Database Update: Music database update done.  Processed {0} tracks in: {1:d2}:{2:d2}:{3:d2}{4}",
        _audioFiles, _ts.Hours, _ts.Minutes, _ts.Seconds, _trackPerSecSummary);

      log.Debug("Database Update: Ending Database Update thread");
    }

    /// <summary>
    /// Adds the song to the database
    /// </summary>
    /// <param name="file"></param>
    private void AddSong(TagLib.File file)
    {
      string artist = "";
      string[] artists = file.Tag.Performers;
      if (artists.Length > 0)
      {
        artist = FormatMultipleEntry(string.Join(";", artists));
      }

      string albumartist = "";
      string[] albumartists = file.Tag.AlbumArtists;
      if (albumartists.Length > 0)
      {
        albumartist = FormatMultipleEntry(string.Join(";", albumartists));
      }

      string genre = "";
      string[] genres = file.Tag.Genres;
      if (genres.Length > 0)
      {
        genre = FormatMultipleEntry(string.Join(";", genres));
      }

      string title = file.Tag.Title == null ? "" : file.Tag.Title.Trim();


      string sql =
          String.Format(
            @"insert into tracks (strPath, strArtist, strAlbumArtist, strAlbum, strGenre, strTitle, iTrack, iNumTracks, iYear, iRating, iDisc, iNumDisc, strLyrics) 
                          values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, {8}, {9}, {10}, {11}, '{12}' )",
            Util.RemoveInvalidChars(file.Name), Util.RemoveInvalidChars(artist), Util.RemoveInvalidChars(albumartist), Util.RemoveInvalidChars(file.Tag.Album), Util.RemoveInvalidChars(genre), Util.RemoveInvalidChars(title),
            file.Tag.Track, file.Tag.TrackCount, file.Tag.Year, 0,
            file.Tag.Disc, file.Tag.DiscCount, "");

      ExecuteDirectSQL(sql);

      AddArtist(Util.RemoveInvalidChars(artist));
      AddAlbumArtist(Util.RemoveInvalidChars(albumartist));
      AddGenre(Util.RemoveInvalidChars(genre));
    }

    /// <summary>
    /// Add the artist to the Artist table, to allow us having mutiple artists per song
    /// </summary>
    /// <param name="strArtist"></param>
    /// <returns></returns>
    private void AddArtist(string strArtist)
    {
      string strSQL;

      // split up the artist, in case we've got multiple artists
      string[] artists = strArtist.Split(new char[] { ';', '|' });
      foreach (string artist in artists)
      {
        if (artist.Trim() == string.Empty)
        {
          continue;
        }

        // ATTENTION: We need to use the 'like' operator instead of '=' to have case insensitive searching
        strSQL = String.Format("select idArtist from artist where strArtist like '{0}'", artist.Trim());
        if (!ExistsItem(strSQL))
        {
          // Insert the Artist
          strSQL = String.Format("insert into artist (strArtist) values ('{0}')", artist.Trim());
          ExecuteDirectSQL(strSQL);
        }
      }
    }

    /// <summary>
    /// Add the albumartist to the AlbumArtist table, to allow us having mutiple artists per song
    /// </summary>
    /// <param name="strArtist"></param>
    /// <returns></returns>
    private void AddAlbumArtist(string strAlbumArtist)
    {
      string strSQL;

      // split up the albumartist, in case we've got multiple albumartists
      string[] artists = strAlbumArtist.Split(new char[] { ';', '|' });
      foreach (string artist in artists)
      {
        if (artist.Trim() == string.Empty)
        {
          continue;
        }

        // ATTENTION: We need to use the 'like' operator instead of '=' to have case insensitive searching
        strSQL = String.Format("select idAlbumArtist from albumartist where strAlbumArtist like '{0}'", artist.Trim());
        if (!ExistsItem(strSQL))
        {
          // Insert the AlbumArtist
          strSQL = String.Format("insert into albumartist (strAlbumArtist) values ('{0}')", artist.Trim());
          ExecuteDirectSQL(strSQL);
        }
      }
    }

    /// <summary>
    /// Add the genre to the Genre Table, to allow maultiple Genres per song
    /// </summary>
    /// <param name="strGenre"></param>
    private void AddGenre(string strGenre)
    {
      string strSQL;

      // split up the artist, in case we've got multiple artists
      string[] genres = strGenre.Split(new char[] { ';', '|' });
      foreach (string genre in genres)
      {
        if (genre.Trim() == string.Empty)
        {
          continue;
        }

        // ATTENTION: We need to use the 'like' operator instead of '=' to have case insensitive searching
        strSQL = String.Format("select idGenre from genre where strGenre like '{0}'", genre.Trim());
        if (!ExistsItem(strSQL))
        {
          // Insert the Genre
          strSQL = String.Format("insert into genre (strGenre) values ('{0}')", genre.Trim());
          ExecuteDirectSQL(strSQL);
        }
      }
    }

    /// <summary>
    /// Multiple Entry fields need to be formatted to contain a | at the end to be able to search correct
    /// </summary>
    /// <param name="str"></param>
    /// <param name="strip"></param>
    /// <returns></returns>
    private string FormatMultipleEntry(string str)
    {
      string[] strSplit = str.Split(new char[] { ';', '|' });
      // Can't use a simple String.Join as i need to trim all the elements 
      string strJoin = "| ";
      foreach (string strTmp in strSplit)
      {
        strJoin += String.Format("{0} | ", strTmp.Trim());
      }
      return strJoin;
    }
    #endregion

    #region Database Scan Status
    public string DatabaseScanStatus()
    {
      if (scanThread == null)
      {
        return localisation.ToString("Settings", "DBScanIdle");
      }

      if (scanThread.ThreadState != ThreadState.Running)
      {
        if (_scanHasRun)
        {
          return string.Format(localisation.ToString("Settings", "DBScanFinished"), _audioFiles, _ts.Hours, _ts.Minutes, _ts.Seconds, _trackPerSecSummary);
        }
        else
          return localisation.ToString("Settings", "DBScanIdle");
      }

      string returnString = string.Format(localisation.ToString("Settings", "DBScanProgress"), _processCount, files.Count);
      return returnString;
    }
    #endregion
    #endregion

    #region Private Methods
    private void OpenConnection()
    {
      try
      {
        string connection = string.Format(@"Data Source={0}", _databaseName);
        conn = new SQLiteConnection(connection);
        conn.Open();
      }
      catch (Exception ex)
      {
        log.Error("Database Connection Open: Error: {0}", ex.Message);
      }
    }

    private void CloseConnection()
    {
      try
      {
        conn.Close();
      }
      catch (Exception ex)
      {
        log.Error("Database Connection Close: Error: {0}", ex.Message);
      }
    }

    private bool ExecuteDirectSQL(string sql)
    {

      try
      {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = sql;
          int result = cmd.ExecuteNonQuery();
        }
      }
      catch (Exception ex)
      {
        log.Error("Database Execute Direct: Error executing sql: {0}", ex.Message);
        return false;
      }
      return true;
    }

    private bool ExistsItem(string sql)
    {

      try
      {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = sql;
          object result = cmd.ExecuteScalar();
          if (result == null)
            return false;
          else
            return true;
        }
      }
      catch (Exception ex)
      {
        log.Error("Database Execute Scalar: Error executing sql: {0}", ex.Message);
        return false;
      }
    }
    #endregion
  }
}
