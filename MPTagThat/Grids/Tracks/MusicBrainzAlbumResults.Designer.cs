namespace MPTagThat.GridView
{
  partial class MusicBrainzAlbumResults
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
      this.lvSearchResults = new System.Windows.Forms.ListView();
      this.chAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.chDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxMusicBrainzMultipleAlbums = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.btUpdate = new MPTagThat.Core.WinControls.MPTButton();
      this.btClose = new MPTagThat.Core.WinControls.MPTButton();
      this.lbArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.gbSearchInfo = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.tbTitle = new System.Windows.Forms.TextBox();
      this.tbArtist = new System.Windows.Forms.TextBox();
      this.chYear = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.chCountry = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxMusicBrainzMultipleAlbums.SuspendLayout();
      this.gbSearchInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // lvSearchResults
      // 
      this.lvSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lvSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAlbum,
            this.chCountry,
            this.chYear,
            this.chDuration});
      this.lvSearchResults.FullRowSelect = true;
      this.lvSearchResults.Location = new System.Drawing.Point(17, 27);
      this.lvSearchResults.MultiSelect = false;
      this.lvSearchResults.Name = "lvSearchResults";
      this.lvSearchResults.Size = new System.Drawing.Size(551, 328);
      this.lvSearchResults.TabIndex = 10;
      this.lvSearchResults.UseCompatibleStateImageBehavior = false;
      this.lvSearchResults.View = System.Windows.Forms.View.Details;
      this.lvSearchResults.DoubleClick += new System.EventHandler(this.lvSearchResults_DoubleClick);
      // 
      // chAlbum
      // 
      this.chAlbum.Text = "Album";
      this.chAlbum.Width = 330;
      // 
      // chDuration
      // 
      this.chDuration.Text = "Duration";
      this.chDuration.Width = 65;
      // 
      // groupBoxMusicBrainzMultipleAlbums
      // 
      this.groupBoxMusicBrainzMultipleAlbums.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMusicBrainzMultipleAlbums.Controls.Add(this.lvSearchResults);
      this.groupBoxMusicBrainzMultipleAlbums.Id = "a6738626-e59a-47c0-b087-323432709d2a";
      this.groupBoxMusicBrainzMultipleAlbums.Localisation = "GroupBoxResults";
      this.groupBoxMusicBrainzMultipleAlbums.LocalisationContext = "MusicBrainz";
      this.groupBoxMusicBrainzMultipleAlbums.Location = new System.Drawing.Point(21, 121);
      this.groupBoxMusicBrainzMultipleAlbums.Name = "groupBoxMusicBrainzMultipleAlbums";
      this.groupBoxMusicBrainzMultipleAlbums.Size = new System.Drawing.Size(587, 372);
      this.groupBoxMusicBrainzMultipleAlbums.TabIndex = 39;
      this.groupBoxMusicBrainzMultipleAlbums.Text = "The track is available on multiple albums. Please select";
      // 
      // lbTitle
      // 
      this.lbTitle.Localisation = "Title";
      this.lbTitle.LocalisationContext = "TagEdit";
      this.lbTitle.Location = new System.Drawing.Point(15, 56);
      this.lbTitle.Name = "lbTitle";
      this.lbTitle.Size = new System.Drawing.Size(30, 13);
      this.lbTitle.TabIndex = 2;
      this.lbTitle.Text = "Title:";
      // 
      // btUpdate
      // 
      this.btUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btUpdate.Id = "7d2c1459-fc4c-4f07-a662-361fa0a6f8bc";
      this.btUpdate.Localisation = "Update";
      this.btUpdate.LocalisationContext = "MusicBrainz";
      this.btUpdate.Location = new System.Drawing.Point(402, 510);
      this.btUpdate.Name = "btUpdate";
      this.btUpdate.Size = new System.Drawing.Size(100, 23);
      this.btUpdate.TabIndex = 43;
      this.btUpdate.Text = "Select";
      this.btUpdate.UseVisualStyleBackColor = true;
      this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
      // 
      // btClose
      // 
      this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btClose.Id = "cc06bed7-bb80-4812-a3c0-9b8d570220d4";
      this.btClose.Localisation = "Cancel";
      this.btClose.LocalisationContext = "MusicBrainz";
      this.btClose.Location = new System.Drawing.Point(508, 510);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(100, 23);
      this.btClose.TabIndex = 44;
      this.btClose.Text = "Cancel";
      this.btClose.UseVisualStyleBackColor = true;
      this.btClose.Click += new System.EventHandler(this.btClose_Click);
      // 
      // lbArtist
      // 
      this.lbArtist.Localisation = "Artist";
      this.lbArtist.LocalisationContext = "TagEdit";
      this.lbArtist.Location = new System.Drawing.Point(15, 29);
      this.lbArtist.Name = "lbArtist";
      this.lbArtist.Size = new System.Drawing.Size(33, 13);
      this.lbArtist.TabIndex = 0;
      this.lbArtist.Text = "Artist:";
      // 
      // gbSearchInfo
      // 
      this.gbSearchInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gbSearchInfo.Controls.Add(this.tbTitle);
      this.gbSearchInfo.Controls.Add(this.lbTitle);
      this.gbSearchInfo.Controls.Add(this.tbArtist);
      this.gbSearchInfo.Controls.Add(this.lbArtist);
      this.gbSearchInfo.Id = "21d39af9-71f4-4959-a434-31df4106de5a";
      this.gbSearchInfo.Localisation = "SearchInfo";
      this.gbSearchInfo.LocalisationContext = "MusicBrainz";
      this.gbSearchInfo.Location = new System.Drawing.Point(21, 21);
      this.gbSearchInfo.Name = "gbSearchInfo";
      this.gbSearchInfo.Size = new System.Drawing.Size(587, 85);
      this.gbSearchInfo.TabIndex = 38;
      this.gbSearchInfo.Text = "Identified track: ";
      // 
      // tbTitle
      // 
      this.tbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbTitle.Enabled = false;
      this.tbTitle.Location = new System.Drawing.Point(73, 53);
      this.tbTitle.Name = "tbTitle";
      this.tbTitle.ReadOnly = true;
      this.tbTitle.Size = new System.Drawing.Size(495, 20);
      this.tbTitle.TabIndex = 2;
      // 
      // tbArtist
      // 
      this.tbArtist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbArtist.Enabled = false;
      this.tbArtist.Location = new System.Drawing.Point(73, 26);
      this.tbArtist.Name = "tbArtist";
      this.tbArtist.ReadOnly = true;
      this.tbArtist.Size = new System.Drawing.Size(495, 20);
      this.tbArtist.TabIndex = 1;
      // 
      // chYear
      // 
      this.chYear.Text = "Year";
      this.chYear.Width = 63;
      // 
      // chCountry
      // 
      this.chCountry.Text = "Country";
      this.chCountry.Width = 88;
      // 
      // MusicBrainzAlbumResults
      // 
      this.AcceptButton = this.btUpdate;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btClose;
      this.ClientSize = new System.Drawing.Size(631, 549);
      this.Controls.Add(this.groupBoxMusicBrainzMultipleAlbums);
      this.Controls.Add(this.btUpdate);
      this.Controls.Add(this.btClose);
      this.Controls.Add(this.gbSearchInfo);
      this.Name = "MusicBrainzAlbumResults";
      this.ShowInTaskbar = false;
      this.Text = "Internet Search Results";
      this.groupBoxMusicBrainzMultipleAlbums.ResumeLayout(false);
      this.gbSearchInfo.ResumeLayout(false);
      this.gbSearchInfo.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lvSearchResults;
    private System.Windows.Forms.ColumnHeader chAlbum;
    private System.Windows.Forms.ColumnHeader chDuration;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxMusicBrainzMultipleAlbums;
    private MPTagThat.Core.WinControls.MPTLabel lbTitle;
    private MPTagThat.Core.WinControls.MPTButton btUpdate;
    private MPTagThat.Core.WinControls.MPTButton btClose;
    private MPTagThat.Core.WinControls.MPTLabel lbArtist;
    private MPTagThat.Core.WinControls.MPTGroupBox gbSearchInfo;
    private System.Windows.Forms.TextBox tbTitle;
    private System.Windows.Forms.TextBox tbArtist;
    private System.Windows.Forms.ColumnHeader chYear;
    private System.Windows.Forms.ColumnHeader chCountry;
  }
}