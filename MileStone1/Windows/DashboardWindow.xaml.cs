using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MileStone1.ViewModel;
using System.Windows.Forms;
using Transmitter;
namespace MileStone1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        string csvPath;
        FlightDetectorModel lt;
        

        public DashboardWindow(string csvPath)
        {
            this.csvPath = csvPath; //update the path to data file
            InitializeComponent();
            initializeComponents();
        }
        private void initializeComponents()
        { 
            FileReader f = new FileReader();
            f.ReadFile(csvPath);
            this.lt = new FlightDetectorModel(f.LinesOfData, "localhost", 5400);
            SliderControlVM ltvm = new SliderControlVM(lt);
            animationSlide.ViewModel = ltvm;

            // create VM for the navigator
            NavigatorStateVM nsvm = new NavigatorStateVM(lt);
            Navigator.Nsvm = nsvm;

            // create VM for the dataInfo
            DataInfoVM divm = new DataInfoVM(lt);
            DataInfo.Divm = divm;

            // create VM for the grpah
            ViewModel.GraphVM gvm = new ViewModel.GraphVM(lt);
            graph.Gvm = gvm;


        }
        public void StartAnimation() {
            this.lt.StartTransmitting();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            b1.Content = "Line Number:" + animationSlide.LineToTransmit;
        }

    }
}
