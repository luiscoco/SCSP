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
    public partial class PCRCMCI_without_ReHeating_Optimization_Analysis_Results : Form
    {
        PCRCMCI_withoutReHeating puntero_aplicacion;

        public PCRCMCI_without_ReHeating_Optimization_Analysis_Results(PCRCMCI_withoutReHeating puntero1)
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

                puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);

                puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

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

                puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCMCIwithoutReheating cicloPCRCMCI_withoutRH = new core.PCRCMCIwithoutReheating();

                double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 4] = "CIT(K)";
                xlWorkSheet1.Cells[4, 5] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500, initial_CIP_value + 1000 });
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out , puntero_aplicacion.p_mc2_out });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value, initial_CIP_value + 1000.0, initial_CIP_value + 2500.0 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {

                        puntero_aplicacion.luis.RecompCycle_PCRCMCI_without_Reheating(puntero_aplicacion.luis, ref cicloPCRCMCI_withoutRH,
                        puntero_aplicacion.w_dot_net, puntero_aplicacion.t_pc_in, puntero_aplicacion.t_mc1_in, puntero_aplicacion.t_mc2_in,
                        puntero_aplicacion.t_t_in, variables[1], variables[2], variables[3],
                        puntero_aplicacion.p_mc2_out, variables[2], variables[3],
                        puntero_aplicacion.ua_lt, puntero_aplicacion.ua_ht, puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_pc,
                        puntero_aplicacion.m_eta_rc, puntero_aplicacion.m_eta_mc1, puntero_aplicacion.m_eta_t,
                        puntero_aplicacion.n_sub_hxrs, variables[0], puntero_aplicacion.tol, puntero_aplicacion.eta_thermal,
                        -puntero_aplicacion.dp_lt1, -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2,
                        -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_pc3, -puntero_aplicacion.dp_phx);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRCMCI_withoutRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRCMCI_withoutRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRCMCI_withoutRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in = variables[1];
                        puntero_aplicacion.p_pc_out = variables[2];
                        puntero_aplicacion.p_mc1_in = variables[2];
                        puntero_aplicacion.p_mc1_out = variables[3];
                        puntero_aplicacion.p_mc2_in = variables[3];

                        puntero_aplicacion.temp21 = cicloPCRCMCI_withoutRH.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRCMCI_withoutRH.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRCMCI_withoutRH.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRCMCI_withoutRH.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRCMCI_withoutRH.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRCMCI_withoutRH.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRCMCI_withoutRH.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRCMCI_withoutRH.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRCMCI_withoutRH.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRCMCI_withoutRH.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRCMCI_withoutRH.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRCMCI_withoutRH.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRCMCI_withoutRH.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRCMCI_withoutRH.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRCMCI_withoutRH.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRCMCI_withoutRH.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRCMCI_withoutRH.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRCMCI_withoutRH.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRCMCI_withoutRH.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRCMCI_withoutRH.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRCMCI_withoutRH.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRCMCI_withoutRH.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRCMCI_withoutRH.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRCMCI_withoutRH.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRCMCI_withoutRH.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRCMCI_withoutRH.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRCMCI_withoutRH.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRCMCI_withoutRH.pres[13];

                        puntero_aplicacion.PHX1 = cicloPCRCMCI_withoutRH.PHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRCMCI_withoutRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRCMCI_withoutRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRCMCI_withoutRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRCMCI_withoutRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRCMCI_withoutRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRCMCI_withoutRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRCMCI_withoutRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRCMCI_withoutRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRCMCI_withoutRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRCMCI_withoutRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRCMCI_withoutRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRCMCI_withoutRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRCMCI_withoutRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRCMCI_withoutRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRCMCI_withoutRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRCMCI_withoutRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRCMCI_withoutRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRCMCI_withoutRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRCMCI_withoutRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRCMCI_withoutRH.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRCMCI_withoutRH.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloPCRCMCI_withoutRH.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloPCRCMCI_withoutRH.PC3.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc1_in.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_mc1_out.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                        double LTR_min_DT_1 = cicloPCRCMCI_withoutRH.temp[7] - cicloPCRCMCI_withoutRH.temp[2];
                        double LTR_min_DT_2 = cicloPCRCMCI_withoutRH.temp[8] - cicloPCRCMCI_withoutRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRCMCI_withoutRH.temp[7] - cicloPCRCMCI_withoutRH.temp[3];
                        double HTR_min_DT_2 = cicloPCRCMCI_withoutRH.temp[6] - cicloPCRCMCI_withoutRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc1_out);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.t_mc1_in - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_lt);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRCMCI_withoutRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRCMCI_withoutRH.HT.eff.ToString();
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
                    textBox5.Text = p_mc1_out2_list[maxIndex].ToString();
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
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "PCRCMCI_withoutRH" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

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

                puntero_aplicacion.dp_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);

                puntero_aplicacion.dp_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp_pc3 = Convert.ToDouble(puntero_aplicacion.textBox107.Text);

                puntero_aplicacion.dp_phx = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

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

                puntero_aplicacion.n_sub_hxrs = Convert.ToInt64(puntero_aplicacion.textBox20.Text);

                puntero_aplicacion.tol = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.PCRCMCIwithoutReheating cicloPCRCMCI_withoutRH = new core.PCRCMCIwithoutReheating();

                double UA_Total = puntero_aplicacion.ua_lt + puntero_aplicacion.ua_ht;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_pc_in2_list = new List<Double>();
                List<Double> p_pc_out2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 4] = "CIT(K)";
                xlWorkSheet1.Cells[4, 5] = "LT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "Rec.Frac.";
                xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, initial_CIP_value + 500, initial_CIP_value + 1000, 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, puntero_aplicacion.p_mc2_out, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 250.0 , 0.05});

                    var initialValue = new[] { 0.2, initial_CIP_value, initial_CIP_value + 1000.0, initial_CIP_value + 2500.0, 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {

                        puntero_aplicacion.luis.RecompCycle_PCRCMCI_without_Reheating_for_optimization(puntero_aplicacion.luis, ref cicloPCRCMCI_withoutRH,
                        puntero_aplicacion.w_dot_net, puntero_aplicacion.t_pc_in, puntero_aplicacion.t_mc1_in, puntero_aplicacion.t_mc2_in,
                        puntero_aplicacion.t_t_in, variables[1], variables[2], variables[3],
                        puntero_aplicacion.p_mc2_out, variables[2], variables[3],
                        variables[4], UA_Total, puntero_aplicacion.m_eta_mc2, puntero_aplicacion.m_eta_pc,
                        puntero_aplicacion.m_eta_rc, puntero_aplicacion.m_eta_mc1, puntero_aplicacion.m_eta_t,
                        puntero_aplicacion.n_sub_hxrs, variables[0], puntero_aplicacion.tol, puntero_aplicacion.eta_thermal,
                        -puntero_aplicacion.dp_lt1, -puntero_aplicacion.dp_lt2, -puntero_aplicacion.dp_ht1, -puntero_aplicacion.dp_ht2,
                        -puntero_aplicacion.dp_pc1, -puntero_aplicacion.dp_pc2, -puntero_aplicacion.dp_pc3, -puntero_aplicacion.dp_phx);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloPCRCMCI_withoutRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloPCRCMCI_withoutRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloPCRCMCI_withoutRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_pc_in = variables[1];
                        puntero_aplicacion.p_pc_out = variables[2];
                        puntero_aplicacion.p_mc1_in = variables[2];
                        puntero_aplicacion.p_mc1_out = variables[3];
                        puntero_aplicacion.p_mc2_in = variables[3];
                        LT_fraction = variables[4];
                        puntero_aplicacion.ua_lt = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloPCRCMCI_withoutRH.temp[0];
                        puntero_aplicacion.temp22 = cicloPCRCMCI_withoutRH.temp[1];
                        puntero_aplicacion.temp23 = cicloPCRCMCI_withoutRH.temp[2];
                        puntero_aplicacion.temp24 = cicloPCRCMCI_withoutRH.temp[3];
                        puntero_aplicacion.temp25 = cicloPCRCMCI_withoutRH.temp[4];
                        puntero_aplicacion.temp26 = cicloPCRCMCI_withoutRH.temp[5];
                        puntero_aplicacion.temp27 = cicloPCRCMCI_withoutRH.temp[6];
                        puntero_aplicacion.temp28 = cicloPCRCMCI_withoutRH.temp[7];
                        puntero_aplicacion.temp29 = cicloPCRCMCI_withoutRH.temp[8];
                        puntero_aplicacion.temp210 = cicloPCRCMCI_withoutRH.temp[9];
                        puntero_aplicacion.temp211 = cicloPCRCMCI_withoutRH.temp[10];
                        puntero_aplicacion.temp212 = cicloPCRCMCI_withoutRH.temp[11];
                        puntero_aplicacion.temp213 = cicloPCRCMCI_withoutRH.temp[12];
                        puntero_aplicacion.temp214 = cicloPCRCMCI_withoutRH.temp[13];

                        puntero_aplicacion.pres21 = cicloPCRCMCI_withoutRH.pres[0];
                        puntero_aplicacion.pres22 = cicloPCRCMCI_withoutRH.pres[1];
                        puntero_aplicacion.pres23 = cicloPCRCMCI_withoutRH.pres[2];
                        puntero_aplicacion.pres24 = cicloPCRCMCI_withoutRH.pres[3];
                        puntero_aplicacion.pres25 = cicloPCRCMCI_withoutRH.pres[4];
                        puntero_aplicacion.pres26 = cicloPCRCMCI_withoutRH.pres[5];
                        puntero_aplicacion.pres27 = cicloPCRCMCI_withoutRH.pres[6];
                        puntero_aplicacion.pres28 = cicloPCRCMCI_withoutRH.pres[7];
                        puntero_aplicacion.pres29 = cicloPCRCMCI_withoutRH.pres[8];
                        puntero_aplicacion.pres210 = cicloPCRCMCI_withoutRH.pres[9];
                        puntero_aplicacion.pres211 = cicloPCRCMCI_withoutRH.pres[10];
                        puntero_aplicacion.pres212 = cicloPCRCMCI_withoutRH.pres[11];
                        puntero_aplicacion.pres213 = cicloPCRCMCI_withoutRH.pres[12];
                        puntero_aplicacion.pres214 = cicloPCRCMCI_withoutRH.pres[13];

                        puntero_aplicacion.PHX1 = cicloPCRCMCI_withoutRH.PHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloPCRCMCI_withoutRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloPCRCMCI_withoutRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloPCRCMCI_withoutRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloPCRCMCI_withoutRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloPCRCMCI_withoutRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloPCRCMCI_withoutRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloPCRCMCI_withoutRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloPCRCMCI_withoutRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloPCRCMCI_withoutRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloPCRCMCI_withoutRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloPCRCMCI_withoutRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloPCRCMCI_withoutRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloPCRCMCI_withoutRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloPCRCMCI_withoutRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloPCRCMCI_withoutRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloPCRCMCI_withoutRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloPCRCMCI_withoutRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloPCRCMCI_withoutRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloPCRCMCI_withoutRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloPCRCMCI_withoutRH.HT.eff;

                        puntero_aplicacion.PC1 = -cicloPCRCMCI_withoutRH.PC1.Q_dot;
                        puntero_aplicacion.PC2 = -cicloPCRCMCI_withoutRH.PC2.Q_dot;
                        puntero_aplicacion.PC3 = -cicloPCRCMCI_withoutRH.PC3.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_pc_in2_list.Add(puntero_aplicacion.p_pc_in);
                        p_pc_out2_list.Add(puntero_aplicacion.p_pc_out);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_pc_in.ToString());
                        listBox9.Items.Add(puntero_aplicacion.p_pc_out.ToString());
                        listBox20.Items.Add(puntero_aplicacion.p_mc1_in.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_mc1_out.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp28.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp29.ToString());

                        double LTR_min_DT_1 = cicloPCRCMCI_withoutRH.temp[7] - cicloPCRCMCI_withoutRH.temp[2];
                        double LTR_min_DT_2 = cicloPCRCMCI_withoutRH.temp[8] - cicloPCRCMCI_withoutRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloPCRCMCI_withoutRH.temp[7] - cicloPCRCMCI_withoutRH.temp[3];
                        double HTR_min_DT_2 = cicloPCRCMCI_withoutRH.temp[6] - cicloPCRCMCI_withoutRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //PC_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_pc_in);
                        //PC_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_pc_out);
                        //MC1_out(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.p_mc1_out);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.t_mc1_in - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_lt);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht);
                        //Rec.Frac.
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloPCRCMCI_withoutRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloPCRCMCI_withoutRH.HT.eff.ToString();
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
                    textBox5.Text = p_mc1_out2_list[maxIndex].ToString();
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
                        puntero_aplicacion.textBox3.Text = p_pc_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "PCRCMCI_withoutRH" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }
        }

        //Run CIT analysis
        private void button7_Click(object sender, EventArgs e)
        {


        }
    }
}
