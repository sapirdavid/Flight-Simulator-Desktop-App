using MileStone1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Transmitter;
namespace MileStone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string txtPath;

        public MainWindow()
        {
            InitializeComponent();
            FileReader f = new FileReader();
            f.ReadFile(@"..\..\..\..\reg_flight.csv");
            FlightDetectorModel lt = new FlightDetectorModel(f.LinesOfData, "localhost", 5400);
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

            lt.StartTransmitting();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            b1.Content = "Line Number:" + animationSlide.LineToTransmit;
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
        {

            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name
            if (result.ToString() != string.Empty)
            {
                txtPath = openFileDlg.FileName;
            }

        }
    }
}
