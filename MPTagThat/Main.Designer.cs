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
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
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
      this.splitterLeft = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelLeft = new MPTagThat.Core.WinControls.TTPanel();
      this.panelLeftTop = new MPTagThat.Core.WinControls.TTPanel();
      this.panelTop = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddle = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddleTop = new MPTagThat.Core.WinControls.TTPanel();
      this.playerPanel = new MPTagThat.Core.WinControls.TTPanel();
      this.playerControl = new MPTagThat.Player.PlayerControl();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).BeginInit();
      this.statusStrip.SuspendLayout();
      this.panelMiddleBottom.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panelMiddleTop.SuspendLayout();
      this.playerPanel.SuspendLayout();
      this.SuspendLayout();
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
      this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelRight.Location = new System.Drawing.Point(808, 0);
      this.panelRight.MaximumSize = new System.Drawing.Size(200, 0);
      this.panelRight.MinimumSize = new System.Drawing.Size(170, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new System.Drawing.Size(200, 511);
      this.panelRight.TabIndex = 3;
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
      this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.panelLeft.Location = new System.Drawing.Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new System.Drawing.Size(150, 511);
      this.panelLeft.TabIndex = 1;
      // 
      // panelLeftTop
      // 
      this.panelLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelLeftTop.Location = new System.Drawing.Point(0, 0);
      this.panelLeftTop.Name = "panelLeftTop";
      this.panelLeftTop.Size = new System.Drawing.Size(150, 511);
      this.panelLeftTop.TabIndex = 4;
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
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).EndInit();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.panelMiddleBottom.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
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
    private System.Windows.Forms.ToolTip toolTip;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterRight;
    private MPTagThat.Core.WinControls.TTPanel panelLeftTop;
    private System.Windows.Forms.DataGridView dataGridViewError;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterBottom;
    private MPTagThat.Core.WinControls.TTPanel panelFileList;
    private System.Windows.Forms.DataGridViewTextBoxColumn File;
    private System.Windows.Forms.DataGridViewTextBoxColumn Message;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFiles;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFolder;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private MPTagThat.Core.WinControls.TTPanel panelMiddle;
    private MPTagThat.Core.WinControls.TTPanel playerPanel;
    private MPTagThat.Player.PlayerControl playerControl;
    private MPTagThat.Core.WinControls.TTPanel panelMiddleTop;
    private MPTagThat.Core.WinControls.TTPanel panelMiddleBottom;
  }
}

