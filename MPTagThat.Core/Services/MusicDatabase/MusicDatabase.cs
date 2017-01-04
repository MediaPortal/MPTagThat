#region Copyright (C) 2009-2016 Team MediaPortal
// Copyright (C) 2009-2016 Team MediaPortal
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.Common;
using MPTagThat.Core.Services.MusicDatabase.Indexes;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Json.Linq;

#endregion 

namespace MPTagThat.Core.Services.MusicDatabase
{
  /// <summary>
  /// This class handles all related RavenDB actions
  /// </summary>
  public class MusicDatabase : IMusicDatabase
  {
    #region Variables

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly string _databaseName = "MusicDatabase";
    private readonly string _workDatabaseName = "MusicWork";
    private IDocumentStore _store;
    private IDocumentSession _session;

    private BackgroundWorker _bgwScanShare;
    private int _audioFiles;
    private DateTime _scanStartTime;

    private readonly ConcurrentDictionary<string, Lazy<IDocumentStore>> _stores =
        new ConcurrentDictionary<string, Lazy<IDocumentStore>>();

    #endregion

    #region ctor / dtor

    public MusicDatabase()
    {
      CurrentDatabase = _databaseName;
    }

    ~MusicDatabase()
    {
      if (_store != null && !_store.WasDisposed)
      {
        _session?.Dispose();
      }
    }

    #endregion

    #region Properties

    public bool ScanActive
    {
      get
      {
        if (_bgwScanShare == null)
          return false;

        if (_bgwScanShare.IsBusy)
          return true;

        return false;
      }
    }

    public string CurrentDatabase { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Return a document store
    /// </summary>
    /// <param name="databaseName"></param>
    /// <returns></returns>
    public IDocumentStore GetDocumentStoreFor(string databaseName)
    {
      return _stores.GetOrAdd(databaseName, CreateDocumentStore).Value;
    }

    /// <summary>
    /// Remove a store
    /// </summary>
    /// <param name="databasename"></param>
    public void RemoveStore(string databasename)
    {
      Lazy<IDocumentStore> store = null;
      _stores.TryRemove(databasename, out store);
    }

    /// <summary>
    /// Builds the database using the Music Share
    /// </summary>
    /// <param name="musicShare"></param>
    /// <param name="deleteDatabase"></param>
    public void BuildDatabase(string musicShare, bool deleteDatabase)
    {
      if (deleteDatabase)
      {
        DeleteDatabase();
      }

      if (_store == null && !CreateDbConnection())
      {
        log.Error("Database Scan aborted.");
        return;
      }

      _bgwScanShare = new BackgroundWorker
      {
        WorkerSupportsCancellation = true,
        WorkerReportsProgress = true
      };
      _bgwScanShare.DoWork += ScanShare_DoWork;
      _bgwScanShare.RunWorkerCompleted += ScanShare_Completed;
      _bgwScanShare.RunWorkerAsync(musicShare);
    }

    /// <summary>
    /// Aborts the scanning of the database
    /// </summary>
    public void AbortDatabaseScan()
    {
      if (_bgwScanShare != null && _bgwScanShare.IsBusy)
      {
        _bgwScanShare.CancelAsync();
      }
    }

    /// <summary>
    /// Deletes the Music Database
    /// </summary>
    public void DeleteDatabase()
    {
      _session?.Advanced.Clear();
      _session = null;
      _store.Dispose();
      _store = null;
      RemoveStore(CurrentDatabase);
      var dbPath = $"{Options.StartupSettings.DatabaseFolder}{CurrentDatabase}";
      Util.DeleteFolder(dbPath);
      CreateDbConnection();
    }

    /// <summary>
    /// Switch between Work and Productive database
    /// </summary>
    public void SwitchDatabase()
    {
      _session?.Advanced.Clear();
      _session = null;
      _store.Dispose();
      _store = null;
      RemoveStore(CurrentDatabase);
      CurrentDatabase = CurrentDatabase == _databaseName ? _workDatabaseName : _databaseName;
      CreateDbConnection();
      log.Info($"Database has been switch to {CurrentDatabase}");
    }

    /// <summary>
    /// Runs the query against the MusicDatabase
    /// </summary>
    /// <param name="query"></param>
    public List<TrackData> ExecuteQuery(string query)
    {
      return ExecuteQuery(query, "");
    }

    /// <summary>
    /// Runs the query against the MusicDatabase
    /// </summary>
    /// <param name="query"></param>
    /// <param name="orderBy"></param>
    public List<TrackData> ExecuteQuery(string query, string orderBy)
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var order = orderBy.Split(',');
      List<TrackData> result = null;

      if (query.Contains(":"))
      {
        // Set Artist as default order, if nothing is specified
        if (order[0] == "")
        {
          Array.Resize(ref order, 3);
          order[0] = "Artist";
          order[1] = "Album";
          order[2] = "Track";
        }

        result = _session.Advanced.DocumentQuery<TrackData>()
          .Where(query)
          .OrderBy(order)
          .Take(int.MaxValue)
          .ToList();
      }
      else
      {
        if (order[0] == "")
        {
          order[0] = "Query";
        }

        var searchText = new List<object>();
        searchText.AddRange(query.Split(new char[] { ' ' }));
        result = _session.Advanced.DocumentQuery<TrackData, DefaultSearchIndex>()
          .ContainsAll("Query", searchText)
          .OrderBy(order)
          .Take(int.MaxValue)
          .ToList();
      }
      return result;
    }

    /// <summary>
    /// Update a track in the database
    /// </summary>
    /// <param name="track"></param>
    /// <param name="originalFileName"></param>
    public void UpdateTrack(TrackData track, string originalFileName)
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return;
      }

      try
      {
        originalFileName = Util.EscapeDatabaseQuery(originalFileName);
        // Lookup the track in the database
        var dbTracks = _session.Advanced.DocumentQuery<TrackData, DefaultSearchIndex>().WhereEquals("Query", originalFileName).ToList();
        if (dbTracks.Count > 0)
        {
          var id = dbTracks[0].Id;
          var dbTrackData = _session.Load<TrackData>($"TrackDatas/{id}");
          track.Id = id;
          // Reset status
          track.Status = -1;
          track.Changed = false;
          track = StoreCoverArt(track);
          dbTrackData = track;
          _session.SaveChanges();
        }
      }
      catch (Exception ex)
      {
        log.Error("UpdateTrack: Error updating track in database {0}: {1}", ex.Message, ex.InnerException);
      }
    }
    
    /// <summary>
    /// Get Distinct Artists and Albumartists
    /// </summary>
    /// <returns></returns>
    public List<DistinctCombinedArtistIndex.Projection> DistinctArtists()
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var artists = _session.Query<DistinctCombinedArtistIndex.Result, DistinctCombinedArtistIndex>()
        .Take(int.MaxValue)
        .ProjectFromIndexFieldsInto<DistinctCombinedArtistIndex.Projection>()
        .OrderBy(x => x.name)
        .ToList();

      return artists;
    }

    public List<DistinctResult> GetArtists()
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var artists = _session.Query<DistinctResult, DistinctArtistIndex>()
        .Take(int.MaxValue)
        .OrderBy(x => x.Name)
        .ToList();

      return artists;
    }

    public List<DistinctResult> GetArtistAlbums(string query)
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var artistalbums = _session.Query<DistinctResult, DistinctArtistAlbumIndex>()
        .Where(x => x.Name == Util.EscapeDatabaseQuery(query))
        .Take(int.MaxValue)
        .OrderBy(x => x.Name)
        .ThenBy(x => x.Album)
        .ToList();

      return artistalbums;
    }

    public List<DistinctResult> GetAlbumArtists()
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var albumartists = _session.Query<DistinctResult, DistinctAlbumArtistIndex>()
        .Take(int.MaxValue)
        .OrderBy(x => x.Name)
        .ToList();

      return albumartists;
    }

    public List<DistinctResult> GetAlbumArtistAlbums(string query)
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var artistalbums = _session.Query<DistinctResult, DistinctAlbumArtistAlbumIndex>()
        .Where(x => x.Name == Util.EscapeDatabaseQuery(query))
        .Take(int.MaxValue)
        .OrderBy(x => x.Name)
        .ThenBy(x => x.Album)
        .ToList();

      return artistalbums;
    }

    public List<DistinctResult> GetGenres()
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var genres = _session.Query<DistinctResult, DistinctGenreIndex>()
        .Take(int.MaxValue)
        .OrderBy(x => x.Genre)
        .ToList();

      return genres;
    }

    public List<DistinctResult> GetGenreArtists(string query)
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var genreartists = _session.Query<DistinctResult, DistinctGenreArtistIndex>()
        .Where(x => x.Genre == Util.EscapeDatabaseQuery(query))
        .Take(int.MaxValue)
        .OrderBy(x => x.Genre)
        .ThenBy(x => x.Name)
        .ToList();

      return genreartists;
    }

    public List<DistinctResult> GetGenreArtistAlbums(string genre, string artist)
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return null;
      }

      var genreartists = _session.Query<DistinctResult, DistinctGenreArtistAlbumIndex>()
        .Where(x => x.Genre == Util.EscapeDatabaseQuery(genre) && x.Name == Util.EscapeDatabaseQuery(artist))
        .Take(int.MaxValue)
        .OrderBy(x => x.Genre)
        .ThenBy(x => x.Name)
        .ThenBy(x => x.Album)
        .ToList();

      return genreartists;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Creates a Database Connection and establishes a session
    /// </summary>
    /// <returns></returns>
    private bool CreateDbConnection()
    {
      if (_store != null)
      {
        return true;
      }

      try
      {
        _store = GetDocumentStoreFor(CurrentDatabase);
        _session = _store.OpenSession();

        IndexCreation.CreateIndexes(typeof(DistinctCombinedArtistIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DefaultSearchIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DistinctArtistIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DistinctArtistAlbumIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DistinctAlbumArtistIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DistinctAlbumArtistAlbumIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DistinctGenreIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DistinctGenreArtistIndex).Assembly, _store);
        IndexCreation.CreateIndexes(typeof(DistinctGenreArtistAlbumIndex).Assembly, _store);

        return true;
      }
      catch (Exception ex)
      {
        log.Error("Error creating DB Connection. {0}", ex.Message);
      }

      return false;
    }

    private void ScanShare_Completed(object sender, RunWorkerCompletedEventArgs e)
    {
      BackgroundWorker bgw = (BackgroundWorker)sender;
      if (e.Cancelled)
      {
        log.Info("Database Scan cancelled");
      }
      else if (e.Error != null)
      {
        log.Error("Database Scan failed with {0}", e.Error.Message);
      }
      else
      {
        TimeSpan ts = DateTime.Now - _scanStartTime;
        Util.SendProgress(string.Format(ServiceScope.Get<ILocalisation>().ToString("Database", "DBScanFinished"), _audioFiles, ts.Hours, ts.Minutes, ts.Seconds));
        log.Info("Database Scan finished");
      }
      bgw.Dispose();
    }

    private void ScanShare_DoWork(object sender, DoWorkEventArgs e)
    {
      _scanStartTime = DateTime.Now;
      var di = new DirectoryInfo((string)e.Argument);
      try
      {
        BulkInsertOptions bulkInsertOptions = new BulkInsertOptions
        {
          BatchSize = 1000,
          OverwriteExisting = true
        };

       
        using (BulkInsertOperation bulkInsert = _store.BulkInsert(null, bulkInsertOptions))
        {
          foreach (FileInfo fi in GetFiles(di, true))
          {
            try
            {
              if (!Util.IsAudio(fi.FullName))
              {
                continue;
              }
              Util.SendProgress($"Reading file {fi.FullName}");
              var track = Track.Create(fi.FullName);
              if (track != null)
              {
                track = StoreCoverArt(track);
                bulkInsert.Store(track);
                _audioFiles++;
                if (_audioFiles%1000 == 0)
                {
                  log.Info($"Number of processed files: {_audioFiles}");
                }
              }
            }
            catch (PathTooLongException)
            {
              continue;
            }
            catch (System.UnauthorizedAccessException)
            {
              continue;
            }
            catch (Exception ex)
            {
              log.Error("Error during Database BulkInsert {0}", ex.Message);
            }
          }
        }
      }
      catch (System.InvalidOperationException ex)
      {
        log.Error("Error during Database BulkInsert {0}", ex.Message);
      }
    }

    private TrackData StoreCoverArt(TrackData track)
    {
      HashAlgorithm sha = new SHA1CryptoServiceProvider();
      // Check for pictures in the track and add it to the hashlist
      // For database objects, the pictures are hashed and stored in the coverart folder
      foreach (Picture picture in track.Pictures)
      {
        string hash = BitConverter.ToString(sha.ComputeHash(picture.Data)).Replace("-", string.Empty);
        track.PictureHashList.Add(hash);
        string fullFileName = $"{Options.StartupSettings.CoverArtFolder}{hash}.png";
        if (!File.Exists(fullFileName))
        {
          try
          {
            Image img = Picture.ImageFromData(picture.Data);
            // Need to make a copy, otherwise we have a GDI+ Error
            Bitmap bCopy = new Bitmap(img);
            bCopy.Save(fullFileName, ImageFormat.Png);
          }
          catch (Exception)
          {
            // ignored
          }
        }
      }
      // finally remove the puctures from the database object
      track.Pictures.Clear();
      return track;
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

            FileInfo[] newFiles = dir.GetFiles("*.*");
            foreach (FileInfo file in newFiles)
            {
              files.Enqueue(file);
            }
          }
        }
        catch (UnauthorizedAccessException ex)
        {
          Console.WriteLine(ex.Message, ex);
        }
      }
    }

    /// <summary>
    /// Creates a Raven Document Store
    /// </summary>
    /// <param name="databaseName"></param>
    /// <returns></returns>
    private Lazy<IDocumentStore> CreateDocumentStore(string databaseName)
    {
      return new Lazy<IDocumentStore>(() =>
      {       
        var docStore = new EmbeddableDocumentStore()
        {
          UseEmbeddedHttpServer = databaseName == "MusicDatabase" && Options.StartupSettings.RavenStudio,
          DataDirectory = $"{Options.StartupSettings.DatabaseFolder}{databaseName}",
          RunInMemory = false,
          Configuration =
          {
            Port = Options.StartupSettings.RavenStudioPort,
            MaxPageSize = 300000,
          },
        };
        if (Options.StartupSettings.RavenStudio)
        {
          Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(Options.StartupSettings.RavenStudioPort);
        }
        docStore.Initialize();

        docStore.Conventions.MaxNumberOfRequestsPerSession = 1000000;
        docStore.Conventions.AllowMultipuleAsyncOperations = true;

        return docStore;
      });
    }
    #endregion

  }
}
