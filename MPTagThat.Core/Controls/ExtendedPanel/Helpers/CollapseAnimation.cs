#region Using directives

using System;
using System.Threading;

#endregion

namespace Stepi.UI
{
    /// <summary>
    /// Event raised during animation notifying the new size for the panel
    /// </summary>
    /// <param name="sender">the object sending the notification</param>
    /// <param name="size">the new size for the window collasping/expanding </param>
    internal delegate void NotifyAnimationEvent(object sender,int size);

    /// <summary>
    /// Event raised when animation is finished
    /// </summary>
    /// <param name="sender"></param>
    internal delegate void NotifyAnimationFinishedEvent(object sender);

    /// <summary>
    /// Defines a class that creates a worker thread handling the collapsing/expanding animation
    /// </summary>
    internal class CollapseAnimation
    {
        #region Members
        
        /// <summary>
        /// The step used in collapsing/exapanding animation
        /// </summary>
        protected int step = 0;

        /// <summary>
        /// minimum size 
        /// </summary>
        protected int minimum = 0;

        /// <summary>
        /// maximum size
        /// </summary>
        protected int maximum = 0;

        /// <summary>
        /// THe worker thread 
        /// </summary>
        protected Thread thread = null;

        /// <summary>
        /// a wait handel used in starting the worker thread
        /// </summary>
        protected ManualResetEvent threadStart = new ManualResetEvent(false);

        /// <summary>
        /// handler for notifying the size has changed
        /// </summary>
        public event NotifyAnimationEvent NotifyAnimation = null;

        /// <summary>
        /// handler for notifying the animation has finished
        /// </summary>
        public event NotifyAnimationFinishedEvent NotifyAnimationFinished = null;
        #endregion

        #region ctor

        internal CollapseAnimation()
        {
        }

        internal CollapseAnimation(int step, int minimum, int maximum)
        {
            this.step = step;
            this.minimum = minimum;
            this.maximum = maximum;
        }

        #endregion

        #region Public

        public void Start()
        {
            if (step == 0)
            {
                throw new InvalidOperationException("Step can not be zero!");
            }
            if (minimum >= maximum)
            {
                throw new InvalidOperationException("Invalid parameters");
            }
            //create the working thread
            threadStart.Reset();
            thread = new Thread(new ThreadStart(Animate));
            thread.IsBackground = true;
            thread.Start();
            //waint until the thread has actually started
            threadStart.WaitOne();
        }

        #endregion

        #region Protected

        /// <summary>
        /// The processing method for the worker thread. Performs the "animation"
        /// </summary>
        protected void Animate()
        {
            //signal the calling thread that the worker started
            threadStart.Set();
            if (null != NotifyAnimation)
            {
                if (step > 0)
                {
                    while (maximum > minimum)
                    {
                        maximum -= step;
                        if (maximum < minimum)
                        {
                            maximum = minimum;
                        }
                        NotifyAnimation(this, maximum);
                        Thread.Sleep(20);
                    }
                    if (NotifyAnimationFinished != null)
                    {
                        NotifyAnimationFinished(this);
                    }
                }
                else
                {
                    while (maximum > minimum)
                    {
                        minimum -= step;
                        if (maximum < minimum)
                        {
                            minimum = maximum;
                        }
                        NotifyAnimation(this, minimum);
                        Thread.Sleep(20);
                    }
                    if (NotifyAnimationFinished != null)
                    {
                        NotifyAnimationFinished(this);
                    }
                }
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get/Set the animation step
        /// </summary>
        public int Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }

        /// <summary>
        /// Get/Set minimum value allowed for size
        /// </summary>
        public int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                minimum = value;
            }
        }

        /// <summary>
        /// Get/Set maximum value allowed for size
        /// </summary>
        public int Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
            }
        }
        #endregion
    }
}
