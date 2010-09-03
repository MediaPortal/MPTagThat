using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MPTagThat.Dialogues
{
  class SplashScreen
  {
    #region Variables
    private bool _stopRequested = false;
    private SplashForm _frm;
    private string _info;
    #endregion

    #region ctor
    public SplashScreen()
    {

    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Starts the splash screen.
    /// </summary>
    public void Run()
    {
      Thread thread = new Thread(new ThreadStart(DoRun));
      thread.Name = "ConfigSplashscreen";
      thread.Start();
    }

    /// <summary>
    /// Stops the splash screen.
    /// </summary>
    public void Stop()
    {
      _stopRequested = true;
    }

    /// <summary>
    /// Stops the splash screen after given wait time
    /// </summary>
    public void Stop(int aWaitTime)
    {
      Thread.Sleep(aWaitTime);
      _stopRequested = true;
    }

    /// <summary>
    /// Determine if the Splash has been closed
    /// </summary>
    public bool isStopped()
    {
      return (_frm == null);
    }

    /// <summary>
    /// Set the contents of the information label of the splash screen
    /// </summary>
    /// <param name="information">the information to set</param>
    public void SetInformation(string information)
    {
      _info = information;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Starts the actual splash screen.
    /// </summary>
    /// <remarks>
    /// This method is started in a background thread by the <see cref="Run"/> method.</remarks>
    private void DoRun()
    {
      string oldInfo = null;
      _frm = new SplashForm();
      _frm.Show();
      _frm.Update();

      while (!_stopRequested) //run until stop of splashscreen is requested
      {
        _frm.TopMost = true;
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
