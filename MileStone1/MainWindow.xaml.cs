using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Transmitter;
namespace MileStone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           // Transmit t = new Transmit("localhost", 5400);
            FileReader f = new FileReader();
            f.ReadFile(@"..\..\..\..\reg_flight.csv");
            FlightDetectorModel lt = new FlightDetectorModel(f.LinesOfData, "localhost", 5400);
            SliderControlVM ltvm = new SliderControlVM(lt);
            animationSlide.ViewModel = ltvm;

            // create VM for the grpah
            NavigatorStateVM nsvm = new NavigatorStateVM(lt);
            Navigator.Nsvm = nsvm;

            // create VM for the grpah
            ViewModel.GraphVM gvm = new ViewModel.GraphVM(lt);
            graph.Gvm = gvm;

            lt.StartTransmitting();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            b1.Content = "Line Number:" + animationSlide.LineToTransmit;
        }

    }
}
