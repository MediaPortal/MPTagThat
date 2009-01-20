using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using System.Resources;
using System.Reflection;

using Un4seen.Bass.AddOn.Cd;

namespace MPTagThat.Core
{
  public sealed class Util
  {
    #region Structures
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct SHFILEOPSTRUCT
    {
      public IntPtr hwnd;
      [MarshalAs(UnmanagedType.U4)]
      public int wFunc;
      public string pFrom;
      public string pTo;
      public short fFlags;
      [MarshalAs(UnmanagedType.Bool)]
      public bool fAnyOperationsAborted;
      public IntPtr hNameMappings;
      public string lpszProgressTitle;
    }

    public struct IconInfo
    {
      public bool fIcon;
      public int xHotspot;
      public int yHotspot;
      public IntPtr hbmMask;
      public IntPtr hbmColor;
    }
    #endregion

    #region Variables
    static Util instance = new Util();
    static ILogger log;
    static readonly object padlock = new object();

    private static char[] _invalidFilenameChars;
    private static char[] _invalidFoldernameChars;

    private static string[] _iso_languages = { 
"aar - Afar",  "abk - Abkhazian", "ace - Achinese", "ach - Acoli", "ada - Adangme", "ady - Adyghe; Adygei", "afa - Afro-Asiatic (Other)", "afh - Afrihili", "afr - Afrikaans", "ain - Ainu", "aka - Akan", "akk - Akkadian", "alb - Albanian", "ale - Aleut", "alg - Algonquian languages", "alt - Southern Altai", "amh - Amharic", "ang - English, Old (ca.450-1100)", "anp - Angika", "apa - Apache languages", "ara - Arabic",
"arc - Official Aramaic (700-300 BCE)", "arg - Aragonese", "arm - Armenian", "arn - Mapudungun; Mapuche", "arp - Arapaho", "art - Artificial (Other)", "arw - Arawak", "asm - Assamese", "ast - Asturian; Bable; Leonese; Asturleonese", "ath - Athapascan languages", "aus - Australian languages", "ava - Avaric", "ave - Avestan", "awa - Awadhi", "aym - Aymara",
"aze - Azerbaijani", "bad - Banda languages", "bai - Bamileke languages", "bak - Bashkir", "bal - Baluchi", "bam - Bambara", "ban - Balinese", "baq - Basque", "bas - Basa", "bat - Baltic (Other)", "bej - Beja; Bedawiyet", "bel - Belarusian", "bem - Bemba", "ben - Bengali",
"ber - Berber (Other)", "bho - Bhojpuri", "bih - Bihari", "bik - Bikol", "bin - Bini; Edo", "bis - Bislama", "bla - Siksika", "bnt - Bantu (Other)", "bos - Bosnian", "bra - Braj",
"bre - Breton", "btk - Batak languages", "bua - Buriat", "bug - Buginese", "bul - Bulgarian", "bur - Burmese", "byn - Blin; Bilin", "cad - Caddo", "cai - Central American Indian (Other)", "car - Galibi Carib", "cat - Catalan; Valencian", "cau - Caucasian (Other)", "ceb - Cebuano", "cel - Celtic (Other)", "cha - Chamorro", "chb - Chibcha",
"che - Chechen", "chg - Chagatai", "chi - Chinese", "chk - Chuukese", "chm - Mari", "chn - Chinook jargon", "cho - Choctaw", "chp - Chipewyan; Dene Suline", "chr - Cherokee", "chu - Church Slavic; Old Slavonic", "chv - Chuvash", "chy - Cheyenne", "cmc - Chamic languages", "cop - Coptic", "cor - Cornish", "cos - Corsican", "cpe - Creoles and pidgins, English based (Other)",
"cpf - Creoles and pidgins, French-based (Other)", "cpp - Creoles and pidgins, Portuguese-based (Other)", "cre - Cree", "crh - Crimean Tatar; Crimean Turkish", "crp - Creoles and pidgins (Other)", "csb - Kashubian", "cus - Cushitic (Other)", "cze - Czech", "dak - Dakota", "dan - Danish", "dar - Dargwa", "day - Land Dayak languages", "del - Delaware", "den - Slave (Athapascan)", "dgr - Dogrib", "din - Dinka", "div - Divehi; Dhivehi; Maldivian", "doi - Dogri", "dra - Dravidian (Other)", "dsb - Lower Sorbian", "dua - Duala", "dum - Dutch, Middle (ca.1050-1350)",
"dut - Dutch; Flemish", "dyu - Dyula", "dzo - Dzongkha", "efi - Efik", "egy - Egyptian (Ancient)", "eka - Ekajuk", "elx - Elamite", "eng - English", "enm - English, Middle (1100-1500)", "epo - Esperanto", "est - Estonian", "ewe - Ewe", "ewo - Ewondo", "fan - Fang", "fao - Faroese", "fat - Fanti", "fij - Fijian", "fil - Filipino; Pilipino", "fin - Finnish", "fiu - Finno-Ugrian (Other)", "fon - Fon", "fre - French", "frm - French, Middle (ca.1400-1600)", "fro - French, Old (842-ca.1400)",
"frr - Northern Frisian", "frs - Eastern Frisian", "fry - Western Frisian", "ful - Fulah", "fur - Friulian", "gaa - Ga", "gay - Gayo", "gba - Gbaya", "gem - Germanic (Other)", "geo - Georgian", "ger - German", "gez - Geez", "gil - Gilbertese", "gla - Gaelic; Scottish Gaelic", "gle - Irish", "glg - Galician", "glv - Manx", "gmh - German, Middle High (ca.1050-1500)",
"goh - German, Old High (ca.750-1050)", "gon - Gondi", "gor - Gorontalo", "got - Gothic", "grb - Grebo", "grc - Greek, Ancient (to 1453)", "gre - Greek, Modern (1453-)", "grn - Guarani", "gsw - Swiss German; Alemannic; Alsatian", "guj - Gujarati", "gwi - Gwich'in", "hai - Haida", "hat - Haitian; Haitian Creole", "hau - Hausa", "haw - Hawaiian", "heb - Hebrew", "her - Herero",
"hil - Hiligaynon", "him - Himachali", "hin - Hindi", "hit - Hittite", "hmn - Hmong", "hmo - Hiri Motu", "hsb - Upper Sorbian", "hun - Hungarian", "hup - Hupa", "iba - Iban", "ibo - Igbo", "ice - Icelandic", "ido - Ido", "iii - Sichuan Yi; Nuosu", "ijo - Ijo languages",
"iku - Inuktitut", "ile - Interlingue; Occidental", "ilo - Iloko", "ina - Interlingua", "inc - Indic (Other)", "ind - Indonesian", "ine - Indo-European (Other)", "inh - Ingush", "ipk - Inupiaq", "ira - Iranian (Other)", "iro - Iroquoian languages", "ita - Italian", "jav - Javanese", "jbo - Lojban", "jpn - Japanese", "jpr - Judeo-Persian", "jrb - Judeo-Arabic", "kaa - Kara-Kalpak", "kab - Kabyle",
"kac - Kachin; Jingpho", "kal - Kalaallisut; Greenlandic", "kam - Kamba", "kan - Kannada", "kar - Karen languages", "kas - Kashmiri", "kau - Kanuri", "kaw - Kawi", "kaz - Kazakh", "kbd - Kabardian", "kha - Khasi", "khi - Khoisan (Other)", "khm - Central Khmer", "kho - Khotanese", "kik - Kikuyu; Gikuyu", "kin - Kinyarwanda", "kir - Kirghiz; Kyrgyz", "kmb - Kimbundu", "kok - Konkani", "kom - Komi",
"kon - Kongo", "kor - Korean", "kos - Kosraean", "kpe - Kpelle", "krc - Karachay-Balkar", "krl - Karelian", "kro - Kru languages", "kru - Kurukh", "kua - Kuanyama; Kwanyama", "kum - Kumyk", "kur - Kurdish", "kut - Kutenai", "lad - Ladino", "lah - Lahnda", "lam - Lamba", "lao - Lao", "lat - Latin", "lav - Latvian",
"lez - Lezghian", "lim - Limburgan; Limburger; Limburgish", "lin - Lingala", "lit - Lithuanian", "lol - Mongo", "loz - Lozi", "ltz - Luxembourgish; Letzeburgesch", "lua - Luba-Lulua", "lub - Luba-Katanga", "lug - Ganda", "lui - Luiseno", "lun - Lunda", "luo - Luo (Kenya and Tanzania)", "lus - Lushai", "mac - Macedonian", "mad - Madurese", "mag - Magahi", "mah - Marshallese", "mai - Maithili",
"mak - Makasar", "mal - Malayalam", "man - Mandingo", "mao - Maori", "map - Austronesian (Other)", "mar - Marathi", "mas - Masai", "may - Malay", "mdf - Moksha", "mdr - Mandar", "men - Mende", "mga - Irish, Middle (900-1200)", "mic - Mi'kmaq; Micmac", "min - Minangkabau", "mis - Uncoded languages", "mkh - Mon-Khmer (Other)", "mlg - Malagasy", "mlt - Maltese",
"mnc - Manchu", "mni - Manipuri", "mno - Manobo languages", "moh - Mohawk", "mol - Moldavian", "mon - Mongolian", "mos - Mossi", "mul - Multiple languages", "mun - Munda languages", "mus - Creek", "mwl - Mirandese", "mwr - Marwari", "myn - Mayan languages", "myv - Erzya", "nah - Nahuatl languages", "nai - North American Indian", "nap - Neapolitan", "nau - Nauru",
"nav - Navajo; Navaho", "nbl - Ndebele, South; South Ndebele", "nde - Ndebele, North; North Ndebele", "ndo - Ndonga", "nds - Low German; Low Saxon; German, Low; Saxon, Low", "nep - Nepali", "new - Nepal Bhasa; Newari", "nia - Nias", "nic - Niger-Kordofanian (Other)", "niu - Niuean", "nno - Norwegian Nynorsk; Nynorsk, Norwegian", "nob - Bokmål, Norwegian; Norwegian Bokmål", "nog - Nogai", "non - Norse, Old", "nor - Norwegian", "nqo - N'Ko", "nso - Pedi; Sepedi; Northern Sotho", "nub - Nubian languages",
"nwc - Classical Newari; Old Newari; Classical Nepal Bhasa", "nya - Chichewa; Chewa; Nyanja", "nym - Nyamwezi", "nyn - Nyankole", "nyo - Nyoro", "nzi - Nzima", "oci - Occitan (post 1500); Provençal", "oji - Ojibwa", "ori - Oriya", "orm - Oromo", "osa - Osage", "oss - Ossetian; Ossetic", "ota - Turkish, Ottoman (1500-1928)", "oto - Otomian languages", "paa - Papuan (Other)", "pag - Pangasinan", "pal - Pahlavi", "pam - Pampanga; Kapampangan", "pan - Panjabi; Punjabi", "pap - Papiamento", "pau - Palauan",
"peo - Persian, Old (ca.600-400 B.C.)", "per - Persian", "phi - Philippine (Other)", "phn - Phoenician", "pli - Pali", "pol - Polish", "pon - Pohnpeian", "por - Portuguese", "pra - Prakrit languages", "pro - Provençal, Old (to 1500)", "pus - Pushto; Pashto", "que - Quechua", "raj - Rajasthani", "rap - Rapanui", "rar - Rarotongan; Cook Islands Maori", "roa - Romance (Other)", "roh - Romansh", "rom - Romany", "rum - Romanian", "run - Rundi",
"rup - Aromanian; Arumanian; Macedo-Romanian", "rus - Russian", "sad - Sandawe", "sag - Sango", "sah - Yakut", "sai - South American Indian (Other)", "sal - Salishan languages", "sam - Samaritan Aramaic", "san - Sanskrit", "sas - Sasak", "sat - Santali", "scc - Serbian", "scn - Sicilian", "sco - Scots", "scr - Croatian", "sel - Selkup", "sem - Semitic (Other)", "sga - Irish, Old (to 900)", "sgn - Sign Languages", "shn - Shan",
"sid - Sidamo", "sin - Sinhala; Sinhalese", "sio - Siouan languages", "sit - Sino-Tibetan (Other)", "sla - Slavic (Other)", "slo - Slovak", "slv - Slovenian", "sma - Southern Sami", "sme - Northern Sami", "smi - Sami languages (Other)", "smj - Lule Sami", "smn - Inari Sami", "smo - Samoan", "sms - Skolt Sami", "sna - Shona", "snd - Sindhi", "snk - Soninke", "sog - Sogdian", "som - Somali", "son - Songhai languages", "sot - Sotho, Southern", "spa - Spanish; Castilian", "srd - Sardinian", "srn - Sranan Tongo", "srr - Serer",
"ssa - Nilo-Saharan (Other)", "ssw - Swati", "suk - Sukuma", "sun - Sundanese", "sus - Susu", "sux - Sumerian", "swa - Swahili", "swe - Swedish", "syc - Classical Syriac", "syr - Syriac", "tah - Tahitian", "tai - Tai (Other)", "tam - Tamil", "tat - Tatar", "tel - Telugu", "tem - Timne", "ter - Tereno", "tet - Tetum",
"tgk - Tajik", "tgl - Tagalog", "tha - Thai", "tib - Tibetan", "tig - Tigre", "tir - Tigrinya", "tiv - Tiv", "tkl - Tokelau", "tlh - Klingon; tlhIngan-Hol", "tli - Tlingit", "tmh - Tamashek", "tog - Tonga (Nyasa)", "ton - Tonga (Tonga Islands)", "tpi - Tok Pisin", "tsi - Tsimshian", "tsn - Tswana", "tso - Tsonga", "tuk - Turkmen", "tum - Tumbuka", "tup - Tupi languages", "tur - Turkish",
"tut - Altaic (Other)", "tvl - Tuvalu", "twi - Twi", "tyv - Tuvinian", "udm - Udmurt", "uga - Ugaritic", "uig - Uighur; Uyghur", "ukr - Ukrainian", "umb - Umbundu", "und - Undetermined", "urd - Urdu", "uzb - Uzbek", "vai - Vai", "ven - Venda", "vie - Vietnamese", "vol - Volapük", "vot - Votic", "wak - Wakashan languages", "wal - Walamo", "war - Waray",
"was - Washo", "wel - Welsh", "wen - Sorbian languages", "wln - Walloon", "wol - Wolof", "xal - Kalmyk; Oirat", "xho - Xhosa", "yao - Yao", "yap - Yapese", "yid - Yiddish", "yor - Yoruba", "ypk - Yupik languages", "zap - Zapotec", "zbl - Blissymbols; Blissymbolics; Bliss", "zen - Zenaga", "zha - Zhuang; Chuang", "znd - Zande languages", "zul - Zulu", "zun - Zuni", "zxx - No linguistic content", "zza - Zaza; Dimili; Dimli; Kirdki; Kirmanjki; Zazaki" 
};
    #endregion

    #region constants
    public const int FO_DELETE = 3;
    public const int FOF_ALLOWUNDO = 0x40;
    public const int FOF_NOCONFIRMATION = 0x10;    //Don't prompt the user.; 
    #endregion

    #region ctor
    Util()
    {
      log = ServiceScope.Get<ILogger>();
      _invalidFilenameChars = System.IO.Path.GetInvalidFileNameChars();
      _invalidFoldernameChars = System.IO.Path.GetInvalidPathChars();

      // The above function doesn't return all character, which would for an invalid Path. so we add it ourselves here
      StringBuilder sb = new StringBuilder();
      sb.Append(_invalidFoldernameChars);
      sb.Append('?');
      sb.Append(':');  // is allowed after the drive letter, but not afterwards
      sb.Append('*');
      sb.Append('/');
      _invalidFoldernameChars = sb.ToString().ToCharArray();
    }
    #endregion

    #region Imports
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

    [DllImport("User32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

    [DllImport("user32.dll")]
    public static extern IntPtr CreateIconIndirect(ref IconInfo icon);
    #endregion

    #region Properties
    public static string[] ISO_LANGUAGES
    {
      get { return _iso_languages; }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Singleton Implementation
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
    /// Is this an Audio file, which can be handled by MPTagThat
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool IsAudio(string fileName)
    {
      string ext = Path.GetExtension(fileName).ToLower();

      switch (ext)
      {
        case ".ape":
        case ".asf":
        case ".flac":
        case ".mp3":
        case ".ogg":
        case ".wv":
        case ".wma":
        case ".mp4":
        case ".m4a":
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
    /// Check the Parameter Format for validity
    /// </summary>
    /// <param name="str"></param>
    /// <param name="formattype"></param>
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
      }

      if (formattype == Options.ParameterFormat.Organise)
      {
        str = str.Replace("<I>", "\x0001"); // Bitrate      

        int index = str.IndexOf("<A:");
        int last = -1;
        if (index > -1)
        {
          last = str.IndexOf(">", index);
          string s1 = str.Substring(index, last - index + 1);

          char c = s1[3];
          if (!Char.IsDigit(c))
            return false;

          if (s1.Length > 5)
            return false;

          str = str.Replace(s1, "\x0001");
        }

        index = str.IndexOf("<O:");
        last = -1;
        if (index > -1)
        {
          last = str.IndexOf(">", index);
          string s1 = str.Substring(index, last - index + 1);

          char c = s1[3];
          if (!Char.IsDigit(c))
            return false;

          if (s1.Length > 5)
            return false;

          str = str.Replace(s1, "\x0001");
        }
      }

      if (formattype == Options.ParameterFormat.FileNameToTag)
      {
        str = str.Replace("<X>", "\x0001"); // Unused
      }

      if ((str.IndexOf("<") >= 0 || str.IndexOf(">") >= 0) || (str.IndexOf("\x0001\x0001") >= 0 && formattype == Options.ParameterFormat.FileNameToTag))
        return false;

      return true;
    }

    /// <summary>
    /// Make a Valid Filename out of a given String
    /// </summary>
    /// <param name="str"></param>
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
    /// Make a Valid Foldername out of a given String
    /// </summary>
    /// <param name="str"></param>
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
    /// Fast Case Sensitive Replace Method
    /// </summary>
    /// <param name="Original String"></param>
    /// <param name="Search Pattern"></param>
    /// <param name="Replacement String"></param>
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
    /// Convert the Label to a Parameter
    /// </summary>
    /// <param name="label"></param>
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
    /// Checks, if the given Drive Letter is a Red Book (Audio) CD
    /// </summary>
    /// <param name="driveLetter"></param>
    /// <returns></returns>
    public static bool isARedBookCD(string drive)
    {
      try
      {
        if (drive.Length < 1) return false;
        char driveLetter = System.IO.Path.GetFullPath(drive).ToCharArray()[0];
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
    /// Converts the given CD/DVD Drive Letter to a number suiteable for BASS
    /// </summary>
    /// <param name="driveLetter"></param>
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
    /// Based on the Options set, use the correct version for ID3
    /// Eventually remove V1 or V2 tags, if set in the options
    /// </summary>
    /// <param name="File"></param>
    public static TagLib.File FormatID3Tag(TagLib.File file)
    {
      if (file.MimeType == "taglib/mp3")
      {
        TagLib.Id3v2.Tag id3v2_tag = file.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag;
        if (id3v2_tag != null && Options.MainSettings.ID3V2Version > 0)
          id3v2_tag.Version = (byte)Options.MainSettings.ID3V2Version;

        // Remove V1 Tags, if checked or "Save V2 only checked"
        if (Options.MainSettings.RemoveID3V1 || Options.MainSettings.ID3Version == 2)
          file.RemoveTags(TagLib.TagTypes.Id3v1);

        // Remove V2 Tags, if checked or "Save V1 only checked"
        if (Options.MainSettings.RemoveID3V2 || Options.MainSettings.ID3Version == 1)
          file.RemoveTags(TagLib.TagTypes.Id3v2);
        
        // Remove V2 Tags, if Ape checked
        if (Options.MainSettings.ID3V2Version == 0)
        {
          file.RemoveTags(TagLib.TagTypes.Id3v2);
        }
        else
        {
          file.RemoveTags(TagLib.TagTypes.Ape);
        }

      }
      return file;
    }

    /// <summary>
    /// Formats a Grid Column based on the Settings
    /// </summary>
    /// <param name="setting"></param>
    public static DataGridViewColumn FormatGridColumn(GridViewColumn setting)
    {
      DataGridViewColumn column;
      switch (setting.Type.ToLower())
      {
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
          column.ValueType = typeof(string);
          break;
        case "number":
        case "check":
        case "rating":
          column.ValueType = typeof(int);
          break;
      }

      return column;
    }


    /// <summary>
    /// Returns the requested WebPage
    /// </summary>
    /// <param name="requestString"></param>
    /// <returns></returns>
    public static string GetWebPage(string requestString)
    {
      string responseString = null;
      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
        request.Proxy.Credentials = CredentialCache.DefaultCredentials;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader reader = new StreamReader(responseStream))
          {
            responseString = reader.ReadToEnd();
          }
        }
      }
      catch (Exception)
      { }

      return responseString;
    }

    /// <summary>
    /// Reads data from a stream until the end is reached. The
    /// data is returned as a byte array. An IOException is
    /// thrown if any of the underlying IO calls fail.
    /// </summary>
    /// <param name="stream">The stream to read data from</param>
    /// <param name="initialLength">The initial buffer length</param>
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
    /// Example using CallingMethod within another method.
    /// This still returns information about the caller
    /// by excluding calls from this Utils class.
    /// </summary>
    static public CallingMethod GetCallingMethod()
    {
      return new CallingMethod(typeof(Util));
    }

    /// <summary>
    /// write the Method Name into the log
    /// </summary>
    /// <param name="method"></param>
    static public void EnterMethod(CallingMethod method)
    {
      log.Debug(">>> {0}", method.MethodNameFull);
    }

    /// <summary>
    /// write the Method Name into the log
    /// </summary>
    /// <param name="method"></param>
    static public void LeaveMethod(CallingMethod method)
    {
      log.Debug("<<< {0}", method.MethodNameFull);
    }

    /// <summary>
    /// This function matches the Longet Common Substring of the source string found in target string
    /// </summary>
    /// <param name="sourceString">The Source String to match</param>
    /// <param name="targetString">The Target String to search within</param>
    /// <returns>a match ratio</returns>
    public static double LongestCommonSubstring(string sourceString, string targetString)
    {
      if (String.IsNullOrEmpty(sourceString) || String.IsNullOrEmpty(targetString))
        return 0;

      sourceString = sourceString.ToLower().Replace(",", "").Replace(" ", "").Replace(";", "").Replace("_", "");
      targetString = targetString.ToLower().Replace(",", "").Replace(" ", "").Replace(";", "").Replace("_", "");

      int[,] num = new int[sourceString.Length, targetString.Length];
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
      return (double)maxlen / (double)sourceString.Length;
    }


    /// <summary>
    /// Create a cursor from the given Resource Name
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="xHotSpot"></param>
    /// <param name="yHotSpot"></param>
    /// <returns></returns>
    public static Cursor CreateCursorFromResource(string resourceName, int xHotSpot, int yHotSpot)
    {
      ResourceManager resourceManager = new ResourceManager("MPTagThat.Properties.Resources", Assembly.GetCallingAssembly());
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
    #endregion
  }
}
