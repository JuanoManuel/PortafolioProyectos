using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Langton_Ant
{
    public partial class Grafica : Form
    {
        private Series serie1,serie2,serie3;
        public Grafica(String titulo,String nombre1,String nombre2,String nombre3)
        {
            InitializeComponent();
            this.Text = titulo;
            serie1 = chart.Series.Add(nombre1);
            serie1.ChartType = SeriesChartType.Line;
            serie2 = chart.Series.Add(nombre2);
            serie2.ChartType = SeriesChartType.Line;
            serie3 = chart.Series.Add(nombre3);
            serie3.ChartType = SeriesChartType.Line;
            foreach (ChartArea chartArea in chart.ChartAreas)
            {
                chartArea.AxisX.ScaleView.Zoomable = true;
                chartArea.AxisY.ScaleView.Zoomable = true;
            }
        }


        public void Graficar(double value,double value2,double value3)
        {
            serie1.Points.Add(value);
            serie2.Points.Add(value2);
            serie3.Points.Add(value3);
        }
    }
}
