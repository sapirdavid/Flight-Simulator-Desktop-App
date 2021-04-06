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

                //if the values of rudder or throttle changed, the slider will change accordingly
                nsvm.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "VM_Aileron")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Canvas.SetLeft(Knob, (nsvm.VM_Aileron * 60 + 125));
                        }));
                    }
                    else if (e.PropertyName == "VM_Elevator")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Canvas.SetTop(Knob, (nsvm.VM_Elevator * 60 + 125));
                        }));

                    };
                };

            }

        }
    }
}
