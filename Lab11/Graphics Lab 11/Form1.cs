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

namespace Graphics_Lab_11
{
    public partial class Form1 : Form
    {
        struct Simple
        {
            public double xx; public double yy; public int ii;
        };
        Simple s;
        FileInfo my_file = new FileInfo(@"C:\Users\admin\Desktop\SCRATCH");
        BinaryWriter fw;
        public Form1()
        { InitializeComponent(); }
        /* Создание файла Scratch и открытие его на запись */
        void pfopen()
        {
            fw = new BinaryWriter(my_file.Open(FileMode.Create,FileAccess.Write));

        }
        /* Запись в файл точки с флагом перемещения */
        void pmove(double x, double y)
        {
            s.xx = x; s.yy = y; s.ii = 0;
            fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);
        }
        /* Запись в файл точки с флагом рисования */
        void pdraw(double x, double y)
        {
            s.xx = x; s.yy = y; s.ii = 1;
            fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);
        }
        /* Закрытие файла */
        void pfclose()
        { fw.Close(); }
        /* Главная функция генерации квадрата и треугольника */
        void square_and_triangle(int n, double x0, double y0,double a, double phi)
        {
            double[] x = new double[5]; double[] y = new double[5];
            double[] xx = new double[5]; double[] yy = new double[5];
            double cphi, sphi, c1, c2, b, c, alpha, calpha, salpha;
            int i;
            if (n == 0) return;
            /* углы phi и alpha в радианах, угол delta в градусах */
            alpha = (45 + int.Parse(textBox3.Text)) * Math.PI / 180.0;
            x[0] = x[3] = x0; x[1] = x[2] = x0 + a;
            y[0] = y[1] = y0; y[2] = y[3] = y0 + a;
            calpha = Math.Cos(alpha); salpha = Math.Sin(alpha);
            c = a * calpha; b = a * salpha;
            x[4] = x[3] + c * calpha;
            y[4] = y[3] + c * salpha;
            /* Поворот вокруг точки (x0, y0) на угол phi;*/
            cphi = Math.Cos(phi); sphi = Math.Sin(phi);
            c1 = x0 - x0 * cphi + y0 * sphi;
            c2 = y0 - x0 * sphi - y0 * cphi;
            for (i = 0; i < 5; i++)
            {
                xx[i] = x[i] * cphi - y[i] * sphi + c1;
                yy[i] = x[i] * sphi + y[i] * cphi + c2;
            }
            pmove(xx[3], yy[3]);
            for (i = 0; i < 5; i++) pdraw(xx[i], yy[i]);
            pdraw(xx[2], yy[2]);
            square_and_triangle(n - 1, xx[3], yy[3], c, phi + alpha);
            square_and_triangle(n - 1, xx[4], yy[4], b, phi + alpha -
            0.5 * Math.PI);
            phi = double.Parse(textBox1.Text);
        }
        /* Вызов функции генерации дерева Пифагора */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    throw new Exception();
                }
                int n = int.Parse(textBox2.Text);
                pfopen();
                square_and_triangle(n, 0.0, 0.0, 1.0, 0.0);
                pfclose();
                MessageBox.Show("Дерево Пифагора записано в файл Scratch");
                
            }
            catch (Exception) { MessageBox.Show("Один из параметров не задан!"); }
        }
    }
 }
