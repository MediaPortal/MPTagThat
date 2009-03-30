namespace MPTagThat.GridView
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
      SaveSettings();
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dataGridViewLyrics = new System.Windows.Forms.DataGridView();
      this.btUpdate = new MPTagThat.Core.WinControls.MPTButton();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.lbStatus = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbFinished = new MPTagThat.Core.WinControls.MPTLabel();
      this.roundRectShape1 = new Telerik.WinControls.RoundRectShape();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLyrics)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridViewLyrics
      // 
      this.dataGridViewLyrics.AllowUserToAddRows = false;
      this.dataGridViewLyrics.AllowUserToDeleteRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
      this.dataGridViewLyrics.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewLyrics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridViewLyrics.BackgroundColor = System.Drawing.Color.White;
      this.dataGridViewLyrics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewLyrics.Location = new System.Drawing.Point(15, 34);
      this.dataGridViewLyrics.Name = "dataGridViewLyrics";
      this.dataGridViewLyrics.RowHeadersVisible = false;
      this.dataGridViewLyrics.Size = new System.Drawing.Size(858, 448);
      this.dataGridViewLyrics.TabIndex = 0;
      // 
      // btUpdate
      // 
      this.btUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btUpdate.Localisation = "Update";
      this.btUpdate.LocalisationContext = "LyricsSearch";
      this.btUpdate.Location = new System.Drawing.Point(667, 494);
      this.btUpdate.Name = "btUpdate";
      this.btUpdate.Size = new System.Drawing.Size(100, 23);
      this.btUpdate.TabIndex = 1;
      this.btUpdate.Text = "Update";
      this.btUpdate.UseVisualStyleBackColor = true;
      this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
      // 
      // btCancel
      // 
      this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "LyricsSearch";
      this.btCancel.Location = new System.Drawing.Point(773, 494);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(100, 23);
      this.btCancel.TabIndex = 2;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      // 
      // lbStatus
      // 
      this.lbStatus.AutoSize = true;
      this.lbStatus.Localisation = "Status";
      this.lbStatus.LocalisationContext = "LyricsSearch";
      this.lbStatus.Location = new System.Drawing.Point(14, 11);
      this.lbStatus.Name = "lbStatus";
      this.lbStatus.Size = new System.Drawing.Size(171, 13);
      this.lbStatus.TabIndex = 3;
      this.lbStatus.Text = "Searching for Lyrics - please wait...";
      // 
      // lbFinished
      // 
      this.lbFinished.AutoSize = true;
      this.lbFinished.Localisation = "Finished";
      this.lbFinished.LocalisationContext = "LyricsSearch";
      this.lbFinished.Location = new System.Drawing.Point(12, 494);
      this.lbFinished.Name = "lbFinished";
      this.lbFinished.Size = new System.Drawing.Size(295, 13);
      this.lbFinished.TabIndex = 4;
      this.lbFinished.Text = "Search Finished. Please unselect tracks with incorrect Lyrics.";
      // 
      // LyricsSearch
      // 
      this.AcceptButton = this.btUpdate;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btCancel;
      this.ClientSize = new System.Drawing.Size(894, 528);
      this.Controls.Add(this.lbFinished);
      this.Controls.Add(this.lbStatus);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btUpdate);
      this.Controls.Add(this.dataGridViewLyrics);
      this.DoubleBuffered = true;
      this.Name = "LyricsSearch";
      this.Shape = this.roundRectShape1;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "LyricsSearch";
      this.Load += new System.EventHandler(this.OnLoad);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLyrics)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridViewLyrics;
    private MPTagThat.Core.WinControls.MPTButton btUpdate;
    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private MPTagThat.Core.WinControls.MPTLabel lbStatus;
    private MPTagThat.Core.WinControls.MPTLabel lbFinished;
    private Telerik.WinControls.RoundRectShape roundRectShape1;
  }
}