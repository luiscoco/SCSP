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
    public partial class WizardEighteen : Form
    {
        MainWindow puntero;

        public WizardEighteen(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Three_Recuperators_and_Two_Recompressor_without_ReHeating_dialog = new SB_with_Three_Recuperators_and_Two_Recompressor_without_ReHeating(puntero);
            puntero.SB_with_Three_Recuperators_and_Two_Recompressor_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Three_Recuperators_and_Two_Recompressor_without_ReHeating_dialog.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating_dialog = new SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating(puntero);
            puntero.SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating_dialog.Show();
        }
    }
}
