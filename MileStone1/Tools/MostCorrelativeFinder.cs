using System;
using System.Collections.Generic;
using System.Text;
namespace MileStone1
{
    class MostCorrelativeFinder
    {
        List<int> corrlativColumns;
        public List<int> CorrlatedColumns
        {
            get {
                return this.corrlativColumns;
            }
        }

        MostCorrelativeFinder(List<List<float>> dataCulomns) {
            this.corrlativColumns = new List<int>(dataCulomns.Count);
            for (int i = 0; i < dataCulomns.Count; i++) {
               this.corrlativColumns[i] = findTheMostCorrelative(dataCulomns, i);
                    }

        }

        private int findTheMostCorrelative(List<List<float>> dataCulomns, int column)
        {

            int mostCorrelativeIdx = -1; //no Correlative yet
            float mostCorrelativeVal = 0; //deafault
            int size = dataCulomns.Count;
            for (int i = 0; i < size; i++) {
                if (i == column) //if the same column of data
                    continue;
                if (mostCorrelativeIdx == -1) {
                    mostCorrelativeIdx = i; //if there isn't yer corrlation
                    mostCorrelativeVal = Math.Abs(pearson(dataCulomns[column], dataCulomns[i]));
                }
                else {
                    float currentCorrelativeVal = Math.Abs(pearson(dataCulomns[column], dataCulomns[i]));
                    if (currentCorrelativeVal > mostCorrelativeVal) { //if more corrlative
                        mostCorrelativeIdx = i;
                        mostCorrelativeVal = currentCorrelativeVal;
                    }
                }
            }
            return mostCorrelativeIdx;
        }

        private float pearson(List<float> x, List<float> y)
        {
            return (float)(cov(x, y, x.Count) / (Math.Sqrt(var(x, x.Count)) * Math.Sqrt(var(y, y.Count))));
        }
        // returns the variance of X and Y
        private float var(List<float> x, int size)
        {
            float av = avg(x, size);
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return sum / size - av * av;
        }
        // returns the covariance of X and Y
        private float cov(List<float> x, List<float> y, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * y[i];
            }
            sum /= size;

            return sum - avg(x, size) * avg(y, size);
        }
        private float avg(List<float> x, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; sum += x[i], i++) ;
            return sum / size;
        }

    }
}
