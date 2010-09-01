using System.Windows.Forms;

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
      this.panelBottom = new System.Windows.Forms.Panel();
      this.playerPanel = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddle = new MPTagThat.Core.WinControls.TTPanel();
      this.panelMiddleTop = new MPTagThat.Core.WinControls.TTPanel();
      this.panelFileList = new MPTagThat.Core.WinControls.TTPanel();
      this.splitterTop = new NJFLib.Controls.CollapsibleSplitter();
      this.panelMiddleDBSearch = new MPTagThat.Core.WinControls.TTPanel();
      this.splitterLeft = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelLeft = new MPTagThat.Core.WinControls.TTPanel();
      this.panelLeftTop = new MPTagThat.Core.WinControls.TTPanel();
      this.splitterRight = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelRight = new MPTagThat.Core.WinControls.TTPanel();
      this.splitterBottom = new MPTagThat.Core.WinControls.MPTCollapsibleSplitter();
      this.panelMiddleBottom = new MPTagThat.Core.WinControls.TTPanel();
      this.splitterPlayer = new NJFLib.Controls.CollapsibleSplitter();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.formFrameSkinner = new Elegant.Ui.FormFrameSkinner();
      this.statusBar = new Elegant.Ui.StatusBar();
      this.statusBarNotificationsArea1 = new Elegant.Ui.StatusBarNotificationsArea();
      this.statusBarPane2 = new Elegant.Ui.StatusBarPane();
      this.toolStripStatusLabelFiles = new Elegant.Ui.Label();
      this.toolStripStatusLabelFilter = new Elegant.Ui.Label();
      this.statusBarPane3 = new Elegant.Ui.StatusBarPane();
      this.toolStripStatusLabelFolder = new Elegant.Ui.Label();
      this.statusBarPane4 = new Elegant.Ui.StatusBarPane();
      this.statusBarControlsArea1 = new Elegant.Ui.StatusBarControlsArea();
      this.statusBarPane1 = new Elegant.Ui.StatusBarPane();
      this.progressBar1 = new Elegant.Ui.ProgressBar();
      this.buttonProgressCancel = new Elegant.Ui.Button();
      this.panelTop = new MPTagThat.Core.WinControls.TTPanel();
      this.toolStripStatusLabelScanProgress = new Elegant.Ui.Label();
      this.panelBottom.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panelMiddleTop.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.statusBar.SuspendLayout();
      this.statusBarNotificationsArea1.SuspendLayout();
      this.statusBarPane2.SuspendLayout();
      this.statusBarPane3.SuspendLayout();
      this.statusBarPane4.SuspendLayout();
      this.statusBarControlsArea1.SuspendLayout();
      this.statusBarPane1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelBottom
      // 
      this.panelBottom.Controls.Add(this.playerPanel);
      this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelBottom.Location = new System.Drawing.Point(0, 831);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new System.Drawing.Size(1008, 90);
      this.panelBottom.TabIndex = 12;
      // 
      // playerPanel
      // 
      this.playerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.playerPanel.Location = new System.Drawing.Point(0, 0);
      this.playerPanel.Name = "playerPanel";
      this.playerPanel.Size = new System.Drawing.Size(1008, 90);
      this.playerPanel.TabIndex = 11;
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
      this.panelMiddle.Location = new System.Drawing.Point(0, 147);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(1008, 676);
      this.panelMiddle.TabIndex = 10;
      // 
      // panelMiddleTop
      // 
      this.panelMiddleTop.Controls.Add(this.panelFileList);
      this.panelMiddleTop.Controls.Add(this.splitterTop);
      this.panelMiddleTop.Controls.Add(this.panelMiddleDBSearch);
      this.panelMiddleTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddleTop.Location = new System.Drawing.Point(158, 0);
      this.panelMiddleTop.Name = "panelMiddleTop";
      this.panelMiddleTop.Size = new System.Drawing.Size(544, 527);
      this.panelMiddleTop.TabIndex = 11;
      // 
      // panelFileList
      // 
      this.panelFileList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelFileList.Location = new System.Drawing.Point(0, 88);
      this.panelFileList.Name = "panelFileList";
      this.panelFileList.Size = new System.Drawing.Size(544, 439);
      this.panelFileList.TabIndex = 9;
      // 
      // splitterTop
      // 
      this.splitterTop.AnimationDelay = 20;
      this.splitterTop.AnimationStep = 20;
      this.splitterTop.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
      this.splitterTop.ControlToHide = this.panelMiddleDBSearch;
      this.splitterTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.splitterTop.ExpandParentForm = false;
      this.splitterTop.Location = new System.Drawing.Point(0, 80);
      this.splitterTop.Name = "splitterTop";
      this.splitterTop.TabIndex = 11;
      this.splitterTop.TabStop = false;
      this.splitterTop.UseAnimations = true;
      this.splitterTop.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      // 
      // panelMiddleDBSearch
      // 
      this.panelMiddleDBSearch.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelMiddleDBSearch.Location = new System.Drawing.Point(0, 0);
      this.panelMiddleDBSearch.Name = "panelMiddleDBSearch";
      this.panelMiddleDBSearch.Size = new System.Drawing.Size(544, 80);
      this.panelMiddleDBSearch.TabIndex = 10;
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
      this.panelLeft.Size = new System.Drawing.Size(150, 676);
      this.panelLeft.TabIndex = 1;
      // 
      // panelLeftTop
      // 
      this.panelLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelLeftTop.Location = new System.Drawing.Point(0, 0);
      this.panelLeftTop.Name = "panelLeftTop";
      this.panelLeftTop.Size = new System.Drawing.Size(150, 676);
      this.panelLeftTop.TabIndex = 4;
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
      this.splitterRight.Location = new System.Drawing.Point(702, 0);
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
      this.panelRight.Location = new System.Drawing.Point(710, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new System.Drawing.Size(298, 676);
      this.panelRight.TabIndex = 3;
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
      this.splitterBottom.Location = new System.Drawing.Point(150, 527);
      this.splitterBottom.Name = "collapsibleSplitter1";
      this.splitterBottom.TabIndex = 6;
      this.splitterBottom.TabStop = false;
      this.splitterBottom.UseAnimations = false;
      this.splitterBottom.VisualStyle = NJFLib.Controls.VisualStyles.XP;
      // 
      // panelMiddleBottom
      // 
      this.panelMiddleBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelMiddleBottom.Location = new System.Drawing.Point(150, 535);
      this.panelMiddleBottom.Name = "panelMiddleBottom";
      this.panelMiddleBottom.Size = new System.Drawing.Size(560, 141);
      this.panelMiddleBottom.TabIndex = 12;
      // 
      // splitterPlayer
      // 
      this.splitterPlayer.AnimationDelay = 20;
      this.splitterPlayer.AnimationStep = 20;
      this.splitterPlayer.BorderStyle3D = System.Windows.Forms.Border3DStyle.RaisedOuter;
      this.splitterPlayer.ControlToHide = this.panelBottom;
      this.splitterPlayer.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.splitterPlayer.ExpandParentForm = false;
      this.splitterPlayer.Location = new System.Drawing.Point(0, 823);
      this.splitterPlayer.Name = "splitterPlayer";
      this.splitterPlayer.TabIndex = 13;
      this.splitterPlayer.TabStop = false;
      this.splitterPlayer.UseAnimations = true;
      this.splitterPlayer.VisualStyle = NJFLib.Controls.VisualStyles.XP;
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
      // formFrameSkinner
      // 
      this.formFrameSkinner.Form = this;
      this.formFrameSkinner.TitleFont = new System.Drawing.Font("Tahoma", 9F);
      // 
      // statusBar
      // 
      this.statusBar.Controls.Add(this.statusBarNotificationsArea1);
      this.statusBar.Controls.Add(this.statusBarControlsArea1);
      this.statusBar.ControlsArea = this.statusBarControlsArea1;
      this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.statusBar.Id = "9f36caa8-c691-4743-a029-2e42ada47e03";
      this.statusBar.Location = new System.Drawing.Point(0, 921);
      this.statusBar.Name = "statusBar";
      this.statusBar.NotificationsArea = this.statusBarNotificationsArea1;
      this.statusBar.Size = new System.Drawing.Size(1008, 22);
      this.statusBar.TabIndex = 16;
      this.statusBar.Text = "statusBar1";
      // 
      // statusBarNotificationsArea1
      // 
      this.statusBarNotificationsArea1.AutoSize = true;
      this.statusBarNotificationsArea1.Controls.Add(this.statusBarPane2);
      this.statusBarNotificationsArea1.Controls.Add(this.statusBarPane3);
      this.statusBarNotificationsArea1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.statusBarNotificationsArea1.Id = "04ca63fd-be8d-465d-b78a-f2dc3829035b";
      this.statusBarNotificationsArea1.Location = new System.Drawing.Point(0, 0);
      this.statusBarNotificationsArea1.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarNotificationsArea1.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarNotificationsArea1.Name = "statusBarNotificationsArea1";
      this.statusBarNotificationsArea1.Size = new System.Drawing.Size(743, 22);
      this.statusBarNotificationsArea1.TabIndex = 1;
      // 
      // statusBarPane2
      // 
      this.statusBarPane2.AutoSize = true;
      this.statusBarPane2.Controls.Add(this.toolStripStatusLabelFiles);
      this.statusBarPane2.Controls.Add(this.toolStripStatusLabelFilter);
      this.statusBarPane2.Id = "31c6473f-9490-4bdb-8506-a8f5736636e4";
      this.statusBarPane2.Location = new System.Drawing.Point(0, 0);
      this.statusBarPane2.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane2.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane2.Name = "statusBarPane2";
      this.statusBarPane2.Size = new System.Drawing.Size(139, 22);
      this.statusBarPane2.TabIndex = 0;
      // 
      // toolStripStatusLabelFiles
      // 
      this.toolStripStatusLabelFiles.Id = "102bb77e-f358-4861-baaf-3d864126348d";
      this.toolStripStatusLabelFiles.Location = new System.Drawing.Point(5, 2);
      this.toolStripStatusLabelFiles.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.toolStripStatusLabelFiles.Name = "toolStripStatusLabelFiles";
      this.toolStripStatusLabelFiles.Size = new System.Drawing.Size(57, 19);
      this.toolStripStatusLabelFiles.TabIndex = 0;
      this.toolStripStatusLabelFiles.Text = "                   ";
      // 
      // toolStripStatusLabelFilter
      // 
      this.toolStripStatusLabelFilter.Id = "9630b051-7abe-463b-846b-3f17f0b608a6";
      this.toolStripStatusLabelFilter.Location = new System.Drawing.Point(68, 2);
      this.toolStripStatusLabelFilter.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.toolStripStatusLabelFilter.Name = "toolStripStatusLabelFilter";
      this.toolStripStatusLabelFilter.Size = new System.Drawing.Size(48, 19);
      this.toolStripStatusLabelFilter.TabIndex = 1;
      this.toolStripStatusLabelFilter.Text = "                ";
      // 
      // statusBarPane3
      // 
      this.statusBarPane3.AutoSize = true;
      this.statusBarPane3.Controls.Add(this.toolStripStatusLabelFolder);
      this.statusBarPane3.Id = "96bcecd2-41a2-4a29-8f20-0147730acd87";
      this.statusBarPane3.Location = new System.Drawing.Point(139, 0);
      this.statusBarPane3.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane3.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane3.Name = "statusBarPane3";
      this.statusBarPane3.Padding = new System.Windows.Forms.Padding(10, 2, 2, 1);
      this.statusBarPane3.Size = new System.Drawing.Size(99, 22);
      this.statusBarPane3.TabIndex = 1;
      // 
      // toolStripStatusLabelFolder
      // 
      this.toolStripStatusLabelFolder.Id = "514085f9-be54-4033-b45d-3f61671f32d7";
      this.toolStripStatusLabelFolder.Location = new System.Drawing.Point(13, 2);
      this.toolStripStatusLabelFolder.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.toolStripStatusLabelFolder.Name = "toolStripStatusLabelFolder";
      this.toolStripStatusLabelFolder.Size = new System.Drawing.Size(63, 19);
      this.toolStripStatusLabelFolder.TabIndex = 0;
      this.toolStripStatusLabelFolder.Text = "                     ";
      // 
      // statusBarPane4
      // 
      this.statusBarPane4.AutoSize = true;
      this.statusBarPane4.Dock = DockStyle.Right;
      this.statusBarPane4.Controls.Add(this.toolStripStatusLabelScanProgress);
      this.statusBarPane4.Id = "5b4f806c-dbe9-4a06-9f7e-1c797e7a188d";
      this.statusBarPane4.Location = new System.Drawing.Point(238, 0);
      this.statusBarPane4.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane4.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane4.Name = "statusBarPane4";
      this.statusBarPane4.Size = new System.Drawing.Size(59, 22);
      this.statusBarPane4.TabIndex = 2;
      // 
      // statusBarControlsArea1
      // 
      this.statusBarControlsArea1.AutoSize = true;
      this.statusBarControlsArea1.Controls.Add(this.statusBarPane4);
      this.statusBarControlsArea1.Controls.Add(this.statusBarPane1);
      this.statusBarControlsArea1.Dock = System.Windows.Forms.DockStyle.Right;
      this.statusBarControlsArea1.Id = "ab6e139b-b0ad-48eb-a50f-ed1567007940";
      this.statusBarControlsArea1.Location = new System.Drawing.Point(743, 0);
      this.statusBarControlsArea1.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarControlsArea1.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarControlsArea1.Name = "statusBarControlsArea1";
      this.statusBarControlsArea1.Size = new System.Drawing.Size(265, 22);
      this.statusBarControlsArea1.TabIndex = 0;
      // 
      // statusBarPane1
      // 
      this.statusBarPane1.AutoSize = true;
      this.statusBarPane1.Controls.Add(this.progressBar1);
      this.statusBarPane1.Controls.Add(this.buttonProgressCancel);
      this.statusBarPane1.Id = "d8b59571-91d3-4185-a9a4-b735139df43b";
      this.statusBarPane1.Location = new System.Drawing.Point(0, 0);
      this.statusBarPane1.MaximumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane1.MinimumSize = new System.Drawing.Size(0, 22);
      this.statusBarPane1.Name = "statusBarPane1";
      this.statusBarPane1.Size = new System.Drawing.Size(229, 22);
      this.statusBarPane1.TabIndex = 0;
      // 
      // progressBar1
      // 
      this.progressBar1.DesiredWidth = 175;
      this.progressBar1.Id = "a3fc702d-0b4f-4160-9fff-0baba8d7430e";
      this.progressBar1.Location = new System.Drawing.Point(3, 2);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(175, 19);
      this.progressBar1.TabIndex = 0;
      this.progressBar1.Text = "progressBar1";
      // 
      // buttonProgressCancel
      // 
      this.buttonProgressCancel.AutoSize = true;
      this.buttonProgressCancel.CommandName = "ProgressCancel";
      this.buttonProgressCancel.Id = "72fee6d7-03bc-42a8-8e13-35309ec722ad";
      this.buttonProgressCancel.Location = new System.Drawing.Point(180, 2);
      this.buttonProgressCancel.Name = "buttonProgressCancel";
      this.buttonProgressCancel.Size = new System.Drawing.Size(26, 19);
      this.buttonProgressCancel.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("buttonProgressCancel.SmallImages.Images"))))});
      this.buttonProgressCancel.TabIndex = 1;
      this.buttonProgressCancel.MouseLeave += new System.EventHandler(this.buttonProgressCancel_MouseLeave);
      this.buttonProgressCancel.MouseEnter += new System.EventHandler(this.buttonProgressCancel_MouseEnter);
      // 
      // panelTop
      // 
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(1008, 147);
      this.panelTop.TabIndex = 17;
      // 
      // toolStripStatusLabelScanProgress
      // 
      this.toolStripStatusLabelScanProgress.Id = "1c0a8709-f9d1-4339-bfcb-50c586fbcceb";
      this.toolStripStatusLabelScanProgress.Location = new System.Drawing.Point(5, 2);
      this.toolStripStatusLabelScanProgress.Name = "toolStripStatusLabelScanProgress";
      this.toolStripStatusLabelScanProgress.Size = new System.Drawing.Size(31, 19);
      this.toolStripStatusLabelScanProgress.TabIndex = 0;
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1008, 943);
      this.Controls.Add(this.panelMiddle);
      this.Controls.Add(this.splitterPlayer);
      this.Controls.Add(this.panelBottom);
      this.Controls.Add(this.panelTop);
      this.Controls.Add(this.statusBar);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Main";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "MPTagThat - The MediaPortal Tag Editor";
      this.Load += new System.EventHandler(this.Main_Load);
      this.Move += new System.EventHandler(this.Main_Move);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Close);
      this.Resize += new System.EventHandler(this.Main_Resize);
      this.panelBottom.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      this.panelMiddleTop.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
      this.statusBar.ResumeLayout(false);
      this.statusBar.PerformLayout();
      this.statusBarNotificationsArea1.ResumeLayout(false);
      this.statusBarNotificationsArea1.PerformLayout();
      this.statusBarPane2.ResumeLayout(false);
      this.statusBarPane2.PerformLayout();
      this.statusBarPane3.ResumeLayout(false);
      this.statusBarPane3.PerformLayout();
      this.statusBarPane4.ResumeLayout(false);
      this.statusBarPane4.PerformLayout();
      this.statusBarControlsArea1.ResumeLayout(false);
      this.statusBarControlsArea1.PerformLayout();
      this.statusBarPane1.ResumeLayout(false);
      this.statusBarPane1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.TTPanel panelLeft;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterLeft;
    private MPTagThat.Core.WinControls.TTPanel panelRight;
    private System.Windows.Forms.ToolTip toolTip;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterRight;
    private MPTagThat.Core.WinControls.TTPanel panelLeftTop;
    private MPTagThat.Core.WinControls.MPTCollapsibleSplitter splitterBottom;
    private MPTagThat.Core.WinControls.TTPanel panelFileList;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private MPTagThat.Core.WinControls.TTPanel panelMiddle;
    private MPTagThat.Core.WinControls.TTPanel playerPanel;
    private MPTagThat.Core.WinControls.TTPanel panelMiddleTop;
    private MPTagThat.Core.WinControls.TTPanel panelMiddleBottom;
    private NJFLib.Controls.CollapsibleSplitter splitterTop;
    private MPTagThat.Core.WinControls.TTPanel panelMiddleDBSearch;
    private System.Windows.Forms.Panel panelBottom;
    private NJFLib.Controls.CollapsibleSplitter splitterPlayer;
    private Elegant.Ui.FormFrameSkinner formFrameSkinner;
    private Elegant.Ui.StatusBar statusBar;
    private Elegant.Ui.StatusBarNotificationsArea statusBarNotificationsArea1;
    private Elegant.Ui.StatusBarPane statusBarPane2;
    private Elegant.Ui.StatusBarControlsArea statusBarControlsArea1;
    private Elegant.Ui.StatusBarPane statusBarPane3;
    private Elegant.Ui.Label toolStripStatusLabelFolder;
    private Elegant.Ui.Label toolStripStatusLabelFiles;
    private Elegant.Ui.Label toolStripStatusLabelFilter;
    private MPTagThat.Core.WinControls.TTPanel panelTop;
    private Elegant.Ui.StatusBarPane statusBarPane1;
    internal Elegant.Ui.ProgressBar progressBar1;
    private Elegant.Ui.Button buttonProgressCancel;
    private Elegant.Ui.StatusBarPane statusBarPane4;
    private Elegant.Ui.Label toolStripStatusLabelScanProgress;
  }
}

