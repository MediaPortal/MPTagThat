namespace MPTagThat.Player
{
  partial class PlayList
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
      this.playListGrid = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).BeginInit();
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
      this.playListGrid.Location = new System.Drawing.Point(0, 3);
      this.playListGrid.Name = "playListGrid";
      this.playListGrid.ReadOnly = true;
      this.playListGrid.RowHeadersVisible = false;
      this.playListGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.playListGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.playListGrid.Size = new System.Drawing.Size(190, 579);
      this.playListGrid.TabIndex = 8;
      this.playListGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseClick);
      this.playListGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseDoubleClick);
      this.playListGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragOver);
      this.playListGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragDrop);
      // 
      // PlayList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(190, 580);
      this.ControlBox = false;
      this.Controls.Add(this.playListGrid);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PlayList";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Load += new System.EventHandler(this.PlayList_Load);
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView playListGrid;
  }
}