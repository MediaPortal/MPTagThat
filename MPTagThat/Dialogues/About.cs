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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat.Dialogues
{
  public partial class About : ShapedForm
  {
    #region Variables

    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();

    #endregion

    #region ctor

    public About()
    {
      InitializeComponent();

      lbWikiLink.Text = Options.HelpLocation;
      lbLinkForum.Text = Options.ForumLocation;

      BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      lbWikiLink.LinkColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
      lbLinkForum.LinkColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      LocaliseScreen();

      Assembly assembly = Assembly.GetExecutingAssembly();
      AssemblyName assemblyName = assembly.GetName();
      Version version = assemblyName.Version;
      lbVersionDetail.Text = version.ToString();
      DateTime lastWrite = File.GetLastWriteTime(assembly.Location);
      lbDate.Text = string.Format("{0} {1}", lastWrite.ToShortDateString(), lastWrite.ToShortTimeString());
    }

    #endregion

    #region Localisation

    /// <summary>
    ///   Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Text = localisation.ToString("About", "Header");
    }

    #endregion

    #region Event Handler

    private void lbWikiLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(Options.HelpLocation);
    }

    private void lbLinkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(Options.ForumLocation);
    }

    #endregion
  }
}