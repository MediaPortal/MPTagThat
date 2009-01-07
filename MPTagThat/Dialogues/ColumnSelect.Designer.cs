namespace MPTagThat.Dialogues
{
  partial class ColumnSelect
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
      this.lblHeading = new MPTagThat.Core.WinControls.MPTLabel();
      this.btOk = new MPTagThat.Core.WinControls.MPTButton();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.lvColumns = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // lblHeading
      // 
      this.lblHeading.AutoSize = true;
      this.lblHeading.Localisation = "Header";
      this.lblHeading.LocalisationContext = "ColumnSelect";
      this.lblHeading.Location = new System.Drawing.Point(12, 20);
      this.lblHeading.Name = "lblHeading";
      this.lblHeading.Size = new System.Drawing.Size(147, 13);
      this.lblHeading.TabIndex = 0;
      this.lblHeading.Text = "Select the Columns to Display";
      // 
      // btOk
      // 
      this.btOk.AutoSize = true;
      this.btOk.Localisation = "Ok";
      this.btOk.LocalisationContext = "ColumnSelect";
      this.btOk.Location = new System.Drawing.Point(71, 530);
      this.btOk.Name = "btOk";
      this.btOk.Size = new System.Drawing.Size(75, 23);
      this.btOk.TabIndex = 2;
      this.btOk.Text = "Ok";
      this.btOk.UseVisualStyleBackColor = true;
      this.btOk.Click += new System.EventHandler(this.btOk_Click);
      // 
      // btCancel
      // 
      this.btCancel.AutoSize = true;
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "ColumnSelect";
      this.btCancel.Location = new System.Drawing.Point(251, 530);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(75, 23);
      this.btCancel.TabIndex = 3;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      // 
      // lvColumns
      // 
      this.lvColumns.CheckBoxes = true;
      this.lvColumns.Location = new System.Drawing.Point(15, 47);
      this.lvColumns.Name = "lvColumns";
      this.lvColumns.Size = new System.Drawing.Size(400, 466);
      this.lvColumns.TabIndex = 4;
      this.lvColumns.UseCompatibleStateImageBehavior = false;
      this.lvColumns.View = System.Windows.Forms.View.List;
      // 
      // ColumnSelect
      // 
      this.AcceptButton = this.btOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.CancelButton = this.btCancel;
      this.ClientSize = new System.Drawing.Size(437, 585);
      this.ControlBox = false;
      this.Controls.Add(this.lvColumns);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btOk);
      this.Controls.Add(this.lblHeading);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "ColumnSelect";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "ColumnSelect";
      this.Load += new System.EventHandler(this.OnLoad);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTLabel lblHeading;
    private MPTagThat.Core.WinControls.MPTButton btOk;
    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private System.Windows.Forms.ListView lvColumns;
  }
}