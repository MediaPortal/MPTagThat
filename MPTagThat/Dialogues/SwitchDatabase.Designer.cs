namespace MPTagThat.Dialogues
{
  partial class SwitchDatabase
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SwitchDatabase));
      this.btClose = new MPTagThat.Core.WinControls.MPTButton();
      this.dataGridViewDatabases = new System.Windows.Forms.DataGridView();
      this.DatabaseDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DatabaseSwitch = new System.Windows.Forms.DataGridViewImageColumn();
      this.DatabaseDelete = new System.Windows.Forms.DataGridViewImageColumn();
      this.lbDatabaseDescription = new MPTagThat.Core.WinControls.MPTLabel();
      this.tbDatabaseDescription = new System.Windows.Forms.TextBox();
      this.btDatabaseAdd = new MPTagThat.Core.WinControls.MPTButton();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDatabases)).BeginInit();
      this.SuspendLayout();
      // 
      // btClose
      // 
      this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btClose.Id = "120f9e51-396f-492b-9cb8-e2489c5543d8";
      this.btClose.Localisation = "Close";
      this.btClose.LocalisationContext = "dbswitch";
      this.btClose.Location = new System.Drawing.Point(260, 351);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(75, 23);
      this.btClose.TabIndex = 4;
      this.btClose.Text = "Close";
      this.btClose.UseVisualStyleBackColor = true;
      this.btClose.Click += new System.EventHandler(this.btClose_Click);
      // 
      // dataGridViewDatabases
      // 
      this.dataGridViewDatabases.AllowUserToAddRows = false;
      this.dataGridViewDatabases.AllowUserToDeleteRows = false;
      this.dataGridViewDatabases.AllowUserToResizeColumns = false;
      this.dataGridViewDatabases.AllowUserToResizeRows = false;
      this.dataGridViewDatabases.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.dataGridViewDatabases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewDatabases.ColumnHeadersVisible = false;
      this.dataGridViewDatabases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DatabaseDescription,
            this.DatabaseSwitch,
            this.DatabaseDelete});
      this.dataGridViewDatabases.Location = new System.Drawing.Point(30, 30);
      this.dataGridViewDatabases.Name = "dataGridViewDatabases";
      this.dataGridViewDatabases.ReadOnly = true;
      this.dataGridViewDatabases.RowHeadersVisible = false;
      this.dataGridViewDatabases.Size = new System.Drawing.Size(520, 230);
      this.dataGridViewDatabases.TabIndex = 5;
      this.dataGridViewDatabases.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDatabases_CellContentClick);
      this.dataGridViewDatabases.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewDatabases_DataError);
      // 
      // DatabaseDescription
      // 
      this.DatabaseDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.DatabaseDescription.DataPropertyName = "DatabaseDescription";
      this.DatabaseDescription.HeaderText = "";
      this.DatabaseDescription.Name = "DatabaseDescription";
      this.DatabaseDescription.ReadOnly = true;
      // 
      // DatabaseSwitch
      // 
      this.DatabaseSwitch.HeaderText = "";
      this.DatabaseSwitch.Image = ((System.Drawing.Image)(resources.GetObject("DatabaseSwitch.Image")));
      this.DatabaseSwitch.Name = "DatabaseSwitch";
      this.DatabaseSwitch.ReadOnly = true;
      this.DatabaseSwitch.Width = 50;
      // 
      // DatabaseDelete
      // 
      this.DatabaseDelete.HeaderText = "";
      this.DatabaseDelete.Image = ((System.Drawing.Image)(resources.GetObject("DatabaseDelete.Image")));
      this.DatabaseDelete.Name = "DatabaseDelete";
      this.DatabaseDelete.ReadOnly = true;
      this.DatabaseDelete.Width = 50;
      // 
      // lbDatabaseDescription
      // 
      this.lbDatabaseDescription.Localisation = "databasedescription";
      this.lbDatabaseDescription.LocalisationContext = "dbswitch";
      this.lbDatabaseDescription.Location = new System.Drawing.Point(30, 288);
      this.lbDatabaseDescription.Name = "lbDatabaseDescription";
      this.lbDatabaseDescription.Size = new System.Drawing.Size(176, 23);
      this.lbDatabaseDescription.TabIndex = 6;
      this.lbDatabaseDescription.Text = "Database Description:";
      // 
      // tbDatabaseDescription
      // 
      this.tbDatabaseDescription.Location = new System.Drawing.Point(171, 288);
      this.tbDatabaseDescription.Name = "tbDatabaseDescription";
      this.tbDatabaseDescription.Size = new System.Drawing.Size(258, 20);
      this.tbDatabaseDescription.TabIndex = 7;
      // 
      // btDatabaseAdd
      // 
      this.btDatabaseAdd.Id = "cc37e616-3cf8-4b2b-a903-20eb40aa3714";
      this.btDatabaseAdd.Localisation = "databaseadd";
      this.btDatabaseAdd.LocalisationContext = "dbswitch";
      this.btDatabaseAdd.Location = new System.Drawing.Point(432, 285);
      this.btDatabaseAdd.Name = "btDatabaseAdd";
      this.btDatabaseAdd.Size = new System.Drawing.Size(118, 23);
      this.btDatabaseAdd.TabIndex = 8;
      this.btDatabaseAdd.Text = "Add Database";
      this.btDatabaseAdd.UseVisualStyleBackColor = true;
      this.btDatabaseAdd.Click += new System.EventHandler(this.btDatabaseAdd_Click);
      // 
      // SwitchDatabase
      // 
      this.AcceptButton = this.btClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderColor = System.Drawing.Color.Silver;
      this.CancelButton = this.btClose;
      this.ClientSize = new System.Drawing.Size(580, 400);
      this.ControlBox = false;
      this.Controls.Add(this.btDatabaseAdd);
      this.Controls.Add(this.tbDatabaseDescription);
      this.Controls.Add(this.lbDatabaseDescription);
      this.Controls.Add(this.dataGridViewDatabases);
      this.Controls.Add(this.btClose);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "SwitchDatabase";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "SwitchDatabase";
      this.Load += new System.EventHandler(this.SwitchDatabase_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDatabases)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Core.WinControls.MPTButton btClose;
    private System.Windows.Forms.DataGridView dataGridViewDatabases;
    private Core.WinControls.MPTLabel lbDatabaseDescription;
    private System.Windows.Forms.TextBox tbDatabaseDescription;
    private Core.WinControls.MPTButton btDatabaseAdd;
    private System.Windows.Forms.DataGridViewTextBoxColumn DatabaseDescription;
    private System.Windows.Forms.DataGridViewImageColumn DatabaseSwitch;
    private System.Windows.Forms.DataGridViewImageColumn DatabaseDelete;
  }
}
