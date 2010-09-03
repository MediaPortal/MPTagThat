using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.FileNameToTag
{
  public class TagFormat
  {
    #region Variables
    private List<ParameterPart> parameterParts = new List<ParameterPart>();
    #endregion

    #region Properties
    public List<ParameterPart> ParameterParts
    {
      get { return parameterParts; }
    }
    #endregion

    #region Constructor
    /// <summary>
    /// Parses the Parameter Format to retrieve Tags from Filenames.
    /// </summary>
    /// <param name="parameterFormat"></param>
    public TagFormat(string parameterFormat)
    {
      parameterParts.Clear();

      // Split the given parameters to see, if folders have been specified
      string[] parms = parameterFormat.Split(new char[] { '\\'});
      for (int i = 0; i < parms.Length; i++)
      {
        if (!parms[i].StartsWith("<"))
          parms[i] = String.Format("<X>{0}",parms[i]);

        if (!parms[i].EndsWith(">"))
          parms[i] = String.Format("{0}<X>", parms[i]);
      }
      for (int i = parms.Length - 1; i >= 0; i--)
      {
        ParameterPart part = new ParameterPart(parms[i]);
        parameterParts.Add(part);
      }
    }
    #endregion
  }
}
