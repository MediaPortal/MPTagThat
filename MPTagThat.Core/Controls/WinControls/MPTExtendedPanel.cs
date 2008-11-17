using System;
using System.ComponentModel;
using System.Drawing;

namespace MPTagThat.Core.WinControls
{
  public class TTExtendedPanel : Stepi.UI.ExtendedPanel
  {
    #region Variables
    private IThemeManager themeManager;
    #endregion

    #region ctor
    public TTExtendedPanel()
    {
      themeManager = ServiceScope.Get<IThemeManager>();
      // Setup message queue for receiving Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Handle Messages
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        // Message sent, when a Theme is changing
        case "themechanged":
          {
            this.BackColor = themeManager.CurrentTheme.BackColor;
            this.CaptionColorOne = themeManager.CurrentTheme.PanelHeadingBackColor;
            this.CaptionFont = themeManager.CurrentTheme.PanelHeadingFont;
            this.CaptionTextColor = themeManager.CurrentTheme.LabelForeColor;
            this.DirectionCtrlColor = themeManager.CurrentTheme.PanelHeadingDirectionCtrlColor;
            break;
          }

        case "languagechanged":
          this.Refresh();
          break;
      }
    }
    #endregion
  }
}
