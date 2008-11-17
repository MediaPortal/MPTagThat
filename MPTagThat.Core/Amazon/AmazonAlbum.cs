using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core.Amazon
{
  public class AmazonAlbum
  {
    #region Private Fields
    private string asin;
    private string title;
    private string artist;
    private string year;
    private string _smallImage;
    private string _mediumImage;
    private string _largeImage;
    #endregion

    #region ctor
    public AmazonAlbum()
    {
    }
    #endregion

    #region Properties
    public string Asin
    {
      get { return asin; }
      set { asin = value; }
    }

    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    public string Artist
    {
      get { return artist; }
      set { artist = value; }
    }

    public string Year
    {
      get { return year; }
      set { year = value; }
    }

    public string SmallImageUrl
    {
      get { return _smallImage; }
      set { _smallImage = value; }
    }

    public string MediumImageUrl
    {
      get { return _mediumImage; }
      set { _mediumImage = value; }
    }

    public string LargeImageUrl
    {
      get { return _largeImage; }
      set { _largeImage = value; }
    }

    public TagLib.ByteVector AlbumImage
    {
      get
      {
        TagLib.ByteVector vector = new TagLib.ByteVector();

        string sURL = _largeImage;
        if (sURL == null)
        {
          sURL = _mediumImage;
          if (sURL == null)
            sURL = _smallImage;
        }

        if (sURL == null)
          return null;

        try
        {
          System.Net.WebRequest webReq = null;
          webReq = System.Net.WebRequest.Create(sURL);
          System.Net.WebResponse webResp = webReq.GetResponse();
          System.IO.Stream stream =  webResp.GetResponseStream();

          byte[] data = Util.ReadFullStream(stream, 32768);
          if (data.Length > 0)
            vector.Add(data);
        }
        catch
        {
        }
        return vector;
      }
    }
    #endregion
  }
}
