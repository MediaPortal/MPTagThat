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
      this.tabControlMisc = new System.Windows.Forms.TabControl();
      this.tabPageNonMusicFiles.SuspendLayout();
      this.tabControlMisc.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabPageNonMusicFiles
      // 
      this.tabPageNonMusicFiles.Controls.Add(this.listViewNonMusicFiles);
      this.tabPageNonMusicFiles.Localisation = "TabPageNonMusicFiles";
      this.tabPageNonMusicFiles.LocalisationContext = "main";
      this.tabPageNonMusicFiles.Location = new System.Drawing.Point(4, 4);
      this.tabPageNonMusicFiles.Name = "tabPageNonMusicFiles";
      this.tabPageNonMusicFiles.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageNonMusicFiles.Size = new System.Drawing.Size(671, 85);
      this.tabPageNonMusicFiles.TabIndex = 1;
      this.tabPageNonMusicFiles.Text = "Non Music Files";
      this.tabPageNonMusicFiles.UseVisualStyleBackColor = true;
      this.tabPageNonMusicFiles.Visible = false;
      // 
      // listViewNonMusicFiles
      // 
      this.listViewNonMusicFiles.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewNonMusicFiles.Location = new System.Drawing.Point(3, 3);
      this.listViewNonMusicFiles.Name = "listViewNonMusicFiles";
      this.listViewNonMusicFiles.Size = new System.Drawing.Size(665, 79);
      this.listViewNonMusicFiles.TabIndex = 0;
      this.listViewNonMusicFiles.UseCompatibleStateImageBehavior = false;
      this.listViewNonMusicFiles.View = System.Windows.Forms.View.List;
      this.listViewNonMusicFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewNonMusicFiles_AfterLabelEdit);
      this.listViewNonMusicFiles.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewNonMusicFiles_BeforeLabelEdit);
      this.listViewNonMusicFiles.DoubleClick += new System.EventHandler(this.listViewNonMusicFiles_DoubleClick);
      // 
      // tabControlMisc
      // 
      this.tabControlMisc.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this.tabControlMisc.Controls.Add(this.tabPageNonMusicFiles);
      this.tabControlMisc.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlMisc.Location = new System.Drawing.Point(0, 0);
      this.tabControlMisc.Name = "tabControlMisc";
      this.tabControlMisc.SelectedIndex = 0;
      this.tabControlMisc.Size = new System.Drawing.Size(679, 111);
      this.tabControlMisc.TabIndex = 0;
      // 
      // MiscInfoControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControlMisc);
      this.Name = "MiscInfoControl";
      this.Size = new System.Drawing.Size(679, 111);
      this.tabPageNonMusicFiles.ResumeLayout(false);
      this.tabControlMisc.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTTabPage tabPageNonMusicFiles;
    private System.Windows.Forms.TabControl tabControlMisc;
    private System.Windows.Forms.ListView listViewNonMusicFiles;
  }
}
