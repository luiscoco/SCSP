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
    public partial class RCMCI_without_ReHeating_Optimization_new_configuration : Form
    {
        RCMCI_without_ReHeating_new_proposed_configuration puntero_aplicacion;

        public RCMCI_without_ReHeating_Optimization_new_configuration(RCMCI_without_ReHeating_new_proposed_configuration puntero1)
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


        //Run optimization button
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," +
                                                  puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                                                  puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," +
                                                  puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc1_in2;
                puntero_aplicacion.t_rht_in2 = puntero_aplicacion.t_t_in2;

                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp2_phx1 = 0.0;
                puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RCMCIwithReheating cicloRCMCI_withRH = new core.RCMCIwithReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_rhx_in2_list = new List<Double>();
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

                using (var solver = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH.temp[8] - cicloRCMCI_withRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH.temp[6] - cicloRCMCI_withRH.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

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

                //SEGUNDALLAMADA para la optimización
                double max_recomp_fraction_1 = 0.0;
                double max_mc1_p_in_1 = 0.0;
                double temp5_max_eff_1 = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.RCMCIwithReheating cicloRCMCI_withRH_Segunda_llamada = new core.RCMCIwithReheating();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver1 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                {
                    solver1.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                    solver1.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                    solver1.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Segunda_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Segunda_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH_Segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH_Segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH_Segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH_Segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH_Segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH_Segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH_Segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH_Segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH_Segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH_Segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH_Segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH_Segunda_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH_Segunda_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH_Segunda_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH_Segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH_Segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH_Segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH_Segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH_Segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH_Segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH_Segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH_Segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH_Segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH_Segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH_Segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH_Segunda_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH_Segunda_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH_Segunda_llamada.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Segunda_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Segunda_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Segunda_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Segunda_llamada.COOLER.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[8] - cicloRCMCI_withRH_Segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[6] - cicloRCMCI_withRH_Segunda_llamada.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Segunda_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Segunda_llamada.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
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

                //TERCERA LLAMADA para la optimización
                double max_recomp_fraction_2 = 0.0;
                double max_mc1_p_in_2 = 0.0;
                double temp5_max_eff_2 = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.RCMCIwithReheating cicloRCMCI_withRH_Tercera_llamada = new core.RCMCIwithReheating();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver2 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                {
                    solver2.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                    solver2.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                    solver2.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH_Tercera_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff_1, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Tercera_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Tercera_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH_Tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH_Tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH_Tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH_Tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH_Tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH_Tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH_Tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH_Tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH_Tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH_Tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH_Tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH_Tercera_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH_Tercera_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH_Tercera_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH_Tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH_Tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH_Tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH_Tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH_Tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH_Tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH_Tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH_Tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH_Tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH_Tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH_Tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH_Tercera_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH_Tercera_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH_Tercera_llamada.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Tercera_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Tercera_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Tercera_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Tercera_llamada.COOLER.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[8] - cicloRCMCI_withRH_Tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[6] - cicloRCMCI_withRH_Tercera_llamada.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Tercera_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Tercera_llamada.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
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

                //CUARTA LLAMADA para la optimización
                double max_recomp_fraction_3 = 0.0;
                double max_mc1_p_in_3 = 0.0;
                double temp5_max_eff_3 = 0.0;

                List<Double> temp5_list_cuarta = new List<Double>();

                core.RCMCIwithReheating cicloRCMCI_withRH_Cuarta_llamada = new core.RCMCIwithReheating();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver3 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                {
                    solver3.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                    solver3.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                    solver3.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH_Cuarta_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff_2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Cuarta_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Cuarta_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH_Cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH_Cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH_Cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH_Cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH_Cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH_Cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH_Cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH_Cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH_Cuarta_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH_Cuarta_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH_Cuarta_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH_Cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH_Cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH_Cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH_Cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH_Cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH_Cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH_Cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH_Cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH_Cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH_Cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH_Cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH_Cuarta_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH_Cuarta_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH_Cuarta_llamada.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Cuarta_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Cuarta_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Cuarta_llamada.COOLER.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[8] - cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[6] - cicloRCMCI_withRH_Cuarta_llamada.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Cuarta_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Cuarta_llamada.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkBook1);
                    //releaseObject(xlApp1);
                } //Fin de la CUARTA LLAMADA para optimización

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

                //QUINTA LLAMADA para la optimización
                double max_recomp_fraction_4 = 0.0;
                double max_mc1_p_in_4 = 0.0;
                double temp5_max_eff_4 = 0.0;

                List<Double> temp5_list_quinta = new List<Double>();

                core.RCMCIwithReheating cicloRCMCI_withRH_Quinta_llamada = new core.RCMCIwithReheating();

                List<Double> recomp_frac2_list_quinta_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_quinta_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_quinta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_quinta_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_quinta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver4 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                {
                    solver4.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                    solver4.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                    solver4.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH_Quinta_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff_3, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Quinta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Quinta_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Quinta_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH_Quinta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH_Quinta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH_Quinta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH_Quinta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH_Quinta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH_Quinta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH_Quinta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH_Quinta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH_Quinta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH_Quinta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH_Quinta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH_Quinta_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH_Quinta_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH_Quinta_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH_Quinta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH_Quinta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH_Quinta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH_Quinta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH_Quinta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH_Quinta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH_Quinta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH_Quinta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH_Quinta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH_Quinta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH_Quinta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH_Quinta_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH_Quinta_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH_Quinta_llamada.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Quinta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Quinta_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Quinta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Quinta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Quinta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Quinta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Quinta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Quinta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Quinta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Quinta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Quinta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Quinta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Quinta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Quinta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Quinta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Quinta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Quinta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Quinta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Quinta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Quinta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Quinta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Quinta_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Quinta_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Quinta_llamada.COOLER.Q_dot;

                        eta_thermal2_list_quinta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_quinta_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_quinta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_quinta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list_quinta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_quinta.Add(puntero_aplicacion.temp25);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH_Quinta_llamada.temp[7] - cicloRCMCI_withRH_Quinta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH_Quinta_llamada.temp[8] - cicloRCMCI_withRH_Quinta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH_Quinta_llamada.temp[7] - cicloRCMCI_withRH_Quinta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH_Quinta_llamada.temp[6] - cicloRCMCI_withRH_Quinta_llamada.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Quinta_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Quinta_llamada.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                        counter_Excel++;

                        return puntero_aplicacion.eta_thermal2;
                    };

                    solver4.SetMaxObjective(funcion);

                    double? finalScore;

                    var result = solver4.Optimize(initialValue, out finalScore);

                    Double max_eta_thermal = 0.0;

                    max_eta_thermal = eta_thermal2_list_quinta_llamada.Max();

                    var maxIndex = eta_thermal2_list_quinta_llamada.IndexOf(eta_thermal2_list_quinta_llamada.Max());

                    textBox86.Text = eta_thermal2_list_quinta_llamada[maxIndex].ToString();
                    textBox90.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                    textBox91.Text = p_mc1_in2_list_quinta_llamada[maxIndex].ToString();
                    textBox2.Text = p_mc1_out2_list_quinta_llamada[maxIndex].ToString();
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

                    max_recomp_fraction_4 = recomp_frac2_list_quinta_llamada[maxIndex];
                    max_mc1_p_in_4 = p_mc1_in2_list_quinta_llamada[maxIndex];
                    temp5_max_eff_4 = temp5_list_quinta[maxIndex];

                    //Copy results as design-point inputs
                    if (checkBox3.Checked == true)
                    {
                        puntero_aplicacion.textBox15.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc1_in2_list_quinta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox8.Text = p_mc1_out2_list_quinta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox23.Text = p_mc1_out2_list_quinta_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                } //Fin de la QUINTA LLAMADA para optimización

            } //Fin del if the UA optimization           

            //-------------------------------------------------------------------------
            //UA optimization True
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," +
                                                  puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                                                  puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," +
                                                  puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc1_in2;
                puntero_aplicacion.t_rht_in2 = puntero_aplicacion.t_t_in2;

                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                puntero_aplicacion.dp2_phx1 = 0.0;
                puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RCMCIwithReheating cicloRCMCI_withRH = new core.RCMCIwithReheating();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc1_in2_list = new List<Double>();
                List<Double> p_mc1_out2_list = new List<Double>();
                List<Double> p_rhx_in2_list = new List<Double>();
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

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        LT_fraction = variables[3];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH.COOLER.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_primera.Add(puntero_aplicacion.temp25);
                        ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH.temp[8] - cicloRCMCI_withRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH.temp[6] - cicloRCMCI_withRH.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
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
                        puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

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

                //SEGUNDA LLAMADA para la optimización
                double max_recomp_fraction_1 = 0.0;
                double max_mc1_p_in_1 = 0.0;
                double temp5_max_eff_1 = 0.0;

                List<Double> temp5_list_segunda = new List<Double>();

                core.RCMCIwithReheating cicloRCMCI_withRH_Segunda_llamada = new core.RCMCIwithReheating();

                List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_segunda_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_segunda_llamada = new List<Double>();
                List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();
                List<Double> ua_LT_list_segunda_llamada = new List<Double>();
                List<Double> ua_HT_list_segunda_llamada = new List<Double>();
                

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver1 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver1.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                    solver1.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                    solver1.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Segunda_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Segunda_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Segunda_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        LT_fraction = variables[3];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH_Segunda_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH_Segunda_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH_Segunda_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH_Segunda_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH_Segunda_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH_Segunda_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH_Segunda_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH_Segunda_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH_Segunda_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH_Segunda_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH_Segunda_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH_Segunda_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH_Segunda_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH_Segunda_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH_Segunda_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH_Segunda_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH_Segunda_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH_Segunda_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH_Segunda_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH_Segunda_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH_Segunda_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH_Segunda_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH_Segunda_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH_Segunda_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH_Segunda_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH_Segunda_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH_Segunda_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH_Segunda_llamada.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Segunda_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Segunda_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Segunda_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Segunda_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Segunda_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Segunda_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Segunda_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Segunda_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Segunda_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Segunda_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Segunda_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Segunda_llamada.COOLER.Q_dot;

                        eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_segunda.Add(puntero_aplicacion.temp25);
                        ua_LT_list_segunda_llamada.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list_segunda_llamada.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[8] - cicloRCMCI_withRH_Segunda_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[6] - cicloRCMCI_withRH_Segunda_llamada.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Segunda_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Segunda_llamada.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                    textBox83.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();

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
                        puntero_aplicacion.textBox17.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
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

                //TERCERA LLAMADA para la optimización
                double max_recomp_fraction_2 = 0.0;
                double max_mc1_p_in_2 = 0.0;
                double temp5_max_eff_2 = 0.0;

                List<Double> temp5_list_tercera = new List<Double>();

                core.RCMCIwithReheating cicloRCMCI_withRH_Tercera_llamada = new core.RCMCIwithReheating();

                List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_tercera_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_tercera_llamada = new List<Double>();
                List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();
                List<Double> ua_LT_list_tercera_llamada = new List<Double>();
                List<Double> ua_HT_list_tercera_llamada = new List<Double>();


                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver2 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver2.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                    solver2.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                    solver2.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH_Tercera_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff_1, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Tercera_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Tercera_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Tercera_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        LT_fraction = variables[3];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH_Tercera_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH_Tercera_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH_Tercera_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH_Tercera_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH_Tercera_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH_Tercera_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH_Tercera_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH_Tercera_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH_Tercera_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH_Tercera_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH_Tercera_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH_Tercera_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH_Tercera_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH_Tercera_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH_Tercera_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH_Tercera_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH_Tercera_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH_Tercera_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH_Tercera_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH_Tercera_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH_Tercera_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH_Tercera_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH_Tercera_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH_Tercera_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH_Tercera_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH_Tercera_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH_Tercera_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH_Tercera_llamada.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Tercera_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Tercera_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Tercera_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Tercera_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Tercera_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Tercera_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Tercera_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Tercera_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Tercera_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Tercera_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Tercera_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Tercera_llamada.COOLER.Q_dot;

                        eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_tercera.Add(puntero_aplicacion.temp25);
                        ua_LT_list_tercera_llamada.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list_tercera_llamada.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[8] - cicloRCMCI_withRH_Tercera_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[6] - cicloRCMCI_withRH_Tercera_llamada.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Tercera_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Tercera_llamada.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                    textBox83.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();

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
                        puntero_aplicacion.textBox17.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    //xlApp1.Quit();

                    //releaseObject(xlWorkSheet1);
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

                //CUARTA LLAMADA para la optimización
                double max_recomp_fraction_3 = 0.0;
                double max_mc1_p_in_3 = 0.0;
                double temp5_max_eff_3 = 0.0;

                List<Double> temp5_list_cuarta = new List<Double>();

                core.RCMCIwithReheating cicloRCMCI_withRH_Cuarta_llamada = new core.RCMCIwithReheating();

                List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_in2_list_cuarta_llamada = new List<Double>();
                List<Double> p_mc1_out2_list_cuarta_llamada = new List<Double>();
                List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();
                List<Double> ua_LT_list_cuarta_llamada = new List<Double>();
                List<Double> ua_HT_list_cuarta_llamada = new List<Double>();

                xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                xlWorkSheet1.Activate();

                using (var solver3 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                {
                    solver3.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                    solver3.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                    solver3.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                    var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0), 0.5 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                        ref cicloRCMCI_withRH_Cuarta_llamada, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc2_in2,
                        temp5_max_eff_2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                        puntero_aplicacion.p_mc2_out2, variables[1], puntero_aplicacion.t_mc1_in2, variables[2],
                        variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                        variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                        -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                        -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Cuarta_llamada.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Cuarta_llamada.W_dot_net;

                        puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Cuarta_llamada.eta_thermal;
                        puntero_aplicacion.recomp_frac2 = variables[0];
                        puntero_aplicacion.p_mc1_in2 = variables[1];
                        puntero_aplicacion.p_rhx_in2 = variables[1];
                        puntero_aplicacion.p_mc1_out2 = variables[2];
                        LT_fraction = variables[3];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                        puntero_aplicacion.temp21 = cicloRCMCI_withRH_Cuarta_llamada.temp[0];
                        puntero_aplicacion.temp22 = cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                        puntero_aplicacion.temp23 = cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                        puntero_aplicacion.temp24 = cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                        puntero_aplicacion.temp25 = cicloRCMCI_withRH_Cuarta_llamada.temp[4];
                        puntero_aplicacion.temp26 = cicloRCMCI_withRH_Cuarta_llamada.temp[5];
                        puntero_aplicacion.temp27 = cicloRCMCI_withRH_Cuarta_llamada.temp[6];
                        puntero_aplicacion.temp28 = cicloRCMCI_withRH_Cuarta_llamada.temp[7];
                        puntero_aplicacion.temp29 = cicloRCMCI_withRH_Cuarta_llamada.temp[8];
                        puntero_aplicacion.temp210 = cicloRCMCI_withRH_Cuarta_llamada.temp[9];
                        puntero_aplicacion.temp211 = cicloRCMCI_withRH_Cuarta_llamada.temp[10];
                        puntero_aplicacion.temp212 = cicloRCMCI_withRH_Cuarta_llamada.temp[11];
                        puntero_aplicacion.temp213 = cicloRCMCI_withRH_Cuarta_llamada.temp[12];
                        puntero_aplicacion.temp214 = cicloRCMCI_withRH_Cuarta_llamada.temp[13];

                        puntero_aplicacion.pres21 = cicloRCMCI_withRH_Cuarta_llamada.pres[0];
                        puntero_aplicacion.pres22 = cicloRCMCI_withRH_Cuarta_llamada.pres[1];
                        puntero_aplicacion.pres23 = cicloRCMCI_withRH_Cuarta_llamada.pres[2];
                        puntero_aplicacion.pres24 = cicloRCMCI_withRH_Cuarta_llamada.pres[3];
                        puntero_aplicacion.pres25 = cicloRCMCI_withRH_Cuarta_llamada.pres[4];
                        puntero_aplicacion.pres26 = cicloRCMCI_withRH_Cuarta_llamada.pres[5];
                        puntero_aplicacion.pres27 = cicloRCMCI_withRH_Cuarta_llamada.pres[6];
                        puntero_aplicacion.pres28 = cicloRCMCI_withRH_Cuarta_llamada.pres[7];
                        puntero_aplicacion.pres29 = cicloRCMCI_withRH_Cuarta_llamada.pres[8];
                        puntero_aplicacion.pres210 = cicloRCMCI_withRH_Cuarta_llamada.pres[9];
                        puntero_aplicacion.pres211 = cicloRCMCI_withRH_Cuarta_llamada.pres[10];
                        puntero_aplicacion.pres212 = cicloRCMCI_withRH_Cuarta_llamada.pres[11];
                        puntero_aplicacion.pres213 = cicloRCMCI_withRH_Cuarta_llamada.pres[12];
                        puntero_aplicacion.pres214 = cicloRCMCI_withRH_Cuarta_llamada.pres[13];

                        puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Cuarta_llamada.PHX.Q_dot;
                        puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Cuarta_llamada.RHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Cuarta_llamada.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Cuarta_llamada.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Cuarta_llamada.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Cuarta_llamada.HT.eff;

                        puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Cuarta_llamada.PC.Q_dot;
                        puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Cuarta_llamada.COOLER.Q_dot;

                        eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                        p_mc1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                        p_mc1_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                        p_rhx_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                        temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                        ua_LT_list_cuarta_llamada.Add(puntero_aplicacion.ua_lt2);
                        ua_HT_list_cuarta_llamada.Add(puntero_aplicacion.ua_ht2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                        double LTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                        double LTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[8] - cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                        double HTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[6] - cicloRCMCI_withRH_Cuarta_llamada.temp[4];
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
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Cuarta_llamada.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Cuarta_llamada.HT.eff.ToString();
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
                    //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
                    textBox82.Text = ua_LT_list_cuarta_llamada[maxIndex].ToString();
                    textBox83.Text = ua_HT_list_cuarta_llamada[maxIndex].ToString();

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
                        puntero_aplicacion.textBox17.Text = ua_LT_list_cuarta_llamada[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_HT_list_cuarta_llamada[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                } //Fin de la CUARTA LLAMADA para optimización
            }
        }

        //Run CIT  Optimization
        private void button7_Click(object sender, EventArgs e)
        {
            int counter = 0;

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

            for (double j = Convert.ToDouble(textBox8.Text); j <= Convert.ToDouble(textBox7.Text); j = j + Convert.ToDouble(textBox6.Text))
            {
                puntero_aplicacion.ua_lt2 = j / 2;
                puntero_aplicacion.ua_ht2 = j / 2;

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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," +
                                                          puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                                                          puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," +
                                                          puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
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
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                        puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc1_in2;
                        puntero_aplicacion.t_rht_in2 = puntero_aplicacion.t_t_in2;

                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                        puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp2_phx1 = 0.0;
                        puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RCMCIwithReheating cicloRCMCI_withRH = new core.RCMCIwithReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;
                       
                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc1_in2_list = new List<Double>();
                        List<Double> p_mc1_out2_list = new List<Double>();
                        List<Double> p_rhx_in2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();                        
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
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();

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

                            xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                            xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
                            xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 7] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 8] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 9] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 10] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 11] = "HTR Pinch(ºC)";

                            if (checkBox7.Checked == false)
                            {
                                xlWorkSheet1.Cells[4, 12] = "PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 13] = "PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 14] = "LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 15] = "LF_Pressure_Drop(bar)";
                            }
                        }

                        //PRIMERA LLAMADA para la optimización
                        double max_recomp_fraction = 0.0;
                        double max_mc1_p_in = 0.0;
                        double temp5_max_eff = 0.0;

                        List<Double> temp5_list_primera = new List<Double>();

                        using (var solver = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH, puntero_aplicacion.w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_primera.Add(puntero_aplicacion.temp25);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH.temp[8] - cicloRCMCI_withRH.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH.temp[6] - cicloRCMCI_withRH.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                            }

                            if (i == Convert.ToDouble(textBox57.Text) && (j == Convert.ToDouble(textBox8.Text)))
                            {
                                //Closing Excel Book
                                xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

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

                        //SEGUNDALLAMADA para la optimización
                        double max_recomp_fraction_1 = 0.0;
                        double max_mc1_p_in_1 = 0.0;
                        double temp5_max_eff_1 = 0.0;

                        List<Double> temp5_list_segunda = new List<Double>();

                        core.RCMCIwithReheating cicloRCMCI_withRH_Segunda_llamada = new core.RCMCIwithReheating();

                        List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                        List<Double> p_mc1_in2_list_segunda_llamada = new List<Double>();
                        List<Double> p_mc1_out2_list_segunda_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                        List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver1 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                        {
                            solver1.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                            solver1.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                            solver1.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, i,
                                temp5_max_eff, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Segunda_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Segunda_llamada.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Segunda_llamada.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH_Segunda_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH_Segunda_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH_Segunda_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH_Segunda_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH_Segunda_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH_Segunda_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH_Segunda_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH_Segunda_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH_Segunda_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH_Segunda_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH_Segunda_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH_Segunda_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH_Segunda_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH_Segunda_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH_Segunda_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH_Segunda_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH_Segunda_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH_Segunda_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH_Segunda_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH_Segunda_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH_Segunda_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH_Segunda_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH_Segunda_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH_Segunda_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH_Segunda_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH_Segunda_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH_Segunda_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH_Segunda_llamada.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Segunda_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Segunda_llamada.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Segunda_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Segunda_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Segunda_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Segunda_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Segunda_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Segunda_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Segunda_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Segunda_llamada.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Segunda_llamada.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Segunda_llamada.COOLER.Q_dot;

                                eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_segunda.Add(puntero_aplicacion.temp25);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[8] - cicloRCMCI_withRH_Segunda_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[6] - cicloRCMCI_withRH_Segunda_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Segunda_llamada.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Segunda_llamada.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
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

                        //TERCERA LLAMADA para la optimización
                        double max_recomp_fraction_2 = 0.0;
                        double max_mc1_p_in_2 = 0.0;
                        double temp5_max_eff_2 = 0.0;

                        List<Double> temp5_list_tercera = new List<Double>();

                        core.RCMCIwithReheating cicloRCMCI_withRH_Tercera_llamada = new core.RCMCIwithReheating();

                        List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                        List<Double> p_mc1_in2_list_tercera_llamada = new List<Double>();
                        List<Double> p_mc1_out2_list_tercera_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                        List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver2 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                        {
                            solver2.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                            solver2.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                            solver2.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH_Tercera_llamada, puntero_aplicacion.w_dot_net2, i,
                                temp5_max_eff_1, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Tercera_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Tercera_llamada.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Tercera_llamada.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH_Tercera_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH_Tercera_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH_Tercera_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH_Tercera_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH_Tercera_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH_Tercera_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH_Tercera_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH_Tercera_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH_Tercera_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH_Tercera_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH_Tercera_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH_Tercera_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH_Tercera_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH_Tercera_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH_Tercera_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH_Tercera_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH_Tercera_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH_Tercera_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH_Tercera_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH_Tercera_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH_Tercera_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH_Tercera_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH_Tercera_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH_Tercera_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH_Tercera_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH_Tercera_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH_Tercera_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH_Tercera_llamada.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Tercera_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Tercera_llamada.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Tercera_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Tercera_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Tercera_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Tercera_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Tercera_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Tercera_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Tercera_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Tercera_llamada.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Tercera_llamada.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Tercera_llamada.COOLER.Q_dot;

                                eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_tercera.Add(puntero_aplicacion.temp25);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[8] - cicloRCMCI_withRH_Tercera_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[6] - cicloRCMCI_withRH_Tercera_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Tercera_llamada.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Tercera_llamada.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
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

                        //CUARTA LLAMADA para la optimización
                        double max_recomp_fraction_3 = 0.0;
                        double max_mc1_p_in_3 = 0.0;
                        double temp5_max_eff_3 = 0.0;

                        List<Double> temp5_list_cuarta = new List<Double>();

                        core.RCMCIwithReheating cicloRCMCI_withRH_Cuarta_llamada = new core.RCMCIwithReheating();

                        List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_mc1_in2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_mc1_out2_list_cuarta_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver3 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                        {
                            solver3.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                            solver3.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                            solver3.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH_Cuarta_llamada, puntero_aplicacion.w_dot_net2, i,
                                temp5_max_eff_2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Cuarta_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Cuarta_llamada.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Cuarta_llamada.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH_Cuarta_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH_Cuarta_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH_Cuarta_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH_Cuarta_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH_Cuarta_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH_Cuarta_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH_Cuarta_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH_Cuarta_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH_Cuarta_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH_Cuarta_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH_Cuarta_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH_Cuarta_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH_Cuarta_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH_Cuarta_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH_Cuarta_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH_Cuarta_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH_Cuarta_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH_Cuarta_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH_Cuarta_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH_Cuarta_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH_Cuarta_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH_Cuarta_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH_Cuarta_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH_Cuarta_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH_Cuarta_llamada.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Cuarta_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Cuarta_llamada.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Cuarta_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Cuarta_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Cuarta_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Cuarta_llamada.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Cuarta_llamada.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Cuarta_llamada.COOLER.Q_dot;

                                eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_cuarta.Add(puntero_aplicacion.temp25);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[8] - cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[6] - cicloRCMCI_withRH_Cuarta_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Cuarta_llamada.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Cuarta_llamada.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

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
                            }

                        } //Fin de la CUARTA LLAMADA para optimización

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

                        //QUINTA LLAMADA para la optimización
                        double max_recomp_fraction_4 = 0.0;
                        double max_mc1_p_in_4 = 0.0;
                        double temp5_max_eff_4 = 0.0;

                        List<Double> temp5_list_quinta = new List<Double>();
                        List<Double> massflow2_list = new List<Double>();
                        List<Double> PHX_Q2_list = new List<Double>();

                        core.RCMCIwithReheating cicloRCMCI_withRH_Quinta_llamada = new core.RCMCIwithReheating();

                        List<Double> recomp_frac2_list_quinta_llamada = new List<Double>();
                        List<Double> p_mc1_in2_list_quinta_llamada = new List<Double>();
                        List<Double> p_mc1_out2_list_quinta_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_quinta_llamada = new List<Double>();
                        List<Double> p_rhx_in2_list_quinta_llamada = new List<Double>();

                        xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                        xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        xlWorkSheet1.Activate();

                        using (var solver4 = new NLoptSolver(algorithm_type, 3, 0.01, 10000))
                        {
                            solver4.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0) });
                            solver4.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                            solver4.SetInitialStepSize(new[] { 0.05, 250.0, 250.0 });

                            var initialValue = new[] { 0.25, initial_CIP_value + 3500.0, (initial_CIP_value + 6500.0) };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH_Quinta_llamada, puntero_aplicacion.w_dot_net2, i,
                                temp5_max_eff_3, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_ht2, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Quinta_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Quinta_llamada.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Quinta_llamada.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH_Quinta_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH_Quinta_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH_Quinta_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH_Quinta_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH_Quinta_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH_Quinta_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH_Quinta_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH_Quinta_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH_Quinta_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH_Quinta_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH_Quinta_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH_Quinta_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH_Quinta_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH_Quinta_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH_Quinta_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH_Quinta_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH_Quinta_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH_Quinta_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH_Quinta_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH_Quinta_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH_Quinta_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH_Quinta_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH_Quinta_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH_Quinta_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH_Quinta_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH_Quinta_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH_Quinta_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH_Quinta_llamada.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Quinta_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Quinta_llamada.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Quinta_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Quinta_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Quinta_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Quinta_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Quinta_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Quinta_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Quinta_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Quinta_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Quinta_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Quinta_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Quinta_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Quinta_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Quinta_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Quinta_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Quinta_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Quinta_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Quinta_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Quinta_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Quinta_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Quinta_llamada.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Quinta_llamada.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Quinta_llamada.COOLER.Q_dot;

                                massflow2_list.Add(puntero_aplicacion.massflow2);
                                eta_thermal2_list_quinta_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_quinta_llamada.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list_quinta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list_quinta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list_quinta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_quinta.Add(puntero_aplicacion.temp25);
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
                                t11_list.Add(puntero_aplicacion.temp211);
                                t12_list.Add(puntero_aplicacion.temp212);
                                t13_list.Add(puntero_aplicacion.temp213);
                                t14_list.Add(puntero_aplicacion.temp214);

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

                                PHX_Q2_list.Add(cicloRCMCI_withRH_Cuarta_llamada.RHX.Q_dot);

                                HT_Eff_list.Add(cicloRCMCI_withRH_Cuarta_llamada.HT.eff);
                                LT_Eff_list.Add(cicloRCMCI_withRH_Cuarta_llamada.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH_Quinta_llamada.temp[7] - cicloRCMCI_withRH_Quinta_llamada.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH_Quinta_llamada.temp[8] - cicloRCMCI_withRH_Quinta_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH_Quinta_llamada.temp[7] - cicloRCMCI_withRH_Quinta_llamada.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH_Quinta_llamada.temp[6] - cicloRCMCI_withRH_Quinta_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Quinta_llamada.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Quinta_llamada.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

                            return puntero_aplicacion.eta_thermal2;
                            };

                            solver4.SetMaxObjective(funcion);

                            double? finalScore;

                            var result = solver4.Optimize(initialValue, out finalScore);

                            Double max_eta_thermal = 0.0;

                            max_eta_thermal = eta_thermal2_list_quinta_llamada.Max();

                            var maxIndex = eta_thermal2_list_quinta_llamada.IndexOf(eta_thermal2_list_quinta_llamada.Max());

                            textBox86.Text = eta_thermal2_list_quinta_llamada[maxIndex].ToString();
                            textBox90.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                            textBox91.Text = p_mc1_in2_list_quinta_llamada[maxIndex].ToString();
                            textBox2.Text = p_mc1_out2_list_quinta_llamada[maxIndex].ToString();
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();

                            max_recomp_fraction_4 = recomp_frac2_list_quinta_llamada[maxIndex];
                            max_mc1_p_in_4 = p_mc1_in2_list_quinta_llamada[maxIndex];
                            temp5_max_eff_4 = temp5_list_quinta[maxIndex];

                            //Copy results as design-point inputs
                            if (checkBox3.Checked == true)
                            {
                                puntero_aplicacion.textBox15.Text = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox3.Text = p_mc1_in2_list_quinta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox8.Text = p_mc1_out2_list_quinta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox23.Text = p_mc1_out2_list_quinta_llamada[maxIndex].ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list_quinta_llamada[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list_quinta_llamada[maxIndex].ToString());
                            listBox15.Items.Add(p_mc1_in2_list_quinta_llamada[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx_in2_list_quinta_llamada[maxIndex].ToString());
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
                                PTC.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                PTC.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                PTC.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                PTC.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
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
                                LF.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                LF.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                LF.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                LF.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
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
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_mc1_in2_list_quinta_llamada[maxIndex]);
                            //Main compressor output pressure (kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(p_mc1_out2_list_quinta_llamada[maxIndex]);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_lt2.ToString();
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.ua_ht2.ToString();
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = recomp_frac2_list_quinta_llamada[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = (eta_thermal2_list_quinta_llamada[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = LT_Eff_list[maxIndex].ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper_max.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = HT_Eff_list[maxIndex].ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper_max.ToString();

                            if (checkBox7.Checked == false)
                            {
                                //PTC_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 12] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                                //PTC_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                                //LF_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                                //LF_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            }

                            counter_Excel++;

                            initial_CIP_value = puntero_aplicacion.p_mc1_in2;

                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);
                        } //Fin de la QUINTA LLAMADA para optimización

                    } //Fin del if the UA optimization           

                    //-------------------------------------------------------------------------
                    //UA optimization True
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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox68.Text + "," +
                                                          puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox69.Text + "," +
                                                          puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox33.Text + "," +
                                                          puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox34.Text, puntero_aplicacion.category);
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
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc1_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc1_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        puntero_aplicacion.p_mc2_in2 = Convert.ToDouble(puntero_aplicacion.textBox23.Text);
                        puntero_aplicacion.p_mc2_out2 = Convert.ToDouble(puntero_aplicacion.textBox22.Text);

                        puntero_aplicacion.p_rhx_in2 = puntero_aplicacion.p_mc1_in2;
                        puntero_aplicacion.t_rht_in2 = puntero_aplicacion.t_t_in2;

                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);
                        puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox15.Text);
                        puntero_aplicacion.eta1_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta2_mc2 = Convert.ToDouble(puntero_aplicacion.textBox27.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.eta_trh2 = puntero_aplicacion.eta_t2;
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);
                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp11_pc1 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp11_pc2 = Convert.ToDouble(puntero_aplicacion.textBox24.Text);
                        puntero_aplicacion.dp2_phx1 = 0.0;
                        puntero_aplicacion.dp2_rhx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RCMCIwithReheating cicloRCMCI_withRH = new core.RCMCIwithReheating();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;                       

                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc1_in2_list = new List<Double>();
                        List<Double> p_mc1_out2_list = new List<Double>();
                        List<Double> p_rhx_in2_list = new List<Double>();
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
                        List<Double> t11_list = new List<Double>();
                        List<Double> t12_list = new List<Double>();
                        List<Double> t13_list = new List<Double>();
                        List<Double> t14_list = new List<Double>();

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

                            xlWorkSheet1.Cells[4, 1] = "MC1_in(kPa)";
                            xlWorkSheet1.Cells[4, 2] = "MC1_out(kPa)";
                            xlWorkSheet1.Cells[4, 3] = "CIT(K)";
                            xlWorkSheet1.Cells[4, 4] = "LT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                            xlWorkSheet1.Cells[4, 6] = "Rec.Frac.";
                            xlWorkSheet1.Cells[4, 7] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 8] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 9] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 10] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 11] = "HTR Pinch(ºC)";

                            if (checkBox7.Checked == false)
                            {
                                xlWorkSheet1.Cells[4, 12] = "PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 13] = "PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 14] = "LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 15] = "LF_Pressure_Drop(bar)";
                            }
                        }
                        //PRIMERA LLAMADA para la optimización
                        double max_recomp_fraction = 0.0;
                        double max_mc1_p_in = 0.0;
                        double temp5_max_eff = 0.0;

                        List<Double> temp5_list_primera = new List<Double>();

                        using (var solver = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                        {
                            solver.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                            solver.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                            solver.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value + 3500.0), (initial_CIP_value + 6500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH, puntero_aplicacion.w_dot_net2, i,
                                puntero_aplicacion.t_t_in2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                LT_fraction = variables[3];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH.COOLER.Q_dot;

                                eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_primera.Add(puntero_aplicacion.temp25);
                                ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH.temp[8] - cicloRCMCI_withRH.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH.temp[7] - cicloRCMCI_withRH.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH.temp[6] - cicloRCMCI_withRH.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
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
                                puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                            }
                            if (i == Convert.ToDouble(textBox57.Text) && (j == Convert.ToDouble(textBox8.Text)))
                            {
                                //Closing Excel Book
                                xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

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

                        //SEGUNDA LLAMADA para la optimización
                        double max_recomp_fraction_1 = 0.0;
                        double max_mc1_p_in_1 = 0.0;
                        double temp5_max_eff_1 = 0.0;

                        List<Double> temp5_list_segunda = new List<Double>();

                        core.RCMCIwithReheating cicloRCMCI_withRH_Segunda_llamada = new core.RCMCIwithReheating();

                        List<Double> recomp_frac2_list_segunda_llamada = new List<Double>();
                        List<Double> p_mc1_in2_list_segunda_llamada = new List<Double>();
                        List<Double> p_mc1_out2_list_segunda_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_segunda_llamada = new List<Double>();
                        List<Double> p_rhx_in2_list_segunda_llamada = new List<Double>();
                        List<Double> ua_LT_list_segunda_llamada = new List<Double>();
                        List<Double> ua_HT_list_segunda_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver1 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                        {
                            solver1.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                            solver1.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                            solver1.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value + 3500.0), (initial_CIP_value + 6500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH_Segunda_llamada, puntero_aplicacion.w_dot_net2, i,
                                temp5_max_eff, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Segunda_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Segunda_llamada.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Segunda_llamada.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                LT_fraction = variables[3];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH_Segunda_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH_Segunda_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH_Segunda_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH_Segunda_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH_Segunda_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH_Segunda_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH_Segunda_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH_Segunda_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH_Segunda_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH_Segunda_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH_Segunda_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH_Segunda_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH_Segunda_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH_Segunda_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH_Segunda_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH_Segunda_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH_Segunda_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH_Segunda_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH_Segunda_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH_Segunda_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH_Segunda_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH_Segunda_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH_Segunda_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH_Segunda_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH_Segunda_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH_Segunda_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH_Segunda_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH_Segunda_llamada.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Segunda_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Segunda_llamada.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Segunda_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Segunda_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Segunda_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Segunda_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Segunda_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Segunda_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Segunda_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Segunda_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Segunda_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Segunda_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Segunda_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Segunda_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Segunda_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Segunda_llamada.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Segunda_llamada.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Segunda_llamada.COOLER.Q_dot;

                                eta_thermal2_list_segunda_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_segunda_llamada.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list_segunda_llamada.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list_segunda_llamada.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_segunda.Add(puntero_aplicacion.temp25);
                                ua_LT_list_segunda_llamada.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list_segunda_llamada.Add(puntero_aplicacion.ua_ht2);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[8] - cicloRCMCI_withRH_Segunda_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH_Segunda_llamada.temp[7] - cicloRCMCI_withRH_Segunda_llamada.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH_Segunda_llamada.temp[6] - cicloRCMCI_withRH_Segunda_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Segunda_llamada.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Segunda_llamada.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                            textBox83.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();

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
                                puntero_aplicacion.textBox17.Text = ua_LT_list_segunda_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list_segunda_llamada[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
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

                        //TERCERA LLAMADA para la optimización
                        double max_recomp_fraction_2 = 0.0;
                        double max_mc1_p_in_2 = 0.0;
                        double temp5_max_eff_2 = 0.0;

                        List<Double> temp5_list_tercera = new List<Double>();

                        core.RCMCIwithReheating cicloRCMCI_withRH_Tercera_llamada = new core.RCMCIwithReheating();

                        List<Double> recomp_frac2_list_tercera_llamada = new List<Double>();
                        List<Double> p_mc1_in2_list_tercera_llamada = new List<Double>();
                        List<Double> p_mc1_out2_list_tercera_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_tercera_llamada = new List<Double>();
                        List<Double> p_rhx_in2_list_tercera_llamada = new List<Double>();
                        List<Double> ua_LT_list_tercera_llamada = new List<Double>();
                        List<Double> ua_HT_list_tercera_llamada = new List<Double>();

                        //xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                        //xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        //xlWorkSheet1.Activate();

                        using (var solver2 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                        {
                            solver2.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                            solver2.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                            solver2.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value + 3500.0), (initial_CIP_value + 6500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH_Tercera_llamada, puntero_aplicacion.w_dot_net2, i,
                                temp5_max_eff_1, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Tercera_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Tercera_llamada.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Tercera_llamada.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                LT_fraction = variables[3];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH_Tercera_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH_Tercera_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH_Tercera_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH_Tercera_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH_Tercera_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH_Tercera_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH_Tercera_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH_Tercera_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH_Tercera_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH_Tercera_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH_Tercera_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH_Tercera_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH_Tercera_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH_Tercera_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH_Tercera_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH_Tercera_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH_Tercera_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH_Tercera_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH_Tercera_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH_Tercera_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH_Tercera_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH_Tercera_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH_Tercera_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH_Tercera_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH_Tercera_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH_Tercera_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH_Tercera_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH_Tercera_llamada.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Tercera_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Tercera_llamada.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Tercera_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Tercera_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Tercera_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Tercera_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Tercera_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Tercera_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Tercera_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Tercera_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Tercera_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Tercera_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Tercera_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Tercera_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Tercera_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Tercera_llamada.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Tercera_llamada.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Tercera_llamada.COOLER.Q_dot;

                                eta_thermal2_list_tercera_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_tercera_llamada.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list_tercera_llamada.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list_tercera_llamada.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_tercera.Add(puntero_aplicacion.temp25);
                                ua_LT_list_tercera_llamada.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list_tercera_llamada.Add(puntero_aplicacion.ua_ht2);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[8] - cicloRCMCI_withRH_Tercera_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH_Tercera_llamada.temp[7] - cicloRCMCI_withRH_Tercera_llamada.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH_Tercera_llamada.temp[6] - cicloRCMCI_withRH_Tercera_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Tercera_llamada.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Tercera_llamada.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                            textBox83.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();

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
                                puntero_aplicacion.textBox17.Text = ua_LT_list_tercera_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list_tercera_llamada[maxIndex].ToString();
                            }

                            //Closing Excel Book
                            //xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            //xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
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

                        //CUARTA LLAMADA para la optimización
                        double max_recomp_fraction_3 = 0.0;
                        double max_mc1_p_in_3 = 0.0;
                        double temp5_max_eff_3 = 0.0;

                        List<Double> temp5_list_cuarta = new List<Double>();

                        core.RCMCIwithReheating cicloRCMCI_withRH_Cuarta_llamada = new core.RCMCIwithReheating();

                        List<Double> recomp_frac2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_mc1_in2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_mc1_out2_list_cuarta_llamada = new List<Double>();
                        List<Double> eta_thermal2_list_cuarta_llamada = new List<Double>();
                        List<Double> p_rhx_in2_list_cuarta_llamada = new List<Double>();
                        List<Double> ua_LT_list_cuarta_llamada = new List<Double>();
                        List<Double> ua_HT_list_cuarta_llamada = new List<Double>();
                        List<Double> massflow2_list = new List<Double>();
                        List<Double> PHX_Q2_list = new List<Double>();

                        xlWorkBook1 = xlApp1.Workbooks.Open(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls");
                        xlWorkSheet1 = xlWorkBook1.Worksheets[1];
                        xlWorkSheet1.Activate();

                        using (var solver3 = new NLoptSolver(algorithm_type, 4, 0.01, 10000))
                        {
                            solver3.SetLowerBounds(new[] { 0.0, initial_CIP_value, (initial_CIP_value + 200.0), 0.0 });
                            solver3.SetUpperBounds(new[] { 1.0, 18500.0, (puntero_aplicacion.p_mc2_out2 / 1.5), 1.0 });

                            solver3.SetInitialStepSize(new[] { 0.05, 250.0, 250.0, 0.05 });

                            var initialValue = new[] { 0.25, (initial_CIP_value + 3500.0), (initial_CIP_value + 6500.0), 0.5 };

                            Func<double[], double> funcion = delegate (double[] variables)
                            {
                                puntero_aplicacion.luis.RecompCycle_RCMCI_without_Reheating_newproposed_for_Optimzation(puntero_aplicacion.luis,
                                ref cicloRCMCI_withRH_Cuarta_llamada, puntero_aplicacion.w_dot_net2, i,
                                temp5_max_eff_2, puntero_aplicacion.t_rht_in2, variables[1], variables[2],
                                puntero_aplicacion.p_mc2_out2, variables[1], i, variables[2],
                                variables[3], UA_Total, puntero_aplicacion.eta2_mc2,
                                puntero_aplicacion.eta_rc2, puntero_aplicacion.eta1_mc2, puntero_aplicacion.eta_t2,
                                puntero_aplicacion.eta_trh2, puntero_aplicacion.n_sub_hxrs2,
                                variables[0], puntero_aplicacion.tol2, puntero_aplicacion.eta_thermal2, -puntero_aplicacion.dp2_lt1,
                                -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_ht2,
                                -puntero_aplicacion.dp11_pc1, -puntero_aplicacion.dp12_pc1, -puntero_aplicacion.dp2_phx1,
                                -puntero_aplicacion.dp2_phx2, -puntero_aplicacion.dp2_rhx1, -puntero_aplicacion.dp2_rhx2,
                                -puntero_aplicacion.dp11_pc2, -puntero_aplicacion.dp12_pc2);

                                counter++;

                                puntero_aplicacion.massflow2 = cicloRCMCI_withRH_Cuarta_llamada.m_dot_turbine;
                                puntero_aplicacion.w_dot_net2 = cicloRCMCI_withRH_Cuarta_llamada.W_dot_net;

                                puntero_aplicacion.eta_thermal2 = cicloRCMCI_withRH_Cuarta_llamada.eta_thermal;
                                puntero_aplicacion.recomp_frac2 = variables[0];
                                puntero_aplicacion.p_mc1_in2 = variables[1];
                                puntero_aplicacion.p_rhx_in2 = variables[1];
                                puntero_aplicacion.p_mc1_out2 = variables[2];
                                LT_fraction = variables[3];
                                puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                puntero_aplicacion.temp21 = cicloRCMCI_withRH_Cuarta_llamada.temp[0];
                                puntero_aplicacion.temp22 = cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                                puntero_aplicacion.temp23 = cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                                puntero_aplicacion.temp24 = cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                                puntero_aplicacion.temp25 = cicloRCMCI_withRH_Cuarta_llamada.temp[4];
                                puntero_aplicacion.temp26 = cicloRCMCI_withRH_Cuarta_llamada.temp[5];
                                puntero_aplicacion.temp27 = cicloRCMCI_withRH_Cuarta_llamada.temp[6];
                                puntero_aplicacion.temp28 = cicloRCMCI_withRH_Cuarta_llamada.temp[7];
                                puntero_aplicacion.temp29 = cicloRCMCI_withRH_Cuarta_llamada.temp[8];
                                puntero_aplicacion.temp210 = cicloRCMCI_withRH_Cuarta_llamada.temp[9];
                                puntero_aplicacion.temp211 = cicloRCMCI_withRH_Cuarta_llamada.temp[10];
                                puntero_aplicacion.temp212 = cicloRCMCI_withRH_Cuarta_llamada.temp[11];
                                puntero_aplicacion.temp213 = cicloRCMCI_withRH_Cuarta_llamada.temp[12];
                                puntero_aplicacion.temp214 = cicloRCMCI_withRH_Cuarta_llamada.temp[13];

                                puntero_aplicacion.pres21 = cicloRCMCI_withRH_Cuarta_llamada.pres[0];
                                puntero_aplicacion.pres22 = cicloRCMCI_withRH_Cuarta_llamada.pres[1];
                                puntero_aplicacion.pres23 = cicloRCMCI_withRH_Cuarta_llamada.pres[2];
                                puntero_aplicacion.pres24 = cicloRCMCI_withRH_Cuarta_llamada.pres[3];
                                puntero_aplicacion.pres25 = cicloRCMCI_withRH_Cuarta_llamada.pres[4];
                                puntero_aplicacion.pres26 = cicloRCMCI_withRH_Cuarta_llamada.pres[5];
                                puntero_aplicacion.pres27 = cicloRCMCI_withRH_Cuarta_llamada.pres[6];
                                puntero_aplicacion.pres28 = cicloRCMCI_withRH_Cuarta_llamada.pres[7];
                                puntero_aplicacion.pres29 = cicloRCMCI_withRH_Cuarta_llamada.pres[8];
                                puntero_aplicacion.pres210 = cicloRCMCI_withRH_Cuarta_llamada.pres[9];
                                puntero_aplicacion.pres211 = cicloRCMCI_withRH_Cuarta_llamada.pres[10];
                                puntero_aplicacion.pres212 = cicloRCMCI_withRH_Cuarta_llamada.pres[11];
                                puntero_aplicacion.pres213 = cicloRCMCI_withRH_Cuarta_llamada.pres[12];
                                puntero_aplicacion.pres214 = cicloRCMCI_withRH_Cuarta_llamada.pres[13];

                                puntero_aplicacion.PHX1 = cicloRCMCI_withRH_Cuarta_llamada.PHX.Q_dot;
                                puntero_aplicacion.RHX1 = cicloRCMCI_withRH_Cuarta_llamada.RHX.Q_dot;

                                puntero_aplicacion.LT_Q = cicloRCMCI_withRH_Cuarta_llamada.LT.Q_dot;
                                puntero_aplicacion.LT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[0];
                                puntero_aplicacion.LT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.LT.m_dot_design[1];
                                puntero_aplicacion.LT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_c_in;
                                puntero_aplicacion.LT_Thin = cicloRCMCI_withRH_Cuarta_llamada.LT.T_h_in;
                                puntero_aplicacion.LT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_in;
                                puntero_aplicacion.LT_Phin = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_in;
                                puntero_aplicacion.LT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_c_out;
                                puntero_aplicacion.LT_Phout = cicloRCMCI_withRH_Cuarta_llamada.LT.P_h_out;
                                puntero_aplicacion.LT_Effc = cicloRCMCI_withRH_Cuarta_llamada.LT.eff;

                                puntero_aplicacion.HT_Q = cicloRCMCI_withRH_Cuarta_llamada.HT.Q_dot;
                                puntero_aplicacion.HT_mdotc = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[0];
                                puntero_aplicacion.HT_mdoth = cicloRCMCI_withRH_Cuarta_llamada.HT.m_dot_design[1];
                                puntero_aplicacion.HT_Tcin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_c_in;
                                puntero_aplicacion.HT_Thin = cicloRCMCI_withRH_Cuarta_llamada.HT.T_h_in;
                                puntero_aplicacion.HT_Pcin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_in;
                                puntero_aplicacion.HT_Phin = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_in;
                                puntero_aplicacion.HT_Pcout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_c_out;
                                puntero_aplicacion.HT_Phout = cicloRCMCI_withRH_Cuarta_llamada.HT.P_h_out;
                                puntero_aplicacion.HT_Effc = cicloRCMCI_withRH_Cuarta_llamada.HT.eff;

                                puntero_aplicacion.PC11 = -cicloRCMCI_withRH_Cuarta_llamada.PC.Q_dot;
                                puntero_aplicacion.PC21 = -cicloRCMCI_withRH_Cuarta_llamada.COOLER.Q_dot;

                                eta_thermal2_list_cuarta_llamada.Add(puntero_aplicacion.eta_thermal2);
                                recomp_frac2_list_cuarta_llamada.Add(puntero_aplicacion.recomp_frac2);
                                p_mc1_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_in2);
                                p_mc1_out2_list_cuarta_llamada.Add(puntero_aplicacion.p_mc1_out2);
                                p_rhx_in2_list_cuarta_llamada.Add(puntero_aplicacion.p_rhx_in2);
                                temp5_list_cuarta.Add(puntero_aplicacion.temp25);
                                ua_LT_list_cuarta_llamada.Add(puntero_aplicacion.ua_lt2);
                                ua_HT_list_cuarta_llamada.Add(puntero_aplicacion.ua_ht2);

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

                                PHX_Q2_list.Add(cicloRCMCI_withRH_Cuarta_llamada.RHX.Q_dot);

                                HT_Eff_list.Add(cicloRCMCI_withRH_Cuarta_llamada.HT.eff);
                                LT_Eff_list.Add(cicloRCMCI_withRH_Cuarta_llamada.LT.eff);

                                listBox1.Items.Add(counter.ToString());
                                listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                                listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                                listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                                listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                                listBox8.Items.Add(puntero_aplicacion.temp25.ToString());
                                listBox9.Items.Add(puntero_aplicacion.temp26.ToString());

                                double LTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[2];
                                double LTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[8] - cicloRCMCI_withRH_Cuarta_llamada.temp[1];
                                double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                                double HTR_min_DT_1 = cicloRCMCI_withRH_Cuarta_llamada.temp[7] - cicloRCMCI_withRH_Cuarta_llamada.temp[3];
                                double HTR_min_DT_2 = cicloRCMCI_withRH_Cuarta_llamada.temp[6] - cicloRCMCI_withRH_Cuarta_llamada.temp[4];
                                double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            ////MC1_in(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc1_in2);
                            ////MC1_out(kPa)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.p_mc1_out2);
                            ////CIT (MC1)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.t_mc1_in2 - 273.15);
                            ////LT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            ////HT UA(kW/K)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                            ////Rec.Frac.
                            //xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                            ////P_rhx_in
                            //xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                            ////Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            ////LTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRCMCI_withRH_Cuarta_llamada.LT.eff.ToString();
                            ////LTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                            ////HTR Eff.(%)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRCMCI_withRH_Cuarta_llamada.HT.eff.ToString();
                            ////HTR Pinch(ºC)
                            //xlWorkSheet1.Cells[counter_Excel + 1, 12] = HTR_min_DT_paper.ToString();

                            //    counter_Excel++;

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
                            //textBox4.Text = p_rhx_in2_list[maxIndex].ToString();
                            textBox82.Text = ua_LT_list_cuarta_llamada[maxIndex].ToString();
                            textBox83.Text = ua_HT_list_cuarta_llamada[maxIndex].ToString();

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
                                puntero_aplicacion.textBox17.Text = ua_LT_list_cuarta_llamada[maxIndex].ToString();
                                puntero_aplicacion.textBox16.Text = ua_HT_list_cuarta_llamada[maxIndex].ToString();
                            }

                            //The variable 'i' is the loop counter for the CIT
                            listBox18.Items.Add(i.ToString());
                            listBox17.Items.Add(eta_thermal2_list_cuarta_llamada[maxIndex].ToString());
                            listBox16.Items.Add(recomp_frac2_list_cuarta_llamada[maxIndex].ToString());
                            listBox15.Items.Add(p_mc1_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox20.Items.Add(p_rhx_in2_list_cuarta_llamada[maxIndex].ToString());
                            listBox11.Items.Add(t5_list[maxIndex].ToString());
                            listBox12.Items.Add(t6_list[maxIndex].ToString());
                            listBox13.Items.Add(ua_HT_list_cuarta_llamada[maxIndex].ToString());
                            listBox14.Items.Add(ua_LT_list_cuarta_llamada[maxIndex].ToString());

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
                                PTC.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                PTC.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                PTC.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                PTC.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
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
                                LF.textBox3.Text = Convert.ToString(t11_list[maxIndex]);
                                LF.textBox6.Text = Convert.ToString(t12_list[maxIndex]);
                                LF.textBox4.Text = Convert.ToString(p11_list[maxIndex]);
                                LF.textBox5.Text = Convert.ToString(p12_list[maxIndex]);
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
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(p_mc1_in2_list_cuarta_llamada[maxIndex]);
                            //Main compressor output pressure (kPa)
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(p_mc1_out2_list_cuarta_llamada[maxIndex]);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(i - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = puntero_aplicacion.ua_lt2.ToString();
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.ua_ht2.ToString();
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = recomp_frac2_list_cuarta_llamada[maxIndex].ToString();
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = (eta_thermal2_list_cuarta_llamada[maxIndex] * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = LT_Eff_list[maxIndex].ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = LTR_min_DT_paper_max.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = HT_Eff_list[maxIndex].ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 11] = HTR_min_DT_paper_max.ToString();

                            if (checkBox7.Checked == false)
                            {
                                //PTC_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 12] = puntero_aplicacion.PTC_Main_SF_Effective_Apperture_Area.ToString();
                                //PTC_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 13] = puntero_aplicacion.PTC_Main_SF_Pressure_drop.ToString();
                                //LF_Apperture_Area(m2)
                                xlWorkSheet1.Cells[counter_Excel + 1, 14] = puntero_aplicacion.LF_Main_SF_Effective_Apperture_Area.ToString();
                                //LF_Pressure_Drop(bar)
                                xlWorkSheet1.Cells[counter_Excel + 1, 15] = puntero_aplicacion.LF_Main_SF_Pressure_drop.ToString();
                            }

                            counter_Excel++;

                            initial_CIP_value = puntero_aplicacion.p_mc1_in2;

                            //Closing Excel Book
                            xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                            xlWorkBook1.Close(true, misValue1, misValue1);
                            //xlApp1.Quit();

                            //releaseObject(xlWorkSheet1);
                            ////releaseObject(xlWorkSheet2);
                            //releaseObject(xlWorkBook1);
                            //releaseObject(xlApp1);

                        } //Fin de la CUARTA LLAMADA para optimización
                    }
                }

            }        
        }
    }
}
