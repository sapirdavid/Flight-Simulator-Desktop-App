using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace MileStone1
{
    public class RegressionAnomalyDetector : AnomalyDetector
    {

        public RegressionAnomalyDetector(string normalCsvPath, string anomalyCsvPath, string anomalyDetectionAlgorithemPath) : base(normalCsvPath, anomalyCsvPath, anomalyDetectionAlgorithemPath)
        { }

    }
    
}
