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

using System.Drawing;

#endregion

namespace Stepi.UI
{
  /// <summary>
  ///   Enumeration used for ExtendedPanel collapsing/expanding
  /// </summary>
  public enum Animation
  {
    No = 0,
    Yes = 1
  } ;

  /// <summary>
  ///   Enumeration for ExtendedPanel control to referr the 3 different status the control can be in
  /// </summary>
  public enum ExtendedPanelState
  {
    Expanded = 0,
    Collapsed = 1,
    Collapsing = 2,
    Expanding = 3
  } ;

  /// <summary>
  ///   How the corners are being rendered ; (used by CaptionCtrl to draw the borders
  /// </summary>
  public enum CornerStyle
  {
    Normal = 0,
    Rounded = 1
  } ;

  /// <summary>
  ///   Used by DirectionCtrl for drawing itself
  /// </summary>
  public enum DirectionStyle
  {
    Left = (RotateFlipType.Rotate180FlipNone),
    Up = (RotateFlipType.Rotate270FlipNone),
    Right = (RotateFlipType.RotateNoneFlipNone),
    Down = (RotateFlipType.Rotate90FlipNone)
  } ;

  /// <summary>
  ///   Used in caption control and frametext for the background
  /// </summary>
  public enum BrushType
  {
    Solid = 0,
    Gradient = 1
  } ;

  /// <summary>
  ///   Defines the way text should be animated
  /// </summary>
  public enum TextAnimationMode
  {
    None = 0,
    Fading = 1,
    Typing = 2
  } ;
}