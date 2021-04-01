using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MileStone1.ViewModel
{
    public class DataInfoVM : INotifyPropertyChanged
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
        public DataInfoVM(FlightDetectorModel fdMoodel)
        {
            this.fdm = fdMoodel;
            this.fdm.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public double VM_Altimeter
        {
            get
            {
                return this.fdm.Altimeter;
            }
        }

        public double VM_Airspeed
        {
            get
            {
                return this.fdm.Airspeed;
            }
        }

        public double VM_Direction
        {
            get
            {
                return this.fdm.Direction;
            }
        }

        public double VM_Pitch
        {
            get
            {
                return this.fdm.Pitch;
            }
        }

        public double VM_Roll
        {
            get
            {
                return this.fdm.Roll;
            }
        }

        public double VM_Yaw
        {
            get
            {
                return this.fdm.Yaw;
            }
        }

    }
}
