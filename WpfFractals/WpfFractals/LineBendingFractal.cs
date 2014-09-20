﻿//-----------------------------------------------------------------------
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

    public class LineBendingFractal : Fractal
    {
        #region Fields
        private double distanceScale = 1.0 / 3;
        double[] deltaTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };
        Polyline pl;
        private Point snowflakePoint = new Point();
        private double snowflakeSize;
        // state indicator, controls execution of pre-render setup in DrawFractal
        private bool rendering;
        #endregion

        #region Constructors
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
        protected override void DrawFractal()
        {
            if (false == this.rendering)
            {
                // first cycle setup
                this.rendering = true;
                pl = new Polyline();
                this.FractalCanvas.Children.Add(this.pl);

                // determine the size of the snowflake
                double sizeY = 0.8 * FractalCanvas.Height / (Math.Sqrt(3) * 4 / 3);
                double sizeX = 0.8 * FractalCanvas.Width / 2;
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
            pl.Points.Add(pta[0]);

            for (int j = 1; j < pta.Length; j++)
            {
                double x1 = pta[j - 1].X;
                double y1 = pta[j - 1].Y;
                double x2 = pta[j].X;
                double y2 = pta[j].Y;
                double dx = x2 - x1;
                double dy = y2 - y1;
                double theta = Math.Atan2(dy, dx);
                snowflakePoint = new Point(x1, y1);
                SnowFlakeEdge(canvas, depth, theta, length);
            }
        }

        private void SnowFlakeEdge(Canvas canvas, int depth, double theta, double distance)
        {
            Point pt = new Point();

            if (depth <= 0)
            {
                pt.X = snowflakePoint.X + (distance * Math.Cos(theta));
                pt.Y = snowflakePoint.Y + (distance * Math.Sin(theta));
                pl.Points.Add(pt);
                snowflakePoint = pt;
                return;
            }

            distance *= distanceScale;
            for (int j = 0; j < 4; j++)
            {
                theta += deltaTheta[j];
                SnowFlakeEdge(canvas, depth - 1, theta, distance);
            }
        }
        #endregion
    }
}