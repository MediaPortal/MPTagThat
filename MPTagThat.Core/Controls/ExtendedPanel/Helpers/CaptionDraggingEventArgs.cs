#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
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

#endregion

namespace Stepi.UI
{
  public class CaptionDraggingEventArgs : EventArgs
  {
    #region Members

    #endregion

    #region ctor

    public CaptionDraggingEventArgs() {}

    public CaptionDraggingEventArgs(int width, int height)
    {
      this.Width = width;
      this.Height = height;
    }

    #endregion

    #region Properties

    public int Width { get; set; }

    public int Height { get; set; }

    #endregion
  }
}