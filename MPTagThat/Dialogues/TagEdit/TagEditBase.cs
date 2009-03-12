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
using MPTagThat.Core.ShellLib;
using MPTagThat.Dialogues;

namespace MPTagThat.TagEdit
{
  public partial class TagEditBase : Telerik.WinControls.UI.ShapedForm
  {
    #region Variables
    protected Main main;
    protected ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    protected ILogger log = ServiceScope.Get<ILogger>();
    protected List<Picture> _pictures = new List<Picture>();
    protected Picture _pic = null;
    protected int _selectedPictureGridRow = -1;
    protected bool _pictureIsChanged = false;
    protected bool _commentIsChanged = false;
    protected bool _involvedPeopleIsChanged = false;
    protected bool _musicianIsChanged = false;
    protected bool _lyricsIsChanged = false;
    protected bool _ratingIsChanged = false;


    protected string[] headerText = { "Main Tags",  "Pictures", "Detailed Information", "Original Information", "Involved People",
                                    "Web Information", "Lyrics", "Rating", "User Defined"};
    #endregion

    #region Properties
    public string Header
    {
      set { this.labelHeader.Text = value; }
    }
    #endregion

    #region ctor
    public TagEditBase()
    {
      InitializeComponent();
    }
    #endregion

    #region Form Opening
    /// <summary>
    /// Called when loading the Dialoue
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnLoad(object sender, EventArgs e)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
      this.labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

      // Set the region for the Tabcontrol to hide the tabs
      this.tabControlTagEdit.Region = new Region(new RectangleF(this.tabPageMain.Left,
                                                                     this.tabPageMain.Top,
                                                                     this.tabPageMain.Width,
                                                                     this.tabPageMain.Height));

      // Fill the Genre Combo Box
      cbGenre.Items.AddRange(TagLib.Genres.Audio);

      // Fill the Picture Type Box
      Type picTypes = typeof(TagLib.PictureType);
      foreach (string type in Enum.GetNames(picTypes))
        cbPicType.Items.Add(type);

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

      this.tabControlTagEdit.SelectedIndex = 0;

      if (Options.MainSettings.UseMediaPortalDatabase && Options.MediaPortalArtists != null)
      {
        // Add Auto Complete Option for Artist
        ShellAutoComplete acArtist = new ShellAutoComplete();

        acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoSuggest;
        acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoAppend;
        acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UpDownKeyDropsList;
        acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.FilterPreFixes;
        acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UseTab;

        SourceCustomList custom = new SourceCustomList();
        custom.StringList = Options.MediaPortalArtists;
        acArtist.ListSource = custom;

        acArtist.EditHandle = tbArtist.Handle;
        acArtist.SetAutoComplete(true);

        // Add Auto Complete Option for AlbumArtist
        ShellAutoComplete acAlbumArtist = new ShellAutoComplete();

        acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoSuggest;
        acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoAppend;
        acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UpDownKeyDropsList;
        acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.FilterPreFixes;
        acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UseTab;

        acAlbumArtist.ListSource = custom;

        acAlbumArtist.EditHandle = tbAlbumArtist.Handle;
        acAlbumArtist.SetAutoComplete(true);
      }
    }

    private void Localisation()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Descriptor.HeaderText = localisation.ToString("TagEdit", "CommentHeaderDescriptor");
      this.Language.HeaderText = localisation.ToString("TagEdit", "CommentHeaderLanguage");
      this.Comment.HeaderText = localisation.ToString("TagEdit", "CommentHeaderComment");
      this.PictureType.HeaderText = localisation.ToString("TagEdit", "PictureHeaderType");
      this.dataGridViewTextBoxColumn4.HeaderText = localisation.ToString("TagEdit", "MusicianHeaderInstrument");
      this.dataGridViewTextBoxColumn6.HeaderText = localisation.ToString("TagEdit", "MusicianHeaderName");
      this.dataGridViewTextBoxColumn3.HeaderText = localisation.ToString("TagEdit", "PersonHeaderName");
      this.dataGridViewTextBoxColumn5.HeaderText = localisation.ToString("TagEdit", "PersonHeaderFunction");
      this.dataGridViewTextBoxColumn7.HeaderText = localisation.ToString("TagEdit", "LyricsHeaderDescriptor");
      this.dataGridViewTextBoxColumn8.HeaderText = localisation.ToString("TagEdit", "LyricsHeaderLanguage");
      this.dataGridViewTextBoxColumn9.HeaderText = localisation.ToString("TagEdit", "LyricsHeaderLyrics");

      headerText[0] = localisation.ToString("TagEdit", "HeaderMainTags");
      headerText[1] = localisation.ToString("TagEdit", "HeaderPictures");
      headerText[2] = localisation.ToString("TagEdit", "HeaderDetail");
      headerText[3] = localisation.ToString("TagEdit", "HeaderOriginal");
      headerText[4] = localisation.ToString("TagEdit", "HeaderInvolved");
      headerText[5] = localisation.ToString("TagEdit", "HeaderWeb");
      headerText[6] = localisation.ToString("TagEdit", "HeaderLyrics");
      headerText[7] = localisation.ToString("TagEdit", "HeaderRating");
      headerText[8] = localisation.ToString("TagEdit", "HeaderUser");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Methods
    protected void AddComment(string desc, string lang, string text)
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

    /// <summary>
    /// Add a picture to the Picture box
    /// </summary>
    private void AddPictureToPictureBox()
    {
      try
      {
        if (_pic != null)
        {
          using (System.IO.MemoryStream ms = new System.IO.MemoryStream(_pic.Data.Data))
          {
            Image img = Image.FromStream(ms);
            if (img != null)
            {
              pictureBoxCover.Image = img;
            }
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("Exception adding picture: {0}", ex.Message);
      }
    }

    /// <summary>
    /// Add the currently selected Picture to the collection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddPictureToList()
    {
      if (_pic != null)
      {
        _pic.Description = tbPicDesc.Text;
        _pic.Type = (PictureType)Enum.Parse(typeof(TagLib.PictureType), cbPicType.SelectedItem != null ? cbPicType.SelectedItem.ToString() : "FrontCover");

        dataGridViewPicture.Rows.Add(new object[] { _pic.Description, Enum.Format(typeof(TagLib.PictureType), _pic.Type, "G") });

        _pictures.Add(_pic);
        pictureBoxCover.Image = null;
        _pictureIsChanged = true;
      }
    }

    protected void AddRating(string user, string rating, string playcount)
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

    protected void AddLyrics(string desc, string lang, string text)
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

    #region Event Handler
    /// <summary>
    /// Hide the Tabcontrol Tabs, as we navigate via Nav Bar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void panelTabPage_Paint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      Rectangle rect = new Rectangle(tabControlTagEdit.Location.X + 4, tabControlTagEdit.Location.Y + 3, tabPageMain.Width - 1, 22);
      g.FillRectangle(new SolidBrush(ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor), rect);
      g.DrawRectangle(new Pen(Color.LightSteelBlue), rect);
      g.DrawString(headerText[tabControlTagEdit.SelectedIndex], new Font(new FontFamily("Arial"), 12f, FontStyle.Bold), new SolidBrush(ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor), new PointF(tabControlTagEdit.Location.X + 8, tabControlTagEdit.Location.Y + 6));
    }

    /// <summary>
    /// Apply the Changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void btApply_Click(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Close the form discarding the changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    /// <summary>
    /// When the Textbox has been edited, the Check Box should be selected automatically
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnTextChanged(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// A text in the Combo has been selected. Mark the Check Box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnComboChanged(object sender, EventArgs e)
    {
    }

    #region Navigation Page
    /// <summary>
    /// User selected a link in the navbar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnNavpageLink_Clicked(object sender, MouseEventArgs e)
    {
      tabPageMain.Hide();
      tabPagePictures.Hide();

      Label label = sender as Label;
      switch (label.Name)
      {
        case "lbLinkMainTags":
          tabControlTagEdit.SelectedIndex = 0;
          break;

        case "lbLinkPictures":
          tabControlTagEdit.SelectedIndex = 1;
          break;

        case "lbLinkDetails":
          tabControlTagEdit.SelectedIndex = 2;
          break;

        case "lbLinkOriginal":
          tabControlTagEdit.SelectedIndex = 3;
          break;

        case "lbLinkInvolvedPeople":
          tabControlTagEdit.SelectedIndex = 4;
          break;

        case "lbLinkWebInformation":
          tabControlTagEdit.SelectedIndex = 5;
          break;

        case "lbLinkLyrics":
          tabControlTagEdit.SelectedIndex = 6;
          break;

        case "lbLinkRating":
          tabControlTagEdit.SelectedIndex = 7;
          break;

        case "lbLinkUserDefined":
          tabControlTagEdit.SelectedIndex = 8;
          break;

      }
    }

    /// <summary>
    /// A new Tab has been selected. Either by clicking the link or PageUp / Down
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tabControlTagEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.panelTabPage_Paint(panelTabPage, new PaintEventArgs(panelTabPage.CreateGraphics(), panelTabPage.ClientRectangle));
    }
    #endregion

    #region Genre
    /// <summary>
    /// Add the selected Genre to the List of Genres
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Remove the selected Genre from the List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btRemoveGenre_Click(object sender, EventArgs e)
    {
      if (listBoxGenre.SelectedIndex > -1)
      {
        ckGenre.Checked = true;
        listBoxGenre.Items.RemoveAt(listBoxGenre.SelectedIndex);
      }
    }

    /// <summary>
    /// Move Selected Genre to Top
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Double Click on Genre. 
    /// Remove the selected Genre
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listBoxGenre_DoubleClick(object sender, EventArgs e)
    {
      ckGenre.Checked = true;
      listBoxGenre.Items.Remove(listBoxGenre.SelectedItem);
    }
    #endregion

    #region Picture
    /// <summary>
    /// Load a new picture from a file
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonGetPicture_Click(object sender, EventArgs e)
    {
      OpenFileDialog oFD = new OpenFileDialog();
      oFD.Filter = "Pictures (Bmp, Jpg, Gif, Png)|*.jpg;*.jpeg;*.bmp;*.Gif;*.png";
      oFD.Multiselect = false;
      oFD.InitialDirectory = main.CurrentDirectory;
      DialogResult result = oFD.ShowDialog();
      if (result == DialogResult.OK)
      {
        try
        {
          _pic = new Picture(oFD.FileName);
          AddPictureToList();
          AddPictureToPictureBox();
        }
        catch (Exception ex)
        {
          log.Error("Exception Loading picture: {0} {1}", oFD.FileName, ex.Message);
        }
      }
    }

    /// <summary>
    /// Remove the selected picture
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Export the selected picture
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            string extension = _pictures[_selectedPictureGridRow].MimeType.Substring(_pictures[_selectedPictureGridRow].MimeType.IndexOf("/") + 1);
            if (extension == "jpeg")
              extension = "jpg";

            string fileName = String.Format("{0}.{1}", sFD.FileName, extension);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(_pictures[_selectedPictureGridRow].Data.Data))
            {
              Image img = Image.FromStream(ms);
              if (img != null)
              {
                img.Save(fileName);
              }
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
    /// A picture has been selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dataGridViewPicture_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex > -1)
      {
        try
        {
          using (System.IO.MemoryStream ms = new System.IO.MemoryStream(_pictures[e.RowIndex].Data.Data))
          {
            Image img = Image.FromStream(ms);
            if (img != null)
            {
              pictureBoxCover.Image = img;
            }
          }
        }
        catch(Exception ex)
        {
          log.Error("TagEdit: Error creating Picture: {0}.", ex.Message);
        }
        _selectedPictureGridRow = e.RowIndex;
        buttonExportPicture.Enabled = true;
        buttonRemovePicture.Enabled = true;
      }
    }

    /// <summary>
    /// Get the Cover Image from Amazon
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonGetPictureInternet_Click(object sender, EventArgs e)
    {
      if (tbAlbum.Text.Length == 0)
        return;

      this.Cursor = Cursors.WaitCursor;
      List<AmazonAlbum> albums = new List<AmazonAlbum>();
      using (AmazonAlbumInfo amazonInfo = new AmazonAlbumInfo())
      {
        albums = amazonInfo.AmazonAlbumSearch(tbArtist.Text, tbAlbum.Text);
      }

      if (albums.Count > 0)
      {
        AmazonAlbum amazonalbum = null;
        if (albums.Count == 1)
        {
          amazonalbum = albums[0];
        }
        else
        {
          AmazonAlbumSearchResults dlgAlbumResults = new AmazonAlbumSearchResults(albums);
          
          this.Cursor = Cursors.Default;
          if (main.ShowForm(dlgAlbumResults) == DialogResult.OK)
          {
            if (dlgAlbumResults.SelectedListItem > -1)
              amazonalbum = albums[dlgAlbumResults.SelectedListItem];
            else
              amazonalbum = albums[0];
          }
          dlgAlbumResults.Dispose();
        }

        if (amazonalbum == null)
          return;

        ByteVector vector = amazonalbum.AlbumImage;
        if (vector != null)
        {
          _pic = new Picture(vector);
          _pic.MimeType = "image/jpg";
          _pic.Description = "";
          _pic.Type = TagLib.PictureType.FrontCover;
          AddPictureToList();
          AddPictureToPictureBox();
          _pictureIsChanged = true;
        }
      }
      this.Cursor = Cursors.Default;
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


    private void lblLyricsMoveTop_Click(object sender, EventArgs e)
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
        System.IO.StreamReader stream = System.IO.File.OpenText(oFD.FileName);
        string input = null;
        while ((input = stream.ReadLine()) != null)
        {
          input += "\r\n";
          tbLyrics.AppendText(input);
        }
        stream.Close();
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
    #endregion

    #region Key Events
    /// <summary>
    /// A Key has been pressed
    /// </summary>
    /// <param name="e"></param>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      Action newaction = new Action();
      if (ServiceScope.Get<IActionHandler>().GetAction(1, keyData, ref newaction))
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

      }
      return handled;
    }
    #endregion
  }
}
