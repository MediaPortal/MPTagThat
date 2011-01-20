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

using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat.InternetLookup
{
  public partial class AlbumSearchResult : ShapedForm
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();

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
      labelHeader.Text = localisation.ToString("Lookup", "HeaderSearchResult");
      chArtist.Text = localisation.ToString("Lookup", "ColArtist");
      chTitle.Text = localisation.ToString("Lookup", "ColAlbum");
      chTracks.Text = localisation.ToString("Lookup", "ColTracks");
      chYear.Text = localisation.ToString("Lookup", "ColYear");
      chLabel.Text = localisation.ToString("Lookup", "ColLabel");
    }

    #endregion

    private void lvAlbumSearchResult_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    #endregion
  }
}