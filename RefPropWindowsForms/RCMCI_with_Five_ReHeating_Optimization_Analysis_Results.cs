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
    public partial class RCMCI_with_Five_ReHeating_Optimization_Analysis_Results : Form
    {
        public RCMCI_with_Five_Reheatings puntero_aplicacion;

        public RCMCI_with_Five_ReHeating_Optimization_Analysis_Results(RCMCI_with_Five_Reheatings puntero1)
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
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //Run CIT Optimization
        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
