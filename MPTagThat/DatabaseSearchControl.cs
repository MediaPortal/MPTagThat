using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;

using MPTagThat.Core;

namespace MPTagThat
{
  public partial class DatabaseSearchControl : UserControl
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();
    #endregion

    #region ctor
    public DatabaseSearchControl(Main main)
    {
      _main = main;

      InitializeComponent();
    }
    #endregion

    #region Events
    private void buttonSearch_Click(object sender, EventArgs e)
    {
      if (!System.IO.File.Exists(Options.MainSettings.MediaPortalDatabase))
      {
        MessageBox.Show(localisation.ToString("dbsearch", "NoMusicDB"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      if (tbArtist.Text.Trim() == "" && tbAlbum.Text.Trim() == "" && tbTitle.Text.Trim() == "")
      {
        MessageBox.Show(localisation.ToString("dbsearch", "NoSearchValues"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      string sql = "select strPath from tracks where {0} order by {1}";

      string whereClause = "";
      string orderByClause = "";

      if (tbArtist.Text.Trim() != "")
      {
        whereClause += string.Format("strArtist like '%| {0}%' ", Util.RemoveInvalidChars(tbArtist.Text.Trim()));
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

      string connection = string.Format(@"Data Source={0}", MPTagThat.Core.Options.MainSettings.MediaPortalDatabase);
      try
      {
        this.Cursor = Cursors.WaitCursor;
        int count = 0;
        SQLiteConnection conn = new SQLiteConnection(connection);
        conn.Open();
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          cmd.Connection = conn;
          cmd.CommandType = System.Data.CommandType.Text;
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
                this.Cursor = Cursors.Default;
                MessageBox.Show(localisation.ToString("dbsearch", "TooMuchRows"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                break;
              }
            }
          }
        }
        this.Cursor = Cursors.Default;
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
