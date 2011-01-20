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
using System.IO;
using System.Reflection;
using MPTagThat.Core;

#endregion

namespace MPTagThat.Dialogues
{
  public partial class SplashForm : ShapedForm
  {
    #region ctor

    public SplashForm()
    {
      InitializeComponent();

      Text = ServiceScope.Get<ILocalisation>().ToString("system", "ApplicationName");

      Assembly assembly = Assembly.GetExecutingAssembly();
      AssemblyName assemblyName = assembly.GetName();
      label1.Text = ServiceScope.Get<ILocalisation>().ToString("splash", "Version");
      Version version = assemblyName.Version;
      lbVersion.Text = version.ToString();
      label2.Text = ServiceScope.Get<ILocalisation>().ToString("splash", "BuildDate");
      DateTime lastWrite = File.GetLastWriteTime(assembly.Location);
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