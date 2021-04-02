using System;
using System.Collections.Generic;
using System.Text;
namespace MileStone1
{
    class Point
    {
        int x, y;
        public int X {
            get { return x; } 
            set { this.x = value; }
        }
        public int Y
        {
            get { return y; }
            set { this.y = value; }
        }
        Point(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
    class Line
    {
        float a;
        float b;
        // the line is ax + b
        public float A {
            get {
                return this.a; 
            }
            set {
                this.a = value; 
            } 
        }
        public float B
        {
            get
            {
                return this.b;
            }
            set
            {
                this.b = value;
            }
        }
        public Line(float a, float b) { //the line is ax+b
            this.a = a;
            this.b = b;
        }
    }

    class MostCorrelativeFinder
    {
        List<int> corrlativColumns;
        List<Line> linearRegList;
        public List<int> CorrlatedColumns
        {
            get {
                return this.corrlativColumns;
            }
        }
        //return list of regression lines that fit to the columns
        public List<Line> linearRegressionList
        {
            get
            {
                return this.linearRegList;
            }
        }
        //return the regresion line with the modt corrlative column to the gaven golumn
        public Line getRegrationLineWithTheMostcorrlative(int column) {

            return this.linearRegList[column];

        }
        MostCorrelativeFinder(List<List<float>> dataColumn) {
            this.corrlativColumns = new List<int>(dataColumn.Count);
            this.linearRegList = new List<Line>(dataColumn.Count);
            int sizeOfColumns = dataColumn.Count;
            //update the corrlation and linear regresion 
            for (int i = 0; i < sizeOfColumns; i++) {
               this.corrlativColumns[i] = findTheMostCorrelative(dataColumn, i);
                this.linearRegList[i] = linear_reg(dataColumn[i], dataColumn[this.corrlativColumns[i]]);
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
        // performs a linear regression and returns the line equation
        Line linear_reg(List<float> x, List<float> y)
        { 
            float a = cov(x, y, x.Count) / var(x, x.Count);
            float b = avg(y, y.Count) - a * (avg(x, x.Count));
            return new Line(a, b);
        }
    }
}
