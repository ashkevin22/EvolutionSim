using EvolutionSim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Display
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MapControlSingleton MapControl;
        private double xBound = 50;
        private double yBound = 50;
        private int numLakes = 4;
        private int InitialPrey = 20;
        private int InitialPreds = 20;
        private int NumIters = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to generate and then display the map for the simulation
        /// </summary>
        /// <param name="xBound">Maximum x value for the map</param>
        /// <param name="yBound">Maximum y value for the map</param>
        /// <param name="numLakes">number of lakes to appear on the map</param>
        void CreateMap(object sender, RoutedEventArgs e)
        {
            Map map = new(xBound, yBound, numLakes);
            for(int i = 0; i < map.Lakes.Count; i++)
            {
                //// Create a StackPanel to contain the shape.
                //StackPanel myStackPanel = new StackPanel();

                //// Create a red Ellipse.
                //Ellipse myEllipse = new Ellipse();

                //// Create a SolidColorBrush with a red color to fill the
                //// Ellipse with.
                //SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                //// Describes the brush's color using RGB values.
                //// Each value has a range of 0-255.
                //mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
                //myEllipse.Fill = mySolidColorBrush;
                //myEllipse.StrokeThickness = 2;
                //myEllipse.Stroke = Brushes.Black;

                //// Set the width and height of the Ellipse.
                //myEllipse.Width = 200;
                //myEllipse.Height = 100;

                //// Add the Ellipse to the StackPanel.
                //myStackPanel.Children.Add(myEllipse);

                Trace.WriteLine("fuafasdf");
                TextBlock test = new TextBlock();
                test.Text = " FUIK YOU";
                Grid.SetColumn(test, 1);
                Grid.SetRow(test, 1);
            }
        }

        void BeginSim(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
