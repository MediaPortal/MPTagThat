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
using System.Windows.Forms;
using MPTagThat.Core;
using TagLib;

#endregion

namespace MPTagThat.InternetLookup
{
  public partial class AlbumDetails : ShapedForm
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();

    #endregion

    #region Properties

    public string Artist
    {
      get { return tbArtist.Text.Trim(); }
      set { tbArtist.Text = value; }
    }

    public string Album
    {
      get { return tbAlbum.Text.Trim(); }
      set { tbAlbum.Text = value; }
    }

    public string Year
    {
      get
      {
        string strYear = tbYear.Text.Trim();
        if (strYear.Length > 4)
          strYear = strYear.Substring(0, 4);

        return strYear;
      }
      set
      {
        string strYear = value;
        if (strYear == null)
          strYear = "";

        if (strYear.Length > 4)
          strYear = strYear.Substring(0, 4);

        tbYear.Text = strYear;
      }
    }

    public string Genre
    {
      get { return cbGenre.SelectedText; }
      set { cbGenre.SelectedText = value; }
    }

    public PictureBox Cover
    {
      get { return pictureBoxCover; }
    }

    public ListView AlbumTracks
    {
      get { return lvAlbumTracks; }
    }

    public ListView DiscTracks
    {
      get { return lvDiscTracks; }
    }

    #endregion

    #region ctor

    public AlbumDetails()
    {
      InitializeComponent();

      // Fill the Genre Combo Box
      cbGenre.Items.AddRange(Genres.Audio);

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      labelHeader.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderForeColor;
      labelHeader.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.FormHeaderFont;

      LocaliseScreen();
    }

    #endregion

    #region Methods

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      labelHeader.Text = localisation.ToString("Lookup", "HeaderDetails");
      chTrackNum.Text = localisation.ToString("Lookup", "ColTrackNum");
      chTitle.Text = localisation.ToString("Lookup", "ColTitle");
      chFileName.Text = localisation.ToString("Lookup", "ColFileName");
    }

    /// <summary>
    ///   Renumber the list, when the item was moved with the buttons
    /// </summary>
    public void Renumber()
    {
      for (int i = 0; i < lvDiscTracks.Items.Count; i++)
      {
        lvDiscTracks.Items[i].SubItems[0].Text = (i + 1).ToString();
      }
    }

    #endregion

    #endregion

    #region Button Events

    private void btUp_Click(object sender, EventArgs e)
    {
      // Set focus back on Listview
      lvDiscTracks.Focus();

      int currPos = lvDiscTracks.SelectedIndices[0];
      if (currPos == 0)
        return;

      ListViewItem lvItem = lvDiscTracks.Items[currPos];
      if ((int)lvItem.Tag > -1)
        lvItem.Checked = true;

      lvDiscTracks.Items.RemoveAt(currPos);
      lvDiscTracks.Items.Insert(currPos - 1, lvItem);
      lvDiscTracks.Items[currPos - 1].Selected = true;

      Renumber();
    }

    private void btDown_Click(object sender, EventArgs e)
    {
      // Set focus back on Listview
      lvDiscTracks.Focus();

      int currPos = lvDiscTracks.SelectedIndices[0];
      if (currPos == lvDiscTracks.Items.Count - 1)
        return;

      ListViewItem lvItem = lvDiscTracks.Items[currPos];
      if ((int)lvItem.Tag > -1)
        lvItem.Checked = true;

      lvDiscTracks.Items.RemoveAt(currPos);
      lvDiscTracks.Items.Insert(currPos + 1, lvItem);
      lvDiscTracks.Items[currPos + 1].Selected = true;

      Renumber();
    }

    #endregion
  }
}