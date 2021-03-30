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
    class NavigatorStateVM : INotifyPropertyChanged
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

        
    }
}
