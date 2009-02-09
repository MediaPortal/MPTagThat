namespace MPTagThat.Player
{
  partial class PlayerControl
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
      this.playListGrid = new System.Windows.Forms.DataGridView();
      this.lblTitle = new MPTagThat.Core.MarqueeLabel();
      this.panelLeft = new MPTagThat.Core.WinControls.TTPanel();
      this.pictureBoxPlayPause = new System.Windows.Forms.PictureBox();
      this.buttonPrev = new System.Windows.Forms.Button();
      this.buttonNext = new System.Windows.Forms.Button();
      this.panelRight = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddle = new MPTagThat.Core.WinControls.TTPanel();
      this.pictureBoxSpectrum = new System.Windows.Forms.PictureBox();
      this.pictureBoxTime = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).BeginInit();
      this.panelLeft.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayPause)).BeginInit();
      this.panelRight.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).BeginInit();
      this.SuspendLayout();
      // 
      // playListGrid
      // 
      this.playListGrid.AllowDrop = true;
      this.playListGrid.AllowUserToAddRows = false;
      this.playListGrid.AllowUserToDeleteRows = false;
      this.playListGrid.AllowUserToResizeRows = false;
      this.playListGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.playListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.playListGrid.ColumnHeadersVisible = false;
      this.playListGrid.Location = new System.Drawing.Point(16, 3);
      this.playListGrid.Name = "playListGrid";
      this.playListGrid.ReadOnly = true;
      this.playListGrid.RowHeadersVisible = false;
      this.playListGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.playListGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.playListGrid.Size = new System.Drawing.Size(117, 55);
      this.playListGrid.TabIndex = 7;
      this.playListGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseClick);
      this.playListGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseDoubleClick);
      this.playListGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragOver);
      this.playListGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragDrop);
      // 
      // lblTitle
      // 
      this.lblTitle.DisplayText = "Title";
      this.lblTitle.Location = new System.Drawing.Point(168, 3);
      this.lblTitle.MinimumSize = new System.Drawing.Size(100, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.ScrollPixelAmount = 10;
      this.lblTitle.Size = new System.Drawing.Size(120, 13);
      this.lblTitle.TabIndex = 8;
      // 
      // panelLeft
      // 
      this.panelLeft.Controls.Add(this.pictureBoxPlayPause);
      this.panelLeft.Controls.Add(this.buttonPrev);
      this.panelLeft.Controls.Add(this.buttonNext);
      this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.panelLeft.Location = new System.Drawing.Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new System.Drawing.Size(170, 60);
      this.panelLeft.TabIndex = 9;
      // 
      // pictureBoxPlayPause
      // 
      this.pictureBoxPlayPause.Image = global::MPTagThat.Properties.Resources.Play_btn;
      this.pictureBoxPlayPause.Location = new System.Drawing.Point(56, 6);
      this.pictureBoxPlayPause.Name = "pictureBoxPlayPause";
      this.pictureBoxPlayPause.Size = new System.Drawing.Size(49, 50);
      this.pictureBoxPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPlayPause.TabIndex = 7;
      this.pictureBoxPlayPause.TabStop = false;
      this.pictureBoxPlayPause.Click += new System.EventHandler(this.buttonPlay_Click);
      // 
      // buttonPrev
      // 
      this.buttonPrev.Image = global::MPTagThat.Properties.Resources.playprev;
      this.buttonPrev.Location = new System.Drawing.Point(15, 28);
      this.buttonPrev.Name = "buttonPrev";
      this.buttonPrev.Size = new System.Drawing.Size(25, 17);
      this.buttonPrev.TabIndex = 2;
      this.buttonPrev.UseVisualStyleBackColor = true;
      this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
      // 
      // buttonNext
      // 
      this.buttonNext.Image = global::MPTagThat.Properties.Resources.playnext;
      this.buttonNext.Location = new System.Drawing.Point(120, 28);
      this.buttonNext.Name = "buttonNext";
      this.buttonNext.Size = new System.Drawing.Size(25, 17);
      this.buttonNext.TabIndex = 6;
      this.buttonNext.UseVisualStyleBackColor = true;
      this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
      // 
      // panelRight
      // 
      this.panelRight.Controls.Add(this.playListGrid);
      this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelRight.Location = new System.Drawing.Point(611, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new System.Drawing.Size(121, 60);
      this.panelRight.TabIndex = 10;
      // 
      // panelMiddle
      // 
      this.panelMiddle.AutoSize = true;
      this.panelMiddle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panelMiddle.Controls.Add(this.lblTitle);
      this.panelMiddle.Controls.Add(this.pictureBoxSpectrum);
      this.panelMiddle.Controls.Add(this.pictureBoxTime);
      this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddle.Location = new System.Drawing.Point(170, 0);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(441, 60);
      this.panelMiddle.TabIndex = 11;
      // 
      // pictureBoxSpectrum
      // 
      this.pictureBoxSpectrum.BackColor = System.Drawing.Color.Black;
      this.pictureBoxSpectrum.Location = new System.Drawing.Point(5, 28);
      this.pictureBoxSpectrum.Name = "pictureBoxSpectrum";
      this.pictureBoxSpectrum.Size = new System.Drawing.Size(87, 28);
      this.pictureBoxSpectrum.TabIndex = 1;
      this.pictureBoxSpectrum.TabStop = false;
      this.pictureBoxSpectrum.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseDown);
      // 
      // pictureBoxTime
      // 
      this.pictureBoxTime.BackColor = System.Drawing.Color.Black;
      this.pictureBoxTime.Location = new System.Drawing.Point(5, 0);
      this.pictureBoxTime.Name = "pictureBoxTime";
      this.pictureBoxTime.Size = new System.Drawing.Size(87, 28);
      this.pictureBoxTime.TabIndex = 0;
      this.pictureBoxTime.TabStop = false;
      // 
      // PlayerControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panelMiddle);
      this.Controls.Add(this.panelRight);
      this.Controls.Add(this.panelLeft);
      this.Name = "PlayerControl";
      this.Size = new System.Drawing.Size(732, 60);
      this.Load += new System.EventHandler(this.OnLoad);
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).EndInit();
      this.panelLeft.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayPause)).EndInit();
      this.panelRight.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBoxTime;
    private System.Windows.Forms.PictureBox pictureBoxSpectrum;
    private System.Windows.Forms.Button buttonPrev;
    private System.Windows.Forms.Button buttonNext;
    private System.Windows.Forms.DataGridView playListGrid;
    private MPTagThat.Core.MarqueeLabel lblTitle;
    private MPTagThat.Core.WinControls.TTPanel panelLeft;
    private MPTagThat.Core.WinControls.TTPanel panelRight;
    private MPTagThat.Core.WinControls.TTPanel panelMiddle;
    private System.Windows.Forms.PictureBox pictureBoxPlayPause;
  }
}
