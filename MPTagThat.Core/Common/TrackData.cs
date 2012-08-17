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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MPTagThat.Core.Common;

#endregion

namespace MPTagThat.Core
{
  [Serializable]
  public class TrackData
  {
    #region Enum

    public enum MP3Error : int
    {
      NoError = 0,
      Fixable = 1,
      NonFixable = 2,
      Fixed = 3
    }

    #endregion

    #region Variables

    private const int NumTrackDigits = 2;

    private string _artist;
    private string _albumArtist;
    private string _album;
    private string _composer;
    private string _conductor;
    private string _copyright;
    private string _genre;
    private string _grouping;
    private string _title;
    private string _replaygainTrack;
    private string _replaygainAlbum;

    private MP3Error _mp3ValError;
    private string _mp3ValErrorText;
    private List<Picture> _pictures = new List<Picture>();
    private List<Comment> _comments = new List<Comment>();
    private List<Lyric> _lyrics = new List<Lyric>();
    private List<PopmFrame> _popmframes = new List<PopmFrame>();
    private List<TagLib.TagTypes> _removedTagTypes = new List<TagLib.TagTypes>();

    private static readonly string[] _standardId3Frames = new[]
                                                            {
                                                              "TPE1", "TPE2", "TALB", "TBPM", "COMM", "TCOM",
                                                              "TPE3", "TCOP", "TPOS", "TCON", "TIT1", "USLT", "APIC",
                                                              "POPM", "TIT2", "TRCK", "TYER", "TDRC"
                                                            };

    private static readonly string[] _extendedId3Frames = new[]
                                                             {
                                                               "TSOP", "TSOA", "WCOM", "WCOP", "TENC", "TPE4", "TIPL",
                                                               "IPLS", "TMED", "TMCL", "WOAF", "WOAR", "WOAS", "WORS", 
                                                               "WPAY", "WPUB", "TOAL", "TOFN", "TOLY", "TOPE", "TOWN", 
                                                               "TDOR", "TORY", "TPUB", "TIT3", "TEXT","TSOT", "TLEN",
                                                               "TCMP"
                                                             };

    #endregion

    #region ctor

    public TrackData()
    {
      Changed = false;
      Status = -1;
      _mp3ValError = MP3Error.NoError;
      _mp3ValErrorText = "";
      Frames = new List<Common.Frame>();
      UserFrames = new List<Frame>();
      ID3Version = 3;
    }

    #endregion

    #region Properties
    #region Common Properties

    private Guid _id;

    /// <summary>
    /// Unique ID of the Track
    /// To be used in identifying cloned / changed tracks
    /// </summary>
    public Guid Id
    {
      get { return _id; }
      set { _id = value; }
    }

    /// <summary>
    /// The ID3 Version
    /// </summary>
    public int ID3Version { get; set; }

    /// <summary>
    /// The Extended Frames included in the file
    /// </summary>
    public List<Common.Frame> Frames { get; set; }

    /// <summary>
    /// The User Defined Frames included in the file
    /// </summary>
    public List<Common.Frame> UserFrames { get; set; }

    /// <summary>
    /// The User Defined Frames that we have read before modification
    /// </summary>
    public List<Common.Frame> SavedUserFrames { get; set; }

    /// <summary>
    /// Current Status of Track, as indicated in Column 0 of grid
    /// -1 = Not set
    /// 0  = Ok
    /// 1  = Changed
    /// 2  = Error
    /// 3  = Broken Song
    /// 4  = Fixed Song
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Has the Track been changed
    /// </summary>
    public bool Changed { get; set; }

    /// <summary>
    /// Indicates, if the Tags have been removed
    /// </summary>
    public List<TagLib.TagTypes> TagsRemoved
    {
      get
      {
        return _removedTagTypes;
      }
    }

    /// <summary>
    /// Is the File Readonly
    /// </summary>
    public bool Readonly { get; set; }

    /// <summary>
    /// The Full Filename including the path
    /// </summary>
    public string FullFileName { get; set; }

    /// <summary>
    /// Filename without Path
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// File Extension
    /// </summary>
    public string FileExt
    {
      get { return Path.GetExtension(FullFileName); }
    }

    /// <summary>
    /// Path of the File
    /// </summary>
    public string FilePath
    {
      get { return Path.GetDirectoryName(FullFileName); }
    }

    /// <summary>
    /// The Tag Type
    /// </summary>
    public string TagType { get; set; }

    /// <summary>
    /// Number of Pictures in File
    /// </summary>
    public int NumPics
    {
      get { return Pictures.Count; }
    }

    /// <summary>
    /// Has the Track fixable errors?
    /// </summary>
    public MP3Error MP3ValidationError
    {
      get { return _mp3ValError; }
      set { _mp3ValError = value; }
    }

    public string MP3ValidationErrorText
    {
      get { return _mp3ValErrorText; }
      set { _mp3ValErrorText = value; }
    }

    /// <summary>
    /// The standard ID3 Frames directly supported by TagLib #
    /// </summary>
    public string[] StandardFrames
    {
      get { return _standardId3Frames; }
    }

    /// <summary>
    /// The extended ID3 Frames 
    /// </summary>
    public string[] ExtendedFrames
    {
      get { return _extendedId3Frames; }
    }

    #endregion

    #region Tags

    /// <summary>
    /// Artist / Performer Tag
    /// ID3: TPE1
    /// </summary>
    public string Artist
    {
      get { return _artist; }
      set { _artist = value ?? ""; }
    }

    /// <summary>
    /// Artist Sortname Tag (V 2.4 only)
    /// ID3: TSOP
    /// </summary>
    public string ArtistSortName
    {
      get { return GetFrame("TSOP"); }
      set { SetText("TSOP", value); }
    }

    /// <summary>
    /// Album Artist / Band  / Orchestra Tag
    /// ID3: TPE2
    /// </summary>
    public string AlbumArtist
    {
      get { return _albumArtist; }
      set { _albumArtist = value ?? ""; }
    }

    /// <summary>
    /// ALbum Tag
    /// ID3: TALB
    /// </summary>
    public string Album
    {
      get { return _album; }
      set { _album = value ?? ""; }
    }

    /// <summary>
    /// Album Sortname Tag (V 2.4 only)
    /// ID3: TSOA
    /// </summary>
    public string AlbumSortName
    {
      get { return GetFrame("TSOA"); }
      set { SetText("TSOA", value); }
    }

    /// <summary>
    /// Beats Per Minute Tag
    /// ID3: TBPM
    /// </summary>
    public int BPM { get; set; }

    /// <summary>
    /// Comment Tag
    /// ID3: COMM
    /// </summary>
    public string Comment
    {
      get
      {
        return _comments.Count > 0 ? _comments[0].Text : "";
      }
      set
      {
        if (_comments.Count == 0)
        {
          _comments.Add(new Comment("", "", value));
        }
        else
        {
          _comments[0].Text = value;
        }
      }
    }

    /// <summary>
    /// Comment Tag
    /// ID3: COMM
    /// </summary>
    public List<Comment> ID3Comments
    {
      get { return _comments; }
    }

    /// <summary>
    /// Commercial Information Tag
    /// ID3: WCOM
    /// </summary>
    public string CommercialInformation
    {
      get { return GetFrame("WCOM"); }
      set { SetText("WCOM", value); }
    }

    public bool Compilation { get; set; }

    /// <summary>
    /// Composer Tag
    /// ID3: TCOM
    /// </summary>
    public string Composer
    {
      get { return _composer; }
      set { _composer = value ?? ""; }
    }

    /// <summary>
    /// Conductor Tag
    /// ID3: TPE3
    /// </summary>
    public string Conductor
    {
      get { return _conductor; }
      set { _conductor = value ?? ""; }
    }

    /// <summary>
    /// Copyright Tag
    /// ID3: TCOP
    /// </summary>
    public string Copyright
    {
      get { return _copyright; }
      set { _copyright = value ?? ""; }
    }

    /// <summary>
    /// Copyright Information Tag
    /// ID3: WCOP
    /// </summary>
    public string CopyrightInformation
    {
      get { return GetFrame("WCOP"); }
      set { SetText("WCOP", value); }
    }

    /// <summary>
    /// Position in Mediaset Tag
    /// ID3: TPOS
    /// </summary>
    public string Disc
    {
      get
      {
        string disc = DiscNumber > 0 ? DiscNumber.ToString().PadLeft(NumTrackDigits, '0') : "";
        return DiscCount > 0
                 ? String.Format("{0}/{1}", disc, DiscCount.ToString().PadLeft(NumTrackDigits, '0'))
                 : disc;
      }

      set
      {
        string[] disc = null;
        try
        {
          disc = value.Split('/');
          if (disc[0] != "")
            DiscNumber = Convert.ToUInt32(disc[0]);
        }
        catch (Exception) { }

        try
        {
          if (disc[1] != "")
            DiscCount = Convert.ToUInt32(disc[1]);
        }
        catch (Exception) { }
      }
    }

    /// <summary>
    /// The Disc Number part of TPOS
    /// </summary>
    public UInt32 DiscNumber { get; set; }

    /// <summary>
    /// The Disc Count part of TPOS
    /// </summary>
    public UInt32 DiscCount { get; set; }

    /// <summary>
    /// Encoded By
    /// ID3: TENC
    /// </summary>
    public string EncodedBy
    {
      get { return GetFrame("TENC"); }
      set { SetText("TENC", value); }
    }

    /// <summary>
    /// Interpreted / Remixed / Modified by Tag
    /// ID3: TPE4
    /// </summary>
    public string Interpreter
    {
      get { return GetFrame("TPE4"); }
      set { SetText("TPE4", value); }
    }

    /// <summary>
    /// Genre Tag
    /// ID3: TCON
    /// </summary>
    public string Genre
    {
      get { return _genre; }
      set { _genre = value ?? ""; }
    }

    /// <summary>
    /// Content Group  Tag
    /// ID3: TIT1
    /// </summary>
    public string Grouping
    {
      get { return _grouping; }
      set { _grouping = value ?? ""; }
    }

    /// <summary>
    /// Involved People Tag
    /// ID3: IPLS (2.3) / TIPL (2.4)
    /// </summary>
    public string InvolvedPeople
    {
      get
      {
        if (TagType != "mp3")
          return "";

        if (ID3Version == 4)
          return GetFrame("TIPL");

        return GetFrame("IPLS");
      }
      set
      {
        if (TagType != "mp3")
          return;

        if (ID3Version == 4)
          SetText("TIPL", value);

        SetText("IPLS", value);
      }
    }

    /// <summary>
    /// Lyrics Tag
    /// ID3: USLT
    /// </summary>
    public string Lyrics
    {
      get
      {
        return _lyrics.Count > 0 ? _lyrics[0].Text : "";
      }
      set
      {
        if (_lyrics.Count == 0)
        {
          _lyrics.Add(new Lyric("", "", value));
        }
        else
        {
          _lyrics[0].Text = value;
        }
      }
    }


    public List<Lyric> LyricsFrames
    {
      get { return _lyrics; }
    }

    /// <summary>
    /// MediaType Tag
    /// ID3: TMED
    /// </summary>
    public string MediaType
    {
      get { return GetFrame("TMED"); }
      set { SetText("TMED", value); }
    }

    /// <summary>
    /// Music Credit List Tag
    /// ID3: TMCL
    /// </summary>
    public string MusicCreditList
    {
      get { return GetFrame("TMCL"); }
      set { SetText("TMCL", value); }
    }

    /// <summary>
    /// Official Audio File Information Tag
    /// ID3: WOAF
    /// </summary>
    public string OfficialAudioFileInformation
    {
      get { return GetFrame("WOAF"); }
      set { SetText("WOAF", value); }
    }

    /// <summary>
    /// Official Artist Information Tag
    /// ID3: WOAR
    /// </summary>
    public string OfficialArtistInformation
    {
      get { return GetFrame("WOAR"); }
      set { SetText("WOAR", value); }
    }

    /// <summary>
    /// Official Audio Source Information Tag
    /// ID3: WOAS
    /// </summary>
    public string OfficialAudioSourceInformation
    {
      get { return GetFrame("WOAS"); }
      set { SetText("WOAS", value); }
    }

    /// <summary>
    /// Official Internet Radio Station Information Tag
    /// ID3: WORS
    /// </summary>
    public string OfficialInternetRadioInformation
    {
      get { return GetFrame("WORS"); }
      set { SetText("WORS", value); }
    }

    /// <summary>
    /// Official Payment Information Tag
    /// ID3: WPAY
    /// </summary>
    public string OfficialPaymentInformation
    {
      get { return GetFrame("WPAY"); }
      set { SetText("WPAY", value); }
    }

    /// <summary>
    /// Official Publisher Information Tag
    /// ID3: WPUB
    /// </summary>
    public string OfficialPublisherInformation
    {
      get { return GetFrame("WPUB"); }
      set { SetText("WPUB", value); }
    }

    /// <summary>
    /// Original Album Title Tag
    /// ID3: TOAL
    /// </summary>
    public string OriginalAlbum
    {
      get { return GetFrame("TOAL"); }
      set { SetText("TOAL", value); }
    }

    /// <summary>
    /// Original File Name Tag
    /// ID3: TOFN
    /// </summary>
    public string OriginalFileName
    {
      get { return GetFrame("TOFN"); }
      set { SetText("TOFN", value); }
    }

    /// <summary>
    /// Original LyricsWriter Tag
    /// ID3: TOLY
    /// </summary>
    public string OriginalLyricsWriter
    {
      get { return GetFrame("TOLY"); }
      set { SetText("TOLY", value); }
    }

    /// <summary>
    /// Original Artist / Performer Tag
    /// ID3: TOPE
    /// </summary>
    public string OriginalArtist
    {
      get { return GetFrame("TOPE"); }
      set { SetText("TOPE", value); }
    }

    /// <summary>
    /// Original Owner Title Tag
    /// ID3: TOWN
    /// </summary>
    public string OriginalOwner
    {
      get { return GetFrame("TOWN"); }
      set { SetText("TOWN", value); }
    }

    /// <summary>
    /// Original Release Time Tag
    /// ID3: TORY (2.3) / TDOR (2.4)
    /// Handled transparently by Taglib. We only need to look for TDOR
    /// </summary>
    public string OriginalRelease
    {
      get
      {
        if (TagType != "mp3")
          return "";

        return GetFrame("TDOR");
      }
      set
      {
        if (TagType != "mp3")
          return;

        SetText("TDOR", value);
      }
    }


    public List<Picture> Pictures
    {
      get { return _pictures; }
    }

    /// <summary>
    /// Publisher Writer Tag
    /// ID3: TPUB
    /// </summary>
    public string Publisher
    {
      get { return GetFrame("TPUB"); }
      set { SetText("TPUB", value); }
    }

    /// <summary>
    /// Rating Tag
    /// ID3: POPM
    /// </summary>
    public int Rating
    {
      get
      {
        return _popmframes.Count > 0 ? _popmframes[0].Rating : 0;
      }
      set
      {
        if (_popmframes.Count == 0)
        {
          _popmframes.Add(new PopmFrame("", value, 0));
        }
        else
        {
          _popmframes[0].Rating = value;
        }
      }
    }

    public List<PopmFrame> Ratings
    {
      get { return _popmframes; }
    }

    public string ReplayGainTrack
    {
      get { return _replaygainTrack; } 
      
      set
      {
        if (value != "" && !value.Contains("db"))
        {
          value += " db";
        }
        _replaygainTrack = value;
      }
    }

    public string ReplayGainTrackPeak { get; set; }

    public string ReplayGainAlbum
    {
      get { return _replaygainAlbum; } 
      
      set
      {
        if (value != "" && !value.Contains("db"))
        {
          value += " db";
        }
        _replaygainAlbum = value;
      }
    }

    public string ReplayGainAlbumPeak { get; set; }


    /// <summary>
    /// SubTitle / More Detailed Description
    /// ID3: TIT3
    /// </summary>
    public string SubTitle
    {
      get { return GetFrame("TIT3"); }
      set { SetText("TIT3", value); }
    }

    /// <summary>
    /// Text / Lyrics Writer Tag
    /// ID3: TPE4
    /// </summary>
    public string TextWriter
    {
      get { return GetFrame("TEXT"); }
      set { SetText("TEXT", value); }
    }

    /// <summary>
    /// Title Tag
    /// ID3: TIT2
    /// </summary>
    public string Title
    {
      get { return _title; }
      set { _title = value ?? ""; }
    }

    /// <summary>
    /// Title SortName Tag (V 2.4 only)
    /// ID3: TSOT
    /// </summary>
    public string TitleSortName
    {
      get { return GetFrame("TSOT"); }
      set { SetText("TSOT", value); }
    }

    /// <summary>
    /// Track Tag
    /// ID3: TRCK
    /// </summary>
    public string Track
    {
      get
      {
        string track = TrackNumber > 0 ? TrackNumber.ToString().PadLeft(NumTrackDigits, '0') : "";
        return TrackCount > 0
                 ? String.Format("{0}/{1}", track, TrackCount.ToString().PadLeft(NumTrackDigits, '0'))
                 : track;
      }

      set
      {
        string[] track = null;
        try
        {
          track =  value.Split('/');
          if (track[0] != "")
            TrackNumber = Convert.ToUInt32(track[0]);
          else
            TrackNumber = 0;
        }
        catch (Exception)
        {
          TrackNumber = 0;
        }

        try
        {
          if (track.Length > 1 && track[1] != "")
            TrackCount = Convert.ToUInt32(track[1]);
          else
            TrackCount = 0;
        }
        catch (Exception)
        {
          TrackCount = 0;
        }
      }
    }

    /// <summary>
    /// The Track Number of the TRCK frame
    /// </summary>
    public UInt32 TrackNumber { get; set; }

    /// <summary>
    /// The Track Count of the TRCK frame
    /// </summary>
    public UInt32 TrackCount { get; set; }

    /// <summary>
    /// Track Length Tag
    /// ID3: TLEN
    /// </summary>
    public string TrackLength
    {
      get { return GetFrame("TLEN"); }
      set { SetText("TLEN", value); }
    }

    /// <summary>
    /// Year Tag
    /// ID3: TYER
    /// </summary>
    public int Year { get; set; }
    #endregion

    #region Audio File Properties

    /// <summary>
    /// The Duration of the File as string
    /// </summary>
    public string Duration
    {
      get
      {
        DateTime dt = new DateTime(DurationTimespan.Ticks);
        return String.Format("{0:HH:mm:ss.fff}", dt);
      }
    }

    /// <summary>
    /// The Duration of the File as timespan
    /// </summary>
    public TimeSpan DurationTimespan { get; set; }

    /// <summary>
    /// The Filesize in kb
    /// </summary>
    public string FileSize { get; set; }

    /// <summary>
    /// The Bitrate
    /// </summary>
    public string BitRate { get; set; }

    /// <summary>
    /// The Sample Rate
    /// </summary>
    public string SampleRate { get; set; }

    /// <summary>
    /// The number of Audio Channels
    /// </summary>
    public string Channels { get; set; }

    /// <summary>
    /// Version of the file
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// File Creation date
    /// </summary>
    public string CreationTime { get; set; }

    /// <summary>
    /// Last Write Date
    /// </summary>
    public string LastWriteTime { get; set; }

    #endregion
    #endregion

    #region Private Methods

    /// <summary>
    /// Returns the Value of the Frame with the specified Frame id
    /// </summary>
    /// <param name="frameId"></param>
    /// <returns></returns>
    private string GetFrame(string frameId)
    {
      int index = -1;
      if ((index = Frames.FindIndex((f => f.Id == frameId))) > -1)
      {
        return Frames[index].Value;
      }
      return "";
    }

    /// <summary>
    /// Returns the value of the frame with the specified frame id and value
    /// </summary>
    /// <param name="frameId"></param>
    /// <param name="frameValue"></param>
    /// <returns></returns>
    private string GetFrame(string frameId, string frameValue)
    {
      int index = -1;
      if ((index = Frames.FindIndex((f => f.Id == frameId && f.Value == frameValue))) > -1)
      {
        return Frames[index].Value;
      }
      return "";
    }

    private void SetText(string frameId, string text)
    {
      int index = -1;
      if ((index = Frames.FindIndex((f => f.Id == frameId))) > -1)
      {
        Frames[index].Value = text;
      }
      else
      {
        Frames.Add(new Frame(frameId, "", text));
      }
    }

    #endregion

  }
}