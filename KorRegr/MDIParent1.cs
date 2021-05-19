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
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 1;
        Form1 f = new Form1();

        public MDIParent1()
        {
            InitializeComponent();
        }

        double x, y;
        string namefile = "", file = "Text files (*.csv)|*.csv";

        void clearForm()
        {
            f.dataGridView1.Rows.Clear();
            f.chart1.Series[0].Points.Clear();
        }
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Текстовые файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 childForm = new Form1();
            childForm.MdiParent = this;
            childForm.Text = "Окно " + childFormNumber++;
            openFileDialog1.Filter = file;
            openFileDialog1.FileName = namefile;
            double SumX = 0, SumY = 0, SumX2 = 0, SumY2 = 0, SumXY = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                clearForm();
                int nomer = 0, n = 0, k = 0;
                double[] MassivDannhIzFile = new double[2];

                foreach (string row in File.ReadLines(openFileDialog1.FileName))
                {
                    string[] DanneIzFile = row.Split(';');
                    if (DanneIzFile[0] != string.Empty && DanneIzFile[1] != string.Empty && double.TryParse(DanneIzFile[0], out x) && double.TryParse(DanneIzFile[1], out y)) // проверка пустоты и на число ячейки в строке
                    {
                        MassivDannhIzFile[MassivDannhIzFile.Length - 2] = x; MassivDannhIzFile[MassivDannhIzFile.Length - 1] = y;
                        Array.Resize(ref MassivDannhIzFile, MassivDannhIzFile.Length + 2);
                        n++;
                        double x2 = Math.Pow(x, 2), y2 = Math.Pow(y, 2), xy = x * y;
                        SumX += x;  SumY += y;  SumX2 += x2;  SumY2 += y2; SumXY += xy;
                        childForm.dataGridView1.Rows.Add(1+nomer++, x, y, Math.Round(x2, 2), Math.Round(y2, 2), Math.Round(xy, 2)); // добавление строки
                        childForm.chart1.Series[0].Points.AddXY(x, y); // добавление точки
                    }
                }

                double sred(double x)
                {
                    return x / nomer;
                }

                double Ox = Math.Round(Math.Sqrt(sred(SumX2) - Math.Pow(sred(SumX), 2)), 2), 
                       Oy = Math.Round(Math.Sqrt(sred(SumY2) - Math.Pow(sred(SumY), 2)), 2);
                childForm.textBox1.Text = Ox.ToString();
                childForm.textBox2.Text = Oy.ToString();
                childForm.textBox3.Text = Math.Round((sred(SumXY)- sred(SumX)* sred(SumY)) / (Ox*Oy), 2).ToString();

                childForm.d = new double[n, 2];
                while (k < n) //запись из 1 в 2 массив
                {
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < 2; j++)
                        {
                            childForm.d[i, j] = MassivDannhIzFile[k];
                            k++;
                        }
                }
                childForm.dataGridView1.Rows.Add("Сумма", Math.Round(SumX, 2), Math.Round(SumY, 2), Math.Round(SumX2, 2), Math.Round(SumY2, 2), Math.Round(SumXY, 2));
                childForm.dataGridView1.Rows.Add("Средняя величина", Math.Round(sred(SumX), 2), Math.Round(sred(SumY), 2), Math.Round(sred(SumX2), 2), Math.Round(sred(SumY2), 2), Math.Round(sred(SumXY), 2));

                // заполнение 2 таблицы
                for (int i = 0; i < n; i++) childForm.dataGridView2.Rows.Add(childForm.d[i, 0], childForm.d[i, 1]);

                void SortAndRang(int y)
                {
                    var SortMas = new double[n];
                    for (int i = 0; i < n; i++)
                        SortMas[i] = childForm.d[i, y];

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

                    //for (int i = 0; i < DlyaRangN.Length; i++)
                    //    for (int j = 0; j < DlyaRangN.Length; j++)
                    //        if ((double)childForm.dataGridView1[y + 1, j].Value == DlyaRangN[i])
                    //            childForm.dataGridView2.Rows[j].Cells[y + 2].Value = i + 1;

                    for (int i = 0; i < DlyaRangN.Length; i++)
                    {
                        double row = 1, kol = 0;
                        for (int j = 0; j < DlyaRangN.Length; j++)
                        {
                            if ((double)childForm.dataGridView1[y + 1, j].Value == DlyaRangN[i])
                            {
                                row += i;
                                kol++;
                            }
                        }
                        for (int j = 0; j < DlyaRangN.Length; j++)
                            if ((double)childForm.dataGridView1[y + 1, j].Value == DlyaRangN[i])
                                childForm.dataGridView2.Rows[j].Cells[y + 2].Value = Math.Round(row / kol, 1);
                    }
                    
                }

                SortAndRang(0); // для Nx
                SortAndRang(1); // для Ny

                for (int i = 0; i < n; i++)
                {
                    childForm.dataGridView2.Rows[i].Cells[4].Value = Convert.ToDouble(childForm.dataGridView2.Rows[i].Cells[2].Value) - Convert.ToDouble(childForm.dataGridView2.Rows[i].Cells[3].Value);
                    childForm.dataGridView2.Rows[i].Cells[5].Value = Math.Pow(Convert.ToDouble(childForm.dataGridView2.Rows[i].Cells[4].Value), 2);
                }

                saveFileDialog1.FileName = openFileDialog1.FileName;
                namefile = openFileDialog1.SafeFileName;
            }
            childForm.Show();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
    }
}