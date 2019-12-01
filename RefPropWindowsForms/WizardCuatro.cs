using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using System.Data.Common;
using System.Threading;

using sc.net;

using Excel = Microsoft.Office.Interop.Excel;

namespace RefPropWindowsForms
{
    public partial class WizardCuatro : Form
    {
        MainWindow puntero;

        //public Effec_TIT Effec_TIT_RC_withReHeating_Dialog;
        //public Effec_TIT_withoutReHeating Effec_TIT_RC_withoutReHeating_Dialog;

        public WizardCuatro(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        //Close Button
        private void button9_Click(object sender, System.EventArgs e)
        {
            this.Hide();
        }

        //PCRCMCI WithoutReheating
        private void button8_Click(object sender, System.EventArgs e)
        {
            
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            

        }
            private void button7_Click(object sender, System.EventArgs e)
        {
          
        }

        private void button1_Click(object sender, System.EventArgs e)
        {

        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRCMCI_without_ReHeating_dialog = new PCRCMCI_withoutReHeating();
            puntero.PCRCMCI_without_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRCMCI_without_ReHeating_dialog.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRCMCI_withReHeating_dialog = new PCRCMCI();
            puntero.PCRCMCI_withReHeating_dialog.MdiParent = puntero;
            puntero.PCRCMCI_withReHeating_dialog.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRCMCI_with_Two_ReHeating_dialog = new PCRCMCI_with_Two_ReHeating();
            puntero.PCRCMCI_with_Two_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRCMCI_with_Two_ReHeating_dialog.Show();
        }
    }
}
