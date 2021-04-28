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
using System.Windows.Forms.DataVisualization.Charting;

namespace KorRegr
{
    public partial class Form1 : Form
    {
        //double Xn, Xh, dX, x, z;
        //string namefile = "", file = "Text files (*.txt)|*.txt";
        public Form1()
        {
            InitializeComponent();
            //chart1.Series[0].ChartType = SeriesChartType.Spline;
        }

        //void clearForm()
        //{
        //    dataGridView1.Rows.Clear();
        //    chart1.Series[0].Points.Clear();
        //}
        //OpenFileDialog openFileDialog1 = new OpenFileDialog();
        //SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        //private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    openFileDialog1.Filter = file;
        //    openFileDialog1.FileName = namefile;
        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        clearForm();

        //        foreach (string line in File.ReadLines(openFileDialog1.FileName))
        //        {
        //            string[] array = line.Split();
        //            dataGridView1.Rows.Add(array);
        //            int n = array.Length;
        //            for (int i = 0; i < array.Length; ++i)
        //            {
        //                for (int j = 0; j < dataGridView1.ColumnCount; ++j)
        //                {
        //                    if (j == 0)
        //                    {
        //                        x = Convert.ToDouble(array[j]);
        //                    }
        //                    else
        //                    {
        //                        z = Convert.ToDouble(array[j]);
        //                        chart1.Series[0].Points.AddXY(x, z);
        //                    }
        //                }
        //            }
        //        }
        //        saveFileDialog1.FileName = openFileDialog1.FileName;
        //        namefile = openFileDialog1.SafeFileName;
        //    }
        //}
    
    }
}
