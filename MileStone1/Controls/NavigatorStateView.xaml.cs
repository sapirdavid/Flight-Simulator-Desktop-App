using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MileStone1.Controls
{
    /// <summary>
    /// Interaction logic for NavigatorStateView.xaml
    /// </summary>
    public partial class NavigatorStateView : UserControl
    {
        NavigatorStateVM nsvm;
        public NavigatorStateView()
        {
            InitializeComponent();
        }

        private void RudderSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

       
        public NavigatorStateVM Nsvm
        {
            get
            {
                return this.nsvm;
            }
            set
            {
                this.nsvm = value;
                DataContext = nsvm;

                nsvm.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "Rudder")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            RudderSlider.Value = nsvm.VM_Rudder;
                        }));
                    } else if (e.PropertyName == "Throttle")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ThrottleSlider.Value = nsvm.VM_Throttle;
                        }));
                    }

                };
            }

        }
        

    }
}
