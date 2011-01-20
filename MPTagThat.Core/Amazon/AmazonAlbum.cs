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

using System.Collections.Generic;
using System.IO;
using System.Net;
using TagLib;

#endregion

namespace MPTagThat.Core.Amazon
{
  public class AmazonAlbum
  {
    #region Private Fields

    private string _coverHeight = "";
    private string _coverWidth = "";
    private string _largeImage;
    private string _mediumImage;
    private string _smallImage;

    #endregion

    #region ctor

    public AmazonAlbum()
    {
      Discs = new List<List<AmazonAlbumTrack>>();
    }

    #endregion

    #region Properties

    public string Asin { get; set; }

    public string Title { get; set; }

    public string Artist { get; set; }

    public string Binding { get; set; }

    public string Label { get; set; }

    public string Year { get; set; }

    public List<List<AmazonAlbumTrack>> Discs { get; set; }

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

    public string CoverWidth
    {
      get { return _coverWidth; }
      set { _coverWidth = value; }
    }

    public string CoverHeight
    {
      get { return _coverHeight; }
      set { _coverHeight = value; }
    }

    public ByteVector AlbumImage
    {
      get
      {
        ByteVector vector = new ByteVector();

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
          WebRequest webReq = null;
          webReq = WebRequest.Create(sURL);
          WebResponse webResp = webReq.GetResponse();
          Stream stream = webResp.GetResponseStream();

          byte[] data = Util.ReadFullStream(stream, 32768);
          if (data.Length > 0)
            vector.Add(data);
        }
        catch {}
        return vector;
      }
    }

    #endregion
  }
}