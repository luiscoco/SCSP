﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using sc.net;

using NLoptNet;

using Excel = Microsoft.Office.Interop.Excel;

using System.Reflection;

namespace RefPropWindowsForms
{
    public partial class PCRC_with_Two_Intercooling_withReheating_Analysis_Results : Form
    {
        PCRC_with_Two_Intercooling_with_ReHeating puntero_aplicacion;

        public PCRC_with_Two_Intercooling_withReheating_Analysis_Results(PCRC_with_Two_Intercooling_with_ReHeating puntero1)
        {
            puntero_aplicacion = puntero1;

            InitializeComponent();
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        //Run Optimization
        private void button3_Click(object sender, EventArgs e)
        {
            int counter_Excel = 4;

            Excel.Application xlApp1;
            Excel.Workbook xlWorkBook1;
            Excel.Worksheet xlWorkSheet1;
            Excel.Worksheet xlWorkSheet2;

            object misValue1 = System.Reflection.Missing.Value;

            xlApp1 = new Excel.Application();
            xlApp1.DisplayAlerts = false;
            xlWorkBook1 = xlApp1.Workbooks.Add(misValue1);

            xlWorkSheet1 = (Excel.Worksheet)xlWorkBook1.Worksheets.Add();

            //xlWorkSheet1 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(xlWorkBook1.Worksheets.Count);           

            double initial_CIP_value = 0;

            //UA optimization false
            if (checkBox2.Checked == false)
            {
                //PureFluid
                if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(this.comboBox1.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
                }

                if (puntero_aplicacion.comboBox1.Text == "PredefinedMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                }

                if (comboBox1.Text == "PseudoPureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PseudoPureFluid;
                }

                if (puntero_aplicacion.comboBox3.Text == "DEF")
                {
                    puntero_aplicacion.referencestate = ReferenceState.DEF;
                }
                if (puntero_aplicacion.comboBox3.Text == "ASH")
                {
                    puntero_aplicacion.referencestate = ReferenceState.ASH;
                }
                if (puntero_aplicacion.comboBox3.Text == "IIR")
                {
                    puntero_aplicacion.referencestate = ReferenceState.IIR;
                }
                if (puntero_aplicacion.comboBox3.Text == "NBP")
                {
                    puntero_aplicacion.referencestate = ReferenceState.NBP;
                }

                puntero_aplicacion.luis.working_fluid.Category = puntero_aplicacion.category;
                puntero_aplicacion.luis.working_fluid.reference = puntero_aplicacion.referencestate;

                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.t_rht_in2 = Convert.ToDouble(puntero_aplicacion.textBox95.Text);
                puntero_aplicacion.p_rhx_in2 = Convert.ToDouble(puntero_aplicacion.textBox94.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_pc22 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.eta_pc12 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox88.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.p_pc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.t_pc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                puntero_aplicacion.p_pc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_pc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.t_pc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.p_pc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                //eta_thermal2 = Convert.ToDouble(textBox50.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc11 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_pc21 = Convert.ToDouble(puntero_aplicacion.textBox85.Text);
                //dp2_pc2 = Convert.ToDouble(textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox89.Text);
                //dp2_phx2 = Convert.ToDouble(textBox1.Text);
                //dp2_rhx1 = Convert.ToDouble(textBox1.Text);
                //dp2_rhx2 = Convert.ToDouble(textBox1.Text);
                //dp2_cooler1 = Convert.ToDouble(textBox1.Text);
                puntero_aplicacion.dp2_cooler2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCwithTwoIntercoolingWithReheating cicloPCRCwithTwoIntercoolingWithReheating = new core.PCRCwithTwoIntercoolingWithReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc2_in2_list = new List<Double>();
                List<Double> p_pc2_out2_list = new List<Double>();
                List<Double> p_pc1_in2_list = new List<Double>();
                List<Double> p_pc1_out2_list = new List<Double>();
                List<Double> p_rhx_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                //List<Double> p_pc1_out2_list = new List<Double>();
                //List<Double> eta_thermal2_list = new List<Double>();

                NLoptAlgorithm algorithm_type = NLoptAlgorithm.LN_BOBYQA;

                if (comboBox19.Text == "BOBYQA")
                    algorithm_type = NLoptAlgorithm.LN_BOBYQA;
                else if (comboBox19.Text == "COBYLA")
                    algorithm_type = NLoptAlgorithm.LN_COBYLA;
                else if (comboBox19.Text == "SUBPLEX")
                    algorithm_type = NLoptAlgorithm.LN_SBPLX;
                else if (comboBox19.Text == "NELDER-MEAD")
                    algorithm_type = NLoptAlgorithm.LN_NELDERMEAD;
                else if (comboBox19.Text == "NEWUOA")
                    algorithm_type = NLoptAlgorithm.LN_NEWUOA;
                else if (comboBox19.Text == "PRAXIS")
                    algorithm_type = NLoptAlgorithm.LN_PRAXIS;

                if (checkBox6.Checked == true)
                {
                    initial_CIP_value = Convert.ToDouble(textBox5.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.luis.working_fluid.CriticalPressure;
                }

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox34.Text;
                xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                xlWorkSheet1.Cells[2, 1] = "";
                xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.luis.working_fluid.CriticalPressure);
                xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.luis.working_fluid.CriticalTemperature - 273.15);

                xlWorkSheet1.Cells[3, 1] = "";
                xlWorkSheet1.Cells[3, 2] = "";
                xlWorkSheet1.Cells[4, 3] = "";

                xlWorkSheet1.Cells[4, 1] = "PC2_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "PC2_out(kPa)";
                xlWorkSheet1.Cells[4, 3] = "PC1_out(kPa)";
                xlWorkSheet1.Cells[4, 4] = "P_RHX(kPa)";
                xlWorkSheet1.Cells[4, 5] = "CIT(K)";
                xlWorkSheet1.Cells[4, 6] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 8] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 9] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 12] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value, initial_CIP_value, 11000 });
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc_out2 / 1.3, puntero_aplicacion.p_mc_out2 / 1.3, puntero_aplicacion.p_mc_out2 / 1.3, puntero_aplicacion.p_mc_out2 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 1000 });

                    var initialValue = new[] { 0.2, initial_CIP_value, initial_CIP_value + 2500.0, initial_CIP_value + 4500.0, initial_CIP_value + 7500.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withTwoIntercooling_with_Reheating(puntero_aplicacion.luis,
                        ref cicloPCRCwithTwoIntercoolingWithReheating, puntero_aplicacion.w_dot_net2,
                        puntero_aplicacion.t_t_in2, variables[4], puntero_aplicacion.t_rht_in2,
                        puntero_aplicacion.t_mc_in2, variables[3], puntero_aplicacion.p_mc_out2,
                        variables[2], puntero_aplicacion.t_pc1_in2, variables[3],
                        variables[1], puntero_aplicacion.t_pc2_in2, variables[2],
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_pc12, puntero_aplicacion.eta_pc22,
                        puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp2_pc11, -puntero_aplicacion.dp2_pc12,
                        -puntero_aplicacion.dp2_pc21, -puntero_aplicacion.dp2_pc22,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1,
                        -puntero_aplicacion.dp2_rhx2, -puntero_aplicacion.dp2_cooler1, -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRCwithTwoIntercoolingWithReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRCwithTwoIntercoolingWithReheating.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRCwithTwoIntercoolingWithReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc2_in2 = variables[1];
                        puntero_aplicacion.p_pc2_out2 = variables[2];
                        puntero_aplicacion.p_pc1_in2 = variables[2];
                        puntero_aplicacion.p_pc1_out2 = variables[3];
                        puntero_aplicacion.p_rhx_in2 = variables[4];

                        puntero_aplicacion.temp21 = cicloPCRCwithTwoIntercoolingWithReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRCwithTwoIntercoolingWithReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRCwithTwoIntercoolingWithReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRCwithTwoIntercoolingWithReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRCwithTwoIntercoolingWithReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRCwithTwoIntercoolingWithReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRCwithTwoIntercoolingWithReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRCwithTwoIntercoolingWithReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRCwithTwoIntercoolingWithReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRCwithTwoIntercoolingWithReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRCwithTwoIntercoolingWithReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRCwithTwoIntercoolingWithReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRCwithTwoIntercoolingWithReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRCwithTwoIntercoolingWithReheating.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRCwithTwoIntercoolingWithReheating.temp[14];
                        puntero_aplicacion.temp216 = cicloPCRCwithTwoIntercoolingWithReheating.temp[15];

                        puntero_aplicacion.pres21 = cicloPCRCwithTwoIntercoolingWithReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRCwithTwoIntercoolingWithReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRCwithTwoIntercoolingWithReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRCwithTwoIntercoolingWithReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRCwithTwoIntercoolingWithReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRCwithTwoIntercoolingWithReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRCwithTwoIntercoolingWithReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRCwithTwoIntercoolingWithReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRCwithTwoIntercoolingWithReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRCwithTwoIntercoolingWithReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRCwithTwoIntercoolingWithReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRCwithTwoIntercoolingWithReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRCwithTwoIntercoolingWithReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRCwithTwoIntercoolingWithReheating.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRCwithTwoIntercoolingWithReheating.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRCwithTwoIntercoolingWithReheating.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRCwithTwoIntercoolingWithReheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRCwithTwoIntercoolingWithReheating.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRCwithTwoIntercoolingWithReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRCwithTwoIntercoolingWithReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRCwithTwoIntercoolingWithReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRCwithTwoIntercoolingWithReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRCwithTwoIntercoolingWithReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRCwithTwoIntercoolingWithReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRCwithTwoIntercoolingWithReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRCwithTwoIntercoolingWithReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRCwithTwoIntercoolingWithReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRCwithTwoIntercoolingWithReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRCwithTwoIntercoolingWithReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRCwithTwoIntercoolingWithReheating.HT.eff;

                        puntero_aplicacion.PC11 = cicloPCRCwithTwoIntercoolingWithReheating.PC1.Q_dot;
                        puntero_aplicacion.PC21 = cicloPCRCwithTwoIntercoolingWithReheating.PC2.Q_dot;
                        puntero_aplicacion.PC31 = cicloPCRCwithTwoIntercoolingWithReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc2_in2_list.Add(puntero_aplicacion.p_pc2_in2);
                        p_pc2_out2_list.Add(puntero_aplicacion.p_pc2_out2);
                        p_pc1_in2_list.Add(puntero_aplicacion.p_pc1_in2);
                        p_pc1_out2_list.Add(puntero_aplicacion.p_pc1_out2);
                        p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc2_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc2_out2.ToString());
                        listBox24.Items.Add(puntero_aplicacion.p_pc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloPCRCwithTwoIntercoolingWithReheating.temp[7] - cicloPCRCwithTwoIntercoolingWithReheating.temp[2];
                        double LTR_min_DT_2 = cicloPCRCwithTwoIntercoolingWithReheating.temp[8] - cicloPCRCwithTwoIntercoolingWithReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRCwithTwoIntercoolingWithReheating.temp[7] - cicloPCRCwithTwoIntercoolingWithReheating.temp[3];
                        double HTR_min_DT_2 = cicloPCRCwithTwoIntercoolingWithReheating.temp[6] - cicloPCRCwithTwoIntercoolingWithReheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC2_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc2_in2);
                        //PC2_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc2_out2);
                        //PC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_pc1_out2);
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.p_rhx_in2.ToString();
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloPCRCwithTwoIntercoolingWithReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = cicloPCRCwithTwoIntercoolingWithReheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox91.Text = p_pc2_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc2_out2_list[maxIndex].ToString();
                    textBox6.Text = p_pc1_out2_list[maxIndex].ToString();
                    textBox1.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc2_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox86.Text = p_pc2_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox87.Text = p_pc1_out2_list[maxIndex].ToString();                     
                        puntero_aplicacion.textBox3.Text = p_pc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox94.Text = p_rhx_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book 
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_Two_Intercooling_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }

            }//UA optimization false


            //UA optimization true
            else if (checkBox2.Checked == true) 
            {
                //PureFluid
                if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(this.comboBox1.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
                }

                if (puntero_aplicacion.comboBox1.Text == "PredefinedMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                }

                if (comboBox1.Text == "PseudoPureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PseudoPureFluid;
                }

                if (puntero_aplicacion.comboBox3.Text == "DEF")
                {
                    puntero_aplicacion.referencestate = ReferenceState.DEF;
                }
                if (puntero_aplicacion.comboBox3.Text == "ASH")
                {
                    puntero_aplicacion.referencestate = ReferenceState.ASH;
                }
                if (puntero_aplicacion.comboBox3.Text == "IIR")
                {
                    puntero_aplicacion.referencestate = ReferenceState.IIR;
                }
                if (puntero_aplicacion.comboBox3.Text == "NBP")
                {
                    puntero_aplicacion.referencestate = ReferenceState.NBP;
                }

                puntero_aplicacion.luis.working_fluid.Category = puntero_aplicacion.category;
                puntero_aplicacion.luis.working_fluid.reference = puntero_aplicacion.referencestate;

                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.t_rht_in2 = Convert.ToDouble(puntero_aplicacion.textBox95.Text);
                puntero_aplicacion.p_rhx_in2 = Convert.ToDouble(puntero_aplicacion.textBox94.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_pc22 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.eta_pc12 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox88.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.p_pc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.t_pc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                puntero_aplicacion.p_pc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_pc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.t_pc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.p_pc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                //eta_thermal2 = Convert.ToDouble(textBox50.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc11 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_pc21 = Convert.ToDouble(puntero_aplicacion.textBox85.Text);
                //dp2_pc2 = Convert.ToDouble(textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox89.Text);
                //dp2_phx2 = Convert.ToDouble(textBox1.Text);
                //dp2_rhx1 = Convert.ToDouble(textBox1.Text);
                //dp2_rhx2 = Convert.ToDouble(textBox1.Text);
                //dp2_cooler1 = Convert.ToDouble(textBox1.Text);
                puntero_aplicacion.dp2_cooler2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCwithTwoIntercoolingWithReheating cicloPCRCwithTwoIntercoolingWithReheating = new core.PCRCwithTwoIntercoolingWithReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc2_in2_list = new List<Double>();
                List<Double> p_pc2_out2_list = new List<Double>();
                List<Double> p_pc1_in2_list = new List<Double>();
                List<Double> p_pc1_out2_list = new List<Double>();
                List<Double> p_rhx_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> ua_LT_list = new List<Double>();
                List<Double> ua_HT_list = new List<Double>();

                NLoptAlgorithm algorithm_type = NLoptAlgorithm.LN_BOBYQA;

                if (comboBox19.Text == "BOBYQA")
                    algorithm_type = NLoptAlgorithm.LN_BOBYQA;
                else if (comboBox19.Text == "COBYLA")
                    algorithm_type = NLoptAlgorithm.LN_COBYLA;
                else if (comboBox19.Text == "SUBPLEX")
                    algorithm_type = NLoptAlgorithm.LN_SBPLX;
                else if (comboBox19.Text == "NELDER-MEAD")
                    algorithm_type = NLoptAlgorithm.LN_NELDERMEAD;
                else if (comboBox19.Text == "NEWUOA")
                    algorithm_type = NLoptAlgorithm.LN_NEWUOA;
                else if (comboBox19.Text == "PRAXIS")
                    algorithm_type = NLoptAlgorithm.LN_PRAXIS;

                if (checkBox6.Checked == true)
                {
                    initial_CIP_value = Convert.ToDouble(textBox5.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.luis.working_fluid.CriticalPressure;
                }

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox34.Text;
                xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                xlWorkSheet1.Cells[2, 1] = "";
                xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.luis.working_fluid.CriticalPressure);
                xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.luis.working_fluid.CriticalTemperature - 273.15);

                xlWorkSheet1.Cells[3, 1] = "";
                xlWorkSheet1.Cells[3, 2] = "";
                xlWorkSheet1.Cells[4, 3] = "";

                xlWorkSheet1.Cells[4, 1] = "PC2_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "PC2_out(kPa)";
                xlWorkSheet1.Cells[4, 3] = "PC1_out(kPa)";
                xlWorkSheet1.Cells[4, 4] = "P_RHX(kPa)";
                xlWorkSheet1.Cells[4, 5] = "CIT(K)";
                xlWorkSheet1.Cells[4, 6] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 8] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 9] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 12] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 6, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value, initial_CIP_value, 11000, 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc_out2 / 1.3, puntero_aplicacion.p_mc_out2 / 1.3, puntero_aplicacion.p_mc_out2 / 1.3, puntero_aplicacion.p_mc_out2, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 1000, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value, initial_CIP_value + 2500.0, initial_CIP_value + 4500.0, initial_CIP_value + 7500.0, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withTwoIntercooling_with_Reheating_for_optimization(puntero_aplicacion.luis,
                        ref cicloPCRCwithTwoIntercoolingWithReheating, puntero_aplicacion.w_dot_net2,
                        puntero_aplicacion.t_t_in2, variables[4], puntero_aplicacion.t_rht_in2,
                        puntero_aplicacion.t_mc_in2, variables[3], puntero_aplicacion.p_mc_out2,
                        variables[2], puntero_aplicacion.t_pc1_in2, variables[3],
                        variables[1], puntero_aplicacion.t_pc2_in2, variables[2],
                        variables[5], UA_Total, puntero_aplicacion.eta_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_pc12, puntero_aplicacion.eta_pc22,
                        puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp2_pc11, -puntero_aplicacion.dp2_pc12,
                        -puntero_aplicacion.dp2_pc21, -puntero_aplicacion.dp2_pc22,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1,
                        -puntero_aplicacion.dp2_rhx2, -puntero_aplicacion.dp2_cooler1, -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRCwithTwoIntercoolingWithReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRCwithTwoIntercoolingWithReheating.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRCwithTwoIntercoolingWithReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc2_in2 = variables[1];
                        puntero_aplicacion.p_pc2_out2 = variables[2];
                        puntero_aplicacion.p_pc1_in2 = variables[2];
                        puntero_aplicacion.p_pc1_out2 = variables[3];
                        puntero_aplicacion.p_rhx_in2 = variables[4];
                        LT_fraction = variables[5];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloPCRCwithTwoIntercoolingWithReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRCwithTwoIntercoolingWithReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRCwithTwoIntercoolingWithReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRCwithTwoIntercoolingWithReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRCwithTwoIntercoolingWithReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRCwithTwoIntercoolingWithReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRCwithTwoIntercoolingWithReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRCwithTwoIntercoolingWithReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRCwithTwoIntercoolingWithReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRCwithTwoIntercoolingWithReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRCwithTwoIntercoolingWithReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRCwithTwoIntercoolingWithReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRCwithTwoIntercoolingWithReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRCwithTwoIntercoolingWithReheating.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRCwithTwoIntercoolingWithReheating.temp[14];
                        puntero_aplicacion.temp216 = cicloPCRCwithTwoIntercoolingWithReheating.temp[15];

                        puntero_aplicacion.pres21 = cicloPCRCwithTwoIntercoolingWithReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRCwithTwoIntercoolingWithReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRCwithTwoIntercoolingWithReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRCwithTwoIntercoolingWithReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRCwithTwoIntercoolingWithReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRCwithTwoIntercoolingWithReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRCwithTwoIntercoolingWithReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRCwithTwoIntercoolingWithReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRCwithTwoIntercoolingWithReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRCwithTwoIntercoolingWithReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRCwithTwoIntercoolingWithReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRCwithTwoIntercoolingWithReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRCwithTwoIntercoolingWithReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRCwithTwoIntercoolingWithReheating.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRCwithTwoIntercoolingWithReheating.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRCwithTwoIntercoolingWithReheating.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRCwithTwoIntercoolingWithReheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRCwithTwoIntercoolingWithReheating.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRCwithTwoIntercoolingWithReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRCwithTwoIntercoolingWithReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRCwithTwoIntercoolingWithReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRCwithTwoIntercoolingWithReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRCwithTwoIntercoolingWithReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRCwithTwoIntercoolingWithReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRCwithTwoIntercoolingWithReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRCwithTwoIntercoolingWithReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRCwithTwoIntercoolingWithReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRCwithTwoIntercoolingWithReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRCwithTwoIntercoolingWithReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRCwithTwoIntercoolingWithReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRCwithTwoIntercoolingWithReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRCwithTwoIntercoolingWithReheating.HT.eff;

                        puntero_aplicacion.PC11 = cicloPCRCwithTwoIntercoolingWithReheating.PC1.Q_dot;
                        puntero_aplicacion.PC21 = cicloPCRCwithTwoIntercoolingWithReheating.PC2.Q_dot;
                        puntero_aplicacion.PC31 = cicloPCRCwithTwoIntercoolingWithReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc2_in2_list.Add(puntero_aplicacion.p_pc2_in2);
                        p_pc2_out2_list.Add(puntero_aplicacion.p_pc2_out2);
                        p_pc1_in2_list.Add(puntero_aplicacion.p_pc1_in2);
                        p_pc1_out2_list.Add(puntero_aplicacion.p_pc1_out2);
                        p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc2_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc2_out2.ToString());
                        listBox24.Items.Add(puntero_aplicacion.p_pc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloPCRCwithTwoIntercoolingWithReheating.temp[7] - cicloPCRCwithTwoIntercoolingWithReheating.temp[2];
                        double LTR_min_DT_2 = cicloPCRCwithTwoIntercoolingWithReheating.temp[8] - cicloPCRCwithTwoIntercoolingWithReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRCwithTwoIntercoolingWithReheating.temp[7] - cicloPCRCwithTwoIntercoolingWithReheating.temp[3];
                        double HTR_min_DT_2 = cicloPCRCwithTwoIntercoolingWithReheating.temp[6] - cicloPCRCwithTwoIntercoolingWithReheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC2_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc2_in2);
                        //PC2_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc2_out2);
                        //PC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_pc1_out2);
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.p_rhx_in2.ToString();
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloPCRCwithTwoIntercoolingWithReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = cicloPCRCwithTwoIntercoolingWithReheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox91.Text = p_pc2_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc2_out2_list[maxIndex].ToString();
                    textBox6.Text = p_pc1_out2_list[maxIndex].ToString();
                    textBox1.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc2_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox86.Text = p_pc2_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox87.Text = p_pc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox94.Text = p_rhx_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book 
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_Two_Intercooling_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }
        }

        //CIT Optimization Analysis
        private void button2_Click(object sender, EventArgs e)
        {










        }
    }
}
