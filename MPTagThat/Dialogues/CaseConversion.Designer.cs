namespace MPTagThat.CaseConversion
{
  partial class CaseConversion
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
      this.tabControlConversion = new System.Windows.Forms.TabControl();
      this.tabPageSettings = new MPTagThat.Core.WinControls.MPTTabPage();
      this.groupBoxOptions = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.checkBoxAlwaysUpperCaseFirstLetter = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxReplaceSpaceByUnderscore = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxReplaceUnderscoreBySpace = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxReplaceSpaceby20 = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxReplace20bySpace = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.groupBoxMethod = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.radioButtonAllFirstLetterUpperCase = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.radioButtonFirstLetterUpperCase = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.radioButtonAllUpperCase = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.radioButtonAllLowerCase = new MPTagThat.Core.WinControls.MPTRadioButton();
      this.groupBoxConvert = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.checkBoxComment = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxTitle = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxAlbum = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxAlbumArtist = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxArtist = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxConvertTags = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.checkBoxConvertFileName = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.Exceptions = new MPTagThat.Core.WinControls.MPTTabPage();
      this.buttonRemoveException = new MPTagThat.Core.WinControls.MPTButton();
      this.buttonAddException = new MPTagThat.Core.WinControls.MPTButton();
      this.tbException = new System.Windows.Forms.TextBox();
      this.listBoxExceptions = new System.Windows.Forms.ListBox();
      this.buttonConvert = new MPTagThat.Core.WinControls.MPTButton();
      this.buttonCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.labelHeader = new MPTagThat.Core.WinControls.MPTLabel();
      this.tabControlConversion.SuspendLayout();
      this.tabPageSettings.SuspendLayout();
      this.groupBoxOptions.SuspendLayout();
      this.groupBoxMethod.SuspendLayout();
      this.groupBoxConvert.SuspendLayout();
      this.Exceptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControlConversion
      // 
      this.tabControlConversion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControlConversion.Controls.Add(this.tabPageSettings);
      this.tabControlConversion.Controls.Add(this.Exceptions);
      this.tabControlConversion.Location = new System.Drawing.Point(11, 32);
      this.tabControlConversion.Name = "tabControlConversion";
      this.tabControlConversion.SelectedIndex = 0;
      this.tabControlConversion.Size = new System.Drawing.Size(466, 341);
      this.tabControlConversion.TabIndex = 0;
      // 
      // tabPageSettings
      // 
      this.tabPageSettings.BackColor = System.Drawing.SystemColors.Control;
      this.tabPageSettings.Controls.Add(this.groupBoxOptions);
      this.tabPageSettings.Controls.Add(this.groupBoxMethod);
      this.tabPageSettings.Controls.Add(this.groupBoxConvert);
      this.tabPageSettings.Controls.Add(this.checkBoxConvertTags);
      this.tabPageSettings.Controls.Add(this.checkBoxConvertFileName);
      this.tabPageSettings.Localisation = "TabSettings";
      this.tabPageSettings.LocalisationContext = "CaseConversion";
      this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
      this.tabPageSettings.Name = "tabPageSettings";
      this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageSettings.Size = new System.Drawing.Size(458, 315);
      this.tabPageSettings.TabIndex = 0;
      this.tabPageSettings.Text = "Settings";
      // 
      // groupBoxOptions
      // 
      this.groupBoxOptions.Controls.Add(this.checkBoxAlwaysUpperCaseFirstLetter);
      this.groupBoxOptions.Controls.Add(this.checkBoxReplaceSpaceByUnderscore);
      this.groupBoxOptions.Controls.Add(this.checkBoxReplaceUnderscoreBySpace);
      this.groupBoxOptions.Controls.Add(this.checkBoxReplaceSpaceby20);
      this.groupBoxOptions.Controls.Add(this.checkBoxReplace20bySpace);
      this.groupBoxOptions.Localisation = "GroupBoxOptions";
      this.groupBoxOptions.LocalisationContext = "CaseConversion";
      this.groupBoxOptions.Location = new System.Drawing.Point(8, 207);
      this.groupBoxOptions.Name = "groupBoxOptions";
      this.groupBoxOptions.Size = new System.Drawing.Size(444, 100);
      this.groupBoxOptions.TabIndex = 4;
      this.groupBoxOptions.TabStop = false;
      this.groupBoxOptions.Text = "Options";
      // 
      // checkBoxAlwaysUpperCaseFirstLetter
      // 
      this.checkBoxAlwaysUpperCaseFirstLetter.AutoSize = true;
      this.checkBoxAlwaysUpperCaseFirstLetter.Checked = true;
      this.checkBoxAlwaysUpperCaseFirstLetter.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxAlwaysUpperCaseFirstLetter.Localisation = "AlwaysUpperCaseFirstLetter";
      this.checkBoxAlwaysUpperCaseFirstLetter.LocalisationContext = "CaseConversion";
      this.checkBoxAlwaysUpperCaseFirstLetter.Location = new System.Drawing.Point(10, 74);
      this.checkBoxAlwaysUpperCaseFirstLetter.Name = "checkBoxAlwaysUpperCaseFirstLetter";
      this.checkBoxAlwaysUpperCaseFirstLetter.Size = new System.Drawing.Size(166, 17);
      this.checkBoxAlwaysUpperCaseFirstLetter.TabIndex = 5;
      this.checkBoxAlwaysUpperCaseFirstLetter.Text = "Always Uppercase First Letter";
      this.checkBoxAlwaysUpperCaseFirstLetter.UseVisualStyleBackColor = true;
      // 
      // checkBoxReplaceSpaceByUnderscore
      // 
      this.checkBoxReplaceSpaceByUnderscore.AutoSize = true;
      this.checkBoxReplaceSpaceByUnderscore.Localisation = "ReplaceSpaceByUnderscore";
      this.checkBoxReplaceSpaceByUnderscore.LocalisationContext = "CaseConversion";
      this.checkBoxReplaceSpaceByUnderscore.Location = new System.Drawing.Point(234, 51);
      this.checkBoxReplaceSpaceByUnderscore.Name = "checkBoxReplaceSpaceByUnderscore";
      this.checkBoxReplaceSpaceByUnderscore.Size = new System.Drawing.Size(133, 17);
      this.checkBoxReplaceSpaceByUnderscore.TabIndex = 4;
      this.checkBoxReplaceSpaceByUnderscore.Text = "Replace Space by \"_\"";
      this.checkBoxReplaceSpaceByUnderscore.UseVisualStyleBackColor = true;
      // 
      // checkBoxReplaceUnderscoreBySpace
      // 
      this.checkBoxReplaceUnderscoreBySpace.AutoSize = true;
      this.checkBoxReplaceUnderscoreBySpace.Localisation = "ReplaceUnderscoreBySpace";
      this.checkBoxReplaceUnderscoreBySpace.LocalisationContext = "CaseConversion";
      this.checkBoxReplaceUnderscoreBySpace.Location = new System.Drawing.Point(10, 51);
      this.checkBoxReplaceUnderscoreBySpace.Name = "checkBoxReplaceUnderscoreBySpace";
      this.checkBoxReplaceUnderscoreBySpace.Size = new System.Drawing.Size(133, 17);
      this.checkBoxReplaceUnderscoreBySpace.TabIndex = 3;
      this.checkBoxReplaceUnderscoreBySpace.Text = "Replace \"_\" by Space";
      this.checkBoxReplaceUnderscoreBySpace.UseVisualStyleBackColor = true;
      // 
      // checkBoxReplaceSpaceby20
      // 
      this.checkBoxReplaceSpaceby20.AutoSize = true;
      this.checkBoxReplaceSpaceby20.Localisation = "ReplaceSpaceby20";
      this.checkBoxReplaceSpaceby20.LocalisationContext = "CaseConversion";
      this.checkBoxReplaceSpaceby20.Location = new System.Drawing.Point(234, 28);
      this.checkBoxReplaceSpaceby20.Name = "checkBoxReplaceSpaceby20";
      this.checkBoxReplaceSpaceby20.Size = new System.Drawing.Size(147, 17);
      this.checkBoxReplaceSpaceby20.TabIndex = 2;
      this.checkBoxReplaceSpaceby20.Text = "Replace Space by \"%20\"";
      this.checkBoxReplaceSpaceby20.UseVisualStyleBackColor = true;
      // 
      // checkBoxReplace20bySpace
      // 
      this.checkBoxReplace20bySpace.AutoSize = true;
      this.checkBoxReplace20bySpace.Localisation = "Replace20bySpace";
      this.checkBoxReplace20bySpace.LocalisationContext = "CaseConversion";
      this.checkBoxReplace20bySpace.Location = new System.Drawing.Point(10, 28);
      this.checkBoxReplace20bySpace.Name = "checkBoxReplace20bySpace";
      this.checkBoxReplace20bySpace.Size = new System.Drawing.Size(147, 17);
      this.checkBoxReplace20bySpace.TabIndex = 1;
      this.checkBoxReplace20bySpace.Text = "Replace \"%20\" by Space";
      this.checkBoxReplace20bySpace.UseVisualStyleBackColor = true;
      // 
      // groupBoxMethod
      // 
      this.groupBoxMethod.Controls.Add(this.radioButtonAllFirstLetterUpperCase);
      this.groupBoxMethod.Controls.Add(this.radioButtonFirstLetterUpperCase);
      this.groupBoxMethod.Controls.Add(this.radioButtonAllUpperCase);
      this.groupBoxMethod.Controls.Add(this.radioButtonAllLowerCase);
      this.groupBoxMethod.Localisation = "GroupBoxMethod";
      this.groupBoxMethod.LocalisationContext = "CaseConversion";
      this.groupBoxMethod.Location = new System.Drawing.Point(232, 55);
      this.groupBoxMethod.Name = "groupBoxMethod";
      this.groupBoxMethod.Size = new System.Drawing.Size(220, 145);
      this.groupBoxMethod.TabIndex = 3;
      this.groupBoxMethod.TabStop = false;
      this.groupBoxMethod.Text = "Conversion Method";
      // 
      // radioButtonAllFirstLetterUpperCase
      // 
      this.radioButtonAllFirstLetterUpperCase.AutoSize = true;
      this.radioButtonAllFirstLetterUpperCase.Checked = true;
      this.radioButtonAllFirstLetterUpperCase.Localisation = "AllFirstLetterUpperCase";
      this.radioButtonAllFirstLetterUpperCase.LocalisationContext = "CaseConversion";
      this.radioButtonAllFirstLetterUpperCase.Location = new System.Drawing.Point(6, 116);
      this.radioButtonAllFirstLetterUpperCase.Name = "radioButtonAllFirstLetterUpperCase";
      this.radioButtonAllFirstLetterUpperCase.Size = new System.Drawing.Size(147, 17);
      this.radioButtonAllFirstLetterUpperCase.TabIndex = 3;
      this.radioButtonAllFirstLetterUpperCase.TabStop = true;
      this.radioButtonAllFirstLetterUpperCase.Text = "All First Letter Upper Case";
      this.radioButtonAllFirstLetterUpperCase.UseVisualStyleBackColor = true;
      // 
      // radioButtonFirstLetterUpperCase
      // 
      this.radioButtonFirstLetterUpperCase.AutoSize = true;
      this.radioButtonFirstLetterUpperCase.Localisation = "FirstLetterUpperCase";
      this.radioButtonFirstLetterUpperCase.LocalisationContext = "CaseConversion";
      this.radioButtonFirstLetterUpperCase.Location = new System.Drawing.Point(6, 85);
      this.radioButtonFirstLetterUpperCase.Name = "radioButtonFirstLetterUpperCase";
      this.radioButtonFirstLetterUpperCase.Size = new System.Drawing.Size(133, 17);
      this.radioButtonFirstLetterUpperCase.TabIndex = 2;
      this.radioButtonFirstLetterUpperCase.Text = "First Letter Upper Case";
      this.radioButtonFirstLetterUpperCase.UseVisualStyleBackColor = true;
      // 
      // radioButtonAllUpperCase
      // 
      this.radioButtonAllUpperCase.AutoSize = true;
      this.radioButtonAllUpperCase.Localisation = "AllUpperCase";
      this.radioButtonAllUpperCase.LocalisationContext = "CaseConversion";
      this.radioButtonAllUpperCase.Location = new System.Drawing.Point(6, 54);
      this.radioButtonAllUpperCase.Name = "radioButtonAllUpperCase";
      this.radioButtonAllUpperCase.Size = new System.Drawing.Size(95, 17);
      this.radioButtonAllUpperCase.TabIndex = 1;
      this.radioButtonAllUpperCase.Text = "All Upper Case";
      this.radioButtonAllUpperCase.UseVisualStyleBackColor = true;
      // 
      // radioButtonAllLowerCase
      // 
      this.radioButtonAllLowerCase.AutoSize = true;
      this.radioButtonAllLowerCase.Localisation = "AllLowerCase";
      this.radioButtonAllLowerCase.LocalisationContext = "CaseConversion";
      this.radioButtonAllLowerCase.Location = new System.Drawing.Point(6, 23);
      this.radioButtonAllLowerCase.Name = "radioButtonAllLowerCase";
      this.radioButtonAllLowerCase.Size = new System.Drawing.Size(95, 17);
      this.radioButtonAllLowerCase.TabIndex = 0;
      this.radioButtonAllLowerCase.Text = "All Lower Case";
      this.radioButtonAllLowerCase.UseVisualStyleBackColor = true;
      // 
      // groupBoxConvert
      // 
      this.groupBoxConvert.Controls.Add(this.checkBoxComment);
      this.groupBoxConvert.Controls.Add(this.checkBoxTitle);
      this.groupBoxConvert.Controls.Add(this.checkBoxAlbum);
      this.groupBoxConvert.Controls.Add(this.checkBoxAlbumArtist);
      this.groupBoxConvert.Controls.Add(this.checkBoxArtist);
      this.groupBoxConvert.Localisation = "GroupBoxConvert";
      this.groupBoxConvert.LocalisationContext = "CaseConversion";
      this.groupBoxConvert.Location = new System.Drawing.Point(8, 55);
      this.groupBoxConvert.Name = "groupBoxConvert";
      this.groupBoxConvert.Size = new System.Drawing.Size(220, 145);
      this.groupBoxConvert.TabIndex = 2;
      this.groupBoxConvert.TabStop = false;
      this.groupBoxConvert.Text = "Convert Tags for ...";
      // 
      // checkBoxComment
      // 
      this.checkBoxComment.AutoSize = true;
      this.checkBoxComment.Checked = true;
      this.checkBoxComment.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxComment.Localisation = "Comment";
      this.checkBoxComment.LocalisationContext = "CaseConversion";
      this.checkBoxComment.Location = new System.Drawing.Point(10, 115);
      this.checkBoxComment.Name = "checkBoxComment";
      this.checkBoxComment.Size = new System.Drawing.Size(70, 17);
      this.checkBoxComment.TabIndex = 4;
      this.checkBoxComment.Text = "Comment";
      this.checkBoxComment.UseVisualStyleBackColor = true;
      // 
      // checkBoxTitle
      // 
      this.checkBoxTitle.AutoSize = true;
      this.checkBoxTitle.Checked = true;
      this.checkBoxTitle.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxTitle.Localisation = "Title";
      this.checkBoxTitle.LocalisationContext = "CaseConversion";
      this.checkBoxTitle.Location = new System.Drawing.Point(10, 92);
      this.checkBoxTitle.Name = "checkBoxTitle";
      this.checkBoxTitle.Size = new System.Drawing.Size(46, 17);
      this.checkBoxTitle.TabIndex = 3;
      this.checkBoxTitle.Text = "Title";
      this.checkBoxTitle.UseVisualStyleBackColor = true;
      // 
      // checkBoxAlbum
      // 
      this.checkBoxAlbum.AutoSize = true;
      this.checkBoxAlbum.Checked = true;
      this.checkBoxAlbum.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxAlbum.Localisation = "Album";
      this.checkBoxAlbum.LocalisationContext = "CaseConversion";
      this.checkBoxAlbum.Location = new System.Drawing.Point(10, 69);
      this.checkBoxAlbum.Name = "checkBoxAlbum";
      this.checkBoxAlbum.Size = new System.Drawing.Size(55, 17);
      this.checkBoxAlbum.TabIndex = 2;
      this.checkBoxAlbum.Text = "Album";
      this.checkBoxAlbum.UseVisualStyleBackColor = true;
      // 
      // checkBoxAlbumArtist
      // 
      this.checkBoxAlbumArtist.AutoSize = true;
      this.checkBoxAlbumArtist.Checked = true;
      this.checkBoxAlbumArtist.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxAlbumArtist.Localisation = "AlbumArtist";
      this.checkBoxAlbumArtist.LocalisationContext = "CaseConversion";
      this.checkBoxAlbumArtist.Location = new System.Drawing.Point(10, 46);
      this.checkBoxAlbumArtist.Name = "checkBoxAlbumArtist";
      this.checkBoxAlbumArtist.Size = new System.Drawing.Size(78, 17);
      this.checkBoxAlbumArtist.TabIndex = 1;
      this.checkBoxAlbumArtist.Text = "AlbumArtist";
      this.checkBoxAlbumArtist.UseVisualStyleBackColor = true;
      // 
      // checkBoxArtist
      // 
      this.checkBoxArtist.AutoSize = true;
      this.checkBoxArtist.Checked = true;
      this.checkBoxArtist.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxArtist.Localisation = "Artist";
      this.checkBoxArtist.LocalisationContext = "CaseConversion";
      this.checkBoxArtist.Location = new System.Drawing.Point(10, 23);
      this.checkBoxArtist.Name = "checkBoxArtist";
      this.checkBoxArtist.Size = new System.Drawing.Size(49, 17);
      this.checkBoxArtist.TabIndex = 0;
      this.checkBoxArtist.Text = "Artist";
      this.checkBoxArtist.UseVisualStyleBackColor = true;
      // 
      // checkBoxConvertTags
      // 
      this.checkBoxConvertTags.AutoSize = true;
      this.checkBoxConvertTags.Checked = true;
      this.checkBoxConvertTags.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxConvertTags.Localisation = "ConvertTags";
      this.checkBoxConvertTags.LocalisationContext = "CaseConversion";
      this.checkBoxConvertTags.Location = new System.Drawing.Point(238, 21);
      this.checkBoxConvertTags.Name = "checkBoxConvertTags";
      this.checkBoxConvertTags.Size = new System.Drawing.Size(90, 17);
      this.checkBoxConvertTags.TabIndex = 1;
      this.checkBoxConvertTags.Text = "Convert Tags";
      this.checkBoxConvertTags.UseVisualStyleBackColor = true;
      // 
      // checkBoxConvertFileName
      // 
      this.checkBoxConvertFileName.AutoSize = true;
      this.checkBoxConvertFileName.Checked = true;
      this.checkBoxConvertFileName.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxConvertFileName.Localisation = "ConvertFileName";
      this.checkBoxConvertFileName.LocalisationContext = "CaseConversion";
      this.checkBoxConvertFileName.Location = new System.Drawing.Point(18, 21);
      this.checkBoxConvertFileName.Name = "checkBoxConvertFileName";
      this.checkBoxConvertFileName.Size = new System.Drawing.Size(108, 17);
      this.checkBoxConvertFileName.TabIndex = 0;
      this.checkBoxConvertFileName.Text = "Convert Filename";
      this.checkBoxConvertFileName.UseVisualStyleBackColor = true;
      // 
      // Exceptions
      // 
      this.Exceptions.BackColor = System.Drawing.SystemColors.Control;
      this.Exceptions.Controls.Add(this.buttonRemoveException);
      this.Exceptions.Controls.Add(this.buttonAddException);
      this.Exceptions.Controls.Add(this.tbException);
      this.Exceptions.Controls.Add(this.listBoxExceptions);
      this.Exceptions.Localisation = "Exceptions";
      this.Exceptions.LocalisationContext = "CaseConversion";
      this.Exceptions.Location = new System.Drawing.Point(4, 22);
      this.Exceptions.Name = "Exceptions";
      this.Exceptions.Padding = new System.Windows.Forms.Padding(3);
      this.Exceptions.Size = new System.Drawing.Size(458, 315);
      this.Exceptions.TabIndex = 1;
      this.Exceptions.Text = "Exceptions";
      // 
      // buttonRemoveException
      // 
      this.buttonRemoveException.Localisation = "RemoveException";
      this.buttonRemoveException.LocalisationContext = "Exceptions";
      this.buttonRemoveException.Location = new System.Drawing.Point(281, 53);
      this.buttonRemoveException.Name = "buttonRemoveException";
      this.buttonRemoveException.Size = new System.Drawing.Size(171, 23);
      this.buttonRemoveException.TabIndex = 3;
      this.buttonRemoveException.Text = "Remove";
      this.buttonRemoveException.UseVisualStyleBackColor = true;
      this.buttonRemoveException.Click += new System.EventHandler(this.buttonRemoveException_Click);
      // 
      // buttonAddException
      // 
      this.buttonAddException.Localisation = "AddException";
      this.buttonAddException.LocalisationContext = "Exceptions";
      this.buttonAddException.Location = new System.Drawing.Point(281, 16);
      this.buttonAddException.Name = "buttonAddException";
      this.buttonAddException.Size = new System.Drawing.Size(171, 23);
      this.buttonAddException.TabIndex = 2;
      this.buttonAddException.Text = "Add";
      this.buttonAddException.UseVisualStyleBackColor = true;
      this.buttonAddException.Click += new System.EventHandler(this.buttonAddException_Click);
      // 
      // tbException
      // 
      this.tbException.Location = new System.Drawing.Point(7, 16);
      this.tbException.Name = "tbException";
      this.tbException.Size = new System.Drawing.Size(247, 20);
      this.tbException.TabIndex = 1;
      // 
      // listBoxExceptions
      // 
      this.listBoxExceptions.FormattingEnabled = true;
      this.listBoxExceptions.Location = new System.Drawing.Point(8, 53);
      this.listBoxExceptions.Name = "listBoxExceptions";
      this.listBoxExceptions.Size = new System.Drawing.Size(246, 251);
      this.listBoxExceptions.TabIndex = 0;
      // 
      // buttonConvert
      // 
      this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonConvert.AutoSize = true;
      this.buttonConvert.Localisation = "Convert";
      this.buttonConvert.LocalisationContext = "CaseConversion";
      this.buttonConvert.Location = new System.Drawing.Point(311, 384);
      this.buttonConvert.Name = "buttonConvert";
      this.buttonConvert.Size = new System.Drawing.Size(75, 23);
      this.buttonConvert.TabIndex = 1;
      this.buttonConvert.Text = "Convert";
      this.buttonConvert.UseVisualStyleBackColor = true;
      this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.AutoSize = true;
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Localisation = "Cancel";
      this.buttonCancel.LocalisationContext = "CaseConversion";
      this.buttonCancel.Location = new System.Drawing.Point(392, 384);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelHeader
      // 
      this.labelHeader.AutoSize = true;
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.ForeColor = System.Drawing.Color.White;
      this.labelHeader.Location = new System.Drawing.Point(7, 9);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(62, 20);
      this.labelHeader.TabIndex = 23;
      this.labelHeader.Text = "Header";
      // 
      // CaseConversion
      // 
      this.AcceptButton = this.buttonConvert;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(489, 419);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonConvert);
      this.Controls.Add(this.tabControlConversion);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "CaseConversion";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "CaseConversion";
      this.tabControlConversion.ResumeLayout(false);
      this.tabPageSettings.ResumeLayout(false);
      this.tabPageSettings.PerformLayout();
      this.groupBoxOptions.ResumeLayout(false);
      this.groupBoxOptions.PerformLayout();
      this.groupBoxMethod.ResumeLayout(false);
      this.groupBoxMethod.PerformLayout();
      this.groupBoxConvert.ResumeLayout(false);
      this.groupBoxConvert.PerformLayout();
      this.Exceptions.ResumeLayout(false);
      this.Exceptions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabControlConversion;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageSettings;
    private MPTagThat.Core.WinControls.MPTTabPage Exceptions;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxConvertTags;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxConvertFileName;
    private System.Windows.Forms.ListBox listBoxExceptions;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxConvert;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxComment;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxTitle;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxAlbum;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxAlbumArtist;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxArtist;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxMethod;
    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxOptions;
    private MPTagThat.Core.WinControls.MPTRadioButton radioButtonAllLowerCase;
    private MPTagThat.Core.WinControls.MPTRadioButton radioButtonFirstLetterUpperCase;
    private MPTagThat.Core.WinControls.MPTRadioButton radioButtonAllUpperCase;
    private MPTagThat.Core.WinControls.MPTRadioButton radioButtonAllFirstLetterUpperCase;
    private MPTagThat.Core.WinControls.MPTButton buttonRemoveException;
    private MPTagThat.Core.WinControls.MPTButton buttonAddException;
    private System.Windows.Forms.TextBox tbException;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxReplace20bySpace;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxAlwaysUpperCaseFirstLetter;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxReplaceSpaceByUnderscore;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxReplaceUnderscoreBySpace;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxReplaceSpaceby20;
    private MPTagThat.Core.WinControls.MPTButton buttonConvert;
    private MPTagThat.Core.WinControls.MPTButton buttonCancel;
    private MPTagThat.Core.WinControls.MPTLabel labelHeader;
  }
}