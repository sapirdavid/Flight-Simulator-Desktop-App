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
        public CorrlativeCircle(int feature1, int feature2, Point middle, float radius) {
            this.feature1 = feature1;
            this.feature2 = feature2;
            this.middle = middle;
            this.radius = radius;
        }
    }
    public class CircleAnomalyDetector : AnomalyDetector
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetAnomaliesCircles(string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr DelFloatArray(IntPtr array);

        public CircleAnomalyDetector(string normalCsvPath, string anomalyCsvPath, string anomalyDetectionAlgorithemPath) : base(normalCsvPath, anomalyCsvPath, anomalyDetectionAlgorithemPath)
        {}
        public List<CorrlativeCircle> getCorrletiveCircles() {
            IntPtr pDll = LoadLibrary(anomalyDetectionAlgorithemPath);
            //oh dear, error handling here
            //if (pDll == IntPtr.Zero)

            IntPtr pAddressOfFunctionToCall = GetProcAddress(pDll, "Circle");
            //oh dear, error handling here
            //if(pAddressOfFunctionToCall == IntPtr.Zero)

            GetAnomaliesCircles getAnomaliesCircles = (GetAnomaliesCircles)Marshal.GetDelegateForFunctionPointer(
            pAddressOfFunctionToCall,
            typeof(GetAnomaliesCircles));

            IntPtr arrayPtr = getAnomaliesCircles(normalCsvPath);
            float[] size = new float[1];
            Marshal.Copy(arrayPtr, size, 0, 1); //copy the first float
            float[] anomaliesCircles = new float[(int)size[0] + 1];
            Marshal.Copy(arrayPtr, anomaliesCircles, 0, (int)size[0] + 1);

            int fullSize = (int)size[0] + 1;
            int loops = (int)size[0] / 5; //every loop copy 5 arguments

            List<CorrlativeCircle> circlesAndAnomalies = new List<CorrlativeCircle>();
            for (int i = 0, j = 1; i < loops; i++) {  //j is the index in the big array
                circlesAndAnomalies.Add(new CorrlativeCircle(
                    (int)anomaliesCircles[j++],
                    (int)anomaliesCircles[j++],
                    new Point(anomaliesCircles[j++], anomaliesCircles[j++]),
                    anomaliesCircles[j++]
                    ));
            }



            pAddressOfFunctionToCall = GetProcAddress(pDll, "delFloatArray");
            DelFloatArray del = (DelFloatArray)Marshal.GetDelegateForFunctionPointer(
            pAddressOfFunctionToCall,
            typeof(DelFloatArray));

            del(arrayPtr); //free the array
            bool result = FreeLibrary(pDll);

            return circlesAndAnomalies;
        }
        
        
    }

}
