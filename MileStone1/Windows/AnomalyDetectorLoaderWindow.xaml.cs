using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MileStone1
{
    public class Anomalies {
        public List<List<Tuple<int, int>>> anomaliesList = null;
        public List<CorrlativeCircle> anomaliesRangeCircles = null;
    }
    /// <summary>
    /// Interaction logic for AnomalyDetectorLoaderWindow.xaml
    /// </summary>
    public partial class AnomalyDetectorLoaderWindow : MetroWindow
    {
        string anomalyCsvPath;
        string dllAlgorithmPath;
        string normalCsvPath;
        bool isAnomalyDetectorInitiated = false;

        float corrlationThreshold = -1;
        public float CorrlationThreshold {
            get {
                return this.corrlationThreshold;
            }

            set {

                if (value >= 0 && value <= 1) //if corrlation represent corllatin value
                    this.corrlationThreshold = value;
            }
                }

        public bool IsAnomalyDetectorInitiated { get; set;}
        public AnomalyDetectorLoaderWindow(string anomalyCsvPath, string normalCsvPath)
        {
            this.anomalyCsvPath = anomalyCsvPath;
            this.normalCsvPath = normalCsvPath;
            InitializeComponent();
        }
        //dll load button
        private void Button_Click(object sender, RoutedEventArgs e)
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
            if (!csvPath.Contains(".dll"))
            {
                System.Windows.MessageBox.Show("You need to load file .DLL file!", "Alert", MessageBoxButton.OK);
            }
            else
            {
                this.dllAlgorithmPath = csvPath;
                if (csvPath != null)
                {
                    IsAnomalyDetectorInitiated = true;
                    finishButton.IsEnabled = true; //enable the button of finsh option
                }
            }
        }

        

        public List<List<Tuple<int, int>>> getAnomalies() {
            AnomalyDetector anomalyDetector = new AnomalyDetector(normalCsvPath, anomalyCsvPath, dllAlgorithmPath);
            anomalyDetector.CorrelationThreshold = CorrlationThreshold;
            return anomalyDetector.detectAnomalies();
        }
        public List<CorrlativeCircle> getCirclesOfAttr()
        {
           
            AnomalyDetector anomalyDetector = new AnomalyDetector(normalCsvPath, anomalyCsvPath, dllAlgorithmPath);
            anomalyDetector.CorrelationThreshold = CorrlationThreshold; 
            return anomalyDetector.getCorrletiveCircles();
          
        }

        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
                System.Environment.Exit(1);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void minimizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);

            }
        }

        private void dragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void finishButton_Click(object sender, RoutedEventArgs e)
        {
            if (corrlationThresholdTextBox.Text != "") {
                try { //try to convert to float
                   CorrlationThreshold =  float.Parse(corrlationThresholdTextBox.Text);
                }
                catch(Exception ex)
                {

                    //do nothing
                }

            }
            this.Hide();
        }
    }
}
