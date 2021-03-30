
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
    public class SliderControlVM : INotifyPropertyChanged
    {
        FlightDetectorModel lt;
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
            get { return lt.RunAnimation; }
            set { lt.RunAnimation = value; }
        }
        public int VM_ListSize
        {
            get
            {
                return lt.ListSize;
            }

        }
        public int VM_FramePerSecond
        {
            get
            {
                return this.lt.FramePerSecond;
            }
            set
            {
                this.lt.FramePerSecond = value;
            }

        }
        public int VM_LineToTransmit
        {
            get
            {
                return this.lt.LineToTransmit;
            }
            set
            {
                this.lt.LineToTransmit = value;
            }
        }
        public static readonly DependencyProperty VM_LineToTransmitProperty = DependencyProperty.Register("VM_LineToTransmit", typeof(int), typeof(SliderControlVM));
        public static readonly DependencyProperty VM_RunAnimationProperty = DependencyProperty.Register("VM_RunAnimation", typeof(bool), typeof(SliderControlVM));
        public SliderControlVM(FlightDetectorModel lt)
        {
            this.lt = lt;
            this.lt.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public void VM_StartTransimttion()
        {
            lt.StartTransmitting();
        }

        public void VM_StopTransimttion()
        {
            lt.StopTransmitting();
        }


    }


}


