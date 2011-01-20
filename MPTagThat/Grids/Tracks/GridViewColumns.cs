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

using System.Collections.Generic;
using System.Windows.Forms;
using MPTagThat.Core;

#endregion

namespace MPTagThat.GridView
{
  public class GridViewColumns
  {
    #region Variables

    private readonly GridViewColumn _OriginalAlbum;
    private readonly GridViewColumn _album;
    private readonly GridViewColumn _albumSortName;
    private readonly GridViewColumn _albumartist;
    private readonly GridViewColumn _artist;
    private readonly GridViewColumn _artistSortName;
    private readonly GridViewColumn _bitrate;
    private readonly GridViewColumn _bpm;
    private readonly GridViewColumn _channels;
    private readonly GridViewColumn _comment;
    private readonly GridViewColumn _commercialInformation;
    private readonly GridViewColumn _composer;
    private readonly GridViewColumn _conductor;
    private readonly GridViewColumn _copyright;
    private readonly GridViewColumn _copyrightInformation;
    private readonly GridViewColumn _creationtime;
    private readonly GridViewColumn _disc;
    private readonly GridViewColumn _duration;
    private readonly GridViewColumn _encodedBy;
    private readonly GridViewColumn _filename;
    private readonly GridViewColumn _filepath;
    private readonly GridViewColumn _filesize;
    private readonly GridViewColumn _genre;
    private readonly GridViewColumn _grouping;
    private readonly GridViewColumn _interpreter;
    private readonly GridViewColumn _lastwritetime;
    private readonly GridViewColumn _lyrics;
    private readonly GridViewColumn _mediaType;
    private readonly GridViewColumn _numpics;
    private readonly GridViewColumn _officialArtistInformation;
    private readonly GridViewColumn _officialAudioFileInformation;
    private readonly GridViewColumn _officialAudioSourceInformation;
    private readonly GridViewColumn _officialInternetRadioInformation;
    private readonly GridViewColumn _officialPaymentInformation;
    private readonly GridViewColumn _officialPublisherInformation;
    private readonly GridViewColumn _originalArtist;
    private readonly GridViewColumn _originalFileName;
    private readonly GridViewColumn _originalLyricsWriter;
    private readonly GridViewColumn _originalOwner;
    private readonly GridViewColumn _originalRelease;
    private readonly GridViewColumn _publisher;
    private readonly GridViewColumn _rating;
    private readonly GridViewColumn _samplerate;
    private readonly GridViewColumn _status;
    private readonly GridViewColumn _subTitle;
    private readonly GridViewColumn _tagtype;
    private readonly GridViewColumn _textWriter;
    private readonly GridViewColumn _title;
    private readonly GridViewColumn _titleSortName;
    private readonly GridViewColumn _track;
    private readonly GridViewColumn _version;
    private readonly GridViewColumn _year;
    private GridViewSettings _settings;

    #endregion

    #region Constructor

    public GridViewColumns()
    {
      _status = new GridViewColumn("Status", "text", 45, true, true, false, true);
      _filename = new GridViewColumn("FileName", "text", 200, true, false, true, true);
      _filepath = new GridViewColumn("FilePath", "text", 200, false, true, true, true); // Initially hidden
      _track = new GridViewColumn("Track", "text", 40, true, false, true, false);
      _artist = new GridViewColumn("Artist", "text", 150, true, false, true, false);
      _albumartist = new GridViewColumn("AlbumArtist", "text", 150, true, false, true, false);
      _album = new GridViewColumn("Album", "text", 150, true, false, true, false);
      _title = new GridViewColumn("Title", "text", 250, true, false, true, false);
      _year = new GridViewColumn("Year", "number", 40, true, false, true, false);
      _genre = new GridViewColumn("Genre", "text", 100, true, false, true, false);
      _creationtime = new GridViewColumn("CreationTime", "text", 100, true, true, true, false);
      _lastwritetime = new GridViewColumn("LastWriteTime", "text", 100, true, true, true, false);
      _tagtype = new GridViewColumn("TagType", "text", 100, true, true, true, false);
      _disc = new GridViewColumn("Disc", "^text", 45, true, false, true, false);
      _bpm = new GridViewColumn("BPM", "number", 40, true, false, true, false);
      _rating = new GridViewColumn("Rating", "rating", 90, true, false, true, false);
      _comment = new GridViewColumn("Comment", "text", 200, true, false, true, false);
      _composer = new GridViewColumn("Composer", "text", 150, true, false, true, false);
      _conductor = new GridViewColumn("Conductor", "text", 150, true, false, true, false);
      _numpics = new GridViewColumn("NumPics", "number", 40, true, false, true, false);
      _duration = new GridViewColumn("Duration", "text", 100, true, true, true, false);
      _filesize = new GridViewColumn("FileSize", "text", 80, true, true, true, false);
      _bitrate = new GridViewColumn("BitRate", "text", 50, true, true, true, false);
      _samplerate = new GridViewColumn("SampleRate", "text", 70, true, true, true, false);
      _channels = new GridViewColumn("Channels", "text", 40, true, true, true, false);
      _version = new GridViewColumn("Version", "text", 100, true, true, true, false);

      // Initially Hidden Columns
      _artistSortName = new GridViewColumn("ArtistSortName", "text", 100, false, false, true, false);
      _albumSortName = new GridViewColumn("AlbumSortName", "text", 100, false, false, true, false);
      _commercialInformation = new GridViewColumn("CommercialInformation", "text", 100, false, false, true, false);
      _copyright = new GridViewColumn("Copyright", "text", 100, false, false, true, false);
      _copyrightInformation = new GridViewColumn("CopyrightInformation", "text", 100, false, false, true, false);
      _encodedBy = new GridViewColumn("EncodedBy", "text", 100, false, false, true, false);
      _interpreter = new GridViewColumn("Interpreter", "text", 100, false, false, true, false);
      _grouping = new GridViewColumn("Grouping", "text", 100, false, false, true, false);
      _lyrics = new GridViewColumn("Lyrics", "text", 100, false, false, true, false);
      _mediaType = new GridViewColumn("MediaType", "text", 100, false, false, true, false);
      _officialAudioFileInformation = new GridViewColumn("OfficialAudioFileInformation", "text", 100, false, false, true,
                                                         false);
      _officialArtistInformation = new GridViewColumn("OfficialArtistInformation", "text", 100, false, false, true,
                                                      false);
      _officialAudioSourceInformation = new GridViewColumn("OfficialAudioSourceInformation", "text", 100, false, false,
                                                           true, false);
      _officialInternetRadioInformation = new GridViewColumn("OfficialInternetRadioInformation", "text", 100, false,
                                                             false, true, false);
      _officialPaymentInformation = new GridViewColumn("OfficialPaymentInformation", "text", 100, false, false, true,
                                                       false);
      _officialPublisherInformation = new GridViewColumn("OfficialPublisherInformation", "text", 100, false, false, true,
                                                         false);
      _OriginalAlbum = new GridViewColumn("OriginalAlbum", "text", 100, false, false, true, false);
      _originalFileName = new GridViewColumn("OriginalFileName", "text", 100, false, false, true, false);
      _originalLyricsWriter = new GridViewColumn("OriginalLyricsWriter", "text", 100, false, false, true, false);
      _originalArtist = new GridViewColumn("OriginalArtist", "text", 100, false, false, true, false);
      _originalOwner = new GridViewColumn("OriginalOwner", "text", 100, false, false, true, false);
      _originalRelease = new GridViewColumn("OriginalRelease", "text", 100, false, false, true, false);
      _publisher = new GridViewColumn("Publisher", "text", 100, false, false, true, false);
      _subTitle = new GridViewColumn("SubTitle", "text", 100, false, false, true, false);
      _textWriter = new GridViewColumn("TextWriter", "text", 100, false, false, true, false);
      _titleSortName = new GridViewColumn("TitleSortName", "text", 100, false, false, true, false);

      LoadSettings();
    }

    #endregion

    #region Properties

    public GridViewSettings Settings
    {
      get { return _settings; }
    }

    #endregion

    #region Private Methods

    private void LoadSettings()
    {
      _settings = new GridViewSettings();
      _settings.Name = "Tracks";
      ServiceScope.Get<ISettingsManager>().Load(_settings);
      if (_settings.Columns.Count == 0)
      {
        // Setup the Default Columns to display on first use of the program
        List<GridViewColumn> columnList = new List<GridViewColumn>();
        columnList = SetDefaultColumns();
        _settings.Columns.Clear();
        foreach (GridViewColumn column in columnList)
        {
          _settings.Columns.Add(column);
        }
        ServiceScope.Get<ISettingsManager>().Save(_settings);
      }
      else
      {
        // Add / Reorder Columns that have been added after Release, so that the settings don't need to be deleted

        // Reorder the Status field
        if (_settings.Columns[0].Name != "Status")
        {
          // We still have an old setting with Status at position 1
          _settings.Columns.RemoveAt(1);
          _settings.Columns.Insert(0, _status);
        }

        // FilePath should be column index #2
        if (_settings.Columns[2].Name != "FilePath")
        {
          _settings.Columns.Insert(2, _filepath);
        }
      }
    }

    public void SaveSettings()
    {
      _settings.Name = "Tracks";
      ServiceScope.Get<ISettingsManager>().Save(_settings);
    }

    public void SaveColumnSettings(DataGridViewColumn column, int colIndex)
    {
      _settings.Columns[colIndex].Width = column.Width;
      _settings.Columns[colIndex].DisplayIndex = column.DisplayIndex;
      _settings.Columns[colIndex].Display = column.Visible;
    }


    private List<GridViewColumn> SetDefaultColumns()
    {
      List<GridViewColumn> columnList = new List<GridViewColumn>();
      columnList.Add(_status);
      columnList.Add(_filename);
      columnList.Add(_filepath);
      columnList.Add(_track);
      columnList.Add(_artist);
      columnList.Add(_albumartist);
      columnList.Add(_album);
      columnList.Add(_title);
      columnList.Add(_year);
      columnList.Add(_genre);
      columnList.Add(_disc);
      columnList.Add(_rating);
      columnList.Add(_bpm);
      columnList.Add(_comment);
      columnList.Add(_composer);
      columnList.Add(_conductor);
      columnList.Add(_numpics);
      // Initially hidden columns
      columnList.Add(_artistSortName);
      columnList.Add(_albumSortName);
      columnList.Add(_commercialInformation);
      columnList.Add(_copyright);
      columnList.Add(_copyrightInformation);
      columnList.Add(_encodedBy);
      columnList.Add(_interpreter);
      columnList.Add(_grouping);
      columnList.Add(_lyrics);
      columnList.Add(_mediaType);
      columnList.Add(_officialAudioFileInformation);
      columnList.Add(_officialArtistInformation);
      columnList.Add(_officialAudioSourceInformation);
      columnList.Add(_officialInternetRadioInformation);
      columnList.Add(_officialPaymentInformation);
      columnList.Add(_officialPublisherInformation);
      columnList.Add(_OriginalAlbum);
      columnList.Add(_originalFileName);
      columnList.Add(_originalLyricsWriter);
      columnList.Add(_originalArtist);
      columnList.Add(_originalOwner);
      columnList.Add(_originalRelease);
      columnList.Add(_publisher);
      columnList.Add(_subTitle);
      columnList.Add(_textWriter);
      columnList.Add(_titleSortName);
      // end of initially hidden columns
      columnList.Add(_tagtype);
      columnList.Add(_duration);
      columnList.Add(_filesize);
      columnList.Add(_bitrate);
      columnList.Add(_samplerate);
      columnList.Add(_channels);
      columnList.Add(_version);
      columnList.Add(_creationtime);
      columnList.Add(_lastwritetime);

      return columnList;
    }

    #endregion
  }
}