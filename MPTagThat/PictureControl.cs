using System;
using System.Drawing;
using System.Windows.Forms;
using Elegant.Ui;

namespace MPTagThat
{
  public partial class PictureControl : Form
  {
    public PictureControl(GalleryItem item, Point location)
    {
      InitializeComponent();

      this.Width = item.Image.Width + ((this.Width - this.ClientSize.Width) / 2) + 8;
      this.Height = item.Image.Height + (this.Height - this.ClientSize.Height - 2 * ((this.Width - this.ClientSize.Width) / 2)) + 11;
      this.pictureBoxCover.Image = item.Image;
      this.Location = location;
      this.Text = string.Format("{0}x{1}", item.Image.Width, item.Image.Height);
      this.Show();
    }

    private void pictureBoxCover_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void pictureBoxCover_MouseLeave(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
