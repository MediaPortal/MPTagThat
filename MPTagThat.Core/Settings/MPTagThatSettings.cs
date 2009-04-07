using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace MPTagThat.Core
{
  public class MPTagThatSettings
  {
    #region Variables
    private int _theme;
    private int _debugLevel;
    private string _lastFolderUsed = "";
    private bool _scanSubFolders = false;
    private bool _databaseMode = false;
    private Point _formLocation;
    private Size _formSize = new Size(1200, 1024);
    private int _leftPanelSize;
    private int _rightPanelSize;
    private bool _rightPanelCollapsed;
    private bool _errorPanelCollapsed;
    private string _activeScript = "";
    private int _numTrackDigits = 2;
    private string _ripTargetFolder;
    private string _ripEncoder;
    private string _ripEncoderAAC;
    private string _ripEncoderAACBitRate;
    private string _ripEncoderAACChannelMode;
    private string _ripEncoderWMA;
    private string _ripEncoderWMABitRate;
    private string _ripEncoderWMASample;
    private string _ripEncoderWMACbrVbr;
    private string _ripFileNameFormat;
    private string _ripEncoderMPCPreset;
    private string _ripEncoderMPCExpert;
    private string _ripEncoderWVPreset;
    private string _ripEncoderWVExpert;

    private string _lastConversionEncoderUsed;
    private string _lastRipEncoderUsed;

    private int _lamePreset;
    private int _lameABRBitrate;
    private string _lameExpert;

    private int _oggQuality;
    private string _oggExpert;

    private int _flacQuality;
    private string _flacExpert;

    private int _tagsID3V2Version;
    private int _tagsID3Version;
    private bool _tagsRemoveID3V1;
    private bool _tagsRemoveID3V2;

    private bool _tagsCopyArtist;
    private bool _tagsUseCaseConversion;
    private bool _tagsCreateFolderThumb;
    private bool _tagsEmbedFolderThumb;
    private bool _tagsOverwriteExitingCovers;
    private bool _tagsOverwriteExitingLyrics;
    private bool _tagsUseMediaPortalDatabase;
    private bool _tagsAutoFillNumberOfTracks;
    private string _tagsMediaPortalDatabase;
    private int _playerSpectrumIndex;
    private bool _mp3Validate;
    private bool _mp3AutoFix;
    private string _amazonSite;
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
    public bool ScanSubFolders
    {
      get { return _scanSubFolders; }
      set { _scanSubFolders = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool DatabaseMode
    {
      get { return _databaseMode; }
      set { _databaseMode = value; }
    }

    [Setting(SettingScope.User, "")]
    public Point FormLocation
    {
      get { return _formLocation; }
      set { _formLocation = value; }
    }

    [Setting(SettingScope.User, "")]
    public Size FormSize
    {
      get { return _formSize; }
      set { _formSize = value; }
    }

    [Setting(SettingScope.User, "-1")]
    public int LeftPanelSize
    {
      get { return _leftPanelSize; }
      set { _leftPanelSize = value; }
    }

    [Setting(SettingScope.User, "-1")]
    public int RightPanelSize
    {
      get { return _rightPanelSize; }
      set { _rightPanelSize = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool RightPanelCollapsed
    {
      get { return _rightPanelCollapsed; }
      set { _rightPanelCollapsed = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool ErrorPanelCollapsed
    {
      get { return _errorPanelCollapsed; }
      set { _errorPanelCollapsed = value; }
    }
    #endregion

    [Setting(SettingScope.User, "")]
    public string ActiveScript
    {
      get { return _activeScript; }
      set { _activeScript = value; }
    }

    [Setting(SettingScope.User, "mp3")]
    public string LastConversionEncoderUsed
    {
      get { return _lastConversionEncoderUsed; }
      set { _lastConversionEncoderUsed = value; }
    }

    [Setting(SettingScope.User, "0")]
    public int PlayerSpectrumIndex
    {
      get { return _playerSpectrumIndex; }
      set { _playerSpectrumIndex = value; }
    }

    #region Tags
    [Setting(SettingScope.User, "2")]
    public int NumberTrackDigits
    {
      get { return _numTrackDigits; }
      set { _numTrackDigits = value; }
    }

    [Setting(SettingScope.User, "3")]
    public int ID3V2Version
    {
      get { return _tagsID3V2Version; }
      set { _tagsID3V2Version = value; }
    }

    [Setting(SettingScope.User, "3")]
    public int ID3Version
    {
      get { return _tagsID3Version; }
      set { _tagsID3Version = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool RemoveID3V1
    {
      get { return _tagsRemoveID3V1; }
      set { _tagsRemoveID3V1 = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool RemoveID3V2
    {
      get { return _tagsRemoveID3V2; }
      set { _tagsRemoveID3V2 = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool CopyArtist
    {
      get { return _tagsCopyArtist; }
      set { _tagsCopyArtist = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool UseCaseConversion
    {
      get { return _tagsUseCaseConversion; }
      set { _tagsUseCaseConversion = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool CreateFolderThumb
    {
      get { return _tagsCreateFolderThumb; }
      set { _tagsCreateFolderThumb = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool EmbedFolderThumb
    {
      get { return _tagsEmbedFolderThumb; }
      set { _tagsEmbedFolderThumb = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool OverwriteExistingCovers
    {
      get { return _tagsOverwriteExitingCovers; }
      set { _tagsOverwriteExitingCovers = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool OverwriteExistingLyrics
    {
      get { return _tagsOverwriteExitingLyrics; }
      set { _tagsOverwriteExitingLyrics = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool UseMediaPortalDatabase
    {
      get { return _tagsUseMediaPortalDatabase; }
      set { _tagsUseMediaPortalDatabase = value; }
    }

    [Setting(SettingScope.User, "")]
    public string MediaPortalDatabase
    {
      get { return _tagsMediaPortalDatabase; }
      set { _tagsMediaPortalDatabase = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool AutoFillNumberOfTracks
    {
      get { return _tagsAutoFillNumberOfTracks; }
      set { _tagsAutoFillNumberOfTracks = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool MP3Validate
    {
      get { return _mp3Validate; }
      set { _mp3Validate = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool MP3AutoFix
    {
      get { return _mp3AutoFix; }
      set { _mp3AutoFix = value; }
    }

    [Setting(SettingScope.User, "com")]
    public string AmazonSite
    {
      get { return _amazonSite; }
      set { _amazonSite = value; }
    }
    #endregion

    #region Lyrics
    private bool _switchArtist;
    private bool _searchLyricWiki;
    private bool _searchHotLyrics;
    private bool _searchLyrics007;
    private bool _searchLyricsOnDemand;
    private bool _searchSeekLyrics;
    private bool _searchActionext;
    private bool _searchLyrDB;

    [Setting(SettingScope.User, "true")]
    public bool SearchLyricWiki
    {
      get { return _searchLyricWiki; }
      set { _searchLyricWiki = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool SearchHotLyrics
    {
      get { return _searchHotLyrics; }
      set { _searchHotLyrics = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool SearchLyrics007
    {
      get { return _searchLyrics007; }
      set { _searchLyrics007 = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool SearchLyricsOnDemand
    {
      get { return _searchLyricsOnDemand; }
      set { _searchLyricsOnDemand = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool SearchSeekLyrics
    {
      get { return _searchSeekLyrics; }
      set { _searchSeekLyrics = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool SearchActionext
    {
      get { return _searchActionext; }
      set { _searchActionext = value; }
    }

    [Setting(SettingScope.User, "true")]
    public bool SearchLyrDB
    {
      get { return _searchLyrDB; }
      set { _searchLyrDB = value; }
    }

    [Setting(SettingScope.User, "false")]
    public bool SwitchArtist
    {
      get { return _switchArtist; }
      set { _switchArtist = value; }
    }
    #endregion

    #region General
    [Setting(SettingScope.User, "1")]
    public int Theme
    {
      get { return _theme; }
      set { _theme = value; }
    }

    [Setting(SettingScope.User, "5")]
    public int DebugLevel
    {
      get { return _debugLevel; }
      set { _debugLevel = value; }
    }
    #endregion

    #region Ripping
    [Setting(SettingScope.User, "")]
    public string RipTargetFolder
    {
      get { return _ripTargetFolder; }
      set { _ripTargetFolder = value; }
    }

    [Setting(SettingScope.User, "mp3")]
    public string RipEncoder
    {
      get { return _ripEncoder; }
      set { _ripEncoder = value; }
    }

    [Setting(SettingScope.User, "<A>\\<B>\\<K> - <T>")]
    public string RipFileNameFormat
    {
      get { return _ripFileNameFormat; }
      set { _ripFileNameFormat = value; }
    }

    #region MP3
    [Setting(SettingScope.User, "0")]
    public int RipLamePreset
    {
      get { return _lamePreset; }
      set { _lamePreset = value; }
    }

    [Setting(SettingScope.User, "0")]
    public int RipLameABRBitRate
    {
      get { return _lameABRBitrate; }
      set { _lameABRBitrate = value; }
    }

    [Setting(SettingScope.User, "")]
    public string RipLameExpert
    {
      get { return _lameExpert; }
      set { _lameExpert = value; }
    }
    #endregion

    #region Ogg
    [Setting(SettingScope.User, "3")]
    public int RipOggQuality
    {
      get { return _oggQuality; }
      set { _oggQuality = value; }
    }

    [Setting(SettingScope.User, "")]
    public string RipOggExpert
    {
      get { return _oggExpert; }
      set { _oggExpert = value; }
    }
    #endregion

    #region FLAC
    [Setting(SettingScope.User, "4")]
    public int RipFlacQuality
    {
      get { return _flacQuality; }
      set { _flacQuality = value; }
    }

    [Setting(SettingScope.User, "")]
    public string RipFlacExpert
    {
      get { return _flacExpert; }
      set { _flacExpert = value; }
    }
    #endregion

    #region AAC
    [Setting(SettingScope.User, "aac")]
    public string RipEncoderAAC
    {
      get { return _ripEncoderAAC; }
      set { _ripEncoderAAC = value; }
    }

    [Setting(SettingScope.User, "128")]
    public string RipEncoderAACBitRate
    {
      get { return _ripEncoderAACBitRate; }
      set { _ripEncoderAACBitRate = value; }
    }

    [Setting(SettingScope.User, "Stereo")]
    public string RipEncoderAACChannelMode
    {
      get { return _ripEncoderAACChannelMode; }
      set { _ripEncoderAACChannelMode = value; }
    }
    #endregion

    #region WMA
    [Setting(SettingScope.User, "wma")]
    public string RipEncoderWMA
    {
      get { return _ripEncoderWMA; }
      set { _ripEncoderWMA = value; }
    }

    [Setting(SettingScope.User, "16,2,44100")]
    public string RipEncoderWMASample
    {
      get { return _ripEncoderWMASample; }
      set { _ripEncoderWMASample = value; }
    }

    [Setting(SettingScope.User, "50")]
    public string RipEncoderWMABitRate
    {
      get { return _ripEncoderWMABitRate; }
      set { _ripEncoderWMABitRate = value; }
    }

    [Setting(SettingScope.User, "Vbr")]
    public string RipEncoderWMACbrVbr
    {
      get { return _ripEncoderWMACbrVbr; }
      set { _ripEncoderWMACbrVbr = value; }
    }
    #endregion

    #region MPC
    [Setting(SettingScope.User, "standard")]
    public string RipEncoderMPCPreset
    {
      get { return _ripEncoderMPCPreset; }
      set { _ripEncoderMPCPreset = value; }
    }

    [Setting(SettingScope.User, "")]
    public string RipEncoderMPCExpert
    {
      get { return _ripEncoderMPCExpert; }
      set { _ripEncoderMPCExpert = value; }
    }
    #endregion

    #region WV
    [Setting(SettingScope.User, "-h")]
    public string RipEncoderWVPreset
    {
      get { return _ripEncoderWVPreset; }
      set { _ripEncoderWVPreset = value; }
    }

    [Setting(SettingScope.User, "")]
    public string RipEncoderWVExpert
    {
      get { return _ripEncoderWVExpert; }
      set { _ripEncoderWVExpert = value; }
    }
    #endregion
    #endregion
    #endregion
  }
}
