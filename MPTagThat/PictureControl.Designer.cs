namespace MPTagThat
{
  partial class PictureControl
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
      this.pictureBoxCover = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBoxCover
      // 
      this.pictureBoxCover.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBoxCover.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxCover.Name = "pictureBoxCover";
      this.pictureBoxCover.Size = new System.Drawing.Size(284, 264);
      this.pictureBoxCover.TabIndex = 0;
      this.pictureBoxCover.TabStop = false;
      this.pictureBoxCover.MouseLeave += new System.EventHandler(this.pictureBoxCover_MouseLeave);
      this.pictureBoxCover.Click += new System.EventHandler(this.pictureBoxCover_Click);
      // 
      // PictureControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 264);
      this.ControlBox = false;
      this.Controls.Add(this.pictureBoxCover);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Name = "PictureControl";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "100 x 100";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBoxCover;
  }
}