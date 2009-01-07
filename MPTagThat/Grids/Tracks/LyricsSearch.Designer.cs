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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dataGridViewLyrics = new System.Windows.Forms.DataGridView();
      this.btUpdate = new MPTagThat.Core.WinControls.MPTButton();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.lbStatus = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbFinished = new MPTagThat.Core.WinControls.MPTLabel();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLyrics)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridViewLyrics
      // 
      this.dataGridViewLyrics.AllowUserToAddRows = false;
      this.dataGridViewLyrics.AllowUserToDeleteRows = false;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
      this.dataGridViewLyrics.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridViewLyrics.BackgroundColor = System.Drawing.Color.White;
      this.dataGridViewLyrics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewLyrics.Location = new System.Drawing.Point(30, 54);
      this.dataGridViewLyrics.Name = "dataGridViewLyrics";
      this.dataGridViewLyrics.RowHeadersVisible = false;
      this.dataGridViewLyrics.Size = new System.Drawing.Size(828, 448);
      this.dataGridViewLyrics.TabIndex = 0;
      // 
      // btUpdate
      // 
      this.btUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btUpdate.Localisation = "btUpdate";
      this.btUpdate.LocalisationContext = "LyricsSearch";
      this.btUpdate.Location = new System.Drawing.Point(255, 553);
      this.btUpdate.Name = "btUpdate";
      this.btUpdate.Size = new System.Drawing.Size(120, 40);
      this.btUpdate.TabIndex = 1;
      this.btUpdate.Text = "Update";
      this.btUpdate.UseVisualStyleBackColor = true;
      this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
      // 
      // btCancel
      // 
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Localisation = "btCancel";
      this.btCancel.LocalisationContext = "LyricsSearch";
      this.btCancel.Location = new System.Drawing.Point(499, 552);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(120, 40);
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
      this.lbStatus.Location = new System.Drawing.Point(27, 19);
      this.lbStatus.Name = "lbStatus";
      this.lbStatus.Size = new System.Drawing.Size(172, 13);
      this.lbStatus.TabIndex = 3;
      this.lbStatus.Text = "Searching for Lyrics ... Please Wait";
      // 
      // lbFinished
      // 
      this.lbFinished.AutoSize = true;
      this.lbFinished.Localisation = "Finished";
      this.lbFinished.LocalisationContext = "LyricsSearch";
      this.lbFinished.Location = new System.Drawing.Point(27, 520);
      this.lbFinished.Name = "lbFinished";
      this.lbFinished.Size = new System.Drawing.Size(265, 13);
      this.lbFinished.TabIndex = 4;
      this.lbFinished.Text = "Search Finished. Unselect tracks with incorrect  Lyrics.";
      // 
      // LyricsSearch
      // 
      this.AcceptButton = this.btUpdate;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btCancel;
      this.ClientSize = new System.Drawing.Size(894, 614);
      this.Controls.Add(this.lbFinished);
      this.Controls.Add(this.lbStatus);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btUpdate);
      this.Controls.Add(this.dataGridViewLyrics);
      this.Name = "LyricsSearch";
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
  }
}