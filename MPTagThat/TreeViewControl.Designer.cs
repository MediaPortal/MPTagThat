namespace MPTagThat
{
  partial class TreeViewControl
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreeViewControl));
      this.treeViewFolderBrowser = new Raccoom.Windows.Forms.TreeViewFolderBrowser();
      this.panelLeftBottom = new MPTagThat.Core.WinControls.TTPanel();
      this.optionsPanelLeft = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.checkBoxRecursive = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.btnRefreshFolder = new MPTagThat.Core.WinControls.MPTButton();
      this.treeViewPanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.treeViewPanelBottom = new MPTagThat.Core.WinControls.TTPanel();
      this.treeViewPanelTop = new MPTagThat.Core.WinControls.TTPanel();
      this.contextMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
      this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
      this.panelLeftBottom.SuspendLayout();
      this.optionsPanelLeft.SuspendLayout();
      this.treeViewPanel.SuspendLayout();
      this.treeViewPanelBottom.SuspendLayout();
      this.contextMenuTreeView.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeViewFolderBrowser
      // 
      this.treeViewFolderBrowser.AllowDrop = true;
      this.treeViewFolderBrowser.BackColor = System.Drawing.SystemColors.Window;
      this.treeViewFolderBrowser.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.treeViewFolderBrowser.DataSource = null;
      this.treeViewFolderBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewFolderBrowser.DriveTypes = ((Raccoom.Windows.Forms.DriveTypes)((((Raccoom.Windows.Forms.DriveTypes.NoRootDirectory | Raccoom.Windows.Forms.DriveTypes.RemovableDisk)
                  | Raccoom.Windows.Forms.DriveTypes.LocalDisk)
                  | Raccoom.Windows.Forms.DriveTypes.NetworkDrive)));
      this.treeViewFolderBrowser.HideSelection = false;
      this.treeViewFolderBrowser.Location = new System.Drawing.Point(0, 0);
      this.treeViewFolderBrowser.Name = "treeViewFolderBrowser";
      this.treeViewFolderBrowser.SelectedDirectories = ((System.Collections.Specialized.StringCollection)(resources.GetObject("treeViewFolderBrowser.SelectedDirectories")));
      this.treeViewFolderBrowser.Size = new System.Drawing.Size(180, 411);
      this.treeViewFolderBrowser.TabIndex = 1;
      this.treeViewFolderBrowser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewFolderBrowser_MouseUp);
      this.treeViewFolderBrowser.Enter += new System.EventHandler(this.treeViewFolderBrowser_Enter);
      this.treeViewFolderBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFolderBrowser_AfterSelect);
      this.treeViewFolderBrowser.Leave += new System.EventHandler(this.treeViewFolderBrowser_Leave);
      this.treeViewFolderBrowser.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewFolderBrowser_BeforeSelect);
      this.treeViewFolderBrowser.Click += new System.EventHandler(this.treeViewFolderBrowser_Click);
      // 
      // panelLeftBottom
      // 
      this.panelLeftBottom.Controls.Add(this.optionsPanelLeft);
      this.panelLeftBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelLeftBottom.Location = new System.Drawing.Point(0, 435);
      this.panelLeftBottom.Name = "panelLeftBottom";
      this.panelLeftBottom.Size = new System.Drawing.Size(180, 141);
      this.panelLeftBottom.TabIndex = 6;
      // 
      // optionsPanelLeft
      // 
      this.optionsPanelLeft.AnimationStep = 30;
      this.optionsPanelLeft.BorderColor = System.Drawing.Color.Transparent;
      this.optionsPanelLeft.CaptionBrush = Stepi.UI.BrushType.Solid;
      this.optionsPanelLeft.CaptionColorOne = System.Drawing.SystemColors.GradientActiveCaption;
      this.optionsPanelLeft.CaptionColorTwo = System.Drawing.SystemColors.GradientInactiveCaption;
      this.optionsPanelLeft.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.optionsPanelLeft.CaptionSize = 24;
      this.optionsPanelLeft.CaptionText = "Options";
      this.optionsPanelLeft.CaptionTextColor = System.Drawing.Color.Black;
      this.optionsPanelLeft.Controls.Add(this.checkBoxRecursive);
      this.optionsPanelLeft.Controls.Add(this.btnRefreshFolder);
      this.optionsPanelLeft.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.optionsPanelLeft.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.optionsPanelLeft.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.optionsPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
      this.optionsPanelLeft.Location = new System.Drawing.Point(0, 0);
      this.optionsPanelLeft.Name = "optionsPanelLeft";
      this.optionsPanelLeft.Size = new System.Drawing.Size(180, 141);
      this.optionsPanelLeft.TabIndex = 2;
      // 
      // checkBoxRecursive
      // 
      this.checkBoxRecursive.AutoSize = true;
      this.checkBoxRecursive.Localisation = "ScanSubfolder";
      this.checkBoxRecursive.LocalisationContext = "main";
      this.checkBoxRecursive.Location = new System.Drawing.Point(8, 43);
      this.checkBoxRecursive.MaximumSize = new System.Drawing.Size(250, 0);
      this.checkBoxRecursive.Name = "checkBoxRecursive";
      this.checkBoxRecursive.Size = new System.Drawing.Size(132, 17);
      this.checkBoxRecursive.TabIndex = 3;
      this.checkBoxRecursive.Text = "Scan all subdirectories";
      this.checkBoxRecursive.UseVisualStyleBackColor = true;
      // 
      // btnRefreshFolder
      // 
      this.btnRefreshFolder.AutoSize = true;
      this.btnRefreshFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.btnRefreshFolder.Image = global::MPTagThat.Properties.Resources.RefreshDocViewHS;
      this.btnRefreshFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnRefreshFolder.Localisation = "ButtonRefreshFolder";
      this.btnRefreshFolder.LocalisationContext = "main";
      this.btnRefreshFolder.Location = new System.Drawing.Point(5, 66);
      this.btnRefreshFolder.MaximumSize = new System.Drawing.Size(220, 0);
      this.btnRefreshFolder.Name = "btnRefreshFolder";
      this.btnRefreshFolder.Size = new System.Drawing.Size(128, 23);
      this.btnRefreshFolder.TabIndex = 4;
      this.btnRefreshFolder.Text = "Refresh Folder View";
      this.btnRefreshFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnRefreshFolder.UseVisualStyleBackColor = true;
      this.btnRefreshFolder.Click += new System.EventHandler(this.btnRefreshFolder_Click);
      // 
      // treeViewPanel
      // 
      this.treeViewPanel.AnimationStep = 30;
      this.treeViewPanel.BorderColor = System.Drawing.Color.Transparent;
      this.treeViewPanel.CaptionBrush = Stepi.UI.BrushType.Solid;
      this.treeViewPanel.CaptionColorOne = System.Drawing.SystemColors.GradientActiveCaption;
      this.treeViewPanel.CaptionColorTwo = System.Drawing.SystemColors.GradientInactiveCaption;
      this.treeViewPanel.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.treeViewPanel.CaptionSize = 24;
      this.treeViewPanel.CaptionText = "Folders";
      this.treeViewPanel.CaptionTextColor = System.Drawing.Color.Black;
      this.treeViewPanel.Controls.Add(this.treeViewPanelBottom);
      this.treeViewPanel.Controls.Add(this.treeViewPanelTop);
      this.treeViewPanel.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.treeViewPanel.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.treeViewPanel.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.treeViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewPanel.Location = new System.Drawing.Point(0, 0);
      this.treeViewPanel.Name = "treeViewPanel";
      this.treeViewPanel.Size = new System.Drawing.Size(180, 435);
      this.treeViewPanel.TabIndex = 7;
      // 
      // treeViewPanelBottom
      // 
      this.treeViewPanelBottom.Controls.Add(this.treeViewFolderBrowser);
      this.treeViewPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewPanelBottom.Location = new System.Drawing.Point(0, 24);
      this.treeViewPanelBottom.Name = "treeViewPanelBottom";
      this.treeViewPanelBottom.Size = new System.Drawing.Size(180, 411);
      this.treeViewPanelBottom.TabIndex = 2;
      // 
      // treeViewPanelTop
      // 
      this.treeViewPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.treeViewPanelTop.Location = new System.Drawing.Point(0, 0);
      this.treeViewPanelTop.Name = "treeViewPanelTop";
      this.treeViewPanelTop.Size = new System.Drawing.Size(180, 24);
      this.treeViewPanelTop.TabIndex = 1;
      // 
      // contextMenuTreeView
      // 
      this.contextMenuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRefresh,
            this.menuDelete});
      this.contextMenuTreeView.Name = "contextMenuTreeView";
      this.contextMenuTreeView.Size = new System.Drawing.Size(114, 48);
      // 
      // menuRefresh
      // 
      this.menuRefresh.Image = global::MPTagThat.Properties.Resources.RefreshDocViewHS;
      this.menuRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.menuRefresh.Name = "menuRefresh";
      this.menuRefresh.Size = new System.Drawing.Size(152, 22);
      this.menuRefresh.Text = "Refresh";
      this.menuRefresh.Click += new System.EventHandler(this.contextMenuTreeViewRefresh_Click);
      // 
      // menuDelete
      // 
      this.menuDelete.Image = ((System.Drawing.Image)(resources.GetObject("menuDelete.Image")));
      this.menuDelete.Name = "menuDelete";
      this.menuDelete.Size = new System.Drawing.Size(152, 22);
      this.menuDelete.Text = "Delete";
      this.menuDelete.Click += new System.EventHandler(this.contextMenuTreeViewDelete_Click);
      // 
      // TreeViewControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.treeViewPanel);
      this.Controls.Add(this.panelLeftBottom);
      this.Name = "TreeViewControl";
      this.Size = new System.Drawing.Size(180, 576);
      this.panelLeftBottom.ResumeLayout(false);
      this.optionsPanelLeft.ResumeLayout(false);
      this.optionsPanelLeft.PerformLayout();
      this.treeViewPanel.ResumeLayout(false);
      this.treeViewPanelBottom.ResumeLayout(false);
      this.contextMenuTreeView.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private Raccoom.Windows.Forms.TreeViewFolderBrowser treeViewFolderBrowser;
    private MPTagThat.Core.WinControls.TTPanel panelLeftBottom;
    private MPTagThat.Core.WinControls.TTExtendedPanel optionsPanelLeft;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxRecursive;
    private MPTagThat.Core.WinControls.MPTButton btnRefreshFolder;
    private MPTagThat.Core.WinControls.TTExtendedPanel treeViewPanel;
    private MPTagThat.Core.WinControls.TTPanel treeViewPanelBottom;
    private MPTagThat.Core.WinControls.TTPanel treeViewPanelTop;
    private System.Windows.Forms.ContextMenuStrip contextMenuTreeView;
    private System.Windows.Forms.ToolStripMenuItem menuRefresh;
    private System.Windows.Forms.ToolStripMenuItem menuDelete;
  }
}
