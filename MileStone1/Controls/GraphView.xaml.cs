using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MileStone1.ViewModel;

namespace MileStone1.Controls
{
    public partial class GraphView : UserControl
    {
        //member of the viewModel
        ViewModel.GraphVM gvm;
        public GraphView()
        {
            InitializeComponent();
            Model.GraphModel gm = new Model.GraphModel();
            gvm = new ViewModel.GraphVM(gm);
            //connects the model and the viewModel (propreties and view)
            DataContext = gvm;
        }

        private void lbx_property_names_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get the selected proprety index
            PropertyIndex lbi = ((sender as ListBox).SelectedItem as PropertyIndex);
            gvm.changeValues(lbi);
        }

    }
}
