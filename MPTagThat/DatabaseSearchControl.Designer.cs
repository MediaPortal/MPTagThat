namespace MPTagThat
{
  partial class DatabaseSearchControl
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
      this.lbArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbArtist = new System.Windows.Forms.TextBox();
      this.tbAlbum = new System.Windows.Forms.TextBox();
      this.tbTitle = new System.Windows.Forms.TextBox();
      this.buttonSearch = new MPTagThat.Core.WinControls.MPTButton();
      this.SuspendLayout();
      // 
      // lbArtist
      // 
      this.lbArtist.AutoSize = true;
      this.lbArtist.Localisation = "Artist";
      this.lbArtist.LocalisationContext = "dbsearch";
      this.lbArtist.Location = new System.Drawing.Point(12, 8);
      this.lbArtist.Name = "lbArtist";
      this.lbArtist.Size = new System.Drawing.Size(33, 13);
      this.lbArtist.TabIndex = 0;
      this.lbArtist.Text = "Artist:";
      // 
      // lbAlbum
      // 
      this.lbAlbum.AutoSize = true;
      this.lbAlbum.Localisation = "Album";
      this.lbAlbum.LocalisationContext = "dbsearch";
      this.lbAlbum.Location = new System.Drawing.Point(12, 30);
      this.lbAlbum.Name = "lbAlbum";
      this.lbAlbum.Size = new System.Drawing.Size(39, 13);
      this.lbAlbum.TabIndex = 1;
      this.lbAlbum.Text = "Album:";
      // 
      // lbTitle
      // 
      this.lbTitle.AutoSize = true;
      this.lbTitle.Localisation = "Title";
      this.lbTitle.LocalisationContext = "dbsearch";
      this.lbTitle.Location = new System.Drawing.Point(12, 52);
      this.lbTitle.Name = "lbTitle";
      this.lbTitle.Size = new System.Drawing.Size(30, 13);
      this.lbTitle.TabIndex = 2;
      this.lbTitle.Text = "Title:";
      // 
      // tbArtist
      // 
      this.tbArtist.Location = new System.Drawing.Point(142, 4);
      this.tbArtist.Name = "tbArtist";
      this.tbArtist.Size = new System.Drawing.Size(268, 20);
      this.tbArtist.TabIndex = 0;
      // 
      // tbAlbum
      // 
      this.tbAlbum.Location = new System.Drawing.Point(142, 27);
      this.tbAlbum.Name = "tbAlbum";
      this.tbAlbum.Size = new System.Drawing.Size(268, 20);
      this.tbAlbum.TabIndex = 1;
      // 
      // tbTitle
      // 
      this.tbTitle.Location = new System.Drawing.Point(142, 49);
      this.tbTitle.Name = "tbTitle";
      this.tbTitle.Size = new System.Drawing.Size(268, 20);
      this.tbTitle.TabIndex = 2;
      // 
      // buttonSearch
      // 
      this.buttonSearch.Localisation = "Search";
      this.buttonSearch.LocalisationContext = "dbsearch";
      this.buttonSearch.Location = new System.Drawing.Point(453, 30);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new System.Drawing.Size(130, 23);
      this.buttonSearch.TabIndex = 3;
      this.buttonSearch.Text = "Search";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
      // 
      // DatabaseSearchControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSearch);
      this.Controls.Add(this.tbTitle);
      this.Controls.Add(this.tbAlbum);
      this.Controls.Add(this.tbArtist);
      this.Controls.Add(this.lbTitle);
      this.Controls.Add(this.lbAlbum);
      this.Controls.Add(this.lbArtist);
      this.Name = "DatabaseSearchControl";
      this.Size = new System.Drawing.Size(600, 80);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTLabel lbArtist;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbum;
    private MPTagThat.Core.WinControls.MPTLabel lbTitle;
    private System.Windows.Forms.TextBox tbArtist;
    private System.Windows.Forms.TextBox tbAlbum;
    private System.Windows.Forms.TextBox tbTitle;
    private MPTagThat.Core.WinControls.MPTButton buttonSearch;
  }
}
