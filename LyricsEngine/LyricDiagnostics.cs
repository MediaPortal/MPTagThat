using System;
using System.Diagnostics;
using System.IO;

namespace LyricsEngine
{
    public static class LyricDiagnostics
    {
        private static string logFileName = "";
        private static FileStream objStream;
        private static TextWriterTraceListener objTraceListener;
        private static Stopwatch stopWatch;
        private static TraceSource ts;

        public static TraceSource TraceSource
        {
            get
            {
                if (ts != null)
                {
                    ts.Flush();
                    return ts;
                }
                else return null;
            }
        }


        public static void OpenLog(string url)
        {
            try
            {
                logFileName = url;

                if (ts == null)
                {
                    if (File.Exists(logFileName))
                    {
                        FileInfo file = new FileInfo(logFileName);
                        try
                        {
                            file.Delete();
                        }
                        catch
                        {
                        }
                        ;
                    }

                    ts = new TraceSource("MyLyrics");
                    ts.Switch = new SourceSwitch("sw1", "All");
                    objStream = new FileStream(logFileName, FileMode.OpenOrCreate);
                    objTraceListener = new TextWriterTraceListener(objStream);
                    objTraceListener.Filter = new EventTypeFilter(SourceLevels.All);
                    ts.Listeners.Add(objTraceListener);
                    StartTimer();
                }
            }
            catch (Exception e)
            {
                ;
            }
        }

        public static void Dispose()
        {
            if (ts != null)
            {
                ts.Flush();
                ts.Close();
                StopTimer();

                objStream.Close();
                objStream.Dispose();
                try
                {
                    objTraceListener.Close();
                    objTraceListener.Dispose();
                }
                catch
                {
                }

                if (File.Exists(logFileName))
                {
                    FileStream file = new FileStream(logFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    file.Close();
                }
            }
        }

        private static void StartTimer()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        private static void StopTimer()
        {
            if (stopWatch != null)
            {
                stopWatch.Stop();
            }
        }

        public static string ElapsedTimeString()
        {
            if (stopWatch != null)
            {
                long time = stopWatch.ElapsedMilliseconds;
                long sec = time/1000;
                long ms = (time/100) - (sec*10);
                string str = "";
                str += (sec < 100) ? "0" : "";
                str += (sec < 10) ? "0" : "";
                str += sec + "." + ms;
                return str + ": ";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}