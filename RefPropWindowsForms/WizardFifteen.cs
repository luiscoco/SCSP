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
    public partial class WizardFifteen : Form
    {
        MainWindow puntero;

        public WizardFifteen(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRCMCI_with_Three_ReHeating_dialog = new PCRCMCI_with_Three_ReHeating();
            puntero.PCRCMCI_with_Three_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRCMCI_with_Three_ReHeating_dialog.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Two_Intercooling_with_Three_ReHeating_dialog = new RCMCI_with_Two_Intercooling_with_Three_Reheating();
            puntero.RCMCI_with_Two_Intercooling_with_Three_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Two_Intercooling_with_Three_ReHeating_dialog.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Two_Intercooling_with_Three_ReHeating_dialog = new PCRC_with_Two_Intercooling_with_Three_ReHeating();
            puntero.PCRC_with_Two_Intercooling_with_Three_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Two_Intercooling_with_Three_ReHeating_dialog.Show();
        }
    }
}
