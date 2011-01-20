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
using TagLib;

#endregion

namespace MPTagThat.TagEdit
{
  public class MultiTagEditOptions
  {
    private int _bpm = -1;
    private List<Comment> _comments = new List<Comment>();
    private int _disc = -1;
    private List<Lyric> _lyrics = new List<Lyric>();
    private int _numDiscs = -1;
    private int _numTracks = -1;
    private List<Picture> _pictures = new List<Picture>();
    private int _track = -1;
    private int _year = -1;

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

    public string Artist { get; set; }

    public string AlbumArtist { get; set; }

    public string Album { get; set; }

    public bool Compilation { get; set; }

    public string Title { get; set; }

    public int Year
    {
      get { return _year; }
      set { _year = value; }
    }

    public string Genre { get; set; }

    public List<Picture> Pictures
    {
      get { return _pictures; }
      set { _pictures = value; }
    }

    public bool RemoveExistingPictures { get; set; }

    public List<Comment> Comments
    {
      get { return _comments; }
      set { _comments = value; }
    }

    public bool RemoveExistingComments { get; set; }

    public bool SetTrackLength { get; set; }

    public string MediaType { get; set; }

    public string TitleSortName { get; set; }

    public string AlbumSortName { get; set; }

    public string ArtistSortName { get; set; }

    public string SubTitle { get; set; }

    public string ContentGroup { get; set; }

    public string Copyright { get; set; }

    public string EncodedBy { get; set; }

    public string Publisher { get; set; }

    public string TextWriter { get; set; }

    public string Interpreter { get; set; }

    public string Composer { get; set; }

    public string Conductor { get; set; }

    public string OriginalRelease { get; set; }

    public string OriginalOwner { get; set; }

    public string OriginalArtist { get; set; }

    public string OriginalLyricsWriter { get; set; }

    public string OriginalFileName { get; set; }

    public string OriginalAlbum { get; set; }

    public string CommercialInformation { get; set; }

    public string OfficialPublisherUrl { get; set; }

    public string OfficialPaymentUrl { get; set; }

    public string OfficialInternetRadioUrl { get; set; }

    public string OfficialAudioSourceUrl { get; set; }

    public string OfficialArtistUrl { get; set; }

    public string OfficialAudioFileUrl { get; set; }

    public string CopyrightInformation { get; set; }

    public string MusicCreditList { get; set; }

    public string InvolvedPeople { get; set; }

    public List<Lyric> Lyrics
    {
      get { return _lyrics; }
      set { _lyrics = value; }
    }

    public bool RemoveExistingLyrics { get; set; }

    public bool RemoveExistingRating { get; set; }

    public List<Rating> Rating { get; set; }
  }
}