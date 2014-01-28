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

    private const string acoustIdUrl = "http://api.acoustid.org/v2/lookup";
    private const string musicBrainzUrl = "http://musicbrainz.org/ws/2/recording";
    private const string musicBrainzRequestByID = "/{0}?format=xml&inc=artists+releases";

    #endregion

    #region ctor

    public MusicBrainzTrackInfo()
    {
      _postParameters.Add("client", "mfbgmu2P");
      _postParameters.Add("format", "xml");
      _postParameters.Add("meta", "recordingids");
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
      log.Debug("Identify: Get track information by using fingerprint");
      List<MusicBrainzTrack> tracks = new List<MusicBrainzTrack>();

      _postParameters.Add("duration", duration.ToString());
      _postParameters.Add("fingerprint", fingerPrint);

      string responseXml = Util.HttpPostRequest(acoustIdUrl, _postParameters);
      if (responseXml == null)
        return tracks;

      var xml = new XmlDocument();
      xml.LoadXml(responseXml);

      List<string> trackIds = new List<string>();
      var idNodes = xml.SelectNodes("/response/results/result/recordings/recording/id");
      if (idNodes != null)
      {
        foreach (XmlNode idNode in idNodes)
        {
          trackIds.Add(idNode.InnerText);
        }
      }

      log.Debug("Identify: Lookup Track and Album Information at MusicBrainz");
      foreach (var trackId in trackIds)
      {
        string musicBrainzRequest = musicBrainzUrl + string.Format(musicBrainzRequestByID, trackId);
        responseXml = Util.GetWebPage(musicBrainzRequest);
        if (responseXml != null)
        {
          MusicBrainzTrack mbTrack = ParseMusicBrainzQueryResult(responseXml);
          if (mbTrack != null)
          {
            tracks.Add(mbTrack);
          }
        }
      }

      return tracks;
    }

    MusicBrainzTrack ParseMusicBrainzQueryResult(string responseXml)
    {
      var track = new MusicBrainzTrack();
      var xml = new XmlDocument();
      xml.LoadXml(responseXml);
      var nsMgr = new XmlNamespaceManager(xml.NameTable);
      nsMgr.AddNamespace("m", "http://musicbrainz.org/ns/mmd-2.0#");

      // Getting the Id
      var idNode = xml.SelectSingleNode("/m:metadata/m:recording", nsMgr);
      if (idNode == null)
      {
        return null;
      }
      track.Id = idNode.Attributes["id"].Value;

      // Selecting the Title
      var nodes = xml.SelectNodes("/m:metadata/m:recording/m:title", nsMgr);
      if (nodes == null)
      {
        return null;
      }
      track.Title = nodes[0].InnerText;

      // Get the Duration
      nodes = xml.SelectNodes("/m:metadata/m:recording/m:length", nsMgr);
      if (nodes != null && nodes.Count > 0)
      {
        track.Duration = Convert.ToInt32(nodes[0].InnerText) / 1000;
      }

      // Get the Artist(s)
      nodes = xml.SelectNodes("/m:metadata/m:recording/m:artist-credit/m:name-credit/m:artist/m:name", nsMgr);
      if (nodes != null)
      {
        var artists = new List<string>();
        foreach (XmlNode node in nodes)
        {
          artists.Add(node.InnerText);
        }
        track.Artist = string.Join(";", artists);
      }

      // Get the Release(s)
      nodes = xml.SelectNodes("/m:metadata/m:recording/m:release-list/m:release", nsMgr);
      if (nodes != null)
      {
        foreach (XmlNode node in nodes)
        {
          var release = new MusicBrainzRelease();
          release.AlbumId = node.Attributes["id"].Value;
          foreach (XmlNode childNode in node)
          {
            if (childNode.Name == "title")
              release.Album = childNode.InnerText;

            if (childNode.Name == "country")
              release.Country = childNode.InnerText;

            if (childNode.Name == "date")
            {
              string year = childNode.InnerText;
              if (year.Length > 4)
              {
                year = year.Substring(0, 4);
              }
              release.Year = Convert.ToInt32(year);
            }
          }
          track.Releases.Add(release);
        }
      }
      return track;
    }

    #endregion

    #region IDisposable Members

    public void Dispose() { }

    #endregion
  }
}