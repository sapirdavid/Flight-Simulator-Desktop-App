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
                            propretyValue.InvalidatePlot(true);
                            correlativeProprety.InvalidatePlot(true);
                            regLineGraph.InvalidatePlot(true);
                            gvm.changeValues(prop);
                            //if (this.gvm.drawCircle == true)
                            //{
                            //    Point p = this.gvm.Circels[prop.Id].center;
                            //    float r = this.gvm.Circels[prop.Id].radaius;
                            //    if(r!=0)
                            //      regLineGraph.Annotations.Add(new OxyPlot.Wpf.EllipseAnnotation { X = p.X, Y = p.Y, Width = r, Height = r, Fill = System.Windows.Media.Color.FromRgb(255, 246, 238),StrokeThickness=1.5, Stroke = System.Windows.Media.Color.FromRgb(0, 0, 0)  });
                            //}


                        }));
                    }

                    //if (e.PropertyName == "UpdateCircele")
                    //{
                    //    this.Dispatcher.Invoke((Action)(() =>
                    //    {
                    //        PropertyIndex prop = lbx_property_names.SelectedItem as PropertyIndex;
                    //        Point p = this.gvm.Circels[prop.Id].center;
                    //        float r = this.gvm.Circels[prop.Id].radaius;
                    //        regLineGraph.Annotations.Add(new OxyPlot.Wpf.EllipseAnnotation { X = p.X, Y = p.Y, Width = r, Height = r, Fill = System.Windows.Media.Color.FromRgb(255, 255, 255), Stroke = System.Windows.Media.Color.FromRgb(0, 0, 0) });

                    //    }));
                    //}

                };
            }

        }


        public void firstTime()
        {
            lbx_property_names.SelectedIndex = 0;
            PropertyIndex prop = lbx_property_names.SelectedItem as PropertyIndex;
            propretyValue.InvalidatePlot(true);
            correlativeProprety.InvalidatePlot(true);
            regLineGraph.InvalidatePlot(true);
            gvm.changeValues(prop);
        }
    }
}
