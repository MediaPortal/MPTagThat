using System;
using System.ComponentModel;
using System.Drawing;

namespace MPTagThat.Core.WinControls
{
  public class TTPanel : System.Windows.Forms.Panel
  {
    #region Variables
    private string _localisation;
    private string _localisationContext;
    private IThemeManager themeManager;
    #endregion

    #region ctor
    public TTPanel()
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
            break;
          }
      }
    }
    #endregion
  }
}
