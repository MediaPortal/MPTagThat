namespace MPTagThat.Dialogues
{
  partial class AmazonAlbumSearchResults
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
      this.chAlbum = new System.Windows.Forms.ColumnHeader();
      this.groupBoxAmazonMultipleAlbums = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.btUpdate = new MPTagThat.Core.WinControls.MPTButton();
      this.btClose = new MPTagThat.Core.WinControls.MPTButton();
      this.roundRectShape1 = new Telerik.WinControls.RoundRectShape();
      this.lblArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbArtistDetail = new System.Windows.Forms.Label();
      this.lbAlbumDetail = new System.Windows.Forms.Label();
      this.lbFileDetails = new System.Windows.Forms.Label();
      this.groupBoxAmazonMultipleAlbums.SuspendLayout();
      this.SuspendLayout();
      // 
      // lvSearchResults
      // 
      this.lvSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lvSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAlbum});
      this.lvSearchResults.FullRowSelect = true;
      this.lvSearchResults.Location = new System.Drawing.Point(15, 85);
      this.lvSearchResults.MultiSelect = false;
      this.lvSearchResults.Name = "lvSearchResults";
      this.lvSearchResults.Size = new System.Drawing.Size(575, 437);
      this.lvSearchResults.TabIndex = 10;
      this.lvSearchResults.UseCompatibleStateImageBehavior = false;
      this.lvSearchResults.View = System.Windows.Forms.View.Details;
      this.lvSearchResults.DoubleClick += new System.EventHandler(this.lvSearchResults_DoubleClick);
      // 
      // chAlbum
      // 
      this.chAlbum.Text = "Album";
      this.chAlbum.Width = 520;
      // 
      // groupBoxAmazonMultipleAlbums
      // 
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lbFileDetails);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lbAlbumDetail);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lbArtistDetail);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lblAlbum);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lblArtist);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lvSearchResults);
      this.groupBoxAmazonMultipleAlbums.Localisation = "GroupBoxResults";
      this.groupBoxAmazonMultipleAlbums.LocalisationContext = "AmazonAlbumSearch";
      this.groupBoxAmazonMultipleAlbums.Location = new System.Drawing.Point(12, 12);
      this.groupBoxAmazonMultipleAlbums.Name = "groupBoxAmazonMultipleAlbums";
      this.groupBoxAmazonMultipleAlbums.Size = new System.Drawing.Size(607, 538);
      this.groupBoxAmazonMultipleAlbums.TabIndex = 39;
      this.groupBoxAmazonMultipleAlbums.TabStop = false;
      this.groupBoxAmazonMultipleAlbums.Text = "Multiple albums found. Please select: ";
      // 
      // btUpdate
      // 
      this.btUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btUpdate.Localisation = "Update";
      this.btUpdate.LocalisationContext = "AmazonAlbumSearch";
      this.btUpdate.Location = new System.Drawing.Point(396, 572);
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
      this.btClose.Localisation = "Close";
      this.btClose.LocalisationContext = "AmazonAlbumSearch";
      this.btClose.Location = new System.Drawing.Point(502, 572);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(100, 23);
      this.btClose.TabIndex = 44;
      this.btClose.Text = "Cancel";
      this.btClose.UseVisualStyleBackColor = true;
      this.btClose.Click += new System.EventHandler(this.btClose_Click);
      // 
      // lblArtist
      // 
      this.lblArtist.AutoSize = true;
      this.lblArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblArtist.Localisation = "Artist";
      this.lblArtist.LocalisationContext = "TagEdit";
      this.lblArtist.Location = new System.Drawing.Point(12, 39);
      this.lblArtist.Name = "lblArtist";
      this.lblArtist.Size = new System.Drawing.Size(40, 16);
      this.lblArtist.TabIndex = 11;
      this.lblArtist.Text = "Artist:";
      // 
      // lblAlbum
      // 
      this.lblAlbum.AutoSize = true;
      this.lblAlbum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAlbum.Localisation = "Album";
      this.lblAlbum.LocalisationContext = "TagEdit";
      this.lblAlbum.Location = new System.Drawing.Point(12, 60);
      this.lblAlbum.Name = "lblAlbum";
      this.lblAlbum.Size = new System.Drawing.Size(49, 16);
      this.lblAlbum.TabIndex = 12;
      this.lblAlbum.Text = "Album:";
      // 
      // lbArtistDetail
      // 
      this.lbArtistDetail.AutoSize = true;
      this.lbArtistDetail.Location = new System.Drawing.Point(118, 39);
      this.lbArtistDetail.Name = "lbArtistDetail";
      this.lbArtistDetail.Size = new System.Drawing.Size(29, 13);
      this.lbArtistDetail.TabIndex = 13;
      this.lbArtistDetail.Text = "artist";
      // 
      // lbAlbumDetail
      // 
      this.lbAlbumDetail.AutoSize = true;
      this.lbAlbumDetail.Location = new System.Drawing.Point(118, 62);
      this.lbAlbumDetail.Name = "lbAlbumDetail";
      this.lbAlbumDetail.Size = new System.Drawing.Size(35, 13);
      this.lbAlbumDetail.TabIndex = 14;
      this.lbAlbumDetail.Text = "album";
      // 
      // lbFileDetails
      // 
      this.lbFileDetails.AutoSize = true;
      this.lbFileDetails.Location = new System.Drawing.Point(15, 20);
      this.lbFileDetails.Name = "lbFileDetails";
      this.lbFileDetails.Size = new System.Drawing.Size(53, 13);
      this.lbFileDetails.TabIndex = 15;
      this.lbFileDetails.Text = "file details";
      // 
      // AmazonAlbumSearchResults
      // 
      this.AcceptButton = this.btUpdate;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btClose;
      this.ClientSize = new System.Drawing.Size(631, 607);
      this.Controls.Add(this.groupBoxAmazonMultipleAlbums);
      this.Controls.Add(this.btUpdate);
      this.Controls.Add(this.btClose);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "AmazonAlbumSearchResults";
      this.Shape = this.roundRectShape1;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Internet Search Results";
      this.groupBoxAmazonMultipleAlbums.ResumeLayout(false);
      this.groupBoxAmazonMultipleAlbums.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lvSearchResults;
    private System.Windows.Forms.ColumnHeader chAlbum;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxAmazonMultipleAlbums;
    private MPTagThat.Core.WinControls.MPTButton btUpdate;
    private MPTagThat.Core.WinControls.MPTButton btClose;
    private Telerik.WinControls.RoundRectShape roundRectShape1;
    protected MPTagThat.Core.WinControls.MPTLabel lblArtist;
    protected MPTagThat.Core.WinControls.MPTLabel lblAlbum;
    private System.Windows.Forms.Label lbArtistDetail;
    private System.Windows.Forms.Label lbAlbumDetail;
    private System.Windows.Forms.Label lbFileDetails;
  }
}