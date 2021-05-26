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

namespace KorRegr
{
    public partial class Form1 : Form
    {
        public double[,] d;
        public int n;
        public double SumX, SumY, SumX2, SumY2, SumXY;

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Line(comboBox1.SelectedIndex+1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            update();
            Line(comboBox1.SelectedIndex+1);
        }

        public Form1()
        {
            InitializeComponent();
        }
        public Form1(double[,] d)
        {
            InitializeComponent();
            this.d = d;
        }

        void update()
        {
            // заполнение 2 таблицы
            for (int i = 0; i < n; i++) dataGridView2.Rows.Add(d[i, 0], d[i, 1]);

            void SortAndRang(int y)
            {
                var SortMas = new double[n];
                for (int i = 0; i < n; i++)
                    SortMas[i] = d[i, y];

                var DlyaRangN = new double[SortMas.Length];
                int kk = 0;

                Array.Sort(SortMas);
                Array.Reverse(SortMas);

                for (int i = 1; i < SortMas.Length; i++)
                    if (SortMas[i] != SortMas[i - 1])
                    {
                        var t = SortMas[i];
                        DlyaRangN[kk] = SortMas[i - 1];
                        DlyaRangN[kk + 1] = t;
                        kk++;
                    }


                for (int i = 0; i < DlyaRangN.Length; i++)
                {
                    double row = 0, kol = 0;
                    for (int j = 0; j < SortMas.Length; j++)
                    {
                        if (DlyaRangN[i] == SortMas[j])
                        {
                            row += j + 1;
                            kol++;
                        }
                    }
                    for (int j = 0; j < SortMas.Length; j++)
                        if (DlyaRangN[i] == (double)dataGridView1[y + 1, j].Value)
                            dataGridView2.Rows[j].Cells[y + 2].Value = Math.Round(row / kol, 1);
                }
            }

            SortAndRang(0); // для Nx
            SortAndRang(1); // для Ny

            double symma = 0;

            for (int i = 0; i < n; i++)
            {
                dataGridView2.Rows[i].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[i].Cells[2].Value) - Convert.ToDouble(dataGridView2.Rows[i].Cells[3].Value);
                dataGridView2.Rows[i].Cells[5].Value = Math.Pow(Convert.ToDouble(dataGridView2.Rows[i].Cells[4].Value), 2);
                symma += Convert.ToDouble(dataGridView2.Rows[i].Cells[5].Value);
            }
            dataGridView2.Rows.Add("n = " + n, null, null, null, "Сумма", symma);

            //расчеты корреляции
            textBox4.Text = (n > 50 && !(n < 30)) ? Convert.ToString(Math.Round((1 - Math.Pow(Convert.ToDouble(textBox3.Text), 2)) / Math.Sqrt(n), 2))
            : Convert.ToString(Math.Round(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(textBox3.Text), 2)) / Math.Sqrt(n - 2), 2)); ;
            textBox5.Text = Convert.ToString(Math.Round(Math.Abs(Convert.ToDouble(textBox3.Text)) / Convert.ToDouble(textBox4.Text), 2));

            var r = Math.Abs(Convert.ToDouble(textBox3.Text));


            var svyaz = (r == 0) ? "отсутствует" : (0 < r && r < 0.3) ? "слабая" : (0.3 <= r && r <= 0.7) ? "средней силы" : "сильная";

            label7.Text = "Связь между признаками х и у: " + svyaz;
            var df = n - 2;
            // textBox6.Text = Convert.ToString();

            //после рангов
            textBox7.Text = Convert.ToString(Math.Round(1 - ((6 * symma) / (n * (Math.Pow(n, 2) - 1))), 2));
        }

        void Line(int index)
        {
            
            chart1.Series[1].Points.Clear();
            var SortMasX = new double[n];
            for (int i = 0; i < n; i++)
                SortMasX[i] = d[i, 0];

            var Tochki = new double[SortMasX.Length];
            int kkk = 0;

            Array.Sort(SortMasX);
            Array.Reverse(SortMasX);

            for (int i = 1; i < SortMasX.Length; i++)
                if (SortMasX[i] != SortMasX[i - 1])
                {
                    var t = SortMasX[i];
                    Tochki[kkk] = SortMasX[i - 1];
                    Tochki[kkk + 1] = t;
                    kkk++;
                }

            for (int i = 0; i < Tochki.Length; i++)
            {
                double X = Tochki[i];
                double notYx;
                //switch (index)
                //{
                //    case 1:
                //    default:
                //        {
                //            var a0 = Math.Round((SumY * SumX2 - SumXY * SumX) / (n * SumX2 - Math.Pow(SumX, 2)), 2);
                //            var a1 = Math.Round((n * SumXY - SumX * SumY) / (n * SumX2 - Math.Pow(SumX, 2)), 2);
                //            notYx = a0 + a1 * X;//прямая
                //            break;
                //        }
                //    case 2:
                //        {
                //            var a0 = Math.Round((SumY * SumX2 - SumXY * SumX) / (n * SumX2 - Math.Pow(SumX, 2)), 2);
                //            var a1 = Math.Round((n * SumXY - SumX * SumY) / (n * SumX2 - Math.Pow(SumX, 2)), 2);
                //            notYx = a0 + a1 * X;//прямая
                //            break;
                //        }
                //    case 3:
                //        {
                            var a0 = Math.Round((SumY * SumX2 - SumXY * SumX) / (n * SumX2 - Math.Pow(SumX, 2)), 2);
                            var a1 = Math.Round(Math.Log10((n * SumX * Math.Log10(SumY) - SumX * Math.Log10(SumY)) / (n * SumY - Math.Pow(SumX, 2))), 2);
                            notYx = a0 * Math.Pow(a1, X);//показательная функция
                //            break;
                //        }
                //}
                chart1.Series[1].Points.AddXY(X, notYx);
            }
        }
    }
}
