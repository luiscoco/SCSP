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
    public partial class PCRC_with_Two_Intercooling_withTwoReheating_Analysis_Results : Form
    {
        PCRC_with_Two_Intercooling_with_Two_ReHeating puntero_aplicacion;

        public PCRC_with_Two_Intercooling_withTwoReheating_Analysis_Results(PCRC_with_Two_Intercooling_with_Two_ReHeating puntero1)
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
    }
}
