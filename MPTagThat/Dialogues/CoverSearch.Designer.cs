namespace MPTagThat.Dialogues
{
  partial class CoverSearch
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
      this.groupBoxAmazonMultipleAlbums = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbFileDetails = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbAlbum = new System.Windows.Forms.TextBox();
      this.btSearch = new MPTagThat.Core.WinControls.MPTButton();
      this.tbArtist = new System.Windows.Forms.TextBox();
      this.lblAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.btUpdate = new MPTagThat.Core.WinControls.MPTButton();
      this.btClose = new MPTagThat.Core.WinControls.MPTButton();
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
      this.lvSearchResults.Location = new System.Drawing.Point(15, 97);
      this.lvSearchResults.MultiSelect = false;
      this.lvSearchResults.Name = "lvSearchResults";
      this.lvSearchResults.Size = new System.Drawing.Size(575, 425);
      this.lvSearchResults.TabIndex = 4;
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
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.tbAlbum);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.btSearch);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.tbArtist);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lblAlbum);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lblArtist);
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lvSearchResults);
      this.groupBoxAmazonMultipleAlbums.Id = "5baf0e4f-45a2-4cf3-804a-d489291e7451";
      this.groupBoxAmazonMultipleAlbums.Localisation = "GroupBoxResults";
      this.groupBoxAmazonMultipleAlbums.LocalisationContext = "AmazonAlbumSearch";
      this.groupBoxAmazonMultipleAlbums.Location = new System.Drawing.Point(12, 12);
      this.groupBoxAmazonMultipleAlbums.Name = "groupBoxAmazonMultipleAlbums";
      this.groupBoxAmazonMultipleAlbums.Size = new System.Drawing.Size(607, 538);
      this.groupBoxAmazonMultipleAlbums.TabIndex = 39;
      this.groupBoxAmazonMultipleAlbums.Text = "Multiple albums found. Please select: ";
      // 
      // lbFileDetails
      // 
      this.lbFileDetails.Localisation = "lbFileDetails";
      this.lbFileDetails.LocalisationContext = "groupBoxAmazonMultipleAlbums";
      this.lbFileDetails.Location = new System.Drawing.Point(19, 20);
      this.lbFileDetails.Name = "lbFileDetails";
      this.lbFileDetails.Size = new System.Drawing.Size(53, 13);
      this.lbFileDetails.TabIndex = 15;
      this.lbFileDetails.Text = "file details";
      // 
      // tbAlbum
      // 
      this.tbAlbum.Location = new System.Drawing.Point(118, 67);
      this.tbAlbum.Name = "tbAlbum";
      this.tbAlbum.Size = new System.Drawing.Size(348, 20);
      this.tbAlbum.TabIndex = 2;
      // 
      // btSearch
      // 
      this.btSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btSearch.Id = "613cc2f5-ca18-4207-a8bb-50779ac5257f";
      this.btSearch.Localisation = "Search";
      this.btSearch.LocalisationContext = "AmazonAlbumSearch";
      this.btSearch.Location = new System.Drawing.Point(490, 42);
      this.btSearch.Name = "btSearch";
      this.btSearch.Size = new System.Drawing.Size(100, 23);
      this.btSearch.TabIndex = 3;
      this.btSearch.Text = "Search";
      this.btSearch.UseVisualStyleBackColor = true;
      this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
      // 
      // tbArtist
      // 
      this.tbArtist.Location = new System.Drawing.Point(118, 41);
      this.tbArtist.Name = "tbArtist";
      this.tbArtist.Size = new System.Drawing.Size(348, 20);
      this.tbArtist.TabIndex = 1;
      // 
      // lblAlbum
      // 
      this.lblAlbum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAlbum.Localisation = "Album";
      this.lblAlbum.LocalisationContext = "TagEdit";
      this.lblAlbum.Location = new System.Drawing.Point(17, 66);
      this.lblAlbum.Name = "lblAlbum";
      this.lblAlbum.Size = new System.Drawing.Size(49, 16);
      this.lblAlbum.TabIndex = 12;
      this.lblAlbum.Text = "Album:";
      // 
      // lblArtist
      // 
      this.lblArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblArtist.Localisation = "Artist";
      this.lblArtist.LocalisationContext = "TagEdit";
      this.lblArtist.Location = new System.Drawing.Point(17, 45);
      this.lblArtist.Name = "lblArtist";
      this.lblArtist.Size = new System.Drawing.Size(40, 16);
      this.lblArtist.TabIndex = 11;
      this.lblArtist.Text = "Artist:";
      // 
      // btUpdate
      // 
      this.btUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btUpdate.Id = "ea25cbfa-08e6-4355-94c1-f61294b83bd6";
      this.btUpdate.Localisation = "Update";
      this.btUpdate.LocalisationContext = "AmazonAlbumSearch";
      this.btUpdate.Location = new System.Drawing.Point(396, 572);
      this.btUpdate.Name = "btUpdate";
      this.btUpdate.Size = new System.Drawing.Size(100, 23);
      this.btUpdate.TabIndex = 5;
      this.btUpdate.Text = "Select";
      this.btUpdate.UseVisualStyleBackColor = true;
      this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
      // 
      // btClose
      // 
      this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btClose.Id = "e2e3d64c-8f56-4ac4-9a34-8811217b9bb8";
      this.btClose.Localisation = "Close";
      this.btClose.LocalisationContext = "AmazonAlbumSearch";
      this.btClose.Location = new System.Drawing.Point(502, 572);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(100, 23);
      this.btClose.TabIndex = 6;
      this.btClose.Text = "Cancel";
      this.btClose.UseVisualStyleBackColor = true;
      this.btClose.Click += new System.EventHandler(this.btClose_Click);
      // 
      // CoverSearch
      // 
      this.AcceptButton = this.btUpdate;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btClose;
      this.ClientSize = new System.Drawing.Size(631, 607);
      this.Controls.Add(this.groupBoxAmazonMultipleAlbums);
      this.Controls.Add(this.btUpdate);
      this.Controls.Add(this.btClose);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "CoverSearch";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Internet Search Results";
      this.Load += new System.EventHandler(this.OnLoad);
      this.groupBoxAmazonMultipleAlbums.ResumeLayout(false);
      this.groupBoxAmazonMultipleAlbums.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView lvSearchResults;
    private System.Windows.Forms.ColumnHeader chAlbum;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxAmazonMultipleAlbums;
    private MPTagThat.Core.WinControls.MPTButton btUpdate;
    private MPTagThat.Core.WinControls.MPTButton btClose;
    protected MPTagThat.Core.WinControls.MPTLabel lblArtist;
    protected MPTagThat.Core.WinControls.MPTLabel lblAlbum;
    private MPTagThat.Core.WinControls.MPTLabel lbFileDetails;
    private System.Windows.Forms.TextBox tbAlbum;
    private System.Windows.Forms.TextBox tbArtist;
    private Core.WinControls.MPTButton btSearch;
  }
}