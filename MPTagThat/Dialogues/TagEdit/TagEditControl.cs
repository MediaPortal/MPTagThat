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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.Amazon;
using MPTagThat.Core.Common;
using MPTagThat.Core.ShellLib;
using MPTagThat.Core.WinControls;
using MPTagThat.Dialogues;
using TagLib;
using Picture = MPTagThat.Core.Common.Picture;
using Action = MPTagThat.Core.Action;

#endregion

namespace MPTagThat.TagEdit
{
  public partial class TagEditControl : UserControl
  {
    #region Variables

    private bool _isMultiTagEdit;
    private bool _commentIsChanged;
    private bool _involvedPeopleIsChanged;
    private bool _lyricsIsChanged;
    private bool _musicianIsChanged;
    private bool _userFrameIsChanged;
    private Picture _pic;
    private bool _pictureIsChanged;
    private List<Picture> _pictures = new List<Picture>();
    private bool _ratingIsChanged;
    private int _selectedPictureGridRow = -1;
    private int _selectedRowIndex = -1;

    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
    private Main main;

    #endregion

    #region ctor
    public TagEditControl(Main main)
    {
      // Activates double buffering 
      this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();

      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;

      this.main = main;
      InitializeComponent();
    }

    #endregion

    #region Properties

    public string LyricsText
    {
      get { return tbLyrics.Text; }
      set { tbLyrics.Text = value; }
    }

    #endregion

    #region Form Opening

    /// <summary>
    ///   Called when loading the Dialoue
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    protected virtual void OnLoad(object sender, EventArgs e)
    {
      log.Trace(">>>");
      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;

      cbGenre.Sorted = true;

      // Fill the Genre Combo Box
      cbGenre.Items.AddRange(Genres.Audio);

      // Add Possible Custom Genres
      foreach (string customGenre in Options.MainSettings.CustomGenres)
      {
        cbGenre.Items.Add(customGenre);
      }

      // Fill the Picture Type Box
      Type picTypes = typeof(PictureType);
      foreach (string type in Enum.GetNames(picTypes))
        cbPicType.Items.Add(type);

      checkBoxRemoveExistingPictures.Checked = Options.MainSettings.ClearExistingPictures;

      // Fill Comments Languages
      cbCommentLanguage.DataSource = Util.ISO_LANGUAGES;
      cbCommentLanguage.Text = "eng - English";

      // Fill Lyrics Language
      cbLyricsLanguage.DataSource = Util.ISO_LANGUAGES;
      cbLyricsLanguage.Text = "eng - English";

      // Fill the Media Type Combo Box
      cbMediaType.Items.Add("ANA (Other analogue media)");
      cbMediaType.Items.Add("CD (CD)");
      cbMediaType.Items.Add("DAT (DAT)");
      cbMediaType.Items.Add("DCC (DCC)");
      cbMediaType.Items.Add("DIG (Other digital media)");
      cbMediaType.Items.Add("DVD (DVD)");
      cbMediaType.Items.Add("LD (Laserdisc)");
      cbMediaType.Items.Add("MC (MC Normal Cassette)");
      cbMediaType.Items.Add("MD (MiniDisc)");
      cbMediaType.Items.Add("RAD (Radio)");
      cbMediaType.Items.Add("REE (REE)");
      cbMediaType.Items.Add("TEL (Telephone)");
      cbMediaType.Items.Add("TT (Turntable records)");
      cbMediaType.Items.Add("TV (Television)");
      cbMediaType.Items.Add("VID (Video)");
      cbMediaType.SelectedIndex = 1;

      Localisation();

      tabControlTagEdit.SelectFirstTab();

      if (Options.MainSettings.UseMediaPortalDatabase && Options.MediaPortalArtists != null)
      {
        // Add Auto Complete Option for Artist
        AutoCompleteStringCollection customSource = new AutoCompleteStringCollection();
        customSource.AddRange(Options.MediaPortalArtists);

        cbArtist.AutoCompleteCustomSource = customSource;
        cbArtist.AutoCompleteSource = AutoCompleteSource.CustomSource;
        cbArtist.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

        tbArtist.AutoCompleteCustomSource = customSource;
        tbArtist.AutoCompleteSource = AutoCompleteSource.CustomSource;
        tbArtist.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

        cbAlbumArtist.AutoCompleteCustomSource = customSource;
        cbAlbumArtist.AutoCompleteSource = AutoCompleteSource.CustomSource;
        cbAlbumArtist.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

        tbAlbumArtist.AutoCompleteCustomSource = customSource;
        tbAlbumArtist.AutoCompleteSource = AutoCompleteSource.CustomSource;
        tbAlbumArtist.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      }

      dataGridViewUserFrames.CellValueChanged += dataGridViewUserFrames_CellValueChanged;

      ChangeCheckboxStatus(false);

      // Register Main Form Closing event, so that we can store values set in the control
      if (ParentForm != null)
      {
        ParentForm.FormClosing += OnLeave;
      }
    }

    private void Localisation()
    {
      log.Trace(">>>");
      Descriptor.HeaderText = localisation.ToString("TagEdit", "CommentHeaderDescriptor");
      Language.HeaderText = localisation.ToString("TagEdit", "CommentHeaderLanguage");
      Comment.HeaderText = localisation.ToString("TagEdit", "CommentHeaderComment");
      PictureType.HeaderText = localisation.ToString("TagEdit", "PictureHeaderType");
      dataGridViewTextBoxColumn4.HeaderText = localisation.ToString("TagEdit", "MusicianHeaderInstrument");
      dataGridViewTextBoxColumn6.HeaderText = localisation.ToString("TagEdit", "MusicianHeaderName");
      dataGridViewTextBoxColumn3.HeaderText = localisation.ToString("TagEdit", "PersonHeaderName");
      dataGridViewTextBoxColumn5.HeaderText = localisation.ToString("TagEdit", "PersonHeaderFunction");
      dataGridViewTextBoxColumn7.HeaderText = localisation.ToString("TagEdit", "LyricsHeaderDescriptor");
      dataGridViewTextBoxColumn8.HeaderText = localisation.ToString("TagEdit", "LyricsHeaderLanguage");
      dataGridViewTextBoxColumn9.HeaderText = localisation.ToString("TagEdit", "LyricsHeaderLyrics");

      FrameID.HeaderText = localisation.ToString("TagEdit", "FrameID");
      FrameDesc.HeaderText = localisation.ToString("TagEdit", "FrameDesc");
      FrameValue.HeaderText = localisation.ToString("TagEdit", "FrameText");
      log.Trace("<<<");
    }


    #endregion

    #region Form Close

    private void OnLeave(object sender, EventArgs e)
    {
      Options.MainSettings.ClearExistingPictures = checkBoxRemoveExistingPictures.Checked;
      Options.SaveAllSettings();
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Whenever a track is selected in the Gridview, fill the Quickedit panel with data
    /// </summary>
    public void FillForm()
    {
      log.Trace(">>>");

      int i = 0;
      string strGenreTemp = "";
      string strCommentTemp = "";
      string strUserFramesTemp = "";
      string strInvoledPeopleTemp = "";
      string strMusicianCreditList = "";

      ClearForm();
      EnableTextBoxEvents(false);

      // Do we have Multiple Rows selected
      if (main.TracksGridView.View.SelectedRows.Count > 1)
      {
        _isMultiTagEdit = true;
        _selectedRowIndex = -1;
        _pictures.Clear();
        UncheckCheckboxes();
        ChangeCheckboxStatus(true);

        // Show the Combo boxes, which are only available for Multi Tag Edit
        cbArtist.Visible = true;
        cbAlbumArtist.Visible = true;
        cbAlbum.Visible = true;

        ckTrackLength.Visible = true;
        tbTrackLength.Visible = false;
        btGetTrackLength.Visible = false;
        btGetLyricsFromText.Visible = false;

        btPrevious.Enabled = false;
        btNext.Enabled = false;
      }
      else
      {
        _isMultiTagEdit = false;
        if (main.TracksGridView.View.SelectedRows.Count > 0)
        {
          _selectedRowIndex = main.TracksGridView.View.SelectedRows[0].Index;
        }
        ChangeCheckboxStatus(false);

        // Hide the Combo boxes, which are only available for Multi Tag Edit
        cbArtist.Visible = false;
        cbAlbumArtist.Visible = false;
        cbAlbum.Visible = false;

        ckTrackLength.Visible = false;
        tbTrackLength.Visible = true;
        btGetTrackLength.Visible = true;
        btGetLyricsFromText.Visible = true;

        btPrevious.Enabled = true;
        btNext.Enabled = true;
      }

      foreach (DataGridViewRow row in main.TracksGridView.View.SelectedRows)
      {

        TrackData track = Options.Songlist[row.Index];

        if (!_isMultiTagEdit)
        {
          lbEditedFile.Text = track.FileName;
        }
        else
        {
          lbEditedFile.Text = "";
        }

        #region Main Tags

        if (_isMultiTagEdit)
        {
          if (cbArtist.Text.Trim() != track.Artist.Trim())
          {
            if (i == 0)
            {
              cbArtist.Text = track.Artist;
            }
            else
            {
              cbArtist.Text = "";
            }
          }

          if (cbAlbumArtist.Text.Trim() != track.AlbumArtist.Trim())
          {
            if (i == 0)
            {
              cbAlbumArtist.Text = track.AlbumArtist;
            }
            else
            {
              cbAlbumArtist.Text = "";
            }
          }

          if (cbAlbum.Text.Trim() != track.Album.Trim())
          {
            if (i == 0)
            {
              cbAlbum.Text = track.Album;
            }
            else
            {
              cbAlbum.Text = "";
            }
          }
        }
        else
        {
          tbArtist.Text = track.Artist;
          tbAlbumArtist.Text = track.AlbumArtist;
          tbAlbum.Text = track.Album;
        }

        if (_isMultiTagEdit)
        {
          if (checkBoxCompilation.Checked != track.Compilation)
          {
            if (i == 0)
            {
              checkBoxCompilation.Checked = track.Compilation;
            }
            else
            {
              checkBoxCompilation.Checked = false;
            }
          }
        }
        else
        {
          checkBoxCompilation.Checked = track.Compilation;
        }

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

        if (tbDisc.Text.Trim() != track.DiscNumber.ToString())
        {
          if (i == 0 && track.DiscNumber != 0)
          {
            tbDisc.Text = track.DiscNumber.ToString();
          }
          else
          {
            tbDisc.Text = "";
          }
        }

        if (tbNumDiscs.Text.Trim() != track.DiscCount.ToString())
        {
          if (i == 0 && track.DiscCount != 0)
          {
            tbNumDiscs.Text = track.DiscCount.ToString();
          }
          else
          {
            tbNumDiscs.Text = "";
          }
        }

        // Don't handle single track for Multitag Edit
        if (!_isMultiTagEdit)
        {
          if (track.TrackNumber != 0)
          {
            tbTrack.Text = track.TrackNumber.ToString();
          }
        }

        if (tbNumTracks.Text.Trim() != track.TrackCount.ToString())
        {
          if (i == 0 && track.TrackCount != 0)
            tbNumTracks.Text = track.TrackCount.ToString();
          else
            tbNumTracks.Text = "";
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
        if (track.IsMp3)
        {
          sComment += track.Comment;
          foreach (Comment commentsframe in track.ID3Comments)
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
            if (track.IsMp3)
            {
              AddComment("", "", track.Comment);
              foreach (Comment commentsframe in track.ID3Comments)
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

        #region Picture

        // Pictures are only filled for Single Tagedit
        if (!_isMultiTagEdit)
        {
          tbPicDesc.Text = "";
          dataGridViewPicture.Rows.Clear();
          _pictures.Clear();
          foreach (Picture picture in track.Pictures)
          {
            dataGridViewPicture.Rows.Add(new object[] { picture.Description, Enum.Format(typeof(PictureType), picture.Type, "G") });
            _pictures.Add(picture);
          }

          // Display first Picture
          if (dataGridViewPicture.Rows.Count > 0)
          {
            DataGridViewCellEventArgs evt = new DataGridViewCellEventArgs(0, 0);
            dataGridViewPicture_CellClick(dataGridViewPicture, evt);
          }
        }
        
        #endregion

        #region Detailed Information

        if (tbConductor.Text.Trim() != track.Conductor.Trim())
        {
          if (i == 0)
          {
            tbConductor.Text = track.Conductor;
          }
          else
          {
            tbConductor.Text = "";
          }
        }

        if (tbComposer.Text.Trim() != track.Composer.Trim())
        {
          if (i == 0)
          {
            tbComposer.Text = track.Composer;
          }
          else
          {
            tbComposer.Text = "";
          }
        }

        if (tbCopyright.Text.Trim() != track.Copyright.Trim())
        {
          if (i == 0)
          {
            tbCopyright.Text = track.Copyright;
          }
          else
          {
            tbCopyright.Text = "";
          }
        }

        if (tbContentGroup.Text.Trim() != track.Grouping.Trim())
        {
          if (i == 0)
          {
            tbContentGroup.Text = track.Grouping;
          }
          else
          {
            tbContentGroup.Text = "";
          }
        }

        if (tbTrackLength.Text.Trim() != track.TrackLength.Trim())
        {
          if (i == 0)
          {
            tbTrackLength.Text = track.TrackLength;
          }
          else
          {
            tbTrackLength.Text = "";
          }
        }

        if (tbArtistSort.Text.Trim() != track.ArtistSortName.Trim())
        {
          if (i == 0)
          {
            tbArtistSort.Text = track.ArtistSortName;
          }
          else
          {
            tbArtistSort.Text = "";
          }
        }

        if (tbAlbumArtistSort.Text.Trim() != track.AlbumArtistSortName.Trim())
        {
          if (i == 0)
          {
            tbAlbumArtistSort.Text = track.AlbumArtistSortName;
          }
          else
          {
            tbAlbumArtistSort.Text = "";
          }
        }

        if (tbAlbumSort.Text.Trim() != track.AlbumSortName.Trim())
        {
          if (i == 0)
          {
            tbAlbumSort.Text = track.AlbumSortName;
          }
          else
          {
            tbAlbumSort.Text = "";
          }
        }

        if (tbTitleSort.Text.Trim() != track.TitleSortName.Trim())
        {
          if (i == 0)
          {
            tbTitleSort.Text = track.TitleSortName;
          }
          else
          {
            tbTitleSort.Text = "";
          }
        }

        // The following values are only ID3 V2 specific
        if (track.IsMp3)
        {
          if (tbInterpretedBy.Text.Trim() != track.Interpreter.Trim())
          {
            if (i == 0)
            {
              tbInterpretedBy.Text = track.Interpreter;
            }
            else
            {
              tbInterpretedBy.Text = "";
            }
          }

          if (tbTextWriter.Text.Trim() != track.TextWriter.Trim())
          {
            if (i == 0)
            {
              tbTextWriter.Text = track.TextWriter;
            }
            else
            {
              tbTextWriter.Text = "";
            }
          }

          if (tbPublisher.Text.Trim() != track.Publisher.Trim())
          {
            if (i == 0)
            {
              tbPublisher.Text = track.Publisher;
            }
            else
            {
              tbPublisher.Text = "";
            }
          }

          if (tbEncodedBy.Text.Trim() != track.EncodedBy.Trim())
          {
            if (i == 0)
            {
              tbEncodedBy.Text = track.EncodedBy;
            }
            else
            {
              tbEncodedBy.Text = "";
            }
          }

          if (tbSubTitle.Text.Trim() != track.SubTitle.Trim())
          {
            if (i == 0)
            {
              tbSubTitle.Text = track.SubTitle;
            }
            else
            {
              tbSubTitle.Text = "";
            }
          }
        }

        #endregion

        #region Original Information

        // The following values are only ID3 V2 specific
        if (track.IsMp3)
        {
          if (tbOriginalAlbum.Text.Trim() != track.OriginalAlbum.Trim())
          {
            if (i == 0)
            {
              tbOriginalAlbum.Text = track.OriginalAlbum;
            }
            else
            {
              tbOriginalAlbum.Text = "";
            }
          }

          if (tbOriginalFileName.Text.Trim() != track.OriginalFileName.Trim())
          {
            if (i == 0)
            {
              tbOriginalFileName.Text = track.OriginalFileName;
            }
            else
            {
              tbOriginalFileName.Text = "";
            }
          }

          if (tbOriginalLyricsWriter.Text.Trim() != track.OriginalLyricsWriter.Trim())
          {
            if (i == 0)
            {
              tbOriginalLyricsWriter.Text = track.OriginalLyricsWriter;
            }
            else
            {
              tbOriginalLyricsWriter.Text = "";
            }
          }

          if (tbOriginalArtist.Text.Trim() != track.OriginalArtist.Trim())
          {
            if (i == 0)
            {
              tbOriginalArtist.Text = track.OriginalArtist;
            }
            else
            {
              tbOriginalArtist.Text = "";
            }
          }

          if (tbOriginalOwner.Text.Trim() != track.OriginalOwner.Trim())
          {
            if (i == 0)
            {
              tbOriginalOwner.Text = track.OriginalOwner;
            }
            else
            {
              tbOriginalOwner.Text = "";
            }
          }

          if (tbOriginalRelease.Text.Trim() != track.OriginalRelease.Trim())
          {
            if (i == 0)
            {
              tbOriginalRelease.Text = track.OriginalRelease;
            }
            else
            {
              tbOriginalRelease.Text = "";
            }
          }
        }

        #endregion

        #region Involved People

        // The following values are only ID3 V2 specific
        if (track.IsMp3)
        {
          if (strInvoledPeopleTemp != track.InvolvedPeople)
          {
            if (i == 0)
            {
              // A IPLS is delimited with "\0"
              // A TIPL is delimited with ";"
              string[] ipls = track.InvolvedPeople.Split(new[] { '\0', ';' });
              for (int j = 0; j < ipls.Length - 1; j += 2)
              {
                dataGridViewInvolvedPeople.Rows.Add(new object[] { ipls[j].Trim(), ipls[j + 1].Trim() });
              }
              strInvoledPeopleTemp = track.InvolvedPeople;
            }
            else
              dataGridViewInvolvedPeople.Rows.Clear();
          }

          // TMCL is only available in 2.4
          if (Options.MainSettings.ID3V2Version == 4)
          {
            groupBoxMusician.Enabled = true;
            if (strMusicianCreditList != track.MusicCreditList)
            {
              if (i == 0)
              {
                string[] mcl = track.MusicCreditList.Split(new[] {'\0', ';'});
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
          else
          {
            groupBoxMusician.Enabled = false;
          }
        }

        #endregion

        #region Web Information

        // The following values are only ID3 V2 specific
        if (track.IsMp3)
        {
          if (tbCopyrightUrl.Text.Trim() != track.CopyrightInformation.Trim())
          {
            if (i == 0)
            {
              tbCopyrightUrl.Text = track.CopyrightInformation;
            }
            else
            {
              tbCopyrightUrl.Text = "";
            }
          }

          if (tbOfficialAudioFileUrl.Text.Trim() != track.OfficialAudioFileInformation.Trim())
          {
            if (i == 0)
            {
              tbOfficialAudioFileUrl.Text = track.OfficialAudioFileInformation;
            }
            else
            {
              tbOfficialAudioFileUrl.Text = "";
            }
          }

          if (tbOfficialArtistUrl.Text.Trim() != track.OfficialArtistInformation.Trim())
          {
            if (i == 0)
            {
              tbOfficialArtistUrl.Text = track.OfficialArtistInformation;
            }
            else
            {
              tbOfficialArtistUrl.Text = "";
            }
          }

          if (tbOfficialAudioSourceUrl.Text.Trim() != track.OfficialAudioSourceInformation.Trim())
          {
            if (i == 0)
            {
              tbOfficialAudioSourceUrl.Text = track.OfficialAudioSourceInformation;
            }
            else
            {
              tbOfficialAudioSourceUrl.Text = "";
            }
          }

          if (tbOfficialInternetRadioUrl.Text.Trim() != track.OfficialInternetRadioInformation.Trim())
          {
            if (i == 0)
            {
              tbOfficialInternetRadioUrl.Text = track.OfficialInternetRadioInformation;
            }
            else
            {
              tbOfficialInternetRadioUrl.Text = "";
            }
          }

          if (tbOfficialPaymentUrl.Text.Trim() != track.OfficialPaymentInformation.Trim())
          {
            if (i == 0)
            {
              tbOfficialPaymentUrl.Text = track.OfficialPaymentInformation;
            }
            else
            {
              tbOfficialPaymentUrl.Text = "";
            }
          }

          if (tbOfficialPublisherUrl.Text.Trim() != track.OfficialPublisherInformation.Trim())
          {
            if (i == 0)
            {
              tbOfficialPublisherUrl.Text = track.OfficialPublisherInformation;
            }
            else
            {
              tbOfficialPublisherUrl.Text = "";
            }
          }

          if (tbCommercialInformationUrl.Text.Trim() != track.CommercialInformation.Trim())
          {
            if (i == 0)
            {
              tbCommercialInformationUrl.Text = track.CommercialInformation;
            }
            else
            {
              tbCommercialInformationUrl.Text = "";
            }
          }
        }

        #endregion

        #region User Defined Frames

        // Build tem string for comparison
        string sFrames = "";
        foreach (Frame frame in track.UserFrames)
        {
          sFrames += frame.Id + frame.Description + frame.Value;
        }

        if (strUserFramesTemp != sFrames)
        {
          if (i == 0)
          {
            strUserFramesTemp = sFrames;
            foreach (Frame frame in track.UserFrames)
            {
              dataGridViewUserFrames.Rows.Add(new object[] { frame.Id, frame.Description, frame.Value });
            }

          }
          else
            dataGridViewUserFrames.Rows.Clear();
        }

        if (track.UserFrames.Count > 0)
        {
          tabPageUserDefined.DefaultSmallImage = Properties.Resources.Warning;
        }

        #endregion

        if (!_isMultiTagEdit)
        {
          #region Lyrics

          dataGridViewLyrics.Rows.Clear();
          if (track.IsMp3)
          {
            foreach (Lyric lyricsframe in track.LyricsFrames)
            {
              AddLyrics(lyricsframe.Description, lyricsframe.Language, lyricsframe.Text);
            }
          }
          else
          {
            if (track.Lyrics != "")
            {
              AddLyrics("", "", track.Lyrics);
            }
          }

          #endregion

          #region Rating

          dataGridViewRating.Rows.Clear();
          if (track.IsMp3)
          {
            foreach (PopmFrame popmframe in track.Ratings)
            {
              AddRating(popmframe.User, Convert.ToString(popmframe.Rating), Convert.ToString(popmframe.PlayCount));
            }
          }
          if (track.IsMp3)
            groupBoxRating.Enabled = true;
          else
            groupBoxRating.Enabled = false;

          #endregion
        }

        i++;
      }

      if (_isMultiTagEdit)
      {
        // Auto fill the Number of Tracks field
        if (tbNumTracks.Text == "")
        {
          tbNumTracks.Text = main.TracksGridView.View.SelectedRows.Count.ToString().PadLeft(2, '0');
          if (Options.MainSettings.AutoFillNumberOfTracks)
          {
            ckTrack.Checked = true;
          }
        }

        cbArtist.Items.Clear();
        cbAlbumArtist.Items.Clear();
        cbAlbum.Items.Clear();

        // Now see, if we have different values in the Artist and AlbumArtist fields and fill the combo
        List<string> itemsArtist = new List<string>();
        List<string> itemsAlbumArtist = new List<string>();
        List<string> itemsAlbum = new List<string>();
        foreach (DataGridViewRow row in main.TracksGridView.View.SelectedRows)
        {
          TrackData track = Options.Songlist[row.Index];

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

        EnableTextBoxEvents(true);
      }
      log.Trace("<<<");
    }


    /// <summary>
    /// Clear the Form
    /// </summary>
    public void ClearForm()
    {
      lbEditedFile.Text = "";

      cbArtist.Text = "";
      cbAlbumArtist.Text = "";
      tbAlbum.Text = "";
      checkBoxCompilation.Checked = false;
      tbTitle.Text = "";
      cbGenre.Text = "";
      tbYear.Text = "";
      tbTrack.Text = "";
      tbNumTracks.Text = "";
      tbDisc.Text = "";
      tbNumTracks.Text = "";

      listBoxGenre.Items.Clear();
      dataGridViewComment.Rows.Clear();
      dataGridViewPicture.Rows.Clear();
      dataGridViewInvolvedPeople.Rows.Clear();
      dataGridViewMusician.Rows.Clear();
      dataGridViewLyrics.Rows.Clear();
      dataGridViewRating.Rows.Clear();
      dataGridViewUserFrames.Rows.Clear();

      tabPageUserDefined.DefaultSmallImage = null;

      checkBoxRemoveComments.Checked = false;
      textBoxComment.Text = "";
      cbCommentDescriptor.Items.Clear();
      cbAlbum.Items.Clear();
      tbNumDiscs.Text = "";
      tbBPM.Text = "";
      tbArtist.Text = "";
      tbAlbumArtist.Text = "";
      checkBoxRemoveExistingPictures.Checked = Options.MainSettings.ClearExistingPictures;
      pictureBoxCover.Image = null;
      tbPicDesc.Text = "";
      tbTitleSort.Text = "";
      tbAlbumSort.Text = "";
      tbArtistSort.Text = "";
      tbAlbumArtistSort.Text = "";
      tbSubTitle.Text = "";
      tbContentGroup.Text = "";
      tbCopyright.Text = "";
      tbEncodedBy.Text = "";
      tbPublisher.Text = "";
      tbTextWriter.Text = "";
      tbInterpretedBy.Text = "";
      tbComposer.Text = "";
      tbConductor.Text = "";
      tabPageOriginal.Text = "";
      tbOriginalRelease.Text = "";
      tbOriginalOwner.Text = "";
      tbOriginalArtist.Text = "";
      tbOriginalLyricsWriter.Text = "";
      tbOriginalFileName.Text = "";
      tbOriginalAlbum.Text = "";
      tbMusicianName.Text = "";
      tbMusicianInstrument.Text = "";
      tbInvolvedPersonName.Text = "";
      tbInvolvedPersonFunction.Text = "";
      tbCommercialInformationUrl.Text = "";
      tbOfficialPublisherUrl.Text = "";
      tbOfficialPaymentUrl.Text = "";
      tbOfficialInternetRadioUrl.Text = "";
      tbOfficialAudioSourceUrl.Text = "";
      tbOfficialArtistUrl.Text = "";
      tbOfficialAudioFileUrl.Text = "";
      tbCopyrightUrl.Text = "";
      tbLyrics.Text = "";
      cbLyricsDescriptor.Items.Clear();
      numericUpDownPlayCounter.Text = "0";
      numericUpDownRating.Text = "0";
      tbRatingUser.Text = "";
    }

    #endregion

    #region Private Methods

    #region General

    /// <summary>
    /// Changes the Checkboxes behind the input fields to visible / Invisble
    /// </summary>
    /// <param name="visible"></param>
    private void ChangeCheckboxStatus(bool visible)
    {
      ckTrack.Visible = visible;
      ckDisk.Visible = visible;
      ckArtist.Visible = visible;
      ckAlbumArtist.Visible = visible;
      ckGenre.Visible = visible;
      ckAlbum.Visible = visible;
      ckTitle.Visible = visible;
      ckYear.Visible = visible;
      ckBPM.Visible = visible;
      ckConductor.Visible = visible;
      ckComposer.Visible = visible;
      ckInterpretedBy.Visible = visible;
      ckTextWriter.Visible = visible;
      ckPublisher.Visible = visible;
      ckEncodedBy.Visible = visible;
      ckCopyright.Visible = visible;
      ckContentGroup.Visible = visible;
      ckSubTitle.Visible = visible;
      ckArtistSort.Visible = visible;
      ckAlbumArtistSort.Visible = visible;
      ckAlbumSort.Visible = visible;
      ckTitleSort.Visible = visible;
      ckOriginalAlbum.Visible = visible;
      ckOriginalArtist.Visible = visible;
      ckOriginalFileName.Visible = visible;
      ckOriginalLyricsWriter.Visible = visible;
      ckOriginalOwner.Visible = visible;
      ckOriginalRelease.Visible = visible;
      ckCopyrightUrl.Visible = visible;
      ckOfficialAudioFileUrl.Visible = visible;
      ckOfficialArtistUrl.Visible = visible;
      ckOfficialAudioSourceUrl.Visible = visible;
      ckOfficialInternetRadioUrl.Visible = visible;
      ckOfficialPaymentUrl.Visible = visible;
      ckCommercialInformationUrl.Visible = visible;
      ckOfficialPublisherUrl.Visible = visible;
      ckInvolvedPerson.Visible = visible;
      ckInvolvedMusician.Visible = visible;
      ckMediaType.Visible = visible;
      ckTrackLength.Visible = visible;
    }

    /// <summary>
    /// Changes the Checkboxes behind the input fields to visible / Invisble
    /// </summary>
    /// <param name="visible"></param>
    private void UncheckCheckboxes()
    {
      ckTrack.Checked = false;
      ckDisk.Checked = false;
      ckArtist.Checked = false;
      ckAlbumArtist.Checked = false;
      ckGenre.Checked = false;
      ckAlbum.Checked = false;
      ckTitle.Checked = false;
      ckYear.Checked = false;
      ckBPM.Checked = false;
      ckConductor.Checked = false;
      ckComposer.Checked = false;
      ckInterpretedBy.Checked = false;
      ckTextWriter.Checked = false;
      ckPublisher.Checked = false;
      ckEncodedBy.Checked = false;
      ckCopyright.Checked = false;
      ckContentGroup.Checked = false;
      ckSubTitle.Checked = false;
      ckArtistSort.Checked = false;
      ckAlbumSort.Checked = false;
      ckAlbumSort.Checked = false;
      ckTitleSort.Checked = false;
      ckOriginalAlbum.Checked = false;
      ckOriginalArtist.Checked = false;
      ckOriginalFileName.Checked = false;
      ckOriginalLyricsWriter.Checked = false;
      ckOriginalOwner.Checked = false;
      ckOriginalRelease.Checked = false;
      ckCopyrightUrl.Checked = false;
      ckOfficialAudioFileUrl.Checked = false;
      ckOfficialArtistUrl.Checked = false;
      ckOfficialAudioSourceUrl.Checked = false;
      ckOfficialInternetRadioUrl.Checked = false;
      ckOfficialPaymentUrl.Checked = false;
      ckCommercialInformationUrl.Checked = false;
      ckOfficialPublisherUrl.Checked = false;
      ckInvolvedPerson.Checked = false;
      ckInvolvedMusician.Checked = false;
      ckRemoveExistingRatings.Checked = false;
      ckRemoveLyrics.Checked = false;
      ckMediaType.Checked = false;
      checkBoxRemoveComments.Checked = false;
      checkBoxRemoveExistingPictures.Checked = Options.MainSettings.ClearExistingPictures;
      ckTrackLength.Checked = false;
    }

    /// <summary>
    /// Enable / Disable the TextBoc Changed event, used for setting the Combo Boxes on Multi Edit
    /// </summary>
    /// <param name="enabled"></param>
    private void EnableTextBoxEvents(bool enabled)
    {
      if (enabled)
      {
        tbYear.TextChanged += OnTextChanged;
        tbTitle.TextChanged += OnTextChanged;
        cbAlbum.TextChanged += OnComboChanged;
        cbArtist.TextChanged += OnComboChanged;
        tbTrack.TextChanged += OnTextChanged;
        cbAlbumArtist.TextChanged += OnComboChanged;
        cbGenre.TextChanged += OnComboChanged;
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
        tbAlbumArtistSort.TextChanged += OnTextChanged;
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
      }
      else
      {
        tbYear.TextChanged -= OnTextChanged;
        tbTitle.TextChanged -= OnTextChanged;
        cbAlbum.TextChanged -= OnComboChanged;
        cbArtist.TextChanged -= OnComboChanged;
        tbTrack.TextChanged -= OnTextChanged;
        cbAlbumArtist.TextChanged -= OnComboChanged;
        cbGenre.TextChanged -= OnComboChanged;
        tbNumTracks.TextChanged -= OnTextChanged;
        tbBPM.TextChanged -= OnTextChanged;
        tbNumDiscs.TextChanged -= OnTextChanged;
        tbDisc.TextChanged -= OnTextChanged;
        tbConductor.TextChanged -= OnTextChanged;
        tbComposer.TextChanged -= OnTextChanged;
        tbInterpretedBy.TextChanged -= OnTextChanged;
        tbTextWriter.TextChanged -= OnTextChanged;
        tbPublisher.TextChanged -= OnTextChanged;
        tbEncodedBy.TextChanged -= OnTextChanged;
        tbCopyright.TextChanged -= OnTextChanged;
        tbContentGroup.TextChanged -= OnTextChanged;
        tbSubTitle.TextChanged -= OnTextChanged;
        tbArtistSort.TextChanged -= OnTextChanged;
        tbAlbumArtistSort.TextChanged -= OnTextChanged;
        tbAlbumSort.TextChanged -= OnTextChanged;
        tbTitleSort.TextChanged -= OnTextChanged;
        cbMediaType.TextChanged -= OnComboChanged;
        tbOfficialArtistUrl.TextChanged -= OnTextChanged;
        tbOfficialAudioFileUrl.TextChanged -= OnTextChanged;
        tbOfficialAudioSourceUrl.TextChanged -= OnTextChanged;
        tbOfficialInternetRadioUrl.TextChanged -= OnTextChanged;
        tbOfficialPaymentUrl.TextChanged -= OnTextChanged;
        tbOfficialPublisherUrl.TextChanged -= OnTextChanged;
        tbCommercialInformationUrl.TextChanged -= OnTextChanged;
        tbCopyrightUrl.TextChanged -= OnTextChanged;
      }
    }

    #endregion

    #region Comment
    private void AddComment(string desc, string lang, string text)
    {
      if (text == null)
        return;

      bool found = false;
      // ID3 V2 comments without a language have XXX, which we will ignore
      if (lang == "XXX")
        lang = "";

      if (lang.Length > 3)
        lang = lang.Substring(0, 3);

      for (int i = 0; i < dataGridViewComment.Rows.Count; i++)
      {
        if (desc == dataGridViewComment.Rows[i].Cells[0].Value.ToString() &&
            lang == dataGridViewComment.Rows[i].Cells[1].Value.ToString())
        {
          dataGridViewComment.Rows[i].Cells[2].Value = text;
          found = true;
          break;
        }
      }

      if (!found)
        dataGridViewComment.Rows.Add(new object[] { desc, lang, text });
    }

    #endregion

    #region Picture
    /// <summary>
    ///   Add a picture to the Picture box
    /// </summary>
    private void AddPictureToPictureBox()
    {
      if (_pic != null)
      {
        pictureBoxCover.Image = Picture.ImageFromData(_pic.Data);
      }
    }

    /// <summary>
    ///   Add the currently selected Picture to the collection
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void AddPictureToList()
    {
      if (_pic != null)
      {
        _pic.Description = tbPicDesc.Text;
        _pic.Type =
          (PictureType)
          Enum.Parse(typeof(PictureType),
                     cbPicType.SelectedItem != null ? cbPicType.SelectedItem.ToString() : "FrontCover");

        dataGridViewPicture.Rows.Add(new object[] { _pic.Description, Enum.Format(typeof(PictureType), _pic.Type, "G") });

        _pictures.Add(_pic);
        pictureBoxCover.Image = null;
        _pictureIsChanged = true;
      }
    }

    #endregion

    #region Rating

    private void AddRating(string user, string rating, string playcount)
    {
      bool found = false;
      for (int i = 0; i < dataGridViewRating.Rows.Count; i++)
      {
        if (user == dataGridViewRating.Rows[i].Cells[0].Value.ToString())
        {
          dataGridViewRating.Rows[i].Cells[1].Value = rating;
          dataGridViewRating.Rows[i].Cells[2].Value = playcount;
          found = true;
          break;
        }
      }

      if (!found)
        dataGridViewRating.Rows.Add(new object[] { user, rating, playcount });
    }

    #endregion

    #region Lyrics

    private void AddLyrics(string desc, string lang, string text)
    {
      if (text == null)
        return;

      bool found = false;
      if (lang.Length > 3)
        lang = lang.Substring(0, 3);

      for (int i = 0; i < dataGridViewLyrics.Rows.Count; i++)
      {
        if (desc == dataGridViewLyrics.Rows[i].Cells[0].Value.ToString() &&
            lang == dataGridViewLyrics.Rows[i].Cells[1].Value.ToString())
        {
          dataGridViewLyrics.Rows[i].Cells[2].Value = text;
          found = true;
          break;
        }
      }

      if (!found)
        dataGridViewLyrics.Rows.Add(new object[] { desc, lang, text });
    }

    #endregion

    #endregion

    #region Event Handler

    /// <summary>
    ///   Apply the Changes
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btApply_Click(object sender, EventArgs e)
    {
      log.Trace(">>>");
      bool bErrors = false;
      DataGridView tracksGrid = main.TracksGridView.View;

      foreach (DataGridViewRow row in tracksGrid.SelectedRows)
      {
        bool trackChanged = false;
        TrackData track = Options.Songlist[row.Index];

        main.TracksGridView.ClearStatusColumn(row.Index);

        try
        {
          #region Main Tags

          if (_isMultiTagEdit)
          {
            if (ckArtist.Checked)
            {
              track.Artist = cbArtist.Text.Trim();
              trackChanged = true;
            }

            if (ckAlbumArtist.Checked)
            {
              track.AlbumArtist = cbAlbumArtist.Text.Trim();
              trackChanged = true;
            }

            if (ckAlbum.Checked)
            {
              track.Album = cbAlbum.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Artist != tbArtist.Text.Trim())
            {
              track.Artist = tbArtist.Text.Trim();
              trackChanged = true;
            }

            if (track.AlbumArtist != tbAlbumArtist.Text.Trim())
            {
              track.AlbumArtist = tbAlbumArtist.Text.Trim();
              trackChanged = true;
            }

            if (track.Album != tbAlbum.Text.Trim())
            {
              track.Album = tbAlbum.Text.Trim();
              trackChanged = true;
            }
          }

          if (track.Compilation != checkBoxCompilation.Checked)
          {
            track.Compilation = checkBoxCompilation.Checked;
            trackChanged = true;
          }

          if (_isMultiTagEdit)
          {
            if (ckTitle.Checked)
            {
              track.Title = tbTitle.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Title != tbTitle.Text.Trim())
            {
              track.Title = tbTitle.Text.Trim();
              trackChanged = true;
            }
          }

          int parsedYear = 0;
          try
          {
            parsedYear = tbYear.Text.Trim() == "" ? 0 : Int32.Parse(tbYear.Text.Trim());
          }
          catch (Exception)
          { }

          if (_isMultiTagEdit)
          {
            if (ckYear.Checked)
            {
              track.Year = parsedYear;
              trackChanged = true;
            }
          }
          else
          {
            if (parsedYear != track.Year)
            {
              track.Year = parsedYear;
              trackChanged = true;
            }
          }

          int parsedBPM = 0;
          try
          {
            parsedBPM = tbBPM.Text.Trim() == "" ? 0 : Int32.Parse(tbBPM.Text.Trim());
          }
          catch (Exception)
          { }

          if (_isMultiTagEdit)
          {
            if (ckBPM.Checked)
            {
              track.BPM = parsedBPM;
              trackChanged = true;
            }
          }
          else
          {
            if (parsedBPM != track.BPM)
            {
              track.BPM = parsedBPM;
              trackChanged = true;
            }
          }

          int tracknumber = 0;
          int trackcount = 0;

          try
          {
            tracknumber = tbTrack.Text.Trim() == "" ? 0 : Int32.Parse(tbTrack.Text.Trim());
          }
          catch (Exception) { }
          try
          {
            trackcount = tbNumTracks.Text.Trim() == "" ? 0 : Int32.Parse(tbNumTracks.Text.Trim());
          }
          catch (Exception) { }

          if (!_isMultiTagEdit && tracknumber != track.TrackNumber)
          {
            track.TrackNumber = (uint)tracknumber;
            trackChanged = true;
          }

          if (_isMultiTagEdit)
          {
            if (ckTrack.Checked)
            {
              track.TrackCount = (uint)trackcount;
              trackChanged = true;
            }
          }
          else
          {
            if (trackcount != track.TrackCount)
            {
              track.TrackCount = (uint)trackcount;
              trackChanged = true;
            }
          }

          int discnumber = 0;
          int disccount = 0;

          try
          {
            discnumber = tbDisc.Text.Trim() == "" ? 0 : Int32.Parse(tbDisc.Text.Trim());
          }
          catch (Exception) { }
          try
          {
            disccount = tbNumDiscs.Text.Trim() == "" ? 0 : Int32.Parse(tbNumDiscs.Text.Trim());
          }
          catch (Exception) { }

          if (discnumber != track.DiscNumber)
          {
            if (_isMultiTagEdit)
            {
              if (ckDisk.Checked)
              {
                track.DiscNumber = (uint)discnumber;
                trackChanged = true;
              }
            }
            else
            {
              track.DiscNumber = (uint)discnumber;
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckDisk.Checked)
            {
              track.DiscCount = (uint)disccount;
              trackChanged = true;
            }
          }
          else
          {
            if (disccount != track.DiscCount)
            {
              track.DiscCount = (uint)disccount;
              trackChanged = true;
            }
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

          if (_isMultiTagEdit)
          {
            if (ckGenre.Checked)
            {
              track.Genre = genre;
              trackChanged = true;
            }
          }
          else
          {
            if (track.Genre != genre)
            {
              track.Genre = genre;
              trackChanged = true;
            }
          }

          if (checkBoxRemoveComments.Checked)
          {
            if (track.ID3Comments.Count > 0)
            {
              trackChanged = true;
            }
            track.ID3Comments.Clear();
          }

          List<Comment> comments = new List<Comment>();
          foreach (DataGridViewRow commentRow in dataGridViewComment.Rows)
          {
            Comment comment = new Comment(commentRow.Cells[0].Value.ToString(), commentRow.Cells[1].Value.ToString(),
                                          commentRow.Cells[2].Value.ToString());
            comments.Add(comment);
          }

          if (_isMultiTagEdit && (comments.Count > 0 || _commentIsChanged))
          {
            track.ID3Comments.Clear();
            track.ID3Comments.AddRange(comments);
            trackChanged = true;
          }
          else
          {
            if (_commentIsChanged)
            {
              track.ID3Comments.Clear();
              track.ID3Comments.AddRange(comments);
              trackChanged = true;
            }
          }

          #endregion

          #region Picture

          if (checkBoxRemoveExistingPictures.Checked)
          {
            track.Pictures.Clear();
            trackChanged = true;
          }

          if (_pictureIsChanged)
          {
            track.Pictures.Clear();
            track.Pictures.AddRange(_pictures);
            trackChanged = true;
          }

          #endregion

          #region Detailed Information

          if (_isMultiTagEdit)
          {
            if (ckConductor.Checked)
            {
              track.Conductor = tbConductor.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Conductor != tbConductor.Text.Trim())
            {
              track.Conductor = tbConductor.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckComposer.Checked)
            {
              track.Composer = tbComposer.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Composer != tbComposer.Text.Trim())
            {
              track.Composer = tbComposer.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckCopyright.Checked)
            {
              track.Copyright = tbCopyright.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Copyright != tbCopyright.Text.Trim())
            {
              track.Copyright = tbCopyright.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckContentGroup.Checked)
            {
              track.Grouping = tbContentGroup.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Grouping != tbContentGroup.Text.Trim())
            {
              track.Grouping = tbContentGroup.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckInterpretedBy.Checked)
            {
              track.Interpreter = tbInterpretedBy.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Interpreter != tbInterpretedBy.Text.Trim())
            {
              track.Interpreter = tbInterpretedBy.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckTextWriter.Checked)
            {
              track.TextWriter = tbTextWriter.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.TextWriter != tbTextWriter.Text.Trim())
            {
              track.TextWriter = tbTextWriter.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckPublisher.Checked)
            {
              track.Publisher = tbPublisher.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.Publisher != tbPublisher.Text.Trim())
            {
              track.Publisher = tbPublisher.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckEncodedBy.Checked)
            {
              track.EncodedBy = tbEncodedBy.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.EncodedBy != tbEncodedBy.Text.Trim())
            {
              track.EncodedBy = tbEncodedBy.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckSubTitle.Checked)
            {
              track.SubTitle = tbSubTitle.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.SubTitle != tbSubTitle.Text.Trim())
            {
              track.SubTitle = tbSubTitle.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckMediaType.Checked)
            {
              track.MediaType = cbMediaType.SelectedText;
              trackChanged = true;
            }
          }
          else
          {
            if (track.MediaType != cbMediaType.SelectedText)
            {
              track.MediaType = cbMediaType.SelectedText;
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckTrackLength.Checked)
            {
              track.TrackLength = track.DurationTimespan.TotalMilliseconds.ToString();
              trackChanged = true;
            }
          }
          else
          {
            if (track.TrackLength != tbTrackLength.Text)
            {
              track.TrackLength = tbTrackLength.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckArtistSort.Checked)
            {
              track.ArtistSortName = tbArtistSort.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.ArtistSortName != tbArtistSort.Text)
            {
              track.ArtistSortName = tbArtistSort.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckAlbumArtistSort.Checked)
            {
              track.AlbumArtistSortName = tbAlbumArtistSort.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.AlbumArtistSortName != tbAlbumArtistSort.Text)
            {
              track.AlbumArtistSortName = tbAlbumArtistSort.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckAlbumSort.Checked)
            {
              track.AlbumSortName = tbAlbumSort.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.AlbumSortName != tbAlbumSort.Text)
            {
              track.AlbumSortName = tbAlbumSort.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckTitleSort.Checked)
            {
              track.TitleSortName = tbTitleSort.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.TitleSortName != tbTitleSort.Text)
            {
              track.TitleSortName = tbTitleSort.Text.Trim();
              trackChanged = true;
            }
          }

          #endregion

          #region Original Information

          if (_isMultiTagEdit)
          {
            if (ckOriginalArtist.Checked)
            {
              track.OriginalArtist = tbOriginalArtist.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OriginalArtist != tbOriginalArtist.Text)
            {
              track.OriginalArtist = tbOriginalArtist.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOriginalAlbum.Checked)
            {
              track.OriginalAlbum = tbOriginalAlbum.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OriginalAlbum != tbOriginalAlbum.Text)
            {
              track.OriginalAlbum = tbOriginalAlbum.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOriginalFileName.Checked)
            {
              track.OriginalFileName = tbOriginalFileName.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OriginalFileName != tbOriginalFileName.Text)
            {
              track.OriginalFileName = tbOriginalFileName.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOriginalLyricsWriter.Checked)
            {
              track.OriginalLyricsWriter = tbOriginalLyricsWriter.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OriginalLyricsWriter != tbOriginalLyricsWriter.Text)
            {
              track.OriginalLyricsWriter = tbOriginalLyricsWriter.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOriginalOwner.Checked)
            {
              track.OriginalOwner = tbOriginalOwner.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OriginalOwner != tbOriginalOwner.Text)
            {
              track.OriginalOwner = tbOriginalOwner.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOriginalRelease.Checked)
            {
              track.OriginalRelease = tbOriginalRelease.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OriginalRelease != tbOriginalRelease.Text)
            {
              track.OriginalRelease = tbOriginalRelease.Text.Trim();
              trackChanged = true;
            }
          }

          #endregion

          #region Web Information

          if (_isMultiTagEdit)
          {
            if (ckCopyrightUrl.Checked)
            {
              track.CopyrightInformation = tbCopyrightUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.CopyrightInformation != tbCopyrightUrl.Text)
            {
              track.CopyrightInformation = tbCopyrightUrl.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOfficialAudioFileUrl.Checked)
            {
              track.OfficialAudioFileInformation = tbOfficialAudioFileUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OfficialAudioFileInformation != tbOfficialAudioFileUrl.Text)
            {
              track.OfficialAudioFileInformation = tbOfficialAudioFileUrl.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOfficialArtistUrl.Checked)
            {
              track.OfficialArtistInformation = tbOfficialArtistUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OfficialArtistInformation != tbOfficialArtistUrl.Text)
            {
              track.OfficialArtistInformation = tbOfficialArtistUrl.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOfficialAudioSourceUrl.Checked)
            {
              track.OfficialAudioSourceInformation = tbOfficialAudioSourceUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OfficialAudioSourceInformation != tbOfficialAudioSourceUrl.Text)
            {
              track.OfficialAudioSourceInformation = tbOfficialAudioSourceUrl.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOfficialInternetRadioUrl.Checked)
            {
              track.OfficialInternetRadioInformation = tbOfficialInternetRadioUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OfficialInternetRadioInformation != tbOfficialInternetRadioUrl.Text)
            {
              track.OfficialInternetRadioInformation = tbOfficialInternetRadioUrl.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOfficialPaymentUrl.Checked)
            {
              track.OfficialPaymentInformation = tbOfficialPaymentUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OfficialPaymentInformation != tbOfficialPaymentUrl.Text)
            {
              track.OfficialPaymentInformation = tbOfficialPaymentUrl.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckOfficialPublisherUrl.Checked)
            {
              track.OfficialPublisherInformation = tbOfficialPublisherUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.OfficialPublisherInformation != tbOfficialPublisherUrl.Text)
            {
              track.OfficialPublisherInformation = tbOfficialPublisherUrl.Text.Trim();
              trackChanged = true;
            }
          }

          if (_isMultiTagEdit)
          {
            if (ckCommercialInformationUrl.Checked)
            {
              track.CommercialInformation = tbCommercialInformationUrl.Text.Trim();
              trackChanged = true;
            }
          }
          else
          {
            if (track.CommercialInformation != tbCommercialInformationUrl.Text)
            {
              track.CommercialInformation = tbCommercialInformationUrl.Text.Trim();
              trackChanged = true;
            }
          }

          #endregion

          #region Involved People

          if (_involvedPeopleIsChanged)
          {
            char[] d = new char[1] { '\0' };
            string delim = new string(d);
            string tmp = "";

            foreach (DataGridViewRow pplrow in dataGridViewInvolvedPeople.Rows)
            {
              tmp += string.Format(@"{0}{1}{2}{3}", pplrow.Cells[0].Value, delim, pplrow.Cells[1].Value, delim);
            }

            if (tmp != "")
              tmp.Trim(new[] { '\0' });

            track.InvolvedPeople = tmp;
            trackChanged = true;
          }

          if (_musicianIsChanged)
          {
            char[] d = new char[1] { '\0' };
            string delim = new string(d);
            string tmp = "";

            foreach (DataGridViewRow musicianrow in dataGridViewMusician.Rows)
            {
              tmp += string.Format(@"{0}{1}{2}{3}", musicianrow.Cells[0].Value, delim, musicianrow.Cells[1].Value, delim);
            }

            if (tmp != null)
              tmp.Trim(new[] { '\0' });

            track.MusicCreditList = tmp;
            trackChanged = true;
          }

          #endregion

          #region Lyrics

          if (ckRemoveLyrics.Checked)
          {
            track.LyricsFrames.Clear();
            trackChanged = true;
          }

          if (_lyricsIsChanged)
          {
            List<Lyric> lyrics = new List<Lyric>();
            foreach (DataGridViewRow lyricsrow in dataGridViewLyrics.Rows)
            {
              Lyric lyric = new Lyric(lyricsrow.Cells[0].Value.ToString(), lyricsrow.Cells[1].Value.ToString(),
                                      lyricsrow.Cells[2].Value.ToString());
              lyrics.Add(lyric);
            }

            if (lyrics.Count > 0)
            {
              track.LyricsFrames.Clear();
              track.LyricsFrames.AddRange(lyrics);
            }
            else
            {
              track.LyricsFrames.Clear();
            }

            trackChanged = true;
          }

          #endregion

          #region Rating

          if (ckRemoveExistingRatings.Checked)
          {
            track.Ratings.Clear();
            trackChanged = true;
          }

          if (_ratingIsChanged)
          {
            List<PopmFrame> ratings = new List<PopmFrame>();
            foreach (DataGridViewRow ratingRow in dataGridViewRating.Rows)
            {
              PopmFrame rating = new PopmFrame(ratingRow.Cells[0].Value.ToString(), Convert.ToInt32(ratingRow.Cells[1].Value.ToString()),
                                         Convert.ToInt32(ratingRow.Cells[2].Value.ToString()));
              ratings.Add(rating);
            }

            if (ratings.Count > 0)
            {
              track.Ratings.Clear();
              track.Ratings.AddRange(ratings);
            }
            else
            {
              track.Ratings.Clear();
            }
            trackChanged = true;
          }

          #endregion

          #region User Defined Frames

          if (_userFrameIsChanged)
          {
            List<Frame> userFrames = new List<Frame>();
            foreach (DataGridViewRow userFrameRow in dataGridViewUserFrames.Rows)
            {
              Frame frame = new Frame(userFrameRow.Cells[0].Value.ToString(), userFrameRow.Cells[1].Value.ToString(),
                                      userFrameRow.Cells[2].Value.ToString());
              userFrames.Add(frame);
            }

            // Let's save the Current User frames first, so that on Save we are able to delete them to avoid duplicates
            track.SavedUserFrames = new List<Frame>(track.UserFrames);

            if (userFrames.Count > 0)
            {
              track.UserFrames.Clear();
              track.UserFrames.AddRange(userFrames);
            }
            else
            {
              track.UserFrames.Clear();
            }
            trackChanged = true;
          }

          #endregion

          if (trackChanged)
          {
            main.TracksGridView.Changed = true;
            main.TracksGridView.SetBackgroundColorChanged(row.Index);
            track.Changed = true;
            Options.Songlist[row.Index] = track;
          }
        }
        catch (Exception ex)
        {
          log.Error("Error applying changes from Tagedit: {0} stack: {1}", ex.Message, ex.StackTrace);
          Options.Songlist[row.Index].Status = 2;
          main.TracksGridView.AddErrorMessage(row, ex.Message);
          bErrors = true;
        }
      }


      main.TracksGridView.Changed = bErrors;
      // check, if we still have changed items in the list
      foreach (TrackData track in Options.Songlist)
      {
        if (track.Changed)
          main.TracksGridView.Changed = true;
      }

      _pictureIsChanged = false;
      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      log.Trace("<<<");
    }

    /// <summary>
    ///   When the Textbox has been edited, the Check Box should be selected automatically
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void OnTextChanged(object sender, EventArgs e)
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

        case "tbAlbumArtistSort":
          ckAlbumArtistSort.Checked = true;
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
    ///   When the ComboBox has been edited, the Check Box should be selected automatically
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void OnComboChanged(object sender, EventArgs e)
    {
      MPTComboBox cb = sender as MPTComboBox;
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

        case "cbGenre":
          ckGenre.Checked = true;
          break;
      }
    }

    /// <summary>
    /// Copy the content of the Artist field to Album Artist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btCopyArtistToAlbumArtist_Click(object sender, EventArgs e)
    {
      if (_isMultiTagEdit)
      {
        cbAlbumArtist.Text = cbArtist.Text;
      }
      else
      {
        tbAlbumArtist.Text = tbArtist.Text;
      }
    }

    #region Genre

    /// <summary>
    ///   Add the selected Genre to the List of Genres
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btAddGenre_Click(object sender, EventArgs e)
    {
      if (cbGenre.Text != "")
      {
        // Only insert the item, if it isn't already in the listbox
        if (listBoxGenre.Items.IndexOf(cbGenre.Text) < 0)
        {
          listBoxGenre.Items.Add(cbGenre.Text);
          cbGenre.Text = "";
          cbGenre.SelectedIndex = -1;
          ckGenre.Checked = true;
        }
      }
    }

    /// <summary>
    ///   Remove the selected Genre from the List
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btRemoveGenre_Click(object sender, EventArgs e)
    {
      if (listBoxGenre.SelectedIndex > -1)
      {
        ckGenre.Checked = true;
        listBoxGenre.Items.RemoveAt(listBoxGenre.SelectedIndex);
      }
    }

    /// <summary>
    ///   Move Selected Genre to Top
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void btGenreToTop_Click(object sender, EventArgs e)
    {
      if (listBoxGenre.SelectedIndex > -1)
      {
        ckGenre.Checked = true;
        string item = listBoxGenre.SelectedItem.ToString();
        listBoxGenre.Items.RemoveAt(listBoxGenre.SelectedIndex);
        listBoxGenre.Items.Insert(0, item);
      }
    }

    /// <summary>
    ///   Double Click on Genre. 
    ///   Remove the selected Genre
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void listBoxGenre_DoubleClick(object sender, EventArgs e)
    {
      ckGenre.Checked = true;
      listBoxGenre.Items.Remove(listBoxGenre.SelectedItem);
    }

    #endregion

    #region Comments

    private void buttonAddComment_Click(object sender, EventArgs e)
    {
      if (textBoxComment.Text.Trim().Length > 0)
        AddComment(cbCommentDescriptor.Text, cbCommentLanguage.Text, textBoxComment.Text);

      cbCommentDescriptor.Text = "";
      textBoxComment.Text = "";
      _commentIsChanged = true;
    }

    private void buttonRemoveComment_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewComment.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewComment.Rows[i].Selected)
        {
          dataGridViewComment.Rows.RemoveAt(i);
          // Now row - 1 is selected unselect it
          if (i > 0)
            dataGridViewComment.Rows[i - 1].Selected = false;

          _commentIsChanged = true;
        }
      }
    }

    private void buttonMoveTop_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewComment.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewComment.Rows[i].Selected)
        {
          DataGridViewRow row = dataGridViewComment.Rows[i];
          dataGridViewComment.Rows.RemoveAt(i);
          dataGridViewComment.Rows.Insert(0, row);
          _commentIsChanged = true;
        }
      }
    }


    private void dataGridViewComment_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      cbCommentDescriptor.Text = dataGridViewComment.Rows[e.RowIndex].Cells[0].Value.ToString();
      cbCommentLanguage.Text = dataGridViewComment.Rows[e.RowIndex].Cells[1].Value.ToString();
      textBoxComment.Text = dataGridViewComment.Rows[e.RowIndex].Cells[2].Value.ToString();
    }

    #endregion

    #region Picture

    /// <summary>
    ///   Load a new picture from a file
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonGetPicture_Click(object sender, EventArgs e)
    {
      var oFD = new ShellOpenFileDialog();
      oFD.Filter = "Pictures (Bmp, Jpg, Gif, Png)\0*.jpg;*.jpeg;*.bmp;*.Gif;*.png\0";
      oFD.InitialDirectory = main.CurrentDirectory;
      if (oFD.ShowDialog())
      {
        if (checkBoxRemoveExistingPictures.Checked)
        {
          dataGridViewPicture.Rows.Clear();
          _pictures.Clear();
        }

        try
        {
          _pic = new Picture(oFD.FileName);
          AddPictureToList();
          AddPictureToPictureBox();
          _pictureIsChanged = true;
        }
        catch (Exception ex)
        {
          log.Error("Exception Loading picture: {0} {1}", oFD.FileName, ex.Message);
        }
      }
    }

    /// <summary>
    ///   Remove the selected picture
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonRemovePicture_Click(object sender, EventArgs e)
    {
      if (_selectedPictureGridRow > -1)
      {
        dataGridViewPicture.Rows.RemoveAt(_selectedPictureGridRow);
        _pictures.RemoveAt(_selectedPictureGridRow);
        pictureBoxCover.Image = null;
        buttonRemovePicture.Enabled = false;
        buttonExportPicture.Enabled = false;
        _pictureIsChanged = true;
      }
    }

    /// <summary>
    ///   Export the selected picture
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonExportPicture_Click(object sender, EventArgs e)
    {
      SaveFileDialog sFD = new SaveFileDialog();
      sFD.Filter = "Extension is retrieved from Picture|*.*";
      sFD.InitialDirectory = main.CurrentDirectory;
      DialogResult result = sFD.ShowDialog();
      if (result == DialogResult.OK)
      {
        if (sFD.FileName != "")
        {
          try
          {
            string extension =
              _pictures[_selectedPictureGridRow].MimeType.Substring(
                _pictures[_selectedPictureGridRow].MimeType.IndexOf("/") + 1);
            if (extension == "jpeg")
              extension = "jpg";

            string fileName = String.Format("{0}.{1}", sFD.FileName, extension);
            Image img = Picture.ImageFromData(_pictures[_selectedPictureGridRow].Data);
            if (img != null)
            {
              img.Save(fileName);
            }
          }
          catch (Exception ex)
          {
            log.Error("Exception Saving picture: {0} {1}", sFD.FileName, ex.Message);
          }
        }
      }
    }

    /// <summary>
    ///   A picture has been selected
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    protected void dataGridViewPicture_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex > -1)
      {
        try
        {
          Image img = Picture.ImageFromData(_pictures[e.RowIndex].Data);
          if (img != null)
          {
            pictureBoxCover.Image = img;
            tbPicDesc.Text = _pictures[e.RowIndex].Description;
            cbPicType.Text = Enum.GetName(typeof (PictureType),_pictures[e.RowIndex].Type);
          }
        }
        catch (Exception ex)
        {
          log.Error("TagEdit: Error creating Picture: {0}.", ex.Message);
        }
        _selectedPictureGridRow = e.RowIndex;
        buttonExportPicture.Enabled = true;
        buttonRemovePicture.Enabled = true;
      }
    }

    /// <summary>
    /// A Picture Type has been selected. Update the Item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbPicType_SelectedIndexChanged(object sender, EventArgs e)
    {
      TagLib.PictureType picType = (PictureType)Enum.Parse(typeof (PictureType), cbPicType.Text);
      foreach (DataGridViewRow row in dataGridViewPicture.SelectedRows)
      {
        if (_pictures[row.Index].Type != picType)
        {
          row.Cells[1].Value = cbPicType.Text;
          _pictures[row.Index].Type = picType;
          _pictureIsChanged = true;
          
        }
      }
    }

    /// <summary>
    /// The Description for the Picture has been changed. Update the Item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tbPicDesc_Validated(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in dataGridViewPicture.SelectedRows)
      {
        if (_pictures[row.Index].Description != tbPicDesc.Text)
        {
          row.Cells[0].Value = tbPicDesc.Text;
          _pictures[row.Index].Description = tbPicDesc.Text;
          _pictureIsChanged = true;

        }
      }
    }

    /// <summary>
    ///   Get the Cover Image from Amazon
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    private void buttonGetPictureInternet_Click(object sender, EventArgs e)
    {
      string searchArtist = "";
      string searchAlbum = "";
      if (_isMultiTagEdit)
      {
        searchArtist = cbArtist.Text.Trim();
        searchAlbum = cbAlbum.Text.Trim();
      }
      else
      {
        searchArtist = tbArtist.Text.Trim();
        searchAlbum = tbAlbum.Text.Trim();
      }

      if (searchAlbum.Length == 0)
      {
        return;
      }

      Cursor = Cursors.WaitCursor;
      CoverSearch dlgAlbumResults = new CoverSearch();
      dlgAlbumResults.Artist = searchArtist;
      dlgAlbumResults.Album = searchAlbum;
      dlgAlbumResults.Owner = main;

      AmazonAlbum amazonAlbum = null;
      if (main.ShowModalDialog(dlgAlbumResults) == DialogResult.OK)
      {
        if (dlgAlbumResults.SelectedAlbum != null)
        {
          amazonAlbum = dlgAlbumResults.SelectedAlbum;
        }
      }
      else
      {
        log.Debug("CoverArt: Album Selection cancelled");
      }
      dlgAlbumResults.Dispose();

      if (amazonAlbum == null)
      {
        return;
      }

      if (checkBoxRemoveExistingPictures.Checked)
      {
        dataGridViewPicture.Rows.Clear();
        _pictures.Clear();
      }

      ByteVector vector = amazonAlbum.AlbumImage;
      if (vector != null)
      {
        _pic = new Picture();
        _pic.MimeType = "image/jpg";
        _pic.Description = "";
        _pic.Type = TagLib.PictureType.FrontCover;
        _pic.Data = vector.Data;
        AddPictureToList();
        AddPictureToPictureBox();
        _pictureIsChanged = true;
      }
      Cursor = Cursors.Default;
    }

    #endregion

    #region Detailed Information

    /// <summary>
    /// Set the TRack Length Tag with the duration from the file
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btGetTrackLength_Click(object sender, EventArgs e)
    {
      if (_selectedRowIndex == -1)
      {
        return;
      }

      TrackData track = Options.Songlist[_selectedRowIndex];
      tbTrackLength.Text = track.DurationTimespan.TotalMilliseconds.ToString();
    }

    #endregion

    #region Involved People

    private void buttonAddInvolvedPerson_Click(object sender, EventArgs e)
    {
      if (tbInvolvedPersonFunction.Text.Trim().Length > 0 || tbInvolvedPersonName.Text.Trim().Length > 0)
      {
        dataGridViewInvolvedPeople.Rows.Add(new object[] { tbInvolvedPersonFunction.Text, tbInvolvedPersonName.Text });
        tbInvolvedPersonFunction.Text = "";
        tbInvolvedPersonName.Text = "";
        ckInvolvedPerson.Checked = true;
        _involvedPeopleIsChanged = true;
      }
    }

    private void buttonRemovePerson_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewInvolvedPeople.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewInvolvedPeople.Rows[i].Selected)
        {
          dataGridViewInvolvedPeople.Rows.RemoveAt(i);
          // Now row - 1 is selected unselect it
          if (i > 0)
            dataGridViewInvolvedPeople.Rows[i - 1].Selected = false;

          _involvedPeopleIsChanged = true;
        }
      }
    }

    private void buttonAddMusician_Click(object sender, EventArgs e)
    {
      if (tbMusicianInstrument.Text.Trim().Length > 0 || tbMusicianName.Text.Trim().Length > 0)
      {
        dataGridViewMusician.Rows.Add(new object[] { tbMusicianInstrument.Text, tbMusicianName.Text });
        tbMusicianInstrument.Text = "";
        tbMusicianName.Text = "";
        ckInvolvedMusician.Checked = true;
        _musicianIsChanged = true;
      }
    }

    private void buttonRemoveMusician_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewMusician.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewMusician.Rows[i].Selected)
        {
          dataGridViewMusician.Rows.RemoveAt(i);
          // Now row - 1 is selected unselect it
          if (i > 0)
            dataGridViewMusician.Rows[i - 1].Selected = false;

          _musicianIsChanged = true;
        }
      }
    }

    #endregion

    #region Lyrics

    private void btAddLyrics_Click(object sender, EventArgs e)
    {
      if (tbLyrics.Text.Trim().Length > 0)
        AddLyrics(cbLyricsDescriptor.Text, cbLyricsLanguage.Text, tbLyrics.Text);

      cbLyricsDescriptor.Text = "";
      tbLyrics.Text = "";
      _lyricsIsChanged = true;
    }

    private void btRemoveLyrics_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewLyrics.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewLyrics.Rows[i].Selected)
        {
          dataGridViewLyrics.Rows.RemoveAt(i);
          // Now row - 1 is selected unselect it
          if (i > 0)
            dataGridViewLyrics.Rows[i - 1].Selected = false;

          _lyricsIsChanged = true;
        }
      }
    }


    private void btLyricsMoveTop_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewLyrics.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewLyrics.Rows[i].Selected)
        {
          DataGridViewRow row = dataGridViewLyrics.Rows[i];
          dataGridViewLyrics.Rows.RemoveAt(i);
          dataGridViewLyrics.Rows.Insert(0, row);
          _lyricsIsChanged = true;
        }
      }
    }

    private void dataGridViewLyrics_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      cbLyricsDescriptor.Text = dataGridViewLyrics.Rows[e.RowIndex].Cells[0].Value.ToString();
      cbLyricsLanguage.Text = dataGridViewLyrics.Rows[e.RowIndex].Cells[1].Value.ToString();
      tbLyrics.Text = dataGridViewLyrics.Rows[e.RowIndex].Cells[2].Value.ToString();
    }


    private void btGetLyricsFromText_Click(object sender, EventArgs e)
    {
      OpenFileDialog oFD = new OpenFileDialog();
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        StreamReader stream = System.IO.File.OpenText(oFD.FileName);
        string input = null;
        while ((input = stream.ReadLine()) != null)
        {
          input += "\r\n";
          tbLyrics.AppendText(input);
        }
        stream.Close();
      }
    }

    private void btGetLyricsFromInternet_Click(object sender, EventArgs e)
    {
      if (_isMultiTagEdit)
      {
        main.TracksGridView.ExecuteCommand("GetLyrics");
      }
      else
      {
        LyricsSearch lyricsSearch = new LyricsSearch(this, tbArtist.Text, tbTitle.Text, false);  
      }
    }

    #endregion

    #region Rating

    private void btAddRating_Click(object sender, EventArgs e)
    {
      if (tbRatingUser.Text.Trim().Length > 0)
      {
        AddRating(tbRatingUser.Text, numericUpDownRating.Value.ToString(), numericUpDownPlayCounter.Value.ToString());

        tbRatingUser.Text = "";
        numericUpDownRating.Value = 0;
        numericUpDownPlayCounter.Value = 0;
        _ratingIsChanged = true;
      }
    }

    private void btRemoveRating_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewRating.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewRating.Rows[i].Selected)
        {
          dataGridViewRating.Rows.RemoveAt(i);
          // Now row - 1 is selected unselect it
          if (i > 0)
            dataGridViewRating.Rows[i - 1].Selected = false;

          _ratingIsChanged = true;
        }
      }
    }

    private void btRatingMoveTop_Click(object sender, EventArgs e)
    {
      for (int i = dataGridViewRating.Rows.Count - 1; i > -1; i--)
      {
        if (dataGridViewRating.Rows[i].Selected)
        {
          DataGridViewRow row = dataGridViewRating.Rows[i];
          dataGridViewRating.Rows.RemoveAt(i);
          dataGridViewRating.Rows.Insert(0, row);
          _ratingIsChanged = true;
        }
      }
    }


    private void dataGridViewRating_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      tbRatingUser.Text = dataGridViewRating.Rows[e.RowIndex].Cells[0].Value.ToString();
      numericUpDownRating.Value = Convert.ToDecimal(dataGridViewRating.Rows[e.RowIndex].Cells[1].Value);
      numericUpDownPlayCounter.Value = Convert.ToDecimal(dataGridViewRating.Rows[e.RowIndex].Cells[2].Value);
    }

    #endregion

    #region User Defined Frames

    /// <summary>
    /// Add a User Frame 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btAddUserFrame_Click(object sender, EventArgs e)
    {
      dataGridViewUserFrames.Rows.Add(new object[] {"TXXX", "", ""});
      _userFrameIsChanged = true;
    }

    /// <summary>
    /// Delete Selected Userdefined Frame
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btDeleteFrame_Click(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in dataGridViewUserFrames.SelectedRows)
      {
        dataGridViewUserFrames.Rows.Remove(row);
        _userFrameIsChanged = true;
      }
    }

    /// <summary>
    /// Delete All User defined Frames
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btDeleteAllFrames_Click(object sender, EventArgs e)
    {
      dataGridViewUserFrames.Rows.Clear();
      _userFrameIsChanged = true;
    }

    /// <summary>
    /// Something was edited in the Grid. Indicate that the cell has been changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void dataGridViewUserFrames_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      _userFrameIsChanged = true;
    }

    #endregion

    #region Key Events

    private void btPrevious_Click(object sender, EventArgs e)
    {
      Action action = new Action();
      action.ID = Action.ActionType.ACTION_PREVFILE;
      OnAction(action);
    }

    private void btNext_Click(object sender, EventArgs e)
    {
      Action action = new Action();
      action.ID = Action.ActionType.ACTION_NEXTFILE;
      OnAction(action);
    }


    /// <summary>
    ///   A Key has been pressed
    /// </summary>
    /// <param name = "e"></param>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == Keys.Enter)   // Handle Enter key as default Apply Button
      {
        btApply_Click(null, new EventArgs());
        return true;
      }
      
      if (keyData == Keys.Escape)  // Handle Escape to clear the form
      {
        ClearForm();
        return true;
      }
      
      if (keyData == (Keys.Control | Keys.C))  // Handle Copy. This would else be consumed by the grid
      {
        var ctrl = this.ActiveControl as TextBoxBase;
        if (ctrl != null)
        {
          ctrl.Copy();
          return true;
        }
      }

      Action newaction = new Action();
      if (ServiceScope.Get<IActionHandler>().GetAction(1, keyData, ref newaction))
      {
        if (OnAction(newaction))
        {
          return true;
        }
      }

      return base.ProcessCmdKey(ref msg, keyData);
    }

    private bool OnAction(Action action)
    {
      if (action == null)
        return false;

      bool handled = true;
      int curRow = -1;
      switch (action.ID)
      {
        case Action.ActionType.ACTION_PAGEDOWN:
          // Scrolls the Tab Pages forward
          if (tabControlTagEdit.SelectedTabPage == tabControlTagEdit.TabPages[tabControlTagEdit.TabPages.Count - 1])
          {
            tabControlTagEdit.SelectFirstTab();
          }
          else
          {
            tabControlTagEdit.SelectNextTab();
          }
          break;

        case Action.ActionType.ACTION_PAGEUP:
          // Scrolls the Tab Pages backwards
          if (tabControlTagEdit.SelectedTabPage == tabControlTagEdit.TabPages[0])
          {
            tabControlTagEdit.SelectLastTab();
          }
          else
          {
            tabControlTagEdit.SelectPreviousTab();
          }
          break;

        case Action.ActionType.ACTION_NEXTFILE:
          if (main.TracksGridView.View.SelectedRows.Count == 1)
          {
            curRow = main.TracksGridView.View.SelectedRows[0].Index;
            _selectedRowIndex = main.TracksGridView.View.SelectedRows[0].Index;
            if ((curRow + 1) < main.TracksGridView.View.RowCount)
            {
              btApply_Click(null, new EventArgs());
              main.TracksGridView.View.Rows[curRow].Selected = false;
              main.TracksGridView.View.Rows[curRow + 1].Selected = true;
            }
          }
          break;

        case Action.ActionType.ACTION_PREVFILE:
          if (main.TracksGridView.View.SelectedRows.Count == 1)
          {
            curRow = main.TracksGridView.View.SelectedRows[0].Index;
            _selectedRowIndex = main.TracksGridView.View.SelectedRows[0].Index;
            if ((curRow - 1) >= 0)
            {
              btApply_Click(null, new EventArgs());
              main.TracksGridView.View.Rows[curRow].Selected = false;
              main.TracksGridView.View.Rows[curRow - 1].Selected = true;
            }
          }
          break;
      }
      return handled;
    }

    #endregion

    #region Global Events
    /// <summary>
    ///   Handle Messages
    /// </summary>
    /// <param name = "message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "customgenresrefreshed":
          cbGenre.Items.Clear();
          cbGenre.Items.AddRange(Genres.Audio);
          foreach (string customGenre in Options.MainSettings.CustomGenres)
          {
            cbGenre.Items.Add(customGenre);
          }
          break;
      }
    }
    #endregion

    #endregion
  }
}
