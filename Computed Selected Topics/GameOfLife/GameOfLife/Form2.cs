using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace GameOfLife
{
    public partial class Form2 : Form
    {
        private Series serie;
        public Form2(String titulo)
        {
            InitializeComponent();
            this.Text = titulo;
            serie = chart.Series.Add(titulo);
            serie.ChartType = SeriesChartType.Line;
            foreach(ChartArea chartArea in chart.ChartAreas)
            {
                chartArea.AxisX.ScaleView.Zoomable = true;
                chartArea.AxisY.ScaleView.Zoomable = true;
            }
        }


        public void Graficar(String nombre,double density)
        {
            serie.Points.Add(density);
            serie.AxisLabel = nombre;
        }
        
    }
}
