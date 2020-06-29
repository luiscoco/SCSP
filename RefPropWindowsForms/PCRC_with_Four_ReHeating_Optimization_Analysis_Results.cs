using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class PCRC_with_Four_ReHeating_Optimization_Analysis_Results : Form
    {
        PCRC_with_Three_ReHeating puntero_aplicacion;

        public PCRC_with_Four_ReHeating_Optimization_Analysis_Results(PCRC_with_Three_ReHeating puntero1)
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

            //UA optimzation false
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," +
                                                  puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," +
                                                  puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox87.Text + "," +
                                                  puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox88.Text,
                                                  puntero_aplicacion.category);
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
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox28.Text);
                puntero_aplicacion.p_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_pc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.t_pc_in2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);
                puntero_aplicacion.p_rhx1_in2 = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in2 = Convert.ToDouble(puntero_aplicacion.textBox6.Text);
                puntero_aplicacion.p_rhx2_in2 = Convert.ToDouble(puntero_aplicacion.textBox91.Text);
                puntero_aplicacion.t_rht2_in2 = Convert.ToDouble(puntero_aplicacion.textBox90.Text);
                puntero_aplicacion.p_rhx3_in2 = Convert.ToDouble(puntero_aplicacion.textBox100.Text);
                puntero_aplicacion.t_rht3_in2 = Convert.ToDouble(puntero_aplicacion.textBox99.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_rhx11 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp2_rhx21 = Convert.ToDouble(puntero_aplicacion.textBox89.Text);
                puntero_aplicacion.dp2_rhx31 = Convert.ToDouble(puntero_aplicacion.textBox98.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_cooler2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.m_recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.m_eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.m_eta_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.m_eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.m_eta_trh12 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);
                puntero_aplicacion.m_eta_trh22 = Convert.ToDouble(puntero_aplicacion.textBox92.Text);
                puntero_aplicacion.m_eta_trh32 = Convert.ToDouble(puntero_aplicacion.textBox97.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCwithThreeReheating cicloPCRC_withThreeRH = new core.PCRCwithThreeReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_rhx1_in2_list = new List<Double>();
                List<Double> p_rhx2_in2_list = new List<Double>();
                List<Double> p_rhx3_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
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

                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," +
                                           puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," +
                                           puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox87.Text + "," +
                                           puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox88.Text;


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
                xlWorkSheet1.Cells[4, 7] = "P_rhx1(kPa)";
                xlWorkSheet1.Cells[4, 8] = "P_rhx2(kPa)";
                xlWorkSheet1.Cells[4, 9] = "P_rhx3(kPa)";
                xlWorkSheet1.Cells[4, 10] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 13] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 14] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 6, 0.01, 10000))
                {

                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 500), initial_CIP_value + 3000, initial_CIP_value + 2000, initial_CIP_value + 1000 });
                    solver.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc_out2 / 1.5), puntero_aplicacion.p_mc_out2 - 1000, puntero_aplicacion.p_mc_out2 - 2000, puntero_aplicacion.p_mc_out2 - 3000 });

                    solver.SetInitialStepSize(new[] { 0.05, 100, 100, 1000, 1000, 1000 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500), 15000, 13000, 11000 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {

                        puntero_aplicacion.luis.RecompCycle_PCRC_withThreeReheating(puntero_aplicacion.luis,
                        ref cicloPCRC_withThreeRH, puntero_aplicacion.m_w_dot_net2,
                        puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2,
                        puntero_aplicacion.t_rht1_in2, variables[3],
                        puntero_aplicacion.t_rht2_in2, variables[4],
                        puntero_aplicacion.t_rht3_in2, variables[5],
                        variables[2], puntero_aplicacion.p_mc_out2, variables[1],
                        puntero_aplicacion.t_pc_in2, variables[2],
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.m_eta_mc2,
                        puntero_aplicacion.m_eta_rc2, puntero_aplicacion.m_eta_pc2, puntero_aplicacion.m_eta_t2,
                        puntero_aplicacion.m_eta_trh12, puntero_aplicacion.m_eta_trh22,
                        puntero_aplicacion.m_eta_trh32, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2,
                        puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp2_pc1, -puntero_aplicacion.dp2_pc2,
                        -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_phx2,
                        -puntero_aplicacion.dp2_rhx11, -puntero_aplicacion.dp2_rhx12,
                        -puntero_aplicacion.dp2_rhx21, -puntero_aplicacion.dp2_rhx22,
                        -puntero_aplicacion.dp2_rhx31, -puntero_aplicacion.dp2_rhx32,
                        -puntero_aplicacion.dp2_cooler1, -puntero_aplicacion.dp2_cooler2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRC_withThreeRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRC_withThreeRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRC_withThreeRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in2 = variables[1];
                        puntero_aplicacion.p_pc_out2 = variables[2];
                        puntero_aplicacion.p_rhx1_in2 = variables[3];
                        puntero_aplicacion.p_rhx2_in2 = variables[4];
                        puntero_aplicacion.p_rhx3_in2 = variables[5];

                        puntero_aplicacion.temp21 = cicloPCRC_withThreeRH.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRC_withThreeRH.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRC_withThreeRH.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRC_withThreeRH.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRC_withThreeRH.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRC_withThreeRH.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRC_withThreeRH.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRC_withThreeRH.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRC_withThreeRH.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRC_withThreeRH.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRC_withThreeRH.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRC_withThreeRH.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRC_withThreeRH.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRC_withThreeRH.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRC_withThreeRH.temp[14];
                        puntero_aplicacion.temp216 = cicloPCRC_withThreeRH.temp[15];
                        puntero_aplicacion.temp217 = cicloPCRC_withThreeRH.temp[16];
                        puntero_aplicacion.temp218 = cicloPCRC_withThreeRH.temp[17];

                        puntero_aplicacion.pres21 = cicloPCRC_withThreeRH.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRC_withThreeRH.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRC_withThreeRH.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRC_withThreeRH.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRC_withThreeRH.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRC_withThreeRH.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRC_withThreeRH.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRC_withThreeRH.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRC_withThreeRH.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRC_withThreeRH.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRC_withThreeRH.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRC_withThreeRH.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRC_withThreeRH.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRC_withThreeRH.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRC_withThreeRH.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRC_withThreeRH.pres[15];
                        puntero_aplicacion.pres217 = cicloPCRC_withThreeRH.pres[16];
                        puntero_aplicacion.pres218 = cicloPCRC_withThreeRH.pres[17];

                        puntero_aplicacion.PHX1 = cicloPCRC_withThreeRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRC_withThreeRH.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRC_withThreeRH.RHX2.Q_dot;
                        puntero_aplicacion.RHX3 = cicloPCRC_withThreeRH.RHX3.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRC_withThreeRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRC_withThreeRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRC_withThreeRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRC_withThreeRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRC_withThreeRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRC_withThreeRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRC_withThreeRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRC_withThreeRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRC_withThreeRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRC_withThreeRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRC_withThreeRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRC_withThreeRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRC_withThreeRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRC_withThreeRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRC_withThreeRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRC_withThreeRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRC_withThreeRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRC_withThreeRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRC_withThreeRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRC_withThreeRH.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRC_withThreeRH.PC.Q_dot;
                        puntero_aplicacion.COOLER1 = -cicloPCRC_withThreeRH.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in2);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out2);
                        p_rhx1_in2_list.Add(puntero_aplicacion.p_rhx1_in2);
                        p_rhx2_in2_list.Add(puntero_aplicacion.p_rhx2_in2);
                        p_rhx3_in2_list.Add(puntero_aplicacion.p_rhx3_in2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in2.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx1_in2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.p_rhx2_in2.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_rhx3_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloPCRC_withThreeRH.temp[7] - cicloPCRC_withThreeRH.temp[2];
                        double LTR_min_DT_2 = cicloPCRC_withThreeRH.temp[8] - cicloPCRC_withThreeRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRC_withThreeRH.temp[7] - cicloPCRC_withThreeRH.temp[3];
                        double HTR_min_DT_2 = cicloPCRC_withThreeRH.temp[6] - cicloPCRC_withThreeRH.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx1_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx1_in2.ToString();
                        //P_rhx2_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.p_rhx2_in2.ToString();
                        //P_rhx3_in
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.p_rhx3_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRC_withThreeRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloPCRC_withThreeRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = HTR_min_DT_paper.ToString();

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
                    textBox6.Text = p_rhx3_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_pc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox91.Text = p_rhx2_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox100.Text = p_rhx3_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox4.Text + "PCRC_with_Three_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }

            //UA optimization true
            else if (checkBox2.Checked == true)
            {

            }
        }

        //Run CIT Optimization
        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
