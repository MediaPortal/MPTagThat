using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTagThat.Core.Services.MusicDatabase
{
  public interface IMusicDatabase
  {
    /// <summary>
    /// Returns, if a Database Scan is Active
    /// </summary>
    bool ScanActive { get; }

    /// <summary>
    /// Start a Database Build out of a given Share
    /// </summary>
    /// <param name="musicShare"></param>
    /// <param name="deleteDatabase"></param>
    void BuildDatabase(string musicShare, bool deleteDatabase);

    /// <summary>
    /// Aborts the scanning of the database
    /// </summary>
    void AbortDatabaseScan();
    
    /// <summary>
    /// Deletes the Music Database
    /// </summary>
    void DeleteDatabase();

    /// <summary>
    /// Runs the query against the MusicDatabase
    /// </summary>
    /// <param name="query"></param>
    List<TrackData> ExecuteQuery(string query);
  }
}
