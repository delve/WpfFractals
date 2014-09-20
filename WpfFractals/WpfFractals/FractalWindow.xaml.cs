//-----------------------------------------------------------------------
// <copyright file="FractalWindow.xaml.cs" company="None">
//     MIT License (MIT)
//     Copyright (c) 2014 Grady Brandt
// </copyright>
// <author>Grady Brandt</author>
//-----------------------------------------------------------------------
namespace WpfFractals
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for LineFractalWindow.xaml
    /// </summary>
    public partial class FractalWindow : Window
    {
        #region Fields
        /// <summary>
        /// A private field backing the DrawFractal property
        /// </summary>
        private Fractal neatFractal;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the FractalWindow class.
        /// This window is used to draw fractals composed of lines
        /// </summary>
        public FractalWindow()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Brush object used as the background property for 
        ///    the canvas. Used to make the window more visually appealing.
        ///    NOTE:this is currently not functioning as intended and therefore unused. 
        ///    See TODO in MainWindow.xaml.cs
        /// </summary>
        public Brush BrushCanvasBG { get; set; }

        /// <summary>
        /// Gets or sets the Fractal object that this window will draw.
        /// Gives the Fractal access to the interaction objects it needs
        /// </summary>
        public Fractal HostedFractal 
        {
            get 
            { 
                return this.neatFractal; 
            }

            set 
            { 
                // Set up the fractal object
                this.neatFractal = value;
                this.HostedFractal.FractalCanvas = this.fractalCanvas;
                this.HostedFractal.StatusUpdate += this.HandleStatusUpdate;
            }
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles status update events from the hosted fractal
        /// </summary>
        /// <param name="message">The message from the fractal</param>
        public void HandleStatusUpdate(string message)
        {
            this.statbarMessage.Text = message;
        }

        /// <summary>
        /// Handles the click event on the start button, beginning the process of drawing the fractal
        /// </summary>
        /// <param name="sender">The object generating the event</param>
        /// <param name="e">RoutedEventArgs event arguments</param>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            this.fractalCanvas.Children.Clear();
            this.statbarMessage.Text = "Drawing...";

            CompositionTarget.Rendering += this.HostedFractal.StartRender;
        }
        #endregion

        #region Methods
        #endregion
    }
}