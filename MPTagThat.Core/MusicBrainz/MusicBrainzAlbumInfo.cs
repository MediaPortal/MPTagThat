using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MPTagThat.Core.Amazon;

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
        album.Id = new Guid(nodes[0].Attributes["id"].InnerXml);
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
              track.Id = new Guid(trackNode.Attributes["id"].InnerXml);
              track.AlbumID = album.Id;
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
    public void Dispose()
    {
    }
    #endregion
  }
}
