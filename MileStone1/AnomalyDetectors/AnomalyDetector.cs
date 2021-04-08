using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MileStone1
{
    public class AnomalyDetector
    {
        [DllImport(@"..\..\..\dlls\anomalyDetector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr detect(string normalCsvPath, string anomalyCsvPath);


        [DllImport(@"..\..\..\dlls\anomalyDetector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr delIntArray(IntPtr intArray);


        protected string normalCsvPath;
        protected string anomalyCsvPath;
        protected string anomalyDetectionAlgorithemPath;
        List<List<Tuple<int, int>>> anomalies;
        public AnomalyDetector(string normalCsvPath, string anomalyCsvPath, string anomalyDetectionAlgorithemPath)
        {
            this.normalCsvPath = normalCsvPath;
            this.anomalyCsvPath = anomalyCsvPath;
            this.anomalyDetectionAlgorithemPath = anomalyDetectionAlgorithemPath;
            //copy the requested dll
            
        }
        //anomalies[i] contain anomalies with the column i, the first in the tuple is the column that anomaly was detect and the second is the line

        public List<List<Tuple<int, int>>> detectAnomalies()
        {
            IntPtr arrayPtr = detect(normalCsvPath, anomalyCsvPath);


            int[] size = new int[1];
            Marshal.Copy(arrayPtr, size, 0, 1);
            int[] anomaliesArray = new int[size[0] + 1];
            Marshal.Copy(arrayPtr, anomaliesArray, 0, size[0] + 1); //copy all the anomalies
            List<List<Tuple<int, int>>> anomalies = new List<List<Tuple<int, int>>>();

            int halfSize = size[0] / 2; //to indicate the location of the lines of the anomalies
            //count the number of columns in csv file
            int columnsNum = 0;
            for (int i = 1; i <= halfSize; i++)
            {
                if (anomaliesArray[i] == -1)
                    columnsNum++;
            }

            for (int i = 0; i < columnsNum; i++)
            {
                anomalies.Add(new List<Tuple<int, int>>());
            }
            int column = 0;
            for (int i = 1; i <= halfSize; i++)
            {
                if (anomaliesArray[i] == -1)
                { //if end of column
                    column++;
                    continue;
                }
                anomalies[column].Add(new Tuple<int, int>(anomaliesArray[i], anomaliesArray[i + halfSize])); //add the column of anomaly and line
            }
            delIntArray(arrayPtr); //free the array
            return anomalies;
        }
    }
}
