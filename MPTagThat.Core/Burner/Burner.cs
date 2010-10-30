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

using System;
using System.Collections.Generic;

#endregion

namespace MPTagThat.Core.Burning
{
  public class Burner : IOpticalDrive
  {
    private readonly char[] trimchars = new[] {'\'', ' ', '\x00'};
    internal string fBusId = "0,0,0";
    internal MediaInfo fCurrentMediaInfo;
    internal string fDeviceName = string.Empty;
    internal string fDeviceVendor = string.Empty;
    internal DriveFeatures fDriveFeatures;
    internal int fDriveSpeed = 2;
    internal bool fHasMedia;
    internal MediaTypeSupport fMediaFeatures;

    public Burner(string aBusId)
    {
      fBusId = aBusId;

      GetFeatures();
    }

    #region getters and setters

    /// <summary>
    ///   gets the type of current burning medium - does always refresh
    /// </summary>
    public MediaType CurrentMedia
    {
      get
      {
        GetCurrentMediaStatus();
        return fCurrentMediaInfo.CurrentMediaType;
      }
    }

    /// <summary>
    ///   holds the drive's reading and writing capabilities
    /// </summary>
    public DriveFeatures DriveFeatures
    {
      get { return fDriveFeatures; }
    }

    /// <summary>
    ///   holds the drive's media support capabilities
    /// </summary>
    public MediaTypeSupport MediaFeatures
    {
      get { return fMediaFeatures; }
    }

    public MediaInfo CurrentMediaInfo
    {
      get
      {
        if (fCurrentMediaInfo == null || fCurrentMediaInfo.CurrentCapacityType == CapacityType.Unknown)
          GetCurrentMediaStatus();
        return fCurrentMediaInfo;
      }
    }

    /// <summary>
    ///   Returns the Supported Drive Speed for the inserted media
    /// </summary>
    public List<string> SupportedDriveSpeed
    {
      get { return ParseSupportedDriveSpeed(); }
    }

    /// <summary>
    ///   gets the SCSI BusId
    /// </summary>
    public string BusId
    {
      get { return fBusId; }
    }

    /// <summary>
    ///   gets the vendor's device identification string
    /// </summary>
    public string DeviceName
    {
      get { return fDeviceName; }
    }

    /// <summary>
    ///   gets the vendor's name of the drive
    /// </summary>
    public string DeviceVendor
    {
      get { return fDeviceVendor; }
    }

    /// <summary>
    ///   gets whether there's a disc present - does always refresh
    /// </summary>
    public bool HasMedia
    {
      get
      {
        GetCurrentMediaStatus();
        return fHasMedia;
      }
    }

    /// <summary>
    ///   Sets / Gets the selected Drive Speed
    /// </summary>
    public int SelectedWriteSpeed
    {
      get { return fDriveSpeed; }
      set { fDriveSpeed = value; }
    }

    #endregion

    #region private methods

    private void GetCurrentMediaStatus()
    {
      List<string> MediaDescription = new List<string>(94);
      MediaInfo MyMediaInfo = new MediaInfo(MediaType.None, false, "Standard", BlankStatus.empty, BlankStatus.empty,
                                            FormatStatus.none, 1, 1, false, 0);
      string cdrParam = string.Format(@"dev={0} -minfo -v", fBusId);

      MediaDescription = DeviceHelper.ExecuteProcReturnStdOut("cdrecord.exe", cdrParam, 40000);
      ParseDescriptionForMediaInfo(MediaDescription, ref MyMediaInfo);
        // 40 sec because an open tray will be closed, etc
    }

    private void ParseDescriptionForMediaInfo(List<string> MediaInfoDescription, ref MediaInfo aMediaInfo)
    {
      lock (this)
      {
        if (MediaInfoDescription.Count > 5)
        {
          for (int i = 0; i < MediaInfoDescription.Count; i++)
          {
            string checkStr = MediaInfoDescription[i];

            if (checkStr.Contains(@"Mounted media type"))
            {
              // Mounted media type:       DVD+R/DL
              // Using generic SCSI-3/mmc-3 DVD+RW driver (mmc_dvdplusrw).
              int checkPos = checkStr.IndexOf(@":");
              if (checkPos >= 0)
              {
                // Assume a disc is there (cdr will close the tray) - if not present it will be set later
                fHasMedia = true;
                string mounted = (checkStr.Substring(checkPos + 1)).Trim();
                checkPos = mounted.IndexOf(@" ");
                if (checkPos >= 0)
                  mounted = (mounted.Remove(checkPos));

                switch (mounted)
                {
                  case "DVD+R/DL":
                    aMediaInfo.CurrentMediaType = MediaType.DlDVDplusR;
                    break;
                  case "DVD-R/DL":
                    aMediaInfo.CurrentMediaType = MediaType.DlDVDminusR;
                    break;
                  case "DVD+RW":
                    aMediaInfo.CurrentMediaType = MediaType.DVDplusRW;
                    break;
                  case "DVD+R":
                    aMediaInfo.CurrentMediaType = MediaType.DVDplusR;
                    break;
                  case "DVD-RW":
                    aMediaInfo.CurrentMediaType = MediaType.DVDminusRW;
                    break;
                  case "DVD-R":
                    aMediaInfo.CurrentMediaType = MediaType.DVDminusR;
                    break;
                  case "DVD-RAM":
                    aMediaInfo.CurrentMediaType = MediaType.DVDRam;
                    break;
                  case "DVD-ROM":
                    aMediaInfo.CurrentMediaType = MediaType.ReadOnly;
                    break;
                  case "CD-RW":
                    aMediaInfo.CurrentMediaType = MediaType.CDRW;
                    break;
                  case "CD-R":
                    aMediaInfo.CurrentMediaType = MediaType.CDR;
                    break;
                  case "CD-ROM":
                    aMediaInfo.CurrentMediaType = MediaType.ReadOnly;
                    break;

                  default:
                    ServiceScope.Get<ILogger>().GetLogger.Debug("Burner: Could not recognize media type: {0}", mounted);
                    aMediaInfo.CurrentMediaType = MediaType.None;
                    fHasMedia = false;
                    break;
                }
              }
              else
                aMediaInfo.CurrentMediaType = MediaType.None;
            }
            else if (checkStr.Contains(@"Disk Is erasable"))
            {
              aMediaInfo.IsErasable = true;
              // cdrecord will use the mmc_cdr driver for CDRW as well
              if (aMediaInfo.CurrentMediaType == MediaType.CDR)
                aMediaInfo.CurrentMediaType = MediaType.CDRW;
            }
            else if (checkStr.Contains(@"data type:"))
            {
              // data type:                standard
              string dataType = checkStr.Substring(11).Trim(trimchars);
              aMediaInfo.DataType = dataType;
            }
            else if (checkStr.Contains(@"disk status:"))
            {
              // disk status:              empty
              string diskStatus = checkStr.Substring(13).Trim(trimchars);
              if (diskStatus != "empty")
                aMediaInfo.DiskStatus = BlankStatus.complete;
            }
            else if (checkStr.Contains(@"session status:"))
            {
              // session status:           empty
              string sessionStatus = checkStr.Substring(16).Trim(trimchars);
              if (sessionStatus != "empty")
                aMediaInfo.SessionStatus = BlankStatus.complete;
            }
            else if (checkStr.Contains(@"Disk Is not unrestricted"))
              aMediaInfo.IsRestricted = true; // false reports here?
            else
            {
              //     Capacity  Blklen/Sparesz.  Format-type  Type
              //       352965             2048         0x00  Formatted Media 
              if (checkStr.Contains(@"Capacity  Blklen/Sparesz."))
              {
                string capacity = MediaInfoDescription[i + 1].Trim().Substring(0,
                                                                               MediaInfoDescription[i + 1].Trim().
                                                                                 IndexOf(" "));
                aMediaInfo.Size = Convert.ToInt32(capacity);
              }
            }
          }
        }
        else // Less than 5 lines of info? Tray must be empty
        {
          fHasMedia = false;
          aMediaInfo.CurrentMediaType = MediaType.None;
        }

        fCurrentMediaInfo = aMediaInfo;
      }
    }

    /// <summary>
    ///   Gathers the devices capabilities
    /// </summary>
    private void GetFeatures()
    {
      List<string> FeatureDescription = new List<string>(85);
      string cdrParam = string.Format(@"dev={0} -prcap -v", fBusId);

      FeatureDescription = DeviceHelper.ExecuteProcReturnStdOut("cdrecord.exe", cdrParam, 20000);
      ParseDescriptionForFeatures(FeatureDescription);
    }

    // ToDo: sort this out..
    private void ParseDescriptionForFeatures(List<string> FeatureDescription)
    {
      lock (this)
      {
        DriveFeatures currentFeatures = new DriveFeatures(false, false, false, false, false, false, false, false, false,
                                                          false, false, false, string.Empty, string.Empty);
        MediaTypeSupport currentProfile = new MediaTypeSupport(false, false, false, false, false, false, false, false,
                                                               false, false);

        for (int i = 0; i < FeatureDescription.Count; i++)
        {
          string checkStr = FeatureDescription[i];

          if (checkStr.Contains(@"Does read CD-R media"))
            currentFeatures.ReadsCDR = true;
          else if (checkStr.Contains(@"Does write CD-R media"))
            currentFeatures.WriteCDR = true;
          else if (checkStr.Contains(@"Does read CD-RW media"))
            currentFeatures.ReadsCDRW = true;
          else if (checkStr.Contains(@"Does write CD-RW"))
            currentFeatures.WriteCDRW = true;
          else if (checkStr.Contains(@"Does read DVD-ROM"))
            currentFeatures.ReadsDVDRom = true;
          else if (checkStr.Contains(@"Does read DVD-R"))
            currentFeatures.ReadsDVDR = true;
          else if (checkStr.Contains(@"Does write DVD-R"))
            currentFeatures.WriteDVDR = true;
          else if (checkStr.Contains(@"Does read DVD-RAM"))
            currentFeatures.ReadsDVDRam = true;
          else if (checkStr.Contains(@"Does write DVD-RAM"))
            currentFeatures.WriteDVDRam = true;
          else if (checkStr.Contains(@"Does support Buffer-Underrun-Free recording"))
            currentFeatures.SupportsBurnFree = true;
          else if (checkStr.Contains(@"Does support test writing"))
            currentFeatures.AllowsDummyWrite = true;
          else if (checkStr.Contains(@"Maximum read"))
            currentFeatures.MaxReadSpeed = checkStr.Substring(23).Trim(trimchars);
          else if (checkStr.Contains(@"Maximum write"))
            currentFeatures.MaxWriteSpeed = checkStr.Substring(23).Trim(trimchars);
          else if (checkStr.Contains(@"Vendor_info"))
            fDeviceVendor = checkStr.Substring(16).Trim(trimchars);
          else if (checkStr.Contains(@"Identifikation : "))
            fDeviceName = checkStr.Substring(16).Trim(trimchars);
          else if (checkStr.Contains(@" DVD+R/DL"))
            currentProfile.WriteDlDVDplusR = true;
          else if (checkStr.Contains(@" DVD+RW"))
            currentProfile.WriteDVDplusRW = true;
          else if (checkStr.Contains(@" DVD+R"))
            currentProfile.WriteDVDplusR = true;
          else if (checkStr.Contains(@" DVD-RW"))
            currentProfile.WriteDVDminusRW = true;
          else if (checkStr.Contains(@" DVD-R"))
            currentProfile.WriteDVDminusR = true;
          else if (checkStr.Contains(@" CD-RW"))
            currentProfile.WriteCDRW = true;
          else if (checkStr.Contains(@" CD-R"))
            currentProfile.WriteCDR = true;
          else if (checkStr.Contains(@"BD-ROM"))
            currentFeatures.ReadsBRRom = true;
        }

        fDriveFeatures = currentFeatures;
        fMediaFeatures = currentProfile;
      }
    }

    private List<string> ParseSupportedDriveSpeed()
    {
      List<string> FeatureDescription = new List<string>();
      string cdrParam = string.Format(@"dev={0} -prcap -v", fBusId);

      FeatureDescription = DeviceHelper.ExecuteProcReturnStdOut("cdrecord.exe", cdrParam, 20000);

      List<string> speed = new List<string>();
      string savedSpeed = string.Empty;
      for (int i = 0; i < FeatureDescription.Count; i++)
      {
        string checkStr = FeatureDescription[i];
        if (checkStr.Contains(@"Write speed #"))
        {
          int start = checkStr.IndexOf("(CD");
          string s = checkStr.Substring(start + 4, checkStr.IndexOf(",", start) - start - 4).Trim();
          if (s != savedSpeed)
          {
            savedSpeed = s;
            speed.Add(s);
          }
        }
      }
      return speed;
    }

    #endregion
  }
}