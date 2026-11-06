using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace dentastu
{
    public class LagElement
    {

        private double[] coeffient; //分母の係数
        private double[] coeffient2; //分子の係数
        private double gein;
        private DK dk;
        private Complex[] res;

        public LagElement(double[] coef1, double[] coef2, double g)
        {

            coeffient = new double[coef1.Length];

            for(int i = 0; i < coeffient.Length; i++)
            {
                coeffient[i] = coef1[i];
            }



            coeffient2 = new double[coef2.Length];

            for (int i = 0; i < coeffient2.Length; i++)
            {
                coeffient2[i] = coef2[i];
            }

            gein = g;
        }

        public void SetDK()
        {
            dk = new DK(coeffient);
            dk.SetInitialValue(5);
            dk.Calclate(100);

            this.res = dk.GetZ();


            Console.WriteLine("residue");
            for(int i = 0; i < res.Length; i++)
            {
                Console.WriteLine(res[i]);
            }

            for (double t = 0.0; t <= 10; t += 0.1)
            {
                //Console.WriteLine(t.ToString() + "," + Calc(t).ToString());
            }
        }


        public double Calc(double t)
        {
            Complex temp = new Complex(0.0, 0.0);

            Polynomial num = new Polynomial(coeffient2);
            Polynomial den = new Polynomial(coeffient);

            for (int i = 0; i < this.coeffient.Length-1; i++)
            {
                temp += num.Func(res[i]) * Complex.Exp(t * res[i]) / dFunc(res[i]);
            }

            return temp.Real;
        }

        public Complex dFunc(Complex c1)
        {
            Complex sum = new Complex(1.0, 0.0);

            for (int i = 0; i < coeffient.Length-1; i++)
            {
                if (c1 != this.res[i]) sum *= (c1 - this.res[i]);
            }

            return sum;
        }

    }
}
