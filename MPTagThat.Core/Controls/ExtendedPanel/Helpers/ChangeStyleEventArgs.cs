#region Using directives
using System;
using System.Text;
#endregion

namespace Stepi.UI
{
    /// <summary>
    /// Class used to send the data to the event raised as a consequence of the "expanding/collapsing" button being hit
    /// </summary>
    public class ChangeStyleEventArgs : EventArgs
    {
        #region Members
        /// <summary>
        /// Old direction
        /// </summary>
        private DirectionStyle oldStyle;
        /// <summary>
        /// New direction
        /// </summary>
        private DirectionStyle newStyle;
        #endregion

        #region ctor
        public ChangeStyleEventArgs()
        { }

        public ChangeStyleEventArgs(DirectionStyle oldStyle, DirectionStyle newStyle)
        {
            this.oldStyle = oldStyle;
            this.newStyle = newStyle;
        }

        #endregion

        #region Properties
        public DirectionStyle Old
        {
            get
            {
                return oldStyle;
            }
            set
            {
                oldStyle = value;
            }
        }

        public DirectionStyle New
        {
            get
            {
                return newStyle;
            }

            set
            {
                newStyle = value;
            }
        }
        #endregion

    }
}
