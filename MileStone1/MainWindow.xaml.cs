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
        DashboardWindow dw = null;
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
            this.dw = new DashboardWindow(csvPath);
        }

        private void startSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            if (dw != null)
            {
                this.Hide();
                dw.Show();
            }
            else
            {
                csvRequestBox.Text = "please choose csv file first";
            }
        }
    }
}
