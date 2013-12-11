namespace MPTagThat.GridView
{
  partial class GridViewRip
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
      this.panelTop = new MPTagThat.Core.WinControls.MPTPanel();
      this.lbRippingStatus = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbYear = new System.Windows.Forms.TextBox();
      this.tbGenre = new System.Windows.Forms.TextBox();
      this.lbYear = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbGenre = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbAlbum = new System.Windows.Forms.TextBox();
      this.tbAlbumArtist = new System.Windows.Forms.TextBox();
      this.lbAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbAlbumArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.panelMiddle = new MPTagThat.Core.WinControls.MPTPanel();
      this.dataGridViewRip = new System.Windows.Forms.DataGridView();
      this.leftAdjustmentPanel = new MPTagThat.Core.WinControls.MPTPanel();
      this.panelTop.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRip)).BeginInit();
      this.SuspendLayout();
      // 
      // panelTop
      // 
      this.panelTop.BackColor = System.Drawing.Color.SteelBlue;
      this.panelTop.Controls.Add(this.lbRippingStatus);
      this.panelTop.Controls.Add(this.tbYear);
      this.panelTop.Controls.Add(this.tbGenre);
      this.panelTop.Controls.Add(this.lbYear);
      this.panelTop.Controls.Add(this.lbGenre);
      this.panelTop.Controls.Add(this.tbAlbum);
      this.panelTop.Controls.Add(this.tbAlbumArtist);
      this.panelTop.Controls.Add(this.lbAlbum);
      this.panelTop.Controls.Add(this.lbAlbumArtist);
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(800, 81);
      this.panelTop.TabIndex = 0;
      // 
      // lbRippingStatus
      // 
      this.lbRippingStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbRippingStatus.ForeColor = System.Drawing.Color.White;
      this.lbRippingStatus.Localisation = "lbRippingStatus";
      this.lbRippingStatus.LocalisationContext = "panelTop";
      this.lbRippingStatus.Location = new System.Drawing.Point(6, 58);
      this.lbRippingStatus.Name = "lbRippingStatus";
      this.lbRippingStatus.Size = new System.Drawing.Size(79, 20);
      this.lbRippingStatus.TabIndex = 8;
      this.lbRippingStatus.Text = "Ripping ...";
      // 
      // tbYear
      // 
      this.tbYear.ForeColor = System.Drawing.SystemColors.WindowText;
      this.tbYear.Location = new System.Drawing.Point(494, 28);
      this.tbYear.Name = "tbYear";
      this.tbYear.Size = new System.Drawing.Size(92, 20);
      this.tbYear.TabIndex = 7;
      // 
      // tbGenre
      // 
      this.tbGenre.ForeColor = System.Drawing.SystemColors.WindowText;
      this.tbGenre.Location = new System.Drawing.Point(494, 4);
      this.tbGenre.Name = "tbGenre";
      this.tbGenre.Size = new System.Drawing.Size(201, 20);
      this.tbGenre.TabIndex = 6;
      // 
      // lbYear
      // 
      this.lbYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbYear.ForeColor = System.Drawing.Color.White;
      this.lbYear.Localisation = "Year";
      this.lbYear.LocalisationContext = "TagEdit";
      this.lbYear.Location = new System.Drawing.Point(386, 29);
      this.lbYear.Name = "lbYear";
      this.lbYear.Size = new System.Drawing.Size(47, 20);
      this.lbYear.TabIndex = 5;
      this.lbYear.Text = "Year:";
      // 
      // lbGenre
      // 
      this.lbGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbGenre.ForeColor = System.Drawing.Color.White;
      this.lbGenre.Localisation = "Genre";
      this.lbGenre.LocalisationContext = "TagEdit";
      this.lbGenre.Location = new System.Drawing.Point(386, 4);
      this.lbGenre.Name = "lbGenre";
      this.lbGenre.Size = new System.Drawing.Size(58, 20);
      this.lbGenre.TabIndex = 4;
      this.lbGenre.Text = "Genre:";
      // 
      // tbAlbum
      // 
      this.tbAlbum.ForeColor = System.Drawing.SystemColors.WindowText;
      this.tbAlbum.Location = new System.Drawing.Point(116, 28);
      this.tbAlbum.Name = "tbAlbum";
      this.tbAlbum.Size = new System.Drawing.Size(201, 20);
      this.tbAlbum.TabIndex = 3;
      // 
      // tbAlbumArtist
      // 
      this.tbAlbumArtist.ForeColor = System.Drawing.SystemColors.WindowText;
      this.tbAlbumArtist.Location = new System.Drawing.Point(116, 4);
      this.tbAlbumArtist.Name = "tbAlbumArtist";
      this.tbAlbumArtist.Size = new System.Drawing.Size(201, 20);
      this.tbAlbumArtist.TabIndex = 2;
      // 
      // lbAlbum
      // 
      this.lbAlbum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbAlbum.ForeColor = System.Drawing.Color.White;
      this.lbAlbum.Localisation = "Album";
      this.lbAlbum.LocalisationContext = "TagEdit";
      this.lbAlbum.Location = new System.Drawing.Point(8, 29);
      this.lbAlbum.Name = "lbAlbum";
      this.lbAlbum.Size = new System.Drawing.Size(58, 20);
      this.lbAlbum.TabIndex = 1;
      this.lbAlbum.Text = "Album:";
      // 
      // lbAlbumArtist
      // 
      this.lbAlbumArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbAlbumArtist.ForeColor = System.Drawing.Color.White;
      this.lbAlbumArtist.Localisation = "AlbumArtist";
      this.lbAlbumArtist.LocalisationContext = "TagEdit";
      this.lbAlbumArtist.Location = new System.Drawing.Point(8, 4);
      this.lbAlbumArtist.Name = "lbAlbumArtist";
      this.lbAlbumArtist.Size = new System.Drawing.Size(99, 20);
      this.lbAlbumArtist.TabIndex = 0;
      this.lbAlbumArtist.Text = "Album Artist:";
      // 
      // panelMiddle
      // 
      this.panelMiddle.Controls.Add(this.dataGridViewRip);
      this.panelMiddle.Controls.Add(this.leftAdjustmentPanel);
      this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMiddle.Location = new System.Drawing.Point(0, 81);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(800, 300);
      this.panelMiddle.TabIndex = 2;
      // 
      // dataGridViewRip
      // 
      this.dataGridViewRip.AllowUserToAddRows = false;
      this.dataGridViewRip.AllowUserToDeleteRows = false;
      this.dataGridViewRip.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
      this.dataGridViewRip.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewRip.BackgroundColor = System.Drawing.Color.White;
      this.dataGridViewRip.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
      this.dataGridViewRip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewRip.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewRip.Location = new System.Drawing.Point(10, 0);
      this.dataGridViewRip.Name = "dataGridViewRip";
      this.dataGridViewRip.RowHeadersVisible = false;
      this.dataGridViewRip.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewRip.Size = new System.Drawing.Size(790, 300);
      this.dataGridViewRip.TabIndex = 0;
      // 
      // leftAdjustmentPanel
      // 
      this.leftAdjustmentPanel.Dock = System.Windows.Forms.DockStyle.Left;
      this.leftAdjustmentPanel.Location = new System.Drawing.Point(0, 0);
      this.leftAdjustmentPanel.Name = "leftAdjustmentPanel";
      this.leftAdjustmentPanel.Size = new System.Drawing.Size(10, 300);
      this.leftAdjustmentPanel.TabIndex = 1;
      // 
      // GridViewRip
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panelMiddle);
      this.Controls.Add(this.panelTop);
      this.Name = "GridViewRip";
      this.Size = new System.Drawing.Size(800, 381);
      this.VisibleChanged += new System.EventHandler(this.GridViewRip_VisibleChanged);
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.panelMiddle.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRip)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTPanel panelTop;
    private MPTagThat.Core.WinControls.MPTPanel panelMiddle;
    private System.Windows.Forms.DataGridView dataGridViewRip;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbum;
    private MPTagThat.Core.WinControls.MPTLabel lbAlbumArtist;
    private System.Windows.Forms.TextBox tbAlbum;
    private System.Windows.Forms.TextBox tbAlbumArtist;
    private System.Windows.Forms.TextBox tbYear;
    private System.Windows.Forms.TextBox tbGenre;
    private MPTagThat.Core.WinControls.MPTLabel lbYear;
    private MPTagThat.Core.WinControls.MPTLabel lbGenre;
    private MPTagThat.Core.WinControls.MPTLabel lbRippingStatus;
    private Core.WinControls.MPTPanel leftAdjustmentPanel;
  }
}
