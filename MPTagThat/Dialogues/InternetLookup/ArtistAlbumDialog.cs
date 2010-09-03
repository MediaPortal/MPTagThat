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

using MPTagThat.Core;

#endregion

namespace MPTagThat.InternetLookup
{
  public partial class ArtistAlbumDialog : ShapedForm
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();

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
      labelHeader.Text = localisation.ToString("Lookup", "HeaderArtistAlbum");
    }

    #endregion

    #endregion
  }
}