using Elegant.Ui;

namespace MPTagThat
{
  partial class MiscInfoControl
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
      this.tabPageNonMusicFiles = new MPTagThat.Core.WinControls.MPTTabPage();
      this.listViewNonMusicFiles = new System.Windows.Forms.ListView();
      this.tabControlMisc = new Elegant.Ui.TabControl();
      this.tabPageNonMusicFiles.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tabControlMisc)).BeginInit();
      this.SuspendLayout();
      // 
      // tabPageNonMusicFiles
      // 
      this.tabPageNonMusicFiles.ActiveControl = null;
      this.tabPageNonMusicFiles.Controls.Add(this.listViewNonMusicFiles);
      this.tabPageNonMusicFiles.KeyTip = null;
      this.tabPageNonMusicFiles.Localisation = "TabPageNonMusicFiles";
      this.tabPageNonMusicFiles.LocalisationContext = "main";
      this.tabPageNonMusicFiles.Name = "tabPageNonMusicFiles";
      this.tabPageNonMusicFiles.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageNonMusicFiles.Size = new System.Drawing.Size(201, 449);
      this.tabPageNonMusicFiles.TabIndex = 1;
      this.tabPageNonMusicFiles.Text = "Non Music Files";
      // 
      // listViewNonMusicFiles
      // 
      this.listViewNonMusicFiles.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewNonMusicFiles.Location = new System.Drawing.Point(3, 3);
      this.listViewNonMusicFiles.Name = "listViewNonMusicFiles";
      this.listViewNonMusicFiles.Size = new System.Drawing.Size(195, 443);
      this.listViewNonMusicFiles.TabIndex = 0;
      this.listViewNonMusicFiles.UseCompatibleStateImageBehavior = false;
      this.listViewNonMusicFiles.View = System.Windows.Forms.View.List;
      this.listViewNonMusicFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewNonMusicFiles_AfterLabelEdit);
      this.listViewNonMusicFiles.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewNonMusicFiles_BeforeLabelEdit);
      this.listViewNonMusicFiles.DoubleClick += new System.EventHandler(this.listViewNonMusicFiles_DoubleClick);
      this.listViewNonMusicFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewNonMusicFiles_MouseDown);
      this.listViewNonMusicFiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewNonMusicFiles_MouseMove);
      this.listViewNonMusicFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewNonMusicFiles_MouseUp);
      // 
      // tabControlMisc
      // 
      this.tabControlMisc.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlMisc.Location = new System.Drawing.Point(0, 0);
      this.tabControlMisc.Name = "tabControlMisc";
      this.tabControlMisc.SelectedTabPage = this.tabPageNonMusicFiles;
      this.tabControlMisc.Size = new System.Drawing.Size(203, 470);
      this.tabControlMisc.TabIndex = 0;
      this.tabControlMisc.TabPages.AddRange(new Elegant.Ui.TabPage[] {
            this.tabPageNonMusicFiles});
      this.tabControlMisc.TabsPlacement = Elegant.Ui.TabsPlacement.Bottom;
      // 
      // MiscInfoControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControlMisc);
      this.Name = "MiscInfoControl";
      this.Size = new System.Drawing.Size(203, 470);
      this.tabPageNonMusicFiles.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.tabControlMisc)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTTabPage tabPageNonMusicFiles;
    private Elegant.Ui.TabControl tabControlMisc;
    private System.Windows.Forms.ListView listViewNonMusicFiles;
  }
}
