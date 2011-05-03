namespace MPTagThat.Core
{
  partial class ReadOnlyDialog
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
      this.labelHeader = new MPTagThat.Core.WinControls.MPTLabel();
      this.lbExplanation = new MPTagThat.Core.WinControls.MPTLabel();
      this.btYes = new MPTagThat.Core.WinControls.MPTButton();
      this.btYesToAll = new MPTagThat.Core.WinControls.MPTButton();
      this.btNo = new MPTagThat.Core.WinControls.MPTButton();
      this.btNoToAll = new MPTagThat.Core.WinControls.MPTButton();
      this.lbFile = new MPTagThat.Core.WinControls.MPTLabel();
      this.SuspendLayout();
      // 
      // labelHeader
      // 
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.ForeColor = System.Drawing.Color.DarkGray;
      this.labelHeader.Localisation = "labelHeader";
      this.labelHeader.LocalisationContext = "ShapedForm";
      this.labelHeader.Location = new System.Drawing.Point(21, 19);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(62, 20);
      this.labelHeader.TabIndex = 24;
      this.labelHeader.Text = "Header";
      // 
      // lbExplanation
      // 
      this.lbExplanation.Localisation = "Explanation";
      this.lbExplanation.LocalisationContext = "readonly";
      this.lbExplanation.Location = new System.Drawing.Point(21, 89);
      this.lbExplanation.Name = "lbExplanation";
      this.lbExplanation.Size = new System.Drawing.Size(529, 61);
      this.lbExplanation.TabIndex = 25;
      // 
      // btYes
      // 
      this.btYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
      this.btYes.Id = "ac8b1fda-5390-44a9-8268-69f1bc70601d";
      this.btYes.Localisation = "Yes";
      this.btYes.LocalisationContext = "readonly";
      this.btYes.Location = new System.Drawing.Point(32, 163);
      this.btYes.Name = "btYes";
      this.btYes.Size = new System.Drawing.Size(104, 23);
      this.btYes.TabIndex = 26;
      this.btYes.Text = "Yes";
      this.btYes.UseVisualStyleBackColor = true;
      this.btYes.Click += new System.EventHandler(this.button_Click);
      // 
      // btYesToAll
      // 
      this.btYesToAll.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btYesToAll.Id = "7ace51fb-eaf0-4db0-bced-ee45622ac148";
      this.btYesToAll.Localisation = "YesToAll";
      this.btYesToAll.LocalisationContext = "readonly";
      this.btYesToAll.Location = new System.Drawing.Point(170, 163);
      this.btYesToAll.Name = "btYesToAll";
      this.btYesToAll.Size = new System.Drawing.Size(104, 23);
      this.btYesToAll.TabIndex = 27;
      this.btYesToAll.Text = "Yes to All";
      this.btYesToAll.UseVisualStyleBackColor = true;
      this.btYesToAll.Click += new System.EventHandler(this.button_Click);
      // 
      // btNo
      // 
      this.btNo.DialogResult = System.Windows.Forms.DialogResult.No;
      this.btNo.Id = "bcc817cc-6172-4adc-b018-8398dd63ef76";
      this.btNo.Localisation = "No";
      this.btNo.LocalisationContext = "readonly";
      this.btNo.Location = new System.Drawing.Point(308, 163);
      this.btNo.Name = "btNo";
      this.btNo.Size = new System.Drawing.Size(104, 23);
      this.btNo.TabIndex = 28;
      this.btNo.Text = "No";
      this.btNo.UseVisualStyleBackColor = true;
      this.btNo.Click += new System.EventHandler(this.button_Click);
      // 
      // btNoToAll
      // 
      this.btNoToAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btNoToAll.Id = "4d31488a-14fd-4ca9-b4a3-8695cd82c670";
      this.btNoToAll.Localisation = "NoToAll";
      this.btNoToAll.LocalisationContext = "readonly";
      this.btNoToAll.Location = new System.Drawing.Point(446, 163);
      this.btNoToAll.Name = "btNoToAll";
      this.btNoToAll.Size = new System.Drawing.Size(104, 23);
      this.btNoToAll.TabIndex = 30;
      this.btNoToAll.Text = "No to All";
      this.btNoToAll.UseVisualStyleBackColor = true;
      this.btNoToAll.Click += new System.EventHandler(this.button_Click);
      // 
      // lbFile
      // 
      this.lbFile.Location = new System.Drawing.Point(21, 52);
      this.lbFile.Name = "lbFile";
      this.lbFile.Size = new System.Drawing.Size(529, 24);
      this.lbFile.TabIndex = 31;
      // 
      // ReadOnlyDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderColor = System.Drawing.Color.Silver;
      this.ClientSize = new System.Drawing.Size(587, 214);
      this.ControlBox = false;
      this.Controls.Add(this.lbFile);
      this.Controls.Add(this.btNoToAll);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.lbExplanation);
      this.Controls.Add(this.btYes);
      this.Controls.Add(this.btNo);
      this.Controls.Add(this.btYesToAll);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "ReadOnlyDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.TopMost = true;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Core.WinControls.MPTLabel labelHeader;
    private Core.WinControls.MPTLabel lbExplanation;
    private Core.WinControls.MPTButton btYes;
    private Core.WinControls.MPTButton btYesToAll;
    private Core.WinControls.MPTButton btNo;
    private Core.WinControls.MPTButton btNoToAll;
    private WinControls.MPTLabel lbFile;
  }
}