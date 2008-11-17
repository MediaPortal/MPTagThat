using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.TagEdit
{
  public class MultiTagEditOptions
  {
    private int _track = -1;
    private int _numTracks = -1;
    private int _disc = -1;
    private int _numDiscs = -1;
    private int _bpm = -1;
    private string _artist = null;
    private string _albumArtist = null;
    private string _album = null;
    private string _title = null;
    private int _year = -1;
    private string _genre = null;
    private List<TagLib.Picture> _pictures = new List<TagLib.Picture>();
    private bool _removePictures = false;
    private List<Comment> _comments = new List<Comment>();
    private List<Lyric> _lyrics = new List<Lyric>();
    private bool _removeComments = false;
    private bool _removelyrics = false;
    private string _conductor;
    private string _composer;
    private string _interpreter;
    private string _textwriter;
    private string _publisher;
    private string _encodedby;
    private string _copyright;
    private string _contentgroup;
    private string _subtitle;
    private string _artistsortname;
    private string _albumsortname;
    private string _titlesortname;
    private string _mediatype;
    private bool _settracklength;
    private string _originalAlbum;
    private string _originalFileName;
    private string _originalLyricswriter;
    private string _originalArtist;
    private string _originalOwner;
    private string _originalRelease;
    private string _copyrightUrl;
    private string _officialAudioFileUrl;
    private string _officialArtistUrl;
    private string _officialAudioSourceUrl;
    private string _officialInternetRadioUrl;
    private string _officialPaymentUrl;
    private string _officialPublisherUrl;
    private string _commercialInformation;
    private string _involvedPeople;
    private string _musicCreditList;
    private List<Rating> _rating;
    private bool _removeRating;

    public int Track
    {
      get { return _track; }
      set { _track = value; }
    }

    public int NumTracks
    {
      get { return _numTracks; }
      set { _numTracks = value; }
    }

    public int Disc
    {
      get { return _disc; }
      set { _disc = value; }
    }

    public int NumDiscs
    {
      get { return _numDiscs; }
      set { _numDiscs = value; }
    }

    public int BPM
    {
      get { return _bpm; }
      set { _bpm = value; }
    }

    public string Artist
    {
      get { return _artist; }
      set { _artist = value; }
    }

    public string AlbumArtist
    {
      get { return _albumArtist; }
      set { _albumArtist = value; }
    }

    public string Album
    {
      get { return _album; }
      set { _album = value; }
    }

    public string Title
    {
      get { return _title; }
      set { _title = value; }
    }

    public int Year
    {
      get { return _year; }
      set { _year = value; }
    }

    public string Genre
    {
      get { return _genre; }
      set { _genre = value; }
    }

    public List<TagLib.Picture> Pictures
    {
      get { return _pictures; }
      set { _pictures = value; }
    }

    public bool RemoveExistingPictures
    {
      get { return _removePictures; }
      set { _removePictures = value; }
    }

    public List<Comment> Comments
    {
      get { return _comments; }
      set { _comments = value; }
    }

    public bool RemoveExistingComments
    {
      get { return _removeComments; }
      set { _removeComments = value; }
    }

    public bool SetTrackLength
    {
      get { return _settracklength; }
      set { _settracklength = value; }
    }
	
    public string MediaType
    {
      get { return _mediatype; }
      set { _mediatype = value; }
    }
	
    public string TitleSortName
    {
      get { return _titlesortname; }
      set { _titlesortname = value; }
    }
	
    public string AlbumSortName
    {
      get { return _albumsortname; }
      set { _albumsortname = value; }
    }
	
    public string ArtistSortName
    {
      get { return _artistsortname; }
      set { _artistsortname = value; }
    }
	
    public string SubTitle
    {
      get { return _subtitle; }
      set { _subtitle = value; }
    }
	
    public string ContentGroup
    {
      get { return _contentgroup; }
      set { _contentgroup = value; }
    }
	
    public string Copyright
    {
      get { return _copyright; }
      set { _copyright = value; }
    }
	
    public string EncodedBy
    {
      get { return _encodedby; }
      set { _encodedby = value; }
    }
	
    public string Publisher
    {
      get { return _publisher; }
      set { _publisher = value; }
    }
	
    public string TextWriter
    {
      get { return _textwriter; }
      set { _textwriter = value; }
    }
	
    public string Interpreter
    {
      get { return _interpreter; }
      set { _interpreter = value; }
    }
	
    public string Composer
    {
      get { return _composer; }
      set { _composer = value; }
    }
	
    public string Conductor
    {
      get { return _conductor; }
      set { _conductor = value; }
    }

    public string OriginalRelease
    {
      get { return _originalRelease; }
      set { _originalRelease = value; }
    }
	
    public string OriginalOwner
    {
      get { return _originalOwner; }
      set { _originalOwner = value; }
    }
	
    public string OriginalArtist
    {
      get { return _originalArtist; }
      set { _originalArtist = value; }
    }
	
    public string OriginalLyricsWriter
    {
      get { return _originalLyricswriter; }
      set { _originalLyricswriter = value; }
    }
	
    public string OriginalFileName
    {
      get { return _originalFileName; }
      set { _originalFileName = value; }
    }
	
    public string OriginalAlbum
    {
      get { return _originalAlbum; }
      set { _originalAlbum = value; }
    }

    public string CommercialInformation
    {
      get { return _commercialInformation; }
      set { _commercialInformation = value; }
    }
	
    public string OfficialPublisherUrl
    {
      get { return _officialPublisherUrl; }
      set { _officialPublisherUrl = value; }
    }
	
    public string OfficialPaymentUrl
    {
      get { return _officialPaymentUrl; }
      set { _officialPaymentUrl = value; }
    }
	
    public string OfficialInternetRadioUrl
    {
      get { return _officialInternetRadioUrl; }
      set { _officialInternetRadioUrl = value; }
    }
	
    public string OfficialAudioSourceUrl
    {
      get { return _officialAudioSourceUrl; }
      set { _officialAudioSourceUrl = value; }
    }
	
    public string OfficialArtistUrl
    {
      get { return _officialArtistUrl; }
      set { _officialArtistUrl = value; }
    }
	
    public string OfficialAudioFileUrl
    {
      get { return _officialAudioFileUrl; }
      set { _officialAudioFileUrl = value; }
    }
	
    public string CopyrightInformation
    {
      get { return _copyrightUrl; }
      set { _copyrightUrl = value; }
    }

    public string MusicCreditList
    {
      get { return _musicCreditList; }
      set { _musicCreditList = value; }
    }
	
    public string InvolvedPeople
    {
      get { return _involvedPeople; }
      set { _involvedPeople = value; }
    }

    public List<Lyric> Lyrics
    {
      get { return _lyrics; }
      set { _lyrics = value; }
    }

    public bool RemoveExistingLyrics
    {
      get { return _removelyrics; }
      set { _removelyrics = value; }
    }

    public bool RemoveExistingRating
    {
      get { return _removeRating; }
      set { _removeRating = value; }
    }
	
    public List<Rating> Rating
    {
      get { return _rating; }
      set { _rating = value; }
    }
	


  }
}
