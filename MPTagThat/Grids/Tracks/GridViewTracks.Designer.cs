namespace MPTagThat.GridView
{
  partial class GridViewTracks
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.tracksGrid = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.tracksGrid)).BeginInit();
      this.SuspendLayout();
      // 
      // tracksGrid
      // 
      this.tracksGrid.AllowUserToAddRows = false;
      this.tracksGrid.AllowUserToDeleteRows = false;
      this.tracksGrid.AllowUserToOrderColumns = true;
      this.tracksGrid.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
      this.tracksGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.tracksGrid.BackgroundColor = System.Drawing.Color.White;
      this.tracksGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
      this.tracksGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tracksGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tracksGrid.Location = new System.Drawing.Point(0, 0);
      this.tracksGrid.Name = "tracksGrid";
      this.tracksGrid.RowHeadersVisible = false;
      this.tracksGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.tracksGrid.Size = new System.Drawing.Size(569, 524);
      this.tracksGrid.TabIndex = 0;
      this.tracksGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tracksGrid_MouseClick);
      this.tracksGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tracksGrid_MouseDoubleClick);
      // 
      // GridViewTracks
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tracksGrid);
      this.Name = "GridViewTracks";
      this.Size = new System.Drawing.Size(569, 524);
      ((System.ComponentModel.ISupportInitialize)(this.tracksGrid)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView tracksGrid;

  }
}
