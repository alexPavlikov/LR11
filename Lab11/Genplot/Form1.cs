using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Genplot
{
    public partial class Form1 : Form
    {
        struct Simple
        {
            public double xx; public double yy; public int ii;
        };
        Simple s;
        FileInfo my_file = new FileInfo(@"C:\Users\admin\Desktop\SCRATCH");
        BinaryReader fr;
        double x, y, xmin, xmax, ymin, ymax;
        double X, Y, Xmin, Xmax, Ymin, Ymax;
        double fx, fy, f, xC, yC, XC, YC, c1, c2;
        Graphics dc; Pen p;
        public Form1()
        {
            InitializeComponent();
            /* Задание границ области вывода по умолчанию */
            Xmin = 0.2; Xmax = 8.2; Ymin = 0.5; Ymax = 6.5;
            textBox1.Text = Xmin.ToString();
            textBox2.Text = Ymin.ToString();
            textBox3.Text = Xmax.ToString();
            textBox4.Text = Ymax.ToString();
            dc = pictureBox1.CreateGraphics();
            p = new Pen(Brushes.Black, 1);
        }
        /* Метод преобразования вещественной координаты X в целую */
        private int IX(double x)
        {
            double xx = x * (pictureBox1.Size.Width / 10.0) + 0.5;
            return (int)xx;
        }
        /* Метод преобразования вещественной координаты Y в целую */
        private int IY(double y)
        {
            double yy = pictureBox1.Size.Height - y *

            (pictureBox1.Size.Height / 7.0) + 0.5;

            return (int)yy;
        }
        /* Функция вычерчивания линии (экран 10х7 условн. единиц) */
        private void Draw(double x1, double y1, double x2, double y2)
        {
            Point point1 = new Point(IX(x1), IY(y1));
            Point point2 = new Point(IX(x2), IY(y2));
            dc.DrawLine(p, point1, point2);
        }
        /* Функция прорисовки меток-уголков области вывода */
        private void initViewPort(double Xmin, double Ymin,
        double Xmax, double Ymax)

        {
            Draw(Xmin, Ymin, Xmin + 0.2, Ymin);
            Draw(Xmin, Ymin, Xmin, Ymin + 0.2);
            Draw(Xmin, Ymax, Xmin + 0.2, Ymax);
            Draw(Xmin, Ymax, Xmin, Ymax - 0.2);
            Draw(Xmax, Ymin, Xmax - 0.2, Ymin);
            Draw(Xmax, Ymin, Xmax, Ymin + 0.2);
            Draw(Xmax, Ymax, Xmax - 0.2, Ymax);
            Draw(Xmax, Ymax, Xmax, Ymax - 0.2);
            Draw(Xmax / 2 - 0.2, Ymin, Xmax / 2 + 0.2, Ymin);
            Draw(Xmax / 2, Ymin, Xmax / 2, Ymin + 0.2);
        }
        /* Чтение из файла Scratch и вычерчивание кривой */
        private void button2_Click(object sender, EventArgs e)
        {
            double Xold = 0, Yold = 0;
            /* Первый проход для определения границ окна xmin, ymin,
            xmax, ymax */
            fr = new BinaryReader(my_file.Open(FileMode.Open,FileAccess.Read));
            while (fr.BaseStream.Position < fr.BaseStream.Length)
            {
                s.xx = fr.ReadDouble(); s.yy = fr.ReadDouble();
                s.ii = fr.ReadInt32();
                x = s.xx; y = s.yy;
                if (x < xmin) xmin = x;
                if (x > xmax) xmax = x;
                if (y < ymin) ymin = y;
                if (y > ymax) ymax = y;
            }
            fr.Close();
            /* Вызов функции отображения границ области вывода */
            initViewPort(Xmin, Ymin, Xmax, Ymax);
            /* Получение коэффициентов формулы перевода мировых
            координат в экранные */
            fx = (Xmax - Xmin) / (xmax - xmin);
            fy = (Ymax - Ymin) / (ymax - ymin);
            f = (fx < fy ? fx : fy);
            xC = 0.5 * (xmin + xmax); yC = 0.5 * (ymin + ymax);
            XC = 0.5 * (Xmin + Xmax); YC = 0.5 * (Ymin + Ymax);
            c1 = XC - f * xC; c2 = YC - f * yC;
            /* Второй проход для вычерчивания */
            fr = new BinaryReader(my_file.Open(FileMode.Open,FileAccess.Read));
            while (fr.BaseStream.Position < fr.BaseStream.Length)
            {
                s.xx = fr.ReadDouble(); s.yy = fr.ReadDouble();
                s.ii = fr.ReadInt32();
                x = s.xx; y = s.yy;
                X = f * x + c1; Y = f * y + c2;
                if (s.ii == 1) { Draw(Xold, Yold, X, Y); }
                Xold = X; Yold = Y;
            }
            fr.Close();
        }
        /* Очистка области вывода */
        private void button3_Click(object sender, EventArgs e)
        { dc.Clear(Color.White); }
        /* Изменение границ области вывода */
        private void button1_Click(object sender, EventArgs e)
        {
            Xmin = Convert.ToDouble(textBox1.Text);
            Ymin = Convert.ToDouble(textBox2.Text);
            Xmax = Convert.ToDouble(textBox3.Text);
            Ymax = Convert.ToDouble(textBox4.Text);
            MessageBox.Show("Координаты области вывода изменены");
        }
    }
}
