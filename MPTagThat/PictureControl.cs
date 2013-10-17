#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion
#region

using System;
using System.Drawing;
using System.Windows.Forms;
using Elegant.Ui;

#endregion

namespace MPTagThat
{
  public partial class PictureControl : Form
  {
    public PictureControl(GalleryItem item, Point location)
    {
      // Activates double buffering 
      this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();

      InitializeComponent();

      Width = item.Image.Width + ((Width - ClientSize.Width) / 2) + 8;
      Height = item.Image.Height + (Height - ClientSize.Height - 2 * ((Width - ClientSize.Width) / 2)) + 11;
      pictureBoxCover.Image = item.Image;
      Location = location;
      Text = string.Format("{0}x{1}", item.Image.Width, item.Image.Height);
      Show();
    }

    private void pictureBoxCover_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void pictureBoxCover_MouseLeave(object sender, EventArgs e)
    {
      Close();
    }
  }
}