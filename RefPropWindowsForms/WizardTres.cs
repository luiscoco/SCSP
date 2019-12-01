using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class WizardTres : Form
    {
        MainWindow puntero;

        public WizardTres(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        //Close button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
        }

        private void button17_Click(object sender, EventArgs e)
        {
           
        }

        private void button15_Click(object sender, EventArgs e)
        {
           
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Two_Intercooling_with_ReHeating_dialog = new PCRC_with_Two_Intercooling_with_ReHeating();
            puntero.PCRC_with_Two_Intercooling_with_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Two_Intercooling_with_ReHeating_dialog.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Two_Intercooling_with_Two_ReHeating_dialog = new PCRC_with_Two_Intercooling_with_Two_ReHeating();
            puntero.PCRC_with_Two_Intercooling_with_Two_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Two_Intercooling_with_Two_ReHeating_dialog.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Two_Intercooling_with_Two_ReHeating_dialog = new RCMCI_with_Two_Intercooling_with_Two_Reheating();
            puntero.RCMCI_with_Two_Intercooling_with_Two_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Two_Intercooling_with_Two_ReHeating_dialog.Show();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.RCMCI_with_Two_Intercooling_with_ReHeating_dialog = new RCMCI_with_Two_Intercooling_with_Reheating();
            puntero.RCMCI_with_Two_Intercooling_with_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Two_Intercooling_with_ReHeating_dialog.Show();
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //RCMCI_with_Two_Intercooling_without_Reheating RCMCI_with_Two_Intercooling_without_ReHeating_dialog;
            puntero.RCMCI_with_Two_Intercooling_without_ReHeating_dialog = new RCMCI_with_Two_Intercooling_without_Reheating();
            puntero.RCMCI_with_Two_Intercooling_without_ReHeating_dialog.MdiParent = puntero;
            puntero.RCMCI_with_Two_Intercooling_without_ReHeating_dialog.Show();
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Two_Intercooling_without_ReHeating_dialog = new PCRC_with_Two_Intercooling_without_ReHeating();
            puntero.PCRC_with_Two_Intercooling_without_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Two_Intercooling_without_ReHeating_dialog.Show();
        }

        private void LinkLabel12_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PCRC_with_Two_Intercooling_without_ReHeating_dialog = new PCRC_with_Two_Intercooling_without_ReHeating();
            puntero.PCRC_with_Two_Intercooling_without_ReHeating_dialog.MdiParent = puntero;
            puntero.PCRC_with_Two_Intercooling_without_ReHeating_dialog.Show();
        }
    }
}
