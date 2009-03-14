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
      this.cbArtist = new System.Windows.Forms.ComboBox();
      this.cbAlbumArtist = new System.Windows.Forms.ComboBox();
      this.groupBoxMedia.Controls.Add(this.ckTrackLength);
      this.groupBoxArtist.Controls.Add(this.cbArtist);
      this.groupBoxArtist.Controls.Add(this.cbAlbumArtist);

      // 
      // cbArtist
      // 
      this.cbArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbArtist.FormattingEnabled = true;
      this.cbArtist.Location = new System.Drawing.Point(150, 12);
      this.cbArtist.Name = "cbArtist";
      this.cbArtist.Size = new System.Drawing.Size(515, 22);
      this.cbArtist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
      this.cbArtist.TabIndex = 0;
      // 
      // cbAlbumArtist
      // 
      this.cbAlbumArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbAlbumArtist.FormattingEnabled = true;
      this.cbAlbumArtist.Location = new System.Drawing.Point(150, 37);
      this.cbAlbumArtist.Name = "cbAlbumArtist";
      this.cbAlbumArtist.Size = new System.Drawing.Size(515, 22);
      this.cbAlbumArtist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
      this.cbArtist.TabIndex = 1;
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
    private System.Windows.Forms.ComboBox cbArtist;
    private System.Windows.Forms.ComboBox cbAlbumArtist;
  }
}