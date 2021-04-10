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
using MahApps.Metro.Controls;
namespace MileStone1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DashboardWindow : MetroWindow
    {
        string anomalyCsvPath;
        string normalCsvPath;
        FlightDetectorModel lt = null;
        Anomalies anomalies = null;
        
        public Anomalies Anomalies {
            get
            {
                return this.anomalies;
            }
            set
            {
                if (value != null)
                {
                    this.anomalies = value;
                    if (lt != null)
                    { //if there is alredy model
                        lt.AnomaliesList = this.anomalies.anomaliesList;
                        lt.CorrlativeCircles = this.anomalies.anomaliesRangeCircles;
                    }
                }
            }
        }

        public DashboardWindow(string anomalyCsvPath, string normalCsvPath)
        {
            this.anomalyCsvPath = anomalyCsvPath; //update the path to data file
            this.normalCsvPath = normalCsvPath;
            InitializeComponent();
            initializeComponents();
        }
        private void initializeComponents()
        { 
            FileReader f = new FileReader();
            f.ReadFile(anomalyCsvPath);
            this.lt = new FlightDetectorModel(f.LinesOfData, "localhost", 5400, anomalyCsvPath, normalCsvPath);

            if (anomalies != null) { //if there is anomalies
                lt.AnomaliesList = this.anomalies.anomaliesList;
                lt.CorrlativeCircles = this.anomalies.anomaliesRangeCircles;
            }


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
        public int StartAnimation() {
            int susccs = this.lt.StartTransmitting();
            this.graph.firstTime();
            return susccs;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            b1.Content = "Line Number:" + animationSlide.LineToTransmit;
        }

        private void DataInfo_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void changeAnomalyDetectorButton_Click(object sender, RoutedEventArgs e)
        {
            if (anomalyCsvPath == normalCsvPath) {
                System.Windows.MessageBox.Show("You need to load normal flight file in the main window, (the normal csv and the anomaly csv need to be different)","Alert",MessageBoxButton.OK);
                return;
            
            }
            AnomalyDetectorLoaderWindow anomalyDetectorWin = new AnomalyDetectorLoaderWindow(this.anomalyCsvPath, this.normalCsvPath);
            anomalyDetectorWin.ShowDialog(); //exeption might be, because the user didnt enter any dll
            //update the model accordingly
            if (anomalyDetectorWin.IsAnomalyDetectorInitiated)
            {
                lt.AnomaliesList = anomalyDetectorWin.getAnomalies();
                lt.CorrlativeCircles = anomalyDetectorWin.getCirclesOfAttr();
            }
            anomalyDetectorWin.Close();
        }

        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.lt.StopTransmitting();
                Close();
                System.Environment.Exit(1);
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);

            }
        }

        private void dragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void returnToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.lt.StopTransmitting();
            this.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }
    }
}
