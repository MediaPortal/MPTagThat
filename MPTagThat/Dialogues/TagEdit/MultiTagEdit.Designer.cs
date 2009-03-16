namespace MPTagThat.TagEdit
{
  partial class MultiTagEdit
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
      this.ckTrackLength = new MPTagThat.Core.WinControls.MPTCheckBox();
      this.groupBoxMedia.Controls.Add(this.ckTrackLength);

      // 
      // ckTrackLength
      // 
      this.ckTrackLength.AutoSize = true;
      this.ckTrackLength.Localisation = "GetTrackLength";
      this.ckTrackLength.LocalisationContext = "TagEdit";
      this.ckTrackLength.Location = new System.Drawing.Point(205, 56);
      this.ckTrackLength.Name = "ckTrackLength";
      this.ckTrackLength.Size = new System.Drawing.Size(183, 20);
      this.ckTrackLength.TabIndex = 47;
      this.ckTrackLength.Text = "Get Track Length from File";
      this.ckTrackLength.UseVisualStyleBackColor = true;
    }
    #endregion

    private MPTagThat.Core.WinControls.MPTCheckBox ckTrackLength;
  }
}