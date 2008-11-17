using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public class Item
  {
    public string Name;
    public string Value;
    public string ToolTip;

    public Item(string name, string value, string tooltip)
    {
      Name = name;
      Value = value;
      ToolTip = tooltip;
    }

    public override string ToString()
    {
      // Generates the text shown in the combo box
      return Name;
    }
  }
}
