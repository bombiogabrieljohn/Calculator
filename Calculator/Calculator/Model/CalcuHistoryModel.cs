using Calculator.Model;
using System;
using System.Configuration;

namespace DemoCalculator.Model
{
    class CalcuHistoryModel 
    {
        public int Hist_ID { get; set; }

        public string Hist_Action { get; set; }

        public string Hist_Value { get; set; }

        public double computeValue(double in1, double in2, Operators op)
        {
            double result = 0;

            if (in1 == 0)
            {
                return in2;
            }
            switch (op)
            {
                case Operators.Add:
                    result = in1 + in2;
                    break;
                case Operators.Subtract:
                    result = in1 - in2;
                    break;
                case Operators.Multiply:
                    result = in1 * in2;
                    break;
                case Operators.Divide:
                    if (in2 == 0)
                    {
                        throw new DivideByZeroException(ConfigurationManager.AppSettings["DividedByZeroErrorMessage"].ToString());
                    }
                    result = in1 / in2;
                    break;
            }
            return result;
        }

    }
}
