namespace MPTagThat.GridView
{
  partial class FreeDBMultiCDSelect
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
        this.lbMultipleMatches = new MPTagThat.Core.WinControls.MPTLabel();
        this.listBoxCDMatches = new System.Windows.Forms.ListBox();
        this.buttonOK = new MPTagThat.Core.WinControls.MPTButton();
        this.buttonCancel = new MPTagThat.Core.WinControls.MPTButton();
        this.SuspendLayout();
        // 
        // lbMultipleMatches
        // 
        this.lbMultipleMatches.AutoSize = true;
        this.lbMultipleMatches.Localisation = "MultipleMatches";
        this.lbMultipleMatches.LocalisationContext = "FreeDB";
        this.lbMultipleMatches.Location = new System.Drawing.Point(23, 18);
        this.lbMultipleMatches.Name = "lbMultipleMatches";
        this.lbMultipleMatches.Size = new System.Drawing.Size(297, 13);
        this.lbMultipleMatches.TabIndex = 0;
        this.lbMultipleMatches.Text = "Multiple matches found in FreeDB. Please select a valid disk: ";
        // 
        // listBoxCDMatches
        // 
        this.listBoxCDMatches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.listBoxCDMatches.FormattingEnabled = true;
        this.listBoxCDMatches.Location = new System.Drawing.Point(26, 43);
        this.listBoxCDMatches.Name = "listBoxCDMatches";
        this.listBoxCDMatches.Size = new System.Drawing.Size(519, 251);
        this.listBoxCDMatches.TabIndex = 1;
        this.listBoxCDMatches.DoubleClick += new System.EventHandler(this.listBoxCDMatches_DoubleClick);
        // 
        // buttonOK
        // 
        this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.buttonOK.Localisation = "OK";
        this.buttonOK.LocalisationContext = "FreeDB";
        this.buttonOK.Location = new System.Drawing.Point(389, 309);
        this.buttonOK.Name = "buttonOK";
        this.buttonOK.Size = new System.Drawing.Size(75, 23);
        this.buttonOK.TabIndex = 2;
        this.buttonOK.Text = "Ok";
        this.buttonOK.UseVisualStyleBackColor = true;
        this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
        // 
        // buttonCancel
        // 
        this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.buttonCancel.Localisation = "Cancel";
        this.buttonCancel.LocalisationContext = "FreeDB";
        this.buttonCancel.Location = new System.Drawing.Point(470, 309);
        this.buttonCancel.Name = "buttonCancel";
        this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        this.buttonCancel.TabIndex = 3;
        this.buttonCancel.Text = "Cancel";
        this.buttonCancel.UseVisualStyleBackColor = true;
        this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        // 
        // FreeDBMultiCDSelect
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.LightSteelBlue;
        this.ClientSize = new System.Drawing.Size(568, 347);
        this.Controls.Add(this.buttonCancel);
        this.Controls.Add(this.buttonOK);
        this.Controls.Add(this.listBoxCDMatches);
        this.Controls.Add(this.lbMultipleMatches);
        this.Name = "FreeDBMultiCDSelect";
        this.Text = "FreeDBMultiCDSelect";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTLabel lbMultipleMatches;
    private System.Windows.Forms.ListBox listBoxCDMatches;
    private MPTagThat.Core.WinControls.MPTButton buttonOK;
    private MPTagThat.Core.WinControls.MPTButton buttonCancel;
  }
}