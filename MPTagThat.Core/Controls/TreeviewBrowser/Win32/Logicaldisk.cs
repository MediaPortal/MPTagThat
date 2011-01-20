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
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Management;

#endregion

namespace ROOT.CIMV2.Win32
{
  // Die Funktionen 'ShouldSerialize<PropertyName>' werden vom VS-Eigenschaftenbrowser verwendet, um zu überprüfen, ob eine Eigenschaft serialisiert werden muss. Die Funktionen werden für alle ValueType-Eigenschaften (Eigenschaften des Typs Int32, BOOL usw.) hinzugefügt, die nicht auf NULL festgelegt werden können. Die Funktionen verwenden die Is<PropertyName>Null-Funktion. Die Funktionen werden auch in der TypeConverter-Implementierung für die Eigenschaften verwendet, um den NULL-Wert zu überprüfen, damit für einen Drag & Drop-Vorgang in Visual Studio ein leerer Wert im Eigenschaftenbrowser angezeigt werden kann.
  // Die Funktionen 'Is<PropertyName>Null()' werden verwendet, um zu überprüfen, ob eine Eigenschaft NULL ist.
  // Die Funktionen 'Reset<PropertyName>' werden für Read/Write-Eigenschaften hinzugefügt. Die Funktionen werden vom VS-Designer im Eigenschaftenbrowser verwendet, um eine Eigenschaft auf NULL zusetzen.
  // Für jede Eigenschaft, die zur Klasse der WMI-Eigenschaft hinzugefügt wird, sind Attribute festgelegt, um das Verhalten im Visual Studio-Designer und auch 'TypeConverter' zu definieren.
  // Die Funktionen 'ToDateTime' und 'ToDmtfDateTime' zum Konvertieren von Datum bzw. Uhrzeit werden zu der Klasse hinzugefügt, um das DMTF-Datum in 'System.DateTime' und umgekehrt zu konvertieren.
  // Eine für die WMI-Klasse generierte EarlyBound-Klasse.Win32_Logicaldisk
  public class Logicaldisk : Component
  {
    // Private Eigenschaft, die den WMI-Namespace enthält, in dem sich diese Klasse befindet.

    #region AccessValues enum

    public enum AccessValues
    {
      Unknown,

      Readable,

      Writeable,

      Read_Write_Supported,

      Write_Once,
    }

    #endregion

    #region AvailabilityValues enum

    public enum AvailabilityValues
    {
      Other = 1,

      Unknown = 2,

      Running_Full_Power = 3,

      Warning = 4,

      In_Test = 5,

      Not_Applicable = 6,

      Power_Off = 7,

      Off_Line = 8,

      Off_Duty = 9,

      Degraded = 10,

      Not_Installed = 11,

      Install_Error = 12,

      Power_Save_Unknown = 13,

      Power_Save_Low_Power_Mode = 14,

      Power_Save_Standby = 15,

      Power_Cycle = 16,

      Power_Save_Warning = 17,

      Paused = 18,

      Not_Ready = 19,

      Not_Configured = 20,

      Quiesced = 21,

      INVALID_ENUM_VALUE = 0,
    }

    #endregion

    #region ConfigManagerErrorCodeValues enum

    public enum ConfigManagerErrorCodeValues : long
    {
      This_device_is_working_properly_ = 0,

      This_device_is_not_configured_correctly_ = 1,

      Windows_cannot_load_the_driver_for_this_device_ = 2,

      The_driver_for_this_device_might_be_corrupted_or_your_system_may_be_running_low_on_memory_or_other_resources_ = 3,

      This_device_is_not_working_properly_One_of_its_drivers_or_your_registry_might_be_corrupted_ = 4,

      The_driver_for_this_device_needs_a_resource_that_Windows_cannot_manage_ = 5,

      The_boot_configuration_for_this_device_conflicts_with_other_devices_ = 6,

      Cannot_filter_ = 7,

      The_driver_loader_for_the_device_is_missing_ = 8,

      This_device_is_not_working_properly_because_the_controlling_firmware_is_reporting_the_resources_for_the_device_incorrectly_
        = 9,

      This_device_cannot_start_ = 10,

      This_device_failed_ = 11,

      This_device_cannot_find_enough_free_resources_that_it_can_use_ = 12,

      Windows_cannot_verify_this_device_s_resources_ = 13,

      This_device_cannot_work_properly_until_you_restart_your_computer_ = 14,

      This_device_is_not_working_properly_because_there_is_probably_a_re_enumeration_problem_ = 15,

      Windows_cannot_identify_all_the_resources_this_device_uses_ = 16,

      This_device_is_asking_for_an_unknown_resource_type_ = 17,

      Reinstall_the_drivers_for_this_device_ = 18,

      Failure_using_the_VxD_loader_ = 19,

      Your_registry_might_be_corrupted_ = 20,

      System_failure_Try_changing_the_driver_for_this_device_If_that_does_not_work_see_your_hardware_documentation_Windows_is_removing_this_device_
        = 21,

      This_device_is_disabled_ = 22,

      System_failure_Try_changing_the_driver_for_this_device_If_that_doesn_t_work_see_your_hardware_documentation_ = 23,

      This_device_is_not_present_is_not_working_properly_or_does_not_have_all_its_drivers_installed_ = 24,

      Windows_is_still_setting_up_this_device_ = 25,

      Windows_is_still_setting_up_this_device_0 = 26,

      This_device_does_not_have_valid_log_configuration_ = 27,

      The_drivers_for_this_device_are_not_installed_ = 28,

      This_device_is_disabled_because_the_firmware_of_the_device_did_not_give_it_the_required_resources_ = 29,

      This_device_is_using_an_Interrupt_Request_IRQ_resource_that_another_device_is_using_ = 30,

      This_device_is_not_working_properly_because_Windows_cannot_load_the_drivers_required_for_this_device_ = 31,
    }

    #endregion

    #region DriveTypeValues enum

    public enum DriveTypeValues
    {
      Unknown,

      No_Root_Directory,

      Removable_Disk,

      Local_Disk,

      Network_Drive,

      Compact_Disc,

      RAM_Disk,
    }

    #endregion

    #region MediaTypeValues enum

    public enum MediaTypeValues
    {
      Format_is_unknown,

      Val_5_Inch_Floppy_Disk,

      Val_3_Inch_Floppy_Disk,

      Val_3_Inch_Floppy_Disk0,

      Val_3_Inch_Floppy_Disk1,

      Val_3_Inch_Floppy_Disk2,

      Val_5_Inch_Floppy_Disk0,

      Val_5_Inch_Floppy_Disk1,

      Val_5_Inch_Floppy_Disk2,

      Val_5_Inch_Floppy_Disk3,

      Val_5_Inch_Floppy_Disk4,

      Removable_media_other_than_floppy,

      Fixed_hard_disk_media,

      Val_3_Inch_Floppy_Disk3,

      Val_3_Inch_Floppy_Disk4,

      Val_5_Inch_Floppy_Disk5,

      Val_5_Inch_Floppy_Disk6,

      Val_3_Inch_Floppy_Disk5,

      Val_3_Inch_Floppy_Disk6,

      Val_5_Inch_Floppy_Disk7,

      Val_3_Inch_Floppy_Disk7,

      Val_3_Inch_Floppy_Disk8,

      Val_8_Inch_Floppy_Disk,
    }

    #endregion

    #region PowerManagementCapabilitiesValues enum

    public enum PowerManagementCapabilitiesValues
    {
      Unknown,

      Not_Supported,

      Disabled,

      Enabled,

      Power_Saving_Modes_Entered_Automatically,

      Power_State_Settable,

      Power_Cycling_Supported,

      Timed_Power_On_Supported,
    }

    #endregion

    #region StatusInfoValues enum

    public enum StatusInfoValues
    {
      Other = 1,

      Unknown = 2,

      Enabled = 3,

      Disabled = 4,

      Not_Applicable = 5,

      INVALID_ENUM_VALUE = 0,
    }

    #endregion

    private static string CreatedWmiNamespace = "root\\cimv2";

    // Private Eigenschaft, die den Namen der WMI-Klasse enthält, die diese Klasse erstellt hat.
    private static string CreatedClassName = "Win32_Logicaldisk";

    // Private Membervariable, die 'ManagementScope' enthält, das von den verschiedenen Methoden verwendet wird.
    private static ManagementScope statMgmtScope;

    // Das darunterliegende LateBound WMI-Objekt.
    private readonly ManagementObject PrivateLateBoundObject;
    private readonly ManagementSystemProperties PrivateSystemProperties;

    // Membervariable, in der das 'automatic commit'-Verhalten für die Klasse gespeichert wird.

    // Das aktuelle WMI-Objekt
    private readonly ManagementBaseObject curObj;
    private readonly ManagementBaseObject embeddedObj;

    // Flag zum Anzeigen, ob die Instanz ein eingebettetes Objekt ist.
    private readonly bool isEmbedded;
    private bool AutoCommitProp = true;

    // Es sind unterschiedliche Konstruktorüberladungen aufgeführt, um die Instanz der Klasse mit einem WMI-Objekt zu initialisieren.
    public Logicaldisk() :
      this(((null)), ((null)), ((null))) {}

    public Logicaldisk(string keyDeviceID) :
      this(((null)), ((new ManagementPath(ConstructPath(keyDeviceID)))), ((null))) {}

    public Logicaldisk(ManagementScope mgmtScope, string keyDeviceID) :
      this(((mgmtScope)), ((new ManagementPath(ConstructPath(keyDeviceID)))), ((null))) {}

    public Logicaldisk(ManagementPath path, ObjectGetOptions getOptions) :
      this(((null)), ((path)), ((getOptions))) {}

    public Logicaldisk(ManagementScope mgmtScope, ManagementPath path) :
      this(((mgmtScope)), ((path)), ((null))) {}

    public Logicaldisk(ManagementPath path) :
      this(((null)), ((path)), ((null))) {}

    public Logicaldisk(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions)
    {
      if ((path != null))
      {
        if ((CheckIfProperClass(mgmtScope, path, getOptions) != true))
        {
          throw new ArgumentException("Klassenname stimmt nicht überein.");
        }
      }
      PrivateLateBoundObject = new ManagementObject(mgmtScope, path, getOptions);
      PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
      curObj = PrivateLateBoundObject;
    }

    public Logicaldisk(ManagementObject theObject)
    {
      if (CheckIfProperClass(theObject))
      {
        PrivateLateBoundObject = theObject;
        PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
        curObj = PrivateLateBoundObject;
      }
      else
      {
        throw new ArgumentException("Klassenname stimmt nicht überein.");
      }
    }

    public Logicaldisk(ManagementBaseObject theObject)
    {
      if (CheckIfProperClass(theObject))
      {
        embeddedObj = theObject;
        PrivateSystemProperties = new ManagementSystemProperties(theObject);
        curObj = embeddedObj;
        isEmbedded = true;
      }
      else
      {
        throw new ArgumentException("Klassenname stimmt nicht überein.");
      }
    }

    // Die Eigenschaft gibt den Namespace der WMI-Klasse zurück.
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string OriginatingNamespace
    {
      get { return "root\\cimv2"; }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ManagementClassName
    {
      get
      {
        string strRet = CreatedClassName;
        if ((curObj != null))
        {
          if ((curObj.ClassPath != null))
          {
            strRet = ((string)(curObj["__CLASS"]));
            if (((strRet == null)
                 || (strRet == String.Empty)))
            {
              strRet = CreatedClassName;
            }
          }
        }
        return strRet;
      }
    }

    // Eigenschaft, die auf ein eingebettetes Objekt zeigt, um die Systemeigenschaften des WMI-Objekts abzurufen.
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ManagementSystemProperties SystemProperties
    {
      get { return PrivateSystemProperties; }
    }

    // Die Eigenschaft, die das darunterliegende LateBound-Objekt zurückgibt.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ManagementBaseObject LateBoundObject
    {
      get { return curObj; }
    }

    // ManagementScope des Objekts.
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ManagementScope Scope
    {
      get
      {
        if ((isEmbedded == false))
        {
          return PrivateLateBoundObject.Scope;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if ((isEmbedded == false))
        {
          PrivateLateBoundObject.Scope = value;
        }
      }
    }

    // Eigenschaften zum Anzeigen des commit-Verhaltens des WMI-Objekts. Wenn die Eigenschaft 'true' ist, wird das WMI-Objekt automatisch nach jeder Eigenschaftsänderung gespeichert (d.h. nach der Änderung einer Eigenschaft wird 'Put()' aufgerufen).
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AutoCommit
    {
      get { return AutoCommitProp; }
      set { AutoCommitProp = value; }
    }

    // ManagementPath des darunterliegenden WMI-Objekts.
    [Browsable(true)]
    public ManagementPath Path
    {
      get
      {
        if ((isEmbedded == false))
        {
          return PrivateLateBoundObject.Path;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if ((isEmbedded == false))
        {
          if ((CheckIfProperClass(null, value, null) != true))
          {
            throw new ArgumentException("Klassenname stimmt nicht überein.");
          }
          PrivateLateBoundObject.Path = value;
        }
      }
    }

    // Eigenschaft für einen öffentlichen statischen Bereich, die von den verschiedenen Methoden verwendet wird.
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static ManagementScope StaticScope
    {
      get { return statMgmtScope; }
      set { statMgmtScope = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsAccessNull
    {
      get
      {
        if ((curObj["Access"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("Access describes whether the media is readable (value=1), writeable (value=2), or" +
                 " both (value=3). \"Unknown\" (0) and \"Write Once\" (4) can also be defined.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public AccessValues Access
    {
      get
      {
        if ((curObj["Access"] == null))
        {
          return ((AccessValues)(Convert.ToInt32(0)));
        }
        return ((AccessValues)(Convert.ToInt32(curObj["Access"])));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsAvailabilityNull
    {
      get
      {
        if ((curObj["Availability"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"The availability and status of the device.  For example, the Availability property indicates that the device is running and has full power (value=3), or is in a warning (4), test (5), degraded (10) or power save state (values 13-15 and 17). Regarding the power saving states, these are defined as follows: Value 13 (""Power Save - Unknown"") indicates that the device is known to be in a power save mode, but its exact status in this mode is unknown; 14 (""Power Save - Low Power Mode"") indicates that the device is in a power save state but still functioning, and may exhibit degraded performance; 15 (""Power Save - Standby"") describes that the device is not functioning but could be brought to full power 'quickly'; and value 17 (""Power Save - Warning"") indicates that the device is in a warning state, though also in a power save mode."
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public AvailabilityValues Availability
    {
      get
      {
        if ((curObj["Availability"] == null))
        {
          return ((AvailabilityValues)(Convert.ToInt32(0)));
        }
        return ((AvailabilityValues)(Convert.ToInt32(curObj["Availability"])));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsBlockSizeNull
    {
      get
      {
        if ((curObj["BlockSize"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"Size in bytes of the blocks which form this StorageExtent. If variable block size, then the maximum block size in bytes should be specified. If the block size is unknown or if a block concept is not valid (for example, for Aggregate Extents, Memory or LogicalDisks), enter a 1."
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public UInt64 BlockSize
    {
      get
      {
        if ((curObj["BlockSize"] == null))
        {
          return Convert.ToUInt64(0);
        }
        return ((UInt64)(curObj["BlockSize"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The Caption property is a short textual description (one-line string) of the obje" +
                 "ct.")]
    public string Caption
    {
      get { return ((string)(curObj["Caption"])); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsCompressedNull
    {
      get
      {
        if ((curObj["Compressed"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The Compressed property indicates whether the logical volume exists as a single c" +
                 "ompressed entity, such as a DoubleSpace volume.  If file based compression is su" +
                 "pported (such as on NTFS), this property will be FALSE.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool Compressed
    {
      get
      {
        if ((curObj["Compressed"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["Compressed"]));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsConfigManagerErrorCodeNull
    {
      get
      {
        if ((curObj["ConfigManagerErrorCode"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("Indicates the Win32 Configuration Manager error code.  The following values may b" +
                 "e returned: \n0\tThis device is working properly. \n1\tThis device is not configured" +
                 " correctly. \n2\tWindows cannot load the driver for this device. \n3\tThe driver for" +
                 " this device might be corrupted, or your system may be running low on memory or " +
                 "other resources. \n4\tThis device is not working properly. One of its drivers or y" +
                 "our registry might be corrupted. \n5\tThe driver for this device needs a resource " +
                 "that Windows cannot manage. \n6\tThe boot configuration for this device conflicts " +
                 "with other devices. \n7\tCannot filter. \n8\tThe driver loader for the device is mis" +
                 "sing. \n9\tThis device is not working properly because the controlling firmware is" +
                 " reporting the resources for the device incorrectly. \n10\tThis device cannot star" +
                 "t. \n11\tThis device failed. \n12\tThis device cannot find enough free resources tha" +
                 "t it can use. \n13\tWindows cannot verify this device\'s resources. \n14\tThis device" +
                 " cannot work properly until you restart your computer. \n15\tThis device is not wo" +
                 "rking properly because there is probably a re-enumeration problem. \n16\tWindows c" +
                 "annot identify all the resources this device uses. \n17\tThis device is asking for" +
                 " an unknown resource type. \n18\tReinstall the drivers for this device. \n19\tYour r" +
                 "egistry might be corrupted. \n20\tFailure using the VxD loader. \n21\tSystem failure" +
                 ": Try changing the driver for this device. If that does not work, see your hardw" +
                 "are documentation. Windows is removing this device. \n22\tThis device is disabled." +
                 " \n23\tSystem failure: Try changing the driver for this device. If that doesn\'t wo" +
                 "rk, see your hardware documentation. \n24\tThis device is not present, is not work" +
                 "ing properly, or does not have all its drivers installed. \n25\tWindows is still s" +
                 "etting up this device. \n26\tWindows is still setting up this device. \n27\tThis dev" +
                 "ice does not have valid log configuration. \n28\tThe drivers for this device are n" +
                 "ot installed. \n29\tThis device is disabled because the firmware of the device did" +
                 " not give it the required resources. \n30\tThis device is using an Interrupt Reque" +
                 "st (IRQ) resource that another device is using. \n31\tThis device is not working p" +
                 "roperly because Windows cannot load the drivers required for this device.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public ConfigManagerErrorCodeValues ConfigManagerErrorCode
    {
      get
      {
        if ((curObj["ConfigManagerErrorCode"] == null))
        {
          return ((ConfigManagerErrorCodeValues)(Convert.ToInt64(0)));
        }
        return ((ConfigManagerErrorCodeValues)(Convert.ToInt64(curObj["ConfigManagerErrorCode"])));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsConfigManagerUserConfigNull
    {
      get
      {
        if ((curObj["ConfigManagerUserConfig"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("Indicates whether the device is using a user-defined configuration.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool ConfigManagerUserConfig
    {
      get
      {
        if ((curObj["ConfigManagerUserConfig"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["ConfigManagerUserConfig"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("CreationClassName indicates the name of the class or the subclass used in the cre" +
                 "ation of an instance. When used with the other key properties of this class, thi" +
                 "s property allows all instances of this class and its subclasses to be uniquely " +
                 "identified.")]
    public string CreationClassName
    {
      get { return ((string)(curObj["CreationClassName"])); }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The Description property provides a textual description of the object. ")]
    public string Description
    {
      get { return ((string)(curObj["Description"])); }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The DeviceID property contains a string uniquely identifying the logical disk fro" +
                 "m other devices on the system.")]
    public string DeviceID
    {
      get { return ((string)(curObj["DeviceID"])); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsDriveTypeNull
    {
      get
      {
        if ((curObj["DriveType"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The DriveType property contains a numeric value corresponding to the type of disk" +
                 " drive this logical disk represents.  Please refer to the Platform SDK documenta" +
                 "tion for additional values.\nExample: A CD-ROM drive would return 5.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public DriveTypeValues DriveType
    {
      get
      {
        if ((curObj["DriveType"] == null))
        {
          return ((DriveTypeValues)(Convert.ToInt32(0)));
        }
        return ((DriveTypeValues)(Convert.ToInt32(curObj["DriveType"])));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsErrorClearedNull
    {
      get
      {
        if ((curObj["ErrorCleared"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("ErrorCleared is a boolean property indicating that the error reported in LastErro" +
                 "rCode property is now cleared.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool ErrorCleared
    {
      get
      {
        if ((curObj["ErrorCleared"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["ErrorCleared"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("ErrorDescription is a free-form string supplying more information about the error" +
                 " recorded in LastErrorCode property, and information on any corrective actions t" +
                 "hat may be taken.")]
    public string ErrorDescription
    {
      get { return ((string)(curObj["ErrorDescription"])); }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("ErrorMethodology is a free-form string describing the type of error detection and" +
                 " correction supported by this storage extent.")]
    public string ErrorMethodology
    {
      get { return ((string)(curObj["ErrorMethodology"])); }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The FileSystem property indicates the file system on the logical disk.\nExample: N" +
                 "TFS")]
    public string FileSystem
    {
      get { return ((string)(curObj["FileSystem"])); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsFreeSpaceNull
    {
      get
      {
        if ((curObj["FreeSpace"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The FreeSpace property indicates in bytes how much free space is available on the" +
                 " logical disk.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public UInt64 FreeSpace
    {
      get
      {
        if ((curObj["FreeSpace"] == null))
        {
          return Convert.ToUInt64(0);
        }
        return ((UInt64)(curObj["FreeSpace"]));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsInstallDateNull
    {
      get
      {
        if ((curObj["InstallDate"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The InstallDate property is datetime value indicating when the object was install" +
                 "ed. A lack of a value does not indicate that the object is not installed.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public DateTime InstallDate
    {
      get
      {
        if ((curObj["InstallDate"] != null))
        {
          return ToDateTime(((string)(curObj["InstallDate"])));
        }
        else
        {
          return DateTime.MinValue;
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsLastErrorCodeNull
    {
      get
      {
        if ((curObj["LastErrorCode"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("LastErrorCode captures the last error code reported by the logical device.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public UInt32 LastErrorCode
    {
      get
      {
        if ((curObj["LastErrorCode"] == null))
        {
          return Convert.ToUInt32(0);
        }
        return ((UInt32)(curObj["LastErrorCode"]));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsMaximumComponentLengthNull
    {
      get
      {
        if ((curObj["MaximumComponentLength"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"The MaximumComponentLength property contains the maximum length of a filename component supported by the Win32 drive. A filename component is that portion of a filename between backslashes.  The value can be used to indicate that long names are supported by the specified file system. For example, for a FAT file system supporting long names, the function stores the value 255, rather than the previous 8.3 indicator. Long names can also be supported on systems that use the NTFS file system.
Example: 255"
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public UInt32 MaximumComponentLength
    {
      get
      {
        if ((curObj["MaximumComponentLength"] == null))
        {
          return Convert.ToUInt32(0);
        }
        return ((UInt32)(curObj["MaximumComponentLength"]));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsMediaTypeNull
    {
      get
      {
        if ((curObj["MediaType"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"The MediaType property indicates the type of media currently present in the logical drive. This value will be one of the values of the MEDIA_TYPE enumeration defined in winioctl.h.
<B>Note:</B> The value may not be exact for removable drives if currently there is no media in the drive."
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public MediaTypeValues MediaType
    {
      get
      {
        if ((curObj["MediaType"] == null))
        {
          return ((MediaTypeValues)(Convert.ToInt32(0)));
        }
        return ((MediaTypeValues)(Convert.ToInt32(curObj["MediaType"])));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The Name property defines the label by which the object is known. When subclassed" +
                 ", the Name property can be overridden to be a Key property.")]
    public string Name
    {
      get { return ((string)(curObj["Name"])); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsNumberOfBlocksNull
    {
      get
      {
        if ((curObj["NumberOfBlocks"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"Total number of consecutive blocks, each block the size of the value contained in the BlockSize property, which form this storage extent. Total size of the storage extent can be calculated by multiplying the value of the BlockSize property by the value of this property. If the value of BlockSize is 1, this property is the total size of the storage extent."
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public UInt64 NumberOfBlocks
    {
      get
      {
        if ((curObj["NumberOfBlocks"] == null))
        {
          return Convert.ToUInt64(0);
        }
        return ((UInt64)(curObj["NumberOfBlocks"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("Indicates the Win32 Plug and Play device ID of the logical device.  Example: *PNP" +
                 "030b")]
    public string PNPDeviceID
    {
      get { return ((string)(curObj["PNPDeviceID"])); }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"Indicates the specific power-related capabilities of the logical device. The array values, 0=""Unknown"", 1=""Not Supported"" and 2=""Disabled"" are self-explanatory. The value, 3=""Enabled"" indicates that the power management features are currently enabled but the exact feature set is unknown or the information is unavailable. ""Power Saving Modes Entered Automatically"" (4) describes that a device can change its power state based on usage or other criteria. ""Power State Settable"" (5) indicates that the SetPowerState method is supported. ""Power Cycling Supported"" (6) indicates that the SetPowerState method can be invoked with the PowerState input variable set to 5 (""Power Cycle""). ""Timed Power On Supported"" (7) indicates that the SetPowerState method can be invoked with the PowerState input variable set to 5 (""Power Cycle"") and the Time parameter set to a specific date and time, or interval, for power-on."
      )]
    public PowerManagementCapabilitiesValues[] PowerManagementCapabilities
    {
      get
      {
        Array arrEnumVals = ((Array)(curObj["PowerManagementCapabilities"]));
        PowerManagementCapabilitiesValues[] enumToRet = new PowerManagementCapabilitiesValues[arrEnumVals.Length];
        Int32 counter = 0;
        for (counter = 0; (counter < arrEnumVals.Length); counter = (counter + 1))
        {
          enumToRet[counter] = ((PowerManagementCapabilitiesValues)(Convert.ToInt32(arrEnumVals.GetValue(counter))));
        }
        return enumToRet;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsPowerManagementSupportedNull
    {
      get
      {
        if ((curObj["PowerManagementSupported"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"Boolean indicating that the Device can be power managed - ie, put into a power save state. This boolean does not indicate that power management features are currently enabled, or if enabled, what features are supported. Refer to the PowerManagementCapabilities array for this information. If this boolean is false, the integer value 1, for the string, ""Not Supported"", should be the only entry in the PowerManagementCapabilities array."
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool PowerManagementSupported
    {
      get
      {
        if ((curObj["PowerManagementSupported"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["PowerManagementSupported"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The ProviderName property indicates the network path name to the logical device.")]
    public string ProviderName
    {
      get { return ((string)(curObj["ProviderName"])); }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("A free form string describing the media and/or its use.")]
    public string Purpose
    {
      get { return ((string)(curObj["Purpose"])); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsQuotasDisabledNull
    {
      get
      {
        if ((curObj["QuotasDisabled"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The QuotasDisabled property indicates that Quota management is not enabled on thi" +
                 "s volume.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool QuotasDisabled
    {
      get
      {
        if ((curObj["QuotasDisabled"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["QuotasDisabled"]));
      }
      set
      {
        curObj["QuotasDisabled"] = value;
        if (((isEmbedded == false)
             && AutoCommitProp))
        {
          PrivateLateBoundObject.Put();
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsQuotasIncompleteNull
    {
      get
      {
        if ((curObj["QuotasIncomplete"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The QuotasIncomplete property indicates that Quota management was used but has be" +
                 "en disabled.  Incomplete refers to the information left in the file system  afte" +
                 "r quota management has been disabled.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool QuotasIncomplete
    {
      get
      {
        if ((curObj["QuotasIncomplete"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["QuotasIncomplete"]));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsQuotasRebuildingNull
    {
      get
      {
        if ((curObj["QuotasRebuilding"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The QuotasRebuilding property indicates an active state signifying that the file " +
                 "system is in process of compiling information and setting the disk up for quota " +
                 "management.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool QuotasRebuilding
    {
      get
      {
        if ((curObj["QuotasRebuilding"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["QuotasRebuilding"]));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsSizeNull
    {
      get
      {
        if ((curObj["Size"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The Size property indicates in bytes, the size of the logical disk.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public UInt64 Size
    {
      get
      {
        if ((curObj["Size"] == null))
        {
          return Convert.ToUInt64(0);
        }
        return ((UInt64)(curObj["Size"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"The Status property is a string indicating the current status of the object. Various operational and non-operational statuses can be defined. Operational statuses are ""OK"", ""Degraded"" and ""Pred Fail"". ""Pred Fail"" indicates that an element may be functioning properly but predicting a failure in the near future. An example is a SMART-enabled hard drive. Non-operational statuses can also be specified. These are ""Error"", ""Starting"", ""Stopping"" and ""Service"". The latter, ""Service"", could apply during mirror-resilvering of a disk, reload of a user permissions list, or other administrative work. Not all such work is on-line, yet the managed element is neither ""OK"" nor in one of the other states."
      )]
    public string Status
    {
      get { return ((string)(curObj["Status"])); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsStatusInfoNull
    {
      get
      {
        if ((curObj["StatusInfo"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("StatusInfo is a string indicating whether the logical device is in an enabled (va" +
                 "lue = 3), disabled (value = 4) or some other (1) or unknown (2) state. If this p" +
                 "roperty does not apply to the logical device, the value, 5 (\"Not Applicable\"), s" +
                 "hould be used.")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public StatusInfoValues StatusInfo
    {
      get
      {
        if ((curObj["StatusInfo"] == null))
        {
          return ((StatusInfoValues)(Convert.ToInt32(0)));
        }
        return ((StatusInfoValues)(Convert.ToInt32(curObj["StatusInfo"])));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsSupportsDiskQuotasNull
    {
      get
      {
        if ((curObj["SupportsDiskQuotas"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The SupportsDiskQuotas property indicates whether this volume supports disk Quota" +
                 "s")]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool SupportsDiskQuotas
    {
      get
      {
        if ((curObj["SupportsDiskQuotas"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["SupportsDiskQuotas"]));
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsSupportsFileBasedCompressionNull
    {
      get
      {
        if ((curObj["SupportsFileBasedCompression"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"The SupportsFileBasedCompression property indicates whether the logical disk partition supports file based compression, such as is the case with NTFS. This property is FALSE, when the Compressed property is TRUE.
Values: TRUE or FALSE. If TRUE, the logical disk supports file based compression."
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool SupportsFileBasedCompression
    {
      get
      {
        if ((curObj["SupportsFileBasedCompression"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["SupportsFileBasedCompression"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The scoping System\'s CreationClassName.")]
    public string SystemCreationClassName
    {
      get { return ((string)(curObj["SystemCreationClassName"])); }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The scoping System\'s Name.")]
    public string SystemName
    {
      get { return ((string)(curObj["SystemName"])); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsVolumeDirtyNull
    {
      get
      {
        if ((curObj["VolumeDirty"] == null))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description(
      @"The VolumeDirty property indicates whether the disk requires chkdsk to be run at next boot up time. The property is applicable to only those instances of logical disk that represent a physical disk in the machine. It is not applicable to mapped logical drives. "
      )]
    [TypeConverter(typeof (WMIValueTypeConverter))]
    public bool VolumeDirty
    {
      get
      {
        if ((curObj["VolumeDirty"] == null))
        {
          return Convert.ToBoolean(0);
        }
        return ((bool)(curObj["VolumeDirty"]));
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The VolumeName property indicates the volume name of the logical disk.\nConstraint" +
                 "s: Maximum 32 characters")]
    public string VolumeName
    {
      get { return ((string)(curObj["VolumeName"])); }
      set
      {
        curObj["VolumeName"] = value;
        if (((isEmbedded == false)
             && AutoCommitProp))
        {
          PrivateLateBoundObject.Put();
        }
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The VolumeSerialNumber property indicates the volume serial number of the logical" +
                 " disk.\nConstraints: Maximum 11 characters\nExample: A8C3-D032")]
    public string VolumeSerialNumber
    {
      get { return ((string)(curObj["VolumeSerialNumber"])); }
    }

    private bool CheckIfProperClass(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions OptionsParam)
    {
      if (((path != null)
           && (String.Compare(path.ClassName, ManagementClassName, true, CultureInfo.InvariantCulture) == 0)))
      {
        return true;
      }
      else
      {
        return CheckIfProperClass(new ManagementObject(mgmtScope, path, OptionsParam));
      }
    }

    private bool CheckIfProperClass(ManagementBaseObject theObj)
    {
      if (((theObj != null)
           &&
           (String.Compare(((string)(theObj["__CLASS"])), ManagementClassName, true, CultureInfo.InvariantCulture) == 0)))
      {
        return true;
      }
      else
      {
        Array parentClasses = ((Array)(theObj["__DERIVATION"]));
        if ((parentClasses != null))
        {
          Int32 count = 0;
          for (count = 0; (count < parentClasses.Length); count = (count + 1))
          {
            if (
              (String.Compare(((string)(parentClasses.GetValue(count))), ManagementClassName, true,
                              CultureInfo.InvariantCulture) == 0))
            {
              return true;
            }
          }
        }
      }
      return false;
    }

    private bool ShouldSerializeAccess()
    {
      if ((IsAccessNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeAvailability()
    {
      if ((IsAvailabilityNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeBlockSize()
    {
      if ((IsBlockSizeNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeCompressed()
    {
      if ((IsCompressedNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeConfigManagerErrorCode()
    {
      if ((IsConfigManagerErrorCodeNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeConfigManagerUserConfig()
    {
      if ((IsConfigManagerUserConfigNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeDriveType()
    {
      if ((IsDriveTypeNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeErrorCleared()
    {
      if ((IsErrorClearedNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeFreeSpace()
    {
      if ((IsFreeSpaceNull == false))
      {
        return true;
      }
      return false;
    }

    // Konvertiert ein gegebenes Datum bzw. eine Uhrzeit im DMTF-Format in ein System.DateTime-Objekt.
    private static DateTime ToDateTime(string dmtfDate)
    {
      int year = DateTime.MinValue.Year;
      int month = DateTime.MinValue.Month;
      int day = DateTime.MinValue.Day;
      int hour = DateTime.MinValue.Hour;
      int minute = DateTime.MinValue.Minute;
      int second = DateTime.MinValue.Second;
      long ticks = 0;
      string dmtf = dmtfDate;
      DateTime datetime = DateTime.MinValue;
      string tempString = String.Empty;
      if ((dmtf == null))
      {
        throw new ArgumentOutOfRangeException();
      }
      if ((dmtf.Length == 0))
      {
        throw new ArgumentOutOfRangeException();
      }
      if ((dmtf.Length != 25))
      {
        throw new ArgumentOutOfRangeException();
      }
      try
      {
        tempString = dmtf.Substring(0, 4);
        if (("****" != tempString))
        {
          year = Int32.Parse(tempString);
        }
        tempString = dmtf.Substring(4, 2);
        if (("**" != tempString))
        {
          month = Int32.Parse(tempString);
        }
        tempString = dmtf.Substring(6, 2);
        if (("**" != tempString))
        {
          day = Int32.Parse(tempString);
        }
        tempString = dmtf.Substring(8, 2);
        if (("**" != tempString))
        {
          hour = Int32.Parse(tempString);
        }
        tempString = dmtf.Substring(10, 2);
        if (("**" != tempString))
        {
          minute = Int32.Parse(tempString);
        }
        tempString = dmtf.Substring(12, 2);
        if (("**" != tempString))
        {
          second = Int32.Parse(tempString);
        }
        tempString = dmtf.Substring(15, 6);
        if (("******" != tempString))
        {
          ticks = (Int64.Parse(tempString)
                   * (TimeSpan.TicksPerMillisecond / 1000));
        }
        if (((((((((year < 0)
                   || (month < 0))
                  || (day < 0))
                 || (hour < 0))
                || (minute < 0))
               || (minute < 0))
              || (second < 0))
             || (ticks < 0)))
        {
          throw new ArgumentOutOfRangeException();
        }
      }
      catch (Exception e)
      {
        e = e;
        throw new ArgumentOutOfRangeException();
      }
      datetime = new DateTime(year, month, day, hour, minute, second, 0);
      datetime = datetime.AddTicks(ticks);
      TimeSpan tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
      int UTCOffset = 0;
      long OffsetToBeAdjusted = 0;
      long OffsetMins = (tickOffset.Ticks / TimeSpan.TicksPerMinute);
      tempString = dmtf.Substring(22, 3);
      if ((tempString != "***"))
      {
        tempString = dmtf.Substring(21, 4);
        try
        {
          UTCOffset = Int32.Parse(tempString);
        }
        catch (Exception e)
        {
          throw new ArgumentOutOfRangeException();
        }
        OffsetToBeAdjusted = (OffsetMins - UTCOffset);
        datetime = datetime.AddMinutes(OffsetToBeAdjusted);
      }
      return datetime;
    }

    // Konvertiert ein angegebenes System.DateTime-Objekt in das DMTF-Datums- bzw. Zeitformat.
    private static string ToDmtfDateTime(DateTime date)
    {
      string utcString = String.Empty;
      TimeSpan tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
      long OffsetMins = (tickOffset.Ticks / TimeSpan.TicksPerMinute);
      if ((Math.Abs(OffsetMins) > 999))
      {
        date = date.ToUniversalTime();
        utcString = "+000";
      }
      else
      {
        if ((tickOffset.Ticks >= 0))
        {
          utcString = ("+" + ((tickOffset.Ticks / TimeSpan.TicksPerMinute)).ToString().PadLeft(3, '0'));
        }
        else
        {
          string strTemp = OffsetMins.ToString();
          utcString = ("-" + strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
        }
      }
      string dmtfDateTime = date.Year.ToString().PadLeft(4, '0');
      dmtfDateTime = (dmtfDateTime + date.Month.ToString().PadLeft(2, '0'));
      dmtfDateTime = (dmtfDateTime + date.Day.ToString().PadLeft(2, '0'));
      dmtfDateTime = (dmtfDateTime + date.Hour.ToString().PadLeft(2, '0'));
      dmtfDateTime = (dmtfDateTime + date.Minute.ToString().PadLeft(2, '0'));
      dmtfDateTime = (dmtfDateTime + date.Second.ToString().PadLeft(2, '0'));
      dmtfDateTime = (dmtfDateTime + ".");
      DateTime dtTemp = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
      long microsec = (((date.Ticks - dtTemp.Ticks)
                        * 1000)
                       / TimeSpan.TicksPerMillisecond);
      string strMicrosec = microsec.ToString();
      if ((strMicrosec.Length > 6))
      {
        strMicrosec = strMicrosec.Substring(0, 6);
      }
      dmtfDateTime = (dmtfDateTime + strMicrosec.PadLeft(6, '0'));
      dmtfDateTime = (dmtfDateTime + utcString);
      return dmtfDateTime;
    }

    private bool ShouldSerializeInstallDate()
    {
      if ((IsInstallDateNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeLastErrorCode()
    {
      if ((IsLastErrorCodeNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeMaximumComponentLength()
    {
      if ((IsMaximumComponentLengthNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeMediaType()
    {
      if ((IsMediaTypeNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeNumberOfBlocks()
    {
      if ((IsNumberOfBlocksNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializePowerManagementSupported()
    {
      if ((IsPowerManagementSupportedNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeQuotasDisabled()
    {
      if ((IsQuotasDisabledNull == false))
      {
        return true;
      }
      return false;
    }

    private void ResetQuotasDisabled()
    {
      curObj["QuotasDisabled"] = null;
      if (((isEmbedded == false)
           && AutoCommitProp))
      {
        PrivateLateBoundObject.Put();
      }
    }

    private bool ShouldSerializeQuotasIncomplete()
    {
      if ((IsQuotasIncompleteNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeQuotasRebuilding()
    {
      if ((IsQuotasRebuildingNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeSize()
    {
      if ((IsSizeNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeStatusInfo()
    {
      if ((IsStatusInfoNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeSupportsDiskQuotas()
    {
      if ((IsSupportsDiskQuotasNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeSupportsFileBasedCompression()
    {
      if ((IsSupportsFileBasedCompressionNull == false))
      {
        return true;
      }
      return false;
    }

    private bool ShouldSerializeVolumeDirty()
    {
      if ((IsVolumeDirtyNull == false))
      {
        return true;
      }
      return false;
    }

    private void ResetVolumeName()
    {
      curObj["VolumeName"] = null;
      if (((isEmbedded == false)
           && AutoCommitProp))
      {
        PrivateLateBoundObject.Put();
      }
    }

    [Browsable(true)]
    public void CommitObject()
    {
      if ((isEmbedded == false))
      {
        PrivateLateBoundObject.Put();
      }
    }

    private static string ConstructPath(string keyDeviceID)
    {
      string strPath = "root\\cimv2:Win32_Logicaldisk";
      strPath = (strPath
                 + (".DeviceID="
                    + ("\""
                       + (keyDeviceID + "\""))));
      return strPath;
    }

    // Unterschiedliche Überladungen der GetInstances()-Hilfe in den Enumerationsinstanzen der WMI-Klasse.
    public static LogicaldiskCollection GetInstances()
    {
      return GetInstances(((null)), ((EnumerationOptions)(null)));
    }

    public static LogicaldiskCollection GetInstances(string condition)
    {
      return GetInstances(null, condition, null);
    }

    public static LogicaldiskCollection GetInstances(String[] selectedProperties)
    {
      return GetInstances(null, null, selectedProperties);
    }

    public static LogicaldiskCollection GetInstances(string condition, String[] selectedProperties)
    {
      return GetInstances(null, condition, selectedProperties);
    }

    public static LogicaldiskCollection GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions)
    {
      if ((mgmtScope == null))
      {
        if ((statMgmtScope == null))
        {
          mgmtScope = new ManagementScope();
          mgmtScope.Path.NamespacePath = "root\\cimv2";
        }
        else
        {
          mgmtScope = statMgmtScope;
        }
      }
      ManagementPath pathObj = new ManagementPath();
      pathObj.ClassName = "Win32_Logicaldisk";
      pathObj.NamespacePath = "root\\cimv2";
      ManagementClass clsObject = new ManagementClass(mgmtScope, pathObj, null);
      if ((enumOptions == null))
      {
        enumOptions = new EnumerationOptions();
        enumOptions.EnsureLocatable = true;
      }
      return new LogicaldiskCollection(clsObject.GetInstances(enumOptions));
    }

    public static LogicaldiskCollection GetInstances(ManagementScope mgmtScope, string condition)
    {
      return GetInstances(mgmtScope, condition, null);
    }

    public static LogicaldiskCollection GetInstances(ManagementScope mgmtScope, String[] selectedProperties)
    {
      return GetInstances(mgmtScope, null, selectedProperties);
    }

    public static LogicaldiskCollection GetInstances(ManagementScope mgmtScope, string condition,
                                                     String[] selectedProperties)
    {
      if ((mgmtScope == null))
      {
        if ((statMgmtScope == null))
        {
          mgmtScope = new ManagementScope();
          mgmtScope.Path.NamespacePath = "root\\cimv2";
        }
        else
        {
          mgmtScope = statMgmtScope;
        }
      }
      ManagementObjectSearcher ObjectSearcher = new ManagementObjectSearcher(mgmtScope,
                                                                             new SelectQuery("Win32_Logicaldisk",
                                                                                             condition,
                                                                                             selectedProperties));
      EnumerationOptions enumOptions = new EnumerationOptions();
      enumOptions.EnsureLocatable = true;
      ObjectSearcher.Options = enumOptions;
      return new LogicaldiskCollection(ObjectSearcher.Get());
    }

    [Browsable(true)]
    public static Logicaldisk CreateInstance()
    {
      ManagementScope mgmtScope = null;
      if ((statMgmtScope == null))
      {
        mgmtScope = new ManagementScope();
        mgmtScope.Path.NamespacePath = CreatedWmiNamespace;
      }
      else
      {
        mgmtScope = statMgmtScope;
      }
      ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
      return new Logicaldisk(new ManagementClass(mgmtScope, mgmtPath, null).CreateInstance());
    }

    [Browsable(true)]
    public void Delete()
    {
      PrivateLateBoundObject.Delete();
    }

    public UInt32 Chkdsk(bool FixErrors, bool ForceDismount, bool OkToRunAtBootUp, bool RecoverBadSectors,
                         bool SkipFolderCycle, bool VigorousIndexCheck)
    {
      if ((isEmbedded == false))
      {
        ManagementBaseObject inParams = null;
        inParams = PrivateLateBoundObject.GetMethodParameters("Chkdsk");
        inParams["FixErrors"] = FixErrors;
        inParams["ForceDismount"] = ForceDismount;
        inParams["OkToRunAtBootUp"] = OkToRunAtBootUp;
        inParams["RecoverBadSectors"] = RecoverBadSectors;
        inParams["SkipFolderCycle"] = SkipFolderCycle;
        inParams["VigorousIndexCheck"] = VigorousIndexCheck;
        ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Chkdsk", inParams, null);
        return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
      }
      else
      {
        return Convert.ToUInt32(0);
      }
    }

    public static UInt32 ExcludeFromAutochk(string[] LogicalDisk)
    {
      bool IsMethodStatic = true;
      if (IsMethodStatic)
      {
        ManagementBaseObject inParams = null;
        ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
        ManagementClass classObj = new ManagementClass(statMgmtScope, mgmtPath, null);
        inParams = classObj.GetMethodParameters("ExcludeFromAutochk");
        inParams["LogicalDisk"] = LogicalDisk;
        ManagementBaseObject outParams = classObj.InvokeMethod("ExcludeFromAutochk", inParams, null);
        return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
      }
      else
      {
        return Convert.ToUInt32(0);
      }
    }

    public UInt32 Reset()
    {
      if ((isEmbedded == false))
      {
        ManagementBaseObject inParams = null;
        ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Reset", inParams, null);
        return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
      }
      else
      {
        return Convert.ToUInt32(0);
      }
    }

    public static UInt32 ScheduleAutoChk(string[] LogicalDisk)
    {
      bool IsMethodStatic = true;
      if (IsMethodStatic)
      {
        ManagementBaseObject inParams = null;
        ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
        ManagementClass classObj = new ManagementClass(statMgmtScope, mgmtPath, null);
        inParams = classObj.GetMethodParameters("ScheduleAutoChk");
        inParams["LogicalDisk"] = LogicalDisk;
        ManagementBaseObject outParams = classObj.InvokeMethod("ScheduleAutoChk", inParams, null);
        return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
      }
      else
      {
        return Convert.ToUInt32(0);
      }
    }

    public UInt32 SetPowerState(UInt16 PowerState, DateTime Time)
    {
      if ((isEmbedded == false))
      {
        ManagementBaseObject inParams = null;
        inParams = PrivateLateBoundObject.GetMethodParameters("SetPowerState");
        inParams["PowerState"] = PowerState;
        inParams["Time"] = ToDmtfDateTime(((Time)));
        ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("SetPowerState", inParams, null);
        return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
      }
      else
      {
        return Convert.ToUInt32(0);
      }
    }

    // Enumerationsimplementierung für die Instanzenenumeration der Klasse.

    #region Nested type: LogicaldiskCollection

    public class LogicaldiskCollection : object, ICollection
    {
      private readonly ManagementObjectCollection ObjectCollection;

      public LogicaldiskCollection(ManagementObjectCollection objCollection)
      {
        ObjectCollection = objCollection;
      }

      #region ICollection Members

      public int Count
      {
        get { return ObjectCollection.Count; }
      }

      public bool IsSynchronized
      {
        get { return ObjectCollection.IsSynchronized; }
      }

      public object SyncRoot
      {
        get { return this; }
      }

      public void CopyTo(Array array, int index)
      {
        ObjectCollection.CopyTo(array, index);
        int nCtr;
        for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1))
        {
          array.SetValue(new Logicaldisk(((ManagementObject)(array.GetValue(nCtr)))), nCtr);
        }
      }

      public IEnumerator GetEnumerator()
      {
        return new LogicaldiskEnumerator(ObjectCollection.GetEnumerator());
      }

      #endregion

      #region Nested type: LogicaldiskEnumerator

      public class LogicaldiskEnumerator : object, IEnumerator
      {
        private readonly ManagementObjectCollection.ManagementObjectEnumerator ObjectEnumerator;

        public LogicaldiskEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum)
        {
          ObjectEnumerator = objEnum;
        }

        #region IEnumerator Members

        public object Current
        {
          get { return new Logicaldisk(((ManagementObject)(ObjectEnumerator.Current))); }
        }

        public bool MoveNext()
        {
          return ObjectEnumerator.MoveNext();
        }

        public void Reset()
        {
          ObjectEnumerator.Reset();
        }

        #endregion
      }

      #endregion
    }

    #endregion

    // TypeConverter zum Behandeln von Nullwerten für ValueType-Eigenschaften

    // Eingebettete Klasse zum Darstellen der WMI-Systemeigenschaften.

    #region Nested type: ManagementSystemProperties

    [TypeConverter(typeof (ExpandableObjectConverter))]
    public class ManagementSystemProperties
    {
      private readonly ManagementBaseObject PrivateLateBoundObject;

      public ManagementSystemProperties(ManagementBaseObject ManagedObject)
      {
        PrivateLateBoundObject = ManagedObject;
      }

      [Browsable(true)]
      public int GENUS
      {
        get { return ((int)(PrivateLateBoundObject["__GENUS"])); }
      }

      [Browsable(true)]
      public string CLASS
      {
        get { return ((string)(PrivateLateBoundObject["__CLASS"])); }
      }

      [Browsable(true)]
      public string SUPERCLASS
      {
        get { return ((string)(PrivateLateBoundObject["__SUPERCLASS"])); }
      }

      [Browsable(true)]
      public string DYNASTY
      {
        get { return ((string)(PrivateLateBoundObject["__DYNASTY"])); }
      }

      [Browsable(true)]
      public string RELPATH
      {
        get { return ((string)(PrivateLateBoundObject["__RELPATH"])); }
      }

      [Browsable(true)]
      public int PROPERTY_COUNT
      {
        get { return ((int)(PrivateLateBoundObject["__PROPERTY_COUNT"])); }
      }

      [Browsable(true)]
      public string[] DERIVATION
      {
        get { return ((string[])(PrivateLateBoundObject["__DERIVATION"])); }
      }

      [Browsable(true)]
      public string SERVER
      {
        get { return ((string)(PrivateLateBoundObject["__SERVER"])); }
      }

      [Browsable(true)]
      public string NAMESPACE
      {
        get { return ((string)(PrivateLateBoundObject["__NAMESPACE"])); }
      }

      [Browsable(true)]
      public string PATH
      {
        get { return ((string)(PrivateLateBoundObject["__PATH"])); }
      }
    }

    #endregion

    #region Nested type: WMIValueTypeConverter

    public class WMIValueTypeConverter : TypeConverter
    {
      private readonly TypeConverter baseConverter;

      public WMIValueTypeConverter(Type baseType)
      {
        baseConverter = TypeDescriptor.GetConverter(baseType);
      }

      public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
      {
        return baseConverter.CanConvertFrom(context, srcType);
      }

      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
        return baseConverter.CanConvertTo(context, destinationType);
      }

      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
      {
        return baseConverter.ConvertFrom(context, culture, value);
      }

      public override object CreateInstance(ITypeDescriptorContext context, IDictionary dictionary)
      {
        return baseConverter.CreateInstance(context, dictionary);
      }

      public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
      {
        return baseConverter.GetCreateInstanceSupported(context);
      }

      public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                 Attribute[] attributeVar)
      {
        return baseConverter.GetProperties(context, value, attributeVar);
      }

      public override bool GetPropertiesSupported(ITypeDescriptorContext context)
      {
        return baseConverter.GetPropertiesSupported(context);
      }

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        return baseConverter.GetStandardValues(context);
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
        return baseConverter.GetStandardValuesExclusive(context);
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
        return baseConverter.GetStandardValuesSupported(context);
      }

      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                       Type destinationType)
      {
        if ((context != null))
        {
          if ((context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))
          {
            return "";
          }
        }
        return baseConverter.ConvertTo(context, culture, value, destinationType);
      }
    }

    #endregion
  }
}