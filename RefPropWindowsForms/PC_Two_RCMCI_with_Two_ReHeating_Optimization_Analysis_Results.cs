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
    public partial class PC_Two_RCMCI_with_Two_ReHeating_Optimization_Analysis_Results : Form
    {
        PC_Two_RCMCI_with_Two_Reheating puntero_aplicacion;

        public PC_Two_RCMCI_with_Two_ReHeating_Optimization_Analysis_Results(PC_Two_RCMCI_with_Two_Reheating puntero1)
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

            //Optimization UA false
            if (checkBox2.Checked == false)
            {
                //PureFluid
                if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                //Store Input Data from Graphical User Interface GUI into variables
                puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox1.Text);

                puntero_aplicacion.t_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                puntero_aplicacion.t_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                puntero_aplicacion.p_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox117.Text);

                puntero_aplicacion.t_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox125.Text);
                puntero_aplicacion.p_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox126.Text);

                puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);

                puntero_aplicacion.p_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc4_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                puntero_aplicacion.p_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc3_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);
                puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox87.Text);

                puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.m_eta_mc4 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.m_eta_mc3 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox98.Text);
                puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox115.Text);

                puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                puntero_aplicacion.dp_pc4 = Convert.ToDouble(puntero_aplicacion.textBox99.Text);

                puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox124.Text);

                puntero_aplicacion.eta_thermal = 0.0;

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PC_Two_RCMCI_with_Two_Reheating cicloPC_Two_RCMCI_with_Two_Reheating = new core.PC_Two_RCMCI_with_Two_Reheating();

                double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_mc4_in2_list = new List<Double>();
                List<Double> p_mc4_out2_list = new List<Double>();
                List<Double> p_mc3_in2_list = new List<Double>();
                List<Double> p_mc3_out2_list = new List<Double>();
                List<Double> p_mc2_in2_list = new List<Double>();
                List<Double> p_mc2_out2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox70.Text;
                xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                xlWorkSheet1.Cells[2, 1] = "";
                xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                xlWorkSheet1.Cells[3, 1] = "";
                xlWorkSheet1.Cells[3, 2] = "";
                xlWorkSheet1.Cells[4, 3] = "";

                xlWorkSheet1.Cells[4, 1] = "PC_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "PC_out(kPa)";
                xlWorkSheet1.Cells[4, 3] = "MC4_in(kPa)";
                xlWorkSheet1.Cells[4, 4] = "MC4_out(kPa)";
                xlWorkSheet1.Cells[4, 5] = "MC3_in(kPa)";
                xlWorkSheet1.Cells[4, 6] = "MC3_out(kPa)";
                xlWorkSheet1.Cells[4, 7] = "MC2_in(kPa)";
                xlWorkSheet1.Cells[4, 8] = "MC2_out(kPa)";
                xlWorkSheet1.Cells[4, 9] = "P_rhx1_in(kPa)";
                xlWorkSheet1.Cells[4, 10] = "P_rhx2_in(kPa)";
                xlWorkSheet1.Cells[4, 11] = "CIT(K)";
                xlWorkSheet1.Cells[4, 12] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 13] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 14] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 15] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 16] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 17] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 18] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 19] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 7, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500, initial_CIP_value + 1000, initial_CIP_value + 1500, initial_CIP_value + 6000, initial_CIP_value + 5000});
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out});

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 250.0, 1000.0, 1000.0});

                    var initialValue = new[] { 0.2, initial_CIP_value + 1500.0, initial_CIP_value + 2500.0, initial_CIP_value + 3500.0, initial_CIP_value + 4500.0, 17000.0, 14000.0};

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PC_Two_RCMCI_with_Two_Reheating(puntero_aplicacion.luis,
                        ref cicloPC_Two_RCMCI_with_Two_Reheating, puntero_aplicacion.w_dot_net, puntero_aplicacion.t_pc_in,
                        puntero_aplicacion.t_mc4_in, puntero_aplicacion.t_mc3_in, puntero_aplicacion.t_mc2_in,
                        puntero_aplicacion.t_t_in, puntero_aplicacion.t_trh1_in, variables[5], 
                        puntero_aplicacion.t_trh2_in, variables[6], variables[1], variables[2], variables[2],
                        variables[3], variables[3], variables[4], variables[4], puntero_aplicacion.p_mc2_out,
                        puntero_aplicacion.ua_lt, puntero_aplicacion.ua_ht, puntero_aplicacion.m_eta_mc4, 
                        puntero_aplicacion.m_eta_pc, puntero_aplicacion.m_eta_rc, puntero_aplicacion.m_eta_mc3,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1, 
                        puntero_aplicacion.m_eta_trh2, puntero_aplicacion.n_sub_hxrs, variables[0], 
                        puntero_aplicacion.tol, puntero_aplicacion.eta_thermal, -puntero_aplicacion.dp_lt1, 
                        -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2, 
                        -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc4, -puntero_aplicacion.dp_pc3, 
                        -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_phx, -puntero_aplicacion.dp_rhx1,
                        -puntero_aplicacion.dp_rhx2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPC_Two_RCMCI_with_Two_Reheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPC_Two_RCMCI_with_Two_Reheating.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPC_Two_RCMCI_with_Two_Reheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in = variables[1];
                        puntero_aplicacion.p_pc_out = variables[2];
                        puntero_aplicacion.p_mc4_in = variables[2];
                        puntero_aplicacion.p_mc4_out = variables[3];
                        puntero_aplicacion.p_mc3_in = variables[3];
                        puntero_aplicacion.p_mc3_out = variables[4];
                        puntero_aplicacion.p_mc2_in = variables[4];
                        puntero_aplicacion.p_trh1_in = variables[5];
                        puntero_aplicacion.p_trh2_in = variables[6];

                        puntero_aplicacion.temp21 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[0];
                        puntero_aplicacion.temp22 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                        puntero_aplicacion.temp23 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                        puntero_aplicacion.temp24 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                        puntero_aplicacion.temp25 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                        puntero_aplicacion.temp26 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[5];
                        puntero_aplicacion.temp27 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6];
                        puntero_aplicacion.temp28 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7];
                        puntero_aplicacion.temp29 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8];
                        puntero_aplicacion.temp210 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[9];
                        puntero_aplicacion.temp211 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[10];
                        puntero_aplicacion.temp212 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[11];
                        puntero_aplicacion.temp213 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[12];
                        puntero_aplicacion.temp214 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[13];
                        puntero_aplicacion.temp215 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[14];
                        puntero_aplicacion.temp216 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[15];
                        puntero_aplicacion.temp217 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[16];
                        puntero_aplicacion.temp218 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[17];
                        puntero_aplicacion.temp219 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[18];
                        puntero_aplicacion.temp220 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[19];

                        puntero_aplicacion.pres21 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[0];
                        puntero_aplicacion.pres22 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[1];
                        puntero_aplicacion.pres23 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[2];
                        puntero_aplicacion.pres24 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[3];
                        puntero_aplicacion.pres25 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[4];
                        puntero_aplicacion.pres26 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[5];
                        puntero_aplicacion.pres27 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[6];
                        puntero_aplicacion.pres28 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[7];
                        puntero_aplicacion.pres29 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[8];
                        puntero_aplicacion.pres210 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[9];
                        puntero_aplicacion.pres211 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[10];
                        puntero_aplicacion.pres212 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[11];
                        puntero_aplicacion.pres213 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[12];
                        puntero_aplicacion.pres214 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[13];
                        puntero_aplicacion.pres215 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[14];
                        puntero_aplicacion.pres216 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[15];
                        puntero_aplicacion.pres217 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[16];
                        puntero_aplicacion.pres218 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[17];
                        puntero_aplicacion.pres219 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[18];
                        puntero_aplicacion.pres220 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[19];

                        puntero_aplicacion.PHX = cicloPC_Two_RCMCI_with_Two_Reheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC3.Q_dot;
                        puntero_aplicacion.PC4 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC4.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                        p_mc4_in2_list.Add(puntero_aplicacion.p_mc4_in);
                        p_mc4_out2_list.Add(puntero_aplicacion.p_mc4_out);
                        p_mc3_in2_list.Add(puntero_aplicacion.p_mc3_in);
                        p_mc3_out2_list.Add(puntero_aplicacion.p_mc3_out);
                        p_mc2_in2_list.Add(puntero_aplicacion.p_mc2_in);
                        p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_trh1_in);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_trh2_in);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc4_in.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_mc4_out.ToString());
                        listBox24.Items.Add(puntero_aplicacion.p_mc3_in.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_mc3_out.ToString());
                        listBox26.Items.Add(puntero_aplicacion.p_mc2_in.ToString());
                        listBox25.Items.Add(puntero_aplicacion.p_mc2_out.ToString());
                        listBox31.Items.Add(puntero_aplicacion.p_trh1_in.ToString());
                        listBox32.Items.Add(puntero_aplicacion.p_trh2_in.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                        double LTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                        double LTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                        double HTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                        //MC4_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc4_in);
                        //MC4_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_mc4_out);
                        //MC3_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_mc3_in);
                        //MC3_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.p_mc3_out);
                        //MC2_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.p_mc2_in);
                        //MC2_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.p_mc2_out);
                        //P_rhx1_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = Convert.ToString(puntero_aplicacion.p_trh1_in);
                        //P_rhx2_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = Convert.ToString(puntero_aplicacion.p_trh2_in);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = Convert.ToString(puntero_aplicacion.t_mc4_in - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = Convert.ToString(puntero_aplicacion.ua_lt);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = Convert.ToString(puntero_aplicacion.ua_ht);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 15] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 16] = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 17] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 18] = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 19] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                    textBox5.Text = p_mc4_in2_list[maxIndex].ToString();
                    textBox6.Text = p_mc4_out2_list[maxIndex].ToString();
                    textBox7.Text = p_mc3_in2_list[maxIndex].ToString();
                    textBox8.Text = p_mc3_out2_list[maxIndex].ToString();
                    textBox13.Text = p_mc2_in2_list[maxIndex].ToString();
                    textBox12.Text = p_mc2_out2_list[maxIndex].ToString();
                    textBox14.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox15.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox3.Text = p_mc4_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc4_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox23.Text = p_mc3_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox22.Text = p_mc3_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox88.Text = p_mc2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox87.Text = p_mc2_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox117.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox126.Text = p_rhx2_in2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "PC_Two_RCMCI_with_Two_RH" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }

            //Optimization UA true
            else if (checkBox2.Checked == true)
            {
                //PureFluid
                if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                //Store Input Data from Graphical User Interface GUI into variables
                puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox1.Text);

                puntero_aplicacion.t_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                puntero_aplicacion.t_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                puntero_aplicacion.p_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox117.Text);

                puntero_aplicacion.t_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox125.Text);
                puntero_aplicacion.p_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox126.Text);

                puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);

                puntero_aplicacion.p_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc4_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                puntero_aplicacion.p_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc3_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);
                puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox87.Text);

                puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.m_eta_mc4 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.m_eta_mc3 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox98.Text);
                puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox115.Text);

                puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                puntero_aplicacion.dp_pc4 = Convert.ToDouble(puntero_aplicacion.textBox99.Text);

                puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox124.Text);

                puntero_aplicacion.eta_thermal = 0.0;

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PC_Two_RCMCI_with_Two_Reheating cicloPC_Two_RCMCI_with_Two_Reheating = new core.PC_Two_RCMCI_with_Two_Reheating();

                double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_mc4_in2_list = new List<Double>();
                List<Double> p_mc4_out2_list = new List<Double>();
                List<Double> p_mc3_in2_list = new List<Double>();
                List<Double> p_mc3_out2_list = new List<Double>();
                List<Double> p_mc2_in2_list = new List<Double>();
                List<Double> p_mc2_out2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox70.Text;
                xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                xlWorkSheet1.Cells[2, 1] = "";
                xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                xlWorkSheet1.Cells[3, 1] = "";
                xlWorkSheet1.Cells[3, 2] = "";
                xlWorkSheet1.Cells[4, 3] = "";

                xlWorkSheet1.Cells[4, 1] = "PC_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "PC_out(kPa)";
                xlWorkSheet1.Cells[4, 3] = "MC4_in(kPa)";
                xlWorkSheet1.Cells[4, 4] = "MC4_out(kPa)";
                xlWorkSheet1.Cells[4, 5] = "MC3_in(kPa)";
                xlWorkSheet1.Cells[4, 6] = "MC3_out(kPa)";
                xlWorkSheet1.Cells[4, 7] = "MC2_in(kPa)";
                xlWorkSheet1.Cells[4, 8] = "MC2_out(kPa)";
                xlWorkSheet1.Cells[4, 9] = "P_rhx1_in(kPa)";
                xlWorkSheet1.Cells[4, 10] = "P_rhx2_in(kPa)";
                xlWorkSheet1.Cells[4, 11] = "CIT(K)";
                xlWorkSheet1.Cells[4, 12] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 13] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 14] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 15] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 16] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 17] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 18] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 19] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 8, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500, 
                    initial_CIP_value + 1000, initial_CIP_value + 1500, initial_CIP_value + 5000, 
                    0.0, initial_CIP_value + 4000 });
                    
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, 
                    puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, 
                    puntero_aplicacion.p_mc2_out, 1.0, puntero_aplicacion.p_mc2_out});

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 250.0, 1000.0, 0.05, 1000.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value + 1500.0, initial_CIP_value + 2500.0,
                    initial_CIP_value + 3500.0, initial_CIP_value + 4500.0, 17000.0, 0.5, 14000.0};

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PC_Two_RCMCI_with_Two_Reheating_for_optimization(puntero_aplicacion.luis,
                        ref cicloPC_Two_RCMCI_with_Two_Reheating, puntero_aplicacion.w_dot_net, puntero_aplicacion.t_pc_in,
                        puntero_aplicacion.t_mc4_in, puntero_aplicacion.t_mc3_in, puntero_aplicacion.t_mc2_in,
                        puntero_aplicacion.t_t_in, puntero_aplicacion.t_trh1_in, variables[5], puntero_aplicacion.t_trh2_in, variables[7],
                        variables[1], variables[2], variables[2], variables[3], variables[3], variables[4],
                        variables[4], puntero_aplicacion.p_mc2_out, variables[6], UA_Total, puntero_aplicacion.m_eta_mc4, puntero_aplicacion.m_eta_pc,
                        puntero_aplicacion.m_eta_rc, puntero_aplicacion.m_eta_mc3, puntero_aplicacion.m_eta_mc2,
                        puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1, puntero_aplicacion.m_eta_trh2,
                        puntero_aplicacion.n_sub_hxrs, variables[0], puntero_aplicacion.tol, puntero_aplicacion.eta_thermal,
                        -puntero_aplicacion.dp_lt1, -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1,
                        -puntero_aplicacion.dp_ht2, -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc4,
                        -puntero_aplicacion.dp_pc3, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_phx,
                        -puntero_aplicacion.dp_rhx1, -puntero_aplicacion.dp_rhx2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPC_Two_RCMCI_with_Two_Reheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPC_Two_RCMCI_with_Two_Reheating.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPC_Two_RCMCI_with_Two_Reheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in = variables[1];
                        puntero_aplicacion.p_pc_out = variables[2];
                        puntero_aplicacion.p_mc4_in = variables[2];
                        puntero_aplicacion.p_mc4_out = variables[3];
                        puntero_aplicacion.p_mc3_in = variables[3];
                        puntero_aplicacion.p_mc3_out = variables[4];
                        puntero_aplicacion.p_mc2_in = variables[4];
                        puntero_aplicacion.p_trh1_in = variables[5];
                        LT_fraction = variables[6];
                        puntero_aplicacion.ua_lt = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht = UA_Total * (1 - LT_fraction);
                        puntero_aplicacion.p_trh2_in = variables[7];

                        puntero_aplicacion.temp21 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[0];
                        puntero_aplicacion.temp22 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                        puntero_aplicacion.temp23 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                        puntero_aplicacion.temp24 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                        puntero_aplicacion.temp25 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                        puntero_aplicacion.temp26 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[5];
                        puntero_aplicacion.temp27 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6];
                        puntero_aplicacion.temp28 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7];
                        puntero_aplicacion.temp29 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8];
                        puntero_aplicacion.temp210 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[9];
                        puntero_aplicacion.temp211 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[10];
                        puntero_aplicacion.temp212 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[11];
                        puntero_aplicacion.temp213 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[12];
                        puntero_aplicacion.temp214 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[13];
                        puntero_aplicacion.temp215 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[14];
                        puntero_aplicacion.temp216 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[15];
                        puntero_aplicacion.temp217 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[16];
                        puntero_aplicacion.temp218 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[17];
                        puntero_aplicacion.temp219 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[18];
                        puntero_aplicacion.temp220 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[19];

                        puntero_aplicacion.pres21 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[0];
                        puntero_aplicacion.pres22 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[1];
                        puntero_aplicacion.pres23 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[2];
                        puntero_aplicacion.pres24 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[3];
                        puntero_aplicacion.pres25 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[4];
                        puntero_aplicacion.pres26 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[5];
                        puntero_aplicacion.pres27 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[6];
                        puntero_aplicacion.pres28 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[7];
                        puntero_aplicacion.pres29 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[8];
                        puntero_aplicacion.pres210 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[9];
                        puntero_aplicacion.pres211 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[10];
                        puntero_aplicacion.pres212 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[11];
                        puntero_aplicacion.pres213 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[12];
                        puntero_aplicacion.pres214 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[13];
                        puntero_aplicacion.pres215 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[14];
                        puntero_aplicacion.pres216 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[15];
                        puntero_aplicacion.pres217 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[16];
                        puntero_aplicacion.pres218 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[17];

                        puntero_aplicacion.PHX = cicloPC_Two_RCMCI_with_Two_Reheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC3.Q_dot;
                        puntero_aplicacion.PC4 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC4.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                        p_mc4_in2_list.Add(puntero_aplicacion.p_mc4_in);
                        p_mc4_out2_list.Add(puntero_aplicacion.p_mc4_out);
                        p_mc3_in2_list.Add(puntero_aplicacion.p_mc3_in);
                        p_mc3_out2_list.Add(puntero_aplicacion.p_mc3_out);
                        p_mc2_in2_list.Add(puntero_aplicacion.p_mc2_in);
                        p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_trh1_in);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_trh2_in);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc4_in.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_mc4_out.ToString());
                        listBox24.Items.Add(puntero_aplicacion.p_mc3_in.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_mc3_out.ToString());
                        listBox26.Items.Add(puntero_aplicacion.p_mc2_in.ToString());
                        listBox25.Items.Add(puntero_aplicacion.p_mc2_out.ToString());
                        listBox31.Items.Add(puntero_aplicacion.p_trh1_in.ToString());
                        listBox32.Items.Add(puntero_aplicacion.p_trh2_in.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                        double LTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                        double LTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                        double HTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                        //MC4_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc4_in);
                        //MC4_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_mc4_out);
                        //MC3_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_mc3_in);
                        //MC3_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.p_mc3_out);
                        //MC2_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.p_mc2_in);
                        //MC2_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.p_mc2_out);
                        //P_rhx1_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = Convert.ToString(puntero_aplicacion.p_trh1_in);
                        //P_rhx2_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = Convert.ToString(puntero_aplicacion.p_trh2_in);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = Convert.ToString(puntero_aplicacion.t_mc4_in - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = Convert.ToString(puntero_aplicacion.ua_lt);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = Convert.ToString(puntero_aplicacion.ua_ht);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 15] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 16] = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 17] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 18] = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 19] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                    textBox5.Text = p_mc4_in2_list[maxIndex].ToString();
                    textBox6.Text = p_mc4_out2_list[maxIndex].ToString();
                    textBox7.Text = p_mc3_in2_list[maxIndex].ToString();
                    textBox8.Text = p_mc3_out2_list[maxIndex].ToString();
                    textBox13.Text = p_mc2_in2_list[maxIndex].ToString();
                    textBox12.Text = p_mc2_out2_list[maxIndex].ToString();
                    textBox14.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox15.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox3.Text = p_mc4_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc4_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox23.Text = p_mc3_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox22.Text = p_mc3_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox88.Text = p_mc2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox87.Text = p_mc2_out2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox117.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox126.Text = p_rhx2_in2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "PC_Two_RCMCI_with_Two_RH" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }
        }

        //Run CIT Optimizations
        private void button7_Click(object sender, EventArgs e)
        {
            int counter = 0;

            double initial_pc_in_value = 0;
            double initial_pc_out_value = 0;
            double initial_mc4_out_value = 0;
            double initial_mc3_out_value = 0;
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

            //xlWorkSheet1 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(xlWorkBook1.Worksheets.Count);    

            double initial_CIP_value = 0;

            //Loop for UA optimization
            for (double j = Convert.ToDouble(textBox11.Text); j <= Convert.ToDouble(textBox10.Text); j = j + Convert.ToDouble(textBox9.Text))
            {
                puntero_aplicacion.ua_lt = j / 2;
                puntero_aplicacion.ua_ht = j / 2;

                //Loop for CIT optimization
                //for (double i = 305.15; i <= 335.15; i = i + 5)
                for (double i = Convert.ToDouble(textBox57.Text); i <= Convert.ToDouble(textBox56.Text); i = i + Convert.ToDouble(textBox55.Text))
                {
                    counter = 0;

                    //Optimization UA false
                    if (checkBox2.Checked == false)
                    {
                        //PureFluid
                        if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text, puntero_aplicacion.category);
                        }

                        //NewMixture
                        if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                        //Store Input Data from Graphical User Interface GUI into variables
                        puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox1.Text);

                        puntero_aplicacion.t_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                        puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                        puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                        puntero_aplicacion.t_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                        puntero_aplicacion.p_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox117.Text);

                        puntero_aplicacion.t_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox125.Text);
                        puntero_aplicacion.p_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox126.Text);

                        puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                        puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);

                        puntero_aplicacion.p_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc4_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                        puntero_aplicacion.p_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc3_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                        puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);
                        puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox87.Text);

                        //puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                        puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                        puntero_aplicacion.m_eta_mc4 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.m_eta_mc3 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox98.Text);
                        puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                        puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox115.Text);

                        puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                        puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                        puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                        puntero_aplicacion.dp_pc4 = Convert.ToDouble(puntero_aplicacion.textBox99.Text);

                        puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                        puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox124.Text);

                        puntero_aplicacion.eta_thermal = 0.0;

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.PC_Two_RCMCI_with_Two_Reheating cicloPC_Two_RCMCI_with_Two_Reheating = new core.PC_Two_RCMCI_with_Two_Reheating();

                        double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_pc_in2_list = new List<Double>();
                        List<Double> p_pc_out2_list = new List<Double>();
                        List<Double> p_mc4_in2_list = new List<Double>();
                        List<Double> p_mc4_out2_list = new List<Double>();
                        List<Double> p_mc3_in2_list = new List<Double>();
                        List<Double> p_mc3_out2_list = new List<Double>();
                        List<Double> p_mc2_in2_list = new List<Double>();
                        List<Double> p_mc2_out2_list = new List<Double>();
                        List<Double> p_rhx1_in2_list = new List<Double>();
                        List<Double> p_rhx2_in2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
                        List<Double> ua_LT_list = new List<Double>();
                        List<Double> ua_HT_list = new List<Double>();

                        List<Double> PHX_Q2_list = new List<Double>();
                        List<Double> RHX1_Q2_list = new List<Double>();
                        List<Double> RHX2_Q2_list = new List<Double>();

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

                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox70.Text;
                            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                            xlWorkSheet1.Cells[2, 1] = "";
                            xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                            xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                            xlWorkSheet1.Cells[3, 1] = "";
                            xlWorkSheet1.Cells[3, 2] = "";
                            xlWorkSheet1.Cells[4, 3] = "";

                            xlWorkSheet1.Cells[4, 1] = "PC_in(kPa)";
                            xlWorkSheet1.Cells[4, 2] = "PC_out(kPa)";
                            xlWorkSheet1.Cells[4, 3] = "MC4_in(kPa)";
                            xlWorkSheet1.Cells[4, 4] = "MC4_out(kPa)";
                            xlWorkSheet1.Cells[4, 5] = "MC3_in(kPa)";
                            xlWorkSheet1.Cells[4, 6] = "MC3_out(kPa)";
                            xlWorkSheet1.Cells[4, 7] = "MC2_in(kPa)";
                            xlWorkSheet1.Cells[4, 8] = "MC2_out(kPa)";
                            xlWorkSheet1.Cells[4, 9] = "P_rhx1_in(kPa)";
                            xlWorkSheet1.Cells[4, 10] = "P_rhx2_in(kPa)";
                            xlWorkSheet1.Cells[4, 11] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 12] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 13] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 14] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 15] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 16] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 17] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 18] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 19] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 20] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 21] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 22] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 23] = "LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 24] = "PTC_rhx1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 25] = "PTC_rhx1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 26] = "LF_rhx1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 27] = "LF_rhx1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 28] = "PTC_rhx2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 29] = "PTC_rhx2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 30] = "LF_rhx2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 31] = "LF_rhx2_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 7, 0.000001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500, initial_CIP_value + 1000, initial_CIP_value + 1500, initial_CIP_value + 6000, initial_CIP_value + 5000 });
                            solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 250.0, 1000.0, 1000.0 });

                            var initialValue = new[] { 0.2, initial_CIP_value + 1500.0, initial_CIP_value + 2500.0, initial_CIP_value + 3500.0, initial_CIP_value + 4500.0, 17000.0, 14000.0 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PC_Two_RCMCI_with_Two_Reheating(puntero_aplicacion.luis,
                                ref cicloPC_Two_RCMCI_with_Two_Reheating, puntero_aplicacion.w_dot_net, i, i, i, i,
                                puntero_aplicacion.t_t_in, puntero_aplicacion.t_trh1_in, variables[5],
                                puntero_aplicacion.t_trh2_in, variables[6], variables[1], variables[2], variables[2],
                                variables[3], variables[3], variables[4], variables[4], puntero_aplicacion.p_mc2_out,
                                puntero_aplicacion.ua_lt, puntero_aplicacion.ua_ht, puntero_aplicacion.m_eta_mc4,
                                puntero_aplicacion.m_eta_pc, puntero_aplicacion.m_eta_rc, puntero_aplicacion.m_eta_mc3,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1,
                                puntero_aplicacion.m_eta_trh2, puntero_aplicacion.n_sub_hxrs, variables[0],
                                puntero_aplicacion.tol, puntero_aplicacion.eta_thermal, -puntero_aplicacion.dp_lt1,
                                -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2,
                                -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc4, -puntero_aplicacion.dp_pc3,
                                -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_phx, -puntero_aplicacion.dp_rhx1,
                                -puntero_aplicacion.dp_rhx2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPC_Two_RCMCI_with_Two_Reheating.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPC_Two_RCMCI_with_Two_Reheating.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPC_Two_RCMCI_with_Two_Reheating.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in = variables[1];
                                puntero_aplicacion.p_pc_out = variables[2];
                                puntero_aplicacion.p_mc4_in = variables[2];
                                puntero_aplicacion.p_mc4_out = variables[3];
                                puntero_aplicacion.p_mc3_in = variables[3];
                                puntero_aplicacion.p_mc3_out = variables[4];
                                puntero_aplicacion.p_mc2_in = variables[4];
                                puntero_aplicacion.p_trh1_in = variables[5];
                                puntero_aplicacion.p_trh2_in = variables[6];

                                puntero_aplicacion.temp21 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[0];
                                puntero_aplicacion.temp22 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                                puntero_aplicacion.temp23 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                                puntero_aplicacion.temp24 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                                puntero_aplicacion.temp25 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                                puntero_aplicacion.temp26 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[5];
                                puntero_aplicacion.temp27 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6];
                                puntero_aplicacion.temp28 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7];
                                puntero_aplicacion.temp29 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8];
                                puntero_aplicacion.temp210 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[9];
                                puntero_aplicacion.temp211 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[10];
                                puntero_aplicacion.temp212 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[11];
                                puntero_aplicacion.temp213 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[12];
                                puntero_aplicacion.temp214 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[13];
                                puntero_aplicacion.temp215 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[14];
                                puntero_aplicacion.temp216 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[15];
                                puntero_aplicacion.temp217 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[16];
                                puntero_aplicacion.temp218 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[17];
                                puntero_aplicacion.temp219 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[18];
                                puntero_aplicacion.temp220 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[19];

                                puntero_aplicacion.pres21 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[0];
                                puntero_aplicacion.pres22 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[1];
                                puntero_aplicacion.pres23 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[2];
                                puntero_aplicacion.pres24 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[3];
                                puntero_aplicacion.pres25 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[4];
                                puntero_aplicacion.pres26 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[5];
                                puntero_aplicacion.pres27 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[6];
                                puntero_aplicacion.pres28 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[7];
                                puntero_aplicacion.pres29 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[8];
                                puntero_aplicacion.pres210 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[9];
                                puntero_aplicacion.pres211 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[10];
                                puntero_aplicacion.pres212 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[11];
                                puntero_aplicacion.pres213 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[12];
                                puntero_aplicacion.pres214 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[13];
                                puntero_aplicacion.pres215 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[14];
                                puntero_aplicacion.pres216 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[15];
                                puntero_aplicacion.pres217 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[16];
                                puntero_aplicacion.pres218 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[17];
                                puntero_aplicacion.pres219 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[18];
                                puntero_aplicacion.pres220 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[19];

                                puntero_aplicacion.PHX = cicloPC_Two_RCMCI_with_Two_Reheating.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC1.Q_dot;
                                puntero_aplicacion.PC2 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC2.Q_dot;
                                puntero_aplicacion.PC3 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC3.Q_dot;
                                puntero_aplicacion.PC4 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC4.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                                p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                                p_mc4_in2_list.Add(puntero_aplicacion.p_mc4_in);
                                p_mc4_out2_list.Add(puntero_aplicacion.p_mc4_out);
                                p_mc3_in2_list.Add(puntero_aplicacion.p_mc3_in);
                                p_mc3_out2_list.Add(puntero_aplicacion.p_mc3_out);
                                p_mc2_in2_list.Add(puntero_aplicacion.p_mc2_in);
                                p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_trh1_in);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_trh2_in);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht);

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

                                PHX_Q2_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.PHX.Q_dot);
                                RHX1_Q2_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.RHX1.Q_dot);
                                RHX2_Q2_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.RHX2.Q_dot);

                                HT_Eff_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff);
                                LT_Eff_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                                listBox20.Items.Add(puntero_aplicacion.p_mc4_in.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_mc4_out.ToString());
                                listBox24.Items.Add(puntero_aplicacion.p_mc3_in.ToString());
                                listBox23.Items.Add(puntero_aplicacion.p_mc3_out.ToString());
                                listBox26.Items.Add(puntero_aplicacion.p_mc2_in.ToString());
                                listBox25.Items.Add(puntero_aplicacion.p_mc2_out.ToString());
                                listBox31.Items.Add(puntero_aplicacion.p_trh1_in.ToString());
                                listBox32.Items.Add(puntero_aplicacion.p_trh2_in.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                                //double LTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                                //double LTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                                //double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                //double HTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                                //double HTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                                //double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                                ////MC4_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc4_in);
                                ////MC4_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_mc4_out);
                                ////MC3_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_mc3_in);
                                ////MC3_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.p_mc3_out);
                                ////MC2_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.p_mc2_in);
                                ////MC2_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.p_mc2_out);
                                ////P_rhx1_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = Convert.ToString(puntero_aplicacion.p_trh1_in);
                                ////P_rhx2_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = Convert.ToString(puntero_aplicacion.p_trh2_in);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = Convert.ToString(puntero_aplicacion.t_mc4_in - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = Convert.ToString(puntero_aplicacion.ua_lt);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 13] = Convert.ToString(puntero_aplicacion.ua_ht);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.recomp_frac2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 15] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 16] = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 17] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 18] = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 19] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list.Max();

                            var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                            textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                            textBox5.Text = p_mc4_in2_list[maxIndex].ToString();
                            textBox6.Text = p_mc4_out2_list[maxIndex].ToString();
                            textBox7.Text = p_mc3_in2_list[maxIndex].ToString();
                            textBox8.Text = p_mc3_out2_list[maxIndex].ToString();
                            textBox13.Text = p_mc2_in2_list[maxIndex].ToString();
                            textBox12.Text = p_mc2_out2_list[maxIndex].ToString();
                            textBox14.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox15.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox3.Text = p_mc4_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc4_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox23.Text = p_mc3_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox22.Text = p_mc3_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox88.Text = p_mc2_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox87.Text = p_mc2_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox117.Text = p_rhx1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox126.Text = p_rhx2_in2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();

                                puntero_aplicacion.textBox2.Text = i.ToString();
                                puntero_aplicacion.textBox28.Text = i.ToString();
                                puntero_aplicacion.textBox102.Text = i.ToString();
                                puntero_aplicacion.textBox86.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_pc_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_pc_out2_list[maxIndex].ToString());
                            listBox22.Items.Add(p_mc4_in2_list[maxIndex].ToString());
                            listBox21.Items.Add(p_mc4_out2_list[maxIndex].ToString());
                            listBox30.Items.Add(p_mc3_in2_list[maxIndex].ToString());
                            listBox29.Items.Add(p_mc3_out2_list[maxIndex].ToString());
                            listBox28.Items.Add(p_mc2_in2_list[maxIndex].ToString());
                            listBox27.Items.Add(p_mc2_out2_list[maxIndex].ToString());
                            listBox34.Items.Add(p_rhx1_in2_list[maxIndex].ToString());
                            listBox33.Items.Add(p_rhx2_in2_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());

                            //Main Solar Field
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "PureFluid";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
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

                            LF_SF_Calculation LF = new LF_SF_Calculation();
                            LF.calledForSensingAnalysis = true;
                            LF.comboBox1.Text = "Solar Salt";
                            LF.comboBox2.Text = "PureFluid";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
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

                            //Reheating_1 Solar Field
                            PTC_SF_Calculation PTC_rhx1 = new PTC_SF_Calculation();
                            PTC_rhx1.calledForSensingAnalysis = true;
                            PTC_rhx1.comboBox1.Text = "Solar Salt";
                            PTC_rhx1.comboBox2.Text = "PureFluid";
                            PTC_rhx1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_rhx1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;

                            if (comboBox1.Text == "Parabolic")
                            {
                                PTC_rhx1.textBox7.Text = "0.141";
                                PTC_rhx1.textBox8.Text = "6.48e-9";
                            }
                            else if (comboBox1.Text == "Parabolic with cavity receiver (Norwich)")
                            {
                                PTC_rhx1.textBox7.Text = "0.3";
                                PTC_rhx1.textBox8.Text = "3.25e-9";
                            }

                            PTC_rhx1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            PTC_rhx1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_rhx1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            PTC_rhx1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            PTC_rhx1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            PTC_rhx1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            PTC_rhx1.textBox107.Text = Convert.ToString(10);
                            PTC_rhx1.button1_Click(this, e);
                            puntero_aplicacion.PTC_Reheating1_SF_Effective_Apperture_Area = PTC_rhx1.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Reheating1_SF_Pressure_drop = PTC_rhx1.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_rhx1 = new LF_SF_Calculation();
                            LF_rhx1.calledForSensingAnalysis = true;
                            LF_rhx1.comboBox1.Text = "Solar Salt";
                            LF_rhx1.comboBox2.Text = "PureFluid";
                            LF_rhx1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_rhx1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_rhx1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            LF_rhx1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_rhx1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            LF_rhx1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            LF_rhx1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            LF_rhx1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            LF_rhx1.textBox107.Text = Convert.ToString(10);
                            LF_rhx1.button1_Click(this, e);
                            puntero_aplicacion.LF_Reheating1_SF_Effective_Apperture_Area = LF_rhx1.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Reheating1_SF_Pressure_drop = LF_rhx1.Total_Pressure_DropResult;

                            //Reheating_2 Solar Field
                            PTC_SF_Calculation PTC_rhx2 = new PTC_SF_Calculation();
                            PTC_rhx2.calledForSensingAnalysis = true;
                            PTC_rhx2.comboBox1.Text = "Solar Salt";
                            PTC_rhx2.comboBox2.Text = "PureFluid";
                            PTC_rhx2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_rhx2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;

                            if (comboBox2.Text == "Parabolic")
                            {
                                PTC_rhx2.textBox7.Text = "0.141";
                                PTC_rhx2.textBox8.Text = "6.48e-9";
                            }
                            else if (comboBox2.Text == "Parabolic with cavity receiver (Norwich)")
                            {
                                PTC_rhx2.textBox7.Text = "0.3";
                                PTC_rhx2.textBox8.Text = "3.25e-9";
                            }

                            PTC_rhx2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            PTC_rhx2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_rhx2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            PTC_rhx2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            PTC_rhx2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            PTC_rhx2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            PTC_rhx2.textBox107.Text = Convert.ToString(10);
                            PTC_rhx2.button1_Click(this, e);
                            puntero_aplicacion.PTC_Reheating2_SF_Effective_Apperture_Area = PTC_rhx2.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Reheating2_SF_Pressure_drop = PTC_rhx2.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_rhx2 = new LF_SF_Calculation();
                            LF_rhx2.calledForSensingAnalysis = true;
                            LF_rhx2.comboBox1.Text = "Solar Salt";
                            LF_rhx2.comboBox2.Text = "PureFluid";
                            LF_rhx2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_rhx2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_rhx2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            LF_rhx2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_rhx2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            LF_rhx2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            LF_rhx2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            LF_rhx2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            LF_rhx2.textBox107.Text = Convert.ToString(10);
                            LF_rhx2.button1_Click(this, e);
                            puntero_aplicacion.LF_Reheating2_SF_Effective_Apperture_Area = LF_rhx2.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Reheating2_SF_Pressure_drop = LF_rhx2.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //PC_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = p_pc_in2_list[maxIndex].ToString();
                            //PC_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = p_pc_out2_list[maxIndex].ToString();
                            //MC4_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = p_mc4_in2_list[maxIndex].ToString();
                            //MC4_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = p_mc4_out2_list[maxIndex].ToString();
                            //MC3_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = p_mc3_in2_list[maxIndex].ToString();
                            //MC3_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = p_mc3_out2_list[maxIndex].ToString();
                            //MC2_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_mc2_in2_list[maxIndex].ToString();
                            //MC2_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = p_mc2_out2_list[maxIndex].ToString();
                            //P_rhx1_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = p_rhx1_in2_list[maxIndex].ToString();
                            //P_rhx2_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = p_rhx2_in2_list[maxIndex].ToString();
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = Convert.ToString(puntero_aplicacion.ua_lt);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = Convert.ToString(puntero_aplicacion.ua_ht);
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = recomp_frac2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 17] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 18] = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 19] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 21] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 22] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 23] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            //PTC_rhx1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 24] = puntero_aplicacion.PTC_Reheating1_SF_Effective_Apperture_Area.ToString();
                            //PTC_rhx1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 25] = puntero_aplicacion.PTC_Reheating1_SF_Pressure_drop.ToString();
                            //LF_rhx1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 26] = puntero_aplicacion.LF_Reheating1_SF_Effective_Apperture_Area.ToString();
                            //LF_rhx1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 27] = puntero_aplicacion.LF_Reheating1_SF_Pressure_drop.ToString();
                            //PTC_rhx2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 28] = puntero_aplicacion.PTC_Reheating2_SF_Effective_Apperture_Area.ToString();
                            //PTC_rhx2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 29] = puntero_aplicacion.PTC_Reheating2_SF_Pressure_drop.ToString();
                            //LF_rhx2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 30] = puntero_aplicacion.LF_Reheating2_SF_Effective_Apperture_Area.ToString();
                            //LF_rhx2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 31] = puntero_aplicacion.LF_Reheating2_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_pc_in_value = puntero_aplicacion.p_pc_in;
                            initial_pc_out_value = puntero_aplicacion.p_pc_out;
                            initial_mc4_out_value = puntero_aplicacion.p_mc4_out;
                            initial_mc3_out_value = puntero_aplicacion.p_mc3_out;
                            initial_mc2_out_value = puntero_aplicacion.p_mc2_out;
                        }
                    }

                    //Optimization UA true
                    else if (checkBox2.Checked == true)
                    {
                        //PureFluid
                        if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text, puntero_aplicacion.category);
                        }

                        //NewMixture
                        if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
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

                        //Store Input Data from Graphical User Interface GUI into variables
                        puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox1.Text);

                        puntero_aplicacion.t_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                        puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                        puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                        puntero_aplicacion.t_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                        puntero_aplicacion.p_trh1_in = Convert.ToDouble(puntero_aplicacion.textBox117.Text);

                        puntero_aplicacion.t_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox125.Text);
                        puntero_aplicacion.p_trh2_in = Convert.ToDouble(puntero_aplicacion.textBox126.Text);

                        puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                        puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);

                        puntero_aplicacion.p_mc4_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc4_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                        puntero_aplicacion.p_mc3_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc3_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                        puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);
                        puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox87.Text);

                        //puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                        puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                        puntero_aplicacion.m_eta_mc4 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.m_eta_mc3 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox98.Text);
                        puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox123.Text);
                        puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox115.Text);

                        puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                        puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                        puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);
                        puntero_aplicacion.dp_pc4 = Convert.ToDouble(puntero_aplicacion.textBox99.Text);

                        puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                        puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox124.Text);

                        puntero_aplicacion.eta_thermal = 0.0;

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.PC_Two_RCMCI_with_Two_Reheating cicloPC_Two_RCMCI_with_Two_Reheating = new core.PC_Two_RCMCI_with_Two_Reheating();

                        double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_pc_in2_list = new List<Double>();
                        List<Double> p_pc_out2_list = new List<Double>();
                        List<Double> p_mc4_in2_list = new List<Double>();
                        List<Double> p_mc4_out2_list = new List<Double>();
                        List<Double> p_mc3_in2_list = new List<Double>();
                        List<Double> p_mc3_out2_list = new List<Double>();
                        List<Double> p_mc2_in2_list = new List<Double>();
                        List<Double> p_mc2_out2_list = new List<Double>();
                        List<Double> p_rhx1_in2_list = new List<Double>();
                        List<Double> p_rhx2_in2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
                        List<Double> ua_LT_list = new List<Double>();
                        List<Double> ua_HT_list = new List<Double>();

                        List<Double> PHX_Q2_list = new List<Double>();
                        List<Double> RHX1_Q2_list = new List<Double>();
                        List<Double> RHX2_Q2_list = new List<Double>();

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

                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox70.Text;
                            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                            xlWorkSheet1.Cells[2, 1] = "";
                            xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                            xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                            xlWorkSheet1.Cells[3, 1] = "";
                            xlWorkSheet1.Cells[3, 2] = "";
                            xlWorkSheet1.Cells[4, 3] = "";

                            xlWorkSheet1.Cells[4, 1] = "PC_in(kPa)";
                            xlWorkSheet1.Cells[4, 2] = "PC_out(kPa)";
                            xlWorkSheet1.Cells[4, 3] = "MC4_in(kPa)";
                            xlWorkSheet1.Cells[4, 4] = "MC4_out(kPa)";
                            xlWorkSheet1.Cells[4, 5] = "MC3_in(kPa)";
                            xlWorkSheet1.Cells[4, 6] = "MC3_out(kPa)";
                            xlWorkSheet1.Cells[4, 7] = "MC2_in(kPa)";
                            xlWorkSheet1.Cells[4, 8] = "MC2_out(kPa)";
                            xlWorkSheet1.Cells[4, 9] = "P_rhx1_in(kPa)";
                            xlWorkSheet1.Cells[4, 10] = "P_rhx2_in(kPa)";
                            xlWorkSheet1.Cells[4, 11] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 12] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 13] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 14] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 15] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 16] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 17] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 18] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 19] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 20] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 21] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 22] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 23] = "LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 24] = "PTC_rhx1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 25] = "PTC_rhx1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 26] = "LF_rhx1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 27] = "LF_rhx1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 28] = "PTC_rhx2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 29] = "PTC_rhx2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 30] = "LF_rhx2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 31] = "LF_rhx2_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 8, 0.000001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500,
                            initial_CIP_value + 1000, initial_CIP_value + 1500, initial_CIP_value + 5000,
                            0.0, initial_CIP_value + 4000 });

                            solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out,
                            puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out,
                            puntero_aplicacion.p_mc2_out, 1.0, puntero_aplicacion.p_mc2_out});

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 250.0, 1000.0, 0.05, 1000.0 });

                            var initialValue = new[] { 0.2, initial_CIP_value + 1500.0, initial_CIP_value + 2500.0,
                            initial_CIP_value + 3500.0, initial_CIP_value + 4500.0, 17000.0, 0.5, 14000.0};

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PC_Two_RCMCI_with_Two_Reheating_for_optimization(puntero_aplicacion.luis,
                                ref cicloPC_Two_RCMCI_with_Two_Reheating, puntero_aplicacion.w_dot_net, i, i, i, i,
                                puntero_aplicacion.t_t_in, puntero_aplicacion.t_trh1_in, variables[5], puntero_aplicacion.t_trh2_in, variables[7],
                                variables[1], variables[2], variables[2], variables[3], variables[3], variables[4],
                                variables[4], puntero_aplicacion.p_mc2_out, variables[6], UA_Total, puntero_aplicacion.m_eta_mc4, puntero_aplicacion.m_eta_pc,
                                puntero_aplicacion.m_eta_rc, puntero_aplicacion.m_eta_mc3, puntero_aplicacion.m_eta_mc2,
                                puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1, puntero_aplicacion.m_eta_trh2,
                                puntero_aplicacion.n_sub_hxrs, variables[0], puntero_aplicacion.tol, puntero_aplicacion.eta_thermal,
                                -puntero_aplicacion.dp_lt1, -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1,
                                -puntero_aplicacion.dp_ht2, -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc4,
                                -puntero_aplicacion.dp_pc3, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_phx,
                                -puntero_aplicacion.dp_rhx1, -puntero_aplicacion.dp_rhx2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPC_Two_RCMCI_with_Two_Reheating.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPC_Two_RCMCI_with_Two_Reheating.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPC_Two_RCMCI_with_Two_Reheating.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in = variables[1];
                                puntero_aplicacion.p_pc_out = variables[2];
                                puntero_aplicacion.p_mc4_in = variables[2];
                                puntero_aplicacion.p_mc4_out = variables[3];
                                puntero_aplicacion.p_mc3_in = variables[3];
                                puntero_aplicacion.p_mc3_out = variables[4];
                                puntero_aplicacion.p_mc2_in = variables[4];
                                puntero_aplicacion.p_trh1_in = variables[5];
                                LT_fraction = variables[6];
                                puntero_aplicacion.ua_lt = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht = UA_Total * (1 - LT_fraction);
                                puntero_aplicacion.p_trh2_in = variables[7];

                                puntero_aplicacion.temp21 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[0];
                                puntero_aplicacion.temp22 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                                puntero_aplicacion.temp23 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                                puntero_aplicacion.temp24 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                                puntero_aplicacion.temp25 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                                puntero_aplicacion.temp26 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[5];
                                puntero_aplicacion.temp27 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6];
                                puntero_aplicacion.temp28 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7];
                                puntero_aplicacion.temp29 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8];
                                puntero_aplicacion.temp210 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[9];
                                puntero_aplicacion.temp211 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[10];
                                puntero_aplicacion.temp212 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[11];
                                puntero_aplicacion.temp213 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[12];
                                puntero_aplicacion.temp214 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[13];
                                puntero_aplicacion.temp215 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[14];
                                puntero_aplicacion.temp216 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[15];
                                puntero_aplicacion.temp217 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[16];
                                puntero_aplicacion.temp218 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[17];
                                puntero_aplicacion.temp219 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[18];
                                puntero_aplicacion.temp220 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[19];

                                puntero_aplicacion.pres21 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[0];
                                puntero_aplicacion.pres22 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[1];
                                puntero_aplicacion.pres23 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[2];
                                puntero_aplicacion.pres24 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[3];
                                puntero_aplicacion.pres25 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[4];
                                puntero_aplicacion.pres26 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[5];
                                puntero_aplicacion.pres27 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[6];
                                puntero_aplicacion.pres28 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[7];
                                puntero_aplicacion.pres29 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[8];
                                puntero_aplicacion.pres210 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[9];
                                puntero_aplicacion.pres211 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[10];
                                puntero_aplicacion.pres212 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[11];
                                puntero_aplicacion.pres213 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[12];
                                puntero_aplicacion.pres214 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[13];
                                puntero_aplicacion.pres215 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[14];
                                puntero_aplicacion.pres216 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[15];
                                puntero_aplicacion.pres217 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[16];
                                puntero_aplicacion.pres218 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[17];
                                puntero_aplicacion.pres219 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[18];
                                puntero_aplicacion.pres220 = cicloPC_Two_RCMCI_with_Two_Reheating.pres[19];

                                puntero_aplicacion.PHX = cicloPC_Two_RCMCI_with_Two_Reheating.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPC_Two_RCMCI_with_Two_Reheating.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPC_Two_RCMCI_with_Two_Reheating.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPC_Two_RCMCI_with_Two_Reheating.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPC_Two_RCMCI_with_Two_Reheating.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC1.Q_dot;
                                puntero_aplicacion.PC2 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC2.Q_dot;
                                puntero_aplicacion.PC3 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC3.Q_dot;
                                puntero_aplicacion.PC4 = -cicloPC_Two_RCMCI_with_Two_Reheating.PC4.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                                p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                                p_mc4_in2_list.Add(puntero_aplicacion.p_mc4_in);
                                p_mc4_out2_list.Add(puntero_aplicacion.p_mc4_out);
                                p_mc3_in2_list.Add(puntero_aplicacion.p_mc3_in);
                                p_mc3_out2_list.Add(puntero_aplicacion.p_mc3_out);
                                p_mc2_in2_list.Add(puntero_aplicacion.p_mc2_in);
                                p_mc2_out2_list.Add(puntero_aplicacion.p_mc2_out);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_trh1_in);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_trh2_in);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht);

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

                                PHX_Q2_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.PHX.Q_dot);
                                RHX1_Q2_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.RHX1.Q_dot);
                                RHX2_Q2_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.RHX2.Q_dot);

                                HT_Eff_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff);
                                LT_Eff_list.Add(cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                                listBox20.Items.Add(puntero_aplicacion.p_mc4_in.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_mc4_out.ToString());
                                listBox24.Items.Add(puntero_aplicacion.p_mc3_in.ToString());
                                listBox23.Items.Add(puntero_aplicacion.p_mc3_out.ToString());
                                listBox26.Items.Add(puntero_aplicacion.p_mc2_in.ToString());
                                listBox25.Items.Add(puntero_aplicacion.p_mc2_out.ToString());
                                listBox31.Items.Add(puntero_aplicacion.p_trh1_in.ToString());
                                listBox32.Items.Add(puntero_aplicacion.p_trh2_in.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                                //double LTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[2];
                                //double LTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[8] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[1];
                                //double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                //double HTR_min_DT_1 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[7] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[3];
                                //double HTR_min_DT_2 = cicloPC_Two_RCMCI_with_Two_Reheating.temp[6] - cicloPC_Two_RCMCI_with_Two_Reheating.temp[4];
                                //double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                                ////MC4_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc4_in);
                                ////MC4_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_mc4_out);
                                ////MC3_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_mc3_in);
                                ////MC3_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.p_mc3_out);
                                ////MC2_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.p_mc2_in);
                                ////MC2_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.p_mc2_out);
                                ////P_rhx1_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = Convert.ToString(puntero_aplicacion.p_trh1_in);
                                ////P_rhx2_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = Convert.ToString(puntero_aplicacion.p_trh2_in);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = Convert.ToString(puntero_aplicacion.t_mc4_in - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = Convert.ToString(puntero_aplicacion.ua_lt);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 13] = Convert.ToString(puntero_aplicacion.ua_ht);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.recomp_frac2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 15] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 16] = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 17] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 18] = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 19] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list.Max();

                            var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                            textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                            textBox5.Text = p_mc4_in2_list[maxIndex].ToString();
                            textBox6.Text = p_mc4_out2_list[maxIndex].ToString();
                            textBox7.Text = p_mc3_in2_list[maxIndex].ToString();
                            textBox8.Text = p_mc3_out2_list[maxIndex].ToString();
                            textBox13.Text = p_mc2_in2_list[maxIndex].ToString();
                            textBox12.Text = p_mc2_out2_list[maxIndex].ToString();
                            textBox14.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox15.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox3.Text = p_mc4_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc4_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox23.Text = p_mc3_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox22.Text = p_mc3_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox88.Text = p_mc2_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox87.Text = p_mc2_out2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox117.Text = p_rhx1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox126.Text = p_rhx2_in2_list[maxIndex].ToString();

                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();

                                puntero_aplicacion.textBox2.Text = i.ToString();
                                puntero_aplicacion.textBox28.Text = i.ToString();
                                puntero_aplicacion.textBox102.Text = i.ToString();
                                puntero_aplicacion.textBox86.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_pc_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_pc_out2_list[maxIndex].ToString());
                            listBox22.Items.Add(p_mc4_in2_list[maxIndex].ToString());
                            listBox21.Items.Add(p_mc4_out2_list[maxIndex].ToString());
                            listBox30.Items.Add(p_mc3_in2_list[maxIndex].ToString());
                            listBox29.Items.Add(p_mc3_out2_list[maxIndex].ToString());
                            listBox28.Items.Add(p_mc2_in2_list[maxIndex].ToString());
                            listBox27.Items.Add(p_mc2_out2_list[maxIndex].ToString());
                            listBox34.Items.Add(p_rhx1_in2_list[maxIndex].ToString());
                            listBox33.Items.Add(p_rhx2_in2_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());

                            //Main Solar Field
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "PureFluid";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
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

                            LF_SF_Calculation LF = new LF_SF_Calculation();
                            LF.calledForSensingAnalysis = true;
                            LF.comboBox1.Text = "Solar Salt";
                            LF.comboBox2.Text = "PureFluid";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
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

                            //Reheating_1 Solar Field
                            PTC_SF_Calculation PTC_rhx1 = new PTC_SF_Calculation();
                            PTC_rhx1.calledForSensingAnalysis = true;
                            PTC_rhx1.comboBox1.Text = "Solar Salt";
                            PTC_rhx1.comboBox2.Text = "PureFluid";
                            PTC_rhx1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_rhx1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;

                            if (comboBox1.Text == "Parabolic")
                            {
                                PTC_rhx1.textBox7.Text = "0.141";
                                PTC_rhx1.textBox8.Text = "6.48e-9";
                            }
                            else if (comboBox1.Text == "Parabolic with cavity receiver (Norwich)")
                            {
                                PTC_rhx1.textBox7.Text = "0.3";
                                PTC_rhx1.textBox8.Text = "3.25e-9";
                            }

                            PTC_rhx1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            PTC_rhx1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_rhx1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            PTC_rhx1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            PTC_rhx1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            PTC_rhx1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            PTC_rhx1.textBox107.Text = Convert.ToString(10);
                            PTC_rhx1.button1_Click(this, e);
                            puntero_aplicacion.PTC_Reheating1_SF_Effective_Apperture_Area = PTC_rhx1.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Reheating1_SF_Pressure_drop = PTC_rhx1.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_rhx1 = new LF_SF_Calculation();
                            LF_rhx1.calledForSensingAnalysis = true;
                            LF_rhx1.comboBox1.Text = "Solar Salt";
                            LF_rhx1.comboBox2.Text = "PureFluid";
                            LF_rhx1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_rhx1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_rhx1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            LF_rhx1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_rhx1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            LF_rhx1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            LF_rhx1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            LF_rhx1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            LF_rhx1.textBox107.Text = Convert.ToString(10);
                            LF_rhx1.button1_Click(this, e);
                            puntero_aplicacion.LF_Reheating1_SF_Effective_Apperture_Area = LF_rhx1.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Reheating1_SF_Pressure_drop = LF_rhx1.Total_Pressure_DropResult;

                            //Reheating_2 Solar Field
                            PTC_SF_Calculation PTC_rhx2 = new PTC_SF_Calculation();
                            PTC_rhx2.calledForSensingAnalysis = true;
                            PTC_rhx2.comboBox1.Text = "Solar Salt";
                            PTC_rhx2.comboBox2.Text = "PureFluid";
                            PTC_rhx2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_rhx2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;

                            if (comboBox2.Text == "Parabolic")
                            {
                                PTC_rhx2.textBox7.Text = "0.141";
                                PTC_rhx2.textBox8.Text = "6.48e-9";
                            }
                            else if (comboBox2.Text == "Parabolic with cavity receiver (Norwich)")
                            {
                                PTC_rhx2.textBox7.Text = "0.3";
                                PTC_rhx2.textBox8.Text = "3.25e-9";
                            }

                            PTC_rhx2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            PTC_rhx2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_rhx2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            PTC_rhx2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            PTC_rhx2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            PTC_rhx2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            PTC_rhx2.textBox107.Text = Convert.ToString(10);
                            PTC_rhx2.button1_Click(this, e);
                            puntero_aplicacion.PTC_Reheating2_SF_Effective_Apperture_Area = PTC_rhx2.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Reheating2_SF_Pressure_drop = PTC_rhx2.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_rhx2 = new LF_SF_Calculation();
                            LF_rhx2.calledForSensingAnalysis = true;
                            LF_rhx2.comboBox1.Text = "Solar Salt";
                            LF_rhx2.comboBox2.Text = "PureFluid";
                            LF_rhx2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_rhx2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_rhx2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            LF_rhx2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_rhx2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp219);
                            LF_rhx2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp220);
                            LF_rhx2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres219);
                            LF_rhx2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres220);
                            LF_rhx2.textBox107.Text = Convert.ToString(10);
                            LF_rhx2.button1_Click(this, e);
                            puntero_aplicacion.LF_Reheating2_SF_Effective_Apperture_Area = LF_rhx2.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_Reheating2_SF_Pressure_drop = LF_rhx2.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //PC_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = p_pc_in2_list[maxIndex].ToString();
                            //PC_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = p_pc_out2_list[maxIndex].ToString();
                            //MC4_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = p_mc4_in2_list[maxIndex].ToString();
                            //MC4_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = p_mc4_out2_list[maxIndex].ToString();
                            //MC3_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = p_mc3_in2_list[maxIndex].ToString();
                            //MC3_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = p_mc3_out2_list[maxIndex].ToString();
                            //MC2_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_mc2_in2_list[maxIndex].ToString();
                            //MC2_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = p_mc2_out2_list[maxIndex].ToString();
                            //P_rhx1_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = p_rhx1_in2_list[maxIndex].ToString();
                            //P_rhx2_in(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = p_rhx2_in2_list[maxIndex].ToString();
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = Convert.ToString(puntero_aplicacion.ua_lt);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = Convert.ToString(puntero_aplicacion.ua_ht);
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = recomp_frac2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = cicloPC_Two_RCMCI_with_Two_Reheating.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 17] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 18] = cicloPC_Two_RCMCI_with_Two_Reheating.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 19] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 21] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 22] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 23] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            //PTC_rhx1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 24] = puntero_aplicacion.PTC_Reheating1_SF_Effective_Apperture_Area.ToString();
                            //PTC_rhx1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 25] = puntero_aplicacion.PTC_Reheating1_SF_Pressure_drop.ToString();
                            //LF_rhx1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 26] = puntero_aplicacion.LF_Reheating1_SF_Effective_Apperture_Area.ToString();
                            //LF_rhx1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 27] = puntero_aplicacion.LF_Reheating1_SF_Pressure_drop.ToString();
                            //PTC_rhx2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 28] = puntero_aplicacion.PTC_Reheating2_SF_Effective_Apperture_Area.ToString();
                            //PTC_rhx2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 29] = puntero_aplicacion.PTC_Reheating2_SF_Pressure_drop.ToString();
                            //LF_rhx2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 30] = puntero_aplicacion.LF_Reheating2_SF_Effective_Apperture_Area.ToString();
                            //LF_rhx2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 31] = puntero_aplicacion.LF_Reheating2_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_pc_in_value = puntero_aplicacion.p_pc_in;
                            initial_pc_out_value = puntero_aplicacion.p_pc_out;
                            initial_mc4_out_value = puntero_aplicacion.p_mc4_out;
                            initial_mc3_out_value = puntero_aplicacion.p_mc3_out;
                            initial_mc2_out_value = puntero_aplicacion.p_mc2_out;
                        }
                    }
                }
            }

            //Closing Excel Book
            xlWorkBook1.SaveAs(textBox3.Text + "PC_Two_RCMCI_with_Two_RH" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }
    }
}
