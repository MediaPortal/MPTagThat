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
      this.dataGridViewError = new System.Windows.Forms.DataGridView();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.tabPageMessages = new MPTagThat.Core.WinControls.MPTTabPage();
      this.tabPageNonMusicFiles = new MPTagThat.Core.WinControls.MPTTabPage();
      this.tabControlMisc = new System.Windows.Forms.TabControl();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).BeginInit();
      this.tabPageMessages.SuspendLayout();
      this.tabControlMisc.SuspendLayout();
      this.SuspendLayout();
      // 
      // dataGridViewError
      // 
      this.dataGridViewError.BackgroundColor = System.Drawing.SystemColors.Window;
      this.dataGridViewError.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dataGridViewError.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      this.dataGridViewError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewError.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
      this.dataGridViewError.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewError.Location = new System.Drawing.Point(3, 3);
      this.dataGridViewError.MultiSelect = false;
      this.dataGridViewError.Name = "dataGridViewError";
      this.dataGridViewError.ReadOnly = true;
      this.dataGridViewError.RowHeadersVisible = false;
      this.dataGridViewError.Size = new System.Drawing.Size(665, 79);
      this.dataGridViewError.TabIndex = 5;
      this.dataGridViewError.MouseClick += new System.Windows.Forms.MouseEventHandler(this.datagridViewError_MouseClick);
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.HeaderText = "File";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 300;
      // 
      // dataGridViewTextBoxColumn2
      // 
      this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.HeaderText = "Message";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      // 
      // tabPageMessages
      // 
      this.tabPageMessages.Controls.Add(this.dataGridViewError);
      this.tabPageMessages.Localisation = "TabPageMessages";
      this.tabPageMessages.LocalisationContext = "main";
      this.tabPageMessages.Location = new System.Drawing.Point(4, 4);
      this.tabPageMessages.Name = "tabPageMessages";
      this.tabPageMessages.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMessages.Size = new System.Drawing.Size(671, 85);
      this.tabPageMessages.TabIndex = 0;
      this.tabPageMessages.Text = "Messages";
      this.tabPageMessages.UseVisualStyleBackColor = true;
      // 
      // tabPageNonMusicFiles
      // 
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
      // tabControlMisc
      // 
      this.tabControlMisc.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this.tabControlMisc.Controls.Add(this.tabPageMessages);
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
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).EndInit();
      this.tabPageMessages.ResumeLayout(false);
      this.tabControlMisc.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridViewError;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageMessages;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageNonMusicFiles;
    private System.Windows.Forms.TabControl tabControlMisc;
  }
}
