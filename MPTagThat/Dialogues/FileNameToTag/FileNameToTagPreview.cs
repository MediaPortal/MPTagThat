using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPTagThat.Core;

namespace MPTagThat.FileNameToTag
{
  public partial class FileNameToTagPreview : Telerik.WinControls.UI.ShapedForm
  {
    #region Variables
    BindingList<TrackDataPreview> _previewTracks = new BindingList<TrackDataPreview>();
    private IThemeManager themeManager = ServiceScope.Get<IThemeManager>();
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    #endregion

    #region Properties
    public BindingList<TrackDataPreview> Tracks
    {
      get { return _previewTracks; }
    }
    #endregion

    #region ctor
    public FileNameToTagPreview()
    {
      InitializeComponent();

      // Insert the first Column
      AddGridColumn(0, "FileName", "File", 250);
    }
    #endregion

    #region Form Load
    /// <summary>
    /// The form is loaded to some initial work
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnLoad(object sender, EventArgs e)
    {
      this.BackColor = themeManager.CurrentTheme.BackColor;
      this.dataGridViewPreview.BackgroundColor = themeManager.CurrentTheme.BackColor;

      dataGridViewPreview.EnableHeadersVisualStyles = false;
      dataGridViewPreview.ColumnHeadersDefaultCellStyle.BackColor = themeManager.CurrentTheme.PanelHeadingBackColor;
      dataGridViewPreview.ColumnHeadersDefaultCellStyle.ForeColor = themeManager.CurrentTheme.LabelForeColor;
      dataGridViewPreview.DefaultCellStyle.BackColor = themeManager.CurrentTheme.DefaultBackColor;
      dataGridViewPreview.DefaultCellStyle.SelectionBackColor = themeManager.CurrentTheme.SelectionBackColor;
      dataGridViewPreview.AlternatingRowsDefaultCellStyle.BackColor = themeManager.CurrentTheme.AlternatingRowBackColor;
      dataGridViewPreview.AlternatingRowsDefaultCellStyle.ForeColor = themeManager.CurrentTheme.AlternatingRowForeColor;

      this.dataGridViewPreview.AutoGenerateColumns = false;
      this.dataGridViewPreview.DataSource = _previewTracks;
    }
    #endregion

    #region Public Methods
    public void AddRemoveColumn(int position, string parm)
    {
      DataGridViewColumn column;
      if (dataGridViewPreview.Columns.Count - 1 < position)
      {
        // We don't have anything in preview yet
        column = new DataGridViewColumn();
      }
      else
      {
        column = dataGridViewPreview.Columns[position];
      }

      switch (parm)
      {
        case "<A>":
          if (column.Name != "Artist")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Artist", localisation.ToString("column_header", "Artist"), 150);
          }
          break;

        case "<T>":
          if (column.Name != "Title")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Title", localisation.ToString("column_header", "Title"), 150);
          }
          break;

        case "<B>":
          if (column.Name != "Album")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Album", localisation.ToString("column_header", "Album"), 150);
          }
          break;

        case "<Y>":
          if (column.Name != "Track")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Year", localisation.ToString("column_header", "Year"), 40);
          }
          break;

        case "<K>":
          if (column.Name != "Track")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Track", localisation.ToString("column_header", "Track"), 40);
          }
          break;

        case "<k>":
          if (column.Name != "NumTrack")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "NumTrack", localisation.ToString("column_header", "NumTracks"), 80);
          }
          break;

        case "<D>":
          if (column.Name != "Disc")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Disc", localisation.ToString("column_header", "Disc"), 40);
          }
          break;

        case "<d>":
          if (column.Name != "NumDisc")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "NumDisc", localisation.ToString("column_header", "NumDiscs"), 80);
          }
          break;

        case "<G>":
          if (column.Name != "Genre")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Genre", localisation.ToString("column_header", "Genre"), 100);
          }          	
          break;

        case "<O>":
          if (column.Name != "AlbumArtist")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "AlbumArtist", localisation.ToString("column_header", "AlbumArtist"), 150);
          }
          break;

        case "<C>":
          if (column.Name != "Comment")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Comment", localisation.ToString("column_header", "Comment"), 150);
          }
          break;

        case "<N>":
          if (column.Name != "Conductor")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Conductor", localisation.ToString("column_header", "Conductor"), 150);
          }
          break;

        case "<R>":
          if (column.Name != "Composer")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Composer", localisation.ToString("column_header", "Composer"), 150);
          }
          break;

        case "<U>":
          if (column.Name != "Grouping")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Grouping", localisation.ToString("column_header", "Grouping"), 150);
          }
          break;

        case "<S>":
          if (column.Name != "Subtitle")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Subtitle", localisation.ToString("column_header", "Subtitle"), 150);
          }
          break;

        case "<M>":
          if (column.Name != "Interpreter")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "Interpreter", localisation.ToString("column_header", "Interpreter"), 150);
          }
          break;

        case "<E>":
          if (column.Name != "BPM")
          {
            RemoveGridColumn(position);
            AddGridColumn(position, "BPM", localisation.ToString("column_header", "BPM"), 50);
          }
          break;

        case "<X>":
          // ignore it
          break;
      }
    }

    /// <summary>
    /// Adds a Grid Column using the selected name to the choosen position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="name"></param>
    /// <param name="header"></param>
    /// <param name="width"></param>
    public void AddGridColumn(int position, string name, string header, int width)
    {
      DataGridViewColumn column = new DataGridViewTextBoxColumn();
      column.Name = name;
      column.DataPropertyName = name;
      column.HeaderText = header;
      column.Width = width;
      dataGridViewPreview.Columns.Insert(position, column);
    }

    /// <summary>
    /// Removes the column at the given position
    /// </summary>
    /// <param name="position"></param>
    public void RemoveGridColumn(int position)
    {
      if (dataGridViewPreview.Columns.Count > position)
      {
        dataGridViewPreview.Columns.RemoveAt(position);
      }
    }

    /// <summary>
    /// Remove redundant Columns, which may be there because of previous format changes
    /// </summary>
    /// <param name="startPosition"></param>
    public void RemoveRedundantColumns(int startPosition)
    {
      int colCount = dataGridViewPreview.Columns.Count;
      for (int i = colCount - 1; i > startPosition; i--)
      {
        dataGridViewPreview.Columns.RemoveAt(i);
      }
    }
    #endregion
  }
}
