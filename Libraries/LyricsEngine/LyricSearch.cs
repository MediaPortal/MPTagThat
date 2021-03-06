using System;
using System.Threading;
using LyricsEngine.LyricsSites;
using Timer = System.Timers.Timer;

namespace LyricsEngine
{
    /// <summary>
    /// Class emulates long process which runs in worker thread
    /// and makes synchronous user UI operations.
    /// </summary>
    public class LyricSearch : IDisposable
    {
        #region Members

        private const int TimeLimit = 30*1000;
        private const int TimeLimitForSite = 15*1000;

        public static string[] LyricsSites;

        // Reference to the lyric controller used to make syncronous user interface calls:
        private readonly LyricsController _mLyricsController;

        // Uses to inform the specified site searches to stop searching and exit
        private readonly ManualResetEvent _mEventStopSiteSearches;

        private readonly bool _mAllowAllToComplete;
        private readonly string _mArtist = "";
        private readonly bool _mAutomaticUpdate;

        private readonly string _mOriginalArtist = "";
        private readonly string _mOriginalTrack = "";
        private readonly string _mTitle = "";
        private readonly Timer _timer;
        private readonly int _mRow;

        private bool _lyricFound;
        private bool _mSearchHasEnded;
        private int _mSitesSearched;

        #endregion

        #region Functions

        internal LyricSearch(LyricsController lyricsController, string artist, string title, string strippedArtistName, int row, bool allowAllToComplete, bool automaticUpdate)
        {
            _mLyricsController = lyricsController;

            _mArtist = strippedArtistName;
            _mTitle = title;

            _mRow = row;

            _mOriginalArtist = artist;
            _mOriginalTrack = title;

            _mAllowAllToComplete = allowAllToComplete;
            _mAutomaticUpdate = automaticUpdate;

            _mEventStopSiteSearches = new ManualResetEvent(false);

            _timer = new Timer();
            _timer.Enabled = true;
            _timer.Interval = TimeLimit;
            _timer.Elapsed += StopDueToTimeLimit;
            _timer.Start();
        }

        public void Dispose()
        {
            _mSearchHasEnded = true;
            _mEventStopSiteSearches.Set();
            _timer.Enabled = false;
            _timer.Stop();
            _timer.Close();
            _timer.Dispose();
        }

        public void Run()
        {
            foreach (var lyricsSearchSite in LyricsSites)
            {
                RunSearchForSiteInThread(lyricsSearchSite);
            }

            while (!_mSearchHasEnded)
            {
                Thread.Sleep(300);
            }

            Thread.CurrentThread.Abort();
        }

        private void RunSearchForSiteInThread(string lyricsSearchSiteName)
        {
            ThreadStart job = delegate
                {
                    var lyricsSearchSite = LyricsSiteFactory.Create(lyricsSearchSiteName, _mArtist, _mTitle, _mEventStopSiteSearches, TimeLimitForSite);
                    lyricsSearchSite.FindLyrics();
                    if (_mAllowAllToComplete)
                    {
                        ValidateSearchOutputForAllowAllToComplete(lyricsSearchSite.Lyric, lyricsSearchSiteName);
                    }
                    else
                    {
                        ValidateSearchOutput(lyricsSearchSite.Lyric, lyricsSearchSiteName);
                    }
                };
            var searchThread = new Thread(job);
            searchThread.Start();
        }

        #endregion

        public bool ValidateSearchOutput(string lyric, string site)
        {
            if (_mSearchHasEnded == false)
            {
                Monitor.Enter(this);
                try
                {
                    ++_mSitesSearched;

                    // Parse the lyrics and find a suitable lyric, if any
                    if (!lyric.Equals(AbstractSite.NotFound) && lyric.Length != 0)
                    {
                        // if the lyrics hasn't been found by another site, then we have found the lyrics to count!
                        if (_lyricFound == false)
                        {
                            _lyricFound = true;
                            _mLyricsController.LyricFound(lyric, _mOriginalArtist, _mOriginalTrack, site, _mRow);
                            Dispose();
                            return true;
                        }
                            // if another was quicker it is just too bad... return
                        else
                        {
                            return false;
                        }
                    }
                        // still other lyricsites to search
                    else if (_mSitesSearched < LyricsSites.Length)
                    {
                        return false;
                    }
                        // the search got to end due to no more sites to search
                    else
                    {
                        _mLyricsController.LyricNotFound(_mOriginalArtist, _mOriginalTrack, "A matching lyric could not be found!", site, _mRow);
                        Dispose();
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

        public bool ValidateSearchOutputForAllowAllToComplete(string lyric, string site)
        {
            if (_mSearchHasEnded == false)
            {
                Monitor.Enter(this);
                try
                {
                    if (!lyric.Equals("Not found") && lyric.Length != 0)
                    {
                        _lyricFound = true;
                        _mLyricsController.LyricFound(lyric, _mOriginalArtist, _mOriginalTrack, site, _mRow);
                        if (++_mSitesSearched == LyricsSites.Length || _mAutomaticUpdate)
                        {
                            Dispose();
                        }
                        return true;
                    }
                    else
                    {
                        _mLyricsController.LyricNotFound(_mOriginalArtist, _mOriginalTrack, "A matching lyric could not be found!", site, _mRow);
                        if (++_mSitesSearched == LyricsSites.Length)
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
            _mLyricsController.LyricNotFound(_mOriginalArtist, _mOriginalTrack, "A matching lyric could not be found!", "All (timed out)", _mRow);
            Dispose();
        }

        #region Properties

        public bool SearchHasEnded
        {
            get { return _mSearchHasEnded; }
            set { _mSearchHasEnded = value; }
        }

        #endregion
    }
}