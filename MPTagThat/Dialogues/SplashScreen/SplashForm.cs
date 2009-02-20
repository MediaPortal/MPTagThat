using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MPTagThat.Dialogues
{
  public partial class SplashForm : Telerik.WinControls.UI.ShapedForm
  {
    #region ctor
    public SplashForm()
    {
      InitializeComponent();

      Assembly assembly = Assembly.GetExecutingAssembly();
      AssemblyName assemblyName = assembly.GetName();
      Version version = assemblyName.Version;
      lbVersion.Text = version.ToString();
      DateTime lastWrite = System.IO.File.GetLastWriteTime(assembly.Location);
      lbDate.Text = string.Format("{0} {1}", lastWrite.ToShortDateString(), lastWrite.ToShortTimeString());
    }
    #endregion

    #region Private Methods
    public void SetInformation(string information)
    {
      lbStatus.Text = information;
      Update();
    }
    #endregion
  }
}
