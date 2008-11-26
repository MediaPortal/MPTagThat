using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.InternetLookup
{
  public partial class ArtistAlbumDialog : Form
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();

    #endregion

    #region Properties
    public string Artist
    {
      get { return textBoxArtist.Text.Trim(); }
      set { textBoxArtist.Text = value; }
    }

    public string Album
    {
      get { return textBoxAlbum.Text.Trim(); }
      set { textBoxAlbum.Text = value; }
    }
    #endregion

    #region ctor
    public ArtistAlbumDialog()
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
      this.Text = localisation.ToString("Lookup", "HeaderArtistAlbum");
    }
    #endregion
    #endregion
  }
}