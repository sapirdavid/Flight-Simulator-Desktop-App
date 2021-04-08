using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MileStone1
{
    public class CorrlativeCircle
    {
        public int feature1;
        public int feature2;
        public Point middle;
        public float radius;
        public CorrlativeCircle(int feature1, int feature2, Point middle, float radius)
        {
            this.feature1 = feature1;
            this.feature2 = feature2;
            this.middle = middle;
            this.radius = radius;
        }
    }

    public class AnomalyDetector
    {
        [DllImport(@"anomalyDetector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr detect(string normalCsvPath, string anomalyCsvPath);
        

        [DllImport(@"anomalyDetector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr delIntArray(IntPtr intArray);

        [DllImport(@"anomalyDetector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Circle(string normalCsvPath);


        [DllImport(@"anomalyDetector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr delFloatArray(IntPtr floatArray);


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
        public List<CorrlativeCircle> getCorrletiveCircles() //if there is correlative circule 
        {
            IntPtr arrayPtr = Circle(normalCsvPath);
            float[] size = new float[1];
            Marshal.Copy(arrayPtr, size, 0, 1); //copy the first float
            float[] anomaliesCircles = new float[(int)size[0] + 1];
            Marshal.Copy(arrayPtr, anomaliesCircles, 0, (int)size[0] + 1);

            int fullSize = (int)size[0] + 1;
            int loops = (int)size[0] / 5; //every loop copy 5 arguments

            List<CorrlativeCircle> circlesAndAnomalies = new List<CorrlativeCircle>();
            for (int i = 0, j = 1; i < loops; i++)
            {  //j is the index in the big array
                circlesAndAnomalies.Add(new CorrlativeCircle(
                    (int)anomaliesCircles[j++],
                    (int)anomaliesCircles[j++],
                    new Point(anomaliesCircles[j++], anomaliesCircles[j++]),
                    anomaliesCircles[j++]
                    ));
            }
            delFloatArray(arrayPtr); //free the array
            return circlesAndAnomalies;
        }
    }
}
