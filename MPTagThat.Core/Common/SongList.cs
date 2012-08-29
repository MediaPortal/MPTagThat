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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace MPTagThat.Core
{
  /// <summary>
  /// This class is used to store a list of Songs, respresented as <see cref="MPTagThat.Core.TrackData" />.
  /// Based on the amount of songs reatrieved, it either stores them in a <see cref="SortableBindingList{T}"/> or uses
  /// a temporary db4o database created on the fly to prevent Out of Memory issues, when processing a large collection of songs.
  /// </summary>
  public class SongList : IEnumerable<TrackData>
  {
    #region Variables

    private readonly string _databaseName = string.Format(@"{0}\MPTagThat\Songs.db4o",
                                                         Environment.GetFolderPath(
                                                           Environment.SpecialFolder.LocalApplicationData));

    private SortableBindingList<TrackData> _bindingList = new SortableBindingList<TrackData>();
    private bool _databaseModeEnabled = false;

    private IObjectContainer _db = null;
    private IEmbeddedConfiguration _dbConfig = null;
    private Dictionary<int, Guid> _dbIdList = new Dictionary<int, Guid>(); 

    private int _lastRetrievedTrackIndex = -1;
    private TrackData _lastRetrievedTrack = null;
    private int _countCache = 0;

    #endregion

    #region ctor / dtor

    public SongList()
    {
      _databaseModeEnabled = false;
    }

    ~SongList()
    {
      if (_db != null)
      {
        _db.Close();
        _db.Dispose();
      }
      System.IO.File.Delete(_databaseName);
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
            _countCache = _db.Query(typeof(TrackData)).Count;
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

          ServiceScope.Get<ILogger>().GetLogger.Trace("Getting Track from database: {0}", i);

          var result = from TrackData d in _db
                       where d.Id == _dbIdList[i]
                       select d;

          foreach (var trackData in result)
          {
            _lastRetrievedTrack = trackData;
            ServiceScope.Get<ILogger>().GetLogger.Trace("Finished Getting Track from database");
            return trackData;
          }
        }
        
        return _bindingList[i];
      }
      set
      {
        _bindingList[i] = value;
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
        _db.Store(track);
        _dbIdList.Add(_dbIdList.Count, track.Id);
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
        var result = from TrackData d in _db
                     where d.Id == _dbIdList[index]
                     select d;
        foreach (var trackData in result)
        {
          _db.Delete(trackData);
        }
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
        var result = from TrackData d in _db
                select d;
        foreach (var trackData in result)
        {
          _db.Delete(trackData);
        }

        _databaseModeEnabled = false;
        _db.Close();
        _db.Dispose();
        System.IO.File.Delete(_databaseName);
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

        var queryableData = _db.AsQueryable<TrackData>();
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
          _dbIdList.Add(i, dataObject.Id);
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
      try
      {
        if (_db != null)
        {
          _db.Close();
          _db.Dispose();
          System.IO.File.Delete(_databaseName);
        }

        _dbConfig = Db4oEmbedded.NewConfiguration();
        _dbConfig.Common.ObjectClass(typeof(TrackData)).ObjectField("_id").Indexed(true);
        _dbConfig.Common.ActivationDepth = 1; // To increase performance
        _dbConfig.Common.ObjectClass(typeof(TrackData)).CascadeOnUpdate(true);

        IStorage fileStorage = new FileStorage();
        IStorage cachingStorage = new CachingStorage(fileStorage, 128, 2048);
        _dbConfig.File.Storage = cachingStorage;

        _db = Db4oEmbedded.OpenFile(_dbConfig, _databaseName);

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
      ServiceScope.Get<ILogger>().GetLogger.Debug("Number of Songs in list exceeded the limnit. Database mode enabled");
      
      if (!CreateDbConnection())
      {
        return;
      }

      _dbIdList.Clear();

      foreach (TrackData track in _bindingList)
      {
        _db.Store(track);
        _dbIdList.Add(_dbIdList.Count, track.Id);
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
        return (from TrackData d in _db
                select d).GetEnumerator();

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

    #endregion
  }
}
