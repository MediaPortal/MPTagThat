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
  public partial class SingleTagEdit : TagEditBase
  {
    #region Variables
    private int _currentRowIndex = -1;
    private bool _trackIsChanged = false;
    private TrackData track = null;
    private string _headerText = "";
    #endregion

    #region Properties
    public string LyricsText
    {
      get { return tbLyrics.Text; }
      set { tbLyrics.Text = value; }
    }
    #endregion

    #region ctor
    public SingleTagEdit(Main main)
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

      // Hide the Combo boxes, which are only available for Multi Tag Edit
      cbArtist.Visible = false;
      cbAlbumArtist.Visible = false;
      cbAlbumArtist.Visible = false;

      Localisation();

      // Load available Scripts
      System.Collections.ArrayList scripts = null;
      scripts = ServiceScope.Get<IScriptManager>().GetScripts();
      foreach (string[] script in scripts)
      {
        Item item = new Item(script[1], script[0], script[2]);
        comboBoxScripts.Items.Add(item);
      }
      if (comboBoxScripts.Items.Count > 0)
        comboBoxScripts.SelectedIndex = 0;

      // Fill in Fields, which are the same in all selected rows
      if (main.TracksGridView.TrackList.Count > 0)
      {
        FillForm();
      }

      // Hide the check boxes, which are available in Multi Tag edit only
      ckTrack.Visible = false;
      ckDisk.Visible = false;
      ckArtist.Visible = false;
      ckAlbumArtist.Visible = false;
      ckGenre.Visible = false;
      ckAlbum.Visible = false;
      ckTitle.Visible = false;
      ckYear.Visible = false;
      ckBPM.Visible = false;
      ckConductor.Visible = false;
      ckComposer.Visible = false;
      ckInterpretedBy.Visible = false;
      ckTextWriter.Visible = false;
      ckPublisher.Visible = false;
      ckEncodedBy.Visible = false;
      ckCopyright.Visible = false;
      ckContentGroup.Visible = false;
      ckSubTitle.Visible = false;
      ckArtistSort.Visible = false;
      ckAlbumSort.Visible = false;
      ckTitleSort.Visible = false;
      ckOriginalAlbum.Visible = false;
      ckOriginalArtist.Visible = false;
      ckOriginalFileName.Visible = false;
      ckOriginalLyricsWriter.Visible = false;
      ckOriginalOwner.Visible = false;
      ckOriginalRelease.Visible = false;
      ckCopyrightUrl.Visible = false;
      ckOfficialAudioFileUrl.Visible = false;
      ckOfficialArtistUrl.Visible = false;
      ckOfficialAudioSourceUrl.Visible = false;
      ckOfficialInternetRadioUrl.Visible = false;
      ckOfficialPaymentUrl.Visible = false;
      ckCommercialInformationUrl.Visible = false;
      ckOfficialPublisherUrl.Visible = false;
      ckInvolvedPerson.Visible = false;
      ckInvolvedMusician.Visible = false;
      ckRemoveExistingRatings.Visible = false;
      ckRemoveLyrics.Visible = false;
      ckMediaType.Visible = false;
      checkBoxRemoveComments.Visible = false;
      checkBoxRemoveExistingPictures.Visible = false;

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    private void Localisation()
    {
      _headerText = localisation.ToString("TagEdit", "SingleHeading");
      base.Header = _headerText;
      this.cmdPanel.CaptionText = localisation.ToString("TagEdit", "Commands");
    }
    #endregion

    /// <summary>
    /// Apply the changes from the Tag edit
    /// </summary>
    private void SingleTagEditApply()
    {
      Util.EnterMethod(Util.GetCallingMethod());

      // if we don't have any rows in the griddon't do any action
      if (main.TracksGridView.TrackList.Count == 0)
      {
        return;
      }

      CheckForChanges();

      if (!_trackIsChanged && !_commentIsChanged && !_involvedPeopleIsChanged && !_lyricsIsChanged &&
          !_musicianIsChanged && !_pictureIsChanged && !_ratingIsChanged)
        return;

      bool bErrors = false;
      try
      {
        main.TracksGridView.Changed = true;
        main.TracksGridView.SetBackgroundColorChanged(_currentRowIndex);
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
        if (tbTrack.Text.Trim().Length > 0 || tbNumTracks.Text.Trim().Length > 0)
        {
          int tracknumber = 0;
          int numbertracks = 0;
          try
          {
            tracknumber = Int32.Parse(tbTrack.Text.Trim());
          }
          catch (Exception)
          { }
          try
          {
            numbertracks = Int32.Parse(tbNumTracks.Text.Trim());
          }
          catch (Exception)
          { }
          track.Track = string.Format("{0}/{1}", tracknumber, numbertracks);
        }
        else
        {
          track.File.Tag.Track = 0;
          track.File.Tag.TrackCount = 0;
        }

        if (tbDisc.Text.Trim().Length > 0 || tbNumDiscs.Text.Trim().Length > 0)
        {
          int discnumber = 0;
          int numberdiscs = 0;
          try
          {
            discnumber = Int32.Parse(tbDisc.Text.Trim());
          }
          catch (Exception)
          { }
          try
          {
            numberdiscs = Int32.Parse(tbNumDiscs.Text.Trim());
          }
          catch (Exception)
          { }

          track.Disc = string.Format("{0}/{1}", discnumber, numberdiscs);
        }
        else
        {
          track.File.Tag.Disc = 0;
          track.File.Tag.DiscCount = 0;
        }

        if (tbBPM.Text.Trim().Length > 0)
        {
          try
          {
            track.File.Tag.BeatsPerMinute = (uint)Int32.Parse(tbBPM.Text.Trim());
          }
          catch (Exception)
          {
            track.File.Tag.BeatsPerMinute = 0;
          }
        }
        else
          track.File.Tag.BeatsPerMinute = 0;


        if (tbYear.Text.Trim().Length > 0)
          try
          {
            track.Year = Int32.Parse(tbYear.Text.Trim());
          }
          catch (Exception)
          {
            track.Year = 0;
          }
        else
          track.Year = 0;

        track.Artist = tbArtist.Text.Trim();
        track.AlbumArtist = tbAlbumArtist.Text.Trim();
        track.Album = tbAlbum.Text.Trim();
        track.Title = tbTitle.Text.Trim();

        string genre = "";
        int i = 0;
        
        // Add the Genre, found in the combo, so we don't need to press "Add Genre" every time
        if (cbGenre.Text.Trim() != "")
        {
          genre += cbGenre.Text.Trim();
          i = 1;
        }
        foreach (string item in listBoxGenre.Items)
        {
          if (i == 0)
            genre += item;
          else
            genre += ";" + item;
          i++;
        }
        track.Genre = genre;

        List<Comment> comments = new List<Comment>();
        foreach (DataGridViewRow row in dataGridViewComment.Rows)
        {
          Comment comment = new Comment(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
          comments.Add(comment);
        }

        if (comments.Count > 0)
        {
          // Clear existing Comments first, otherwise, we will have them stored several times
          track.Comment = "";
          if (track.TagType.ToLower() == "mp3")
          {
            id3v1tag.Comment = comments[0].Text;
            foreach (Comment comment in comments)
            {
              TagLib.Id3v2.CommentsFrame commentsframe = TagLib.Id3v2.CommentsFrame.Get(id3v2tag, comment.Description, comment.Language, true);
              commentsframe.Text = comment.Text;
            }
          }
          else
            track.Comment = comments[0].Text;
        }
        else
          track.Comment = "";
        #endregion

        #region Detailed Information
        track.Conductor = tbConductor.Text.Trim();
        track.Composer = tbComposer.Text.Trim();
        track.Copyright = tbCopyright.Text.Trim();
        track.Grouping = tbContentGroup.Text.Trim();

        // The following values are only ID3 V2 specific
        if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
        {
          track.Interpreter = tbInterpretedBy.Text.Trim();
          track.TextWriter = tbTextWriter.Text.Trim();
          track.Publisher = tbPublisher.Text.Trim();
          track.EncodedBy = tbEncodedBy.Text.Trim();
          track.SubTitle = tbSubTitle.Text.Trim();
          track.MediaType = cbMediaType.SelectedText;
          track.TrackLength = tbTrackLength.Text.Trim();

          // V 2.4 Frames only
          if (Options.MainSettings.ID3V2Version == 4)
          {
            track.ArtistSortName = tbArtistSort.Text.Trim();
            track.AlbumSortName = tbAlbumSort.Text.Trim();
            track.TitleSortName = tbTitleSort.Text.Trim();
          }
        }
        #endregion

        #region Original Information
        if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
        {
          track.OriginalAlbum = tbOriginalAlbum.Text.Trim();
          track.OriginalArtist = tbOriginalArtist.Text.Trim();
          track.OriginalFileName = tbOriginalFileName.Text.Trim();
          track.OriginalLyricsWriter = tbOriginalLyricsWriter.Text.Trim();
          track.OriginalOwner = tbOriginalOwner.Text.Trim();
          track.OriginalRelease = tbOriginalRelease.Text.Trim();
        }
        #endregion

        #region Pictures
        if (_pictureIsChanged)
          track.File.Tag.Pictures = _pictures.ToArray();
        #endregion

        #region Involved People
        // The following values are only ID3 V2 specific
        if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
        {
          char[] d = new char[1] { '\0' };
          string delim = new string(d);
          string tmp = "";

          foreach (DataGridViewRow row in dataGridViewInvolvedPeople.Rows)
          {
            tmp += string.Format(@"{0}{1}{2}{3}", row.Cells[0].Value.ToString(), delim, row.Cells[1].Value.ToString(), delim);
          }

          if (tmp != "")
            tmp.Trim(new char[] { '\0' });

          track.InvolvedPeople = tmp;

          // remove frames, which may have been left from a previous save in a different version
          if (Options.MainSettings.ID3V2Version == 4)
            id3v2tag.RemoveFrames("IPLS");
          else
            id3v2tag.RemoveFrames("TIPL");

          // V 2.4 Frames only
          if (Options.MainSettings.ID3V2Version == 4)
          {
            tmp = "";
            foreach (DataGridViewRow row in dataGridViewMusician.Rows)
            {
              tmp += string.Format(@"{0}{1}{2}{3}", row.Cells[0].Value.ToString(), delim, row.Cells[1].Value.ToString(), delim);
            }

            if (tmp != null)
              tmp.Trim(new char[] { '\0' });

            track.MusicCreditList = tmp;
          }
          else
            id3v2tag.RemoveFrames("TMCL");  // remove the frame, when 2.3. 
        }
        #endregion

        #region Web Information
        if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
        {
          track.CopyrightInformation = tbCopyrightUrl.Text.Trim();
          track.OfficialAudioFileInformation = tbOfficialAudioFileUrl.Text.Trim();
          track.OfficialArtistInformation = tbOfficialArtistUrl.Text.Trim();
          track.OfficialAudioSourceInformation = tbOfficialAudioSourceUrl.Text.Trim();
          track.OfficialInternetRadioInformation = tbOfficialInternetRadioUrl.Text.Trim();
          track.OfficialPaymentInformation = tbOfficialPaymentUrl.Text.Trim();
          track.OfficialPublisherInformation = tbOfficialPublisherUrl.Text.Trim();
          track.CommercialInformation = tbCommercialInformationUrl.Text.Trim();
        }
        #endregion

        #region Lyrics
        if (_lyricsIsChanged)
        {
          List<Lyric> lyrics = new List<Lyric>();
          foreach (DataGridViewRow row in dataGridViewLyrics.Rows)
          {
            Lyric lyric = new Lyric(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
            lyrics.Add(lyric);
          }

          if (lyrics.Count > 0)
          {
            if (track.TagType.ToLower() == "mp3")
            {
              foreach (Lyric lyric in lyrics)
              {
                TagLib.Id3v2.UnsynchronisedLyricsFrame lyricsframe = TagLib.Id3v2.UnsynchronisedLyricsFrame.Get(id3v2tag, lyric.Description, lyric.Language, true);
                lyricsframe.Text = lyric.Text;
              }
            }
            else
              track.Lyrics = lyrics[0].Text;
          }
          else
            track.Lyrics = "";
        }
        #endregion

        #region Rating
        if (_ratingIsChanged)
        {
          // The following values are only ID3 V2 specific
          if (track.TagType.ToLower() == "mp3" && id3v2tag != null)
          {
            List<Rating> ratings = new List<Rating>();
            foreach (DataGridViewRow row in dataGridViewRating.Rows)
            {
              Rating rating = new Rating(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
              ratings.Add(rating);
            }

            if (ratings.Count > 0)
            {
              foreach (Rating rating in ratings)
              {
                TagLib.Id3v2.PopularimeterFrame popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(id3v2tag, rating.User, true);
                popmFrame.Rating = Convert.ToByte(rating.RatingValue);
                popmFrame.PlayCount = Convert.ToUInt32(rating.PlayCounter);
              }
            }
            else
              id3v2tag.RemoveFrames("POPM");
          }
        }
        #endregion
      }
      catch (Exception ex)
      {
        log.Error("Error applying changes from Tagedit: {0} stack: {1}", ex.Message, ex.StackTrace);
        main.TracksGridView.View.Rows[_currentRowIndex].Cells[1].Value = localisation.ToString("message", "Error");
        main.TracksGridView.AddErrorMessage(main.TracksGridView.TrackList[_currentRowIndex].File.Name, ex.Message);
        bErrors = true;
      }

      main.TracksGridView.Changed = bErrors;
      // check, if we still have changed items in the list
      foreach (TrackData track in main.TracksGridView.TrackList)
      {
        if (track.Changed)
          main.TracksGridView.Changed = true;
      }

      main.TracksGridView.View.Refresh();
      main.TracksGridView.View.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    /// <summary>
    /// Fill the fields in the Form with Values which are the same in all selected rows
    /// </summary>
    private void FillForm()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      _trackIsChanged = false;
      _commentIsChanged = false;
      _involvedPeopleIsChanged = false;
      _lyricsIsChanged = false;
      _musicianIsChanged = false;
      _pictureIsChanged = false;
      _ratingIsChanged = false;


      if (_currentRowIndex == -1)
      {
        _currentRowIndex = main.TracksGridView.View.CurrentRow.Index;
      }
      track = main.TracksGridView.TrackList[_currentRowIndex];

      base.Header = string.Format("{0} - {1}", _headerText, track.FileName);

      // Get the ID3 Frame for ID3 specifc frame handling
      TagLib.Id3v1.Tag id3v1tag = null;
      TagLib.Id3v2.Tag id3v2tag = null;
      if (track.TagType.ToLower() == "mp3")
      {
        id3v1tag = track.File.GetTag(TagTypes.Id3v1, true) as TagLib.Id3v1.Tag;
        id3v2tag = track.File.GetTag(TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;
      }

      #region Main Tags
      tbArtist.Text = track.Artist;
      tbAlbumArtist.Text = track.AlbumArtist;
      tbAlbum.Text = track.Album;
      tbTitle.Text = track.Title;
      tbYear.Text = track.Year.ToString();
      tbBPM.Text = track.BPM.ToString();

      string[] disc = track.Disc.Split('/');
      tbDisc.Text = disc[0];
      if (disc.Length > 1)
        tbNumDiscs.Text = disc[1];
      else
        tbNumDiscs.Text = "";

      string[] tracks = track.Track.Split('/');
      tbTrack.Text = tracks[0];
      if (tracks.Length > 1)
        tbNumTracks.Text = tracks[1];
      else
        tbNumTracks.Text = "";

      listBoxGenre.Items.Clear();
      listBoxGenre.Items.AddRange(track.Genre.Split(';'));

      dataGridViewComment.Rows.Clear();
      if (track.TagType.ToLower() == "mp3")
      {
        AddComment("", "", id3v1tag.Comment);
        foreach (TagLib.Id3v2.CommentsFrame commentsframe in id3v2tag.GetFrames<TagLib.Id3v2.CommentsFrame>())
        {
          AddComment(commentsframe.Description, commentsframe.Language, commentsframe.Text);
        }
      }
      else
        AddComment("", "", track.Comment);
      #endregion

      #region Picture
      tbPicDesc.Text = "";
      dataGridViewPicture.Rows.Clear();
      _pictures.Clear();
      foreach (IPicture picture in track.File.Tag.Pictures)
      {
        dataGridViewPicture.Rows.Add(new object[] { picture.Description, Enum.Format(typeof(TagLib.PictureType), picture.Type, "G") });
        Picture pic = new Picture();
        pic.Data = picture.Data;
        pic.Description = picture.Description;
        pic.MimeType = picture.MimeType;
        pic.Type = picture.Type;
        _pictures.Add(pic);
      }

      // Display first Picture
      if (dataGridViewPicture.Rows.Count > 0)
      {
        DataGridViewCellEventArgs evt = new DataGridViewCellEventArgs(0, 0);
        dataGridViewPicture_CellClick(dataGridViewPicture, evt);
      }
      #endregion

      #region Detailed Information
      tbConductor.Text = track.Conductor;
      tbComposer.Text = track.Composer;
      tbCopyright.Text = track.Copyright;
      tbInterpretedBy.Text = track.Interpreter;
      tbTextWriter.Text = track.TextWriter;
      tbPublisher.Text = track.Publisher;
      tbEncodedBy.Text = track.EncodedBy;
      tbContentGroup.Text = track.Grouping;
      tbSubTitle.Text = track.SubTitle;
      tbArtistSort.Text = track.ArtistSortName;
      tbAlbumSort.Text = track.AlbumSortName;
      tbTitleSort.Text = track.TitleSortName;
      tbTrackLength.Text = track.TrackLength;
      cbMediaType.Text = track.MediaType;

      if (track.TagType.ToLower() == "mp3")
      {
        tbInterpretedBy.Enabled = true;
        tbTextWriter.Enabled = true;
        tbPublisher.Enabled = true;
        tbEncodedBy.Enabled = true;
        tbContentGroup.Enabled = true;
        tbSubTitle.Enabled = true;
        tbArtistSort.Enabled = true;
        tbAlbumSort.Enabled = true;
        tbTitleSort.Enabled = true;
        tbTrackLength.Enabled = true;
        cbMediaType.Enabled = true;
      }
      else
      {
        tbInterpretedBy.Enabled = false;
        tbTextWriter.Enabled = false;
        tbPublisher.Enabled = false;
        tbEncodedBy.Enabled = false;
        tbContentGroup.Enabled = false;
        tbSubTitle.Enabled = false;
        tbArtistSort.Enabled = false;
        tbAlbumSort.Enabled = false;
        tbTitleSort.Enabled = false;
        tbTrackLength.Enabled = false;
        cbMediaType.Enabled = false;
      }
      #endregion

      #region Original Information
      tbOriginalAlbum.Text = track.OriginalAlbum;
      tbOriginalFileName.Text = track.OriginalFileName;
      tbOriginalLyricsWriter.Text = track.OriginalLyricsWriter;
      tbOriginalArtist.Text = track.OriginalArtist;
      tbOriginalOwner.Text = track.OriginalOwner;
      tbOriginalRelease.Text = track.OriginalRelease;

      if (track.TagType.ToLower() == "mp3")
        groupBoxOriginalInformation.Enabled = true;
      else
        groupBoxOriginalInformation.Enabled = false;
      #endregion

      #region Involved People
      dataGridViewInvolvedPeople.Rows.Clear();
      // A IPLS is delimited with "\0"
      // A TIPL is delimited with ";"
      string[] ipls = track.InvolvedPeople.Split(new char[] { '\0', ';' });
      for (int j = 0; j < ipls.Length - 1; j += 2)
      {
        dataGridViewInvolvedPeople.Rows.Add(new object[] { ipls[j].Trim(), ipls[j + 1].Trim() });
      }

      dataGridViewMusician.Rows.Clear();
      string[] mcl = track.MusicCreditList.Split(';');
      for (int j = 0; j < mcl.Length - 1; j += 2)
      {
        dataGridViewMusician.Rows.Add(new object[] { mcl[j].Trim(), mcl[j + 1].Trim() });
      }

      if (track.TagType.ToLower() == "mp3")
      {
        groupBoxInvolvedPeople.Enabled = true;
        groupBoxMusician.Enabled = true;
      }
      else
      {
        groupBoxInvolvedPeople.Enabled = false;
        groupBoxMusician.Enabled = true;
      }
      #endregion

      #region Web Information
      tbCopyrightUrl.Text = track.CopyrightInformation;
      tbOfficialAudioFileUrl.Text = track.OfficialAudioFileInformation;
      tbOfficialArtistUrl.Text = track.OfficialArtistInformation;
      tbOfficialAudioSourceUrl.Text = track.OfficialAudioSourceInformation;
      tbOfficialInternetRadioUrl.Text = track.OfficialInternetRadioInformation;
      tbOfficialPaymentUrl.Text = track.OfficialPaymentInformation;
      tbOfficialPublisherUrl.Text = track.OfficialPublisherInformation;
      tbCommercialInformationUrl.Text = track.CommercialInformation;

      if (track.TagType.ToLower() == "mp3")
        groupBoxWebInformation.Enabled = true;
      else
        groupBoxWebInformation.Enabled = false;
      #endregion

      #region Lyrics
      dataGridViewLyrics.Rows.Clear();
      if (track.TagType.ToLower() == "mp3")
      {
        if ((track.File.TagTypesOnDisk & TagTypes.Ape) == TagTypes.Ape)
        {
          AddLyrics("", "", track.Lyrics);
        }

        foreach (TagLib.Id3v2.UnsynchronisedLyricsFrame lyricsframe in id3v2tag.GetFrames<TagLib.Id3v2.UnsynchronisedLyricsFrame>())
        {
          AddLyrics(lyricsframe.Description, lyricsframe.Language, lyricsframe.Text);
        }
      }
      else
        AddLyrics("", "", track.Lyrics);
      #endregion

      #region Rating
      dataGridViewRating.Rows.Clear();
      if (track.TagType.ToLower() == "mp3")
      {
        foreach (TagLib.Id3v2.PopularimeterFrame popmframe in id3v2tag.GetFrames<TagLib.Id3v2.PopularimeterFrame>())
        {
          AddRating(popmframe.User, Convert.ToString((int)popmframe.Rating), Convert.ToString(popmframe.PlayCount));
        }
      }
      if (track.TagType.ToLower() == "mp3")
        groupBoxRating.Enabled = true;
      else
        groupBoxRating.Enabled = false;
      #endregion

      Util.LeaveMethod(Util.GetCallingMethod());
    }

    private void CheckForChanges()
    {
      #region Main Tags
      if (tbArtist.Text != track.Artist) _trackIsChanged = true;
      if (tbAlbumArtist.Text != track.AlbumArtist) _trackIsChanged = true;
      if (tbAlbum.Text != track.Album) _trackIsChanged = true;
      if (tbTitle.Text != track.Title) _trackIsChanged = true;
      if (tbYear.Text != track.Year.ToString()) _trackIsChanged = true;
      if (tbBPM.Text != track.BPM.ToString()) _trackIsChanged = true;

      string[] disc = track.Disc.Split('/');
      if (tbDisc.Text != disc[0]) _trackIsChanged = true;
      if (disc.Length > 1)
      {
        if (tbNumDiscs.Text != disc[1]) _trackIsChanged = true;
      }
      else
      {
        if (tbNumDiscs.Text.Trim().Length > 0) _trackIsChanged = true;
      }

      string[] tracks = track.Track.Split('/');
      if (tbTrack.Text != tracks[0]) _trackIsChanged = true;
      if (tracks.Length > 1)
      {
        if (tbNumTracks.Text != tracks[1]) _trackIsChanged = true;
      }
      else
      {
        if (tbNumTracks.Text.Trim().Length > 0) _trackIsChanged = true;
      }

      string genre = "";
      int i = 0;
      if (cbGenre.Text.Trim() != "")
      {
        genre += cbGenre.Text.Trim();
        i = 1;
      }
      foreach (string item in listBoxGenre.Items)
      {
        if (i == 0)
          genre += item;
        else
          genre += ";" + item;
        i++;
      }
      if (track.Genre != genre) _trackIsChanged = true;
      #endregion

      #region Detailed Information
      if (track.Conductor != tbConductor.Text) _trackIsChanged = true;
      if (track.Composer != tbComposer.Text) _trackIsChanged = true;
      if (track.Copyright != tbCopyright.Text) _trackIsChanged = true;
      if (track.Grouping != tbContentGroup.Text) _trackIsChanged = true;

      // The following values are only ID3 V2 specific
      if (track.TagType.ToLower() == "mp3")
      {
        if (track.Interpreter != tbInterpretedBy.Text) _trackIsChanged = true;
        if (track.TextWriter != tbTextWriter.Text) _trackIsChanged = true;
        if (track.Publisher != tbPublisher.Text) _trackIsChanged = true;
        if (track.EncodedBy != tbEncodedBy.Text) _trackIsChanged = true;
        if (track.SubTitle != tbSubTitle.Text) _trackIsChanged = true;
        if (track.MediaType != cbMediaType.SelectedText) _trackIsChanged = true;
        if (track.TrackLength != tbTrackLength.Text) _trackIsChanged = true;

        // V 2.4 Frames only
        if (Options.MainSettings.ID3V2Version == 4)
        {
          if (track.ArtistSortName != tbArtistSort.Text) _trackIsChanged = true;
          if (track.AlbumSortName != tbAlbumSort.Text) _trackIsChanged = true;
          if (track.TitleSortName != tbTitleSort.Text) _trackIsChanged = true;
        }
      }
      #endregion

      #region Original Information
      if (track.TagType.ToLower() == "mp3")
      {
        if (track.OriginalAlbum != tbOriginalAlbum.Text) _trackIsChanged = true;
        if (track.OriginalArtist != tbOriginalArtist.Text) _trackIsChanged = true;
        if (track.OriginalFileName != tbOriginalFileName.Text) _trackIsChanged = true;
        if (track.OriginalLyricsWriter != tbOriginalLyricsWriter.Text) _trackIsChanged = true;
        if (track.OriginalOwner != tbOriginalOwner.Text) _trackIsChanged = true;
        if (track.OriginalRelease != tbOriginalRelease.Text) _trackIsChanged = true;
      }
      #endregion

      #region Web Information
      if (track.TagType.ToLower() == "mp3")
      {
        if (track.CopyrightInformation != tbCopyrightUrl.Text) _trackIsChanged = true;
        if (track.OfficialAudioFileInformation != tbOfficialAudioFileUrl.Text) _trackIsChanged = true;
        if (track.OfficialArtistInformation != tbOfficialArtistUrl.Text) _trackIsChanged = true;
        if (track.OfficialAudioSourceInformation != tbOfficialAudioSourceUrl.Text) _trackIsChanged = true;
        if (track.OfficialInternetRadioInformation != tbOfficialInternetRadioUrl.Text) _trackIsChanged = true;
        if (track.OfficialPaymentInformation != tbOfficialPaymentUrl.Text) _trackIsChanged = true;
        if (track.OfficialPublisherInformation != tbOfficialPublisherUrl.Text) _trackIsChanged = true;
        if (track.CommercialInformation != tbCommercialInformationUrl.Text) _trackIsChanged = true;
      }
      #endregion
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
      SingleTagEditApply();
      this.Close();
    }

    #region Navigation Page
    private void btPreviousFile_Click(object sender, EventArgs e)
    {
      Action action = new Action();
      action.ID = Action.ActionType.ACTION_PREVFILE;
      OnAction(action);
    }

    private void btNextFile_Click(object sender, EventArgs e)
    {
      Action action = new Action();
      action.ID = Action.ActionType.ACTION_NEXTFILE;
      OnAction(action);
    }

    private void btScriptExecute_Click(object sender, EventArgs e)
    {
      Action action = new Action();
      action.ID = Action.ActionType.ACTION_SCRIPTEXECUTE;
      OnAction(action);
    }
    #endregion

    #region Detailed Information
    private void btGetTrackLength_Click(object sender, EventArgs e)
    {
      tbTrackLength.Text = track.File.Properties.Duration.TotalMilliseconds.ToString();
    }
    #endregion

    #region Lyrics
    private void btGetLyricsFromInternet_Click(object sender, EventArgs e)
    {
      LyricsSearch lyricsSearch = new LyricsSearch(this, tbArtist.Text, tbTitle.Text, false);
    }
    #endregion

    #endregion

    #region Key Events
    /// <summary>
    /// A Key has been pressed
    /// </summary>
    /// <param name="e"></param>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      Action newaction = new Action();
      if (ServiceScope.Get<IActionHandler>().GetAction(2, keyData, ref newaction))
      {
        if (OnAction(newaction))
        {

          newaction = null;
          return true;
        }
      }
      newaction = null;
      return base.ProcessCmdKey(ref msg, keyData);
    }

    bool OnAction(Action action)
    {
      if (action == null)
        return false;

      bool handled = true;
      switch (action.ID)
      {
        case Action.ActionType.ACTION_PAGEDOWN:
          if (tabControlTagEdit.SelectedIndex == tabControlTagEdit.TabCount - 1)
            tabControlTagEdit.SelectedIndex = 0;
          else
            tabControlTagEdit.SelectedIndex = tabControlTagEdit.SelectedIndex + 1;
          break;

        case Action.ActionType.ACTION_PAGEUP:
          if (tabControlTagEdit.SelectedIndex == 0)
            tabControlTagEdit.SelectedIndex = tabControlTagEdit.TabCount - 1;
          else
            tabControlTagEdit.SelectedIndex = tabControlTagEdit.SelectedIndex - 1;
          break;

        case Action.ActionType.ACTION_NEXTFILE:
          if ((_currentRowIndex + 1) < main.TracksGridView.View.RowCount)
          {
            SingleTagEditApply();
            _currentRowIndex++;
            FillForm();
          }
          break;

        case Action.ActionType.ACTION_PREVFILE:
          if ((_currentRowIndex - 1) >= 0)
          {
            SingleTagEditApply();
            _currentRowIndex--;
            FillForm();
          }
          break;

        case Action.ActionType.ACTION_SCRIPTEXECUTE:
          if (comboBoxScripts.SelectedIndex > -1)
          {
            Item item = (Item)comboBoxScripts.SelectedItem;
            System.Reflection.Assembly assembly = ServiceScope.Get<IScriptManager>().Load(item.Value);

            try
            {
              if (assembly != null)
              {
                IScript script = (IScript)assembly.CreateInstance("Script");
                List<TrackData> tracks = new List<TrackData>();
                tracks.Add(track);
                script.Invoke(tracks);
                FillForm();
                main.TracksGridView.Changed = true;
                main.TracksGridView.SetBackgroundColorChanged(_currentRowIndex);
              }
            }
            catch (Exception ex)
            {
              log.Error("Script Execution failed: {0}", ex.Message);
              MessageBox.Show(localisation.ToString("message", "Script_Compile_Failed"), localisation.ToString("message", "Error_Title"), MessageBoxButtons.OK);
            }
          }
          break;
      }
      return handled;
    }
    #endregion
  }
}
