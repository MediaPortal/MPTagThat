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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

#endregion

namespace MPTagThat.Core
{
  public class LocalisationProvider
  {
    #region Variables

    private readonly List<string> _languageDirectories;
    private readonly Dictionary<string, Dictionary<string, StringLocalised>> _languageStrings;
    private readonly string _systemDirectory;
    private readonly string _userDirectory;
    private Dictionary<string, CultureInfo> _availableLanguages;
    private int _characters;
    private CultureInfo _currentLanguage;
    private bool _userLanguage;

    #endregion

    #region Constructors/Destructors

    public LocalisationProvider(string systemDirectory, string userDirectory, string cultureName)
    {
      // Base strings directory
      _systemDirectory = systemDirectory;
      // User strings directory
      _userDirectory = userDirectory;

      _languageDirectories = new List<string>();
      _languageDirectories.Add(_systemDirectory);

      GetAvailableLangauges();

      // If the language cannot be found default to Local language or English
      if (cultureName != null && _availableLanguages.ContainsKey(cultureName))
        _currentLanguage = _availableLanguages[cultureName];
      else
        _currentLanguage = GetBestLanguage();

      if (_currentLanguage == null)
        throw (new ArgumentException("No available language found"));

      _languageStrings = new Dictionary<string, Dictionary<string, StringLocalised>>();

      CheckUserStrings();
      ReloadAll();
    }

    public LocalisationProvider(string directory, string cultureName)
      : this(directory, directory, cultureName) {}

    public void Dispose()
    {
      Clear();
    }

    #endregion

    #region Properties

    public CultureInfo CurrentCulture
    {
      get { return _currentLanguage; }
    }

    public int Characters
    {
      get { return _characters; }
    }

    #endregion

    #region Public Methods

    public void AddDirectory(string directory)
    {
      // Add directory to list, to enable reloading/changing language
      _languageDirectories.Add(directory);

      LoadStrings(directory);
    }

    public void ChangeLanguage(string cultureName)
    {
      if (!_availableLanguages.ContainsKey(cultureName))
        throw new ArgumentException("Language not available");

      _currentLanguage = _availableLanguages[cultureName];

      ReloadAll();

      QueueMessage msg = new QueueMessage();
      msg.MessageData["action"] = "languagechanged";
      IMessageQueue queue = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queue.Send(msg);
    }

    public string ToString(string section, string id)
    {
      id = id.ToLower();
      section = section.ToLower();

      try
      {
        if (_languageStrings.ContainsKey(section) && _languageStrings[section].ContainsKey(id))
          return _languageStrings[section][id].text;
      }
      catch (KeyNotFoundException) {}

      return null;
    }

    public string ToString(string section, string id, object[] parameters)
    {
      string translation = ToString(section, id);
      // if parameters or the translation is null, return the translation.
      if ((translation == null) || (parameters == null))
      {
        return translation;
      }
      // return the formatted string. If formatting fails, log the error
      // and return the unformatted string.
      try
      {
        return String.Format(translation, parameters);
      }
      catch (FormatException)
      {
        //Log.Error("Error formatting translation with id {0}", dwCode);
        //Log.Error("Unformatted translation: {0}", translation);
        //Log.Error(e);  
        // Throw exception??
        return translation;
      }
    }

    public CultureInfo[] AvailableLanguages()
    {
      CultureInfo[] available = new CultureInfo[_availableLanguages.Count];

      IDictionaryEnumerator languageEnumerator = _availableLanguages.GetEnumerator();

      for (int i = 0; i < _availableLanguages.Count; i++)
      {
        languageEnumerator.MoveNext();
        available[i] = (CultureInfo)languageEnumerator.Value;
      }

      return available;
    }

    public bool IsLocalSupported()
    {
      if (_availableLanguages.ContainsKey(CultureInfo.CurrentCulture.Name))
        return true;

      return false;
    }

    public CultureInfo GetBestLanguage()
    {
      // Try current local language
      if (_availableLanguages.ContainsKey(CultureInfo.CurrentCulture.Name))
        return CultureInfo.CurrentCulture;

      // Try Language Parent if it has one
      if (!CultureInfo.CurrentCulture.IsNeutralCulture &&
          _availableLanguages.ContainsKey(CultureInfo.CurrentCulture.Parent.Name))
        return CultureInfo.CurrentCulture.Parent;

      // default to English
      if (_availableLanguages.ContainsKey("en"))
        return _availableLanguages["en"];

      return null;
    }

    #endregion

    #region Private Methods

    private void LoadUserStrings()
    {
      // Load User Custom strings
      if (_userLanguage)
        LoadStrings(_userDirectory, "user", false);
    }

    private void LoadStrings(string directory)
    {
      // Local Language
      LoadStrings(directory, _currentLanguage.Name, false);

      // Parent Language
      if (!_currentLanguage.IsNeutralCulture)
        LoadStrings(directory, _currentLanguage.Parent.Name, false);

      // Default to English
      if (_currentLanguage.Name != "en")
        LoadStrings(directory, "en", true);
    }

    private void ReloadAll()
    {
      Clear();

      LoadUserStrings();

      foreach (string directoy in _languageDirectories)
        LoadStrings(directoy);
    }

    private void Clear()
    {
      if (_languageStrings != null)
        _languageStrings.Clear();

      _characters = 255;
    }

    private void CheckUserStrings()
    {
      _userLanguage = false;

      string path = Path.Combine(_userDirectory, "strings_user.xml");

      if (File.Exists(path))
        _userLanguage = true;
    }

    private void GetAvailableLangauges()
    {
      _availableLanguages = new Dictionary<string, CultureInfo>();

      DirectoryInfo dir = new DirectoryInfo(_systemDirectory);
      foreach (FileInfo file in dir.GetFiles("strings_*.xml"))
      {
        int pos = file.Name.IndexOf('_') + 1;
        string cultName = file.Name.Substring(pos, file.Name.Length - file.Extension.Length - pos);

        try
        {
          CultureInfo cultInfo = new CultureInfo(cultName);
          _availableLanguages.Add(cultName, cultInfo);
        }
        catch (ArgumentException)
        {
          // Log file error?
        }
      }
    }

    private void LoadStrings(string directory, string language, bool log)
    {
      string filename = "strings_" + language + ".xml";
      ServiceScope.Get<ILogger>().GetLogger.Info("Loading strings file: {0}", filename);

      string path = Path.Combine(directory, filename);
      if (File.Exists(path))
      {
        StringFile strings;
        try
        {
          XmlSerializer s = new XmlSerializer(typeof (StringFile));
          TextReader r = new StreamReader(path);
          strings = (StringFile)s.Deserialize(r);
        }
        catch (Exception ex)
        {
          ServiceScope.Get<ILogger>().GetLogger.Error("Error loading strings file: {0}", ex.Message);
          return;
        }

        if (_characters < strings.characters)
          _characters = strings.characters;

        foreach (StringSection section in strings.sections)
        {
          // convert section name tolower -> no case matching.
          section.name = section.name.ToLower();

          Dictionary<string, StringLocalised> newSection;
          if (_languageStrings.ContainsKey(section.name))
          {
            newSection = _languageStrings[section.name];
            _languageStrings.Remove(section.name);
          }
          else
          {
            newSection = new Dictionary<string, StringLocalised>();
          }

          foreach (StringLocalised languageString in section.localisedStrings)
          {
            languageString.id = languageString.id.ToLower();

            if (!newSection.ContainsKey(languageString.id))
            {
              languageString.language = language;
              newSection.Add(languageString.id, languageString);
              if (log)
                ServiceScope.Get<ILogger>().GetLogger.Info("    String not found, using English: {0}", languageString.ToString());
            }
          }

          if (newSection.Count > 0)
            _languageStrings.Add(section.name, newSection);
        }
      }
    }

    #endregion
  }
}