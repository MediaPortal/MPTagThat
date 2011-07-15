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

using System.Diagnostics.CodeAnalysis;
using Elegant.Ui;

#endregion

[assembly: CommandSource("MPTagThat.ApplicationCommands", "MPTagThat.ApplicationCommands")]

namespace MPTagThat
{
  //  This class is required for commands suppport.
// Do not modify this code inside the editor.

  public class ApplicationCommands
  {
    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command Exit =
      new Command("Exit");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command Save =
      new Command("Save");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command Options =
      new Command("Options");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command Refresh =
      new Command("Refresh");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command Help =
      new Command("Help");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      FileNameToTag = new Command("FileNameToTag");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      TagFromInternet = new Command("TagFromInternet");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      IdentifyFiles = new Command("IdentifyFiles");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command GetCoverArt
      = new Command("GetCoverArt");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      RemoveCoverArt = new Command("RemoveCoverArt");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command GetLyrics =
      new Command("GetLyrics");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      RemoveComment = new Command("RemoveComment");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command RenameFiles
      = new Command("RenameFiles");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      RenameFileOptions = new Command("RenameFileOptions");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      CaseConversion = new Command("CaseConversion");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      CaseConversionOptions = new Command("CaseConversionOptions");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      OrganiseFiles = new Command("OrganiseFiles");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command AutoNumber
      = new Command("AutoNumber");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      ScriptExecute = new Command("ScriptExecute");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command AddToBurner
      = new Command("AddToBurner");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      AddToConversion = new Command("AddToConversion");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      AddToPlaylist = new Command("AddToPlaylist");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command RipStart =
      new Command("RipStart");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command RipCancel =
      new Command("RipCancel");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      DeleteAllTags = new Command("DeleteAllTags");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command DeleteID3v1
      = new Command("DeleteID3v1");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command DeleteID3v2
      = new Command("DeleteID3v2");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      FolderSelect = new Command("FolderSelect");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      ConvertStart = new Command("ConvertStart");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      ConvertCancel = new Command("ConvertCancel");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command BurnStart =
      new Command("BurnStart");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command BurnCancel
      = new Command("BurnCancel");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      ChangeDisplayColumns = new Command("ChangeDisplayColumns");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command
      ProgressCancel = new Command("ProgressCancel");

    [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")] public static Command SaveAsThumb
      = new Command("SaveAsThumb");
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
    public static Command Find = new Elegant.Ui.Command("Find");
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
    public static Command Replace = new Elegant.Ui.Command("Replace");
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
    public static Command ValidateSong = new Elegant.Ui.Command("ValidateSong");
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
    public static Command FixSong = new Elegant.Ui.Command("FixSong");
  }
}