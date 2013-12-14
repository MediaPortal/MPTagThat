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
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat
{
  public partial class DatabaseSearchControl : UserControl
  {
    #region Variables

    private readonly Main _main;
    private readonly ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private readonly NLog.Logger log = ServiceScope.Get<ILogger>().GetLogger;

    #endregion

    #region ctor

    public DatabaseSearchControl(Main main)
    {
      // Activates double buffering 
      this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.OptimizedDoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();

      _main = main;

      InitializeComponent();
    }

    #endregion

    #region Events

    private void buttonSearch_Click(object sender, EventArgs e)
    {
      if (!File.Exists(Options.MainSettings.MediaPortalDatabase))
      {
        MessageBox.Show(localisation.ToString("dbsearch", "NoMusicDB"), "", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        return;
      }

      if (tbArtist.Text.Trim() == "" && tbAlbum.Text.Trim() == "" && tbTitle.Text.Trim() == "")
      {
        MessageBox.Show(localisation.ToString("dbsearch", "NoSearchValues"), "", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        return;
      }

      if (!_main.TreeView.DatabaseMode)
      {
        _main.TreeView.DatabaseMode = true;
      }

      string sql = "select strPath from tracks where {0} order by {1}";

      string whereClause = "";
      string orderByClause = "";

      if (tbArtist.Text.Trim() != "")
      {
        whereClause += string.Format("(strArtist like '%{0}%' OR strAlbumArtist like '%{0}%')",
                                     Util.RemoveInvalidChars(tbArtist.Text.Trim()));
        orderByClause += "strArtist";
      }

      if (tbAlbum.Text.Trim() != "")
      {
        if (whereClause != "")
        {
          whereClause += " AND ";
        }
        whereClause += string.Format("strAlbum like '%{0}%' ", Util.RemoveInvalidChars(tbAlbum.Text.Trim()));

        if (orderByClause != "")
        {
          orderByClause += ",";
        }
        orderByClause += "strAlbum";
      }

      if (tbTitle.Text.Trim() != "")
      {
        if (whereClause != "")
        {
          whereClause += " AND ";
        }
        whereClause += string.Format("strTitle like '%{0}%' ", Util.RemoveInvalidChars(tbTitle.Text.Trim()));
        if (orderByClause != "")
        {
          orderByClause += ",";
        }
        orderByClause += "strTitle";
      }

      sql = string.Format(sql, whereClause, orderByClause);
      List<string> songs = new List<string>();

      string connection = string.Format(@"Data Source={0}", Options.MainSettings.MediaPortalDatabase);
      try
      {
        Cursor = Cursors.WaitCursor;
        int count = 0;
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = CommandType.Text;
          cmd.CommandText = sql;
          log.Debug("Database Scan: Executing sql: {0}", sql);
          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              songs.Add(reader.GetString(0));
              count++;
              if (count > 999)
              {
                Cursor = Cursors.Default;
                MessageBox.Show(localisation.ToString("dbsearch", "TooMuchRows"), "", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                break;
              }
            }
          }
        }
        Cursor = Cursors.Default;
        conn.Close();
      }
      catch (Exception ex)
      {
        log.Error("Database Scan: Error executing sql: {0}", ex.Message);
      }

      log.Debug("Database Scan: Query returned {0} songs", songs.Count);
      _main.TracksGridView.AddDatabaseSongsToGrid(songs);
    }

    #endregion
  }
}