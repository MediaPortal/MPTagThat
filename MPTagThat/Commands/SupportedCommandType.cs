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
using System.Collections.Generic;

namespace MPTagThat.Commands
{
  /// <summary>
	///    This class provides an attribute for listing supported command-types
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public sealed class SupportedCommandType : Attribute
  {
    /// <summary>
    ///    Contains the registered <see cref="SupportedCommandType" />
    ///    objects.
    /// </summary>
    private static List<SupportedCommandType> _commandTypes = new List<SupportedCommandType>();

    /// <summary>
    ///    Contains the command-type.
    /// </summary>
    private string _commandType;
    
    /// <summary>
		///    Constructs and initializes the <see
    ///    cref="SupportedCommandType" /> class by initializing the
		///    <see cref="CommandTypes" /> class.
		/// </summary>
    static SupportedCommandType()
		{
			CommandTypes.Init ();
		}
 
    /// <summary>
		///    Constructs and initializes a new instance of the <see
		///    cref="SupportedCommandType" /> attribute for a specified
		///    command-type.
		/// </summary>
    public SupportedCommandType(string commandtype)
		{
			this._commandType = commandtype;
			_commandTypes.Add (this);
		}

    /// <summary>
    ///    Gets the command-type registered by the current instance.
    /// </summary>
    public string CommandType
    {
      get { return _commandType; }
    }

    /// <summary>
    ///    Gets all the command-types that have been registered with
    ///    <see cref="SupportedCommandType" />.
    /// </summary>
    public static IEnumerable<string> AllCommandTypes
    {
      get
      {
        foreach (SupportedCommandType type in _commandTypes)
          yield return type.CommandType;
      }
    }

  }
}