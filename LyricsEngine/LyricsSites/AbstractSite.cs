using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace LyricsEngine.LyricsSites
{
    public abstract class AbstractSite : ILyricSite
    {
        #region const

        // Not found
        public const string NotFound = "Not found";

        #endregion const

        #region members

        // Artist
        protected readonly string Artist;
        // Title
        protected readonly string Title;
        // Stop event
        protected WaitHandle MEventStopSiteSearches;
        // Time Limit
        protected readonly int TimeLimit;

        // Lyrics
        protected string LyricText = "";

        // Complete
        protected bool Complete;
        private Timer _searchTimer;

        #endregion members


        protected AbstractSite(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit)
        {
            // Artist
            Artist = artist;
            // Title
            Title = title;
            // Stop search event
            MEventStopSiteSearches = mEventStopSiteSearches;
            // Time Limit
            TimeLimit = timeLimit;
        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Close();
            _searchTimer.Dispose();

            LyricText = NotFound;
            Complete = true;
            Thread.CurrentThread.Abort();
        }

        protected abstract void FindLyricsWithTimer();


        #region interface abstract methods

        public string Lyric
        {
            get { return LyricText; }
        }

        public abstract string Name { get; }
        public abstract string BaseUrl { get; }
        public void FindLyrics()
        {
            try
            {
                // timer
                _searchTimer = new Timer { Enabled = false, Interval = TimeLimit };
                _searchTimer.Elapsed += TimerElapsed;
                _searchTimer.Start();

                // Find Lyrics
                FindLyricsWithTimer();
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
        
        public abstract LyricType GetLyricType();
        public abstract SiteType GetSiteType();
        public abstract SiteComplexity GetSiteComplexity();
        public abstract SiteSpeed GetSiteSpeed();
        public abstract bool SiteActive();

        #endregion interface abstract methods
    }
}
