using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;



namespace dentastu
{
   

    class DK
    {
        const double M_PI = 3.141592653589793;

        private int degree;
        private Complex[] z;
        private double[] coefficient;


        public DK(double[] coef)
        {
            this.degree = coef.Length - 1;

            this.z = new Complex[this.degree];

            this.coefficient = new double[this.degree + 1];

            for(int i = 0; i <= this.degree; i++)
            {
                this.coefficient[i] = coef[i];
            }


        }

        public void ShowDegree()
        {
            Console.WriteLine("degree = " + this.degree);    
        }

        public void ShowCoef()
        {
            Console.Write("Coefficient = [");

            for (int i = 0; i <= this.degree; i++)
            {
                Console.Write(this.coefficient[i] + ",");
            }

            Console.WriteLine("]");
        }

        public void ShowZ()
        {
            {
                Console.Write("z = [");

                for (int i = 0; i < this.degree; i++)
                {
                    Console.Write(this.z[i] + ",");
                }

                Console.WriteLine("]");
            }
        }

        public Complex[] GetZ()
        {
            return this.z;
        }


        public Complex Func(Complex c1)
        {
            Complex zout = new Complex(0.0, 0.0);

            for (int i = 0; i <= this.degree; i++) 
            {
                zout += Complex.Pow(c1, i) * coefficient[i];
            }

            return zout;
        }


        public Complex dFunc(Complex c1)
        {
            Complex sum = new Complex(this.coefficient[degree], 0.0);

            for (int i = 0; i < degree; i++)
            {
                if (c1 != this.z[i]) sum *= (c1 - this.z[i]);
            }

            return sum;
        }

        public void Calclate(int ite)
        {
            int k = 0;

            while (k <= ite)
            {

                for(int i = 0; i < this.degree; i++)
                {
                    this.z[i] = this.z[i] - this.Func(this.z[i]) / this.dFunc(this.z[i]);
                }

                k++;

            }

            Console.WriteLine("The solutions");
            this.ShowZ();


        }

        public void SetInitialValue(double r)
        {
            double beta = -1.0 * this.coefficient[this.degree - 1] / (this.degree * this.coefficient[this.degree]);

            Complex temp;

            for(int i = 0; i < this.degree; i++)
            {
                temp = new Complex(0.0, (2.0 * M_PI * i + 1.5) / this.degree);
                this.z[i] = beta + (Complex)r * Complex.Exp(temp);
            }

        }

    }
}
