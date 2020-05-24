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
    public partial class PCRCMCI_with_Two_ReHeating_Optimization_Analysis_Results : Form
    {
        PCRCMCI_with_Two_ReHeating puntero_aplicacion;

        public PCRCMCI_with_Two_ReHeating_Optimization_Analysis_Results(PCRCMCI_with_Two_ReHeating puntero1)
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
        private void button2_Click(object sender, System.EventArgs e)
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
                    puntero_aplicacion.luis.core1(this.comboBox2.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox48.Text);

                puntero_aplicacion.t_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);

                puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                puntero_aplicacion.p_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                puntero_aplicacion.p_rhx1_in = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in = Convert.ToDouble(puntero_aplicacion.textBox6.Text);

                puntero_aplicacion.p_rhx2_in = Convert.ToDouble(puntero_aplicacion.textBox113.Text);
                puntero_aplicacion.t_rht2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);

                puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);

                puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.m_eta_mc1 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);

                puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCMCIwithTwoReheating cicloPCRCMCI_withTwoRH = new core.PCRCMCIwithTwoReheating();

                double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_rhx1_list = new List<Double>();
                List<Double> p_rhx2_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 3] = "MC1_out(kPa)";
                xlWorkSheet1.Cells[4, 4] = "P_rhx1(kPa)";
                xlWorkSheet1.Cells[4, 5] = "P_rhx2(kPa)";
                xlWorkSheet1.Cells[4, 6] = "CIT(K)";
                xlWorkSheet1.Cells[4, 7] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 8] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 9] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 10] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 13] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 14] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 6, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500.0, initial_CIP_value + 1000.0, initial_CIP_value + 3000.0, initial_CIP_value + 2000.0 });
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 1000.0, 1000.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value + 1000.0, initial_CIP_value + 2000.0, initial_CIP_value + 3000.0, 16000.0, 14000.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {

                        puntero_aplicacion.luis.RecompCycle_PCRCMCI_withTwoReheating(puntero_aplicacion.luis, 
                        ref cicloPCRCMCI_withTwoRH, puntero_aplicacion.w_dot_net, puntero_aplicacion.t_pc_in,
                        puntero_aplicacion.t_mc1_in, puntero_aplicacion.t_mc2_in, puntero_aplicacion.t_t_in, 
                        puntero_aplicacion.t_rht1_in, variables[4], puntero_aplicacion.t_rht2_in,
                        variables[5], variables[1], variables[2],
                        variables[3], puntero_aplicacion.p_mc2_out, variables[2], 
                        variables[3], puntero_aplicacion.ua_lt, puntero_aplicacion.ua_ht,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_pc, puntero_aplicacion.m_eta_rc, 
                        puntero_aplicacion.m_eta_mc1, puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1, 
                        puntero_aplicacion.m_eta_trh2, puntero_aplicacion.n_sub_hxrs, variables[0],
                        puntero_aplicacion.tol, puntero_aplicacion.eta_thermal, -puntero_aplicacion.dp_lt1, 
                        -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2, 
                        -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_pc3, 
                        -puntero_aplicacion.dp_phx, -puntero_aplicacion.dp_rhx1, -puntero_aplicacion.dp_rhx2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRCMCI_withTwoRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRCMCI_withTwoRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRCMCI_withTwoRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in = variables[1];
                        puntero_aplicacion.p_pc_out = variables[2];
                        puntero_aplicacion.p_mc1_in = variables[2];
                        puntero_aplicacion.p_mc1_out = variables[3];
                        puntero_aplicacion.p_mc2_in = variables[3];
                        puntero_aplicacion.p_rhx1_in = variables[4];
                        puntero_aplicacion.p_rhx2_in = variables[5];

                        puntero_aplicacion.temp21 = cicloPCRCMCI_withTwoRH.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRCMCI_withTwoRH.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRCMCI_withTwoRH.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRCMCI_withTwoRH.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRCMCI_withTwoRH.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRCMCI_withTwoRH.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRCMCI_withTwoRH.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRCMCI_withTwoRH.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRCMCI_withTwoRH.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRCMCI_withTwoRH.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRCMCI_withTwoRH.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRCMCI_withTwoRH.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRCMCI_withTwoRH.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRCMCI_withTwoRH.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRCMCI_withTwoRH.temp[14];
                        puntero_aplicacion.temp216 = cicloPCRCMCI_withTwoRH.temp[15];
                        puntero_aplicacion.temp217 = cicloPCRCMCI_withTwoRH.temp[16];
                        puntero_aplicacion.temp218 = cicloPCRCMCI_withTwoRH.temp[17];

                        puntero_aplicacion.pres21 = cicloPCRCMCI_withTwoRH.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRCMCI_withTwoRH.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRCMCI_withTwoRH.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRCMCI_withTwoRH.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRCMCI_withTwoRH.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRCMCI_withTwoRH.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRCMCI_withTwoRH.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRCMCI_withTwoRH.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRCMCI_withTwoRH.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRCMCI_withTwoRH.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRCMCI_withTwoRH.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRCMCI_withTwoRH.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRCMCI_withTwoRH.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRCMCI_withTwoRH.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRCMCI_withTwoRH.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRCMCI_withTwoRH.pres[15];
                        puntero_aplicacion.pres217 = cicloPCRCMCI_withTwoRH.pres[16];
                        puntero_aplicacion.pres218 = cicloPCRCMCI_withTwoRH.pres[17];

                        puntero_aplicacion.PHX1 = cicloPCRCMCI_withTwoRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRCMCI_withTwoRH.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRCMCI_withTwoRH.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRCMCI_withTwoRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRCMCI_withTwoRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRCMCI_withTwoRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRCMCI_withTwoRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRCMCI_withTwoRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRCMCI_withTwoRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRCMCI_withTwoRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRCMCI_withTwoRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRCMCI_withTwoRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRCMCI_withTwoRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRCMCI_withTwoRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRCMCI_withTwoRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRCMCI_withTwoRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRCMCI_withTwoRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRCMCI_withTwoRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRCMCI_withTwoRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRCMCI_withTwoRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRCMCI_withTwoRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRCMCI_withTwoRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRCMCI_withTwoRH.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRCMCI_withTwoRH.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloPCRCMCI_withTwoRH.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloPCRCMCI_withTwoRH.PC3.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out);
                        p_rhx1_list.Add(puntero_aplicacion.p_rhx1_in);
                        p_rhx2_list.Add(puntero_aplicacion.p_rhx2_in);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc1_in.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_mc1_out.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_rhx1_in.ToString());
                        listBox24.Items.Add(puntero_aplicacion.p_rhx2_in.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                        double LTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[2];
                        double LTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[8] - cicloPCRCMCI_withTwoRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[3];
                        double HTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[6] - cicloPCRCMCI_withTwoRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc1_out);
                        //P_rxh1(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_rhx1_in);
                        //P_rxh2(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_rhx2_in);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.t_mc1_in - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.ua_lt);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.ua_ht);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRCMCI_withTwoRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloPCRCMCI_withTwoRH.HT.eff.ToString();
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

                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                    textBox5.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox6.Text = p_rhx1_list[maxIndex].ToString();
                    textBox7.Text = p_rhx2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_list[maxIndex].ToString();
                        puntero_aplicacion.textBox113.Text = p_rhx2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "PCRCMCI_with_Two_ReHeating" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

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
                    puntero_aplicacion.luis.core1(this.comboBox2.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox48.Text);

                puntero_aplicacion.t_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);

                puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                puntero_aplicacion.p_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                puntero_aplicacion.p_rhx1_in = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                puntero_aplicacion.t_rht1_in = Convert.ToDouble(puntero_aplicacion.textBox6.Text);

                puntero_aplicacion.p_rhx2_in = Convert.ToDouble(puntero_aplicacion.textBox113.Text);
                puntero_aplicacion.t_rht2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);

                puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);

                puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.m_eta_mc1 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);

                puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCMCIwithTwoReheating cicloPCRCMCI_withTwoRH = new core.PCRCMCIwithTwoReheating();

                double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_rhx1_list = new List<Double>();
                List<Double> p_rhx2_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 3] = "MC1_out(kPa)";
                xlWorkSheet1.Cells[4, 4] = "P_rhx1(kPa)";
                xlWorkSheet1.Cells[4, 5] = "P_rhx2(kPa)";
                xlWorkSheet1.Cells[4, 6] = "CIT(K)";
                xlWorkSheet1.Cells[4, 7] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 8] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 9] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 10] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 13] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 14] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 7, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500.0, initial_CIP_value + 1000.0, initial_CIP_value + 3000.0, initial_CIP_value + 2000.0, 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, 1.0});

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 1000.0, 1000.0, 0.05 });

                    var initialValue = new[] { 0.2, initial_CIP_value + 1000.0, initial_CIP_value + 2000.0, initial_CIP_value + 3000.0, 16000.0, 14000.0, 0.5};

                    Func<double[], double> funcion = delegate (double[] variables)
                    {

                        puntero_aplicacion.luis.RecompCycle_PCRCMCI_withTwoReheating_for_optimization(puntero_aplicacion.luis,
                        ref cicloPCRCMCI_withTwoRH, puntero_aplicacion.w_dot_net, puntero_aplicacion.t_pc_in,
                        puntero_aplicacion.t_mc1_in, puntero_aplicacion.t_mc2_in, puntero_aplicacion.t_t_in,
                        puntero_aplicacion.t_rht1_in, variables[4], puntero_aplicacion.t_rht2_in,
                        variables[5], variables[1], variables[2],
                        variables[3], puntero_aplicacion.p_mc2_out, variables[2],
                        variables[3], variables[6], UA_Total,
                        puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_pc, puntero_aplicacion.m_eta_rc,
                        puntero_aplicacion.m_eta_mc1, puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1,
                        puntero_aplicacion.m_eta_trh2, puntero_aplicacion.n_sub_hxrs, variables[0],
                        puntero_aplicacion.tol, puntero_aplicacion.eta_thermal, -puntero_aplicacion.dp_lt1,
                        -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2,
                        -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_pc3,
                        -puntero_aplicacion.dp_phx, -puntero_aplicacion.dp_rhx1, -puntero_aplicacion.dp_rhx2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRCMCI_withTwoRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRCMCI_withTwoRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRCMCI_withTwoRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in = variables[1];
                        puntero_aplicacion.p_pc_out = variables[2];
                        puntero_aplicacion.p_mc1_in = variables[2];
                        puntero_aplicacion.p_mc1_out = variables[3];
                        puntero_aplicacion.p_mc2_in = variables[3];
                        puntero_aplicacion.p_rhx1_in = variables[4];
                        puntero_aplicacion.p_rhx2_in = variables[5];
                        LT_fraction = variables[6];
                        puntero_aplicacion.ua_lt = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloPCRCMCI_withTwoRH.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRCMCI_withTwoRH.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRCMCI_withTwoRH.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRCMCI_withTwoRH.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRCMCI_withTwoRH.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRCMCI_withTwoRH.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRCMCI_withTwoRH.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRCMCI_withTwoRH.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRCMCI_withTwoRH.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRCMCI_withTwoRH.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRCMCI_withTwoRH.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRCMCI_withTwoRH.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRCMCI_withTwoRH.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRCMCI_withTwoRH.temp[13];
                        puntero_aplicacion.temp215 = cicloPCRCMCI_withTwoRH.temp[14];
                        puntero_aplicacion.temp216 = cicloPCRCMCI_withTwoRH.temp[15];
                        puntero_aplicacion.temp217 = cicloPCRCMCI_withTwoRH.temp[16];
                        puntero_aplicacion.temp218 = cicloPCRCMCI_withTwoRH.temp[17];

                        puntero_aplicacion.pres21 = cicloPCRCMCI_withTwoRH.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRCMCI_withTwoRH.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRCMCI_withTwoRH.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRCMCI_withTwoRH.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRCMCI_withTwoRH.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRCMCI_withTwoRH.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRCMCI_withTwoRH.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRCMCI_withTwoRH.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRCMCI_withTwoRH.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRCMCI_withTwoRH.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRCMCI_withTwoRH.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRCMCI_withTwoRH.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRCMCI_withTwoRH.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRCMCI_withTwoRH.pres[13];
                        puntero_aplicacion.pres215 = cicloPCRCMCI_withTwoRH.pres[14];
                        puntero_aplicacion.pres216 = cicloPCRCMCI_withTwoRH.pres[15];
                        puntero_aplicacion.pres217 = cicloPCRCMCI_withTwoRH.pres[16];
                        puntero_aplicacion.pres218 = cicloPCRCMCI_withTwoRH.pres[17];

                        puntero_aplicacion.PHX1 = cicloPCRCMCI_withTwoRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloPCRCMCI_withTwoRH.RHX1.Q_dot;
                        puntero_aplicacion.RHX2 = cicloPCRCMCI_withTwoRH.RHX2.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRCMCI_withTwoRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRCMCI_withTwoRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRCMCI_withTwoRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRCMCI_withTwoRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRCMCI_withTwoRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRCMCI_withTwoRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRCMCI_withTwoRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRCMCI_withTwoRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRCMCI_withTwoRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRCMCI_withTwoRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRCMCI_withTwoRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRCMCI_withTwoRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRCMCI_withTwoRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRCMCI_withTwoRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRCMCI_withTwoRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRCMCI_withTwoRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRCMCI_withTwoRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRCMCI_withTwoRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRCMCI_withTwoRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRCMCI_withTwoRH.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRCMCI_withTwoRH.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloPCRCMCI_withTwoRH.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloPCRCMCI_withTwoRH.PC3.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out);
                        p_rhx1_list.Add(puntero_aplicacion.p_rhx1_in);
                        p_rhx2_list.Add(puntero_aplicacion.p_rhx2_in);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc1_in.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_mc1_out.ToString());
                        listBox23.Items.Add(puntero_aplicacion.p_rhx1_in.ToString());
                        listBox24.Items.Add(puntero_aplicacion.p_rhx2_in.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                        double LTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[2];
                        double LTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[8] - cicloPCRCMCI_withTwoRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[3];
                        double HTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[6] - cicloPCRCMCI_withTwoRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc1_out);
                        //P_rxh1(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_rhx1_in);
                        //P_rxh2(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_rhx2_in);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.t_mc1_in - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.ua_lt);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.ua_ht);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRCMCI_withTwoRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloPCRCMCI_withTwoRH.HT.eff.ToString();
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

                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                    textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                    textBox5.Text = p_mc1_out2_list[maxIndex].ToString();
                    textBox6.Text = p_rhx1_list[maxIndex].ToString();
                    textBox7.Text = p_rhx2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list[maxIndex].ToString();
                    textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox7.Text = p_rhx1_list[maxIndex].ToString();
                        puntero_aplicacion.textBox113.Text = p_rhx2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "PCRCMCI_with_Two_ReHeating" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }

        }

        //CIT Optimization analysis
        private void button3_Click(object sender, System.EventArgs e)
        {
            int counter = 0;

            double initial_pc_in_value = 0;
            double initial_pc_out_value = 0;
            double initial_mc1_out_value = 0;

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
                            puntero_aplicacion.luis.core1(this.comboBox2.Text, puntero_aplicacion.category);
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
                        puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox48.Text);

                        puntero_aplicacion.t_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);

                        puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                        puntero_aplicacion.p_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc1_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                        puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                        puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                        puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                        puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                        puntero_aplicacion.p_rhx1_in = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                        puntero_aplicacion.t_rht1_in = Convert.ToDouble(puntero_aplicacion.textBox6.Text);

                        puntero_aplicacion.p_rhx2_in = Convert.ToDouble(puntero_aplicacion.textBox113.Text);
                        puntero_aplicacion.t_rht2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);

                        puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                        puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);

                        puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                        puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                        puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        //puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                        puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                        puntero_aplicacion.m_eta_mc1 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                        puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);

                        puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                        puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.PCRCMCIwithTwoReheating cicloPCRCMCI_withTwoRH = new core.PCRCMCIwithTwoReheating();

                        double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                        double LT_fraction = 0.1;

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_pc_in2_list = new List<Double>();
                        List<Double> p_pc_out2_list = new List<Double>();
                        List<Double> p_mc1_out2_list = new List<Double>();
                        List<Double> p_rhx1_list = new List<Double>();
                        List<Double> p_rhx2_list = new List<Double>();
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
                            xlWorkSheet1.Cells[4, 3] = "MC1_out(kPa)";
                            xlWorkSheet1.Cells[4, 4] = "P_rhx1(kPa)";
                            xlWorkSheet1.Cells[4, 5] = "P_rhx2(kPa)";
                            xlWorkSheet1.Cells[4, 6] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 7] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 8] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 9] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 10] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 11] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 12] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 13] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 14] = "HTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 15] = "PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 16] = "PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 17] = "LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 18] = "LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 19] = "ReHeating1_PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 20] = "ReHeating1_PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 21] = "ReHeating1_LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 22] = "ReHeating1_LF_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 23] = "ReHeating2_PTC_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 24] = "ReHeating2_PTC_Pressure_Drop(bar)";
                            xlWorkSheet1.Cells[4, 25] = "ReHeating2_LF_Apperture_Area(m2)";
                            xlWorkSheet1.Cells[4, 26] = "ReHeating2_LF_Pressure_Drop(bar)";
                        }

                        using (var solver = new NLoptSolver(algorithm_type, 6, 0.000001, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500.0, initial_CIP_value + 1000.0, initial_CIP_value + 3000.0, initial_CIP_value + 2000.0 });
                            solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 1000.0, 1000.0 });

                            var initialValue = new[] { 0.2, initial_CIP_value + 1000.0, initial_CIP_value + 2000.0, initial_CIP_value + 3000.0, 16000.0, 14000.0 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {

                                puntero_aplicacion.luis.RecompCycle_PCRCMCI_withTwoReheating(puntero_aplicacion.luis,
                                ref cicloPCRCMCI_withTwoRH, puntero_aplicacion.w_dot_net, i,
                                i, i, puntero_aplicacion.t_t_in,
                                puntero_aplicacion.t_rht1_in, variables[4], puntero_aplicacion.t_rht2_in,
                                variables[5], variables[1], variables[2],
                                variables[3], puntero_aplicacion.p_mc2_out, variables[2],
                                variables[3], puntero_aplicacion.ua_lt, puntero_aplicacion.ua_ht,
                                puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_pc, puntero_aplicacion.m_eta_rc,
                                puntero_aplicacion.m_eta_mc1, puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1,
                                puntero_aplicacion.m_eta_trh2, puntero_aplicacion.n_sub_hxrs, variables[0],
                                puntero_aplicacion.tol, puntero_aplicacion.eta_thermal, -puntero_aplicacion.dp_lt1,
                                -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2,
                                -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_pc3,
                                -puntero_aplicacion.dp_phx, -puntero_aplicacion.dp_rhx1, -puntero_aplicacion.dp_rhx2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloPCRCMCI_withTwoRH.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloPCRCMCI_withTwoRH.W_dot_net;
                                puntero_aplicacion.eta_thermal2 = cicloPCRCMCI_withTwoRH.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_pc_in = variables[1];
                                puntero_aplicacion.p_pc_out = variables[2];
                                puntero_aplicacion.p_mc1_in = variables[2];
                                puntero_aplicacion.p_mc1_out = variables[3];
                                puntero_aplicacion.p_mc2_in = variables[3];
                                puntero_aplicacion.p_rhx1_in = variables[4];
                                puntero_aplicacion.p_rhx2_in = variables[5];

                                puntero_aplicacion.temp21 = cicloPCRCMCI_withTwoRH.temp[0];
                                puntero_aplicacion.temp22 = cicloPCRCMCI_withTwoRH.temp[1];
                                puntero_aplicacion.temp23 = cicloPCRCMCI_withTwoRH.temp[2];
                                puntero_aplicacion.temp24 = cicloPCRCMCI_withTwoRH.temp[3];
                                puntero_aplicacion.temp25 = cicloPCRCMCI_withTwoRH.temp[4];
                                puntero_aplicacion.temp26 = cicloPCRCMCI_withTwoRH.temp[5];
                                puntero_aplicacion.temp27 = cicloPCRCMCI_withTwoRH.temp[6];
                                puntero_aplicacion.temp28 = cicloPCRCMCI_withTwoRH.temp[7];
                                puntero_aplicacion.temp29 = cicloPCRCMCI_withTwoRH.temp[8];
                                puntero_aplicacion.temp210 = cicloPCRCMCI_withTwoRH.temp[9];
                                puntero_aplicacion.temp211 = cicloPCRCMCI_withTwoRH.temp[10];
                                puntero_aplicacion.temp212 = cicloPCRCMCI_withTwoRH.temp[11];
                                puntero_aplicacion.temp213 = cicloPCRCMCI_withTwoRH.temp[12];
                                puntero_aplicacion.temp214 = cicloPCRCMCI_withTwoRH.temp[13];
                                puntero_aplicacion.temp215 = cicloPCRCMCI_withTwoRH.temp[14];
                                puntero_aplicacion.temp216 = cicloPCRCMCI_withTwoRH.temp[15];
                                puntero_aplicacion.temp217 = cicloPCRCMCI_withTwoRH.temp[16];
                                puntero_aplicacion.temp218 = cicloPCRCMCI_withTwoRH.temp[17];

                                puntero_aplicacion.pres21 = cicloPCRCMCI_withTwoRH.pres[0];
                                puntero_aplicacion.pres22 = cicloPCRCMCI_withTwoRH.pres[1];
                                puntero_aplicacion.pres23 = cicloPCRCMCI_withTwoRH.pres[2];
                                puntero_aplicacion.pres24 = cicloPCRCMCI_withTwoRH.pres[3];
                                puntero_aplicacion.pres25 = cicloPCRCMCI_withTwoRH.pres[4];
                                puntero_aplicacion.pres26 = cicloPCRCMCI_withTwoRH.pres[5];
                                puntero_aplicacion.pres27 = cicloPCRCMCI_withTwoRH.pres[6];
                                puntero_aplicacion.pres28 = cicloPCRCMCI_withTwoRH.pres[7];
                                puntero_aplicacion.pres29 = cicloPCRCMCI_withTwoRH.pres[8];
                                puntero_aplicacion.pres210 = cicloPCRCMCI_withTwoRH.pres[9];
                                puntero_aplicacion.pres211 = cicloPCRCMCI_withTwoRH.pres[10];
                                puntero_aplicacion.pres212 = cicloPCRCMCI_withTwoRH.pres[11];
                                puntero_aplicacion.pres213 = cicloPCRCMCI_withTwoRH.pres[12];
                                puntero_aplicacion.pres214 = cicloPCRCMCI_withTwoRH.pres[13];
                                puntero_aplicacion.pres215 = cicloPCRCMCI_withTwoRH.pres[14];
                                puntero_aplicacion.pres216 = cicloPCRCMCI_withTwoRH.pres[15];
                                puntero_aplicacion.pres217 = cicloPCRCMCI_withTwoRH.pres[16];
                                puntero_aplicacion.pres218 = cicloPCRCMCI_withTwoRH.pres[17];

                                puntero_aplicacion.PHX1 = cicloPCRCMCI_withTwoRH.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloPCRCMCI_withTwoRH.RHX1.Q_dot;
                                puntero_aplicacion.RHX2 = cicloPCRCMCI_withTwoRH.RHX2.Q_dot;

                                puntero_aplicacion.LT_Q = cicloPCRCMCI_withTwoRH.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloPCRCMCI_withTwoRH.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloPCRCMCI_withTwoRH.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloPCRCMCI_withTwoRH.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloPCRCMCI_withTwoRH.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloPCRCMCI_withTwoRH.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloPCRCMCI_withTwoRH.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloPCRCMCI_withTwoRH.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloPCRCMCI_withTwoRH.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloPCRCMCI_withTwoRH.LT.eff;

                                puntero_aplicacion.HT_Q = cicloPCRCMCI_withTwoRH.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloPCRCMCI_withTwoRH.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloPCRCMCI_withTwoRH.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloPCRCMCI_withTwoRH.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloPCRCMCI_withTwoRH.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloPCRCMCI_withTwoRH.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloPCRCMCI_withTwoRH.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloPCRCMCI_withTwoRH.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloPCRCMCI_withTwoRH.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloPCRCMCI_withTwoRH.HT.eff;

                                puntero_aplicacion.PC1 = -cicloPCRCMCI_withTwoRH.PC1.Q_dot;
                                puntero_aplicacion.PC2 = -cicloPCRCMCI_withTwoRH.PC2.Q_dot;
                                puntero_aplicacion.PC3 = -cicloPCRCMCI_withTwoRH.PC3.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                                p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                                p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out);
                                p_rhx1_list.Add(puntero_aplicacion.p_rhx1_in);
                                p_rhx2_list.Add(puntero_aplicacion.p_rhx2_in);
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

                                PHX_Q2_list.Add(cicloPCRCMCI_withTwoRH.PHX.Q_dot);
                                RHX1_Q2_list.Add(cicloPCRCMCI_withTwoRH.RHX1.Q_dot);
                                RHX2_Q2_list.Add(cicloPCRCMCI_withTwoRH.RHX2.Q_dot);

                                HT_Eff_list.Add(cicloPCRCMCI_withTwoRH.HT.eff);
                                LT_Eff_list.Add(cicloPCRCMCI_withTwoRH.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                                listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                                listBox20.Items.Add(puntero_aplicacion.p_mc1_in.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_mc1_out.ToString());
                                listBox23.Items.Add(puntero_aplicacion.p_rhx1_in.ToString());
                                listBox24.Items.Add(puntero_aplicacion.p_rhx2_in.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                                listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                                //double LTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[2];
                                //double LTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[8] - cicloPCRCMCI_withTwoRH.temp[1];
                                //double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                //double HTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[3];
                                //double HTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[6] - cicloPCRCMCI_withTwoRH.temp[4];
                                //double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                                ////PC_in(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                                ////PC_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                                ////MC1_out(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc1_out);
                                ////P_rxh1(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_rhx1_in);
                                ////P_rxh2(kPa)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_rhx2_in);
                                ////CIT
                                //xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.t_mc1_in - 273.15);
                                ////LT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.ua_lt);
                                ////HT UA(kW/K)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.ua_ht);
                                ////Rec.Frac.
                                //xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.recomp_frac2.ToString();
                                ////Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 10] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                                ////LTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRCMCI_withTwoRH.LT.eff.ToString();
                                ////LTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                                ////HTR Eff.(%)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloPCRCMCI_withTwoRH.HT.eff.ToString();
                                ////HTR Pinch(ºC)
                                //xlWorkSheet1.Cells[counter_Excel + 1, 14] = HTR_min_DT_paper.ToString();

                                //counter_Excel++;

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
                            textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                            textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                            textBox5.Text = p_mc1_out2_list[maxIndex].ToString();
                            textBox6.Text = p_rhx1_list[maxIndex].ToString();
                            textBox7.Text = p_rhx2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list[maxIndex].ToString();
                            textBox83.Text = ua_HT_list[maxIndex].ToString();

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox7.Text = p_rhx1_list[maxIndex].ToString();
                                puntero_aplicacion.textBox113.Text = p_rhx2_list[maxIndex].ToString();
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox2.Text = i.ToString();
                                puntero_aplicacion.textBox28.Text = i.ToString();
                                puntero_aplicacion.textBox102.Text = i.ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                            listBox15.Items.Add(p_pc_in2_list[maxIndex].ToString());
                            listBox10.Items.Add(p_pc_out2_list[maxIndex].ToString());
                            listBox23.Items.Add(p_rhx1_list[maxIndex].ToString());
                            listBox24.Items.Add(p_rhx2_list[maxIndex].ToString());
                            listBox22.Items.Add(p_pc_out2_list[maxIndex].ToString());
                            listBox21.Items.Add(p_mc1_out2_list[maxIndex].ToString());
                            listBox11.Items.Add(t8_list[maxIndex].ToString());
                            listBox12.Items.Add(t9_list[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list[maxIndex].ToString());

                            //Main_Solar_Field
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

                            LF_SF_Calculation LF = new LF_SF_Calculation();
                            LF.calledForSensingAnalysis = true;
                            LF.comboBox1.Text = "Solar Salt";
                            LF.comboBox2.Text = "PureFluid";
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

                            //ReHeating_Solar_Field_1
                            PTC_SF_Calculation PTC_RHX1 = new PTC_SF_Calculation();
                            PTC_RHX1.calledForSensingAnalysis = true;
                            PTC_RHX1.comboBox1.Text = "Solar Salt";
                            PTC_RHX1.comboBox2.Text = "PureFluid";
                            PTC_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
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

                            PTC_RHX1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            PTC_RHX1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp215);
                            PTC_RHX1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp216);
                            PTC_RHX1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres215);
                            PTC_RHX1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres216);
                            PTC_RHX1.textBox107.Text = Convert.ToString(10);
                            PTC_RHX1.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area = PTC_RHX1.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop = PTC_RHX1.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_RHX1 = new LF_SF_Calculation();
                            LF_RHX1.calledForSensingAnalysis = true;
                            LF_RHX1.comboBox1.Text = "Solar Salt";
                            LF_RHX1.comboBox2.Text = "PureFluid";
                            LF_RHX1.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX1.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX1.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX1);
                            LF_RHX1.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX1.textBox3.Text = Convert.ToString(puntero_aplicacion.temp215);
                            LF_RHX1.textBox6.Text = Convert.ToString(puntero_aplicacion.temp216);
                            LF_RHX1.textBox4.Text = Convert.ToString(puntero_aplicacion.pres215);
                            LF_RHX1.textBox5.Text = Convert.ToString(puntero_aplicacion.pres216);
                            LF_RHX1.textBox107.Text = Convert.ToString(10);
                            LF_RHX1.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area = LF_RHX1.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop = LF_RHX1.Total_Pressure_DropResult;

                            //ReHeating_Solar_Field_2
                            PTC_SF_Calculation PTC_RHX2 = new PTC_SF_Calculation();
                            PTC_RHX2.calledForSensingAnalysis = true;
                            PTC_RHX2.comboBox1.Text = "Solar Salt";
                            PTC_RHX2.comboBox2.Text = "PureFluid";
                            PTC_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            PTC_RHX2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //PTC.textBox31.Text = this.textBox31.Text;
                            //PTC.textBox36.Text = this.textBox36.Text;

                            if (comboBox4.Text == "Parabolic")
                            {
                                PTC_RHX2.textBox7.Text = "0.141";
                                PTC_RHX2.textBox8.Text = "6.48e-9";
                            }
                            else if (comboBox4.Text == "Parabolic with cavity receiver (Norwich)")
                            {
                                PTC_RHX2.textBox7.Text = "0.3";
                                PTC_RHX2.textBox8.Text = "3.25e-9";
                            }

                            PTC_RHX2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            PTC_RHX2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            PTC_RHX2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            PTC_RHX2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            PTC_RHX2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            PTC_RHX2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            PTC_RHX2.textBox107.Text = Convert.ToString(10);
                            PTC_RHX2.button1_Click(this, e);
                            puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area = PTC_RHX2.ReflectorApertureAreaResult;
                            puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop = PTC_RHX2.Total_Pressure_DropResult;

                            LF_SF_Calculation LF_RHX2 = new LF_SF_Calculation();
                            LF_RHX2.calledForSensingAnalysis = true;
                            LF_RHX2.comboBox1.Text = "Solar Salt";
                            LF_RHX2.comboBox2.Text = "PureFluid";
                            LF_RHX2.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                            LF_RHX2.comboBox14.Text = puntero_aplicacion.comboBox16.Text;
                            //LF.textBox31.Text = this.textBox31.Text;
                            //LF.textBox36.Text = this.textBox36.Text;
                            LF_RHX2.textBox1.Text = Convert.ToString(puntero_aplicacion.RHX2);
                            LF_RHX2.textBox2.Text = Convert.ToString(puntero_aplicacion.massflow2);
                            LF_RHX2.textBox3.Text = Convert.ToString(puntero_aplicacion.temp217);
                            LF_RHX2.textBox6.Text = Convert.ToString(puntero_aplicacion.temp218);
                            LF_RHX2.textBox4.Text = Convert.ToString(puntero_aplicacion.pres217);
                            LF_RHX2.textBox5.Text = Convert.ToString(puntero_aplicacion.pres218);
                            LF_RHX2.textBox107.Text = Convert.ToString(10);
                            LF_RHX2.button1_Click(this, e);
                            puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area = LF_RHX2.ReflectorApertureAreaResult;
                            puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop = LF_RHX2.Total_Pressure_DropResult;

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
                            //MC1_out(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = p_mc1_out2_list[maxIndex].ToString();
                            //P_rhx1(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = p_rhx1_list[maxIndex].ToString();
                            //P_rhx2(kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = p_rhx2_list[maxIndex].ToString();
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.ua_lt);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.ua_ht);
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = recomp_frac2_list[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = (eta_thermal2_list[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRCMCI_withTwoRH.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloPCRCMCI_withTwoRH.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 14] = HTR_min_DT_paper.ToString();
                            //PTC_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                            //PTC_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 16] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                            //LF_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 17] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                            //LF_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 18] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            //PTC_ReHeating_1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 19] = puntero_aplicacion.PTC_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //PTC_ReHeating_1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 20] = puntero_aplicacion.PTC_ReHeating1_SF_Pressure_drop.ToString();
                            //LF_ReHeating_1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 21] = puntero_aplicacion.LF_ReHeating1_SF_Effective_Apperture_Area.ToString();
                            //LF_ReHeating_1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 22] = puntero_aplicacion.LF_ReHeating1_SF_Pressure_drop.ToString();
                            //PTC_ReHeating_1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 23] = puntero_aplicacion.PTC_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //PTC_ReHeating_1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 24] = puntero_aplicacion.PTC_ReHeating2_SF_Pressure_drop.ToString();
                            //LF_ReHeating_1_Apperture_Area(m2)
                            xlWorkSheet1.Cells[counter_Excel + 1, 25] = puntero_aplicacion.LF_ReHeating2_SF_Effective_Apperture_Area.ToString();
                            //LF_ReHeating_1_Pressure_Drop(bar)
                            xlWorkSheet1.Cells[counter_Excel + 1, 26] = puntero_aplicacion.LF_ReHeating2_SF_Pressure_drop.ToString();

                            counter_Excel++;

                            initial_pc_in_value = puntero_aplicacion.p_pc_in;
                            initial_pc_out_value = puntero_aplicacion.p_pc_out;
                            initial_mc1_out_value = puntero_aplicacion.p_mc1_out;
                        }
                    }

                    ////Optimization UA true
                    //else if (checkBox2.Checked == true)
                    //{
                    //    //PureFluid
                    //    if (puntero_aplicacion.comboBox1.Text == "PureFluid")
                    //    {
                    //        puntero_aplicacion.category = RefrigerantCategory.PureFluid;
                    //        puntero_aplicacion.luis.core1(this.comboBox2.Text, puntero_aplicacion.category);
                    //    }

                    //    //NewMixture
                    //    if (puntero_aplicacion.comboBox1.Text == "NewMixture")
                    //    {
                    //        puntero_aplicacion.category = RefrigerantCategory.NewMixture;
                    //        puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + "=" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + "=" + puntero_aplicacion.textBox70.Text, puntero_aplicacion.category);
                    //    }

                    //    if (puntero_aplicacion.comboBox1.Text == "PredefinedMixture")
                    //    {
                    //        puntero_aplicacion.category = RefrigerantCategory.PredefinedMixture;
                    //    }

                    //    if (puntero_aplicacion.comboBox1.Text == "PseudoPureFluid")
                    //    {
                    //        puntero_aplicacion.category = RefrigerantCategory.PseudoPureFluid;
                    //    }

                    //    if (puntero_aplicacion.comboBox3.Text == "DEF")
                    //    {
                    //        puntero_aplicacion.referencestate = ReferenceState.DEF;
                    //    }
                    //    if (puntero_aplicacion.comboBox3.Text == "ASH")
                    //    {
                    //        puntero_aplicacion.referencestate = ReferenceState.ASH;
                    //    }
                    //    if (puntero_aplicacion.comboBox3.Text == "IIR")
                    //    {
                    //        puntero_aplicacion.referencestate = ReferenceState.IIR;
                    //    }
                    //    if (puntero_aplicacion.comboBox3.Text == "NBP")
                    //    {
                    //        puntero_aplicacion.referencestate = ReferenceState.NBP;
                    //    }

                    //    puntero_aplicacion.luis.working_fluid.Category = puntero_aplicacion.category;
                    //    puntero_aplicacion.luis.working_fluid.reference = puntero_aplicacion.referencestate;

                    //    //Store Input Data from Graphical User Interface GUI into variables
                    //    puntero_aplicacion.w_dot_net = Convert.ToDouble(puntero_aplicacion.textBox48.Text);

                    //    puntero_aplicacion.t_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                    //    puntero_aplicacion.t_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox28.Text);

                    //    puntero_aplicacion.t_t_in = Convert.ToDouble(puntero_aplicacion.textBox4.Text);

                    //    puntero_aplicacion.p_mc1_in = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                    //    puntero_aplicacion.p_mc1_out = Convert.ToDouble(puntero_aplicacion.textBox8.Text);

                    //    puntero_aplicacion.p_mc2_in = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                    //    puntero_aplicacion.p_mc2_out = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                    //    puntero_aplicacion.p_pc_in = Convert.ToDouble(puntero_aplicacion.textBox103.Text);
                    //    puntero_aplicacion.p_pc_out = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                    //    puntero_aplicacion.t_pc_in = Convert.ToDouble(puntero_aplicacion.textBox102.Text);

                    //    puntero_aplicacion.p_rhx1_in = Convert.ToDouble(puntero_aplicacion.textBox7.Text);
                    //    puntero_aplicacion.t_rht1_in = Convert.ToDouble(puntero_aplicacion.textBox6.Text);

                    //    puntero_aplicacion.p_rhx2_in = Convert.ToDouble(puntero_aplicacion.textBox113.Text);
                    //    puntero_aplicacion.t_rht2_in = Convert.ToDouble(puntero_aplicacion.textBox88.Text);

                    //    puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                    //    puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                    //    puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                    //    puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                    //    puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);

                    //    puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                    //    puntero_aplicacion.dp_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox9.Text);
                    //    puntero_aplicacion.dp_rhx2 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);

                    //    puntero_aplicacion.dp_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                    //    puntero_aplicacion.dp_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                    //    puntero_aplicacion.ua_lt = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                    //    puntero_aplicacion.ua_ht = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                    //    puntero_aplicacion.m_recomp_frac = Convert.ToDouble(puntero_aplicacion.textBox15.Text);

                    //    puntero_aplicacion.m_eta_pc = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                    //    puntero_aplicacion.m_eta_mc1 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                    //    puntero_aplicacion.m_eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                    //    puntero_aplicacion.m_eta_rc = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                    //    puntero_aplicacion.m_eta_t = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                    //    puntero_aplicacion.m_eta_trh1 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                    //    puntero_aplicacion.m_eta_trh2 = Convert.ToDouble(puntero_aplicacion.textBox18.Text);

                    //    puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                    //    puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                    //    puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                    //    core.PCRCMCIwithTwoReheating cicloPCRCMCI_withTwoRH = new core.PCRCMCIwithTwoReheating();

                    //    double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                    //    double LT_fraction = 0.1;

                    //    List<Double> recomp_frac2_list = new List<Double>();
                    //    List<Double> p_pc_in2_list = new List<Double>();
                    //    List<Double> p_pc_out2_list = new List<Double>();
                    //    List<Double> p_mc1_out2_list = new List<Double>();
                    //    List<Double> p_rhx1_list = new List<Double>();
                    //    List<Double> p_rhx2_list = new List<Double>();
                    //    List<Double> eta_thermal2_list = new List<Double>();
                    //    List<Double> ua_LT_list = new List<Double>();
                    //    List<Double> ua_HT_list = new List<Double>();

                    //    NLoptAlgorithm algorithm_type = NLoptAlgorithm.LN_BOBYQA;

                    //    if (comboBox19.Text == "BOBYQA")
                    //        algorithm_type = NLoptAlgorithm.LN_BOBYQA;
                    //    else if (comboBox19.Text == "COBYLA")
                    //        algorithm_type = NLoptAlgorithm.LN_COBYLA;
                    //    else if (comboBox19.Text == "SUBPLEX")
                    //        algorithm_type = NLoptAlgorithm.LN_SBPLX;
                    //    else if (comboBox19.Text == "NELDER-MEAD")
                    //        algorithm_type = NLoptAlgorithm.LN_NELDERMEAD;
                    //    else if (comboBox19.Text == "NEWUOA")
                    //        algorithm_type = NLoptAlgorithm.LN_NEWUOA;
                    //    else if (comboBox19.Text == "PRAXIS")
                    //        algorithm_type = NLoptAlgorithm.LN_PRAXIS;

                    //    if (checkBox6.Checked == true)
                    //    {
                    //        initial_CIP_value = Convert.ToDouble(textBox1.Text);
                    //    }
                    //    else
                    //    {
                    //        initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                    //    }

                    //    xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                    //    xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox35.Text + "," + puntero_aplicacion.comboBox16.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox18.Text + ":" + puntero_aplicacion.textBox69.Text + "," + puntero_aplicacion.comboBox17.Text + ":" + puntero_aplicacion.textBox70.Text;
                    //    xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
                    //    xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

                    //    xlWorkSheet1.Cells[2, 1] = "";
                    //    xlWorkSheet1.Cells[2, 2] = Convert.ToString(puntero_aplicacion.MixtureCriticalPressure);
                    //    xlWorkSheet1.Cells[2, 3] = Convert.ToString(puntero_aplicacion.MixtureCriticalTemperature - 273.15);

                    //    xlWorkSheet1.Cells[3, 1] = "";
                    //    xlWorkSheet1.Cells[3, 2] = "";
                    //    xlWorkSheet1.Cells[4, 3] = "";

                    //    xlWorkSheet1.Cells[4, 1] = "PC_in(kPa)";
                    //    xlWorkSheet1.Cells[4, 2] = "PC_out(kPa)";
                    //    xlWorkSheet1.Cells[4, 3] = "MC1_out(kPa)";
                    //    xlWorkSheet1.Cells[4, 4] = "P_rhx1(kPa)";
                    //    xlWorkSheet1.Cells[4, 5] = "P_rhx2(kPa)";
                    //    xlWorkSheet1.Cells[4, 6] = "CIT(K)";
                    //    xlWorkSheet1.Cells[4, 7] = "LT UA(kW/K)";
                    //    xlWorkSheet1.Cells[4, 8] = "HT UA(kW/K)";
                    //    xlWorkSheet1.Cells[4, 9] = "Rec.Frac.";
                    //    xlWorkSheet1.Cells[4, 10] = "Eff.(%)";
                    //    xlWorkSheet1.Cells[4, 11] = "LTR Eff.(%)";
                    //    xlWorkSheet1.Cells[4, 12] = "LTR Pinch(ºC)";
                    //    xlWorkSheet1.Cells[4, 13] = "HTR Eff.(%)";
                    //    xlWorkSheet1.Cells[4, 14] = "HTR Pinch(ºC)";

                    //    using (var solver = new NLoptSolver(algorithm_type, 7, 0.000001, 10000))
                    //    {
                    //        solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500.0, initial_CIP_value + 1000.0, initial_CIP_value + 3000.0, initial_CIP_value + 2000.0, 0.0 });
                    //        solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, 1.0 });

                    //        solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0, 1000.0, 1000.0, 0.05 });

                    //        var initialValue = new[] { 0.2, initial_CIP_value + 1000.0, initial_CIP_value + 2000.0, initial_CIP_value + 3000.0, 16000.0, 14000.0, 0.5 };

                    //        Func<double[], double> funcion = delegate (double[] variables)
                    //        {

                    //            puntero_aplicacion.luis.RecompCycle_PCRCMCI_withTwoReheating_for_optimization(puntero_aplicacion.luis,
                    //            ref cicloPCRCMCI_withTwoRH, puntero_aplicacion.w_dot_net, puntero_aplicacion.t_pc_in,
                    //            puntero_aplicacion.t_mc1_in, puntero_aplicacion.t_mc2_in, puntero_aplicacion.t_t_in,
                    //            puntero_aplicacion.t_rht1_in, variables[4], puntero_aplicacion.t_rht2_in,
                    //            variables[5], variables[1], variables[2],
                    //            variables[3], puntero_aplicacion.p_mc2_out, variables[2],
                    //            variables[3], variables[6], UA_Total,
                    //            puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_pc, puntero_aplicacion.m_eta_rc,
                    //            puntero_aplicacion.m_eta_mc1, puntero_aplicacion.m_eta_t, puntero_aplicacion.m_eta_trh1,
                    //            puntero_aplicacion.m_eta_trh2, puntero_aplicacion.n_sub_hxrs, variables[0],
                    //            puntero_aplicacion.tol, puntero_aplicacion.eta_thermal, -puntero_aplicacion.dp_lt1,
                    //            -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2,
                    //            -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_pc3,
                    //            -puntero_aplicacion.dp_phx, -puntero_aplicacion.dp_rhx1, -puntero_aplicacion.dp_rhx2);

                    //            counter++;

                    //            puntero_aplicacion.massflow2 = cicloPCRCMCI_withTwoRH.m_dot_turbine;
                    //            puntero_aplicacion.w_dot_net2 = cicloPCRCMCI_withTwoRH.W_dot_net;
                    //            puntero_aplicacion.eta_thermal2 = cicloPCRCMCI_withTwoRH.eta_thermal;
                    //            puntero_aplicacion.recomp_frac2 = variables[0];
                    //            puntero_aplicacion.p_pc_in = variables[1];
                    //            puntero_aplicacion.p_pc_out = variables[2];
                    //            puntero_aplicacion.p_mc1_in = variables[2];
                    //            puntero_aplicacion.p_mc1_out = variables[3];
                    //            puntero_aplicacion.p_mc2_in = variables[3];
                    //            puntero_aplicacion.p_rhx1_in = variables[4];
                    //            puntero_aplicacion.p_rhx2_in = variables[5];
                    //            LT_fraction = variables[6];
                    //            puntero_aplicacion.ua_lt = UA_Total * LT_fraction;
                    //            puntero_aplicacion.ua_ht = UA_Total * (1 - LT_fraction);

                    //            puntero_aplicacion.temp21 = cicloPCRCMCI_withTwoRH.temp[0];
                    //            puntero_aplicacion.temp22 = cicloPCRCMCI_withTwoRH.temp[1];
                    //            puntero_aplicacion.temp23 = cicloPCRCMCI_withTwoRH.temp[2];
                    //            puntero_aplicacion.temp24 = cicloPCRCMCI_withTwoRH.temp[3];
                    //            puntero_aplicacion.temp25 = cicloPCRCMCI_withTwoRH.temp[4];
                    //            puntero_aplicacion.temp26 = cicloPCRCMCI_withTwoRH.temp[5];
                    //            puntero_aplicacion.temp27 = cicloPCRCMCI_withTwoRH.temp[6];
                    //            puntero_aplicacion.temp28 = cicloPCRCMCI_withTwoRH.temp[7];
                    //            puntero_aplicacion.temp29 = cicloPCRCMCI_withTwoRH.temp[8];
                    //            puntero_aplicacion.temp210 = cicloPCRCMCI_withTwoRH.temp[9];
                    //            puntero_aplicacion.temp211 = cicloPCRCMCI_withTwoRH.temp[10];
                    //            puntero_aplicacion.temp212 = cicloPCRCMCI_withTwoRH.temp[11];
                    //            puntero_aplicacion.temp213 = cicloPCRCMCI_withTwoRH.temp[12];
                    //            puntero_aplicacion.temp214 = cicloPCRCMCI_withTwoRH.temp[13];
                    //            puntero_aplicacion.temp215 = cicloPCRCMCI_withTwoRH.temp[14];
                    //            puntero_aplicacion.temp216 = cicloPCRCMCI_withTwoRH.temp[15];
                    //            puntero_aplicacion.temp217 = cicloPCRCMCI_withTwoRH.temp[16];
                    //            puntero_aplicacion.temp218 = cicloPCRCMCI_withTwoRH.temp[17];

                    //            puntero_aplicacion.pres21 = cicloPCRCMCI_withTwoRH.pres[0];
                    //            puntero_aplicacion.pres22 = cicloPCRCMCI_withTwoRH.pres[1];
                    //            puntero_aplicacion.pres23 = cicloPCRCMCI_withTwoRH.pres[2];
                    //            puntero_aplicacion.pres24 = cicloPCRCMCI_withTwoRH.pres[3];
                    //            puntero_aplicacion.pres25 = cicloPCRCMCI_withTwoRH.pres[4];
                    //            puntero_aplicacion.pres26 = cicloPCRCMCI_withTwoRH.pres[5];
                    //            puntero_aplicacion.pres27 = cicloPCRCMCI_withTwoRH.pres[6];
                    //            puntero_aplicacion.pres28 = cicloPCRCMCI_withTwoRH.pres[7];
                    //            puntero_aplicacion.pres29 = cicloPCRCMCI_withTwoRH.pres[8];
                    //            puntero_aplicacion.pres210 = cicloPCRCMCI_withTwoRH.pres[9];
                    //            puntero_aplicacion.pres211 = cicloPCRCMCI_withTwoRH.pres[10];
                    //            puntero_aplicacion.pres212 = cicloPCRCMCI_withTwoRH.pres[11];
                    //            puntero_aplicacion.pres213 = cicloPCRCMCI_withTwoRH.pres[12];
                    //            puntero_aplicacion.pres214 = cicloPCRCMCI_withTwoRH.pres[13];
                    //            puntero_aplicacion.pres215 = cicloPCRCMCI_withTwoRH.pres[14];
                    //            puntero_aplicacion.pres216 = cicloPCRCMCI_withTwoRH.pres[15];
                    //            puntero_aplicacion.pres217 = cicloPCRCMCI_withTwoRH.pres[16];
                    //            puntero_aplicacion.pres218 = cicloPCRCMCI_withTwoRH.pres[17];

                    //            puntero_aplicacion.PHX1 = cicloPCRCMCI_withTwoRH.PHX.Q_dot;
                    //            puntero_aplicacion.RHX1 = cicloPCRCMCI_withTwoRH.RHX1.Q_dot;
                    //            puntero_aplicacion.RHX2 = cicloPCRCMCI_withTwoRH.RHX2.Q_dot;

                    //            puntero_aplicacion.LT_Q = cicloPCRCMCI_withTwoRH.LT.Q_dot;
                    //            puntero_aplicacion.LT_mdotc = cicloPCRCMCI_withTwoRH.LT.m_dot_design[0];
                    //            puntero_aplicacion.LT_mdoth = cicloPCRCMCI_withTwoRH.LT.m_dot_design[1];
                    //            puntero_aplicacion.LT_Tcin = cicloPCRCMCI_withTwoRH.LT.T_c_in;
                    //            puntero_aplicacion.LT_Thin = cicloPCRCMCI_withTwoRH.LT.T_h_in;
                    //            puntero_aplicacion.LT_Pcin = cicloPCRCMCI_withTwoRH.LT.P_c_in;
                    //            puntero_aplicacion.LT_Phin = cicloPCRCMCI_withTwoRH.LT.P_h_in;
                    //            puntero_aplicacion.LT_Pcout = cicloPCRCMCI_withTwoRH.LT.P_c_out;
                    //            puntero_aplicacion.LT_Phout = cicloPCRCMCI_withTwoRH.LT.P_h_out;
                    //            puntero_aplicacion.LT_Effc = cicloPCRCMCI_withTwoRH.LT.eff;

                    //            puntero_aplicacion.HT_Q = cicloPCRCMCI_withTwoRH.HT.Q_dot;
                    //            puntero_aplicacion.HT_mdotc = cicloPCRCMCI_withTwoRH.HT.m_dot_design[0];
                    //            puntero_aplicacion.HT_mdoth = cicloPCRCMCI_withTwoRH.HT.m_dot_design[1];
                    //            puntero_aplicacion.HT_Tcin = cicloPCRCMCI_withTwoRH.HT.T_c_in;
                    //            puntero_aplicacion.HT_Thin = cicloPCRCMCI_withTwoRH.HT.T_h_in;
                    //            puntero_aplicacion.HT_Pcin = cicloPCRCMCI_withTwoRH.HT.P_c_in;
                    //            puntero_aplicacion.HT_Phin = cicloPCRCMCI_withTwoRH.HT.P_h_in;
                    //            puntero_aplicacion.HT_Pcout = cicloPCRCMCI_withTwoRH.HT.P_c_out;
                    //            puntero_aplicacion.HT_Phout = cicloPCRCMCI_withTwoRH.HT.P_h_out;
                    //            puntero_aplicacion.HT_Effc = cicloPCRCMCI_withTwoRH.HT.eff;

                    //            puntero_aplicacion.PC1 = -cicloPCRCMCI_withTwoRH.PC1.Q_dot;
                    //            puntero_aplicacion.PC2 = -cicloPCRCMCI_withTwoRH.PC2.Q_dot;
                    //            puntero_aplicacion.PC3 = -cicloPCRCMCI_withTwoRH.PC3.Q_dot;

                    //            eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                    //            recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                    //            p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                    //            p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                    //            p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out);
                    //            p_rhx1_list.Add(puntero_aplicacion.p_rhx1_in);
                    //            p_rhx2_list.Add(puntero_aplicacion.p_rhx2_in);
                    //            ua_LT_list.Add(puntero_aplicacion.ua_lt);
                    //            ua_HT_list.Add(puntero_aplicacion.ua_ht);

                    //            listBox1.Items.Add(counter.ToString());
                    //            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                    //            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                    //            listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                    //            listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                    //            listBox20.Items.Add(puntero_aplicacion.p_mc1_in.ToString());
                    //            listBox19.Items.Add(puntero_aplicacion.p_mc1_out.ToString());
                    //            listBox23.Items.Add(puntero_aplicacion.p_rhx1_in.ToString());
                    //            listBox24.Items.Add(puntero_aplicacion.p_rhx2_in.ToString());
                    //            listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                    //            listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                    //            listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                    //            listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                    //            double LTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[2];
                    //            double LTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[8] - cicloPCRCMCI_withTwoRH.temp[1];
                    //            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    //            double HTR_min_DT_1 = cicloPCRCMCI_withTwoRH.temp[7] - cicloPCRCMCI_withTwoRH.temp[3];
                    //            double HTR_min_DT_2 = cicloPCRCMCI_withTwoRH.temp[6] - cicloPCRCMCI_withTwoRH.temp[4];
                    //            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    //            //PC_in(kPa)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                    //            //PC_out(kPa)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                    //            //MC1_out(kPa)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc1_out);
                    //            //P_rxh1(kPa)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.p_rhx1_in);
                    //            //P_rxh2(kPa)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.p_rhx2_in);
                    //            //CIT
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.t_mc1_in - 273.15);
                    //            //LT UA(kW/K)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 7] = Convert.ToString(puntero_aplicacion.ua_lt);
                    //            //HT UA(kW/K)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 8] = Convert.ToString(puntero_aplicacion.ua_ht);
                    //            //Rec.Frac.
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.recomp_frac2.ToString();
                    //            //Eff.(%)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 10] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                    //            //LTR Eff.(%)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRCMCI_withTwoRH.LT.eff.ToString();
                    //            //LTR Pinch(ºC)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                    //            //HTR Eff.(%)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 13] = cicloPCRCMCI_withTwoRH.HT.eff.ToString();
                    //            //HTR Pinch(ºC)
                    //            xlWorkSheet1.Cells[counter_Excel + 1, 14] = HTR_min_DT_paper.ToString();

                    //            counter_Excel++;

                    //            return puntero_aplicacion.eta_thermal2;
                    //        };

                    //        solver.SetMaxObjective(funcion);

                    //        double? finalScore;

                    //        var result = solver.Optimize(initialValue, out finalScore);

                    //        Double max_eta_thermal = 0.0;

                    //        max_eta_thermal = eta_thermal2_list.Max();

                    //        var maxIndex = eta_thermal2_list.IndexOf(eta_thermal2_list.Max());

                    //        textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    //        textBox90.Text = recomp_frac2_list[maxIndex].ToString();
                    //        textBox91.Text = p_pc_in2_list[maxIndex].ToString();
                    //        textBox2.Text = p_pc_out2_list[maxIndex].ToString();
                    //        textBox5.Text = p_mc1_out2_list[maxIndex].ToString();
                    //        textBox6.Text = p_rhx1_list[maxIndex].ToString();
                    //        textBox7.Text = p_rhx2_list[maxIndex].ToString();
                    //        textBox82.Text = ua_LT_list[maxIndex].ToString();
                    //        textBox83.Text = ua_HT_list[maxIndex].ToString();

                    //        //Copy results as design-point inputs
                    //        if (checkBox3.Checked == true)
                    //        {
                    //            puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox103.Text = p_pc_in2_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox104.Text = p_pc_out2_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox7.Text = p_rhx1_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox113.Text = p_rhx2_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                    //            puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    //        }

                    //        //Closing Excel Book
                    //        xlWorkBook1.SaveAs(textBox3.Text + "PCRCMCI_with_Two_ReHeating" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    //        xlWorkBook1.Close(true, misValue1, misValue1);
                    //        xlApp1.Quit();

                    //        releaseObject(xlWorkSheet1);
                    //        //releaseObject(xlWorkSheet2);
                    //        releaseObject(xlWorkBook1);
                    //        releaseObject(xlApp1);
                    //    }
                    //}
                
                }
            }

            //Closing Excel Book
            xlWorkBook1.SaveAs(textBox3.Text + "PCRCMCI_with_Two_ReHeating" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }
    }
}
