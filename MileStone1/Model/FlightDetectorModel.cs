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
    //model!
    public class FlightDetectorModel : INotifyPropertyChanged
    {
        Transmit tr;
        List<string> data;
        int fps = 0; //default frame per second
        int currentLine = 0;
        int listSize = 0;
        bool runAnimation = true;
        bool stopTransmitting = false;
        Thread t;
        /***belong to NavigatorState****/
        double rudder, throttle;
        /*******/
        public event PropertyChangedEventHandler PropertyChanged;
        // test
        public void INotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public bool RunAnimation
        {
            get { return this.runAnimation; }
            set
            {
                this.runAnimation = value;
                if (this.runAnimation)
                {
                    t.Interrupt();
                }
            }

        }
        public int ListSize
        {
            get
            {
                return this.listSize;
            }

        }
        public int LineToTransmit
        {
            get
            {
                return this.currentLine;
            }
            set
            {
                this.currentLine = value;
                this.INotifyPropertyChanged("LineToTransmit");
            }
        }
        public int FramePerSecond
        {
            get
            {
                return fps;
            }
            set
            {
                this.fps = value;
            }
        }

        public double Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                this.INotifyPropertyChanged("Rudder");
            }
        }

        public double Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                this.INotifyPropertyChanged("Throttle");
            }
        }

        //the function update the values of rudder and throttle at specific line
        public void updateRudderAndThrottle(string data)
        {
            string[] curLineSplit = data.Split(",");
            Rudder = Double.Parse(curLineSplit[2]);
            Throttle = Double.Parse(curLineSplit[6]);
        }

      

        public FlightDetectorModel(List<string> data, string hostName, int port)
        {
            this.data = data;
           // this.fps = fps;
            this.tr = new Transmit(hostName, port);
            this.listSize = data.Count;
            //the thread that teansmit the data

        }
        //not using
        public void TransmitLines()
        {
            int listSize = data.Count;
            while (LineToTransmit < listSize)
            {
                if (runAnimation)
                {
                    tr.SendData(data[currentLine]);
                    LineToTransmit++;
                    this.INotifyPropertyChanged("LineToTransmitChanged");
                    Thread.Sleep(1000 / this.fps);
                }
            }
        }

       

        //getRudderAndThrottle(data[currentLine]);

        public void StartTransmitting()
        {
            this.stopTransmitting = false;
            this.tr.Connect(); //connect with tcp (with the transmitter)
            this.t = new Thread(delegate () {
                int listSize = data.Count;
                while (!stopTransmitting)
                {
                    if (LineToTransmit < listSize)
                    {
                        if (!RunAnimation)
                        {
                            try
                            {
                                Thread.Sleep(Timeout.Infinite);
                            }
                            catch (ThreadInterruptedException)
                            {

                            }
                        }
                        tr.SendData(data[currentLine]);
                        updateRudderAndThrottle(data[currentLine]);
                        LineToTransmit++;
                        this.INotifyPropertyChanged("LineToTransmitChanged");
                        Thread.Sleep(1000 / FramePerSecond);
                    }
                }

            });
            t.Start();
        }
        public void StopTransmitting()
        {
            this.stopTransmitting = true;
            this.tr.CloseConnection();
        }
    
    }
}
