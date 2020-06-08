using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RefPropWindowsForms
{
    public partial class WizardFourteen : Form
    {
        MainWindow puntero;

        public WizardFourteen(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RC_with_Four_ReHeating_dialog = new RC_with_Four_ReHeating();
            puntero.RC_with_Four_ReHeating_dialog.MdiParent = puntero;
            puntero.RC_with_Four_ReHeating_dialog.Show();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RC_with_Five_ReHeating_dialog = new RC_with_Five_ReHeating();
            puntero.RC_with_Five_ReHeating_dialog.MdiParent = puntero;
            puntero.RC_with_Five_ReHeating_dialog.Show();
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Four_ReHeating_dialog = new PCRC_with_Four_ReHeatings();
            puntero.PCRC_with_Four_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Four_ReHeating_dialog.Show();
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Five_ReHeating_dialog = new PCRC_with_Five_ReHeatings();
            puntero.PCRC_with_Five_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Five_ReHeating_dialog.Show();
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Four_ReHeating_dialog = new RCMCI_with_Four_Reheatings();
            puntero.RCMCI_with_Four_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Four_ReHeating_dialog.Show();
        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Five_ReHeating_dialog = new RCMCI_with_Five_Reheatings();
            puntero.RCMCI_with_Five_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Five_ReHeating_dialog.Show();
        }
    }
}
