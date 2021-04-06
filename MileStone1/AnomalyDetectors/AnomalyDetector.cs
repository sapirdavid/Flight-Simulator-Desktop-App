using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MileStone1
{
    public class AnomalyDetector
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr Detect(string a,string b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr DelIntArray(IntPtr array);
        

        protected string normalCsvPath;
        protected string anomalyCsvPath;
        protected string anomalyDetectionAlgorithemPath;
        List<List<Tuple<int, int>>> anomalies;
        public AnomalyDetector(string normalCsvPath, string anomalyCsvPath, string anomalyDetectionAlgorithemPath)
        {
            this.normalCsvPath = normalCsvPath;
            this.anomalyCsvPath = anomalyCsvPath;
            this.anomalyDetectionAlgorithemPath = anomalyDetectionAlgorithemPath;
        }
        //anomalies[i] contain anomalies with the column i, the first in the tuple is the column that anomaly was detect and the second is the line

        public List<List<Tuple<int, int>>> detectAnomalies()
        {

            IntPtr pDll = LoadLibrary(anomalyDetectionAlgorithemPath);
            //oh dear, error handling here
            //if (pDll == IntPtr.Zero)

            IntPtr pAddressOfFunctionToCall = GetProcAddress(pDll, "detect");
            //oh dear, error handling here
            //if(pAddressOfFunctionToCall == IntPtr.Zero)

            Detect detect = (Detect)Marshal.GetDelegateForFunctionPointer(
            pAddressOfFunctionToCall,
            typeof(Detect));

            IntPtr arrayPtr = detect(normalCsvPath, anomalyCsvPath);


            
           
            //IntPtr arrayPtr = detect(normalPath, anomalyPath);
            //IntPtr anomaliesArray = detect(this.normalCsvPath, this.regularCsvPath);
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
           DelIntArray del = (DelIntArray)Marshal.GetDelegateForFunctionPointer(
           pAddressOfFunctionToCall,
           typeof(DelIntArray));


            del(arrayPtr); //free the array
            bool result = FreeLibrary(pDll);
            return anomalies;


        }
    }
}
