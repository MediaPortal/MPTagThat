namespace MPTagThat.Player
{
  partial class PlayerControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
      this.panelMiddle = new MPTagThat.Core.WinControls.TTPanel();
      this.lbAlbumText = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbArtistText = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbTitleText = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.pictureBoxSpectrum = new System.Windows.Forms.PictureBox();
      this.pictureBoxTime = new System.Windows.Forms.PictureBox();
      this.panelRight = new MPTagThat.Core.WinControls.TTPanel();
      this.btnPlayList = new MPTagThat.Core.WinControls.MPTButton();
      this.panelLeft = new MPTagThat.Core.WinControls.TTPanel();
      this.pictureBoxPlayPause = new System.Windows.Forms.PictureBox();
      this.buttonPrev = new System.Windows.Forms.Button();
      this.buttonNext = new System.Windows.Forms.Button();
      this.panelTop = new MPTagThat.Core.WinControls.TTPanel();
      this.playBackSlider = new MPTagThat.Core.ColorSlider();
      this.panelBottom = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddle.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).BeginInit();
      this.panelRight.SuspendLayout();
      this.panelLeft.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayPause)).BeginInit();
      this.panelTop.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelMiddle
      // 
      this.panelMiddle.AutoSize = true;
      this.panelMiddle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panelMiddle.Controls.Add(this.lbAlbumText);
      this.panelMiddle.Controls.Add(this.lbArtistText);
      this.panelMiddle.Controls.Add(this.lbTitleText);
      this.panelMiddle.Controls.Add(this.lbAlbum);
      this.panelMiddle.Controls.Add(this.lbArtist);
      this.panelMiddle.Controls.Add(this.lbTitle);
      this.panelMiddle.Controls.Add(this.pictureBoxSpectrum);
      this.panelMiddle.Controls.Add(this.pictureBoxTime);
      this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddle.Location = new System.Drawing.Point(170, 0);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(477, 60);
      this.panelMiddle.TabIndex = 11;
      // 
      // lbAlbumText
      // 
      this.lbAlbumText.AutoSize = true;
      this.lbAlbumText.Localisation = "mptLabel1";
      this.lbAlbumText.LocalisationContext = "panelMiddle";
      this.lbAlbumText.Location = new System.Drawing.Point(186, 42);
      this.lbAlbumText.Name = "lbAlbumText";
      this.lbAlbumText.Size = new System.Drawing.Size(36, 13);
      this.lbAlbumText.TabIndex = 14;
      this.lbAlbumText.Text = "Album";
      // 
      // lbArtistText
      // 
      this.lbArtistText.AutoSize = true;
      this.lbArtistText.Localisation = "mptLabel1";
      this.lbArtistText.LocalisationContext = "panelMiddle";
      this.lbArtistText.Location = new System.Drawing.Point(186, 24);
      this.lbArtistText.Name = "lbArtistText";
      this.lbArtistText.Size = new System.Drawing.Size(30, 13);
      this.lbArtistText.TabIndex = 13;
      this.lbArtistText.Text = "Artist";
      // 
      // lbTitleText
      // 
      this.lbTitleText.AutoSize = true;
      this.lbTitleText.Localisation = "mptLabel1";
      this.lbTitleText.LocalisationContext = "panelMiddle";
      this.lbTitleText.Location = new System.Drawing.Point(186, 8);
      this.lbTitleText.Name = "lbTitleText";
      this.lbTitleText.Size = new System.Drawing.Size(27, 13);
      this.lbTitleText.TabIndex = 12;
      this.lbTitleText.Text = "Title";
      // 
      // lbAlbum
      // 
      this.lbAlbum.AutoSize = true;
      this.lbAlbum.Localisation = "Album";
      this.lbAlbum.LocalisationContext = "Player";
      this.lbAlbum.Location = new System.Drawing.Point(99, 42);
      this.lbAlbum.MinimumSize = new System.Drawing.Size(80, 0);
      this.lbAlbum.Name = "lbAlbum";
      this.lbAlbum.Size = new System.Drawing.Size(80, 13);
      this.lbAlbum.TabIndex = 11;
      this.lbAlbum.Text = "Album";
      // 
      // lbArtist
      // 
      this.lbArtist.AutoSize = true;
      this.lbArtist.Localisation = "Artist";
      this.lbArtist.LocalisationContext = "Player";
      this.lbArtist.Location = new System.Drawing.Point(99, 24);
      this.lbArtist.MinimumSize = new System.Drawing.Size(80, 0);
      this.lbArtist.Name = "lbArtist";
      this.lbArtist.Size = new System.Drawing.Size(80, 13);
      this.lbArtist.TabIndex = 10;
      this.lbArtist.Text = "Artist";
      // 
      // lbTitle
      // 
      this.lbTitle.AutoSize = true;
      this.lbTitle.Localisation = "Title";
      this.lbTitle.LocalisationContext = "Player";
      this.lbTitle.Location = new System.Drawing.Point(98, 8);
      this.lbTitle.MinimumSize = new System.Drawing.Size(80, 0);
      this.lbTitle.Name = "lbTitle";
      this.lbTitle.Size = new System.Drawing.Size(80, 13);
      this.lbTitle.TabIndex = 9;
      this.lbTitle.Text = "Title";
      // 
      // pictureBoxSpectrum
      // 
      this.pictureBoxSpectrum.BackColor = System.Drawing.Color.Black;
      this.pictureBoxSpectrum.Location = new System.Drawing.Point(5, 28);
      this.pictureBoxSpectrum.Name = "pictureBoxSpectrum";
      this.pictureBoxSpectrum.Size = new System.Drawing.Size(87, 26);
      this.pictureBoxSpectrum.TabIndex = 1;
      this.pictureBoxSpectrum.TabStop = false;
      this.pictureBoxSpectrum.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseDown);
      // 
      // pictureBoxTime
      // 
      this.pictureBoxTime.BackColor = System.Drawing.Color.Black;
      this.pictureBoxTime.Location = new System.Drawing.Point(5, 2);
      this.pictureBoxTime.Name = "pictureBoxTime";
      this.pictureBoxTime.Size = new System.Drawing.Size(87, 26);
      this.pictureBoxTime.TabIndex = 0;
      this.pictureBoxTime.TabStop = false;
      // 
      // panelRight
      // 
      this.panelRight.Controls.Add(this.btnPlayList);
      this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelRight.Location = new System.Drawing.Point(647, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new System.Drawing.Size(121, 60);
      this.panelRight.TabIndex = 10;
      // 
      // btnPlayList
      // 
      this.btnPlayList.Localisation = "PlaylistButton";
      this.btnPlayList.LocalisationContext = "Player";
      this.btnPlayList.Location = new System.Drawing.Point(7, 8);
      this.btnPlayList.Name = "btnPlayList";
      this.btnPlayList.Size = new System.Drawing.Size(30, 23);
      this.btnPlayList.TabIndex = 8;
      this.btnPlayList.Text = "PL";
      this.btnPlayList.UseVisualStyleBackColor = true;
      this.btnPlayList.Click += new System.EventHandler(this.btnPlayList_Click);
      // 
      // panelLeft
      // 
      this.panelLeft.Controls.Add(this.pictureBoxPlayPause);
      this.panelLeft.Controls.Add(this.buttonPrev);
      this.panelLeft.Controls.Add(this.buttonNext);
      this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.panelLeft.Location = new System.Drawing.Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new System.Drawing.Size(170, 60);
      this.panelLeft.TabIndex = 9;
      // 
      // pictureBoxPlayPause
      // 
      this.pictureBoxPlayPause.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPlayPause.Image")));
      this.pictureBoxPlayPause.Location = new System.Drawing.Point(56, 4);
      this.pictureBoxPlayPause.Name = "pictureBoxPlayPause";
      this.pictureBoxPlayPause.Size = new System.Drawing.Size(49, 52);
      this.pictureBoxPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPlayPause.TabIndex = 7;
      this.pictureBoxPlayPause.TabStop = false;
      this.pictureBoxPlayPause.Click += new System.EventHandler(this.buttonPlay_Click);
      // 
      // buttonPrev
      // 
      this.buttonPrev.Image = global::MPTagThat.Properties.Resources.playprev;
      this.buttonPrev.Location = new System.Drawing.Point(15, 28);
      this.buttonPrev.Name = "buttonPrev";
      this.buttonPrev.Size = new System.Drawing.Size(25, 17);
      this.buttonPrev.TabIndex = 2;
      this.buttonPrev.UseVisualStyleBackColor = true;
      this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
      // 
      // buttonNext
      // 
      this.buttonNext.Image = global::MPTagThat.Properties.Resources.playnext;
      this.buttonNext.Location = new System.Drawing.Point(120, 28);
      this.buttonNext.Name = "buttonNext";
      this.buttonNext.Size = new System.Drawing.Size(25, 17);
      this.buttonNext.TabIndex = 6;
      this.buttonNext.UseVisualStyleBackColor = true;
      this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
      // 
      // panelTop
      // 
      this.panelTop.Controls.Add(this.playBackSlider);
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(768, 10);
      this.panelTop.TabIndex = 12;
      // 
      // playBackSlider
      // 
      this.playBackSlider.BackColor = System.Drawing.Color.Transparent;
      this.playBackSlider.BarInnerColor = System.Drawing.Color.Black;
      this.playBackSlider.BarOuterColor = System.Drawing.Color.Black;
      this.playBackSlider.BorderRoundRectSize = new System.Drawing.Size(8, 8);
      this.playBackSlider.Dock = System.Windows.Forms.DockStyle.Fill;
      this.playBackSlider.ElapsedInnerColor = System.Drawing.Color.Orange;
      this.playBackSlider.ElapsedOuterColor = System.Drawing.Color.Orange;
      this.playBackSlider.LargeChange = ((uint)(5u));
      this.playBackSlider.Location = new System.Drawing.Point(0, 0);
      this.playBackSlider.Name = "playBackSlider";
      this.playBackSlider.Size = new System.Drawing.Size(768, 10);
      this.playBackSlider.SmallChange = ((uint)(1u));
      this.playBackSlider.TabIndex = 0;
      this.playBackSlider.Text = "colorSlider1";
      this.playBackSlider.ThumbInnerColor = System.Drawing.Color.OrangeRed;
      this.playBackSlider.ThumbOuterColor = System.Drawing.Color.OrangeRed;
      this.playBackSlider.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
      this.playBackSlider.ThumbSize = 20;
      this.playBackSlider.Value = 0;
      this.playBackSlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.playBackSlider_Scroll);
      // 
      // panelBottom
      // 
      this.panelBottom.Controls.Add(this.panelMiddle);
      this.panelBottom.Controls.Add(this.panelLeft);
      this.panelBottom.Controls.Add(this.panelRight);
      this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelBottom.Location = new System.Drawing.Point(0, 10);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new System.Drawing.Size(768, 60);
      this.panelBottom.TabIndex = 13;
      // 
      // PlayerControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panelBottom);
      this.Controls.Add(this.panelTop);
      this.Name = "PlayerControl";
      this.Size = new System.Drawing.Size(768, 70);
      this.Load += new System.EventHandler(this.OnLoad);
      this.panelMiddle.ResumeLayout(false);
      this.panelMiddle.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).EndInit();
      this.panelRight.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayPause)).EndInit();
      this.panelTop.ResumeLayout(false);
      this.panelBottom.ResumeLayout(false);
      this.panelBottom.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBoxTime;
    private System.Windows.Forms.PictureBox pictureBoxSpectrum;
    private System.Windows.Forms.Button buttonPrev;
    private System.Windows.Forms.Button buttonNext;
    private MPTagThat.Core.WinControls.TTPanel panelLeft;
    private MPTagThat.Core.WinControls.TTPanel panelRight;
    private MPTagThat.Core.WinControls.TTPanel panelMiddle;
    private System.Windows.Forms.PictureBox pictureBoxPlayPause;
    private MPTagThat.Core.WinControls.MPTLabel lbTitle;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbum;
    private MPTagThat.Core.WinControls.MPTLabel lbArtist;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbumText;
    private MPTagThat.Core.WinControls.MPTLabel lbArtistText;
    private MPTagThat.Core.WinControls.MPTLabel lbTitleText;
    private MPTagThat.Core.WinControls.TTPanel panelTop;
    private MPTagThat.Core.WinControls.TTPanel panelBottom;
    private MPTagThat.Core.ColorSlider playBackSlider;
    private MPTagThat.Core.WinControls.MPTButton btnPlayList;
  }
}
