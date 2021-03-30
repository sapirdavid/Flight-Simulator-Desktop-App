using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MileStone1.Model
{
    class GraphModel : INotifyPropertyChanged
    {
        //Occurs when a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;
        //list of all proprety's names 
        public List<String> PropertyNames { get; set; }
        //list of all proprety's values 
        public List<List<float>> PropertyValues { get; set; }

        public GraphModel()
        {

        }


        //method to notify of change
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        //parser xml - extract the names of features
        public void readXml()
        {
            PropertyNames = new List<String>();
            // XmlReader reader = XmlReader.Create(@"C:\Users\shova\source\repos\progressed_programing\MileStone1_6\MileStone1_6\playback_small.xml");

            // var reader = XDocument.Parse(@"C:\Users\shova\source\repos\progressed_programing\MileStone1_6\MileStone1_6\playback_small.xml");
            var reader = XmlReader.Create(@"C:\Users\shova\source\repos\progressed_programing\MileStone1_6\MileStone1_6\playback_small.xml");
            //read only from the input tag
            reader.ReadToFollowing("input");
            //get all features names 
            while (reader.ReadToFollowing("name"))
            {
                string key = reader.ReadElementContentAsString();
                PropertyNames.Add(key); ;
            }
            NotifyPropertyChanged("PropertyNames");

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
            using (var reader = new StreamReader(@"C:\Users\shova\source\repos\progressed_programing\MileStone1_6\MileStone1_6\reg_flight.csv"))
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
            NotifyPropertyChanged("PropertyValues");
        }
    }
}

