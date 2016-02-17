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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.Common;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Document;

#endregion 

namespace MPTagThat
{
  /// <summary>
  /// This class handles all related RavenDB actions
  /// </summary>
  public class MusicDatabase
  {
    #region Variables

    private Main _main;
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private readonly string _databaseName = "MusicDatabase";

    private IDocumentStore _store;
    private IDatabaseCommands _dbCommands;
    private IDocumentSession _session;

    private BackgroundWorker _bgwScanShare;

    #endregion

    #region ctor / dtor

    public MusicDatabase(Main main)
    {
      _main = main;
    }

    ~MusicDatabase()
    {
      if (_store != null && !_store.WasDisposed)
      {
        _session?.Dispose();
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Builds the database using the Music Share
    /// </summary>
    /// <param name="musicShare"></param>
    /// <param name="deleteDatabase"></param>
    public void BuildDatabase(string musicShare, bool deleteDatabase)
    {
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Database Scan aborted.");
        return;
      }

      if (deleteDatabase)
      {
        _session?.Dispose();
        try
        {
          _dbCommands.GlobalAdmin.DeleteDatabase(_databaseName, true);
        }
        catch (Exception ex)
        {
          if (!ex.Message.Contains("NotFound"))
          {
            log.Error("Exception deleting database {0}", ex.Message);
          }
        }
        
        _dbCommands.GlobalAdmin.EnsureDatabaseExists(_databaseName);
        _store?.Initialize();
        _session = _store?.OpenSession(_databaseName);
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
      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not delete Database.");
        return;
      }

      _session?.Dispose();
      _dbCommands.GlobalAdmin.DeleteDatabase(_databaseName, true);
      _dbCommands.GlobalAdmin.EnsureDatabaseExists(_databaseName);
      _store?.Initialize();
      _session = _store?.OpenSession(_databaseName);
    }

    /// <summary>
    /// Runs the query against the MusicDatabase and adds the songs to the grid
    /// </summary>
    /// <param name="query"></param>
    public void ExecuteQuery(string query)
    {
      _main.TracksGridView.View.Rows.Clear();
      Options.Songlist.Clear();
      _main.MiscInfoPanel.ClearNonMusicFiles();
      GC.Collect();


      if (_store == null && !CreateDbConnection())
      {
        log.Error("Could not establish a session.");
        return;
      }

      var result = _session.Advanced.DocumentQuery<TrackData>()
        .Where(query)
        .Take(int.MaxValue)
        .ToList();

      foreach (var track in result)
      {
        _main.TracksGridView.AddTrack(track);
        _main.TracksGridView.View.Rows.Add(); // Add a row to the grid. Virtualmode will handle the filling of cells
      }
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
        _store = RavenDocumentStore.GetDocumentStoreFor(_databaseName);
        _dbCommands = _store.DatabaseCommands.ForDatabase(_databaseName);
        _session = _store.OpenSession(_databaseName);

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
        Util.SendProgress("Database Scan finished");
        log.Info("Database Scan finished");
      }
      bgw.Dispose();
    }

    private void ScanShare_DoWork(object sender, DoWorkEventArgs e)
    {
      var di = new DirectoryInfo((string)e.Argument);
      try
      {
        BulkInsertOptions bulkInsertOptions = new BulkInsertOptions
        {
          BatchSize = 200,
          OverwriteExisting = true
        };

        using (BulkInsertOperation bulkInsert = _store.BulkInsert(_databaseName, bulkInsertOptions))
        {
          foreach (FileInfo fi in GetFiles(di, true))
          {
            try
            {
              if (!Util.IsAudio(fi.FullName))
              {
                continue;
              }
              Util.SendProgress(string.Format("Reading file {0}", fi.FullName));
              var track = Track.Create(fi.FullName);
              if (track != null)
              {
                bulkInsert.Store(track);
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


    #endregion


  }
}
