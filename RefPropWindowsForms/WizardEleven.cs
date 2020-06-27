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
    public partial class WizardEleven : Form
    {
        MainWindow puntero;

        public WizardEleven(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating_dialog = new SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating(puntero);
            puntero.SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating_dialog.Show();
        }
    }
}
