using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RefPropWindowsForms
{
    public partial class WizardCinco : Form
    {
        MainWindow puntero;

        //public Effec_TIT Effec_TIT_RC_withReHeating_Dialog;
        //public Effec_TIT_withoutReHeating Effec_TIT_RC_withoutReHeating_Dialog;

        public WizardCinco(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.Two_PC_Two_RCMCI_withoutReheating_dialog = new Two_PC_Two_RCMCI_withoutReheating();
            puntero.Two_PC_Two_RCMCI_withoutReheating_dialog.MdiParent = puntero;
            puntero.Two_PC_Two_RCMCI_withoutReheating_dialog.Show();
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.Two_PC_RCMCI_withoutReheating_dialog = new Two_PC_RCMCI_withoutReheating();
            puntero.Two_PC_RCMCI_withoutReheating_dialog.MdiParent = puntero;
            puntero.Two_PC_RCMCI_withoutReheating_dialog.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.PC_Two_RCMCI_withoutReheating_dialog = new PC_Two_RCMCI_withoutReheating();
            puntero.PC_Two_RCMCI_withoutReheating_dialog.MdiParent = puntero;
            puntero.PC_Two_RCMCI_withoutReheating_dialog.Show();
        }

        //Close Wizard
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
