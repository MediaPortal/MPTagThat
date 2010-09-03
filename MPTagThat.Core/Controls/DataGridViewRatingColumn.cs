using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MPTagThat.Core
{
  public class DataGridViewRatingColumn : DataGridViewImageColumn
  {
    public DataGridViewRatingColumn()
    {
      this.CellTemplate = new RatingCell();
      this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      this.ValueType = typeof(int);
    }
  }

  public class RatingCell : DataGridViewImageCell
  {
    #region Variables
    static Image[] starImages;
    static Image[] starHotImages;
    const int IMAGEWIDTH = 58;
    #endregion

    // setup star images
    #region ctor
    static RatingCell()
    {
      starImages = new Image[6];
      starHotImages = new Image[6];
      // load normal stars
      for (int i = 0; i <= 5; i++)
        starImages[i] = (Image)MPTagThat.Core.Properties.Resources.ResourceManager.GetObject("star" + i.ToString());

      // load hot normal stars
      for (int i = 0; i <= 5; i++)
        starHotImages[i] = (Image)MPTagThat.Core.Properties.Resources.ResourceManager.GetObject("starhot" + i.ToString());
    }

    public RatingCell()
    {
      // Value type is an integer.
      // Formatted value type is an image since we derive from the ImageCell
      this.ValueType = typeof(int);
    }
    #endregion

    protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
    {
      // Check for a valid range
      if ((int)value < 0 || (int)value > 5)
        return starImages[0];

      // Convert integer to star images
      return starImages[(int)value];
    }

    public override object DefaultNewRowValue
    {
      // default new row to 0 stars
      get { return 0; }
    }

    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
      Image cellImage = (Image)formattedValue;

      int starNumber = GetStarFromMouse(cellBounds, this.DataGridView.PointToClient(Control.MousePosition));

      if (starNumber != -1)
        cellImage = starHotImages[starNumber];

      // surpress painting of selection
      base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, cellImage, errorText, cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.SelectionBackground));
    }

    // Update cell's value when the user clicks on a star
    protected override void OnContentClick(DataGridViewCellEventArgs e)
    {
      base.OnContentClick(e);
      int starNumber = GetStarFromMouse(this.DataGridView.GetCellDisplayRectangle(this.DataGridView.CurrentCellAddress.X, this.DataGridView.CurrentCellAddress.Y, false), this.DataGridView.PointToClient(Control.MousePosition));

      if (starNumber != -1)
        this.Value = starNumber;
    }

    #region Invalidate cells when mouse moves or leaves the cell
    protected override void OnMouseLeave(int rowIndex)
    {
      base.OnMouseLeave(rowIndex);
      this.DataGridView.InvalidateCell(this);
    }

    protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
    {
      base.OnMouseMove(e);
      this.DataGridView.InvalidateCell(this);
    }
    #endregion

    #region Private Implementation

    private int GetStarFromMouse(Rectangle cellBounds, Point mouseLocation)
    {
      if (cellBounds.Contains(mouseLocation))
      {

        int mouseXRelativeToCell = (mouseLocation.X - cellBounds.X);
        int imageXArea = (cellBounds.Width / 2) - (IMAGEWIDTH / 2);
        if (((mouseXRelativeToCell + 4) < imageXArea) || (mouseXRelativeToCell >= (imageXArea + IMAGEWIDTH)))
          return -1;
        else
        {
          int oo = (int)Math.Round((((float)(mouseXRelativeToCell - imageXArea + 5) / (float)IMAGEWIDTH) * 5f), MidpointRounding.AwayFromZero);
          if (oo > 5 || oo < 0) System.Diagnostics.Debugger.Break();
          return oo;
        }
      }
      else
        return -1;
    }
    #endregion

  }
}
