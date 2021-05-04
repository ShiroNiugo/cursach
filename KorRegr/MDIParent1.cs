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
            double x2, y2, xy, ee = 0, rr = 0, tt = 0, yy = 0, uu = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                clearForm();
                int nomer = 1, n = 0, k = 0;
                double[] d = new double[2];

                foreach (string line in File.ReadLines(openFileDialog1.FileName))
                {
                    string[] array = line.Split(";".ToCharArray());
                    if (array[0] != string.Empty && array[1] != string.Empty && double.TryParse(array[0], out x) && double.TryParse(array[1], out y)) // проверка пустоты и на число ячейки в строке
                    {
                        d[d.Length-2] = x; d[d.Length-1] = y;
                        Array.Resize(ref d, d.Length + 2);

                        n++;
                        x2 = Math.Pow(x, 2);
                        y2 = Math.Pow(y, 2);
                        xy = x * y;
                        ee+=x; rr+=y; tt+=x2; yy+=y2; uu+=xy;
                        childForm.dataGridView1.Rows.Add(nomer++, x, y, x2, y2, xy); // добавление строки
                        childForm.chart1.Series[0].Points.AddXY(x, y); // добавление точки
                    }
                }
                childForm.d = new double[n, 2];
                while (k < n) {
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < 2; j++)
                        {
                            childForm.d[i, j] = d[k];
                            k++;
                        }
                }

                for (int i = 0; i < n; i++)
                    childForm.dataGridView2.Rows.Add(childForm.d[i, 0], childForm.d[i, 1]);

                void Swap(ref double e1, ref double e2)
                {
                    var temp = e1;
                    e1 = e2;
                    e2 = temp;
                }

                int u = 0, o = 0;
                var array1 = new double[n];
                for (int i = 0; i < n; i++) array1[i] = childForm.d[i, 0];
                var len = array1.Length;
                for (var i = 1; i < len; i++)
                {
                    for (var j = 0; j < len - i; j++)
                    {
                        if (array1[j] > array1[j + 1])
                        {
                            Swap(ref array1[j], ref array1[j + 1]);
                        }
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    double p = array1[i];
                    o = Convert.ToDouble(childForm.dataGridView2[i, 0].Value);
                    if (o == p) childForm.dataGridView2.Rows.Add(u);
                    if (o < p) u++; 
                }

                childForm.dataGridView1.Rows.Add("Сумма", ee, rr, tt, yy, uu);
                childForm.dataGridView1.Rows.Add("Средняя величина", ee/nomer, rr/nomer, tt / nomer, yy / nomer, uu / nomer);


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