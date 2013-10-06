using System;
using System.Collections.Generic;
using System.Threading;

[assembly: CLSCompliant(true)]

namespace LyricsEngine
{
    public class LyricsController : IDisposable
    {
        private readonly string[] _lyricsSites;

        private readonly bool _mAllowAllToComplete;
        private readonly bool _mAutomaticUpdate;

        // Main thread sets this event to stop LyricController
        private readonly ManualResetEvent _mEventStopLyricController;

        // The LyricController sets this when all lyricSearch threads have been aborted
        private readonly ManualResetEvent _mEventStoppedLyricController;

        private readonly string[] _mFindArray;
        private readonly ILyricForm _mForm;

        private readonly string[] _mReplaceArray;

        private readonly List<Thread> _threadList = new List<Thread>();

        private int _mNoOfCurrentSearches;
        private int _mNoOfLyricsFound;
        private int _mNoOfLyricsNotFound;
        private int _mNoOfLyricsSearched;
        private int _mNoOfLyricsToSearch;
        
        private bool _mStopSearches;
        

        public LyricsController(ILyricForm mainForm,
                                ManualResetEvent eventStopThread,
                                string[] lyricSites,
                                bool allowAllToComplete, bool automaticUpdate,
                                string find, string replace)
        {
            _mForm = mainForm;
            _mAllowAllToComplete = allowAllToComplete;
            _mAutomaticUpdate = automaticUpdate;

            _mNoOfLyricsToSearch = 1;
            _mNoOfLyricsSearched = 0;
            _mNoOfLyricsFound = 0;
            _mNoOfLyricsNotFound = 0;
            _mNoOfCurrentSearches = 0;

            _lyricsSites = lyricSites;

            LyricSearch.LyricsSites = _lyricsSites;

            _mEventStopLyricController = eventStopThread;
            _mEventStoppedLyricController = new ManualResetEvent(false);

            if (!string.IsNullOrEmpty(find) && !string.IsNullOrEmpty(replace))
            {
                if (find != "")
                {
                    _mFindArray = find.Split(',');
                    _mReplaceArray = replace.Split(',');
                }
            }
        }

        public bool StopSearches
        {
            get { return _mStopSearches; }
            set { _mStopSearches = value; }
        }

        public int NoOfLyricsToSearch
        {
            set { _mNoOfLyricsToSearch = value; }
        }

        public int NoOfCurrentSearches
        {
            get { return _mNoOfCurrentSearches; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            // clean-up operations may be placed here
            foreach (var thread in _threadList)
            {
                thread.Abort();
            }

            _mStopSearches = true;

            var stillThreadsAlive = _threadList.Count > 0;
            while (stillThreadsAlive)
            {
                stillThreadsAlive = false;
                foreach (var thread in _threadList)
                {
                    if (thread.IsAlive)
                    {
                        stillThreadsAlive = true;
                    }
                }
            }

            FinishThread("", "", "The search has ended.", "");
        }

        #endregion

        public void Run()
        {
            // check if thread is cancelled
            while (true)
            {
                Thread.Sleep(100);

                // check if thread is cancelled
                if (_mEventStopLyricController.WaitOne())
                {
                    // clean-up operations may be placed here
                    foreach (var thread in _threadList)
                    {
                        thread.Abort();
                    }

                    _mStopSearches = true;

                    var stillThreadsAlive = _threadList.Count > 0;
                    while (stillThreadsAlive)
                    {
                        stillThreadsAlive = false;
                        foreach (var thread in _threadList)
                        {
                            if (thread.IsAlive)
                            {
                                stillThreadsAlive = true;
                            }
                        }
                    }

                    _mEventStoppedLyricController.Set();
                    break;
                }
            }
        }


        public void AddNewLyricSearch(string artist, string title, string strippedArtistName)
        {
            AddNewLyricSearch(artist, title, strippedArtistName, -1);
        }

        public void AddNewLyricSearch(string artist, string title, string strippedArtistName, int row)
        {
            if (_lyricsSites.Length > 0 && !string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(title))
            {
                ++_mNoOfCurrentSearches;

                // create worker thread instance
                ThreadStart threadInstance = delegate
                    {
                        var lyricSearch = new LyricSearch(this, artist, title, strippedArtistName, row, _mAllowAllToComplete, _mAutomaticUpdate);
                        lyricSearch.Run();
                    };

                var lyricSearchThread = new Thread(threadInstance);
                lyricSearchThread.Name = "BasicSearch for " + artist + " - " + title; // looks nice in Output window
                lyricSearchThread.IsBackground = true;
                lyricSearchThread.Start();
                _threadList.Add(lyricSearchThread);
            }
        }


        internal void UpdateString(String message, String site)
        {
            _mForm.UpdateString = new Object[] {message, site};
        }

        internal void StatusUpdate(string artist, string title, string site, bool lyricFound)
        {
            if (lyricFound)
            {
                ++_mNoOfLyricsFound;
            }
            else
            {
                ++_mNoOfLyricsNotFound;
            }

            ++_mNoOfLyricsSearched;

            _mForm.UpdateStatus = new Object[]
                {
                    _mNoOfLyricsToSearch, _mNoOfLyricsSearched, _mNoOfLyricsFound,
                    _mNoOfLyricsNotFound
                };

            if ((_mNoOfLyricsSearched >= _mNoOfLyricsToSearch))
            {
                FinishThread(artist, title, "All songs have been searched!", site);
            }
        }


        internal void LyricFound(String lyricStrings, String artist, String title, String site, int row)
        {
            var cleanLyric = LyricUtil.FixLyrics(lyricStrings, _mFindArray, _mReplaceArray);

            --_mNoOfCurrentSearches;

            if (_mAllowAllToComplete || _mStopSearches == false)
            {
                _mForm.LyricFound = new Object[] {cleanLyric, artist, title, site, row};
                StatusUpdate(artist, title, site, true);
            }
        }

        internal void LyricNotFound(String artist, String title, String message, String site, int row)
        {
            --_mNoOfCurrentSearches;

            if (_mAllowAllToComplete || _mStopSearches == false)
            {
                _mForm.LyricNotFound = new Object[] {artist, title, message, site, row};
                StatusUpdate(artist, title, site, false);
            }
        }

        public void FinishThread(String artist, String title, String message, String site)
        {
            _mStopSearches = true;
            _mEventStopLyricController.Set();

            while (!_mEventStoppedLyricController.WaitOne(Timeout.Infinite, true))
            {
                Thread.Sleep(50);
            }
            _mForm.ThreadFinished = new Object[] {artist, title, message, site};
        }

        internal void ThreadException(String s)
        {
            _mForm.ThreadException = s;
        }
    }
}