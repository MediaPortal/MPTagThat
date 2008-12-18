using System;
using System.Collections.Generic;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;

namespace MPTagThat.Core
{
  /// <summary>
  /// Main Config Service
  /// </summary>
  public class SettingsManager : ISettingsManager
  {

    private int _portable = 0;

    /// <summary>
    /// Retrieves an object's public properties from an Xml file 
    /// </summary>
    /// <param name="settingsObject">Object's instance</param>
    public void Load(object settingsObject)
    {
      ObjectParser.Deserialize(settingsObject);
    }

    /// <summary>
    /// Stores an object's public properties to an Xml file 
    /// </summary>
    /// <param name="settingsObject">Object's instance</param>
    public void Save(object settingsObject)
    {
      ObjectParser.Serialize(settingsObject);
    }

    /// <summary>
    /// Sets the Portable Status
    /// </summary>
    /// <param name="portable"></param>
    public void SetPortable(int portable)
    {
      _portable = portable;
    }

    /// <summary>
    /// Gets the Portable Status
    /// </summary>
    /// <returns></returns>
    public int GetPortable()
    {
      return _portable;
    }
  }
}
