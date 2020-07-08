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
    public partial class RCMCI_with_Five_ReHeating_Optimization_Analysis_Results : Form
    {
        public RCMCI_with_Five_Reheatings puntero_aplicacion;

        public RCMCI_with_Five_ReHeating_Optimization_Analysis_Results(RCMCI_with_Five_Reheatings puntero1)
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

            //Optimize UA
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," +
                               puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," +
                               puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                               puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                //Store Input Data from Graphical User Interface GUI into variables
                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox39.Text);
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox38.Text);
                puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.p_rhx4_in2 = Convert.ToDouble(puntero_aplicacion.textBox119.Text);
                puntero_aplicacion.t_rht4_in2 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                puntero_aplicacion.p_rhx5_in2 = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                puntero_aplicacion.t_rht5_in2 = Convert.ToDouble(puntero_aplicacion.textBox122.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox37.Text);
                puntero_aplicacion.eta_trh32 = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                puntero_aplicacion.eta_trh42 = Convert.ToDouble(puntero_aplicacion.textBox117.Text);
                puntero_aplicacion.eta_trh52 = Convert.ToDouble(puntero_aplicacion.textBox121.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox29.Text);
                puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox105.Text);
                puntero_aplicacion.dp2_rhx31 = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                puntero_aplicacion.dp2_rhx41 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_rhx51 = Convert.ToDouble(puntero_aplicacion.textBox120.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                core.RCMCIwithFiveReheating cicloRCMCIwithFiveReheating = new core.RCMCIwithFiveReheating();

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
                List<Double> p_rhx3_in2_list = new List<Double>();
                List<Double> p_rhx4_in2_list = new List<Double>();
                List<Double> p_rhx5_in2_list = new List<Double>();
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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox34.Text;
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
                xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 7] = "P_rhx1(kPa)";
                xlWorkSheet1.Cells[4, 8] = "P_rhx2(kPa)";
                xlWorkSheet1.Cells[4, 9] = "P_rhx3(kPa)";
                xlWorkSheet1.Cells[4, 10] = "P_rhx4(kPa)";
                xlWorkSheet1.Cells[4, 11] = "P_rhx5(kPa)";
                xlWorkSheet1.Cells[4, 12] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 14] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 15] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 16] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 8, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200),
                    (initial_CIP_value + 2000), (initial_CIP_value + 2500), (initial_CIP_value + 3000),
                    (initial_CIP_value + 3500) , (initial_CIP_value + 4000)});

                    solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc2_out2 / 1.5), (puntero_aplicacion.p_mc2_out2 / 1.5),
                    puntero_aplicacion.p_mc2_out2 ,  puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2 ,
                    puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2});

                    solver.SetInitialStepSize(new[] { 0.05, 100, 100, 500, 500, 500, 500, 500 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 18000, 17000, 15000, 14000, 13000 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withFiveReheating(puntero_aplicacion.luis,
                        ref cicloRCMCIwithFiveReheating, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[4], puntero_aplicacion.t_rht3_in2,
                        variables[5], puntero_aplicacion.t_rht4_in2, variables[6],
                        puntero_aplicacion.t_rht5_in2, variables[7], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2,
                        puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22,
                        puntero_aplicacion.eta_trh32, puntero_aplicacion.eta_trh42, puntero_aplicacion.eta_trh52,
                        puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2,
                        puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2,
                        -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1,
                        -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2,
                        -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21,
                        -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_rhx31, -puntero_aplicacion.dp2_rhx32,
                        -puntero_aplicacion.dp2_rhx41, -puntero_aplicacion.dp2_rhx42, -puntero_aplicacion.dp2_rhx51,
                        -puntero_aplicacion.dp2_rhx52, -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithFiveReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithFiveReheating.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithFiveReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_mc2_in2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        puntero_aplicacion.p_rhx2_in2 = variables[4];
                        puntero_aplicacion.p_rhx3_in2 = variables[5];
                        puntero_aplicacion.p_rhx4_in2 = variables[6];
                        puntero_aplicacion.p_rhx5_in2 = variables[7];

                        puntero_aplicacion.temp21 = cicloRCMCIwithFiveReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithFiveReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithFiveReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithFiveReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithFiveReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithFiveReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithFiveReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithFiveReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithFiveReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithFiveReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithFiveReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithFiveReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithFiveReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithFiveReheating.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithFiveReheating.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithFiveReheating.temp[15];
                        puntero_aplicacion.temp217 = cicloRCMCIwithFiveReheating.temp[16];
                        puntero_aplicacion.temp218 = cicloRCMCIwithFiveReheating.temp[17];
                        puntero_aplicacion.temp219 = cicloRCMCIwithFiveReheating.temp[18];
                        puntero_aplicacion.temp220 = cicloRCMCIwithFiveReheating.temp[19];
                        puntero_aplicacion.temp221 = cicloRCMCIwithFiveReheating.temp[20];
                        puntero_aplicacion.temp222 = cicloRCMCIwithFiveReheating.temp[21];

                        puntero_aplicacion.pres21 = cicloRCMCIwithFiveReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithFiveReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithFiveReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithFiveReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithFiveReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithFiveReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithFiveReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithFiveReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithFiveReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithFiveReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithFiveReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithFiveReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithFiveReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithFiveReheating.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithFiveReheating.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithFiveReheating.pres[15];
                        puntero_aplicacion.pres217 = cicloRCMCIwithFiveReheating.pres[16];
                        puntero_aplicacion.pres218 = cicloRCMCIwithFiveReheating.pres[17];
                        puntero_aplicacion.pres219 = cicloRCMCIwithFiveReheating.pres[18];
                        puntero_aplicacion.pres220 = cicloRCMCIwithFiveReheating.pres[19];
                        puntero_aplicacion.pres221 = cicloRCMCIwithFiveReheating.pres[20];
                        puntero_aplicacion.pres222 = cicloRCMCIwithFiveReheating.pres[21];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithFiveReheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithFiveReheating.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithFiveReheating.RHX2.Q_dot;
                        puntero_aplicacion.RHX3 = cicloRCMCIwithFiveReheating.RHX3.Q_dot;
                        puntero_aplicacion.RHX4 = cicloRCMCIwithFiveReheating.RHX4.Q_dot;
                        puntero_aplicacion.RHX5 = cicloRCMCIwithFiveReheating.RHX5.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithFiveReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithFiveReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithFiveReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithFiveReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithFiveReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithFiveReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithFiveReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithFiveReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithFiveReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithFiveReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithFiveReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithFiveReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithFiveReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithFiveReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithFiveReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithFiveReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithFiveReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithFiveReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithFiveReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithFiveReheating.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithFiveReheating.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithFiveReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
                        p_rhx4_in2_list.Add(puntero_aplicacion.p_rhx4_in2);
                        p_rhx5_in2_list.Add(puntero_aplicacion.p_rhx5_in2);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                        listBox27.Items.Add(puntero_aplicacion.p_rhx4_in2.ToString());
                        listBox28.Items.Add(puntero_aplicacion.p_rhx5_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithFiveReheating.temp[7] - cicloRCMCIwithFiveReheating.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithFiveReheating.temp[8] - cicloRCMCIwithFiveReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithFiveReheating.temp[7] - cicloRCMCIwithFiveReheating.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithFiveReheating.temp[6] - cicloRCMCIwithFiveReheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //MC1_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                        //CIT (MC1)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx1_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //P_rhx2_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.p_rhx2_in2.ToString();
                        //P_rhx3_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.p_rhx3_in2.ToString();
                        //P_rhx4_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = puntero_aplicacion.p_rhx4_in2.ToString();
                        //P_rhx5_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = puntero_aplicacion.p_rhx5_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloRCMCIwithFiveReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 15] = cicloRCMCIwithFiveReheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 16] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    //max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox5.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox6.Text = p_rhx3_in2_list[maxIndex].ToString();
                    textBox8.Text = p_rhx4_in2_list[maxIndex].ToString();
                    textBox7.Text = p_rhx5_in2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox39.Text = p_rhx2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox107.Text = p_rhx3_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox119.Text = p_rhx4_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox123.Text = p_rhx5_in2_list[maxIndex].ToString();
                    }
                }
            }

            //Optimize UA
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," +
                               puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," +
                               puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                               puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                //Store Input Data from Graphical User Interface GUI into variables
                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox39.Text);
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox38.Text);
                puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.p_rhx4_in2 = Convert.ToDouble(puntero_aplicacion.textBox119.Text);
                puntero_aplicacion.t_rht4_in2 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                puntero_aplicacion.p_rhx5_in2 = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                puntero_aplicacion.t_rht5_in2 = Convert.ToDouble(puntero_aplicacion.textBox122.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox37.Text);
                puntero_aplicacion.eta_trh32 = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                puntero_aplicacion.eta_trh42 = Convert.ToDouble(puntero_aplicacion.textBox117.Text);
                puntero_aplicacion.eta_trh52 = Convert.ToDouble(puntero_aplicacion.textBox121.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox29.Text);
                puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox105.Text);
                puntero_aplicacion.dp2_rhx31 = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                puntero_aplicacion.dp2_rhx41 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_rhx51 = Convert.ToDouble(puntero_aplicacion.textBox120.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                core.RCMCIwithFiveReheating cicloRCMCIwithFiveReheating = new core.RCMCIwithFiveReheating();

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
                List<Double> p_rhx3_in2_list = new List<Double>();
                List<Double> p_rhx4_in2_list = new List<Double>();
                List<Double> p_rhx5_in2_list = new List<Double>();
                List<Double> ua_LT_list = new List<Double>();
                List<Double> ua_HT_list = new List<Double>();
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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox34.Text;
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
                xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 7] = "P_rhx1(kPa)";
                xlWorkSheet1.Cells[4, 8] = "P_rhx2(kPa)";
                xlWorkSheet1.Cells[4, 9] = "P_rhx3(kPa)";
                xlWorkSheet1.Cells[4, 10] = "P_rhx4(kPa)";
                xlWorkSheet1.Cells[4, 11] = "P_rhx5(kPa)";
                xlWorkSheet1.Cells[4, 12] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 14] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 15] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 16] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 9, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200),
                    (initial_CIP_value + 2000), (initial_CIP_value + 2500), (initial_CIP_value + 3000),
                    (initial_CIP_value + 3500) , (initial_CIP_value + 4000), 0.0});

                    solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc2_out2 / 1.5), (puntero_aplicacion.p_mc2_out2 / 1.5),
                    puntero_aplicacion.p_mc2_out2 ,  puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2 ,
                    puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2, 1.0});

                    solver.SetInitialStepSize(new[] { 0.05, 100, 100, 500, 500, 500, 500, 500, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500),
                                               18000, 17000, 15000, 14000, 13000, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withFiveReheating_for_optimization(puntero_aplicacion.luis,
                        ref cicloRCMCIwithFiveReheating, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[4], puntero_aplicacion.t_rht3_in2,
                        variables[5], puntero_aplicacion.t_rht4_in2, variables[6],
                        puntero_aplicacion.t_rht5_in2, variables[7], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], variables[8], UA_Total,
                        puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2,
                        puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22,
                        puntero_aplicacion.eta_trh32, puntero_aplicacion.eta_trh42, puntero_aplicacion.eta_trh52,
                        puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2,
                        puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2,
                        -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1,
                        -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2,
                        -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21,
                        -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_rhx31, -puntero_aplicacion.dp2_rhx32,
                        -puntero_aplicacion.dp2_rhx41, -puntero_aplicacion.dp2_rhx42, -puntero_aplicacion.dp2_rhx51,
                        -puntero_aplicacion.dp2_rhx52, -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithFiveReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithFiveReheating.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithFiveReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_mc2_in2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        puntero_aplicacion.p_rhx2_in2 = variables[4];
                        puntero_aplicacion.p_rhx3_in2 = variables[5];
                        puntero_aplicacion.p_rhx4_in2 = variables[6];
                        puntero_aplicacion.p_rhx5_in2 = variables[7];
                        LT_fraction = variables[8];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRCMCIwithFiveReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithFiveReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithFiveReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithFiveReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithFiveReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithFiveReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithFiveReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithFiveReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithFiveReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithFiveReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithFiveReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithFiveReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithFiveReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithFiveReheating.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithFiveReheating.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithFiveReheating.temp[15];
                        puntero_aplicacion.temp217 = cicloRCMCIwithFiveReheating.temp[16];
                        puntero_aplicacion.temp218 = cicloRCMCIwithFiveReheating.temp[17];
                        puntero_aplicacion.temp219 = cicloRCMCIwithFiveReheating.temp[18];
                        puntero_aplicacion.temp220 = cicloRCMCIwithFiveReheating.temp[19];
                        puntero_aplicacion.temp221 = cicloRCMCIwithFiveReheating.temp[20];
                        puntero_aplicacion.temp222 = cicloRCMCIwithFiveReheating.temp[21];

                        puntero_aplicacion.pres21 = cicloRCMCIwithFiveReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithFiveReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithFiveReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithFiveReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithFiveReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithFiveReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithFiveReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithFiveReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithFiveReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithFiveReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithFiveReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithFiveReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithFiveReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithFiveReheating.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithFiveReheating.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithFiveReheating.pres[15];
                        puntero_aplicacion.pres217 = cicloRCMCIwithFiveReheating.pres[16];
                        puntero_aplicacion.pres218 = cicloRCMCIwithFiveReheating.pres[17];
                        puntero_aplicacion.pres219 = cicloRCMCIwithFiveReheating.pres[18];
                        puntero_aplicacion.pres220 = cicloRCMCIwithFiveReheating.pres[19];
                        puntero_aplicacion.pres221 = cicloRCMCIwithFiveReheating.pres[20];
                        puntero_aplicacion.pres222 = cicloRCMCIwithFiveReheating.pres[21];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithFiveReheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithFiveReheating.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithFiveReheating.RHX2.Q_dot;
                        puntero_aplicacion.RHX3 = cicloRCMCIwithFiveReheating.RHX3.Q_dot;
                        puntero_aplicacion.RHX4 = cicloRCMCIwithFiveReheating.RHX4.Q_dot;
                        puntero_aplicacion.RHX5 = cicloRCMCIwithFiveReheating.RHX5.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithFiveReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithFiveReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithFiveReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithFiveReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithFiveReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithFiveReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithFiveReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithFiveReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithFiveReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithFiveReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithFiveReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithFiveReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithFiveReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithFiveReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithFiveReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithFiveReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithFiveReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithFiveReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithFiveReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithFiveReheating.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithFiveReheating.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithFiveReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
                        p_rhx4_in2_list.Add(puntero_aplicacion.p_rhx4_in2);
                        p_rhx5_in2_list.Add(puntero_aplicacion.p_rhx5_in2);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                        listBox27.Items.Add(puntero_aplicacion.p_rhx4_in2.ToString());
                        listBox28.Items.Add(puntero_aplicacion.p_rhx5_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithFiveReheating.temp[7] - cicloRCMCIwithFiveReheating.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithFiveReheating.temp[8] - cicloRCMCIwithFiveReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithFiveReheating.temp[7] - cicloRCMCIwithFiveReheating.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithFiveReheating.temp[6] - cicloRCMCIwithFiveReheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //MC1_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                        //CIT (MC1)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx1_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //P_rhx2_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.p_rhx2_in2.ToString();
                        //P_rhx3_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.p_rhx3_in2.ToString();
                        //P_rhx4_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = puntero_aplicacion.p_rhx4_in2.ToString();
                        //P_rhx5_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = puntero_aplicacion.p_rhx5_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloRCMCIwithFiveReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 15] = cicloRCMCIwithFiveReheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 16] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    //max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox5.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox6.Text = p_rhx3_in2_list[maxIndex].ToString();
                    textBox8.Text = p_rhx4_in2_list[maxIndex].ToString();
                    textBox7.Text = p_rhx5_in2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox39.Text = p_rhx2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox107.Text = p_rhx3_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox119.Text = p_rhx4_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox123.Text = p_rhx5_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }
                }
            }

            //Closing Excel Book
            xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_Five_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //Run CIT Optimization
        private void button7_Click(object sender, EventArgs e)
        {
            int counter = 0;

            double initial_mc1_in_value = 0;
            double initial_mc1_out_value = 0;

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

            //Loop for UA optimization
            for (double j = Convert.ToDouble(textBox11.Text); j <= Convert.ToDouble(textBox10.Text); j = j + Convert.ToDouble(textBox9.Text))
            {
                puntero_aplicacion.ua_lt2 = j / 2;
                puntero_aplicacion.ua_ht2 = j / 2;

                //Loop for CIT optimization
                for (double i = Convert.ToDouble(textBox57.Text); i <= Convert.ToDouble(textBox56.Text); i = i + Convert.ToDouble(textBox55.Text))
                {
                    counter = 0;

                    //Optimize UA
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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," +
                                       puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," +
                                       puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                                       puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                        //Store Input Data from Graphical User Interface GUI into variables
                        puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                        puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                        puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                        puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                        puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox39.Text);
                        puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox38.Text);
                        puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                        puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                        puntero_aplicacion.p_rhx4_in2 = Convert.ToDouble(puntero_aplicacion.textBox119.Text);
                        puntero_aplicacion.t_rht4_in2 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                        puntero_aplicacion.p_rhx5_in2 = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                        puntero_aplicacion.t_rht5_in2 = Convert.ToDouble(puntero_aplicacion.textBox122.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                        puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                        puntero_aplicacion.eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox37.Text);
                        puntero_aplicacion.eta_trh32 = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                        puntero_aplicacion.eta_trh42 = Convert.ToDouble(puntero_aplicacion.textBox117.Text);
                        puntero_aplicacion.eta_trh52 = Convert.ToDouble(puntero_aplicacion.textBox121.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox29.Text);
                        puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox105.Text);
                        puntero_aplicacion.dp2_rhx31 = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                        puntero_aplicacion.dp2_rhx41 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                        puntero_aplicacion.dp2_rhx51 = Convert.ToDouble(puntero_aplicacion.textBox120.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        core.RCMCIwithFiveReheating cicloRCMCIwithFiveReheating = new core.RCMCIwithFiveReheating();

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc1_in2_list = new List<Double>();
                        List<Double> p_mc1_out2_list = new List<Double>();
                        List<Double> p_rhx1_in2_list = new List<Double>();
                        List<Double> p_rhx2_in2_list = new List<Double>();
                        List<Double> p_rhx3_in2_list = new List<Double>();
                        List<Double> p_rhx4_in2_list = new List<Double>();
                        List<Double> p_rhx5_in2_list = new List<Double>();
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
                        List<Double> t15_list = new List<Double>();
                        List<Double> t16_list = new List<Double>();
                        List<Double> t17_list = new List<Double>();
                        List<Double> t18_list = new List<Double>();
                        List<Double> t19_list = new List<Double>();
                        List<Double> t20_list = new List<Double>();
                        List<Double> t21_list = new List<Double>();
                        List<Double> t22_list = new List<Double>();

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
                        List<Double> p15_list = new List<Double>();
                        List<Double> p16_list = new List<Double>();
                        List<Double> p17_list = new List<Double>();
                        List<Double> p18_list = new List<Double>();
                        List<Double> p19_list = new List<Double>();
                        List<Double> p20_list = new List<Double>();
                        List<Double> p21_list = new List<Double>();
                        List<Double> p22_list = new List<Double>();

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

                        if (checkBox6.Checked == true)
                        {
                            initial_mc1_in_value = Convert.ToDouble(textBox1.Text);
                        }
                        else
                        {
                            initial_mc1_in_value = puntero_aplicacion.MixtureCriticalPressure;
                        }

                        if (i == Convert.ToDouble(textBox57.Text))
                        {
                            initial_mc1_in_value = puntero_aplicacion.MixtureCriticalPressure;
                            initial_mc1_out_value = puntero_aplicacion.MixtureCriticalPressure + 500;

                            xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," +
                                                       puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," +
                                                       puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox69.Text + "," +
                                                       puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox70.Text;
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
                            xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 7] = "P_rhx1(kPa)";
                            xlWorkSheet1.Cells[4, 8] = "P_rhx2(kPa)";
                            xlWorkSheet1.Cells[4, 9] = "P_rhx3(kPa)";
                            xlWorkSheet1.Cells[4, 10] = "P_rhx4(kPa)";
                            xlWorkSheet1.Cells[4, 11] = "P_rhx5(kPa)";
                            xlWorkSheet1.Cells[4, 12] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 13] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 14] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 15] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 16] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 17] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 18] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 19] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 20] = "LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 21] = "PTC_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 22] = "PTC_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 23] = "LF_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 24] = "LF_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 25] = "PTC_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 26] = "PTC_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 27] = "LF_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 28] = "LF_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 29] = "PTC_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 30] = "PTC_RHX3_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 31] = "LF_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 32] = "LF_RHX3_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 33] = "PTC_RHX4_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 34] = "PTC_RHX4_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 35] = "LF_RHX4_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 36] = "LF_RHX4_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 37] = "PTC_RHX5_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 38] = "PTC_RHX5_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 39] = "LF_RHX5_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 40] = "LF_RHX5_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 8, 0.000001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.1, initial_mc1_in_value, (initial_mc1_in_value + 200),
                             (initial_mc1_in_value + 2000), (initial_mc1_in_value + 2500), (initial_mc1_in_value + 3000),
                             (initial_mc1_in_value + 3500) , (initial_mc1_in_value + 4000)});

                            solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc2_out2 / 1.5), (puntero_aplicacion.p_mc2_out2 / 1.5),
                            puntero_aplicacion.p_mc2_out2 ,  puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2 ,
                            puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2});

                            solver.SetInitialStepSize(new[] { 0.05, 100, 100, 500, 500, 500, 500, 500 });

                            var initialValue = new[] { 0.2, initial_mc1_in_value, (initial_mc1_in_value + 500), 18000, 17000, 15000, 14000, 13000 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_withFiveReheating(puntero_aplicacion.luis,
                                ref cicloRCMCIwithFiveReheating, puntero_aplicacion.w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[4], puntero_aplicacion.t_rht3_in2,
                                variables[5], puntero_aplicacion.t_rht4_in2, variables[6],
                                puntero_aplicacion.t_rht5_in2, variables[7], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i,
                                variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                                puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2,
                                puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22,
                                puntero_aplicacion.eta_trh32, puntero_aplicacion.eta_trh42, puntero_aplicacion.eta_trh52,
                                puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2,
                                puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2,
                                -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1,
                                -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2,
                                -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21,
                                -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_rhx31, -puntero_aplicacion.dp2_rhx32,
                                -puntero_aplicacion.dp2_rhx41, -puntero_aplicacion.dp2_rhx42, -puntero_aplicacion.dp2_rhx51,
                                -puntero_aplicacion.dp2_rhx52, -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCIwithFiveReheating.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCIwithFiveReheating.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCIwithFiveReheating.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                puntero_aplicacion.p_mc2_in2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];
                                puntero_aplicacion.p_rhx2_in2 = variables[4];
                                puntero_aplicacion.p_rhx3_in2 = variables[5];
                                puntero_aplicacion.p_rhx4_in2 = variables[6];
                                puntero_aplicacion.p_rhx5_in2 = variables[7];

                                puntero_aplicacion.temp21 = cicloRCMCIwithFiveReheating.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCIwithFiveReheating.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCIwithFiveReheating.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCIwithFiveReheating.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCIwithFiveReheating.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCIwithFiveReheating.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCIwithFiveReheating.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCIwithFiveReheating.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCIwithFiveReheating.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCIwithFiveReheating.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCIwithFiveReheating.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCIwithFiveReheating.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCIwithFiveReheating.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCIwithFiveReheating.temp[13];
                                puntero_aplicacion.temp215 = cicloRCMCIwithFiveReheating.temp[14];
                                puntero_aplicacion.temp216 = cicloRCMCIwithFiveReheating.temp[15];
                                puntero_aplicacion.temp217 = cicloRCMCIwithFiveReheating.temp[16];
                                puntero_aplicacion.temp218 = cicloRCMCIwithFiveReheating.temp[17];
                                puntero_aplicacion.temp219 = cicloRCMCIwithFiveReheating.temp[18];
                                puntero_aplicacion.temp220 = cicloRCMCIwithFiveReheating.temp[19];
                                puntero_aplicacion.temp221 = cicloRCMCIwithFiveReheating.temp[20];
                                puntero_aplicacion.temp222 = cicloRCMCIwithFiveReheating.temp[21];

                                puntero_aplicacion.pres21 = cicloRCMCIwithFiveReheating.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCIwithFiveReheating.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCIwithFiveReheating.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCIwithFiveReheating.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCIwithFiveReheating.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCIwithFiveReheating.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCIwithFiveReheating.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCIwithFiveReheating.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCIwithFiveReheating.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCIwithFiveReheating.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCIwithFiveReheating.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCIwithFiveReheating.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCIwithFiveReheating.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCIwithFiveReheating.pres[13];
                                puntero_aplicacion.pres215 = cicloRCMCIwithFiveReheating.pres[14];
                                puntero_aplicacion.pres216 = cicloRCMCIwithFiveReheating.pres[15];
                                puntero_aplicacion.pres217 = cicloRCMCIwithFiveReheating.pres[16];
                                puntero_aplicacion.pres218 = cicloRCMCIwithFiveReheating.pres[17];
                                puntero_aplicacion.pres219 = cicloRCMCIwithFiveReheating.pres[18];
                                puntero_aplicacion.pres220 = cicloRCMCIwithFiveReheating.pres[19];
                                puntero_aplicacion.pres221 = cicloRCMCIwithFiveReheating.pres[20];
                                puntero_aplicacion.pres222 = cicloRCMCIwithFiveReheating.pres[21];

                                puntero_aplicacion.PHX1 = cicloRCMCIwithFiveReheating.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCIwithFiveReheating.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloRCMCIwithFiveReheating.RHX2.Q_dot;
                                puntero_aplicacion.RHX3 = cicloRCMCIwithFiveReheating.RHX3.Q_dot;
                                puntero_aplicacion.RHX4 = cicloRCMCIwithFiveReheating.RHX4.Q_dot;
                                puntero_aplicacion.RHX5 = cicloRCMCIwithFiveReheating.RHX5.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCIwithFiveReheating.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCIwithFiveReheating.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCIwithFiveReheating.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCIwithFiveReheating.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCIwithFiveReheating.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCIwithFiveReheating.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCIwithFiveReheating.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCIwithFiveReheating.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCIwithFiveReheating.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCIwithFiveReheating.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCIwithFiveReheating.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCIwithFiveReheating.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCIwithFiveReheating.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCIwithFiveReheating.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCIwithFiveReheating.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCIwithFiveReheating.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCIwithFiveReheating.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCIwithFiveReheating.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCIwithFiveReheating.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCIwithFiveReheating.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCIwithFiveReheating.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCIwithFiveReheating.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                                p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
                                p_rhx4_in2_list.Add(puntero_aplicacion.p_rhx4_in2);
                                p_rhx5_in2_list.Add(puntero_aplicacion.p_rhx5_in2);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                                HT_Eff_list.Add(cicloRCMCIwithFiveReheating.HT.eff);
                                LT_Eff_list.Add(cicloRCMCIwithFiveReheating.LT.eff);

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
                                t15_list.Add(puntero_aplicacion.temp215);
                                t16_list.Add(puntero_aplicacion.temp216);
                                t17_list.Add(puntero_aplicacion.temp217);
                                t18_list.Add(puntero_aplicacion.temp218);
                                t19_list.Add(puntero_aplicacion.temp219);
                                t20_list.Add(puntero_aplicacion.temp220);
                                t21_list.Add(puntero_aplicacion.temp221);
                                t22_list.Add(puntero_aplicacion.temp222);

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
                                p15_list.Add(puntero_aplicacion.pres215);
                                p16_list.Add(puntero_aplicacion.pres216);
                                p17_list.Add(puntero_aplicacion.pres217);
                                p18_list.Add(puntero_aplicacion.pres218);
                                p19_list.Add(puntero_aplicacion.pres219);
                                p20_list.Add(puntero_aplicacion.pres220);
                                p21_list.Add(puntero_aplicacion.pres221);
                                p22_list.Add(puntero_aplicacion.pres222);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox23.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                                listBox27.Items.Add(puntero_aplicacion.p_rhx4_in2.ToString());
                                listBox28.Items.Add(puntero_aplicacion.p_rhx5_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp28.ToString());
                                
                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            //max_eta_thermal = eta_thermal2_list.Max();

                            var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                            textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                            textBox4.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox5.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox6.Text = p_rhx3_in2_list[maxIndex].ToString();
                            textBox8.Text = p_rhx4_in2_list[maxIndex].ToString();
                            textBox7.Text = p_rhx5_in2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox39.Text = p_rhx2_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox107.Text = p_rhx3_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox119.Text = p_rhx4_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox123.Text = p_rhx5_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox2.Text = i.ToString();
                                puntero_aplicacion.textBox28.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_mc1_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_mc1_out2_list[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx1_in2_list[maxIndex].ToString());
                            listBox22.Items.Add(p_rhx2_in2_list[maxIndex].ToString());
                            listBox24.Items.Add(p_rhx3_in2_list[maxIndex].ToString());
                            listBox25.Items.Add(p_rhx4_in2_list[maxIndex].ToString());
                            listBox26.Items.Add(p_rhx5_in2_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());

                            //MAIN SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "NewMixture";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX1);
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
                            LF.comboBox2.Text = "NewMixture";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX1);
                            LF.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF.textBox3.Text = Convert.ToString(puntero_aplicacion.temp25);
                            LF.textBox6.Text = Convert.ToString(puntero_aplicacion.temp26);
                            LF.textBox4.Text = Convert.ToString(puntero_aplicacion.pres25);
                            LF.textBox5.Text = Convert.ToString(puntero_aplicacion.pres26);
                            LF.textBox107.Text = Convert.ToString(10);
                            LF.button1_Click(this, e);
                            puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area = LF.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Main_SF_Pressure_drop = LF.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX1 = new PTC_SF_Calculation();
                            PTC_RHX1.calledForSensingAnalysis = true;
                            PTC_RHX1.comboBox1.Text = "Solar Salt";
                            PTC_RHX1.comboBox2.Text = "NewMixture";
                            PTC_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            PTC_RHX1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp211);
                            PTC_RHX1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp212);
                            PTC_RHX1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres211);
                            PTC_RHX1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres212);
                            PTC_RHX1.textBox107.Text = Convert.ToString(10);
                            PTC_RHX1.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area = PTC_RHX1.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop = PTC_RHX1.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX1 = new LF_SF_Calculation();
                            LF_RHX1.calledForSensingAnalysis = true;
                            LF_RHX1.comboBox1.Text = "Solar Salt";
                            LF_RHX1.comboBox2.Text = "NewMixture";
                            LF_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            LF_RHX1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp211);
                            LF_RHX1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp212);
                            LF_RHX1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres211);
                            LF_RHX1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres212);
                            LF_RHX1.textBox107.Text = Convert.ToString(10);
                            LF_RHX1.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area = LF_RHX1.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop = LF_RHX1.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX2 = new PTC_SF_Calculation();
                            PTC_RHX2.calledForSensingAnalysis = true;
                            PTC_RHX2.comboBox1.Text = "Solar Salt";
                            PTC_RHX2.comboBox2.Text = "NewMixture";
                            PTC_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            PTC_RHX2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp215);
                            PTC_RHX2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp216);
                            PTC_RHX2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres215);
                            PTC_RHX2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres216);
                            PTC_RHX2.textBox107.Text = Convert.ToString(10);
                            PTC_RHX2.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area = PTC_RHX2.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop = PTC_RHX2.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX2 = new LF_SF_Calculation();
                            LF_RHX2.calledForSensingAnalysis = true;
                            LF_RHX2.comboBox1.Text = "Solar Salt";
                            LF_RHX2.comboBox2.Text = "NewMixture";
                            LF_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            LF_RHX2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp215);
                            LF_RHX2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp216);
                            LF_RHX2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres215);
                            LF_RHX2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres216);
                            LF_RHX2.textBox107.Text = Convert.ToString(10);
                            LF_RHX2.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area = LF_RHX2.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop = LF_RHX2.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX3 = new PTC_SF_Calculation();
                            PTC_RHX3.calledForSensingAnalysis = true;
                            PTC_RHX3.comboBox1.Text = "Solar Salt";
                            PTC_RHX3.comboBox2.Text = "NewMixture";
                            PTC_RHX3.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX3.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX3.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX3);
                            PTC_RHX3.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX3.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            PTC_RHX3.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            PTC_RHX3.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            PTC_RHX3.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            PTC_RHX3.textBox107.Text = Convert.ToString(10);
                            PTC_RHX3.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area = PTC_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop = PTC_RHX3.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX3 = new LF_SF_Calculation();
                            LF_RHX3.calledForSensingAnalysis = true;
                            LF_RHX3.comboBox1.Text = "Solar Salt";
                            LF_RHX3.comboBox2.Text = "NewMixture";
                            LF_RHX3.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX3.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX3.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX3);
                            LF_RHX3.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX3.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            LF_RHX3.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            LF_RHX3.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            LF_RHX3.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            LF_RHX3.textBox107.Text = Convert.ToString(10);
                            LF_RHX3.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area = LF_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop = LF_RHX3.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX4 = new PTC_SF_Calculation();
                            PTC_RHX4.calledForSensingAnalysis = true;
                            PTC_RHX4.comboBox1.Text = "Solar Salt";
                            PTC_RHX4.comboBox2.Text = "NewMixture";
                            PTC_RHX4.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX4.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX4.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX4);
                            PTC_RHX4.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX4.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            PTC_RHX4.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            PTC_RHX4.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            PTC_RHX4.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            PTC_RHX4.textBox107.Text = Convert.ToString(10);
                            PTC_RHX4.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating4_SF_Effective_Apperture_Area = PTC_RHX4.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating4_SF_Pressure_drop = PTC_RHX4.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX4 = new LF_SF_Calculation();
                            LF_RHX4.calledForSensingAnalysis = true;
                            LF_RHX4.comboBox1.Text = "Solar Salt";
                            LF_RHX4.comboBox2.Text = "NewMixture";
                            LF_RHX4.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX4.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX4.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX4);
                            LF_RHX4.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX4.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            LF_RHX4.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            LF_RHX4.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            LF_RHX4.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            LF_RHX4.textBox107.Text = Convert.ToString(10);
                            LF_RHX4.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating4_SF_Effective_Apperture_Area = LF_RHX4.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating4_SF_Pressure_drop = LF_RHX4.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX5 = new PTC_SF_Calculation();
                            PTC_RHX5.calledForSensingAnalysis = true;
                            PTC_RHX5.comboBox1.Text = "Solar Salt";
                            PTC_RHX5.comboBox2.Text = "NewMixture";
                            PTC_RHX5.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX5.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX5.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX5);
                            PTC_RHX5.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX5.textBox3.Text = Convert.ToString(puntero_aplicacion.temp221);
                            PTC_RHX5.textBox6.Text = Convert.ToString(puntero_aplicacion.temp222);
                            PTC_RHX5.textBox4.Text = Convert.ToString(puntero_aplicacion.pres221);
                            PTC_RHX5.textBox5.Text = Convert.ToString(puntero_aplicacion.pres222);
                            PTC_RHX5.textBox107.Text = Convert.ToString(10);
                            PTC_RHX5.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating5_SF_Effective_Apperture_Area = PTC_RHX5.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating5_SF_Pressure_drop = PTC_RHX5.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX5 = new LF_SF_Calculation();
                            LF_RHX5.calledForSensingAnalysis = true;
                            LF_RHX5.comboBox1.Text = "Solar Salt";
                            LF_RHX5.comboBox2.Text = "NewMixture";
                            LF_RHX5.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX5.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX5.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX5);
                            LF_RHX5.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX5.textBox3.Text = Convert.ToString(puntero_aplicacion.temp221);
                            LF_RHX5.textBox6.Text = Convert.ToString(puntero_aplicacion.temp222);
                            LF_RHX5.textBox4.Text = Convert.ToString(puntero_aplicacion.pres221);
                            LF_RHX5.textBox5.Text = Convert.ToString(puntero_aplicacion.pres222);
                            LF_RHX5.textBox107.Text = Convert.ToString(10);
                            LF_RHX5.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating5_SF_Effective_Apperture_Area = LF_RHX5.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating5_SF_Pressure_drop = LF_RHX5.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //PC_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = p_mc1_in2_list[maxIndex].ToString();
                            //PC_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = p_mc1_out2_list[maxIndex].ToString();
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = recomp_frac2_list[maxIndex].ToString();
                            //P_rhx1_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_rhx1_in2_list[maxIndex].ToString();
                            //P_rhx2_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = p_rhx2_in2_list[maxIndex].ToString();
                            //P_rhx3_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = p_rhx3_in2_list[maxIndex].ToString();
                            //P_rhx4_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = p_rhx4_in2_list[maxIndex].ToString();
                            //P_rhx5_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = p_rhx5_in2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloRCMCIwithFiveReheating.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = cicloRCMCIwithFiveReheating.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 17] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 18] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 19] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            //PTC_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 21] = puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 22] = puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop.ToString();
                            //LF_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 23] = puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 24] = puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop.ToString();
                            //PTC_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 25] = puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 26] = puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop.ToString();
                            //LF_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 27] = puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 28] = puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop.ToString();
                            //PTC_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 29] = puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 30] = puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop.ToString();
                            //LF_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 31] = puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 32] = puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop.ToString();
                            //PTC_RHX4_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 33] = puntero_aplicacion.PTC_ReHeating4_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX4_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 34] = puntero_aplicacion.PTC_ReHeating4_SF_Pressure_drop.ToString();
                            //LF_RHX4_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 35] = puntero_aplicacion.LF_ReHeating4_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX4_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 36] = puntero_aplicacion.LF_ReHeating4_SF_Pressure_drop.ToString();
                            //PTC_RHX5_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 37] = puntero_aplicacion.PTC_ReHeating5_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX5_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 38] = puntero_aplicacion.PTC_ReHeating5_SF_Pressure_drop.ToString();
                            //LF_RHX5_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 39] = puntero_aplicacion.LF_ReHeating5_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX5_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 40] = puntero_aplicacion.LF_ReHeating5_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_mc1_in_value = puntero_aplicacion.p_mc1_in2;
                            initial_mc1_out_value = puntero_aplicacion.p_mc1_out2;
                        }
                    }

                    //Optimize UA
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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," +
                                       puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," +
                                       puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                                       puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                        //Store Input Data from Graphical User Interface GUI into variables
                        puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                        puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                        puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                        puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                        puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox39.Text);
                        puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox38.Text);
                        puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                        puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                        puntero_aplicacion.p_rhx4_in2 = Convert.ToDouble(puntero_aplicacion.textBox119.Text);
                        puntero_aplicacion.t_rht4_in2 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                        puntero_aplicacion.p_rhx5_in2 = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                        puntero_aplicacion.t_rht5_in2 = Convert.ToDouble(puntero_aplicacion.textBox122.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                        puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                        puntero_aplicacion.eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox37.Text);
                        puntero_aplicacion.eta_trh32 = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                        puntero_aplicacion.eta_trh42 = Convert.ToDouble(puntero_aplicacion.textBox117.Text);
                        puntero_aplicacion.eta_trh52 = Convert.ToDouble(puntero_aplicacion.textBox121.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox29.Text);
                        puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox105.Text);
                        puntero_aplicacion.dp2_rhx31 = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                        puntero_aplicacion.dp2_rhx41 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                        puntero_aplicacion.dp2_rhx51 = Convert.ToDouble(puntero_aplicacion.textBox120.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        core.RCMCIwithFiveReheating cicloRCMCIwithFiveReheating = new core.RCMCIwithFiveReheating();

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc1_in2_list = new List<Double>();
                        List<Double> p_mc1_out2_list = new List<Double>();
                        List<Double> p_rhx1_in2_list = new List<Double>();
                        List<Double> p_rhx2_in2_list = new List<Double>();
                        List<Double> p_rhx3_in2_list = new List<Double>();
                        List<Double> p_rhx4_in2_list = new List<Double>();
                        List<Double> p_rhx5_in2_list = new List<Double>();
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
                        List<Double> t15_list = new List<Double>();
                        List<Double> t16_list = new List<Double>();
                        List<Double> t17_list = new List<Double>();
                        List<Double> t18_list = new List<Double>();
                        List<Double> t19_list = new List<Double>();
                        List<Double> t20_list = new List<Double>();
                        List<Double> t21_list = new List<Double>();
                        List<Double> t22_list = new List<Double>();

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
                        List<Double> p15_list = new List<Double>();
                        List<Double> p16_list = new List<Double>();
                        List<Double> p17_list = new List<Double>();
                        List<Double> p18_list = new List<Double>();
                        List<Double> p19_list = new List<Double>();
                        List<Double> p20_list = new List<Double>();
                        List<Double> p21_list = new List<Double>();
                        List<Double> p22_list = new List<Double>();

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

                        if (checkBox6.Checked == true)
                        {
                            initial_mc1_in_value = Convert.ToDouble(textBox1.Text);
                        }
                        else
                        {
                            initial_mc1_in_value = puntero_aplicacion.MixtureCriticalPressure;
                        }

                        if (i == Convert.ToDouble(textBox57.Text))
                        {
                            initial_mc1_in_value = puntero_aplicacion.MixtureCriticalPressure;
                            initial_mc1_out_value = puntero_aplicacion.MixtureCriticalPressure + 500;

                            xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," +
                                                       puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," +
                                                       puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox69.Text + "," +
                                                       puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox70.Text;
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
                            xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 7] = "P_rhx1(kPa)";
                            xlWorkSheet1.Cells[4, 8] = "P_rhx2(kPa)";
                            xlWorkSheet1.Cells[4, 9] = "P_rhx3(kPa)";
                            xlWorkSheet1.Cells[4, 10] = "P_rhx4(kPa)";
                            xlWorkSheet1.Cells[4, 11] = "P_rhx5(kPa)";
                            xlWorkSheet1.Cells[4, 12] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 13] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 14] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 15] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 16] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 17] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 18] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 19] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 20] = "LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 21] = "PTC_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 22] = "PTC_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 23] = "LF_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 24] = "LF_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 25] = "PTC_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 26] = "PTC_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 27] = "LF_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 28] = "LF_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 29] = "PTC_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 30] = "PTC_RHX3_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 31] = "LF_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 32] = "LF_RHX3_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 33] = "PTC_RHX4_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 34] = "PTC_RHX4_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 35] = "LF_RHX4_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 36] = "LF_RHX4_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 37] = "PTC_RHX5_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 38] = "PTC_RHX5_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 39] = "LF_RHX5_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 40] = "LF_RHX5_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 9, 0.000001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_mc1_in_value, (initial_mc1_in_value + 200),
                            (initial_mc1_in_value + 2000), (initial_mc1_in_value + 2500), (initial_mc1_in_value + 3000),
                            (initial_mc1_in_value + 3500) , (initial_mc1_in_value + 4000), 0.0});

                            solver.SetUpperBounds(new[] { 1.0, (puntero_aplicacion.p_mc2_out2 / 1.5), (puntero_aplicacion.p_mc2_out2 / 1.5),
                            puntero_aplicacion.p_mc2_out2 ,  puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2 ,
                            puntero_aplicacion.p_mc2_out2 , puntero_aplicacion.p_mc2_out2, 1.0});

                            solver.SetInitialStepSize(new[] { 0.05, 100, 100, 500, 500, 500, 500, 500, 0.05 });

                            var initialValue = new[] { 0.2, initial_mc1_in_value, (initial_mc1_in_value + 500),
                                               18000, 17000, 15000, 14000, 13000, 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_withFiveReheating_for_optimization(puntero_aplicacion.luis,
                                ref cicloRCMCIwithFiveReheating, puntero_aplicacion.w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[4], puntero_aplicacion.t_rht3_in2,
                                variables[5], puntero_aplicacion.t_rht4_in2, variables[6],
                                puntero_aplicacion.t_rht5_in2, variables[7], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i,
                                variables[2], variables[8], UA_Total,
                                puntero_aplicacion.eta2_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2,
                                puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22,
                                puntero_aplicacion.eta_trh32, puntero_aplicacion.eta_trh42, puntero_aplicacion.eta_trh52,
                                puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2,
                                puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2,
                                -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1,
                                -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2,
                                -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21,
                                -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_rhx31, -puntero_aplicacion.dp2_rhx32,
                                -puntero_aplicacion.dp2_rhx41, -puntero_aplicacion.dp2_rhx42, -puntero_aplicacion.dp2_rhx51,
                                -puntero_aplicacion.dp2_rhx52, -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCIwithFiveReheating.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCIwithFiveReheating.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCIwithFiveReheating.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                puntero_aplicacion.p_mc2_in2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];
                                puntero_aplicacion.p_rhx2_in2 = variables[4];
                                puntero_aplicacion.p_rhx3_in2 = variables[5];
                                puntero_aplicacion.p_rhx4_in2 = variables[6];
                                puntero_aplicacion.p_rhx5_in2 = variables[7];
                                LT_fraction = variables[8];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloRCMCIwithFiveReheating.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCIwithFiveReheating.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCIwithFiveReheating.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCIwithFiveReheating.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCIwithFiveReheating.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCIwithFiveReheating.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCIwithFiveReheating.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCIwithFiveReheating.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCIwithFiveReheating.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCIwithFiveReheating.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCIwithFiveReheating.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCIwithFiveReheating.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCIwithFiveReheating.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCIwithFiveReheating.temp[13];
                                puntero_aplicacion.temp215 = cicloRCMCIwithFiveReheating.temp[14];
                                puntero_aplicacion.temp216 = cicloRCMCIwithFiveReheating.temp[15];
                                puntero_aplicacion.temp217 = cicloRCMCIwithFiveReheating.temp[16];
                                puntero_aplicacion.temp218 = cicloRCMCIwithFiveReheating.temp[17];
                                puntero_aplicacion.temp219 = cicloRCMCIwithFiveReheating.temp[18];
                                puntero_aplicacion.temp220 = cicloRCMCIwithFiveReheating.temp[19];
                                puntero_aplicacion.temp221 = cicloRCMCIwithFiveReheating.temp[20];
                                puntero_aplicacion.temp222 = cicloRCMCIwithFiveReheating.temp[21];

                                puntero_aplicacion.pres21 = cicloRCMCIwithFiveReheating.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCIwithFiveReheating.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCIwithFiveReheating.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCIwithFiveReheating.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCIwithFiveReheating.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCIwithFiveReheating.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCIwithFiveReheating.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCIwithFiveReheating.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCIwithFiveReheating.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCIwithFiveReheating.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCIwithFiveReheating.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCIwithFiveReheating.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCIwithFiveReheating.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCIwithFiveReheating.pres[13];
                                puntero_aplicacion.pres215 = cicloRCMCIwithFiveReheating.pres[14];
                                puntero_aplicacion.pres216 = cicloRCMCIwithFiveReheating.pres[15];
                                puntero_aplicacion.pres217 = cicloRCMCIwithFiveReheating.pres[16];
                                puntero_aplicacion.pres218 = cicloRCMCIwithFiveReheating.pres[17];
                                puntero_aplicacion.pres219 = cicloRCMCIwithFiveReheating.pres[18];
                                puntero_aplicacion.pres220 = cicloRCMCIwithFiveReheating.pres[19];
                                puntero_aplicacion.pres221 = cicloRCMCIwithFiveReheating.pres[20];
                                puntero_aplicacion.pres222 = cicloRCMCIwithFiveReheating.pres[21];

                                puntero_aplicacion.PHX1 = cicloRCMCIwithFiveReheating.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCIwithFiveReheating.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloRCMCIwithFiveReheating.RHX2.Q_dot;
                                puntero_aplicacion.RHX3 = cicloRCMCIwithFiveReheating.RHX3.Q_dot;
                                puntero_aplicacion.RHX4 = cicloRCMCIwithFiveReheating.RHX4.Q_dot;
                                puntero_aplicacion.RHX5 = cicloRCMCIwithFiveReheating.RHX5.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCIwithFiveReheating.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCIwithFiveReheating.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCIwithFiveReheating.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCIwithFiveReheating.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCIwithFiveReheating.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCIwithFiveReheating.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCIwithFiveReheating.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCIwithFiveReheating.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCIwithFiveReheating.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCIwithFiveReheating.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCIwithFiveReheating.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCIwithFiveReheating.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCIwithFiveReheating.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCIwithFiveReheating.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCIwithFiveReheating.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCIwithFiveReheating.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCIwithFiveReheating.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCIwithFiveReheating.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCIwithFiveReheating.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCIwithFiveReheating.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCIwithFiveReheating.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCIwithFiveReheating.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                                p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
                                p_rhx4_in2_list.Add(puntero_aplicacion.p_rhx4_in2);
                                p_rhx5_in2_list.Add(puntero_aplicacion.p_rhx5_in2);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                                HT_Eff_list.Add(cicloRCMCIwithFiveReheating.HT.eff);
                                LT_Eff_list.Add(cicloRCMCIwithFiveReheating.LT.eff);

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
                                t15_list.Add(puntero_aplicacion.temp215);
                                t16_list.Add(puntero_aplicacion.temp216);
                                t17_list.Add(puntero_aplicacion.temp217);
                                t18_list.Add(puntero_aplicacion.temp218);
                                t19_list.Add(puntero_aplicacion.temp219);
                                t20_list.Add(puntero_aplicacion.temp220);
                                t21_list.Add(puntero_aplicacion.temp221);
                                t22_list.Add(puntero_aplicacion.temp222);

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
                                p15_list.Add(puntero_aplicacion.pres215);
                                p16_list.Add(puntero_aplicacion.pres216);
                                p17_list.Add(puntero_aplicacion.pres217);
                                p18_list.Add(puntero_aplicacion.pres218);
                                p19_list.Add(puntero_aplicacion.pres219);
                                p20_list.Add(puntero_aplicacion.pres220);
                                p21_list.Add(puntero_aplicacion.pres221);
                                p22_list.Add(puntero_aplicacion.pres222);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox23.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                                listBox27.Items.Add(puntero_aplicacion.p_rhx4_in2.ToString());
                                listBox28.Items.Add(puntero_aplicacion.p_rhx5_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp28.ToString());
                                                              
                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            //max_eta_thermal = eta_thermal2_list.Max();

                            var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                            textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                            textBox4.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox5.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox6.Text = p_rhx3_in2_list[maxIndex].ToString();
                            textBox8.Text = p_rhx4_in2_list[maxIndex].ToString();
                            textBox7.Text = p_rhx5_in2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox39.Text = p_rhx2_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox107.Text = p_rhx3_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox119.Text = p_rhx4_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox123.Text = p_rhx5_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox2.Text = i.ToString();
                                puntero_aplicacion.textBox28.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_mc1_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_mc1_out2_list[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx1_in2_list[maxIndex].ToString());
                            listBox22.Items.Add(p_rhx2_in2_list[maxIndex].ToString());
                            listBox24.Items.Add(p_rhx3_in2_list[maxIndex].ToString());
                            listBox25.Items.Add(p_rhx4_in2_list[maxIndex].ToString());
                            listBox26.Items.Add(p_rhx5_in2_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());

                            //MAIN SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "NewMixture";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX1);
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
                            LF.comboBox2.Text = "NewMixture";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF.textBox1.Text = Convert.ToString(puntero_aplicacion.PHX1);
                            LF.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF.textBox3.Text = Convert.ToString(puntero_aplicacion.temp25);
                            LF.textBox6.Text = Convert.ToString(puntero_aplicacion.temp26);
                            LF.textBox4.Text = Convert.ToString(puntero_aplicacion.pres25);
                            LF.textBox5.Text = Convert.ToString(puntero_aplicacion.pres26);
                            LF.textBox107.Text = Convert.ToString(10);
                            LF.button1_Click(this, e);
                            puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area = LF.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Main_SF_Pressure_drop = LF.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX1 = new PTC_SF_Calculation();
                            PTC_RHX1.calledForSensingAnalysis = true;
                            PTC_RHX1.comboBox1.Text = "Solar Salt";
                            PTC_RHX1.comboBox2.Text = "NewMixture";
                            PTC_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            PTC_RHX1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp211);
                            PTC_RHX1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp212);
                            PTC_RHX1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres211);
                            PTC_RHX1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres212);
                            PTC_RHX1.textBox107.Text = Convert.ToString(10);
                            PTC_RHX1.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area = PTC_RHX1.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop = PTC_RHX1.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX1 = new LF_SF_Calculation();
                            LF_RHX1.calledForSensingAnalysis = true;
                            LF_RHX1.comboBox1.Text = "Solar Salt";
                            LF_RHX1.comboBox2.Text = "NewMixture";
                            LF_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            LF_RHX1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp211);
                            LF_RHX1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp212);
                            LF_RHX1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres211);
                            LF_RHX1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres212);
                            LF_RHX1.textBox107.Text = Convert.ToString(10);
                            LF_RHX1.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area = LF_RHX1.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop = LF_RHX1.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX2 = new PTC_SF_Calculation();
                            PTC_RHX2.calledForSensingAnalysis = true;
                            PTC_RHX2.comboBox1.Text = "Solar Salt";
                            PTC_RHX2.comboBox2.Text = "NewMixture";
                            PTC_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            PTC_RHX2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp215);
                            PTC_RHX2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp216);
                            PTC_RHX2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres215);
                            PTC_RHX2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres216);
                            PTC_RHX2.textBox107.Text = Convert.ToString(10);
                            PTC_RHX2.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area = PTC_RHX2.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop = PTC_RHX2.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX2 = new LF_SF_Calculation();
                            LF_RHX2.calledForSensingAnalysis = true;
                            LF_RHX2.comboBox1.Text = "Solar Salt";
                            LF_RHX2.comboBox2.Text = "NewMixture";
                            LF_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            LF_RHX2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp215);
                            LF_RHX2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp216);
                            LF_RHX2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres215);
                            LF_RHX2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres216);
                            LF_RHX2.textBox107.Text = Convert.ToString(10);
                            LF_RHX2.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area = LF_RHX2.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop = LF_RHX2.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX3 = new PTC_SF_Calculation();
                            PTC_RHX3.calledForSensingAnalysis = true;
                            PTC_RHX3.comboBox1.Text = "Solar Salt";
                            PTC_RHX3.comboBox2.Text = "NewMixture";
                            PTC_RHX3.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX3.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX3.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX3);
                            PTC_RHX3.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX3.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            PTC_RHX3.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            PTC_RHX3.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            PTC_RHX3.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            PTC_RHX3.textBox107.Text = Convert.ToString(10);
                            PTC_RHX3.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area = PTC_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop = PTC_RHX3.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX3 = new LF_SF_Calculation();
                            LF_RHX3.calledForSensingAnalysis = true;
                            LF_RHX3.comboBox1.Text = "Solar Salt";
                            LF_RHX3.comboBox2.Text = "NewMixture";
                            LF_RHX3.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX3.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX3.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX3);
                            LF_RHX3.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX3.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            LF_RHX3.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            LF_RHX3.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            LF_RHX3.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            LF_RHX3.textBox107.Text = Convert.ToString(10);
                            LF_RHX3.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area = LF_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop = LF_RHX3.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX4 = new PTC_SF_Calculation();
                            PTC_RHX4.calledForSensingAnalysis = true;
                            PTC_RHX4.comboBox1.Text = "Solar Salt";
                            PTC_RHX4.comboBox2.Text = "NewMixture";
                            PTC_RHX4.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX4.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX4.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX4);
                            PTC_RHX4.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX4.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            PTC_RHX4.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            PTC_RHX4.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            PTC_RHX4.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            PTC_RHX4.textBox107.Text = Convert.ToString(10);
                            PTC_RHX4.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating4_SF_Effective_Apperture_Area = PTC_RHX4.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating4_SF_Pressure_drop = PTC_RHX4.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX4 = new LF_SF_Calculation();
                            LF_RHX4.calledForSensingAnalysis = true;
                            LF_RHX4.comboBox1.Text = "Solar Salt";
                            LF_RHX4.comboBox2.Text = "NewMixture";
                            LF_RHX4.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX4.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX4.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX4);
                            LF_RHX4.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX4.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            LF_RHX4.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            LF_RHX4.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            LF_RHX4.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            LF_RHX4.textBox107.Text = Convert.ToString(10);
                            LF_RHX4.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating4_SF_Effective_Apperture_Area = LF_RHX4.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating4_SF_Pressure_drop = LF_RHX4.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC_RHX5 = new PTC_SF_Calculation();
                            PTC_RHX5.calledForSensingAnalysis = true;
                            PTC_RHX5.comboBox1.Text = "Solar Salt";
                            PTC_RHX5.comboBox2.Text = "NewMixture";
                            PTC_RHX5.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX5.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX5.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX5);
                            PTC_RHX5.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX5.textBox3.Text = Convert.ToString(puntero_aplicacion.temp221);
                            PTC_RHX5.textBox6.Text = Convert.ToString(puntero_aplicacion.temp222);
                            PTC_RHX5.textBox4.Text = Convert.ToString(puntero_aplicacion.pres221);
                            PTC_RHX5.textBox5.Text = Convert.ToString(puntero_aplicacion.pres222);
                            PTC_RHX5.textBox107.Text = Convert.ToString(10);
                            PTC_RHX5.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating5_SF_Effective_Apperture_Area = PTC_RHX5.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating5_SF_Pressure_drop = PTC_RHX5.Total_Pressure_DropResult;

                            //REHEATING SOLAR FIELD CALCULATION
                            LF_SF_Calculation LF_RHX5 = new LF_SF_Calculation();
                            LF_RHX5.calledForSensingAnalysis = true;
                            LF_RHX5.comboBox1.Text = "Solar Salt";
                            LF_RHX5.comboBox2.Text = "NewMixture";
                            LF_RHX5.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX5.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX5.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX5);
                            LF_RHX5.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX5.textBox3.Text = Convert.ToString(puntero_aplicacion.temp221);
                            LF_RHX5.textBox6.Text = Convert.ToString(puntero_aplicacion.temp222);
                            LF_RHX5.textBox4.Text = Convert.ToString(puntero_aplicacion.pres221);
                            LF_RHX5.textBox5.Text = Convert.ToString(puntero_aplicacion.pres222);
                            LF_RHX5.textBox107.Text = Convert.ToString(10);
                            LF_RHX5.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating5_SF_Effective_Apperture_Area = LF_RHX5.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating5_SF_Pressure_drop = LF_RHX5.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //PC_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = p_mc1_in2_list[maxIndex].ToString();
                            //PC_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = p_mc1_out2_list[maxIndex].ToString();
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = recomp_frac2_list[maxIndex].ToString();
                            //P_rhx1_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_rhx1_in2_list[maxIndex].ToString();
                            //P_rhx2_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = p_rhx2_in2_list[maxIndex].ToString();
                            //P_rhx3_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = p_rhx3_in2_list[maxIndex].ToString();
                            //P_rhx4_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = p_rhx4_in2_list[maxIndex].ToString();
                            //P_rhx5_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = p_rhx5_in2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloRCMCIwithFiveReheating.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = cicloRCMCIwithFiveReheating.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 17] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 18] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 19] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            //PTC_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 21] = puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 22] = puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop.ToString();
                            //LF_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 23] = puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 24] = puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop.ToString();
                            //PTC_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 25] = puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 26] = puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop.ToString();
                            //LF_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 27] = puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 28] = puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop.ToString();
                            //PTC_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 29] = puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 30] = puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop.ToString();
                            //LF_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 31] = puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 32] = puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop.ToString();
                            //PTC_RHX4_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 33] = puntero_aplicacion.PTC_ReHeating4_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX4_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 34] = puntero_aplicacion.PTC_ReHeating4_SF_Pressure_drop.ToString();
                            //LF_RHX4_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 35] = puntero_aplicacion.LF_ReHeating4_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX4_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 36] = puntero_aplicacion.LF_ReHeating4_SF_Pressure_drop.ToString();
                            //PTC_RHX5_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 37] = puntero_aplicacion.PTC_ReHeating5_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX5_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 38] = puntero_aplicacion.PTC_ReHeating5_SF_Pressure_drop.ToString();
                            //LF_RHX5_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 39] = puntero_aplicacion.LF_ReHeating5_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX5_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 40] = puntero_aplicacion.LF_ReHeating5_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_mc1_in_value = puntero_aplicacion.p_mc1_in2;
                            initial_mc1_out_value = puntero_aplicacion.p_mc1_out2;
                        }
                    }
















                }
            }

            //Closing Excel Book
            xlWorkBook1.SaveAs(textBox4.Text + "CIT_Optimization_RCMCI_with_Five_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }
    }
}
