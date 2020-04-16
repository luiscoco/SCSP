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
    public partial class RCMCI_with_ReHeating_Optimization_new_configuration : Form
    {
        RCMCI_with_ReHeating_new_proposed_configuration puntero_aplicacion;

        public RCMCI_with_ReHeating_Optimization_new_configuration(RCMCI_with_ReHeating_new_proposed_configuration puntero1)
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

                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);

                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_rhx2_in2 = puntero_aplicacion.p_mc1_in2;
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);

                puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht1_in2;

                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);

                puntero_aplicacion.eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.eta_t2 = puntero_aplicacion.eta_trh12;

                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp2_phx1 = 0.0;
                puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating = new core.RCMCIwithTwoReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
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

                xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
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

                //PRIMERA LLAMADA para la optimización
                double max_recomp_fraction = 0.0;
                double max_mc1_p_in = 0.0;
                double temp5_max_eff = 0.0;

                List<Double> temp5_list_primera = new List<Double>();

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0 });
                    solver.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed(puntero_aplicacion.luis, ref cicloRCMCIwithTwoReheating,
                        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2, puntero_aplicacion.t_t_in2,
                        puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];


                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating.temp[7] - cicloRCMCIwithTwoReheating.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating.temp[8] - cicloRCMCIwithTwoReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating.temp[7] - cicloRCMCIwithTwoReheating.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating.temp[6] - cicloRCMCIwithTwoReheating.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating.HT.eff.ToString();
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

                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list[maxIndex].ToString();

                    max_recomp_fraction = recomp_frac2_list[maxIndex];
                    max_mc1_p_in = p_mc1_in2_list[maxIndex];
                    temp5_max_eff = temp5_list_primera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Final de la PRIMERA llamada

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

                //SEGUNDALLAMADA para la optimización
                double max_recomp_fraction_1 = 0.0;
                double max_mc1_p_in_1 = 0.0;
                double temp5_max_eff_1 = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating_Segunda_llamada = new core.RCMCIwithTwoReheating();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_segunda_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver1 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver1.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0 });
                    solver1.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0 });

                    solver1.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCIwithTwoReheating_Segunda_llamada, puntero_aplicacion.w_dot_net2,
                        puntero_aplicacion.t_mc2_in2, temp5_max_eff,
                        puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating_Segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating_Segunda_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating_Segunda_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];


                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating_Segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating_Segunda_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating_Segunda_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating_Segunda_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating_Segunda_llamada.COOLER.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[7] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[8] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[7] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[6] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver1.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver1.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                    var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                    textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list_segunda_llamada[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();

                    max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                    max_mc1_p_in_1 = p_mc1_in2_list_segunda_llamada[maxIndex];
                    temp5_max_eff_1 = temp5_list_segunda[maxIndex];


                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Final de la SEGUNDA llamada

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
                double max_mc1_p_in_2 = 0.0;
                double temp5_max_eff_2 = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating_Tercera_llamada = new core.RCMCIwithTwoReheating();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_tercera_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver2 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver2.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0 });
                    solver2.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0 });

                    solver2.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCIwithTwoReheating_Tercera_llamada, puntero_aplicacion.w_dot_net2,
                        puntero_aplicacion.t_mc2_in2, temp5_max_eff_1,
                        puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating_Tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating_Tercera_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating_Tercera_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];


                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating_Tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating_Tercera_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating_Tercera_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating_Tercera_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating_Tercera_llamada.COOLER.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[7] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[8] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[7] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[6] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver2.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver2.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                    var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                    textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list_tercera_llamada[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();

                    max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                    max_mc1_p_in_2 = p_mc1_in2_list_tercera_llamada[maxIndex];
                    temp5_max_eff_2 = temp5_list_tercera[maxIndex];


                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Final de la TERCERA llamada

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
                double max_mc1_p_in_3 = 0.0;
                double temp5_max_eff_3 = 0.0;

                List<Double> temp5_list_cuarta = new List<Double>();

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating_Cuarta_llamada = new core.RCMCIwithTwoReheating();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver3 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver3.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0 });
                    solver3.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0 });

                    solver3.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCIwithTwoReheating_Cuarta_llamada, puntero_aplicacion.w_dot_net2,
                        puntero_aplicacion.t_mc2_in2, temp5_max_eff_2,
                        puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating_Cuarta_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating_Cuarta_llamada.COOLER.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[7] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[8] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[7] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[6] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver3.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver3.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                    var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                    textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list_cuarta_llamada[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();

                    max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                    max_mc1_p_in_3 = p_mc1_in2_list_cuarta_llamada[maxIndex];
                    temp5_max_eff_3 = temp5_list_cuarta[maxIndex];


                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);

                } //Final de la CUARTA llamada

            }

            //Optimized UA 
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

                puntero_aplicacion.w_dot_net2 = Convert.ToDouble(puntero_aplicacion.textBox1.Text);
                puntero_aplicacion.t_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);

                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_rhx2_in2 = puntero_aplicacion.p_mc1_in2;
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);

                puntero_aplicacion.t_t_in2 = puntero_aplicacion.t_rht1_in2;

                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);

                puntero_aplicacion.eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.eta_t2 = puntero_aplicacion.eta_trh12;

                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp2_phx1 = 0.0;
                puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating = new core.RCMCIwithTwoReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
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

                xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
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

                //PRIMERA LLAMADA para la optimización
                double max_recomp_fraction = 0.0;
                double max_mc1_p_in = 0.0;
                double temp5_max_eff = 0.0;

                List<Double> temp5_list_primera = new List<Double>();

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0, 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCIwithTwoReheating, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2, 
                        puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], variables[4], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        LT_fraction = variables[4];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating.temp[7] - cicloRCMCIwithTwoReheating.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating.temp[8] - cicloRCMCIwithTwoReheating.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating.temp[7] - cicloRCMCIwithTwoReheating.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating.temp[6] - cicloRCMCIwithTwoReheating.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating.HT.eff.ToString();
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

                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction = recomp_frac2_list[maxIndex];
                    max_mc1_p_in = p_mc1_in2_list[maxIndex];
                    temp5_max_eff = temp5_list_primera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Final de la PRIMERA llamada

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

                //SEGUNDALLAMADA para la optimización
                double max_recomp_fraction_1 = 0.0;
                double max_mc1_p_in_1 = 0.0;
                double temp5_max_eff_1 = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating_Segunda_llamada = new core.RCMCIwithTwoReheating();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_segunda_llamada = new List<Double>();
                List<Double> ua_LT_list_segunda_llamada = new List<Double>();
                List<Double> ua_HT_list_segunda_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver1 = new NLoptSolver(algorithm_type, 5, 0.01, 10000))
                {
                    solver1.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0, 0.0 });
                    solver1.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0, 1.0 });

                    solver1.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCIwithTwoReheating_Segunda_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff, puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], variables[4], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating_Segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating_Segunda_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating_Segunda_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating_Segunda_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating_Segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating_Segunda_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating_Segunda_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating_Segunda_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating_Segunda_llamada.COOLER.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[7] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[8] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[7] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating_Segunda_llamada.temp[6] - cicloRCMCIwithTwoReheating_Segunda_llamada.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating_Segunda_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating_Segunda_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver1.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver1.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_segunda_llamada.Max();

                    var maxIndex = eta_thermal2_list_segunda_llamada.IndexOf(eta_thermal2_list_segunda_llamada.Max());

                    textBox86.Text = eta_thermal2_list_segunda_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list_segunda_llamada[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction_1 = recomp_frac2_list_segunda_llamada[maxIndex];
                    max_mc1_p_in_1 = p_mc1_in2_list_segunda_llamada[maxIndex];
                    temp5_max_eff_1 = temp5_list_segunda[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Final de la SEGUNDA llamada

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
                double max_mc1_p_in_2 = 0.0;
                double temp5_max_eff_2 = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating_Tercera_llamada = new core.RCMCIwithTwoReheating();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_tercera_llamada = new List<Double>();
                List<Double> ua_LT_list_tercera_llamada = new List<Double>();
                List<Double> ua_HT_list_tercera_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver2 = new NLoptSolver(algorithm_type, 5, 0.01, 10000))
                {
                    solver2.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0, 0.0 });
                    solver2.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0, 1.0 });

                    solver2.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCIwithTwoReheating_Tercera_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff_1, puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], variables[4], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating_Tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating_Tercera_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating_Tercera_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating_Tercera_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating_Tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating_Tercera_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating_Tercera_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating_Tercera_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating_Tercera_llamada.COOLER.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[7] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[8] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[7] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating_Tercera_llamada.temp[6] - cicloRCMCIwithTwoReheating_Tercera_llamada.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating_Tercera_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating_Tercera_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver2.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver2.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_tercera_llamada.Max();

                    var maxIndex = eta_thermal2_list_tercera_llamada.IndexOf(eta_thermal2_list_tercera_llamada.Max());

                    textBox86.Text = eta_thermal2_list_tercera_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list_tercera_llamada[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction_2 = recomp_frac2_list_tercera_llamada[maxIndex];
                    max_mc1_p_in_2 = p_mc1_in2_list_tercera_llamada[maxIndex];
                    temp5_max_eff_2 = temp5_list_tercera[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);

                } //Final de la TERCERA llamada

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
                double max_mc1_p_in_3 = 0.0;
                double temp5_max_eff_3 = 0.0;

                List<Double> temp5_list_cuarta = new List<Double>();

                core.RCMCIwithTwoReheating cicloRCMCIwithTwoReheating_Cuarta_llamada = new core.RCMCIwithTwoReheating();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx2_in2_list_cuarta_llamada = new List<Double>();
                List<Double> ua_LT_list_cuarta_llamada = new List<Double>();
                List<Double> ua_HT_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver3 = new NLoptSolver(algorithm_type, 5, 0.01, 10000))
                {
                    solver3.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0), 11000.0, 0.0 });
                    solver3.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5), 22000.0, 1.0 });

                    solver3.SetInitialStepSize(new[] { 0.05, 50.0, 50.0, 100.0, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 11000.0, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_withReheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCIwithTwoReheating_Cuarta_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff_2, puntero_aplicacion.t_rht1_in2, variables[3], puntero_aplicacion.t_rht2_in2,
                        variables[1], variables[2], puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2,
                        variables[2], variables[4], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh12, puntero_aplicacion.eta_trh22, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2,
                        -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_ht2, -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx11,
                        -puntero_aplicacion.dp2_rhx12, -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        puntero_aplicacion.p_rhx2_in2 = variables[1];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];

                        puntero_aplicacion.temp21 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[13];
                        puntero_aplicacion.temp215 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[14];
                        puntero_aplicacion.temp216 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[15];

                        puntero_aplicacion.pres21 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[13];
                        puntero_aplicacion.pres215 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[14];
                        puntero_aplicacion.pres216 = cicloRCMCIwithTwoReheating_Cuarta_llamada.pres[15];

                        puntero_aplicacion.PHX1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCIwithTwoReheating_Cuarta_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCIwithTwoReheating_Cuarta_llamada.COOLER.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx2_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[7] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[8] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[7] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[6] - cicloRCMCIwithTwoReheating_Cuarta_llamada.temp[4];
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
                        //P_rhx_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCIwithTwoReheating_Cuarta_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCIwithTwoReheating_Cuarta_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver3.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver3.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_cuarta_llamada.Max();

                    var maxIndex = eta_thermal2_list_cuarta_llamada.IndexOf(eta_thermal2_list_cuarta_llamada.Max());

                    textBox86.Text = eta_thermal2_list_cuarta_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list_cuarta_llamada[maxIndex].ToString();
                    textBox4.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    max_recomp_fraction_3 = recomp_frac2_list_cuarta_llamada[maxIndex];
                    max_mc1_p_in_3 = p_mc1_in2_list_cuarta_llamada[maxIndex];
                    temp5_max_eff_3 = temp5_list_cuarta[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_with_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);

                } //Final de la CUARTA llamada
            }  
        }
    }
}
