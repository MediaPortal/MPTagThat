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
      this.buttonPause = new System.Windows.Forms.Button();
      this.buttonStop = new System.Windows.Forms.Button();
      this.playListGrid = new System.Windows.Forms.DataGridView();
      this.lblTitle = new MPTagThat.Core.MarqueeLabel();
      this.buttonNext = new System.Windows.Forms.Button();
      this.buttonPlay = new System.Windows.Forms.Button();
      this.buttonPrev = new System.Windows.Forms.Button();
      this.pictureBoxSpectrum = new System.Windows.Forms.PictureBox();
      this.pictureBoxTime = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonPause
      // 
      this.buttonPause.Image = global::MPTagThat.Properties.Resources.playpause;
      this.buttonPause.Location = new System.Drawing.Point(50, 63);
      this.buttonPause.Name = "buttonPause";
      this.buttonPause.Size = new System.Drawing.Size(25, 17);
      this.buttonPause.TabIndex = 4;
      this.buttonPause.UseVisualStyleBackColor = true;
      this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
      // 
      // buttonStop
      // 
      this.buttonStop.Image = global::MPTagThat.Properties.Resources.playstop;
      this.buttonStop.Location = new System.Drawing.Point(75, 63);
      this.buttonStop.Name = "buttonStop";
      this.buttonStop.Size = new System.Drawing.Size(25, 17);
      this.buttonStop.TabIndex = 5;
      this.buttonStop.UseVisualStyleBackColor = true;
      this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
      // 
      // playListGrid
      // 
      this.playListGrid.AllowDrop = true;
      this.playListGrid.AllowUserToAddRows = false;
      this.playListGrid.AllowUserToDeleteRows = false;
      this.playListGrid.AllowUserToResizeRows = false;
      this.playListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.playListGrid.ColumnHeadersVisible = false;
      this.playListGrid.Location = new System.Drawing.Point(0, 85);
      this.playListGrid.Name = "playListGrid";
      this.playListGrid.ReadOnly = true;
      this.playListGrid.RowHeadersVisible = false;
      this.playListGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.playListGrid.Size = new System.Drawing.Size(198, 192);
      this.playListGrid.TabIndex = 7;
      this.playListGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseClick);
      this.playListGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseDoubleClick);
      this.playListGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragOver);
      this.playListGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragDrop);
      // 
      // lblTitle
      // 
      this.lblTitle.DisplayText = "Title";
      this.lblTitle.Location = new System.Drawing.Point(94, 4);
      this.lblTitle.MinimumSize = new System.Drawing.Size(100, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.ScrollPixelAmount = 10;
      this.lblTitle.Size = new System.Drawing.Size(100, 13);
      this.lblTitle.TabIndex = 8;
      // 
      // buttonNext
      // 
      this.buttonNext.Image = global::MPTagThat.Properties.Resources.playnext;
      this.buttonNext.Location = new System.Drawing.Point(101, 63);
      this.buttonNext.Name = "buttonNext";
      this.buttonNext.Size = new System.Drawing.Size(25, 17);
      this.buttonNext.TabIndex = 6;
      this.buttonNext.UseVisualStyleBackColor = true;
      this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
      // 
      // buttonPlay
      // 
      this.buttonPlay.Image = global::MPTagThat.Properties.Resources.play;
      this.buttonPlay.Location = new System.Drawing.Point(25, 63);
      this.buttonPlay.Name = "buttonPlay";
      this.buttonPlay.Size = new System.Drawing.Size(25, 17);
      this.buttonPlay.TabIndex = 3;
      this.buttonPlay.UseVisualStyleBackColor = true;
      this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
      // 
      // buttonPrev
      // 
      this.buttonPrev.Image = global::MPTagThat.Properties.Resources.playprev;
      this.buttonPrev.Location = new System.Drawing.Point(0, 63);
      this.buttonPrev.Name = "buttonPrev";
      this.buttonPrev.Size = new System.Drawing.Size(25, 17);
      this.buttonPrev.TabIndex = 2;
      this.buttonPrev.UseVisualStyleBackColor = true;
      this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
      // 
      // pictureBoxSpectrum
      // 
      this.pictureBoxSpectrum.BackColor = System.Drawing.Color.Black;
      this.pictureBoxSpectrum.Location = new System.Drawing.Point(0, 28);
      this.pictureBoxSpectrum.Name = "pictureBoxSpectrum";
      this.pictureBoxSpectrum.Size = new System.Drawing.Size(87, 28);
      this.pictureBoxSpectrum.TabIndex = 1;
      this.pictureBoxSpectrum.TabStop = false;
      this.pictureBoxSpectrum.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseDown);
      // 
      // pictureBoxTime
      // 
      this.pictureBoxTime.BackColor = System.Drawing.Color.Black;
      this.pictureBoxTime.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxTime.Name = "pictureBoxTime";
      this.pictureBoxTime.Size = new System.Drawing.Size(87, 28);
      this.pictureBoxTime.TabIndex = 0;
      this.pictureBoxTime.TabStop = false;
      // 
      // PlayerControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lblTitle);
      this.Controls.Add(this.playListGrid);
      this.Controls.Add(this.buttonNext);
      this.Controls.Add(this.buttonStop);
      this.Controls.Add(this.buttonPause);
      this.Controls.Add(this.buttonPlay);
      this.Controls.Add(this.buttonPrev);
      this.Controls.Add(this.pictureBoxSpectrum);
      this.Controls.Add(this.pictureBoxTime);
      this.Name = "PlayerControl";
      this.Size = new System.Drawing.Size(198, 280);
      this.Load += new System.EventHandler(this.OnLoad);
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBoxTime;
    private System.Windows.Forms.PictureBox pictureBoxSpectrum;
    private System.Windows.Forms.Button buttonPrev;
    private System.Windows.Forms.Button buttonPlay;
    private System.Windows.Forms.Button buttonPause;
    private System.Windows.Forms.Button buttonStop;
    private System.Windows.Forms.Button buttonNext;
    private System.Windows.Forms.DataGridView playListGrid;
    private MPTagThat.Core.MarqueeLabel lblTitle;
  }
}
