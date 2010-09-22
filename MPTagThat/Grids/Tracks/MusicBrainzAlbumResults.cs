#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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
using System.Collections.Generic;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.MusicBrainz;

#endregion

namespace MPTagThat.GridView
{
  public partial class MusicBrainzAlbumResults : Form
  {
    #region Variables

    private readonly List<MusicBrainzTrack> tracks;

    #endregion

    #region Properties

    public int SelectedListItem
    {
      get { return lvSearchResults.SelectedIndices[0]; }
    }

    #endregion

    #region ctor

    public MusicBrainzAlbumResults(List<MusicBrainzTrack> tracks)
    {
      InitializeComponent();

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      Text = ServiceScope.Get<ILocalisation>().ToString("MusicBrainz", "Header");
      chAlbum.Text = ServiceScope.Get<ILocalisation>().ToString("MusicBrainz", "Album");
      chDuration.Text = ServiceScope.Get<ILocalisation>().ToString("MusicBrainz", "Duration");

      this.tracks = tracks;
      FillResults();
    }

    #endregion

    #region Methods

    private void FillResults()
    {
      tbArtist.Text = tracks[0].Artist;
      tbTitle.Text = tracks[0].Title;

      foreach (MusicBrainzTrack track in tracks)
      {
        ListViewItem item = new ListViewItem(track.Album);
        string minute = (track.Duration / 1000 / 60).ToString().PadLeft(2, '0');
        string secs = ((track.Duration / 1000) % 60).ToString().PadLeft(2, '0');
        item.SubItems.Add(string.Format("{0}:{1}", minute, secs));
        lvSearchResults.Items.Add(item);
      }
    }

    #endregion

    #region Events

    private void btClose_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void btUpdate_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void lvSearchResults_DoubleClick(object sender, EventArgs e)
    {
      btUpdate.PerformClick();
    }

    #endregion
  }
}