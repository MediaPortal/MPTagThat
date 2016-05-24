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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    private string _databaseFolder;
    private IDocumentStore _store;
    private IDocumentSession _session;

    private BackgroundWorker _bgwScanShare;

    #endregion

    #region ctor / dtor

    public MusicDatabase(Main main)
    {
      _main = main;
      _databaseFolder = $"~\\Databases\\{_databaseName}";
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
          if (Directory.Exists(_databaseFolder))
          {
            Directory.Delete(_databaseFolder);
          }
        }
        catch (Exception ex)
        {
          log.Error("Exception deleting database {0}", ex.Message);
        }
        
        _store?.Initialize();
        _session = _store?.OpenSession();
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
      _session?.Dispose();
      _store?.Dispose();
      if (Directory.Exists(_databaseFolder))
      {
        Directory.Delete(_databaseFolder);
      }
      CreateDbConnection();
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
        _session = _store.OpenSession();

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
          BatchSize = 1000,
          OverwriteExisting = true
        };

        // Create folder to store Coverart
        var picFolder = $@"{Application.StartupPath}\Database\Coverart\";
        if (!Directory.Exists(picFolder))
        {
          Directory.CreateDirectory(picFolder);
        }

        HashAlgorithm sha = new SHA1CryptoServiceProvider();
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
                foreach (Picture picture in track.Pictures)
                {
                  string fileName = BitConverter.ToString(sha.ComputeHash(picture.Data)).Replace("-", string.Empty);
                  string fullFileName = $"{picFolder}{fileName}.png";
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
                    }
                  }
                }
                track.Pictures.Clear();
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
