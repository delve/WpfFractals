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

            LineFractalWindow winBinaryTree = new LineFractalWindow() { BrushCanvasBG = brushBG };
            // winBinaryTree.Show();

            LineExtensionFractal fractal = new LineExtensionFractal(1, 10, 5);

            FractalWindow winLineFrac = new FractalWindow(fractal);
            winLineFrac.fractalCanvas.Background = brushBG;
            winLineFrac.Show();

            // TODO: using the BrushCanvasBG property as the data bind source for the convas background doesn't seem to work. 
            //       Don't know why offhand. Assigning the brush property to the canvas background in the constructor didn't work 
            //       either. Probably that assignment occurs before the object initialization syntax puts an object in the property. 
            //       Assigning it directly like this works but feels ugly like puppet strings.
            winBinaryTree.fractalCanvas.Background = brushBG;
        }
    }
}
