namespace MPTagThat.TagEdit
{
  partial class SingleTagEdit
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
      this.cmdPanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.btScriptExecute = new MPTagThat.Core.WinControls.MPTButton();
      this.comboBoxScripts = new MPTagThat.Core.WinControls.MPTComboBox();
      this.btPreviousFile = new MPTagThat.Core.WinControls.MPTButton();
      this.btNextFile = new MPTagThat.Core.WinControls.MPTButton();
      this.btGetTrackLength = new MPTagThat.Core.WinControls.MPTButton();
      this.tbTrackLength = new System.Windows.Forms.TextBox();
      this.btGetLyricsFromInternet = new MPTagThat.Core.WinControls.MPTButton();
      this.tabControlTagEdit.SuspendLayout();
      this.tabPageMain.SuspendLayout();
      this.groupBoxArtist.SuspendLayout();
      this.tabPagePictures.SuspendLayout();
      this.groupBoxGenre.SuspendLayout();
      this.groupBoxPicture.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).BeginInit();
      this.panelNavigation.SuspendLayout();
      this.panelTabPage.SuspendLayout();
      this.tagPanel.SuspendLayout();
      this.groupBoxComment.SuspendLayout();
      this.tabPageDetails.SuspendLayout();
      this.groupBoxPeople.SuspendLayout();
      this.groupBoxContent.SuspendLayout();
      this.groupBoxSort.SuspendLayout();
      this.groupBoxMedia.SuspendLayout();
      this.tabPageOriginal.SuspendLayout();
      this.tabPageInvolvedPeople.SuspendLayout();
      this.tabPageWebInformation.SuspendLayout();
      this.tabPageLyrics.SuspendLayout();
      this.tabPageRating.SuspendLayout();
      this.groupBoxOriginalInformation.SuspendLayout();
      this.groupBoxWebInformation.SuspendLayout();
      this.groupBoxInvolvedPeople.SuspendLayout();
      this.groupBoxMusician.SuspendLayout();
      this.groupBoxLyrics.SuspendLayout();
      this.groupBoxRating.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPlayCounter)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRating)).BeginInit();
      this.cmdPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelNavigation
      // 
      this.panelNavigation.Controls.Add(this.cmdPanel);
      this.panelNavigation.Controls.SetChildIndex(this.cmdPanel, 0);
      this.panelNavigation.Controls.SetChildIndex(this.tagPanel, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkPictures, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkMainTags, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkDetails, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkOriginal, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkInvolvedPeople, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkWebInformation, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkLyrics, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkRating, 0);
      this.tagPanel.Controls.SetChildIndex(this.lbLinkUserDefined, 0);
      // 
      // groupBoxMedia
      // 
      this.groupBoxMedia.Controls.Add(this.btGetTrackLength);
      this.groupBoxMedia.Controls.Add(this.tbTrackLength);
      this.groupBoxMedia.Controls.SetChildIndex(this.tbTrackLength, 0);
      this.groupBoxMedia.Controls.SetChildIndex(this.btGetTrackLength, 0);
      this.groupBoxMedia.Controls.SetChildIndex(this.lblMediaType, 0);
      this.groupBoxMedia.Controls.SetChildIndex(this.cbMediaType, 0);
      this.groupBoxMedia.Controls.SetChildIndex(this.ckMediaType, 0);
      this.groupBoxMedia.Controls.SetChildIndex(this.lblTRackLength, 0);
      // 
      // groupBoxLyrics
      // 
      this.groupBoxLyrics.Controls.Add(this.btGetLyricsFromInternet);
      this.groupBoxLyrics.Controls.SetChildIndex(this.btGetLyricsFromInternet, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.cbLyricsDescriptor, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.cbLyricsLanguage, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.lblLyricsDescriptor, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.lblLyricsLanguage, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.tbLyrics, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.btAddLyrics, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.btRemoveLyrics, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.lblLyricsMoveTop, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.ckRemoveLyrics, 0);
      this.groupBoxLyrics.Controls.SetChildIndex(this.btGetLyricsFromText, 0);
      // 
      // cmdPanel
      // 
      this.cmdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdPanel.AnimationStep = 30;
      this.cmdPanel.BorderColor = System.Drawing.Color.Gray;
      this.cmdPanel.CaptionBrush = Stepi.UI.BrushType.Solid;
      this.cmdPanel.CaptionColorOne = System.Drawing.SystemColors.GradientActiveCaption;
      this.cmdPanel.CaptionColorTwo = System.Drawing.SystemColors.GradientInactiveCaption;
      this.cmdPanel.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmdPanel.CaptionSize = 24;
      this.cmdPanel.CaptionText = "Commands";
      this.cmdPanel.CaptionTextColor = System.Drawing.Color.Black;
      this.cmdPanel.Controls.Add(this.btScriptExecute);
      this.cmdPanel.Controls.Add(this.comboBoxScripts);
      this.cmdPanel.Controls.Add(this.btPreviousFile);
      this.cmdPanel.Controls.Add(this.btNextFile);
      this.cmdPanel.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.cmdPanel.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.cmdPanel.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.cmdPanel.Location = new System.Drawing.Point(3, 331);
      this.cmdPanel.Name = "cmdPanel";
      this.cmdPanel.Size = new System.Drawing.Size(160, 220);
      this.cmdPanel.TabIndex = 9;
      // 
      // btScriptExecute
      // 
      this.btScriptExecute.Id = "6e4dfbb8-5de3-40bd-a0cc-a3f70cc73d07";
      this.btScriptExecute.Localisation = "ttButton1";
      this.btScriptExecute.LocalisationContext = "ExtendedPanel";
      this.btScriptExecute.Location = new System.Drawing.Point(130, 40);
      this.btScriptExecute.Name = "btScriptExecute";
      this.btScriptExecute.Size = new System.Drawing.Size(23, 23);
      this.btScriptExecute.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::MPTagThat.Properties.Resources.FormRunHS)});
      this.btScriptExecute.TabIndex = 4;
      this.btScriptExecute.UseVisualStyleBackColor = true;
      this.btScriptExecute.Click += new System.EventHandler(this.btScriptExecute_Click);
      // 
      // comboBoxScripts
      // 
      this.comboBoxScripts.FormattingEnabled = true;
      this.comboBoxScripts.Location = new System.Drawing.Point(6, 40);
      this.comboBoxScripts.Name = "comboBoxScripts";
      this.comboBoxScripts.Size = new System.Drawing.Size(121, 21);
      this.comboBoxScripts.TabIndex = 3;
      // 
      // btPreviousFile
      // 
      this.btPreviousFile.Id = "c8ebfe43-e283-49f0-918c-614bc9757037";
      this.btPreviousFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btPreviousFile.Localisation = "Previous";
      this.btPreviousFile.LocalisationContext = "TagEdit";
      this.btPreviousFile.Location = new System.Drawing.Point(6, 158);
      this.btPreviousFile.Name = "btPreviousFile";
      this.btPreviousFile.Size = new System.Drawing.Size(118, 23);
      this.btPreviousFile.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::MPTagThat.Properties.Resources.NavBack)});
      this.btPreviousFile.TabIndex = 2;
      this.btPreviousFile.Text = "Previous File";
      this.btPreviousFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btPreviousFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btPreviousFile.UseVisualStyleBackColor = true;
      this.btPreviousFile.Click += new System.EventHandler(this.btPreviousFile_Click);
      // 
      // btNextFile
      // 
      this.btNextFile.Id = "4cd9939b-e767-4b1f-b779-2267bc710d53";
      this.btNextFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btNextFile.Localisation = "Next";
      this.btNextFile.LocalisationContext = "TagEdit";
      this.btNextFile.Location = new System.Drawing.Point(6, 187);
      this.btNextFile.Name = "btNextFile";
      this.btNextFile.Size = new System.Drawing.Size(118, 23);
      this.btNextFile.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::MPTagThat.Properties.Resources.NavForward)});
      this.btNextFile.TabIndex = 1;
      this.btNextFile.Text = "Next File";
      this.btNextFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btNextFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btNextFile.UseVisualStyleBackColor = true;
      this.btNextFile.Click += new System.EventHandler(this.btNextFile_Click);
      // 
      // btGetTrackLength
      // 
      this.btGetTrackLength.Id = "129645cd-3836-4481-93a4-bafa3d2bce6b";
      this.btGetTrackLength.Localisation = "ttButton1";
      this.btGetTrackLength.LocalisationContext = "groupBoxMedia";
      this.btGetTrackLength.Location = new System.Drawing.Point(322, 50);
      this.btGetTrackLength.Name = "btGetTrackLength";
      this.btGetTrackLength.Size = new System.Drawing.Size(186, 23);
      this.btGetTrackLength.TabIndex = 48;
      this.btGetTrackLength.Text = "Get from File";
      this.btGetTrackLength.UseVisualStyleBackColor = true;
      this.btGetTrackLength.Click += new System.EventHandler(this.btGetTrackLength_Click);
      // 
      // tbTrackLength
      // 
      this.tbTrackLength.Location = new System.Drawing.Point(205, 50);
      this.tbTrackLength.Name = "tbTrackLength";
      this.tbTrackLength.Size = new System.Drawing.Size(100, 22);
      this.tbTrackLength.TabIndex = 47;
      // 
      // btGetLyricsFromInternet
      // 
      this.btGetLyricsFromInternet.Id = "ccf644b5-4604-4aa4-b8e6-2fed12d6e095";
      this.btGetLyricsFromInternet.Localisation = "GetLyricsFromInternet";
      this.btGetLyricsFromInternet.LocalisationContext = "TagEdit";
      this.btGetLyricsFromInternet.Location = new System.Drawing.Point(359, 246);
      this.btGetLyricsFromInternet.Name = "btGetLyricsFromInternet";
      this.btGetLyricsFromInternet.Size = new System.Drawing.Size(307, 23);
      this.btGetLyricsFromInternet.TabIndex = 12;
      this.btGetLyricsFromInternet.Text = "Get Lyrics from the Internet";
      this.btGetLyricsFromInternet.UseVisualStyleBackColor = true;
      this.btGetLyricsFromInternet.Click += new System.EventHandler(this.btGetLyricsFromInternet_Click);
      // 
      // SingleTagEdit
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.ClientSize = new System.Drawing.Size(904, 661);
      this.Name = "SingleTagEdit";
      this.Controls.SetChildIndex(this.btApply, 0);
      this.Controls.SetChildIndex(this.btCancel, 0);
      this.Controls.SetChildIndex(this.panelNavigation, 0);
      this.Controls.SetChildIndex(this.panelTabPage, 0);
      this.tabControlTagEdit.ResumeLayout(false);
      this.tabPageMain.ResumeLayout(false);
      this.groupBoxArtist.ResumeLayout(false);
      this.groupBoxArtist.PerformLayout();
      this.tabPagePictures.ResumeLayout(false);
      this.groupBoxGenre.ResumeLayout(false);
      this.groupBoxGenre.PerformLayout();
      this.groupBoxPicture.ResumeLayout(false);
      this.groupBoxPicture.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).EndInit();
      this.panelNavigation.ResumeLayout(false);
      this.panelTabPage.ResumeLayout(false);
      this.tagPanel.ResumeLayout(false);
      this.tagPanel.PerformLayout();
      this.groupBoxComment.ResumeLayout(false);
      this.groupBoxComment.PerformLayout();
      this.tabPageDetails.ResumeLayout(false);
      this.groupBoxPeople.ResumeLayout(false);
      this.groupBoxPeople.PerformLayout();
      this.groupBoxContent.ResumeLayout(false);
      this.groupBoxContent.PerformLayout();
      this.groupBoxSort.ResumeLayout(false);
      this.groupBoxSort.PerformLayout();
      this.groupBoxMedia.ResumeLayout(false);
      this.groupBoxMedia.PerformLayout();
      this.tabPageOriginal.ResumeLayout(false);
      this.tabPageInvolvedPeople.ResumeLayout(false);
      this.tabPageWebInformation.ResumeLayout(false);
      this.tabPageLyrics.ResumeLayout(false);
      this.tabPageRating.ResumeLayout(false);
      this.groupBoxOriginalInformation.ResumeLayout(false);
      this.groupBoxOriginalInformation.PerformLayout();
      this.groupBoxWebInformation.ResumeLayout(false);
      this.groupBoxWebInformation.PerformLayout();
      this.groupBoxInvolvedPeople.ResumeLayout(false);
      this.groupBoxInvolvedPeople.PerformLayout();
      this.groupBoxMusician.ResumeLayout(false);
      this.groupBoxMusician.PerformLayout();
      this.groupBoxLyrics.ResumeLayout(false);
      this.groupBoxLyrics.PerformLayout();
      this.groupBoxRating.ResumeLayout(false);
      this.groupBoxRating.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPlayCounter)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRating)).EndInit();
      this.cmdPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    private MPTagThat.Core.WinControls.MPTButton btGetLyricsFromInternet;
    private MPTagThat.Core.WinControls.TTExtendedPanel cmdPanel;
    private MPTagThat.Core.WinControls.MPTButton btScriptExecute;
    private MPTagThat.Core.WinControls.MPTComboBox comboBoxScripts;
    private MPTagThat.Core.WinControls.MPTButton btPreviousFile;
    private MPTagThat.Core.WinControls.MPTButton btNextFile;
    private MPTagThat.Core.WinControls.MPTButton btGetTrackLength;
    private System.Windows.Forms.TextBox tbTrackLength;
  }
}