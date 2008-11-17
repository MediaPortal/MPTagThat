using System;
using System.Collections.Generic;
using System.Text;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace MPTagThat.Core.AudioEncoder
{
  public class AudioEncoder : IAudioEncoder
  {
    #region Variables
    private string _pathToEncoders = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "bin\\Encoder\\");
    private string _outFile;
    private string _encoder;
    private ILogger log;
    private QueueMessage msg;
    IMessageQueue queue;
    #endregion

    #region ctor
    public AudioEncoder()
    {
      log = ServiceScope.Get<ILogger>();
      queue = ServiceScope.Get<IMessageBroker>().GetOrCreate("encoding");
      msg = new QueueMessage();
    }
    #endregion

    #region IAudioEncoder Members
    /// <summary>
    /// Sets the Encoder and the Outfile Name
    /// </summary>
    /// <param name="encoder"></param>
    /// <param name="outFile"></param>
    /// <returns>Formatted Outfile with Extension</returns>
    public string SetEncoder(string encoder, string outFile)
    {
      _encoder = encoder;
      _outFile = SetOutFileExtension(outFile);
      return _outFile;
    }

    /// <summary>
    /// Starts encoding using the given Parameters
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoderParms"></param>
    /// 
    public BASSError StartEncoding(int stream)
    {
      BaseEncoder encoder = SetEncoderSettings(stream);
      encoder.EncoderDirectory = _pathToEncoders;
      encoder.OutputFile = _outFile;
      encoder.InputFile = null;    // Use stdin

      bool encoderHandle = encoder.Start(null, IntPtr.Zero, false);
      if (!encoderHandle)
      {
        return Bass.BASS_ErrorGetCode();
      }

      long pos = 0;
      long chanLength = Bass.BASS_ChannelGetLength(stream);

      byte[] encBuffer = new byte[60000]; // our encoding buffer
      while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
      {
        // getting sample data will automatically feed the encoder
        int len = Bass.BASS_ChannelGetData(stream, encBuffer, encBuffer.Length);
        pos = Bass.BASS_ChannelGetPosition(stream);
        double percentComplete = (double)pos / (double)chanLength * 100.0;

        // Send the message
        msg.MessageData["progress"] = percentComplete;       
        queue.Send(msg);
      }

      encoder.Stop();
      return BASSError.BASS_OK;
    }
    #endregion

    #region Private Methods
    private string SetOutFileExtension(string outFile)
    {
      string outFileName = outFile;
      switch (_encoder)
      {
        case "mp3":
          outFileName += ".mp3";
          break;

        case "ogg":
          outFileName += ".ogg";
          break;

        case "flac":
          outFileName += ".flac";
          break;

        case "m4a":
          outFileName += ".m4a";
          break;

        case "wav":
          outFileName += ".wav";
          break;

        case "wma":
          outFileName += ".wma";
          break;

        case "mpc":
          outFileName += ".mpc";
          break;

        case "wv":
          outFileName += ".wv";
          break;
      }
      return outFileName;
    }

    private BaseEncoder SetEncoderSettings(int stream)
    {
      BaseEncoder encoder = null;
      switch (_encoder)
      {
        case "mp3":
          EncoderLAME encLame = new EncoderLAME(stream);
          if (Options.MainSettings.RipLameExpert.Length > 0)
          {
            encLame.LAME_CustomOptions = Options.MainSettings.RipLameExpert;
            encLame.LAME_UseCustomOptionsOnly = true;
          }
          else
          {
            if (Options.MainSettings.RipLamePreset == (int)Options.LamePreset.ABR)
              encLame.LAME_PresetName = Options.MainSettings.RipLameABRBitRate.ToString();
            else
              encLame.LAME_PresetName = Enum.GetName(typeof(Options.LamePreset), Options.MainSettings.RipLamePreset).ToLower();
          }
          encoder = encLame;
          break;

        case "ogg":
          EncoderOGG encOgg = new EncoderOGG(stream);
          if (Options.MainSettings.RipOggExpert.Length > 0)
          {
            encOgg.OGG_CustomOptions = Options.MainSettings.RipOggExpert;
            encOgg.OGG_UseCustomOptionsOnly = true;
          }
          else
          {
            encOgg.OGG_Quality = (float)Convert.ToInt32(Options.MainSettings.RipOggQuality);
          }
          encoder = encOgg;
          break;

        case "flac":
          EncoderFLAC encFlac = new EncoderFLAC(stream);
          if (Options.MainSettings.RipFlacExpert.Length > 0)
          {
            encFlac.FLAC_CustomOptions = Options.MainSettings.RipFlacExpert;
            encFlac.FLAC_UseCustomOptionsOnly = true;
          }
          else
          {
            encFlac.FLAC_CompressionLevel = Options.MainSettings.RipFlacQuality;
          }
          // put a 1k padding block for Tagging in front
          encFlac.FLAC_Padding = 1024;
          encoder = encFlac;
          break;

        case "m4a":
          EncoderWinampAACplus encAAC = new EncoderWinampAACplus(stream);
          encAAC.AACPlus_Mp4Box = true;
          
          int bitrate = Convert.ToInt32(Options.MainSettings.RipEncoderAACBitRate.Substring(0, Options.MainSettings.RipEncoderAACBitRate.IndexOf(' ')));
          encAAC.AACPlus_Bitrate = bitrate;

          if (Options.MainSettings.RipEncoderAAC.Contains("High"))
          {
            encAAC.AACPlus_High = true;
          }
          if (Options.MainSettings.RipEncoderAAC.Contains("LC"))
          {
            encAAC.AACPlus_LC = true;
          }
          encoder = encAAC;
          break;

        case "wav":
          EncoderWAV encWav = new EncoderWAV(stream);
          encoder = encWav;
          break;

        case "wma":
          EncoderWMA encWma = new EncoderWMA(stream);
          string[] sampleFormat = Options.MainSettings.RipEncoderWMASample.Split(',');
          string encoderFormat = Options.MainSettings.RipEncoderWMA;
          if (encoderFormat == "wmapro" || encoderFormat == "wmalossless")
            encWma.WMA_UsePro = true;
          else
            encWma.WMA_ForceStandard = true;

          if (Options.MainSettings.RipEncoderWMACbrVbr == "Vbr")
          {
            encWma.WMA_UseVBR = true;
            encWma.WMA_VBRQuality = Convert.ToInt32(Options.MainSettings.RipEncoderWMABitRate) / 1000;
          }
          else
            encWma.WMA_Bitrate = Convert.ToInt32(Options.MainSettings.RipEncoderWMABitRate) / 1000;


          if (sampleFormat[0] == "24")
            encWma.WMA_Use24Bit = true;

          encoder = encWma;
          break;

        case "mpc":
          EncoderMPC encMpc = new EncoderMPC(stream);
          if (Options.MainSettings.RipEncoderMPCExpert.Length > 0)
          {
            encMpc.MPC_CustomOptions = Options.MainSettings.RipEncoderMPCExpert;
            encMpc.MPC_UseCustomOptionsOnly = true;
          }
          else
          {
            encMpc.MPC_Preset = (EncoderMPC.MPCPreset)Enum.Parse(typeof(EncoderMPC.MPCPreset), Options.MainSettings.RipEncoderMPCPreset);
          }
          encoder = encMpc;
          break;

        case "wv":
          EncoderWavPack encWv = new EncoderWavPack(stream);
          if (Options.MainSettings.RipEncoderWVExpert.Length > 0)
          {
            encWv.WV_CustomOptions = Options.MainSettings.RipEncoderWVExpert;
            encWv.WV_UseCustomOptionsOnly = true;
          }
          else
          {
            if (Options.MainSettings.RipEncoderWVPreset == "-f")
              encWv.WV_FastMode = true;
            else
              encWv.WV_HighQuality = true;
          }
          encoder = encWv;
          break;
      }

      return encoder;
    }
    #endregion
  }
}
