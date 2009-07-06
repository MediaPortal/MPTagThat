using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using LyricsEngine.lrcfinder;
using LyricsEngine;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace LyricsEngine.LyricSites
{
    public class LrcFinder
    {
        private lrcfinder.LrcFinder lrcFinder;
        private string artist = "";
        private string title = "";
        public static bool Abort;
        public static string Domain = null;

        public static bool WebExceptionOccured = false;


        public LrcFinder()
        {

        }

        public string FindLRC(string artist, string title)
        {
            this.artist = artist;
            this.title = title;

            string lrc = string.Empty;

            if (Abort == false)
            {
                lrcFinder = new lrcfinder.LrcFinder();

                try
                {
                    string url = GetUrl();

                    if (url == null)
                    {
                        lrc = null;
                    }
                    else
                    {
                        lrcFinder.Url = GetUrl();

                        lrc = lrcFinder.FindLRC(this.artist, this.title);
                    }
                }
                catch (Exception e)
                {
                    lrc = "Not found";
                }
            }

            if (LrcReturned(lrc))
            {
                //Encoding iso8859 = Encoding.GetEncoding("ISO-8859-1");
                //string make = Encoding.UTF8.GetString(iso8859.GetBytes(lrc));
                return lrc;
            }
            else
            {
                return "Not found";
            }
        }

        public DataTable FindLRCs(string artist, string title)
        {
            this.artist = artist;
            this.title = title;

            DataTable lrcs = null;

            if (Abort == false)
            {
                lrcFinder = new lrcfinder.LrcFinder();

                try
                {
                    string url = GetUrl();

                    if (url == null)
                    {
                        lrcs = null;
                    }
                    else
                    {
                        lrcFinder.Url = GetUrl();

                        lrcs = lrcFinder.FindLRCs(this.artist, this.title);
                    }
                }
                catch
                {
                    //lrcs = null;
                }
            }

            return lrcs;
        }

        private bool LrcReturned(string lrc)
        {
            if (lrc != null && !lrc.Equals("Not found") && !lrc.Equals("NOT FOUND") && lrc.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveLrc(string lrcFile)
        {
            lrcFinder = new lrcfinder.LrcFinder();

            try
            {

                string url = GetUrl();

                if (url == null)
                {
                    return false;
                }
                else
                {
                    lrcFinder.Url = GetUrl();

                    string result = lrcFinder.SaveLRC(lrcFile);
                    return result.Equals("DONE");
                }
            }
            catch
            {
                return false;
            }
        }

        public bool SaveLrcWithGuid(string lrcFile, Guid guid)
        {
            lrcFinder = new lrcfinder.LrcFinder();

            try
            {
                string url = GetUrl();

                if (url == null)
                {
                    return false;
                }
                else
                {
                    lrcFinder.Url = GetUrl();

                    string result = lrcFinder.SaveLRCWithGuid(lrcFile, guid);
                    return result.Equals("DONE");
                }
            }
            catch
            {
                return false;
            }
        }

        private string GetUrl()
        {
            //Domain = "http://testLRCFinder.profiler.nl";

            if (WebExceptionOccured)
            {
                return null;
            }

            try
            {

                if (string.IsNullOrEmpty(Domain))
                {
                    string[] domains = lrcFinder.NewDomain();
                    Random r = new Random();
                    Domain = domains[r.Next(domains.Length)] as string;
                }

                return Domain + @"/LrcFinder.asmx";
            }
            catch (WebException e)
            {
                WebExceptionOccured = true;
                return null;
            }
        }

    }
}
