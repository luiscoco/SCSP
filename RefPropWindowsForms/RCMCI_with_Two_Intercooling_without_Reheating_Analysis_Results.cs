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
    public partial class RCMCI_with_Two_Intercooling_without_Reheating_Analysis_Results : Form
    {
        RCMCI_with_Two_Intercooling_without_Reheating puntero_aplicacion;

        public RCMCI_with_Two_Intercooling_without_Reheating_Analysis_Results(RCMCI_with_Two_Intercooling_without_Reheating puntero1)
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

            //Optimize UA false
            if (checkBox2.Checked == false)
            {
                //PureFluid
                if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
                }

                if (puntero_aplicacion.comboBox1.Text == "PredefinedMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                }

                if (puntero_aplicacion.comboBox1.Text == "PseudoPureFluid")
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

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.t_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox85.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                puntero_aplicacion.p_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.p_mc3_out2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta3_mc2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp1_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp3_pc2 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RCMCIwithTwoIntercoolingwithoutReheating cicloRCMCIwithTwoIntercoolingwithoutReheating = new core.RCMCIwithTwoIntercoolingwithoutReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_mc2_out2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();

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
                    initial_CIP_value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                }

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text
                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox34.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox69.Text;
                xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                xlWorkSheet1.Cells[2, 1] = "";
                xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                xlWorkSheet1.Cells[3, 1] = "";
                xlWorkSheet1.Cells[3, 2] = "";
                xlWorkSheet1.Cells[4, 3] = "";

                xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
                xlWorkSheet1.Cells[4, 3] = "MC2_out(kPa)";
                xlWorkSheet1.Cells[4, 4] = "CIT(K)";
                xlWorkSheet1.Cells[4, 5] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value, initial_CIP_value });
                    solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3) });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value + 3000.0, initial_CIP_value + 4500.0, initial_CIP_value + 5500.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_with_Two_Intercooling_without_Reheating(puntero_aplicacion.luis, ref cicloRCMCIwithTwoIntercoolingwithoutReheating,
                        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2, puntero_aplicacion.t_t_in2, variables[2], variables[3], variables[1], puntero_aplicacion.t_mc1_in2, variables[2], variables[3], puntero_aplicacion.t_mc3_in2, puntero_aplicacion.p_mc3_out2,
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta3_mc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1, 
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp1_pc1, -puntero_aplicacion.dp1_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp3_pc2, -puntero_aplicacion.dp3_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_mc2_in2 = variables[2];
                        puntero_aplicacion.p_mc2_out2 = variables[3];
                        puntero_aplicacion.p_mc3_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[13];

                        puntero_aplicacion.PHX = cicloRCMCIwithTwoIntercoolingwithoutReheating.PHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff;

                        puntero_aplicacion.PC1 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc2_out2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //MC1_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                        //MC2_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc2_out2);
                        //CIT (MC1)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox4.Text = p_mc2_out2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox22.Text = p_mc2_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox84.Text = p_mc2_out2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_Two_Intercooling_without_ReHeating" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }
            //-------------------------------------------------------------------------
           
            //Optimize UA false
            else if (checkBox2.Checked == true)
            {
                //PureFluid
                if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
                }

                if (puntero_aplicacion.comboBox1.Text == "PredefinedMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                }

                if (puntero_aplicacion.comboBox1.Text == "PseudoPureFluid")
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

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.t_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox85.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                puntero_aplicacion.p_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.p_mc3_out2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta3_mc2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp1_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp3_pc2 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RCMCIwithTwoIntercoolingwithoutReheating cicloRCMCIwithTwoIntercoolingwithoutReheating = new core.RCMCIwithTwoIntercoolingwithoutReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_mc2_out2_list = new List<Double>();
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
                    initial_CIP_value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                }

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text
                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox34.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox69.Text;
                xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                xlWorkSheet1.Cells[2, 1] = "";
                xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                xlWorkSheet1.Cells[3, 1] = "";
                xlWorkSheet1.Cells[3, 2] = "";
                xlWorkSheet1.Cells[4, 3] = "";

                xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
                xlWorkSheet1.Cells[4, 3] = "MC2_out(kPa)";
                xlWorkSheet1.Cells[4, 4] = "CIT(K)";
                xlWorkSheet1.Cells[4, 5] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value , initial_CIP_value , initial_CIP_value , 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3), 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value + 3000.0, initial_CIP_value + 4500.0, initial_CIP_value + 5500.0, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_with_Two_Intercooling_without_Reheating_for_Optimization(puntero_aplicacion.luis, ref cicloRCMCIwithTwoIntercoolingwithoutReheating,
                        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2, puntero_aplicacion.t_t_in2, variables[2], variables[3], variables[1], puntero_aplicacion.t_mc1_in2, variables[2], variables[3], puntero_aplicacion.t_mc3_in2, puntero_aplicacion.p_mc3_out2,
                        variables[4], UA_Total, puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta3_mc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp1_pc1, -puntero_aplicacion.dp1_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp3_pc2, -puntero_aplicacion.dp3_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_mc2_in2 = variables[2];
                        puntero_aplicacion.p_mc2_out2 = variables[3];
                        puntero_aplicacion.p_mc3_in2 = variables[3];
                        LT_fraction = variables[4];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[13];

                        puntero_aplicacion.PHX = cicloRCMCIwithTwoIntercoolingwithoutReheating.PHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff;

                        puntero_aplicacion.PC1 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out2);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc2_out2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //MC1_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                        //MC2_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc2_out2);
                        //CIT (MC1)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox4.Text = p_mc2_out2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox22.Text = p_mc2_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox84.Text = p_mc2_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_Two_Intercooling_without_ReHeating" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            } //UA optimization true        
        }

        //CIT Optimization
        private void button7_Click(object sender, EventArgs e)
        {
            int counter = 0;

            double initial_mc1_in_value = 0;
            double initial_mc1_out_value = 0;
            double initial_mc2_out_value = 0;

            int counter_Excel = 4;

            Excel.Application xlApp1;
            Excel.Workbook xlWorkBook1;
            Excel.Worksheet xlWorkSheet1;
            //Excel.Worksheet xlWorkSheet2;

            object misValue1 = System.Reflection.Missing.Value;

            xlApp1 = new Excel.Application();
            xlApp1.DisplayAlerts = false;
            xlWorkBook1 = xlApp1.Workbooks.Add(misValue1);

            xlWorkSheet1 = (Excel.Worksheet)xlWorkBook1.Worksheets.Add();

            double initial_CIP_value = 0;

            //xlWorkSheet1 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(xlWorkBook1.Worksheets.Count);    

            //Loop for UA optimization
            for (double j = Convert.ToDouble(textBox11.Text); j <= Convert.ToDouble(textBox10.Text); j = j + Convert.ToDouble(textBox9.Text))
            {
                puntero_aplicacion.ua_lt2 = j / 2;
                puntero_aplicacion.ua_ht2 = j / 2;

                //Loop for CIT optimization
                for (double i = Convert.ToDouble(textBox57.Text); i <= Convert.ToDouble(textBox56.Text); i = i + Convert.ToDouble(textBox55.Text))
                {
                    counter = 0;

                    //Optimize UA false
                    if (checkBox2.Checked == false)
                    {
                        //PureFluid
                        if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                        }

                        //NewMixture
                        if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
                        }

                        if (puntero_aplicacion.comboBox1.Text == "PredefinedMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                        }

                        if (puntero_aplicacion.comboBox1.Text == "PseudoPureFluid")
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

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                        puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.t_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox85.Text);
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                        puntero_aplicacion.p_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                        puntero_aplicacion.p_mc3_out2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                        puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.eta3_mc2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp1_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp3_pc2 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RCMCIwithTwoIntercoolingwithoutReheating cicloRCMCIwithTwoIntercoolingwithoutReheating = new core.RCMCIwithTwoIntercoolingwithoutReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc1_in2_list = new List<Double>();
                        List<Double> p_mc1_out2_list = new List<Double>();
                        List<Double> p_mc2_out2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
                        List<Double> ua_HT_list = new List<Double>();
                        List<Double> ua_LT_list = new List<Double>();

                        List<Double> t1_list = new List<Double>();
                        List<Double> t2_list = new List<Double>();
                        List<Double> t3_list = new List<Double>();
                        List<Double> t4_list = new List<Double>();
                        List<Double> t5_list = new List<Double>();
                        List<Double> t6_list = new List<Double>();
                        List<Double> t7_list = new List<Double>();
                        List<Double> t8_list = new List<Double>();
                        List<Double> t9_list = new List<Double>();
                        List<Double> t10_list = new List<Double>();
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();

                        List<Double> p1_list = new List<Double>();
                        List<Double> p2_list = new List<Double>();
                        List<Double> p3_list = new List<Double>();
                        List<Double> p4_list = new List<Double>();
                        List<Double> p5_list = new List<Double>();
                        List<Double> p6_list = new List<Double>();
                        List<Double> p7_list = new List<Double>();
                        List<Double> p8_list = new List<Double>();
                        List<Double> p9_list = new List<Double>();
                        List<Double> p10_list = new List<Double>();
                        List<Double> p13_list = new List<Double>();
                        List<Double> p14_list = new List<Double>();

                        List<Double> HT_Eff_list = new List<Double>();
                        List<Double> LT_Eff_list = new List<Double>();

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

                        if (i == Convert.ToDouble(textBox57.Text))
                        {
                            if (checkBox6.Checked == true)
                            {
                                initial_CIP_value = Convert.ToDouble(textBox1.Text);
                            }
                            else
                            {
                                initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                            }

                            xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                            //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text
                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox34.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox69.Text;
                            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                            xlWorkSheet1.Cells[2, 1] = "";
                            xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                            xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                            xlWorkSheet1.Cells[3, 1] = "";
                            xlWorkSheet1.Cells[3, 2] = "";
                            xlWorkSheet1.Cells[4, 3] = "";

                            xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                            xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
                            xlWorkSheet1.Cells[4, 3] = "MC2_out(kPa)";
                            xlWorkSheet1.Cells[4, 4] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 5] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 7] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 13] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 14] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 15] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 16] = "LF_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 4, 0.000001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value, initial_CIP_value });
                            solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3) });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                            var initialValue = new[] { 0.2, initial_CIP_value, initial_CIP_value + 1500.0, initial_CIP_value + 3500.0 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_with_Two_Intercooling_without_Reheating(puntero_aplicacion.luis, 
                                ref cicloRCMCIwithTwoIntercoolingwithoutReheating, puntero_aplicacion.w_dot_net2,
                                i, puntero_aplicacion.t_t_in2, variables[2], variables[3], variables[1], 
                                i, variables[2], variables[3], i, 
                                puntero_aplicacion.p_mc3_out2, puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, 
                                puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, 
                                puntero_aplicacion.eta3_mc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, 
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, 
                                -puntero_aplicacion.dp1_pc1, -puntero_aplicacion.dp1_pc2, -puntero_aplicacion.dp2_phx1, 
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_pc2, 
                                -puntero_aplicacion.dp3_pc2, -puntero_aplicacion.dp3_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                puntero_aplicacion.p_mc2_in2 = variables[2];
                                puntero_aplicacion.p_mc2_out2 = variables[3];
                                puntero_aplicacion.p_mc3_in2 = variables[3];

                                puntero_aplicacion.temp21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[13];

                                puntero_aplicacion.PHX = cicloRCMCIwithTwoIntercoolingwithoutReheating.PHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff;

                                puntero_aplicacion.PC1 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC1.Q_dot;
                                puntero_aplicacion.PC2 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC2.Q_dot;
                                puntero_aplicacion.PC3 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                                p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out2);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                                HT_Eff_list.Add(cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff);
                                LT_Eff_list.Add(cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff);

                                t1_list.Add(puntero_aplicacion.temp21);
                                t2_list.Add(puntero_aplicacion.temp22);
                                t3_list.Add(puntero_aplicacion.temp23);
                                t4_list.Add(puntero_aplicacion.temp24);
                                t5_list.Add(puntero_aplicacion.temp25);
                                t6_list.Add(puntero_aplicacion.temp26);
                                t7_list.Add(puntero_aplicacion.temp27);
                                t8_list.Add(puntero_aplicacion.temp28);
                                t9_list.Add(puntero_aplicacion.temp29);
                                t10_list.Add(puntero_aplicacion.temp210);
                                t13_list.Add(puntero_aplicacion.temp213);
                                t14_list.Add(puntero_aplicacion.temp214);

                                p1_list.Add(puntero_aplicacion.pres21);
                                p2_list.Add(puntero_aplicacion.pres22);
                                p3_list.Add(puntero_aplicacion.pres23);
                                p4_list.Add(puntero_aplicacion.pres24);
                                p5_list.Add(puntero_aplicacion.pres25);
                                p6_list.Add(puntero_aplicacion.pres26);
                                p7_list.Add(puntero_aplicacion.pres27);
                                p8_list.Add(puntero_aplicacion.pres28);
                                p9_list.Add(puntero_aplicacion.pres29);
                                p10_list.Add(puntero_aplicacion.pres210);
                                p13_list.Add(puntero_aplicacion.pres213);
                                p14_list.Add(puntero_aplicacion.pres214);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox20.Items.Add(puntero_aplicacion.p_mc2_out2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp28.ToString());

                                //double LTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                                //double LTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                                //double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                //double HTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                                //double HTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                                //double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////MC1_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                                ////MC1_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                                ////MC2_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc2_out2);
                                ////CIT (MC1)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list.Max();

                            var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                            textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                            textBox4.Text = p_mc2_out2_list[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox22.Text = p_mc2_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox84.Text = p_mc2_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox2.Text = i.ToString();
                                puntero_aplicacion.textBox28.Text = i.ToString();
                                puntero_aplicacion.textBox85.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_mc1_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_mc1_out2_list[maxIndex].ToString());
                            listBox19.Items.Add(p_mc2_out2_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());

                            //MAIN SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "PureFluid";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;

                            if (comboBox4.Text == "Parabolic")
                            {
                                PTC.textBox7.Text = "0.141";
                                PTC.textBox8.Text = "6.48e-9";
                            }
                            else if (comboBox4.Text == "Parabolic with cavity receiver (Norwich)")
                            {
                                PTC.textBox7.Text = "0.3";
                                PTC.textBox8.Text = "3.25e-9";
                            }

                            PTC.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX);
                            PTC.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC.textBox3.Text = Convert.ToString(puntero_aplicacion.temp25);
                            PTC.textBox6.Text = Convert.ToString(puntero_aplicacion.temp26);
                            PTC.textBox4.Text = Convert.ToString(puntero_aplicacion.pres25);
                            PTC.textBox5.Text = Convert.ToString(puntero_aplicacion.pres26);
                            PTC.textBox107.Text = Convert.ToString(10);
                            PTC.button1_Click(this, e);
                            puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area = PTC.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Main_SF_Pressure_drop = PTC.Total_Pressure_DropResult;

                            //MAIN SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF = new LF_SF_Calculation();
                            LF.calledForSensingAnalysis = true;
                            LF.comboBox1.Text = "Solar Salt";
                            LF.comboBox2.Text = "PureFluid";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX);
                            LF.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF.textBox3.Text = Convert.ToString(puntero_aplicacion.temp25);
                            LF.textBox6.Text = Convert.ToString(puntero_aplicacion.temp26);
                            LF.textBox4.Text = Convert.ToString(puntero_aplicacion.pres25);
                            LF.textBox5.Text = Convert.ToString(puntero_aplicacion.pres26);
                            LF.textBox107.Text = Convert.ToString(10);
                            LF.button1_Click(this, e);
                            puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area = LF.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Main_SF_Pressure_drop = LF.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //MC1_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = p_mc1_in2_list[maxIndex].ToString();
                            //MC1_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = p_mc1_out2_list[maxIndex].ToString();
                            //MC2_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = p_mc2_out2_list[maxIndex].ToString();
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = recomp_frac2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_mc1_in_value = puntero_aplicacion.p_mc1_in2;
                            initial_mc1_out_value = puntero_aplicacion.p_mc1_out2;
                            initial_mc2_out_value = puntero_aplicacion.p_mc2_out2;
                        }
                    }
                    //-------------------------------------------------------------------------

                    //Optimize UA false
                    else if (checkBox2.Checked == true)
                    {
                        //PureFluid
                        if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                        }

                        //NewMixture
                        if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
                        }

                        if (puntero_aplicacion.comboBox1.Text == "PredefinedMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                        }

                        if (puntero_aplicacion.comboBox1.Text == "PseudoPureFluid")
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

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                        puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.t_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox85.Text);
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                        puntero_aplicacion.p_mc3_in2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                        puntero_aplicacion.p_mc3_out2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                        puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.eta3_mc2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp1_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp3_pc2 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RCMCIwithTwoIntercoolingwithoutReheating cicloRCMCIwithTwoIntercoolingwithoutReheating = new core.RCMCIwithTwoIntercoolingwithoutReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc1_in2_list = new List<Double>();
                        List<Double> p_mc1_out2_list = new List<Double>();
                        List<Double> p_mc2_out2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
                        List<Double> ua_LT_list = new List<Double>();
                        List<Double> ua_HT_list = new List<Double>();

                        List<Double> t1_list = new List<Double>();
                        List<Double> t2_list = new List<Double>();
                        List<Double> t3_list = new List<Double>();
                        List<Double> t4_list = new List<Double>();
                        List<Double> t5_list = new List<Double>();
                        List<Double> t6_list = new List<Double>();
                        List<Double> t7_list = new List<Double>();
                        List<Double> t8_list = new List<Double>();
                        List<Double> t9_list = new List<Double>();
                        List<Double> t10_list = new List<Double>();
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();

                        List<Double> p1_list = new List<Double>();
                        List<Double> p2_list = new List<Double>();
                        List<Double> p3_list = new List<Double>();
                        List<Double> p4_list = new List<Double>();
                        List<Double> p5_list = new List<Double>();
                        List<Double> p6_list = new List<Double>();
                        List<Double> p7_list = new List<Double>();
                        List<Double> p8_list = new List<Double>();
                        List<Double> p9_list = new List<Double>();
                        List<Double> p10_list = new List<Double>();
                        List<Double> p13_list = new List<Double>();
                        List<Double> p14_list = new List<Double>();

                        List<Double> HT_Eff_list = new List<Double>();
                        List<Double> LT_Eff_list = new List<Double>();

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

                        if (i == Convert.ToDouble(textBox57.Text))
                        {

                            if (checkBox6.Checked == true)
                            {
                                initial_CIP_value = Convert.ToDouble(textBox1.Text);
                            }
                            else
                            {
                                initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                            }

                            xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                            //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text
                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox34.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox69.Text;
                            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                            xlWorkSheet1.Cells[2, 1] = "";
                            xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                            xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                            xlWorkSheet1.Cells[3, 1] = "";
                            xlWorkSheet1.Cells[3, 2] = "";
                            xlWorkSheet1.Cells[4, 3] = "";

                            xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                            xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
                            xlWorkSheet1.Cells[4, 3] = "MC2_out(kPa)";
                            xlWorkSheet1.Cells[4, 4] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 5] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 7] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 13] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 14] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 15] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 16] = "LF_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 5, 0.000001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value, initial_CIP_value, 0.0 });
                            solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3), (puntero_aplicacion.p_mc3_out2 / 1.3), 1.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.2, initial_CIP_value, initial_CIP_value + 1500.0, initial_CIP_value + 3500.0, 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_with_Two_Intercooling_without_Reheating_for_Optimization(puntero_aplicacion.luis, 
                                ref cicloRCMCIwithTwoIntercoolingwithoutReheating, puntero_aplicacion.w_dot_net2, i, 
                                puntero_aplicacion.t_t_in2, variables[2], variables[3], variables[1], i, variables[2], 
                                variables[3], i, puntero_aplicacion.p_mc3_out2,
                                variables[4], UA_Total, puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, 
                                puntero_aplicacion.eta3_mc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, variables[0], 
                                puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, 
                                -puntero_aplicacion.dp1_pc1, -puntero_aplicacion.dp1_pc2, -puntero_aplicacion.dp2_phx1, 
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_pc2, 
                                -puntero_aplicacion.dp3_pc2, -puntero_aplicacion.dp3_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                puntero_aplicacion.p_mc2_in2 = variables[2];
                                puntero_aplicacion.p_mc2_out2 = variables[3];
                                puntero_aplicacion.p_mc3_in2 = variables[3];
                                LT_fraction = variables[4];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCIwithTwoIntercoolingwithoutReheating.pres[13];

                                puntero_aplicacion.PHX = cicloRCMCIwithTwoIntercoolingwithoutReheating.PHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff;

                                puntero_aplicacion.PC1 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC1.Q_dot;
                                puntero_aplicacion.PC2 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.PC2.Q_dot;
                                puntero_aplicacion.PC3 = -cicloRCMCIwithTwoIntercoolingwithoutReheating.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                                p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out2);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                                HT_Eff_list.Add(cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff);
                                LT_Eff_list.Add(cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff);

                                t1_list.Add(puntero_aplicacion.temp21);
                                t2_list.Add(puntero_aplicacion.temp22);
                                t3_list.Add(puntero_aplicacion.temp23);
                                t4_list.Add(puntero_aplicacion.temp24);
                                t5_list.Add(puntero_aplicacion.temp25);
                                t6_list.Add(puntero_aplicacion.temp26);
                                t7_list.Add(puntero_aplicacion.temp27);
                                t8_list.Add(puntero_aplicacion.temp28);
                                t9_list.Add(puntero_aplicacion.temp29);
                                t10_list.Add(puntero_aplicacion.temp210);
                                t13_list.Add(puntero_aplicacion.temp213);
                                t14_list.Add(puntero_aplicacion.temp214);

                                p1_list.Add(puntero_aplicacion.pres21);
                                p2_list.Add(puntero_aplicacion.pres22);
                                p3_list.Add(puntero_aplicacion.pres23);
                                p4_list.Add(puntero_aplicacion.pres24);
                                p5_list.Add(puntero_aplicacion.pres25);
                                p6_list.Add(puntero_aplicacion.pres26);
                                p7_list.Add(puntero_aplicacion.pres27);
                                p8_list.Add(puntero_aplicacion.pres28);
                                p9_list.Add(puntero_aplicacion.pres29);
                                p10_list.Add(puntero_aplicacion.pres210);
                                p13_list.Add(puntero_aplicacion.pres213);
                                p14_list.Add(puntero_aplicacion.pres214);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox20.Items.Add(puntero_aplicacion.p_mc2_out2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp28.ToString());

                                //double LTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[2];
                                //double LTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[8] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[1];
                                //double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                //double HTR_min_DT_1 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[7] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[3];
                                //double HTR_min_DT_2 = cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[6] - cicloRCMCIwithTwoIntercoolingwithoutReheating.temp[4];
                                //double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////MC1_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                                ////MC1_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                                ////MC2_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc2_out2);
                                ////CIT (MC1)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list.Max();

                            var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                            textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                            textBox4.Text = p_mc2_out2_list[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox22.Text = p_mc2_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox84.Text = p_mc2_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox2.Text = i.ToString();
                                puntero_aplicacion.textBox28.Text = i.ToString();
                                puntero_aplicacion.textBox85.Text = i.ToString();                             
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_mc1_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_mc1_out2_list[maxIndex].ToString());
                            listBox19.Items.Add(p_mc2_out2_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());

                            //MAIN SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "PureFluid";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;

                            if (comboBox4.Text == "Parabolic")
                            {
                                PTC.textBox7.Text = "0.141";
                                PTC.textBox8.Text = "6.48e-9";
                            }
                            else if (comboBox4.Text == "Parabolic with cavity receiver (Norwich)")
                            {
                                PTC.textBox7.Text = "0.3";
                                PTC.textBox8.Text = "3.25e-9";
                            }

                            PTC.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX);
                            PTC.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC.textBox3.Text = Convert.ToString(puntero_aplicacion.temp25);
                            PTC.textBox6.Text = Convert.ToString(puntero_aplicacion.temp26);
                            PTC.textBox4.Text = Convert.ToString(puntero_aplicacion.pres25);
                            PTC.textBox5.Text = Convert.ToString(puntero_aplicacion.pres26);
                            PTC.textBox107.Text = Convert.ToString(10);
                            PTC.button1_Click(this, e);
                            puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area = PTC.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Main_SF_Pressure_drop = PTC.Total_Pressure_DropResult;

                            //MAIN SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF = new LF_SF_Calculation();
                            LF.calledForSensingAnalysis = true;
                            LF.comboBox1.Text = "Solar Salt";
                            LF.comboBox2.Text = "PureFluid";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX);
                            LF.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF.textBox3.Text = Convert.ToString(puntero_aplicacion.temp25);
                            LF.textBox6.Text = Convert.ToString(puntero_aplicacion.temp26);
                            LF.textBox4.Text = Convert.ToString(puntero_aplicacion.pres25);
                            LF.textBox5.Text = Convert.ToString(puntero_aplicacion.pres26);
                            LF.textBox107.Text = Convert.ToString(10);
                            LF.button1_Click(this, e);
                            puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area = LF.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Main_SF_Pressure_drop = LF.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //MC1_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = p_mc1_in2_list[maxIndex].ToString();
                            //MC1_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = p_mc1_out2_list[maxIndex].ToString();
                            //MC2_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = p_mc2_out2_list[maxIndex].ToString();
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = ua_LT_list[maxIndex].ToString();
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = ua_HT_list[maxIndex].ToString();
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = recomp_frac2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoIntercoolingwithoutReheating.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoIntercoolingwithoutReheating.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_mc1_in_value = puntero_aplicacion.p_mc1_in2;
                            initial_mc1_out_value = puntero_aplicacion.p_mc1_out2;
                            initial_mc2_out_value = puntero_aplicacion.p_mc2_out2;
                        }
                    } //UA optimization true

                }
            }

            //Closing Excel Book
            xlWorkBook1.SaveAs(textBox3.Text + "CIT_Optimization_RCMCI_with_Two_Intercooling_without_ReHeating" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }
    }
}
