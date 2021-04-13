﻿using MileStone1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Transmitter;
using MahApps.Metro.Controls;
using System.Windows.Input;
using System.IO;

namespace MileStone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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
            if (!csvPath.Contains(".csv"))
            {
                System.Windows.MessageBox.Show("You need to load file .CSV file!", "Alert", MessageBoxButton.OK);
            }
            else
            {
                this.anomalyCsvPath = csvPath;
                this.dw = new DashboardWindow(csvPath, csvPath); //default is to load only anomaly file

                startSimulationButton.IsEnabled = true; //enable the button of the simulation

                normalFlightCsv.IsEnabled = true;
            }
        }

        private void startSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            
                if (normalCsvPath == string.Empty) { //no normal flight csv was entered
                    this.normalCsvPath = this.anomalyCsvPath;
                }
                
                // System.Diagnostics.Process.Start("fgfs.exe");
                this.Close();
               
                int res = dw.StartAnimation();
                if(res != 0) { 
                    dw.Show();
                this.dw.Anomalies = this.dw.Anomalies; //activate the functionality
                 }
           
        }

        private void loadAnomalyDet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.anomalyDetWin = new AnomalyDetectorLoaderWindow(this.anomalyCsvPath, this.normalCsvPath);
                anomalyDetWin.ShowDialog();
            }
            catch (Exception exp) {
                return;
            }
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

        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
                System.Environment.Exit(0);
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
                if (!csvPath.Contains(".csv"))
                {
                    System.Windows.MessageBox.Show("You need to load file .CSV file!", "Alert", MessageBoxButton.OK);
                }
                else
                {
                    this.normalCsvPath = csvPath;
                    this.dw = new DashboardWindow(anomalyCsvPath, normalCsvPath);
                    loadAnomalyDet.IsEnabled = true; //enable option to enter dll
                }
            }
            
        }

        private void dragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void instructionButton(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.MessageBox.Show(this, "Instructions to begin simulation:\n" +
                "1)Click on the \"save xml for flight \" button,save the file on your PC and copy the file to the directory of FlightGeat \\data\\Protocols (also you can save directly the file the the FG app with administrator access) \n\n" +
                "2)Please open the FlightGear app and fly with the following settings: \n--generic=socket,in,10,127.0.0.1,5400,tcp,playback_small \n--fdm=null\n\n" +
                "3)Please upload CSV file.\nfor simulation click on start.\nfor anomaly detection upload normal CSV + DLL and click start.\nEnjoy!", "Instruction");

        }

        private void downloadXML(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog save = new Microsoft.Win32.SaveFileDialog();
            save.InitialDirectory = @"C:\Program Files";
            save.RestoreDirectory = true;
            save.FileName = "playback_small.xml";
            save.DefaultExt = "playback_small.xml";
            save.Filter = "xml fles (*.xml) | *.xml";
            if(save.ShowDialog()==true)
            {
                File.Copy(@"..\..\..\..\playback_small.xml", save.FileName, true);
            }
        }
    }
}
