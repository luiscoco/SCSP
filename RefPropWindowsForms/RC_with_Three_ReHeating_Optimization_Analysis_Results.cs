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
    public partial class RC_with_Three_ReHeating_Optimization_Analysis_Results : Form
    {
        RC_with_Three_ReHeating puntero_aplicacion;

        public RC_with_Three_ReHeating_Optimization_Analysis_Results(RC_with_Three_ReHeating puntero1)
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

        //Optimization Button
        private void Button3_Click(object sender, EventArgs e)
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox16.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," +
                                                  puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," +
                                                  puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," +
                                                  puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text, 
                                                  puntero_aplicacion.category);                   
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
                puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox63.Text);
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox62.Text);
                puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox69.Text);
                puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox68.Text);
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

                puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta_mc = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox70.Text);
                puntero_aplicacion.eta_trh3 = Convert.ToDouble(puntero_aplicacion.textBox71.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycleThreeReheating cicloRC_withThreeRH = new core.RecompCycleThreeReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
                List<Double> p_rhx3_in2_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 9] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 12] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "HTR Pinch(ºC)";              

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, initial_CIP_value + 3000, initial_CIP_value + 2000, initial_CIP_value + 1000 });
                    solver.SetUpperBounds(new[] { 1.0, 125000, puntero_aplicacion.p_mc_out2 - 1000, puntero_aplicacion.p_mc_out2 - 2000, puntero_aplicacion.p_mc_out2 - 3000 });

                    solver.SetInitialStepSize(new[] { 0.01, 200, 1000, 1000, 1000 });

                    var initialValue = new[] { 0.1, initial_CIP_value, 15000, 13000, 11000 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_withThreeReheating(puntero_aplicacion.luis, 
                        ref cicloRC_withThreeRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, 
                        puntero_aplicacion.t_t_in2, variables[1], puntero_aplicacion.p_mc_out2,
                         variables[2], puntero_aplicacion.t_rht1_in2, variables[3],
                         puntero_aplicacion.t_rht2_in2, variables[4], puntero_aplicacion.t_rht3_in2, 
                         -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, 
                         -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2, 
                         -puntero_aplicacion.dp2_rhx3, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, 
                         puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, variables[0],
                         puntero_aplicacion.eta_mc, puntero_aplicacion.eta_rc, puntero_aplicacion.eta_t, 
                         puntero_aplicacion.eta_trh1, puntero_aplicacion.eta_trh2, puntero_aplicacion.eta_trh3, 
                         puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withThreeRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withThreeRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withThreeRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[3];
                        puntero_aplicacion.p_rhx3_in2 = variables[4];

                        puntero_aplicacion.temp21 = cicloRC_withThreeRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withThreeRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withThreeRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withThreeRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withThreeRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withThreeRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withThreeRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withThreeRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withThreeRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withThreeRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withThreeRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withThreeRH.temp[11];
                        puntero_aplicacion.temp213 = cicloRC_withThreeRH.temp[12];
                        puntero_aplicacion.temp214 = cicloRC_withThreeRH.temp[13];
                        puntero_aplicacion.temp215 = cicloRC_withThreeRH.temp[14];
                        puntero_aplicacion.temp216 = cicloRC_withThreeRH.temp[15];
                        puntero_aplicacion.temp217 = cicloRC_withThreeRH.temp[16];
                        puntero_aplicacion.temp218 = cicloRC_withThreeRH.temp[17];

                        puntero_aplicacion.pres21 = cicloRC_withThreeRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withThreeRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withThreeRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withThreeRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withThreeRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withThreeRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withThreeRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withThreeRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withThreeRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withThreeRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withThreeRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withThreeRH.pres[11];
                        puntero_aplicacion.pres213 = cicloRC_withThreeRH.pres[12];
                        puntero_aplicacion.pres214 = cicloRC_withThreeRH.pres[13];
                        puntero_aplicacion.pres215 = cicloRC_withThreeRH.pres[14];
                        puntero_aplicacion.pres216 = cicloRC_withThreeRH.pres[15];
                        puntero_aplicacion.pres217 = cicloRC_withThreeRH.pres[16];
                        puntero_aplicacion.pres218 = cicloRC_withThreeRH.pres[17];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withThreeRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1_Q2 = cicloRC_withThreeRH.RHX1.Q_dot;
                        puntero_aplicacion.RHX2_Q2 = cicloRC_withThreeRH.RHX2.Q_dot;
                        puntero_aplicacion.RHX3_Q2 = cicloRC_withThreeRH.RHX3.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withThreeRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withThreeRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withThreeRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withThreeRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withThreeRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withThreeRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withThreeRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withThreeRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withThreeRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withThreeRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withThreeRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withThreeRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withThreeRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withThreeRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withThreeRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withThreeRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withThreeRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withThreeRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withThreeRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withThreeRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withThreeRH.PC.Q_dot;
                      
                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
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

                        double LTR_min_DT_1 = cicloRC_withThreeRH.temp[7] - cicloRC_withThreeRH.temp[2];
                        double LTR_min_DT_2 = cicloRC_withThreeRH.temp[8] - cicloRC_withThreeRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withThreeRH.temp[7] - cicloRC_withThreeRH.temp[3];
                        double HTR_min_DT_2 = cicloRC_withThreeRH.temp[6] - cicloRC_withThreeRH.temp[4];
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
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withThreeRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = cicloRC_withThreeRH.HT.eff.ToString();
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

                    textBox91.Text = p_mc_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                    textBox2.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox4.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox5.Text = p_rhx3_in2_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox63.Text = p_rhx2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox69.Text = p_rhx3_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_with_Three_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }

            //-------------------------------------------------------------------------

            //UA Optimization true
            else if (checkBox2.Checked == true)
            {
                //PureFluid
                if (puntero_aplicacion.comboBox16.Text == "PureFluid")
                {
                    puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                }

                //NewMixture
                if (puntero_aplicacion.comboBox16.Text == "NewMixture")
                {
                    puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," +
                                                  puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," +
                                                  puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," +
                                                  puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text,
                                                  puntero_aplicacion.category);
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
                puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox63.Text);
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox62.Text);
                puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox69.Text);
                puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox68.Text);
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

                puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta_mc = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox70.Text);
                puntero_aplicacion.eta_trh3 = Convert.ToDouble(puntero_aplicacion.textBox71.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycleThreeReheating cicloRC_withThreeRH = new core.RecompCycleThreeReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
                List<Double> p_rhx3_in2_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 9] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 12] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "HTR Pinch(ºC)";
               
                using (var solver = new NLoptSolver(algorithm_type, 6, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0, initial_CIP_value + 3000, initial_CIP_value + 2000, initial_CIP_value + 1000 });
                    solver.SetUpperBounds(new[] { 1.0, 125000, 1.0, puntero_aplicacion.p_mc_out2 - 1000, puntero_aplicacion.p_mc_out2 - 2000, puntero_aplicacion.p_mc_out2 - 3000 });

                    solver.SetInitialStepSize(new[] { 0.05, 200, 0.05, 1000, 1000, 1000 });

                    var initialValue = new[] { 0.2, initial_CIP_value, 0.5, 15000, 13000, 11000 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_withThreeReheating_for_Optimization(puntero_aplicacion.luis,
                        ref cicloRC_withThreeRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2,
                        puntero_aplicacion.t_t_in2, variables[1], puntero_aplicacion.p_mc_out2,
                        variables[3], puntero_aplicacion.t_rht1_in2, variables[4],
                        puntero_aplicacion.t_rht2_in2, variables[5], puntero_aplicacion.t_rht3_in2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp2_rhx3, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2,
                        variables[2], UA_Total, variables[0],
                        puntero_aplicacion.eta_mc, puntero_aplicacion.eta_rc, puntero_aplicacion.eta_t,
                        puntero_aplicacion.eta_trh1, puntero_aplicacion.eta_trh2, puntero_aplicacion.eta_trh3,
                        puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withThreeRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withThreeRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withThreeRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc_in2 = variables[1];
                        LT_fraction = variables[2];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        puntero_aplicacion.p_rhx2_in2 = variables[4];
                        puntero_aplicacion.p_rhx3_in2 = variables[5];

                        puntero_aplicacion.temp21 = cicloRC_withThreeRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withThreeRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withThreeRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withThreeRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withThreeRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withThreeRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withThreeRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withThreeRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withThreeRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withThreeRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withThreeRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withThreeRH.temp[11];
                        puntero_aplicacion.temp213 = cicloRC_withThreeRH.temp[12];
                        puntero_aplicacion.temp214 = cicloRC_withThreeRH.temp[13];
                        puntero_aplicacion.temp215 = cicloRC_withThreeRH.temp[14];
                        puntero_aplicacion.temp216 = cicloRC_withThreeRH.temp[15];
                        puntero_aplicacion.temp217 = cicloRC_withThreeRH.temp[16];
                        puntero_aplicacion.temp218 = cicloRC_withThreeRH.temp[17];

                        puntero_aplicacion.pres21 = cicloRC_withThreeRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withThreeRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withThreeRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withThreeRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withThreeRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withThreeRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withThreeRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withThreeRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withThreeRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withThreeRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withThreeRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withThreeRH.pres[11];
                        puntero_aplicacion.pres213 = cicloRC_withThreeRH.pres[12];
                        puntero_aplicacion.pres214 = cicloRC_withThreeRH.pres[13];
                        puntero_aplicacion.pres215 = cicloRC_withThreeRH.pres[14];
                        puntero_aplicacion.pres216 = cicloRC_withThreeRH.pres[15];
                        puntero_aplicacion.pres217 = cicloRC_withThreeRH.pres[16];
                        puntero_aplicacion.pres218 = cicloRC_withThreeRH.pres[17];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withThreeRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1_Q2 = cicloRC_withThreeRH.RHX1.Q_dot;
                        puntero_aplicacion.RHX2_Q2 = cicloRC_withThreeRH.RHX2.Q_dot;
                        puntero_aplicacion.RHX3_Q2 = cicloRC_withThreeRH.RHX3.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withThreeRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withThreeRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withThreeRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withThreeRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withThreeRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withThreeRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withThreeRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withThreeRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withThreeRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withThreeRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withThreeRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withThreeRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withThreeRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withThreeRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withThreeRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withThreeRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withThreeRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withThreeRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withThreeRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withThreeRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withThreeRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
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

                        double LTR_min_DT_1 = cicloRC_withThreeRH.temp[7] - cicloRC_withThreeRH.temp[2];
                        double LTR_min_DT_2 = cicloRC_withThreeRH.temp[8] - cicloRC_withThreeRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withThreeRH.temp[7] - cicloRC_withThreeRH.temp[3];
                        double HTR_min_DT_2 = cicloRC_withThreeRH.temp[6] - cicloRC_withThreeRH.temp[4];
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
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withThreeRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = cicloRC_withThreeRH.HT.eff.ToString();
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

                    puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                    puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                    textBox91.Text = p_mc_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox2.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox4.Text = p_rhx2_in2_list[maxIndex].ToString();
                    textBox5.Text = p_rhx3_in2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox63.Text = p_rhx2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox69.Text = p_rhx3_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_with_Three_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }
        }

        //NLopt CIT Optimization Button
        private void Button6_Click(object sender, EventArgs e)
        {
            int counter = 0;

            double initial_Pressure_value = 0;

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

                    //UA optimization false
                    if (checkBox2.Checked == false)
                    {
                        //PureFluid
                        if (puntero_aplicacion.comboBox16.Text == "PureFluid")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                        }

                        //NewMixture
                        if (puntero_aplicacion.comboBox16.Text == "NewMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," +
                                                      puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," +
                                                      puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," +
                                                      puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text,
                                                      puntero_aplicacion.category);
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
                        puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox63.Text);
                        puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox62.Text);
                        puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox69.Text);
                        puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox68.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                        puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                        puntero_aplicacion.dp2_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                        puntero_aplicacion.dp2_rhx3 = Convert.ToDouble(puntero_aplicacion.textBox72.Text);

                        puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta_mc = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                        puntero_aplicacion.eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox70.Text);
                        puntero_aplicacion.eta_trh3 = Convert.ToDouble(puntero_aplicacion.textBox71.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RecompCycleThreeReheating cicloRC_withThreeRH = new core.RecompCycleThreeReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        //int counter = 0;

                        List<Double> massflow2_list = new List<Double>();
                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc_in2_list = new List<Double>();
                        List<Double> p_rhx1_in2_list = new List<Double>();
                        List<Double> p_rhx2_in2_list = new List<Double>();
                        List<Double> p_rhx3_in2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
                        List<Double> PHX_Q2_list = new List<Double>();
                        List<Double> RHX1_Q2_list = new List<Double>();
                        List<Double> RHX2_Q2_list = new List<Double>();
                        List<Double> RHX3_Q2_list = new List<Double>();
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
                        List<Double> t11_list = new List<Double>();
                        List<Double> t12_list = new List<Double>();
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();
                        List<Double> t15_list = new List<Double>();
                        List<Double> t16_list = new List<Double>();
                        List<Double> t17_list = new List<Double>();
                        List<Double> t18_list = new List<Double>();

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
                        List<Double> p17_list = new List<Double>();
                        List<Double> p18_list = new List<Double>();

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
                            initial_Pressure_value = puntero_aplicacion.MixtureCriticalPressure;

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
                            xlWorkSheet1.Cells[4, 6] = "P_rhx1_in";
                            xlWorkSheet1.Cells[4, 7] = "P_rhx2_in";
                            xlWorkSheet1.Cells[4, 8] = "P_rhx3_in";
                            xlWorkSheet1.Cells[4, 9] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 11] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 12] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 13] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 14] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 15] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 16] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 17] = "LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 18] = "PTC_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 19] = "PTC_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 20] = "LF_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 21] = "LF_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 22] = "PTC_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 23] = "PTC_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 24] = "LF_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 25] = "LF_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 26] = "PTC_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 27] = "PTC_RHX3_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 28] = "LF_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 29] = "LF_RHX3_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 5, 0.01, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.1, initial_Pressure_value, initial_Pressure_value + 3000, initial_Pressure_value + 2000, initial_Pressure_value + 1000 });
                            solver.SetUpperBounds(new[] { 1.0, 125000, puntero_aplicacion.p_mc_out2 - 1000, puntero_aplicacion.p_mc_out2 - 2000, puntero_aplicacion.p_mc_out2 - 3000 });

                            solver.SetInitialStepSize(new[] { 0.01, 200, 1000, 1000, 1000 });

                            var initialValue = new[] { 0.1, initial_Pressure_value, 15000, 13000, 11000 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycledesign_withThreeReheating(puntero_aplicacion.luis,
                                ref cicloRC_withThreeRH, puntero_aplicacion.w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, variables[1], puntero_aplicacion.p_mc_out2,
                                 variables[2], puntero_aplicacion.t_rht1_in2, variables[3],
                                 puntero_aplicacion.t_rht2_in2, variables[4], puntero_aplicacion.t_rht3_in2,
                                 -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2,
                                 -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                 -puntero_aplicacion.dp2_rhx3, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2,
                                 puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, variables[0],
                                 puntero_aplicacion.eta_mc, puntero_aplicacion.eta_rc, puntero_aplicacion.eta_t,
                                 puntero_aplicacion.eta_trh1, puntero_aplicacion.eta_trh2, puntero_aplicacion.eta_trh3,
                                 puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRC_withThreeRH.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRC_withThreeRH.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloRC_withThreeRH.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc_in2 = variables[1];
                                puntero_aplicacion.p_rhx1_in2 = variables[2];
                                puntero_aplicacion.p_rhx2_in2 = variables[3];
                                puntero_aplicacion.p_rhx3_in2 = variables[4];

                                puntero_aplicacion.temp21 = cicloRC_withThreeRH.temp[0];
                                puntero_aplicacion.temp22 = cicloRC_withThreeRH.temp[1];
                                puntero_aplicacion.temp23 = cicloRC_withThreeRH.temp[2];
                                puntero_aplicacion.temp24 = cicloRC_withThreeRH.temp[3];
                                puntero_aplicacion.temp25 = cicloRC_withThreeRH.temp[4];
                                puntero_aplicacion.temp26 = cicloRC_withThreeRH.temp[5];
                                puntero_aplicacion.temp27 = cicloRC_withThreeRH.temp[6];
                                puntero_aplicacion.temp28 = cicloRC_withThreeRH.temp[7];
                                puntero_aplicacion.temp29 = cicloRC_withThreeRH.temp[8];
                                puntero_aplicacion.temp210 = cicloRC_withThreeRH.temp[9];
                                puntero_aplicacion.temp211 = cicloRC_withThreeRH.temp[10];
                                puntero_aplicacion.temp212 = cicloRC_withThreeRH.temp[11];
                                puntero_aplicacion.temp213 = cicloRC_withThreeRH.temp[12];
                                puntero_aplicacion.temp214 = cicloRC_withThreeRH.temp[13];
                                puntero_aplicacion.temp215 = cicloRC_withThreeRH.temp[14];
                                puntero_aplicacion.temp216 = cicloRC_withThreeRH.temp[15];
                                puntero_aplicacion.temp217 = cicloRC_withThreeRH.temp[16];
                                puntero_aplicacion.temp218 = cicloRC_withThreeRH.temp[17];

                                puntero_aplicacion.pres21 = cicloRC_withThreeRH.pres[0];
                                puntero_aplicacion.pres22 = cicloRC_withThreeRH.pres[1];
                                puntero_aplicacion.pres23 = cicloRC_withThreeRH.pres[2];
                                puntero_aplicacion.pres24 = cicloRC_withThreeRH.pres[3];
                                puntero_aplicacion.pres25 = cicloRC_withThreeRH.pres[4];
                                puntero_aplicacion.pres26 = cicloRC_withThreeRH.pres[5];
                                puntero_aplicacion.pres27 = cicloRC_withThreeRH.pres[6];
                                puntero_aplicacion.pres28 = cicloRC_withThreeRH.pres[7];
                                puntero_aplicacion.pres29 = cicloRC_withThreeRH.pres[8];
                                puntero_aplicacion.pres210 = cicloRC_withThreeRH.pres[9];
                                puntero_aplicacion.pres211 = cicloRC_withThreeRH.pres[10];
                                puntero_aplicacion.pres212 = cicloRC_withThreeRH.pres[11];
                                puntero_aplicacion.pres213 = cicloRC_withThreeRH.pres[12];
                                puntero_aplicacion.pres214 = cicloRC_withThreeRH.pres[13];
                                puntero_aplicacion.pres215 = cicloRC_withThreeRH.pres[14];
                                puntero_aplicacion.pres216 = cicloRC_withThreeRH.pres[15];
                                puntero_aplicacion.pres217 = cicloRC_withThreeRH.pres[16];
                                puntero_aplicacion.pres218 = cicloRC_withThreeRH.pres[17];

                                massflow2_list.Add(puntero_aplicacion.massflow2);
                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                                p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);

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
                                t17_list.Add(puntero_aplicacion.temp217);
                                t18_list.Add(puntero_aplicacion.temp218);

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
                                p17_list.Add(puntero_aplicacion.pres217);
                                p18_list.Add(puntero_aplicacion.pres218);

                                PHX_Q2_list.Add(cicloRC_withThreeRH.PHX.Q_dot);
                                RHX1_Q2_list.Add(cicloRC_withThreeRH.RHX1.Q_dot);
                                RHX2_Q2_list.Add(cicloRC_withThreeRH.RHX2.Q_dot);
                                RHX3_Q2_list.Add(cicloRC_withThreeRH.RHX3.Q_dot);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                                HT_Eff_list.Add(cicloRC_withThreeRH.HT.eff);
                                LT_Eff_list.Add(cicloRC_withThreeRH.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

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
                            textBox2.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox4.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox5.Text = p_rhx3_in2_list[maxIndex].ToString();
                            textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox63.Text = p_rhx2_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox69.Text = p_rhx3_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox2.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_rhx1_in2_list[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx2_in2_list[maxIndex].ToString());
                            listBox22.Items.Add(p_rhx3_in2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_mc_in2_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());

                            //MAIN SOLAR FIELD CALCULATION
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "NewMixture";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC.textBox1.Text = Convert.ToString(PHX_Q2_list[maxIndex]);
                            PTC.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            PTC.textBox3.Text = Convert.ToString(t5_list[maxIndex]);
                            PTC.textBox6.Text = Convert.ToString(t6_list[maxIndex]);
                            PTC.textBox4.Text = Convert.ToString(p5_list[maxIndex]);
                            PTC.textBox5.Text = Convert.ToString(p6_list[maxIndex]);
                            PTC.textBox107.Text = Convert.ToString(10);
                            PTC.button1_Click(this, e);
                            puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area = PTC.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Main_SF_Pressure_drop = PTC.Total_Pressure_DropResult;

                            LF_SF_Calculation LF = new LF_SF_Calculation();
                            LF.calledForSensingAnalysis = true;
                            LF.comboBox1.Text = "Solar Salt";
                            LF.comboBox2.Text = "NewMixture";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF.textBox1.Text = Convert.ToString(PHX_Q2_list[maxIndex]);
                            LF.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF.textBox3.Text = Convert.ToString(t5_list[maxIndex]);
                            LF.textBox6.Text = Convert.ToString(t6_list[maxIndex]);
                            LF.textBox4.Text = Convert.ToString(p5_list[maxIndex]);
                            LF.textBox5.Text = Convert.ToString(p6_list[maxIndex]);
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
                            PTC_RHX1.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX1.textBox1.Text = Convert.ToString(RHX1_Q2_list[maxIndex]);
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
                            LF_RHX1.textBox1.Text = Convert.ToString(RHX1_Q2_list[maxIndex]);
                            LF_RHX1.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF_RHX1.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                            LF_RHX1.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                            LF_RHX1.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                            LF_RHX1.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
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
                            PTC_RHX2.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX2.textBox1.Text = Convert.ToString(RHX2_Q2_list[maxIndex]);
                            PTC_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            PTC_RHX2.textBox3.Text = Convert.ToString(t13_list[maxIndex]);
                            PTC_RHX2.textBox6.Text = Convert.ToString(t14_list[maxIndex]);
                            PTC_RHX2.textBox4.Text = Convert.ToString(p13_list[maxIndex]);
                            PTC_RHX2.textBox5.Text = Convert.ToString(p14_list[maxIndex]);
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
                            LF_RHX2.textBox1.Text = Convert.ToString(RHX2_Q2_list[maxIndex]);
                            LF_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF_RHX2.textBox3.Text = Convert.ToString(t13_list[maxIndex]);
                            LF_RHX2.textBox6.Text = Convert.ToString(t14_list[maxIndex]);
                            LF_RHX2.textBox4.Text = Convert.ToString(p13_list[maxIndex]);
                            LF_RHX2.textBox5.Text = Convert.ToString(p14_list[maxIndex]);
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
                            PTC_RHX3.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX3.textBox1.Text = Convert.ToString(RHX3_Q2_list[maxIndex]);
                            PTC_RHX3.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            PTC_RHX3.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                            PTC_RHX3.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                            PTC_RHX3.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                            PTC_RHX3.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                            PTC_RHX3.textBox107.Text = Convert.ToString(10);
                            PTC_RHX3.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area = PTC_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop = PTC_RHX3.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_RHX3 = new LF_SF_Calculation();
                            LF_RHX3.calledForSensingAnalysis = true;
                            LF_RHX3.comboBox1.Text = "Solar Salt";
                            LF_RHX3.comboBox2.Text = "NewMixture";
                            LF_RHX3.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX3.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX3.textBox1.Text = Convert.ToString(RHX3_Q2_list[maxIndex]);
                            LF_RHX3.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF_RHX3.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                            LF_RHX3.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                            LF_RHX3.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                            LF_RHX3.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                            LF_RHX3.textBox107.Text = Convert.ToString(10);
                            LF_RHX3.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area = LF_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop = LF_RHX3.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //CIP
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_mc_in2_list[maxIndex]);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = recomp_frac2_list[maxIndex].ToString();
                            //P_rhx1_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = p_rhx1_in2_list[maxIndex].ToString();
                            //P_rhx2_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_rhx2_in2_list[maxIndex].ToString();
                            //P_rhx3_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = p_rhx3_in2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = LT_Eff_list[maxIndex].ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = HT_Eff_list[maxIndex].ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 17] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            //PTC_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 18] = puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 19] = puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop.ToString();
                            //LF_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 21] = puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop.ToString();
                            //PTC_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 22] = puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 23] = puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop.ToString();
                            //LF_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 24] = puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 25] = puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop.ToString();
                            //PTC_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 26] = puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 27] = puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop.ToString();
                            //LF_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 28] = puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 29] = puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_Pressure_value = puntero_aplicacion.p_mc_in2;
                        }
                    }

                    //-------------------------------------------------------------------------

                    //UA optimization true
                    else if (checkBox2.Checked == true)
                    {
                        //PureFluid
                        if (puntero_aplicacion.comboBox16.Text == "PureFluid")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox1.Text, puntero_aplicacion.category);
                        }

                        //NewMixture
                        if (puntero_aplicacion.comboBox16.Text == "NewMixture")
                        {
                            puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," +
                                                          puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," +
                                                          puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," +
                                                          puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text,
                                                          puntero_aplicacion.category);
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
                        puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox63.Text);
                        puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox62.Text);
                        puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox69.Text);
                        puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox68.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                        puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                        puntero_aplicacion.dp2_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                        puntero_aplicacion.dp2_rhx3 = Convert.ToDouble(puntero_aplicacion.textBox72.Text);

                        puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta_mc = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                        puntero_aplicacion.eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox70.Text);
                        puntero_aplicacion.eta_trh3 = Convert.ToDouble(puntero_aplicacion.textBox71.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RecompCycleThreeReheating cicloRC_withThreeRH = new core.RecompCycleThreeReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        //int counter = 0;

                        List<Double> massflow2_list = new List<Double>();
                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc_in2_list = new List<Double>();
                        List<Double> p_rhx1_in2_list = new List<Double>();
                        List<Double> p_rhx2_in2_list = new List<Double>();
                        List<Double> p_rhx3_in2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
                        List<Double> PHX_Q2_list = new List<Double>();
                        List<Double> RHX1_Q2_list = new List<Double>();
                        List<Double> RHX2_Q2_list = new List<Double>();
                        List<Double> RHX3_Q2_list = new List<Double>();
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
                        List<Double> t11_list = new List<Double>();
                        List<Double> t12_list = new List<Double>();
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();
                        List<Double> t15_list = new List<Double>();
                        List<Double> t16_list = new List<Double>();
                        List<Double> t17_list = new List<Double>();
                        List<Double> t18_list = new List<Double>();

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
                        List<Double> p17_list = new List<Double>();
                        List<Double> p18_list = new List<Double>();

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
                            initial_Pressure_value = puntero_aplicacion.MixtureCriticalPressure;

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
                            xlWorkSheet1.Cells[4, 6] = "P_rhx1_in";
                            xlWorkSheet1.Cells[4, 7] = "P_rhx2_in";
                            xlWorkSheet1.Cells[4, 8] = "P_rhx3_in";
                            xlWorkSheet1.Cells[4, 9] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 11] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 12] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 13] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 14] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 15] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 16] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 17] = "LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 18] = "PTC_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 19] = "PTC_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 20] = "LF_RHX1_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 21] = "LF_RHX1_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 22] = "PTC_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 23] = "PTC_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 24] = "LF_RHX2_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 25] = "LF_RHX2_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 26] = "PTC_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 27] = "PTC_RHX3_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 28] = "LF_RHX3_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 29] = "LF_RHX3_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 6, 0.01, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.1, initial_Pressure_value, 0.2, initial_Pressure_value + 3000, initial_Pressure_value + 2000, initial_Pressure_value + 1000 });
                            solver.SetUpperBounds(new[] { 1.0, 125000, 0.8, puntero_aplicacion.p_mc_out2 - 1000, puntero_aplicacion.p_mc_out2 - 2000, puntero_aplicacion.p_mc_out2 - 3000 });

                            solver.SetInitialStepSize(new[] { 0.05, 200, 0.05, 1000, 1000, 1000 });

                            var initialValue = new[] { 0.2, initial_Pressure_value, 0.2, 15000, 13000, 11000 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycledesign_withThreeReheating_for_Optimization(puntero_aplicacion.luis,
                                ref cicloRC_withThreeRH, puntero_aplicacion.w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, variables[1], puntero_aplicacion.p_mc_out2,
                                variables[3], puntero_aplicacion.t_rht1_in2, variables[4],
                                puntero_aplicacion.t_rht2_in2, variables[5], puntero_aplicacion.t_rht3_in2,
                                -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2,
                                -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp2_rhx3, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2,
                                variables[2], UA_Total, variables[0],
                                puntero_aplicacion.eta_mc, puntero_aplicacion.eta_rc, puntero_aplicacion.eta_t,
                                puntero_aplicacion.eta_trh1, puntero_aplicacion.eta_trh2, puntero_aplicacion.eta_trh3,
                                puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRC_withThreeRH.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRC_withThreeRH.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloRC_withThreeRH.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc_in2 = variables[1];
                                LT_fraction = variables[2];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);
                                puntero_aplicacion.p_rhx1_in2 = variables[3];
                                puntero_aplicacion.p_rhx2_in2 = variables[4];
                                puntero_aplicacion.p_rhx3_in2 = variables[5];

                                puntero_aplicacion.temp21 = cicloRC_withThreeRH.temp[0];
                                puntero_aplicacion.temp22 = cicloRC_withThreeRH.temp[1];
                                puntero_aplicacion.temp23 = cicloRC_withThreeRH.temp[2];
                                puntero_aplicacion.temp24 = cicloRC_withThreeRH.temp[3];
                                puntero_aplicacion.temp25 = cicloRC_withThreeRH.temp[4];
                                puntero_aplicacion.temp26 = cicloRC_withThreeRH.temp[5];
                                puntero_aplicacion.temp27 = cicloRC_withThreeRH.temp[6];
                                puntero_aplicacion.temp28 = cicloRC_withThreeRH.temp[7];
                                puntero_aplicacion.temp29 = cicloRC_withThreeRH.temp[8];
                                puntero_aplicacion.temp210 = cicloRC_withThreeRH.temp[9];
                                puntero_aplicacion.temp211 = cicloRC_withThreeRH.temp[10];
                                puntero_aplicacion.temp212 = cicloRC_withThreeRH.temp[11];
                                puntero_aplicacion.temp213 = cicloRC_withThreeRH.temp[12];
                                puntero_aplicacion.temp214 = cicloRC_withThreeRH.temp[13];
                                puntero_aplicacion.temp215 = cicloRC_withThreeRH.temp[14];
                                puntero_aplicacion.temp216 = cicloRC_withThreeRH.temp[15];
                                puntero_aplicacion.temp217 = cicloRC_withThreeRH.temp[16];
                                puntero_aplicacion.temp218 = cicloRC_withThreeRH.temp[17];

                                puntero_aplicacion.pres21 = cicloRC_withThreeRH.pres[0];
                                puntero_aplicacion.pres22 = cicloRC_withThreeRH.pres[1];
                                puntero_aplicacion.pres23 = cicloRC_withThreeRH.pres[2];
                                puntero_aplicacion.pres24 = cicloRC_withThreeRH.pres[3];
                                puntero_aplicacion.pres25 = cicloRC_withThreeRH.pres[4];
                                puntero_aplicacion.pres26 = cicloRC_withThreeRH.pres[5];
                                puntero_aplicacion.pres27 = cicloRC_withThreeRH.pres[6];
                                puntero_aplicacion.pres28 = cicloRC_withThreeRH.pres[7];
                                puntero_aplicacion.pres29 = cicloRC_withThreeRH.pres[8];
                                puntero_aplicacion.pres210 = cicloRC_withThreeRH.pres[9];
                                puntero_aplicacion.pres211 = cicloRC_withThreeRH.pres[10];
                                puntero_aplicacion.pres212 = cicloRC_withThreeRH.pres[11];
                                puntero_aplicacion.pres213 = cicloRC_withThreeRH.pres[12];
                                puntero_aplicacion.pres214 = cicloRC_withThreeRH.pres[13];
                                puntero_aplicacion.pres215 = cicloRC_withThreeRH.pres[14];
                                puntero_aplicacion.pres216 = cicloRC_withThreeRH.pres[15];
                                puntero_aplicacion.pres217 = cicloRC_withThreeRH.pres[16];
                                puntero_aplicacion.pres218 = cicloRC_withThreeRH.pres[17];

                                massflow2_list.Add(puntero_aplicacion.massflow2);
                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                                p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                                p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                                p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

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
                                t17_list.Add(puntero_aplicacion.temp217);
                                t18_list.Add(puntero_aplicacion.temp218);

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
                                p17_list.Add(puntero_aplicacion.pres217);
                                p18_list.Add(puntero_aplicacion.pres218);

                                PHX_Q2_list.Add(cicloRC_withThreeRH.PHX.Q_dot);
                                RHX1_Q2_list.Add(cicloRC_withThreeRH.RHX1.Q_dot);
                                RHX2_Q2_list.Add(cicloRC_withThreeRH.RHX2.Q_dot);
                                RHX3_Q2_list.Add(cicloRC_withThreeRH.RHX3.Q_dot);

                                HT_Eff_list.Add(cicloRC_withThreeRH.HT.eff);
                                LT_Eff_list.Add(cicloRC_withThreeRH.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                                listBox21.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

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
                            textBox2.Text = p_rhx1_in2_list[maxIndex].ToString();
                            textBox4.Text = p_rhx2_in2_list[maxIndex].ToString();
                            textBox5.Text = p_rhx3_in2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox63.Text = p_rhx2_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox69.Text = p_rhx3_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox2.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_mc_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_rhx1_in2_list[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx2_in2_list[maxIndex].ToString());
                            listBox22.Items.Add(p_rhx3_in2_list[maxIndex].ToString());
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
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC.textBox1.Text = Convert.ToString(PHX_Q2_list[maxIndex]);
                            PTC.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            PTC.textBox3.Text = Convert.ToString(t5_list[maxIndex]);
                            PTC.textBox6.Text = Convert.ToString(t6_list[maxIndex]);
                            PTC.textBox4.Text = Convert.ToString(p5_list[maxIndex]);
                            PTC.textBox5.Text = Convert.ToString(p6_list[maxIndex]);
                            PTC.textBox107.Text = Convert.ToString(10);
                            PTC.button1_Click(this, e);
                            puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area = PTC.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_Main_SF_Pressure_drop = PTC.Total_Pressure_DropResult;

                            LF_SF_Calculation LF = new LF_SF_Calculation();
                            LF.calledForSensingAnalysis = true;
                            LF.comboBox1.Text = "Solar Salt";
                            LF.comboBox2.Text = "NewMixture";
                            LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF.textBox1.Text = Convert.ToString(PHX_Q2_list[maxIndex]);
                            LF.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF.textBox3.Text = Convert.ToString(t5_list[maxIndex]);
                            LF.textBox6.Text = Convert.ToString(t6_list[maxIndex]);
                            LF.textBox4.Text = Convert.ToString(p5_list[maxIndex]);
                            LF.textBox5.Text = Convert.ToString(p6_list[maxIndex]);
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
                            PTC_RHX1.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX1.textBox1.Text = Convert.ToString(RHX1_Q2_list[maxIndex]);
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
                            LF_RHX1.textBox1.Text = Convert.ToString(RHX1_Q2_list[maxIndex]);
                            LF_RHX1.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF_RHX1.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                            LF_RHX1.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                            LF_RHX1.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                            LF_RHX1.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
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
                            PTC_RHX2.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX2.textBox1.Text = Convert.ToString(RHX2_Q2_list[maxIndex]);
                            PTC_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            PTC_RHX2.textBox3.Text = Convert.ToString(t13_list[maxIndex]);
                            PTC_RHX2.textBox6.Text = Convert.ToString(t14_list[maxIndex]);
                            PTC_RHX2.textBox4.Text = Convert.ToString(p13_list[maxIndex]);
                            PTC_RHX2.textBox5.Text = Convert.ToString(p14_list[maxIndex]);
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
                            LF_RHX2.textBox1.Text = Convert.ToString(RHX2_Q2_list[maxIndex]);
                            LF_RHX2.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF_RHX2.textBox3.Text = Convert.ToString(t13_list[maxIndex]);
                            LF_RHX2.textBox6.Text = Convert.ToString(t14_list[maxIndex]);
                            LF_RHX2.textBox4.Text = Convert.ToString(p13_list[maxIndex]);
                            LF_RHX2.textBox5.Text = Convert.ToString(p14_list[maxIndex]);
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
                            PTC_RHX3.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;
                            PTC_RHX3.textBox1.Text = Convert.ToString(RHX3_Q2_list[maxIndex]);
                            PTC_RHX3.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            PTC_RHX3.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                            PTC_RHX3.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                            PTC_RHX3.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                            PTC_RHX3.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                            PTC_RHX3.textBox107.Text = Convert.ToString(10);
                            PTC_RHX3.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area = PTC_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop = PTC_RHX3.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_RHX3 = new LF_SF_Calculation();
                            LF_RHX3.calledForSensingAnalysis = true;
                            LF_RHX3.comboBox1.Text = "Solar Salt";
                            LF_RHX3.comboBox2.Text = "NewMixture";
                            LF_RHX3.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX3.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX3.textBox1.Text = Convert.ToString(RHX3_Q2_list[maxIndex]);
                            LF_RHX3.textBox2.Text = Convert.ToString(massflow2_list[maxIndex]);
                            LF_RHX3.textBox3.Text = Convert.ToString(t15_list[maxIndex]);
                            LF_RHX3.textBox6.Text = Convert.ToString(t16_list[maxIndex]);
                            LF_RHX3.textBox4.Text = Convert.ToString(p15_list[maxIndex]);
                            LF_RHX3.textBox5.Text = Convert.ToString(p16_list[maxIndex]);
                            LF_RHX3.textBox107.Text = Convert.ToString(10);
                            LF_RHX3.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area = LF_RHX3.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop = LF_RHX3.Total_Pressure_DropResult;

                            //Copy results to EXCEL
                            double LTR_min_DT_1 = t8_list[maxIndex] - t3_list[maxIndex];
                            double LTR_min_DT_2 = t9_list[maxIndex] - t2_list[maxIndex];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = t8_list[maxIndex] - t4_list[maxIndex];
                            double HTR_min_DT_2 = t7_list[maxIndex] - t5_list[maxIndex];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //CIP
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_mc_in2_list[maxIndex]);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = ua_LT_list[maxIndex].ToString();
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = ua_HT_list[maxIndex].ToString();
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = recomp_frac2_list[maxIndex].ToString();
                            //P_rhx1_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = p_rhx1_in2_list[maxIndex].ToString();
                            //P_rhx2_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = p_rhx2_in2_list[maxIndex].ToString();
                            //P_rhx3_in
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = p_rhx3_in2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = LT_Eff_list[maxIndex].ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = HT_Eff_list[maxIndex].ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 17] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            //PTC_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 18] = puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 19] = puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop.ToString();
                            //LF_RHX1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 21] = puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop.ToString();
                            //PTC_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 22] = puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 23] = puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop.ToString();
                            //LF_RHX2_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 24] = puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX2_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 25] = puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop.ToString();
                            //PTC_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 26] = puntero_aplicacion.PTC_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //PTC_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 27] = puntero_aplicacion.PTC_ReHeating3_SF_Pressure_drop.ToString();
                            //LF_RHX3_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 28] = puntero_aplicacion.LF_ReHeating3_SF_Effective_Apperture_Area.ToString();
                            //LF_RHX3_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 29] = puntero_aplicacion.LF_ReHeating3_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_Pressure_value = puntero_aplicacion.p_mc_in2;
                        }
                    } //checkBox2.Checked (optimize UA)

                } //loop for CIT optimization analysis

            }

            //Closing Excel Book
            xlWorkBook1.SaveAs(textBox3.Text + "CIT_Optimization_RC_with_Three_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //Clear Lists
        private void Button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            listBox8.Items.Clear();
            listBox9.Items.Clear();
            listBox10.Items.Clear();
            listBox11.Items.Clear();
            listBox12.Items.Clear();
            listBox13.Items.Clear();
            listBox14.Items.Clear();
            listBox15.Items.Clear();
            listBox16.Items.Clear();
            listBox17.Items.Clear();
            listBox18.Items.Clear();
            listBox19.Items.Clear();
            listBox20.Items.Clear();
            listBox21.Items.Clear();
            listBox22.Items.Clear();
        }

        private void RC_with_Three_ReHeating_Optimization_Analysis_Results_Load(object sender, EventArgs e)
        {
            textBox1.Text = puntero_aplicacion.textBox59.Text;
        }
    }
}
