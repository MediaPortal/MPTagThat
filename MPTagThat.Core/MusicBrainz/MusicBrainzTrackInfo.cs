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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Un4seen.Bass;

#endregion

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzTrackInfo : IDisposable
  {
    #region Private Fields

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private string fingerprint = string.Empty;
    private long lengthInMS;
    private string fingerPrint = string.Empty;
    private List<string> _stdOutList = new List<string>(); 
    private Dictionary<string, string> _postParameters = new Dictionary<string, string>(); 

    #endregion

    #region Private Constants

    private const string musicBrainzUrl = "http://api.acoustid.org/v2/lookup";

    #endregion

    #region ctor

    public MusicBrainzTrackInfo()
    {
      _postParameters.Add("client", "mfbgmu2P");
      _postParameters.Add("format", "xml");
      _postParameters.Add("meta", "recordings+releases+tracks");
    }

    #endregion

    #region Public Methods

    public List<MusicBrainzTrack> GetMusicBrainzTrack(string fileName)
    {
      log.Debug("Identify: Creating Fingerprint");
      int duration = 0;
      // Get Fingerprint
      fingerPrint = GetFingerPrint(fileName, out duration);

      if (fingerPrint == "")
        return null;

      return RetrieveMusicBrainzTrack(fingerPrint, duration);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Get the Fingerprint by calling fpcalc / Chromaprint
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="duration"></param>
    /// <returns>FingerPrint</returns>
    private string GetFingerPrint(string fileName, out int duration)
    {
      //string fopsParam = string.Format("-length {0} \"{1}\"", duration, fileName);
      string fopsParam = string.Format("\"{0}\"", fileName);

      List<string> fingerPrint = ExecuteProcReturnStdOut("fpcalc.exe", fopsParam, 40000);
      if (fingerPrint.Count == 3)
      {
        duration = Convert.ToInt32(fingerPrint[1].Substring(9));
        return fingerPrint[2].Substring(12);
      }
      duration = 0;
      return "";
    }

    public List<string> ExecuteProcReturnStdOut(string aAppName, string aArguments, int aExpectedTimeoutMs)
    {
      _stdOutList.Clear();

      var fopsProc = new Process();
      var procOptions = new ProcessStartInfo(aAppName, aArguments);

      procOptions.UseShellExecute = false; // Important for WorkingDirectory behaviour
      procOptions.RedirectStandardOutput = true; // The precious data we're after
      procOptions.StandardOutputEncoding = Encoding.GetEncoding("ISO-8859-1"); // the output contains "Umlaute", etc.
      procOptions.CreateNoWindow = true; // Do not spawn a "Dos-Box"      
      procOptions.ErrorDialog = false; // Do not open an error box on failure        

      fopsProc.OutputDataReceived += StdOutDataReceived;
      fopsProc.EnableRaisingEvents = true; // We want to know when and why the process died        
      fopsProc.StartInfo = procOptions;

      try
      {
        fopsProc.Start();
        fopsProc.BeginOutputReadLine();
        // wait this many seconds until process has to be finished
        fopsProc.WaitForExit(aExpectedTimeoutMs);
      }
      catch (Exception ex)
      {
        log.Error("Identify: Error getting Fingerprint. {0} - {1}", ex.Message, ex.StackTrace);
      }

      return _stdOutList;
    }

    private void StdOutDataReceived(object sendingProc, DataReceivedEventArgs e)
    {
      if (e.Data != null)
      {
        _stdOutList.Add(e.Data);
      }
    }

    /// <summary>
    ///   Get the MusicBrainz Track information for the PUID
    /// </summary>
    /// <param name = "fingerPrint"></param>
    /// <param name = "duration"></param>
    /// <returns></returns>
    private List<MusicBrainzTrack> RetrieveMusicBrainzTrack(string fingerPrint, int duration)
    {
      log.Debug("Identify: Get track information");
      List<MusicBrainzTrack> tracks = new List<MusicBrainzTrack>();

      _postParameters.Add("duration", duration.ToString());
      _postParameters.Add("fingerprint", fingerPrint);

      string responseXml = Util.HttpPostRequest(musicBrainzUrl, _postParameters);
      if (responseXml == null)
        return tracks;

      XmlDocument xml = new XmlDocument();
      xml.LoadXml(responseXml);

      XmlNodeList nodes = xml.SelectNodes("/response/results/result/recordings/recording/releases/release");
      if (nodes.Count > 0)
      {
        foreach (XmlNode releaseNode in nodes)
        {
          MusicBrainzTrack track = new MusicBrainzTrack();
          foreach (XmlNode childNode in releaseNode)
          {
            if (childNode.Name == "title")
            {
              track.Album = childNode.InnerText;
            }

            if (childNode.Name == "artists")
            {
              foreach (XmlNode artistsNode in childNode)
              {
                foreach (XmlNode artistNode in artistsNode)
                {
                  if (artistNode.Name == "name")
                  {
                    track.AlbumArtist = artistNode.InnerText;
                    break;
                  }
                }
              }
            }

            if (childNode.Name == "date")
            {
              foreach (XmlNode dateNode in childNode)
              {
                if (dateNode.Name == "year")
                {
                  track.Year = Convert.ToInt32(dateNode.InnerText);
                  break;
                }
              }
            }

            if (childNode.Name == "id")
            {
              track.AlbumId = childNode.InnerText;
            }

            if (childNode.Name == "mediums")
            {
              foreach (XmlNode mediumNode in childNode.ChildNodes[0].ChildNodes)
              {
                if (mediumNode.Name == "tracks")
                {
                  foreach (XmlNode trackNode in mediumNode.ChildNodes[0])
                  {
                    if (trackNode.Name == "title")
                      track.Title = trackNode.InnerText;

                    if (trackNode.Name == "id")
                      track.Id = trackNode.InnerText;

                    if (trackNode.Name == "position")
                      track.Number = Convert.ToInt32(trackNode.InnerXml);

                    if (trackNode.Name == "artists")
                    {
                      foreach (XmlNode artistsNode in trackNode.ChildNodes)
                      {
                        foreach (XmlNode artistNode in artistsNode.ChildNodes)
                        {
                          if (artistNode.Name == "name")
                          {
                            track.Artist = artistNode.InnerText;
                            break;
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          tracks.Add(track);
        }
      }
      return tracks;
    }

    #endregion

    #region IDisposable Members

    public void Dispose() { }

    #endregion
  }
}