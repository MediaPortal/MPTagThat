namespace MPTagThat.InternetLookup
{
  partial class ArtistAlbumDialog
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
      this.mptLabel1 = new MPTagThat.Core.WinControls.MPTLabel();
      this.mptLabel2 = new MPTagThat.Core.WinControls.MPTLabel();
      this.textBoxArtist = new System.Windows.Forms.TextBox();
      this.textBoxAlbum = new System.Windows.Forms.TextBox();
      this.btContinue = new MPTagThat.Core.WinControls.MPTButton();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.roundRectShape1 = new Telerik.WinControls.RoundRectShape();
      this.labelHeader = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // mptLabel1
      // 
      this.mptLabel1.AutoSize = true;
      this.mptLabel1.Localisation = "Artist";
      this.mptLabel1.LocalisationContext = "TagEdit";
      this.mptLabel1.Location = new System.Drawing.Point(22, 81);
      this.mptLabel1.Name = "mptLabel1";
      this.mptLabel1.Size = new System.Drawing.Size(33, 13);
      this.mptLabel1.TabIndex = 0;
      this.mptLabel1.Text = "Artist:";
      // 
      // mptLabel2
      // 
      this.mptLabel2.AutoSize = true;
      this.mptLabel2.Localisation = "Album:";
      this.mptLabel2.LocalisationContext = "TagEdit";
      this.mptLabel2.Location = new System.Drawing.Point(22, 120);
      this.mptLabel2.Name = "mptLabel2";
      this.mptLabel2.Size = new System.Drawing.Size(39, 13);
      this.mptLabel2.TabIndex = 1;
      this.mptLabel2.Text = "Album:";
      // 
      // textBoxArtist
      // 
      this.textBoxArtist.Location = new System.Drawing.Point(150, 73);
      this.textBoxArtist.Name = "textBoxArtist";
      this.textBoxArtist.Size = new System.Drawing.Size(328, 20);
      this.textBoxArtist.TabIndex = 2;
      // 
      // textBoxAlbum
      // 
      this.textBoxAlbum.Location = new System.Drawing.Point(150, 117);
      this.textBoxAlbum.Name = "textBoxAlbum";
      this.textBoxAlbum.Size = new System.Drawing.Size(328, 20);
      this.textBoxAlbum.TabIndex = 3;
      // 
      // btContinue
      // 
      this.btContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btContinue.Localisation = "Continue";
      this.btContinue.LocalisationContext = "Lookup";
      this.btContinue.Location = new System.Drawing.Point(150, 167);
      this.btContinue.Name = "btContinue";
      this.btContinue.Size = new System.Drawing.Size(125, 43);
      this.btContinue.TabIndex = 4;
      this.btContinue.Text = "Continue >";
      this.btContinue.UseVisualStyleBackColor = true;
      // 
      // btCancel
      // 
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "Lookup";
      this.btCancel.Location = new System.Drawing.Point(353, 167);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(125, 43);
      this.btCancel.TabIndex = 5;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      // 
      // labelHeader
      // 
      this.labelHeader.AutoSize = true;
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.ForeColor = System.Drawing.Color.White;
      this.labelHeader.Location = new System.Drawing.Point(21, 25);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(62, 20);
      this.labelHeader.TabIndex = 23;
      this.labelHeader.Text = "Header";
      // 
      // ArtistAlbumDialog
      // 
      this.AcceptButton = this.btContinue;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btCancel;
      this.ClientSize = new System.Drawing.Size(534, 242);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btContinue);
      this.Controls.Add(this.textBoxAlbum);
      this.Controls.Add(this.textBoxArtist);
      this.Controls.Add(this.mptLabel2);
      this.Controls.Add(this.mptLabel1);
      this.Name = "ArtistAlbumDialog";
      this.Shape = this.roundRectShape1;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Internet Lookup - Specify Artist / Album";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTLabel mptLabel1;
    private MPTagThat.Core.WinControls.MPTLabel mptLabel2;
    private System.Windows.Forms.TextBox textBoxArtist;
    private System.Windows.Forms.TextBox textBoxAlbum;
    private MPTagThat.Core.WinControls.MPTButton btContinue;
    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private Telerik.WinControls.RoundRectShape roundRectShape1;
    private System.Windows.Forms.Label labelHeader;
  }
}