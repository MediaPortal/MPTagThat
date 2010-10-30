#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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
using System.Collections.Generic;
using System.Windows.Forms;
using MPTagThat.Core;
using TagLib;
using TagLib.Id3v2;
using Tag = TagLib.Id3v1.Tag;

#endregion

namespace MPTagThat.TagEdit
{
  public partial class MultiTagEdit : TagEditBase
  {
    #region ctor

    public MultiTagEdit(Main main)
    {
      this.main = main;
      InitializeComponent();

      base.IsMultiTagEdit = true;
    }

    #endregion

    #region Methods

    #region Form Opening

    /// <summary>
    ///   Called when loading the Dialoue
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    protected override void OnLoad(object sender, EventArgs e)
    {
      log.Trace(">>>");

      base.OnLoad(sender, e);

      // Hide the Artist / Aöbumartist Textbox as we use a Combo here
      tbArtist.Visible = false;
      tbAlbumArtist.Visible = false;
      tbAlbum.Visible = false;

      // Fill in Fields, which are the same in all selected rows
      FillForm();

      // Now Set the Event Handlers to detect changes in the Text Boxes
      tbYear.TextChanged += OnTextChanged;
      tbTitle.TextChanged += OnTextChanged;
      cbAlbum.TextChanged += OnComboChanged;
      cbArtist.TextChanged += OnComboChanged;
      tbTrack.TextChanged += OnTextChanged;
      cbAlbumArtist.TextChanged += OnComboChanged;
      tbNumTracks.TextChanged += OnTextChanged;
      tbBPM.TextChanged += OnTextChanged;
      tbNumDiscs.TextChanged += OnTextChanged;
      tbDisc.TextChanged += OnTextChanged;
      tbConductor.TextChanged += OnTextChanged;
      tbComposer.TextChanged += OnTextChanged;
      tbInterpretedBy.TextChanged += OnTextChanged;
      tbTextWriter.TextChanged += OnTextChanged;
      tbPublisher.TextChanged += OnTextChanged;
      tbEncodedBy.TextChanged += OnTextChanged;
      tbCopyright.TextChanged += OnTextChanged;
      tbContentGroup.TextChanged += OnTextChanged;
      tbSubTitle.TextChanged += OnTextChanged;
      tbArtistSort.TextChanged += OnTextChanged;
      tbAlbumSort.TextChanged += OnTextChanged;
      tbTitleSort.TextChanged += OnTextChanged;
      cbMediaType.TextChanged += OnComboChanged;
      tbOfficialArtistUrl.TextChanged += OnTextChanged;
      tbOfficialAudioFileUrl.TextChanged += OnTextChanged;
      tbOfficialAudioSourceUrl.TextChanged += OnTextChanged;
      tbOfficialInternetRadioUrl.TextChanged += OnTextChanged;
      tbOfficialPaymentUrl.TextChanged += OnTextChanged;
      tbOfficialPublisherUrl.TextChanged += OnTextChanged;
      tbCommercialInformationUrl.TextChanged += OnTextChanged;
      tbCopyrightUrl.TextChanged += OnTextChanged;

      Localisation();
      log.Trace("<<<");
    }

    private void Localisation()
    {
      base.Header = localisation.ToString("TagEdit", "MultiHeading");
    }

    #endregion

    /// <summary>
    ///   Apply the changes from the Multi Tag edit
    /// </summary>
    /// <param name = "options"></param>
    private void MultiTagEditApply(MultiTagEditOptions options)
    {
      log.Trace(">>>");
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
          Tag id3v1tag = null;
          TagLib.Id3v2.Tag id3v2tag = null;
          if (track.TagType.ToLower() == "mp3")
          {
            id3v1tag = track.File.GetTag(TagTypes.Id3v1, true) as Tag;
            id3v2tag = track.File.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;
          }

          #region Main Tags

          track.Compilation = options.Compilation;
          if (options.Track > -1 || options.NumTracks > -1)
            track.Track = string.Format("{0}/{1}", options.Track, options.NumTracks);

          if (options.Disc > -1 || options.NumDiscs > -1)
            track.Disc = string.Format("{0}/{1}", options.Disc, options.NumDiscs);

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
          {
            track.Comment = "";
            if (track.TagType.ToLower() == "mp3")
            {
              id3v1tag.Comment = null;
              id3v2tag.RemoveFrames("COMM");
            }
          }

          if (options.Comments.Count > 0)
          {
            if (track.TagType.ToLower() == "mp3")
            {
              id3v1tag.Comment = options.Comments[0].Text;
              foreach (Comment comment in options.Comments)
              {
                CommentsFrame commentsframe = CommentsFrame.Get(id3v2tag, comment.Description, comment.Language, true);
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
              id3v2tag.RemoveFrames("TMCL"); // remove the frame, when 2.3. 
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
                UnsynchronisedLyricsFrame lyricsframe = UnsynchronisedLyricsFrame.Get(id3v2tag, lyric.Description,
                                                                                      lyric.Language, true);
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
                PopularimeterFrame popmFrame = PopularimeterFrame.Get(id3v2tag, rating.User, true);
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
          row.Cells[0].Value = localisation.ToString("message", "Error");
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
      log.Trace("<<<");
    }

    /// <summary>
    ///   Set the options selected in the Form
    /// </summary>
    /// <returns></returns>
    private MultiTagEditOptions SetOptions()
    {
      log.Trace(">>>");
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
          if (tbBPM.Text.Trim() == "")
            options.BPM = 0;
          else
            options.BPM = Int32.Parse(tbBPM.Text.Trim());
        }
        catch (Exception)
        {
          options.BPM = -1;
        }
      }

      if (ckYear.Checked)
      {
        try
        {
          if (tbYear.Text.Trim() == "")
            options.Year = 0;
          else
            options.Year = Int32.Parse(tbYear.Text.Trim());
        }
        catch (Exception)
        {
          options.Year = -1;
        }
      }

      if (ckGenre.Checked || cbGenre.Text.Trim() != "")
      {
        int i = 0;

        // Add the Genre, found in the combo, so we don't need to press "Add Genre" every time
        if (cbGenre.Text.Trim() != "")
        {
          options.Genre += cbGenre.Text.Trim();
          i = 1;
        }

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

      options.Artist = (ckArtist.Checked ? cbArtist.Text : null);
      options.AlbumArtist = (ckAlbumArtist.Checked ? cbAlbumArtist.Text : null);
      options.Album = (ckAlbum.Checked ? cbAlbum.Text : null);
      options.Compilation = checkBoxCompilation.Checked;
      options.Title = (ckTitle.Checked ? tbTitle.Text : null);


      List<Comment> comments = new List<Comment>();
      foreach (DataGridViewRow row in dataGridViewComment.Rows)
      {
        Comment comment = new Comment(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(),
                                      row.Cells[2].Value.ToString());
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

      char[] d = new char[1] {'\0'};
      string delim = new string(d);
      foreach (DataGridViewRow row in dataGridViewInvolvedPeople.Rows)
      {
        options.InvolvedPeople += string.Format(@"{0}{1}{2}{3}", row.Cells[0].Value, delim, row.Cells[1].Value, delim);
      }

      if (options.InvolvedPeople != null)
        options.InvolvedPeople.Trim(new[] {'\0'});

      foreach (DataGridViewRow row in dataGridViewMusician.Rows)
      {
        options.MusicCreditList += string.Format(@"{0}{1}{2}{3}", row.Cells[0].Value, delim, row.Cells[1].Value, delim);
      }

      if (options.MusicCreditList != null)
        options.MusicCreditList.Trim(new[] {'\0'});

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
        Lyric lyric = new Lyric(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(),
                                row.Cells[2].Value.ToString());
        lyrics.Add(lyric);
      }
      options.Lyrics = lyrics;
      options.RemoveExistingLyrics = ckRemoveLyrics.Checked;

      #endregion

      #region Ratings

      List<Rating> ratings = new List<Rating>();
      foreach (DataGridViewRow row in dataGridViewRating.Rows)
      {
        Rating rating = new Rating(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(),
                                   row.Cells[2].Value.ToString());
        ratings.Add(rating);
      }
      options.Rating = ratings;
      options.RemoveExistingRating = ckRemoveExistingRatings.Checked;

      #endregion

      log.Trace("<<<");
      return options;
    }

    /// <summary>
    ///   Fill the fields in the Form with Values which are the same in all selected rows
    /// </summary>
    private void FillForm()
    {
      log.Trace(">>>");
      int i = 0;
      string strGenreTemp = "";
      string strCommentTemp = "";
      string strInvoledPeopleTemp = "";
      string strMusicianCreditList = "";

      foreach (DataGridViewRow row in main.TracksGridView.View.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = main.TracksGridView.TrackList[row.Index];

        // Get the ID3 Frame for ID3 specifc frame handling
        Tag id3v1tag = null;
        TagLib.Id3v2.Tag id3v2tag = null;
        if (track.TagType.ToLower() == "mp3")
        {
          id3v1tag = track.File.GetTag(TagTypes.Id3v1, true) as Tag;
          id3v2tag = track.File.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;
        }

        #region Main Tags

        if (cbArtist.Text.Trim() != track.Artist.Trim())
          if (i == 0)
            cbArtist.Text = track.Artist;
          else
            cbArtist.Text = "";

        if (cbAlbumArtist.Text.Trim() != track.AlbumArtist.Trim())
          if (i == 0)
            cbAlbumArtist.Text = track.AlbumArtist;
          else
            cbAlbumArtist.Text = "";

        if (cbAlbum.Text.Trim() != track.Album.Trim())
          if (i == 0)
            cbAlbum.Text = track.Album;
          else
            cbAlbum.Text = "";

        if (checkBoxCompilation.Checked != track.Compilation)
          if (i == 0)
            checkBoxCompilation.Checked = track.Compilation;
          else
            checkBoxCompilation.Checked = false;

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

        // Get Comment
        string sComment = "";
        if (track.TagType.ToLower() == "mp3")
        {
          sComment += id3v1tag.Comment;
          foreach (CommentsFrame commentsframe in id3v2tag.GetFrames<CommentsFrame>())
          {
            sComment += commentsframe.Text;
          }
        }
        else
          sComment = track.Comment;

        if (strCommentTemp != sComment)
        {
          if (i == 0)
          {
            strCommentTemp = sComment;
            if (track.TagType.ToLower() == "mp3")
            {
              AddComment("", "", id3v1tag.Comment);
              foreach (CommentsFrame commentsframe in id3v2tag.GetFrames<CommentsFrame>())
              {
                AddComment(commentsframe.Description, commentsframe.Language, commentsframe.Text);
              }
            }
            else
              AddComment("", "", track.Comment);
          }
          else
            dataGridViewComment.Rows.Clear();
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
              string[] ipls = track.InvolvedPeople.Split(new[] {'\0', ';'});
              for (int j = 0; j < ipls.Length - 1; j += 2)
              {
                dataGridViewInvolvedPeople.Rows.Add(new object[] {ipls[j].Trim(), ipls[j + 1].Trim()});
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
                dataGridViewMusician.Rows.Add(new object[] {mcl[j].Trim(), mcl[j + 1].Trim()});
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

      // Auto fill the Number of Tracks field
      if (tbNumTracks.Text == "")
      {
        tbNumTracks.Text = main.TracksGridView.View.SelectedRows.Count.ToString().PadLeft(2, '0');
        if (Options.MainSettings.AutoFillNumberOfTracks)
        {
          ckTrack.Checked = true;
        }
      }

      // Now see, if we have different values in the Artist and AlbumArtist fields and fill the combo
      List<string> itemsArtist = new List<string>();
      List<string> itemsAlbumArtist = new List<string>();
      List<string> itemsAlbum = new List<string>();
      foreach (DataGridViewRow row in main.TracksGridView.View.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = main.TracksGridView.TrackList[row.Index];

        bool found = false;
        foreach (string item in itemsArtist)
        {
          if (item == track.Artist)
          {
            found = true;
            break;
          }
        }

        if (!found)
        {
          itemsArtist.Add(track.Artist);
        }

        found = false;
        foreach (string item in itemsAlbumArtist)
        {
          if (item == track.AlbumArtist)
          {
            found = true;
            break;
          }
        }

        if (!found)
        {
          itemsAlbumArtist.Add(track.AlbumArtist);
        }

        found = false;
        foreach (string item in itemsAlbum)
        {
          if (item == track.Album)
          {
            found = true;
            break;
          }
        }

        if (!found)
        {
          itemsAlbum.Add(track.Album);
        }
      }

      if (itemsArtist.Count > 0)
      {
        cbArtist.Items.AddRange(itemsArtist.ToArray());
      }

      if (itemsAlbumArtist.Count > 0)
      {
        cbAlbumArtist.Items.AddRange(itemsAlbumArtist.ToArray());
      }

      if (itemsAlbum.Count > 0)
      {
        cbAlbum.Items.AddRange(itemsAlbum.ToArray());
      }
      log.Trace("<<<");
    }

    #endregion

    #region Event Handler

    /// <summary>
    ///   Apply the Changes
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    protected override void btApply_Click(object sender, EventArgs e)
    {
      MultiTagEditOptions options = SetOptions();
      MultiTagEditApply(options);
      Close();
    }

    /// <summary>
    ///   When the Textbox has been edited, the Check Box should be selected automatically
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
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
    ///   A text in the Combo has been selected. Mark the Check Box
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    protected override void OnComboChanged(object sender, EventArgs e)
    {
      ComboBox cb = sender as ComboBox;
      switch (cb.Name)
      {
        case "cbMediaType":
          ckMediaType.Checked = true;
          break;

        case "cbArtist":
          ckArtist.Checked = true;
          break;

        case "cbAlbumArtist":
          ckAlbumArtist.Checked = true;
          break;

        case "cbAlbum":
          ckAlbum.Checked = true;
          break;
      }
    }

    #endregion
  }
}