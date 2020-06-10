using System;
using System.Collections.Generic;
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
    public partial class WizardTwelve : Form
    {
        MainWindow puntero;

        public WizardTwelve(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RC_without_ReHeating_new_configuration_window = new RC_without_ReHeating_new_proposed_configuration();
            puntero.RC_without_ReHeating_new_configuration_window.MdiParent = puntero;
            puntero.RC_without_ReHeating_new_configuration_window.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_without_ReHeating_new_proposed_configuration_window = new RCMCI_without_ReHeating_new_proposed_configuration();
            puntero.RCMCI_without_ReHeating_new_proposed_configuration_window.MdiParent = puntero;
            puntero.RCMCI_without_ReHeating_new_proposed_configuration_window.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_without_ReHeating_new_proposed_configuration_window = new PCRC_without_ReHeating_new_proposed_configuration();
            puntero.PCRC_without_ReHeating_new_proposed_configuration_window.MdiParent = puntero;
            puntero.PCRC_without_ReHeating_new_proposed_configuration_window.Show();
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RC_withReHeating_new_configuration_window = new RC_withReHeating_new_proposed_configuration();
            puntero.RC_withReHeating_new_configuration_window.MdiParent = puntero;
            puntero.RC_withReHeating_new_configuration_window.Show();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_withReHeating_new_configuration_window = new PCRC_withReHeating_new_proposed_configuration();
            puntero.PCRC_withReHeating_new_configuration_window.MdiParent = puntero;
            puntero.PCRC_withReHeating_new_configuration_window.Show();
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_ReHeating_new_configuration_window = new RCMCI_with_ReHeating_new_proposed_configuration();
            puntero.RCMCI_with_ReHeating_new_configuration_window.MdiParent = puntero;
            puntero.RCMCI_with_ReHeating_new_configuration_window.Show();
        }
    }
}
