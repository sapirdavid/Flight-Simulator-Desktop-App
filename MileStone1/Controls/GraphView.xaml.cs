using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            //connects the model and the viewModel (propreties and view)
            DataContext = gvm;
        }

        private void lbx_property_names_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get the selected proprety index
            PropertyIndex lbi = ((sender as ListBox).SelectedItem as PropertyIndex);
            gvm.changeValues(lbi);
            gvm.pressed = true;
        }

        public GraphVM Gvm
        {
            get
            {
                return this.gvm;
            }
            set
            {
                this.gvm = value;
                DataContext = gvm;

                gvm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
                {
                    //if the propreties have changed
                    if (e.PropertyName == "PropertyNames")
                    {
                        gvm.PropertyIndexes = new List<PropertyIndex>();
                        int counter = 0;
                        //pass on each proprety name and create propertyIndex according to it's index and name
                        foreach (var item in gvm.PropertyNames)
                        {
                            PropertyIndex propertyIndex = new PropertyIndex();
                            propertyIndex.Name = item;
                            propertyIndex.Id = counter++;
                            gvm.PropertyIndexes.Add(propertyIndex);


                        }

                    }
                    if (e.PropertyName == "UpdateGraph")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            PropertyIndex prop = lbx_property_names.SelectedItem as PropertyIndex;
                            gvm.changeValues(prop);
                        }));
                    }







                };
            }

        }

    }
}
