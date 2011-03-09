using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MPTagThat.TagEdit
{
  public partial class TagEditControl : UserControl
  {
    #region Variables


    #endregion

    #region ctor
    public TagEditControl()
    {
      InitializeComponent();
    }

    #endregion

    #region Properties

    public string LyricsText
    {
      get { return tbLyrics.Text; }
      set { tbLyrics.Text = value; }
    }

    #endregion
  }
}
