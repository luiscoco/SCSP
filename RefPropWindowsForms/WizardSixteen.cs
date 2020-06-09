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
    public partial class WizardSixteen : Form
    {
        MainWindow puntero;

        public WizardSixteen(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RC_with_Two_ReHeating_new_configuration_window = new RC_with_Two_ReHeating_new_proposed_configuration();
            puntero.RC_with_Two_ReHeating_new_configuration_window.MdiParent = puntero;
            puntero.RC_with_Two_ReHeating_new_configuration_window.Show();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Two_ReHeating_new_configuration_window = new PCRC_with_Two_ReHeating_new_proposed_configuration();
            puntero.PCRC_with_Two_ReHeating_new_configuration_window.MdiParent = puntero;
            puntero.PCRC_with_Two_ReHeating_new_configuration_window.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Two_ReHeating_new_configuration_window = new RCMCI_with_Two_ReHeating_new_proposed_configuration();
            puntero.RCMCI_with_Two_ReHeating_new_configuration_window.MdiParent = puntero;
            puntero.RCMCI_with_Two_ReHeating_new_configuration_window.Show();
        }
    }
}
