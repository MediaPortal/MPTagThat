using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TagLib;

namespace MPTagThat.Core
{
  public class TrackDataPreview
  {
    #region Variables
    private string _fullFileName;
    private string _fileName;
    private string _newFullFileName;
    private string _newFileName;
    private string _artist;
    private string _title;
    private string _album;
    private string _year;
    private string _track;
    private string _numtracks;
    private string _disc;
    private string _numdiscs;
    private string _genre;
    private string _albumartist;
    private string _comment;
    private string _conductor;
    private string _composer;
    private string _grouping;
    private string _subtitle;
    private string _interpreter;
    private string _bpm;
    #endregion

    #region ctor
    public TrackDataPreview(string fileName)
    {
      _fullFileName = fileName;
      _fileName = Path.GetFileName(fileName);
    }
    #endregion

    #region Properties
    #region Common Properties
    /// <summary>
    /// The Full Filename including the path
    /// </summary>
    public string FullFileName
    {
      get { return _fullFileName; }
    }

    /// <summary>
    /// Filename without Path
    /// </summary>
    public string FileName
    {
      get { return _fileName; }
      set { _fileName = value; }
    }

    /// <summary>
    /// The New Full Filename including the path
    /// </summary>
    public string NewFullFileName
    {
      get { return _newFullFileName; }
      set { _newFullFileName = value; }
    }

    /// <summary>
    /// New Filename without Path
    /// </summary>
    public string NewFileName
    {
      get { return _newFileName; }
      set { _newFileName = value; }
    }
    #endregion

    #region Tags
    /// <summary>
    /// Artist / Performer Tag
    /// ID3: TPE1
    /// </summary>
    public string Artist
    {
      get { return _artist; }
      set { _artist = value; }
    }
    /// <summary>
    /// Album Artist / Band  / Orchestra Tag
    /// ID3: TPE2
    /// </summary>
    public string AlbumArtist
    {
      get { return _albumartist; }
      set { _albumartist = value; }
    }

    /// <summary>
    /// ALbum Tag
    /// ID3: TALB
    /// </summary>
    public string Album
    {
      get { return _album; }
      set { _album = value; }
    }

    /// <summary>
    /// Beats Per Minute Tag
    /// ID3: TBPM
    /// </summary>
    public string BPM
    {
      get { return _bpm; }
      set { _bpm = value; }
    }

    /// <summary>
    /// Comment Tag
    /// ID3: COMM
    /// </summary>
    public string Comment
    {
      get { return _comment; }
      set { _comment = value; }
    }

    /// <summary>
    /// Composert Tag
    /// ID3: TCOM
    /// </summary>
    public string Composer
    {
      get { return _composer; }
      set { _composer = value; }
    }

    /// <summary>
    /// Conductor Tag
    /// ID3: TPE3
    /// </summary>
    public string Conductor
    {
      get { return _conductor; }
      set { _conductor = value; }
    }

    /// <summary>
    /// Position in Mediaset Tag
    /// ID3: TPOS
    /// </summary>
    public string Disc
    {
      get { return _disc; }
      set { _disc = value; }
    }

    public string NumDisc
    {
      get { return _numdiscs; }
      set { _numdiscs = value; }
    }


    /// <summary>
    /// Interpreted / Remixed / Modified by Tag
    /// ID3: TPE4
    /// </summary>
    public string Interpreter
    {
      get { return _interpreter; }
      set { _interpreter = value; }
    }

    /// <summary>
    /// Genre Tag
    /// ID3: TCON
    /// </summary>
    public string Genre
    {
      get { return _genre; }
      set { _genre = value; }
    }

    /// <summary>
    /// Content Group  Tag
    /// ID3: TIT1
    /// </summary>
    public string Grouping
    {
      get { return _grouping; }
      set { _grouping = value; }
    }

     /// <summary>
    /// SubTitle / More Detailed Description
    /// ID3: TIT3
    /// </summary>
    public string SubTitle
    {
      get { return _subtitle; }
      set { _subtitle = value; }
    }

    /// <summary>
    /// Title Tag
    /// ID3: TIT2
    /// </summary>
    public string Title
    {
      get { return _title; }
      set { _title = value; }
    }

    /// <summary>
    /// Track Tag
    /// ID3: TRCK
    /// </summary>
    public string Track
    {
      get { return _track; }
      set { _track = value; }
    }

    /// <summary>
    /// Track Tag
    /// ID3: TRCK
    /// </summary>
    public string NumTrack
    {
      get { return _numtracks; }
      set { _numtracks = value; }
    }

    /// <summary>
    /// Year Tag
    /// ID3: TYER
    /// </summary>
    public string Year
    {
      get { return _year; }
      set { _year = value; }
    }
    #endregion
    #endregion
  }
}
