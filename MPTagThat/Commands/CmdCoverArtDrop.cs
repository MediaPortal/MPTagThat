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
using TagLib;
using Picture = MPTagThat.Core.Common.Picture;

namespace MPTagThat.Commands
{
  [SupportedCommandType("CoverArtDrop")]
  public class CmdCoverArtDrop : Command
  {
    #region Variables

    #endregion

    private Picture _pic;

    #region ctor

    public CmdCoverArtDrop(object[] parameters)
    {
      object[] cmdParms = (object[])parameters[1];
      var fileName = (string)cmdParms[0];

      Util.SendMessage("setwaitcursor", null);
      Util.SendProgress(string.Format("Downloading picture from {0}", fileName));
      if (fileName.ToLower().StartsWith("http"))
      {
        _pic = GetCoverArtFromUrl(fileName);
      }
      else
      {
        _pic = new Picture(fileName);
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

    public override bool Execute(ref TrackData track, int rowIndex)
    {
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
      try
      {
        // When dragging from Google images, we have a imgurl. extract the right url.
        int imgurlIndex = url.IndexOf("imgurl=", StringComparison.Ordinal);
        if (imgurlIndex > -1)
        {
          url = url.Substring(imgurlIndex + 7);
          url = url.Substring(0, url.IndexOf("&", StringComparison.Ordinal));
        }

        Log.Info("Retrieving Coverart from: {0}", url);
        WebRequest req = WebRequest.Create(url);
        req.Proxy = new WebProxy();
        req.Proxy.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = req.GetResponse();
        Stream stream = response.GetResponseStream();
        if (stream == null)
        {
          return null;
        }
        Image img = Image.FromStream(stream);
        stream.Close();

        var pic = new Picture { Data = Picture.ImageToByte((Image)img.Clone()) };

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
    /// <returns></returns>
    public override bool PostProcess()
    {
      TracksGrid.MainForm.SetGalleryItem();
      Util.SendMessage("resetwaitcursor", null);
      return false;
    }

    #endregion

  }
}