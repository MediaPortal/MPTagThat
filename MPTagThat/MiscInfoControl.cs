using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat
{
  public partial class MiscInfoControl : UserControl
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private IThemeManager themeManager = ServiceScope.Get<IThemeManager>();
    #endregion


    #region ctor
    public MiscInfoControl()
    {
      InitializeComponent();

      // Listen to Messages
      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);

      LocaliseScreen();
      SetColorBase();

      // Build the Context Menu for the Error Grid
      MenuItem[] rmitems = new MenuItem[1];
      rmitems[0] = new MenuItem();
      rmitems[0].Text = "Clear List";
      rmitems[0].Click += new System.EventHandler(dataGridViewError_ClearList);
      rmitems[0].DefaultItem = true;
      this.dataGridViewError.ContextMenu = new ContextMenu(rmitems);
    }
    #endregion

    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      this.dataGridViewError.Columns[0].HeaderText = localisation.ToString("main", "ErrorHeaderFile");
      this.dataGridViewError.Columns[1].HeaderText = localisation.ToString("main", "ErrorHeaderMessage");
    }

    private void SetColorBase()
    {
      this.BackColor = themeManager.CurrentTheme.BackColor;
      // We want to have our own header color
      dataGridViewError.EnableHeadersVisualStyles = false;
      dataGridViewError.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      dataGridViewError.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Returns the Error Gridview
    /// </summary>
    public DataGridView ErrorGridView
    {
      get { return this.dataGridViewError; }
    }
    #endregion

    #region Events
    #region Error Grid
    /// <summary>
    /// Handle Right Mouse Click to open the context Menu in the Error DataGrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void datagridViewError_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        this.dataGridViewError.ContextMenu.Show(dataGridViewError, new Point(e.X, e.Y));
    }

    /// <summary>
    /// Context Menu entry has been selected
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void dataGridViewError_ClearList(object o, System.EventArgs e)
    {
      dataGridViewError.Rows.Clear();
    }
    #endregion


    #endregion

    #region General Message Handling
    /// <summary>
    /// Handle Messages
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        case "themechanged":
          {
            SetColorBase();
            break;
          }

        case "languagechanged":
          {
            LocaliseScreen();
            this.Refresh();
            break;
          }
      }
    }
    #endregion
  }
}
