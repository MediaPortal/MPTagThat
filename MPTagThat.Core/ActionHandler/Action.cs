using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class Action
  {
    #region enums
    public enum ActionType
    {
      ACTION_INVALID = 0,
      ACTION_SAVE = 1,
      ACTION_MULTI_EDIT = 2,
      ACTION_FILENAME2TAG = 3,
      ACTION_TAG2FILENAME = 4,
      ACTION_SELECTALL = 5,
      ACTION_COPY = 6,
      ACTION_PASTE = 7,
      ACTION_SCRIPTEXECUTE = 8,
      ACTION_TREEREFRESH = 9,
      ACTION_FOLDERDELETE = 10,
      ACTION_CASECONVERSION = 11,
      ACTION_REFRESH = 12,
      ACTION_OPTIONS = 13,
      ACTION_DELETE = 14,
      ACTION_PAGEDOWN = 15,
      ACTION_PAGEUP = 16,
      ACTION_NEXTFILE = 17,
      ACTION_PREVFILE = 18,
      ACTION_EDIT = 19,
      ACTION_ORGANISE = 20,
      ACTION_IDENTIFYFILE = 21,
      ACTION_GETCOVERART = 22,
      ACTION_GETLYRICS = 23,
      ACTION_EXIT = 24,
      ACTION_HELP = 25,
      ACTION_TAGFROMINTERNET = 26,
      ACTION_TOGGLESPLITTER = 27,
      ACTION_REMOVECOMMENT = 28,
      ACTION_REMOVEPICTURE = 29,
      ACTION_SAVEALL = 30,
    }
    #endregion

    #region Variables
    private ActionType id;
    #endregion

    #region Properties
    public ActionType ID
    {
      get { return id; }
      set { id = value; }
    }
    #endregion

    #region ctor
    /// <summary>
    /// The (emtpy) constructur of the Action class.
    /// </summary>
    public Action()
    {
    }
    #endregion
  }
}
