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

using System;
using System.Collections.Generic;
using System.Threading;
using MPTagThat.Core.AlbumInfo.AlbumSites;
using Timer = System.Timers.Timer;

namespace MPTagThat.Core.AlbumInfo
{
  public class AlbumSearch : IDisposable
  {
    #region Variables

    private const int TimeLimit = 30 * 1000;
    private const int TimeLimitForSite = 15 * 1000;

    public List<string> AlbumSites = new List<string>();

    // Uses to inform the specified site searches to stop searching and exit
    private readonly ManualResetEvent _mEventStopSiteSearches;

    private readonly string _artist = "";
    private readonly string _albumTitle = "";
    private readonly IAlbumInfo _controller;
    
    private readonly Timer _timer;

    private bool _albumFound;
    private bool _searchHasEnded;
    private int _mSitesSearched;

    #endregion

    #region Properties

    public bool SearchHasEnded
    {
        get { return _searchHasEnded; }
        set { _searchHasEnded = value; }
    }

    #endregion

    #region Methods

    public AlbumSearch(IAlbumInfo controller, string artist, string album)
    {
      _artist = artist;
      _albumTitle = album;
      _controller = controller;

      _mEventStopSiteSearches = new ManualResetEvent(false);

      _timer = new Timer();
      _timer.Enabled = true;
      _timer.Interval = TimeLimit;
      _timer.Elapsed += StopDueToTimeLimit;
      _timer.Start();
    }

    public void Dispose()
    {
      _searchHasEnded = true;
      _mEventStopSiteSearches.Set();
      _timer.Enabled = false;
      _timer.Stop();
      _timer.Close();
      _timer.Dispose();
      _controller.SearchFinished = new object[] {};
    }

    public void Run()
    {
      foreach (var albumInfoSite in AlbumSites)
      {
        RunSearchForSiteInThread(albumInfoSite);
      }
    }

    private void RunSearchForSiteInThread(string albumInfoSite)
    {
      ThreadStart job = delegate
      {
        var albumSearchSite = AlbumSiteFactory.Create(albumInfoSite, _artist, _albumTitle, _mEventStopSiteSearches, TimeLimitForSite);
        albumSearchSite.GetAlbumInfo();
        ValidateSearchOutput(albumSearchSite.AlbumInfo, albumInfoSite);
      };
      var searchThread = new Thread(job);
      searchThread.Start();
    }

    private bool ValidateSearchOutput(List<Album> albums, string site)
    {
      if (_searchHasEnded == false)
      {
        Monitor.Enter(this);
        try
        {
          if (albums.Count > 0)
          {
            _albumFound = true;
            _controller.AlbumFound = new Object[] { albums, site};
            if (++_mSitesSearched == AlbumSites.Count - 1)
            {
              Dispose();
            }
            return true;
          }
          else
          {
            if (++_mSitesSearched == AlbumSites.Count - 1)
            {
              Dispose();
            }
            return false;
          }
        }
        finally
        {
          Monitor.Exit(this);
        }
      }
      return false;
    }

    private void StopDueToTimeLimit(object sender, EventArgs e)
    {
      Dispose();
    }

    #endregion
  }
}
