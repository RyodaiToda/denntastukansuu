using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dentastu
{
    public partial class Form3 : Form
    {
        public LagElement le;
        PictureBox pb1 = new PictureBox();
        private double tmax;

        public Form3(LagElement l, double t)
        {
            this.Size = new Size(640, 480);

            pb1.Width = this.Width;
            pb1.Height = this.Height;

            Controls.Add(pb1);

            le = l;
            tmax = t;

            DrawGraph();



            InitializeComponent();


        }


        public void DrawGraph()
        {

            Bitmap canvas = new Bitmap(pb1.Width, pb1.Height);
            Graphics g = Graphics.FromImage(canvas);

            double t = 0;
            double dt = tmax / Width;

            int marginx = 30;//ｘ軸のマージン
            int marginy = 30;//y軸のマージン

            Pen p = new Pen(Color.Black, 1);
            g.DrawLine(p, 0, this.Height / 2 + marginy, this.Width, this.Height / 2 + marginy);//x axis
            g.DrawLine(p, marginx, 0, marginx, this.Height);//y axis



            Font fnt = new Font("Arial", 10, FontStyle.Italic);


            //ｘ軸に数字をフル
            for (int i = 0; i < 4; i++)
            {
                g.DrawString((i * tmax / 4).ToString(), fnt, Brushes.Black, i * 160 + marginx, 240 + marginy);
            }

            double max = 0.0;

            while ((int)(t / dt) + marginx < this.Width)
            {
                if (Abs(le.Calc(t)) > max) max = le.Calc(t);
                t += dt;
            }

            t = 0.0;

            //y軸に数字を振る
            for (int i = 0; i <= 4; i++)
            {
                g.DrawString((i * max / 4).ToString("F2"), fnt, Brushes.Black, marginx - 20, marginy + 240 - i * Height / (2 * 4));
            }

            while ((int)(t / dt) + marginx < this.Width)
            {
                Console.WriteLine(t + "," + le.Calc(t));

                //Draw_XY(g, (int)(t * 150), (int)(t * 150));
                canvas.SetPixel((int)(t / dt) + marginx, this.Height / 2 - (int)(Height * le.Calc(t) / (2 * max)) + marginy, Color.Black);
                t += dt;
            }


            //リソースを解放する
            g.Dispose();
            //PictureBox1に表示する
            pb1.Image = canvas;

        }

        public void Draw_XY(Graphics g, int x1, int y1)
        {
            Pen p = new Pen(Color.Black, 1);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            //g.DrawLine(p, x1, -y1 + this.Height / 2, x2, -y2 + this.Height / 2);
            g.FillEllipse(Brushes.Black, x1, -y1 + this.Height / 2, 3, 3);



        }



        private double Abs(double d)
        {
            if (d >= 0.0) return d;
            else return -1.0 * d;
        }

    }
}
