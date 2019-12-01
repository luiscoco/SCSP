using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ChartDirector;
using CSharpChartExplorer;

namespace RefPropWindowsForms
{
    public partial class ChartsExample : Form
    {
        public ChartsExample()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            simplepie Chart_1_Example = new simplepie();
            Chart_1_Example.createChart(chartViewer1, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            contour Chart_2_Example = new contour();
            Chart_2_Example.createChart(chartViewer2, 1);
        }
    }   
}
