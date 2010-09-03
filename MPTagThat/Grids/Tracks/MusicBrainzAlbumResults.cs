using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MPTagThat.Core;
using MPTagThat.Core.MusicBrainz;


namespace MPTagThat.GridView
{
  public partial class MusicBrainzAlbumResults : Form
  {
    #region Variables
    List<MusicBrainzTrack> tracks = null;
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

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      this.Text = ServiceScope.Get<ILocalisation>().ToString("MusicBrainz", "Header");
      this.chAlbum.Text = ServiceScope.Get<ILocalisation>().ToString("MusicBrainz", "Album");
      this.chDuration.Text = ServiceScope.Get<ILocalisation>().ToString("MusicBrainz", "Duration");

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
        string minute = (track.Duration / 1000 / 60).ToString().PadLeft(2,'0');
        string secs = ((track.Duration / 1000) % 60).ToString().PadLeft(2, '0');
        item.SubItems.Add(string.Format("{0}:{1}", minute, secs));
        lvSearchResults.Items.Add(item);
      }
    }
    #endregion

    #region Events
    private void btClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btUpdate_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void lvSearchResults_DoubleClick(object sender, EventArgs e)
    {
      btUpdate.PerformClick();
    }
    #endregion
  }
}