using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MPTagThat.Core.AlbumInfo.AlbumSites
{
	public abstract class AbstractAlbumSite
	{
		#region Variables

		// Not found
		public const string NotFound = "Not found";

		#endregion 

		#region Properties

		// Artist
		protected readonly string ArtistName;
		// Album
		protected readonly string AlbumName;
		// Stop event
		protected WaitHandle MEventStopSiteSearches;
		// Time Limit
		protected readonly int TimeLimit;

		// Album
		protected List<Album> Albums = new List<Album>();

		// Complete
		protected bool Complete;
		private Timer _searchTimer;

    // Album Information
    public List<Album> AlbumInfo
    {
      get { return Albums; }
    }

    // The AlbumSiteName
    public abstract string SiteName { get; }

    public abstract bool SiteActive();

		#endregion 

    #region ctor

    protected AbstractAlbumSite(string artist, string album, WaitHandle mEventStopSiteSearches, int timeLimit)
    {
        // Artist
        ArtistName = artist;
        // AlbumName
        AlbumName = album;
        // Stop search event
        MEventStopSiteSearches = mEventStopSiteSearches;
        // Time Limit
        TimeLimit = timeLimit;
    }

    #endregion

    #region Methods

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
      _searchTimer.Stop();
      _searchTimer.Close();
      _searchTimer.Dispose();

      Albums.Clear();;
      Complete = true;
      Thread.CurrentThread.Abort();
    }

    protected abstract void GetAlbumInfoWithTimer();

    public void GetAlbumInfo()
    {
      try
      {
        _searchTimer = new Timer { Enabled = false, Interval = TimeLimit };
        _searchTimer.Elapsed += TimerElapsed;
        _searchTimer.Start();

        GetAlbumInfoWithTimer();
      }
      finally
      {
        if (_searchTimer != null)
        {
          _searchTimer.Stop();
          _searchTimer.Close();
        }
      }
    }

    #endregion
  }
}
