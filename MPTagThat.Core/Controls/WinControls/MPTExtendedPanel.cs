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

using Stepi.UI;

#endregion

namespace MPTagThat.Core.WinControls
{
  public class TTExtendedPanel : ExtendedPanel
  {
    #region Variables

    private readonly IThemeManager themeManager;

    #endregion

    #region ctor

    public TTExtendedPanel()
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
            BackColor = themeManager.CurrentTheme.BackColor;
            CaptionColorOne = themeManager.CurrentTheme.PanelHeadingBackColor;
            CaptionFont = themeManager.CurrentTheme.PanelHeadingFont;
            CaptionTextColor = themeManager.CurrentTheme.LabelForeColor;
            DirectionCtrlColor = themeManager.CurrentTheme.PanelHeadingDirectionCtrlColor;
            break;
          }

        case "languagechanged":
          Refresh();
          break;
      }
    }

    #endregion
  }
}