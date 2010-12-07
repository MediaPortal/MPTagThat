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
      this.components = new System.ComponentModel.Container();
      this.PanelBottom = new MPTagThat.Core.WinControls.MPTPanel();
      this.ckUseRelativePath = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.btPlayListSave = new MPTagThat.Core.WinControls.MPTButton();
      this.btPlaylistLoad = new MPTagThat.Core.WinControls.MPTButton();
      this.panelTop = new MPTagThat.Core.WinControls.MPTPanel();
      this.playListGrid = new System.Windows.Forms.DataGridView();
      this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuClear = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.menuLoad = new System.Windows.Forms.ToolStripMenuItem();
      this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
      this.PanelBottom.SuspendLayout();
      this.panelTop.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).BeginInit();
      this.contextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // PanelBottom
      // 
      this.PanelBottom.Controls.Add(this.ckUseRelativePath);
      this.PanelBottom.Controls.Add(this.btPlayListSave);
      this.PanelBottom.Controls.Add(this.btPlaylistLoad);
      this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.PanelBottom.Location = new System.Drawing.Point(0, 507);
      this.PanelBottom.Name = "PanelBottom";
      this.PanelBottom.Size = new System.Drawing.Size(178, 61);
      this.PanelBottom.TabIndex = 10;
      // 
      // ckUseRelativePath
      // 
      this.ckUseRelativePath.AutoSize = true;
      this.ckUseRelativePath.Checked = true;
      this.ckUseRelativePath.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckUseRelativePath.Localisation = "UseRelativePath";
      this.ckUseRelativePath.LocalisationContext = "player";
      this.ckUseRelativePath.Location = new System.Drawing.Point(5, 41);
      this.ckUseRelativePath.Name = "ckUseRelativePath";
      this.ckUseRelativePath.Size = new System.Drawing.Size(106, 17);
      this.ckUseRelativePath.TabIndex = 2;
      this.ckUseRelativePath.Text = "Use relative path";
      this.ckUseRelativePath.UseVisualStyleBackColor = true;
      // 
      // btPlayListSave
      // 
      this.btPlayListSave.Localisation = "PlayListSave";
      this.btPlayListSave.LocalisationContext = "Player";
      this.btPlayListSave.Location = new System.Drawing.Point(93, 12);
      this.btPlayListSave.Name = "btPlayListSave";
      this.btPlayListSave.Size = new System.Drawing.Size(90, 23);
      this.btPlayListSave.TabIndex = 1;
      this.btPlayListSave.Text = "Save";
      this.btPlayListSave.UseVisualStyleBackColor = true;
      this.btPlayListSave.Click += new System.EventHandler(this.btPlayListSave_Click);
      // 
      // btPlaylistLoad
      // 
      this.btPlaylistLoad.Localisation = "PlayListLoad";
      this.btPlaylistLoad.LocalisationContext = "Player";
      this.btPlaylistLoad.Location = new System.Drawing.Point(2, 12);
      this.btPlaylistLoad.Name = "btPlaylistLoad";
      this.btPlaylistLoad.Size = new System.Drawing.Size(90, 23);
      this.btPlaylistLoad.TabIndex = 0;
      this.btPlaylistLoad.Text = "Load";
      this.btPlaylistLoad.UseVisualStyleBackColor = true;
      this.btPlaylistLoad.Click += new System.EventHandler(this.btPlaylistLoad_Click);
      // 
      // panelTop
      // 
      this.panelTop.Controls.Add(this.playListGrid);
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(178, 568);
      this.panelTop.TabIndex = 9;
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
      this.playListGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.playListGrid.Location = new System.Drawing.Point(0, 0);
      this.playListGrid.Name = "playListGrid";
      this.playListGrid.ReadOnly = true;
      this.playListGrid.RowHeadersVisible = false;
      this.playListGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.playListGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.playListGrid.Size = new System.Drawing.Size(178, 568);
      this.playListGrid.TabIndex = 8;
      this.playListGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseClick);
      this.playListGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.playListGrid_MouseDoubleClick);
      this.playListGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragOver);
      this.playListGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.playListGrid_DragDrop);
      // 
      // contextMenu
      // 
      this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuClear,
            this.toolStripSeparator1,
            this.menuLoad,
            this.menuSave});
      this.contextMenu.Name = "contextMenu";
      this.contextMenu.Size = new System.Drawing.Size(142, 76);
      // 
      // menuClear
      // 
      this.menuClear.Name = "menuClear";
      this.menuClear.Size = new System.Drawing.Size(141, 22);
      this.menuClear.Text = "Clear Playlist";
      this.menuClear.Click += new System.EventHandler(this.playListGrid_ClearPlayList);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(138, 6);
      // 
      // menuLoad
      // 
      this.menuLoad.Name = "menuLoad";
      this.menuLoad.Size = new System.Drawing.Size(141, 22);
      this.menuLoad.Text = "Load Playlist";
      this.menuLoad.Click += new System.EventHandler(this.btPlaylistLoad_Click);
      // 
      // menuSave
      // 
      this.menuSave.Name = "menuSave";
      this.menuSave.Size = new System.Drawing.Size(141, 22);
      this.menuSave.Text = "Save Playlist";
      this.menuSave.Click += new System.EventHandler(this.btPlayListSave_Click);
      // 
      // PlayList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(178, 568);
      this.ControlBox = false;
      this.Controls.Add(this.PanelBottom);
      this.Controls.Add(this.panelTop);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PlayList";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Load += new System.EventHandler(this.PlayList_Load);
      this.PanelBottom.ResumeLayout(false);
      this.PanelBottom.PerformLayout();
      this.panelTop.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.playListGrid)).EndInit();
      this.contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView playListGrid;
    private MPTagThat.Core.WinControls.MPTPanel panelTop;
    private MPTagThat.Core.WinControls.MPTPanel PanelBottom;
    private MPTagThat.Core.WinControls.MPTButton btPlayListSave;
    private MPTagThat.Core.WinControls.MPTButton btPlaylistLoad;
    private System.Windows.Forms.ContextMenuStrip contextMenu;
    private System.Windows.Forms.ToolStripMenuItem menuClear;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem menuLoad;
    private System.Windows.Forms.ToolStripMenuItem menuSave;
    private MPTagThat.Core.WinControls.MPTCheckBox ckUseRelativePath;
  }
}