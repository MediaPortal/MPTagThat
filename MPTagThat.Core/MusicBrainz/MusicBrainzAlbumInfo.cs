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
using System.Xml;
using MPTagThat.Core.Amazon;

#endregion

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzAlbumInfo : IDisposable
  {
    #region Private Constants

    private const string musicBrainzUrl = "http://musicbrainz.org/ws/2/release";
    private const string requestByID = "/{0}?format=xml&inc=artists+recordings";

    #endregion

    #region Public methods

    public MusicBrainzAlbum GetMusicBrainzAlbumById(string albumID)
    {
      var album = new MusicBrainzAlbum();
      album.Id = albumID;

      var requestString = musicBrainzUrl + string.Format(requestByID, albumID);
      var responseXml = Util.GetWebPage(requestString);
      if (responseXml == null)
        return null;

      var xml = new XmlDocument();
      xml.LoadXml(responseXml);
      var nsMgr = new XmlNamespaceManager(xml.NameTable);
      nsMgr.AddNamespace("m", "http://musicbrainz.org/ns/mmd-2.0#");

      // Selecting the Title
      var nodes = xml.SelectNodes("/m:metadata/m:release/m:title", nsMgr);
      if (nodes == null)
      {
        return null;
      }
      album.Title = nodes[0].InnerText;

      // Get the Album Artist(s)
      nodes = xml.SelectNodes("/m:metadata/m:release/m:artist-credit/m:name-credit/m:artist/m:name", nsMgr);
      if (nodes != null && nodes.Count > 0)
      {
        var artists = new List<string>();
        foreach (XmlNode node in nodes)
        {
          artists.Add(node.InnerText);
        }
        album.Artist = string.Join(";", artists);
      }

      // Selecting the Date
      nodes = xml.SelectNodes("/m:metadata/m:release/m:date", nsMgr);
      if (nodes != null && nodes.Count > 0)
      {
        string year = nodes[0].InnerText;
        if (year.Length > 4)
        {
          year = year.Substring(0, 4);
        }
        album.Year = year;
      }
      else
      {
        album.Year = string.Empty;
      }

      // Selecting the Asin
      nodes = xml.SelectNodes("/m:metadata/m:release/m:asin", nsMgr);
      if (nodes != null && nodes.Count > 0)
      {
        album.Asin = nodes[0].InnerText;
      }

      // Selecting the Media
      nodes = xml.SelectNodes("/m:metadata/m:release/m:medium-list/m:medium", nsMgr);
      if (nodes != null && nodes.Count > 0)
      {
        int pos = 1;
        album.DiscCount = nodes.Count;
        foreach (XmlNode node in nodes)
        {
          foreach (XmlNode childNode in node)
          {
            if (childNode.Name == "position")
              pos = Convert.ToInt32(childNode.InnerText);

            if (childNode.Name == "track-list")
            {
              int trackCount = Convert.ToInt32(childNode.Attributes["count"].Value);
              foreach (XmlNode trackNode in childNode.ChildNodes)
              {
                var track = new MusicBrainzTrack();
                track.DiscId = pos;
                track.TrackCount = trackCount;
                foreach (XmlNode trackDetail in trackNode.ChildNodes)
                {
                  if (trackDetail.Name == "recording")
                    track.Id = trackDetail.Attributes["id"].InnerXml;

                  if (trackDetail.Name == "position")
                    track.Number = Convert.ToInt32(trackDetail.InnerText);
                }
                album.Tracks.Add(track);
              }
            }
            
          }
        }
      }

      if (album.Asin != null)
      {
        // Now do a lookup on Amazon for the Image Url
        using (var amazonInfo = new AmazonAlbumInfo())
        {
          AmazonAlbum amazonAlbum = amazonInfo.AmazonAlbumLookup(album.Asin);
          album.Amazon = amazonAlbum;
        }
      }

      return album;
    }

    #endregion

    #region IDisposable Members

    public void Dispose() { }

    #endregion
  }
}