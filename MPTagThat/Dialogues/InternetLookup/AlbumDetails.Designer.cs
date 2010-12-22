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
      this.lvAlbumTracks = new System.Windows.Forms.ListView();
      this.chTrackNum = new System.Windows.Forms.ColumnHeader();
      this.chTitle = new System.Windows.Forms.ColumnHeader();
      this.lvDiscTracks = new System.Windows.Forms.ListView();
      this.chNumber = new System.Windows.Forms.ColumnHeader();
      this.chFileName = new System.Windows.Forms.ColumnHeader();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.btContinue = new MPTagThat.Core.WinControls.MPTButton();
      this.lbTracksToTag = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbAlbumTracks = new MPTagThat.Core.WinControls.MPTLabel();
      this.btUp = new MPTagThat.Core.WinControls.MPTButton();
      this.btDown = new MPTagThat.Core.WinControls.MPTButton();
      this.labelHeader = new MPTagThat.Core.WinControls.MPTLabel();
      this.GroupBoxAlbumDetails = new System.Windows.Forms.GroupBox();
      this.pictureBoxCover = new System.Windows.Forms.PictureBox();
      this.cbGenre = new System.Windows.Forms.ComboBox();
      this.tbYear = new System.Windows.Forms.TextBox();
      this.tbAlbum = new System.Windows.Forms.TextBox();
      this.tbArtist = new System.Windows.Forms.TextBox();
      this.lbGenre = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbYear = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.GroupBoxAlbumDetails.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).BeginInit();
      this.SuspendLayout();
      // 
      // lvAlbumTracks
      // 
      this.lvAlbumTracks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.lvAlbumTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTrackNum,
            this.chTitle});
      this.lvAlbumTracks.FullRowSelect = true;
      this.lvAlbumTracks.Location = new System.Drawing.Point(22, 212);
      this.lvAlbumTracks.MultiSelect = false;
      this.lvAlbumTracks.Name = "lvAlbumTracks";
      this.lvAlbumTracks.Size = new System.Drawing.Size(315, 298);
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
      this.chTitle.Width = 231;
      // 
      // lvDiscTracks
      // 
      this.lvDiscTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lvDiscTracks.CheckBoxes = true;
      this.lvDiscTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chNumber,
            this.chFileName});
      this.lvDiscTracks.FullRowSelect = true;
      this.lvDiscTracks.Location = new System.Drawing.Point(367, 212);
      this.lvDiscTracks.MultiSelect = false;
      this.lvDiscTracks.Name = "lvDiscTracks";
      this.lvDiscTracks.Size = new System.Drawing.Size(352, 298);
      this.lvDiscTracks.TabIndex = 11;
      this.lvDiscTracks.UseCompatibleStateImageBehavior = false;
      this.lvDiscTracks.View = System.Windows.Forms.View.Details;
      // 
      // chNumber
      // 
      this.chNumber.Text = "#";
      // 
      // chFileName
      // 
      this.chFileName.Text = "File Name";
      this.chFileName.Width = 250;
      // 
      // btCancel
      // 
      this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "Lookup";
      this.btCancel.Location = new System.Drawing.Point(664, 520);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(100, 23);
      this.btCancel.TabIndex = 6;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      // 
      // btContinue
      // 
      this.btContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btContinue.Localisation = "Continue";
      this.btContinue.LocalisationContext = "Lookup";
      this.btContinue.Location = new System.Drawing.Point(558, 520);
      this.btContinue.Name = "btContinue";
      this.btContinue.Size = new System.Drawing.Size(100, 23);
      this.btContinue.TabIndex = 5;
      this.btContinue.Text = "Continue >";
      this.btContinue.UseVisualStyleBackColor = true;
      // 
      // lbTracksToTag
      // 
      this.lbTracksToTag.AutoSize = true;
      this.lbTracksToTag.Localisation = "TracksToTag";
      this.lbTracksToTag.LocalisationContext = "AlbumDetails";
      this.lbTracksToTag.Location = new System.Drawing.Point(364, 192);
      this.lbTracksToTag.Name = "lbTracksToTag";
      this.lbTracksToTag.Size = new System.Drawing.Size(70, 13);
      this.lbTracksToTag.TabIndex = 9;
      this.lbTracksToTag.Text = "Tracks to tag";
      // 
      // lbAlbumTracks
      // 
      this.lbAlbumTracks.AutoSize = true;
      this.lbAlbumTracks.Localisation = "AlbumTracks";
      this.lbAlbumTracks.LocalisationContext = "AlbumDetails";
      this.lbAlbumTracks.Location = new System.Drawing.Point(19, 192);
      this.lbAlbumTracks.Name = "lbAlbumTracks";
      this.lbAlbumTracks.Size = new System.Drawing.Size(86, 13);
      this.lbAlbumTracks.TabIndex = 8;
      this.lbAlbumTracks.Text = "Tracks on album";
      // 
      // btUp
      // 
      this.btUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btUp.Localisation = "MoveUp";
      this.btUp.LocalisationContext = "AlbumDetails";
      this.btUp.Location = new System.Drawing.Point(725, 339);
      this.btUp.Name = "btUp";
      this.btUp.Size = new System.Drawing.Size(50, 23);
      this.btUp.TabIndex = 7;
      this.btUp.Text = "Up";
      this.btUp.UseVisualStyleBackColor = true;
      this.btUp.Click += new System.EventHandler(this.btUp_Click);
      // 
      // btDown
      // 
      this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btDown.Localisation = "MoveDown";
      this.btDown.LocalisationContext = "AlbumDetails";
      this.btDown.Location = new System.Drawing.Point(725, 368);
      this.btDown.Name = "btDown";
      this.btDown.Size = new System.Drawing.Size(50, 23);
      this.btDown.TabIndex = 8;
      this.btDown.Text = "Down";
      this.btDown.UseVisualStyleBackColor = true;
      this.btDown.Click += new System.EventHandler(this.btDown_Click);
      // 
      // labelHeader
      // 
      this.labelHeader.AutoSize = true;
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.ForeColor = System.Drawing.Color.White;
      this.labelHeader.Location = new System.Drawing.Point(18, 22);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(62, 20);
      this.labelHeader.TabIndex = 23;
      this.labelHeader.Text = "Header";
      // 
      // GroupBoxAlbumDetails
      // 
      this.GroupBoxAlbumDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.GroupBoxAlbumDetails.Controls.Add(this.pictureBoxCover);
      this.GroupBoxAlbumDetails.Controls.Add(this.cbGenre);
      this.GroupBoxAlbumDetails.Controls.Add(this.tbYear);
      this.GroupBoxAlbumDetails.Controls.Add(this.tbAlbum);
      this.GroupBoxAlbumDetails.Controls.Add(this.tbArtist);
      this.GroupBoxAlbumDetails.Controls.Add(this.lbGenre);
      this.GroupBoxAlbumDetails.Controls.Add(this.lbYear);
      this.GroupBoxAlbumDetails.Controls.Add(this.lbAlbum);
      this.GroupBoxAlbumDetails.Controls.Add(this.lbArtist);
      this.GroupBoxAlbumDetails.Location = new System.Drawing.Point(22, 54);
      this.GroupBoxAlbumDetails.Name = "GroupBoxAlbumDetails";
      this.GroupBoxAlbumDetails.Size = new System.Drawing.Size(742, 121);
      this.GroupBoxAlbumDetails.TabIndex = 24;
      this.GroupBoxAlbumDetails.TabStop = false;
      this.GroupBoxAlbumDetails.Text = "Internet album details";
      // 
      // pictureBoxCover
      // 
      this.pictureBoxCover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBoxCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.pictureBoxCover.Location = new System.Drawing.Point(634, 13);
      this.pictureBoxCover.Name = "pictureBoxCover";
      this.pictureBoxCover.Size = new System.Drawing.Size(100, 100);
      this.pictureBoxCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBoxCover.TabIndex = 13;
      this.pictureBoxCover.TabStop = false;
      // 
      // cbGenre
      // 
      this.cbGenre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cbGenre.FormattingEnabled = true;
      this.cbGenre.Location = new System.Drawing.Point(226, 86);
      this.cbGenre.Name = "cbGenre";
      this.cbGenre.Size = new System.Drawing.Size(390, 21);
      this.cbGenre.TabIndex = 12;
      // 
      // tbYear
      // 
      this.tbYear.Location = new System.Drawing.Point(71, 86);
      this.tbYear.Name = "tbYear";
      this.tbYear.Size = new System.Drawing.Size(89, 20);
      this.tbYear.TabIndex = 10;
      // 
      // tbAlbum
      // 
      this.tbAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tbAlbum.Location = new System.Drawing.Point(71, 54);
      this.tbAlbum.Name = "tbAlbum";
      this.tbAlbum.Size = new System.Drawing.Size(545, 20);
      this.tbAlbum.TabIndex = 9;
      // 
      // tbArtist
      // 
      this.tbArtist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tbArtist.Location = new System.Drawing.Point(71, 22);
      this.tbArtist.Name = "tbArtist";
      this.tbArtist.Size = new System.Drawing.Size(545, 20);
      this.tbArtist.TabIndex = 6;
      // 
      // lbGenre
      // 
      this.lbGenre.AutoSize = true;
      this.lbGenre.Localisation = "Genre";
      this.lbGenre.LocalisationContext = "TagEdit";
      this.lbGenre.Location = new System.Drawing.Point(181, 89);
      this.lbGenre.Name = "lbGenre";
      this.lbGenre.Size = new System.Drawing.Size(39, 13);
      this.lbGenre.TabIndex = 11;
      this.lbGenre.Text = "Genre:";
      // 
      // lbYear
      // 
      this.lbYear.AutoSize = true;
      this.lbYear.Localisation = "Year";
      this.lbYear.LocalisationContext = "TagEdit";
      this.lbYear.Location = new System.Drawing.Point(9, 89);
      this.lbYear.Name = "lbYear";
      this.lbYear.Size = new System.Drawing.Size(32, 13);
      this.lbYear.TabIndex = 8;
      this.lbYear.Text = "Year:";
      // 
      // lbAlbum
      // 
      this.lbAlbum.AutoSize = true;
      this.lbAlbum.Localisation = "Album";
      this.lbAlbum.LocalisationContext = "TagEdit";
      this.lbAlbum.Location = new System.Drawing.Point(9, 57);
      this.lbAlbum.Name = "lbAlbum";
      this.lbAlbum.Size = new System.Drawing.Size(39, 13);
      this.lbAlbum.TabIndex = 7;
      this.lbAlbum.Text = "Album:";
      // 
      // lbArtist
      // 
      this.lbArtist.AutoSize = true;
      this.lbArtist.Localisation = "Artist";
      this.lbArtist.LocalisationContext = "TagEdit";
      this.lbArtist.Location = new System.Drawing.Point(9, 25);
      this.lbArtist.Name = "lbArtist";
      this.lbArtist.Size = new System.Drawing.Size(33, 13);
      this.lbArtist.TabIndex = 5;
      this.lbArtist.Text = "Artist:";
      // 
      // AlbumDetails
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderColor = System.Drawing.Color.LightGray;
      this.ClientSize = new System.Drawing.Size(787, 555);
      this.Controls.Add(this.GroupBoxAlbumDetails);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.btDown);
      this.Controls.Add(this.btUp);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btContinue);
      this.Controls.Add(this.lvDiscTracks);
      this.Controls.Add(this.lvAlbumTracks);
      this.Controls.Add(this.lbTracksToTag);
      this.Controls.Add(this.lbAlbumTracks);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "AlbumDetails";
      this.Resizeable = true;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Details on selected Album";
      this.GroupBoxAlbumDetails.ResumeLayout(false);
      this.GroupBoxAlbumDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTLabel lbAlbumTracks;
    private MPTagThat.Core.WinControls.MPTLabel lbTracksToTag;
    private System.Windows.Forms.ListView lvAlbumTracks;
    private System.Windows.Forms.ColumnHeader chTrackNum;
    private System.Windows.Forms.ColumnHeader chTitle;
    private System.Windows.Forms.ListView lvDiscTracks;
    private System.Windows.Forms.ColumnHeader chFileName;
    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private MPTagThat.Core.WinControls.MPTButton btContinue;
    private MPTagThat.Core.WinControls.MPTButton btUp;
    private MPTagThat.Core.WinControls.MPTButton btDown;
    private MPTagThat.Core.WinControls.MPTLabel labelHeader;
    private System.Windows.Forms.GroupBox GroupBoxAlbumDetails;
    private System.Windows.Forms.ComboBox cbGenre;
    private System.Windows.Forms.TextBox tbYear;
    private System.Windows.Forms.TextBox tbAlbum;
    private System.Windows.Forms.TextBox tbArtist;
    private MPTagThat.Core.WinControls.MPTLabel lbGenre;
    private MPTagThat.Core.WinControls.MPTLabel lbYear;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbum;
    private MPTagThat.Core.WinControls.MPTLabel lbArtist;
    private System.Windows.Forms.PictureBox pictureBoxCover;
    private System.Windows.Forms.ColumnHeader chNumber;
  }
}