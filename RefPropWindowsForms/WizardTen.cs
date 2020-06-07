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
    public partial class WizardTen : Form
    {
        MainWindow puntero;

        //public Effec_TIT Effec_TIT_RC_withReHeating_Dialog;
        //public Effec_TIT_withoutReHeating Effec_TIT_RC_withoutReHeating_Dialog;

        public WizardTen(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_PreHeating_without_Reheating_dialog = new SB_with_PreHeating_without_Reheating();
            puntero.SB_with_PreHeating_without_Reheating_dialog.MdiParent = puntero;
            puntero.SB_with_PreHeating_without_Reheating_dialog.Show();
        }
    }
}
