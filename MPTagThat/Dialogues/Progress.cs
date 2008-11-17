using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.Dialogues
{
  public partial class Progress : Form
  {
    #region Variables
    private delegate void ThreadSafeProgressUpdateDelegate(ProgressBarStyle style, string formattedMsg, int curCount, int trackCount, bool showProgressBar);
    private bool _cancel = false;
    #endregion

    #region Properties
    public bool IsCancelled
    {
      get { return _cancel; }
    }

    public string StatusLabel
    {
      set { this.labelStatus.Text = value; }
    }

    public string StatusLabel2
    {
      set { this.labelStatus2.Text = value; }
    }
    #endregion

    #region ctor
    public Progress()
    {
      InitializeComponent();

      this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      this.labelStatus.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
      this.labelStatus.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelFont;
      this.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelFont;
      this.buttonCancel.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.ButtonBackColor;
      this.buttonCancel.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.ButtonForeColor;
      this.buttonCancel.Font = ServiceScope.Get<IThemeManager>().CurrentTheme.ButtonFont;

      this.labelStatus.Text = "";
      this.labelStatus2.Text = "";
    }
    #endregion

    #region Public Megthods
    public void UpdateProgress(ProgressBarStyle style, string formattedMsg, int curCount, int trackCount, bool showProgressBar)
    {
      if (this.InvokeRequired)
      {
        ThreadSafeProgressUpdateDelegate d = new ThreadSafeProgressUpdateDelegate(UpdateProgress);
        this.Invoke(d, new object[] { style, formattedMsg, curCount, trackCount, showProgressBar });
        return;
      }

      progressBarScanning.Maximum = trackCount;
      progressBarScanning.Value = curCount;

      labelStatus.Text = formattedMsg;
      Application.DoEvents();
    }
    #endregion

    #region Private Methods
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      _cancel = true;
    }
    #endregion
  }
}