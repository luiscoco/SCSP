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
    public partial class WizardNine : Form
    {
        MainWindow puntero;

        //public Effec_TIT Effec_TIT_RC_withReHeating_Dialog;
        //public Effec_TIT_withoutReHeating Effec_TIT_RC_withoutReHeating_Dialog;

        public WizardNine(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           puntero.RC_with_LTR_PreHeating_without_ReHeating_dialog = new RC_with_LTR_PreHeating_without_ReHeating();
           puntero.RC_with_LTR_PreHeating_without_ReHeating_dialog.MdiParent = puntero;
           puntero.RC_with_LTR_PreHeating_without_ReHeating_dialog.Show();
        }
    }
}
