#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#endregion

namespace MPTagThat.Core
{
  public class ActionHandler : IActionHandler
  {
    #region Variables

    private readonly ILogger Log;
    private readonly List<WindowMap> mapWindows;

    #endregion

    #region ctor

    public ActionHandler()
    {
      Log = ServiceScope.Get<ILogger>();
      mapWindows = new List<WindowMap>();

      LoadKeyMap();
    }

    #endregion

    #region IActionHandler Members

    /// <summary>
    ///   Get the Assigned action for the pressed Key
    /// </summary>
    /// <param name = "iWindow"></param>
    /// <param name = "key"></param>
    /// <param name = "action"></param>
    /// <returns></returns>
    public bool GetAction(int iWindow, Keys key, ref Action action)
    {
      // Don't handle normal typing (exclude 0-9 and A-Z)
      if ((int)key > 47 && (int)key < 91) return false;
      int wAction = GetActionCode(iWindow, key);
      if (wAction == 0) return false;
      // Now fill our action structure
      action.ID = (Action.ActionType)wAction;
      return true;
    }

    public string GetKeyCode(Action.ActionType action)
    {
      foreach (Button but in mapWindows[0].Buttons)
      {
        if (but.ActionType == action)
        {
          return but.RibbonKeyCode;
        }
      }
      return "";
    }

    /// <summary>
    ///   Loads the keymap file and creates the mapping.
    /// </summary>
    /// <returns>True if the load was successfull, false if it failed.</returns>
    public bool LoadKeyMap()
    {
      mapWindows.Clear();
      string strFilename = String.Format(@"{0}\{1}", Options.ConfigDir, "keymap.xml");
      if (!File.Exists(strFilename))
        strFilename = String.Format(@"{0}\bin\{1}", Application.StartupPath, "keymap.xml");
      Log.Info("  Load key mapping from {0}", strFilename);
      try
      {
        // Load the XML file
        XmlDocument doc = new XmlDocument();
        doc.Load(strFilename);
        // Check if it is a keymap
        if (doc.DocumentElement == null) return false;
        string strRoot = doc.DocumentElement.Name;
        if (strRoot != "keymap") return false;

        // For each window
        XmlNodeList listWindows = doc.DocumentElement.SelectNodes("/keymap/window");
        foreach (XmlNode nodeWindow in listWindows)
        {
          XmlNode nodeWindowId = nodeWindow.SelectSingleNode("id");
          if (null != nodeWindowId)
          {
            WindowMap map = new WindowMap();
            map.Window = Int32.Parse(nodeWindowId.InnerText);
            XmlNodeList listNodes = nodeWindow.SelectNodes("action");
            // Create a list of key/actiontype mappings
            foreach (XmlNode node in listNodes)
            {
              XmlNode nodeId = node.SelectSingleNode("id");
              XmlNode nodeKey = node.SelectSingleNode("key");
              XmlNode nodeRibbonKey = node.SelectSingleNode("ribbon");
              MapAction(ref map, nodeId, nodeKey, nodeRibbonKey);
            }
            if (map.Buttons.Count > 0)
            {
              mapWindows.Add(map);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("exception loading keymap {0} err:{1} stack:{2}", strFilename, ex.Message, ex.StackTrace);
      }
      return false;
    }

    #endregion

    #region Private Methods

    /// <summary>
    ///   Map an action in a windowmap based on the id and key xml nodes.
    /// </summary>
    /// <param name = "map">The windowmap that needs to be filled in.</param>
    /// <param name = "nodeId">The id of the action</param>
    /// <param name = "nodeKey">The key corresponding to the mapping.</param>
    private void MapAction(ref WindowMap map, XmlNode nodeId, XmlNode nodeKey, XmlNode nodeRibbonKey)
    {
      if (null == nodeId) return;
      Button but = new Button();
      but.ActionType = (Action.ActionType)Int32.Parse(nodeId.InnerText);

      if (nodeRibbonKey != null)
      {
        but.RibbonKeyCode = nodeRibbonKey.InnerText;
      }

      if (nodeKey != null)
      {
        string[] buttons = nodeKey.InnerText.Split('-');
        for (int i = 0; i < buttons.Length - 1; i++)
        {
          if (buttons[i] == "Alt")
            but.Modifiers |= Keys.Alt;
          else if (buttons[i] == "Ctrl")
            but.Modifiers |= Keys.Control;
          else if (buttons[i] == "Shift")
            but.Modifiers |= Keys.Shift;
        }

        string strButton = buttons[buttons.Length - 1];

        try
        {
          if (strButton != "")
            but.KeyCode = (int)Enum.Parse(typeof (Keys), strButton);
        }
        catch (ArgumentException)
        {
          Log.Error("Invalid buttons for action {0}", nodeId.InnerText);
        }
      }

      map.Buttons.Add(but);
    }

    /// <summary>
    ///   Gets the action based on a window key combination
    /// </summary>
    /// <param name = "wWindow">The window id.</param>
    /// <param name = "key">The key.</param>
    /// <returns>The action if it is found in the map, 0 if not.</returns>
    private int GetActionCode(int wWindow, Keys key)
    {
      foreach (WindowMap window in mapWindows)
      {
        if (window.Window == wWindow)
        {
          foreach (Button but in window.Buttons)
          {
            if ((but.KeyCode | (int)but.Modifiers) == (int)key)
              return (int)but.ActionType;
          }
          return 0;
        }
      }
      return 0;
    }

    #endregion
  }
}