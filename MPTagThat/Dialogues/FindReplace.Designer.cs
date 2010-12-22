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
      this.tabControlFindReplace = new System.Windows.Forms.TabControl();
      this.tabPageFind = new MPTagThat.Core.WinControls.MPTTabPage();
      this.tabPageReplace = new MPTagThat.Core.WinControls.MPTTabPage();
      this.buttonReplaceAll = new MPTagThat.Core.WinControls.MPTButton();
      this.buttonReplace = new MPTagThat.Core.WinControls.MPTButton();
      this.cbReplace = new System.Windows.Forms.ComboBox();
      this.lblReplaceWith = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblFindWhat = new MPTagThat.Core.WinControls.MPTLabel();
      this.cbFind = new System.Windows.Forms.ComboBox();
      this.groupBoxMatching = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.checkBoxMatchWholeWords = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxMatchCase = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.groupBoxOptions = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.rbSearchRegEx = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.rbSearchNormal = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.buttonClose = new MPTagThat.Core.WinControls.MPTButton();
      this.labelHeader = new MPTagThat.Core.WinControls.MPTLabel();
      this.buttonFindNext = new MPTagThat.Core.WinControls.MPTButton();
      this.tabControlFindReplace.SuspendLayout();
      this.tabPageReplace.SuspendLayout();
      this.groupBoxMatching.SuspendLayout();
      this.groupBoxOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControlFindReplace
      // 
      this.tabControlFindReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControlFindReplace.Controls.Add(this.tabPageFind);
      this.tabControlFindReplace.Controls.Add(this.tabPageReplace);
      this.tabControlFindReplace.Location = new System.Drawing.Point(11, 32);
      this.tabControlFindReplace.Name = "tabControlFindReplace";
      this.tabControlFindReplace.SelectedIndex = 0;
      this.tabControlFindReplace.Size = new System.Drawing.Size(466, 375);
      this.tabControlFindReplace.TabIndex = 0;
      // 
      // tabPageFind
      // 
      this.tabPageFind.BackColor = System.Drawing.SystemColors.Control;
      this.tabPageFind.Localisation = "TabFind";
      this.tabPageFind.LocalisationContext = "FindReplace";
      this.tabPageFind.Location = new System.Drawing.Point(4, 22);
      this.tabPageFind.Name = "tabPageFind";
      this.tabPageFind.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageFind.Size = new System.Drawing.Size(458, 349);
      this.tabPageFind.TabIndex = 0;
      this.tabPageFind.Text = "Find";
      // 
      // tabPageReplace
      // 
      this.tabPageReplace.BackColor = System.Drawing.SystemColors.Control;
      this.tabPageReplace.Controls.Add(this.buttonReplaceAll);
      this.tabPageReplace.Controls.Add(this.buttonReplace);
      this.tabPageReplace.Controls.Add(this.cbReplace);
      this.tabPageReplace.Controls.Add(this.lblReplaceWith);
      this.tabPageReplace.Localisation = "TabReplace";
      this.tabPageReplace.LocalisationContext = "FindReplace";
      this.tabPageReplace.Location = new System.Drawing.Point(4, 22);
      this.tabPageReplace.Name = "tabPageReplace";
      this.tabPageReplace.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReplace.Size = new System.Drawing.Size(458, 349);
      this.tabPageReplace.TabIndex = 1;
      this.tabPageReplace.Text = "Replace";
      // 
      // buttonReplaceAll
      // 
      this.buttonReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonReplaceAll.AutoSize = true;
      this.buttonReplaceAll.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonReplaceAll.Localisation = "ReplaceAllBtn";
      this.buttonReplaceAll.LocalisationContext = "FindReplace";
      this.buttonReplaceAll.Location = new System.Drawing.Point(343, 84);
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
      this.buttonReplace.AutoSize = true;
      this.buttonReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonReplace.Localisation = "ReplaceBtn";
      this.buttonReplace.LocalisationContext = "FindReplace";
      this.buttonReplace.Location = new System.Drawing.Point(343, 54);
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
      this.lblReplaceWith.AutoSize = true;
      this.lblReplaceWith.Localisation = "ReplaceWith";
      this.lblReplaceWith.LocalisationContext = "FindReplace";
      this.lblReplaceWith.Location = new System.Drawing.Point(39, 57);
      this.lblReplaceWith.Name = "lblReplaceWith";
      this.lblReplaceWith.Size = new System.Drawing.Size(72, 13);
      this.lblReplaceWith.TabIndex = 0;
      this.lblReplaceWith.Text = "Replace with:";
      // 
      // lblFindWhat
      // 
      this.lblFindWhat.AutoSize = true;
      this.lblFindWhat.Localisation = "FindWhat";
      this.lblFindWhat.LocalisationContext = "FindReplace";
      this.lblFindWhat.Location = new System.Drawing.Point(74, 84);
      this.lblFindWhat.Name = "lblFindWhat";
      this.lblFindWhat.Size = new System.Drawing.Size(56, 13);
      this.lblFindWhat.TabIndex = 5;
      this.lblFindWhat.Text = "Find what:";
      // 
      // cbFind
      // 
      this.cbFind.Location = new System.Drawing.Point(147, 77);
      this.cbFind.Name = "cbFind";
      this.cbFind.Size = new System.Drawing.Size(195, 21);
      this.cbFind.TabIndex = 0;
      // 
      // groupBoxMatching
      // 
      this.groupBoxMatching.Controls.Add(this.checkBoxMatchWholeWords);
      this.groupBoxMatching.Controls.Add(this.checkBoxMatchCase);
      this.groupBoxMatching.Localisation = "GroupBoxConvert";
      this.groupBoxMatching.LocalisationContext = "FindReplace";
      this.groupBoxMatching.Location = new System.Drawing.Point(25, 201);
      this.groupBoxMatching.Name = "groupBoxMatching";
      this.groupBoxMatching.Size = new System.Drawing.Size(220, 80);
      this.groupBoxMatching.TabIndex = 2;
      this.groupBoxMatching.TabStop = false;
      // 
      // checkBoxMatchWholeWords
      // 
      this.checkBoxMatchWholeWords.AutoSize = true;
      this.checkBoxMatchWholeWords.Localisation = "MatchWords";
      this.checkBoxMatchWholeWords.LocalisationContext = "FindReplace";
      this.checkBoxMatchWholeWords.Location = new System.Drawing.Point(10, 46);
      this.checkBoxMatchWholeWords.Name = "checkBoxMatchWholeWords";
      this.checkBoxMatchWholeWords.Size = new System.Drawing.Size(140, 17);
      this.checkBoxMatchWholeWords.TabIndex = 1;
      this.checkBoxMatchWholeWords.Text = "Match whole words only";
      this.checkBoxMatchWholeWords.UseVisualStyleBackColor = true;
      // 
      // checkBoxMatchCase
      // 
      this.checkBoxMatchCase.AutoSize = true;
      this.checkBoxMatchCase.Localisation = "MatchCase";
      this.checkBoxMatchCase.LocalisationContext = "FindReplace";
      this.checkBoxMatchCase.Location = new System.Drawing.Point(10, 23);
      this.checkBoxMatchCase.Name = "checkBoxMatchCase";
      this.checkBoxMatchCase.Size = new System.Drawing.Size(83, 17);
      this.checkBoxMatchCase.TabIndex = 0;
      this.checkBoxMatchCase.Text = "Match Case";
      this.checkBoxMatchCase.UseVisualStyleBackColor = true;
      // 
      // groupBoxOptions
      // 
      this.groupBoxOptions.Controls.Add(this.rbSearchRegEx);
      this.groupBoxOptions.Controls.Add(this.rbSearchNormal);
      this.groupBoxOptions.Localisation = "GroupBoxSearch";
      this.groupBoxOptions.LocalisationContext = "FindReplace";
      this.groupBoxOptions.Location = new System.Drawing.Point(25, 287);
      this.groupBoxOptions.Name = "groupBoxOptions";
      this.groupBoxOptions.Size = new System.Drawing.Size(220, 85);
      this.groupBoxOptions.TabIndex = 4;
      this.groupBoxOptions.TabStop = false;
      this.groupBoxOptions.Text = "Search Mode";
      // 
      // rbSearchRegEx
      // 
      this.rbSearchRegEx.AutoSize = true;
      this.rbSearchRegEx.Localisation = "SearchRegex";
      this.rbSearchRegEx.LocalisationContext = "FindReplace";
      this.rbSearchRegEx.Location = new System.Drawing.Point(10, 52);
      this.rbSearchRegEx.Name = "rbSearchRegEx";
      this.rbSearchRegEx.Size = new System.Drawing.Size(116, 17);
      this.rbSearchRegEx.TabIndex = 8;
      this.rbSearchRegEx.Text = "Regular Expression";
      this.rbSearchRegEx.UseVisualStyleBackColor = true;
      // 
      // rbSearchNormal
      // 
      this.rbSearchNormal.AutoSize = true;
      this.rbSearchNormal.Checked = true;
      this.rbSearchNormal.Localisation = "SearchNormal";
      this.rbSearchNormal.LocalisationContext = "FindReplace";
      this.rbSearchNormal.Location = new System.Drawing.Point(10, 29);
      this.rbSearchNormal.Name = "rbSearchNormal";
      this.rbSearchNormal.Size = new System.Drawing.Size(58, 17);
      this.rbSearchNormal.TabIndex = 7;
      this.rbSearchNormal.TabStop = true;
      this.rbSearchNormal.Text = "Normal";
      this.rbSearchNormal.UseVisualStyleBackColor = true;
      // 
      // buttonClose
      // 
      this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClose.AutoSize = true;
      this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonClose.Localisation = "Cancel";
      this.buttonClose.LocalisationContext = "FindReplace";
      this.buttonClose.Location = new System.Drawing.Point(358, 170);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new System.Drawing.Size(108, 23);
      this.buttonClose.TabIndex = 2;
      this.buttonClose.Text = "Close";
      this.buttonClose.UseVisualStyleBackColor = true;
      this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
      // 
      // labelHeader
      // 
      this.labelHeader.AutoSize = true;
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.ForeColor = System.Drawing.Color.DarkGray;
      this.labelHeader.Location = new System.Drawing.Point(7, 9);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(62, 20);
      this.labelHeader.TabIndex = 23;
      this.labelHeader.Text = "Header";
      // 
      // buttonFindNext
      // 
      this.buttonFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonFindNext.AutoSize = true;
      this.buttonFindNext.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonFindNext.Localisation = "FindNext";
      this.buttonFindNext.LocalisationContext = "FindReplace";
      this.buttonFindNext.Location = new System.Drawing.Point(358, 74);
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
      this.ClientSize = new System.Drawing.Size(489, 419);
      this.Controls.Add(this.lblFindWhat);
      this.Controls.Add(this.buttonFindNext);
      this.Controls.Add(this.cbFind);
      this.Controls.Add(this.groupBoxMatching);
      this.Controls.Add(this.buttonClose);
      this.Controls.Add(this.groupBoxOptions);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.tabControlFindReplace);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FindReplace";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "FindReplace";
      this.TopMost = true;
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
      this.tabControlFindReplace.ResumeLayout(false);
      this.tabPageReplace.ResumeLayout(false);
      this.tabPageReplace.PerformLayout();
      this.groupBoxMatching.ResumeLayout(false);
      this.groupBoxMatching.PerformLayout();
      this.groupBoxOptions.ResumeLayout(false);
      this.groupBoxOptions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabControlFindReplace;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageFind;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageReplace;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxMatching;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxMatchWholeWords;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxMatchCase;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxOptions;
    private MPTagThat.Core.WinControls.MPTButton buttonClose;
    private MPTagThat.Core.WinControls.MPTLabel labelHeader;
    private MPTagThat.Core.WinControls.MPTLabel lblFindWhat;
    private System.Windows.Forms.ComboBox cbFind;
    private MPTagThat.Core.WinControls.MPTRadioButton rbSearchRegEx;
    private MPTagThat.Core.WinControls.MPTRadioButton rbSearchNormal;
    private MPTagThat.Core.WinControls.MPTButton buttonFindNext;
    private MPTagThat.Core.WinControls.MPTLabel lblReplaceWith;
    private MPTagThat.Core.WinControls.MPTButton buttonReplace;
    private System.Windows.Forms.ComboBox cbReplace;
    private MPTagThat.Core.WinControls.MPTButton buttonReplaceAll;
  }
}