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
using System.Collections;
using System.IO;
using System.Reflection;
using CSScriptLibrary;

#endregion

namespace MPTagThat.Core
{
  public class ScriptManager : IScriptManager
  {
    #region Variables

    private readonly ArrayList _organiseScripts = new ArrayList();
    private readonly string _sharedAsemblyDir;
    private readonly ArrayList _tagScripts = new ArrayList();
    private Assembly _assembly;

    #endregion

    #region ctor

    public ScriptManager()
    {
      //script (to be compiled) must be in the same folder with Core (MPTagThat.Core.dll) 
      _sharedAsemblyDir = Path.GetDirectoryName(GetLoadedAssemblyLocation("MPTagThat.core"));
      LoadAvailableScripts();
    }

    #endregion

    #region Properties

    public ArrayList GetScripts()
    {
      return _tagScripts;
    }

    public ArrayList GetOrganiseScripts()
    {
      return _organiseScripts;
    }

    #endregion

    #region Initialisation

    private void LoadAvailableScripts()
    {
      DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(_sharedAsemblyDir, "Scripts"));
      FileInfo[] files = dirInfo.GetFiles();
      foreach (FileInfo file in files)
      {
        string ext = Path.GetExtension(file.Name);
        string[] desc = GetDescription(file.FullName);
        if (desc == null)
          continue; // we had an error reading the file

        if (desc[1] == String.Empty)
          desc[1] = Path.GetFileNameWithoutExtension(file.Name); // Use the Filename as Title            

        if (ext == ".sct")
        {
          _tagScripts.Add(desc);
        }
        else if (ext == ".osc")
        {
          _organiseScripts.Add(desc);
        }
      }
    }

    private string[] GetDescription(string fileName)
    {
      string[] description = new string[3];
      StreamReader file;
      try
      {
        file = File.OpenText(fileName);
        string line1 = file.ReadLine();
        string line2 = file.ReadLine();

        description[0] = Path.GetFileName(fileName);
        if (line1.StartsWith("// Title:"))
          description[1] = line1.Substring(9).Trim();
        else
          description[1] = String.Empty;

        if (line2.StartsWith("// Description:"))
          description[2] = line2.Substring(15).Trim();
        else
          description[2] = String.Empty;

        return description;
      }
      catch (Exception)
      {
        return null;
      }
    }

    #endregion

    #region Script Compiling and Loading

    public Assembly Load(string script)
    {
      string scriptFile = String.Format(@"scripts\{0}", script);

      try
      {
        string configDir = Options.ConfigDir.Substring(0, Options.ConfigDir.LastIndexOf("\\"));
        string compiledScriptsDir = Path.Combine(configDir, @"scripts\compiled");
        if (!Directory.Exists(compiledScriptsDir))
        {
          Directory.CreateDirectory(compiledScriptsDir);
        }

        string finalName = String.Format(@"{0}.dll", Path.GetFileNameWithoutExtension(scriptFile));
        finalName = Path.Combine(compiledScriptsDir, finalName);
        string name = String.Format(@"{0}.dll", Path.GetFileNameWithoutExtension(scriptFile));

        // The script file could have been deleted while already executing the progrsm
        if (!File.Exists(scriptFile) && !File.Exists(finalName))
          return null;

        DateTime lastwriteSourceFile = File.GetLastWriteTime(scriptFile);

        // Load the compiled Assembly only, if it is newer than the source file, to get changes done on the file
        if (File.Exists(finalName) && File.GetLastWriteTime(finalName) > lastwriteSourceFile)
        {
          _assembly = Assembly.LoadFile(finalName);
        }
        else
        {
          _assembly = CSScript.Load(scriptFile, finalName, true);

          // And reload the assembly from the compiled dir, so that we may execute it
          Assembly.LoadFile(finalName);
        }
        return _assembly;
      }
      catch (Exception ex)
      {
        ServiceScope.Get<ILogger>().Error("Error loading script: {0} {1}", scriptFile, ex.Message);
        return null;
      }
    }

    private string GetLoadedAssemblyLocation(string name)
    {
      foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
      {
        string asmName = asm.FullName.Split(",".ToCharArray())[0];
        if (string.Compare(name, asmName, true) == 0)
          return asm.Location;
      }
      return "";
    }

    #endregion
  }
}