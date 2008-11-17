#region Using directives
using System.Windows.Forms;
#endregion

namespace Stepi.UI
{
    /// <summary>
    /// An extension of the Panel class that enables double buffering(all painting occurs in WM_PAINT
    /// </summary>
    public class BufferPaintingCtrl : Panel
    {
        #region ctor
        protected BufferPaintingCtrl()
        {
            ///set up the control styles so that it support double buffering painting
            this.SetStyle(  ControlStyles.AllPaintingInWmPaint |
                            ControlStyles.UserPaint |
                            ControlStyles.OptimizedDoubleBuffer |
                            ControlStyles.DoubleBuffer,true);

            UpdateStyles();
            
        }
        #endregion
    }
}
