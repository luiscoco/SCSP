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
    public partial class RC_without_ReHeating_Optimization_Analysis_Results : Form
    {
        RC_without_ReHeating puntero_aplicacion;
        public RC_without_ReHeating_Optimization_Analysis_Results(RC_without_ReHeating puntero1)
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

            double initial_recomp_frac_value = 0.2;

            double initial_LT_fraction = 0.5;

            double optimization_error_tolerance = 0.00001;

            optimization_error_tolerance = Convert.ToDouble(textBox3.Text);

            double recomp_frac_step_size = 0.005;

            recomp_frac_step_size = Convert.ToDouble(textBox4.Text);

            double CIP_step_size = 50.0;

            CIP_step_size = Convert.ToDouble(textBox5.Text);

            double LT_fraction_step_size = 0.01;

            LT_fraction_step_size = Convert.ToDouble(textBox6.Text);

            //Do not optimize UA
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text, puntero_aplicacion.category);
                                     //luis.core1(this.comboBox2.Text + "=" + textBox31.Text + "," + this.comboBox6.Text + "=" + textBox36.Text, category);
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
                puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                //recomp_frac2 = Convert.ToDouble(textBox15.Text);
                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle_withoutRH cicloRC_withoutRH = new core.RecompCycle_withoutRH();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
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

                //Set CIP initial value
                if (checkBox6.Checked == true)
                {
                    initial_CIP_value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                }

                //Set recompresion fraction initial value
                if (checkBox8.Checked == true)
                {
                    initial_recomp_frac_value = Convert.ToDouble(textBox7.Text);
                }                
             
                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                                         //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text
                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text;
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
                xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";                

                //Simple Brayton power cycle
                if (puntero_aplicacion.checkBox2.Checked == true)
                {                    
                    using (var solver = new NLoptSolver(algorithm_type, 1, optimization_error_tolerance, 10000))
                    {
                        solver.SetLowerBounds(new[] { initial_CIP_value });
                        solver.SetUpperBounds(new[] {  125000.0 });

                        solver.SetInitialStepSize(new[] { CIP_step_size });

                        var initialValue = new[] { initial_CIP_value };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2, variables[0],
                            puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2,
                            0.0, 0.0, puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                            puntero_aplicacion.p_mc_in2 = variables[0];

                            puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                            puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withoutRH.PHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withoutRH.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withoutRH.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withoutRH.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withoutRH.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withoutRH.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withoutRH.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withoutRH.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withoutRH.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withoutRH.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withoutRH.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withoutRH.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withoutRH.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withoutRH.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withoutRH.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withoutRH.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withoutRH.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withoutRH.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withoutRH.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withoutRH.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withoutRH.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withoutRH.PC.Q_dot;
                                                       
                            eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                            p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add("0.0");
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(cicloRC_withoutRH.LT.UA.ToString());
                            listBox6.Items.Add(cicloRC_withoutRH.HT.UA.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                            double LTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[2];
                            double LTR_min_DT_2 = cicloRC_withoutRH.temp[8] - cicloRC_withoutRH.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[3];
                            double HTR_min_DT_2 = cicloRC_withoutRH.temp[6] - cicloRC_withoutRH.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //CIP
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = "0.0";
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = "0.0";
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = cicloRC_withoutRH.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = "n/a";
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = "n/a";
                           
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
                        textBox90.Text = "0.0";
                        textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                        textBox82.Text = cicloRC_withoutRH.LT.UA.ToString();
                        textBox83.Text = cicloRC_withoutRH.HT.UA.ToString();

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = "0.0";
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        xlWorkBook1.SaveAs(textBox2.Text + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        xlWorkBook1.Close(true, misValue1, misValue1);
                        xlApp1.Quit();

                        releaseObject(xlWorkSheet1);
                        //releaseObject(xlWorkSheet2);
                        releaseObject(xlWorkBook1);
                        releaseObject(xlApp1);
                    }
                }

                //Recompression Brayton power cycle
                else if (puntero_aplicacion.checkBox2.Checked == false)
                {                    
                    using (var solver = new NLoptSolver(algorithm_type, 2, optimization_error_tolerance, 10000))
                    {
                        solver.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                        solver.SetUpperBounds(new[] { 1.0, 125000 });

                        solver.SetInitialStepSize(new[] { recomp_frac_step_size, CIP_step_size });

                        var initialValue = new[] { initial_recomp_frac_value, initial_CIP_value };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2, variables[1],
                            puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2,
                            puntero_aplicacion.ua_ht2, variables[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables[0];
                            puntero_aplicacion.p_mc_in2 = variables[1];

                            puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                            puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withoutRH.PHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withoutRH.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withoutRH.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withoutRH.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withoutRH.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withoutRH.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withoutRH.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withoutRH.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withoutRH.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withoutRH.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withoutRH.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withoutRH.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withoutRH.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withoutRH.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withoutRH.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withoutRH.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withoutRH.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withoutRH.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withoutRH.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withoutRH.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withoutRH.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withoutRH.PC.Q_dot;
                                                        
                            eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                            double LTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[2];
                            double LTR_min_DT_2 = cicloRC_withoutRH.temp[8] - cicloRC_withoutRH.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[3];
                            double HTR_min_DT_2 = cicloRC_withoutRH.temp[6] - cicloRC_withoutRH.temp[4];
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
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = cicloRC_withoutRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRC_withoutRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = HTR_min_DT_paper.ToString();
                       
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

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        xlWorkBook1.SaveAs(textBox2.Text + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        xlWorkBook1.Close(true, misValue1, misValue1);
                        xlApp1.Quit();

                        releaseObject(xlWorkSheet1);
                        //releaseObject(xlWorkSheet2);
                        releaseObject(xlWorkBook1);
                        releaseObject(xlApp1);
                    }
                }
            }

            //-------------------------------------------------------------------------

            //Optimize UA
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text, puntero_aplicacion.category);
                    //luis.core1(this.comboBox2.Text + "=" + textBox31.Text + "," + this.comboBox6.Text + "=" + textBox36.Text, category);
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
                puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                //recomp_frac2 = Convert.ToDouble(textBox15.Text);
                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle_withoutRH cicloRC_withoutRH = new core.RecompCycle_withoutRH();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
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

                //Set CIP initival value
                if (checkBox6.Checked == true)
                {
                    initial_CIP_value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                }

                //Set recompresion fraction initial value
                if (checkBox8.Checked == true)
                {
                    initial_recomp_frac_value = Convert.ToDouble(textBox7.Text);
                }

                //Set LT_fraction initial value
                if (checkBox9.Checked == true)
                {
                    initial_LT_fraction = Convert.ToDouble(textBox8.Text);
                }

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                                         //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text
                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text;
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
                xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";               

                //Simple Brayton power cycle
                if (puntero_aplicacion.checkBox2.Checked == true)
                {
                    using (var solver = new NLoptSolver(algorithm_type, 1, optimization_error_tolerance, 10000))
                    {
                        solver.SetLowerBounds(new[] { initial_CIP_value });
                        solver.SetUpperBounds(new[] { 125000.0 });

                        solver.SetInitialStepSize(new[] { CIP_step_size });

                        var initialValue = new[] { initial_CIP_value };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2, variables[0],
                            puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2,
                            0.0, 0.0, puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                            puntero_aplicacion.p_mc_in2 = variables[0];

                            puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                            puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withoutRH.PHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withoutRH.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withoutRH.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withoutRH.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withoutRH.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withoutRH.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withoutRH.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withoutRH.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withoutRH.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withoutRH.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withoutRH.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withoutRH.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withoutRH.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withoutRH.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withoutRH.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withoutRH.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withoutRH.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withoutRH.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withoutRH.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withoutRH.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withoutRH.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withoutRH.PC.Q_dot;
                                                       
                            eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                            p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add("0.0");
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(cicloRC_withoutRH.LT.UA.ToString());
                            listBox6.Items.Add(cicloRC_withoutRH.HT.UA.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                            double LTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[2];
                            double LTR_min_DT_2 = cicloRC_withoutRH.temp[8] - cicloRC_withoutRH.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[3];
                            double HTR_min_DT_2 = cicloRC_withoutRH.temp[6] - cicloRC_withoutRH.temp[4];
                            double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                            //CIP
                            xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                            //CIT
                            xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                            //LT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                            //HT UA(kW/K)
                            xlWorkSheet1.Cells[counter_Excel + 1, 4] = "0.0";
                            //Rec.Frac.
                            xlWorkSheet1.Cells[counter_Excel + 1, 5] = "0.0";
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = cicloRC_withoutRH.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = "n/a";
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = "n/a";
                           
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
                        textBox90.Text = "0.0";
                        textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                        textBox82.Text = cicloRC_withoutRH.LT.UA.ToString();
                        textBox83.Text = cicloRC_withoutRH.HT.UA.ToString();

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = "0.0";
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\SolarPaces 2019\\SolarPaces2019_Paper_Results_SB_without_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        xlWorkBook1.Close(true, misValue1, misValue1);
                        xlApp1.Quit();

                        releaseObject(xlWorkSheet1);
                        //releaseObject(xlWorkSheet2);
                        releaseObject(xlWorkBook1);
                        releaseObject(xlApp1);
                    }
                }

                //Recompression Brayton power cycle
                else if (puntero_aplicacion.checkBox2.Checked == false)
                {
                    using (var solver = new NLoptSolver(algorithm_type, 3, optimization_error_tolerance, 10000))
                    {
                        solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.2 });
                        solver.SetUpperBounds(new[] { 1.0, 125000, 0.8 });

                        solver.SetInitialStepSize(new[] { recomp_frac_step_size, CIP_step_size, LT_fraction_step_size });

                        var initialValue = new[] { initial_recomp_frac_value, initial_CIP_value, initial_LT_fraction };

                        Func<double[], double> funcion = delegate (double[] variables)
                        {
                            puntero_aplicacion.luis.RecompCycledesign_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2, variables[1],
                            puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, variables[2],
                            UA_Total, variables[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                            counter++;

                            puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                            puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                            puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                            puntero_aplicacion.recomp_frac2 = variables[0];
                            puntero_aplicacion.p_mc_in2 = variables[1];
                            LT_fraction = variables[2];
                            puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                            puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                            puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                            puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                            puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                            puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                            puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                            puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                            puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                            puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                            puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                            puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                            puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                            puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                            puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                            puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                            puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                            puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                            puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                            puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                            puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                            puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                            puntero_aplicacion.PHX_Q2 = cicloRC_withoutRH.PHX.Q_dot;

                            puntero_aplicacion.LT_Q = cicloRC_withoutRH.LT.Q_dot;
                            puntero_aplicacion.LT_mdotc = cicloRC_withoutRH.LT.m_dot_design[0];
                            puntero_aplicacion.LT_mdoth = cicloRC_withoutRH.LT.m_dot_design[1];
                            puntero_aplicacion.LT_Tcin = cicloRC_withoutRH.LT.T_c_in;
                            puntero_aplicacion.LT_Thin = cicloRC_withoutRH.LT.T_h_in;
                            puntero_aplicacion.LT_Pcin = cicloRC_withoutRH.LT.P_c_in;
                            puntero_aplicacion.LT_Phin = cicloRC_withoutRH.LT.P_h_in;
                            puntero_aplicacion.LT_Pcout = cicloRC_withoutRH.LT.P_c_out;
                            puntero_aplicacion.LT_Phout = cicloRC_withoutRH.LT.P_h_out;
                            puntero_aplicacion.LT_Effc = cicloRC_withoutRH.LT.eff;

                            puntero_aplicacion.HT_Q = cicloRC_withoutRH.HT.Q_dot;
                            puntero_aplicacion.HT_mdotc = cicloRC_withoutRH.HT.m_dot_design[0];
                            puntero_aplicacion.HT_mdoth = cicloRC_withoutRH.HT.m_dot_design[1];
                            puntero_aplicacion.HT_Tcin = cicloRC_withoutRH.HT.T_c_in;
                            puntero_aplicacion.HT_Thin = cicloRC_withoutRH.HT.T_h_in;
                            puntero_aplicacion.HT_Pcin = cicloRC_withoutRH.HT.P_c_in;
                            puntero_aplicacion.HT_Phin = cicloRC_withoutRH.HT.P_h_in;
                            puntero_aplicacion.HT_Pcout = cicloRC_withoutRH.HT.P_c_out;
                            puntero_aplicacion.HT_Phout = cicloRC_withoutRH.HT.P_h_out;
                            puntero_aplicacion.HT_Effc = cicloRC_withoutRH.HT.eff;

                            puntero_aplicacion.PC_Q2 = cicloRC_withoutRH.PC.Q_dot;
                                                       
                            listBox1.Items.Add(counter.ToString());
                            listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                            listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                            listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                            listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                            listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                            listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                            listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                            eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                            recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                            p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                            ua_LT_list.Add(puntero_aplicacion.ua_lt2);
                            ua_HT_list.Add(puntero_aplicacion.ua_ht2);

                            double LTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[2];
                            double LTR_min_DT_2 = cicloRC_withoutRH.temp[8] - cicloRC_withoutRH.temp[1];
                            double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                            double HTR_min_DT_1 = cicloRC_withoutRH.temp[7] - cicloRC_withoutRH.temp[3];
                            double HTR_min_DT_2 = cicloRC_withoutRH.temp[6] - cicloRC_withoutRH.temp[4];
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
                            //Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 6] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                            //LTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 7] = cicloRC_withoutRH.LT.eff.ToString();
                            //LTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                            //HTR Eff.(%)
                            xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRC_withoutRH.HT.eff.ToString();
                            //HTR Pinch(ºC)
                            xlWorkSheet1.Cells[counter_Excel + 1, 10] = HTR_min_DT_paper.ToString();
                           
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
                        textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                        textBox82.Text = ua_LT_list[maxIndex].ToString();
                        textBox83.Text = ua_HT_list[maxIndex].ToString();

                        //Copy results as design-point inputs
                        if (checkBox3.Checked == true)
                        {
                            puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                            puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                            puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                        }

                        //Closing Excel Book
                        xlWorkBook1.SaveAs(textBox2.Text + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                        xlWorkBook1.Close(true, misValue1, misValue1);
                        xlApp1.Quit();

                        releaseObject(xlWorkSheet1);
                        //releaseObject(xlWorkSheet2);
                        releaseObject(xlWorkBook1);
                        releaseObject(xlApp1);
                    }
                }
            }
        }

        //Clear Lists
        public void Clear_all_Lists()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            listBox8.Items.Clear();

            listBox11.Items.Clear();
            listBox12.Items.Clear();
            listBox13.Items.Clear();
            listBox14.Items.Clear();
            listBox15.Items.Clear();
            listBox16.Items.Clear();
            listBox17.Items.Clear();
            listBox18.Items.Clear();
        }
        
        private void Button2_Click(object sender, EventArgs e)
        {
            Clear_all_Lists();
        }

        //OK Button
        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Run Design-Point analysis
        private void Button5_Click(object sender, EventArgs e)
        {
            puntero_aplicacion.button11_Click(this, e);
        }

        //Run CIT Optimization
        private void Button6_Click(object sender, EventArgs e)
        {
            int counter = 0;

            double initial_CIP_value = 0;

            double initial_recomp_frac_value = 0.2;

            double initial_LT_fraction = 0.5;

            double optimization_error_tolerance = 0.00001;

            optimization_error_tolerance = Convert.ToDouble(textBox3.Text);

            double recomp_frac_step_size = 0.005;

            recomp_frac_step_size = Convert.ToDouble(textBox4.Text);

            double CIP_step_size = 50.0;

            CIP_step_size = Convert.ToDouble(textBox5.Text);

            double LT_fraction_step_size = 0.01;

            LT_fraction_step_size = Convert.ToDouble(textBox6.Text);

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

                    // Not UA Optimization 
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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text, puntero_aplicacion.category);
                            //luis.core1(this.comboBox2.Text + "=" + textBox31.Text + "," + this.comboBox6.Text + "=" + textBox36.Text, category);
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
                        //puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                        puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                        //recomp_frac2 = Convert.ToDouble(textBox15.Text);
                        puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RecompCycle_withoutRH cicloRC_withoutRH = new core.RecompCycle_withoutRH();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        //int counter = 0;

                        List<Double> massflow2_list = new List<Double>();
                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc_in2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
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
                            //Set CIP initial value
                            if (checkBox6.Checked == true)
                            {
                                initial_CIP_value = Convert.ToDouble(textBox1.Text);
                            }
                            else
                            {
                                initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                            }

                            //Set recompresion fraction initial value
                            if (checkBox8.Checked == true)
                            {
                                initial_recomp_frac_value = Convert.ToDouble(textBox7.Text);
                            }

                            xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";
                            //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text
                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text;
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
                            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";

                            if (checkBox7.Checked == false)
                            {
                                xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";
                            }
                        }

                        //Simple Brayton power cycle
                        if (puntero_aplicacion.checkBox2.Checked == true)
                        {
                            using (var solver = new NLoptSolver(algorithm_type, 1, optimization_error_tolerance, 10000))
                            {
                                solver.SetLowerBounds(new[] { initial_CIP_value });
                                solver.SetUpperBounds(new[] { 125000.0 });

                                solver.SetInitialStepSize(new[] { CIP_step_size });

                                var initialValue = new[] { initial_CIP_value };

                                Func<double[], double> funcion = delegate (double[] variables)
                                {
                                    puntero_aplicacion.luis.RecompCycledesign(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, i, puntero_aplicacion.t_t_in2, variables[0],
                                    puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2,
                                    0.0, 0.0, puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                                    counter++;

                                    puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                                    puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                                    puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                                    puntero_aplicacion.p_mc_in2 = variables[0];

                                    puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                                    puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                                    puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                                    puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                                    puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                                    puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                                    puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                                    puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                                    puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                                    puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                                    puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                                    puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                                    puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                                    puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                                    puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                                    puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                                    puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                                    puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                                    puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                                    puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                                    massflow2_list.Add(puntero_aplicacion.massflow2);
                                    eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                    p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

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

                                    PHX_Q2_list.Add(cicloRC_withoutRH.PHX.Q_dot);

                                    HT_Eff_list.Add(cicloRC_withoutRH.HT.eff);
                                    LT_Eff_list.Add(cicloRC_withoutRH.LT.eff);

                                    ua_lt_list.Add(cicloRC_withoutRH.LT.UA);
                                    ua_ht_list.Add(cicloRC_withoutRH.HT.UA);

                                    listBox1.Items.Add(counter.ToString());
                                    listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                    listBox3.Items.Add("0.0");
                                    listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
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

                                textBox90.Text = "0.0";
                                textBox91.Text = p_mc_in2_list[maxIndex].ToString();
                                textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                                textBox82.Text = ua_lt_list[maxIndex].ToString();
                                textBox83.Text = "0.0";

                                //Copy results as design-point inputs
                                if (checkBox3.Checked == true)
                                {
                                    puntero_aplicacion.textBox15.Text = "0.0";
                                    puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                                }

                                //The variable 'i' is the loop counter for the CIT
                                listBox18.Items.Add(i.ToString());
                                listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                                listBox15.Items.Add(p_mc_in2_list[maxIndex].ToString());
                                listBox16.Items.Add("0.0");
                                listBox11.Items.Add(t8_list[maxIndex].ToString());
                                listBox12.Items.Add(t9_list[maxIndex].ToString());
                                listBox13.Items.Add(ua_ht_list[maxIndex].ToString());
                                listBox14.Items.Add(ua_lt_list[maxIndex].ToString());

                                if (checkBox7.Checked == false)
                                {
                                    //Calculo del campo solar
                                    PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                                    PTC.calledForSensingAnalysis = true;
                                    PTC.comboBox1.Text = "Solar Salt";
                                    PTC.comboBox2.Text = "PureFluid";
                                    PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    PTC.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                    LF.comboBox2.Text = "PureFluid";
                                    LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    LF.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                xlWorkSheet1.Cells[counter_Excel + 1, 4] = "0.0";
                                //Rec.Frac.
                                xlWorkSheet1.Cells[counter_Excel + 1, 5] = "0.0";
                                //Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 6] = (eta_thermal2_list[maxIndex] * 100).ToString();
                                //LTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 7] = LT_Eff_list[maxIndex].ToString();
                                //LTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                                //HTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 9] = "n/a";
                                //HTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 10] = "n/a";

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
                            }
                        }

                        //Recompression Brayton power cycle
                        else if (puntero_aplicacion.checkBox2.Checked == false)
                        {
                            using (var solver = new NLoptSolver(algorithm_type, 2, optimization_error_tolerance, 10000))
                            {
                                solver.SetLowerBounds(new[] { 0.1, initial_CIP_value });
                                solver.SetUpperBounds(new[] { 1.0, 125000 });

                                solver.SetInitialStepSize(new[] { recomp_frac_step_size, CIP_step_size });

                                var initialValue = new[] { initial_recomp_frac_value, initial_CIP_value };

                                Func<double[], double> funcion = delegate (double[] variables)
                                {
                                    puntero_aplicacion.luis.RecompCycledesign(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, i, puntero_aplicacion.t_t_in2, variables[1],
                                    puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2,
                                    puntero_aplicacion.ua_ht2, variables[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                                    counter++;

                                    puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                                    puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                                    puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                                    puntero_aplicacion.recomp_frac2 = variables[0];
                                    puntero_aplicacion.p_mc_in2 = variables[1];

                                    puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                                    puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                                    puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                                    puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                                    puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                                    puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                                    puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                                    puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                                    puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                                    puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                                    puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                                    puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                                    puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                                    puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                                    puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                                    puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                                    puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                                    puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                                    puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                                    puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                                    massflow2_list.Add(puntero_aplicacion.massflow2);
                                    eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                    recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                    p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

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

                                    PHX_Q2_list.Add(cicloRC_withoutRH.PHX.Q_dot);

                                    HT_Eff_list.Add(cicloRC_withoutRH.HT.eff);
                                    LT_Eff_list.Add(cicloRC_withoutRH.LT.eff);

                                    listBox1.Items.Add(counter.ToString());
                                    listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                    listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                    listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());

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

                                //Copy results as design-point inputs
                                if (checkBox3.Checked == true)
                                {
                                    puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                    puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                                }

                                //The variable 'i' is the loop counter for the CIT
                                listBox18.Items.Add(i.ToString());
                                listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                                listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                                listBox15.Items.Add(p_mc_in2_list[maxIndex].ToString());
                                listBox11.Items.Add(t8_list[maxIndex].ToString());
                                listBox12.Items.Add(t9_list[maxIndex].ToString());

                                if (checkBox7.Checked == false)
                                {

                                    //Calculo del campo solar
                                    PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                                    PTC.calledForSensingAnalysis = true;
                                    PTC.comboBox1.Text = "Solar Salt";
                                    PTC.comboBox2.Text = "PureFluid";
                                    PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    PTC.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                    LF.comboBox2.Text = "PureFluid";
                                    LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    LF.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                //Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 6] = (eta_thermal2_list[maxIndex] * 100).ToString();
                                //LTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 7] = LT_Eff_list[maxIndex].ToString();
                                //LTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                                //HTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 9] = HT_Eff_list[maxIndex].ToString();
                                //HTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 10] = HTR_min_DT_paper.ToString();

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
                            }
                        }
                    }

                    //-------------------------------------------------------------------------

                    // UA Optimization 
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
                            puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text, puntero_aplicacion.category);
                            //luis.core1(this.comboBox2.Text + "=" + textBox31.Text + "," + this.comboBox6.Text + "=" + textBox36.Text, category);
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
                        //puntero_aplicacion.t_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox2.Text);
                        puntero_aplicacion.t_t_in2 = Convert.ToDouble(puntero_aplicacion.textBox4.Text);
                        puntero_aplicacion.p_mc_in2 = Convert.ToDouble(puntero_aplicacion.textBox3.Text);
                        puntero_aplicacion.p_mc_out2 = Convert.ToDouble(puntero_aplicacion.textBox8.Text);
                        //puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                        //puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                        puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                        puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                        puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                        puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                        puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                        puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                        //recomp_frac2 = Convert.ToDouble(textBox15.Text);
                        puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                        puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox13.Text);
                        puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                        puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                        puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                        puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                        core.RecompCycle_withoutRH cicloRC_withoutRH = new core.RecompCycle_withoutRH();

                        double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                        double LT_fraction = 0.1;

                        //int counter = 0;

                        List<Double> massflow2_list = new List<Double>();
                        List<Double> recomp_frac2_list = new List<Double>();
                        List<Double> p_mc_in2_list = new List<Double>();
                        List<Double> eta_thermal2_list = new List<Double>();
                        List<Double> PHX_Q2_list = new List<Double>();
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
                            //Set CIP initial value
                            if (checkBox6.Checked == true)
                            {
                                initial_CIP_value = Convert.ToDouble(textBox1.Text);
                            }
                            else
                            {
                                initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                            }

                            //Set recompresion fraction initial value
                            if (checkBox8.Checked == true)
                            {
                                initial_recomp_frac_value = Convert.ToDouble(textBox7.Text);
                            }

                            //Set LT_fraction initial value
                            if (checkBox9.Checked == true)
                            {
                                initial_LT_fraction = Convert.ToDouble(textBox8.Text);
                            }

                            xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                            //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text
                            xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + ":" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + ":" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + ":" + puntero_aplicacion.textBox68.Text;
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
                            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
                            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
                            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
                            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";

                            if (checkBox7.Checked == false)
                            {
                                xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
                                xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
                                xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";
                            }
                        }

                        //Simple Brayton power cycle
                        if (puntero_aplicacion.checkBox2.Checked == true)
                        {
                            using (var solver = new NLoptSolver(algorithm_type, 1, optimization_error_tolerance, 10000))
                            {
                                solver.SetLowerBounds(new[] { initial_CIP_value });
                                solver.SetUpperBounds(new[] { 125000.0 });

                                solver.SetInitialStepSize(new[] { CIP_step_size });

                                var initialValue = new[] { initial_CIP_value };

                                Func<double[], double> funcion = delegate (double[] variables)
                                {
                                    puntero_aplicacion.luis.RecompCycledesign(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, i, puntero_aplicacion.t_t_in2, variables[0],
                                    puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, puntero_aplicacion.ua_lt2,
                                    0.0, 0.0, puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                                    counter++;

                                    puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                                    puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                                    puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                                    puntero_aplicacion.p_mc_in2 = variables[0];

                                    puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                                    puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                                    puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                                    puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                                    puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                                    puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                                    puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                                    puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                                    puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                                    puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                                    puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                                    puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                                    puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                                    puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                                    puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                                    puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                                    puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                                    puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                                    puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                                    puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                                    massflow2_list.Add(puntero_aplicacion.massflow2);
                                    eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                    p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

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

                                    PHX_Q2_list.Add(cicloRC_withoutRH.PHX.Q_dot);

                                    HT_Eff_list.Add(cicloRC_withoutRH.HT.eff);
                                    LT_Eff_list.Add(cicloRC_withoutRH.LT.eff);

                                    ua_LT_list.Add(cicloRC_withoutRH.LT.UA);
                                    ua_HT_list.Add(cicloRC_withoutRH.HT.UA);

                                    listBox1.Items.Add(counter.ToString());
                                    listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                    listBox3.Items.Add("0.0");
                                    listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
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

                                textBox90.Text = "0.0";
                                textBox91.Text = p_mc_in2_list[maxIndex].ToString();
                                textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                                textBox82.Text = ua_LT_list[maxIndex].ToString();
                                textBox83.Text = "0.0";

                                //Copy results as design-point inputs
                                if (checkBox3.Checked == true)
                                {
                                    puntero_aplicacion.textBox15.Text = "0.0";
                                    puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                                }

                                //The variable 'i' is the loop counter for the CIT
                                listBox18.Items.Add(i.ToString());
                                listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                                listBox15.Items.Add(p_mc_in2_list[maxIndex].ToString());
                                listBox16.Items.Add("0.0");
                                listBox11.Items.Add(t8_list[maxIndex].ToString());
                                listBox12.Items.Add(t9_list[maxIndex].ToString());
                                listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                                listBox13.Items.Add(ua_HT_list[maxIndex].ToString());

                                if (checkBox7.Checked == false)
                                {

                                    //Calculo del campo solar
                                    PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                                    PTC.calledForSensingAnalysis = true;
                                    PTC.comboBox1.Text = "Solar Salt";
                                    PTC.comboBox2.Text = "PureFluid";
                                    PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    PTC.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                    LF.comboBox2.Text = "PureFluid";
                                    LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    LF.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                xlWorkSheet1.Cells[counter_Excel + 1, 4] = "0.0";
                                //Rec.Frac.
                                xlWorkSheet1.Cells[counter_Excel + 1, 5] = "0.0";
                                //Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 6] = (eta_thermal2_list[maxIndex] * 100).ToString();
                                //LTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 7] = LT_Eff_list[maxIndex].ToString();
                                //LTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                                //HTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 9] = "n/a";
                                //HTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 10] = "n/a";

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
                            }
                        }

                        //Recompression Brayton power cycle
                        else if (puntero_aplicacion.checkBox2.Checked == false)
                        {
                            using (var solver = new NLoptSolver(algorithm_type, 3, optimization_error_tolerance, 10000))
                            {
                                solver.SetLowerBounds(new[] { 0.1, initial_CIP_value, 0.2 });
                                solver.SetUpperBounds(new[] { 1.0, 125000, 0.8 });

                                solver.SetInitialStepSize(new[] { recomp_frac_step_size, CIP_step_size, LT_fraction_step_size });

                                var initialValue = new[] { initial_recomp_frac_value, initial_CIP_value, initial_LT_fraction };

                                Func<double[], double> funcion = delegate (double[] variables)
                                {
                                    puntero_aplicacion.luis.RecompCycledesign_for_Optimization(puntero_aplicacion.luis, ref cicloRC_withoutRH, puntero_aplicacion.w_dot_net2, i, puntero_aplicacion.t_t_in2, variables[1],
                                    puntero_aplicacion.p_mc_out2, -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_ht2, variables[2],
                                    UA_Total, variables[0], puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                                    counter++;

                                    puntero_aplicacion.massflow2 = cicloRC_withoutRH.m_dot_turbine;
                                    puntero_aplicacion.w_dot_net2 = cicloRC_withoutRH.W_dot_net;
                                    puntero_aplicacion.eta_thermal2 = cicloRC_withoutRH.eta_thermal;
                                    puntero_aplicacion.recomp_frac2 = variables[0];
                                    puntero_aplicacion.p_mc_in2 = variables[1];
                                    LT_fraction = variables[2];
                                    puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                                    puntero_aplicacion.ua_ht2 = UA_Total * (1 - LT_fraction);

                                    puntero_aplicacion.temp21 = cicloRC_withoutRH.temp[0];
                                    puntero_aplicacion.temp22 = cicloRC_withoutRH.temp[1];
                                    puntero_aplicacion.temp23 = cicloRC_withoutRH.temp[2];
                                    puntero_aplicacion.temp24 = cicloRC_withoutRH.temp[3];
                                    puntero_aplicacion.temp25 = cicloRC_withoutRH.temp[4];
                                    puntero_aplicacion.temp26 = cicloRC_withoutRH.temp[5];
                                    puntero_aplicacion.temp27 = cicloRC_withoutRH.temp[6];
                                    puntero_aplicacion.temp28 = cicloRC_withoutRH.temp[7];
                                    puntero_aplicacion.temp29 = cicloRC_withoutRH.temp[8];
                                    puntero_aplicacion.temp210 = cicloRC_withoutRH.temp[9];

                                    puntero_aplicacion.pres21 = cicloRC_withoutRH.pres[0];
                                    puntero_aplicacion.pres22 = cicloRC_withoutRH.pres[1];
                                    puntero_aplicacion.pres23 = cicloRC_withoutRH.pres[2];
                                    puntero_aplicacion.pres24 = cicloRC_withoutRH.pres[3];
                                    puntero_aplicacion.pres25 = cicloRC_withoutRH.pres[4];
                                    puntero_aplicacion.pres26 = cicloRC_withoutRH.pres[5];
                                    puntero_aplicacion.pres27 = cicloRC_withoutRH.pres[6];
                                    puntero_aplicacion.pres28 = cicloRC_withoutRH.pres[7];
                                    puntero_aplicacion.pres29 = cicloRC_withoutRH.pres[8];
                                    puntero_aplicacion.pres210 = cicloRC_withoutRH.pres[9];

                                    massflow2_list.Add(puntero_aplicacion.massflow2);
                                    eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                                    recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                                    p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
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

                                    PHX_Q2_list.Add(cicloRC_withoutRH.PHX.Q_dot);

                                    HT_Eff_list.Add(cicloRC_withoutRH.HT.eff);
                                    LT_Eff_list.Add(cicloRC_withoutRH.LT.eff);

                                    listBox1.Items.Add(counter.ToString());
                                    listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                                    listBox3.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                                    listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                                    listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                                    listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());

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
                                textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                                textBox82.Text = ua_LT_list[maxIndex].ToString();
                                textBox83.Text = ua_HT_list[maxIndex].ToString();

                                //Copy results as design-point inputs
                                if (checkBox3.Checked == true)
                                {
                                    puntero_aplicacion.textBox15.Text = recomp_frac2_list[maxIndex].ToString();
                                    puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                                    puntero_aplicacion.textBox17.Text = ua_LT_list[maxIndex].ToString();
                                    puntero_aplicacion.textBox16.Text = ua_HT_list[maxIndex].ToString();
                                }

                                //The variable 'i' is the loop counter for the CIT
                                listBox18.Items.Add(i.ToString());
                                listBox17.Items.Add(eta_thermal2_list[maxIndex].ToString());
                                listBox16.Items.Add(recomp_frac2_list[maxIndex].ToString());
                                listBox15.Items.Add(p_mc_in2_list[maxIndex].ToString());
                                listBox14.Items.Add(ua_LT_list[maxIndex].ToString());
                                listBox13.Items.Add(ua_HT_list[maxIndex].ToString());
                                listBox11.Items.Add(t8_list[maxIndex].ToString());
                                listBox12.Items.Add(t9_list[maxIndex].ToString());

                                if (checkBox7.Checked == false)
                                {
                                    //Calculo del campo solar
                                    PTC_SF_Calculation PTC = new PTC_SF_Calculation();
                                    PTC.calledForSensingAnalysis = true;
                                    PTC.comboBox1.Text = "Solar Salt";
                                    PTC.comboBox2.Text = "PureFluid";
                                    PTC.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    PTC.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                    LF.comboBox2.Text = "PureFluid";
                                    LF.comboBox13.Text = puntero_aplicacion.comboBox2.Text;
                                    LF.comboBox14.Text = puntero_aplicacion.comboBox6.Text;
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
                                //Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 6] = (eta_thermal2_list[maxIndex] * 100).ToString();
                                //LTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 7] = LT_Eff_list[maxIndex].ToString();
                                //LTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 8] = LTR_min_DT_paper.ToString();
                                //HTR Eff.(%)
                                xlWorkSheet1.Cells[counter_Excel + 1, 9] = HT_Eff_list[maxIndex].ToString();
                                //HTR Pinch(ºC)
                                xlWorkSheet1.Cells[counter_Excel + 1, 10] = HTR_min_DT_paper.ToString();

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
                            }
                        }

                    } //checkBox2.Checked (optimize UA)

                } //loop for CIT optimization analysis

            } //loop for UA optimization analysis

            //Closing Excel Book
            if (puntero_aplicacion.checkBox2.Checked == true)
            {
                //Simple Brayton power cycle
                xlWorkBook1.SaveAs(textBox2.Text + "SolarPaces2019_Paper_CIT_Optimization_SB_without_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);
            }
            else if (puntero_aplicacion.checkBox2.Checked == false)
            {
                //Recompression Brayton power cycle
                xlWorkBook1.SaveAs(textBox2.Text + "SolarPaces2019_Paper_CIT_Optimization_RC_without_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);
            }

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //When the Optimization analysis Form is loaded the CIT is read from the power-cycle window
        private void RC_without_ReHeating_Optimization_Analysis_Results_Load(object sender, EventArgs e)
        {
            //textBox57.Text = puntero_aplicacion.textBox2.Text;
            //textBox56.Text = Convert.ToString(Convert.ToDouble(puntero_aplicacion.textBox2.Text) + 30);

            textBox1.Text = puntero_aplicacion.textBox37.Text;
        }
    }
}
