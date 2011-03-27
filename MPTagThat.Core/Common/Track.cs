using System;
using System.IO;
using System.Linq;
using MPTagThat.Core.Common;
using TagLib;
using TagLib.Id3v2;

namespace MPTagThat.Core
{
  public class Track
  {
    #region Variables

    private static readonly string[] _standardId3Frames = new string[] { "TPE1", "TPE2", "TALB", "TBPM", "COMM", "TCOM", "TPE3", "TCOP", "TPOS", "TCON", "TIT1", "USLT", "APIC", "POPM", "TIT2", "TRCK", "TYER" };
    private static readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

    #endregion

    #region Pubic Methods

    /// <summary>
    /// Read the Tags from the File 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static TrackData Create(string fileName)
    {
      TrackData track = new TrackData();
      TagLib.File file = null;
      bool error = false;

      try
      {
        TagLib.ByteVector.UseBrokenLatin1Behavior = true;
        file = TagLib.File.Create(fileName);
      }
      catch (CorruptFileException)
      {
        log.Warn("File Read: Ignoring track {0} - Corrupt File!", fileName);
        error = true;
      }
      catch (UnsupportedFormatException)
      {
        log.Warn("File Read: Ignoring track {0} - Unsupported format!", fileName);
        error = true;
      }
      catch (FileNotFoundException)
      {
        log.Warn("File Read: Ignoring track {0} - Physical file no longer existing!", fileName);
        error = true;
      }
      catch (Exception ex)
      {
        log.Error("File Read: Error processing file: {0} {1}", fileName, ex.Message);
        error = true;
      }

      if (error)
      {
        return null;
      }

      TagLib.Id3v2.Tag id3v2tag = null;
      if (file.MimeType.Substring(file.MimeType.IndexOf("/") + 1) == "mp3")
      {
        id3v2tag = file.GetTag(TagTypes.Id3v2, false) as TagLib.Id3v2.Tag;
      }

      #region Set Common Values
      
      track.Id = new Guid();
      track.FullFileName = fileName;
      track.FileName = Path.GetFileName(fileName);
      FileInfo fi = new FileInfo(fileName);
      track.Readonly = fi.IsReadOnly;
      track.TagType = file.MimeType.Substring(file.MimeType.IndexOf("/") + 1);
      #endregion

      #region Set Tags
      // Artist
      track.Artist = String.Join(";", file.Tag.Performers);
      if (track.Artist.Contains("AC;DC"))
      {
        track.Artist = track.Artist.Replace("AC;DC", "AC/DC");
      }

      track.AlbumArtist = String.Join(";", file.Tag.AlbumArtists);
      if (track.AlbumArtist.Contains("AC;DC"))
      {
        track.AlbumArtist = track.AlbumArtist.Replace("AC;DC", "AC/DC");
      }

      track.Album = file.Tag.Album ?? "";
      track.BPM = (int)file.Tag.BeatsPerMinute;
      track.Compilation = id3v2tag == null ? false : id3v2tag.IsCompilation;
      track.Composer = string.Join(";", file.Tag.Composers);
      track.Conductor = file.Tag.Conductor ?? "";
      track.Copyright = file.Tag.Copyright ?? "";

      track.DiscNumber = file.Tag.Disc;
      track.DiscCount = file.Tag.DiscCount;

      track.Genre = string.Join(";", file.Tag.Genres);
      track.Grouping = file.Tag.Grouping ?? "";
      track.Title = file.Tag.Title ?? "";

      track.TrackNumber = file.Tag.Track;
      track.TrackCount = file.Tag.TrackCount;
      track.Year = (int)file.Tag.Year;

      // Pictures
      foreach (IPicture picture in file.Tag.Pictures)
      {
        MPTagThat.Core.Common.Picture pic = new MPTagThat.Core.Common.Picture
        {
          Type = picture.Type,
          MimeType = picture.MimeType,
          Description = picture.Description
        };

        pic.Data = pic.ImageFromData(picture.Data.Data);
        track.Pictures.Add(pic);
      }

      // Comments
      if (track.TagType == "mp3" && id3v2tag != null)
      {
        foreach (CommentsFrame commentsframe in id3v2tag.GetFrames<CommentsFrame>())
        {
          track.ID3Comments.Add(new Comment(commentsframe.Description, commentsframe.Language, commentsframe.Text));
        }
      }
      else
      {
        track.Comment = file.Tag.Comment;
      }

      // Lyrics
      track.Lyrics = file.Tag.Lyrics;
      foreach (UnsynchronisedLyricsFrame lyricsframe in id3v2tag.GetFrames<UnsynchronisedLyricsFrame>())
      {
        track.LyricsFrames.Add(new Lyric(lyricsframe.Description, lyricsframe.Language, lyricsframe.Text));
      }

      // Rating
      track.Rating = 0;
      if (track.TagType == "mp3")
      {
        // First read in all POPM Frames found
        foreach (PopularimeterFrame popmframe in id3v2tag.GetFrames<PopularimeterFrame>())
        {
         track.Ratings.Add(new PopmFrame(popmframe.User, (int)popmframe.Rating, (int)popmframe.PlayCount));
        }

        TagLib.Id3v2.PopularimeterFrame popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(id3v2tag, "MPTagThat", false);
        if (popmFrame != null)
        {
          track.Rating = popmFrame.Rating;
        }
        else
        {
          // Now check for Ape Rating
          TagLib.Ape.Tag apetag = file.GetTag(TagTypes.Ape, true) as TagLib.Ape.Tag;
          TagLib.Ape.Item apeItem = apetag.GetItem("RATING");
          if (apeItem != null)
          {
            string rating = apeItem.ToString();
            try
            {
              track.Rating = Convert.ToInt32(rating);
            }
            catch (Exception)
            { }
          }
        }
      }
      else
      {
        if (track.TagType == "ape")
        {
          TagLib.Ape.Tag apetag = file.GetTag(TagTypes.Ape, true) as TagLib.Ape.Tag;
          TagLib.Ape.Item apeItem = apetag.GetItem("RATING");
          if (apeItem != null)
          {
            string rating = apeItem.ToString();
            try
            {
              track.Rating = Convert.ToInt32(rating);
            }
            catch (Exception)
            { }
          }
        }
      }

      #endregion


      #region Set Properties

      track.DurationTimespan = file.Properties.Duration;

      int fileLength = (int)(fi.Length / 1024);
      track.FileSize = fileLength.ToString();

      track.BitRate = file.Properties.AudioBitrate.ToString();
      track.SampleRate = file.Properties.AudioSampleRate.ToString();
      track.Channels = file.Properties.AudioChannels.ToString();
      track.Version = file.Properties.Description;
      track.CreationTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", fi.CreationTime);
      track.LastWriteTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", fi.LastWriteTime);

      #endregion

      // Now copy all Text frames of an ID3 V2

      if (track.TagType == "mp3" && id3v2tag != null)
      {
        foreach (TagLib.Id3v2.Frame frame in id3v2tag.GetFrames())
        {
          string id = frame.FrameId.ToString();
          if (!track.Frames.ContainsKey(id) && !_standardId3Frames.Contains(id))
          {
            track.Frames.Add(id, frame.ToString());
          }
        }

        track.ID3Version = id3v2tag.Version;
      }

      return track;
    }

    /// <summary>
    /// Clear all the tags
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public static TrackData ClearTag(TrackData track)
    {
      track.Artist = "";
      track.AlbumArtist = "";
      track.Album = "";
      track.BPM = 0;
      track.ID3Comments.Clear();
      track.Frames.Clear();
      track.Compilation = false;
      track.Composer = "";
      track.Conductor = "";
      track.Copyright = "";

      track.DiscNumber = 0;
      track.DiscCount = 0;

      track.Genre = "";
      track.Grouping = "";
      track.LyricsFrames.Clear();
      track.Pictures.Clear();
      track.Title = "";
      track.TrackNumber = 0;
      track.TrackCount = 0;
      track.Year = 0;
      track.Ratings.Clear();
      return track;
    }

    /// <summary>
    /// Save the Modified file
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public static bool SaveFile(TrackData track)
    {
      if (!track.Changed)
      {
        return true;
      }

      TagLib.File file = null;
      bool error = false;
      try
      {
        TagLib.ByteVector.UseBrokenLatin1Behavior = true;
        file = TagLib.File.Create(track.FullFileName);
      }
      catch (CorruptFileException)
      {
        log.Warn("File Read: Ignoring track {0} - Corrupt File!", track.FullFileName);
        error = true;
      }
      catch (UnsupportedFormatException)
      {
        log.Warn("File Read: Ignoring track {0} - Unsupported format!", track.FullFileName);
        error = true;
      }
      catch (FileNotFoundException)
      {
        log.Warn("File Read: Ignoring track {0} - Physical file no longer existing!", track.FullFileName);
        error = true;
      }
      catch (Exception ex)
      {
        log.Error("File Read: Error processing file: {0} {1}", track.FullFileName, ex.Message);
        error = true;
      }


      // Remove Tags first!!!!
      // Remove Comments first!!!!

      /*
       bool commentRemoved = false;
        if (track.TagType.ToLower() == "mp3")
        {
          Tag id3v1tag = track.File.GetTag(TagTypes.Id3v1, true) as Tag;
          TagLib.Id3v2.Tag id3v2tag = track.File.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;

          if (id3v1tag.Comment != null)
          {
            track.Comment = "";
            id3v1tag.Comment = null;
            commentRemoved = true;
          }
          IEnumerator<CommentsFrame> id3v2comments = id3v2tag.GetFrames<CommentsFrame>().GetEnumerator();
          if (id3v2comments.MoveNext())
          {
            track.Comment = "";
            id3v2tag.RemoveFrames("COMM");
            commentRemoved = true;
          }
        }
        else
        {
          if (track.Comment != "")
          {
            commentRemoved = true;
            track.Comment = "";
          }
        }

        if (commentRemoved)
        {
          SetBackgroundColorChanged(row.Index);
          track.Changed = true;
          _itemsChanged = true;
        }
      */
      // Check for renamed file ???
      
      // Involved People - Look at Single Tagedit Apply
      
      // Lyrics

      // Ratings

      return error;
    }

    #endregion
  }
}
