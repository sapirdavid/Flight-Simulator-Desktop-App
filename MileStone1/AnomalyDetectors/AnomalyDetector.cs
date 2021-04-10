using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MileStone1
{
    public class Circle
    {
        public float radaius;
        public Point center;
        public Circle(Point center, float r)
        {
            this.center = center;
            this.radaius = r;
        }
        public Circle(int X,int Y, float r)
        {
            this.center = new Point(X,Y);
            this.radaius = r;
        }

    }

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
        float correlationThreshold = -1; //default no corrlation threshold

        public float CorrelationThreshold
        {
            get
            {
                return this.correlationThreshold;
            }

            set
            {

                if (value >= 0 && value <= 1) //if corrlation represent corllatin value
                    this.correlationThreshold = value;
            }
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr Detect(string a, string b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DelIntArray(IntPtr array);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetAnomaliesCircles(string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DelFloatArray(IntPtr array);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetThreshold(float threshold);


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

            IntPtr pDll = LoadLibrary(anomalyDetectionAlgorithemPath);
            //oh dear, error handling here
            //if (pDll == IntPtr.Zero)

            //set corrlation thereshold
            if (correlationThreshold != -1)
            {
                IntPtr pAddressOfFunctionToCallT = GetProcAddress(pDll, "setThreshold");
                //oh dear, error handling here
                //if(pAddressOfFunctionToCall == IntPtr.Zero)
                SetThreshold setThreshold = (SetThreshold)Marshal.GetDelegateForFunctionPointer(
                pAddressOfFunctionToCallT,
                typeof(SetThreshold));
                setThreshold(CorrelationThreshold);
            }

            IntPtr pAddressOfFunctionToCall = GetProcAddress(pDll, "detect");
            //oh dear, error handling here
            //if(pAddressOfFunctionToCall == IntPtr.Zero)

            Detect detect = (Detect)Marshal.GetDelegateForFunctionPointer(
            pAddressOfFunctionToCall,
            typeof(Detect));

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

            pAddressOfFunctionToCall = GetProcAddress(pDll, "delIntArray");
            DelIntArray delIntArray = (DelIntArray)Marshal.GetDelegateForFunctionPointer(
            pAddressOfFunctionToCall,
            typeof(DelIntArray));

            delIntArray(arrayPtr); //free the array
            FreeLibrary(pDll);
            return anomalies;
        }
        public List<CorrlativeCircle> getCorrletiveCircles() //if there is correlative circule 
        {
            IntPtr pDll = LoadLibrary(anomalyDetectionAlgorithemPath);
            //oh dear, error handling here
            //if (pDll == IntPtr.Zero)


            //set corrlation thereshold
            if (correlationThreshold != -1)
            {
                IntPtr pAddressOfFunctionToCallT = GetProcAddress(pDll, "setThreshold");
                //oh dear, error handling here
                //if(pAddressOfFunctionToCall == IntPtr.Zero)
                SetThreshold setThreshold = (SetThreshold)Marshal.GetDelegateForFunctionPointer(
                pAddressOfFunctionToCallT,
                typeof(SetThreshold));
                setThreshold(CorrelationThreshold);
            }

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
            for (int i = 0, j = 1; i < loops; i++)
            {  //j is the index in the big array
                circlesAndAnomalies.Add(new CorrlativeCircle(
                    (int)anomaliesCircles[j++],
                    (int)anomaliesCircles[j++],
                    new Point(anomaliesCircles[j++], anomaliesCircles[j++]),
                    anomaliesCircles[j++]
                    ));
            }

            pAddressOfFunctionToCall = GetProcAddress(pDll, "delFloatArray");
            DelFloatArray delFloatArray = (DelFloatArray)Marshal.GetDelegateForFunctionPointer(
            pAddressOfFunctionToCall,
            typeof(DelFloatArray));
            delFloatArray(arrayPtr); //free the array

            FreeLibrary(pDll);
            return circlesAndAnomalies;
        }



    }
}
