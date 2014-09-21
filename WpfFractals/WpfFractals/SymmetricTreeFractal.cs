//-----------------------------------------------------------------------
// <copyright file="SymmetricTreeFractal.cs" company="None">
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
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// a class
    /// </summary>
    public class SymmetricTreeFractal : Fractal
    {
        #region Fields
        /// <summary>
        /// UI control object for this.ChildCount
        /// </summary>
        private TextBox uiChildren;

        /// <summary>
        /// UI control object for this.MaxDepth
        /// </summary>
        private TextBox uiDepth;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the SymmetricTreeFractal class.
        /// This class is used to draw fractals by algorithmically adding additional line
        /// segments onto each parent.
        /// </summary>
        public SymmetricTreeFractal()
            : this(1, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SymmetricTreeFractal class.
        /// This class is used to draw fractals by algorithmically adding additional line
        /// segments onto each parent.
        /// </summary>
        /// <param name="minPixels">Sets the minimum size in pixels per line segment to use as an escape value for the recursion</param>
        public SymmetricTreeFractal(int minPixels)
            : this(minPixels, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SymmetricTreeFractal class.
        /// This class is used to draw fractals by algorithmically adding additional line
        /// segments onto each parent.
        /// </summary>
        /// <param name="minPixels">Sets the minimum size in pixels per line segment to use as an escape value for the recursion. 
        /// 0 will draw the fractal to the defined maximum depth, or depth 1 if depth is also set to 0.</param>
        /// <param name="depth">Sets the maximum depth to use in the recursion. 
        /// 0 will draw the fractal to the defined minimum line segment length, or depth 1 if minPixels is also set to 0.</param>
        /// <param name="speed">Roughly controls the drawing speed by skipping 'speed' number of rendering cycles between depth renderings. Higher = slower</param>
        /// <param name="angleDelta">Child branchs' angle +/- delta from the parent</param>
        /// <param name="childScale">Ratio of child branchs' length to parents'</param>
        /// <param name="childOffset">The offset of the child lines as a percentage of parent segment length</param>
        /// <param name="childOffsetRotation">The angle of rotation from the theta of the parent segment applied to the offset</param>
        /// <param name="children">Number of child branches</param>
        public SymmetricTreeFractal(int minPixels, int depth, int speed = 1, double angleDelta = 2 * Math.PI / 5, double childScale = 0.75, double childOffset = 0, double childOffsetRotation = 0, int children = 2)
        {
            this.MinSize = minPixels;
            this.MaxDepth = depth;
            this.DrawSpeed = speed;
            this.DeltaTheta = angleDelta;
            this.ChildScale = childScale;
            this.RenderTicks = 0;
            this.FractalDepth = 0;
            this.ChildOffset = childOffset;
            this.ChildOffsetRotation = childOffsetRotation;
            this.ChildCount = children;

            // Safety check. MinSize 0 
            if (0 == this.MinSize && 0 == this.MaxDepth)
            {
                this.MaxDepth = 1;
            }

            // deal with the controls
            Label label;

            // Number of children
            label = new Label { Content = "Branches" };
            this.uiChildren = new TextBox { Width = 20, DataContext = this };
            this.uiChildren.SetBinding(TextBox.TextProperty, new Binding("ChildCount") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            this.FractalParameterControls.Add(label);
            this.FractalParameterControls.Add(this.uiChildren);

            label = new Label { Content = "Depth" };
            this.uiDepth = new TextBox { Width = 30, DataContext = this };
            this.uiDepth.SetBinding(TextBox.TextProperty, new Binding("MaxDepth") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            this.FractalParameterControls.Add(label);
            this.FractalParameterControls.Add(this.uiDepth);
        }
        #endregion

        #region Events
        /// <summary>
        /// Status update events
        /// </summary>
        public override event StatusDelegate StatusUpdate;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the minimum size of a line segment in pixels (ending point for the recursion, defaults 0)
        /// </summary>
        public int MinSize { get; set; }

        /// <summary>
        /// Gets or sets the length ratio of child : parent branch.
        /// </summary>
        public double ChildScale { get; set; }

        /// <summary>
        /// Gets or sets the total angle +/- delta which is divided among the total number of child branches
        /// </summary>
        public double DeltaTheta { get; set; }

        /// <summary>
        /// Gets or sets the offset of the child lines as a percentage of parent segment length.
        /// Starting point of child line segments will be moved back from the end of the parent 
        /// segment by this percentage of the parent line length.
        /// Expeced values 0.0->1.0, values outside this range will cause the fractal to be non-contiguous
        /// </summary>
        public double ChildOffset { get; set; }

        /// <summary>
        /// Gets or sets the angle of rotation from the theta of the parent segment applied to the offset.
        /// If ChildOffset is 1.0 (no offset) this property is meaningless. This value is added to the
        /// parent's theta prior to calculating the offset point.
        /// </summary>
        public double ChildOffsetRotation { get; set; }

        /// <summary>
        /// Gets or sets the number of children for each parent branch
        /// </summary>
        public int ChildCount { get; set; }
        #endregion

        #region Event handlers
        #endregion

        #region Methods
        /// <summary>
        /// Executes one pass of the iterative process of drawing the fractal
        /// </summary>
        protected override void DrawFractal()
        {
            this.FractalCanvas.Children.Clear();

            // Start the actual rendering
            this.DrawBranch(
                this.FractalCanvas,
                this.FractalDepth,
                new Point(this.FractalCanvas.Width / 2, 0.83 * this.FractalCanvas.Height),
                0.2 * this.FractalCanvas.Width,
                -Math.PI / 2);

            this.StatusUpdate("Binary Tree - Depth = " + this.FractalDepth.ToString() + ". # of Branches = " + this.FractalCanvas.Children.Count);
            this.FractalDepth += 1;
            if (this.FractalDepth > this.MaxDepth || this.FractalDepth < 0)
            {
                this.StatusUpdate("Binary Tree - Depth = " + this.FractalDepth.ToString() + ". Finished. # of Branches = " + this.FractalCanvas.Children.Count);

                // stop the render process and reset the fractal to draw again
                CompositionTarget.Rendering -= this.StartRender;
                this.RenderTicks = 0;
                this.FractalDepth = 0;
            }
        }

        /// <summary>
        /// Draws a single branch of the tree and, if not at the recursion limit, calls DrawBranch for each of this branch's children
        /// </summary>
        /// <param name="canvas">Target canvas</param>
        /// <param name="depth">Current drawing depth (recursion limit)</param>
        /// <param name="pt">Point representing the starting location for this branch</param>
        /// <param name="length">The length of the line segment to draw for this branch</param>
        /// <param name="theta">The angle this line segment will be drawn at</param>
        private void DrawBranch(Canvas canvas, int depth, Point pt, double length, double theta)
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

            // check for reaching minimum line length
            if (length * this.ChildScale < this.MinSize)
            {
                // Set FractalDepth to a breakout value
                this.FractalDepth = -1;

                // abort before starting the child rendering process
                return;
            }

            // We still have depth remaining to plumb, so recurse into the next level of depth
            if (depth > 1)
            {
                // adjust the point representing this segment's endpoint based on ChildOffset
                endX = endX - ((this.ChildOffset * length) * Math.Cos(theta + this.ChildOffsetRotation));
                endY = endY - ((this.ChildOffset * length) * Math.Sin(theta + this.ChildOffsetRotation));

                // calc the angle between each child and the starting position of the leftmost
                double betweenTheta = this.DeltaTheta / (this.ChildCount - 1);
                double childTheta = theta - (this.DeltaTheta / 2);
                for (int c = 0; c < this.ChildCount; c++)
                {
                    this.DrawBranch(
                        canvas,
                        depth - 1,
                        new Point(endX, endY),
                        length * this.ChildScale,
                        childTheta);
                    childTheta = childTheta + betweenTheta;
                }
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}
