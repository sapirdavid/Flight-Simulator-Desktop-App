using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Transmitter;


namespace MileStone1
{
    public class NavigatorStateVM : INotifyPropertyChanged
    {
        FlightDetectorModel fdm;
        public event PropertyChangedEventHandler PropertyChanged;
        //notify all observers
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public NavigatorStateVM(FlightDetectorModel fdMoodel)
        {
            this.fdm = fdMoodel;
            this.fdm.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public double VM_Rudder
        {
            get
            {
                return this.fdm.Rudder;
            }
        }

        public double VM_Throttle
        {
            get
            {
                return this.fdm.Throttle;
            }
        }

        public double VM_Aileron
        {
            get
            {
                return this.fdm.Aileron;
            }
        }

        public double VM_Elevator
        {
            get
            {
                return this.fdm.Elevator;
            }
        }
    }
}
