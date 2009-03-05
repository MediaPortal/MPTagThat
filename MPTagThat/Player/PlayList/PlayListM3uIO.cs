#region Copyright (C) 2005-2009 Team MediaPortal

/* 
 *	Copyright (C) 2005-2009 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.IO;
using System.Text;
using MPTagThat.Core;

namespace MPTagThat.Player
{
  public class PlayListM3uIO : IPlayListIO
  {
    #region Variables
    private const string M3U_START_MARKER = "#EXTM3U";
    private const string M3U_INFO_MARKER = "#EXTINF";
    private SortableBindingList<PlayListData> playlist;
    private StreamReader file;
    private string basePath;
    private ILogger log;
    #endregion

    #region ctor
    public PlayListM3uIO()
    {
      log = ServiceScope.Get<ILogger>();
    }
    #endregion

    #region Public Methods
    public bool Load(SortableBindingList<PlayListData> incomingPlaylist, string playlistFileName)
    {
      if (playlistFileName == null)
      {
        return false;
      }
      playlist = incomingPlaylist;
      playlist.Clear();

      try
      {
        basePath = Path.GetDirectoryName(Path.GetFullPath(playlistFileName));

        using (file = new StreamReader(playlistFileName, Encoding.Default, true))
        {
          if (file == null)
          {
            return false;
          }

          string line = file.ReadLine();
          if (line == null || line.Length == 0)
          {
            return false;
          }

          string trimmedLine = line.Trim();

          if (trimmedLine != M3U_START_MARKER)
          {
            string fileName = trimmedLine;
            if (!AddItem("", "0", fileName))
            {
              return false;
            }
          }

          line = file.ReadLine();
          while (line != null)
          {
            trimmedLine = line.Trim();

            if (trimmedLine != "")
            {
              if (trimmedLine.StartsWith(M3U_INFO_MARKER))
              {
                string songName = null;
                string lDuration = "0";

                if (ExtractM3uInfo(trimmedLine, ref songName, ref lDuration))
                {
                  line = file.ReadLine();
                  if (!AddItem(songName, Util.SecondsToHMSString(lDuration), line))
                  {
                    break;
                  }
                }
              }
              else
              {
                if (!AddItem("", "0", trimmedLine))
                {
                  break;
                }
              }
            }
            line = file.ReadLine();
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("exception loading playlist {0} err:{1} stack:{2}", playlistFileName, ex.Message, ex.StackTrace);
        return false;
      }
      return true;
    }


    public void Save(SortableBindingList<PlayListData> playlist, string fileName)
    {
      try
      {
        using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.Default))
        {
          writer.WriteLine(M3U_START_MARKER);

          foreach (PlayListData item in playlist)
          {
            writer.WriteLine("{0}:{1},{2}", M3U_INFO_MARKER, Util.DurationToSeconds(item.Duration), string.Format("{0} - {1}", item.Artist, item.Title));
            writer.WriteLine("{0}", item.FileName);
          }
        }
      }
      catch (Exception e)
      {
        log.Error("failed to save a playlist {0}. err: {1} stack: {2}", fileName, e.Message, e.StackTrace);
      }
    }
    #endregion

    #region Private Methods
    private static bool ExtractM3uInfo(string trimmedLine, ref string songName, ref string lDuration)
    {
      //bool successfull;
      int iColon = (int) trimmedLine.IndexOf(":");
      int iComma = (int) trimmedLine.IndexOf(",");
      if (iColon >= 0 && iComma >= 0 && iComma > iColon)
      {
        iColon++;
        string duration = trimmedLine.Substring(iColon, iComma - iColon);
        iComma++;
        songName = trimmedLine.Substring(iComma);
        lDuration = duration;
        return true;
      }
      return false;
    }


    private bool AddItem(string songName, string duration, string fileName)
    {
      if (fileName == null || fileName.Length == 0)
      {
        return false;
      }

      PlayListData newItem = new PlayListData(songName, fileName, duration);
      playlist.Add(newItem);
      return true;
    }
    #endregion
  }
}