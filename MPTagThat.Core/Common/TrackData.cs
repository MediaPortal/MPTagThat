using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TagLib;

namespace MPTagThat.Core
{
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
    private bool _changed;
    private MP3Error _mp3ValError;
    private TagLib.File _file;
    private FileInfo _fi;
    private string _fileName;
    private int _numTrackDigits = Options.MainSettings.NumberTrackDigits;
    private TagLib.Id3v1.Tag id3v1tag = null;
    private TagLib.Id3v2.Tag id3v2tag = null;
    private TagLib.Ape.Tag apetag = null;
    #endregion

    #region ctor
    public TrackData(TagLib.File file)
    {
      _changed = false;
      _mp3ValError = MP3Error.NoError;
      _file = file;
      _fi = new FileInfo(file.Name);
      _fileName = Path.GetFileName(file.Name);

      //if (_file.MimeType.Substring(_file.MimeType.IndexOf("/") + 1).ToLower() == "mp3")
      if (TagType.ToLower() == "mp3")
      {
        id3v1tag = _file.GetTag(TagTypes.Id3v1, true) as TagLib.Id3v1.Tag;
        id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;
        apetag = _file.GetTag(TagTypes.Ape, true) as TagLib.Ape.Tag;
      }
    }
    #endregion

    #region Properties
    #region Common Properties
    /// <summary>
    /// Has the Track been changed
    /// </summary>
    public bool Changed
    {
      get { return _changed; }
      set { _changed = value; }
    }

    /// <summary>
    /// The TagLib File instance
    /// </summary>
    public TagLib.File File
    {
      get { return _file; }
      set { _file = value; }
    }

    /// <summary>
    /// The Full Filename including the path
    /// </summary>
    public string FullFileName
    {
      get { return _file.Name; }
    }

    /// <summary>
    /// Filename without Path
    /// </summary>
    public string FileName
    {
      get { return _fileName; }
      set { _fileName = value; }
    }

    /// <summary>
    /// The Tag Type
    /// </summary>
    public string TagType
    {
      get { return _file.MimeType.Substring(_file.MimeType.IndexOf("/") + 1); }
    }

    /// <summary>
    /// Number of Pictures in File
    /// </summary>
    public int NumPics
    {
      get { return _file.Tag.Pictures.Length; }
    }

    /// <summary>
    /// Has the Track fixable errors?
    /// </summary>
    public MP3Error MP3ValidationError
    {
      get { return _mp3ValError; }
      set { _mp3ValError = value; }
    }
    #endregion

    #region Tags
    /// <summary>
    /// Artist / Performer Tag
    /// ID3: TPE1
    /// </summary>
    public string Artist
    {
      get 
      {
        string artist = String.Join(";", File.Tag.Performers);
        if (artist.Contains("AC;DC"))
          artist = artist.Replace("AC;DC", "AC/DC");

        return artist; 
      }
      set
      {
        string[] artists = value.Split(new char[] { ';', '|' });
        _file.Tag.Performers = artists;
      }
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
      get 
      {
        string albumartist = String.Join(";", File.Tag.AlbumArtists);
        if (albumartist.Contains("AC;DC"))
          albumartist = albumartist.Replace("AC;DC", "AC/DC");

        return albumartist; 
      }
      set
      {
        string[] artists = new string[] { };
        if (value != null)
          artists = value.Split(new char[] { ';', '|' });
        _file.Tag.AlbumArtists = artists;
      }
    }

    /// <summary>
    /// ALbum Tag
    /// ID3: TALB
    /// </summary>
    public string Album
    {
      get { return _file.Tag.Album == null ? "" : _file.Tag.Album; }
      set { _file.Tag.Album = value; }
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
    public int BPM
    {
      get { return (int)_file.Tag.BeatsPerMinute; }
      set { _file.Tag.BeatsPerMinute = Convert.ToUInt32(value); }
    }

    /// <summary>
    /// Comment Tag
    /// ID3: COMM
    /// </summary>
    public string Comment
    {
      get { return _file.Tag.Comment == null ? "" : _file.Tag.Comment; }
      set { _file.Tag.Comment = value; }
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

    /// <summary>
    /// Composert Tag
    /// ID3: TCOM
    /// </summary>
    public string Composer
    {
      get { return String.Join(";", File.Tag.Composers); }
      set
      {
        string[] composers = value.Split(new char[] { ';', '|' });
        _file.Tag.Composers = composers;
      }
    }

    /// <summary>
    /// Conductor Tag
    /// ID3: TPE3
    /// </summary>
    public string Conductor
    {
      get { return _file.Tag.Conductor == null ? "" : _file.Tag.Conductor; }
      set { _file.Tag.Conductor = value; }
    }

    /// <summary>
    /// Copyright Tag
    /// ID3: TCOP
    /// </summary>
    public string Copyright
    {
      get { return _file.Tag.Copyright == null ? "" : _file.Tag.Copyright; }
      set { _file.Tag.Copyright = value; }
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
        string disc = _file.Tag.Disc > 0 ? _file.Tag.Disc.ToString().PadLeft(_numTrackDigits, '0') : "";
        return _file.Tag.DiscCount > 0 ? String.Format("{0}/{1}", disc, _file.Tag.DiscCount.ToString().PadLeft(_numTrackDigits, '0')) : disc;
      }

      set
      {
        string[] disc = value.Split('/');
        try
        {
          if (disc[0] != "")
            _file.Tag.Disc = Convert.ToUInt32(disc[0]);
        }
        catch (Exception) { }

        try
        {
          if (disc[1] != "")
            _file.Tag.DiscCount = Convert.ToUInt32(disc[1]);
        }
        catch (Exception) { }
      }
    }

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
      get { return String.Join(";", File.Tag.Genres); }
      set
      {
        string[] genres = value.Split(new char[] { ';', '|' });
        _file.Tag.Genres = genres;
      }
    }

    /// <summary>
    /// Content Group  Tag
    /// ID3: TIT1
    /// </summary>
    public string Grouping
    {
      get { return _file.Tag.Grouping == null ? "" : _file.Tag.Grouping; }
      set { _file.Tag.Grouping = value; }
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

        if (id3v2tag == null)
          id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

        if (id3v2tag.Version == 4)
          return GetFrame("TIPL");

        return GetFrame("IPLS");
      }
      set
      {
        if (TagType != "mp3")
          return;

        if (id3v2tag == null)
          id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

        if (id3v2tag.Version == 4)
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
      get { return _file.Tag.Lyrics; }
      set { _file.Tag.Lyrics = value; }
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
    /// </summary>
    public string OriginalRelease
    {
      get
      {
        if (TagType != "mp3")
          return "";

        if (id3v2tag == null)
          id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

        if (id3v2tag.Version == 4)
          return GetFrame("TDOR");

        return GetFrame("TORY");
      }
      set
      {
        if (TagType != "mp3")
          return;

        if (id3v2tag == null)
          id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

        if (id3v2tag.Version == 4)
          SetText("TDOR", value);

        string year = value;
        if (year.Length > 4)
          year = year.Substring(0, 4);

        SetText("TORY", value);
      }
    }


    public IPicture[] Pictures
    {
      get { return _file.Tag.Pictures; }
      set { _file.Tag.Pictures = value; }
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
        if (TagType == "mp3")
        {
          if (id3v2tag == null)
            id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

          TagLib.Id3v2.PopularimeterFrame popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(id3v2tag, "MPTagThat", false);
          if (popmFrame != null)
            return popmFrame.Rating;

          // Now check for Ape Rating
          TagLib.Ape.Item apeItem = apetag.GetItem("RATING");
          if (apeItem != null)
          {
            string rating = apeItem.ToString();
            try
            {
              return Convert.ToInt32(rating);
            }
            catch (Exception)
            {
              return 0;
            }
          }
        }
        else
        {
          if (TagType == "ape")
          {
            TagLib.Ape.Tag apetag = _file.GetTag(TagTypes.Ape, true) as TagLib.Ape.Tag;
            TagLib.Ape.Item apeItem = apetag.GetItem("RATING");
            if (apeItem != null)
            {
              string rating = apeItem.ToString();
              try
              {
                return Convert.ToInt32(rating);
              }
              catch (Exception)
              {
                return 0;
              }
            }
          }
        }
        return 0;
      }
      set
      {
        if (TagType == "mp3")
        {
          if (id3v2tag == null)
            id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

          // Set ID3 V2
          TagLib.Id3v2.PopularimeterFrame popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(id3v2tag, "MPTagThat", true);
          popmFrame.Rating = Convert.ToByte(value);
          popmFrame.PlayCount = Convert.ToUInt32(0);

          // Set Ape Tag
          apetag.SetItem(new TagLib.Ape.Item("RATING", value.ToString()));

        }
        else if (TagType == "ape")
        {
          TagLib.Ape.Tag apetag = _file.GetTag(TagTypes.Ape, true) as TagLib.Ape.Tag;
          apetag.SetItem(new TagLib.Ape.Item("RATING", value.ToString()));
        } 
      }
    }

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
      get { return _file.Tag.Title == null ? "" : _file.Tag.Title; }
      set { _file.Tag.Title = value; }
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
        string track = _file.Tag.Track > 0 ? _file.Tag.Track.ToString().PadLeft(_numTrackDigits, '0') : "";
        return _file.Tag.TrackCount > 0 ? String.Format("{0}/{1}", track, _file.Tag.TrackCount.ToString().PadLeft(_numTrackDigits, '0')) : track;
      }

      set
      {
        string[] track = value.Split('/');
        try
        {
          if (track[0] != "")
            _file.Tag.Track = Convert.ToUInt32(track[0]);
          else
            _file.Tag.Track = 0;
        }
        catch (Exception) { }

        try
        {
          if (track.Length > 1 && track[1] != "")
            _file.Tag.TrackCount = Convert.ToUInt32(track[1]);
          else
            _file.Tag.TrackCount = 0;
        }
        catch (Exception) { }
      }
    }

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
    public int Year
    {
      get { return (int)_file.Tag.Year; }
      set { _file.Tag.Year = Convert.ToUInt32(value); }
    }
    #endregion

    #region Audio File Properties
    /// <summary>
    /// The Duration of the File
    /// </summary>
    public string Duration
    {
      get
      {
        TimeSpan ts = _file.Properties.Duration;
        DateTime dt = new DateTime(ts.Ticks);
        return String.Format("{0:HH:mm:ss.fff}", dt);
      }
    }

    /// <summary>
    /// The Filesize in kb
    /// </summary>
    public string FileSize
    {
      get
      {
        FileInfo fi = new FileInfo(_file.Name);
        int fileLength = (int)(fi.Length / 1024);
        return fileLength.ToString();
      }
    }

    /// <summary>
    /// The Bitrate
    /// </summary>
    public string BitRate
    {
      get { return _file.Properties.AudioBitrate.ToString(); }
    }

    /// <summary>
    /// The Sample Rate
    /// </summary>
    public string SampleRate
    {
      get { return _file.Properties.AudioSampleRate.ToString(); }
    }

    /// <summary>
    /// The number of Audio Channels
    /// </summary>
    public string Channels
    {
      get { return _file.Properties.AudioChannels.ToString(); }
    }

    /// <summary>
    /// Version of the file
    /// </summary>
    public string Version
    {
      get { return _file.Properties.Description; }
    }

    /// <summary>
    /// File Creation date
    /// </summary>
    public string CreationTime
    {
      get { return String.Format("{0:yyyy-MM-dd HH:mm:ss}", _fi.CreationTime); }
      set { }
    }

    /// <summary>
    /// Last Write Date
    /// </summary>
    public string LastWriteTime
    {
      get { return String.Format("{0:yyyy-MM-dd HH:mm:ss}", _fi.LastWriteTime); }
      set { }
    }
    #endregion
    #endregion

    #region Private Methods
    private string GetFrame(string frameID)
    {
      if (TagType != "mp3")
        return "";

      if (id3v2tag == null)
        id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

      foreach (TagLib.Id3v2.Frame frame in id3v2tag.GetFrames(frameID))
      {
        if (frameID.StartsWith("W"))
        {
          return (frame as TagLib.Id3v2.UrlLinkFrame).ToString();
        }
        else
          return (frame as TagLib.Id3v2.TextInformationFrame).ToString();
      }
      return "";
    }

    private void SetText(string frameID, string text)
    {
      if (TagType != "mp3")
        return;

      if (id3v2tag == null)
        id3v2tag = _file.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

      if (frameID.StartsWith("W"))
      {
        TagLib.Id3v2.UrlLinkFrame urlLinkFrame = TagLib.Id3v2.UrlLinkFrame.Get(id3v2tag, frameID, true);
        urlLinkFrame.Text = new string[] { text };
      }
      else
        id3v2tag.SetTextFrame(frameID, text);
    }
    #endregion
  }
}
