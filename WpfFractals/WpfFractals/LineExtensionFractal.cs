﻿//-----------------------------------------------------------------------
// <copyright file="LineExtensionFractal.cs" company="None">
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// a class
    /// </summary>
    public class LineExtensionFractal : Fractal
    {
        #region Fields
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the LineExtensionFractal class.
        /// This class is used to draw fractals by algorithmically adding additional line
        /// segments onto each parent.
        /// </summary>
        public LineExtensionFractal()
            : this(1, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LineExtensionFractal class.
        /// This class is used to draw fractals by algorithmically adding additional line
        /// segments onto each parent.
        /// </summary>
        /// <param name="minPixels">Sets the minimum size in pixels per line segment to use as an escape value for the recursion</param>
        public LineExtensionFractal(int minPixels)
            : this(minPixels, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LineExtensionFractal class.
        /// This class is used to draw fractals by algorithmically adding additional line
        /// segments onto each parent.
        /// </summary>
        /// <param name="minPixels">Sets the minimum size in pixels per line segment to use as an escape value for the recursion. 
        /// 0 will draw the fractal to the defined maximum depth, or depth 1 if depth is also set to 0.</param>
        /// <param name="depth">Sets the maximum depth to use in the recursion. 
        /// 0 will draw the fractal to the defined minimum line segment length, or depth 1 if minPixels is also set to 0.</param>
        /// <param name="speed">Roughly controls the drawing speed by skipping 'speed' number of rendering cycles between depth renderings</param>
        /// <param name="angleDelta">Child branchs' angle +/- delta from the parent</param>
        /// <param name="childScale">Ratio of child branchs' length to parents'</param>
        public LineExtensionFractal(int minPixels, int depth = 0, int speed = 0, double angleDelta = Math.PI / 5, double childScale = 0.75)
        {
            this.MinSize = minPixels;
            this.MaxDepth = depth;
            this.DrawSpeed = speed;
            this.DeltaTheta = angleDelta;
            this.LengthScale = childScale;

            // Safety check. MinSize 0 
            if (0 == this.MinSize && 0 == this.MaxDepth)
            {
                this.MaxDepth = 1;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the minimum size of a line segment in pixels (ending point for the recursion, defaults 0)
        /// </summary>
        public int MinSize { get; set; }

        /// <summary>
        /// Gets or sets the length ratio of child : parent branch.
        /// </summary>
        public double LengthScale { get; set; }

        /// <summary>
        /// Gets or sets the angle +/- delta for child branches
        /// </summary>
        public double DeltaTheta { get; set; }        
        #endregion

        #region Event handlers
        /// <summary>
        /// Sets up to render the fractal to screen and starts the rendering process
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">EventArgs event arguments</param>
        public override void StartRender(object sender, EventArgs e)
        {
            // Sanity check, we need a canvas in order to render
            // TODO: factor this into the partent somehow (it's an abstract class, so how?)
            if (null == this.FractalCanvas)
            {
                return;
            }

            // Track how many times the 'CompositionTarget.Rendering' event fires in order to slow down the render animation.
            this.RenderTicks += 1;
            if (0 == this.RenderTicks % this.DrawSpeed)
            {
                this.FractalCanvas.Children.Clear();

                // Start the actual rendering
                this.DrawBinaryTreeBranch(
                    this.FractalCanvas,
                    this.FractalDepth,
                    new Point(this.FractalCanvas.Width / 2, 0.83 * this.FractalCanvas.Height),
                    0.2 * this.FractalCanvas.Width,
                    -Math.PI / 2);

                // TODO: re-implement this somehow
                ////this.statbarMessage.Text = "Binary Tree - Depth = " + this.fractalDepth.ToString() + ". # of Branches = " + this.fractalCanvas.Children.Count;
                this.FractalDepth += 1;
                if (this.FractalDepth > 10)
                {
                    // TODO: re-implement this somehow
                    ////this.statbarMessage.Text = "Binary Tree - Depth = 10. Finished. # of Branches = " + this.fractalCanvas.Children.Count;
                    CompositionTarget.Rendering -= this.StartRender;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws the fractal upon the parameter canvas object.
        /// </summary>
        /// <param name="canvas">The canvas to be drawn upon</param>
        protected override void DrawFractal(System.Windows.Controls.Canvas canvas)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws a single branch of the binary tree and, if not at the recursion limit, calls DrawBinaryTreeBranch for each of this branch's children
        /// </summary>
        /// <param name="canvas">Target canvas</param>
        /// <param name="depth">Current drawing depth (recursion limit)</param>
        /// <param name="pt">Point representing the starting location for this branch</param>
        /// <param name="length">The length of the line segment to draw for this branch</param>
        /// <param name="theta">The angle this line segment will be drawn at</param>
        private void DrawBinaryTreeBranch(Canvas canvas, int depth, Point pt, double length, double theta)
        {
            // define the endpoint of this line segment using tangent length and angle (theta)
            //  in other words, Pythagorean theorem
            double endX = pt.X + (length * Math.Cos(theta));
            double endY = pt.Y + (length * Math.Sin(theta));

            // create this line segment
            Line line = new Line();
            line.Stroke = Brushes.Green;
            line.X1 = pt.X;
            line.Y1 = pt.Y;
            line.X2 = endX;
            line.Y2 = endY;

            // and stick it on the canvas
            canvas.Children.Add(line);

            // We still have depth remaining to plumb, so draw the next two segments
            if (depth > 1)
            {
                // draw the +theta segment
                this.DrawBinaryTreeBranch(
                    canvas,
                    depth - 1,
                    new Point(endX, endY),
                    length * this.LengthScale,
                    theta + this.DeltaTheta);

                // draw the -theta segment
                this.DrawBinaryTreeBranch(
                    canvas,
                    depth - 1,
                    new Point(endX, endY),
                    length * this.LengthScale,
                    theta - this.DeltaTheta);

                ////// draw the +1/2theta segment
                ////this.DrawBinaryTreeBranch(
                ////    canvas,
                ////    depth - 1,
                ////    new Point(endX, endY),
                ////    length * this.lengthScale,
                ////    theta + this.deltaTheta/2);

                ////// draw the -1/2theta segment
                ////this.DrawBinaryTreeBranch(
                ////    canvas,
                ////    depth - 1,
                ////    new Point(endX, endY),
                ////    length * this.lengthScale,
                ////    theta - this.deltaTheta/2);

                ////// draw a neutral theta segment
                ////this.DrawBinaryTreeBranch(
                ////    canvas,
                ////    depth - 1,
                ////    new Point(endX, endY),
                ////    length * this.lengthScale,
                ////    theta);
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}