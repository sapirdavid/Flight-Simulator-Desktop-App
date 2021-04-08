using System;
using System.Collections.Generic;
using System.Text;
namespace MileStone1
{
    public class Point
    {
        float x, y;
        public float X
        {
            get { return x; }
            set { this.x = value; }
        }
        public float Y
        {
            get { return y; }
            set { this.y = value; }
        }
        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class Line
    {
        float a;
        float b;
        // the line is ax + b
        public float A
        {
            get
            {
                return this.a;
            }
            set
            {
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
        public Line(float a, float b)
        { //the line is ax+b
            this.a = a;
            this.b = b;
        }
    }

    public class MostCorrelativeFinder
    {
        List<int> corrlativColumns;
        List<Line> linearRegList;
        public List<float> pearsonValue;
        public List<int> CorrlatedColumns
        {
            get
            {
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
        // public Line getRegrationLineWithTheMostcorrlative(int column)
        //  {

        //  return this.linearRegList[column];

        //  }
        public MostCorrelativeFinder(List<List<float>> dataColumn)
        {
            this.corrlativColumns = new List<int>();

            this.linearRegList = new List<Line>();

            this.pearsonValue = new List<float>();


            int sizeOfColumns = dataColumn.Count;
            //update the corrlation and linear regresion 
            for (int i = 0; i < sizeOfColumns; i++)
            {
                this.corrlativColumns.Add(findTheMostCorrelative(dataColumn, i));
                this.linearRegList.Add(linear_reg(dataColumn[i], dataColumn[this.corrlativColumns[i]]));
            }
        }

        public int findTheMostCorrelative(List<List<float>> dataCulomns, int column)
        {
            int mostCorrelativeIdx = -1; //no Correlative yet
            float mostCorrelativeVal = 0; //deafault
            int size = dataCulomns.Count;
            for (int i = 0; i < size; i++)
            {
                if (i == column) //if the same column of data
                    continue;
                if (mostCorrelativeIdx == -1)
                {
                    mostCorrelativeIdx = i; //if there isn't yet corrlation
                    mostCorrelativeVal = Math.Abs(pearson(dataCulomns[column], dataCulomns[i]));
                }
                else
                {
                    float currentCorrelativeVal = Math.Abs(pearson(dataCulomns[column], dataCulomns[i]));
                    if (currentCorrelativeVal > mostCorrelativeVal)
                    { //if more corrlative
                        mostCorrelativeIdx = i;
                        mostCorrelativeVal = currentCorrelativeVal;
                    }
                }
            }
            this.pearsonValue.Add(mostCorrelativeVal);
            return mostCorrelativeIdx;
        }

        private float pearson(List<float> x, List<float> y)
        {
            float varX = var(x, x.Count);
            float varY = var(y, y.Count);
            if(varX < 0 || varY < 0)
            {
                return 0;
            }
            float sqrtVarX = (float)(Math.Sqrt(varX));
            float sqrtVarY = (float)(Math.Sqrt(varY));
            //check devision in zero
            if (sqrtVarX != 0 && sqrtVarY != 0)
            {
                float covariance = cov(x, y, x.Count);
                return (covariance / (sqrtVarX * sqrtVarY));
            } else
            {
                return 0;
            }
        }
        // returns the variance of X and Y
        private float var(List<float> x, int size)
        {
            float average = avg(x, size);
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return ((sum / size) - (average * average));
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

            return (sum - (avg(x, size) * avg(y, size)));
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
            float varX = var(x, x.Count);
            float a, b;
            if (varX != 0)
            {
                a = cov(x, y, x.Count) / varX;
                b = avg(y, y.Count) - a * (avg(x, x.Count));
            } else
            {
                a = 0;
                b = 0;
            }
            return new Line(a, b);
        }

        public float calcY (Line regLine, float x)
        {
            return (regLine.A * x + regLine.B);

        }
    }
}
