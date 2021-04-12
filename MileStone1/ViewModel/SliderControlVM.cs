
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Transmitter;

namespace MileStone1
{
    public class ProgressBarVM : INotifyPropertyChanged
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
        public bool VM_RunAnimation
        {
            get { return fdm.RunAnimation; }
            set { fdm.RunAnimation = value; }
        }
        public int VM_ListSize
        {
            get
            {
                return fdm.ListSize;
            }

        }
        public int VM_FramePerSecond
        {
            get
            {
                return this.fdm.FramePerSecond;
            }
            set
            {
                this.fdm.FramePerSecond = value;
            }

        }
        public int VM_LineToTransmit
        {
            get
            {
                return this.fdm.LineToTransmit;
            }
            set
            {
                this.fdm.LineToTransmit = value;
            }
        }
        
        public ProgressBarVM(FlightDetectorModel fdm)
        {
            this.fdm = fdm;
            this.fdm.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public void VM_StartTransimttion()
        {
            fdm.StartTransmitting();
        }

        public void VM_StopTransimttion()
        {
            fdm.StopTransmitting();
        }

    }
}


