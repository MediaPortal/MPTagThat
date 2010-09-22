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

using System.ComponentModel;
using System.Windows.Forms;

#endregion

namespace MPTagThat.Core.WinControls
{
  public class MPTGroupBox : GroupBox
  {
    #region Variables

    private readonly IThemeManager themeManager;
    private string _localisation;
    private string _localisationContext;

    #endregion

    #region Properties

    /// <summary>
    ///   The string to do the locilisation for
    /// </summary>
    [Bindable(true), Category("MPTagThat Options"), DefaultValue(""),
     Description("The string that the system will look for in the localisation file")]
    public string Localisation
    {
      get { return _localisation; }
      set { _localisation = value; }
    }

    /// <summary>
    ///   The Localisation Context
    /// </summary>
    [Bindable(true), Category("MPTagThat Options"), DefaultValue(""),
     Description("The context to search for localisation. Default is the parent Context.")]
    public string LocalisationContext
    {
      get { return _localisationContext; }
      set { _localisationContext = value; }
    }

    public override string Text
    {
      get
      {
        if (_localisation == null || _localisation == "")
          _localisation = base.Name;

        if (_localisationContext == null || _localisationContext == "")
        {
          if (Parent != null)
            _localisationContext = Parent.Name;
          else
            _localisationContext = "";
        }

        string localisedText = null;
        if (_localisationContext == "")
          return base.Text;

        localisedText = MPTWinControlsCommon.Localise(_localisationContext, _localisation);
        if (localisedText == null)
          return base.Text;
        else
          return localisedText;
      }
    }

    #endregion

    #region ctor

    public MPTGroupBox()
    {
      themeManager = ServiceScope.Get<IThemeManager>();
      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += OnMessageReceive;
    }

    #endregion

    #region Private Methods

    /// <summary>
    ///   Handle Messages
    /// </summary>
    /// <param name = "message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
          // Message sent, when a Theme is changing
        case "themechanged":
          {
            ForeColor = themeManager.CurrentTheme.LabelForeColor;
            Font = themeManager.CurrentTheme.LabelFont;
            break;
          }

        case "languagechanged":
          Text = MPTWinControlsCommon.Localise(_localisationContext, _localisation);
          Refresh();
          break;
      }
    }

    #endregion
  }
}