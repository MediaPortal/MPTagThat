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

namespace MPTagThat.Core.Burning
{
  public enum BurnStatus
  {
    Unknown = 0,
    Finished = 1,
    LeadOut = 2,
    Burning = 3,
    LeadIn = 4,
    Converting = 5,
    Caching = 6,
    Checking = 7,
    Blanking = 8,
  }
}