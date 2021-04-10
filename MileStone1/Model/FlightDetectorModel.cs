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

        public string AnomalyCsvPath { get; private set; }
        public string NormalCsvPath { get; private set; }
        List<string> data;
        int fps = 10; //default frame per second
        int currentLine = 0;
        int listSize = 0;
        bool runAnimation = true;
        bool stopTransmitting = false;
        Thread t;
        /***belong to NavigatorState****/
        double rudder, throttle, aileron, elevator, altimeter, airspeed, direction, pitch, roll, yaw;
        /*******/
        //anomalies
        //anomalies[0][0].first is the first anomalie of 0 column with the column anomalies[0][0].first in line anomalies[0][0].second
        List<List<Tuple<int, int>>> anomaliesList = new List<List<Tuple<int, int>>>();
        List<CorrlativeCircle> corrlativeCircles = null;

        public List<List<Tuple<int, int>>> AnomaliesList {
            get
            {
                return this.anomaliesList;
            }
            set
            {
                if (value != null)
                {
                    this.anomaliesList = value;
                    this.INotifyPropertyChanged("AnomaliesList");
                }
            }
        }
        public List<CorrlativeCircle> CorrlativeCircles
        {
            get
            {
                return this.corrlativeCircles;
            }
            set
            {
                if (value != null)
                {
                    this.corrlativeCircles = value;
                    this.INotifyPropertyChanged("CorrlativeCircles");
                }
            }
        }


        // Graph parametrs
        public event PropertyChangedEventHandler PropertyChanged;
        public List<String> PropertyNames { get; set; }
        //list of all proprety's values 
        public List<List<float>> AnomalyPropertyValues { get; set; }
        public List<List<float>> NormalPropertyValues { get; set; }

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
            //    this.INotifyPropertyChanged("fps");
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

        public double Altimeter
        {
            get
            {
                return this.altimeter;
            }
            set
            {
                this.altimeter = value;
                this.INotifyPropertyChanged("Altimeter");
            }
        }

        public double Airspeed
        {
            get
            {
                return this.airspeed;
            }
            set
            {
                this.airspeed = value;
                this.INotifyPropertyChanged("Airspeed");
            }
        }

        public double Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
                this.INotifyPropertyChanged("Direction");
            }
        }

        public double Pitch
        {
            get
            {
                return this.pitch;
            }
            set
            {
                this.pitch = value;
                this.INotifyPropertyChanged("Pitch");
            }
        }

        public double Roll
        {
            get
            {
                return this.roll;
            }
            set
            {
                this.roll = value;
                this.INotifyPropertyChanged("Roll");
            }
        }

        public double Yaw
        {
            get
            {
                return this.yaw;
            }
            set
            {
                this.yaw = value;
                this.INotifyPropertyChanged("Yaw");
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
            Altimeter = Double.Parse(curLineSplit[22]);
            Airspeed = Double.Parse(curLineSplit[21]);
            Pitch = Double.Parse(curLineSplit[18]);
            Roll = Double.Parse(curLineSplit[17]);
            /*** need to find the values***/
            Direction = Double.Parse(curLineSplit[0]);
            Yaw = Double.Parse(curLineSplit[0]);
        }



        public FlightDetectorModel(List<string> data, string hostName, int port,string anomalyCsvPath, string normalCsvPath)
        {
            this.AnomalyCsvPath = anomalyCsvPath;
            this.NormalCsvPath = normalCsvPath;
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
                        int timeToWait = 1000 / FramePerSecond;
                        var firstTime = DateTime.Now;
                        tr.SendData(data[currentLine]);
                        updateJoystickData(data[currentLine]);
                        LineToTransmit++;
                        this.INotifyPropertyChanged("LineToTransmitChanged");
                        var secondTime = DateTime.Now; ;
                        //update the time to wait Considering the past time
                        timeToWait = Math.Max(0, timeToWait - (int)(secondTime.Millisecond - firstTime.Millisecond));
                        Thread.Sleep(timeToWait);
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
        public List<List<float>> readCsv(string path)
        {
            //list of lists - each list will contain the values according to the feature name
            List<List<float>> PropertyValues = new List<List<float>>();
            for (int i = 0; i < PropertyNames.Count; i++)
            {
                PropertyValues.Add(new List<float>());
            }
            using (var reader = new StreamReader(path))
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
            return PropertyValues;
            //notify of change of the values
           // INotifyPropertyChanged("AbomalyPropertyValues");
        }
        public void readAnomalyCsv() {
            AnomalyPropertyValues = readCsv(AnomalyCsvPath);
            //notify of change of the values
            INotifyPropertyChanged("AnomalyPropertyValues");
        }
        public void readNormalCsv() {
            NormalPropertyValues = readCsv(NormalCsvPath);
            //notify of change of the values
            INotifyPropertyChanged("NormalPropertyValues");
        }

    }
}
