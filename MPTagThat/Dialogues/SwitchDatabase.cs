#region Copyright (C) 2009-2017 Team MediaPortal
// Copyright (C) 2009-2017 Team MediaPortal
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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using MPTagThat.Core;
using MPTagThat.Core.Services.MusicDatabase;

namespace MPTagThat.Dialogues
{
  public partial class SwitchDatabase : ShapedForm
  {
    #region Variables

    private readonly List<Database> _databases = new List<Database>();

    #endregion

    #region Properties

    public string Description { get; set; }

    #endregion
    
    #region ctor

    public SwitchDatabase()
    {
      InitializeComponent();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Initialize the Form Content
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SwitchDatabase_Load(object sender, EventArgs e)
    {
      // Set the Theme
      base.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      dataGridViewDatabases.BackgroundColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
      ServiceScope.Get<IThemeManager>().NotifyThemeChange();

      GetDatabases();

      dataGridViewDatabases.AutoGenerateColumns = false;
      dataGridViewDatabases.DataSource = _databases;
    }

    /// <summary>
    /// Retrieves the existing databases from the folder structure
    /// </summary>
    private void GetDatabases()
    {
      foreach (var folder in Directory.GetDirectories(Options.StartupSettings.DatabaseFolder))
      {
        if (folder.EndsWith("SongsTemp")) // ignore the temp database
          continue;

        if (File.Exists(Path.Combine(folder, "raven-data.ico")))
        {
          var database = new Database
          {
            Folder = folder.Substring(folder.LastIndexOf("\\") + 1),
            DatabaseDescription = ReadDatabaseDescription(folder)
          };
          _databases.Add(database);
        }
      }
    }

    /// <summary>
    /// Returns the Databasename from Folder
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    private string ReadDatabaseDescription(string folder)
    {
      string databaseDescription = "";
      try
      {
        databaseDescription = File.ReadAllText(Path.Combine(folder, "mptagthat-databaseninfo.txt"));
      }
      catch (Exception)
      {
        if (folder.EndsWith("MusicDatabase"))
        {
          databaseDescription = ServiceScope.Get<ILocalisation>().ToString("dbswitch", "defaultdatabasedescription");
        }
        else
        {
          databaseDescription = ServiceScope.Get<ILocalisation>().ToString("dbswitch", "notavailable");
        }
      }

      return databaseDescription;
    }

    #endregion

    #region Events

    /// <summary>
    /// Form is closed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btClose_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(Description))
      {
        Description =
          ReadDatabaseDescription(Path.Combine(Options.StartupSettings.DatabaseFolder,
            ServiceScope.Get<IMusicDatabase>().CurrentDatabase));
      }
    }

    /// <summary>
    /// If one of the Icon columns has been clicked, execute the function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dataGridViewDatabases_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      var folder = _databases[e.RowIndex].Folder;
      if (e.ColumnIndex == 1)
      {
        ServiceScope.Get<IMusicDatabase>().SwitchDatabase(folder);
        Description = _databases[e.RowIndex].DatabaseDescription;
        Close();
      }
      else
      {
        ServiceScope.Get<IMusicDatabase>().DeleteDatabase(folder);
        _databases.RemoveAt(e.RowIndex);
        dataGridViewDatabases.DataSource = null;
        dataGridViewDatabases.DataSource = _databases;
      }
    }

    /// <summary>
    /// Add the database and switch to it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btDatabaseAdd_Click(object sender, EventArgs e)
    {
      // All non-default databases start with "DB", so we get the element before the last, which is "MusicDatabase"
      var databaseNumber = 0;
      if (_databases.Count == 1)
      {
        databaseNumber = 1;
      }
      else
      {
        var name = _databases[_databases.Count - 2].Folder;
        databaseNumber = Int32.Parse(name.Substring(2)) + 1;
      }

      var dbName = $"DB{databaseNumber}";
      if (ServiceScope.Get<IMusicDatabase>().SwitchDatabase(dbName))
      {
        var text = tbDatabaseDescription.Text.Trim() == "" ? "N/A" : tbDatabaseDescription.Text.Trim();
        var dbPath = Path.Combine(Options.StartupSettings.DatabaseFolder, dbName);
        using (StreamWriter sw = new StreamWriter(Path.Combine(dbPath, "mptagthat-databaseninfo.txt")))
        {
          sw.WriteLine(text);
        }
        Description = text;
        Close();
      }
    }

    /// <summary>
    /// Default Dataerror handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dataGridViewDatabases_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      return;
    }

    #endregion

    #region local classes

    private class Database
    {
      public string Folder { get; set; }
      public string DatabaseDescription { get; set; }
    }

    #endregion
  }

}
