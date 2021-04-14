using MileStone1.ViewModel;
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
    /// Interaction logic for DataInfoView.xaml
    /// </summary>
    public partial class DataInfoView : UserControl
    {
        DataInfoVM divm;
        public DataInfoView()
        {
            InitializeComponent();
        }



        public DataInfoVM Divm
        {
            get
            {
                return this.divm;
            }
            set
            {
                this.divm = value;
                DataContext = divm;

                //if the values of rudder or throttle changed, the slider will change accordingly

                divm.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "VM_Altimeter")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Altimeter.Text = divm.VM_Altimeter.ToString("0.###");
                        }));
                    }
                    else if (e.PropertyName == "VM_Airspeed")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Airspeed.Text = divm.VM_Airspeed.ToString("0.###");
                        }));
                    }
                    else if (e.PropertyName == "VM_Direction")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Direction.Text = divm.VM_Direction.ToString("0.###");
                        }));
                    }
                };
            }
        }
    }
}


