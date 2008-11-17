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
      this.comboBoxScripts = new System.Windows.Forms.ComboBox();
      this.btPreviousFile = new MPTagThat.Core.WinControls.MPTButton();
      this.btNextFile = new MPTagThat.Core.WinControls.MPTButton();
      this.btGetTrackLength = new MPTagThat.Core.WinControls.MPTButton();
      this.tbTrackLength = new System.Windows.Forms.TextBox();
      this.btGetLyricsFromInternet = new MPTagThat.Core.WinControls.MPTButton();

      this.cmdPanel.SuspendLayout();
      this.SuspendLayout();

      this.groupBoxMedia.Controls.Add(this.btGetTrackLength);
      this.groupBoxMedia.Controls.Add(this.tbTrackLength);
 
      // 
      // btGetTrackLength
      // 
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

      this.groupBoxLyrics.Controls.Add(this.btGetLyricsFromInternet);
      // 
      // btGetLyricsFromInternet
      // 
      this.btGetLyricsFromInternet.Localisation = "GetLyricsFromInternet";
      this.btGetLyricsFromInternet.LocalisationContext = "TagEdit";
      this.btGetLyricsFromInternet.Location = new System.Drawing.Point(359, 246);
      this.btGetLyricsFromInternet.Name = "btGetLyricsFromInternet";
      this.btGetLyricsFromInternet.Size = new System.Drawing.Size(307, 23);
      this.btGetLyricsFromInternet.TabIndex = 12;
      this.btGetLyricsFromInternet.Text = "Get Lyrics from the Internet";
      this.btGetLyricsFromInternet.UseVisualStyleBackColor = true;
      this.btGetLyricsFromInternet.Click += new System.EventHandler(this.btGetLyricsFromInternet_Click);

      this.panelNavigation.Controls.Add(this.cmdPanel);
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
      this.btScriptExecute.Image = global::MPTagThat.Properties.Resources.FormRunHS;
      this.btScriptExecute.Localisation = "ttButton1";
      this.btScriptExecute.LocalisationContext = "ExtendedPanel";
      this.btScriptExecute.Location = new System.Drawing.Point(130, 40);
      this.btScriptExecute.Name = "btScriptExecute";
      this.btScriptExecute.Size = new System.Drawing.Size(23, 23);
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
      this.btPreviousFile.Image = global::MPTagThat.Properties.Resources.NavBack;
      this.btPreviousFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btPreviousFile.Localisation = "Previous";
      this.btPreviousFile.LocalisationContext = "TagEdit";
      this.btPreviousFile.Location = new System.Drawing.Point(6, 158);
      this.btPreviousFile.Name = "btPreviousFile";
      this.btPreviousFile.Size = new System.Drawing.Size(118, 23);
      this.btPreviousFile.TabIndex = 2;
      this.btPreviousFile.Text = "Previous File";
      this.btPreviousFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btPreviousFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btPreviousFile.UseVisualStyleBackColor = true;
      this.btPreviousFile.Click += new System.EventHandler(this.btPreviousFile_Click);
      // 
      // btNextFile
      // 
      this.btNextFile.Image = global::MPTagThat.Properties.Resources.NavForward;
      this.btNextFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btNextFile.Localisation = "Next";
      this.btNextFile.LocalisationContext = "TagEdit";
      this.btNextFile.Location = new System.Drawing.Point(6, 187);
      this.btNextFile.Name = "btNextFile";
      this.btNextFile.Size = new System.Drawing.Size(118, 23);
      this.btNextFile.TabIndex = 1;
      this.btNextFile.Text = "Next File";
      this.btNextFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btNextFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btNextFile.UseVisualStyleBackColor = true;
      this.btNextFile.Click += new System.EventHandler(this.btNextFile_Click);
    }
    #endregion

    private MPTagThat.Core.WinControls.MPTButton btGetLyricsFromInternet;
    private MPTagThat.Core.WinControls.TTExtendedPanel cmdPanel;
    private MPTagThat.Core.WinControls.MPTButton btScriptExecute;
    private System.Windows.Forms.ComboBox comboBoxScripts;
    private MPTagThat.Core.WinControls.MPTButton btPreviousFile;
    private MPTagThat.Core.WinControls.MPTButton btNextFile;
    private MPTagThat.Core.WinControls.MPTButton btGetTrackLength;
    private System.Windows.Forms.TextBox tbTrackLength;
  }
}