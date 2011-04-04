namespace MPTagThat.GridView
{
  partial class GridViewBurn
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
      CleanupBurnDirectory();
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
      this.panelTop = new MPTagThat.Core.WinControls.MPTPanel();
      this.lbMediaInfo = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbUsed = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbBurningStatus = new MPTagThat.Core.WinControls.MPTLabel();
      this.panelMiddle = new MPTagThat.Core.WinControls.MPTPanel();
      this.dataGridViewBurn = new System.Windows.Forms.DataGridView();
      this.panelTop.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBurn)).BeginInit();
      this.SuspendLayout();
      // 
      // panelTop
      // 
      this.panelTop.BackColor = System.Drawing.Color.SteelBlue;
      this.panelTop.Controls.Add(this.lbMediaInfo);
      this.panelTop.Controls.Add(this.lbUsed);
      this.panelTop.Controls.Add(this.lbBurningStatus);
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(527, 81);
      this.panelTop.TabIndex = 2;
      // 
      // lbMediaInfo
      // 
      this.lbMediaInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbMediaInfo.ForeColor = System.Drawing.Color.White;
      this.lbMediaInfo.Localisation = "lbMediaInfo";
      this.lbMediaInfo.LocalisationContext = "panelTop";
      this.lbMediaInfo.Location = new System.Drawing.Point(10, 6);
      this.lbMediaInfo.Name = "lbMediaInfo";
      this.lbMediaInfo.Size = new System.Drawing.Size(137, 20);
      this.lbMediaInfo.TabIndex = 2;
      this.lbMediaInfo.Text = "No Media inserted";
      // 
      // lbUsed
      // 
      this.lbUsed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbUsed.ForeColor = System.Drawing.Color.White;
      this.lbUsed.Localisation = "lbUsed";
      this.lbUsed.LocalisationContext = "panelTop";
      this.lbUsed.Location = new System.Drawing.Point(10, 31);
      this.lbUsed.Name = "lbUsed";
      this.lbUsed.Size = new System.Drawing.Size(155, 20);
      this.lbUsed.TabIndex = 1;
      this.lbUsed.Text = "Used: 0:0 ( 0 Tracks)";
      // 
      // lbBurningStatus
      // 
      this.lbBurningStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbBurningStatus.ForeColor = System.Drawing.Color.White;
      this.lbBurningStatus.Localisation = "lbBurningStatus";
      this.lbBurningStatus.LocalisationContext = "panelTop";
      this.lbBurningStatus.Location = new System.Drawing.Point(10, 55);
      this.lbBurningStatus.Name = "lbBurningStatus";
      this.lbBurningStatus.Size = new System.Drawing.Size(80, 20);
      this.lbBurningStatus.TabIndex = 0;
      this.lbBurningStatus.Text = "Burning ...";
      // 
      // panelMiddle
      // 
      this.panelMiddle.BackColor = System.Drawing.SystemColors.Control;
      this.panelMiddle.Controls.Add(this.dataGridViewBurn);
      this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddle.Location = new System.Drawing.Point(0, 81);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(527, 355);
      this.panelMiddle.TabIndex = 3;
      // 
      // dataGridViewBurn
      // 
      this.dataGridViewBurn.AllowDrop = true;
      this.dataGridViewBurn.AllowUserToAddRows = false;
      this.dataGridViewBurn.AllowUserToDeleteRows = false;
      this.dataGridViewBurn.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
      this.dataGridViewBurn.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewBurn.BackgroundColor = System.Drawing.Color.White;
      this.dataGridViewBurn.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridViewBurn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridViewBurn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewBurn.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewBurn.Location = new System.Drawing.Point(0, 0);
      this.dataGridViewBurn.Name = "dataGridViewBurn";
      this.dataGridViewBurn.ReadOnly = true;
      this.dataGridViewBurn.RowHeadersVisible = false;
      this.dataGridViewBurn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewBurn.Size = new System.Drawing.Size(527, 355);
      this.dataGridViewBurn.TabIndex = 0;
      this.dataGridViewBurn.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.OnCellPainting);
      this.dataGridViewBurn.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewBurn_RowsAdded);
      this.dataGridViewBurn.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewBurn_RowsRemoved);
      this.dataGridViewBurn.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
      this.dataGridViewBurn.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
      this.dataGridViewBurn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
      this.dataGridViewBurn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
      // 
      // GridViewBurn
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panelMiddle);
      this.Controls.Add(this.panelTop);
      this.Name = "GridViewBurn";
      this.Size = new System.Drawing.Size(527, 436);
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.panelMiddle.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBurn)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTPanel panelTop;
    private MPTagThat.Core.WinControls.MPTPanel panelMiddle;
    private System.Windows.Forms.DataGridView dataGridViewBurn;
    private MPTagThat.Core.WinControls.MPTLabel lbBurningStatus;
    private MPTagThat.Core.WinControls.MPTLabel lbMediaInfo;
    private MPTagThat.Core.WinControls.MPTLabel lbUsed;
  }
}
