using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.XPath;

using Un4seen.Bass;

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzTrackInfo : IDisposable
  {
    #region Private Fields
    private string fingerprint = string.Empty;
    private long lengthInMS = 0;
    private string puid = string.Empty;
    #endregion

    #region Private Constants
    private const string musicDnsKey = "2188faab0b911688a1079f4297253ccf";
    private const string musicDnsUrl = "http://ofa.musicdns.org/ofa/1/track";
    private const string fingerprint_request_format = "cid={0}&cvr={1}&fpt={2}&rmd={3:d}&brt={4:d}&fmt={5}&dur={6:d}&art={7}&ttl={8}&alb={9}&tnm={10:d}&gnr={11}&yrr={12}&enc={13}";
    private const string clientVersion = "1.0.0.0";
    private const string unknown = "unknown";

    private const string musicBrainzUrl = "http://musicbrainz.org/ws/1/track/?type=xml&inc=artist+releases&puid=";
    #endregion

    #region Public Methods
    public List<MusicBrainzTrack> GetMusicBrainzTrack(string fileName)
    {
      // Create a Decoding Channel
      int channel = Bass.BASS_StreamCreateFile(fileName, 0, 0, BASSFlag.BASS_STREAM_DECODE);
      if (channel == 0)
        return null;

      // Now get information about channel and retrieve length
      BASS_CHANNELINFO chinfo;
      chinfo = Bass.BASS_ChannelGetInfo(channel);


      // Get 135 Seconds of the file to be used for the fingerprint
      int sampleLength = (int)Bass.BASS_ChannelSeconds2Bytes(channel, 135);
      byte[] samples = new byte[sampleLength];
      sampleLength = Bass.BASS_ChannelGetData(channel, samples, sampleLength);
      lengthInMS = (int)Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel)) * 1000;
      Bass.BASS_StreamFree(channel);

      // Get Fingerprint
      puid = "";
      StringBuilder fp = new StringBuilder(758);
      if (NativeMethods.ofa_create_print(fp, samples, 0, (int)sampleLength / 2, chinfo.freq, chinfo.chans == 2 ? true : false))
      {
        fingerprint = fp.ToString();
        puid = GetPuid();
      }

      if (puid == "")
        return null;

      return RetrieveMusicBrainzTrack(puid);
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Get the PUID, which we need for MusicBrainz lookup, from the MusicDNS Server
    /// </summary>
    /// <returns></returns>
    private string GetPuid()
    {
      string requestString = musicDnsUrl + "?" + string.Format(fingerprint_request_format,
        musicDnsKey.Trim(),         // Dns Key
        clientVersion.Trim(),       // Version
        fingerprint,                // Fingerprint Lookup
        "0",                        // No Info to be returned
        0,                          // The bitrate -> "0"
        "wav",                      // Fo
        lengthInMS,
        unknown,                    // Artist
        unknown,                    // Title,
        unknown,                    // Album,
        0,                          // Track number
        unknown,                    // Genre,
        "0",                        // Year,
        "");

      string responseXml = Util.GetWebPage(requestString);
      string puid = string.Empty;

      if (responseXml == null)
        return puid;

      XmlDocument xml = new XmlDocument();
      xml.LoadXml(responseXml);
      XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
      nsMgr.AddNamespace("m", "http://musicbrainz.org/ns/mmd-1.0#");

      XmlNodeList nodes = xml.SelectNodes("/m:metadata/m:track/m:puid-list/m:puid", nsMgr);
      if (nodes.Count > 0)
      {
        puid = nodes[0].Attributes["id"].InnerXml;
      }
      return puid;
    }

    /// <summary>
    /// Get the MusicBrainz Track information for the PUID
    /// </summary>
    /// <param name="puid"></param>
    /// <returns></returns>
    private List<MusicBrainzTrack> RetrieveMusicBrainzTrack(string puid)
    {
      List<MusicBrainzTrack> tracks = new List<MusicBrainzTrack>();

      string requestString = musicBrainzUrl + puid;
      string responseXml = Util.GetWebPage(requestString);
      if (responseXml == null)
        return tracks;

      XmlDocument xml = new XmlDocument();
      xml.LoadXml(responseXml);
      XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
      nsMgr.AddNamespace("m", "http://musicbrainz.org/ns/mmd-1.0#");

      XmlNodeList nodes = xml.SelectNodes("/m:metadata/m:track-list/m:track", nsMgr);
      if (nodes.Count > 0)
      {
        foreach (XmlNode trackListNode in nodes)
        {
          MusicBrainzTrack track = new MusicBrainzTrack();
          track.Id = new Guid(trackListNode.Attributes["id"].InnerXml);
          foreach (XmlNode childNode in trackListNode)
          {
            if (childNode.Name == "title")
              track.Title = childNode.InnerText;

            if (childNode.Name == "duration")
              track.Duration = Convert.ToInt32(childNode.InnerText);

            if (childNode.Name == "artist")
            {
              foreach (XmlNode artistNode in childNode)
              {
                if (artistNode.Name == "name")
                {
                  track.Artist = artistNode.InnerText;
                  break;
                }
              }
            }

            if (childNode.Name == "release-list")
            {
              track.AlbumID = new Guid(childNode.ChildNodes[0].Attributes["id"].InnerXml);
              foreach (XmlNode releaseNode in childNode.ChildNodes[0].ChildNodes)
              {
                if (releaseNode.Name == "title")
                  track.Album = releaseNode.InnerText;

                if (releaseNode.Name == "track-list")
                  track.Number = Convert.ToInt32(releaseNode.Attributes["offset"].InnerXml) + 1;
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

    public void Dispose()
    {
    }

    #endregion
  }
}