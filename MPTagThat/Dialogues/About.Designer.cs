namespace MPTagThat.Dialogues
{
  partial class About
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
      this.lbAppTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.btOk = new MPTagThat.Core.WinControls.MPTButton();
      this.lbVersion = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbBuildDate = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbWikiLink = new System.Windows.Forms.LinkLabel();
      this.lbVersionDetail = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbDate = new MPTagThat.Core.WinControls.MPTLabel();
      this.pictureBoxAbout = new System.Windows.Forms.PictureBox();
      this.lbLinkForum = new System.Windows.Forms.LinkLabel();
      this.tbDescription = new System.Windows.Forms.TextBox();
      this.lbContributors = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbContributors = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAbout)).BeginInit();
      this.SuspendLayout();
      // 
      // lbAppTitle
      // 
      this.lbAppTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbAppTitle.Localisation = "ApplicationName";
      this.lbAppTitle.LocalisationContext = "system";
      this.lbAppTitle.Location = new System.Drawing.Point(63, 30);
      this.lbAppTitle.Name = "lbAppTitle";
      this.lbAppTitle.Size = new System.Drawing.Size(243, 16);
      this.lbAppTitle.TabIndex = 0;
      this.lbAppTitle.Text = "MPTagThat the MediaPortal Tag Editor";
      // 
      // btOk
      // 
      this.btOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btOk.Id = "55f9833e-8053-49e4-bc73-490f80af59ec";
      this.btOk.Localisation = "Ok";
      this.btOk.LocalisationContext = "About";
      this.btOk.Location = new System.Drawing.Point(212, 344);
      this.btOk.Name = "btOk";
      this.btOk.Size = new System.Drawing.Size(75, 23);
      this.btOk.TabIndex = 2;
      this.btOk.Text = "Ok";
      this.btOk.UseVisualStyleBackColor = true;
      // 
      // lbVersion
      // 
      this.lbVersion.Localisation = "Version";
      this.lbVersion.LocalisationContext = "About";
      this.lbVersion.Location = new System.Drawing.Point(15, 65);
      this.lbVersion.Name = "lbVersion";
      this.lbVersion.Size = new System.Drawing.Size(45, 13);
      this.lbVersion.TabIndex = 3;
      this.lbVersion.Text = "Version:";
      // 
      // lbBuildDate
      // 
      this.lbBuildDate.Localisation = "BuildDate";
      this.lbBuildDate.LocalisationContext = "About";
      this.lbBuildDate.Location = new System.Drawing.Point(15, 91);
      this.lbBuildDate.Name = "lbBuildDate";
      this.lbBuildDate.Size = new System.Drawing.Size(59, 13);
      this.lbBuildDate.TabIndex = 4;
      this.lbBuildDate.Text = "Build Date:";
      // 
      // lbWikiLink
      // 
      this.lbWikiLink.AutoSize = true;
      this.lbWikiLink.Location = new System.Drawing.Point(12, 279);
      this.lbWikiLink.Name = "lbWikiLink";
      this.lbWikiLink.Size = new System.Drawing.Size(356, 13);
      this.lbWikiLink.TabIndex = 5;
      this.lbWikiLink.TabStop = true;
      this.lbWikiLink.Text = "http://www.team-mediaportal.com/manual/MediaPortalTools/MPTagThat";
      this.lbWikiLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbWikiLink_LinkClicked);
      // 
      // lbVersionDetail
      // 
      this.lbVersionDetail.Localisation = "mptLabel1";
      this.lbVersionDetail.LocalisationContext = "About";
      this.lbVersionDetail.Location = new System.Drawing.Point(142, 64);
      this.lbVersionDetail.Name = "lbVersionDetail";
      this.lbVersionDetail.Size = new System.Drawing.Size(38, 13);
      this.lbVersionDetail.TabIndex = 6;
      this.lbVersionDetail.Text = "1.0.x.x";
      // 
      // lbDate
      // 
      this.lbDate.Localisation = "mptLabel1";
      this.lbDate.LocalisationContext = "About";
      this.lbDate.Location = new System.Drawing.Point(142, 91);
      this.lbDate.Name = "lbDate";
      this.lbDate.Size = new System.Drawing.Size(61, 13);
      this.lbDate.TabIndex = 7;
      this.lbDate.Text = "2009-01-08";
      // 
      // pictureBoxAbout
      // 
      this.pictureBoxAbout.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxAbout.Image")));
      this.pictureBoxAbout.Location = new System.Drawing.Point(341, 30);
      this.pictureBoxAbout.Name = "pictureBoxAbout";
      this.pictureBoxAbout.Size = new System.Drawing.Size(163, 162);
      this.pictureBoxAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBoxAbout.TabIndex = 8;
      this.pictureBoxAbout.TabStop = false;
      // 
      // lbLinkForum
      // 
      this.lbLinkForum.AutoSize = true;
      this.lbLinkForum.Location = new System.Drawing.Point(12, 308);
      this.lbLinkForum.Name = "lbLinkForum";
      this.lbLinkForum.Size = new System.Drawing.Size(243, 13);
      this.lbLinkForum.TabIndex = 9;
      this.lbLinkForum.TabStop = true;
      this.lbLinkForum.Text = "http://forum.team-mediaportal.com/mptagthat-310";
      this.lbLinkForum.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbLinkForum_LinkClicked);
      // 
      // tbDescription
      // 
      this.tbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tbDescription.Location = new System.Drawing.Point(15, 221);
      this.tbDescription.Multiline = true;
      this.tbDescription.Name = "tbDescription";
      this.tbDescription.ReadOnly = true;
      this.tbDescription.Size = new System.Drawing.Size(480, 55);
      this.tbDescription.TabIndex = 10;
      this.tbDescription.Text = "MPTagThat is an open source tag editor which allows to manage your complete music" +
          " collection.\r\n\r\nMore information at:";
      // 
      // lbContributors
      // 
      this.lbContributors.Localisation = "Contributors";
      this.lbContributors.LocalisationContext = "About";
      this.lbContributors.Location = new System.Drawing.Point(15, 128);
      this.lbContributors.Name = "lbContributors";
      this.lbContributors.Size = new System.Drawing.Size(59, 13);
      this.lbContributors.TabIndex = 11;
      this.lbContributors.Text = "Contributors";
      // 
      // tbContributors
      // 
      this.tbContributors.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tbContributors.Location = new System.Drawing.Point(15, 145);
      this.tbContributors.Multiline = true;
      this.tbContributors.Name = "tbContributors";
      this.tbContributors.ReadOnly = true;
      this.tbContributors.Size = new System.Drawing.Size(323, 55);
      this.tbContributors.TabIndex = 12;
      this.tbContributors.Text = "Development: Helmut Wahrmann (Main Development), mackey (Lyrics), rtv (Burner Sup" +
          "port)\r\n\r\nTesting: Roy Nilsen (Main Tester, Translation)";
      // 
      // About
      // 
      this.AcceptButton = this.btOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderColor = System.Drawing.Color.Silver;
      this.CancelButton = this.btOk;
      this.ClientSize = new System.Drawing.Size(528, 394);
      this.ControlBox = false;
      this.Controls.Add(this.tbContributors);
      this.Controls.Add(this.lbContributors);
      this.Controls.Add(this.pictureBoxAbout);
      this.Controls.Add(this.lbLinkForum);
      this.Controls.Add(this.lbDate);
      this.Controls.Add(this.tbDescription);
      this.Controls.Add(this.lbVersionDetail);
      this.Controls.Add(this.lbBuildDate);
      this.Controls.Add(this.lbVersion);
      this.Controls.Add(this.lbWikiLink);
      this.Controls.Add(this.lbAppTitle);
      this.Controls.Add(this.btOk);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "About";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "About";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAbout)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTLabel lbAppTitle;
    private MPTagThat.Core.WinControls.MPTButton btOk;
    private MPTagThat.Core.WinControls.MPTLabel lbVersion;
    private MPTagThat.Core.WinControls.MPTLabel lbBuildDate;
    private System.Windows.Forms.LinkLabel lbWikiLink;
    private MPTagThat.Core.WinControls.MPTLabel lbVersionDetail;
    private MPTagThat.Core.WinControls.MPTLabel lbDate;
    private System.Windows.Forms.PictureBox pictureBoxAbout;
    private System.Windows.Forms.LinkLabel lbLinkForum;
    private System.Windows.Forms.TextBox tbDescription;
    private MPTagThat.Core.WinControls.MPTLabel lbContributors;
    private System.Windows.Forms.TextBox tbContributors;
  }
}