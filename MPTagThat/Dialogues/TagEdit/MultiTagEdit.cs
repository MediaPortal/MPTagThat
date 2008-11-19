using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TagLib;
using MPTagThat.Core;
using MPTagThat.Core.Amazon;

namespace MPTagThat.TagEdit
{
  public partial class MultiTagEdit : TagEditBase
  {
    #region ctor
    public MultiTagEdit(Main main)
    {
      this.main = main;
      InitializeComponent();
    }
    #endregion

    #region Methods
    #region Form Opening
    /// <summary>
    /// Called when loading the Dialoue
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnLoad(object sender, EventArgs e)
    {
      Util.EnterMethod(Util.GetCallingMethod());

      base.OnLoad(sender, e);

      // Fill in Fields, which are the same in all selected rows
      FillForm();

      // Now Set the Event Handlers to detect changes in the Text Boxes
      this.tbYear.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbTitle.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbAlbum.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbArtist.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbTrack.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbAlbumArtist.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbNumTracks.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbBPM.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbNumDiscs.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbDisc.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbConductor.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbComposer.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbInterpretedBy.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbTextWriter.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbPublisher.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbEncodedBy.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbCopyright.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbContentGroup.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbSubTitle.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbArtistSort.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbAlbumSort.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbTitleSort.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.cbMediaType.TextChanged += new System.EventHandler(this.OnComboChanged);
      this.tbOfficialArtistUrl.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbOfficialAudioFileUrl.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbOfficialAudioSourceUrl.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbOfficialInternetRadioUrl.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbOfficialPaymentUrl.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbOfficialPublisherUrl.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbCommercialInformationUrl.TextChanged += new System.EventHandler(this.OnTextChanged);
      this.tbCopyrightUrl.TextChanged += new System.EventHandler(this.OnTextChanged);

      Localisation();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    private void Localisation()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Text = localisation.ToString("TagEdit", "MultiHeading");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    /// <summary>
    /// Apply the changes from the Multi Tag edit
    /// </summary>
    /// <param name="options"></param>
    private void MultiTagEditApply(MultiTagEditOptions options)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      bool bErrors = false;
      DataGridView tracksGrid = main.TracksGridView.View;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        try
        {
          main.TracksGridView.Changed = true;
          main.TracksGridView.SetBackgroundColorChanged(row.Index);
          TrackData track = main.TracksGridView.TrackList[row.Index];
          track.Changed = true;

          // Get the ID3 Frame for ID3 specifc frame handling
          TagLib.Id3v1.Tag id3v1tag = null;
          TagLib.Id3v2.Tag id3v2tag = null;
          if (track.TagType.ToLower() == "mp3")
          {
            id3v1tag = track.File.GetTag(TagTypes.Id3v1, true) as TagLib.Id3v1.Tag;
            id3v2tag = track.File.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;
          }

          #region Main Tags
          if (options.Track > -1 || options.NumTracks > -1)
            track.Track = string.Format("{0}/{1}", options.Track.ToString(), options.NumTracks.ToString());

          if (options.Disc > -1 || options.NumDiscs > -1)
            track.Disc = string.Format("{0}/{1}", options.Disc.ToString(), options.NumDiscs.ToString());

          if (options.BPM > -1)
            track.File.Tag.BeatsPerMinute = (uint)options.BPM;

          if (options.Artist != null)
          {
            track.Artist = options.Artist;
          }

          if (options.AlbumArtist != null)
          {
            track.AlbumArtist = options.AlbumArtist;
          }

          if (options.Album != null)
          {
            track.Album = options.Album;
          }

          if (options.Title != null)
          {
            track.Title = options.Title;
          }

          if (options.Year > -1)
          {
            track.Year = options.Year;
          }

          if (options.Genre != null)
          {
            track.Genre = options.Genre;
          }

          if (options.RemoveExistingComments)
            track.Comment = "";

          if (options.Comments.Count > 0)
          {

            if (track.TagType.ToLower() == "mp3")
            {
              id3v1tag.Comment = options.Comments[0].Text;
              foreach (Comment comment in options.Comments)
              {
                TagLib.Id3v2.CommentsFrame commentsframe = TagLib.Id3v2.CommentsFrame.Get(id3v2tag, comment.Description, comment.Language, true);
                commentsframe.Text = comment.Text;
              }
            }
            else
              track.Comment = options.Comments[0].Text;
          }
          #endregion

          #region Detailed Information
          if (options.Conductor != null)
            track.Conductor = options.Conductor;

          if (options.Composer != null)
            track.Composer = options.Composer;

          if (options.Copyright != null)
            track.Copyright = options.Copyright;

          if (options.ContentGroup != null)
            track.Grouping = options.ContentGroup;

          // The following values are only ID3 V2 specific
          if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
          {
            if (options.Interpreter != null)
              track.Interpreter = options.Interpreter;

            if (options.TextWriter != null)
              track.TextWriter = options.TextWriter;

            if (options.Publisher != null)
              track.Publisher = options.Publisher;

            if (options.EncodedBy != null)
              track.EncodedBy = options.EncodedBy;

            if (options.SubTitle != null)
              track.SubTitle = options.SubTitle;

            if (options.MediaType != null)
              track.MediaType = options.MediaType;

            if (options.SetTrackLength)
              track.TrackLength = Convert.ToString(track.File.Properties.Duration.TotalMilliseconds);

            // V 2.4 Frames only
            if (Options.MainSettings.ID3V2Version == 4)
            {
              if (options.ArtistSortName != null)
                track.ArtistSortName = options.ArtistSortName;

              if (options.AlbumSortName != null)
                track.AlbumSortName = options.AlbumSortName;

              if (options.TitleSortName != null)
                track.TitleSortName = options.TitleSortName;
            }
          }
          #endregion

          #region Original Information
          if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
          {
            if (options.OriginalAlbum != null)
              track.OriginalAlbum = options.OriginalAlbum;

            if (options.OriginalArtist != null)
              track.OriginalArtist = options.OriginalArtist;

            if (options.OriginalFileName != null)
              track.OriginalFileName = options.OriginalFileName;

            if (options.OriginalLyricsWriter != null)
              track.OriginalLyricsWriter = options.OriginalLyricsWriter;

            if (options.OriginalOwner != null)
              track.OriginalOwner = options.OriginalOwner;

            if (options.OriginalRelease != null)
              track.OriginalRelease = options.OriginalRelease;
          }
          #endregion

          #region Pictures
          if (options.Pictures.Count > 0 || options.RemoveExistingPictures)
          {
            List<IPicture> pics = new List<IPicture>();
            if (!options.RemoveExistingPictures)
            {
              pics = new List<IPicture>(track.Pictures);
            }

            foreach (IPicture pic in options.Pictures)
              pics.Add(pic);

            track.Pictures = pics.ToArray();
          }
          #endregion

          #region Involved People
          // The following values are only ID3 V2 specific
          if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
          {
            if (options.InvolvedPeople != null)
              track.InvolvedPeople = options.InvolvedPeople;

            // remove frames, which may have been left from a previous save in a different version
            if (Options.MainSettings.ID3V2Version == 4)
              id3v2tag.RemoveFrames("IPLS");
            else
              id3v2tag.RemoveFrames("TIPL");

            // V 2.4 Frames only
            if (Options.MainSettings.ID3V2Version == 4)
            {
              if (options.MusicCreditList != null)
                track.MusicCreditList = options.MusicCreditList;
            }
            else
              id3v2tag.RemoveFrames("TMCL");  // remove the frame, when 2.3. 
          }
          #endregion

          #region Web Information
          if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
          {
            if (options.CopyrightInformation != null)
              track.CopyrightInformation = options.CopyrightInformation;

            if (options.OfficialAudioFileUrl != null)
              track.OfficialAudioFileInformation = options.OfficialAudioFileUrl;

            if (options.OfficialArtistUrl != null)
              track.OfficialArtistInformation = options.OfficialArtistUrl;

            if (options.OfficialAudioSourceUrl != null)
              track.OfficialAudioSourceInformation = options.OfficialAudioSourceUrl;

            if (options.OfficialInternetRadioUrl != null)
              track.OfficialInternetRadioInformation = options.OfficialInternetRadioUrl;

            if (options.OfficialPaymentUrl != null)
              track.OfficialPaymentInformation = options.OfficialPaymentUrl;

            if (options.OfficialPublisherUrl != null)
              track.OfficialPublisherInformation = options.OfficialPublisherUrl;

            if (options.CommercialInformation != null)
              track.CommercialInformation = options.CommercialInformation;
          }
          #endregion

          #region Lyrics
          if (options.RemoveExistingLyrics)
            track.Lyrics = "";

          if (options.Lyrics.Count > 0)
          {
            if (track.TagType.ToLower() == "mp3")
            {
              foreach (Lyric lyric in options.Lyrics)
              {
                TagLib.Id3v2.UnsynchronisedLyricsFrame lyricsframe = TagLib.Id3v2.UnsynchronisedLyricsFrame.Get(id3v2tag, lyric.Description, lyric.Language, true);
                lyricsframe.Text = lyric.Text;
              }
            }
            else
              track.Lyrics = options.Lyrics[0].Text;

          }
          #endregion

          #region Rating
          // The following values are only ID3 V2 specific
          if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
          {
            if (options.RemoveExistingLyrics)
              id3v2tag.RemoveFrames("POPM");

            if (options.Rating.Count > 0)
            {
              foreach (Rating rating in options.Rating)
              {
                TagLib.Id3v2.PopularimeterFrame popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(id3v2tag, rating.User, true);
                popmFrame.Rating = Convert.ToByte(rating.RatingValue);
                popmFrame.PlayCount = Convert.ToUInt32(rating.PlayCounter);
              }
            }
          }
          #endregion
        }
        catch (Exception ex)
        {
          log.Error("Error applying changes from MultiTagedit: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[1].Value = localisation.ToString("message", "Error");
          main.TracksGridView.AddErrorMessage(main.TracksGridView.TrackList[row.Index].File.Name, ex.Message);
          bErrors = true;
        }
      }

      main.TracksGridView.Changed = bErrors;
      // check, if we still have changed items in the list
      foreach (TrackData track in main.TracksGridView.TrackList)
      {
        if (track.Changed)
          main.TracksGridView.Changed = true;
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Set the options selected in the Form
    /// </summary>
    /// <returns></returns>
    private MultiTagEditOptions SetOptions()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      MultiTagEditOptions options = new MultiTagEditOptions();
      #region Main Tags
      if (ckTrack.Checked)
      {
        try
        {
          options.Track = Int32.Parse(tbTrack.Text.Trim());
        }
        catch (Exception)
        {
          options.Track = -1;
        }

        try
        {
          options.NumTracks = Int32.Parse(tbNumTracks.Text.Trim());
        }
        catch (Exception)
        {
          options.NumTracks = -1;
        }
      }

      if (ckDisk.Checked)
      {
        try
        {
          options.Disc = Int32.Parse(tbDisc.Text.Trim());
        }
        catch (Exception)
        {
          options.Disc = -1;
        }

        try
        {
          options.NumDiscs = Int32.Parse(tbNumDiscs.Text.Trim());
        }
        catch (Exception)
        {
          options.NumDiscs = -1;
        }
      }

      if (ckBPM.Checked)
      {
        try
        {
          options.BPM = Int32.Parse(tbBPM.Text.Trim());
        }
        catch (Exception)
        {
          options.BPM = -1;
        }
      }

      try
      {
        options.Year = (ckYear.Checked ? Int32.Parse(tbYear.Text) : -1);
      }
      catch (Exception)
      {
        options.Year = -1;
      }

      if (ckGenre.Checked)
      {
        int i = 0;
        foreach (string item in listBoxGenre.Items)
        {
          if (i == 0)
            options.Genre += item;
          else
            options.Genre += ";" + item;
          i++;
        }

        // We have removed all Genres, without setting a bew one
        if (options.Genre == null)
          options.Genre = "";
      }
      else
        options.Genre = null;

      options.Artist = (ckArtist.Checked ? tbArtist.Text : null);
      options.AlbumArtist = (ckAlbumArtist.Checked ? tbAlbumArtist.Text : null);
      options.Album = (ckAlbum.Checked ? tbAlbum.Text : null);
      options.Title = (ckTitle.Checked ? tbTitle.Text : null);


      List<Comment> comments = new List<Comment>();
      foreach (DataGridViewRow row in dataGridViewComment.Rows)
      {
        Comment comment = new Comment(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
        comments.Add(comment);
      }
      options.Comments = comments;
      options.RemoveExistingComments = checkBoxRemoveComments.Checked;
      #endregion

      #region Detailed Information
      options.Conductor = (ckConductor.Checked ? tbConductor.Text : null);
      options.Composer = (ckComposer.Checked ? tbComposer.Text : null);
      options.Copyright = (ckCopyright.Checked ? tbCopyright.Text : null);
      options.Interpreter = (ckInterpretedBy.Checked ? tbInterpretedBy.Text : null);
      options.TextWriter = (ckTextWriter.Checked ? tbTextWriter.Text : null);
      options.Publisher = (ckPublisher.Checked ? tbPublisher.Text : null);
      options.EncodedBy = (ckEncodedBy.Checked ? tbEncodedBy.Text : null);
      options.ContentGroup = (ckContentGroup.Checked ? tbContentGroup.Text : null);
      options.SubTitle = (ckSubTitle.Checked ? tbSubTitle.Text : null);
      options.ArtistSortName = (ckArtistSort.Checked ? tbArtistSort.Text : null);
      options.AlbumSortName = (ckAlbumSort.Checked ? tbAlbumSort.Text : null);
      options.TitleSortName = (ckTitleSort.Checked ? tbTitleSort.Text : null);
      options.MediaType = (ckMediaType.Checked ? cbMediaType.SelectedText : null);
      options.SetTrackLength = ckTrackLength.Checked;
      #endregion

      #region Original Information
      options.OriginalAlbum = (ckOriginalAlbum.Checked ? tbOriginalAlbum.Text : null);
      options.OriginalArtist = (ckOriginalArtist.Checked ? tbOriginalArtist.Text : null);
      options.OriginalFileName = (ckOriginalFileName.Checked ? tbOriginalFileName.Text : null);
      options.OriginalLyricsWriter = (ckOriginalLyricsWriter.Checked ? tbOriginalLyricsWriter.Text : null);
      options.OriginalOwner = (ckOriginalOwner.Checked ? tbOriginalOwner.Text : null);
      options.OriginalRelease = (ckOriginalRelease.Checked ? tbOriginalRelease.Text : null);
      #endregion

      #region Pictures
      options.Pictures = _pictures;
      options.RemoveExistingPictures = checkBoxRemoveExistingPictures.Checked;
      #endregion

      #region Involved People
      char[] d = new char[1] { '\0' };
      string delim = new string(d);
      foreach (DataGridViewRow row in dataGridViewInvolvedPeople.Rows)
      {
        options.InvolvedPeople += string.Format(@"{0}{1}{2}{3}", row.Cells[0].Value.ToString(), delim, row.Cells[1].Value.ToString(), delim);
      }

      if (options.InvolvedPeople != null)
        options.InvolvedPeople.Trim(new char[] { '\0' });

      foreach (DataGridViewRow row in dataGridViewMusician.Rows)
      {
        options.MusicCreditList += string.Format(@"{0}{1}{2}{3}", row.Cells[0].Value.ToString(), delim, row.Cells[1].Value.ToString(), delim);
      }

      if (options.MusicCreditList != null)
        options.MusicCreditList.Trim(new char[] { '\0' });
      #endregion

      #region Web Information
      options.CopyrightInformation = (ckCopyrightUrl.Checked ? tbCopyrightUrl.Text : null);
      options.OfficialAudioFileUrl = (ckOfficialAudioFileUrl.Checked ? tbOfficialAudioFileUrl.Text : null);
      options.OfficialArtistUrl = (ckOfficialArtistUrl.Checked ? tbOfficialArtistUrl.Text : null);
      options.OfficialAudioSourceUrl = (ckOfficialAudioSourceUrl.Checked ? tbOfficialAudioSourceUrl.Text : null);
      options.OfficialInternetRadioUrl = (ckOfficialInternetRadioUrl.Checked ? tbOfficialInternetRadioUrl.Text : null);
      options.OfficialPaymentUrl = (ckOfficialPaymentUrl.Checked ? tbOfficialPaymentUrl.Text : null);
      options.OfficialPublisherUrl = (ckOfficialPublisherUrl.Checked ? tbOfficialPublisherUrl.Text : null);
      options.CommercialInformation = (ckCommercialInformationUrl.Checked ? tbCommercialInformationUrl.Text : null);
      #endregion

      #region Lyrics
      List<Lyric> lyrics = new List<Lyric>();
      foreach (DataGridViewRow row in dataGridViewLyrics.Rows)
      {
        Lyric lyric = new Lyric(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
        lyrics.Add(lyric);
      }
      options.Lyrics = lyrics;
      options.RemoveExistingLyrics = ckRemoveLyrics.Checked;
      #endregion

      #region Ratings
      List<Rating> ratings = new List<Rating>();
      foreach (DataGridViewRow row in dataGridViewRating.Rows)
      {
        Rating rating = new Rating(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
        ratings.Add(rating);
      }
      options.Rating = ratings;
      options.RemoveExistingRating = ckRemoveExistingRatings.Checked;
      #endregion

      Util.LeaveMethod(Util.GetCallingMethod());
      return options;
    }

    /// <summary>
    /// Fill the fields in the Form with Values which are the same in all selected rows
    /// </summary>
    private void FillForm()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      int i = 0;
      string strGenreTemp = "";
      string strInvoledPeopleTemp = "";
      string strMusicianCreditList = "";

      foreach (DataGridViewRow row in main.TracksGridView.View.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = main.TracksGridView.TrackList[row.Index];

        #region Main Tags
        if (tbArtist.Text.Trim() != track.Artist.Trim())
          if (i == 0)
            tbArtist.Text = track.Artist;
          else
            tbArtist.Text = "";

        if (tbAlbumArtist.Text.Trim() != track.AlbumArtist.Trim())
          if (i == 0)
            tbAlbumArtist.Text = track.AlbumArtist;
          else
            tbAlbumArtist.Text = "";

        if (tbAlbum.Text.Trim() != track.Album.Trim())
          if (i == 0)
            tbAlbum.Text = track.Album;
          else
            tbAlbum.Text = "";

        if (tbTitle.Text.Trim() != track.Title.Trim())
          if (i == 0)
            tbTitle.Text = track.Title;
          else
            tbTitle.Text = "";

        if (tbYear.Text.Trim() != track.Year.ToString())
          if (i == 0 && track.Year > 0)
            tbYear.Text = track.Year.ToString();
          else
            tbYear.Text = "";

        if (tbBPM.Text.Trim() != track.BPM.ToString())
          if (i == 0 && track.BPM > 0)
            tbBPM.Text = track.BPM.ToString();
          else
            tbBPM.Text = "";

        string[] disc = track.Disc.Split('/');
        if (tbDisc.Text.Trim() != disc[0])
        {
          if (i == 0 && disc[0] != "")
            tbDisc.Text = disc[0];
          else
            tbDisc.Text = "";
        }
        if (disc.Length > 1)
        {
          if (tbNumDiscs.Text.Trim() != disc[1])
          {
            if (i == 0 && disc[1] != "")
              tbNumDiscs.Text = disc[1];
            else
              tbNumDiscs.Text = "";
          }
        }

        string[] tracks = track.Track.Split('/');
        if (tracks.Length > 1)
        {
          if (tbNumTracks.Text.Trim() != tracks[1])
          {
            if (i == 0 && tracks[1] != "")
              tbNumTracks.Text = tracks[1];
            else
              tbNumTracks.Text = "";
          }
        }

        if (strGenreTemp != track.Genre)
        {
          if (i == 0)
          {
            listBoxGenre.Items.AddRange(track.Genre.Split(';'));
            strGenreTemp = track.Genre;
          }
          else
            listBoxGenre.Items.Clear();
        }
        #endregion

        #region Detailed Information
        if (tbConductor.Text.Trim() != track.Conductor.Trim())
          if (i == 0)
            tbConductor.Text = track.Conductor;
          else
            tbConductor.Text = "";

        if (tbComposer.Text.Trim() != track.Composer.Trim())
          if (i == 0)
            tbComposer.Text = track.Composer;
          else
            tbComposer.Text = "";

        if (tbCopyright.Text.Trim() != track.Copyright.Trim())
          if (i == 0)
            tbCopyright.Text = track.Copyright;
          else
            tbCopyright.Text = "";

        if (tbContentGroup.Text.Trim() != track.Grouping.Trim())
          if (i == 0)
            tbContentGroup.Text = track.Grouping;
          else
            tbContentGroup.Text = "";

        // The following values are only ID3 V2 specific
        if (track.TagType.ToLower() == "mp3")
        {
          if (tbInterpretedBy.Text.Trim() != track.Interpreter.Trim())
            if (i == 0)
              tbInterpretedBy.Text = track.Interpreter;
            else
              tbInterpretedBy.Text = "";

          if (tbTextWriter.Text.Trim() != track.TextWriter.Trim())
            if (i == 0)
              tbTextWriter.Text = track.TextWriter;
            else
              tbTextWriter.Text = "";

          if (tbPublisher.Text.Trim() != track.Publisher.Trim())
            if (i == 0)
              tbPublisher.Text = track.Publisher;
            else
              tbPublisher.Text = "";

          if (tbEncodedBy.Text.Trim() != track.EncodedBy.Trim())
            if (i == 0)
              tbEncodedBy.Text = track.EncodedBy;
            else
              tbEncodedBy.Text = "";

          if (tbSubTitle.Text.Trim() != track.SubTitle.Trim())
            if (i == 0)
              tbSubTitle.Text = track.SubTitle;
            else
              tbSubTitle.Text = "";

          if (tbArtistSort.Text.Trim() != track.ArtistSortName.Trim())
            if (i == 0)
              tbArtistSort.Text = track.ArtistSortName;
            else
              tbArtistSort.Text = "";

          if (tbAlbumSort.Text.Trim() != track.AlbumSortName.Trim())
            if (i == 0)
              tbAlbumSort.Text = track.AlbumSortName;
            else
              tbAlbumSort.Text = "";

          if (tbTitleSort.Text.Trim() != track.TitleSortName.Trim())
            if (i == 0)
              tbTitleSort.Text = track.TitleSortName;
            else
              tbTitleSort.Text = "";
        }
        #endregion

        #region Original Information
        // The following values are only ID3 V2 specific
        if (track.TagType.ToLower() == "mp3")
        {
          if (tbOriginalAlbum.Text.Trim() != track.OriginalAlbum.Trim())
            if (i == 0)
              tbOriginalAlbum.Text = track.OriginalAlbum;
            else
              tbOriginalAlbum.Text = "";

          if (tbOriginalFileName.Text.Trim() != track.OriginalFileName.Trim())
            if (i == 0)
              tbOriginalFileName.Text = track.OriginalFileName;
            else
              tbOriginalFileName.Text = "";

          if (tbOriginalLyricsWriter.Text.Trim() != track.OriginalLyricsWriter.Trim())
            if (i == 0)
              tbOriginalLyricsWriter.Text = track.OriginalLyricsWriter;
            else
              tbOriginalLyricsWriter.Text = "";

          if (tbOriginalArtist.Text.Trim() != track.OriginalArtist.Trim())
            if (i == 0)
              tbOriginalArtist.Text = track.OriginalArtist;
            else
              tbOriginalArtist.Text = "";

          if (tbOriginalOwner.Text.Trim() != track.OriginalOwner.Trim())
            if (i == 0)
              tbOriginalOwner.Text = track.OriginalOwner;
            else
              tbOriginalOwner.Text = "";

          if (tbOriginalRelease.Text.Trim() != track.OriginalRelease.Trim())
            if (i == 0)
              tbOriginalRelease.Text = track.OriginalRelease;
            else
              tbOriginalRelease.Text = "";
        }
        #endregion

        #region Involved People
        // The following values are only ID3 V2 specific
        if (track.TagType.ToLower() == "mp3")
        {

          if (strInvoledPeopleTemp != track.InvolvedPeople)
          {
            if (i == 0)
            {
              // A IPLS is delimited with "\0"
              // A TIPL is delimited with ";"
              string[] ipls = track.InvolvedPeople.Split(new char[] { '\0', ';' });
              for (int j = 0; j < ipls.Length - 1; j += 2)
              {
                dataGridViewInvolvedPeople.Rows.Add(new object[] { ipls[j].Trim(), ipls[j + 1].Trim() });
              }
              strInvoledPeopleTemp = track.InvolvedPeople;
            }
            else
              dataGridViewInvolvedPeople.Rows.Clear();
          }

          if (strMusicianCreditList != track.MusicCreditList)
          {
            if (i == 0)
            {
              string[] mcl = track.MusicCreditList.Split(';');
              for (int j = 0; j < mcl.Length - 1; j += 2)
              {
                dataGridViewMusician.Rows.Add(new object[] { mcl[j].Trim(), mcl[j + 1].Trim() });
              }
              strMusicianCreditList = track.MusicCreditList;
            }
            else
              dataGridViewMusician.Rows.Clear();
          }
        }
        #endregion

        #region Web Information
        // The following values are only ID3 V2 specific
        if (track.TagType.ToLower() == "mp3")
        {
          if (tbCopyrightUrl.Text.Trim() != track.CopyrightInformation.Trim())
            if (i == 0)
              tbCopyrightUrl.Text = track.CopyrightInformation;
            else
              tbCopyrightUrl.Text = "";

          if (tbOfficialAudioFileUrl.Text.Trim() != track.OfficialAudioFileInformation.Trim())
            if (i == 0)
              tbOfficialAudioFileUrl.Text = track.OfficialAudioFileInformation;
            else
              tbOfficialAudioFileUrl.Text = "";

          if (tbOfficialArtistUrl.Text.Trim() != track.OfficialArtistInformation.Trim())
            if (i == 0)
              tbOfficialArtistUrl.Text = track.OfficialArtistInformation;
            else
              tbOfficialArtistUrl.Text = "";

          if (tbOfficialAudioSourceUrl.Text.Trim() != track.OfficialAudioSourceInformation.Trim())
            if (i == 0)
              tbOfficialAudioSourceUrl.Text = track.OfficialAudioSourceInformation;
            else
              tbOfficialAudioSourceUrl.Text = "";

          if (tbOfficialInternetRadioUrl.Text.Trim() != track.OfficialInternetRadioInformation.Trim())
            if (i == 0)
              tbOfficialInternetRadioUrl.Text = track.OfficialInternetRadioInformation;
            else
              tbOfficialInternetRadioUrl.Text = "";

          if (tbOfficialPaymentUrl.Text.Trim() != track.OfficialPaymentInformation.Trim())
            if (i == 0)
              tbOfficialPaymentUrl.Text = track.OfficialPaymentInformation;
            else
              tbOfficialPaymentUrl.Text = "";

          if (tbOfficialPublisherUrl.Text.Trim() != track.OfficialPublisherInformation.Trim())
            if (i == 0)
              tbOfficialPublisherUrl.Text = track.OfficialPublisherInformation;
            else
              tbOfficialPublisherUrl.Text = "";

          if (tbCommercialInformationUrl.Text.Trim() != track.CommercialInformation.Trim())
            if (i == 0)
              tbCommercialInformationUrl.Text = track.CommercialInformation;
            else
              tbCommercialInformationUrl.Text = "";
        }
        #endregion
        i++;
      }
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Event Handler
    /// <summary>
    /// Apply the Changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void btApply_Click(object sender, EventArgs e)
    {
      MultiTagEditOptions options = SetOptions();
      MultiTagEditApply(options);
      this.Close();
    }

    /// <summary>
    /// When the Textbox has been edited, the Check Box should be selected automatically
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnTextChanged(object sender, EventArgs e)
    {
      TextBox tb = sender as TextBox;
      switch (tb.Name)
      {
        case "tbTrack":
        case "tbNumTracks":
          ckTrack.Checked = true;
          break;

        case "tbDisc":
        case "tbNumDiscs":
          ckDisk.Checked = true;
          break;

        case "tbArtist":
          ckArtist.Checked = true;
          break;

        case "tbAlbumArtist":
          ckAlbumArtist.Checked = true;
          break;

        case "tbAlbum":
          ckAlbum.Checked = true;
          break;

        case "tbTitle":
          ckTitle.Checked = true;
          break;

        case "tbYear":
          ckYear.Checked = true;
          break;

        case "tbBPM":
          ckBPM.Checked = true;
          break;

        case "tbConductor":
          ckConductor.Checked = true;
          break;

        case "tbComposer":
          ckComposer.Checked = true;
          break;

        case "tbInterpretedBy":
          ckInterpretedBy.Checked = true;
          break;

        case "tbTextWriter":
          ckTextWriter.Checked = true;
          break;

        case "tbPublisher":
          ckPublisher.Checked = true;
          break;

        case "tbEncodedBy":
          ckEncodedBy.Checked = true;
          break;

        case "tbCopyright":
          ckCopyright.Checked = true;
          break;

        case "tbContentGroup":
          ckContentGroup.Checked = true;
          break;

        case "tbSubTitle":
          ckSubTitle.Checked = true;
          break;

        case "tbArtistSort":
          ckArtistSort.Checked = true;
          break;

        case "tbAlbumSort":
          ckAlbumSort.Checked = true;
          break;

        case "tbTitleSort":
          ckTitleSort.Checked = true;
          break;

        case "tbOriginalAlbum":
          ckOriginalAlbum.Checked = true;
          break;

        case "tbOriginalArtist":
          ckOriginalArtist.Checked = true;
          break;

        case "tbOriginalFileName":
          ckOriginalFileName.Checked = true;
          break;

        case "tbOriginalLyricsWriter":
          ckOriginalLyricsWriter.Checked = true;
          break;

        case "tbOriginalOwner":
          ckOriginalOwner.Checked = true;
          break;

        case "tbOriginalRelease":
          ckOriginalRelease.Checked = true;
          break;

        case "tbCopyrightUrl":
          ckCopyrightUrl.Checked = true;
          break;

        case "tbOfficialAudioFileUrl":
          ckOfficialAudioFileUrl.Checked = true;
          break;

        case "tbOfficialArtistUrl":
          ckOfficialArtistUrl.Checked = true;
          break;

        case "tbOfficialAudioSourceUrl":
          ckOfficialAudioSourceUrl.Checked = true;
          break;

        case "tbOfficialInternetRadioUrl":
          ckOfficialInternetRadioUrl.Checked = true;
          break;

        case "tbOfficialPaymentUrl":
          ckOfficialPaymentUrl.Checked = true;
          break;

        case "tbOfficialPublisherUrl":
          ckOfficialPublisherUrl.Checked = true;
          break;

        case "tbCommercialInformationUrl":
          ckCommercialInformationUrl.Checked = true;
          break;
      }
    }

    /// <summary>
    /// A text in the Combo has been selected. Mark the Check Box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnComboChanged(object sender, EventArgs e)
    {
      ComboBox cb = sender as ComboBox;
      switch (cb.Name)
      {
        case "cbMediaType":
          ckMediaType.Checked = true;
          break;
      }
    }
    #endregion
  }
}
