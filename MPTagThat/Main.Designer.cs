namespace MPTagThat
{
  partial class Main
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.contextMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
      this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
      this.dataGridViewError = new System.Windows.Forms.DataGridView();
      this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.toolStripStatusLabelFiles = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabelFolder = new System.Windows.Forms.ToolStripStatusLabel();
      this.statusStrip = new System.Windows.Forms.StatusStrip();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.panelFileList = new MPTagThat.Core.WinControls.TTPanel();
      this.splitterBottom = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelMiddleBottom = new MPTagThat.Core.WinControls.TTPanel();
      this.splitterRight = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelRight = new MPTagThat.Core.WinControls.TTPanel();
      this.btnSaveFolderThumb = new MPTagThat.Core.WinControls.MPTButton();
      this.picturePanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.panelPicSize = new MPTagThat.Core.WinControls.TTPanel();
      this.pictureBoxAlbumArt = new System.Windows.Forms.PictureBox();
      this.fileInfoPanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.listViewFileInfo = new System.Windows.Forms.ListView();
      this.splitterLeft = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelLeft = new MPTagThat.Core.WinControls.TTPanel();
      this.panelLeftTop = new MPTagThat.Core.WinControls.TTPanel();
      this.treeViewPanel = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.treeViewPanelBottom = new MPTagThat.Core.WinControls.TTPanel();
      this.treeViewFolderBrowser = new Raccoom.Windows.Forms.TreeViewFolderBrowser();
      this.treeViewPanelTop = new MPTagThat.Core.WinControls.TTPanel();
      this.panelLeftBottom = new MPTagThat.Core.WinControls.TTPanel();
      this.optionsPanelLeft = new MPTagThat.Core.WinControls.TTExtendedPanel();
      this.checkBoxRecursive = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.btnRefreshFolder = new MPTagThat.Core.WinControls.MPTButton();
      this.panelTop = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddle = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddleTop = new MPTagThat.Core.WinControls.TTPanel();
      this.playerPanel = new MPTagThat.Core.WinControls.TTPanel();
      this.playerControl = new MPTagThat.Player.PlayerControl();
      this.contextMenuTreeView.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).BeginInit();
      this.statusStrip.SuspendLayout();
      this.panelMiddleBottom.SuspendLayout();
      this.panelRight.SuspendLayout();
      this.picturePanel.SuspendLayout();
      this.panelPicSize.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAlbumArt)).BeginInit();
      this.fileInfoPanel.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.panelLeftTop.SuspendLayout();
      this.treeViewPanel.SuspendLayout();
      this.treeViewPanelBottom.SuspendLayout();
      this.panelLeftBottom.SuspendLayout();
      this.optionsPanelLeft.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panelMiddleTop.SuspendLayout();
      this.playerPanel.SuspendLayout();
      this.SuspendLayout();
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
      this.menuRefresh.Size = new System.Drawing.Size(113, 22);
      this.menuRefresh.Text = "Refresh";
      this.menuRefresh.Click += new System.EventHandler(this.contextMenuTreeViewRefresh_Click);
      // 
      // menuDelete
      // 
      this.menuDelete.Image = ((System.Drawing.Image)(resources.GetObject("menuDelete.Image")));
      this.menuDelete.Name = "menuDelete";
      this.menuDelete.Size = new System.Drawing.Size(113, 22);
      this.menuDelete.Text = "Delete";
      this.menuDelete.Click += new System.EventHandler(this.contextMenuTreeViewDelete_Click);
      // 
      // dataGridViewError
      // 
      this.dataGridViewError.BackgroundColor = System.Drawing.SystemColors.Window;
      this.dataGridViewError.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dataGridViewError.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      this.dataGridViewError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewError.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.Message});
      this.dataGridViewError.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewError.Location = new System.Drawing.Point(0, 0);
      this.dataGridViewError.MultiSelect = false;
      this.dataGridViewError.Name = "dataGridViewError";
      this.dataGridViewError.ReadOnly = true;
      this.dataGridViewError.RowHeadersVisible = false;
      this.dataGridViewError.Size = new System.Drawing.Size(658, 100);
      this.dataGridViewError.TabIndex = 5;
      this.dataGridViewError.MouseClick += new System.Windows.Forms.MouseEventHandler(this.datagridViewError_MouseClick);
      // 
      // File
      // 
      this.File.HeaderText = "File";
      this.File.Name = "File";
      this.File.ReadOnly = true;
      this.File.Width = 300;
      // 
      // Message
      // 
      this.Message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Message.HeaderText = "Message";
      this.Message.Name = "Message";
      this.Message.ReadOnly = true;
      // 
      // toolStripStatusLabelFiles
      // 
      this.toolStripStatusLabelFiles.AutoSize = false;
      this.toolStripStatusLabelFiles.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
      this.toolStripStatusLabelFiles.Name = "toolStripStatusLabelFiles";
      this.toolStripStatusLabelFiles.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.toolStripStatusLabelFiles.Size = new System.Drawing.Size(250, 19);
      this.toolStripStatusLabelFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // toolStripStatusLabelFolder
      // 
      this.toolStripStatusLabelFolder.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
      this.toolStripStatusLabelFolder.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
      this.toolStripStatusLabelFolder.Name = "toolStripStatusLabelFolder";
      this.toolStripStatusLabelFolder.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.toolStripStatusLabelFolder.Size = new System.Drawing.Size(25, 19);
      this.toolStripStatusLabelFolder.Text = "   ";
      this.toolStripStatusLabelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // statusStrip
      // 
      this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelFiles,
            this.toolStripStatusLabelFolder});
      this.statusStrip.Location = new System.Drawing.Point(0, 728);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new System.Drawing.Size(1008, 24);
      this.statusStrip.TabIndex = 0;
      this.statusStrip.Text = "statusStrip";
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.HeaderText = "File";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Width = 300;
      // 
      // dataGridViewTextBoxColumn2
      // 
      this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.HeaderText = "Message";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      // 
      // panelFileList
      // 
      this.panelFileList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelFileList.Location = new System.Drawing.Point(0, 0);
      this.panelFileList.Name = "panelFileList";
      this.panelFileList.Size = new System.Drawing.Size(642, 403);
      this.panelFileList.TabIndex = 9;
      // 
      // splitterBottom
      // 
      this.splitterBottom.AnimationDelay = 20;
      this.splitterBottom.AnimationStep = 20;
      this.splitterBottom.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterBottom.ControlToHide = this.panelMiddleBottom;
      this.splitterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.splitterBottom.ExpandParentForm = false;
      this.splitterBottom.Localisation = "collapsibleSplitter1";
      this.splitterBottom.LocalisationContext = "Main";
      this.splitterBottom.Location = new System.Drawing.Point(150, 403);
      this.splitterBottom.Name = "collapsibleSplitter1";
      this.splitterBottom.TabIndex = 6;
      this.splitterBottom.TabStop = false;
      this.splitterBottom.UseAnimations = false;
      this.splitterBottom.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      // 
      // panelMiddleBottom
      // 
      this.panelMiddleBottom.Controls.Add(this.dataGridViewError);
      this.panelMiddleBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelMiddleBottom.Location = new System.Drawing.Point(150, 411);
      this.panelMiddleBottom.Name = "panelMiddleBottom";
      this.panelMiddleBottom.Size = new System.Drawing.Size(658, 100);
      this.panelMiddleBottom.TabIndex = 12;
      // 
      // splitterRight
      // 
      this.splitterRight.AnimationDelay = 20;
      this.splitterRight.AnimationStep = 20;
      this.splitterRight.BackColor = System.Drawing.SystemColors.Control;
      this.splitterRight.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterRight.ControlToHide = this.panelRight;
      this.splitterRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.splitterRight.ExpandParentForm = false;
      this.splitterRight.Localisation = "splitterRight";
      this.splitterRight.LocalisationContext = "Main";
      this.splitterRight.Location = new System.Drawing.Point(800, 0);
      this.splitterRight.Name = "splitterRight";
      this.splitterRight.TabIndex = 4;
      this.splitterRight.TabStop = false;
      this.splitterRight.UseAnimations = false;
      this.splitterRight.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      this.splitterRight.Click += new System.EventHandler(this.splitterRight_Click);
      // 
      // panelRight
      // 
      this.panelRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panelRight.BackColor = System.Drawing.SystemColors.Control;
      this.panelRight.Controls.Add(this.btnSaveFolderThumb);
      this.panelRight.Controls.Add(this.picturePanel);
      this.panelRight.Controls.Add(this.fileInfoPanel);
      this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelRight.Location = new System.Drawing.Point(808, 0);
      this.panelRight.MaximumSize = new System.Drawing.Size(200, 0);
      this.panelRight.MinimumSize = new System.Drawing.Size(170, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new System.Drawing.Size(200, 511);
      this.panelRight.TabIndex = 3;
      // 
      // btnSaveFolderThumb
      // 
      this.btnSaveFolderThumb.AutoSize = true;
      this.btnSaveFolderThumb.Localisation = "SaveFolderThumb";
      this.btnSaveFolderThumb.LocalisationContext = "file_info";
      this.btnSaveFolderThumb.Location = new System.Drawing.Point(5, 224);
      this.btnSaveFolderThumb.Name = "btnSaveFolderThumb";
      this.btnSaveFolderThumb.Size = new System.Drawing.Size(190, 41);
      this.btnSaveFolderThumb.TabIndex = 3;
      this.btnSaveFolderThumb.Text = "Save as folder thumb";
      this.btnSaveFolderThumb.UseVisualStyleBackColor = true;
      this.btnSaveFolderThumb.Click += new System.EventHandler(this.btnSaveFolderThumb_Click);
      // 
      // picturePanel
      // 
      this.picturePanel.AnimationStep = 30;
      this.picturePanel.BackColor = System.Drawing.SystemColors.Control;
      this.picturePanel.BorderColor = System.Drawing.Color.Transparent;
      this.picturePanel.CaptionBrush = Stepi.UI.BrushType.Solid;
      this.picturePanel.CaptionColorOne = System.Drawing.SystemColors.GradientActiveCaption;
      this.picturePanel.CaptionColorTwo = System.Drawing.SystemColors.GradientInactiveCaption;
      this.picturePanel.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.picturePanel.CaptionSize = 24;
      this.picturePanel.CaptionText = "Picture";
      this.picturePanel.CaptionTextColor = System.Drawing.Color.Black;
      this.picturePanel.Controls.Add(this.panelPicSize);
      this.picturePanel.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.picturePanel.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.picturePanel.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.picturePanel.Dock = System.Windows.Forms.DockStyle.Top;
      this.picturePanel.Location = new System.Drawing.Point(0, 0);
      this.picturePanel.Name = "picturePanel";
      this.picturePanel.Size = new System.Drawing.Size(200, 237);
      this.picturePanel.TabIndex = 1;
      // 
      // panelPicSize
      // 
      this.panelPicSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelPicSize.Controls.Add(this.pictureBoxAlbumArt);
      this.panelPicSize.Location = new System.Drawing.Point(5, 28);
      this.panelPicSize.Name = "panelPicSize";
      this.panelPicSize.Size = new System.Drawing.Size(190, 240);
      this.panelPicSize.TabIndex = 2;
      // 
      // pictureBoxAlbumArt
      // 
      this.pictureBoxAlbumArt.Dock = System.Windows.Forms.DockStyle.Top;
      this.pictureBoxAlbumArt.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxAlbumArt.Name = "pictureBoxAlbumArt";
      this.pictureBoxAlbumArt.Size = new System.Drawing.Size(190, 190);
      this.pictureBoxAlbumArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBoxAlbumArt.TabIndex = 2;
      this.pictureBoxAlbumArt.TabStop = false;
      // 
      // fileInfoPanel
      // 
      this.fileInfoPanel.AnimationStep = 30;
      this.fileInfoPanel.BorderColor = System.Drawing.Color.Transparent;
      this.fileInfoPanel.CaptionBrush = Stepi.UI.BrushType.Solid;
      this.fileInfoPanel.CaptionColorOne = System.Drawing.SystemColors.GradientActiveCaption;
      this.fileInfoPanel.CaptionColorTwo = System.Drawing.SystemColors.GradientInactiveCaption;
      this.fileInfoPanel.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.fileInfoPanel.CaptionSize = 24;
      this.fileInfoPanel.CaptionText = "Information";
      this.fileInfoPanel.CaptionTextColor = System.Drawing.Color.Black;
      this.fileInfoPanel.Controls.Add(this.listViewFileInfo);
      this.fileInfoPanel.CornerStyle = Stepi.UI.CornerStyle.Normal;
      this.fileInfoPanel.DirectionCtrlColor = System.Drawing.Color.DarkGray;
      this.fileInfoPanel.DirectionCtrlHoverColor = System.Drawing.Color.Orange;
      this.fileInfoPanel.Location = new System.Drawing.Point(3, 271);
      this.fileInfoPanel.Name = "fileInfoPanel";
      this.fileInfoPanel.Size = new System.Drawing.Size(194, 173);
      this.fileInfoPanel.TabIndex = 0;
      // 
      // listViewFileInfo
      // 
      this.listViewFileInfo.BackColor = System.Drawing.SystemColors.Control;
      this.listViewFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.listViewFileInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewFileInfo.Location = new System.Drawing.Point(0, 25);
      this.listViewFileInfo.Name = "listViewFileInfo";
      this.listViewFileInfo.Size = new System.Drawing.Size(198, 192);
      this.listViewFileInfo.TabIndex = 1;
      this.listViewFileInfo.UseCompatibleStateImageBehavior = false;
      this.listViewFileInfo.View = System.Windows.Forms.View.Details;
      // 
      // splitterLeft
      // 
      this.splitterLeft.AnimationDelay = 20;
      this.splitterLeft.AnimationStep = 20;
      this.splitterLeft.BackColor = System.Drawing.SystemColors.Control;
      this.splitterLeft.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterLeft.ControlToHide = this.panelLeft;
      this.splitterLeft.ExpandParentForm = false;
      this.splitterLeft.Localisation = "splitterLeft";
      this.splitterLeft.LocalisationContext = "Main";
      this.splitterLeft.Location = new System.Drawing.Point(150, 0);
      this.splitterLeft.Name = "splitterLeft";
      this.splitterLeft.TabIndex = 2;
      this.splitterLeft.TabStop = false;
      this.splitterLeft.UseAnimations = false;
      this.splitterLeft.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      // 
      // panelLeft
      // 
      this.panelLeft.Controls.Add(this.panelLeftTop);
      this.panelLeft.Controls.Add(this.panelLeftBottom);
      this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.panelLeft.Location = new System.Drawing.Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new System.Drawing.Size(150, 511);
      this.panelLeft.TabIndex = 1;
      // 
      // panelLeftTop
      // 
      this.panelLeftTop.Controls.Add(this.treeViewPanel);
      this.panelLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelLeftTop.Location = new System.Drawing.Point(0, 0);
      this.panelLeftTop.Name = "panelLeftTop";
      this.panelLeftTop.Size = new System.Drawing.Size(150, 370);
      this.panelLeftTop.TabIndex = 4;
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
      this.treeViewPanel.Size = new System.Drawing.Size(150, 370);
      this.treeViewPanel.TabIndex = 3;
      // 
      // treeViewPanelBottom
      // 
      this.treeViewPanelBottom.Controls.Add(this.treeViewFolderBrowser);
      this.treeViewPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewPanelBottom.Location = new System.Drawing.Point(0, 24);
      this.treeViewPanelBottom.Name = "treeViewPanelBottom";
      this.treeViewPanelBottom.Size = new System.Drawing.Size(150, 346);
      this.treeViewPanelBottom.TabIndex = 2;
      // 
      // treeViewFolderBrowser
      // 
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
      this.treeViewFolderBrowser.Size = new System.Drawing.Size(150, 346);
      this.treeViewFolderBrowser.TabIndex = 0;
      this.treeViewFolderBrowser.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewFolderBrowser_Click);
      this.treeViewFolderBrowser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewFolderBrowser_MouseUp);
      this.treeViewFolderBrowser.Enter += new System.EventHandler(this.treeViewFolderBrowser_Enter);
      this.treeViewFolderBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFolderBrowser_AfterSelect);
      this.treeViewFolderBrowser.Leave += new System.EventHandler(this.treeViewFolderBrowser_Leave);
      this.treeViewFolderBrowser.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewFolderBrowser_BeforeSelect);
      // 
      // treeViewPanelTop
      // 
      this.treeViewPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.treeViewPanelTop.Location = new System.Drawing.Point(0, 0);
      this.treeViewPanelTop.Name = "treeViewPanelTop";
      this.treeViewPanelTop.Size = new System.Drawing.Size(150, 24);
      this.treeViewPanelTop.TabIndex = 1;
      // 
      // panelLeftBottom
      // 
      this.panelLeftBottom.Controls.Add(this.optionsPanelLeft);
      this.panelLeftBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelLeftBottom.Location = new System.Drawing.Point(0, 370);
      this.panelLeftBottom.Name = "panelLeftBottom";
      this.panelLeftBottom.Size = new System.Drawing.Size(150, 141);
      this.panelLeftBottom.TabIndex = 5;
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
      this.optionsPanelLeft.Size = new System.Drawing.Size(150, 141);
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
      // panelTop
      // 
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(1008, 149);
      this.panelTop.TabIndex = 0;
      // 
      // panelMiddle
      // 
      this.panelMiddle.Controls.Add(this.panelMiddleTop);
      this.panelMiddle.Controls.Add(this.splitterLeft);
      this.panelMiddle.Controls.Add(this.splitterRight);
      this.panelMiddle.Controls.Add(this.splitterBottom);
      this.panelMiddle.Controls.Add(this.panelMiddleBottom);
      this.panelMiddle.Controls.Add(this.panelLeft);
      this.panelMiddle.Controls.Add(this.panelRight);
      this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddle.Location = new System.Drawing.Point(0, 149);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(1008, 511);
      this.panelMiddle.TabIndex = 10;
      // 
      // panelMiddleTop
      // 
      this.panelMiddleTop.Controls.Add(this.panelFileList);
      this.panelMiddleTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddleTop.Location = new System.Drawing.Point(158, 0);
      this.panelMiddleTop.Name = "panelMiddleTop";
      this.panelMiddleTop.Size = new System.Drawing.Size(642, 403);
      this.panelMiddleTop.TabIndex = 11;
      // 
      // playerPanel
      // 
      this.playerPanel.Controls.Add(this.playerControl);
      this.playerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.playerPanel.Location = new System.Drawing.Point(0, 660);
      this.playerPanel.Name = "playerPanel";
      this.playerPanel.Size = new System.Drawing.Size(1008, 68);
      this.playerPanel.TabIndex = 11;
      // 
      // playerControl
      // 
      this.playerControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.playerControl.Location = new System.Drawing.Point(0, 0);
      this.playerControl.Name = "playerControl";
      this.playerControl.Size = new System.Drawing.Size(1008, 68);
      this.playerControl.TabIndex = 0;
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1008, 752);
      this.ControlBox = false;
      this.Controls.Add(this.panelMiddle);
      this.Controls.Add(this.playerPanel);
      this.Controls.Add(this.panelTop);
      this.Controls.Add(this.statusStrip);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Main";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Load += new System.EventHandler(this.Main_Load);
      this.Move += new System.EventHandler(this.Main_Move);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Close);
      this.Resize += new System.EventHandler(this.Main_Resize);
      this.contextMenuTreeView.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).EndInit();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.panelMiddleBottom.ResumeLayout(false);
      this.panelRight.ResumeLayout(false);
      this.panelRight.PerformLayout();
      this.picturePanel.ResumeLayout(false);
      this.panelPicSize.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAlbumArt)).EndInit();
      this.fileInfoPanel.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
      this.panelLeftTop.ResumeLayout(false);
      this.treeViewPanel.ResumeLayout(false);
      this.treeViewPanelBottom.ResumeLayout(false);
      this.panelLeftBottom.ResumeLayout(false);
      this.optionsPanelLeft.ResumeLayout(false);
      this.optionsPanelLeft.PerformLayout();
      this.panelMiddle.ResumeLayout(false);
      this.panelMiddleTop.ResumeLayout(false);
      this.playerPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.TTPanel panelTop;
    private MPTagThat.Core.WinControls.TTPanel panelLeft;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterLeft;
    private MPTagThat.Core.WinControls.TTPanel panelRight;
    private Raccoom.Windows.Forms.TreeViewFolderBrowser treeViewFolderBrowser;
    private MPTagThat.Core.WinControls.TTExtendedPanel fileInfoPanel;
    private MPTagThat.Core.WinControls.TTExtendedPanel picturePanel;
    private System.Windows.Forms.ListView listViewFileInfo;
    private System.Windows.Forms.ToolTip toolTip;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterRight;
    private MPTagThat.Core.WinControls.TTPanel panelPicSize;
    private System.Windows.Forms.PictureBox pictureBoxAlbumArt;
    private MPTagThat.Core.WinControls.MPTCheckBox checkBoxRecursive;
    private MPTagThat.Core.WinControls.TTPanel panelLeftTop;
    private MPTagThat.Core.WinControls.TTPanel panelLeftBottom;
    private MPTagThat.Core.WinControls.TTExtendedPanel optionsPanelLeft;
    private MPTagThat.Core.WinControls.TTExtendedPanel treeViewPanel;
    private MPTagThat.Core.WinControls.TTPanel treeViewPanelTop;
    private MPTagThat.Core.WinControls.MPTButton btnRefreshFolder;
    private System.Windows.Forms.ContextMenuStrip contextMenuTreeView;
    private System.Windows.Forms.ToolStripMenuItem menuRefresh;
    private System.Windows.Forms.ToolStripMenuItem menuDelete;
    private MPTagThat.Core.WinControls.TTPanel treeViewPanelBottom;
    private System.Windows.Forms.DataGridView dataGridViewError;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterBottom;
    private MPTagThat.Core.WinControls.TTPanel panelFileList;
    private System.Windows.Forms.DataGridViewTextBoxColumn File;
    private System.Windows.Forms.DataGridViewTextBoxColumn Message;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFiles;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFolder;
    private System.Windows.Forms.StatusStrip statusStrip;
    private MPTagThat.Core.WinControls.MPTButton btnSaveFolderThumb;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private MPTagThat.Core.WinControls.TTPanel panelMiddle;
    private MPTagThat.Core.WinControls.TTPanel playerPanel;
    private MPTagThat.Player.PlayerControl playerControl;
    private MPTagThat.Core.WinControls.TTPanel panelMiddleTop;
    private MPTagThat.Core.WinControls.TTPanel panelMiddleBottom;
  }
}

