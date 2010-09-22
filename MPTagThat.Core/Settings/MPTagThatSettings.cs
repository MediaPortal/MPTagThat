#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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
using System.Drawing;

#endregion

namespace MPTagThat.Core
{
  public class MPTagThatSettings
  {
    #region Variables

    private string _activeScript = "";
    private Size _formSize = new Size(1200, 1024);
    private string _lastFolderUsed = "";

    private string _lastRipEncoderUsed;
    private int _numTrackDigits = 2;

    private List<string> _recentFolders = new List<string>();

    #endregion

    #region Properties

    #region Layout

    [Setting(SettingScope.User, "")]
    public string LastFolderUsed
    {
      get { return _lastFolderUsed; }
      set { _lastFolderUsed = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ScanSubFolders { get; set; }

    [Setting(SettingScope.User, "0")]
    public int DataProvider { get; set; }

    [Setting(SettingScope.User, "")]
    public Point FormLocation { get; set; }

    [Setting(SettingScope.User, "")]
    public Size FormSize
    {
      get { return _formSize; }
      set { _formSize = value; }
    }

    [Setting(SettingScope.User, "-1")]
    public int LeftPanelSize { get; set; }

    [Setting(SettingScope.User, "-1")]
    public int RightPanelSize { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool RightPanelCollapsed { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool ErrorPanelCollapsed { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool PlayerPanelCollapsed { get; set; }

    #endregion

    [Setting(SettingScope.User, "")]
    public string ActiveScript
    {
      get { return _activeScript; }
      set { _activeScript = value; }
    }

    [Setting(SettingScope.User, "mp3")]
    public string LastConversionEncoderUsed { get; set; }

    [Setting(SettingScope.User, "0")]
    public int PlayerSpectrumIndex { get; set; }

    [Setting(SettingScope.User, "")]
    public string SingleEditLastUsedScript { get; set; }

    [Setting(SettingScope.User, "")]
    public List<string> RecentFolders
    {
      get { return _recentFolders; }
      set { _recentFolders = value; }
    }

    #region Tags

    [Setting(SettingScope.User, "2")]
    public int NumberTrackDigits
    {
      get { return _numTrackDigits; }
      set { _numTrackDigits = value; }
    }

    [Setting(SettingScope.User, "3")]
    public int ID3V2Version { get; set; }

    [Setting(SettingScope.User, "3")]
    public int ID3Version { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool RemoveID3V1 { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool RemoveID3V2 { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool CopyArtist { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool UseCaseConversion { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool CreateFolderThumb { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool EmbedFolderThumb { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool OverwriteExistingCovers { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool OverwriteExistingLyrics { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool UseMediaPortalDatabase { get; set; }

    [Setting(SettingScope.User, "")]
    public string MediaPortalDatabase { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool AutoFillNumberOfTracks { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool MP3Validate { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool MP3AutoFix { get; set; }

    [Setting(SettingScope.User, "com")]
    public string AmazonSite { get; set; }

    #endregion

    #region Lyrics

    [Setting(SettingScope.User, "true")]
    public bool SearchLyricWiki { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool SearchHotLyrics { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool SearchLyrics007 { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool SearchLyricsOnDemand { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool SearchLyricsPlugin { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool SearchActionext { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool SearchLyrDB { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool SearchLRCFinder { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool SwitchArtist { get; set; }

    #endregion

    #region General

    [Setting(SettingScope.User, "1")]
    public int Theme { get; set; }

    [Setting(SettingScope.User, "5")]
    public int DebugLevel { get; set; }

    #endregion

    #region Ripping

    [Setting(SettingScope.User, "")]
    public string RipTargetFolder { get; set; }

    [Setting(SettingScope.User, "mp3")]
    public string RipEncoder { get; set; }

    [Setting(SettingScope.User, "<A>\\<B>\\<K> - <T>")]
    public string RipFileNameFormat { get; set; }

    [Setting(SettingScope.User, "true")]
    public bool RipEjectCD { get; set; }

    [Setting(SettingScope.User, "false")]
    public bool RipActivateTargetFolder { get; set; }

    #region MP3

    [Setting(SettingScope.User, "0")]
    public int RipLamePreset { get; set; }

    [Setting(SettingScope.User, "0")]
    public int RipLameABRBitRate { get; set; }

    [Setting(SettingScope.User, "")]
    public string RipLameExpert { get; set; }

    #endregion

    #region Ogg

    [Setting(SettingScope.User, "3")]
    public int RipOggQuality { get; set; }

    [Setting(SettingScope.User, "")]
    public string RipOggExpert { get; set; }

    #endregion

    #region FLAC

    [Setting(SettingScope.User, "4")]
    public int RipFlacQuality { get; set; }

    [Setting(SettingScope.User, "")]
    public string RipFlacExpert { get; set; }

    #endregion

    #region AAC

    [Setting(SettingScope.User, "aac")]
    public string RipEncoderAAC { get; set; }

    [Setting(SettingScope.User, "128")]
    public string RipEncoderAACBitRate { get; set; }

    [Setting(SettingScope.User, "Stereo")]
    public string RipEncoderAACChannelMode { get; set; }

    #endregion

    #region WMA

    [Setting(SettingScope.User, "wma")]
    public string RipEncoderWMA { get; set; }

    [Setting(SettingScope.User, "16,2,44100")]
    public string RipEncoderWMASample { get; set; }

    [Setting(SettingScope.User, "50")]
    public string RipEncoderWMABitRate { get; set; }

    [Setting(SettingScope.User, "Vbr")]
    public string RipEncoderWMACbrVbr { get; set; }

    #endregion

    #region MPC

    [Setting(SettingScope.User, "standard")]
    public string RipEncoderMPCPreset { get; set; }

    [Setting(SettingScope.User, "")]
    public string RipEncoderMPCExpert { get; set; }

    #endregion

    #region WV

    [Setting(SettingScope.User, "-h")]
    public string RipEncoderWVPreset { get; set; }

    [Setting(SettingScope.User, "")]
    public string RipEncoderWVExpert { get; set; }

    #endregion

    #endregion

    #endregion
  }
}