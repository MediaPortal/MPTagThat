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
#region

using System;
using System.IO;
using System.Text;
using MPTagThat.Core;

#endregion

namespace MPTagThat.Player
{
  public class PlayListM3uIO : IPlayListIO
  {
    #region Variables

    private const string M3U_START_MARKER = "#EXTM3U";
    private const string M3U_INFO_MARKER = "#EXTINF";
    private readonly NLog.Logger log;
    private string basePath;
    private StreamReader file;
    private SortableBindingList<PlayListData> playlist;

    #endregion

    #region ctor

    public PlayListM3uIO()
    {
      log = ServiceScope.Get<ILogger>().GetLogger;
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


    public void Save(SortableBindingList<PlayListData> playlist, string fileName, bool useRelativePath)
    {
      try
      {
        using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.Default))
        {
          writer.WriteLine(M3U_START_MARKER);

          foreach (PlayListData item in playlist)
          {
            writer.WriteLine("{0}:{1},{2}", M3U_INFO_MARKER, Util.DurationToSeconds(item.Duration),
                             string.Format("{0} - {1}", item.Artist, item.Title));

            string musicFile = item.FileName;
            if (useRelativePath)
            {
              musicFile =
                Path.Combine(
                  Util.RelativePathTo(Path.GetDirectoryName(fileName), Path.GetDirectoryName(item.FileName)),
                  Path.GetFileName(item.FileName));
            }
            writer.WriteLine("{0}", musicFile);
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
      int iColon = trimmedLine.IndexOf(":");
      int iComma = trimmedLine.IndexOf(",");
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

      Util.GetQualifiedFilename(basePath, ref fileName);
      PlayListData newItem = new PlayListData(songName, fileName, duration);
      playlist.Add(newItem);
      return true;
    }

    #endregion
  }
}