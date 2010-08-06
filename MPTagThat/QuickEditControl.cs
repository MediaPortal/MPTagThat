using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;
using MPTagThat.Core.ShellLib;
using TagLib;

namespace MPTagThat
{
  public partial class QuickEditControl : UserControl
  {
    #region Variables

    private Main main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();

    private ShellAutoComplete _acArtist;
    private ShellAutoComplete _acAlbumArtist;

    private bool _multiSelect = false;
    #endregion

    #region ctor
    public QuickEditControl(Main main)
    {
      this.main = main;
      InitializeComponent();
    }
    #endregion

    #region FormLoad
    /// <summary>
    /// Do some init work, when control gets loaded
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QuickEditControl_Load(object sender, EventArgs e)
    {
      // Fill the Genre Combo Box
      cbGenre.Items.AddRange(TagLib.Genres.Audio);

      if (Options.MainSettings.UseMediaPortalDatabase && Options.MediaPortalArtists != null)
      {
        // Add Auto Complete Option for Artist
        _acArtist = new ShellAutoComplete();

        _acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoSuggest;
        _acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoAppend;
        _acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UpDownKeyDropsList;
        _acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.FilterPreFixes;
        _acArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UseTab;

        SourceCustomList custom = new SourceCustomList();
        custom.StringList = Options.MediaPortalArtists;
        _acArtist.ListSource = custom;

        // We need to get the Combobox Handle first
        ShellApi.ComboBoxInfo cbInfoArtist = new ShellApi.ComboBoxInfo();
        cbInfoArtist.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(cbInfoArtist);
        ShellApi.GetComboBoxInfo(cbArtist.Handle, ref cbInfoArtist);
        _acArtist.EditHandle = cbInfoArtist.hwndEdit;
        _acArtist.SetAutoComplete(true);

        // Add Auto Complete Option for AlbumArtist
        _acAlbumArtist = new ShellAutoComplete();

        _acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoSuggest;
        _acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.AutoAppend;
        _acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UpDownKeyDropsList;
        _acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.FilterPreFixes;
        _acAlbumArtist.ACOptions |= ShellAutoComplete.AutoCompleteOptions.UseTab;
        _acAlbumArtist.ListSource = custom;

        // We need to get the Combobox Handle first
        ShellApi.ComboBoxInfo cbInfoAlbumArtist = new ShellApi.ComboBoxInfo();
        cbInfoAlbumArtist.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(cbInfoAlbumArtist);
        ShellApi.GetComboBoxInfo(cbAlbumArtist.Handle, ref cbInfoAlbumArtist);
        _acAlbumArtist.EditHandle = cbInfoAlbumArtist.hwndEdit;
        _acAlbumArtist.SetAutoComplete(true);
      }
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Whenever a track is selected in the Gridview, fill the Quickedit panel with data
    /// </summary>
    /// <param name="track"></param>
    public void FillForm(TrackData track)
    {
      Util.EnterMethod(Util.GetCallingMethod());

      if (main.TracksGridView.View.SelectedRows.Count > 1)
      {
        _multiSelect = true;
      }
      else
      {
        _multiSelect = false;
      }

      cbArtist.Text = track.Artist;
      cbAlbumArtist.Text = track.AlbumArtist;
      tbAlbum.Text = track.Album;
      checkBoxCompilation.Checked = track.Compilation;

      // Disable Title, when multiple values are selected
      if (_multiSelect)
      {
        tbTitle.Text = "";
        tbTitle.Enabled = false;
      }
      else
      {
        tbTitle.Text = track.Title;
        tbTitle.Enabled = true;
      }
      
      cbGenre.Text = track.Genre;
      tbYear.Text = track.Year.ToString();

      string[] disc = track.Disc.Split('/');
      tbDisc.Text = disc[0];
      if (disc.Length > 1)
      {
        tbNumDiscs.Text = disc[1];
      }
      else
      {
        tbNumDiscs.Text = "";
      }

      string[] tracks = track.Track.Split('/');

      // Disable Track number on Multi Select
      if (_multiSelect)
      {
        tbTrack.Text = "";
        tbTrack.Enabled = false;
      }
      else
      {
        tbTrack.Text = tracks[0];
        tbTrack.Enabled = true;
      }
      
      if (tracks.Length > 1)
      {
        tbNumTracks.Text = tracks[1];
      }
      else
      {
        tbNumTracks.Text = "";
      }
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    public void ClearForm()
    {
      cbArtist.Text = "";
      cbAlbumArtist.Text = "";
      tbAlbum.Text = "";
      checkBoxCompilation.Checked = false;
      tbTitle.Text = "";
      cbGenre.Text = "";
      tbYear.Text = "";
      tbTrack.Text = "";
      tbNumTracks.Text = "";
      tbDisc.Text = "";
      tbNumTracks.Text = "";
    }
    #endregion

    #region Events
    /// <summary>
    /// The Apply button has been clicked.
    /// Apply all changes to the selected row
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btApply_Click(object sender, EventArgs e)
    {
      Util.EnterMethod(Util.GetCallingMethod());
      bool bErrors = false;
      DataGridView tracksGrid = main.TracksGridView.View;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        try
        {
          bool trackChanged = false;
          TrackData track = main.TracksGridView.TrackList[row.Index];

          if (cbArtist.Text != track.Artist)
          {
            track.Artist = cbArtist.Text;
            trackChanged = true;
          }

          if (cbAlbumArtist.Text != track.AlbumArtist)
          {
            track.AlbumArtist = cbAlbumArtist.Text;
            trackChanged = true;
          }

          if (tbAlbum.Text != track.Album)
          {
            track.Album = tbAlbum.Text;
            trackChanged = true;
          }

          if (track.Compilation != checkBoxCompilation.Checked)
          {
            track.Compilation = checkBoxCompilation.Checked;
            trackChanged = true;
          }

          if (!_multiSelect && tbTitle.Text.Trim() != track.Title)
          {
            track.Title = tbTitle.Text;
            trackChanged = true;
          }

          if (cbGenre.Text != track.Genre)
          {
            track.Genre = cbGenre.Text;
            trackChanged = true;
          }

          int parsedYear = 0;
          try
          {
            parsedYear = Int32.Parse(tbYear.Text.Trim());
          }
          catch (Exception)
          {
            parsedYear = 0;
          }
          if (parsedYear != track.Year)
          {
            track.Year = parsedYear;
            trackChanged = true;
          }

          int tracknumber = 0;
          int numbertracks = 0;
          int oritracknumber = 0;
          int orinumbertracks = 0;
          string[] tracks = track.Track.Split('/');

          try
          {
            tracknumber = Int32.Parse(tbTrack.Text.Trim());
          }
          catch (Exception)
          { }
          try
          {
            numbertracks = Int32.Parse(tbNumTracks.Text.Trim());
          }
          catch (Exception)
          { }

          try
          {
            oritracknumber = Int32.Parse(tracks[0]);
          }
          catch (Exception)
          { }
          try
          {
            orinumbertracks = Int32.Parse(tracks[1]);
          }
          catch (Exception)
          { }
          string trackNumber = string.Format("{0}/{1}", tracknumber, numbertracks);
          
          if (!_multiSelect && tracknumber != oritracknumber)
          {
            track.Track = trackNumber;
            trackChanged = true;
          }

          if (tracks.Length > 1 || numbertracks > 0)
          {
            if (numbertracks != orinumbertracks)
            {
              // We need to reformat the tracknumber in case of multiselect
              if (_multiSelect)
              {
                trackNumber = string.Format("{0}/{1}", tracks[0], numbertracks);
              }
              track.Track = trackNumber;
              trackChanged = true;
            } 
          }

          int discnumber = 0;
          int numberdiscs = 0;
          int oridiscnumber = 0;
          int orinumberdiscs = 0;
          try
          {
            discnumber = Int32.Parse(tbDisc.Text.Trim());
          }
          catch (Exception)
          { }
          try
          {
            numberdiscs = Int32.Parse(tbNumDiscs.Text.Trim());
          }
          catch (Exception)
          { }

          string[] discs = track.Disc.Split('/');
          try
          {
            oridiscnumber = Int32.Parse(discs[0]);
          }
          catch (Exception)
          { }
          try
          {
            orinumberdiscs = Int32.Parse(discs[1]);
          }
          catch (Exception)
          { }
          string discNumber = string.Format("{0}/{1}", discnumber, numberdiscs);

          if (discnumber != oridiscnumber)
          {
            track.Disc = discNumber;
            trackChanged = true;
          }

          if (discs.Length > 1 || numberdiscs > 0)
          {
            if (numberdiscs != orinumberdiscs)
            {
              track.Disc = discNumber;
              trackChanged = true;
            }
          }

          if (trackChanged)
          {
            main.TracksGridView.Changed = true;
            main.TracksGridView.SetBackgroundColorChanged(row.Index);
            track.Changed = true;
          }
        }
        catch (Exception ex)
        {
          log.Error("Error applying changes from MultiTagedit: {0} stack: {1}", ex.Message, ex.StackTrace);
          row.Cells[0].Value = localisation.ToString("message", "Error");
          main.TracksGridView.AddErrorMessage(main.TracksGridView.TrackList[row.Index].File.Name, ex.Message);
          bErrors = true;
        }
      }

      main.TracksGridView.Changed = bErrors;
      // check, if we still have changed items in the list
      foreach (TrackData track in main.TracksGridView.TrackList)
      {
        if (track.Changed)
          main.TracksGridView.Changed = true;
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
      Util.LeaveMethod(Util.GetCallingMethod());
    }

    #endregion
  }
}
