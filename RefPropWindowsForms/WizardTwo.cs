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
    public partial class WizardTwo : Form
    {
        MainWindow puntero;

        //public Effec_TIT Effec_TIT_RC_withReHeating_Dialog;
        //public Effec_TIT_withoutReHeating Effec_TIT_RC_withoutReHeating_Dialog;

        public WizardTwo(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            puntero.RC_with_Two_ReHeating_dialog = new RC_with_Two_ReHeating();
            puntero.RC_with_Two_ReHeating_dialog.MdiParent = puntero;
            puntero.RC_with_Two_ReHeating_dialog.Show();
        }

        private void button35_Click(object sender, System.EventArgs e)
        {
           
        }

        //PCRC with two reheatings
        private void button3_Click(object sender, System.EventArgs e)
        {
            
        }

        //PCRC with three reheatings
        private void button25_Click(object sender, System.EventArgs e)
        {
           
        }

        //RCMCI with two reheatings
        private void button13_Click(object sender, System.EventArgs e)
        {
           
        }

        //RCMCI with three reheatings
        private void button11_Click(object sender, System.EventArgs e)
        {
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RC_with_Two_ReHeating_dialog = new RC_with_Two_ReHeating();
            puntero.RC_with_Two_ReHeating_dialog.MdiParent = puntero;
            puntero.RC_with_Two_ReHeating_dialog.Show();
        }

        private void button4_Click(object sender, System.EventArgs e)
        {

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Two_ReHeating_dialog = new PCRC_with_Two_ReHeating();
            puntero.PCRC_with_Two_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Two_ReHeating_dialog.Show();
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Two_ReHeating_dialog = new RCMCI_with_Two_Reheatings();
            puntero.RCMCI_with_Two_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Two_ReHeating_dialog.Show();
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Three_ReHeating_dialog = new RCMCI_with_Three_Reheatings();
            puntero.RCMCI_with_Three_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Three_ReHeating_dialog.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Three_ReHeating_dialog = new PCRC_with_Three_ReHeating();
            puntero.PCRC_with_Three_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Three_ReHeating_dialog.Show();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RC_with_Three_ReHeating_dialog = new RC_with_Three_ReHeating();
            puntero.RC_with_Three_ReHeating_dialog.MdiParent = puntero;
            puntero.RC_with_Three_ReHeating_dialog.Show();
        }
    }
}
