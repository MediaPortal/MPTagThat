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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using BassRegistration;

namespace MPTagThat.Core.AlbumInfo.AlbumSites
{
  public class Amazon : AbstractAlbumSite
  {
    #region Variables

    private const string itemSearch =
      "&Operation=ItemSearch&Artist={0}&Title={1}&SearchIndex=Music&ResponseGroup=Images,ItemAttributes,Tracks";

    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

    #endregion

    #region Properties

    public override string SiteName
    {
      get { return "Amazon"; }
    }

    public override bool SiteActive()
    {
      return true;
    }

    #endregion

    #region ctor

    public Amazon(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit) : base(artist, title, mEventStopSiteSearches, timeLimit)
    {
    }

    #endregion

    #region Methods

    protected override void GetAlbumInfoWithTimer()
    {
      log.Debug("Amazon: Searching Amazon Webservices");
      var helper = new SignedRequestHelper(Options.MainSettings.AmazonSite);
      var requestString =
        helper.Sign(string.Format(itemSearch, HttpUtility.UrlEncode(ArtistName), HttpUtility.UrlEncode(AlbumName)));

      var uri = new Uri(requestString);
      var client = new AlbumInfoWebClient();
      client.OpenReadCompleted += CallbackMethod;
      client.OpenReadAsync(uri);

      while (Complete == false)
      {
        if (MEventStopSiteSearches.WaitOne(500, true))
        {
          Complete = true;
        }
      }
    }

    private void CallbackMethod(object sender, OpenReadCompletedEventArgs e)
    {
      Stream reply = null;
      StreamReader reader = null;
      Albums.Clear();

      try
      {
        reply = e.Result;
        reader = new StreamReader(reply, Encoding.UTF8);
        var responseString = reader.ReadToEnd();

        var xml = new XmlDocument();
        xml.LoadXml(responseString);

        if (xml.DocumentElement == null)
        {
          log.Debug("Amazon: Amazon Webservices did not return any data");
          return;
        }

        // Retrieve default Namespace of the document and add it to the NameSpacemanager
        string defaultNameSpace = xml.DocumentElement.GetNamespaceOfPrefix("");
        XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
        nsMgr.AddNamespace("ns", defaultNameSpace);

        XmlNodeList nodes = xml.SelectNodes("/ns:ItemSearchResponse/ns:Items/ns:Item", nsMgr);
        if (nodes.Count > 0)
        {
          foreach (XmlNode node in nodes)
          {
            Album newAlbum = FillAlbum(node);

            // Check if we already got an album with the same title.
            // This happens, if the same album has been released by different distributors, labels
            // we will skip the album in this case
            bool found = Albums.Any(album => album.Title == newAlbum.Title);

            if (!found)
            {
              if (newAlbum.LargeImageUrl != null || newAlbum.MediumImageUrl != null || newAlbum.SmallImageUrl != null)
              {
                Albums.Add(newAlbum);
              }
            }
          }
        }
        log.Debug("Amazon: Found {0} albums", Albums.Count);
      }
      catch
      {
        log.Debug("Amazon: Exception receiving Album Information");
        Albums.Clear();
      }
      finally
      {
        if (reader != null)
        {
          reader.Close();
        }

        if (reply != null)
        {
          reply.Close();
        }
        Complete = true;
      }
    }

    private Album FillAlbum(XmlNode node)
    {
      Album album = new Album();

      foreach (XmlNode childNode in node)
      {
        if (childNode.Name == "ASIN")
          album.Asin = childNode.InnerText;

        if (childNode.Name == "SmallImage")
        {
          album.SmallImageUrl = GetNode(childNode, "URL");
          album.CoverWidth = GetNode(childNode, "Width");
          album.CoverHeight = GetNode(childNode, "Height");
        }

        if (childNode.Name == "MediumImage")
        {
          album.MediumImageUrl = GetNode(childNode, "URL");
          album.CoverWidth = GetNode(childNode, "Width");
          album.CoverHeight = GetNode(childNode, "Height");
        }

        if (childNode.Name == "LargeImage")
        {
          album.LargeImageUrl = GetNode(childNode, "URL");
          album.CoverWidth = GetNode(childNode, "Width");
          album.CoverHeight = GetNode(childNode, "Height");
        }

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

          List<List<AlbumTrack>> discs = new List<List<AlbumTrack>>();
          List<AlbumTrack> tracks = new List<AlbumTrack>();
          foreach (XmlNode discNode in childNode.ChildNodes)
          {
            foreach (XmlNode trackNode in discNode)
            {
              AlbumTrack track = new AlbumTrack();
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
    ///   Get the Url node
    /// </summary>
    /// <param name = "node"></param>
    /// <returns></returns>
    private string GetNode(XmlNode node, string nodeString)
    {
      foreach (XmlNode child in node.ChildNodes)
      {
        if (child.Name == nodeString)
          return child.InnerText;
      }
      return "";
    }

    #endregion

  }
}
