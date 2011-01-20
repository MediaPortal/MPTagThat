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

using System.IO;

#endregion

namespace MPTagThat.Core
{
  public class TrackDataPreview
  {
    #region Variables

    private readonly string _fullFileName;

    #endregion

    #region ctor

    public TrackDataPreview(string fileName)
    {
      _fullFileName = fileName;
      FileName = Path.GetFileName(fileName);
    }

    #endregion

    #region Properties

    #region Common Properties

    /// <summary>
    ///   The Full Filename including the path
    /// </summary>
    public string FullFileName
    {
      get { return _fullFileName; }
    }

    /// <summary>
    ///   Filename without Path
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    ///   The New Full Filename including the path
    /// </summary>
    public string NewFullFileName { get; set; }

    /// <summary>
    ///   New Filename without Path
    /// </summary>
    public string NewFileName { get; set; }

    #endregion

    #region Tags

    /// <summary>
    ///   Artist / Performer Tag
    ///   ID3: TPE1
    /// </summary>
    public string Artist { get; set; }

    /// <summary>
    ///   Album Artist / Band  / Orchestra Tag
    ///   ID3: TPE2
    /// </summary>
    public string AlbumArtist { get; set; }

    /// <summary>
    ///   ALbum Tag
    ///   ID3: TALB
    /// </summary>
    public string Album { get; set; }

    /// <summary>
    ///   Beats Per Minute Tag
    ///   ID3: TBPM
    /// </summary>
    public string BPM { get; set; }

    /// <summary>
    ///   Comment Tag
    ///   ID3: COMM
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    ///   Composert Tag
    ///   ID3: TCOM
    /// </summary>
    public string Composer { get; set; }

    /// <summary>
    ///   Conductor Tag
    ///   ID3: TPE3
    /// </summary>
    public string Conductor { get; set; }

    /// <summary>
    ///   Position in Mediaset Tag
    ///   ID3: TPOS
    /// </summary>
    public string Disc { get; set; }

    public string NumDisc { get; set; }


    /// <summary>
    ///   Interpreted / Remixed / Modified by Tag
    ///   ID3: TPE4
    /// </summary>
    public string Interpreter { get; set; }

    /// <summary>
    ///   Genre Tag
    ///   ID3: TCON
    /// </summary>
    public string Genre { get; set; }

    /// <summary>
    ///   Content Group  Tag
    ///   ID3: TIT1
    /// </summary>
    public string Grouping { get; set; }

    /// <summary>
    ///   SubTitle / More Detailed Description
    ///   ID3: TIT3
    /// </summary>
    public string SubTitle { get; set; }

    /// <summary>
    ///   Title Tag
    ///   ID3: TIT2
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///   Track Tag
    ///   ID3: TRCK
    /// </summary>
    public string Track { get; set; }

    /// <summary>
    ///   Track Tag
    ///   ID3: TRCK
    /// </summary>
    public string NumTrack { get; set; }

    /// <summary>
    ///   Year Tag
    ///   ID3: TYER
    /// </summary>
    public string Year { get; set; }

    #endregion

    #endregion
  }
}