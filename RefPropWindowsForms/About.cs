using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RefPropWindowsForms
{
    public partial class About : Form
    {
        MainWindow puntero;
        public About(MainWindow puntero_Main_Window)
        {
            puntero = puntero_Main_Window;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "dVeN#we@tNH_?¨Çç121345873sgd*DºGºªf+sd+sg210^`0:s:9ssdf4gsd532")
            {
                puntero.configurations712ToolStripMenuItem.Enabled = true;
                puntero.configurations1318ToolStripMenuItem.Enabled = true;
                puntero.configurations1924ToolStripMenuItem.Enabled = true;
                puntero.configurations2224ToolStripMenuItem.Enabled = true;
                puntero.configurations2527ToolStripMenuItem.Enabled = true;
                puntero.configurations2830ToolStripMenuItem.Enabled = true;
                puntero.configurations3133ToolStripMenuItem.Enabled = true;
                puntero.configurations3437ToolStripMenuItem.Enabled = true;
                puntero.configurations3840ToolStripMenuItem.Enabled = true;
                puntero.configurationsToolStripMenuItem.Enabled = true;
                puntero.adobePDFViewerToolStripMenuItem.Enabled = true;
                puntero.chartsExampleToolStripMenuItem.Enabled = true;
                puntero.toolsToolStripMenuItem.Enabled = true;
                puntero.aboutToolStripMenuItem.Enabled = true;
                puntero.validationsToolStripMenuItem.Enabled = true;
                puntero.configurationsSummaryToolStripMenuItem.Enabled = true;
            }
            else
            {
                puntero.configurations712ToolStripMenuItem.Enabled = true;
                puntero.configurations1318ToolStripMenuItem.Enabled = true;
                puntero.configurations1924ToolStripMenuItem.Enabled = true;
                puntero.configurations2224ToolStripMenuItem.Enabled = false;
                puntero.configurations2527ToolStripMenuItem.Enabled = false;
                puntero.configurations2830ToolStripMenuItem.Enabled = false;
                puntero.configurations3133ToolStripMenuItem.Enabled = false;
                puntero.configurations3437ToolStripMenuItem.Enabled = false;
                puntero.configurations3840ToolStripMenuItem.Enabled = false;
                puntero.configurationsToolStripMenuItem.Enabled = false;
                puntero.adobePDFViewerToolStripMenuItem.Enabled = false;
                puntero.chartsExampleToolStripMenuItem.Enabled = false;
                puntero.toolsToolStripMenuItem.Enabled = true;
                puntero.aboutToolStripMenuItem.Enabled = false;
                puntero.validationsToolStripMenuItem.Enabled = false;
                puntero.configurationsSummaryToolStripMenuItem.Enabled = false;
            }
            this.Dispose();
        }
    }
}
