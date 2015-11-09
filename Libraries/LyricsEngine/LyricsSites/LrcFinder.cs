using System;
using System.Data;
using System.Net;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public class LrcFinder : AbstractSite
    {
        # region const

        // Name
        private const string SiteName = "LrcFinder";

        // Base url
        private const string SiteBaseUrl = "http://testLRCFinder.profiler.nl";

        # endregion

        public static bool Abort;
        public static string Domain = null;

        public static bool WebExceptionOccured = false;

        public LrcFinder(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
        {
        }

        #region interface implemetation

        protected override void FindLyricsWithTimer()
        {
            var findLRC = FindLRC();
            LyricText = !string.IsNullOrEmpty(findLRC) ? findLRC : NotFound;
        }

        public override string Name
        {
            get { return SiteName; }
        }

        public override string BaseUrl
        {
            get { return SiteBaseUrl; }
        }

        public override LyricType GetLyricType()
        {
            return LyricType.Lrc;
        }

        public override SiteType GetSiteType()
        {
            return SiteType.Api;
        }

        public override SiteComplexity GetSiteComplexity()
        {
            return SiteComplexity.OneStep;
        }

        public override SiteSpeed GetSiteSpeed()
        {
            return SiteSpeed.Medium;
        }

        public override bool SiteActive()
        {
            return true;
        }

        #endregion interface implemetation 

        #region public methods

        public string FindLRC()
        {
            var artist = Artist;
            var title = Title;

            var lrc = string.Empty;

            if (Abort == false)
            {
                var lrcFinder = new lrcfinder.LrcFinder();

                try
                {
                    var url = GetUrl(lrcFinder);

                    if (url == null)
                    {
                        lrc = null;
                    }
                    else
                    {
                        lrcFinder.Url = GetUrl(lrcFinder);

                        lrc = lrcFinder.FindLRC(artist, title);
                    }
                }
                catch (Exception)
                {
                    lrc = NotFound;
                }
            }

            if (LrcReturned(lrc))
            {
                //Encoding iso8859 = Encoding.GetEncoding("ISO-8859-1");
                //string make = Encoding.UTF8.GetString(iso8859.GetBytes(lrc));
                return lrc;
            }
            return NotFound;
        }

        public DataTable FindLRCs()
        {
            var artist = Artist;
            var title = Title;

            DataTable lrcs = null;

            if (Abort == false)
            {
                var lrcFinder = new lrcfinder.LrcFinder();

                try
                {
                    var url = GetUrl(lrcFinder);

                    if (url != null)
                    {
                        lrcFinder.Url = GetUrl(lrcFinder);

                        lrcs = lrcFinder.FindLRCs(artist, title);
                    }
                }
                catch
                {
                    ;
                }
            }

            return lrcs;
        }

        public bool SaveLrc(string lrcFile)
        {
            var lrcFinder = new lrcfinder.LrcFinder();

            try
            {
                var url = GetUrl(lrcFinder);

                if (url == null)
                {
                    return false;
                }
                lrcFinder.Url = GetUrl(lrcFinder);

                var result = lrcFinder.SaveLRC(lrcFile);

                return result.Equals("DONE");
            }
            catch
            {
                return false;
            }
        }

        public static bool SaveLrcWithGuid(string lrcFile, Guid guid)
        {
            var lrcFinder = new lrcfinder.LrcFinder();

            try
            {
                var url = GetUrl(lrcFinder);

                if (url == null)
                {
                    return false;
                }
                lrcFinder.Url = GetUrl(lrcFinder);

                var result = lrcFinder.SaveLRCWithGuid(lrcFile, guid);
                return result.Equals("DONE");
            }
            catch
            {
                return false;
            }
        }

        #endregion public methods

        #region private methods

        private static bool LrcReturned(string lrc)
        {
            return !string.IsNullOrEmpty(lrc) && !lrc.Equals(NotFound, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string GetUrl(lrcfinder.LrcFinder lrcFinder)
        {
            if (WebExceptionOccured)
            {
                return null;
            }

            try
            {
                if (string.IsNullOrEmpty(Domain))
                {
                    var domains = lrcFinder.NewDomain();
                    var r = new Random();
                    Domain = domains[r.Next(domains.Length)];
                }

                return Domain + @"/LrcFinder.asmx";
            }
            catch (WebException)
            {
                WebExceptionOccured = true;
                return null;
            }
        }
        
        #endregion private methods
    }
}