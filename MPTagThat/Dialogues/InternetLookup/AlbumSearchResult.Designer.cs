namespace MPTagThat.InternetLookup
{
  partial class AlbumSearchResult
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
      this.lvAlbumSearchResult = new System.Windows.Forms.ListView();
      this.chArtist = new System.Windows.Forms.ColumnHeader();
      this.chTitle = new System.Windows.Forms.ColumnHeader();
      this.chTracks = new System.Windows.Forms.ColumnHeader();
      this.chYear = new System.Windows.Forms.ColumnHeader();
      this.chLabel = new System.Windows.Forms.ColumnHeader();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.btContinue = new MPTagThat.Core.WinControls.MPTButton();
      this.SuspendLayout();
      // 
      // lvAlbumSearchResult
      // 
      this.lvAlbumSearchResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chArtist,
            this.chTitle,
            this.chTracks,
            this.chYear,
            this.chLabel});
      this.lvAlbumSearchResult.FullRowSelect = true;
      this.lvAlbumSearchResult.Location = new System.Drawing.Point(30, 27);
      this.lvAlbumSearchResult.MultiSelect = false;
      this.lvAlbumSearchResult.Name = "lvAlbumSearchResult";
      this.lvAlbumSearchResult.Size = new System.Drawing.Size(723, 432);
      this.lvAlbumSearchResult.TabIndex = 0;
      this.lvAlbumSearchResult.UseCompatibleStateImageBehavior = false;
      this.lvAlbumSearchResult.View = System.Windows.Forms.View.Details;
      this.lvAlbumSearchResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvAlbumSearchResult_MouseDoubleClick);
      // 
      // chArtist
      // 
      this.chArtist.Text = "Artist";
      this.chArtist.Width = 186;
      // 
      // chTitle
      // 
      this.chTitle.Text = "Title";
      this.chTitle.Width = 281;
      // 
      // chTracks
      // 
      this.chTracks.Text = "# Tracks";
      this.chTracks.Width = 55;
      // 
      // chYear
      // 
      this.chYear.Text = "Year";
      this.chYear.Width = 55;
      // 
      // chLabel
      // 
      this.chLabel.Text = "Label";
      this.chLabel.Width = 141;
      // 
      // btCancel
      // 
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "Lookup";
      this.btCancel.Location = new System.Drawing.Point(419, 486);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(125, 43);
      this.btCancel.TabIndex = 7;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      // 
      // btContinue
      // 
      this.btContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btContinue.Localisation = "Continue";
      this.btContinue.LocalisationContext = "Lookup";
      this.btContinue.Location = new System.Drawing.Point(216, 486);
      this.btContinue.Name = "btContinue";
      this.btContinue.Size = new System.Drawing.Size(125, 43);
      this.btContinue.TabIndex = 6;
      this.btContinue.Text = "Continue >";
      this.btContinue.UseVisualStyleBackColor = true;
      // 
      // AlbumSearchResult
      // 
      this.AcceptButton = this.btContinue;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btCancel;
      this.ClientSize = new System.Drawing.Size(784, 564);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btContinue);
      this.Controls.Add(this.lvAlbumSearchResult);
      this.Name = "AlbumSearchResult";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Mulitple Albums found. Please select ...";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lvAlbumSearchResult;
    private System.Windows.Forms.ColumnHeader chArtist;
    private System.Windows.Forms.ColumnHeader chTitle;
    private System.Windows.Forms.ColumnHeader chYear;
    private System.Windows.Forms.ColumnHeader chLabel;
    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private MPTagThat.Core.WinControls.MPTButton btContinue;
    private System.Windows.Forms.ColumnHeader chTracks;
  }
}