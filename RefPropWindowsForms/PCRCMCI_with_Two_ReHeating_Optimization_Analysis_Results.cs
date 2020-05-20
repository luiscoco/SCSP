using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using sc.net;

using NLoptNet;

using Excel = Microsoft.Office.Interop.Excel;

using System.Reflection;

namespace RefPropWindowsForms
{
    public partial class PCRCMCI_with_Two_ReHeating_Optimization_Analysis_Results : Form
    {
        PCRCMCI_with_Two_ReHeating puntero_aplicacion;

        public PCRCMCI_with_Two_ReHeating_Optimization_Analysis_Results(PCRCMCI_with_Two_ReHeating puntero1)
        {
            puntero_aplicacion = puntero1;

            InitializeComponent();
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        //Run Optimization
        private void button2_Click(object sender, System.EventArgs e)
        {

        }

        //CIT Optimization analysis
        private void button3_Click(object sender, System.EventArgs e)
        {

        }
    }
}
