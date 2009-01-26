using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using MPTagThat.Core;

namespace MPTagThat.Dialogues
{
  public partial class About : Telerik.WinControls.UI.ShapedForm
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    #endregion

    #region ctor
    public About()
    {
      InitializeComponent();

      lbWikiLink.Text = Options.HelpLocation;

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      LocaliseScreen();

      Assembly assembly = Assembly.GetExecutingAssembly();
      AssemblyName assemblyName = assembly.GetName(); 
      Version version = assemblyName.Version;
      lbVersionDetail.Text = version.ToString();
      DateTime lastWrite = System.IO.File.GetLastWriteTime(assembly.Location);
      lbDate.Text = string.Format("{0} {1}", lastWrite.ToShortDateString(), lastWrite.ToShortTimeString());
    }
    #endregion

    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Text = localisation.ToString("About", "Header");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    #region Event Handler
    private void lbWikiLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(Options.HelpLocation);
    }
    #endregion
  }
}
