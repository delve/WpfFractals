//-----------------------------------------------------------------------
// <copyright file="Fractal.cs" company="None">
//     MIT License (MIT)
//     Copyright (c) 2014 Grady Brandt
// </copyright>
// <author>Grady Brandt</author>
//-----------------------------------------------------------------------
namespace WpfFractals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Abstract class defines the most basic values and behaviors of all fractal objects
    /// </summary>
    public abstract class Fractal
    {
        #region Fields
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Fractal class.
        /// </summary>
        public Fractal()
        {
        }
        #endregion

        #region Delegates
        /// <summary>
        /// Delegates for handling status message events
        /// </summary>
        /// <param name="message">The status update message</param>
        public delegate void StatusDelegate(string message);
        #endregion

        #region Events
        /// <summary>
        /// Status update events
        /// </summary>
        public abstract event StatusDelegate StatusUpdate;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the canvas that the fractal will be drawn onto
        /// </summary>
        public Canvas FractalCanvas { get; set; }

        /// <summary>
        /// Gets or sets the maximum iteration count for the drawing recursion
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets the drawing speed factor.
        /// Controls the speed of the drawing process. 
        /// 0 is intended to mean "as fast as computationally possible" whereas any
        /// higher value is used to impose an implementation-specific means of slowing
        /// the rendering to achieve an animation-like effect. Typically 
        /// higher values = slower but this is at the discretion of the implementation.
        /// </summary>
        public int DrawSpeed { get; set; }

        /// <summary>
        /// Gets or sets the value tracking how many times CompositionTarget.Rendering has fired during rendering
        /// </summary>
        protected int RenderTicks { get; set; }

        /// <summary>
        /// Gets or sets the current fractal drawing depth.
        /// Tracks current depth of fractal rendering during the render process.
        /// </summary>
        protected int FractalDepth { get; set; }
        #endregion

        #region Event handlers
        /// <summary>
        /// Sets up to render the fractal to screen and starts the rendering process.
        /// This can be assigned to an event typically CompositionTarget.Rendering and
        /// calls the DrawFractal method
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">EventArgs event arguments</param>
        public abstract void StartRender(object sender, EventArgs e);
        #endregion

        #region Methods
        #endregion
    }
}
