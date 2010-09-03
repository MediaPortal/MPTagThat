#region Using Directives

using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.ComponentModel;

#endregion

namespace Stepi.UI
{
    /// <summary>
    /// Raised when the direction pointed by this control changes
    /// </summary>
    /// <param name="sender">instance of a directioncontrol object</param>
    /// <param name="e">instance of an object holding the data for the event</param>
    public delegate void DirectionCtrlStyleChangedEvent(object sender, ChangeStyleEventArgs e);

    /// <summary>
    /// A "direction" control class
    /// </summary>
    public class DirectionCtrl : BufferPaintingCtrl
    {
        #region Members

        /// <summary>
        /// Instance of a bool variable telling wheather the mouse is inside this control or not
        /// </summary>
        private bool mouseInside = false;

        /// <summary>
        /// Color used in drawing the arrow
        /// </summary>
        private Color color = Color.DarkGray;

        /// <summary>
        /// Color used in drawing the arrow while the mouse pointer is inside this control
        /// </summary>
        private Color hoverColor = Color.Orange;

        /// <summary>
        /// Holds the direction pointed by this control
        /// </summary>
        private DirectionStyle directionStyle = DirectionStyle.Up;

        /// <summary>
        /// Instance of the image being painted inside the control
        /// </summary>
        private Image image = null;

        /// <summary>
        /// Instance of the image being painted inside the control while the mouse is inside
        /// </summary>
        private Image imageHover = null;

        /// <summary>
        /// Handler for the change "direction" event 
        /// </summary>
        public event DirectionCtrlStyleChangedEvent handlerStyleChange = null;
        #endregion

        #region ctor
        public DirectionCtrl():base()
        {
            //set up the mouse events
            this.MouseEnter += new EventHandler(OnMouseEnterEvent);       
            this.MouseLeave += new EventHandler(OnMouseLeaveEvent);
            this.MouseClick += new MouseEventHandler(OnMouseClickEvent);
            //InitializeGraphicPath();
        }

        
        #endregion

        #region Properties
        [Category("Apperance")]
        [Description("Get/Set where this control points to")]
        [DefaultValue(DirectionStyle.Up)]
        public DirectionStyle DirectionStyle
        {
            get
            {
                return directionStyle;
            }
            set
            {
                if (directionStyle != value)
                {
                    image.Dispose();
                    image = null;
                    
                }
                directionStyle = value;
            }
        }

        [Category("Appearance")]
        [Description("Get/Set the color used for the direction control")]
        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                if (value != color)
                {
                    color = value;
                    InitializeImage();
                }
            }
        }

        [Category("Appearance")]
        [Description("Get/Set the color used for the direction control")]
        public Color HoverColor
        {
            get
            {
                return hoverColor;
            }

            set
            {
                if (value != hoverColor)
                {
                    hoverColor = value;
                    InitializeImage();
                }
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// Creates the two images used in the WM_PAINT handler
        /// </summary>
        private void InitializeImage()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
            if (imageHover != null)
            {
                imageHover.Dispose();
                imageHover = null;
            }

            Brush brush = new SolidBrush(color);

            image = CreateImage(brush,true);

            brush.Dispose();
            brush = new SolidBrush(hoverColor);

            imageHover = CreateImage(brush,false);
            brush.Dispose();
        }

        /// <summary>
        /// Method used in creating the two images
        /// </summary>
        /// <param name="brush">the brush used to draw the arrow</param>
        /// <param name="flag">true if the mouse is inside the control</param>
        /// <returns>a bitmap image displaying an arrow</returns>
        private Image CreateImage(Brush brush,bool flag)
        {
            string imgText = "»";
            
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;


            int imageWidth = this.Width;
            int imageHeight = this.Height;

            if (imageWidth == 0)
            {
                imageWidth = 1;
            }
            if (imageHeight == 0)
            {
                imageHeight = 1;
            }
            Image image = new Bitmap(imageWidth, imageHeight, PixelFormat.Format32bppPArgb);
            //create the Graphics object
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Transparent);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            SizeF sizeString = g.MeasureString(imgText, this.Font);

            Font font = null;
            if (flag)
            {
                font = new Font("Arial", 12, FontStyle.Bold);
            }
            else
            {
                font = new Font("Arial", 15, FontStyle.Bold);
            }
            g.DrawString(imgText, font, brush, new RectangleF(0, 0, this.Width, this.Height), format);
            g.Dispose();
            image.RotateFlip((RotateFlipType)((int)directionStyle));
            return image;
        }

        #endregion

        #region WM_PAINT
        /// <summary>
        /// Handles the paint event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (null == image)
            {
                InitializeImage();
            }

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            
            if (!mouseInside)
            {
                e.Graphics.DrawImage(image, new Point(0, 0));
            }
            else
            {
                e.Graphics.DrawImage(imageHover, new Point(0, 0));
            }
            base.OnPaint(e);
        }
        #endregion

        #region Override
        /// <summary>
        /// If resized the images displaying the arrow have to be recreated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            InitializeImage();
            Update();
        }
      
        #endregion

        #region Events

        void OnMouseEnterEvent(object sender, EventArgs e)
        {
            
            mouseInside = true;
            
            Refresh();
        }

        void OnMouseLeaveEvent(object sender, EventArgs e)
        {
            
            mouseInside = false;
            
            Refresh();
        }

        void OnMouseClickEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DirectionStyle oldStyle = directionStyle;
            switch (directionStyle)
            {
                case DirectionStyle.Up:
                    directionStyle = DirectionStyle.Down;
                    break;

                case DirectionStyle.Down:
                    directionStyle = DirectionStyle.Up;
                    break;

                case DirectionStyle.Left:
                    directionStyle = DirectionStyle.Right;
                    break;

                case DirectionStyle.Right:
                    directionStyle = DirectionStyle.Left;
                    break;
            }
            InitializeImage();
           // Update();
            if (handlerStyleChange != null)
            {
                ChangeStyleEventArgs args = new ChangeStyleEventArgs(oldStyle,directionStyle);
                handlerStyleChange(this, args);
            }
        }
        #endregion

    }
}
