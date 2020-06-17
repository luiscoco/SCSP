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
    public partial class WizardSeventeen : Form
    {
        MainWindow puntero;

        public WizardSeventeen(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_without_Reheating_dialog = new SB_without_Reheating();
            puntero.SB_without_Reheating_dialog.MdiParent = puntero;
            puntero.SB_without_Reheating_dialog.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Two_Recuperators_without_ReHeating_dialog = new SB_with_Two_Recuperators_without_ReHeating(puntero);
            puntero.SB_with_Two_Recuperators_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Two_Recuperators_without_ReHeating_dialog.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Three_Recuperators_without_ReHeating_dialog = new SB_with_Three_Recuperators_without_ReHeating(puntero);
            puntero.SB_with_Three_Recuperators_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Three_Recuperators_without_ReHeating_dialog.Show();
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Four_Recuperators_without_ReHeating_dialog = new SB_with_Four_Recuperators_without_ReHeating();
            puntero.SB_with_Four_Recuperators_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Four_Recuperators_without_ReHeating_dialog.Show();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Two_Recuperators_and_Additional_HX_without_ReHeating_dialog = new SB_with_Two_Recuperators_and_Additional_HX_without_ReHeating();
            puntero.SB_with_Two_Recuperators_and_Additional_HX_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Two_Recuperators_and_Additional_HX_without_ReHeating_dialog.Show();
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.SB_with_Three_Recuperators_and_One_Recompressor_without_ReHeating_dialog = new SB_with_Three_Recuperators_and_One_Recompressors_without_ReHeating(puntero);
            puntero.SB_with_Three_Recuperators_and_One_Recompressor_without_ReHeating_dialog.MdiParent = puntero;
            puntero.SB_with_Three_Recuperators_and_One_Recompressor_without_ReHeating_dialog.Show();
        }
    }
}
