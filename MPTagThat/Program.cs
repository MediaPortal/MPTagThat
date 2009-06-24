using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;

using MPTagThat.Core;
using MPTagThat.Core.AudioEncoder;
using MPTagThat.Core.Burning;
using MPTagThat.Core.MediaChangeMonitor;

namespace MPTagThat
{
  static class Program
  {
    #region Imports
    [DllImport("kernel32.dll", EntryPoint = "SetEnvironmentVariableA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern int SetEnvironmentVariable(string lpName, string lpValue);
    #endregion

    #region Variables
    private static int _portable;
    private static string _startupFolder;
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="/folder=">A startup folder. used when executing via Explorer Context Menu</param>
    /// <param name="/portable">Run in Portable mode</param>
    [STAThread]
    static void Main(string[] args)
    {
      // Need to reset the Working directory, since when we called via the Explorer Context menu, it'll be different
      Directory.SetCurrentDirectory(Application.StartupPath);

      // Add our Bin and Bin\Bass Directory to the Path
      SetEnvironmentVariable("Path", System.IO.Path.Combine(Application.StartupPath, "Bin"));
      SetEnvironmentVariable("Path", System.IO.Path.Combine(Application.StartupPath, @"Bin\Bass"));

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

      // Read the Config file
      ReadConfig();

      // Register Bass
      BassRegistration.BassRegistration.Register();

      using (new ServiceScope(true))
      {
        ILogger logger = new FileLogger("MPTagThat.log", LogLevel.Debug, _portable);
        ServiceScope.Add(logger);
        logger.Info("MPTagThat is starting...");

        logger.Info("Registering Settings Manager");
        ServiceScope.Add<ISettingsManager>(new SettingsManager());
        // Set the portable Indicator
        ServiceScope.Get<ISettingsManager>().SetPortable(_portable);

        logger.Level = (LogLevel)Options.MainSettings.DebugLevel;

        logger.Info("Registering Localisation Services");
        ServiceScope.Add<ILocalisation>(new StringManager());

        logger.Debug("Registering Message Broker");
        ServiceScope.Add<IMessageBroker>(new MessageBroker());

        logger.Info("Registering Script Manager");
        ServiceScope.Add<IScriptManager>(new ScriptManager());

        logger.Info("Registering Burn Manager");
        ServiceScope.Add<IBurnManager>(new BurnManager());

        logger.Info("Registering Audio Encoder");
        ServiceScope.Add<IAudioEncoder>(new AudioEncoder());

        logger.Info("Registering Media Change Monitor");
        ServiceScope.Add<IMediaChangeMonitor>(new MediaChangeMonitor());

        logger.Info("Registering Theme Manager");
        ServiceScope.Add<IThemeManager>(new ThemeManager());

        logger.Info("Registering Action Handöer");
        ServiceScope.Add<IActionHandler>(new ActionHandler());

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
          Main main = new Main();

          // Set the Startup Folder we might have received via an argument, before invoking the form
          main.CurrentDirectory = _startupFolder;
          Application.Run(main);
        }
        catch (Exception ex)
        {
          string message = "Fatal Exception. MPTagThat will be terminated\r\nPlease look at log for the reason of termination";
          MessageBox.Show(message, "Error", MessageBoxButtons.OK);
          logger.Error("Fatal Exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
        }
      }
    }

    static void ReadConfig()
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
      }
      catch (Exception)
      {
      }
    }
  }
}