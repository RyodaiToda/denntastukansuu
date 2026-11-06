using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace dentastu
{
    public partial class Form1 : Form
    {

        public Polynomial input;
        public FirstOrderLagElement fole = new FirstOrderLagElement(1.0, 1.0);
        public LagElement le;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(comboBox1.SelectedIndex == 0)
            {
                double[] a = {1.0};
                input = new Polynomial(a);

            }
            else if(comboBox1.SelectedIndex == 1)
            {
                double[] a = { 0.0, 1.0 };
                input = new Polynomial(a);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void gein_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox2.SelectedIndex == 0)//1次遅れ
            {
                /*  fole.SetGein(double.Parse(gein.Text));
                  fole.SetTimeConstant(double.Parse(timeconstant.Text));

                  fole.SetPolynomial();



                  Form2 form2 = new Form2(fole, double.Parse(textBox1.Text));

                  form2.Show();
                */


                double[] den = { 1.0 + double.Parse(gein.Text), double.Parse(timeconstant.Text) };
                Polynomial denp = new Polynomial(den);

                Polynomial pout = Polynomial.Multiplication(denp, input);

                double[] a = { 1.0 };

                //Console.WriteLine("pout Show Coef");
                //pout.ShowCoeff();

                le = new LagElement(pout.GetCoef(), a, double.Parse(gein.Text));
                le.SetDK();
                Form3 form3 = new Form3(le, double.Parse(textBox1.Text));
                form3.Show();

            } else if (comboBox2.SelectedIndex == 1)//2次遅れ
            {
                double[] den = { 1.0, double.Parse(tc1.Text), double.Parse(tc2.Text) };
                Polynomial denp = new Polynomial(den);

                Polynomial pout = Polynomial.Multiplication(denp, input);

                double[] a = { 1.0 };

                //Console.WriteLine("pout Show Coef");
                //pout.ShowCoeff();

                le = new LagElement(pout.GetCoef(), a, double.Parse(gein.Text));
                le.SetDK();
                Form3 form3 = new Form3(le, double.Parse(textBox1.Text));
                form3.Show();
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void t_max_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 1)
            {
                F_1.Visible = false;
                F_2.Visible = false;
                timeconstant.Visible = false;

                S_1.Visible = true;
                S_2.Visible = true;
                S_3.Visible = true;

                tc1.Visible = true;
                tc2.Visible = true;
            }else
            {
                F_1.Visible = true;
                F_2.Visible = true;
                timeconstant.Visible = true;

                S_1.Visible = false;
                S_2.Visible = false;
                S_3.Visible = false;

                tc1.Visible = false;
                tc2.Visible = false;
            }
        }

        private void timeconstant_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void S_1_Click(object sender, EventArgs e)
        {

        }

        private void F_3_Click(object sender, EventArgs e)
        {

        }
    }
}
