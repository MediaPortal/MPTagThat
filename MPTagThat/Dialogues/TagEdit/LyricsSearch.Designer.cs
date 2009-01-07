namespace MPTagThat.TagEdit
{
  partial class LyricsSearch
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
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.tbTitle = new System.Windows.Forms.TextBox();
      this.lvSearchResults = new System.Windows.Forms.ListView();
      this.chSite = new System.Windows.Forms.ColumnHeader();
      this.chResult = new System.Windows.Forms.ColumnHeader();
      this.chLyric = new System.Windows.Forms.ColumnHeader();
      this.groupBox1 = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.btUpdate = new MPTagThat.Core.WinControls.MPTButton();
      this.tbArtist = new System.Windows.Forms.TextBox();
      this.btFind = new MPTagThat.Core.WinControls.MPTButton();
      this.btClose = new MPTagThat.Core.WinControls.MPTButton();
      this.tbLyrics = new System.Windows.Forms.TextBox();
      this.groupBox2 = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lbArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.gbSearchInfo = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.btSwitchArtist = new MPTagThat.Core.WinControls.MPTButton();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.gbSearchInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // btCancel
      // 
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "LyricsSearch";
      this.btCancel.Location = new System.Drawing.Point(349, 129);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(100, 23);
      this.btCancel.TabIndex = 42;
      this.btCancel.Text = "&Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      // 
      // tbTitle
      // 
      this.tbTitle.Location = new System.Drawing.Point(73, 53);
      this.tbTitle.Name = "tbTitle";
      this.tbTitle.Size = new System.Drawing.Size(344, 20);
      this.tbTitle.TabIndex = 2;
      // 
      // lvSearchResults
      // 
      this.lvSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSite,
            this.chResult,
            this.chLyric});
      this.lvSearchResults.FullRowSelect = true;
      this.lvSearchResults.Location = new System.Drawing.Point(17, 21);
      this.lvSearchResults.MultiSelect = false;
      this.lvSearchResults.Name = "lvSearchResults";
      this.lvSearchResults.Size = new System.Drawing.Size(528, 152);
      this.lvSearchResults.TabIndex = 10;
      this.lvSearchResults.UseCompatibleStateImageBehavior = false;
      this.lvSearchResults.View = System.Windows.Forms.View.Details;
      this.lvSearchResults.SelectedIndexChanged += new System.EventHandler(this.lvSearchResults_SelectedIndexChanged);
      this.lvSearchResults.DoubleClick += new System.EventHandler(this.lvSearchResults_DoubleClick);
      // 
      // chSite
      // 
      this.chSite.Text = "Site";
      this.chSite.Width = 113;
      // 
      // chResult
      // 
      this.chResult.Text = "Result";
      this.chResult.Width = 58;
      // 
      // chLyric
      // 
      this.chLyric.Text = "Lyric";
      this.chLyric.Width = 352;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.lvSearchResults);
      this.groupBox1.Localisation = "GroupBoxSearchResults";
      this.groupBox1.LocalisationContext = "LyricsSearch";
      this.groupBox1.Location = new System.Drawing.Point(22, 189);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(567, 193);
      this.groupBox1.TabIndex = 39;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Search results";
      // 
      // lbTitle
      // 
      this.lbTitle.AutoSize = true;
      this.lbTitle.Localisation = "Title";
      this.lbTitle.LocalisationContext = "LyricsSearch";
      this.lbTitle.Location = new System.Drawing.Point(15, 56);
      this.lbTitle.Name = "lbTitle";
      this.lbTitle.Size = new System.Drawing.Size(30, 13);
      this.lbTitle.TabIndex = 2;
      this.lbTitle.Text = "Title:";
      // 
      // btUpdate
      // 
      this.btUpdate.Enabled = false;
      this.btUpdate.Localisation = "Update";
      this.btUpdate.LocalisationContext = "LyricsSearch";
      this.btUpdate.Location = new System.Drawing.Point(214, 650);
      this.btUpdate.Name = "btUpdate";
      this.btUpdate.Size = new System.Drawing.Size(100, 23);
      this.btUpdate.TabIndex = 43;
      this.btUpdate.Text = "Update";
      this.btUpdate.UseVisualStyleBackColor = true;
      this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
      // 
      // tbArtist
      // 
      this.tbArtist.Location = new System.Drawing.Point(73, 26);
      this.tbArtist.Name = "tbArtist";
      this.tbArtist.Size = new System.Drawing.Size(344, 20);
      this.tbArtist.TabIndex = 1;
      // 
      // btFind
      // 
      this.btFind.Localisation = "Find";
      this.btFind.LocalisationContext = "LyricsSearch";
      this.btFind.Location = new System.Drawing.Point(94, 129);
      this.btFind.Name = "btFind";
      this.btFind.Size = new System.Drawing.Size(100, 23);
      this.btFind.TabIndex = 41;
      this.btFind.Text = "&Fetch";
      this.btFind.UseVisualStyleBackColor = true;
      this.btFind.Click += new System.EventHandler(this.btFind_Click);
      // 
      // btClose
      // 
      this.btClose.Localisation = "Close";
      this.btClose.LocalisationContext = "LyricsSearch";
      this.btClose.Location = new System.Drawing.Point(349, 650);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(100, 23);
      this.btClose.TabIndex = 44;
      this.btClose.Text = "Close";
      this.btClose.UseVisualStyleBackColor = true;
      this.btClose.Click += new System.EventHandler(this.btClose_Click);
      // 
      // tbLyrics
      // 
      this.tbLyrics.Location = new System.Drawing.Point(17, 19);
      this.tbLyrics.Multiline = true;
      this.tbLyrics.Name = "tbLyrics";
      this.tbLyrics.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tbLyrics.Size = new System.Drawing.Size(528, 193);
      this.tbLyrics.TabIndex = 0;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.tbLyrics);
      this.groupBox2.Localisation = "GroupBoxLyric";
      this.groupBox2.LocalisationContext = "LyricsSearch";
      this.groupBox2.Location = new System.Drawing.Point(22, 401);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(567, 228);
      this.groupBox2.TabIndex = 40;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Lyric";
      // 
      // lbArtist
      // 
      this.lbArtist.AutoSize = true;
      this.lbArtist.Localisation = "Artist";
      this.lbArtist.LocalisationContext = "LyricsSearch";
      this.lbArtist.Location = new System.Drawing.Point(15, 29);
      this.lbArtist.Name = "lbArtist";
      this.lbArtist.Size = new System.Drawing.Size(33, 13);
      this.lbArtist.TabIndex = 0;
      this.lbArtist.Text = "Artist:";
      // 
      // gbSearchInfo
      // 
      this.gbSearchInfo.Controls.Add(this.tbTitle);
      this.gbSearchInfo.Controls.Add(this.lbTitle);
      this.gbSearchInfo.Controls.Add(this.tbArtist);
      this.gbSearchInfo.Controls.Add(this.lbArtist);
      this.gbSearchInfo.Localisation = "GroupBoxSearchInfo";
      this.gbSearchInfo.LocalisationContext = "LyricsSearch";
      this.gbSearchInfo.Location = new System.Drawing.Point(21, 25);
      this.gbSearchInfo.Name = "gbSearchInfo";
      this.gbSearchInfo.Size = new System.Drawing.Size(432, 85);
      this.gbSearchInfo.TabIndex = 38;
      this.gbSearchInfo.TabStop = false;
      this.gbSearchInfo.Text = "Search information";
      // 
      // btSwitchArtist
      // 
      this.btSwitchArtist.Localisation = "SwitchArtist";
      this.btSwitchArtist.LocalisationContext = "LyricsSearch";
      this.btSwitchArtist.Location = new System.Drawing.Point(214, 129);
      this.btSwitchArtist.Name = "btSwitchArtist";
      this.btSwitchArtist.Size = new System.Drawing.Size(100, 23);
      this.btSwitchArtist.TabIndex = 45;
      this.btSwitchArtist.Text = "&Switch Artist";
      this.btSwitchArtist.UseVisualStyleBackColor = true;
      this.btSwitchArtist.Click += new System.EventHandler(this.btSwitchArtist_Click);
      // 
      // LyricsSearch
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(631, 688);
      this.Controls.Add(this.btSwitchArtist);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btUpdate);
      this.Controls.Add(this.btFind);
      this.Controls.Add(this.btClose);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.gbSearchInfo);
      this.Name = "LyricsSearch";
      this.ShowInTaskbar = false;
      this.Text = "LyricsSearch";
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.gbSearchInfo.ResumeLayout(false);
      this.gbSearchInfo.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private System.Windows.Forms.TextBox tbTitle;
    private System.Windows.Forms.ListView lvSearchResults;
    private System.Windows.Forms.ColumnHeader chSite;
    private System.Windows.Forms.ColumnHeader chResult;
    private System.Windows.Forms.ColumnHeader chLyric;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBox1;
    private MPTagThat.Core.WinControls.MPTLabel lbTitle;
    private MPTagThat.Core.WinControls.MPTButton btUpdate;
    private System.Windows.Forms.TextBox tbArtist;
    private MPTagThat.Core.WinControls.MPTButton btFind;
    private MPTagThat.Core.WinControls.MPTButton btClose;
    private System.Windows.Forms.TextBox tbLyrics;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBox2;
    private MPTagThat.Core.WinControls.MPTLabel lbArtist;
    private MPTagThat.Core.WinControls.MPTGroupBox gbSearchInfo;
    private MPTagThat.Core.WinControls.MPTButton btSwitchArtist;
  }
}