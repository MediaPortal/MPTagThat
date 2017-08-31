#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
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
namespace MPTagThat.Core
{
  public class Action
  {
    #region enums

    public enum ActionType
    {
      ACTION_INVALID = 0,
      ACTION_SAVE = 1,
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
      ACTION_ORGANISE = 20,
      ACTION_IDENTIFYFILE = 21,
      ACTION_GETCOVERART = 22,
      ACTION_GETLYRICS = 23,
      ACTION_EXIT = 24,
      ACTION_HELP = 25,
      ACTION_TAGFROMINTERNET = 26,
      ACTION_TOGGLESTREEVIEWSPLITTER = 27,
      ACTION_REMOVECOMMENT = 28,
      ACTION_REMOVEPICTURE = 29,
      ACTION_SAVEALL = 30,
      ACTION_VALIDATEMP3 = 32,
      ACTION_FIXMP3 = 33,
      ACTION_FIND = 34,
      ACTION_REPLACE = 35,
      ACTION_TOGGLEQUICKEDIT = 36,
      ACTION_TOGGLEMISCFILES = 37,
      ACTION_REPLAYGAIN = 38,
    }

    #endregion

    #region Variables

    #endregion

    #region Properties

    public ActionType ID { get; set; }

    #endregion

    #region ctor

    public static string ActionToCommand(Action action, ref bool checkSelections)
    {
      switch (action.ID)
      {
        case ActionType.ACTION_SAVE:
          return "Save";

        case ActionType.ACTION_SAVEALL:
          return "SaveAll";

        case ActionType.ACTION_IDENTIFYFILE:
          checkSelections = true;
          return "IdentifyFiles";

        case Action.ActionType.ACTION_GETCOVERART:
          checkSelections = true;
          return "GetCoverArt";

        case Action.ActionType.ACTION_GETLYRICS:
          checkSelections = true;
          return "GetLyrics";

        case Action.ActionType.ACTION_REMOVECOMMENT:
          checkSelections = true;
          return "RemoveComments";

        case Action.ActionType.ACTION_REMOVEPICTURE:
          checkSelections = true;
          return "RemoveCoverArt";

        case Action.ActionType.ACTION_VALIDATEMP3:
          checkSelections = true;
          return "ValidateMP3File";

        case Action.ActionType.ACTION_FIXMP3:
          checkSelections = true;
          return "FixMP3File";

        case Action.ActionType.ACTION_REPLAYGAIN:
          checkSelections = true;
          return "ReplayGain";

        default:
          return "";
      }
    }

    #endregion
  }
}