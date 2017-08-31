namespace MPTagThat.Dialogues
{
  partial class FindReplace
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
      this.tabControlFindReplace = new Elegant.Ui.TabControl();
      this.tabPageFind = new MPTagThat.Core.WinControls.MPTTabPage();
      this.tabPageReplace = new MPTagThat.Core.WinControls.MPTTabPage();
      this.buttonReplaceAll = new MPTagThat.Core.WinControls.MPTButton();
      this.buttonReplace = new MPTagThat.Core.WinControls.MPTButton();
      this.cbReplace = new System.Windows.Forms.ComboBox();
      this.lblReplaceWith = new MPTagThat.Core.WinControls.MPTLabel();
      this.buttonClose = new MPTagThat.Core.WinControls.MPTButton();
      this.lblFindWhat = new MPTagThat.Core.WinControls.MPTLabel();
      this.cbFind = new System.Windows.Forms.ComboBox();
      this.groupBoxMatching = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.checkBoxMatchWholeWords = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxMatchCase = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.labelHeader = new MPTagThat.Core.WinControls.MPTLabel();
      this.buttonFindNext = new MPTagThat.Core.WinControls.MPTButton();
      ((System.ComponentModel.ISupportInitialize)(this.tabControlFindReplace)).BeginInit();
      this.tabPageReplace.SuspendLayout();
      this.groupBoxMatching.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControlFindReplace
      // 
      this.tabControlFindReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControlFindReplace.Location = new System.Drawing.Point(11, 32);
      this.tabControlFindReplace.Name = "tabControlFindReplace";
      this.tabControlFindReplace.SelectedTabPage = this.tabPageFind;
      this.tabControlFindReplace.Size = new System.Drawing.Size(466, 273);
      this.tabControlFindReplace.TabIndex = 0;
      this.tabControlFindReplace.TabPages.AddRange(new Elegant.Ui.TabPage[] {
            this.tabPageFind,
            this.tabPageReplace});
      // 
      // tabPageFind
      // 
      this.tabPageFind.ActiveControl = null;
      this.tabPageFind.BackColor = System.Drawing.SystemColors.Control;
      this.tabPageFind.KeyTip = null;
      this.tabPageFind.Localisation = "TabFind";
      this.tabPageFind.LocalisationContext = "FindReplace";
      this.tabPageFind.Name = "tabPageFind";
      this.tabPageFind.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageFind.Size = new System.Drawing.Size(464, 252);
      this.tabPageFind.TabIndex = 0;
      this.tabPageFind.Text = "Find";
      // 
      // tabPageReplace
      // 
      this.tabPageReplace.ActiveControl = null;
      this.tabPageReplace.BackColor = System.Drawing.SystemColors.Control;
      this.tabPageReplace.Controls.Add(this.buttonReplaceAll);
      this.tabPageReplace.Controls.Add(this.buttonReplace);
      this.tabPageReplace.Controls.Add(this.cbReplace);
      this.tabPageReplace.Controls.Add(this.lblReplaceWith);
      this.tabPageReplace.KeyTip = null;
      this.tabPageReplace.Localisation = "TabReplace";
      this.tabPageReplace.LocalisationContext = "FindReplace";
      this.tabPageReplace.Name = "tabPageReplace";
      this.tabPageReplace.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReplace.Size = new System.Drawing.Size(464, 252);
      this.tabPageReplace.TabIndex = 1;
      this.tabPageReplace.Text = "Replace";
      // 
      // buttonReplaceAll
      // 
      this.buttonReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonReplaceAll.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonReplaceAll.Id = "eb8696a5-2e99-46d3-b041-edfa0b71e041";
      this.buttonReplaceAll.Localisation = "ReplaceAllBtn";
      this.buttonReplaceAll.LocalisationContext = "FindReplace";
      this.buttonReplaceAll.Location = new System.Drawing.Point(346, 94);
      this.buttonReplaceAll.Name = "buttonReplaceAll";
      this.buttonReplaceAll.Size = new System.Drawing.Size(108, 23);
      this.buttonReplaceAll.TabIndex = 26;
      this.buttonReplaceAll.Text = "Replace All";
      this.buttonReplaceAll.UseVisualStyleBackColor = true;
      this.buttonReplaceAll.Click += new System.EventHandler(this.buttonReplaceAll_Click);
      // 
      // buttonReplace
      // 
      this.buttonReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonReplace.Id = "1be41ce4-fc96-4c03-943f-8ea8df530a4f";
      this.buttonReplace.Localisation = "ReplaceBtn";
      this.buttonReplace.LocalisationContext = "FindReplace";
      this.buttonReplace.Location = new System.Drawing.Point(346, 63);
      this.buttonReplace.Name = "buttonReplace";
      this.buttonReplace.Size = new System.Drawing.Size(108, 23);
      this.buttonReplace.TabIndex = 25;
      this.buttonReplace.Text = "Replace";
      this.buttonReplace.UseVisualStyleBackColor = true;
      this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
      // 
      // cbReplace
      // 
      this.cbReplace.Location = new System.Drawing.Point(132, 54);
      this.cbReplace.Name = "cbReplace";
      this.cbReplace.Size = new System.Drawing.Size(195, 21);
      this.cbReplace.TabIndex = 0;
      // 
      // lblReplaceWith
      // 
      this.lblReplaceWith.Localisation = "ReplaceWith";
      this.lblReplaceWith.LocalisationContext = "FindReplace";
      this.lblReplaceWith.Location = new System.Drawing.Point(39, 57);
      this.lblReplaceWith.Name = "lblReplaceWith";
      this.lblReplaceWith.Size = new System.Drawing.Size(72, 13);
      this.lblReplaceWith.TabIndex = 0;
      this.lblReplaceWith.Text = "Replace with:";
      // 
      // buttonClose
      // 
      this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonClose.Id = "534e1f87-ff24-4653-aa52-0bb88a95f758";
      this.buttonClose.Localisation = "Cancel";
      this.buttonClose.LocalisationContext = "FindReplace";
      this.buttonClose.Location = new System.Drawing.Point(358, 176);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new System.Drawing.Size(108, 23);
      this.buttonClose.TabIndex = 2;
      this.buttonClose.Text = "Close";
      this.buttonClose.UseVisualStyleBackColor = true;
      this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
      // 
      // lblFindWhat
      // 
      this.lblFindWhat.Localisation = "FindWhat";
      this.lblFindWhat.LocalisationContext = "FindReplace";
      this.lblFindWhat.Location = new System.Drawing.Point(68, 84);
      this.lblFindWhat.Name = "lblFindWhat";
      this.lblFindWhat.Size = new System.Drawing.Size(56, 13);
      this.lblFindWhat.TabIndex = 5;
      this.lblFindWhat.Text = "Find what:";
      // 
      // cbFind
      // 
      this.cbFind.Location = new System.Drawing.Point(144, 79);
      this.cbFind.Name = "cbFind";
      this.cbFind.Size = new System.Drawing.Size(195, 21);
      this.cbFind.TabIndex = 0;
      // 
      // groupBoxMatching
      // 
      this.groupBoxMatching.Controls.Add(this.checkBoxMatchWholeWords);
      this.groupBoxMatching.Controls.Add(this.checkBoxMatchCase);
      this.groupBoxMatching.Id = "a2ba2e69-6147-49fb-a7f0-aacdc8f051ec";
      this.groupBoxMatching.Localisation = "GroupBoxConvert";
      this.groupBoxMatching.LocalisationContext = "FindReplace";
      this.groupBoxMatching.Location = new System.Drawing.Point(25, 200);
      this.groupBoxMatching.Name = "groupBoxMatching";
      this.groupBoxMatching.Size = new System.Drawing.Size(220, 81);
      this.groupBoxMatching.TabIndex = 2;
      // 
      // checkBoxMatchWholeWords
      // 
      this.checkBoxMatchWholeWords.Id = "cba3926c-5836-4143-8dce-eb480b03a5b5";
      this.checkBoxMatchWholeWords.Localisation = "MatchWords";
      this.checkBoxMatchWholeWords.LocalisationContext = "FindReplace";
      this.checkBoxMatchWholeWords.Location = new System.Drawing.Point(10, 46);
      this.checkBoxMatchWholeWords.Name = "checkBoxMatchWholeWords";
      this.checkBoxMatchWholeWords.Size = new System.Drawing.Size(140, 26);
      this.checkBoxMatchWholeWords.TabIndex = 1;
      this.checkBoxMatchWholeWords.Text = "Match whole words only";
      // 
      // checkBoxMatchCase
      // 
      this.checkBoxMatchCase.Id = "e50c4a0b-6a62-4310-b93f-75da4bb28d9c";
      this.checkBoxMatchCase.Localisation = "MatchCase";
      this.checkBoxMatchCase.LocalisationContext = "FindReplace";
      this.checkBoxMatchCase.Location = new System.Drawing.Point(10, 23);
      this.checkBoxMatchCase.Name = "checkBoxMatchCase";
      this.checkBoxMatchCase.Size = new System.Drawing.Size(83, 26);
      this.checkBoxMatchCase.TabIndex = 0;
      this.checkBoxMatchCase.Text = "Match Case";
      // 
      // labelHeader
      // 
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.ForeColor = System.Drawing.Color.DarkGray;
      this.labelHeader.Localisation = "labelHeader";
      this.labelHeader.LocalisationContext = "ShapedForm";
      this.labelHeader.Location = new System.Drawing.Point(7, 9);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(62, 20);
      this.labelHeader.TabIndex = 23;
      this.labelHeader.Text = "Header";
      // 
      // buttonFindNext
      // 
      this.buttonFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonFindNext.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonFindNext.Id = "8ca33566-e3a6-485c-a2f0-f5eb534f2c6b";
      this.buttonFindNext.Localisation = "FindNext";
      this.buttonFindNext.LocalisationContext = "FindReplace";
      this.buttonFindNext.Location = new System.Drawing.Point(358, 82);
      this.buttonFindNext.Name = "buttonFindNext";
      this.buttonFindNext.Size = new System.Drawing.Size(108, 23);
      this.buttonFindNext.TabIndex = 24;
      this.buttonFindNext.Text = "Find Next";
      this.buttonFindNext.UseVisualStyleBackColor = true;
      this.buttonFindNext.Click += new System.EventHandler(this.buttonFindNext_Click);
      // 
      // FindReplace
      // 
      this.AcceptButton = this.buttonFindNext;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.CancelButton = this.buttonClose;
      this.ClientSize = new System.Drawing.Size(489, 318);
      this.Controls.Add(this.lblFindWhat);
      this.Controls.Add(this.buttonFindNext);
      this.Controls.Add(this.buttonClose);
      this.Controls.Add(this.cbFind);
      this.Controls.Add(this.groupBoxMatching);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.tabControlFindReplace);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FindReplace";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "FindReplace";
      this.TopMost = true;
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
      ((System.ComponentModel.ISupportInitialize)(this.tabControlFindReplace)).EndInit();
      this.tabPageReplace.ResumeLayout(false);
      this.tabPageReplace.PerformLayout();
      this.groupBoxMatching.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Elegant.Ui.TabControl tabControlFindReplace;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageFind;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageReplace;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxMatching;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxMatchWholeWords;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxMatchCase;
    private MPTagThat.Core.WinControls.MPTButton buttonClose;
    private MPTagThat.Core.WinControls.MPTLabel labelHeader;
    private MPTagThat.Core.WinControls.MPTLabel lblFindWhat;
    private System.Windows.Forms.ComboBox cbFind;
    private MPTagThat.Core.WinControls.MPTButton buttonFindNext;
    private MPTagThat.Core.WinControls.MPTLabel lblReplaceWith;
    private MPTagThat.Core.WinControls.MPTButton buttonReplace;
    private System.Windows.Forms.ComboBox cbReplace;
    private MPTagThat.Core.WinControls.MPTButton buttonReplaceAll;
  }
}
