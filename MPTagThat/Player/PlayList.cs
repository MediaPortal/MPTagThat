using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.Player
{
  public partial class PlayList : Form
  {
    #region Variables
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private PlayerControl _player;
    #endregion

    #region Properties
    public DataGridView PlayListGrid
    {
      get { return playListGrid; }
    }
    #endregion

    #region ctor
    public PlayList(PlayerControl player)
    {
      _player = player;

      InitializeComponent();

      IMessageQueue queueMessage = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      queueMessage.OnMessageReceive += new MessageReceivedHandler(OnMessageReceive);
    }
    #endregion

    #region Form Load
    private void PlayList_Load(object sender, EventArgs e)
    {
      // Setup Grid
      playListGrid.AutoGenerateColumns = false;
      playListGrid.DataSource = _player.PlayList;

      DataGridViewColumn col = new DataGridViewTextBoxColumn();
      col.Name = "Title";
      col.DataPropertyName = "Title";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 140;
      playListGrid.Columns.Add(col);

      col = new DataGridViewTextBoxColumn();
      col.Name = "Duration";
      col.DataPropertyName = "Duration";
      col.ReadOnly = true;
      col.Visible = true;
      col.Width = 70;
      playListGrid.Columns.Add(col);

      Localisation();
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Create Context Menu
    /// </summary>
    private void Localisation()
    {
      contextMenu.Items[0].Text = localisation.ToString("player", "ClearPlayList");
      contextMenu.Items[2].Text = localisation.ToString("player", "LoadPlayList");
      contextMenu.Items[3].Text = localisation.ToString("player", "SavePlayList");
    }

    #endregion

    #region Event Handler
    /// <summary>
    /// Mouse Click in Datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void playListGrid_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        contextMenu.Show(playListGrid, new Point(e.X, e.Y));
    }

    /// <summary>
    /// Double click on a playlist entry. Start Playback for the item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void playListGrid_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      int index = playListGrid.HitTest(e.X, e.Y).RowIndex;
      _player.Play(index);
    }

    #region Context Menu
    /// <summary>
    /// Clear the Playlist
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void playListGrid_ClearPlayList(object o, System.EventArgs e)
    {
      _player.PlayList.Clear();
      _player.Stop();
    }
    #endregion

    #region Buttons
    private void btPlaylistLoad_Click(object sender, EventArgs e)
    {
      OpenFileDialog oFD = new OpenFileDialog();
      oFD.Filter = "M3U Format (*.m3u)|*.m3u|Winamp Playlist (*.pls)|*.pls";
      if (oFD.ShowDialog() == DialogResult.OK)
      {
        IPlayListIO loader = PlayListFactory.CreateIO(oFD.FileName);
        loader.Load(_player.PlayList, oFD.FileName);
      }
    }

    private void btPlayListSave_Click(object sender, EventArgs e)
    {
      if (_player.PlayList.Count == 0)
      {
        return;
      }

      SaveFileDialog sFD = new SaveFileDialog();
      sFD.Filter = "M3U Format (*.m3u)|*.m3u|Winamp Playlist (*.pls)|*.pls";
      if (sFD.ShowDialog() == DialogResult.OK)
      {
        IPlayListIO saver = PlayListFactory.CreateIO(sFD.FileName);
        saver.Save(_player.PlayList, sFD.FileName);
      }
    }
    #endregion

    #region Drag & Drop
    /// <summary>
    /// Tracks are Dropped on the Playlist Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void playListGrid_DragDrop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(typeof(List<TrackData>)))
      {
        return;
      }

      List<TrackData> selectedRows = (List<TrackData>)e.Data.GetData(typeof(List<TrackData>));
      foreach (TrackData track in selectedRows)
      {
        PlayListData playListItem = new PlayListData();
        playListItem.FileName = track.FullFileName;
        playListItem.Artist = track.Artist;
        playListItem.Album = track.Album;
        playListItem.Title = track.Title;
        playListItem.Duration = track.Duration.Substring(3, 5);  // Just get Minutes and seconds

        _player.PlayList.Add(playListItem);
      }
    }

    /// <summary>
    /// Tracks are dragged over the Playlist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void playListGrid_DragOver(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Copy;
    }
    #endregion

    /// <summary>
    /// Handle Key input on the Grid
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      switch (keyData)
      {
        case Keys.Delete:
          for (int i = playListGrid.Rows.Count - 1; i > -1; i--)
          {
            if (!playListGrid.Rows[i].Selected)
              continue;

            _player.PlayList.RemoveAt(i);
          }
          return true;
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }
    #endregion

    #region Message Handling
    /// <summary>
    /// Handle Messages
    /// </summary>
    /// <param name="message"></param>
    private void OnMessageReceive(QueueMessage message)
    {
      string action = message.MessageData["action"] as string;

      switch (action.ToLower())
      {
        // Message sent, when a Theme is changing
        case "themechanged":
          {
            this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            this.playListGrid.BackgroundColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            this.playListGrid.DefaultCellStyle.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            this.playListGrid.DefaultCellStyle.ForeColor = ServiceScope.Get<IThemeManager>().CurrentTheme.LabelForeColor;
            this.playListGrid.DefaultCellStyle.SelectionForeColor = Color.OrangeRed;
            this.playListGrid.DefaultCellStyle.SelectionBackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            this.playListGrid.GridColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
            break;
          }

        case "languagechanged":
          {
            Localisation();
            break;
          }
      }
    }
    #endregion
  }
}
