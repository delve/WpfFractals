//-----------------------------------------------------------------------
// <copyright file="LineBendingFractal.cs" company="None">
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
    /// Implements a fractal constructed of algorithmically bending a line (polyline)
    /// </summary>
    public class LineBendingFractal : Fractal
    {
        #region Fields
        /// <summary>
        /// Scale of child segments
        /// </summary>
        private double distanceScale = 1.0 / 3;

        /// <summary>
        /// Array of offset angles for each fold
        /// </summary>
        private double[] deltaTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };

        /// <summary>
        /// The polyline to be folded into a fractal shape
        /// </summary>
        private Polyline pl;

        /// <summary>
        /// Defines the legnth of the sides of the base shape
        /// </summary>
        private double baseShapeSize;

        /// <summary>
        /// The current starting 'point' of the rendering process.
        /// Carries the progress of the polyline back up the recursion chain for the next recursive dive
        /// </summary>
        private Point fractalPoint = new Point();

        /// <summary>
        /// state indicator, controls execution of pre-render setup in DrawFractal
        /// </summary>
        private bool rendering;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the LineBendingFractal class.
        /// </summary>
        public LineBendingFractal()
        {
            this.MaxDepth = 5;
            this.DrawSpeed = 10;
            this.rendering = false;
        }
        #endregion

        #region Events
        /// <summary>
        /// Status update events
        /// </summary>
        public override event StatusDelegate StatusUpdate;
        #endregion

        #region Properties
        #endregion

        #region Event Handlers
        #endregion

        #region Methods
        /// <summary>
        /// Executes one pass of the iterative process of animating the fractal
        /// </summary>
        protected override void DrawFractal()
        {
            // first cycle setup
            // We don't want to re-calc all this every rendering cycle.
            if (false == this.rendering)
            {
                // basic setup
                this.rendering = true;
                this.pl = new Polyline();
                this.FractalCanvas.Children.Add(this.pl);
                this.pl.Stroke = Brushes.Blue;

                //ensure these control properties are reset
                this.RenderTicks = 0;
                this.FractalDepth = 0;

                // determine the size of the initial trianle sides
                double sizeY = 0.8 * this.FractalCanvas.Height / (Math.Sqrt(3) * 4 / 3);
                double sizeX = 0.8 * this.FractalCanvas.Width / 2;
                double size = 0;
                if (sizeY < sizeX)
                {
                    size = sizeY;
                }
                else
                {
                    size = sizeX;
                }

                this.baseShapeSize = 2 * size;
            }

            // clear the fractal in advance of this animation step
            this.pl.Points.Clear();

            // draw this animation step
            this.DrawBaseShape(this.FractalCanvas, this.baseShapeSize, this.FractalDepth);
                
            this.StatusUpdate("Snowflake - Depth = " + this.FractalDepth.ToString() + ". # of Polyline points = " + this.pl.Points.Count.ToString());
            if (this.FractalDepth > this.MaxDepth || this.FractalDepth < 0)
            {
                this.StatusUpdate("Snowflake - Depth = " + this.FractalDepth.ToString() + ". Finished. # of Polyline points = " + this.pl.Points.Count.ToString());

                // Rendering is complete, cleanup
                CompositionTarget.Rendering -= this.StartRender;
                this.RenderTicks = 0;
                this.FractalDepth = 0;
                this.rendering = false;
            }
            this.FractalDepth += 1;
        } 

        /// <summary>
        /// Calculates the 3 points of the polyline that form the base triangle (plus the starting point)
        /// </summary>
        /// <param name="canvas">The canvas to draw on</param>
        /// <param name="length">Length of the sides of the initial triangle</param>
        /// <param name="depth">Current drawing depth</param>
        private void DrawBaseShape(Canvas canvas, double length, int depth)
        {
            double xmid = canvas.Width / 2;
            double ymid = canvas.Height / 2;
            Point[] basePoints = new Point[4];

            // define the bottom point
            basePoints[0] = new Point(
                xmid,
                ymid + (length / 2 * Math.Sqrt(3) * 2 / 3));

            // define the upper left point
            basePoints[1] = new Point(
                xmid + (length / 2),
                ymid - (length / 2 * Math.Sqrt(3) / 3));

            // define the upper right point
            basePoints[2] = new Point(
                xmid - (length / 2),
                ymid - (length / 2 * Math.Sqrt(3) / 3));

            // and back to the bottom
            basePoints[3] = basePoints[0];
            this.pl.Points.Add(basePoints[0]);

            // recurse down each leg of the triangle building out the fractal depth
            for (int j = 1; j < basePoints.Length; j++)
            {
                double x1 = basePoints[j - 1].X;
                double y1 = basePoints[j - 1].Y;
                double x2 = basePoints[j].X;
                double y2 = basePoints[j].Y;
                double dx = x2 - x1;
                double dy = y2 - y1;
                double theta = Math.Atan2(dy, dx);
                this.fractalPoint = new Point(x1, y1);
                this.AddFractalPoints(canvas, depth, theta, length);
            }
        }

        /// <summary>
        /// Draws the edges of the snowflake by adding points to the polyline object
        /// </summary>
        /// <param name="canvas">The canvas to draw on</param>
        /// <param name="depth">Current drawing depth</param>
        /// <param name="theta">Angle of the line currently being folded</param>
        /// <param name="distance">Length of the line segments at this depth</param>
        private void AddFractalPoints(Canvas canvas, int depth, double theta, double distance)
        {
            Point pt = new Point();

            if (depth <= 0)
            {
                // we've reached the bottom of the recursion this animation frame
                // calculate the next point based on the fully modified segment length and angle
                pt.X = this.fractalPoint.X + (distance * Math.Cos(theta));
                pt.Y = this.fractalPoint.Y + (distance * Math.Sin(theta));
                this.pl.Points.Add(pt);

                // save this point for the next time we reach the bottom of the recursion
                this.fractalPoint = pt;

                // close this leg of the recursion
                return;
            }

            // Adjust the length for the next layer down
            distance *= this.distanceScale;
            for (int angleIndex = 0; angleIndex < this.deltaTheta.Length; angleIndex++)
            {
                // Adjust the line angle for each of the points at the next layer
                theta += this.deltaTheta[angleIndex];
                this.AddFractalPoints(canvas, depth - 1, theta, distance);
            }
        }
        #endregion
    }
}
