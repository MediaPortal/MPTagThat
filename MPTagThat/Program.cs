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
    static int _portable;
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // Add our Bin and Bin\Bass Directory to the Path
      SetEnvironmentVariable("Path", System.IO.Path.Combine(Application.StartupPath, "Bin"));
      SetEnvironmentVariable("Path", System.IO.Path.Combine(Application.StartupPath, @"Bin\Bass"));

      // Read the Config File
      _portable = 0;
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
          Application.Run(new Main());
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
          _portable = Convert.ToInt32(portableNode.InnerText);
        }
      }
      catch (Exception)
      {
      }
    }
  }
}