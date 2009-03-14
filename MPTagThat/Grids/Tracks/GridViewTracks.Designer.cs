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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.tracksGrid = new System.Windows.Forms.DataGridView();
      this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
      this.menuSavePlaylist = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.tracksGrid)).BeginInit();
      this.contextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // tracksGrid
      // 
      this.tracksGrid.AllowUserToAddRows = false;
      this.tracksGrid.AllowUserToDeleteRows = false;
      this.tracksGrid.AllowUserToOrderColumns = true;
      this.tracksGrid.AllowUserToResizeRows = false;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
      this.tracksGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
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
      // contextMenu
      // 
      this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem8,
            this.toolStripSeparator3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.menuSavePlaylist,
            this.toolStripSeparator4,
            this.toolStripMenuItem7});
      this.contextMenu.Name = "contextMenuTreeView";
      this.contextMenu.Size = new System.Drawing.Size(195, 236);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Image = global::MPTagThat.Properties.Resources.CopyHS;
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem1.Text = "Copy";
      this.toolStripMenuItem1.Click += new System.EventHandler(this.tracksGrid_Copy);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Image = global::MPTagThat.Properties.Resources.CutHS;
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem2.Text = "Cut";
      this.toolStripMenuItem2.Click += new System.EventHandler(this.tracksGrid_Cut);
      // 
      // toolStripMenuItem3
      // 
      this.toolStripMenuItem3.Enabled = false;
      this.toolStripMenuItem3.Image = global::MPTagThat.Properties.Resources.PasteHS;
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem3.Text = "Paste";
      this.toolStripMenuItem3.Click += new System.EventHandler(this.tracksGrid_Paste);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
      // 
      // toolStripMenuItem4
      // 
      this.toolStripMenuItem4.Image = global::MPTagThat.Properties.Resources.ribbon_AddBurnList_16x;
      this.toolStripMenuItem4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      this.toolStripMenuItem4.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem4.Text = "Add to Burnlist";
      this.toolStripMenuItem4.Click += new System.EventHandler(this.tracksGrid_AddToBurner);
      // 
      // toolStripMenuItem5
      // 
      this.toolStripMenuItem5.Image = global::MPTagThat.Properties.Resources.ribbon_AddConversionList_16x;
      this.toolStripMenuItem5.Name = "toolStripMenuItem5";
      this.toolStripMenuItem5.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem5.Text = "Add to Conversion List";
      this.toolStripMenuItem5.Click += new System.EventHandler(this.tracksGrid_AddToConvert);
      // 
      // toolStripMenuItem6
      // 
      this.toolStripMenuItem6.Image = global::MPTagThat.Properties.Resources.ribbon_AddPlayList_16x;
      this.toolStripMenuItem6.Name = "toolStripMenuItem6";
      this.toolStripMenuItem6.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem6.Text = "Add To Playlist";
      this.toolStripMenuItem6.Click += new System.EventHandler(this.tracksGrid_AddToPlayList);
      // 
      // menuSavePlaylist
      // 
      this.menuSavePlaylist.Name = "menuSavePlaylist";
      this.menuSavePlaylist.Size = new System.Drawing.Size(194, 22);
      this.menuSavePlaylist.Text = "Save as PlayList";
      this.menuSavePlaylist.Click += new System.EventHandler(this.tracksGrid_SaveAsPlayList);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(191, 6);
      // 
      // toolStripMenuItem7
      // 
      this.toolStripMenuItem7.Image = global::MPTagThat.Properties.Resources.ribbon_CoverArt_16x;
      this.toolStripMenuItem7.Name = "toolStripMenuItem7";
      this.toolStripMenuItem7.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem7.Text = "Create Folder Thumb";
      this.toolStripMenuItem7.Click += new System.EventHandler(this.tracksGrid_CreateFolderThumb);
      // 
      // toolStripMenuItem8
      // 
      this.toolStripMenuItem8.Image = global::MPTagThat.Properties.Resources.DeleteHS;
      this.toolStripMenuItem8.Name = "toolStripMenuItem8";
      this.toolStripMenuItem8.Size = new System.Drawing.Size(194, 22);
      this.toolStripMenuItem8.Text = "Delete";
      this.toolStripMenuItem8.Click += new System.EventHandler(this.tracksGrid_Delete);
      // 
      // GridViewTracks
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tracksGrid);
      this.Name = "GridViewTracks";
      this.Size = new System.Drawing.Size(569, 524);
      ((System.ComponentModel.ISupportInitialize)(this.tracksGrid)).EndInit();
      this.contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView tracksGrid;
    private System.Windows.Forms.ContextMenuStrip contextMenu;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
    private System.Windows.Forms.ToolStripMenuItem menuSavePlaylist;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;

  }
}
