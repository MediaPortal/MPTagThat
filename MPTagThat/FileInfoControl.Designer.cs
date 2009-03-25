namespace MPTagThat
{
  partial class FileInfoControl
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
      this.picturePanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.panelPicSize = new MPTagThat.Core.WinControls.TTPanel();
      this.pictureBoxAlbumArt = new System.Windows.Forms.PictureBox();
      this.btnSaveFolderThumb = new MPTagThat.Core.WinControls.MPTButton();
      this.fileInfoPanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.listViewFileInfo = new System.Windows.Forms.ListView();
      this.picturePanel.SuspendLayout();
      this.panelPicSize.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAlbumArt)).BeginInit();
      this.fileInfoPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // picturePanel
      // 
      this.picturePanel.AnimationStep = 30;
      this.picturePanel.BackColor = System.Drawing.SystemColors.Control;
      this.picturePanel.BorderColor = System.Drawing.Color.Transparent;
      this.picturePanel.CaptionBrush = Stepi.UI.BrushType.Solid;
      this.picturePanel.CaptionColorOne = System.Drawing.SystemColors.GradientActiveCaption;
      this.picturePanel.CaptionColorTwo = System.Drawing.SystemColors.GradientInactiveCaption;
      this.picturePanel.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.picturePanel.CaptionSize = 24;
      this.picturePanel.CaptionText = "Picture";
      this.picturePanel.CaptionTextColor = System.Drawing.Color.Black;
      this.picturePanel.Controls.Add(this.panelPicSize);
      this.picturePanel.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.picturePanel.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.picturePanel.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.picturePanel.Dock = System.Windows.Forms.DockStyle.Top;
      this.picturePanel.Location = new System.Drawing.Point(0, 0);
      this.picturePanel.Name = "picturePanel";
      this.picturePanel.Size = new System.Drawing.Size(200, 237);
      this.picturePanel.TabIndex = 2;
      // 
      // panelPicSize
      // 
      this.panelPicSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelPicSize.Controls.Add(this.pictureBoxAlbumArt);
      this.panelPicSize.Location = new System.Drawing.Point(5, 28);
      this.panelPicSize.Name = "panelPicSize";
      this.panelPicSize.Size = new System.Drawing.Size(190, 240);
      this.panelPicSize.TabIndex = 2;
      // 
      // pictureBoxAlbumArt
      // 
      this.pictureBoxAlbumArt.Dock = System.Windows.Forms.DockStyle.Top;
      this.pictureBoxAlbumArt.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxAlbumArt.Name = "pictureBoxAlbumArt";
      this.pictureBoxAlbumArt.Size = new System.Drawing.Size(190, 190);
      this.pictureBoxAlbumArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBoxAlbumArt.TabIndex = 2;
      this.pictureBoxAlbumArt.TabStop = false;
      // 
      // btnSaveFolderThumb
      // 
      this.btnSaveFolderThumb.AutoSize = true;
      this.btnSaveFolderThumb.Localisation = "SaveFolderThumb";
      this.btnSaveFolderThumb.LocalisationContext = "file_info";
      this.btnSaveFolderThumb.Location = new System.Drawing.Point(4, 224);
      this.btnSaveFolderThumb.Name = "btnSaveFolderThumb";
      this.btnSaveFolderThumb.Size = new System.Drawing.Size(190, 41);
      this.btnSaveFolderThumb.TabIndex = 4;
      this.btnSaveFolderThumb.Text = "Save as folder thumb";
      this.btnSaveFolderThumb.UseVisualStyleBackColor = true;
      this.btnSaveFolderThumb.Click += new System.EventHandler(this.btnSaveFolderThumb_Click);
      // 
      // fileInfoPanel
      // 
      this.fileInfoPanel.AnimationStep = 30;
      this.fileInfoPanel.BorderColor = System.Drawing.Color.Transparent;
      this.fileInfoPanel.CaptionBrush = Stepi.UI.BrushType.Solid;
      this.fileInfoPanel.CaptionColorOne = System.Drawing.SystemColors.GradientActiveCaption;
      this.fileInfoPanel.CaptionColorTwo = System.Drawing.SystemColors.GradientInactiveCaption;
      this.fileInfoPanel.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.fileInfoPanel.CaptionSize = 24;
      this.fileInfoPanel.CaptionText = "Information";
      this.fileInfoPanel.CaptionTextColor = System.Drawing.Color.Black;
      this.fileInfoPanel.Controls.Add(this.listViewFileInfo);
      this.fileInfoPanel.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.fileInfoPanel.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.fileInfoPanel.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.fileInfoPanel.Location = new System.Drawing.Point(5, 271);
      this.fileInfoPanel.Name = "fileInfoPanel";
      this.fileInfoPanel.Size = new System.Drawing.Size(194, 173);
      this.fileInfoPanel.TabIndex = 5;
      // 
      // listViewFileInfo
      // 
      this.listViewFileInfo.BackColor = System.Drawing.SystemColors.Control;
      this.listViewFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.listViewFileInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewFileInfo.Location = new System.Drawing.Point(0, 25);
      this.listViewFileInfo.Name = "listViewFileInfo";
      this.listViewFileInfo.Size = new System.Drawing.Size(198, 192);
      this.listViewFileInfo.TabIndex = 1;
      this.listViewFileInfo.UseCompatibleStateImageBehavior = false;
      this.listViewFileInfo.View = System.Windows.Forms.View.Details;
      // 
      // FileInfoControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.fileInfoPanel);
      this.Controls.Add(this.btnSaveFolderThumb);
      this.Controls.Add(this.picturePanel);
      this.Name = "FileInfoControl";
      this.Size = new System.Drawing.Size(200, 573);
      this.picturePanel.ResumeLayout(false);
      this.panelPicSize.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAlbumArt)).EndInit();
      this.fileInfoPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.TTExtendedPanel picturePanel;
    private MPTagThat.Core.WinControls.TTPanel panelPicSize;
    private System.Windows.Forms.PictureBox pictureBoxAlbumArt;
    private MPTagThat.Core.WinControls.MPTButton btnSaveFolderThumb;
    private MPTagThat.Core.WinControls.TTExtendedPanel fileInfoPanel;
    private System.Windows.Forms.ListView listViewFileInfo;
  }
}
