//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="None">
//     MIT License (MIT)
//     Copyright (c) 2014 Grady Brandt
// </copyright>
// <author>Grady Brandt</author>
//-----------------------------------------------------------------------
namespace WpfFractals
{
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
        private void BtnBinaryTree_Click(object sender, RoutedEventArgs e)
        {
            // set up the canvas background brush
            GradientStopCollection gradStop = new GradientStopCollection();
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF2499FA"), 0.529));
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF683205"), 1));
            LinearGradientBrush brushBG = new LinearGradientBrush(gradStop, new Point(0.5, 0), new Point(0.5, 1));

            // setup the fractal we're going to draw and put it in a new FractalWindow object
            LineExtensionFractal fractal = new LineExtensionFractal(1, 10, 5);
            FractalWindow winFrac = new FractalWindow();
            winFrac.HostedFractal = fractal;

            // TODO: using the BrushCanvasBG property as the data bind source for the convas background doesn't seem to work. 
            //       Don't know why offhand. Assigning the brush property to the canvas background in the constructor didn't work 
            //       either. Probably that assignment occurs before the object initialization syntax puts an object in the property. 
            //       Assigning it directly like this works but feels ugly like puppet strings.
            winFrac.fractalCanvas.Background = brushBG;
            winFrac.Show();
        }

        /// <summary>
        /// Handles the click event on the Koch Snowflake button
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">RoutedEventArgs event arguments</param>
        private void BtnSnowflake_Click(object sender, RoutedEventArgs e)
        {
            // set up the canvas background brush
            GradientStopCollection gradStop = new GradientStopCollection();
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF88ADD8"), 0.947));
            gradStop.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF235087"), 0.992));
            gradStop.Add(new GradientStop(Colors.White, 0));
            RadialGradientBrush brushBG = new RadialGradientBrush(gradStop);

            // setup the fractal we're going to draw and put it in a new FractalWindow object
            LineBendingFractal fractal = new LineBendingFractal();
            FractalWindow winFrac = new FractalWindow();
            winFrac.HostedFractal = fractal;

            // TODO: using the BrushCanvasBG property as the data bind source for the convas background doesn't seem to work. 
            //       Don't know why offhand. Assigning the brush property to the canvas background in the constructor didn't work 
            //       either. Probably that assignment occurs before the object initialization syntax puts an object in the property. 
            //       Assigning it directly like this works but feels ugly like puppet strings.
            winFrac.fractalCanvas.Background = brushBG;
            winFrac.Show();
        }
    }
}
