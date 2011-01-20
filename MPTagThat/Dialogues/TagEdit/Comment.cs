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
namespace MPTagThat.TagEdit
{
  public class Comment
  {
    #region Variables

    public Comment() {}

    public Comment(string desc, string lang, string text)
    {
      Description = desc;
      if (lang.Length > 3)
        Language = lang.Substring(0, 3);
      else
        Language = lang;

      Text = text;
    }

    public string Description { get; set; }

    public string Language { get; set; }

    public string Text { get; set; }

    #endregion
  }
}