using EvolutionSim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        private Map Map;
        private double xBound = 50;
        private double yBound = 50;
        private int numLakes = 4;
        private int InitialPrey = 20;
        private int InitialPreds = 20;
        private int NumIters = 500;
        private SimulationState SimState = SimulationState.Uninitialized;

        private Thread DisplayThread;
        private static ManualResetEvent mre = new ManualResetEvent(false);

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
            // Create a green background for the grass
            Path grass = new();
            grass.Fill = Brushes.Green;
            grass.Stroke = Brushes.Green;
            grass.Data = new RectangleGeometry(new Rect(new Point(0, 0), new Point(MapStackPanel.ActualWidth, MapStackPanel.ActualHeight)));
            MapCanvas.Children.Add(grass);

            //Generate the map object 
            Map = new(xBound, yBound, numLakes);

            for (int i = 0; i < Map.Lakes.Count; i++)
            {
                // Create a blue circle for all of the lakes on the map
                Circle lake = Map.Lakes[i];
                Path drawLake = new();
                drawLake.Fill = Brushes.Blue;
                drawLake.Stroke = Brushes.Blue;
                (double x, double y) center = MapPointToDrawPoint(lake.Center);
                (double r1, double r2) radius = MapPointToDrawPoint((lake.Radius, lake.Radius));
                drawLake.Data = new EllipseGeometry(new Point(center.x, center.y), radius.r1*0.95, radius.r2*0.95);
                MapCanvas.Children.Add(drawLake);
            }
        }

        /// <summary>
        /// Function to disable/enable buttons 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChangeSimState(object sender, RoutedEventArgs e)
        {
            switch (SimState)
            {
                case SimulationState.Uninitialized:
                    // TODO: init simulation
                    GenerateMapButton.IsEnabled = false;
                    SimulationButton.Background = Brushes.Red;
                    SimulationButton.Content = "Stop Simulation";
                    SimState = SimulationState.Running;
                    BeginSimulation();
                    break;
                case SimulationState.Running:
                    // TODO: pause simulation
                    GenerateMapButton.IsEnabled = false;
                    SimulationButton.Background = Brushes.LightGreen;
                    SimulationButton.Content = "Resume Simulation";
                    SimState = SimulationState.Stopped;
                    PauseSimulation();
                    break;
                case SimulationState.Stopped:
                    // TODO: restart simulation
                    GenerateMapButton.IsEnabled = false;
                    SimulationButton.Background = Brushes.Red;
                    SimulationButton.Content = "Stop Simulation";
                    SimState = SimulationState.Running;
                    RestartSimulation();
                    break;
            }
        }

        /// <summary>
        /// Function to begin the simulation (for the first time)
        /// </summary>
        void BeginSimulation()
        {
            MapControl = MapControlSingleton.GetInstance();
            MapControl.CreateSimulation(Map, InitialPreds, InitialPrey);
            DisplayThread = new Thread(DisplayLoop);
            DisplayThread.Start();
            //Trace.WriteLine("Here");
            //for(int i = 0; i < NumIters; i++)
            //{
            //    DisplayLoop();
            //    Thread.Sleep(1000);
            //}
        }

        /// <summary>
        /// Function to restart the simulation
        /// </summary>
        void RestartSimulation()
        {
            mre.Set();
        }

        /// <summary>
        /// Function to stop or pause the simulation
        /// </summary>
        void PauseSimulation()
        {
            mre.WaitOne();
        }

        /// <summary>
        /// Loop to be run by a thread that will run and display the simulation to the screen
        /// </summary>
        void DisplayLoop()
        {
            // Don't need to write to the screen every loop, as most movements are small(ish)
            int _loopsBetweenDisplay = 0;
            int count = 0;
            for(int i = 0; i < NumIters; i++)
            {
                if(count >= _loopsBetweenDisplay)
                {
                    count = 0;
                    // Remove all previous drawings of the animals
                    this.Dispatcher.Invoke(() =>
                    {
                        AnimalCanvas.Children.Clear();
                    });
                    for(int j = 0; j < MapControl.Predators.Count; j++)
                    {
                        DisplayPredator(MapControl.Predators[j].xLoc, MapControl.Predators[j].yLoc);
                    }
                    for (int j = 0; j < MapControl.Prey.Count; j++)
                    {
                        DisplayPrey(MapControl.Prey[j].xLoc, MapControl.Prey[j].yLoc);
                    }
                }

                MapControl.RunIteration();
                count++;
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Function to display a single predator to the screen
        /// </summary>
        /// <param name="x">x location of predator</param>
        /// <param name="y">y location of predator</param>
        void DisplayPredator(double x, double y)
        {
            this.Dispatcher.Invoke(() =>
            {
                Path drawPred = new()
                {
                    Fill = Brushes.Red,
                    Stroke = Brushes.Red
                };
                (double x, double y) position = MapPointToDrawPoint((x, y));
                drawPred.Data = new RectangleGeometry(new Rect(new Point(position.x, position.y), new Size(10, 10)));
                AnimalCanvas.Children.Add(drawPred);
            });
        }

        /// <summary>
        /// Function to display a single prey to the screen
        /// </summary>
        /// <param name="x">x location of prey</param>
        /// <param name="y">y location of prey</param>
        void DisplayPrey(double x, double y)
        {
            this.Dispatcher.Invoke(() =>
            {
                Path drawPrey = new()
                {
                    Fill = Brushes.Gold,
                    Stroke = Brushes.Gold
                };
                (double x, double y) position = MapPointToDrawPoint((x, y));
                drawPrey.Data = new EllipseGeometry(new Point(position.x, position.y), 5, 5);
                AnimalCanvas.Children.Add(drawPrey);
            });
        }

        /// <summary>
        /// Function to convert a point in the xBound/yBound scale to the drawing scale
        /// </summary>
        /// <param name="x">x point in xBound scale</param>
        /// <param name="y">y point in yBound scale</param>
        /// <returns>(x,y) as point in drawing scale</returns>
        (double x, double y) MapPointToDrawPoint((double x, double y) point)
        {
            return (point.x / xBound * MapStackPanel.ActualWidth, point.y / yBound * MapStackPanel.ActualHeight);
        }
    }
}
