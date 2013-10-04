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
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using MPTagThat.Core;
using MPTagThat.Core.AudioEncoder;
using MPTagThat.Core.MediaChangeMonitor;

#endregion

namespace MPTagThat
{
  internal static class Program
  {
    #region Variables

    private static int _portable;
    private static int _maxSongs;
    private static string _startupFolder;
    private static Main _main;

    private delegate void ThreadSafeSetCurrentFolderDelegate(string startupFolder);

    #endregion

    /// <summary>
    ///   The main entry point for the application.
    /// </summary>
    /// <param name = "/folder=">A startup folder. used when executing via Explorer Context Menu</param>
    /// <param name = "/portable">Run in Portable mode</param>
    [STAThread]
    private static void Main(string[] args)
    {
      // Need to reset the Working directory, since when we called via the Explorer Context menu, it'll be different
      Directory.SetCurrentDirectory(Application.StartupPath);

      // Add our Bin and Bin\Bass Directory to the Path
      SetPath(Path.Combine(Application.StartupPath, "Bin"));
      SetPath(Path.Combine(Application.StartupPath, @"Bin\Bass"));

      _portable = 0;
      _startupFolder = "";
      // Process Command line Arguments
      foreach (string arg in args)
      {
        if (arg.ToLower().StartsWith("/folder="))
        {
          _startupFolder = arg.Substring(8);
        }
        else if (arg.ToLower() == "/portable")
        {
          _portable = 1;
        }
      }

      try
      {
        // Let's see, if we already have an instance of MPTagThat open
        using (var mmf = MemoryMappedFile.OpenExisting("MPTagThat"))
        {
          if (_startupFolder == string.Empty)
          {
            // Don't allow a second instance of MPTagThat running
            return;
          }

          byte[] buffer = Encoding.Default.GetBytes(_startupFolder);
          var messageWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "MPTagThat_IPC");

          // Create accessor to MMF
          using (var accessor = mmf.CreateViewAccessor(0, buffer.Length))
          {
            // Write to MMF
            accessor.WriteArray<byte>(0, buffer, 0, buffer.Length);
            messageWaitHandle.Set();

            // End exit this instance
            return;
          }
        }
      }
      catch (FileNotFoundException)
      {
        // The Memorymap does not exist, so MPTagThat is not yet running
      }

      // Read the Config file
      ReadConfig();

      // Register Bass
      BassRegistration.BassRegistration.Register();

      using (new ServiceScope(true))
      {
        ILogger logger = new FileLogger("MPTagThat.log", NLog.LogLevel.Debug, _portable);
        ServiceScope.Add(logger);
        
        NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;
        
        log.Info("MPTagThat is starting...");

        log.Info("Registering Services");
        log.Debug("Registering Settings Manager");
        ServiceScope.Add<ISettingsManager>(new SettingsManager());
        // Set the portable Indicator
        ServiceScope.Get<ISettingsManager>().SetPortable(_portable);
        // Set the Max Songs number
        ServiceScope.Get<ISettingsManager>().SetMaxSongs(_maxSongs);

        try
        {
          logger.Level = NLog.LogLevel.FromString(Options.MainSettings.DebugLevel);
        }
        catch (ArgumentException)
        {
          Options.MainSettings.DebugLevel = "Info";
          logger.Level = NLog.LogLevel.FromString(Options.MainSettings.DebugLevel);
        }

        log.Debug("Registering Localisation Services");
        ServiceScope.Add<ILocalisation>(new StringManager());

        log.Debug("Registering Message Broker");
        ServiceScope.Add<IMessageBroker>(new MessageBroker());

        log.Debug("Registering Theme Manager");
        ServiceScope.Add<IThemeManager>(new ThemeManager());

        log.Debug("Registering Action Handler");
        ServiceScope.Add<IActionHandler>(new ActionHandler());

        // Move Init of Services, which we don't need immediately to a separate thread to increase startup performance
        Thread initService = new Thread(DoInitService);
        initService.IsBackground = true;
        initService.Name = "InitService";
        initService.Start();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
          _main = new Main();

          // Set the Startup Folder we might have received via an argument, before invoking the form
          _main.CurrentDirectory = _startupFolder;
          Application.Run(_main);
        }
        catch (OutOfMemoryException)
        {
          GC.Collect();
          MessageBox.Show(ServiceScope.Get<ILocalisation>().ToString("message", "OutOfMemory"),
                          ServiceScope.Get<ILocalisation>().ToString("message", "Error_Title"), MessageBoxButtons.OK);
          log.Error("Running out of memory. Scanning aborted.");
        }
        catch (Exception ex)
        {
          MessageBox.Show(ServiceScope.Get<ILocalisation>().ToString("message", "FatalError"),
                          ServiceScope.Get<ILocalisation>().ToString("message", "Error_Title"), MessageBoxButtons.OK);
          log.Error("Fatal Exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
        }
      }
    }

    #region Methods

    /// <summary>
    /// Init Service Thread
    /// </summary>
    private static void DoInitService()
    {
      ServiceScope.Get<ILogger>().GetLogger.Debug("Registering Script Manager");
      ServiceScope.Add<IScriptManager>(new ScriptManager());

      ServiceScope.Get<ILogger>().GetLogger.Debug("Registering Audio Encoder");
      ServiceScope.Add<IAudioEncoder>(new AudioEncoder());

      ServiceScope.Get<ILogger>().GetLogger.Debug("Registering Media Change Monitor");
      ServiceScope.Add<IMediaChangeMonitor>(new MediaChangeMonitor());

      ServiceScope.Get<ILogger>().GetLogger.Info("Finished registering services");

      byte[] buffer = new byte[2048];
      var messageWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "MPTagThat_IPC");

      ServiceScope.Get<ILogger>().GetLogger.Debug("Registering MemoryMappedFile");
      // Create named MMF
      using (var mmf = MemoryMappedFile.CreateOrOpen("MPTagThat", 2048))
      {
        // Create accessor to MMF
        using (var accessor = mmf.CreateViewAccessor(0, buffer.Length))
        {
          // Wait for the Message to be fired
          while (true)
          {
            ServiceScope.Get<ILogger>().GetLogger.Debug("Wait for Startup Event to be fired");
            messageWaitHandle.WaitOne();
            ServiceScope.Get<ILogger>().GetLogger.Debug("Startup Event fired");

            // Read from MMF
            accessor.ReadArray<byte>(0, buffer, 0, buffer.Length);

            string startupFolder = Encoding.Default.GetString(buffer).Trim(new char[] { ' ', '\x00' });
            SetCurrentFolder(startupFolder);
          }
        }
      }
    }

    /// <summary>
    /// Read the Config.xml file
    /// </summary>
    private static void ReadConfig()
    {
      string configFile = Path.Combine(Application.StartupPath, "Config.xml");
      if (!File.Exists(configFile))
        return;

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(configFile);

        // Check, if we got a config.xml
        if (doc.DocumentElement == null) return;
        string strRoot = doc.DocumentElement.Name;
        if (strRoot != "config") return;

        XmlNode portableNode = doc.DocumentElement.SelectSingleNode("/config/portable");
        if (portableNode != null)
        {
          if (_portable == 0)
          {
            // Only use the value from Config, if not overriden by an argument
            _portable = Convert.ToInt32(portableNode.InnerText);
          }
        }

        XmlNode maxSongsNode = doc.DocumentElement.SelectSingleNode("/config/MaximumNumberOfSongsInList");
        if (maxSongsNode != null)
        {
          _maxSongs = Convert.ToInt32(maxSongsNode.InnerText);
        }
        else
        {
          _maxSongs = 200;
        }
      }
      catch (Exception) {}
    }

    /// <summary>
    /// Set the Path for the binaries
    /// </summary>
    /// <param name="path"></param>
    private static void SetPath(string path)
    {
      string currentPath = Environment.GetEnvironmentVariable("Path");
      string newPath = string.Format("{0};{1}",currentPath, path);
      Environment.SetEnvironmentVariable("Path", newPath);
    }

    private static void SetCurrentFolder(string startupFolder)
    {
      if (_main.InvokeRequired)
      {
        ThreadSafeSetCurrentFolderDelegate d = SetCurrentFolder;
        _main.Invoke(d, new object[] { startupFolder });
        return;
      }

      _main.CurrentDirectory = startupFolder;
      _main.RefreshTrackList();
      _main.TreeView.TreeView.ShowFolder(startupFolder);
      _main.Activate();
    }

    #endregion
  }
}