namespace MPTagThat.Dialogues
{
  partial class SplashForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
      this.roundRectShape1 = new Telerik.WinControls.RoundRectShape();
      this.pictureBoxAbout = new System.Windows.Forms.PictureBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.lbVersion = new System.Windows.Forms.Label();
      this.lbDate = new System.Windows.Forms.Label();
      this.lbStatus = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAbout)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBoxAbout
      // 
      this.pictureBoxAbout.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxAbout.Image")));
      this.pictureBoxAbout.Location = new System.Drawing.Point(12, 35);
      this.pictureBoxAbout.Name = "pictureBoxAbout";
      this.pictureBoxAbout.Size = new System.Drawing.Size(163, 162);
      this.pictureBoxAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBoxAbout.TabIndex = 9;
      this.pictureBoxAbout.TabStop = false;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(196, 35);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(308, 50);
      this.pictureBox1.TabIndex = 10;
      this.pictureBox1.TabStop = false;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(196, 111);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(57, 16);
      this.label1.TabIndex = 11;
      this.label1.Text = "Version:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(196, 138);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(73, 16);
      this.label2.TabIndex = 12;
      this.label2.Text = "Build Date:";
      // 
      // lbVersion
      // 
      this.lbVersion.AutoSize = true;
      this.lbVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbVersion.Location = new System.Drawing.Point(318, 111);
      this.lbVersion.Name = "lbVersion";
      this.lbVersion.Size = new System.Drawing.Size(45, 16);
      this.lbVersion.TabIndex = 13;
      this.lbVersion.Text = "1.0.0.0";
      // 
      // lbDate
      // 
      this.lbDate.AutoSize = true;
      this.lbDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbDate.Location = new System.Drawing.Point(318, 138);
      this.lbDate.Name = "lbDate";
      this.lbDate.Size = new System.Drawing.Size(72, 16);
      this.lbDate.TabIndex = 14;
      this.lbDate.Text = "2009-01-01";
      // 
      // lbStatus
      // 
      this.lbStatus.AutoSize = true;
      this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbStatus.Location = new System.Drawing.Point(196, 181);
      this.lbStatus.Name = "lbStatus";
      this.lbStatus.Size = new System.Drawing.Size(93, 16);
      this.lbStatus.TabIndex = 15;
      this.lbStatus.Text = ".....  Loading ....";
      // 
      // SplashForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(538, 238);
      this.Controls.Add(this.lbStatus);
      this.Controls.Add(this.lbDate);
      this.Controls.Add(this.lbVersion);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.pictureBoxAbout);
      this.Name = "SplashForm";
      this.Opacity = 0.75;
      this.Shape = this.roundRectShape1;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SplashForm";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAbout)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Telerik.WinControls.RoundRectShape roundRectShape1;
    private System.Windows.Forms.PictureBox pictureBoxAbout;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lbVersion;
    private System.Windows.Forms.Label lbDate;
    private System.Windows.Forms.Label lbStatus;
  }
}