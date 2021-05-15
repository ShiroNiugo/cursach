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

        double Xn, Xh, dX, x, y, x1, y1;
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
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 childForm = new Form1();
            childForm.MdiParent = this;
            childForm.Text = "Окно " + childFormNumber++;
            openFileDialog1.Filter = file;
            openFileDialog1.FileName = namefile;
            double ee = 0, rr = 0, tt = 0, yy = 0, uu = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                clearForm();
                int nomer = 1, n = 0, k = 0;
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
                        ee += x;  rr += y;  tt += x2;  yy += y2; uu += xy;
                        childForm.dataGridView1.Rows.Add(nomer++, x, y, x2, y2, xy); // добавление строки
                        childForm.chart1.Series[0].Points.AddXY(x, y); // добавление точки
                    }
                }
                childForm.d = new double[n, 2];
                while (k < n)
                {//запись из 1 в 2 массив
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < 2; j++)
                        {
                            childForm.d[i, j] = MassivDannhIzFile[k];
                            k++;
                        }
                }
                childForm.dataGridView1.Rows.Add("Сумма", ee, rr, tt, yy, uu);
                childForm.dataGridView1.Rows.Add("Средняя величина", ee / nomer, rr / nomer, tt / nomer, yy / nomer, uu / nomer);


                // заполнение 2 таблицы
                for (int i = 0; i < n; i++) childForm.dataGridView2.Rows.Add(childForm.d[i, 0], childForm.d[i, 1]);

                // для Nx
                var SortMasX = new double[n];
                var DlyaRangNx = new double[n];
                int kk = 0;

                for (int i = 0; i < n; i++) SortMasX[i] = childForm.d[i, 0];

                // Сортировка по убыванию
                Array.Sort(SortMasX);
                Array.Reverse(SortMasX);
                for (int i = 1; i < SortMasX.Length; i++)
                    if (SortMasX[i] != SortMasX[i - 1])
                    {
                        var t = SortMasX[i];
                        DlyaRangNx[kk] = SortMasX[i - 1];
                        DlyaRangNx[kk + 1] = t;
                        kk++;
                    }

                for (int i = 0; i < DlyaRangNx.Length; i++)
                    for (int j = 0; j < DlyaRangNx.Length; j++)
                        if (DlyaRangNx[i] == (double)childForm.dataGridView1[1, j].Value)
                            childForm.dataGridView2.Rows[j].Cells[2].Value = i + 1;

                // для Ny
                var SortMasY = new double[n];
                var DlyaRangNy = new double[n];
                int kkk = 0;

                for (int i = 0; i < n; i++) SortMasY[i] = childForm.d[i, 1];

                // Сортировка по убыванию
                Array.Sort(SortMasY);
                Array.Reverse(SortMasY);
                for (int i = 1; i < SortMasY.Length; i++)
                    if (SortMasY[i] != SortMasY[i - 1])
                    {
                        var t = SortMasY[i];
                        DlyaRangNy[kkk] = SortMasY[i - 1];
                        DlyaRangNy[kkk + 1] = t;
                        kkk++;
                    }

                for (int i = 0; i < DlyaRangNy.Length; i++)
                    for (int j = 0; j < DlyaRangNy.Length; j++)
                        if (DlyaRangNy[i] == (double)childForm.dataGridView1[2, j].Value)
                            childForm.dataGridView2.Rows[j].Cells[3].Value = i + 1;

                for (int i = 0; i < n; i++)
                    childForm.dataGridView2.Rows[i].Cells[4].Value = (int)childForm.dataGridView2.Rows[i].Cells[3].Value - (int)childForm.dataGridView2.Rows[i].Cells[2].Value;
                for (int i = 0; i < n; i++)
                    childForm.dataGridView2.Rows[i].Cells[5].Value = Math.Pow((int)childForm.dataGridView2.Rows[i].Cells[4].Value, 2);


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