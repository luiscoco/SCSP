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
    public partial class PCRC_withReHeating_Optimization_new_configuration : Form
    {
        PCRC_withReHeating_new_proposed_configuration puntero_aplicacion;

        public PCRC_withReHeating_Optimization_new_configuration(PCRC_withReHeating_new_proposed_configuration puntero1)
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox87.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox88.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.m_w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);

                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.p_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_pc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.t_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_rhx2_in2 = puntero_aplicacion.p_pc_in2;
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht1_in2;

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = 0.0;
                puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_cooler2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);

                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.m_recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.m_eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.m_eta_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);

                puntero_aplicacion.m_eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.m_eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.m_eta_t2 = puntero_aplicacion.m_eta_trh12;

                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration = new core.PCRCwithTwoReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
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

                xlWorkSheet1.Cells[4, 1] = "PC_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "PC_out(kPa)";
                xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 7] = "P_rhx(kPa)";
                xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                //PRIMERA LLAMADA
                double max_recomp_fraction = 0.0;
                double max_pc1_p_in = 0.0;
                double temp5_max_eff = 0.0;

                List<Double> temp5_list_primera = new List<Double>();

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                        -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration.temp[13];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration.temp[7] - cicloPCRC_withTwoRH_new_configuration.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration.temp[8] - cicloPCRC_withTwoRH_new_configuration.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration.temp[7] - cicloPCRC_withTwoRH_new_configuration.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration.temp[6] - cicloPCRC_withTwoRH_new_configuration.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration.HT.eff.ToString();
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

                    textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();

                    max_recomp_fraction = recomp_frac2_list[maxIndex];
                    max_pc1_p_in = p_pc_in2_list[maxIndex];
                    temp5_max_eff = temp5_list_primera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin PRIMERA LLAMADA

                //listBox1.Items.Clear();
                //listBox2.Items.Clear();
                //listBox3.Items.Clear();
                //listBox4.Items.Clear();
                //listBox5.Items.Clear();
                //listBox6.Items.Clear();
                //listBox7.Items.Clear();
                //listBox8.Items.Clear();

                textBox86.Text = "";
                textBox90.Text = "";
                textBox91.Text = "";
                textBox2.Text = "";
                textBox82.Text = "";
                textBox83.Text = "";

                //SEGUNDA LLAMADA para la optimización
                double max_recomp_fraction_1 = 0.0;
                double max_pc1_p_in_1 = 0.0;
                double temp5_max_eff_1 = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_segunda_llamada = new core.PCRCwithTwoReheating();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_pc_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_pc_out2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_segunda_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration_segunda_llamada, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        temp5_max_eff, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                        -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.COOLER.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                    var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                    textBox91.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();

                    max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                    max_pc1_p_in_1 = p_pc_in2_list_segunda_llamada[maxIndex];
                    temp5_max_eff_1 = temp5_list_segunda[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin SEGUNDA LLAMADA

                //listBox1.Items.Clear();
                //listBox2.Items.Clear();
                //listBox3.Items.Clear();
                //listBox4.Items.Clear();
                //listBox5.Items.Clear();
                //listBox6.Items.Clear();
                //listBox7.Items.Clear();
                //listBox8.Items.Clear();

                textBox86.Text = "";
                textBox90.Text = "";
                textBox91.Text = "";
                textBox2.Text = "";
                textBox82.Text = "";
                textBox83.Text = "";

                //TERCERA LLAMADA para la optimización
                double max_recomp_fraction_2 = 0.0;
                double max_pc1_p_in_2 = 0.0;
                double temp5_max_eff_2 = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_tercera_llamada = new core.PCRCwithTwoReheating();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_pc_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_pc_out2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_tercera_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration_tercera_llamada, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        temp5_max_eff_1, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                        -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.COOLER.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                    var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                    textBox91.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();

                    max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                    max_pc1_p_in_2 = p_pc_in2_list_tercera_llamada[maxIndex];
                    temp5_max_eff_2 = temp5_list_tercera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin TERCERA LLAMADA


                //listBox1.Items.Clear();
                //listBox2.Items.Clear();
                //listBox3.Items.Clear();
                //listBox4.Items.Clear();
                //listBox5.Items.Clear();
                //listBox6.Items.Clear();
                //listBox7.Items.Clear();
                //listBox8.Items.Clear();

                textBox86.Text = "";
                textBox90.Text = "";
                textBox91.Text = "";
                textBox2.Text = "";
                textBox82.Text = "";
                textBox83.Text = "";

                //CUARTA LLAMADA para la optimización
                double max_recomp_fraction_3 = 0.0;
                double max_pc1_p_in_3 = 0.0;
                double temp5_max_eff_3 = 0.0;

                List<Double> temp5_list_cuarta = new List<Double>();

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_cuarta_llamada = new core.PCRCwithTwoReheating();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_pc_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_pc_out2_list_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration_cuarta_llamada, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        temp5_max_eff_2, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                        -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.COOLER.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                    var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                    textBox91.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();

                    max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                    max_pc1_p_in_3 = p_pc_in2_list_cuarta_llamada[maxIndex];
                    temp5_max_eff_3 = temp5_list_cuarta[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin CUARTA LLAMADA
            }

            //Optimize UA
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox87.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox88.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.m_w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);

                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.p_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_pc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.t_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_rhx2_in2 = puntero_aplicacion.p_pc_in2;
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht1_in2;

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = 0.0;
                puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_cooler2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);

                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.m_recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.m_eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.m_eta_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);

                puntero_aplicacion.m_eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.m_eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.m_eta_t2 = puntero_aplicacion.m_eta_trh12;

                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration = new core.PCRCwithTwoReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox34.Text;
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
                xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 7] = "P_rhx(kPa)";
                xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                //PRIMERA LLAMADA
                double max_recomp_fraction = 0.0;
                double max_pc1_p_in = 0.0;
                double temp5_max_eff = 0.0;

                List<Double> temp5_list_primera = new List<Double>();

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], variables[4], UA_Total,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, 
                        puntero_aplicacion.m_eta_t2, puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, 
                        puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, 
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, 
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, 
                        -puntero_aplicacion.dp2_cooler1, -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        LT_fraction = variables[4];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration.temp[14];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration.temp[15];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration.temp[7] - cicloPCRC_withTwoRH_new_configuration.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration.temp[8] - cicloPCRC_withTwoRH_new_configuration.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration.temp[7] - cicloPCRC_withTwoRH_new_configuration.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration.temp[6] - cicloPCRC_withTwoRH_new_configuration.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration.HT.eff.ToString();
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

                    textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction = recomp_frac2_list[maxIndex];
                    max_pc1_p_in = p_pc_in2_list[maxIndex];
                    temp5_max_eff = temp5_list_primera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin PRIMERA LLAMADA

                //listBox1.Items.Clear();
                //listBox2.Items.Clear();
                //listBox3.Items.Clear();
                //listBox4.Items.Clear();
                //listBox5.Items.Clear();
                //listBox6.Items.Clear();
                //listBox7.Items.Clear();
                //listBox8.Items.Clear();

                textBox86.Text = "";
                textBox90.Text = "";
                textBox91.Text = "";
                textBox2.Text = "";
                textBox82.Text = "";
                textBox83.Text = "";

                //SEGUNDA LLAMADA para la optimización
                double max_recomp_fraction_1 = 0.0;
                double max_pc1_p_in_1 = 0.0;
                double temp5_max_eff_1 = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_segunda_llamada = new core.PCRCwithTwoReheating();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_pc_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_pc_out2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_segunda_llamada = new List<Double>();
                List<Double> ua_LT_list_segunda_llamada = new List<Double>();
                List<Double> ua_HT_list_segunda_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration_segunda_llamada, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        temp5_max_eff, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], variables[4], UA_Total,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                        -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        LT_fraction = variables[4];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.COOLER.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);
                        ua_LT_list_segunda_llamada.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list_segunda_llamada.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                    var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                    textBox91.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                    textBox83.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();

                    max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                    max_pc1_p_in_1 = p_pc_in2_list_segunda_llamada[maxIndex];
                    temp5_max_eff_1 = temp5_list_segunda[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin SEGUNDA LLAMADA

                //listBox1.Items.Clear();
                //listBox2.Items.Clear();
                //listBox3.Items.Clear();
                //listBox4.Items.Clear();
                //listBox5.Items.Clear();
                //listBox6.Items.Clear();
                //listBox7.Items.Clear();
                //listBox8.Items.Clear();

                textBox86.Text = "";
                textBox90.Text = "";
                textBox91.Text = "";
                textBox2.Text = "";
                textBox82.Text = "";
                textBox83.Text = "";

                //TERCERA LLAMADA para la optimización
                double max_recomp_fraction_2 = 0.0;
                double max_pc1_p_in_2 = 0.0;
                double temp5_max_eff_2 = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_tercera_llamada = new core.PCRCwithTwoReheating();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_pc_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_pc_out2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_tercera_llamada = new List<Double>();
                List<Double> ua_LT_list_tercera_llamada = new List<Double>();
                List<Double> ua_HT_list_tercera_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration_tercera_llamada, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        temp5_max_eff_1, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], variables[4], UA_Total,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                        -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        LT_fraction = variables[4];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.COOLER.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);
                        ua_LT_list_tercera_llamada.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list_tercera_llamada.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                    var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                    textBox91.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                    textBox83.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();

                    max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                    max_pc1_p_in_2 = p_pc_in2_list_tercera_llamada[maxIndex];
                    temp5_max_eff_2 = temp5_list_tercera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin TERCERA LLAMADA


                //listBox1.Items.Clear();
                //listBox2.Items.Clear();
                //listBox3.Items.Clear();
                //listBox4.Items.Clear();
                //listBox5.Items.Clear();
                //listBox6.Items.Clear();
                //listBox7.Items.Clear();
                //listBox8.Items.Clear();

                textBox86.Text = "";
                textBox90.Text = "";
                textBox91.Text = "";
                textBox2.Text = "";
                textBox82.Text = "";
                textBox83.Text = "";

                //CUARTA LLAMADA para la optimización
                double max_recomp_fraction_3 = 0.0;
                double max_pc1_p_in_3 = 0.0;
                double temp5_max_eff_3 = 0.0;

                List<Double> temp5_list_cuarta = new List<Double>();

                core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_cuarta_llamada = new core.PCRCwithTwoReheating();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_pc_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_pc_out2_list_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_cuarta_llamada = new List<Double>();
                List<Double> ua_LT_list_cuarta_llamada = new List<Double>();
                List<Double> ua_HT_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                        ref cicloPCRC_withTwoRH_new_configuration_cuarta_llamada, puntero_aplicacion.m_w_dot_net2, puntero_aplicacion.t_mc_in2,
                        temp5_max_eff_2, puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_pc_in2,
                        variables[2], variables[4], UA_Total,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                        -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.eta_thermal;
                        puntero_aplicacion.m_recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        LT_fraction = variables[4];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];
                        puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.COOLER.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                        p_pc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                        ua_LT_list_cuarta_llamada.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list_cuarta_llamada.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                    var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                    textBox91.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                    textBox1.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox3.Text = p_rhx2_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list_cuarta_llamada[maxIndex].ToString();
                    textBox83.Text = ua_HT_list_cuarta_llamada[maxIndex].ToString();

                    max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                    max_pc1_p_in_3 = p_pc_in2_list_cuarta_llamada[maxIndex];
                    temp5_max_eff_3 = temp5_list_cuarta[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list_cuarta_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                }//Fin CUARTA LLAMADA
            }
        
        }

        //Run CIT Optimization
        private void button7_Click(object sender, EventArgs e)
        {
            int counter = 0;

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

            for (double j = Convert.ToDouble(textBox8.Text); j <= Convert.ToDouble(textBox7.Text); j = j + Convert.ToDouble(textBox6.Text))
            {
                puntero_aplicacion.ua_lt2 = j / 2;
                puntero_aplicacion.ua_ht2 = j / 2;

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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text, puntero_aplicacion.category);
                        }

                        //NewMixture
                        if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox87.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox88.Text, puntero_aplicacion.category);
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
                        puntero_aplicacion.m_w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                        puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);

                        puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.p_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_pc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.t_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                        puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                        puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_rhx2_in2 = puntero_aplicacion.p_pc_in2;
                        puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                        puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht1_in2;

                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp2_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_phx1 = 0.0;
                        puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                        puntero_aplicacion.dp2_cooler2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);

                        puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.m_recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                        puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.m_eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.m_eta_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);

                        puntero_aplicacion.m_eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.m_eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                        puntero_aplicacion.m_eta_t2 = puntero_aplicacion.m_eta_trh12;

                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH = new core.PCRCwithTwoReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_pc_in2_list = new List<Double>();
                        List<Double> p_pc_out2_list = new List<Double>();
                        List<Double> p_rhx1_in2_list = new List<Double>();
                        List<Double> p_rhx2_in2_list = new List<Double>();
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

                        if (i == Convert.ToDouble(textBox57.Text) && (j == Convert.ToDouble(textBox8.Text)))
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

                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox34.Text;
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
                            xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 7] = "P_rhx(kPa)";
                            xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                            if (checkBox7.Checked == false)
                            {
                                xlWorkSheet1.Cells[4, 13] = "Main PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 14] = "Main PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 15] = "Main LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 16] = "Main LF_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 17] = "ReHeating PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 18] = "ReHeating PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 19] = "ReHeating LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 20] = "ReHeating LF_Pressure_Drop(bar)";
                            }
                        }

                        //PRIMERA LLAMADA
                        double max_recomp_fraction = 0.0;
                        double max_pc1_p_in = 0.0;
                        double temp5_max_eff = 0.0;

                        List<Double> temp5_list_primera = new List<Double>();

                        using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH, puntero_aplicacion.m_w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, 
                                puntero_aplicacion.m_eta_t2, puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22,
                                puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                                -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                                -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                //puntero_aplicacion.p_mc_in2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH.temp[14];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH.temp[15];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_primera.Add(puntero_aplicacion.temp25);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH.temp[7] - cicloPCRC_withTwoRH.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH.temp[8] - cicloPCRC_withTwoRH.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH.temp[7] - cicloPCRC_withTwoRH.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH.temp[6] - cicloPCRC_withTwoRH.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration.HT.eff.ToString();
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

                            textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();

                            max_recomp_fraction = recomp_frac2_list[maxIndex];
                            max_pc1_p_in = p_pc_in2_list[maxIndex];
                            temp5_max_eff = temp5_list_primera[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }// Fin de la PRIMERA LLAMADA

                        //listBox1.Items.Clear();
                        //listBox2.Items.Clear();
                        //listBox3.Items.Clear();
                        //listBox4.Items.Clear();
                        //listBox5.Items.Clear();
                        //listBox6.Items.Clear();
                        //listBox7.Items.Clear();
                        //listBox8.Items.Clear();

                        textBox86.Text = "";
                        textBox90.Text = "";
                        textBox91.Text = "";
                        textBox2.Text = "";
                        textBox82.Text = "";
                        textBox83.Text = "";

                        //SEGUNDA LLAMADA para la optimización
                        double max_recomp_fraction_1 = 0.0;
                        double max_pc1_p_in_1 = 0.0;
                        double temp5_max_eff_1 = 0.0;

                        List<Double> temp5_list_segunda = new List<Double>();

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_segunda_llamada = new core.PCRCwithTwoReheating();

                        List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                        List<Double> p_pc_in2_list_segunda_llamada = new List<Double>();
                        List<Double> p_pc_out2_list_segunda_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                        List<Double> p_rhx1_in2_list_segunda_llamada = new List<Double>();
                        List<Double> p_rhx2_in2_list_segunda_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH_new_configuration_segunda_llamada, puntero_aplicacion.m_w_dot_net2, i,
                                temp5_max_eff, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, 
                                puntero_aplicacion.m_eta_t2, puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, 
                                puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2, 
                                puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, 
                                -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp2_pc1, 
                                -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, 
                                -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21,
                                -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1, -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[14];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[15];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.COOLER.Q_dot;

                                eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_segunda.Add(puntero_aplicacion.temp25);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                            var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                            textBox91.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list_segunda_llamada[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();

                            max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                            max_pc1_p_in_1 = p_pc_in2_list_segunda_llamada[maxIndex];
                            temp5_max_eff_1 = temp5_list_segunda[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }//Fin SEGUNDA LLAMADA

                        //listBox1.Items.Clear();
                        //listBox2.Items.Clear();
                        //listBox3.Items.Clear();
                        //listBox4.Items.Clear();
                        //listBox5.Items.Clear();
                        //listBox6.Items.Clear();
                        //listBox7.Items.Clear();
                        //listBox8.Items.Clear();

                        textBox86.Text = "";
                        textBox90.Text = "";
                        textBox91.Text = "";
                        textBox2.Text = "";
                        textBox82.Text = "";
                        textBox83.Text = "";

                        //TERCERA LLAMADA para la optimización
                        double max_recomp_fraction_2 = 0.0;
                        double max_pc1_p_in_2 = 0.0;
                        double temp5_max_eff_2 = 0.0;

                        List<Double> temp5_list_tercera = new List<Double>();

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_tercera_llamada = new core.PCRCwithTwoReheating();

                        List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                        List<Double> p_pc_in2_list_tercera_llamada = new List<Double>();
                        List<Double> p_pc_out2_list_tercera_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                        List<Double> p_rhx1_in2_list_tercera_llamada = new List<Double>();
                        List<Double> p_rhx2_in2_list_tercera_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH_new_configuration_tercera_llamada, puntero_aplicacion.m_w_dot_net2, 
                                i, temp5_max_eff_1, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, 
                                puntero_aplicacion.m_eta_t2, puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, 
                                puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, 
                                -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, 
                                -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, 
                                -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, 
                                -puntero_aplicacion.dp2_cooler1, -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[14];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[15];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.COOLER.Q_dot;

                                eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_tercera.Add(puntero_aplicacion.temp25);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                            var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                            textBox91.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list_tercera_llamada[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();

                            max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                            max_pc1_p_in_2 = p_pc_in2_list_tercera_llamada[maxIndex];
                            temp5_max_eff_2 = temp5_list_tercera[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }//Fin TERCERA LLAMADA

                        //listBox1.Items.Clear();
                        //listBox2.Items.Clear();
                        //listBox3.Items.Clear();
                        //listBox4.Items.Clear();
                        //listBox5.Items.Clear();
                        //listBox6.Items.Clear();
                        //listBox7.Items.Clear();
                        //listBox8.Items.Clear();

                        textBox86.Text = "";
                        textBox90.Text = "";
                        textBox91.Text = "";
                        textBox2.Text = "";
                        textBox82.Text = "";
                        textBox83.Text = "";

                        //CUARTA LLAMADA para la optimización
                        double max_recomp_fraction_3 = 0.0;
                        double max_pc1_p_in_3 = 0.0;
                        double temp5_max_eff_3 = 0.0;

                        List<Double> temp5_list_cuarta = new List<Double>();

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_cuarta_llamada = new core.PCRCwithTwoReheating();

                        List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_pc_in2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_pc_out2_list_cuarta_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_rhx1_in2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_rhx2_in2_list_cuarta_llamada = new List<Double>();

                        List<Double> massflow2_list = new List<Double>();

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
                        List<Double> t11_list = new List<Double>();
                        List<Double> t12_list = new List<Double>();
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();
                        List<Double> t15_list = new List<Double>();
                        List<Double> t16_list = new List<Double>();

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
                        List<Double> p11_list = new List<Double>();
                        List<Double> p12_list = new List<Double>();
                        List<Double> p13_list = new List<Double>();
                        List<Double> p14_list = new List<Double>();
                        List<Double> p15_list = new List<Double>();
                        List<Double> p16_list = new List<Double>();

                        List<Double> HT_Eff_list = new List<Double>();
                        List<Double> LT_Eff_list = new List<Double>();

                        List<Double> RHX1_Q_list = new List<Double>();
                        List<Double> RHX2_Q_list = new List<Double>();

                        List<Double> ua_lt_list = new List<Double>();
                        List<Double> ua_ht_list = new List<Double>();

                        xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                        xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        xlWorkSheet1.Activate();

                        using (var solver = new NLoptSolver(algorithm_type, 4, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0) });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH_new_configuration_cuarta_llamada, puntero_aplicacion.m_w_dot_net2, i,
                                temp5_max_eff_2, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                                puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                                -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                                -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[14];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[15];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.COOLER.Q_dot;

                                eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                                ua_lt_list.Add(puntero_aplicacion.ua_lt2);
                                ua_ht_list.Add(puntero_aplicacion.ua_ht2);
                                massflow2_list.Add(puntero_aplicacion.massflow2);

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
                                t11_list.Add(puntero_aplicacion.temp211);
                                t12_list.Add(puntero_aplicacion.temp212);
                                t13_list.Add(puntero_aplicacion.temp213);
                                t14_list.Add(puntero_aplicacion.temp214);
                                t15_list.Add(puntero_aplicacion.temp215);
                                t16_list.Add(puntero_aplicacion.temp216);

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
                                p11_list.Add(puntero_aplicacion.pres211);
                                p12_list.Add(puntero_aplicacion.pres212);
                                p13_list.Add(puntero_aplicacion.pres213);
                                p14_list.Add(puntero_aplicacion.pres214);
                                p15_list.Add(puntero_aplicacion.pres215);
                                p16_list.Add(puntero_aplicacion.pres216);

                                RHX1_Q_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX1.Q_dot);
                                RHX2_Q_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX2.Q_dot);

                                HT_Eff_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff);
                                LT_Eff_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                            var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                            textBox91.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list_cuarta_llamada[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();

                            max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                            max_pc1_p_in_3 = p_pc_in2_list_cuarta_llamada[maxIndex];
                            temp5_max_eff_3 = temp5_list_cuarta[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list_cuarta_llamada[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list_cuarta_llamada[maxIndex].ToString());
                            listBox15.Items.Add(p_pc_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox10.Items.Add(p_pc_out2_list_cuarta_llamada[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox22.Items.Add(p_rhx2_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox11.Items.Add(t5_list[maxIndex].ToString());
                            listBox12.Items.Add(t6_list[maxIndex].ToString());
                            listBox13.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox14.Items.Add(puntero_aplicacion.ua_ht2.ToString());

                            if (checkBox7.Checked == false)
                            {
                                //REHEATING_1 Calculo del campo solar
                                PTC_SF_Calculation PTC_RHX1 = new PTC_SF_Calculation();
                                PTC_RHX1.calledForSensingAnalysis = true;
                                PTC_RHX1.comboBox1.Text = "Solar Salt";
                                PTC_RHX1.comboBox2.Text = "NewMixture";
                                PTC_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                PTC_RHX1.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //PTC.textBox31.Text = this.textBox31.Text;
                                //PTC.textBox36.Text = this.textBox36.Text;

                                if (comboBox4.Text == "Parabolic")
                                {
                                    PTC_RHX1.textBox7.Text = "0.141";
                                    PTC_RHX1.textBox8.Text = "6.48e-9";
                                }
                                else if (comboBox4.Text == "Parabolic with cavity receiver (Norwich)")
                                {
                                    PTC_RHX1.textBox7.Text = "0.3";
                                    PTC_RHX1.textBox8.Text = "3.25e-9";
                                }

                                PTC_RHX1.textBox1.Text = Convert.ToString(RHX1_Q_list[maxIndex]);
                                PTC_RHX1.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                PTC_RHX1.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                PTC_RHX1.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                PTC_RHX1.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                PTC_RHX1.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
                                PTC_RHX1.textBox107.Text = Convert.ToString(10);
                                PTC_RHX1.button1_Click(this, e);
                                puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area = PTC_RHX1.ReflectorApertureAreaResult;
                                puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop = PTC_RHX1.Total_Pressure_DropResult;

                                LF_SF_Calculation LF_RHX1 = new LF_SF_Calculation();
                                LF_RHX1.calledForSensingAnalysis = true;
                                LF_RHX1.comboBox1.Text = "Solar Salt";
                                LF_RHX1.comboBox2.Text = "NewMixture";
                                LF_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                LF_RHX1.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //LF.textBox31.Text = this.textBox31.Text;
                                //LF.textBox36.Text = this.textBox36.Text;
                                LF_RHX1.textBox1.Text = Convert.ToString(RHX1_Q_list[maxIndex]);
                                LF_RHX1.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                LF_RHX1.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                LF_RHX1.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                LF_RHX1.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                LF_RHX1.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
                                LF_RHX1.textBox107.Text = Convert.ToString(10);
                                LF_RHX1.button1_Click(this, e);
                                puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area = LF_RHX1.ReflectorApertureAreaResult;
                                puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop = LF_RHX1.Total_Pressure_DropResult;

                                //REHEATING_2 Calculo del campo solar
                                PTC_SF_Calculation PTC_RHX2 = new PTC_SF_Calculation();
                                PTC_RHX2.calledForSensingAnalysis = true;
                                PTC_RHX2.comboBox1.Text = "Solar Salt";
                                PTC_RHX2.comboBox2.Text = "NewMixture";
                                PTC_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                PTC_RHX2.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //PTC.textBox31.Text = this.textBox31.Text;
                                //PTC.textBox36.Text = this.textBox36.Text;

                                if (comboBox1.Text == "Parabolic")
                                {
                                    PTC_RHX2.textBox7.Text = "0.141";
                                    PTC_RHX2.textBox8.Text = "6.48e-9";
                                }
                                else if (comboBox1.Text == "Parabolic with cavity receiver (Norwich)")
                                {
                                    PTC_RHX2.textBox7.Text = "0.3";
                                    PTC_RHX2.textBox8.Text = "3.25e-9";
                                }

                                PTC_RHX2.textBox1.Text = Convert.ToString(RHX2_Q_list[maxIndex]);
                                PTC_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                PTC_RHX2.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                                PTC_RHX2.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                                PTC_RHX2.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                                PTC_RHX2.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                                PTC_RHX2.textBox107.Text = Convert.ToString(10);
                                PTC_RHX2.button1_Click(this, e);
                                puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area = PTC_RHX2.ReflectorApertureAreaResult;
                                puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop = PTC_RHX2.Total_Pressure_DropResult;

                                LF_SF_Calculation LF_RHX2 = new LF_SF_Calculation();
                                LF_RHX2.calledForSensingAnalysis = true;
                                LF_RHX2.comboBox1.Text = "Solar Salt";
                                LF_RHX2.comboBox2.Text = "NewMixture";
                                LF_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                LF_RHX2.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //LF.textBox31.Text = this.textBox31.Text;
                                //LF.textBox36.Text = this.textBox36.Text;
                                LF_RHX2.textBox1.Text = Convert.ToString(RHX2_Q_list[maxIndex]);
                                LF_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                LF_RHX2.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                                LF_RHX2.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                                LF_RHX2.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                                LF_RHX2.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                                LF_RHX2.textBox107.Text = Convert.ToString(10);
                                LF_RHX2.button1_Click(this, e);
                                puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area = LF_RHX2.ReflectorApertureAreaResult;
                                puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop = LF_RHX2.Total_Pressure_DropResult;
                            }

                            //Copy results to EXCEL
                            double LTR_min_DT_1_max = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2_max = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper_max = Math.Min(LTR_min_DT_1_max, LTR_min_DT_2_max);

                            double HTR_min_DT_1_max = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2_max = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper_max = Math.Min(HTR_min_DT_1_max, HTR_min_DT_2_max);

                            //Precompressor inlet pressure (kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_pc_in2_list_cuarta_llamada[maxIndex]);
                            //Precompressor output pressure (kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(p_pc_out2_list_cuarta_llamada[maxIndex]);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_lt2.ToString();
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.ua_ht2.ToString();
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                            //P_rhx1
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = (eta_thermal2_list_cuarta_llamada[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = LT_Eff_list[maxIndex].ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper_max.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = HT_Eff_list[maxIndex].ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper_max.ToString();

                            if (checkBox7.Checked == false)
                            {
                                //PTC_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area.ToString();
                                //PTC_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop.ToString();
                                //LF_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area.ToString();
                                //LF_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 16] = puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop.ToString();
                                //PTC_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 17] = puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area.ToString();
                                //PTC_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 18] = puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop.ToString();
                                //LF_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 19] = puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area.ToString();
                                //LF_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop.ToString();
                            }

                            counter_Excel++;

                            initial_CIP_value = puntero_aplicacion.p_pc_in2;

                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }//Fin CUARTA LLAMADA


                    }// Fin UA optimization false

                    //Optimize UA
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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox87.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox88.Text, puntero_aplicacion.category);
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
                        puntero_aplicacion.m_w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                        puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);

                        puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                        puntero_aplicacion.p_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_pc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.t_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                        puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                        puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_rhx2_in2 = puntero_aplicacion.p_pc_in2;
                        puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                        puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht1_in2;

                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp2_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_phx1 = 0.0;
                        puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                        puntero_aplicacion.dp2_cooler2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);

                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.m_recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                        puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.m_eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.m_eta_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);

                        puntero_aplicacion.m_eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.m_eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                        puntero_aplicacion.m_eta_t2 = puntero_aplicacion.m_eta_trh12;

                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration = new core.PCRCwithTwoReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_pc_in2_list = new List<Double>();
                        List<Double> p_pc_out2_list = new List<Double>();
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

                        if (i == Convert.ToDouble(textBox57.Text) && (j == Convert.ToDouble(textBox8.Text)))
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

                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox68.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox33.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox34.Text;
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
                            xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 7] = "P_rhx(kPa)";
                            xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                            if (checkBox7.Checked == false)
                            {
                                xlWorkSheet1.Cells[4, 13] = "Main PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 14] = "Main PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 15] = "Main LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 16] = "Main LF_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 17] = "ReHeating PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 18] = "ReHeating PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 19] = "ReHeating LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 20] = "ReHeating LF_Pressure_Drop(bar)";
                            }
                        }

                        //PRIMERA LLAMADA
                        double max_recomp_fraction = 0.0;
                        double max_pc1_p_in = 0.0;
                        double temp5_max_eff = 0.0;

                        List<Double> temp5_list_primera = new List<Double>();

                        using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH_new_configuration, puntero_aplicacion.m_w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], variables[4], UA_Total,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2,
                                puntero_aplicacion.m_eta_t2, puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22,
                                puntero_aplicacion.n_sub_hxrs2, variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                                -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2,
                                -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                                -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                                -puntero_aplicacion.dp2_cooler1, -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];
                                LT_fraction = variables[4];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration.temp[14];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration.temp[15];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_primera.Add(puntero_aplicacion.temp25);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration.temp[7] - cicloPCRC_withTwoRH_new_configuration.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration.temp[8] - cicloPCRC_withTwoRH_new_configuration.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration.temp[7] - cicloPCRC_withTwoRH_new_configuration.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration.temp[6] - cicloPCRC_withTwoRH_new_configuration.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration.HT.eff.ToString();
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

                            textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            max_recomp_fraction = recomp_frac2_list[maxIndex];
                            max_pc1_p_in = p_pc_in2_list[maxIndex];
                            temp5_max_eff = temp5_list_primera[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }//Fin PRIMERA LLAMADA

                        //listBox1.Items.Clear();
                        //listBox2.Items.Clear();
                        //listBox3.Items.Clear();
                        //listBox4.Items.Clear();
                        //listBox5.Items.Clear();
                        //listBox6.Items.Clear();
                        //listBox7.Items.Clear();
                        //listBox8.Items.Clear();

                        textBox86.Text = "";
                        textBox90.Text = "";
                        textBox91.Text = "";
                        textBox2.Text = "";
                        textBox82.Text = "";
                        textBox83.Text = "";

                        //SEGUNDA LLAMADA para la optimización
                        double max_recomp_fraction_1 = 0.0;
                        double max_pc1_p_in_1 = 0.0;
                        double temp5_max_eff_1 = 0.0;

                        List<Double> temp5_list_segunda = new List<Double>();

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_segunda_llamada = new core.PCRCwithTwoReheating();

                        List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                        List<Double> p_pc_in2_list_segunda_llamada = new List<Double>();
                        List<Double> p_pc_out2_list_segunda_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                        List<Double> p_rhx1_in2_list_segunda_llamada = new List<Double>();
                        List<Double> p_rhx2_in2_list_segunda_llamada = new List<Double>();
                        List<Double> ua_LT_list_segunda_llamada = new List<Double>();
                        List<Double> ua_HT_list_segunda_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH_new_configuration_segunda_llamada, puntero_aplicacion.m_w_dot_net2, i,
                                temp5_max_eff, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], variables[4], UA_Total,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                                puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                                -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                                -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];
                                LT_fraction = variables[4];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_segunda_llamada.COOLER.Q_dot;

                                eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list_segunda_llamada.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_segunda.Add(puntero_aplicacion.temp25);
                                ua_LT_list_segunda_llamada.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list_segunda_llamada.Add(puntero_aplicacion.ua_ht2);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_segunda_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_segunda_llamada.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                            var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                            textBox91.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list_segunda_llamada[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                            textBox82.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                            textBox83.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();

                            max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                            max_pc1_p_in_1 = p_pc_in2_list_segunda_llamada[maxIndex];
                            temp5_max_eff_1 = temp5_list_segunda[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }//Fin SEGUNDA LLAMADA

                        //listBox1.Items.Clear();
                        //listBox2.Items.Clear();
                        //listBox3.Items.Clear();
                        //listBox4.Items.Clear();
                        //listBox5.Items.Clear();
                        //listBox6.Items.Clear();
                        //listBox7.Items.Clear();
                        //listBox8.Items.Clear();

                        textBox86.Text = "";
                        textBox90.Text = "";
                        textBox91.Text = "";
                        textBox2.Text = "";
                        textBox82.Text = "";
                        textBox83.Text = "";

                        //TERCERA LLAMADA para la optimización
                        double max_recomp_fraction_2 = 0.0;
                        double max_pc1_p_in_2 = 0.0;
                        double temp5_max_eff_2 = 0.0;

                        List<Double> temp5_list_tercera = new List<Double>();

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_tercera_llamada = new core.PCRCwithTwoReheating();

                        List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                        List<Double> p_pc_in2_list_tercera_llamada = new List<Double>();
                        List<Double> p_pc_out2_list_tercera_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                        List<Double> p_rhx1_in2_list_tercera_llamada = new List<Double>();
                        List<Double> p_rhx2_in2_list_tercera_llamada = new List<Double>();
                        List<Double> ua_LT_list_tercera_llamada = new List<Double>();
                        List<Double> ua_HT_list_tercera_llamada = new List<Double>();

                        xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                        xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        xlWorkSheet1.Activate();

                        using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH_new_configuration_tercera_llamada, puntero_aplicacion.m_w_dot_net2, i,
                                temp5_max_eff_1, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], variables[4], UA_Total,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                                puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                                -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                                -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];
                                LT_fraction = variables[4];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_tercera_llamada.COOLER.Q_dot;

                                eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list_tercera_llamada.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_tercera.Add(puntero_aplicacion.temp25);
                                ua_LT_list_tercera_llamada.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list_tercera_llamada.Add(puntero_aplicacion.ua_ht2);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_tercera_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_tercera_llamada.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                            var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                            textBox91.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list_tercera_llamada[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                            textBox82.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                            textBox83.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();

                            max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                            max_pc1_p_in_2 = p_pc_in2_list_tercera_llamada[maxIndex];
                            temp5_max_eff_2 = temp5_list_tercera[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }//Fin TERCERA LLAMADA


                        //listBox1.Items.Clear();
                        //listBox2.Items.Clear();
                        //listBox3.Items.Clear();
                        //listBox4.Items.Clear();
                        //listBox5.Items.Clear();
                        //listBox6.Items.Clear();
                        //listBox7.Items.Clear();
                        //listBox8.Items.Clear();

                        textBox86.Text = "";
                        textBox90.Text = "";
                        textBox91.Text = "";
                        textBox2.Text = "";
                        textBox82.Text = "";
                        textBox83.Text = "";

                        //CUARTA LLAMADA para la optimización
                        double max_recomp_fraction_3 = 0.0;
                        double max_pc1_p_in_3 = 0.0;
                        double temp5_max_eff_3 = 0.0;

                        List<Double> temp5_list_cuarta = new List<Double>();

                        core.PCRCwithTwoReheating cicloPCRC_withTwoRH_new_configuration_cuarta_llamada = new core.PCRCwithTwoReheating();

                        List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_pc_in2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_pc_out2_list_cuarta_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_rhx1_in2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_rhx2_in2_list_cuarta_llamada = new List<Double>();

                        List<Double> massflow2_list = new List<Double>();

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
                        List<Double> t11_list = new List<Double>();
                        List<Double> t12_list = new List<Double>();
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();
                        List<Double> t15_list = new List<Double>();
                        List<Double> t16_list = new List<Double>();

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
                        List<Double> p11_list = new List<Double>();
                        List<Double> p12_list = new List<Double>();
                        List<Double> p13_list = new List<Double>();
                        List<Double> p14_list = new List<Double>();
                        List<Double> p15_list = new List<Double>();
                        List<Double> p16_list = new List<Double>();

                        List<Double> HT_Eff_list = new List<Double>();
                        List<Double> LT_Eff_list = new List<Double>();

                        List<Double> RHX1_Q_list = new List<Double>();
                        List<Double> RHX2_Q_list = new List<Double>();

                        List<Double> ua_lt_list_cuarta_llamada = new List<Double>();
                        List<Double> ua_ht_list_cuarta_llamada = new List<Double>();

                        xlWorkBook1 = xlApp1.Workbooks.Open(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls");
                        xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        xlWorkSheet1.Activate();

                        using (var solver = new NLoptSolver(algorithm_type, 5, 0.00001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 250.0), (initial_CIP_value + 1000.0), 0.0 });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc_out2 / 1.5), 22000.0, 1.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value), (initial_CIP_value + 3500.0), (initial_CIP_value + 9500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_PCRC_withReheating_newproposed_for_optimization(puntero_aplicacion.luis,
                                ref cicloPCRC_withTwoRH_new_configuration_cuarta_llamada, puntero_aplicacion.m_w_dot_net2, i,
                                temp5_max_eff_2, puntero_aplicacion.t_rht1_in2, variables[3],
                                puntero_aplicacion.t_rht2_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc_out2, variables[1], i,
                                variables[2], variables[4], UA_Total,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                                puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                                -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22, -puntero_aplicacion.dp2_cooler1,
                                -puntero_aplicacion.dp2_cooler2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.eta_thermal;
                                puntero_aplicacion.m_recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in2 = variables[1];
                                puntero_aplicacion.p_rhx2_in2 = variables[1];
                                puntero_aplicacion.p_pc_out2 = variables[2];
                                puntero_aplicacion.p_rhx1_in2 = variables[3];
                                LT_fraction = variables[4];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[14];
                                puntero_aplicacion.temp216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[15];

                                puntero_aplicacion.pres21 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.pres[15];

                                puntero_aplicacion.PHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.PC.Q_dot;
                                puntero_aplicacion.COOLER1 = -cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.COOLER.Q_dot;

                                eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.m_recomp_frac2);
                                p_pc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_in2);
                                p_pc_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_pc_out2);
                                p_rhx1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                                temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                                ua_lt_list_cuarta_llamada.Add(puntero_aplicacion.ua_lt2);
                                ua_ht_list_cuarta_llamada.Add(puntero_aplicacion.ua_ht2);
                                massflow2_list.Add(puntero_aplicacion.massflow2);

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
                                t11_list.Add(puntero_aplicacion.temp211);
                                t12_list.Add(puntero_aplicacion.temp212);
                                t13_list.Add(puntero_aplicacion.temp213);
                                t14_list.Add(puntero_aplicacion.temp214);
                                t15_list.Add(puntero_aplicacion.temp215);
                                t16_list.Add(puntero_aplicacion.temp216);

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
                                p11_list.Add(puntero_aplicacion.pres211);
                                p12_list.Add(puntero_aplicacion.pres212);
                                p13_list.Add(puntero_aplicacion.pres213);
                                p14_list.Add(puntero_aplicacion.pres214);
                                p15_list.Add(puntero_aplicacion.pres215);
                                p16_list.Add(puntero_aplicacion.pres216);

                                RHX1_Q_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX1.Q_dot);
                                RHX2_Q_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.RHX2.Q_dot);

                                HT_Eff_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff);
                                LT_Eff_list.Add(cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.m_recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[2];
                                double LTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[8] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[7] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[3];
                                double HTR_min_DT_2 = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[6] - cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in2);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out2);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.m_recomp_frac2.ToString();
                                ////P_rhx_in
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withTwoRH_new_configuration_cuarta_llamada.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

                                return puntero_aplicacion.eta_thermal2;
                            };

                            solver.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                            var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                            textBox91.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                            textBox1.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                            textBox3.Text = p_rhx2_in2_list_cuarta_llamada[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                            textBox82.Text = ua_lt_list_cuarta_llamada[maxIndex].ToString();
                            textBox83.Text = ua_ht_list_cuarta_llamada[maxIndex].ToString();

                            max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                            max_pc1_p_in_3 = p_pc_in2_list_cuarta_llamada[maxIndex];
                            temp5_max_eff_3 = temp5_list_cuarta[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_pc_in2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_lt_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_ht_list_cuarta_llamada[maxIndex].ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list_cuarta_llamada[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list_cuarta_llamada[maxIndex].ToString());
                            listBox15.Items.Add(p_pc_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox10.Items.Add(p_pc_out2_list_cuarta_llamada[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox22.Items.Add(p_rhx2_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox11.Items.Add(t5_list[maxIndex].ToString());
                            listBox12.Items.Add(t6_list[maxIndex].ToString());
                            listBox13.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox14.Items.Add(puntero_aplicacion.ua_ht2.ToString());

                            if (checkBox7.Checked == false)
                            {
                                //REHEATING_1 Calculo del campo solar
                                PTC_SF_Calculation PTC_RHX1 = new PTC_SF_Calculation();
                                PTC_RHX1.calledForSensingAnalysis = true;
                                PTC_RHX1.comboBox1.Text = "Solar Salt";
                                PTC_RHX1.comboBox2.Text = "NewMixture";
                                PTC_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                PTC_RHX1.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //PTC.textBox31.Text = this.textBox31.Text;
                                //PTC.textBox36.Text = this.textBox36.Text;

                                if (comboBox4.Text == "Parabolic")
                                {
                                    PTC_RHX1.textBox7.Text = "0.141";
                                    PTC_RHX1.textBox8.Text = "6.48e-9";
                                }
                                else if (comboBox4.Text == "Parabolic with cavity receiver (Norwich)")
                                {
                                    PTC_RHX1.textBox7.Text = "0.3";
                                    PTC_RHX1.textBox8.Text = "3.25e-9";
                                }

                                PTC_RHX1.textBox1.Text = Convert.ToString(RHX1_Q_list[maxIndex]);
                                PTC_RHX1.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                PTC_RHX1.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                PTC_RHX1.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                PTC_RHX1.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                PTC_RHX1.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
                                PTC_RHX1.textBox107.Text = Convert.ToString(10);
                                PTC_RHX1.button1_Click(this, e);
                                puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area = PTC_RHX1.ReflectorApertureAreaResult;
                                puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop = PTC_RHX1.Total_Pressure_DropResult;

                                LF_SF_Calculation LF_RHX1 = new LF_SF_Calculation();
                                LF_RHX1.calledForSensingAnalysis = true;
                                LF_RHX1.comboBox1.Text = "Solar Salt";
                                LF_RHX1.comboBox2.Text = "NewMixture";
                                LF_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                LF_RHX1.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //LF.textBox31.Text = this.textBox31.Text;
                                //LF.textBox36.Text = this.textBox36.Text;
                                LF_RHX1.textBox1.Text = Convert.ToString(RHX1_Q_list[maxIndex]);
                                LF_RHX1.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                LF_RHX1.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                LF_RHX1.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                LF_RHX1.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                LF_RHX1.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
                                LF_RHX1.textBox107.Text = Convert.ToString(10);
                                LF_RHX1.button1_Click(this, e);
                                puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area = LF_RHX1.ReflectorApertureAreaResult;
                                puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop = LF_RHX1.Total_Pressure_DropResult;

                                //REHEATING_2 Calculo del campo solar
                                PTC_SF_Calculation PTC_RHX2 = new PTC_SF_Calculation();
                                PTC_RHX2.calledForSensingAnalysis = true;
                                PTC_RHX2.comboBox1.Text = "Solar Salt";
                                PTC_RHX2.comboBox2.Text = "NewMixture";
                                PTC_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                PTC_RHX2.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //PTC.textBox31.Text = this.textBox31.Text;
                                //PTC.textBox36.Text = this.textBox36.Text;

                                if (comboBox1.Text == "Parabolic")
                                {
                                    PTC_RHX2.textBox7.Text = "0.141";
                                    PTC_RHX2.textBox8.Text = "6.48e-9";
                                }
                                else if (comboBox1.Text == "Parabolic with cavity receiver (Norwich)")
                                {
                                    PTC_RHX2.textBox7.Text = "0.3";
                                    PTC_RHX2.textBox8.Text = "3.25e-9";
                                }

                                PTC_RHX2.textBox1.Text = Convert.ToString(RHX2_Q_list[maxIndex]);
                                PTC_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                PTC_RHX2.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                                PTC_RHX2.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                                PTC_RHX2.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                                PTC_RHX2.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                                PTC_RHX2.textBox107.Text = Convert.ToString(10);
                                PTC_RHX2.button1_Click(this, e);
                                puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area = PTC_RHX2.ReflectorApertureAreaResult;
                                puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop = PTC_RHX2.Total_Pressure_DropResult;

                                LF_SF_Calculation LF_RHX2 = new LF_SF_Calculation();
                                LF_RHX2.calledForSensingAnalysis = true;
                                LF_RHX2.comboBox1.Text = "Solar Salt";
                                LF_RHX2.comboBox2.Text = "NewMixture";
                                LF_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                LF_RHX2.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                                //LF.textBox31.Text = this.textBox31.Text;
                                //LF.textBox36.Text = this.textBox36.Text;
                                LF_RHX2.textBox1.Text = Convert.ToString(RHX2_Q_list[maxIndex]);
                                LF_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                                LF_RHX2.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                                LF_RHX2.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                                LF_RHX2.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                                LF_RHX2.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                                LF_RHX2.textBox107.Text = Convert.ToString(10);
                                LF_RHX2.button1_Click(this, e);
                                puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area = LF_RHX2.ReflectorApertureAreaResult;
                                puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop = LF_RHX2.Total_Pressure_DropResult;
                            }

                            //Copy results to EXCEL
                            double LTR_min_DT_1_max = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2_max = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper_max = Math.Min(LTR_min_DT_1_max, LTR_min_DT_2_max);

                            double HTR_min_DT_1_max = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2_max = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper_max = Math.Min(HTR_min_DT_1_max, HTR_min_DT_2_max);

                            //Precompressor inlet pressure (kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_pc_in2_list_cuarta_llamada[maxIndex]);
                            //Precompressor output pressure (kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(p_pc_out2_list_cuarta_llamada[maxIndex]);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_lt2.ToString();
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.ua_ht2.ToString();
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                            //P_rhx1
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = (eta_thermal2_list_cuarta_llamada[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = LT_Eff_list[maxIndex].ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper_max.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = HT_Eff_list[maxIndex].ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper_max.ToString();

                            if (checkBox7.Checked == false)
                            {
                                //PTC_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area.ToString();
                                //PTC_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop.ToString();
                                //LF_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area.ToString();
                                //LF_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 16] = puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop.ToString();
                                //PTC_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 17] = puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area.ToString();
                                //PTC_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 18] = puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop.ToString();
                                //LF_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 19] = puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area.ToString();
                                //LF_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop.ToString();
                            }

                            counter_Excel++;

                            initial_CIP_value = puntero_aplicacion.p_pc_in2;

                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_ReHeating_new_configuration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        }//Fin CUARTA LLAMADA
                    }
                }
            }
        }
    }
}