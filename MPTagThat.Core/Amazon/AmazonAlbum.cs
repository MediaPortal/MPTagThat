using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core.Amazon
{
  public class AmazonAlbum
  {
    #region Private Fields
    private string _asin;
    private string _title;
    private string _artist;
    private string _year;
    private string _smallImage;
    private string _mediumImage;
    private string _largeImage;
    private List<List<AmazonAlbumTrack>> _discs;
    private string _binding;
    private string _label;
    #endregion

    #region ctor
    public AmazonAlbum()
    {
      _discs = new List<List<AmazonAlbumTrack>>();
    }
    #endregion

    #region Properties
    public string Asin
    {
      get { return _asin; }
      set { _asin = value; }
    }

    public string Title
    {
      get { return _title; }
      set { _title = value; }
    }

    public string Artist
    {
      get { return _artist; }
      set { _artist = value; }
    }

    public string Binding
    {
      get { return _binding; }
      set { _binding = value; }
    }

    public string Label
    {
      get { return _label; }
      set { _label = value; }
    }

    public string Year
    {
      get { return _year; }
      set { _year = value; }
    }

    public List<List<AmazonAlbumTrack>> Discs
    {
      get { return _discs; }
      set { _discs = value; }
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
