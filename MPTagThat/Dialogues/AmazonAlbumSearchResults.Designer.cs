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
      this.groupBoxAmazonMultipleAlbums.SuspendLayout();
      this.SuspendLayout();
      // 
      // lvSearchResults
      // 
      this.lvSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAlbum});
      this.lvSearchResults.FullRowSelect = true;
      this.lvSearchResults.Location = new System.Drawing.Point(17, 21);
      this.lvSearchResults.MultiSelect = false;
      this.lvSearchResults.Name = "lvSearchResults";
      this.lvSearchResults.Size = new System.Drawing.Size(524, 458);
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
      this.groupBoxAmazonMultipleAlbums.Controls.Add(this.lvSearchResults);
      this.groupBoxAmazonMultipleAlbums.Localisation = "GroupBoxResults";
      this.groupBoxAmazonMultipleAlbums.LocalisationContext = "AmazonAlbumSearch";
      this.groupBoxAmazonMultipleAlbums.Location = new System.Drawing.Point(22, 26);
      this.groupBoxAmazonMultipleAlbums.Name = "groupBoxAmazonMultipleAlbums";
      this.groupBoxAmazonMultipleAlbums.Size = new System.Drawing.Size(567, 493);
      this.groupBoxAmazonMultipleAlbums.TabIndex = 39;
      this.groupBoxAmazonMultipleAlbums.TabStop = false;
      this.groupBoxAmazonMultipleAlbums.Text = "Multiple Albums found. Please select ";
      // 
      // btUpdate
      // 
      this.btUpdate.Localisation = "Update";
      this.btUpdate.LocalisationContext = "AmazonAlbumSearch";
      this.btUpdate.Location = new System.Drawing.Point(172, 544);
      this.btUpdate.Name = "btUpdate";
      this.btUpdate.Size = new System.Drawing.Size(100, 23);
      this.btUpdate.TabIndex = 43;
      this.btUpdate.Text = "Select";
      this.btUpdate.UseVisualStyleBackColor = true;
      this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
      // 
      // btClose
      // 
      this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btClose.Localisation = "Close";
      this.btClose.LocalisationContext = "AmazonAlbumSearch";
      this.btClose.Location = new System.Drawing.Point(323, 544);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(100, 23);
      this.btClose.TabIndex = 44;
      this.btClose.Text = "Cancel";
      this.btClose.UseVisualStyleBackColor = true;
      this.btClose.Click += new System.EventHandler(this.btClose_Click);
      // 
      // AmazonAlbumSearchResults
      // 
      this.AcceptButton = this.btUpdate;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btClose;
      this.ClientSize = new System.Drawing.Size(631, 591);
      this.Controls.Add(this.groupBoxAmazonMultipleAlbums);
      this.Controls.Add(this.btUpdate);
      this.Controls.Add(this.btClose);
      this.Name = "AmazonAlbumSearchResults";
      this.Shape = this.roundRectShape1;
      this.ShowInTaskbar = false;
      this.Text = "Internet Search Results";
      this.groupBoxAmazonMultipleAlbums.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lvSearchResults;
    private System.Windows.Forms.ColumnHeader chAlbum;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxAmazonMultipleAlbums;
    private MPTagThat.Core.WinControls.MPTButton btUpdate;
    private MPTagThat.Core.WinControls.MPTButton btClose;
    private Telerik.WinControls.RoundRectShape roundRectShape1;
  }
}