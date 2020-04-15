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
                    solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, (initial_CIP_value + 200.0) });
                    solver.SetUpperBounds(new[] { 1.0, 125000, (puntero_aplicacion.p_mc2_out2 / 1.5) });

                    solver.SetInitialStepSize(new[] { 0.05, 100.0, 100.0 });

                    var initialValue = new[] { 0.2, initial_CIP_value, (initial_CIP_value + 500.0) };

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

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc1_in2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.p_mc1_out2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.p_rhx_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox9.Items.Add(puntero_aplicacion.temp28.ToString());

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
                }

                //Closing Excel Book
                xlWorkBook1.SaveAs(textBox3.Text + "RCMCI_without_ReHeating_newproposedconfiguration" + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                xlWorkBook1.Close(true, misValue1, misValue1);
                //xlApp1.Quit();

                //releaseObject(xlWorkSheet1);
                //releaseObject(xlWorkBook1);
                //releaseObject(xlApp1);

            }//Fin de la PRIMERA LLAMADA para optimización
        }
    }
}
