namespace MPTagThat.Dialogues
{
  partial class Progress
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
      this.buttonCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.progressBarScanning = new System.Windows.Forms.ProgressBar();
      this.labelStatus = new MPTagThat.Core.WinControls.MPTLabel();
      this.labelStatus2 = new MPTagThat.Core.WinControls.MPTLabel();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Localisation = "Cancel";
      this.buttonCancel.LocalisationContext = "Progress";
      this.buttonCancel.Location = new System.Drawing.Point(191, 103);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(83, 23);
      this.buttonCancel.TabIndex = 0;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // progressBarScanning
      // 
      this.progressBarScanning.Location = new System.Drawing.Point(32, 68);
      this.progressBarScanning.Name = "progressBarScanning";
      this.progressBarScanning.Size = new System.Drawing.Size(400, 23);
      this.progressBarScanning.TabIndex = 1;
      // 
      // labelStatus
      // 
      this.labelStatus.AutoSize = true;
      this.labelStatus.Localisation = "labelStatus";
      this.labelStatus.LocalisationContext = "Progress";
      this.labelStatus.Location = new System.Drawing.Point(31, 15);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new System.Drawing.Size(37, 13);
      this.labelStatus.TabIndex = 2;
      this.labelStatus.Text = "Status";
      // 
      // labelStatus2
      // 
      this.labelStatus2.AutoSize = true;
      this.labelStatus2.Localisation = "labelStatus2";
      this.labelStatus2.LocalisationContext = "Progress";
      this.labelStatus2.Location = new System.Drawing.Point(31, 41);
      this.labelStatus2.Name = "labelStatus2";
      this.labelStatus2.Size = new System.Drawing.Size(37, 13);
      this.labelStatus2.TabIndex = 3;
      this.labelStatus2.Text = "Status";
      // 
      // Progress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ClientSize = new System.Drawing.Size(467, 134);
      this.Controls.Add(this.labelStatus2);
      this.Controls.Add(this.labelStatus);
      this.Controls.Add(this.progressBarScanning);
      this.Controls.Add(this.buttonCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Progress";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "ScanningProgress";
      this.TopMost = true;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTButton buttonCancel;
    private System.Windows.Forms.ProgressBar progressBarScanning;
    private MPTagThat.Core.WinControls.MPTLabel labelStatus;
    private MPTagThat.Core.WinControls.MPTLabel labelStatus2;
  }
}