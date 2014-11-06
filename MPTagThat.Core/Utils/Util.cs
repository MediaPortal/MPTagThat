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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Elegant.Ui;
using NLog;
using TagLib;
using Un4seen.Bass.AddOn.Cd;
using File = TagLib.File;
using StringCollection = System.Collections.Specialized.StringCollection;
using Tag = TagLib.Id3v2.Tag;

#endregion

namespace MPTagThat.Core
{
  public sealed class Util
  {
    #region Enum

    public enum MP3Error : int
    {
      NoError = 0,
      Fixable = 1,
      NonFixable = 2,
      Fixed = 3
    }

    #endregion

    #region Structures

    #region Nested type: IconInfo

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IconInfo
    {
      public bool fIcon;
      public int xHotspot;
      public int yHotspot;
      public IntPtr hbmMask;
      public IntPtr hbmColor;
    }

    #endregion

    #region Nested type: SHFILEOPSTRUCT

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct SHFILEOPSTRUCT
    {
      public IntPtr hwnd;
      [MarshalAs(UnmanagedType.U4)] public int wFunc;
      public string pFrom;
      public string pTo;
      public short fFlags;
      [MarshalAs(UnmanagedType.Bool)] public bool fAnyOperationsAborted;
      public IntPtr hNameMappings;
      public string lpszProgressTitle;
    }

    #endregion

    #endregion

    #region Variables

    private static Util instance = new Util();
    private static Logger log;
    private static readonly object padlock = new object();

    private static char[] _invalidFilenameChars;
    private static char[] _invalidFoldernameChars;

    private static string _cdDriveLetters; // Contains the Drive letters of all available CD Drives

    private static readonly string[] _iso_languages = {
                                                        "aar - Afar", "abk - Abkhazian", "ace - Achinese", "ach - Acoli"
                                                        , "ada - Adangme", "ady - Adyghe; Adygei",
                                                        "afa - Afro-Asiatic (Other)", "afh - Afrihili",
                                                        "afr - Afrikaans", "ain - Ainu", "aka - Akan", "akk - Akkadian",
                                                        "alb - Albanian", "ale - Aleut", "alg - Algonquian languages",
                                                        "alt - Southern Altai", "amh - Amharic",
                                                        "ang - English, Old (ca.450-1100)", "anp - Angika",
                                                        "apa - Apache languages", "ara - Arabic",
                                                        "arc - Official Aramaic (700-300 BCE)", "arg - Aragonese",
                                                        "arm - Armenian", "arn - Mapudungun; Mapuche", "arp - Arapaho",
                                                        "art - Artificial (Other)", "arw - Arawak", "asm - Assamese",
                                                        "ast - Asturian; Bable; Leonese; Asturleonese",
                                                        "ath - Athapascan languages", "aus - Australian languages",
                                                        "ava - Avaric", "ave - Avestan", "awa - Awadhi", "aym - Aymara",
                                                        "aze - Azerbaijani", "bad - Banda languages",
                                                        "bai - Bamileke languages", "bak - Bashkir", "bal - Baluchi",
                                                        "bam - Bambara", "ban - Balinese", "baq - Basque", "bas - Basa",
                                                        "bat - Baltic (Other)", "bej - Beja; Bedawiyet",
                                                        "bel - Belarusian", "bem - Bemba", "ben - Bengali",
                                                        "ber - Berber (Other)", "bho - Bhojpuri", "bih - Bihari",
                                                        "bik - Bikol", "bin - Bini; Edo", "bis - Bislama",
                                                        "bla - Siksika", "bnt - Bantu (Other)", "bos - Bosnian",
                                                        "bra - Braj",
                                                        "bre - Breton", "btk - Batak languages", "bua - Buriat",
                                                        "bug - Buginese", "bul - Bulgarian", "bur - Burmese",
                                                        "byn - Blin; Bilin", "cad - Caddo",
                                                        "cai - Central American Indian (Other)", "car - Galibi Carib",
                                                        "cat - Catalan; Valencian", "cau - Caucasian (Other)",
                                                        "ceb - Cebuano", "cel - Celtic (Other)", "cha - Chamorro",
                                                        "chb - Chibcha",
                                                        "che - Chechen", "chg - Chagatai", "chi - Chinese",
                                                        "chk - Chuukese", "chm - Mari", "chn - Chinook jargon",
                                                        "cho - Choctaw", "chp - Chipewyan; Dene Suline",
                                                        "chr - Cherokee", "chu - Church Slavic; Old Slavonic",
                                                        "chv - Chuvash", "chy - Cheyenne", "cmc - Chamic languages",
                                                        "cop - Coptic", "cor - Cornish", "cos - Corsican",
                                                        "cpe - Creoles and pidgins, English based (Other)",
                                                        "cpf - Creoles and pidgins, French-based (Other)",
                                                        "cpp - Creoles and pidgins, Portuguese-based (Other)",
                                                        "cre - Cree", "crh - Crimean Tatar; Crimean Turkish",
                                                        "crp - Creoles and pidgins (Other)", "csb - Kashubian",
                                                        "cus - Cushitic (Other)", "cze - Czech", "dak - Dakota",
                                                        "dan - Danish", "dar - Dargwa", "day - Land Dayak languages",
                                                        "del - Delaware", "den - Slave (Athapascan)", "dgr - Dogrib",
                                                        "din - Dinka", "div - Divehi; Dhivehi; Maldivian", "doi - Dogri"
                                                        , "dra - Dravidian (Other)", "dsb - Lower Sorbian",
                                                        "dua - Duala", "dum - Dutch, Middle (ca.1050-1350)",
                                                        "dut - Dutch; Flemish", "dyu - Dyula", "dzo - Dzongkha",
                                                        "efi - Efik", "egy - Egyptian (Ancient)", "eka - Ekajuk",
                                                        "elx - Elamite", "eng - English",
                                                        "enm - English, Middle (1100-1500)", "epo - Esperanto",
                                                        "est - Estonian", "ewe - Ewe", "ewo - Ewondo", "fan - Fang",
                                                        "fao - Faroese", "fat - Fanti", "fij - Fijian",
                                                        "fil - Filipino; Pilipino", "fin - Finnish",
                                                        "fiu - Finno-Ugrian (Other)", "fon - Fon", "fre - French",
                                                        "frm - French, Middle (ca.1400-1600)",
                                                        "fro - French, Old (842-ca.1400)",
                                                        "frr - Northern Frisian", "frs - Eastern Frisian",
                                                        "fry - Western Frisian", "ful - Fulah", "fur - Friulian",
                                                        "gaa - Ga", "gay - Gayo", "gba - Gbaya",
                                                        "gem - Germanic (Other)", "geo - Georgian", "ger - German",
                                                        "gez - Geez", "gil - Gilbertese",
                                                        "gla - Gaelic; Scottish Gaelic", "gle - Irish", "glg - Galician"
                                                        , "glv - Manx", "gmh - German, Middle High (ca.1050-1500)",
                                                        "goh - German, Old High (ca.750-1050)", "gon - Gondi",
                                                        "gor - Gorontalo", "got - Gothic", "grb - Grebo",
                                                        "grc - Greek, Ancient (to 1453)", "gre - Greek, Modern (1453-)",
                                                        "grn - Guarani", "gsw - Swiss German; Alemannic; Alsatian",
                                                        "guj - Gujarati", "gwi - Gwich'in", "hai - Haida",
                                                        "hat - Haitian; Haitian Creole", "hau - Hausa", "haw - Hawaiian"
                                                        , "heb - Hebrew", "her - Herero",
                                                        "hil - Hiligaynon", "him - Himachali", "hin - Hindi",
                                                        "hit - Hittite", "hmn - Hmong", "hmo - Hiri Motu",
                                                        "hsb - Upper Sorbian", "hun - Hungarian", "hup - Hupa",
                                                        "iba - Iban", "ibo - Igbo", "ice - Icelandic", "ido - Ido",
                                                        "iii - Sichuan Yi; Nuosu", "ijo - Ijo languages",
                                                        "iku - Inuktitut", "ile - Interlingue; Occidental",
                                                        "ilo - Iloko", "ina - Interlingua", "inc - Indic (Other)",
                                                        "ind - Indonesian", "ine - Indo-European (Other)",
                                                        "inh - Ingush", "ipk - Inupiaq", "ira - Iranian (Other)",
                                                        "iro - Iroquoian languages", "ita - Italian", "jav - Javanese",
                                                        "jbo - Lojban", "jpn - Japanese", "jpr - Judeo-Persian",
                                                        "jrb - Judeo-Arabic", "kaa - Kara-Kalpak", "kab - Kabyle",
                                                        "kac - Kachin; Jingpho", "kal - Kalaallisut; Greenlandic",
                                                        "kam - Kamba", "kan - Kannada", "kar - Karen languages",
                                                        "kas - Kashmiri", "kau - Kanuri", "kaw - Kawi", "kaz - Kazakh",
                                                        "kbd - Kabardian", "kha - Khasi", "khi - Khoisan (Other)",
                                                        "khm - Central Khmer", "kho - Khotanese", "kik - Kikuyu; Gikuyu"
                                                        , "kin - Kinyarwanda", "kir - Kirghiz; Kyrgyz", "kmb - Kimbundu"
                                                        , "kok - Konkani", "kom - Komi",
                                                        "kon - Kongo", "kor - Korean", "kos - Kosraean", "kpe - Kpelle",
                                                        "krc - Karachay-Balkar", "krl - Karelian", "kro - Kru languages"
                                                        , "kru - Kurukh", "kua - Kuanyama; Kwanyama", "kum - Kumyk",
                                                        "kur - Kurdish", "kut - Kutenai", "lad - Ladino", "lah - Lahnda"
                                                        , "lam - Lamba", "lao - Lao", "lat - Latin", "lav - Latvian",
                                                        "lez - Lezghian", "lim - Limburgan; Limburger; Limburgish",
                                                        "lin - Lingala", "lit - Lithuanian", "lol - Mongo", "loz - Lozi"
                                                        , "ltz - Luxembourgish; Letzeburgesch", "lua - Luba-Lulua",
                                                        "lub - Luba-Katanga", "lug - Ganda", "lui - Luiseno",
                                                        "lun - Lunda", "luo - Luo (Kenya and Tanzania)", "lus - Lushai",
                                                        "mac - Macedonian", "mad - Madurese", "mag - Magahi",
                                                        "mah - Marshallese", "mai - Maithili",
                                                        "mak - Makasar", "mal - Malayalam", "man - Mandingo",
                                                        "mao - Maori", "map - Austronesian (Other)", "mar - Marathi",
                                                        "mas - Masai", "may - Malay", "mdf - Moksha", "mdr - Mandar",
                                                        "men - Mende", "mga - Irish, Middle (900-1200)",
                                                        "mic - Mi'kmaq; Micmac", "min - Minangkabau",
                                                        "mis - Uncoded languages", "mkh - Mon-Khmer (Other)",
                                                        "mlg - Malagasy", "mlt - Maltese",
                                                        "mnc - Manchu", "mni - Manipuri", "mno - Manobo languages",
                                                        "moh - Mohawk", "mol - Moldavian", "mon - Mongolian",
                                                        "mos - Mossi", "mul - Multiple languages",
                                                        "mun - Munda languages", "mus - Creek", "mwl - Mirandese",
                                                        "mwr - Marwari", "myn - Mayan languages", "myv - Erzya",
                                                        "nah - Nahuatl languages", "nai - North American Indian",
                                                        "nap - Neapolitan", "nau - Nauru",
                                                        "nav - Navajo; Navaho", "nbl - Ndebele, South; South Ndebele",
                                                        "nde - Ndebele, North; North Ndebele", "ndo - Ndonga",
                                                        "nds - Low German; Low Saxon; German, Low; Saxon, Low",
                                                        "nep - Nepali", "new - Nepal Bhasa; Newari", "nia - Nias",
                                                        "nic - Niger-Kordofanian (Other)", "niu - Niuean",
                                                        "nno - Norwegian Nynorsk; Nynorsk, Norwegian",
                                                        "nob - Bokmål, Norwegian; Norwegian Bokmål", "nog - Nogai",
                                                        "non - Norse, Old", "nor - Norwegian", "nqo - N'Ko",
                                                        "nso - Pedi; Sepedi; Northern Sotho", "nub - Nubian languages",
                                                        "nwc - Classical Newari; Old Newari; Classical Nepal Bhasa",
                                                        "nya - Chichewa; Chewa; Nyanja", "nym - Nyamwezi",
                                                        "nyn - Nyankole", "nyo - Nyoro", "nzi - Nzima",
                                                        "oci - Occitan (post 1500); Provençal", "oji - Ojibwa",
                                                        "ori - Oriya", "orm - Oromo", "osa - Osage",
                                                        "oss - Ossetian; Ossetic", "ota - Turkish, Ottoman (1500-1928)",
                                                        "oto - Otomian languages", "paa - Papuan (Other)",
                                                        "pag - Pangasinan", "pal - Pahlavi",
                                                        "pam - Pampanga; Kapampangan", "pan - Panjabi; Punjabi",
                                                        "pap - Papiamento", "pau - Palauan",
                                                        "peo - Persian, Old (ca.600-400 B.C.)", "per - Persian",
                                                        "phi - Philippine (Other)", "phn - Phoenician", "pli - Pali",
                                                        "pol - Polish", "pon - Pohnpeian", "por - Portuguese",
                                                        "pra - Prakrit languages", "pro - Provençal, Old (to 1500)",
                                                        "pus - Pushto; Pashto", "que - Quechua", "raj - Rajasthani",
                                                        "rap - Rapanui", "rar - Rarotongan; Cook Islands Maori",
                                                        "roa - Romance (Other)", "roh - Romansh", "rom - Romany",
                                                        "rum - Romanian", "run - Rundi",
                                                        "rup - Aromanian; Arumanian; Macedo-Romanian", "rus - Russian",
                                                        "sad - Sandawe", "sag - Sango", "sah - Yakut",
                                                        "sai - South American Indian (Other)",
                                                        "sal - Salishan languages", "sam - Samaritan Aramaic",
                                                        "san - Sanskrit", "sas - Sasak", "sat - Santali",
                                                        "scc - Serbian", "scn - Sicilian", "sco - Scots",
                                                        "scr - Croatian", "sel - Selkup", "sem - Semitic (Other)",
                                                        "sga - Irish, Old (to 900)", "sgn - Sign Languages",
                                                        "shn - Shan",
                                                        "sid - Sidamo", "sin - Sinhala; Sinhalese",
                                                        "sio - Siouan languages", "sit - Sino-Tibetan (Other)",
                                                        "sla - Slavic (Other)", "slo - Slovak", "slv - Slovenian",
                                                        "sma - Southern Sami", "sme - Northern Sami",
                                                        "smi - Sami languages (Other)", "smj - Lule Sami",
                                                        "smn - Inari Sami", "smo - Samoan", "sms - Skolt Sami",
                                                        "sna - Shona", "snd - Sindhi", "snk - Soninke", "sog - Sogdian",
                                                        "som - Somali", "son - Songhai languages",
                                                        "sot - Sotho, Southern", "spa - Spanish; Castilian",
                                                        "srd - Sardinian", "srn - Sranan Tongo", "srr - Serer",
                                                        "ssa - Nilo-Saharan (Other)", "ssw - Swati", "suk - Sukuma",
                                                        "sun - Sundanese", "sus - Susu", "sux - Sumerian",
                                                        "swa - Swahili", "swe - Swedish", "syc - Classical Syriac",
                                                        "syr - Syriac", "tah - Tahitian", "tai - Tai (Other)",
                                                        "tam - Tamil", "tat - Tatar", "tel - Telugu", "tem - Timne",
                                                        "ter - Tereno", "tet - Tetum",
                                                        "tgk - Tajik", "tgl - Tagalog", "tha - Thai", "tib - Tibetan",
                                                        "tig - Tigre", "tir - Tigrinya", "tiv - Tiv", "tkl - Tokelau",
                                                        "tlh - Klingon; tlhIngan-Hol", "tli - Tlingit", "tmh - Tamashek"
                                                        , "tog - Tonga (Nyasa)", "ton - Tonga (Tonga Islands)",
                                                        "tpi - Tok Pisin", "tsi - Tsimshian", "tsn - Tswana",
                                                        "tso - Tsonga", "tuk - Turkmen", "tum - Tumbuka",
                                                        "tup - Tupi languages", "tur - Turkish",
                                                        "tut - Altaic (Other)", "tvl - Tuvalu", "twi - Twi",
                                                        "tyv - Tuvinian", "udm - Udmurt", "uga - Ugaritic",
                                                        "uig - Uighur; Uyghur", "ukr - Ukrainian", "umb - Umbundu",
                                                        "und - Undetermined", "urd - Urdu", "uzb - Uzbek", "vai - Vai",
                                                        "ven - Venda", "vie - Vietnamese", "vol - Volapük",
                                                        "vot - Votic", "wak - Wakashan languages", "wal - Walamo",
                                                        "war - Waray",
                                                        "was - Washo", "wel - Welsh", "wen - Sorbian languages",
                                                        "wln - Walloon", "wol - Wolof", "xal - Kalmyk; Oirat",
                                                        "xho - Xhosa", "yao - Yao", "yap - Yapese", "yid - Yiddish",
                                                        "yor - Yoruba", "ypk - Yupik languages", "zap - Zapotec",
                                                        "zbl - Blissymbols; Blissymbolics; Bliss", "zen - Zenaga",
                                                        "zha - Zhuang; Chuang", "znd - Zande languages", "zul - Zulu",
                                                        "zun - Zuni", "zxx - No linguistic content",
                                                        "zza - Zaza; Dimili; Dimli; Kirdki; Kirmanjki; Zazaki"
                                                      };

    private static readonly string[] _standardId3Frames = new[]
                                                            {
                                                              "TPE1", "TPE2", "TALB", "TBPM", "COMM", "TCOM",
                                                              "TPE3", "TCOP", "TPOS", "TCON", "TIT1", "USLT", "APIC",
                                                              "POPM", "TIT2", "TRCK", "TYER", "TDRC"
                                                            };

    private static readonly string[] _extendedId3Frames = new[]
                                                            {
                                                              "TSOP", "TSOA", "WCOM", "WCOP", "TENC", "TPE4", "TIPL",
                                                              "IPLS", "TMED", "TMCL", "WOAF", "WOAR", "WOAS", "WORS", 
                                                              "WPAY", "WPUB", "TOAL", "TOFN", "TOLY", "TOPE", "TOWN", 
                                                              "TDOR", "TORY", "TPUB", "TIT3", "TEXT","TSOT", "TLEN",
                                                              "TCMP", "TSO2"
                                                            };

    private static readonly Dictionary<string, Image> FileLargeImageCache = new Dictionary<string, Image>();
    private static readonly Dictionary<string, Image> FileSmallImageCache = new Dictionary<string, Image>();

    #endregion

    #region constants

    public const int FO_DELETE = 3;
    public const int FOF_ALLOWUNDO = 0x40;
    public const int FOF_NOCONFIRMATION = 0x10; //Don't prompt the user.; 

    #endregion

    #region ctor

    private Util()
    {
      log = ServiceScope.Get<ILogger>().GetLogger;
      _invalidFilenameChars = Path.GetInvalidFileNameChars();
      _invalidFoldernameChars = Path.GetInvalidPathChars();

      // The above function doesn't return all character, which would for an invalid Path. so we add it ourselves here
      StringBuilder sb = new StringBuilder();
      sb.Append(_invalidFoldernameChars);
      sb.Append('?');
      sb.Append(':'); // is allowed after the drive letter, but not afterwards
      sb.Append('*');
      sb.Append('/');
      _invalidFoldernameChars = sb.ToString().ToCharArray();

      // Get the drive letters of available CD/DVD drives
      int driveCount = BassCd.BASS_CD_GetDriveCount();
      StringBuilder builderDriveLetter = new StringBuilder();
      // Get Drive letters assigned
      for (int i = 0; i < driveCount; i++)
      {
        builderDriveLetter.Append(BassCd.BASS_CD_GetInfo(i).DriveLetter);
        BassCd.BASS_CD_Release(i);
      }
      _cdDriveLetters = builderDriveLetter.ToString();
    }

    #endregion

    #region Imports

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

    [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

    [DllImport("user32.dll")]
    public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

    [DllImport("user32.dll", CharSet = CharSet.Ansi)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int len, IntPtr order);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool SetWindowText(IntPtr hwnd, String lpString);

    #endregion

    #region Properties

    /// <summary>
    /// The available ISO Languages
    /// </summary>
    public static string[] ISO_LANGUAGES
    {
      get { return _iso_languages; }
    }

    /// <summary>
    /// The standard ID3 Frames directly supported by TagLib #
    /// </summary>
    public static string[] StandardFrames
    {
      get { return _standardId3Frames; }
    }

    /// <summary>
    /// The extended ID3 Frames 
    /// </summary>
    public static string[] ExtendedFrames
    {
      get { return _extendedId3Frames; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Singleton Implementation
    /// </summary>
    public static Util Instance
    {
      get
      {
        lock (padlock)
        {
          if (instance == null)
          {
            instance = new Util();
          }
          return instance;
        }
      }
    }

    /// <summary>
    ///   Is this an Audio file, which can be handled by MPTagThat
    /// </summary>
    /// <param name = "fileName"></param>
    /// <returns></returns>
    public static bool IsAudio(string fileName)
    {
      string ext = Path.GetExtension(fileName).ToLower();

      switch (ext)
      {
        case ".aif":
        case ".aiff":
        case ".ape":
        case ".asf":
        case ".dsf":
        case ".flac":
        case ".mp3":
        case ".ogg":
        case ".wv":
        case ".wma":
        case ".mp4":
        case ".m4a":
        case ".m4b":
        case ".m4p":
        case ".mpc":
        case ".mp+":
        case ".mpp":
        case ".wav":
          return true;
      }
      return false;
    }

    /// <summary>
    ///   Is this a Picture file, which can be shown in Listview
    /// </summary>
    /// <param name = "fileName"></param>
    /// <returns></returns>
    public static bool IsPicture(string fileName)
    {
      string ext = Path.GetExtension(fileName).ToLower();

      switch (ext)
      {
        case ".bmp":
        case ".gif":
        case ".jpg":
        case ".png":
          return true;
      }
      return false;
    }

    /// <summary>
    /// Checks, if we got a replaygain frame
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static bool IsReplayGain(string description)
    {
      switch (description.ToLowerInvariant())
      {
        case "replaygain_track_gain":
        case "replaygain_track_peak":
        case "replaygain_album_gain":
        case "replaygain_album_peak":
          return true;
      }
      return false;
    }

    /// <summary>
    ///   Check the Parameter Format for validity
    /// </summary>
    /// <param name = "str"></param>
    /// <param name = "formattype"></param>
    /// <returns></returns>
    public static bool CheckParameterFormat(string str, Options.ParameterFormat formattype)
    {
      if (formattype == Options.ParameterFormat.FileNameToTag || formattype == Options.ParameterFormat.TagToFileName ||
          formattype == Options.ParameterFormat.RipFileName || formattype == Options.ParameterFormat.Organise)
      {
        str = str.Replace("<A>", "\x0001"); // Artist
        str = str.Replace("<T>", "\x0001"); // Title
        str = str.Replace("<B>", "\x0001"); // Album
        str = str.Replace("<G>", "\x0001"); // Genre
        str = str.Replace("<Y>", "\x0001"); // Year
        str = str.Replace("<O>", "\x0001"); // Album Artist / Orchestra / Band
        str = str.Replace("<K>", "\x0001"); // Track
      }

      if (formattype == Options.ParameterFormat.FileNameToTag || formattype == Options.ParameterFormat.TagToFileName ||
          formattype == Options.ParameterFormat.Organise)
      {
        str = str.Replace("<C>", "\x0001"); // Comment
        str = str.Replace("<D>", "\x0001"); // Disk (Position in Mediaset)
        str = str.Replace("<d>", "\x0001"); // Total # of Disks
        str = str.Replace("<k>", "\x0001"); // Total # of Tracks
        str = str.Replace("<U>", "\x0001"); // Content Group
        str = str.Replace("<N>", "\x0001"); // Conductor
        str = str.Replace("<R>", "\x0001"); // Composer
        str = str.Replace("<S>", "\x0001"); // Subtitle
        str = str.Replace("<E>", "\x0001"); // BPM
        str = str.Replace("<M>", "\x0001"); // Modified by / remixed by
      }

      if (formattype == Options.ParameterFormat.TagToFileName)
      {
        str = str.Replace("<F>", "\x0001"); // FileName      
        str = str.Replace("<#>", "\x0001"); // Enumerate in File

        int index = str.IndexOf("<K:"); // Track Number with a given length of digits
        if (index > -1 && !CheckParmWithLengthIndicator(index, str, out str))
        {
          return false;
        }

        index = str.IndexOf("<k:"); // Total Number Tracks with a given length of digits
        if (index > -1 && !CheckParmWithLengthIndicator(index, str, out str))
        {
          return false;
        }

        index = str.IndexOf("<D:"); // Disc Number with a given length of digits
        if (index > -1 && !CheckParmWithLengthIndicator(index, str, out str))
        {
          return false;
        }

        index = str.IndexOf("<d:"); // Total Number Discs with a given length of digits
        if (index > -1 && !CheckParmWithLengthIndicator(index, str, out str))
        {
          return false;
        }
      }

      if (formattype == Options.ParameterFormat.Organise)
      {
        str = str.Replace("<I>", "\x0001"); // Bitrate      

        int index = str.IndexOf("<A:");
        if (index > -1 && !CheckParmWithLengthIndicator(index, str, out str))
        {
          return false;
        }

        index = str.IndexOf("<O:");
        if (index > -1 && !CheckParmWithLengthIndicator(index, str, out str))
        {
          return false;
        }
      }

      if (formattype == Options.ParameterFormat.FileNameToTag)
      {
        str = str.Replace("<X>", "\x0001"); // Unused
      }

      if ((str.IndexOf("<") >= 0 || str.IndexOf(">") >= 0) ||
          (str.IndexOf("\x0001\x0001") >= 0 && formattype == Options.ParameterFormat.FileNameToTag))
        return false;

      return true;
    }

    /// <summary>
    ///   Check a Parameter with a given Length Indicator for correctness
    /// </summary>
    /// <param name = "startIndex"></param>
    /// <param name = "str"></param>
    /// <param name = "parmString"></param>
    /// <returns></returns>
    private static bool CheckParmWithLengthIndicator(int startIndex, string str, out string parmString)
    {
      bool retVal = true;
      int last = -1;
      last = str.IndexOf(">", startIndex);
      string s1 = str.Substring(startIndex, last - startIndex + 1);

      char c = s1[3];
      if (!Char.IsDigit(c))
        retVal = false;

      if (s1.Length > 5)
        retVal = false;

      parmString = str.Replace(s1, "\x0001");
      return retVal;
    }


    /// <summary>
    ///   Make a Valid Filename out of a given String
    /// </summary>
    /// <param name = "str"></param>
    /// <returns></returns>
    public static string MakeValidFileName(string str)
    {
      if (str.IndexOfAny(_invalidFilenameChars) > -1)
      {
        foreach (char c in _invalidFilenameChars)
          str = str.Replace(c, '_');
      }
      return str;
    }

    /// <summary>
    ///   Make a Valid Foldername out of a given String
    /// </summary>
    /// <param name = "str"></param>
    /// <returns></returns>
    public static string MakeValidFolderName(string str)
    {
      if (str.IndexOfAny(_invalidFoldernameChars) > -1)
      {
        foreach (char c in _invalidFoldernameChars)
          str = str.Replace(c, '_');
      }
      return str;
    }

    /// <summary>
    ///   Fast Case Sensitive Replace Method
    /// </summary>
    /// <param name = "Original String"></param>
    /// <param name = "Search Pattern"></param>
    /// <param name = "Replacement String"></param>
    /// <returns></returns>
    public static string ReplaceEx(string original, string pattern, string replacement)
    {
      int count, position0, position1;
      count = position0 = position1 = 0;
      string upperString = original.ToUpper();
      string upperPattern = pattern.ToUpper();
      int inc = (original.Length / pattern.Length) *
                (replacement.Length - pattern.Length);
      char[] chars = new char[original.Length + Math.Max(0, inc)];
      while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
      {
        for (int i = position0; i < position1; ++i)
          chars[count++] = original[i];
        for (int i = 0; i < replacement.Length; ++i)
          chars[count++] = replacement[i];
        position0 = position1 + pattern.Length;
      }
      if (position0 == 0) return original;
      for (int i = position0; i < original.Length; ++i)
        chars[count++] = original[i];
      return new string(chars, 0, count);
    }

    /// <summary>
    ///   Convert the Label to a Parameter
    /// </summary>
    /// <param name = "label"></param>
    /// <returns></returns>
    public static string LabelToParameter(string label)
    {
      string parameter = String.Empty;

      switch (label)
      {
        case "lblParmArtist":
          parameter = "<A>";
          break;

        case "lblParmTitle":
          parameter = "<T>";
          break;

        case "lblParmAlbum":
          parameter = "<B>";
          break;

        case "lblParmYear":
          parameter = "<Y>";
          break;

        case "lblParmTrack":
          parameter = "<K>";
          break;

        case "lblParmTrackTotal":
          parameter = "<k>";
          break;

        case "lblParmDisc":
          parameter = "<D>";
          break;

        case "lblParmDiscTotal":
          parameter = "<d>";
          break;

        case "lblParmGenre":
          parameter = "<G>";
          break;

        case "lblAlbumArtist":
          parameter = "<O>";
          break;

        case "lblParmComment":
          parameter = "<C>";
          break;

        case "lblConductor":
          parameter = "<N>";
          break;

        case "lblComposer":
          parameter = "<R>";
          break;

        case "lblModifiedBy":
          parameter = "<M>";
          break;

        case "lblBPM":
          parameter = "<E>";
          break;

        case "lblSubTitle":
          parameter = "<S>";
          break;

        case "lblContentGroup":
          parameter = "<U>";
          break;

        case "lblParmFileName":
          parameter = "<F>";
          break;

        case "lblParmEnumerate":
          parameter = "<#>";
          break;

        case "lblParmBitRate":
          parameter = "<I>";
          break;

        case "lblParmFirstArtist":
          parameter = "<A:n>";
          break;

        case "lblParmFirstAlbumArtist":
          parameter = "<O:n>";
          break;

        case "lblParmUnused":
          parameter = "<X>";
          break;

        case "lblParmFolder":
          parameter = @"\";
          break;
      }
      return parameter;
    }


    /// <summary>
    ///   Checks, if the given Drive Letter is a Red Book (Audio) CD
    /// </summary>
    /// <param name = "driveLetter"></param>
    /// <returns></returns>
    public static bool isARedBookCD(string drive)
    {
      try
      {
        if (drive.Length < 1) return false;
        char driveLetter = Path.GetFullPath(drive).ToCharArray()[0];
        int cddaTracks = BassCd.BASS_CD_GetTracks(Drive2BassID(driveLetter));

        if (cddaTracks > 0)
          return true;
        else
          return false;
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    ///   Converts the given CD/DVD Drive Letter to a number suiteable for BASS
    /// </summary>
    /// <param name = "driveLetter"></param>
    /// <returns></returns>
    public static int Drive2BassID(char driveLetter)
    {
      BASS_CD_INFO cdinfo = new BASS_CD_INFO();
      for (int i = 0; i < 25; i++)
      {
        if (BassCd.BASS_CD_GetInfo(i, cdinfo))
        {
          if (cdinfo.DriveLetter == driveLetter)
            return i;
        }
      }
      return -1;
    }

    /// <summary>
    /// Checks if the given Drive Letter is a CD/DVD Drive
    /// </summary>
    /// <param name="driveLetter"></param>
    /// <returns></returns>
    public static bool IsCDDrive(string driveLetter)
    {
      if (_cdDriveLetters.Contains(driveLetter))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    ///   Based on the Options set, use the correct version for ID3
    ///   Eventually remove V1 or V2 tags, if set in the options
    /// </summary>
    /// <param name = "File"></param>
    public static File FormatID3Tag(File file)
    {
      if (file.MimeType == "taglib/mp3")
      {
        Tag id3v2_tag = file.GetTag(TagTypes.Id3v2) as Tag;
        if (id3v2_tag != null && Options.MainSettings.ID3V2Version > 0)
          id3v2_tag.Version = (byte)Options.MainSettings.ID3V2Version;

        // Remove V1 Tags, if checked or "Save V2 only checked"
        if (Options.MainSettings.RemoveID3V1 || Options.MainSettings.ID3Version == 2)
          file.RemoveTags(TagTypes.Id3v1);

        // Remove V2 Tags, if checked or "Save V1 only checked"
        if (Options.MainSettings.RemoveID3V2 || Options.MainSettings.ID3Version == 1)
          file.RemoveTags(TagTypes.Id3v2);

        // Remove V2 Tags, if Ape checked
        if (Options.MainSettings.ID3V2Version == 0)
        {
          file.RemoveTags(TagTypes.Id3v2);
        }
        else
        {
          file.RemoveTags(TagTypes.Ape);
        }
      }
      return file;
    }

    /// <summary>
    ///   Formats a Grid Column based on the Settings
    /// </summary>
    /// <param name = "setting"></param>
    public static DataGridViewColumn FormatGridColumn(GridViewColumn setting)
    {
      DataGridViewColumn column;
      switch (setting.Type.ToLower())
      {
        case "image":
          column = new DataGridViewImageColumn();
          ((DataGridViewImageColumn) column).Image = new Bitmap(1, 1);   // Default empty Image
          break;

        case "process":
          column = new DataGridViewProgressColumn();
          break;

        case "check":
          column = new DataGridViewCheckBoxColumn();
          break;

        case "rating":
          column = new DataGridViewRatingColumn();
          break;

        default:
          column = new DataGridViewTextBoxColumn();
          break;
      }

      column.Name = setting.Name;
      column.HeaderText = setting.Title;
      column.ReadOnly = setting.Readonly;
      column.Visible = setting.Display;
      column.Width = setting.Width;
      column.Frozen = setting.Frozen;
      // For columns bound to a data Source set the property
      if (setting.Bound)
      {
        column.DataPropertyName = setting.Name;
      }

      switch (setting.Type.ToLower())
      {
        case "text":
        case "process":
          column.ValueType = typeof (string);
          break;
        case "number":
        case "check":
        case "rating":
          column.ValueType = typeof (int);
          break;
      }

      return column;
    }


    /// <summary>
    ///   Returns the requested WebPage
    /// </summary>
    /// <param name = "requestString"></param>
    /// <returns></returns>
    public static string GetWebPage(string requestString)
    {
      string responseString = null;
      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
        request.Proxy.Credentials = CredentialCache.DefaultCredentials;
        request.UserAgent = "MPTagThat/3.1 ( hwahrmann@team-mediaportal.com )";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader reader = new StreamReader(responseStream))
          {
            responseString = reader.ReadToEnd();
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("Util: Error retrieving Web Page: {0} {1} {2}", requestString, ex.Message, ex.StackTrace);
      }

      return responseString;
    }

    /// <summary>
    /// Post data to WebPage and get result.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="postParameters"></param>
    /// <returns></returns>
    public static string HttpPostRequest(string url, Dictionary<string, string> postParameters)
    {
      string postData = "";

      foreach (string key in postParameters.Keys)
      {
        postData += key + "=" + postParameters[key] + "&";
      }

      HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
      myHttpWebRequest.Method = "POST";

      byte[] data = Encoding.ASCII.GetBytes(postData);

      myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
      myHttpWebRequest.ContentLength = data.Length;

      Stream requestStream = myHttpWebRequest.GetRequestStream();
      requestStream.Write(data, 0, data.Length);
      requestStream.Close();

      HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

      Stream responseStream = myHttpWebResponse.GetResponseStream();

      StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

      string pageContent = myStreamReader.ReadToEnd();

      myStreamReader.Close();
      responseStream.Close();

      myHttpWebResponse.Close();

      return pageContent;
    }
    
    /// <summary>
    ///   Reads data from a stream until the end is reached. The
    ///   data is returned as a byte array. An IOException is
    ///   thrown if any of the underlying IO calls fail.
    /// </summary>
    /// <param name = "stream">The stream to read data from</param>
    /// <param name = "initialLength">The initial buffer length</param>
    public static byte[] ReadFullStream(Stream stream, int initialLength)
    {
      // If we've been passed an unhelpful initial length, just
      // use 32K.
      if (initialLength < 1)
      {
        initialLength = 32768;
      }

      byte[] buffer = new byte[initialLength];
      int read = 0;

      int chunk;
      while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
      {
        read += chunk;

        // If we've reached the end of our buffer, check to see if there's
        // any more information
        if (read == buffer.Length)
        {
          int nextByte = stream.ReadByte();

          // End of stream? If so, we're done
          if (nextByte == -1)
          {
            return buffer;
          }

          // Nope. Resize the buffer, put in the byte we've just
          // read, and continue
          byte[] newBuffer = new byte[buffer.Length * 2];
          Array.Copy(buffer, newBuffer, buffer.Length);
          newBuffer[read] = (byte)nextByte;
          buffer = newBuffer;
          read++;
        }
      }
      // Buffer is now too big. Shrink it.
      byte[] ret = new byte[read];
      Array.Copy(buffer, ret, read);
      return ret;
    }


    /// <summary>
    ///   Example using CallingMethod within another method.
    ///   This still returns information about the caller
    ///   by excluding calls from this Utils class.
    /// </summary>
    public static CallingMethod GetCallingMethod()
    {
      return new CallingMethod(typeof (Util));
    }

    /// <summary>
    ///   write the Method Name into the log
    /// </summary>
    /// <param name = "method"></param>
    public static void EnterMethod(CallingMethod method)
    {
      log.Debug(">>> {0}", method.MethodNameFull);
    }

    /// <summary>
    ///   write the Method Name into the log
    /// </summary>
    /// <param name = "method"></param>
    public static void LeaveMethod(CallingMethod method)
    {
      log.Debug("<<< {0}", method.MethodNameFull);
    }

    /// <summary>
    ///   This function matches the Longet Common Substring of the source string found in target string
    /// </summary>
    /// <param name = "sourceString">The Source String to match</param>
    /// <param name = "targetString">The Target String to search within</param>
    /// <returns>a match ratio</returns>
    public static double LongestCommonSubstring(string sourceString, string targetString)
    {
      if (String.IsNullOrEmpty(sourceString) || String.IsNullOrEmpty(targetString))
        return 0;

      sourceString = sourceString.ToLower().Replace(",", "").Replace(" ", "").Replace(";", "").Replace("_", "");
      targetString = targetString.ToLower().Replace(",", "").Replace(" ", "").Replace(";", "").Replace("_", "");

      int[,] num = new int[sourceString.Length,targetString.Length];
      int maxlen = 0;

      for (int i = 0; i < sourceString.Length; i++)
      {
        for (int j = 0; j < targetString.Length; j++)
        {
          if (sourceString[i] != targetString[j])
            num[i, j] = 0;
          else
          {
            if ((i == 0) || (j == 0))
              num[i, j] = 1;
            else
              num[i, j] = 1 + num[i - 1, j - 1];

            if (num[i, j] > maxlen)
            {
              maxlen = num[i, j];
            }
          }
        }
      }
      return maxlen / (double)sourceString.Length;
    }


    /// <summary>
    ///   Create a cursor from the given Resource Name
    /// </summary>
    /// <param name = "resourceName"></param>
    /// <param name = "xHotSpot"></param>
    /// <param name = "yHotSpot"></param>
    /// <returns></returns>
    public static Cursor CreateCursorFromResource(string resourceName, int xHotSpot, int yHotSpot)
    {
      ResourceManager resourceManager = new ResourceManager("MPTagThat.Properties.Resources",
                                                            Assembly.GetCallingAssembly());
      Bitmap bmp = (Bitmap)resourceManager.GetObject(resourceName);

      if (bmp != null)
      {
        IntPtr ptr = bmp.GetHicon();
        IconInfo tmp = new IconInfo();
        GetIconInfo(ptr, ref tmp);
        tmp.xHotspot = xHotSpot;
        tmp.yHotspot = yHotSpot;
        tmp.fIcon = false;
        ptr = CreateIconIndirect(ref tmp);
        return new Cursor(ptr);
      }
      return Cursors.Default;
    }

    /// <summary>
    ///   Replace the given Parameter string with the values from the Track
    /// </summary>
    /// <param name = "parameter"></param>
    /// <param name = "track"></param>
    /// <returns></returns>
    public static string ReplaceParametersWithTrackValues(string parameter, TrackData track)
    {
      string replacedString = parameter.Trim(new[] {'\\'});

      try
      {
        if (replacedString.IndexOf("<A>") > -1)
          replacedString = replacedString.Replace("<A>", track.Artist.Replace(';', '_').Trim());

        if (replacedString.IndexOf("<T>") > -1)
          replacedString = replacedString.Replace("<T>", track.Title.Trim());

        if (replacedString.IndexOf("<B>") > -1)
          replacedString = replacedString.Replace("<B>", track.Album.Trim());

        if (replacedString.IndexOf("<Y>") > -1)
          replacedString = replacedString.Replace("<Y>", track.Year.ToString().Trim());

        if (replacedString.IndexOf("<K>") > -1)
        {
          string[] str = track.Track.Split('/');
          replacedString = replacedString.Replace("<K>", str[0]);
        }

        if (replacedString.IndexOf("<k>") > -1)
        {
          string[] str = track.Track.Split('/');
          replacedString = replacedString.Replace("<k>", str[1]);
        }

        if (replacedString.IndexOf("<D>") > -1)
        {
          string[] str = track.Disc.Split('/');
          replacedString = replacedString.Replace("<D>", str[0]);
        }

        if (replacedString.IndexOf("<d>") > -1)
        {
          string[] str = track.Disc.Split('/');
          replacedString = replacedString.Replace("<d>", str[1]);
        }

        if (replacedString.IndexOf("<G>") > -1)
        {
          string[] str = track.Genre.Split(';');
          replacedString = replacedString.Replace("<G>", str[0].Trim());
        }

        if (replacedString.IndexOf("<O>") > -1)
          replacedString = replacedString.Replace("<O>", track.AlbumArtist.Replace(';', '_').Trim());

        if (replacedString.IndexOf("<C>") > -1)
          replacedString = replacedString.Replace("<C>", track.Comment.Trim());

        if (replacedString.IndexOf("<U>") > -1)
          replacedString = replacedString.Replace("<U>", track.Grouping.Trim());

        if (replacedString.IndexOf("<N>") > -1)
          replacedString = replacedString.Replace("<N>", track.Conductor.Trim());

        if (replacedString.IndexOf("<R>") > -1)
          replacedString = replacedString.Replace("<R>", track.Composer.Replace(';', '_').Trim());

        if (replacedString.IndexOf("<S>") > -1)
          replacedString = replacedString.Replace("<S>", track.SubTitle.Trim());

        if (replacedString.IndexOf("<E>") > -1)
          replacedString = replacedString.Replace("<E>", track.BPM.ToString());

        if (replacedString.IndexOf("<M>") > -1)
          replacedString = replacedString.Replace("<M>", track.Interpreter.Trim());

        if (replacedString.IndexOf("<I>") > -1)
          replacedString = replacedString.Replace("<I>", track.BitRate);

        int index = replacedString.IndexOf("<A:");
        if (index > -1)
        {
          replacedString = ReplaceStringWithLengthIndicator(index, replacedString, track.Artist.Replace(';', '_').Trim());
        }

        index = replacedString.IndexOf("<O:");
        if (index > -1)
        {
          replacedString = ReplaceStringWithLengthIndicator(index, replacedString,
                                                            track.AlbumArtist.Replace(';', '_').Trim());
        }

        index = replacedString.IndexOf("<K:");
        if (index > -1)
        {
          string[] str = track.Track.Split('/');
          replacedString = ReplaceStringWithLengthIndicator(index, replacedString, str[0]);
        }

        index = replacedString.IndexOf("<k:");
        if (index > -1)
        {
          string[] str = track.Track.Split('/');
          replacedString = ReplaceStringWithLengthIndicator(index, replacedString, str[1]);
        }

        index = replacedString.IndexOf("<D:");
        if (index > -1)
        {
          string[] str = track.Disc.Split('/');
          replacedString = ReplaceStringWithLengthIndicator(index, replacedString, str[0]);
        }

        index = replacedString.IndexOf("<d:");
        if (index > -1)
        {
          string[] str = track.Disc.Split('/');
          replacedString = ReplaceStringWithLengthIndicator(index, replacedString, str[1]);
        }

        // Empty Values would create invalid folders
        replacedString = replacedString.Replace(@"\\", @"\_\");

        // If the directory name starts with a backslash, we've got an empty value on the beginning
        if (replacedString.IndexOf("\\") == 0)
          replacedString = "_" + replacedString;

        // We might have an empty value on the end of the path, which is indicated by a slash. 
        // replace it with underscore
        if (replacedString.LastIndexOf("\\") == replacedString.Length - 1)
          replacedString += "_";

        replacedString = MakeValidFolderName(replacedString);
      }
      catch (Exception)
      {
        return "";
      }
      return replacedString;
    }

    private static string ReplaceStringWithLengthIndicator(int startIndex, string replaceString, string replaceValue)
    {
      // Check if we have a numeric Parameter as replace value
      bool isNumericParm = (replaceString.Substring(1, 1).IndexOfAny(new[] {'K', 'k', 'D', 'd'}) > -1);
      int last = -1;
      last = replaceString.IndexOf(">", startIndex);
      string s1 = replaceString.Substring(startIndex, last - startIndex + 1);
      int strLength = Convert.ToInt32(s1.Substring(3, 1));

      if (replaceValue.Length >= strLength)
      {
        if (isNumericParm)
        {
          replaceValue = replaceValue.Substring(replaceValue.Length - strLength);
        }
        else
        {
          replaceValue = replaceValue.Substring(0, strLength);
        }
      }
      else if (isNumericParm && replaceValue.Length < strLength)
      {
        // Do Pad numeric values with zeroes
        replaceValue = replaceValue.PadLeft(strLength, '0');
      }

      return replaceString.Replace(s1, replaceValue);
    }


    /// <summary>
    ///   Converts a time string "HH:mm:ss" to seconds
    /// </summary>
    /// <param name = "durationString"></param>
    /// <returns></returns>
    public static int DurationToSeconds(string durationString)
    {
      int duration = 0;
      int index = durationString.LastIndexOf(":");
      int i = 0;
      while (index > -1)
      {
        if (i == 0)
        {
          // We've got seconds
          duration = Convert.ToInt32(durationString.Substring(index + 1));
        }
        else
        {
          duration += Convert.ToInt32(durationString.Substring(index + 1)) * (int)Math.Pow(60.0, i);
        }

        durationString = durationString.Substring(0, index);
        index = durationString.LastIndexOf(":");
        i++;
      }
      if (durationString.Length > 0)
      {
        duration += Convert.ToInt32(durationString) * (int)Math.Pow(60.0, i);
      }
      return duration;
    }

    /// <summary>
    ///   Converts given seconds to HH:mm:ss
    /// </summary>
    /// <param name = "lSeconds"></param>
    /// <returns></returns>
    public static string SecondsToHMSString(string sSeconds)
    {
      int lSeconds = Int32.Parse(sSeconds);
      if (lSeconds < 0) return ("0:00");
      int hh = lSeconds / 3600;
      lSeconds = lSeconds % 3600;
      int mm = lSeconds / 60;
      int ss = lSeconds % 60;

      string strHMS = "";
      if (hh >= 1)
        strHMS = String.Format("{0}:{1:00}:{2:00}", hh, mm, ss);
      else
        strHMS = String.Format("{0}:{1:00}", mm, ss);
      return strHMS;
    }

    /// <summary>
    ///   Changes the Quote to a double Quote, to have correct SQL Syntax
    /// </summary>
    /// <param name = "strText"></param>
    /// <returns></returns>
    public static string RemoveInvalidChars(string strText)
    {
      if (strText == null)
      {
        return "";
      }
      return strText.Replace("'", "''").Trim();
    }

    /// <summary>
    ///   Returns a filename prefixed with the Path
    /// </summary>
    /// <param name = "strBasePath"></param>
    /// <param name = "strFileName"></param>
    public static void GetQualifiedFilename(string strBasePath, ref string strFileName)
    {
      if (strFileName == null) return;
      if (strFileName.Length <= 2) return;
      if (strFileName[1] == ':') return;
      strBasePath = RemoveTrailingSlash(strBasePath);
      while (strFileName.StartsWith(@"..\") || strFileName.StartsWith("../"))
      {
        strFileName = strFileName.Substring(3);
        int pos = strBasePath.LastIndexOf(@"\");
        if (pos > 0)
        {
          strBasePath = strBasePath.Substring(0, pos);
        }
        else
        {
          pos = strBasePath.LastIndexOf(@"/");
          if (pos > 0)
          {
            strBasePath = strBasePath.Substring(0, pos);
          }
        }
      }
      if (strBasePath.Length == 2 && strBasePath[1] == ':')
        strBasePath += @"\";
      strFileName = Path.Combine(strBasePath, strFileName);
    }

    public static string RemoveTrailingSlash(string strLine)
    {
      if (strLine == null) return String.Empty;
      if (strLine.Length == 0) return String.Empty;
      string strPath = strLine;
      while (strPath.Length > 0)
      {
        if (strPath[strPath.Length - 1] == '\\' || strPath[strPath.Length - 1] == '/')
        {
          strPath = strPath.Substring(0, strPath.Length - 1);
        }
        else break;
      }
      return strPath;
    }

    /// <summary>
    ///   Creates a relative path from one file or folder to another.
    /// </summary>
    /// <param name = "fromDirectory">
    ///   Contains the directory that defines the 
    ///   start of the relative path.
    /// </param>
    /// <param name = "toPath">
    ///   Contains the path that defines the
    ///   endpoint of the relative path.
    /// </param>
    /// <returns>
    ///   The relative path from the start
    ///   directory to the end path.
    /// </returns>
    /// <exception cref = "ArgumentNullException"></exception>
    public static string RelativePathTo(string fromDirectory, string toPath)
    {
      if (fromDirectory == null)
        throw new ArgumentNullException("fromDirectory");

      if (toPath == null)
        throw new ArgumentNullException("toPath");

      bool isRooted = Path.IsPathRooted(fromDirectory) && Path.IsPathRooted(toPath);

      if (isRooted)
      {
        bool isDifferentRoot = String.Compare(Path.GetPathRoot(fromDirectory), Path.GetPathRoot(toPath), true) != 0;

        if (isDifferentRoot)
          return toPath;
      }

      StringCollection relativePath = new StringCollection();
      string[] fromDirectories = fromDirectory.Split(Path.DirectorySeparatorChar);

      string[] toDirectories = toPath.Split(Path.DirectorySeparatorChar);

      int length = Math.Min(fromDirectories.Length, toDirectories.Length);

      int lastCommonRoot = -1;

      // find common root
      for (int x = 0; x < length; x++)
      {
        if (String.Compare(fromDirectories[x], toDirectories[x], true) != 0)
          break;

        lastCommonRoot = x;
      }

      if (lastCommonRoot == -1)
        return toPath;

      // add relative folders in from path
      for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
        if (fromDirectories[x].Length > 0)
          relativePath.Add("..");

      // add to folders to path
      for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
        relativePath.Add(toDirectories[x]);

      // create relative path
      string[] relativeParts = new string[relativePath.Count];
      relativePath.CopyTo(relativeParts, 0);

      string newPath = String.Join(Path.DirectorySeparatorChar.ToString(), relativeParts);

      return newPath;
    }

    public static Image GetImageAssociatedWithFile(string filePath, bool small)
    {
      string fileExtension = Path.GetExtension(filePath);

      Dictionary<string, Image> fileImageCache = small ? FileSmallImageCache : FileLargeImageCache;

      Image image;
      if (fileImageCache.TryGetValue(fileExtension, out image))
        return image;

      WinApi.SHFILEINFO info = new WinApi.SHFILEINFO();
      int cbFileInfo = Marshal.SizeOf(info);
      int flags = WinApi.SHGFI_ICON | WinApi.SHGFI_USEFILEATTRIBUTES;
      if (small)
        flags |= WinApi.SHGFI_SMALLICON;
      else
        flags |= WinApi.SHGFI_LARGEICON;

      WinApi.SHGetFileInfo(
        fileExtension,
        WinApi.FILE_ATTRIBUTE_NORMAL,
        ref info,
        cbFileInfo,
        flags);

      Icon iconFromHandle = null;

      try
      {
        iconFromHandle = Icon.FromHandle(info.hIcon);
        image = iconFromHandle.ToBitmap();
      }
      finally
      {
        WinApi.DestroyIcon(info.hIcon);
        if (iconFromHandle != null)
          iconFromHandle.Dispose();
      }

      fileImageCache[fileExtension] = image;
      return image;
    }

    /// <summary>
    /// Sends a Progress Message, so that the status bar can be updated
    /// </summary>
    /// <param name="message"></param>
    public static void SendProgress(string message)
    {
      QueueMessage msg = new QueueMessage();
      msg.MessageData["action"] = "statusprogress";
      msg.MessageData["data"] = message;
      IMessageQueue msgQueue = ServiceScope.Get<IMessageBroker>().GetOrCreate("message");
      msgQueue.Send(msg);
    }

    #endregion
  }
}