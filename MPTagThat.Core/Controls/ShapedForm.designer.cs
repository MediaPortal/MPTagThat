namespace MPTagThat.Core
{
  partial class ShapedForm
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
      this.labelResize = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // labelResize
      // 
      this.labelResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResize.AutoSize = true;
      this.labelResize.BackColor = System.Drawing.Color.Transparent;
      this.labelResize.Font = new System.Drawing.Font("Marlett", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.labelResize.Location = new System.Drawing.Point(253, 232);
      this.labelResize.Name = "labelResize";
      this.labelResize.Size = new System.Drawing.Size(17, 11);
      this.labelResize.TabIndex = 3;
      this.labelResize.Text = "o";
      // 
      // ShapedForm
      // 
      //this.ClientSize = new System.Drawing.Size(268, 243);
      this.Controls.Add(this.labelResize);
      this.Name = "ShapedForm";
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion


    private System.Windows.Forms.Label labelResize;
  }
}
