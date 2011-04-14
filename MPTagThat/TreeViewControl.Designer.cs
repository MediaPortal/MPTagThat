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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.contextMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
      this.menuCut = new System.Windows.Forms.ToolStripMenuItem();
      this.menuPaste = new System.Windows.Forms.ToolStripMenuItem();
      this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
      this.contextMenuStripFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuInsertFilter = new System.Windows.Forms.ToolStripMenuItem();
      this.menuDeleteFilter = new System.Windows.Forms.ToolStripMenuItem();
      this.tabControlTreeView = new Elegant.Ui.TabControl();
      this.tabPageViews = new MPTagThat.Core.WinControls.MPTTabPage();
      this.treeViewPanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.treeViewPanelBottom = new MPTagThat.Core.WinControls.MPTPanel();
      this.treeViewFolderBrowser = new Raccoom.Windows.Forms.TreeViewFolderBrowser();
      this.treeViewPanelTop = new MPTagThat.Core.WinControls.MPTPanel();
      this.tabPageFilter = new MPTagThat.Core.WinControls.MPTTabPage();
      this.dataGridViewTagFilter = new System.Windows.Forms.DataGridView();
      this.TagFilterField = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.TagFilterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TagFilterOperator = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.lbFileMask = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbFileMask = new System.Windows.Forms.TextBox();
      this.cbListFormats = new MPTagThat.Core.WinControls.MPTComboBox();
      this.ckUseTagFilter = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.panelLeftBottom = new MPTagThat.Core.WinControls.MPTPanel();
      this.optionsPanelLeft = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.btnRefreshFolder = new MPTagThat.Core.WinControls.MPTButton();
      this.cbDataProvider = new MPTagThat.Core.WinControls.MPTComboBox();
      this.checkBoxRecursive = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.contextMenuTreeView.SuspendLayout();
      this.contextMenuStripFilter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tabControlTreeView)).BeginInit();
      this.tabPageViews.SuspendLayout();
      this.treeViewPanel.SuspendLayout();
      this.treeViewPanelBottom.SuspendLayout();
      this.tabPageFilter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTagFilter)).BeginInit();
      this.panelLeftBottom.SuspendLayout();
      this.optionsPanelLeft.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuTreeView
      // 
      this.contextMenuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopy,
            this.menuCut,
            this.menuPaste,
            this.menuDelete,
            this.toolStripSeparator1,
            this.menuRefresh});
      this.contextMenuTreeView.Name = "contextMenuTreeView";
      this.contextMenuTreeView.Size = new System.Drawing.Size(114, 120);
      // 
      // menuCopy
      // 
      this.menuCopy.Image = global::MPTagThat.Properties.Resources.CopyHS;
      this.menuCopy.Name = "menuCopy";
      this.menuCopy.Size = new System.Drawing.Size(113, 22);
      this.menuCopy.Text = "Copy";
      this.menuCopy.Click += new System.EventHandler(this.contextMenuTreeViewCopy_Click);
      // 
      // menuCut
      // 
      this.menuCut.Image = global::MPTagThat.Properties.Resources.CutHS;
      this.menuCut.Name = "menuCut";
      this.menuCut.Size = new System.Drawing.Size(113, 22);
      this.menuCut.Text = "Cut";
      this.menuCut.Click += new System.EventHandler(this.contextMenuTreeViewCut_Click);
      // 
      // menuPaste
      // 
      this.menuPaste.Enabled = false;
      this.menuPaste.Image = global::MPTagThat.Properties.Resources.PasteHS;
      this.menuPaste.Name = "menuPaste";
      this.menuPaste.Size = new System.Drawing.Size(113, 22);
      this.menuPaste.Text = "Paste";
      this.menuPaste.Click += new System.EventHandler(this.contextMenuTreeViewPaste_Click);
      // 
      // menuDelete
      // 
      this.menuDelete.Image = ((System.Drawing.Image)(resources.GetObject("menuDelete.Image")));
      this.menuDelete.Name = "menuDelete";
      this.menuDelete.Size = new System.Drawing.Size(113, 22);
      this.menuDelete.Text = "Delete";
      this.menuDelete.Click += new System.EventHandler(this.contextMenuTreeViewDelete_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
      // 
      // menuRefresh
      // 
      this.menuRefresh.Image = global::MPTagThat.Properties.Resources.RefreshDocViewHS;
      this.menuRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.menuRefresh.Name = "menuRefresh";
      this.menuRefresh.Size = new System.Drawing.Size(113, 22);
      this.menuRefresh.Text = "Refresh";
      this.menuRefresh.Click += new System.EventHandler(this.contextMenuTreeViewRefresh_Click);
      // 
      // contextMenuStripFilter
      // 
      this.contextMenuStripFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuInsertFilter,
            this.menuDeleteFilter});
      this.contextMenuStripFilter.Name = "contextMenuStripFilter";
      this.contextMenuStripFilter.Size = new System.Drawing.Size(137, 48);
      // 
      // menuInsertFilter
      // 
      this.menuInsertFilter.Name = "menuInsertFilter";
      this.menuInsertFilter.Size = new System.Drawing.Size(136, 22);
      this.menuInsertFilter.Text = "Insert Filter";
      this.menuInsertFilter.Click += new System.EventHandler(this.menuInsertFilter_Click);
      // 
      // menuDeleteFilter
      // 
      this.menuDeleteFilter.Name = "menuDeleteFilter";
      this.menuDeleteFilter.Size = new System.Drawing.Size(136, 22);
      this.menuDeleteFilter.Text = "Delete Filter";
      this.menuDeleteFilter.Click += new System.EventHandler(this.menuDeleteFilter_Click);
      // 
      // tabControlTreeView
      // 
      this.tabControlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlTreeView.Location = new System.Drawing.Point(0, 0);
      this.tabControlTreeView.Name = "tabControlTreeView";
      this.tabControlTreeView.SelectedTabPage = this.tabPageFilter;
      this.tabControlTreeView.Size = new System.Drawing.Size(218, 535);
      this.tabControlTreeView.TabIndex = 8;
      this.tabControlTreeView.TabPages.AddRange(new Elegant.Ui.TabPage[] {
            this.tabPageViews,
            this.tabPageFilter});
      // 
      // tabPageViews
      // 
      this.tabPageViews.ActiveControl = null;
      this.tabPageViews.Controls.Add(this.treeViewPanel);
      this.tabPageViews.KeyTip = null;
      this.tabPageViews.Localisation = "TreeViewViews";
      this.tabPageViews.LocalisationContext = "main";
      this.tabPageViews.Name = "tabPageViews";
      this.tabPageViews.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageViews.Size = new System.Drawing.Size(216, 514);
      this.tabPageViews.TabIndex = 0;
      this.tabPageViews.Text = "Views";
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
      this.treeViewPanel.Location = new System.Drawing.Point(3, 3);
      this.treeViewPanel.Name = "treeViewPanel";
      this.treeViewPanel.Size = new System.Drawing.Size(210, 508);
      this.treeViewPanel.TabIndex = 7;
      // 
      // treeViewPanelBottom
      // 
      this.treeViewPanelBottom.Controls.Add(this.treeViewFolderBrowser);
      this.treeViewPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewPanelBottom.Location = new System.Drawing.Point(0, 24);
      this.treeViewPanelBottom.Name = "treeViewPanelBottom";
      this.treeViewPanelBottom.Size = new System.Drawing.Size(210, 484);
      this.treeViewPanelBottom.TabIndex = 2;
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
      this.treeViewFolderBrowser.LabelEdit = true;
      this.treeViewFolderBrowser.Location = new System.Drawing.Point(0, 0);
      this.treeViewFolderBrowser.Name = "treeViewFolderBrowser";
      this.treeViewFolderBrowser.SelectedDirectories = ((System.Collections.Specialized.StringCollection)(resources.GetObject("treeViewFolderBrowser.SelectedDirectories")));
      this.treeViewFolderBrowser.Size = new System.Drawing.Size(210, 484);
      this.treeViewFolderBrowser.TabIndex = 1;
      this.treeViewFolderBrowser.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewFolderBrowser_AfterLabelEdit);
      this.treeViewFolderBrowser.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeViewFolderBrowser_NodeMouseHover);
      this.treeViewFolderBrowser.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewFolderBrowser_BeforeSelect);
      this.treeViewFolderBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFolderBrowser_AfterSelect);
      this.treeViewFolderBrowser.Click += new System.EventHandler(this.treeViewFolderBrowser_Click);
      this.treeViewFolderBrowser.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewFolderBrowser_DragDrop);
      this.treeViewFolderBrowser.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewFolderBrowser_DragOver);
      this.treeViewFolderBrowser.Enter += new System.EventHandler(this.treeViewFolderBrowser_Enter);
      this.treeViewFolderBrowser.Leave += new System.EventHandler(this.treeViewFolderBrowser_Leave);
      this.treeViewFolderBrowser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewFolderBrowser_MouseUp);
      // 
      // treeViewPanelTop
      // 
      this.treeViewPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.treeViewPanelTop.Location = new System.Drawing.Point(0, 0);
      this.treeViewPanelTop.Name = "treeViewPanelTop";
      this.treeViewPanelTop.Size = new System.Drawing.Size(210, 24);
      this.treeViewPanelTop.TabIndex = 1;
      // 
      // tabPageFilter
      // 
      this.tabPageFilter.ActiveControl = null;
      this.tabPageFilter.Controls.Add(this.dataGridViewTagFilter);
      this.tabPageFilter.Controls.Add(this.lbFileMask);
      this.tabPageFilter.Controls.Add(this.tbFileMask);
      this.tabPageFilter.Controls.Add(this.cbListFormats);
      this.tabPageFilter.Controls.Add(this.ckUseTagFilter);
      this.tabPageFilter.KeyTip = null;
      this.tabPageFilter.Localisation = "TreeViewFilters";
      this.tabPageFilter.LocalisationContext = "main";
      this.tabPageFilter.Name = "tabPageFilter";
      this.tabPageFilter.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageFilter.Size = new System.Drawing.Size(216, 514);
      this.tabPageFilter.TabIndex = 1;
      this.tabPageFilter.Text = "Filters";
      // 
      // dataGridViewTagFilter
      // 
      this.dataGridViewTagFilter.AllowUserToAddRows = false;
      this.dataGridViewTagFilter.AllowUserToDeleteRows = false;
      this.dataGridViewTagFilter.AllowUserToResizeColumns = false;
      this.dataGridViewTagFilter.AllowUserToResizeRows = false;
      this.dataGridViewTagFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridViewTagFilter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridViewTagFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewTagFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TagFilterField,
            this.TagFilterValue,
            this.TagFilterOperator});
      this.dataGridViewTagFilter.Location = new System.Drawing.Point(6, 34);
      this.dataGridViewTagFilter.Name = "dataGridViewTagFilter";
      this.dataGridViewTagFilter.RowHeadersVisible = false;
      this.dataGridViewTagFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
      this.dataGridViewTagFilter.Size = new System.Drawing.Size(198, 392);
      this.dataGridViewTagFilter.TabIndex = 4;
      this.dataGridViewTagFilter.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTagFilter_CellEndEdit);
      this.dataGridViewTagFilter.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewTagFilter_CurrentCellDirtyStateChanged);
      this.dataGridViewTagFilter.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewTagFilter_DataError);
      this.dataGridViewTagFilter.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewTagFilter_RowsRemoved);
      this.dataGridViewTagFilter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridViewTagFilter_MouseUp);
      // 
      // TagFilterField
      // 
      this.TagFilterField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.TagFilterField.HeaderText = "Field";
      this.TagFilterField.MaxDropDownItems = 50;
      this.TagFilterField.Name = "TagFilterField";
      this.TagFilterField.Width = 35;
      // 
      // TagFilterValue
      // 
      this.TagFilterValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
      this.TagFilterValue.DefaultCellStyle = dataGridViewCellStyle1;
      this.TagFilterValue.HeaderText = "Filter";
      this.TagFilterValue.Name = "TagFilterValue";
      this.TagFilterValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.TagFilterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // TagFilterOperator
      // 
      this.TagFilterOperator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      this.TagFilterOperator.FillWeight = 80F;
      this.TagFilterOperator.HeaderText = "Operator";
      this.TagFilterOperator.Name = "TagFilterOperator";
      this.TagFilterOperator.Width = 58;
      // 
      // lbFileMask
      // 
      this.lbFileMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbFileMask.Localisation = "FilterFileMask";
      this.lbFileMask.LocalisationContext = "main";
      this.lbFileMask.Location = new System.Drawing.Point(8, 459);
      this.lbFileMask.Name = "lbFileMask";
      this.lbFileMask.Size = new System.Drawing.Size(52, 13);
      this.lbFileMask.TabIndex = 3;
      this.lbFileMask.Text = "File Mask";
      // 
      // tbFileMask
      // 
      this.tbFileMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbFileMask.Location = new System.Drawing.Point(6, 476);
      this.tbFileMask.Name = "tbFileMask";
      this.tbFileMask.Size = new System.Drawing.Size(201, 20);
      this.tbFileMask.TabIndex = 2;
      this.tbFileMask.TextChanged += new System.EventHandler(this.tbFileMask_TextChanged);
      // 
      // cbListFormats
      // 
      this.cbListFormats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cbListFormats.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cbListFormats.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cbListFormats.Editable = false;
      this.cbListFormats.FormattingEnabled = true;
      this.cbListFormats.Id = "7e083ceb-b5c5-420b-b12a-6c8239e676c9";
      this.cbListFormats.Location = new System.Drawing.Point(6, 432);
      this.cbListFormats.Name = "cbListFormats";
      this.cbListFormats.Size = new System.Drawing.Size(201, 21);
      this.cbListFormats.TabIndex = 1;
      this.cbListFormats.SelectedIndexChanged += new System.EventHandler(this.cbListFormats_SelectedIndexChanged);
      // 
      // ckUseTagFilter
      // 
      this.ckUseTagFilter.Id = "02abeb36-c67e-413a-b98d-fb986ae6e931";
      this.ckUseTagFilter.Localisation = "FilterUseTag";
      this.ckUseTagFilter.LocalisationContext = "main";
      this.ckUseTagFilter.Location = new System.Drawing.Point(6, 7);
      this.ckUseTagFilter.Name = "ckUseTagFilter";
      this.ckUseTagFilter.Size = new System.Drawing.Size(85, 26);
      this.ckUseTagFilter.TabIndex = 0;
      this.ckUseTagFilter.Text = "Use tag filter";
      this.ckUseTagFilter.CheckedChanged += new System.EventHandler(this.ckUseTagFilter_CheckedChanged);
      // 
      // panelLeftBottom
      // 
      this.panelLeftBottom.Controls.Add(this.optionsPanelLeft);
      this.panelLeftBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelLeftBottom.Location = new System.Drawing.Point(0, 535);
      this.panelLeftBottom.Name = "panelLeftBottom";
      this.panelLeftBottom.Size = new System.Drawing.Size(218, 141);
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
      this.optionsPanelLeft.Controls.Add(this.btnRefreshFolder);
      this.optionsPanelLeft.Controls.Add(this.cbDataProvider);
      this.optionsPanelLeft.Controls.Add(this.checkBoxRecursive);
      this.optionsPanelLeft.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.optionsPanelLeft.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.optionsPanelLeft.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.optionsPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
      this.optionsPanelLeft.Location = new System.Drawing.Point(0, 0);
      this.optionsPanelLeft.Name = "optionsPanelLeft";
      this.optionsPanelLeft.Size = new System.Drawing.Size(218, 141);
      this.optionsPanelLeft.TabIndex = 2;
      // 
      // btnRefreshFolder
      // 
      this.btnRefreshFolder.Id = "2c274489-4997-4f22-abf7-cd1182404d55";
      this.btnRefreshFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnRefreshFolder.Localisation = "ButtonRefreshFolder";
      this.btnRefreshFolder.LocalisationContext = "main";
      this.btnRefreshFolder.Location = new System.Drawing.Point(13, 98);
      this.btnRefreshFolder.MaximumSize = new System.Drawing.Size(220, 20);
      this.btnRefreshFolder.Name = "btnRefreshFolder";
      this.btnRefreshFolder.Size = new System.Drawing.Size(184, 20);
      this.btnRefreshFolder.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::MPTagThat.Properties.Resources.RefreshDocViewHS)});
      this.btnRefreshFolder.TabIndex = 4;
      this.btnRefreshFolder.Text = "Refresh Folder View";
      this.btnRefreshFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnRefreshFolder.UseVisualStyleBackColor = true;
      this.btnRefreshFolder.Click += new System.EventHandler(this.btnRefreshFolder_Click);
      // 
      // cbDataProvider
      // 
      this.cbDataProvider.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cbDataProvider.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cbDataProvider.Editable = false;
      this.cbDataProvider.FormattingEnabled = true;
      this.cbDataProvider.Id = "2c9ad13d-d9c5-490c-ad67-bcc11a584c96";
      this.cbDataProvider.Location = new System.Drawing.Point(17, 34);
      this.cbDataProvider.Name = "cbDataProvider";
      this.cbDataProvider.Size = new System.Drawing.Size(180, 21);
      this.cbDataProvider.TabIndex = 5;
      // 
      // checkBoxRecursive
      // 
      this.checkBoxRecursive.Id = "b29408d1-fb73-4f6b-a7ca-ad3a9493b46a";
      this.checkBoxRecursive.Localisation = "ScanSubfolder";
      this.checkBoxRecursive.LocalisationContext = "main";
      this.checkBoxRecursive.Location = new System.Drawing.Point(17, 68);
      this.checkBoxRecursive.MaximumSize = new System.Drawing.Size(250, 0);
      this.checkBoxRecursive.Name = "checkBoxRecursive";
      this.checkBoxRecursive.Size = new System.Drawing.Size(132, 26);
      this.checkBoxRecursive.TabIndex = 3;
      this.checkBoxRecursive.Text = "Scan all subdirectories";
      // 
      // TreeViewControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControlTreeView);
      this.Controls.Add(this.panelLeftBottom);
      this.Name = "TreeViewControl";
      this.Size = new System.Drawing.Size(218, 676);
      this.contextMenuTreeView.ResumeLayout(false);
      this.contextMenuStripFilter.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.tabControlTreeView)).EndInit();
      this.tabPageViews.ResumeLayout(false);
      this.treeViewPanel.ResumeLayout(false);
      this.treeViewPanelBottom.ResumeLayout(false);
      this.tabPageFilter.ResumeLayout(false);
      this.tabPageFilter.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTagFilter)).EndInit();
      this.panelLeftBottom.ResumeLayout(false);
      this.optionsPanelLeft.ResumeLayout(false);
      this.optionsPanelLeft.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private Raccoom.Windows.Forms.TreeViewFolderBrowser treeViewFolderBrowser;
    private MPTagThat.Core.WinControls.MPTPanel panelLeftBottom;
    private MPTagThat.Core.WinControls.TTExtendedPanel optionsPanelLeft;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxRecursive;
    private MPTagThat.Core.WinControls.MPTButton btnRefreshFolder;
    private MPTagThat.Core.WinControls.TTExtendedPanel treeViewPanel;
    private MPTagThat.Core.WinControls.MPTPanel treeViewPanelBottom;
    private MPTagThat.Core.WinControls.MPTPanel treeViewPanelTop;
    private System.Windows.Forms.ContextMenuStrip contextMenuTreeView;
    private System.Windows.Forms.ToolStripMenuItem menuRefresh;
    private System.Windows.Forms.ToolStripMenuItem menuDelete;
    private System.Windows.Forms.ToolStripMenuItem menuCopy;
    private System.Windows.Forms.ToolStripMenuItem menuCut;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem menuPaste;
    private Elegant.Ui.TabControl tabControlTreeView;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageViews;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageFilter;
    private MPTagThat.Core.WinControls.MPTComboBox cbListFormats;
    private MPTagThat.Core.WinControls.MPTCheckBox ckUseTagFilter;
    private System.Windows.Forms.TextBox tbFileMask;
    private MPTagThat.Core.WinControls.MPTLabel lbFileMask;
    private System.Windows.Forms.DataGridView dataGridViewTagFilter;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripFilter;
    private System.Windows.Forms.ToolStripMenuItem menuInsertFilter;
    private System.Windows.Forms.ToolStripMenuItem menuDeleteFilter;
    private System.Windows.Forms.DataGridViewComboBoxColumn TagFilterField;
    private System.Windows.Forms.DataGridViewTextBoxColumn TagFilterValue;
    private System.Windows.Forms.DataGridViewComboBoxColumn TagFilterOperator;
    private MPTagThat.Core.WinControls.MPTComboBox cbDataProvider;
  }
}
