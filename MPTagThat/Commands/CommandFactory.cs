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

namespace MPTagThat.Commands
{
  public class CommandFactory
  {

    public static ICommand Create(string command)
    {
      if (string.IsNullOrEmpty(command))
      {
        throw new Exception("Command must not be empty");
      }

      if (!CommandTypes.AvailableCommands.ContainsKey(command))
      {
        throw new Exception(string.Format("Command not supported: {0}", command));
      }

      Type command_type = CommandTypes.AvailableCommands[command];

      try
      {
        ICommand commandobj = (ICommand)Activator.CreateInstance(command_type, new object[] { });
        return commandobj;
      }
      catch (System.Reflection.TargetInvocationException e)
      {
        throw e.InnerException;
      }

    }

  }
}