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

namespace MPTagThat.Core.AlbumInfo
{
  public class Album
  {
    #region Private Fields

    private string _largeImage;
    private string _mediumImage;
    private string _smallImage;

    #endregion

    #region ctor

    public Album()
    {
      CoverHeight = "";
      CoverWidth = "";
      Discs = new List<List<AlbumTrack>>();
    }

    #endregion

    #region Properties

    public string MusicBrainzId { get; set; }

    public string Asin { get; set; }

    public string Title { get; set; }

    public string Artist { get; set; }

    public string Binding { get; set; }

    public string Label { get; set; }

    public string Year { get; set; }

    public int DiscCount { get; set; }

    public List<List<AlbumTrack>> Discs { get; set; }

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

    public string CoverWidth { get; set; }

    public string CoverHeight { get; set; }

    public ByteVector AlbumImage
    {
      get
      {
        ByteVector vector = new ByteVector();

        var sUrl = _largeImage ?? (_mediumImage ?? _smallImage);

	      if (sUrl == null)
          return null;

        try
        {
          WebRequest webReq = null;
          webReq = WebRequest.Create(sUrl);
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