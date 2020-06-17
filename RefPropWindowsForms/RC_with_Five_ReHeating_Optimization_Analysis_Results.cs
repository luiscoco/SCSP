using System;
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
    public partial class RC_with_Five_ReHeating_Optimization_Analysis_Results : Form
    {
        RC_with_Five_ReHeating puntero_aplicacion;

        public RC_with_Five_ReHeating_Optimization_Analysis_Results(RC_with_Five_ReHeating puntero1)
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

            //UA Optimization false
            if (checkBox2.Checked == false)
            {
                //PureFluid
                if (puntero_aplicacion.comboBox16.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox16.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," +
                       puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," +
                       puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," +
                       puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text, puntero_aplicacion.category);
                }

                if (puntero_aplicacion.comboBox16.Text == "PredefinedMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                }

                if (puntero_aplicacion.comboBox16.Text == "PseudoPureFluid")
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
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox114.Text);
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox113.Text);
                puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox116.Text);
                puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox115.Text);
                puntero_aplicacion.p_rhx4_in2 = Convert.ToDouble(puntero_aplicacion.textBox118.Text);
                puntero_aplicacion.t_rht4_in2 = Convert.ToDouble(puntero_aplicacion.textBox117.Text);
                puntero_aplicacion.p_rhx5_in2 = Convert.ToDouble(puntero_aplicacion.textBox120.Text);
                puntero_aplicacion.t_rht5_in2 = Convert.ToDouble(puntero_aplicacion.textBox119.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.dp2_rhx3 = Convert.ToDouble(puntero_aplicacion.textBox72.Text);
                puntero_aplicacion.dp2_rhx4 = Convert.ToDouble(puntero_aplicacion.textBox109.Text);
                puntero_aplicacion.dp2_rhx5 = Convert.ToDouble(puntero_aplicacion.textBox110.Text);

                puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta_mc = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox70.Text);
                puntero_aplicacion.eta_trh3 = Convert.ToDouble(puntero_aplicacion.textBox71.Text);
                puntero_aplicacion.eta_trh4 = Convert.ToDouble(puntero_aplicacion.textBox111.Text);
                puntero_aplicacion.eta_trh5 = Convert.ToDouble(puntero_aplicacion.textBox112.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycleFiveReheating cicloRC_withFiveRH = new core.RecompCycleFiveReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
                List<Double> p_rhx3_in2_list = new List<Double>();
                List<Double> p_rhx4_in2_list = new List<Double>();
                List<Double> p_rhx5_in2_list = new List<Double>();
                List<Double> ua_HT_list = new List<Double>();
                List<Double> ua_LT_list = new List<Double>();

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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox60.Text + "," +
                                           puntero_aplicacion.comboBox1.Text + ":" + puntero_aplicacion.textBox61.Text + "," +
                                           puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox51.Text + "," +
                                           puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox80.Text;

                xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                xlWorkSheet1.Cells[2, 1] = "";
                xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                xlWorkSheet1.Cells[3, 1] = "";
                xlWorkSheet1.Cells[3, 2] = "";
                xlWorkSheet1.Cells[4, 3] = "";

                xlWorkSheet1.Cells[4, 1] = "CIP(kPa)";
                xlWorkSheet1.Cells[4, 2] = "CIT(K)";
                xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 6] = "P_rhx1_in(kPa)";
                xlWorkSheet1.Cells[4, 7] = "P_rhx2_in(kPa)";
                xlWorkSheet1.Cells[4, 8] = "P_rhx3_in(kPa)";
                xlWorkSheet1.Cells[4, 9] = "P_rhx4_in(kPa)";
                xlWorkSheet1.Cells[4, 10] = "P_rhx5_in(kPa)";
                xlWorkSheet1.Cells[4, 11] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 14] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 15] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 7, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, initial_CIP_value + 5000, initial_CIP_value + 4000, initial_CIP_value + 3000, initial_CIP_value + 2000, initial_CIP_value + 1000 });
                    solver.SetUpperBounds(new[] { 1.0, 125000, puntero_aplicacion.p_mc_out2 - 1000, puntero_aplicacion.p_mc_out2 - 2000, puntero_aplicacion.p_mc_out2 - 3000, puntero_aplicacion.p_mc_out2 - 4000, puntero_aplicacion.p_mc_out2 - 5000 });

                    solver.SetInitialStepSize(new[] { 0.01, 200, 1000, 1000, 1000, 1000, 1000 });

                    var initialValue = new[] { 0.1, initial_CIP_value, 19000, 17000, 15000, 13000, 11000 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_withFiveReheating(puntero_aplicacion.luis,
                        ref cicloRC_withFiveRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2,
                        puntero_aplicacion.t_t_in2, variables[1], puntero_aplicacion.p_mc_out2,
                        variables[2], puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[4], puntero_aplicacion.t_rht3_in2,
                        variables[5], puntero_aplicacion.t_rht4_in2, variables[6], puntero_aplicacion.t_rht5_in2, 
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, 
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp2_rhx3, -puntero_aplicacion.dp2_rhx4, -puntero_aplicacion.dp2_rhx5, 
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2, 
                        puntero_aplicacion.ua_ht2, variables[0], puntero_aplicacion.eta_mc, puntero_aplicacion.eta_rc,
                        puntero_aplicacion.eta_t, puntero_aplicacion.eta_trh1, puntero_aplicacion.eta_trh2, 
                        puntero_aplicacion.eta_trh3, puntero_aplicacion.eta_trh4, puntero_aplicacion.eta_trh5,
                        puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withFiveRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withFiveRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withFiveRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[3];
                        puntero_aplicacion.p_rhx3_in2 = variables[4];
                        puntero_aplicacion.p_rhx4_in2 = variables[5];
                        puntero_aplicacion.p_rhx5_in2 = variables[6];

                        puntero_aplicacion.temp21 = cicloRC_withFiveRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withFiveRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withFiveRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withFiveRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withFiveRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withFiveRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withFiveRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withFiveRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withFiveRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withFiveRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withFiveRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withFiveRH.temp[11];
                        puntero_aplicacion.temp213 = cicloRC_withFiveRH.temp[12];
                        puntero_aplicacion.temp214 = cicloRC_withFiveRH.temp[13];
                        puntero_aplicacion.temp215 = cicloRC_withFiveRH.temp[14];
                        puntero_aplicacion.temp216 = cicloRC_withFiveRH.temp[15];
                        puntero_aplicacion.temp217 = cicloRC_withFiveRH.temp[16];
                        puntero_aplicacion.temp218 = cicloRC_withFiveRH.temp[17];
                        puntero_aplicacion.temp219 = cicloRC_withFiveRH.temp[18];
                        puntero_aplicacion.temp220 = cicloRC_withFiveRH.temp[19];

                        puntero_aplicacion.pres21 = cicloRC_withFiveRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withFiveRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withFiveRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withFiveRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withFiveRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withFiveRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withFiveRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withFiveRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withFiveRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withFiveRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withFiveRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withFiveRH.pres[11];
                        puntero_aplicacion.pres213 = cicloRC_withFiveRH.pres[12];
                        puntero_aplicacion.pres214 = cicloRC_withFiveRH.pres[13];
                        puntero_aplicacion.pres215 = cicloRC_withFiveRH.pres[14];
                        puntero_aplicacion.pres216 = cicloRC_withFiveRH.pres[15];
                        puntero_aplicacion.pres217 = cicloRC_withFiveRH.pres[16];
                        puntero_aplicacion.pres218 = cicloRC_withFiveRH.pres[17];
                        puntero_aplicacion.pres219 = cicloRC_withFiveRH.pres[18];
                        puntero_aplicacion.pres220 = cicloRC_withFiveRH.pres[19];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withFiveRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1_Q2 = cicloRC_withFiveRH.RHX1.Q_dot;
                        puntero_aplicacion.RHX2_Q2 = cicloRC_withFiveRH.RHX2.Q_dot;
                        puntero_aplicacion.RHX3_Q2 = cicloRC_withFiveRH.RHX3.Q_dot;
                        puntero_aplicacion.RHX4_Q2 = cicloRC_withFiveRH.RHX4.Q_dot;
                        puntero_aplicacion.RHX5_Q2 = cicloRC_withFiveRH.RHX5.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withFiveRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withFiveRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withFiveRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withFiveRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withFiveRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withFiveRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withFiveRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withFiveRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withFiveRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withFiveRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withFiveRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withFiveRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withFiveRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withFiveRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withFiveRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withFiveRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withFiveRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withFiveRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withFiveRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withFiveRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withFiveRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
                        p_rhx4_in2_list.Add(puntero_aplicacion.p_rhx4_in2);
                        p_rhx5_in2_list.Add(puntero_aplicacion.p_rhx5_in2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                        listBox24.Items.Add(puntero_aplicacion.p_rhx4_in2.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_rhx5_in2.ToString());

                        double LTR_min_DT_1 = cicloRC_withFiveRH.temp[7] - cicloRC_withFiveRH.temp[2];
                        double LTR_min_DT_2 = cicloRC_withFiveRH.temp[8] - cicloRC_withFiveRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withFiveRH.temp[7] - cicloRC_withFiveRH.temp[3];
                        double HTR_min_DT_2 = cicloRC_withFiveRH.temp[6] - cicloRC_withFiveRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx1_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //P_rhx2_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx2_in2.ToString();
                        //P_rhx3_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.p_rhx3_in2.ToString();
                        //P_rhx4_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.p_rhx4_in2.ToString();
                        //P_rhx5_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = puntero_aplicacion.p_rhx5_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = cicloRC_withFiveRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = cicloRC_withFiveRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 15] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list.Max();

                    var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    textBox91.Text = p_mc_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                    textBox2.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox4.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox5.Text = p_rhx3_in2_list[maxIndex].ToString();
                    textBox6.Text = p_rhx4_in2_list[maxIndex].ToString();
                    textBox7.Text = p_rhx5_in2_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox114.Text = p_rhx2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox116.Text = p_rhx3_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox118.Text = p_rhx4_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox120.Text = p_rhx5_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_with_Five_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }

            //UA Optimization true
        }

        //Run CIT Optimization
        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
