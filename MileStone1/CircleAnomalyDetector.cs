using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MileStone1
{

    public class CorrlativeCircle
    {
       public int feature1;
       public int feature2;
       public Point middle;
       public float radius;
    }
    public class CircleAnomalyDetector : AnomalyDetector
    {
        [DllImport(@"CircleAnomalyDetector.dll")]
        public static extern IntPtr Circle(StringBuilder normalCsvPath);

        [DllImport(@"CircleAnomalyDetector.dll")]
        public static extern void delFloatArray(IntPtr arr);
        public CircleAnomalyDetector(string normalCsvPath, string anomalyCsvPath, string anomalyDetectionAlgorithemPath) : base(normalCsvPath, anomalyCsvPath, anomalyDetectionAlgorithemPath)
        {}
        List<CorrlativeCircle> getCorrletiveCycles() {
            StringBuilder csvPath = new StringBuilder();
            csvPath.Append(normalCsvPath);
            IntPtr arrayPtr = Circle(csvPath);
            //IntPtr anomaliesArray = detect(this.normalCsvPath, this.regularCsvPath);
            float[] size = new float[1];
           // Marshal.Copy(arrayPtr, size, 0, 1);
            //int[] anomaliesArray = new int[size[0] + 1];
           // Marshal.Copy(arrayPtr, anomaliesArray, 0, size[0] + 1); //copy all the anomalies

            return new List<CorrlativeCircle>();
        }
        
        
    }

}
