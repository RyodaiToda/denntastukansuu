using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace dentastu
{
    public class FirstOrderLagElement
    {
        private double timeconstant;
        private double gein;
        private Polynomial denominator;
        private Polynomial numarator;
        private DK dk;
        private Complex[] res;//residue

        public FirstOrderLagElement(double t, double g)
        {
            this.timeconstant = t;
            this.gein = g;
        }

        public void SetTimeConstant(double t)
        {
            this.timeconstant = t;
            Console.WriteLine("timeconstant :" + t.ToString());
        }

        public void SetGein(double g)
        {
            this.gein = g;
            Console.WriteLine("gein : " + gein.ToString());
        }

        public void SetPolynomial()
        {
            double[] den = { 0.0, (1.0 + this.gein) / this.timeconstant, 1.0 };
            this.denominator = new Polynomial(den);

            double[] num = { this.gein / this.timeconstant };
            this.numarator = new Polynomial(num);
            

            dk = new DK(den);
            dk.SetInitialValue(5);
            dk.Calclate(100);

            this.res = dk.GetZ();


            //Console.WriteLine("Result");


            for (double t = 0.0; t <= 10; t += 0.1) 
            {
                Console.WriteLine(t.ToString() + "," + Calc(t).ToString());
            }

           // Console.WriteLine("dFunc(res[1])");
            //Console.WriteLine(dFunc(res[0]));

        }


        public double Calc(double t)
        {

            Complex temp = new Complex(0.0, 0.0);
            
            for(int i = 0; i < this.denominator.GetDegree(); i++)
            {
                temp += this.numarator.Func(res[i]) * Complex.Exp(t * res[i]) / dFunc(res[i]);
            }

            return temp.Real;

        }

        public Complex dFunc(Complex c1)
        {
            Complex sum = new Complex(1.0, 0.0);

            for (int i = 0; i < this.denominator.GetDegree(); i++)
            {
                if (c1 != this.res[i]) sum *= (c1 - this.res[i]);
            }

            return sum;
        }

    }
}
