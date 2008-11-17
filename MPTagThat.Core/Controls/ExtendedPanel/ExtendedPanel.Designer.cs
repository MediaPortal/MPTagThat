using System.Drawing;
using System.Windows.Forms;
namespace Stepi.UI
{
    partial class ExtendedPanel 
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            //add the caption control
            this.captionCtrl = new CaptionCtrl();
            this.captionCtrl.Width = this.Width;
            this.captionCtrl.Height = this.Height;
            this.captionCtrl.Location = new Point(0, 0);
            this.captionCtrl.BackColor = Color.Transparent;


            this.Controls.Add(captionCtrl);
            // 
            // ExtendedPanel
            //
            this.Name = "ExtendedPanel";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
