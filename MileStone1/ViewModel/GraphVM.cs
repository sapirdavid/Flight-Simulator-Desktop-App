using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MileStone1.ViewModel
{
    public class GraphVM : INotifyPropertyChanged
    {
        //member of the model
        private FlightDetectorModel fdm;
        public MostCorrelativeFinder mcf;
        public List<PropertyIndex> PropertyIndexes { get; set; }
        //create title
        public String Title { get; set; }
        public String CorrelatedTitle { get; set; }

        // create data point so we can do create the graph (the graph receives data points)
        public List<DataPoint> Points { get; private set; }
        public List<DataPoint> CorrelatedPoints { get; private set; }
        public List<DataPoint> RegPoints { get; private set; }
        public List<DataPoint> RegLinePoints { get; private set; }
        public List<float> minCorrelatedPointsXValue { get; private set; }
        public List<float> maxCorrelatedPointsXValue { get; private set; }
        public List<List<DataPoint>> AnomaliesPoints { get; private set; }
        public List<DataPoint> AnomaliesPointsSpecificFeature { get; private set; }



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

        public long prevLineIndex { get; set; }
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
                    //create data points aacording to the time and value
                    foreach (var item in valuesGraph)
                    {
                        Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), item));
                        date = date.AddMinutes(1);
                    }

                    List<float> correlatedPropretyGraph = fdm.PropertyValues[0];
                    CorrelatedPoints = new List<DataPoint>();
                    foreach (var item in correlatedPropretyGraph)
                    {
                        CorrelatedPoints.Add(new DataPoint(DateTimeAxis.ToDouble(date), item));
                        date = date.AddMinutes(1);
                    }

                    List<float> regLinePropretyGraph = fdm.PropertyValues[0];
                    RegPoints = new List<DataPoint>();
                    for (int i = 0; i < correlatedPropretyGraph.Count; i++)
                    {
                        RegPoints.Add(new DataPoint(valuesGraph[i], correlatedPropretyGraph[i]));
                        date = date.AddMinutes(1);
                    }

                     RegLinePoints = new List<DataPoint>();
                     AnomaliesPoints = new List<List<DataPoint>>();
                     AnomaliesPointsSpecificFeature = new List<DataPoint>();
                }

                //if the values have changed
                if (e.PropertyName == "LineToTransmit")
                {
                    //update the values proprety that is chosen at the list box
                    this.currenLineIndex = fdm.LineToTransmit;
                    if (pressed)
                        INotifyPropertyChanged("UpdateGraph");
                }
            };
            this.fdm.readXml();
            this.fdm.readCsv();
            this.mcf = new MostCorrelativeFinder(fdm.PropertyValues);
            this.minCorrelatedPointsXValue = new List<float>();
            this.maxCorrelatedPointsXValue = new List<float>();
            //List<int> cor = mcf.CorrlatedColumns;
            //printCor(cor);
            //updateMinMaxValues();
            updateAnomaliesPoints();
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
      
        //the function update the anomalies points for every property
        public void updateAnomaliesPoints()
        {
            List<List<Tuple<int, int>>> anomaliesLists = this.fdm.AnomaliesList;
            int i = 0, firstFeatureColumn, SecondFeatureColumn, anomalyRow;
            float x, y;
            //pass on each proprety
            foreach (var property in this.PropertyIndexes)
            {
                List<Tuple<int, int>> anomaliesOfPropertyI = anomaliesLists[i];
                firstFeatureColumn = i;
                foreach (var anomaly in anomaliesOfPropertyI)
                {
                    SecondFeatureColumn = anomaly.Item1;
                    if(mcf.CorrlatedColumns[firstFeatureColumn] != SecondFeatureColumn)
                    {
                        Debug.WriteLine("error of correlated features: " + firstFeatureColumn + SecondFeatureColumn);
                    }
                    anomalyRow = anomaly.Item2;
                    x = fdm.PropertyValues[firstFeatureColumn][anomalyRow];
                    y = fdm.PropertyValues[SecondFeatureColumn][anomalyRow];
                    this.AnomaliesPoints[i].Add(new DataPoint(x, y));
                }
            }
        }


        //the function update for every property the min and max x value of his most correlated property
        public void updateMinMaxValues()
        {
            int i = 0;
            //pass on each proprety
            foreach (var property in this.PropertyIndexes)
            {
                int idxOfMostCorrelative = mcf.CorrlatedColumns[i];
                //all values of the current correlative feature
                List<float> correlatedPropretyData = fdm.PropertyValues[idxOfMostCorrelative];
                this.minCorrelatedPointsXValue.Add(correlatedPropretyData.Min());
                this.maxCorrelatedPointsXValue.Add(correlatedPropretyData.Max());
                i++;
            }
        }

        public void printCor(List<int> cor)
        {
            for(int i = 0; i < 42; i++)
            {
                Debug.WriteLine(this.PropertyNames[i] + "  to  " + this.PropertyNames[mcf.CorrlatedColumns[i]]);
                Debug.WriteLine("the pearson is: " + this.mcf.pearsonValue[i]);
            }
        }
        
        // change proprety and show it's values at the graph
        public void changeValues(PropertyIndex property)
        {
            //update every 3 rows 
            int lineDiff = 3;
            long lineToCopy = 0;
            int idxOfMostCorrelative = mcf.findTheMostCorrelative(fdm.PropertyValues, property.Id);
            Line regLine = mcf.linearRegressionList[idxOfMostCorrelative];
            //List<int> cor = mcf.CorrlatedColumns;


            //check if we need update
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
            //all values of the  current feature column 
            List<float> AllData = fdm.PropertyValues[property.Id];
            //the relevant points need to be shown at the graph
            List<float> valuesGraph = new List<float>();
            //all values of the current correlative feature
            List<float> correlatedPropretyData = fdm.PropertyValues[idxOfMostCorrelative];
            //the relevant points need to be shown at the graph
            List<float> correlatedPropretyGraph = new List<float>();


            for (int i = 0; i < lineToCopy; i++)
            {
                valuesGraph.Add(AllData[i]);
                correlatedPropretyGraph.Add(correlatedPropretyData[i]);

            }

            Points = new List<DataPoint>();
            CorrelatedPoints = new List<DataPoint>();
            RegPoints = new List<DataPoint>();
            RegLinePoints = new List<DataPoint>();
            AnomaliesPointsSpecificFeature = new List<DataPoint>();

            DateTime date = new DateTime(2020, 3, 26, 0, 0, 0);

            for (int i = 0; i < lineToCopy; i++)
            {
                Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), AllData[i]));
                CorrelatedPoints.Add(new DataPoint(DateTimeAxis.ToDouble(date), correlatedPropretyData[i]));
                RegPoints.Add(new DataPoint(correlatedPropretyGraph[i], AllData[i]));
                date = date.AddMilliseconds(100);
            }

            if(correlatedPropretyGraph.Count > 0)
            {
                RegLinePoints.Add(new DataPoint(minCorrelatedPointsXValue[property.Id], mcf.calcY(regLine, minCorrelatedPointsXValue[property.Id])));
                RegLinePoints.Add(new DataPoint(maxCorrelatedPointsXValue[property.Id], mcf.calcY(regLine, maxCorrelatedPointsXValue[property.Id])));
            }

            int amountAnomalies = this.AnomaliesPoints[property.Id].Count;
            for(int i = 0; i < amountAnomalies; i++)
            {
                this.AnomaliesPointsSpecificFeature.Add(this.AnomaliesPoints[property.Id][i]);
            }

            this.Title = property.Name;
            this.CorrelatedTitle = PropertyIndexes[idxOfMostCorrelative].Name;
            NotifyPropertyChanged("Title");
            NotifyPropertyChanged("CorrelatedTitle");
            NotifyPropertyChanged("Points");
            NotifyPropertyChanged("CorrelatedPoints");
            NotifyPropertyChanged("RegPoints");
            NotifyPropertyChanged("RegLinePoints");
            NotifyPropertyChanged("AnomaliesPointsSpecificFeature");
        }
    }
    // create class so each proprety will have a name and an index (id)
    public class PropertyIndex
    {
        public string Name { get; set; }
        public int Id { get; set; }

    }
}
