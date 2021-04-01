using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStone1.ViewModel
{
    public class GraphVM : INotifyPropertyChanged
    {
        //member of the model
        private FlightDetectorModel fdm;
        //all names
        public List<PropertyIndex> PropertyIndexes { get; set; }
        //create title
        public String Title { get; set; }
        // create data point so we can do create the graph (the graph receives data points)
        public List<DataPoint> Points { get; private set; }
        public bool pressed = false;
    public long currenLineIndex
        {
            get
            {
                return this.fdm.LineToTransmit;
            }
            set
            {
            }
        }

        public long prevLineIndex{ get; set; }
        public List<String> PropertyNames
        {
            get
            {
                return fdm.PropertyNames;
            }
        }


        //c'tor
        public GraphVM(FlightDetectorModel fdm)
        {

            this.fdm = fdm;
            this.prevLineIndex = 0;
            this.fdm.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                //if the propreties have changed
                if (e.PropertyName == "PropertyNames")
                {
                    PropertyIndexes = new List<PropertyIndex>();
                    int counter = 0;
                    //pass on each proprety name and create propertyIndex according to it's index and name
                    foreach (var item in this.fdm.PropertyNames)
                    {
                        PropertyIndex propertyIndex = new PropertyIndex();
                        propertyIndex.Name = item;
                        propertyIndex.Id = counter++;
                        PropertyIndexes.Add(propertyIndex);
                    }

                }
                //if the values have changed
                if (e.PropertyName == "PropertyValues")
                {
                    List<float> valuesGraph = fdm.PropertyValues[0];
                    Points = new List<DataPoint>();
                    DateTime date = new DateTime(2020, 3, 26, 0, 0, 0);
                    foreach (var item in valuesGraph)
                    {
                        Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), item));
                        date = date.AddMinutes(1);
                    }
                }

                //if the values have changed
                if (e.PropertyName == "LineToTransmit")
                {
                    this.currenLineIndex = fdm.LineToTransmit;
                    if (pressed)
                       INotifyPropertyChanged("UpdateGraph");
                }
            };
            this.fdm.readXml();
            this.fdm.readCsv();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void INotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        // change proprety and show it's values at the graph
        public void changeValues(PropertyIndex property)
        {
            int lineDiff = 3;
            long lineToCopy = 0;
            if (Math.Abs(this.currenLineIndex - this.prevLineIndex) >= lineDiff)
            {
                //update the cuurent line
                this.prevLineIndex = currenLineIndex;
                lineToCopy = currenLineIndex;

            }
            else
            {
                lineToCopy = prevLineIndex;
            }
                List<float> AllData = fdm.PropertyValues[property.Id];
                List<float> valuesGraph = new List<float>();

                for(int i =0; i < lineToCopy; i++)
                {
                    valuesGraph.Add(AllData[i]);
                }

                Points = new List<DataPoint>();
                DateTime date = new DateTime(2020, 3, 26, 0, 0, 0);
               // DateTime date = DateTime.UtcNow;

                foreach (var item in valuesGraph)
                {
                    Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), item));
                    date = date.AddMilliseconds(100);
                }

                this.Title = property.Name;
           //     INotifyPropertyChanged("UpdateGraph");
                NotifyPropertyChanged("Title");
                NotifyPropertyChanged("Points");

            
        }
    }
    // create class so each proprety will have a name and an index (id)
    public class PropertyIndex
    {
        public string Name { get; set; }
        public int Id { get; set; }

    }
}
