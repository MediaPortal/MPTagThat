using System;
using System.Collections.Generic;
using System.Text;
using MPTagThat.Core.Amazon;

namespace MPTagThat.Core.MusicBrainz
{
  public class MusicBrainzAlbum
  {
    #region Private Fields
    private Guid id;
    private string asin;
    private string title;
    private string artist;
    private string year;
    private AmazonAlbum amazonAlbum;
    private List<MusicBrainzTrack> tracks;
    #endregion

    #region ctor
    public MusicBrainzAlbum()
    {
      tracks = new List<MusicBrainzTrack>();
    }
    #endregion

    #region Properties
    public Guid Id
    {
      get { return id; }
      set { id = value; }
    }

    public string Asin
    {
      get { return asin; }
      set { asin = value; }
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

    public string Year
    {
      get { return year; }
      set { year = value; }
    }

    public List<MusicBrainzTrack> Tracks
    {
      get { return tracks; }
      set { tracks = value; }
    }

    public AmazonAlbum Amazon
    {
      get { return amazonAlbum; }
      set { amazonAlbum = value; }
    }
    #endregion
  }
}
