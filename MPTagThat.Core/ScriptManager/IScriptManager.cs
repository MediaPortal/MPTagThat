using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MPTagThat.Core
{
  public interface IScriptManager
  {
    Assembly Load(string scriptFile);
    ArrayList GetScripts();
    ArrayList GetOrganiseScripts();
  }
}
