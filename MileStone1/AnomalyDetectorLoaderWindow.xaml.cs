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

namespace MileStone1
{
    public class Anomalies {
        public List<List<Tuple<int, int>>> anomaliesList = null;
        public List<CorrlativeCircle> anomaliesRangeCircles = null;
    }
    /// <summary>
    /// Interaction logic for AnomalyDetectorLoaderWindow.xaml
    /// </summary>
    public partial class AnomalyDetectorLoaderWindow : Window
    {
        string anomalyCsvPath;
        string dllAlgorthemPath;
        string normalCsvPath;
        bool IsCircleAnomalyDetector{
            get {
                return (bool)circleDLl.IsChecked;
            }
        }
        public AnomalyDetectorLoaderWindow(string anomalyCsvPath)
        {
            this.anomalyCsvPath = anomalyCsvPath;
            InitializeComponent();
            loadDllButton.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string csvPath = ""; //need to change
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name
            if (result.ToString() != string.Empty)
            {
                csvPath = openFileDlg.FileName;
            }
            this.dllAlgorthemPath = csvPath;
            LoadNormalPathButton.IsEnabled = true; //enable the button of the simulation
        }

        private void LoadNormalPathButton_Click(object sender, RoutedEventArgs e)
        {
            string csvPath = ""; //need to change
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name
            if (result.ToString() != string.Empty)
            {
                csvPath = openFileDlg.FileName;
            }
            this.normalCsvPath = csvPath;
            circleDLl.IsEnabled = true;
            regressionDll.IsEnabled = true;
        }

        private void regressionDll_Checked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void circleDLl_Checked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        public List<List<Tuple<int, int>>> getAnomalies() {
            AnomalyDetector anomalyDetector = new AnomalyDetector(normalCsvPath, anomalyCsvPath, dllAlgorthemPath);
            return anomalyDetector.detectAnomalies();
        }
        public List<CorrlativeCircle> getCirclesOfAttr()
        {
            if (circleDLl.IsChecked == true)
            {
                CircleAnomalyDetector anomalyDetector = new CircleAnomalyDetector(normalCsvPath, anomalyCsvPath, dllAlgorthemPath);
                return anomalyDetector.getCorrletiveCircles();
            }
            return null;
        }

    }
}
