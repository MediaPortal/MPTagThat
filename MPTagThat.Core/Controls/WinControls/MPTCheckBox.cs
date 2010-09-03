using System;
using System.ComponentModel;
using System.Drawing;

namespace MPTagThat.Core.WinControls
{
  public class MPTCheckBox : System.Windows.Forms.CheckBox
  {
    #region Variables
    private string _localisation;
    private string _localisationContext;
    private IThemeManager themeManager;
    #endregion

    #region Properties
    /// <summary>
    /// The string to do the locilisation for
    /// </summary>
    [Bindable(true), Category("MPTagThat Options"), DefaultValue(""),
    Description("The string that the system will look for in the localisation file")]
    public string Localisation
    {
      get { return this._localisation; }
      set { this._localisation = value; }
    }

    /// <summary>
    /// The Localisation Context
    /// </summary>
    [Bindable(true), Category("MPTagThat Options"), DefaultValue(""),
    Description("The context to search for localisation. Default is the parent Context.")]
    public string LocalisationContext
    {
      get { return this._localisationContext; }
      set { this._localisationContext = value; }
    }

    public override string Text
    {
      get
      {
        if (_localisation == null || _localisation == "")
          _localisation = base.Name;

        if (_localisationContext == null || _localisationContext == "")
        {
          if (this.Parent != null)
            _localisationContext = this.Parent.Name;
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
    public MPTCheckBox()
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
            this.ForeColor = themeManager.CurrentTheme.LabelForeColor;
            this.Font = themeManager.CurrentTheme.LabelFont;
            break;
          }

        case "languagechanged":
          this.Text = MPTWinControlsCommon.Localise(_localisationContext, _localisation);
          this.Refresh();
          break;
      }
    }
    #endregion
  }
}
