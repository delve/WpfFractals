//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="None">
//     MIT License (MIT)
//     Copyright (c) 2014 Grady Brandt
// </copyright>
// <author>Grady Brandt</author>
//-----------------------------------------------------------------------
namespace WpfFractals
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// ... damn StyleCop is anal.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the click event on the Binary Tree button
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">RoutedEventArgs event arguments</param>
        private void BtnTree_Click(object sender, RoutedEventArgs e)
        {
            // set up the canvas background brush and instantiate the window with it
            GradientStopCollection gradStop = new GradientStopCollection();
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF2499FA"), 0.529));
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF683205"), 1));
            LinearGradientBrush brushBG = new LinearGradientBrush(gradStop, new Point(0.5, 0), new Point(0.5, 1));
            FractalWindow winFrac = new FractalWindow(brushBG);

            // setup the fractal we're going to draw and put it in a new FractalWindow object
            SymmetricTreeFractal fractal = new SymmetricTreeFractal(1, 10, 5);
            ////fractal.ChildOffset = 0.2;
            ////fractal.ChildOffsetRotation = 3 * Math.PI / 2;
            fractal.DrawSpeed = 1;
            fractal.DeltaTheta = 2 * (Math.PI / 5);
            fractal.ChildCount = 4;

            winFrac.HostedFractal = fractal;
            winFrac.Show();
        }

        /// <summary>
        /// Handles the click event on the Koch Snowflake button
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">RoutedEventArgs event arguments</param>
        private void BtnSnowflake_Click(object sender, RoutedEventArgs e)
        {
            // set up the canvas background brush and instantiate the window with it
            GradientStopCollection gradStop = new GradientStopCollection();
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF88ADD8"), 0.947));
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF235087"), 0.992));
            gradStop.Add(new GradientStop(Colors.White, 0));
            RadialGradientBrush brushBG = new RadialGradientBrush(gradStop);
            FractalWindow winFrac = new FractalWindow(brushBG);

            // setup the fractal we're going to draw and put it in the FractalWindow object
            LineBendingFractal fractal = new LineBendingFractal();
            winFrac.HostedFractal = fractal;
            winFrac.Show();
        }
    }
}
