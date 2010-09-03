#region Using directives

using System;

#endregion

namespace Stepi.UI
{
    public class CaptionDraggingEventArgs :EventArgs
    {
        #region Members
        /// <summary>
        /// Instance of the width change
        /// </summary>
        private int width = 0;

        /// <summary>
        /// Instance of the height change
        /// </summary>
        private int height = 0;
        #endregion

        #region ctor
        public CaptionDraggingEventArgs()
        { }

        public CaptionDraggingEventArgs(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        #endregion

        #region Properties

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        #endregion
    }
}
