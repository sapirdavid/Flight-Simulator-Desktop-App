using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace MileStone1
{
    class AnomalyDetector
    {
        [DllImport(@"dll1t.dll")]
        public static extern IntPtr detect(StringBuilder normalCsvPath, StringBuilder regularCsvPath);

        [DllImport(@"dll1t.dll")]
        public static extern void del(IntPtr arr);

        string normalCsvPath;
        string anomalyCsvPath;
        string anomalyDetectionAlgorithemPath;
        List<List<Tuple<int, int>>> anomalies;
       public AnomalyDetector(string normalCsvPath, string anomalyCsvPath, string anomalyDetectionAlgorithemPath) {
            this.normalCsvPath = normalCsvPath;
            this.anomalyCsvPath = anomalyCsvPath;
            this.anomalyDetectionAlgorithemPath = anomalyDetectionAlgorithemPath;
        }
        //anomalies[i] contain anomalies with the column i, the first in the tuple is the column that anomaly was detect and the second is the line

        public List<List<Tuple<int, int>>> detectAnomalies()
        {
            StringBuilder normalPath = new StringBuilder();
            normalPath.Append(normalCsvPath);

            StringBuilder anomalyPath = new StringBuilder();
            anomalyPath.Append(anomalyCsvPath);

            IntPtr arrayPtr = detect(normalPath, anomalyPath);
            //IntPtr anomaliesArray = detect(this.normalCsvPath, this.regularCsvPath);
            int[] size = new int[1];
            Marshal.Copy(arrayPtr, size, 0, 1);
            int[] anomaliesArray = new int[size[0] + 1];
            Marshal.Copy(arrayPtr, anomaliesArray, 0, size[0] + 1); //copy all the anomalies
            List<List<Tuple<int, int>>>  anomalies =  new List<List<Tuple<int, int>>>();
            
            int halfSize = size[0] / 2; //to indicate the location of the lines of the anomalies
            //count the number of columns in csv file
            int columnsNum = 0; 
            for (int i = 1; i <= halfSize; i++) {
                if (anomaliesArray[i] == -1)
                    columnsNum++;
            }

            for (int i = 0; i < columnsNum; i++)
            {
                anomalies.Add(new List<Tuple<int, int>>());
            }
            int column = 0;
            for (int i = 1; i <= halfSize; i++) {
                if (anomaliesArray[i] == -1) { //if end of column
                    column++;
                    continue;
                }
                anomalies[column].Add(new Tuple<int, int>(anomaliesArray[i], anomaliesArray[i + halfSize])); //add the column of anomaly and line
            }
            del(arrayPtr); //free the array
            return anomalies;


        }
    }
}
