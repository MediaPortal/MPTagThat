using System;
using System.Collections;
using System.Threading;

[assembly: CLSCompliant(true)]

namespace LyricsEngine
{
    public class LyricsController : IDisposable
    {
        private string[] lyricsSites;
        private bool m_allowAllToComplete;
        private bool m_automaticUpdate;

        // Main thread sets this event to stop LyricController
        private ManualResetEvent m_EventStop_LyricController;

        // The LyricController sets this when all lyricSearch threads have been aborted
        private ManualResetEvent m_EventStopped_LyricController;

        private string[] m_findArray;
        private ILyricForm m_Form;
        private int m_noOfCurrentSearches;
        private int m_noOfLyricsFound;
        private int m_noOfLyricsNotFound;
        private int m_noOfLyricsSearched;
        private int m_noOfLyricsToSearch;
        private string[] m_replaceArray;
        private bool m_StopSearches;
        private ArrayList threadList = new ArrayList();

        public LyricsController(ILyricForm mainForm,
                                ManualResetEvent eventStopThread,
                                string[] lyricSites,
                                bool allowAllToComplete, bool automaticUpdate,
                                string find, string replace)
        {
            m_Form = mainForm;
            m_allowAllToComplete = allowAllToComplete;
            m_automaticUpdate = automaticUpdate;

            m_noOfLyricsToSearch = 1;
            m_noOfLyricsSearched = 0;
            m_noOfLyricsFound = 0;
            m_noOfLyricsNotFound = 0;
            m_noOfCurrentSearches = 0;

            ArrayList sitesArrayList = new ArrayList();

            // If search all, then include all
            foreach (string site in lyricSites)
            {
                if (Setup.IsMember(site))
                {
                    sitesArrayList.Add(site);
                }
            }
            lyricsSites = (string[])sitesArrayList.ToArray(typeof(string));


            LyricSearch.LyricsSites = lyricsSites;

            m_EventStop_LyricController = eventStopThread;
            m_EventStopped_LyricController = new ManualResetEvent(false);

            if (!string.IsNullOrEmpty(find) && !string.IsNullOrEmpty(replace))
            {
                if (find != "")
                {
                    m_findArray = find.Split(',');
                    m_replaceArray = replace.Split(',');
                }
            }
        }

        public bool StopSearches
        {
            get { return m_StopSearches; }
            set
            {
                if (value == true)
                {
                    m_StopSearches = true;
                    //StopTheSearchAndAbort.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    m_StopSearches = false;
                }
            }
        }

        public int NoOfLyricsToSearch
        {
            set { m_noOfLyricsToSearch = value; }
        }

        public int NoOfCurrentSearches
        {
            get { return m_noOfCurrentSearches; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            // clean-up operations may be placed here
            for (int i = 0; i < threadList.Count; i++)
            {
                ((Thread)threadList[i]).Abort();
            }

            m_StopSearches = true;

            bool stillThreadsAlive = threadList.Count > 0;
            while (stillThreadsAlive)
            {
                stillThreadsAlive = false;
                for (int i = 0; i < threadList.Count; i++)
                {
                    if (((Thread)threadList[i]).IsAlive)
                        stillThreadsAlive = true;
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
                if (m_EventStop_LyricController.WaitOne())
                {
                    // clean-up operations may be placed here
                    for (int i = 0; i < threadList.Count; i++)
                    {
                        ((Thread)threadList[i]).Abort();
                    }

                    m_StopSearches = true;

                    bool stillThreadsAlive = threadList.Count > 0;
                    while (stillThreadsAlive)
                    {
                        stillThreadsAlive = false;
                        for (int i = 0; i < threadList.Count; i++)
                        {
                            if (((Thread)threadList[i]).IsAlive)
                                stillThreadsAlive = true;
                        }

                    }

                    m_EventStopped_LyricController.Set();
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
            if (lyricsSites.Length > 0 && !string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(title))
            {
                ++m_noOfCurrentSearches;

                // create worker thread instance
                ThreadStart threadInstance = delegate
                {
                    LyricSearch lyricSearch = new LyricSearch(this, artist, title,
                                                              strippedArtistName,
                                                              row,
                                                              m_allowAllToComplete,
                                                              m_automaticUpdate);
                    lyricSearch.Run();
                };

                Thread lyricSearchThread = new Thread(threadInstance);
                lyricSearchThread.Name = "BasicSearch for " + artist + " - " + title; // looks nice in Output window
                lyricSearchThread.IsBackground = true;
                lyricSearchThread.Start();
                threadList.Add(lyricSearchThread);
            }
            else
            {
                return;
            }
        }


        internal void UpdateString(String message, String site)
        {
            m_Form.UpdateString = new Object[] { message, site };
        }

        internal void StatusUpdate(string artist, string title, string site, bool lyricFound)
        {
            if (lyricFound)
                ++m_noOfLyricsFound;
            else
                ++m_noOfLyricsNotFound;

            ++m_noOfLyricsSearched;

            m_Form.UpdateStatus = new Object[]
                                      {
                                          m_noOfLyricsToSearch, m_noOfLyricsSearched, m_noOfLyricsFound,
                                          m_noOfLyricsNotFound
                                      };

            if ((m_noOfLyricsSearched >= m_noOfLyricsToSearch))
            {
                FinishThread(artist, title, "All songs have been searched!", site);
            }
        }


        internal void LyricFound(String lyricStrings, String artist, String title, String site, int row)
        {
            string cleanLyric = LyricUtil.FixLyrics(lyricStrings, m_findArray, m_replaceArray);

            --m_noOfCurrentSearches;

            if (m_allowAllToComplete || m_StopSearches == false)
            {
                m_Form.LyricFound = new Object[] { cleanLyric, artist, title, site, row };
                StatusUpdate(artist, title, site, true);
            }
        }

        internal void LyricNotFound(String artist, String title, String message, String site, int row)
        {
            --m_noOfCurrentSearches;

            if (m_allowAllToComplete || m_StopSearches == false)
            {
                m_Form.LyricNotFound = new Object[] { artist, title, message, site, row };
                StatusUpdate(artist, title, site, false);
            }
        }

        public void FinishThread(String artist, String title, String message, String site)
        {
            m_StopSearches = true;
            m_EventStop_LyricController.Set();

            while (!m_EventStopped_LyricController.WaitOne(Timeout.Infinite, true))
            {
                Thread.Sleep(50);
            }
            m_Form.ThreadFinished = new Object[] { artist, title, message, site };
        }

        internal void ThreadException(String s)
        {
            m_Form.ThreadException = s;
        }
    }
}