using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPTagThat.Core;
using MPTagThat.Core.Amazon;

namespace MPTagThat.InternetLookup
{
  public partial class AlbumSearchResult : Form
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();

    #endregion

    #region Properties
    public ListView ResultView
    {
      get { return lvAlbumSearchResult; }
    }
    #endregion

    #region ctor
    public AlbumSearchResult()
    {
      InitializeComponent();

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      LocaliseScreen();
    }
    #endregion

    #region Methods
    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      this.Text = localisation.ToString("Lookup", "HeaderSearchResult");
      this.chArtist.Text = localisation.ToString("Lookup", "ColArtist");
      this.chTitle.Text = localisation.ToString("Lookup", "ColAlbum");
      this.chTracks.Text = localisation.ToString("Lookup", "ColTracks");
      this.chYear.Text = localisation.ToString("Lookup", "ColYear");
      this.chLabel.Text = localisation.ToString("Lookup", "ColLabel");
    }
    #endregion

    private void lvAlbumSearchResult_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion
  }
}
