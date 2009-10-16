#region Copyright (C) 2007-2008 Team MediaPortal

/*
    Copyright (C) 2007-2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Xml;

namespace MPTagThat.Core
{
  public class ThemeManager : IThemeManager
  {
    #region Variables
    private ILogger Logger;
    private string _selectedTheme;
    private Theme _currentTheme;
    #endregion

    #region Properties
    public Theme CurrentTheme
    {
      get { return _currentTheme; }
    }
    #endregion

    #region ctor / dtor
    public ThemeManager()
    {
      Logger = ServiceScope.Get<ILogger>();
      _currentTheme = new Theme();
    }
    #endregion

    #region IThemeManager implementation
    public void ChangeTheme(string aThemeName)
    {
      LoadTheme(aThemeName);
      NotifyThemeChange();
    }

    public void NotifyThemeChange()
    {
      QueueMessage msg = new QueueMessage();
      msg.MessageData["action"] = "themechanged";
      IMessageQueue queue = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queue.Send(msg);
    }
    #endregion

    #region Private Methods
    private void LoadTheme(string aTheme)
    {
      if (_currentTheme.ThemeName == aTheme)
        return;

      XmlDocument themeDoc = new XmlDocument();
      themeDoc.Load("Themes\\Themes.xml");

      XmlNode root = themeDoc.DocumentElement;
      XmlNodeList themesList = root.SelectNodes("Theme");
      foreach (XmlNode themeNode in themesList)
      {
        XmlAttributeCollection attributes = themeNode.Attributes;
        if (attributes.GetNamedItem("name").Value == aTheme)
        {
          Theme theme = new Theme();
          theme.ThemeName = aTheme;
          string[] colorArray = attributes.GetNamedItem("BackColor").Value.Split(',');
          theme.BackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));

          foreach (XmlNode attr in themeNode.ChildNodes)
          {
            switch (attr.Name)
            {
              case "Label" :
                colorArray = attr.Attributes.GetNamedItem("color").Value.Split(',');
                theme.LabelForeColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                theme.LabelFont = new Font(attr.Attributes.GetNamedItem("font").Value, (float)Convert.ToDecimal(attr.Attributes.GetNamedItem("size").Value, CultureInfo.InvariantCulture), FontStyle.Regular);
                break;

              case "PanelHeading":
                colorArray = attr.Attributes.GetNamedItem("backcolor").Value.Split(',');
                theme.PanelHeadingBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("directionctrlcolor").Value.Split(',');
                theme.PanelHeadingDirectionCtrlColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                theme.PanelHeadingFont = new Font(attr.Attributes.GetNamedItem("font").Value, (float)Convert.ToDecimal(attr.Attributes.GetNamedItem("size").Value, CultureInfo.InvariantCulture), FontStyle.Regular);
                break;

              case "FormHeader":
                colorArray = attr.Attributes.GetNamedItem("forecolor").Value.Split(',');
                theme.FormHeaderForeColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                theme.FormHeaderFont = new Font(attr.Attributes.GetNamedItem("font").Value, (float)Convert.ToDecimal(attr.Attributes.GetNamedItem("size").Value, CultureInfo.InvariantCulture), FontStyle.Regular);
                break;

              case "GridView":
                colorArray = attr.Attributes.GetNamedItem("defaultbackcolor").Value.Split(',');
                theme.DefaultBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("selectionbackcolor").Value.Split(',');
                theme.SelectionBackColor= Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("alternatingrowbackcolor").Value.Split(',');
                theme.AlternatingRowBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("alternatingrowforecolor").Value.Split(',');
                theme.AlternatingRowForeColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("changedbackcolor").Value.Split(',');
                theme.ChangedBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("changedforecolor").Value.Split(',');
                theme.ChangedForeColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("fixableerrorbackcolor").Value.Split(',');
                theme.FixableErrorBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("fixableerrorforecolor").Value.Split(',');
                theme.FixableErrorForeColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("nonfixableerrorbackcolor").Value.Split(',');
                theme.NonFixableErrorBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("nonfixableerrorforecolor").Value.Split(',');
                theme.NonFixableErrorForeColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("findreplacebackcolor").Value.Split(',');
                theme.FindReplaceBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("findreplaceforecolor").Value.Split(',');
                theme.FindReplaceForeColor= Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                break;

              case "Button":
                colorArray = attr.Attributes.GetNamedItem("backcolor").Value.Split(',');
                theme.ButtonBackColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                colorArray = attr.Attributes.GetNamedItem("color").Value.Split(',');
                theme.ButtonForeColor = Color.FromArgb(Convert.ToInt16(colorArray[0]), Convert.ToInt16(colorArray[1]), Convert.ToInt16(colorArray[2]), Convert.ToInt16(colorArray[3]));
                theme.ButtonFont = new Font(attr.Attributes.GetNamedItem("font").Value, (float)Convert.ToDecimal(attr.Attributes.GetNamedItem("size").Value, CultureInfo.InvariantCulture), FontStyle.Regular);
                break;

            }
          }
          _currentTheme = theme;
          _selectedTheme = aTheme;
        }
      }
    }
    #endregion
  }
}
