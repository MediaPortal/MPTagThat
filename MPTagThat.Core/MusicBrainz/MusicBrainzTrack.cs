using System;
using System.Collections.Generic;

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzTrack
  {
    #region Constructors
    public MusicBrainzTrack()
    {
    }
    #endregion

    #region Private Fields
    private Guid id;
    private int number;
    private string title;
    private string artist;
    private string album;
    private Guid albumid;
    private int duration;
    #endregion

    #region Properties
    public Guid Id
    {
      get { return id; }
      set { id = value; }
    }

    public int Number
    {
      get { return number; }
      set { number = value; }
    }

    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    public string Artist
    {
      get { return artist; }
      set { artist = value; }
    }

    public string Album
    {
      get { return album; }
      set { album = value; }
    }

    public Guid AlbumID
    {
      get { return albumid; }
      set { albumid = value; }
    }

    public int Duration
    {
      get { return duration; }
      set { duration = value; }
    }
    #endregion
  }
}