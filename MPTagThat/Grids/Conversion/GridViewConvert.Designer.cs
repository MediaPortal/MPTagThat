namespace MPTagThat.GridView
{
  partial class GridViewConvert
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
      SaveSettings();
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dataGridViewConvert = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConvert)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridViewConvert
      // 
      this.dataGridViewConvert.AllowDrop = true;
      this.dataGridViewConvert.AllowUserToAddRows = false;
      this.dataGridViewConvert.AllowUserToDeleteRows = false;
      this.dataGridViewConvert.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
      this.dataGridViewConvert.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewConvert.BackgroundColor = System.Drawing.Color.White;
      this.dataGridViewConvert.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridViewConvert.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridViewConvert.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewConvert.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewConvert.Location = new System.Drawing.Point(0, 0);
      this.dataGridViewConvert.Name = "dataGridViewConvert";
      this.dataGridViewConvert.ReadOnly = true;
      this.dataGridViewConvert.RowHeadersVisible = false;
      this.dataGridViewConvert.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewConvert.Size = new System.Drawing.Size(540, 447);
      this.dataGridViewConvert.TabIndex = 1;
      this.dataGridViewConvert.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConvert_MouseClick);
      // 
      // GridViewConvert
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.dataGridViewConvert);
      this.Name = "GridViewConvert";
      this.Size = new System.Drawing.Size(540, 447);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConvert)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridViewConvert;
  }
}
