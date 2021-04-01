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
using System.Xml;
using System.Xml.Linq;
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
        double rudder, throttle, aileron, elevator;
        /*******/

        // Graph parametrs
        public event PropertyChangedEventHandler PropertyChanged;
        public List<String> PropertyNames { get; set; }
        //list of all proprety's values 
        public List<List<float>> PropertyValues { get; set; }

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

        public double Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                this.INotifyPropertyChanged("Aileron");
            }
        }

        public double Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {
                this.elevator = value;
                this.INotifyPropertyChanged("Elevator");
            }
        }

        //the function update the values of rudder,throttle,aileron,elevator at specific line
        public void updateJoystickData(string data)
        {
            string[] curLineSplit = data.Split(",");
            Aileron = Double.Parse(curLineSplit[0]);
            Elevator = Double.Parse(curLineSplit[1]);
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
                        updateJoystickData(data[currentLine]);
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

        public void readXml()
        {
            PropertyNames = new List<String>();

            var reader = XmlReader.Create(@"..\..\..\..\playback_small.xml");
            //read only from the input tag
            reader.ReadToFollowing("input");
            //get all features names 
            while (reader.ReadToFollowing("name"))
            {
                string key = reader.ReadElementContentAsString();
                PropertyNames.Add(key); ;
            }
            INotifyPropertyChanged("PropertyNames");

        }

        //parser xml - extract the values of features
        public void readCsv()
        {
            //list of lists - each list will contain the values according to the feature name
            PropertyValues = new List<List<float>>();
            for (int i = 0; i < PropertyNames.Count; i++)
            {
                PropertyValues.Add(new List<float>());
            }
            using (var reader = new StreamReader(@"..\..\..\..\reg_flight.csv"))
            {
                while (!reader.EndOfStream)
                {
                    //get line
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    int i = 0;
                    //insert to the list
                    foreach (var item in values)
                    {
                        PropertyValues[i++].Add(float.Parse(item));
                    }
                }
            }
            //notify of change of the values
            INotifyPropertyChanged("PropertyValues");
        }

    }
}
