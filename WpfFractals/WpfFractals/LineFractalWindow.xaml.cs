//-----------------------------------------------------------------------
// <copyright file="LineFractalWindow.xaml.cs" company="None">
//     
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
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for LineFractalWindow.xaml
    /// </summary>
    public partial class LineFractalWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the LineFractalWindow class.
        /// This window is used to draw fractals composed of lines
        /// </summary>
        public LineFractalWindow() : this(1, 0) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the LineFractalWindow class.
        /// This window is used to draw fractals composed of lines
        /// </summary>
        /// <param name="minPixels">Sets the minimum size in pixels per line segment to use as the escape value for the recursion</param>
        public LineFractalWindow(int minPixels) : this(minPixels, 0) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the LineFractalWindow class.
        /// This window is used to draw fractals composed of lines
        /// </summary>
        /// <param name="minPixels">Sets the minimum size in pixels per line segment to use as the escape value for the recursion</param>
        /// <param name="depth">Sets the maximum depth to use in the recursion. 0 usually means 'until a segment is less than 1 pixel in size'</param>
        public LineFractalWindow(int minPixels, int depth)
        {
            this.InitializeComponent();

            this.MinSize = minPixels;
            this.MaxDepth = depth;
            this.drawSpeed = 0;
            Shape shapeToRender = new Rectangle() { Fill = Brushes.Red, Height = 35, Width = 35, RadiusX = 5, RadiusY = 5 };
            Canvas.SetLeft(shapeToRender, 5);
            Canvas.SetTop(shapeToRender, 5);

            // Draw the shaaaape
            fractalCanvas.Children.Add(shapeToRender);
        }

        /// <summary>
        /// Gets or sets the Brush object used as the background property for 
        ///    the canvas. Used to make the window more visually appealing.
        ///    NOTE:this is currently not functioning as intende and therefore unused. 
        ///    See TODO in MainWindow.xaml.cs
        /// </summary>
        public Brush BrushCanvasBG { get; set; }

        /// <summary>
        /// Gets or sets the maximum iteration count for the drawing recursion
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets the minimum size of a line segment in pixels (ending point for the recursion, defaults 0)
        /// </summary>
        public int MinSize { get; set; }

        // ---------------------------------------------------------------------------------------

        /// <summary>
        /// Slows down the drawing process. A depth level is drawn every 'drawSpeed'th tick of CompositionTarget.Rendering
        /// </summary>
        private int drawSpeed;

        /// <summary>
        /// Current fractal drawing depth
        /// </summary>
        private int fractalDepth;

        /// <summary>
        /// Handles the click event on the start button, beginning the process of drawing the fractal
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">RoutedEventArgs event arguments</param>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            this.fractalCanvas.Children.Clear();
            this.lblProgress.Content = "Drawing...";
            this.drawSpeed = 0;
            this.fractalDepth = 1;
            CompositionTarget.Rendering += this.StartAnimation;
        }

        /// <summary>
        /// Renders the fractal to screen
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">EventArgs event arguments</param>
        private void StartAnimation(object sender, EventArgs e)
        {
            this.drawSpeed += 1;
            if (0 == this.drawSpeed % 10)
            {
                this.fractalCanvas.Children.Clear();
                this.DrawBinaryTreeBranch(
                    this.fractalCanvas, 
                    this.fractalDepth,
                    new Point(this.fractalCanvas.Width / 2, 0.83 * this.fractalCanvas.Height),
                    0.2 * this.fractalCanvas.Width, 
                    -Math.PI / 2);
                string str = "Binary Tree - Depth = " + this.fractalDepth.ToString() + ". # of Branches = " + this.fractalCanvas.Children.Count;
                this.lblProgress.Content = str;
                this.fractalDepth += 1;
                if (this.fractalDepth > 10)
                {
                    this.lblProgress.Content = "Binary Tree - Depth = 10. Finished. # of Branches = " + this.fractalCanvas.Children.Count;
                    CompositionTarget.Rendering -= this.StartAnimation;
                }
            }
        }

        /// <summary>
        /// Line segment scaling factor
        /// </summary>
        private double lengthScale = 0.75;

        /// <summary>
        /// Angle offset (+/-) for each child line segment
        /// </summary>
        private double deltaTheta = Math.PI / 5;

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
                    length * this.lengthScale,
                    theta + this.deltaTheta);

                // draw the -theta segment
                this.DrawBinaryTreeBranch(
                    canvas,
                    depth - 1,
                    new Point(endX, endY),
                    length * this.lengthScale,
                    theta - this.deltaTheta);

                //// draw the +1/2theta segment
                //this.DrawBinaryTreeBranch(
                //    canvas,
                //    depth - 1,
                //    new Point(endX, endY),
                //    length * this.lengthScale,
                //    theta + this.deltaTheta/2);

                //// draw the -1/2theta segment
                //this.DrawBinaryTreeBranch(
                //    canvas,
                //    depth - 1,
                //    new Point(endX, endY),
                //    length * this.lengthScale,
                //    theta - this.deltaTheta/2);

                //// draw a neutral theta segment
                //this.DrawBinaryTreeBranch(
                //    canvas,
                //    depth - 1,
                //    new Point(endX, endY),
                //    length * this.lengthScale,
                //    theta);
            }
            else
            {
                return;
            }
        }
    }
}