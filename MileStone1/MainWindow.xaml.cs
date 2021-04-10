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
        string anomalyCsvPath;
        string normalCsvPath = ""; //intiate normal csv with empty string
        DashboardWindow dw = null;
        AnomalyDetectorLoaderWindow anomalyDetWin = null;
        
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private void loadAnomalyClicked(object sender, RoutedEventArgs e)
        {
            string csvPath = ""; 
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name
            if (result.ToString() != string.Empty)
            {
                csvPath = openFileDlg.FileName;
            }
            this.anomalyCsvPath = csvPath;
            this.dw = new DashboardWindow(csvPath, csvPath); //default is to load only anomaly file
            this.anomalyDetWin = new AnomalyDetectorLoaderWindow(this.anomalyCsvPath ,this.normalCsvPath);
            startSimulationButton.IsEnabled = true; //enable the button of the simulation
            
            normalFlightCsv.IsEnabled = true;

        }

        private void startSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            
                if (normalCsvPath == string.Empty) { //no normal flight csv was entered
                    this.normalCsvPath = this.anomalyCsvPath;
                }
                
                // System.Diagnostics.Process.Start("fgfs.exe");
                this.Close();
                dw.Show();
                dw.StartAnimation();
                this.dw.Anomalies = this.dw.Anomalies; //activate the functionality
            
           
        }

        private void loadAnomalyDet_Click(object sender, RoutedEventArgs e)
        {
            
            anomalyDetWin.ShowDialog();
            //while (anomalyDetWin.) { }
            //update the anomalies in deshboard window
            if (this.dw != null)
            {
                Anomalies anomalies = new Anomalies();
                if (this.anomalyDetWin.IsAnomalyDetectorInitiated)
                {
                    anomalies.anomaliesList = this.anomalyDetWin.getAnomalies();
                    anomalies.anomaliesRangeCircles = this.anomalyDetWin.getCirclesOfAttr();
                    this.dw.Anomalies = anomalies; //update deshboard windows with the anomalies
                }
            }
            this.anomalyDetWin.Close();
        }

        private void normalFlightCsv_Click(object sender, RoutedEventArgs e)
        {
            string csvPath = "";
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name
            if (result != null)
            {
                csvPath = openFileDlg.FileName;
                this.normalCsvPath = csvPath;
                this.dw = new DashboardWindow(anomalyCsvPath, normalCsvPath);
                loadAnomalyDet.IsEnabled = true; //enable option to enter dll
            }
            
        }
    }
}
