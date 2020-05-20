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
    public partial class PCRCMCI_without_ReHeating_Optimization_Analysis_Results : Form
    {
        PCRCMCI_withoutReHeating puntero_aplicacion;

        public PCRCMCI_without_ReHeating_Optimization_Analysis_Results(PCRCMCI_withoutReHeating puntero1)
        {
            puntero_aplicacion = puntero1;

            InitializeComponent();
        }

        //Run Optimization
        private void button3_Click(object sender, EventArgs e)
        {

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

        //Run CIT analysis
        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
