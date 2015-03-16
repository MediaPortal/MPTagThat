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
  /// This static class provides a mechanism for registering Command Types, to be used
  /// by CommandFactory
  /// </summary>
  public static class CommandTypes
  {
    #region Variables

    private static Dictionary<string, Type> _commandTypes;
    
    private static Type[] _staticCommandTypes = new Type[]
    {
      typeof (CmdIdentifyFile),
      typeof (CmdGetCoverArt),
      typeof (CmdGetLyrics),
      typeof (CmdRemoveComment),
      typeof (CmdRemoveCoverArt),
      typeof (CmdValidateMp3File),
      typeof (CmdFixMp3File),
      typeof (CmdReplayGain),
      typeof (CmdBpm),
      typeof (CmdCoverArtDrop),
      typeof (CmdSave),
    };

    #endregion

    #region ctor

    static CommandTypes()
    {
      Init();
    }

    #endregion

    #region private Methods

    /// <summary>
    ///    Initializes the class by registering the default types.
    /// </summary>
    internal static void Init()
    {
      if (_commandTypes != null)
        return;

      _commandTypes = new Dictionary<string, Type>();

      foreach (Type type in _staticCommandTypes)
        Register(type);
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///    Registers a Command subclass to be used when
    /// </summary>
    /// <param name="type">
    ///    A <see cref="Type" /> object for the class to register.
    /// </param>
    public static void Register(Type type)
    {
      Attribute[] attrs = Attribute.GetCustomAttributes(type, typeof(SupportedCommandType), false);

      if (attrs.Length == 0)
        return;	

      foreach (var attribute in attrs)
      {
        var attr = (SupportedCommandType) attribute;
        _commandTypes.Add(attr.CommandType, type);
      }
    }

    /// <summary>
    ///    Gets a dictionary containing all the supported Commands
    ///    />
    /// </summary>
    public static IDictionary<string, Type> AvailableCommands
    {
      get { return _commandTypes; }
    }

    #endregion
  }
}