#region Copyright (C) 2009-2015 Team MediaPortal
// Copyright (C) 2009-2015 Team MediaPortal
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

using System;
using System.IO;
using MPTagThat.Core;
using MPTagThat.Core.AudioEncoder;
using MPTagThat.GridView;
using Un4seen.Bass;
using File = TagLib.File;

namespace MPTagThat.Commands
{
  [SupportedCommandType("Convert")]
  public class CmdConvert : Command
  {
    #region Variables

    private readonly ILocalisation _localisation = ServiceScope.Get<ILocalisation>();
    private readonly QueueMessage _msg = new QueueMessage();
    private readonly IMessageQueue _queue = ServiceScope.Get<IMessageBroker>().GetOrCreate("encoding");
    private ConversionData _conversionData;

    #endregion

    #region ctor

    public CmdConvert(object[] parameters)
    {
      _conversionData = (ConversionData)parameters[1];
    }

    #endregion


    #region Command Implementation

    public override bool Execute(ref TrackData track, int rowIndex)
    {
      string inputFile = track.FullFileName;
      string outFile = Util.ReplaceParametersWithTrackValues(Options.MainSettings.RipFileNameFormat, track);
      outFile = Path.Combine(_conversionData.RootFolder, outFile);
      string directoryName = Path.GetDirectoryName(outFile);

      // Now check the validity of the directory
      if (directoryName != null && !Directory.Exists(directoryName))
      {
        try
        {
          Directory.CreateDirectory(directoryName);
        }
        catch (Exception e1)
        {
          Log.Error("Error creating folder: {0} {1]", directoryName, e1.Message);
          // Send the message
          _msg.MessageData["action"] = "error";
          _msg.MessageData["error"] = _localisation.ToString("message", "Error");
          _msg.MessageData["tooltip"] = String.Format("{0}: {1}", _localisation.ToString("message", "Error"), e1.Message);
          _msg.MessageData["rowindex"] = rowIndex;
          _queue.Send(_msg);
          return false; 
        }
      }

      IAudioEncoder audioEncoder = new AudioEncoder();
      outFile = audioEncoder.SetEncoder(_conversionData.Encoder, outFile);
      
      // Send message with new Filename
      _msg.MessageData["action"] = "newfilename";
      _msg.MessageData["filename"] = outFile;
      _msg.MessageData["rowindex"] = rowIndex;
      _queue.Send(_msg);

      if (inputFile == outFile)
      {
        _msg.MessageData["action"] = "error";
        _msg.MessageData["error"] = _localisation.ToString("message", "Error");
        _msg.MessageData["tooltip"] = String.Format("{0}: {1}", inputFile, _localisation.ToString("Conversion", "SameFile"));
        _msg.MessageData["rowindex"] = rowIndex;
        _queue.Send(_msg);
        Log.Error("No conversion for {0}. Output would overwrite input", inputFile);
        return false;
      }

      int stream = Bass.BASS_StreamCreateFile(inputFile, 0, 0, BASSFlag.BASS_STREAM_DECODE);
      if (stream == 0)
      {
        _msg.MessageData["action"] = "error";
        _msg.MessageData["error"] = _localisation.ToString("message", "Error");
        _msg.MessageData["tooltip"] = String.Format("{0}: {1}", inputFile, _localisation.ToString("Conversion", "OpenFileError"));
        _msg.MessageData["rowindex"] = rowIndex;
        _queue.Send(_msg);
        Log.Error("Error creating stream for file {0}. Error: {1}", inputFile,
                  Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode()));
        return false;
      }

      Log.Info("Convert file {0} -> {1}", inputFile, outFile);

      if (audioEncoder.StartEncoding(stream, rowIndex) != BASSError.BASS_OK)
      {
        _msg.MessageData["action"] = "error";
        _msg.MessageData["error"] = _localisation.ToString("message", "Error");
        _msg.MessageData["tooltip"] = String.Format("{0}: {1}", inputFile, _localisation.ToString("Conversion", "EncodingFileError"));
        _msg.MessageData["rowindex"] = rowIndex;
        _queue.Send(_msg);
        Log.Error("Error starting Encoder for File {0}. Error: {1}", inputFile,
                  Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode()));
        Bass.BASS_StreamFree(stream);
        return false;
      }

      _msg.MessageData["action"] = "progress";
      _msg.MessageData["percent"] = 100.0;
      _msg.MessageData["rowindex"] = rowIndex;
      _queue.Send(_msg);

      Bass.BASS_StreamFree(stream);

      try
      {
        // Now Tag the encoded File
        File tagInFile = File.Create(inputFile);
        File tagOutFile = File.Create(outFile);
        tagOutFile.Tag.AlbumArtists = tagInFile.Tag.AlbumArtists;
        tagOutFile.Tag.Album = tagInFile.Tag.Album;
        tagOutFile.Tag.Genres = tagInFile.Tag.Genres;
        tagOutFile.Tag.Year = tagInFile.Tag.Year;
        tagOutFile.Tag.Performers = tagInFile.Tag.Performers;
        tagOutFile.Tag.Track = tagInFile.Tag.Track;
        tagOutFile.Tag.TrackCount = tagInFile.Tag.TrackCount;
        tagOutFile.Tag.Title = tagInFile.Tag.Title;
        tagOutFile.Tag.Comment = tagInFile.Tag.Comment;
        tagOutFile.Tag.Composers = tagInFile.Tag.Composers;
        tagOutFile.Tag.Conductor = tagInFile.Tag.Conductor;
        tagOutFile.Tag.Copyright = tagInFile.Tag.Copyright;
        tagOutFile.Tag.Disc = tagInFile.Tag.Disc;
        tagOutFile.Tag.DiscCount = tagInFile.Tag.DiscCount;
        tagOutFile.Tag.Lyrics = tagInFile.Tag.Lyrics;
        tagOutFile.Tag.Pictures = tagInFile.Tag.Pictures;
        tagOutFile = Util.FormatID3Tag(tagOutFile);
        tagOutFile.Save();

        Log.Info("Finished converting file {0} -> {1}", inputFile, outFile);

        return true;
      }
      catch (Exception ex)
      {
        Log.Error("Error tagging encoded file {0}. Error: {1}", outFile, ex.Message);
      }
      return false;
    }

    #endregion
  }
}