namespace MPTagThat.InternetLookup
{
  partial class AlbumDetails
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tbArtist = new System.Windows.Forms.TextBox();
      this.tbAlbum = new System.Windows.Forms.TextBox();
      this.tbYear = new System.Windows.Forms.TextBox();
      this.cbGenre = new System.Windows.Forms.ComboBox();
      this.lvAlbumTracks = new System.Windows.Forms.ListView();
      this.chTrackNum = new System.Windows.Forms.ColumnHeader();
      this.chTitle = new System.Windows.Forms.ColumnHeader();
      this.lvDiscTracks = new System.Windows.Forms.ListView();
      this.chFileName = new System.Windows.Forms.ColumnHeader();
      this.pictureBoxCover = new System.Windows.Forms.PictureBox();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.btContinue = new MPTagThat.Core.WinControls.MPTButton();
      this.lbTracksToTag = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbAlbumTracks = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbGenre = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbYear = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.btUp = new MPTagThat.Core.WinControls.MPTButton();
      this.btDown = new MPTagThat.Core.WinControls.MPTButton();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).BeginInit();
      this.SuspendLayout();
      // 
      // tbArtist
      // 
      this.tbArtist.Location = new System.Drawing.Point(211, 25);
      this.tbArtist.Name = "tbArtist";
      this.tbArtist.Size = new System.Drawing.Size(306, 20);
      this.tbArtist.TabIndex = 1;
      // 
      // tbAlbum
      // 
      this.tbAlbum.Location = new System.Drawing.Point(211, 57);
      this.tbAlbum.Name = "tbAlbum";
      this.tbAlbum.Size = new System.Drawing.Size(306, 20);
      this.tbAlbum.TabIndex = 2;
      // 
      // tbYear
      // 
      this.tbYear.Location = new System.Drawing.Point(211, 89);
      this.tbYear.Name = "tbYear";
      this.tbYear.Size = new System.Drawing.Size(89, 20);
      this.tbYear.TabIndex = 3;
      // 
      // cbGenre
      // 
      this.cbGenre.FormattingEnabled = true;
      this.cbGenre.Location = new System.Drawing.Point(211, 121);
      this.cbGenre.Name = "cbGenre";
      this.cbGenre.Size = new System.Drawing.Size(306, 21);
      this.cbGenre.TabIndex = 4;
      // 
      // lvAlbumTracks
      // 
      this.lvAlbumTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTrackNum,
            this.chTitle});
      this.lvAlbumTracks.FullRowSelect = true;
      this.lvAlbumTracks.Location = new System.Drawing.Point(30, 211);
      this.lvAlbumTracks.MultiSelect = false;
      this.lvAlbumTracks.Name = "lvAlbumTracks";
      this.lvAlbumTracks.Size = new System.Drawing.Size(291, 298);
      this.lvAlbumTracks.TabIndex = 10;
      this.lvAlbumTracks.UseCompatibleStateImageBehavior = false;
      this.lvAlbumTracks.View = System.Windows.Forms.View.Details;
      // 
      // chTrackNum
      // 
      this.chTrackNum.Text = "#";
      this.chTrackNum.Width = 43;
      // 
      // chTitle
      // 
      this.chTitle.Text = "Title";
      this.chTitle.Width = 243;
      // 
      // lvDiscTracks
      // 
      this.lvDiscTracks.CheckBoxes = true;
      this.lvDiscTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFileName});
      this.lvDiscTracks.FullRowSelect = true;
      this.lvDiscTracks.Location = new System.Drawing.Point(354, 211);
      this.lvDiscTracks.MultiSelect = false;
      this.lvDiscTracks.Name = "lvDiscTracks";
      this.lvDiscTracks.Size = new System.Drawing.Size(344, 298);
      this.lvDiscTracks.TabIndex = 11;
      this.lvDiscTracks.UseCompatibleStateImageBehavior = false;
      this.lvDiscTracks.View = System.Windows.Forms.View.Details;
      // 
      // chFileName
      // 
      this.chFileName.Text = "File Name";
      this.chFileName.Width = 338;
      // 
      // pictureBoxCover
      // 
      this.pictureBoxCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.pictureBoxCover.Location = new System.Drawing.Point(578, 22);
      this.pictureBoxCover.Name = "pictureBoxCover";
      this.pictureBoxCover.Size = new System.Drawing.Size(120, 120);
      this.pictureBoxCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBoxCover.TabIndex = 12;
      this.pictureBoxCover.TabStop = false;
      // 
      // btCancel
      // 
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "Lookup";
      this.btCancel.Location = new System.Drawing.Point(417, 536);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(125, 43);
      this.btCancel.TabIndex = 6;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      // 
      // btContinue
      // 
      this.btContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btContinue.Localisation = "Continue";
      this.btContinue.LocalisationContext = "Lookup";
      this.btContinue.Location = new System.Drawing.Point(214, 536);
      this.btContinue.Name = "btContinue";
      this.btContinue.Size = new System.Drawing.Size(125, 43);
      this.btContinue.TabIndex = 5;
      this.btContinue.Text = "Continue >";
      this.btContinue.UseVisualStyleBackColor = true;
      // 
      // lbTracksToTag
      // 
      this.lbTracksToTag.AutoSize = true;
      this.lbTracksToTag.Localisation = "TracksToTag";
      this.lbTracksToTag.LocalisationContext = "AlbumDetails";
      this.lbTracksToTag.Location = new System.Drawing.Point(351, 179);
      this.lbTracksToTag.Name = "lbTracksToTag";
      this.lbTracksToTag.Size = new System.Drawing.Size(74, 13);
      this.lbTracksToTag.TabIndex = 9;
      this.lbTracksToTag.Text = "Tracks to Tag";
      // 
      // lbAlbumTracks
      // 
      this.lbAlbumTracks.AutoSize = true;
      this.lbAlbumTracks.Localisation = "AlbumTracks";
      this.lbAlbumTracks.LocalisationContext = "AlbumDetails";
      this.lbAlbumTracks.Location = new System.Drawing.Point(27, 179);
      this.lbAlbumTracks.Name = "lbAlbumTracks";
      this.lbAlbumTracks.Size = new System.Drawing.Size(87, 13);
      this.lbAlbumTracks.TabIndex = 8;
      this.lbAlbumTracks.Text = "Tracks on Album";
      // 
      // lbGenre
      // 
      this.lbGenre.AutoSize = true;
      this.lbGenre.Localisation = "Genre";
      this.lbGenre.LocalisationContext = "TagEdit";
      this.lbGenre.Location = new System.Drawing.Point(25, 125);
      this.lbGenre.Name = "lbGenre";
      this.lbGenre.Size = new System.Drawing.Size(39, 13);
      this.lbGenre.TabIndex = 3;
      this.lbGenre.Text = "Genre:";
      // 
      // lbYear
      // 
      this.lbYear.AutoSize = true;
      this.lbYear.Localisation = "Year";
      this.lbYear.LocalisationContext = "TagEdit";
      this.lbYear.Location = new System.Drawing.Point(25, 93);
      this.lbYear.Name = "lbYear";
      this.lbYear.Size = new System.Drawing.Size(32, 13);
      this.lbYear.TabIndex = 2;
      this.lbYear.Text = "Year:";
      // 
      // lbAlbum
      // 
      this.lbAlbum.AutoSize = true;
      this.lbAlbum.Localisation = "Album";
      this.lbAlbum.LocalisationContext = "TagEdit";
      this.lbAlbum.Location = new System.Drawing.Point(25, 61);
      this.lbAlbum.Name = "lbAlbum";
      this.lbAlbum.Size = new System.Drawing.Size(39, 13);
      this.lbAlbum.TabIndex = 1;
      this.lbAlbum.Text = "Album:";
      // 
      // lbArtist
      // 
      this.lbArtist.AutoSize = true;
      this.lbArtist.Localisation = "Artist";
      this.lbArtist.LocalisationContext = "TagEdit";
      this.lbArtist.Location = new System.Drawing.Point(25, 29);
      this.lbArtist.Name = "lbArtist";
      this.lbArtist.Size = new System.Drawing.Size(33, 13);
      this.lbArtist.TabIndex = 0;
      this.lbArtist.Text = "Artist:";
      // 
      // btUp
      // 
      this.btUp.Localisation = "MoveUp";
      this.btUp.LocalisationContext = "AlbumDetails";
      this.btUp.Location = new System.Drawing.Point(716, 283);
      this.btUp.Name = "btUp";
      this.btUp.Size = new System.Drawing.Size(115, 49);
      this.btUp.TabIndex = 7;
      this.btUp.Text = "Move Up";
      this.btUp.UseVisualStyleBackColor = true;
      this.btUp.Click += new System.EventHandler(this.btUp_Click);
      // 
      // btDown
      // 
      this.btDown.Localisation = "MoveDown";
      this.btDown.LocalisationContext = "AlbumDetails";
      this.btDown.Location = new System.Drawing.Point(716, 369);
      this.btDown.Name = "btDown";
      this.btDown.Size = new System.Drawing.Size(115, 49);
      this.btDown.TabIndex = 8;
      this.btDown.Text = "Move Down";
      this.btDown.UseVisualStyleBackColor = true;
      this.btDown.Click += new System.EventHandler(this.btDown_Click);
      // 
      // AlbumDetails
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(845, 599);
      this.Controls.Add(this.btDown);
      this.Controls.Add(this.btUp);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btContinue);
      this.Controls.Add(this.pictureBoxCover);
      this.Controls.Add(this.lvDiscTracks);
      this.Controls.Add(this.lvAlbumTracks);
      this.Controls.Add(this.lbTracksToTag);
      this.Controls.Add(this.lbAlbumTracks);
      this.Controls.Add(this.cbGenre);
      this.Controls.Add(this.tbYear);
      this.Controls.Add(this.tbAlbum);
      this.Controls.Add(this.tbArtist);
      this.Controls.Add(this.lbGenre);
      this.Controls.Add(this.lbYear);
      this.Controls.Add(this.lbAlbum);
      this.Controls.Add(this.lbArtist);
      this.Name = "AlbumDetails";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Details on selected Album";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTLabel lbArtist;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbum;
    private MPTagThat.Core.WinControls.MPTLabel lbYear;
    private MPTagThat.Core.WinControls.MPTLabel lbGenre;
    private System.Windows.Forms.TextBox tbArtist;
    private System.Windows.Forms.TextBox tbAlbum;
    private System.Windows.Forms.TextBox tbYear;
    private System.Windows.Forms.ComboBox cbGenre;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbumTracks;
    private MPTagThat.Core.WinControls.MPTLabel lbTracksToTag;
    private System.Windows.Forms.ListView lvAlbumTracks;
    private System.Windows.Forms.ColumnHeader chTrackNum;
    private System.Windows.Forms.ColumnHeader chTitle;
    private System.Windows.Forms.ListView lvDiscTracks;
    private System.Windows.Forms.ColumnHeader chFileName;
    private System.Windows.Forms.PictureBox pictureBoxCover;
    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private MPTagThat.Core.WinControls.MPTButton btContinue;
    private MPTagThat.Core.WinControls.MPTButton btUp;
    private MPTagThat.Core.WinControls.MPTButton btDown;
  }
}