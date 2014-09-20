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
        /// Scale of child folds
        /// </summary>
        private double distanceScale = 1.0 / 3;

        /// <summary>
        /// Array of offset angles for each triangular fold
        /// </summary>
        private double[] deltaTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };

        /// <summary>
        /// The polyline to be folded into a fractal shape
        /// </summary>
        private Polyline pl;

        /// <summary>
        /// A point, need to grok the algorithm better to know what it really does
        /// </summary>
        private Point snowflakePoint = new Point();

        /// <summary>
        /// need to grok the algorithm better to know what it really does
        /// </summary>
        private double snowflakeSize;

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
        /// Executes one pass of the iterative process of drawing the fractal
        /// </summary>
        protected override void DrawFractal()
        {
                // first cycle setup
            if (false == this.rendering)
            {
                this.rendering = true;
                this.pl = new Polyline();
                this.FractalCanvas.Children.Add(this.pl);

                // determine the size of the snowflake
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

                this.snowflakeSize = 2 * size;
                this.pl.Stroke = Brushes.Blue;
                this.RenderTicks = 0;
                this.FractalDepth = 0;
            }

            this.pl.Points.Clear();
            this.DrawSnowFlake(this.FractalCanvas, this.snowflakeSize, this.FractalDepth);
                
            this.StatusUpdate("Koch Snowflake - Depth = " + this.FractalDepth.ToString() + ". # of Polyline points = " + this.pl.Points.Count.ToString());
            this.FractalDepth += 1;
            if (this.FractalDepth > this.MaxDepth || this.FractalDepth < 0)
            {
                this.StatusUpdate("Koch Snowflake - Depth = " + this.FractalDepth.ToString() + ". Finished. # of Polyline points = " + this.pl.Points.Count.ToString());

                // Rendering is complete, cleanup
                CompositionTarget.Rendering -= this.StartRender;
                this.RenderTicks = 0;
                this.FractalDepth = 0;
                this.rendering = false;
            }
        } 

        /// <summary>
        /// Adds a single fold pattern to the polyline
        /// </summary>
        /// <param name="canvas">The canvas to draw on</param>
        /// <param name="length">Length of the sides of the initial triangle</param>
        /// <param name="depth">Current drawing depth</param>
        private void DrawSnowFlake(Canvas canvas, double length, int depth)
        {
            double xmid = canvas.Width / 2;
            double ymid = canvas.Height / 2;
            Point[] pta = new Point[4];
            pta[0] = new Point(
                xmid,
                ymid + (length / 2 * Math.Sqrt(3) * 2 / 3));
            pta[1] = new Point(
                xmid + (length / 2),
                ymid - (length / 2 * Math.Sqrt(3) / 3));
            pta[2] = new Point(
                xmid - (length / 2),
                ymid - (length / 2 * Math.Sqrt(3) / 3));
            pta[3] = pta[0];
            this.pl.Points.Add(pta[0]);

            for (int j = 1; j < pta.Length; j++)
            {
                double x1 = pta[j - 1].X;
                double y1 = pta[j - 1].Y;
                double x2 = pta[j].X;
                double y2 = pta[j].Y;
                double dx = x2 - x1;
                double dy = y2 - y1;
                double theta = Math.Atan2(dy, dx);
                this.snowflakePoint = new Point(x1, y1);
                this.SnowFlakeEdge(canvas, depth, theta, length);
            }
        }

        /// <summary>
        /// Draws the edges of the snowflake
        /// </summary>
        /// <param name="canvas">The canvas to draw on</param>
        /// <param name="depth">Current drawing depth</param>
        /// <param name="theta">Angle offsets</param>
        /// <param name="distance">Length of the line segments at this depth</param>
        private void SnowFlakeEdge(Canvas canvas, int depth, double theta, double distance)
        {
            Point pt = new Point();

            if (depth <= 0)
            {
                pt.X = this.snowflakePoint.X + (distance * Math.Cos(theta));
                pt.Y = this.snowflakePoint.Y + (distance * Math.Sin(theta));
                this.pl.Points.Add(pt);
                this.snowflakePoint = pt;
                return;
            }

            distance *= this.distanceScale;
            for (int j = 0; j < 4; j++)
            {
                theta += this.deltaTheta[j];
                this.SnowFlakeEdge(canvas, depth - 1, theta, distance);
            }
        }
        #endregion
    }
}
