#region Copyright (C) 2009-2015 Team MediaPortal
// Copyright (C) 2009-2015 Team MediaPortal
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
using System.Drawing;
using System.IO;
using System.Net;
using MPTagThat.Core;
using MPTagThat.GridView;
using TagLib;
using Picture = MPTagThat.Core.Common.Picture;

namespace MPTagThat.Commands
{
  [SupportedCommandType("CoverArtDrop")]
  public class CmdCoverArtDrop : Command
  {
    #region Variables

    private GridViewTracks _tracksGrid;

    #endregion

    private Picture _pic = null;
    private string _fileName = null;

    #region ctor

    public CmdCoverArtDrop(object[] parameters)
    {
      object[] cmdParms = (object[])parameters[1];
      _fileName = (string)cmdParms[0];

      Util.SendMessage("setwaitcursor", null);
      Util.SendProgress(string.Format("Downloading picture from {0}", _fileName));
      if (_fileName.ToLower().StartsWith("http"))
      {
        _pic = GetCoverArtFromUrl(_fileName);
      }
      else
      {
        _pic = new Picture(_fileName);
      }
      if (_pic == null || _pic.Data == null)
      {
        Util.SendMessage("resetwaitcursor", null);
        return;
      }

      if (Options.MainSettings.ChangeCoverSize && Picture.ImageFromData(_pic.Data).Width > Options.MainSettings.MaxCoverWidth)
      {
        _pic.Resize(Options.MainSettings.MaxCoverWidth);
      }

      _pic.MimeType = "image/jpg";
      _pic.Description = "Front Cover";
      _pic.Type = PictureType.FrontCover;
    }

    #endregion

    #region Command Implementation

    public override bool Execute(ref TrackData track, GridViewTracks tracksGrid,int rowIndex)
    {
      _tracksGrid = tracksGrid;

      if (_pic == null || _pic.Data == null)
      {
        return false;
      }

      track.Pictures.Clear();
      track.Pictures.Add(_pic);
      return true;
    }

    /// <summary>
    /// Get Cover Art from a given Url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private Picture GetCoverArtFromUrl(string url)
    {
      Picture pic = null;
      try
      {
        // When dragging from Google images, we have a imgurl. extract the right url.
        int imgurlIndex = url.IndexOf("imgurl=");
        if (imgurlIndex > -1)
        {
          url = url.Substring(imgurlIndex + 7);
          url = url.Substring(0, url.IndexOf("&"));
        }

        Log.Info("Retrieving Coverart from: {0}", url);
        WebRequest req = WebRequest.Create(url);
        req.Proxy = new WebProxy();
        req.Proxy.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = req.GetResponse();
        if (response == null)
        {
          return null;
        }
        Stream stream = response.GetResponseStream();
        if (stream == null)
        {
          return null;
        }
        Image img = Image.FromStream(stream);
        stream.Close();

        pic = new Picture { Data = Picture.ImageToByte((Image)img.Clone()) };

        if (Options.MainSettings.ChangeCoverSize && img.Width > Options.MainSettings.MaxCoverWidth)
        {
          pic.Resize(Options.MainSettings.MaxCoverWidth);
        }

        img.Dispose();
        return pic;
      }
      catch (Exception ex)
      {
        Log.Error("Error retrieving Image from Url: {0} Error: {1}", url, ex.Message);
      }
      return null;
    }

    /// <summary>
    /// Post Process after command execution
    /// </summary>
    /// <param name="tracksGrid"></param>
    /// <returns></returns>
    public override bool PostProcess(GridViewTracks tracksGrid)
    {
      _tracksGrid.MainForm.SetGalleryItem();
      Util.SendMessage("resetwaitcursor", null);
      return false;
    }

    #endregion

  }
}