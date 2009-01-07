using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MPTagThat.Core.Amazon
{
  public class AmazonAlbumInfo : IDisposable
  {

    #region Private Constants
    private const string amazonUrl = "http://webservices.amazon.com/onca/xml?Service=AWSECommerceService&SubscriptionId=0XCDYPB7YGRYE8T6G302";
    private const string itemLookup = "&Operation=ItemLookup&ItemId={0}&ResponseGroup=Images,ItemAttributes,Tracks";
    private const string itemSearch = "&Operation=ItemSearch&Artist={0}&Title={1}&SearchIndex=Music&ResponseGroup=Images,ItemAttributes,Tracks";
    #endregion

    #region Public methods
    /// <summary>
    /// Lookup an Amazon Album, for which the ASIN is known.
    /// Should find one exact match.
    /// </summary>
    /// <param name="albumID"></param>
    /// <returns></returns>
    public AmazonAlbum AmazonAlbumLookup(string albumID)
    {
      AmazonAlbum album = new AmazonAlbum();

      string requestString = amazonUrl + string.Format(itemLookup, albumID);
      string responseXml = Util.GetWebPage(requestString);
      if (responseXml == null)
        return album;

      XmlDocument xml = new XmlDocument();
      xml.LoadXml(responseXml);
      XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
      nsMgr.AddNamespace("ns", "http://webservices.amazon.com/AWSECommerceService/2005-10-05");

      XmlNodeList nodes = xml.SelectNodes("/ns:ItemLookupResponse/ns:Items/ns:Item", nsMgr);
      if (nodes.Count > 0)
      {
        album = FillAlbum(nodes[0]);
      }
      return album;
    }

    /// <summary>
    /// Search on Amazon for an albums of a specific artist
    /// </summary>
    /// <param name="artist"></param>
    /// <param name="albumTitle"></param>
    /// <returns></returns>
    public List<AmazonAlbum> AmazonAlbumSearch(string artist, string albumTitle)
    {
      List<AmazonAlbum> albums = new List<AmazonAlbum>();

      string requestString = amazonUrl + string.Format(itemSearch, System.Web.HttpUtility.UrlEncode(artist), System.Web.HttpUtility.UrlEncode(albumTitle));
      string responseXml = Util.GetWebPage(requestString);
      if (responseXml == null)
        return albums;

      XmlDocument xml = new XmlDocument();
      xml.LoadXml(responseXml);
      XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
      nsMgr.AddNamespace("ns", "http://webservices.amazon.com/AWSECommerceService/2005-10-05");

      XmlNodeList nodes = xml.SelectNodes("/ns:ItemSearchResponse/ns:Items/ns:Item", nsMgr);
      if (nodes.Count > 0)
      {
        foreach (XmlNode node in nodes)
        {
          AmazonAlbum newAlbum = FillAlbum(node);

          // Check if we already got an album with the same title.
          // This happens, if the same album has been released by different distributors, labels
          // we will skip the album in this case
          bool found = false;
          foreach (AmazonAlbum album in albums)
          {
            if (album.Title == newAlbum.Title)
            {
              found = true;
              break;
            }
          }

          if (!found)
            if (newAlbum.LargeImageUrl != null || newAlbum.MediumImageUrl != null || newAlbum.SmallImageUrl != null)
              albums.Add(newAlbum);
        }
      }
      return albums;
    }
    #endregion

    #region Private Methods
    private AmazonAlbum FillAlbum(XmlNode node)
    {
      AmazonAlbum album = new AmazonAlbum();

      foreach (XmlNode childNode in node)
      {
        if (childNode.Name == "ASIN")
          album.Asin = childNode.InnerText;

        if (childNode.Name == "SmallImage")
          album.SmallImageUrl = GetUrl(childNode);

        if (childNode.Name == "MediumImage")
          album.MediumImageUrl = GetUrl(childNode);

        if (childNode.Name == "LargeImage")
          album.LargeImageUrl = GetUrl(childNode);

        if (childNode.Name == "ItemAttributes")
        {
          foreach (XmlNode attributeNode in childNode)
          {
            if (attributeNode.Name == "Artist")
              album.Artist = attributeNode.InnerText;

            if (attributeNode.Name == "Title")
              album.Title = attributeNode.InnerText;

            if (attributeNode.Name == "ReleaseDate")
              album.Year = attributeNode.InnerText;

            if (attributeNode.Name == "Binding")
              album.Binding = attributeNode.InnerText;

            if (attributeNode.Name == "Label")
              album.Label = attributeNode.InnerText;
          }
        }

        if (childNode.Name == "Tracks")
        {
          // The node starts with a "<Disc Number Node" , we want all subnodes of it

          List<List<AmazonAlbumTrack>> discs = new List<List<AmazonAlbumTrack>>();
          List<AmazonAlbumTrack> tracks = new List<AmazonAlbumTrack>();
          foreach (XmlNode discNode in childNode.ChildNodes)
          {
            foreach (XmlNode trackNode in discNode)
            {
              AmazonAlbumTrack track = new AmazonAlbumTrack();
              track.Number = Convert.ToInt32(trackNode.Attributes["Number"].Value);
              track.Title = trackNode.InnerText;
              tracks.Add(track);
            }
            discs.Add(tracks);
          }
          album.Discs = discs;
        }
      }

      return album;
    }

    /// <summary>
    /// Get the Url node
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private string GetUrl(XmlNode node)
    {
      string url = "";
      foreach (XmlNode child in node.ChildNodes)
      {
        if (child.Name == "URL")
          return child.InnerText;
      }
      return url;
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
    }
    #endregion
  }
}
