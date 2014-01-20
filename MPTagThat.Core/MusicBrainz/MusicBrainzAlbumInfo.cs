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
using System.Xml;
using MPTagThat.Core.Amazon;

#endregion

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzAlbumInfo : IDisposable
  {
    #region Private Constants

    private const string musicBrainzUrl = "http://musicbrainz.org/ws/1/release/";
    private const string requestByID = "{0}?type=xml&inc=artist+release-events+tracks";

    #endregion

    #region Public methods

    public MusicBrainzAlbum GetMusicBrainzAlbumById(string albumID)
    {
      MusicBrainzAlbum album = new MusicBrainzAlbum();

      string requestString = musicBrainzUrl + string.Format(requestByID, albumID);
      string responseXml = Util.GetWebPage(requestString);
      if (responseXml == null)
        return null;

      XmlDocument xml = new XmlDocument();
      xml.LoadXml(responseXml);
      XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
      nsMgr.AddNamespace("m", "http://musicbrainz.org/ns/mmd-1.0#");

      XmlNodeList nodes = xml.SelectNodes("/m:metadata/m:release", nsMgr);
      if (nodes.Count > 0)
      {
        album.Id = nodes[0].Attributes["id"].InnerXml;
        foreach (XmlNode childNode in nodes[0])
        {
          if (childNode.Name == "title")
            album.Title = childNode.InnerText;

          if (childNode.Name == "asin")
            album.Asin = childNode.InnerText;

          if (childNode.Name == "artist")
          {
            foreach (XmlNode artistNode in childNode)
            {
              if (artistNode.Name == "name")
              {
                album.Artist = artistNode.InnerText;
                break;
              }
            }
          }

          if (childNode.Name == "release-event-list")
          {
            foreach (XmlNode releaseNode in childNode)
            {
              if (releaseNode.Name == "event")
                album.Year = releaseNode.Attributes["date"].InnerXml;
            }
          }

          if (childNode.Name == "track-list")
          {
            foreach (XmlNode trackNode in childNode.ChildNodes)
            {
              MusicBrainzTrack track = new MusicBrainzTrack();
              track.Id = trackNode.Attributes["id"].InnerXml;
              track.AlbumId = album.Id;
              track.Album = album.Title;
              track.Artist = album.Artist;
              foreach (XmlNode trackDetail in trackNode.ChildNodes)
              {
                if (trackDetail.Name == "title")
                  track.Title = trackDetail.InnerText;

                if (trackDetail.Name == "duration")
                  track.Duration = Convert.ToInt32(trackDetail.InnerText);
              }
              album.Tracks.Add(track);
            }
          }
        }

        if (album.Asin != null)
        {
          // Now do a lookup on Amazon for the Image Url
          using (AmazonAlbumInfo amazonInfo = new AmazonAlbumInfo())
          {
            AmazonAlbum amazonAlbum = amazonInfo.AmazonAlbumLookup(album.Asin);
            album.Amazon = amazonAlbum;
          }
        }
      }

      return album;
    }

    #endregion

    #region IDisposable Members

    public void Dispose() {}

    #endregion
  }
}