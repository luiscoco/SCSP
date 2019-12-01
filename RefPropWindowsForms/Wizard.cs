using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NLoptNet;

//using Accord;
//using Accord.Math.Optimization;

namespace RefPropWindowsForms
{
    public partial class Wizard : Form
    {
        MainWindow puntero;

        public Effec_Recomp_Fract_withoutReHeating Effec_Recomp_Fract_withoutReHeating_Dialog;
        public Effec_CIT_withoutReHeating Effec_CIT_withoutReHeating_Dialog;
        public Effec_TIP_withoutReHeating Effec_TIP_withoutReHeating_Dialog;

        public Effec_Recomp_Fract Effec_Recomp_Fract_Dialog;
        public Effec_CIT Effec_CIT_Dialog;
        public Effec_TIP Effec_TIP_Dialog;

        public Effec_Recomp_Fract_PCRC_withReHeating Effec_Recomp_Fract_PCRC_withReHeating_Dialog;
        public Effec_CIT_PCRC_withReHeating Effec_CIT_PCRC_withReHeating_Dialog;
        public Effec_TIP_PCRC_withReHeating Effec_TIP_PCRC_withReHeating_Dialog;

        public Effec_Recomp_Fract_PCRC_withoutReHeating Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog;
        public Effec_CIT_PCRC_withoutReHeating Effec_CIT_PCRC_withoutReHeating_Dialog;
        public Effec_TIP_PCRC_withoutReHeating Effec_TIP_PCRC_withoutReHeating_Dialog;

        public Effec_Recomp_Fract_RCMCI_withoutReHeating Effec_Recomp_Fract_RCMCI_withoutReHeating_Dialog;
        public Effec_CIT_RCMCI_withoutReHeating Effec_CIT_RCMCI_withoutReHeating_Dialog;
        public Effec_TIP_RCMCI_withoutReHeating Effec_TIP_RCMCI_withoutReHeating_Dialog;

        public Effec_Recomp_Fract_RCMCI_withReHeating Effec_Recomp_Fract_RCMCI_withReHeating_Dialog;
        public Effec_CIT_RCMCI_withReHeating Effec_CIT_RCMCI_withReHeating_Dialog;
        public Effec_TIP_RCMCI_withReHeating Effec_TIP_RCMCI_withReHeating_Dialog;

        public Effec_TIT Effec_TIT_RC_withReHeating_Dialog;
        public Effec_TIT_withoutReHeating Effec_TIT_RC_withoutReHeating_Dialog;

        public Wizard(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //RC Detail Design with ReHeating
        private void button2_Click(object sender, EventArgs e)
        {
            puntero.DesignPoint_Click(this,e);
        }

        //RC Optimization with ReHeating
        private void button4_Click(object sender, EventArgs e)
        {
            puntero.optimizationdesignToolStripMenuItem_Click(this, e);
        }

        //RC Detail Design without ReHeating
        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        //RC Optimization without ReHeating
        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        //PCRC Detail Design with ReHeating
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        //PCRC Optimization with ReHeating
        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        //PCRC Detail Design without ReHeating
        private void button9_Click(object sender, EventArgs e)
        {
            
        }

        //PCRC Optimization without ReHeating
        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        //RCMCI Detail Design with ReHeating
        private void button13_Click(object sender, EventArgs e)
        {
            
        }

        //RCMCI Optimization with ReHeating
        private void button12_Click(object sender, EventArgs e)
        {
            
        }

        //RCMCI Detail Design without ReHeating
        private void button11_Click(object sender, EventArgs e)
        {
            
        }

        //RCMCI Optimization without ReHeating
        private void button10_Click(object sender, EventArgs e)
        {
            
        }

        //Sensing CIT RC with ReHeating
        private void button14_Click(object sender, EventArgs e)
        {
            Effec_CIT_Dialog = new Effec_CIT();
            Effec_CIT_Dialog.MdiParent = puntero;
            Effec_CIT_Dialog.Show();
        }

        //Sensing TIP RC with ReHeating
        private void button15_Click(object sender, EventArgs e)
        {
            Effec_TIP_Dialog = new Effec_TIP();
            Effec_TIP_Dialog.MdiParent = puntero;
            Effec_TIP_Dialog.Show();
        }

        //Sensing Recompression Flow Fraction in RC with ReHeating
        private void button16_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_Dialog = new Effec_Recomp_Fract();
            Effec_Recomp_Fract_Dialog.MdiParent = puntero;
            Effec_Recomp_Fract_Dialog.Show();
        }

        //Sensing CIT in RC without ReHeating
        private void button19_Click(object sender, EventArgs e)
        {
           
        }

        //Sensing TIP in RC without ReHeating
        private void button18_Click(object sender, EventArgs e)
        {
           
        }

        //Sensing Recompression Fraction in RC without ReHeating
        private void button17_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_withoutReHeating_Dialog = new Effec_Recomp_Fract_withoutReHeating();
            Effec_Recomp_Fract_withoutReHeating_Dialog.MdiParent = puntero;
            Effec_Recomp_Fract_withoutReHeating_Dialog.Show();
        }

        //Sensing Recompression Fraction in PCRC without ReHeating
        private void button23_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog = new Effec_Recomp_Fract_PCRC_withoutReHeating();
            Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog.MdiParent = puntero;
            Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog.Show();
        }

        //Sensing CIT in PCRC without ReHeating
        private void button25_Click(object sender, EventArgs e)
        {
           
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Effec_TIP_PCRC_withoutReHeating_Dialog = new Effec_TIP_PCRC_withoutReHeating();
            Effec_TIP_PCRC_withoutReHeating_Dialog.MdiParent = puntero;
            Effec_TIP_PCRC_withoutReHeating_Dialog.Show();
        }


        //Sensing Recompression Fraction in PCRC with ReHeating
        private void button20_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_PCRC_withReHeating_Dialog = new Effec_Recomp_Fract_PCRC_withReHeating();
            Effec_Recomp_Fract_PCRC_withReHeating_Dialog.MdiParent = puntero;
            Effec_Recomp_Fract_PCRC_withReHeating_Dialog.Show();
        }

        //Sensing analysis CIT PCRC with ReHeating
        private void button22_Click(object sender, EventArgs e)
        {
            Effec_CIT_PCRC_withReHeating_Dialog = new Effec_CIT_PCRC_withReHeating();
            Effec_CIT_PCRC_withReHeating_Dialog.MdiParent = puntero;
            Effec_CIT_PCRC_withReHeating_Dialog.Show();
        }

        //Sensing analysis TIP PCRC with ReHeating
        private void button21_Click(object sender, EventArgs e)
        {
            Effec_TIP_PCRC_withReHeating_Dialog = new Effec_TIP_PCRC_withReHeating();
            Effec_TIP_PCRC_withReHeating_Dialog.MdiParent = puntero;
            Effec_TIP_PCRC_withReHeating_Dialog.Show();
        }  

        private void Wizard_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.DesignPoint_Click(this, e);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimizationdesignToolStripMenuItem_Click(this, e);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.designPointToolStripMenuItem_Click(this, e);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimaldesignToolStripMenuItem_Click(this, e);
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.designPointToolStripMenuItem1_Click(this, e);
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimaldesignToolStripMenuItem1_Click(this, e);
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.designPointToolStripMenuItem4_Click(this, e);
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimaldesignToolStripMenuItem4_Click(this, e);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.designPointToolStripMenuItem2_Click(this, e);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimaldesignToolStripMenuItem2_Click(this, e);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.designPointToolStripMenuItem3_Click(this, e);
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimaldesignToolStripMenuItem3_Click(this, e);
        }

        //NLopt Validation
        //private void button2_Click_1(object sender, EventArgs e)
        //{
        //    using (var solver = new NLoptSolver(NLoptAlgorithm.LN_SBPLX, 1, 0.001, 100))
        //    {
        //        solver.SetLowerBounds(new[] { -10.0 });
        //        solver.SetUpperBounds(new[] { 100.0 });

        //        solver.SetInitialStepSize(new[] { 0.01 });

        //        //solver.SetMinObjective(variables => Math.Pow(variables[0] - 3.0, 2.0) + 4.0);

        //        //Func<double[], double> funcion = variables => Math.Pow(variables[0] - 3.0, 2.0) + 4.0;

        //        Func<double[], double> funcion = delegate (double[] variables)
        //        {

        //            return Math.Pow(variables[0] - 3.0, 2.0) + 4.0;

        //        };

        //        solver.SetMinObjective(funcion);

        //        double? finalScore;
        //        var initialValue = new[] { 2.0 };
        //        var result = solver.Optimize(initialValue, out finalScore);

        //        textBox1.Text = result.ToString();
        //        textBox2.Text = initialValue[0].ToString();
        //        textBox3.Text = finalScore.GetValueOrDefault().ToString();

        //        //Console.ReadLine();
        //        //Assert.AreEqual(NloptResult.XTOL_REACHED, result);
        //        //Assert.AreEqual(3.0, initialValue[0], 0.1);
        //        //Assert.AreEqual(4.0, finalScore.GetValueOrDefault(), 0.1);
        //    }

        //    //ConstructorTest4();
        //}       

        //public void ConstructorTest4()
        //{
        //    //Weak version of Rosenbrock's problem.
        //    //var function = new NonlinearObjectiveFunction(2, x =>
        //    //    Math.Pow(x[0] * x[0] - x[1], 2.0) + Math.Pow(1.0 + x[0], 2.0));

        //    Func<double [], double> func = Sum;

        //    var function = new NonlinearObjectiveFunction(2, func);

        //    Subplex solver = new Subplex(function);

        //    solver.Minimize();
        //    double minimum = solver.Value;
        //    double[] solution = solver.Solution;
        //}       

        //public double Sum(double [] x)
        //{
        //    return Math.Pow(x[0] * x[0] - x[1], 2.0) + Math.Pow(1.0 + x[0], 2.0);
        //}

        //RC with ReHeating Design-Point
        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.DesignPoint_Click(this, e);
        }

        //RC with ReHeating Optimization
        private void LinkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimizationdesignToolStripMenuItem_Click(this, e);
        }

        //PCRC with ReHeating Design-Point
        private void LinkLabel4_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.designPointToolStripMenuItem_Click(this, e);
        }

        //PCRC with ReHeating Optimization
        private void LinkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimaldesignToolStripMenuItem_Click(this, e);
        }
        
        //RCMCI with ReHeating Design-Point
        private void LinkLabel10_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.designPointToolStripMenuItem1_Click(this, e);
        }

        //RCMCI with ReHeating Optimization
        private void LinkLabel9_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            puntero.optimaldesignToolStripMenuItem1_Click(this, e);
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}
