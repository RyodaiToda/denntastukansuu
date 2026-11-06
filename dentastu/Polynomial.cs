using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace dentastu
{
    public class Polynomial
    {
        private int degree;
        private double[] coefficient;

        public Polynomial(int n, double[] coef)
        {

            this.degree = n;
            this.coefficient = new double[n + 1];

            for(int i = 0; i <= degree; i++)
            {
                this.coefficient[i] = coef[i];
            }

        }

        public Polynomial( double[] coef)
        {
            this.degree = coef.Length - 1;
            this.coefficient = new double[this.degree + 1];

            for (int i = 0; i <= degree; i++)
            {
                this.coefficient[i] = coef[i];
            }

        }

        public Complex Func(Complex c1)
        {
            Complex zout = new Complex(0.0, 0.0);

            for (int i = 0; i <= this.degree; i++)
            {
                zout += Complex.Pow(c1, i) * this.coefficient[i];
            }

            return zout;
        }



        public void ShowDegree()
        {
            Console.WriteLine("degree = " + this.degree.ToString());
        }

        public void ShowCoeff()
        {

            Console.Write("coeff = [");
            for(int i = 0; i <=this.degree; i++)
            {
                Console.Write(this.coefficient[i].ToString() + ",");
            }

            Console.WriteLine("]");
        }


        public int GetDegree()
        {
            return this.degree;
        }

        public double[] GetCoef()
        {
            return this.coefficient;
        }

        public static Polynomial Addition(Polynomial p1, Polynomial p2)
        {

            int deg1 = p1.GetDegree();
            int deg2 = p2.GetDegree();

            int deg = (deg1 > deg2) ? deg1 : deg2;

            double[] coef1 = p1.GetCoef();
            double[] coef2 = p2.GetCoef();

            double[] coef = new double[deg + 1];

            for(int i = 0; i <= deg; i++)
            {
                coef[i] = (i <= deg1) ? coef1[i] : 0.0;
                coef[i] += (i <= deg2) ? coef2[i] : 0.0;
            }


            Polynomial pout = new Polynomial(deg, coef);
            return pout;
        }


        public static Polynomial Multiplication(Polynomial p1, Polynomial p2)
        {
            int deg1 = p1.GetDegree();
            int deg2 = p2.GetDegree();

            int deg = deg1 + deg2;

            double[] coef1 = p1.GetCoef();
            double[] coef2 = p2.GetCoef();

            double[] coef = new double[deg + 1];


            double sum ;

            for (int k = 0; k <= deg; k++) 
            {
                sum = 0.0;

                for (int i = 0; i <= deg1 ; i++) 
                {
                    if (k - i <= deg2 && k - i >= 0) sum += coef1[i] * coef2[k - i];
                }

                coef[k] = sum;
            }


            Polynomial pout = new Polynomial(deg, coef);
            return pout;
        }
    }
}
