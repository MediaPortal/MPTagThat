namespace MPTagThat.Dialogues
{
  partial class Preview
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
      this.dataGridViewPreview = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridViewPreview
      // 
      this.dataGridViewPreview.AllowUserToAddRows = false;
      this.dataGridViewPreview.AllowUserToDeleteRows = false;
      this.dataGridViewPreview.AllowUserToResizeRows = false;
      this.dataGridViewPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridViewPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewPreview.Location = new System.Drawing.Point(13, 12);
      this.dataGridViewPreview.MultiSelect = false;
      this.dataGridViewPreview.Name = "dataGridViewPreview";
      this.dataGridViewPreview.ReadOnly = true;
      this.dataGridViewPreview.RowHeadersVisible = false;
      this.dataGridViewPreview.Size = new System.Drawing.Size(755, 197);
      this.dataGridViewPreview.TabIndex = 0;
      // 
      // Preview
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.dataGridViewPreview);
      this.MaximumSize = new System.Drawing.Size(780, 375);
      this.Name = "Preview";
      this.Size = new System.Drawing.Size(780, 222);
      this.Load += new System.EventHandler(this.OnLoad);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridViewPreview;
  }
}