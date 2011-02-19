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

using System.Threading;

#endregion

namespace MPTagThat.Dialogues
{
  internal class SplashScreen
  {
    #region Variables

    private SplashForm _frm;
    private string _info;
    private bool _stopRequested;

    #endregion

    #region ctor

    #endregion

    #region Public Methods

    /// <summary>
    ///   Starts the splash screen.
    /// </summary>
    public void Run()
    {
      Thread thread = new Thread(DoRun);
      thread.Name = "ConfigSplashscreen";
      thread.Start();
    }

    /// <summary>
    ///   Stops the splash screen.
    /// </summary>
    public void Stop()
    {
      _stopRequested = true;
    }

    /// <summary>
    ///   Stops the splash screen after given wait time
    /// </summary>
    public void Stop(int aWaitTime)
    {
      Thread.Sleep(aWaitTime);
      _stopRequested = true;
    }

    /// <summary>
    ///   Determine if the Splash has been closed
    /// </summary>
    public bool isStopped()
    {
      return (_frm == null);
    }

    /// <summary>
    ///   Set the contents of the information label of the splash screen
    /// </summary>
    /// <param name = "information">the information to set</param>
    public void SetInformation(string information)
    {
      _info = information;
    }

    #endregion

    #region Private Methods

    /// <summary>
    ///   Starts the actual splash screen.
    /// </summary>
    /// <remarks>
    ///   This method is started in a background thread by the <see cref = "Run" /> method.
    /// </remarks>
    private void DoRun()
    {
      string oldInfo = null;
      _frm = new SplashForm();
      _frm.Show();
      _frm.Update();

      while (!_stopRequested) //run until stop of splashscreen is requested
      {
        _frm.TopMost = false;
        _frm.BringToFront();

        if (oldInfo != _info)
        {
          _frm.SetInformation(_info);
          oldInfo = _info;
        }
        Thread.Sleep(25);
      }
      _frm.Close(); //closes, and disposes the form
      _frm = null;
    }

    #endregion
  }
}