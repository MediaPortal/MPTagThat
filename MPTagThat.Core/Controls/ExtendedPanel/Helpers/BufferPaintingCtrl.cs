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

using System.Windows.Forms;

#endregion

namespace Stepi.UI
{
  /// <summary>
  ///   An extension of the Panel class that enables double buffering(all painting occurs in WM_PAINT
  /// </summary>
  public class BufferPaintingCtrl : Panel
  {
    #region ctor

    protected BufferPaintingCtrl()
    {
      ///set up the control styles so that it support double buffering painting
      SetStyle(ControlStyles.AllPaintingInWmPaint |
               ControlStyles.UserPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.DoubleBuffer, true);

      UpdateStyles();
    }

    #endregion
  }
}