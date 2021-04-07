﻿using MileStone1.ViewModel;
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
        DashboardWindow dw = null;
        AnomalyDetectorLoaderWindow anomalyDetWin = null;
        
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private void openFile_Click(object sender, RoutedEventArgs e)
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
            this.txtPath = csvPath;
            this.dw = new DashboardWindow(csvPath);
            
            startSimulationButton.IsEnabled = true; //enable the button of the simulation
            loadAnomalyDet.IsEnabled = true;
        }

        private void startSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            if (dw != null)
            {
                // System.Diagnostics.Process.Start("fgfs.exe");
                this.Hide();
                dw.Show();
                dw.StartAnimation();
                this.dw.Anomalies = this.dw.Anomalies; //activate the functionality
            }
           
        }

        private void loadAnomalyDet_Click(object sender, RoutedEventArgs e)
        {
            
           this.anomalyDetWin = new AnomalyDetectorLoaderWindow(txtPath);
            anomalyDetWin.ShowDialog();
            //while (anomalyDetWin.) { }
            //update the anomalies in deshboard window
            if (this.dw != null)
            {
                Anomalies anomalies = new Anomalies();
                anomalies.anomaliesList = this.anomalyDetWin.getAnomalies();
                anomalies.anomaliesRangeCircles = this.anomalyDetWin.getCirclesOfAttr();
                this.dw.Anomalies = anomalies; //update deshboard windows with the anomalies
                this.anomalyDetWin.Close();
            }

            
        }
    }
}
