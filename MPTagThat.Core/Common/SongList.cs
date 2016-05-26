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
using System.Linq;
using System.Linq.Expressions;
using Raven.Client.Embedded;
using Raven.Client;
using Raven.Client.Document;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MPTagThat.Core.Common;
using Raven.Abstractions.Data;
using Raven.Client.Connection;

#endregion

namespace MPTagThat.Core
{
  /// <summary>
  /// This class is used to store a list of Songs, respresented as <see cref="MPTagThat.Core.TrackData" />.
  /// Based on the amount of songs reatrieved, it either stores them in a <see cref="SortableBindingList{T}"/> or uses
  /// a temporary db4o database created on the fly to prevent Out of Memory issues, when processing a large collection of songs.
  /// </summary>
  public class SongList : IEnumerable<TrackData>, IDisposable
  {
    #region Variables

	  private readonly string _databaseName = "SongsTemp";
    private string _databaseFolder;

    private SortableBindingList<TrackData> _bindingList = new SortableBindingList<TrackData>();
    private bool _databaseModeEnabled = false;

		private IDocumentStore _store;
    private IDocumentSession _session;
		private List<Guid> _dbIdList = new List<Guid>(); 

    private int _lastRetrievedTrackIndex = -1;
    private TrackData _lastRetrievedTrack = null;
    private int _countCache = 0;

    #endregion

    #region ctor / dtor

    public SongList()
    {
      _databaseModeEnabled = false;
      _databaseFolder = $@"{System.Windows.Forms.Application.StartupPath}\Database\Databases\{_databaseName}";
    }

    #endregion

    #region Properties

    /// <summary>
    /// Return the count of songs in the list
    /// </summary>
    /// <returns></returns>
    public int Count
    {
      get
      {
        if (_databaseModeEnabled)
        {
          if (_countCache == 0)
          {
	          _countCache = _session.Query<TrackData>().Count();
          }
          return _countCache;
        }

        return _bindingList.Count;
      }
    }

    #endregion

    #region Indexer

    /// <summary>
    /// Implementation of indexer
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public TrackData this[int i]
    {
      get
      {
        if (_databaseModeEnabled)
        {
          if (i == _lastRetrievedTrackIndex)
          {
            return _lastRetrievedTrack;
          }

          _lastRetrievedTrackIndex = i;

	        var result = _session.Load<TrackData>($"TrackDatas/{_dbIdList[i]}");

          _lastRetrievedTrack = result;
          return _lastRetrievedTrack;
        }
        
        return _bindingList[i];
      }
      set
      {
        if (_databaseModeEnabled)
        {
					var result = _session.Load<TrackData>($"TrackDatas/{_dbIdList[i]}");

					var track = result;
          track = value;
          _session.Store(track);
					_session.SaveChanges();
        }
        else
        {
          _bindingList[i] = value;
        }
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adding of new songs to the list
    /// </summary>
    /// <param name="track"></param>
    public void Add(TrackData track)
    {
      if (!_databaseModeEnabled && _bindingList.Count > Options.MaximumNumberOfSongsInList )
      {
        CopyLIstToDatabase();  
      }

      if (_databaseModeEnabled)
      {
        _session.Store(track);
				_session.SaveChanges();
        _dbIdList.Add(track.Id);
      }
      else
      {
        _bindingList.Add(track); 
      }
    }

    /// <summary>
    /// Removes the object at the specified index
    /// </summary>
    /// <param name="index"></param>
    public void RemoveAt(int index)
    {
      if (_databaseModeEnabled)
      {
				var track = _session.Load<TrackData>($"TrackDatas/{_dbIdList[index]}");
				_session.Delete(track);
				_session.SaveChanges();
	      _dbIdList.RemoveAt(index);
      }
      else
      {
        _bindingList.RemoveAt(index);
      }
    }

    /// <summary>
    /// Clear the list
    /// </summary>
    public void Clear()
    {
      if (_databaseModeEnabled)
      {
        _databaseModeEnabled = false;
				_store.DatabaseCommands.DeleteByIndex("Auto/TrackDatas", new IndexQuery());	
				_dbIdList.Clear();
			}
      else
      {
        _bindingList.Clear();
      }
    }

    /// <summary>
    /// Apply Sorting
    /// </summary>
    /// <param name="property"></param>
    /// <param name="direction"></param>
    public void Sort(PropertyDescriptor property, ListSortDirection direction)
    {
      if (_databaseModeEnabled)
      {
        // Build the Linq Expression tree for sorting
        string sortFieldName = property.Name;
        string sortMethod = "OrderBy";
        if (direction.ToString() == "Descending")
        {
          sortMethod = "OrderbyDescending";
        }

        var queryableData = _session.Query<TrackData>();
        var type = typeof(TrackData);
        var prop = type.GetProperty(sortFieldName);
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var queryExpr = Expression.Call(typeof(Queryable), sortMethod,
                                                new[] { type, property.PropertyType },
                                                queryableData.Expression, Expression.Quote(orderByExpression));


        var result = queryableData.Provider.CreateQuery<TrackData>(queryExpr);


        int i = 0;
        _dbIdList.Clear();
        foreach (TrackData dataObject in result)
        {
          _dbIdList.Add(dataObject.Id);
          i++;
        }

      }
      else
      {
        _bindingList.ApplySortCore(property, direction);
      }
    }

    #endregion

    #region Private Methods

    private bool CreateDbConnection()
    {
	    if (_store != null)
	    {
		    return true;
	    }

      try
      {
	      try
	      {
					System.IO.Directory.Delete($@"{System.Windows.Forms.Application.StartupPath}\Database\{_databaseName}", true);
	      }
	      catch (IOException)
	      {
	      }

	      _store = RavenDocumentStore.GetDocumentStoreFor(_databaseName);
	      _session = _store.OpenSession();

				return true;
      }
      catch (Exception ex)
      {
        ServiceScope.Get<ILogger>().GetLogger.Error("Error creating DB Connection. Database Mode disabled. {0}", ex.Message);
      }

      return false;
    }


    /// <summary>
    /// The number of allowed objects in the BindingList has been exceeded
    /// Copy all the data to the database
    /// </summary>
    private void CopyLIstToDatabase()
    {
      ServiceScope.Get<ILogger>().GetLogger.Debug("Number of Songs in list exceeded the limit. Database mode enabled");
      
      if (!CreateDbConnection())
      {
        return;
      }

      _dbIdList.Clear();

	    using (BulkInsertOperation bulkInsert = _store.BulkInsert())
	    {
		    foreach (TrackData track in _bindingList)
		    {
			    bulkInsert.Store(track);
			    _dbIdList.Add(track.Id);
		    }
	    }

	    _bindingList.Clear();
      _databaseModeEnabled = true;
      ServiceScope.Get<ILogger>().GetLogger.Debug("Finished enabling database mode.");
    }

    #endregion

    #region Interfaces

    /// <summary>
    /// Provide enumeration
    /// </summary>
    /// <returns></returns>
    public IEnumerator<TrackData> GetEnumerator()
    {
      if (_databaseModeEnabled)
      {
	      return _session.Query<TrackData>().Take(int.MaxValue).GetEnumerator();
      }

      return _bindingList.GetEnumerator();
    }

    /// <summary>
    /// Provide enumeration
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          if (_store != null && !_store.WasDisposed)
          {
            _session?.Dispose();
            _store.Dispose();
            Util.DeleteFolder(_databaseName);
          }
        }
        disposedValue = true;
      }
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
    }
    #endregion

    #endregion
  }
}
