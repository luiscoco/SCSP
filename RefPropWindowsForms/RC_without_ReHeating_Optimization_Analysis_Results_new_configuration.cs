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
    public partial class RC_without_ReHeating_Optimization_Analysis_Results_new_configuration : Form
    {
        RC_without_ReHeating_new_proposed_configuration puntero_aplicacion;

        public RC_without_ReHeating_Optimization_Analysis_Results_new_configuration(RC_without_ReHeating_new_proposed_configuration puntero1)
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

        //Optimization button
        private void button3_Click(object sender, EventArgs e)
        {
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

            //UA optimization False
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc_in2;
                puntero_aplicacion.t_rht_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht_in2;
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = 0.0;
                puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                //puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle cicloRC_withRH = new core.RecompCycle();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> p_rhx_in2_list = new List<Double>();

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

                //Set Initial CIP
                if (checkBox6.Checked == true)
                {
                    initial_CIP_value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                }

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + ":" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox80.Text;
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
                xlWorkSheet1.Cells[4, 6] = "P_rhx_in(kPa)";
                xlWorkSheet1.Cells[4, 7] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 8] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 10] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "HTR Pinch(ºC)";

                //PRIMERA LLAMADA para la optimización
                double max_recomp_fraction = 0.0;
                double max_mc_p_in = 0.0;
                double temp5_max_eff = 0.0;

                List<Double> temp5_list_primera = new List<Double>();

                using (var solver = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                    solver.SetUpperBounds(new[] { 1.0, 125000 });

                    solver.SetInitialStepSize(new[] { 0.005, 50 });

                    var initialValue = new[] { 0.2, initial_CIP_value };

                    Func<double[], double> funcion = delegate (double[] variables1)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis, ref cicloRC_withRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2, variables1[1], puntero_aplicacion.p_mc_out2,
                        variables1[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        variables1[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables1[0];
                        puntero_aplicacion.p_mc_in2 = variables1[1];
                        //puntero_aplicacion.p_rhx_in2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRC_withRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                        double LTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH.temp[8] - cicloRC_withRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH.temp[6] - cicloRC_withRH.temp[4];
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
                        //P_rhx_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

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
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                    max_recomp_fraction = recomp_frac2_list[maxIndex];
                    max_mc_p_in = p_mc_in2_list[maxIndex];
                    temp5_max_eff = temp5_list_primera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);
                } //Fin de la PRIMERA LLAMADA para optimización

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

                //SEGUNDA LLAMADA
                double max_recomp_fraction_1 = 0.0;
                double max_mc_p_in_1 = 0.0;
                double temp5_max_eff_segunda = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.RecompCycle cicloRC_withRH_Segunda_llamada = new core.RecompCycle();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc_in2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver1 = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                {
                    solver1.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                    solver1.SetUpperBounds(new[] { 1.0, 125000 });

                    solver1.SetInitialStepSize(new[] { 0.005, 50 });

                    var initialValue = new[] { max_recomp_fraction, max_mc_p_in };

                    Func<double[], double> funcion = delegate (double[] variables2)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis, ref cicloRC_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, temp5_max_eff, variables2[1], puntero_aplicacion.p_mc_out2,
                        variables2[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        variables2[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH_Segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Segunda_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Segunda_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables2[0];
                        puntero_aplicacion.p_mc_in2 = variables2[1];
                        //puntero_aplicacion.p_rhx_in2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRC_withRH_Segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH_Segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH_Segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH_Segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH_Segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH_Segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH_Segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH_Segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH_Segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH_Segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH_Segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH_Segunda_llamada.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH_Segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH_Segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH_Segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH_Segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH_Segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH_Segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH_Segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH_Segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH_Segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH_Segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH_Segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH_Segunda_llamada.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Segunda_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH_Segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH_Segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH_Segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH_Segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH_Segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH_Segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH_Segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH_Segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH_Segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH_Segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH_Segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH_Segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH_Segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH_Segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH_Segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH_Segunda_llamada.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH_Segunda_llamada.PC.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                        double LTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[8] - cicloRC_withRH_Segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[6] - cicloRC_withRH_Segunda_llamada.temp[4];
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
                        //P_rhx_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver1.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver1.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                    var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                    textBox91.Text = p_mc_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                    max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                    max_mc_p_in_1 = p_mc_in2_list_segunda_llamada[maxIndex];
                    temp5_max_eff_segunda = temp5_list_segunda[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);
                } //Fin de la SEGUNDA LLAMADA para optimización

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

                //TERCERA LLAMADA
                double max_recomp_fraction_2 = 0.0;
                double max_mc_p_in_2 = 0.0;
                double temp5_max_eff_tercera = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.RecompCycle cicloRC_withRH_tercera_llamada = new core.RecompCycle();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc_in2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver2 = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                {
                    solver2.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                    solver2.SetUpperBounds(new[] { 1.0, 125000 });

                    solver2.SetInitialStepSize(new[] { 0.005, 50 });

                    var initialValue = new[] { max_recomp_fraction_1, max_mc_p_in_1 };

                    Func<double[], double> funcion = delegate (double[] variables3)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis, 
                        ref cicloRC_withRH_tercera_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, 
                        temp5_max_eff_segunda, variables3[1], puntero_aplicacion.p_mc_out2,
                        variables3[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, 
                        -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, 
                        -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, 
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        variables3[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, 
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH_tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_tercera_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_tercera_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables3[0];
                        puntero_aplicacion.p_mc_in2 = variables3[1];
                        //puntero_aplicacion.p_rhx_in2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRC_withRH_tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH_tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH_tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH_tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH_tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH_tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH_tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH_tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH_tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH_tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH_tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH_tercera_llamada.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH_tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH_tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH_tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH_tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH_tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH_tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH_tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH_tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH_tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH_tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH_tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH_tercera_llamada.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_tercera_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH_tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH_tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH_tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH_tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH_tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH_tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH_tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH_tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH_tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH_tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH_tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH_tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH_tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH_tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH_tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH_tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH_tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH_tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH_tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH_tercera_llamada.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH_tercera_llamada.PC.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                        double LTR_min_DT_1 = cicloRC_withRH_tercera_llamada.temp[7] - cicloRC_withRH_tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH_tercera_llamada.temp[8] - cicloRC_withRH_tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH_tercera_llamada.temp[7] - cicloRC_withRH_tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH_tercera_llamada.temp[6] - cicloRC_withRH_tercera_llamada.temp[4];
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
                        //P_rhx_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver2.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver2.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                    var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                    textBox91.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                    max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                    max_mc_p_in_2 = p_mc_in2_list_tercera_llamada[maxIndex];
                    temp5_max_eff_tercera = temp5_list_tercera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);
                } //Fin de la TERCERA LLAMADA para optimización

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

                //CUARTA LLAMADA
                double temp5_max_eff_cuarta = 0.0;
                List<Double> temp5_list_cuarta = new List<Double>();

                core.RecompCycle cicloRC_withRH_cuarta_llamada = new core.RecompCycle();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc_in2_list_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver3 = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                {
                    solver3.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                    solver3.SetUpperBounds(new[] { 1.0, 125000 });

                    solver3.SetInitialStepSize(new[] { 0.005, 50 });

                    var initialValue = new[] { max_recomp_fraction_2, max_mc_p_in_2 };

                    Func<double[], double> funcion = delegate (double[] variables4)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis, ref cicloRC_withRH_cuarta_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, temp5_max_eff_tercera, variables4[1], puntero_aplicacion.p_mc_out2,
                        variables4[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                        variables4[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH_cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_cuarta_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_cuarta_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables4[0];
                        puntero_aplicacion.p_mc_in2 = variables4[1];
                        //puntero_aplicacion.p_rhx_in2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRC_withRH_cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH_cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH_cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH_cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH_cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH_cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH_cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH_cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH_cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH_cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH_cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH_cuarta_llamada.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH_cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH_cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH_cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH_cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH_cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH_cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH_cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH_cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH_cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH_cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH_cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH_cuarta_llamada.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_cuarta_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH_cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH_cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH_cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH_cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH_cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH_cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH_cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH_cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH_cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH_cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH_cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH_cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH_cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH_cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH_cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH_cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH_cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH_cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH_cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH_cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH_cuarta_llamada.PC.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                        double LTR_min_DT_1 = cicloRC_withRH_cuarta_llamada.temp[7] - cicloRC_withRH_cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH_cuarta_llamada.temp[8] - cicloRC_withRH_cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH_cuarta_llamada.temp[7] - cicloRC_withRH_cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH_cuarta_llamada.temp[6] - cicloRC_withRH_cuarta_llamada.temp[4];
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
                        //P_rhx_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver3.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver3.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                    var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                    textBox91.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                } //Fin de la CUARTA LLAMADA para optimización




            }

            //-------------------------------------------------------------------------
            //UA optimization True
            else if (checkBox2.Checked == true)
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc_in2;
                puntero_aplicacion.t_rht_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht_in2;
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx1 = puntero_aplicacion.dp2_phx1;

                //puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle cicloRC_withRH = new core.RecompCycle();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> p_rhx_in2_list = new List<Double>();
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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + ":" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox80.Text;
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
                xlWorkSheet1.Cells[4, 6] = "P_rhx_in(kPa)";
                xlWorkSheet1.Cells[4, 7] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 8] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 10] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "HTR Pinch(ºC)";

                //PRIMERA LLAMADA para la optimización
                double max_recomp_fraction = 0.0;
                double max_mc_p_in = 0.0;
                double temp5_max_eff = 0.0;

                List<Double> temp5_list_primera = new List<Double>();

                using (var solver = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 125000, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.005, 50, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, 
                        ref cicloRC_withRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2, 
                        variables[1], puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, 
                        -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, 
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, variables[2], UA_Total,
                        variables[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, 
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc_in2 = variables[1];
                        LT_fraction = variables[2];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRC_withRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                        double LTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH.temp[8] - cicloRC_withRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH.temp[6] - cicloRC_withRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

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
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction = recomp_frac2_list[maxIndex];
                    max_mc_p_in = p_mc_in2_list[maxIndex];
                    temp5_max_eff = temp5_list_primera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);
                } //Fin de la PRIMERA LLAMADA para optimización

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

                //SEGUNDA LLAMADA
                double max_recomp_fraction_1 = 0.0;
                double max_mc_p_in_1 = 0.0;
                double temp5_max_eff_segunda = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.RecompCycle cicloRC_withRH_Segunda_llamada = new core.RecompCycle();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc_in2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver1 = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                {
                    solver1.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                    solver1.SetUpperBounds(new[] { 1.0, 125000, 1.0 });

                    solver1.SetInitialStepSize(new[] { 0.005, 50, 0.05 });

                    var initialValue = new[] { max_recomp_fraction, max_mc_p_in, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, 
                        ref cicloRC_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, 
                        temp5_max_eff, variables[1], puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_rht_in2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, 
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                        -puntero_aplicacion.dp2_ht2, variables[2], UA_Total, variables[0], puntero_aplicacion.eta_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, 
                        puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH_Segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Segunda_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Segunda_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc_in2 = variables[1];
                        LT_fraction = variables[2];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRC_withRH_Segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH_Segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH_Segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH_Segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH_Segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH_Segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH_Segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH_Segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH_Segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH_Segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH_Segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH_Segunda_llamada.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH_Segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH_Segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH_Segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH_Segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH_Segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH_Segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH_Segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH_Segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH_Segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH_Segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH_Segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH_Segunda_llamada.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Segunda_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH_Segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH_Segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH_Segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH_Segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH_Segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH_Segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH_Segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH_Segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH_Segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH_Segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH_Segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH_Segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH_Segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH_Segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH_Segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH_Segunda_llamada.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH_Segunda_llamada.PC.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc_in2);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                        double LTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[8] - cicloRC_withRH_Segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[6] - cicloRC_withRH_Segunda_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver1.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver1.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                    var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                    puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                    puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                    textBox91.Text = p_mc_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                    max_mc_p_in_1 = p_mc_in2_list_segunda_llamada[maxIndex];
                    temp5_max_eff_segunda = temp5_list_segunda[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list_segunda_llamada[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Fin de la SEGUNDA LLAMADA para optimización  

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

                //TERCERA LLAMADA
                double max_recomp_fraction_2 = 0.0;
                double max_mc_p_in_2 = 0.0;
                double temp5_max_eff_tercera = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.RecompCycle cicloRC_withRH_Tercera_llamada = new core.RecompCycle();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc_in2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver2 = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                {
                    solver2.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                    solver2.SetUpperBounds(new[] { 1.0, 125000, 1.0 });

                    solver2.SetInitialStepSize(new[] { 0.005, 50, 0.05 });

                    var initialValue = new[] { max_recomp_fraction_1, max_mc_p_in_1, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withRH_Tercera_llamada,
                        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, temp5_max_eff_segunda, variables[1], puntero_aplicacion.p_mc_out2,
                        variables[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                        -puntero_aplicacion.dp2_ht2, variables[2], UA_Total, variables[0], puntero_aplicacion.eta_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH_Tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Tercera_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Tercera_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc_in2 = variables[1];
                        LT_fraction = variables[2];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRC_withRH_Tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH_Tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH_Tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH_Tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH_Tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH_Tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH_Tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH_Tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH_Tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH_Tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH_Tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH_Tercera_llamada.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH_Tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH_Tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH_Tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH_Tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH_Tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH_Tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH_Tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH_Tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH_Tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH_Tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH_Tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH_Tercera_llamada.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Tercera_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH_Tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH_Tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH_Tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH_Tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH_Tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH_Tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH_Tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH_Tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH_Tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH_Tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH_Tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH_Tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH_Tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH_Tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH_Tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH_Tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH_Tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH_Tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH_Tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH_Tercera_llamada.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH_Tercera_llamada.PC.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc_in2);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                        double LTR_min_DT_1 = cicloRC_withRH_Tercera_llamada.temp[7] - cicloRC_withRH_Tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH_Tercera_llamada.temp[8] - cicloRC_withRH_Tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH_Tercera_llamada.temp[7] - cicloRC_withRH_Tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH_Tercera_llamada.temp[6] - cicloRC_withRH_Tercera_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver2.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver2.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                    var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                    puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                    puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                    textBox91.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                    max_mc_p_in_2 = p_mc_in2_list_tercera_llamada[maxIndex];
                    temp5_max_eff_tercera = temp5_list_tercera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    ////releaseObject(xlWorkSheet2);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Fin de la TERCERA LLAMADA para optimización  

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

                //CUARTA LLAMADA
                double max_recomp_fraction_3 = 0.0;
                double max_mc_p_in_3 = 0.0;
                double max_lt_fraction_3 = 0.0;
                double temp5_max_eff_cuarta = 0.0;

                List<Double> temp5_list_cuarta = new List<Double>();

                core.RecompCycle cicloRC_withRH_Cuarta_llamada = new core.RecompCycle();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc_in2_list_cuarta_llamada = new List<Double>();
                List<Double> lt_fraction_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver3 = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                {
                    solver3.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                    solver3.SetUpperBounds(new[] { 1.0, 125000, 1.0 });

                    solver3.SetInitialStepSize(new[] { 0.005, 50, 0.05 });

                    var initialValue = new[] { max_recomp_fraction_2, max_mc_p_in_2, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withRH_Cuarta_llamada,
                        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, temp5_max_eff_tercera, variables[1], puntero_aplicacion.p_mc_out2,
                        variables[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                        -puntero_aplicacion.dp2_ht2, variables[2], UA_Total, variables[0], puntero_aplicacion.eta_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRC_withRH_Cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Cuarta_llamada.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Cuarta_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc_in2 = variables[1];
                        LT_fraction = variables[2];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRC_withRH_Cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRC_withRH_Cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRC_withRH_Cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRC_withRH_Cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRC_withRH_Cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRC_withRH_Cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRC_withRH_Cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRC_withRH_Cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRC_withRH_Cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRC_withRH_Cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRC_withRH_Cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRC_withRH_Cuarta_llamada.temp[11];

                        puntero_aplicacion.pres21 = cicloRC_withRH_Cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRC_withRH_Cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRC_withRH_Cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRC_withRH_Cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRC_withRH_Cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRC_withRH_Cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRC_withRH_Cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRC_withRH_Cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRC_withRH_Cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRC_withRH_Cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRC_withRH_Cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRC_withRH_Cuarta_llamada.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Cuarta_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRC_withRH_Cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRC_withRH_Cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRC_withRH_Cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRC_withRH_Cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRC_withRH_Cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRC_withRH_Cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRC_withRH_Cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRC_withRH_Cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRC_withRH_Cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRC_withRH_Cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRC_withRH_Cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRC_withRH_Cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRC_withRH_Cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRC_withRH_Cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRC_withRH_Cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRC_withRH_Cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRC_withRH_Cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRC_withRH_Cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRC_withRH_Cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRC_withRH_Cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRC_withRH_Cuarta_llamada.PC.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc_in2);
                        lt_fraction_cuarta_llamada.Add(LT_fraction);
                        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                        double LTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[8] - cicloRC_withRH_Cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[6] - cicloRC_withRH_Cuarta_llamada.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver3.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver3.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                    var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                    puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                    puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                    textBox91.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                    max_mc_p_in_3 = p_mc_in2_list_cuarta_llamada[maxIndex];
                    temp5_max_eff_cuarta = temp5_list_cuarta[maxIndex];
                    max_lt_fraction_3 = lt_fraction_cuarta_llamada[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);

                } //Fin de la CUARTA LLAMADA para optimización  

                ////QUINTA LLAMADA
                //double max_recomp_fraction_4 = 0.0;
                //double max_mc_p_in_4 = 0.0;
                //double temp5_max_eff_quinta = 0.0;

                //List<Double> temp5_list_quinta = new List<Double>();

                //core.RecompCycle cicloRC_withRH_quinta_llamada = new core.RecompCycle();

                //List<Double> recomp_frac2_list_quinta_llamada = new List<Double>();
                //List<Double> p_mc_in2_list_quinta_llamada = new List<Double>();
                //List<Double> eta_thermal2_list_quinta_llamada = new List<Double>();
                //List<Double> p_rhx_in2_list_quinta_llamada = new List<Double>();

                //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                //xlWorkSheet1.Activate();

                //using (var solver4 = new NLoptSolver(algorithm_type, 1, 0.00001, 10000))
                //{
                //    solver4.SetLowerBounds(new[] { initial_CIP_value });
                //    solver4.SetUpperBounds(new[] { 125000.0 });

                //    solver4.SetInitialStepSize(new[] { 10.0});

                //    var initialValue = new[] { 7400.0 };

                //    Func<double[], double> funcion = delegate (double[] variables)
                //    {
                //        puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withRH_quinta_llamada,
                //        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, temp5_max_eff_cuarta, variables[0], puntero_aplicacion.p_mc_out2,
                //        variables[0], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1,
                //        -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                //        -puntero_aplicacion.dp2_ht2, max_lt_fraction_3, UA_Total, max_recomp_fraction_3, puntero_aplicacion.eta_mc2,
                //        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                //        puntero_aplicacion.tol2);

                //        counter++;

                //        puntero_aplicacion.massflow2 = cicloRC_withRH_quinta_llamada.m_dot_turbine;
                //        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_quinta_llamada.W_dot_net;
                //        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_quinta_llamada.eta_thermal;
                //        //puntero_aplicacion.recomp_frac2 = variables[0];
                //        puntero_aplicacion.p_mc_in2 = variables[0];
                //        //LT_fraction = variables[2];
                //        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                //        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                //        puntero_aplicacion.temp21 = cicloRC_withRH_quinta_llamada.temp[0];
                //        puntero_aplicacion.temp22 = cicloRC_withRH_quinta_llamada.temp[1];
                //        puntero_aplicacion.temp23 = cicloRC_withRH_quinta_llamada.temp[2];
                //        puntero_aplicacion.temp24 = cicloRC_withRH_quinta_llamada.temp[3];
                //        puntero_aplicacion.temp25 = cicloRC_withRH_quinta_llamada.temp[4];
                //        puntero_aplicacion.temp26 = cicloRC_withRH_quinta_llamada.temp[5];
                //        puntero_aplicacion.temp27 = cicloRC_withRH_quinta_llamada.temp[6];
                //        puntero_aplicacion.temp28 = cicloRC_withRH_quinta_llamada.temp[7];
                //        puntero_aplicacion.temp29 = cicloRC_withRH_quinta_llamada.temp[8];
                //        puntero_aplicacion.temp210 = cicloRC_withRH_quinta_llamada.temp[9];
                //        puntero_aplicacion.temp211 = cicloRC_withRH_quinta_llamada.temp[10];
                //        puntero_aplicacion.temp212 = cicloRC_withRH_quinta_llamada.temp[11];

                //        puntero_aplicacion.pres21 = cicloRC_withRH_quinta_llamada.pres[0];
                //        puntero_aplicacion.pres22 = cicloRC_withRH_quinta_llamada.pres[1];
                //        puntero_aplicacion.pres23 = cicloRC_withRH_quinta_llamada.pres[2];
                //        puntero_aplicacion.pres24 = cicloRC_withRH_quinta_llamada.pres[3];
                //        puntero_aplicacion.pres25 = cicloRC_withRH_quinta_llamada.pres[4];
                //        puntero_aplicacion.pres26 = cicloRC_withRH_quinta_llamada.pres[5];
                //        puntero_aplicacion.pres27 = cicloRC_withRH_quinta_llamada.pres[6];
                //        puntero_aplicacion.pres28 = cicloRC_withRH_quinta_llamada.pres[7];
                //        puntero_aplicacion.pres29 = cicloRC_withRH_quinta_llamada.pres[8];
                //        puntero_aplicacion.pres210 = cicloRC_withRH_quinta_llamada.pres[9];
                //        puntero_aplicacion.pres211 = cicloRC_withRH_quinta_llamada.pres[10];
                //        puntero_aplicacion.pres212 = cicloRC_withRH_quinta_llamada.pres[11];

                //        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_quinta_llamada.PHX.Q_dot;
                //        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_quinta_llamada.RHX.Q_dot;

                //        puntero_aplicacion.LT_Q = cicloRC_withRH_quinta_llamada.LT.Q_dot;
                //        puntero_aplicacion.LT_mdotc = cicloRC_withRH_quinta_llamada.LT.m_dot_design[0];
                //        puntero_aplicacion.LT_mdoth = cicloRC_withRH_quinta_llamada.LT.m_dot_design[1];
                //        puntero_aplicacion.LT_Tcin = cicloRC_withRH_quinta_llamada.LT.T_c_in;
                //        puntero_aplicacion.LT_Thin = cicloRC_withRH_quinta_llamada.LT.T_h_in;
                //        puntero_aplicacion.LT_Pcin = cicloRC_withRH_quinta_llamada.LT.P_c_in;
                //        puntero_aplicacion.LT_Phin = cicloRC_withRH_quinta_llamada.LT.P_h_in;
                //        puntero_aplicacion.LT_Pcout = cicloRC_withRH_quinta_llamada.LT.P_c_out;
                //        puntero_aplicacion.LT_Phout = cicloRC_withRH_quinta_llamada.LT.P_h_out;
                //        puntero_aplicacion.LT_Effc = cicloRC_withRH_quinta_llamada.LT.eff;

                //        puntero_aplicacion.HT_Q = cicloRC_withRH_quinta_llamada.HT.Q_dot;
                //        puntero_aplicacion.HT_mdotc = cicloRC_withRH_quinta_llamada.HT.m_dot_design[0];
                //        puntero_aplicacion.HT_mdoth = cicloRC_withRH_quinta_llamada.HT.m_dot_design[1];
                //        puntero_aplicacion.HT_Tcin = cicloRC_withRH_quinta_llamada.HT.T_c_in;
                //        puntero_aplicacion.HT_Thin = cicloRC_withRH_quinta_llamada.HT.T_h_in;
                //        puntero_aplicacion.HT_Pcin = cicloRC_withRH_quinta_llamada.HT.P_c_in;
                //        puntero_aplicacion.HT_Phin = cicloRC_withRH_quinta_llamada.HT.P_h_in;
                //        puntero_aplicacion.HT_Pcout = cicloRC_withRH_quinta_llamada.HT.P_c_out;
                //        puntero_aplicacion.HT_Phout = cicloRC_withRH_quinta_llamada.HT.P_h_out;
                //        puntero_aplicacion.HT_Effc = cicloRC_withRH_quinta_llamada.HT.eff;

                //        puntero_aplicacion.PC_Q2 = cicloRC_withRH_quinta_llamada.PC.Q_dot;

                //        eta_thermal2_list_quinta_llamada.Add(puntero_aplicacion.eta_thermal2);
                //        recomp_frac2_list_quinta_llamada.Add(puntero_aplicacion.recomp_frac2);
                //        p_mc_in2_list_quinta_llamada.Add(puntero_aplicacion.p_mc_in2);
                //        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                //        temp5_list_quinta.Add(puntero_aplicacion.temp25);
                //        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                //        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                //        listBox1.Items.Add(counter.ToString());
                //        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                //        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                //        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                //        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                //        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                //        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                //        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                //        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                //        double LTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[2];
                //        double LTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[8] - cicloRC_withRH_Cuarta_llamada.temp[1];
                //        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                //        double HTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[3];
                //        double HTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[6] - cicloRC_withRH_Cuarta_llamada.temp[4];
                //        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                //        //CIP
                //        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                //        //CIT
                //        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                //        //LT UA(kW/K)
                //        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                //        //HT UA(kW/K)
                //        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                //        //Rec.Frac.
                //        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                //        //P_rhx_in
                //        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                //        //Eff.(%)
                //        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                //        //LTR Eff.(%)
                //        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                //        //LTR Pinch(ºC)
                //        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                //        //HTR Eff.(%)
                //        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                //        //HTR Pinch(ºC)
                //        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                //        counter_Excel++;

                //        return puntero_aplicacion.eta_thermal2;
                //    };

                //    solver4.SetMaxObjective(funcion);

                //    double? finalScore;

                //    var result = solver4.Optimize(initialValue, out finalScore);

                //    Double max_eta_thermal = 0.0;

                //    max_eta_thermal = eta_thermal2_list_quinta_llamada.Max();

                //    var maxIndex = eta_thermal2_list_quinta_llamada.IndexOf(eta_thermal2_list_quinta_llamada.Max());

                //    puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                //    puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                //    textBox91.Text = p_mc_in2_list_quinta_llamada[maxIndex].ToString();
                //    textBox90.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                //    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                //    textBox86.Text = eta_thermal2_list_quinta_llamada[maxIndex].ToString();
                //    textBox82.Text = ua_LT_list[maxIndex].ToString();
                //    textBox83.Text = ua_HT_list[maxIndex].ToString();

                //    //Copy results as design-point inputs
                //    if (checkBox3.Checked == true)
                //    {
                //        puntero_aplicacion.textBox15.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                //        puntero_aplicacion.textBox3.Text = p_mc_in2_list_quinta_llamada[maxIndex].ToString();
                //        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                //        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                //        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                //    }

                //    //Closing Excel Book
                //    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                //    xlWorkBook1.Close(true, misValue1, misValue1);
                //    xlApp1.Quit();

                //    releaseObject(xlWorkSheet1);
                //    //releaseObject(xlWorkSheet2);
                //    releaseObject(xlWorkBook1);
                //    releaseObject(xlApp1);

                //} //Fin de la QUINTA LLAMADA para optimización  


            }
        
        }

        //Run CIT Optimization Button
        private void button6_Click(object sender, EventArgs e)
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

            for (double i = Convert.ToDouble(textBox57.Text); i <= Convert.ToDouble(textBox56.Text); i = i + Convert.ToDouble(textBox55.Text))
            {
                counter = 0;

                //UA optimization False
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
                        puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text, puntero_aplicacion.category);
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
                    puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                    puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                    puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc_in2;
                    puntero_aplicacion.t_rht_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                    puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht_in2;
                    puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                    puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                    puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                    puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                    puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                    puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                    puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                    puntero_aplicacion.dp2_phx1 = 0.0;
                    puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                    //puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                    puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                    puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                    puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                    puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                    puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                    puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                    puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                    core.RecompCycle cicloRC_withRH = new core.RecompCycle();

                    double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                    double LT_fraction = 0.1;

                    List<Double> massflow2_list = new List<Double>();
                    List<Double> recomp_frac2_list = new List<Double>();
                    List<Double> p_mc_in2_list = new List<Double>();
                    List<Double> eta_thermal2_list = new List<Double>();
                    List<Double> p_rhx_in2_list = new List<Double>();
                    List<Double> PHX_Q2_list = new List<Double>();
                    List<Double> ua_lt_list = new List<Double>();
                    List<Double> ua_ht_list = new List<Double>();

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
                        //Set Initial CIP
                        if (checkBox6.Checked == true)
                        {
                            initial_CIP_value = Convert.ToDouble(textBox1.Text);
                        }
                        else
                        {
                            initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                        }

                        xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                        xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + ":" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox80.Text;
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
                        xlWorkSheet1.Cells[4, 6] = "P_rhx_in(kPa)";
                        xlWorkSheet1.Cells[4, 7] = "Eff.(%)";
                        xlWorkSheet1.Cells[4, 8] = "LTR Eff.(%)";
                        xlWorkSheet1.Cells[4, 9] = "LTR Pinch(ºC)";
                        xlWorkSheet1.Cells[4, 10] = "HTR Eff.(%)";
                        xlWorkSheet1.Cells[4, 11] = "HTR Pinch(ºC)";

                        if (checkBox7.Checked == false)
                        {
                            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";
                        }
                    }

                    //PRIMERA LLAMADA para la optimización
                    double max_recomp_fraction = 0.0;
                    double max_mc_p_in = 0.0;
                    double temp5_max_eff = 0.0;

                    List<Double> temp5_list_primera = new List<Double>();

                    using (var solver = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                    {
                        solver.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                        solver.SetUpperBounds(new[] { 1.0, 12500.0 });

                        solver.SetInitialStepSize(new[] { 0.005, 50.0 });

                        var initialValue = new[] { 0.2, initial_CIP_value };

                        Func<double[], double> funcion = delegate (double[] variables1)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis, ref cicloRC_withRH, puntero_aplicacion.w_dot_net2, i, puntero_aplicacion.t_t_in2, variables1[1], puntero_aplicacion.p_mc_out2,
                            variables1[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1,
                            -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                            variables1[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables1[0];
                            puntero_aplicacion.p_mc_in2 = variables1[1];
                            //puntero_aplicacion.p_rhx_in2 = variables[2];

                            puntero_aplicacion.temp21 = cicloRC_withRH.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH.PC.Q_dot;

                            eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                            temp5_list_primera.Add(puntero_aplicacion.temp25);
                            //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                            double LTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH.temp[8] - cicloRC_withRH.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH.temp[6] - cicloRC_withRH.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

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
                        //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                        max_recomp_fraction = recomp_frac2_list[maxIndex];
                        max_mc_p_in = p_mc_in2_list[maxIndex];
                        temp5_max_eff = temp5_list_primera[maxIndex];

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        }

                        if (i == Convert.ToDouble(textBox57.Text))
                        {
                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);
                        }
                    } //Fin de la PRIMERA LLAMADA para optimización

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

                    //SEGUNDA LLAMADA
                    double max_recomp_fraction_1 = 0.0;
                    double max_mc_p_in_1 = 0.0;
                    double temp5_max_eff_segunda = 0.0;

                    List<Double> temp5_list_segunda = new List<Double>();

                    core.RecompCycle cicloRC_withRH_Segunda_llamada = new core.RecompCycle();

                    List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                    List<Double> p_mc_in2_list_segunda_llamada = new List<Double>();
                    List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                    List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();

                    //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                    //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                    //xlWorkSheet1.Activate();

                    using (var solver1 = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                    {
                        solver1.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                        solver1.SetUpperBounds(new[] { 1.0, 12500.0 });

                        solver1.SetInitialStepSize(new[] { 0.005, 50.0 });

                        var initialValue = new[] { max_recomp_fraction, max_mc_p_in };

                        Func<double[], double> funcion = delegate (double[] variables2)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis, 
                            ref cicloRC_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, i,
                            temp5_max_eff, variables2[1], puntero_aplicacion.p_mc_out2,
                            variables2[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, 
                            -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                            -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2,
                            puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                            variables2[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, 
                            puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                            puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH_Segunda_llamada.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Segunda_llamada.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Segunda_llamada.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables2[0];
                            puntero_aplicacion.p_mc_in2 = variables2[1];
                            //puntero_aplicacion.p_rhx_in2 = variables[2];

                            puntero_aplicacion.temp21 = cicloRC_withRH_Segunda_llamada.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH_Segunda_llamada.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH_Segunda_llamada.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH_Segunda_llamada.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH_Segunda_llamada.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH_Segunda_llamada.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH_Segunda_llamada.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH_Segunda_llamada.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH_Segunda_llamada.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH_Segunda_llamada.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH_Segunda_llamada.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH_Segunda_llamada.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH_Segunda_llamada.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH_Segunda_llamada.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH_Segunda_llamada.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH_Segunda_llamada.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH_Segunda_llamada.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH_Segunda_llamada.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH_Segunda_llamada.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH_Segunda_llamada.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH_Segunda_llamada.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH_Segunda_llamada.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH_Segunda_llamada.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH_Segunda_llamada.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Segunda_llamada.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Segunda_llamada.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH_Segunda_llamada.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH_Segunda_llamada.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH_Segunda_llamada.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH_Segunda_llamada.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH_Segunda_llamada.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH_Segunda_llamada.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH_Segunda_llamada.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH_Segunda_llamada.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH_Segunda_llamada.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH_Segunda_llamada.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH_Segunda_llamada.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH_Segunda_llamada.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH_Segunda_llamada.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH_Segunda_llamada.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH_Segunda_llamada.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH_Segunda_llamada.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH_Segunda_llamada.PC.Q_dot;

                            eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc_in2);
                            temp5_list_segunda.Add(puntero_aplicacion.temp25);
                            //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                            double LTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[8] - cicloRC_withRH_Segunda_llamada.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[6] - cicloRC_withRH_Segunda_llamada.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

                            return puntero_aplicacion.eta_thermal2;
                        };

                        solver1.SetMaxObjective(funcion);

                        double? finalScore;

                        var result = solver1.Optimize(initialValue, out finalScore);

                        Double max_eta_thermal = 0.0;

                        max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                        var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                        textBox91.Text = p_mc_in2_list_segunda_llamada[maxIndex].ToString();
                        textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                        textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                        textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                        textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                        //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                        max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                        max_mc_p_in_1 = p_mc_in2_list_segunda_llamada[maxIndex];
                        temp5_max_eff_segunda = temp5_list_segunda[maxIndex];

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        //xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        //xlWorkBook1.Close(true, misValue1, misValue1);
                        //xlApp1.Quit();

                        //releaseObject(xlWorkSheet1);
                        //releaseObject(xlWorkSheet2);
                        //releaseObject(xlWorkBook1);
                        //releaseObject(xlApp1);
                    } //Fin de la SEGUNDA LLAMADA para optimización

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

                    //TERCERA LLAMADA
                    double max_recomp_fraction_2 = 0.0;
                    double max_mc_p_in_2 = 0.0;
                    double temp5_max_eff_tercera = 0.0;

                    List<Double> temp5_list_tercera = new List<Double>();

                    core.RecompCycle cicloRC_withRH_tercera_llamada = new core.RecompCycle();

                    List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                    List<Double> p_mc_in2_list_tercera_llamada = new List<Double>();
                    List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                    List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();

                    //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                    //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                    //xlWorkSheet1.Activate();

                    using (var solver2 = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                    {
                        solver2.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                        solver2.SetUpperBounds(new[] { 1.0, 12500.0 });

                        solver2.SetInitialStepSize(new[] { 0.005, 50.0 });

                        var initialValue = new[] { max_recomp_fraction_1, max_mc_p_in_1 };

                        Func<double[], double> funcion = delegate (double[] variables3)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis,
                            ref cicloRC_withRH_tercera_llamada, puntero_aplicacion.w_dot_net2, i,
                            temp5_max_eff_segunda, variables3[1], puntero_aplicacion.p_mc_out2,
                            variables3[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1,
                            -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                            -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2,
                            puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                            variables3[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2,
                            puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH_tercera_llamada.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH_tercera_llamada.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH_tercera_llamada.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables3[0];
                            puntero_aplicacion.p_mc_in2 = variables3[1];
                            //puntero_aplicacion.p_rhx_in2 = variables[2];

                            puntero_aplicacion.temp21 = cicloRC_withRH_tercera_llamada.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH_tercera_llamada.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH_tercera_llamada.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH_tercera_llamada.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH_tercera_llamada.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH_tercera_llamada.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH_tercera_llamada.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH_tercera_llamada.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH_tercera_llamada.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH_tercera_llamada.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH_tercera_llamada.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH_tercera_llamada.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH_tercera_llamada.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH_tercera_llamada.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH_tercera_llamada.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH_tercera_llamada.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH_tercera_llamada.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH_tercera_llamada.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH_tercera_llamada.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH_tercera_llamada.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH_tercera_llamada.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH_tercera_llamada.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH_tercera_llamada.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH_tercera_llamada.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH_tercera_llamada.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH_tercera_llamada.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH_tercera_llamada.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH_tercera_llamada.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH_tercera_llamada.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH_tercera_llamada.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH_tercera_llamada.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH_tercera_llamada.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH_tercera_llamada.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH_tercera_llamada.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH_tercera_llamada.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH_tercera_llamada.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH_tercera_llamada.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH_tercera_llamada.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH_tercera_llamada.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH_tercera_llamada.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH_tercera_llamada.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH_tercera_llamada.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH_tercera_llamada.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH_tercera_llamada.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH_tercera_llamada.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH_tercera_llamada.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH_tercera_llamada.PC.Q_dot;

                            eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc_in2);
                            temp5_list_tercera.Add(puntero_aplicacion.temp25);
                            //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                            double LTR_min_DT_1 = cicloRC_withRH_tercera_llamada.temp[7] - cicloRC_withRH_tercera_llamada.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH_tercera_llamada.temp[8] - cicloRC_withRH_tercera_llamada.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH_tercera_llamada.temp[7] - cicloRC_withRH_tercera_llamada.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH_tercera_llamada.temp[6] - cicloRC_withRH_tercera_llamada.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

                            return puntero_aplicacion.eta_thermal2;
                        };

                        solver2.SetMaxObjective(funcion);

                        double? finalScore;

                        var result = solver2.Optimize(initialValue, out finalScore);

                        Double max_eta_thermal = 0.0;

                        max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                        var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                        textBox91.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                        textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                        textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                        textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                        //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                        max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                        max_mc_p_in_2 = p_mc_in2_list_tercera_llamada[maxIndex];
                        temp5_max_eff_tercera = temp5_list_tercera[maxIndex];

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        //xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        //xlWorkBook1.Close(true, misValue1, misValue1);
                        //xlApp1.Quit();

                        //releaseObject(xlWorkSheet1);
                        //releaseObject(xlWorkSheet2);
                        //releaseObject(xlWorkBook1);
                        //releaseObject(xlApp1);
                    } //Fin de la TERCERA LLAMADA para optimización

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

                    //CUARTA LLAMADA
                    double temp5_max_eff_cuarta = 0.0;
                    List<Double> temp5_list_cuarta = new List<Double>();

                    core.RecompCycle cicloRC_withRH_cuarta_llamada = new core.RecompCycle();

                    List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                    List<Double> p_mc_in2_list_cuarta_llamada = new List<Double>();
                    List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                    List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();

                    xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                    xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                    xlWorkSheet1.Activate();

                    using (var solver3 = new NLoptSolver(algorithm_type, 2, 0.00001, 10000))
                    {
                        solver3.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                        solver3.SetUpperBounds(new[] { 1.0, 12500.0 });

                        solver3.SetInitialStepSize(new[] { 0.005, 50.0 });

                        var initialValue = new[] { max_recomp_fraction_2, max_mc_p_in_2 };

                        Func<double[], double> funcion = delegate (double[] variables4)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed(puntero_aplicacion.luis, 
                            ref cicloRC_withRH_cuarta_llamada, puntero_aplicacion.w_dot_net2, 
                            i, temp5_max_eff_tercera, variables4[1], puntero_aplicacion.p_mc_out2,
                            variables4[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, 
                            -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, 
                            -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, 
                            puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2,
                            variables4[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, 
                            puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, 
                            puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH_cuarta_llamada.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH_cuarta_llamada.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH_cuarta_llamada.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables4[0];
                            puntero_aplicacion.p_mc_in2 = variables4[1];
                            puntero_aplicacion.p_rhx_in2 = variables4[1];

                            puntero_aplicacion.temp21 = cicloRC_withRH_cuarta_llamada.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH_cuarta_llamada.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH_cuarta_llamada.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH_cuarta_llamada.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH_cuarta_llamada.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH_cuarta_llamada.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH_cuarta_llamada.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH_cuarta_llamada.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH_cuarta_llamada.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH_cuarta_llamada.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH_cuarta_llamada.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH_cuarta_llamada.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH_cuarta_llamada.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH_cuarta_llamada.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH_cuarta_llamada.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH_cuarta_llamada.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH_cuarta_llamada.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH_cuarta_llamada.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH_cuarta_llamada.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH_cuarta_llamada.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH_cuarta_llamada.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH_cuarta_llamada.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH_cuarta_llamada.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH_cuarta_llamada.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH_cuarta_llamada.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH_cuarta_llamada.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH_cuarta_llamada.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH_cuarta_llamada.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH_cuarta_llamada.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH_cuarta_llamada.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH_cuarta_llamada.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH_cuarta_llamada.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH_cuarta_llamada.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH_cuarta_llamada.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH_cuarta_llamada.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH_cuarta_llamada.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH_cuarta_llamada.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH_cuarta_llamada.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH_cuarta_llamada.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH_cuarta_llamada.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH_cuarta_llamada.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH_cuarta_llamada.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH_cuarta_llamada.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH_cuarta_llamada.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH_cuarta_llamada.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH_cuarta_llamada.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH_cuarta_llamada.PC.Q_dot;

                            massflow2_list.Add(puntero_aplicacion.massflow2);
                            eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc_in2);
                            temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                            p_rhx_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                            ua_lt_list.Add(puntero_aplicacion.ua_lt2);
                            ua_ht_list.Add(puntero_aplicacion.ua_ht2);

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

                            PHX_Q2_list.Add(cicloRC_withRH_cuarta_llamada.PHX.Q_dot);

                            HT_Eff_list.Add(cicloRC_withRH_cuarta_llamada.HT.eff);
                            LT_Eff_list.Add(cicloRC_withRH_cuarta_llamada.LT.eff);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());

                            double LTR_min_DT_1 = cicloRC_withRH_cuarta_llamada.temp[7] - cicloRC_withRH_cuarta_llamada.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH_cuarta_llamada.temp[8] - cicloRC_withRH_cuarta_llamada.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH_cuarta_llamada.temp[7] - cicloRC_withRH_cuarta_llamada.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH_cuarta_llamada.temp[6] - cicloRC_withRH_cuarta_llamada.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

                            return puntero_aplicacion.eta_thermal2;
                        };

                        solver3.SetMaxObjective(funcion);

                        double? finalScore;

                        var result = solver3.Optimize(initialValue, out finalScore);

                        Double max_eta_thermal = 0.0;

                        max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                        var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                        textBox91.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                        textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                        textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                        textBox83.Text = puntero_aplicacion.ua_ht2.ToString();
                        textBox2.Text = p_rhx_in2_list[maxIndex].ToString();

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                        }

                        //The variable 'i' is the loop counter for the CIT
                        listBox18.Items.Add(i.ToString());
                        listBox17.Items.Add(eta_thermal2_list_cuarta_llamada[maxIndex].ToString());
                        listBox16.Items.Add(recomp_frac2_list_cuarta_llamada[maxIndex].ToString());
                        listBox15.Items.Add(p_mc_in2_list_cuarta_llamada[maxIndex].ToString());
                        listBox10.Items.Add(p_rhx_in2_list_cuarta_llamada[maxIndex].ToString());
                        listBox11.Items.Add(t5_list[maxIndex].ToString());
                        listBox12.Items.Add(t6_list[maxIndex].ToString());
                        listBox13.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox14.Items.Add(puntero_aplicacion.ua_ht2.ToString());

                        if (checkBox7.Checked == false)
                        {

                            //Calculo del campo solar
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "NewMixture";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
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
                        }

                        //Copy results to EXCEL
                        double LTR_min_DT_1_max = t8_list[maxIndex] - t3_list[maxIndex];
                        double LTR_min_DT_2_max = t9_list[maxIndex] - t2_list[maxIndex];
                        double LTR_min_DT_paper_max = Math.Min(LTR_min_DT_1_max, LTR_min_DT_2_max);

                        double HTR_min_DT_1_max = t8_list[maxIndex] - t4_list[maxIndex];
                        double HTR_min_DT_2_max = t7_list[maxIndex] - t5_list[maxIndex];
                        double HTR_min_DT_paper_max = Math.Min(HTR_min_DT_1_max, HTR_min_DT_2_max);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_mc_in2_list_cuarta_llamada[maxIndex]);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(i - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = (eta_thermal2_list_cuarta_llamada[maxIndex] * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = LT_Eff_list[maxIndex].ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper_max.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = HT_Eff_list[maxIndex].ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = HTR_min_DT_paper_max.ToString();

                        if (checkBox7.Checked == false)
                        {
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();

                        }

                        counter_Excel++;

                        initial_CIP_value = puntero_aplicacion.p_mc_in2;

                        //Closing Excel Book
                        xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        xlWorkBook1.Close(true, misValue1, misValue1);
                        //xlApp1.Quit();

                        //releaseObject(xlWorkSheet1);
                        ////releaseObject(xlWorkSheet2);
                        //releaseObject(xlWorkBook1);
                        //releaseObject(xlApp1);
                    } //Fin de la CUARTA LLAMADA para optimización
                }

                //-------------------------------------------------------------------------
                //UA optimization True
                else if (checkBox2.Checked == true)
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
                        puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox80.Text, puntero_aplicacion.category);
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
                    puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                    puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                    puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc_in2;
                    puntero_aplicacion.t_rht_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                    puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht_in2;
                    puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                    puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                    puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                    puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                    puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                    puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                    puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                    puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                    puntero_aplicacion.dp2_rhx1 = puntero_aplicacion.dp2_phx1;

                    //puntero_aplicacion.recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                    puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                    puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                    puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                    puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                    puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                    puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                    puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                    core.RecompCycle cicloRC_withRH = new core.RecompCycle();

                    double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                    double LT_fraction = 0.1;

                    List<Double> massflow2_list = new List<Double>();
                    List<Double> recomp_frac2_list = new List<Double>();
                    List<Double> p_mc_in2_list = new List<Double>();
                    List<Double> eta_thermal2_list = new List<Double>();
                    List<Double> p_rhx_in2_list = new List<Double>();
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

                    List<Double> HT_Eff_list = new List<Double>();
                    List<Double> LT_Eff_list = new List<Double>();

                    List<Double> PHX_Q2_list = new List<Double>();                    

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

                        if (i == Convert.ToDouble(textBox57.Text))
                        {

                            xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + ":" + puntero_aplicacion.textBox61.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox51.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox80.Text;
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
                            xlWorkSheet1.Cells[4, 6] = "P_rhx_in(kPa)";
                            xlWorkSheet1.Cells[4, 7] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 8] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 9] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 10] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 11] = "HTR Pinch(ºC)";

                            if (checkBox7.Checked == false)
                            {
                                xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";
                            }
                        }
                    }

                    //PRIMERA LLAMADA para la optimización
                    double max_recomp_fraction = 0.0;
                    double max_mc_p_in = 0.0;
                    double temp5_max_eff = 0.0;

                    List<Double> temp5_list_primera = new List<Double>();

                    using (var solver = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                    {
                        solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                        solver.SetUpperBounds(new[] { 1.0, 12500.0, 1.0 });

                        solver.SetInitialStepSize(new[] { 0.005, 50.0, 0.05 });

                        var initialValue = new[] { 0.2, initial_CIP_value, 0.5 };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis,
                            ref cicloRC_withRH, puntero_aplicacion.w_dot_net2, i, puntero_aplicacion.t_t_in2,
                            variables[1], puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1,
                            -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1,
                            -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, variables[2], UA_Total,
                            variables[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2,
                            puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables[0];
                            puntero_aplicacion.p_mc_in2 = variables[1];
                            LT_fraction = variables[2];
                            puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                            puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                            puntero_aplicacion.temp21 = cicloRC_withRH.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH.PC.Q_dot;

                            eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                            temp5_list_primera.Add(puntero_aplicacion.temp25);
                            //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                            ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                            ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                            double LTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH.temp[8] - cicloRC_withRH.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH.temp[7] - cicloRC_withRH.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH.temp[6] - cicloRC_withRH.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

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
                        //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                        textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                        textBox82.Text = ua_LT_list[maxIndex].ToString();
                        textBox83.Text = ua_HT_list[maxIndex].ToString();

                        max_recomp_fraction = recomp_frac2_list[maxIndex];
                        max_mc_p_in = p_mc_in2_list[maxIndex];
                        temp5_max_eff = temp5_list_primera[maxIndex];

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                            puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                        }

                        if (i == Convert.ToDouble(textBox57.Text))
                        {
                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);
                        }
                    } //Fin de la PRIMERA LLAMADA para optimización

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

                    //SEGUNDA LLAMADA
                    double max_recomp_fraction_1 = 0.0;
                    double max_mc_p_in_1 = 0.0;
                    double temp5_max_eff_segunda = 0.0;

                    List<Double> temp5_list_segunda = new List<Double>();

                    core.RecompCycle cicloRC_withRH_Segunda_llamada = new core.RecompCycle();

                    List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                    List<Double> p_mc_in2_list_segunda_llamada = new List<Double>();
                    List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                    List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();

                    //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                    //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                    //xlWorkSheet1.Activate();

                    using (var solver1 = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                    {
                        solver1.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                        solver1.SetUpperBounds(new[] { 1.0, 12500.0, 1.0 });

                        solver1.SetInitialStepSize(new[] { 0.005, 50.0, 0.05 });

                        var initialValue = new[] { max_recomp_fraction, max_mc_p_in, 0.5 };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis,
                            ref cicloRC_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, i,
                            temp5_max_eff, variables[1], puntero_aplicacion.p_mc_out2, variables[1], puntero_aplicacion.t_rht_in2,
                            -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2,
                            -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                            -puntero_aplicacion.dp2_ht2, variables[2], UA_Total, variables[0], puntero_aplicacion.eta_mc2,
                            puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2,
                            puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH_Segunda_llamada.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Segunda_llamada.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Segunda_llamada.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables[0];
                            puntero_aplicacion.p_mc_in2 = variables[1];
                            LT_fraction = variables[2];
                            puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                            puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                            puntero_aplicacion.temp21 = cicloRC_withRH_Segunda_llamada.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH_Segunda_llamada.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH_Segunda_llamada.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH_Segunda_llamada.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH_Segunda_llamada.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH_Segunda_llamada.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH_Segunda_llamada.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH_Segunda_llamada.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH_Segunda_llamada.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH_Segunda_llamada.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH_Segunda_llamada.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH_Segunda_llamada.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH_Segunda_llamada.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH_Segunda_llamada.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH_Segunda_llamada.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH_Segunda_llamada.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH_Segunda_llamada.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH_Segunda_llamada.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH_Segunda_llamada.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH_Segunda_llamada.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH_Segunda_llamada.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH_Segunda_llamada.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH_Segunda_llamada.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH_Segunda_llamada.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Segunda_llamada.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Segunda_llamada.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH_Segunda_llamada.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH_Segunda_llamada.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH_Segunda_llamada.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH_Segunda_llamada.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH_Segunda_llamada.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH_Segunda_llamada.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH_Segunda_llamada.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH_Segunda_llamada.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH_Segunda_llamada.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH_Segunda_llamada.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH_Segunda_llamada.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH_Segunda_llamada.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH_Segunda_llamada.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH_Segunda_llamada.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH_Segunda_llamada.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH_Segunda_llamada.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH_Segunda_llamada.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH_Segunda_llamada.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH_Segunda_llamada.PC.Q_dot;

                            eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc_in2);
                            //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                            temp5_list_segunda.Add(puntero_aplicacion.temp25);
                            ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                            ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                            double LTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[8] - cicloRC_withRH_Segunda_llamada.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH_Segunda_llamada.temp[7] - cicloRC_withRH_Segunda_llamada.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH_Segunda_llamada.temp[6] - cicloRC_withRH_Segunda_llamada.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

                            return puntero_aplicacion.eta_thermal2;
                        };

                        solver1.SetMaxObjective(funcion);

                        double? finalScore;

                        var result = solver1.Optimize(initialValue, out finalScore);

                        Double max_eta_thermal = 0.0;

                        max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                        var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        textBox91.Text = p_mc_in2_list_segunda_llamada[maxIndex].ToString();
                        textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                        //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                        textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                        textBox82.Text = ua_LT_list[maxIndex].ToString();
                        textBox83.Text = ua_HT_list[maxIndex].ToString();

                        max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                        max_mc_p_in_1 = p_mc_in2_list_segunda_llamada[maxIndex];
                        temp5_max_eff_segunda = temp5_list_segunda[maxIndex];

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list_segunda_llamada[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                            puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        //xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        //xlWorkBook1.Close(true, misValue1, misValue1);
                        //xlApp1.Quit();

                        //releaseObject(xlWorkSheet1);
                        ////releaseObject(xlWorkSheet2);
                        //releaseObject(xlWorkBook1);
                        //releaseObject(xlApp1);

                    } //Fin de la SEGUNDA LLAMADA para optimización  

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

                    //TERCERA LLAMADA
                    double max_recomp_fraction_2 = 0.0;
                    double max_mc_p_in_2 = 0.0;
                    double temp5_max_eff_tercera = 0.0;

                    List<Double> temp5_list_tercera = new List<Double>();

                    core.RecompCycle cicloRC_withRH_Tercera_llamada = new core.RecompCycle();

                    List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                    List<Double> p_mc_in2_list_tercera_llamada = new List<Double>();
                    List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                    List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();

                    //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                    //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                    //xlWorkSheet1.Activate();

                    using (var solver2 = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                    {
                        solver2.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                        solver2.SetUpperBounds(new[] { 1.0, 12500.0, 1.0 });

                        solver2.SetInitialStepSize(new[] { 0.005, 50.0, 0.05 });

                        var initialValue = new[] { max_recomp_fraction_1, max_mc_p_in_1, 0.5 };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withRH_Tercera_llamada,
                            puntero_aplicacion.w_dot_net2, i, temp5_max_eff_segunda, variables[1], puntero_aplicacion.p_mc_out2,
                            variables[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1,
                            -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                            -puntero_aplicacion.dp2_ht2, variables[2], UA_Total, variables[0], puntero_aplicacion.eta_mc2,
                            puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                            puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH_Tercera_llamada.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Tercera_llamada.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Tercera_llamada.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables[0];
                            puntero_aplicacion.p_mc_in2 = variables[1];
                            LT_fraction = variables[2];
                            puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                            puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                            puntero_aplicacion.temp21 = cicloRC_withRH_Tercera_llamada.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH_Tercera_llamada.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH_Tercera_llamada.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH_Tercera_llamada.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH_Tercera_llamada.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH_Tercera_llamada.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH_Tercera_llamada.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH_Tercera_llamada.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH_Tercera_llamada.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH_Tercera_llamada.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH_Tercera_llamada.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH_Tercera_llamada.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH_Tercera_llamada.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH_Tercera_llamada.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH_Tercera_llamada.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH_Tercera_llamada.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH_Tercera_llamada.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH_Tercera_llamada.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH_Tercera_llamada.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH_Tercera_llamada.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH_Tercera_llamada.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH_Tercera_llamada.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH_Tercera_llamada.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH_Tercera_llamada.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Tercera_llamada.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Tercera_llamada.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH_Tercera_llamada.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH_Tercera_llamada.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH_Tercera_llamada.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH_Tercera_llamada.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH_Tercera_llamada.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH_Tercera_llamada.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH_Tercera_llamada.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH_Tercera_llamada.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH_Tercera_llamada.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH_Tercera_llamada.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH_Tercera_llamada.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH_Tercera_llamada.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH_Tercera_llamada.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH_Tercera_llamada.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH_Tercera_llamada.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH_Tercera_llamada.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH_Tercera_llamada.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH_Tercera_llamada.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH_Tercera_llamada.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH_Tercera_llamada.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH_Tercera_llamada.PC.Q_dot;

                            eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc_in2);
                            //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                            temp5_list_tercera.Add(puntero_aplicacion.temp25);
                            ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                            ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                            double LTR_min_DT_1 = cicloRC_withRH_Tercera_llamada.temp[7] - cicloRC_withRH_Tercera_llamada.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH_Tercera_llamada.temp[8] - cicloRC_withRH_Tercera_llamada.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH_Tercera_llamada.temp[7] - cicloRC_withRH_Tercera_llamada.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH_Tercera_llamada.temp[6] - cicloRC_withRH_Tercera_llamada.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

                            return puntero_aplicacion.eta_thermal2;
                        };

                        solver2.SetMaxObjective(funcion);

                        double? finalScore;

                        var result = solver2.Optimize(initialValue, out finalScore);

                        Double max_eta_thermal = 0.0;

                        max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                        var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        textBox91.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                        textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                        textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                        textBox82.Text = ua_LT_list[maxIndex].ToString();
                        textBox83.Text = ua_HT_list[maxIndex].ToString();

                        max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                        max_mc_p_in_2 = p_mc_in2_list_tercera_llamada[maxIndex];
                        temp5_max_eff_tercera = temp5_list_tercera[maxIndex];

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list_tercera_llamada[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                            puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        //xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        //xlWorkBook1.Close(true, misValue1, misValue1);
                        //xlApp1.Quit();

                        //releaseObject(xlWorkSheet1);
                        ////releaseObject(xlWorkSheet2);
                        //releaseObject(xlWorkBook1);
                        //releaseObject(xlApp1);

                    } //Fin de la TERCERA LLAMADA para optimización  

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

                    //CUARTA LLAMADA
                    double max_recomp_fraction_3 = 0.0;
                    double max_mc_p_in_3 = 0.0;
                    double max_lt_fraction_3 = 0.0;
                    double temp5_max_eff_cuarta = 0.0;

                    List<Double> temp5_list_cuarta = new List<Double>();

                    core.RecompCycle cicloRC_withRH_Cuarta_llamada = new core.RecompCycle();

                    List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                    List<Double> p_mc_in2_list_cuarta_llamada = new List<Double>();
                    List<Double> lt_fraction_cuarta_llamada = new List<Double>();
                    List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                    List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();

                    xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                    xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                    xlWorkSheet1.Activate();

                    using (var solver3 = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                    {
                        solver3.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.0 });
                        solver3.SetUpperBounds(new[] { 1.0, 12500.0, 1.0 });

                        solver3.SetInitialStepSize(new[] { 0.005, 50.0, 0.05 });

                        var initialValue = new[] { max_recomp_fraction_2, max_mc_p_in_2, 0.5 };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withRH_Cuarta_llamada,
                            puntero_aplicacion.w_dot_net2, i, temp5_max_eff_tercera, variables[1], puntero_aplicacion.p_mc_out2,
                            variables[1], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1,
                            -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                            -puntero_aplicacion.dp2_ht2, variables[2], UA_Total, variables[0], puntero_aplicacion.eta_mc2,
                            puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                            puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withRH_Cuarta_llamada.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withRH_Cuarta_llamada.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withRH_Cuarta_llamada.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables[0];
                            puntero_aplicacion.p_mc_in2 = variables[1];
                            puntero_aplicacion.p_rhx_in2 = variables[1];
                            LT_fraction = variables[2];
                            puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                            puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                            puntero_aplicacion.temp21 = cicloRC_withRH_Cuarta_llamada.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withRH_Cuarta_llamada.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withRH_Cuarta_llamada.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withRH_Cuarta_llamada.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withRH_Cuarta_llamada.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withRH_Cuarta_llamada.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withRH_Cuarta_llamada.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withRH_Cuarta_llamada.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withRH_Cuarta_llamada.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withRH_Cuarta_llamada.temp[9];
                            puntero_aplicacion.temp211 = cicloRC_withRH_Cuarta_llamada.temp[10];
                            puntero_aplicacion.temp212 = cicloRC_withRH_Cuarta_llamada.temp[11];

                            puntero_aplicacion.pres21 = cicloRC_withRH_Cuarta_llamada.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withRH_Cuarta_llamada.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withRH_Cuarta_llamada.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withRH_Cuarta_llamada.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withRH_Cuarta_llamada.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withRH_Cuarta_llamada.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withRH_Cuarta_llamada.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withRH_Cuarta_llamada.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withRH_Cuarta_llamada.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withRH_Cuarta_llamada.pres[9];
                            puntero_aplicacion.pres211 = cicloRC_withRH_Cuarta_llamada.pres[10];
                            puntero_aplicacion.pres212 = cicloRC_withRH_Cuarta_llamada.pres[11];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withRH_Cuarta_llamada.PHX.Q_dot;
                            puntero_aplicacion.RHX_Q2 = cicloRC_withRH_Cuarta_llamada.RHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withRH_Cuarta_llamada.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withRH_Cuarta_llamada.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withRH_Cuarta_llamada.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withRH_Cuarta_llamada.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withRH_Cuarta_llamada.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withRH_Cuarta_llamada.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withRH_Cuarta_llamada.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withRH_Cuarta_llamada.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withRH_Cuarta_llamada.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withRH_Cuarta_llamada.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withRH_Cuarta_llamada.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withRH_Cuarta_llamada.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withRH_Cuarta_llamada.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withRH_Cuarta_llamada.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withRH_Cuarta_llamada.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withRH_Cuarta_llamada.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withRH_Cuarta_llamada.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withRH_Cuarta_llamada.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withRH_Cuarta_llamada.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withRH_Cuarta_llamada.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withRH_Cuarta_llamada.PC.Q_dot;

                            massflow2_list.Add(puntero_aplicacion.massflow2);
                            eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc_in2);
                            lt_fraction_cuarta_llamada.Add(LT_fraction);
                            p_rhx_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                            temp5_list_cuarta.Add(puntero_aplicacion.temp25);
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

                            PHX_Q2_list.Add(cicloRC_withRH_Cuarta_llamada.PHX.Q_dot);

                            HT_Eff_list.Add(cicloRC_withRH_Cuarta_llamada.HT.eff);
                            LT_Eff_list.Add(cicloRC_withRH_Cuarta_llamada.LT.eff);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                            listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                            double LTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[2];
                            double LTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[8] - cicloRC_withRH_Cuarta_llamada.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[3];
                            double HTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[6] - cicloRC_withRH_Cuarta_llamada.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////CIP
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            ////CIT
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                            //counter_Excel++;

                            return puntero_aplicacion.eta_thermal2;
                        };

                        solver3.SetMaxObjective(funcion);

                        double? finalScore;

                        var result = solver3.Optimize(initialValue, out finalScore);

                        Double max_eta_thermal = 0.0;

                        max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                        var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        textBox91.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                        textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        textBox2.Text = p_rhx_in2_list_cuarta_llamada[maxIndex].ToString();
                        textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                        textBox82.Text = ua_LT_list[maxIndex].ToString();
                        textBox83.Text = ua_HT_list[maxIndex].ToString();

                        max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                        max_mc_p_in_3 = p_mc_in2_list_cuarta_llamada[maxIndex];
                        temp5_max_eff_cuarta = temp5_list_cuarta[maxIndex];
                        max_lt_fraction_3 = lt_fraction_cuarta_llamada[maxIndex];

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list_cuarta_llamada[maxIndex].ToString();
                            //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                            puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                        }

                        //The variable 'i' is the loop counter for the CIT
                        listBox18.Items.Add(i.ToString());
                        listBox17.Items.Add(eta_thermal2_list_cuarta_llamada[maxIndex].ToString());
                        listBox16.Items.Add(recomp_frac2_list_cuarta_llamada[maxIndex].ToString());
                        listBox15.Items.Add(p_mc_in2_list_cuarta_llamada[maxIndex].ToString());
                        listBox10.Items.Add(p_rhx_in2_list_cuarta_llamada[maxIndex].ToString());
                        listBox11.Items.Add(t5_list[maxIndex].ToString());
                        listBox12.Items.Add(t6_list[maxIndex].ToString());
                        listBox13.Items.Add(ua_HT_list[maxIndex].ToString());
                        listBox14.Items.Add(ua_LT_list[maxIndex].ToString());

                        if (checkBox7.Checked == false)
                        {
                            //Calculo del campo solar
                            PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                            PTC.calledForSensingAnalysis = true;
                            PTC.comboBox1.Text = "Solar Salt";
                            PTC.comboBox2.Text = "NewMixture";
                            PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC.comboBox14.Text = puntero_aplicacion.comboBox1.Text;
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
                        }

                        //Copy results to EXCEL
                        double LTR_min_DT_1_max = t8_list[maxIndex] - t3_list[maxIndex];
                        double LTR_min_DT_2_max = t9_list[maxIndex] - t2_list[maxIndex];
                        double LTR_min_DT_paper_max = Math.Min(LTR_min_DT_1_max, LTR_min_DT_2_max);

                        double HTR_min_DT_1_max = t8_list[maxIndex] - t4_list[maxIndex];
                        double HTR_min_DT_2_max = t7_list[maxIndex] - t5_list[maxIndex];
                        double HTR_min_DT_paper_max = Math.Min(HTR_min_DT_1_max, HTR_min_DT_2_max);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_mc_in2_list_cuarta_llamada[maxIndex]);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(i - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = (eta_thermal2_list_cuarta_llamada[maxIndex] * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = LT_Eff_list[maxIndex].ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper_max.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = HT_Eff_list[maxIndex].ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = HTR_min_DT_paper_max.ToString();

                        if (checkBox7.Checked == false)
                        {
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();

                        }

                        counter_Excel++;

                        initial_CIP_value = puntero_aplicacion.p_mc_in2;

                        //Closing Excel Book
                        xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        xlWorkBook1.Close(true, misValue1, misValue1);
                        //xlApp1.Quit();

                        //releaseObject(xlWorkSheet1);
                        ////releaseObject(xlWorkSheet2);
                        //releaseObject(xlWorkBook1);
                        //releaseObject(xlApp1);

                    } //Fin de la CUARTA LLAMADA para optimización  

                    ////QUINTA LLAMADA
                    //double max_recomp_fraction_4 = 0.0;
                    //double max_mc_p_in_4 = 0.0;
                    //double temp5_max_eff_quinta = 0.0;

                    //List<Double> temp5_list_quinta = new List<Double>();

                    //core.RecompCycle cicloRC_withRH_quinta_llamada = new core.RecompCycle();

                    //List<Double> recomp_frac2_list_quinta_llamada = new List<Double>();
                    //List<Double> p_mc_in2_list_quinta_llamada = new List<Double>();
                    //List<Double> eta_thermal2_list_quinta_llamada = new List<Double>();
                    //List<Double> p_rhx_in2_list_quinta_llamada = new List<Double>();

                    //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls");
                    //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                    //xlWorkSheet1.Activate();

                    //using (var solver4 = new NLoptSolver(algorithm_type, 1, 0.00001, 10000))
                    //{
                    //    solver4.SetLowerBounds(new[] { initial_CIP_value });
                    //    solver4.SetUpperBounds(new[] { 125000.0 });

                    //    solver4.SetInitialStepSize(new[] { 10.0});

                    //    var initialValue = new[] { 7400.0 };

                    //    Func<double[], double> funcion = delegate (double[] variables)
                    //    {
                    //        puntero_aplicacion.luis.RecompCycledesign_newproposed_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withRH_quinta_llamada,
                    //        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, temp5_max_eff_cuarta, variables[0], puntero_aplicacion.p_mc_out2,
                    //        variables[0], puntero_aplicacion.t_rht_in2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1,
                    //        -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_lt2,
                    //        -puntero_aplicacion.dp2_ht2, max_lt_fraction_3, UA_Total, max_recomp_fraction_3, puntero_aplicacion.eta_mc2,
                    //        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                    //        puntero_aplicacion.tol2);

                    //        counter++;

                    //        puntero_aplicacion.massflow2 = cicloRC_withRH_quinta_llamada.m_dot_turbine;
                    //        puntero_aplicacion.w_dot_net2 = cicloRC_withRH_quinta_llamada.W_dot_net;
                    //        puntero_aplicacion.eta_thermal2 = cicloRC_withRH_quinta_llamada.eta_thermal;
                    //        //puntero_aplicacion.recomp_frac2 = variables[0];
                    //        puntero_aplicacion.p_mc_in2 = variables[0];
                    //        //LT_fraction = variables[2];
                    //        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                    //        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                    //        puntero_aplicacion.temp21 = cicloRC_withRH_quinta_llamada.temp[0];
                    //        puntero_aplicacion.temp22 = cicloRC_withRH_quinta_llamada.temp[1];
                    //        puntero_aplicacion.temp23 = cicloRC_withRH_quinta_llamada.temp[2];
                    //        puntero_aplicacion.temp24 = cicloRC_withRH_quinta_llamada.temp[3];
                    //        puntero_aplicacion.temp25 = cicloRC_withRH_quinta_llamada.temp[4];
                    //        puntero_aplicacion.temp26 = cicloRC_withRH_quinta_llamada.temp[5];
                    //        puntero_aplicacion.temp27 = cicloRC_withRH_quinta_llamada.temp[6];
                    //        puntero_aplicacion.temp28 = cicloRC_withRH_quinta_llamada.temp[7];
                    //        puntero_aplicacion.temp29 = cicloRC_withRH_quinta_llamada.temp[8];
                    //        puntero_aplicacion.temp210 = cicloRC_withRH_quinta_llamada.temp[9];
                    //        puntero_aplicacion.temp211 = cicloRC_withRH_quinta_llamada.temp[10];
                    //        puntero_aplicacion.temp212 = cicloRC_withRH_quinta_llamada.temp[11];

                    //        puntero_aplicacion.pres21 = cicloRC_withRH_quinta_llamada.pres[0];
                    //        puntero_aplicacion.pres22 = cicloRC_withRH_quinta_llamada.pres[1];
                    //        puntero_aplicacion.pres23 = cicloRC_withRH_quinta_llamada.pres[2];
                    //        puntero_aplicacion.pres24 = cicloRC_withRH_quinta_llamada.pres[3];
                    //        puntero_aplicacion.pres25 = cicloRC_withRH_quinta_llamada.pres[4];
                    //        puntero_aplicacion.pres26 = cicloRC_withRH_quinta_llamada.pres[5];
                    //        puntero_aplicacion.pres27 = cicloRC_withRH_quinta_llamada.pres[6];
                    //        puntero_aplicacion.pres28 = cicloRC_withRH_quinta_llamada.pres[7];
                    //        puntero_aplicacion.pres29 = cicloRC_withRH_quinta_llamada.pres[8];
                    //        puntero_aplicacion.pres210 = cicloRC_withRH_quinta_llamada.pres[9];
                    //        puntero_aplicacion.pres211 = cicloRC_withRH_quinta_llamada.pres[10];
                    //        puntero_aplicacion.pres212 = cicloRC_withRH_quinta_llamada.pres[11];

                    //        puntero_aplicacion.PHX_Q2 = cicloRC_withRH_quinta_llamada.PHX.Q_dot;
                    //        puntero_aplicacion.RHX_Q2 = cicloRC_withRH_quinta_llamada.RHX.Q_dot;

                    //        puntero_aplicacion.LT_Q = cicloRC_withRH_quinta_llamada.LT.Q_dot;
                    //        puntero_aplicacion.LT_mdotc = cicloRC_withRH_quinta_llamada.LT.m_dot_design[0];
                    //        puntero_aplicacion.LT_mdoth = cicloRC_withRH_quinta_llamada.LT.m_dot_design[1];
                    //        puntero_aplicacion.LT_Tcin = cicloRC_withRH_quinta_llamada.LT.T_c_in;
                    //        puntero_aplicacion.LT_Thin = cicloRC_withRH_quinta_llamada.LT.T_h_in;
                    //        puntero_aplicacion.LT_Pcin = cicloRC_withRH_quinta_llamada.LT.P_c_in;
                    //        puntero_aplicacion.LT_Phin = cicloRC_withRH_quinta_llamada.LT.P_h_in;
                    //        puntero_aplicacion.LT_Pcout = cicloRC_withRH_quinta_llamada.LT.P_c_out;
                    //        puntero_aplicacion.LT_Phout = cicloRC_withRH_quinta_llamada.LT.P_h_out;
                    //        puntero_aplicacion.LT_Effc = cicloRC_withRH_quinta_llamada.LT.eff;

                    //        puntero_aplicacion.HT_Q = cicloRC_withRH_quinta_llamada.HT.Q_dot;
                    //        puntero_aplicacion.HT_mdotc = cicloRC_withRH_quinta_llamada.HT.m_dot_design[0];
                    //        puntero_aplicacion.HT_mdoth = cicloRC_withRH_quinta_llamada.HT.m_dot_design[1];
                    //        puntero_aplicacion.HT_Tcin = cicloRC_withRH_quinta_llamada.HT.T_c_in;
                    //        puntero_aplicacion.HT_Thin = cicloRC_withRH_quinta_llamada.HT.T_h_in;
                    //        puntero_aplicacion.HT_Pcin = cicloRC_withRH_quinta_llamada.HT.P_c_in;
                    //        puntero_aplicacion.HT_Phin = cicloRC_withRH_quinta_llamada.HT.P_h_in;
                    //        puntero_aplicacion.HT_Pcout = cicloRC_withRH_quinta_llamada.HT.P_c_out;
                    //        puntero_aplicacion.HT_Phout = cicloRC_withRH_quinta_llamada.HT.P_h_out;
                    //        puntero_aplicacion.HT_Effc = cicloRC_withRH_quinta_llamada.HT.eff;

                    //        puntero_aplicacion.PC_Q2 = cicloRC_withRH_quinta_llamada.PC.Q_dot;

                    //        eta_thermal2_list_quinta_llamada.Add(puntero_aplicacion.eta_thermal2);
                    //        recomp_frac2_list_quinta_llamada.Add(puntero_aplicacion.recomp_frac2);
                    //        p_mc_in2_list_quinta_llamada.Add(puntero_aplicacion.p_mc_in2);
                    //        //p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                    //        temp5_list_quinta.Add(puntero_aplicacion.temp25);
                    //        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                    //        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                    //        listBox1.Items.Add(counter.ToString());
                    //        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                    //        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                    //        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                    //        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                    //        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                    //        listBox7.Items.Add(puntero_aplicacion.temp25.ToString());
                    //        listBox8.Items.Add(puntero_aplicacion.temp26.ToString());
                    //        //listBox9.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());                                              

                    //        double LTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[2];
                    //        double LTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[8] - cicloRC_withRH_Cuarta_llamada.temp[1];
                    //        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    //        double HTR_min_DT_1 = cicloRC_withRH_Cuarta_llamada.temp[7] - cicloRC_withRH_Cuarta_llamada.temp[3];
                    //        double HTR_min_DT_2 = cicloRC_withRH_Cuarta_llamada.temp[6] - cicloRC_withRH_Cuarta_llamada.temp[4];
                    //        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    //        //CIP
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                    //        //CIT
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                    //        //LT UA(kW/K)
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 3] = puntero_aplicacion.ua_lt2.ToString();
                    //        //HT UA(kW/K)
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_ht2.ToString();
                    //        //Rec.Frac.
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac2.ToString();
                    //        //P_rhx_in
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.p_mc_in2.ToString();
                    //        //Eff.(%)
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 7] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                    //        //LTR Eff.(%)
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 8] = cicloRC_withRH.LT.eff.ToString();
                    //        //LTR Pinch(ºC)
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper.ToString();
                    //        //HTR Eff.(%)
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRC_withRH.HT.eff.ToString();
                    //        //HTR Pinch(ºC)
                    //        xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper.ToString();

                    //        counter_Excel++;

                    //        return puntero_aplicacion.eta_thermal2;
                    //    };

                    //    solver4.SetMaxObjective(funcion);

                    //    double? finalScore;

                    //    var result = solver4.Optimize(initialValue, out finalScore);

                    //    Double max_eta_thermal = 0.0;

                    //    max_eta_thermal = eta_thermal2_list_quinta_llamada.Max();

                    //    var maxIndex = eta_thermal2_list_quinta_llamada.IndexOf(eta_thermal2_list_quinta_llamada.Max());

                    //    puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                    //    puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                    //    textBox91.Text = p_mc_in2_list_quinta_llamada[maxIndex].ToString();
                    //    textBox90.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                    //    //textBox2.Text = p_rhx_in2_list[maxIndex].ToString();
                    //    textBox86.Text = eta_thermal2_list_quinta_llamada[maxIndex].ToString();
                    //    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    //    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //    //Copy results as design-point inputs
                    //    if (checkBox3.Checked == true)
                    //    {
                    //        puntero_aplicacion.textBox15.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                    //        puntero_aplicacion.textBox3.Text = p_mc_in2_list_quinta_llamada[maxIndex].ToString();
                    //        //puntero_aplicacion.textBox7.Text = p_rhx_in2_list[maxIndex].ToString();
                    //        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                    //        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    //    }

                    //    //Closing Excel Book
                    //    xlWorkBook1.SaveAs(textBox3.Text + "RC_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    //    xlWorkBook1.Close(true, misValue1, misValue1);
                    //    xlApp1.Quit();

                    //    releaseObject(xlWorkSheet1);
                    //    //releaseObject(xlWorkSheet2);
                    //    releaseObject(xlWorkBook1);
                    //    releaseObject(xlApp1);

                    //} //Fin de la QUINTA LLAMADA para optimización  


                }

            }
        }
    }
}
