﻿using System;
using System.Drawing;
using System.IO;
using TagLib;

namespace MPTagThat.Core.Common
{
  [Serializable]
  public class Picture
  {
    #region ctor

    /// <summary>
    /// Standard ctor
    /// </summary>
    public Picture()
    {
    }

    /// <summary>
    /// Create a Picture out of the given filename
    /// </summary>
    /// <param name="fileName"></param>
    public Picture(string fileName)
    {
      try
      {
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          Image img = Image.FromStream(fs);
          fs.Close();
          if (img != null)
          {
            Data = img.Clone() as Image;
            img.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        ServiceScope.Get<ILogger>().GetLogger.Error("Error creating picture from file: {0}. Error: {1}", fileName, ex.Message);
      }
    }

    #endregion

    #region Properties

    /// <summary>
    ///    Gets and sets the mime-type of the picture data
    ///    stored in the current instance.
    /// </summary>
    /// <value>
    ///    A <see cref="string" /> object containing the mime-type
    ///    of the picture data stored in the current instance.
    /// </value>
    public string MimeType { get; set; }

    /// <summary>
    ///    Gets and sets the type of content visible in the picture
    ///    stored in the current instance.
    /// </summary>
    /// <value>
    ///    A <see cref="PictureType" /> containing the type of
    ///    content visible in the picture stored in the current
    ///    instance.
    /// </value>
    public TagLib.PictureType Type { get; set; }

    /// <summary>
    ///    Gets and sets a description of the picture stored in the
    ///    current instance.
    /// </summary>
    /// <value>
    ///    A <see cref="string" /> object containing a description
    ///    of the picture stored in the current instance.
    /// </value>
    public string Description { get; set; }

    /// <summary>
    ///    Gets and sets the picture data stored in the current
    ///    instance.
    /// </summary>
    /// <value>
    ///    A <see cref="Image" /> object containing the picture
    ///    data stored in the current instance.
    /// </value>
    public Image Data { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Creates an Image from a Taglib Byte structure
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Image ImageFromData(byte[] data)
    {
      Image img = null;
      try
      {
        using (MemoryStream ms = new MemoryStream(data))
        {
          img = Image.FromStream(ms);
        }
      }
      catch (Exception)
      {
      }

      return img;
    }

    /// <summary>
    /// Returns the Byte array from an image to be used in Taglib.Picture
    /// </summary>
    /// <param name="img"></param>
    /// <returns></returns>
    public static byte[] ImageToByte(Image img)
    {
      // Need to make a copy, otherwise we have a GDI+ Error
      
      byte[] byteArray = new byte[0];
      using (MemoryStream stream = new MemoryStream())
      {
        Bitmap bCopy = new Bitmap(img);
        bCopy.Save(stream, img.RawFormat);
        stream.Close();
        byteArray = stream.ToArray();
      }
      return byteArray;
    }


    #endregion
  }
}
