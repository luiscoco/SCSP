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
    public partial class SB_with_Three_Recuperators_Two_RC_without_RH_Optimization_Analysis : Form
    {
        SB_with_Three_Recuperators_and_Two_Recompressors_without_ReHeating puntero_aplicacion;

        public SB_with_Three_Recuperators_Two_RC_without_RH_Optimization_Analysis(SB_with_Three_Recuperators_and_Two_Recompressors_without_ReHeating puntero1)
        {
            puntero_aplicacion = puntero1;
            InitializeComponent();
        }

        //Mixtures calculations
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

            //Not optimized UA
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.recomp_frac1 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox93.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_mt2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_mt1 = Convert.ToDouble(puntero_aplicacion.textBox82.Text);
                puntero_aplicacion.dp2_mt2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc1 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox92.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH = new core.RecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac1_list = new List<Double>();
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

                //initial_CIP_value
                if (checkBox6.Checked == true)
                {
                    initial_CIP_value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                }                

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text
                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + ":" + puntero_aplicacion.textBox61.Text;
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
                xlWorkSheet1.Cells[4, 5] = "Rec.Frac_1";
                xlWorkSheet1.Cells[4, 6] = "Rec.Frac_2";
                xlWorkSheet1.Cells[4, 7] = "P_rhx_in(kPa)";
                xlWorkSheet1.Cells[4, 8] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 9] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 11] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 3, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, 0.0, initial_CIP_value });
                    solver.SetUpperBounds(new[] { 1.0, 0.5, 25000.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 0.05, 250.0});

                    var initialValue = new[] { 0.0, 0.0, initial_CIP_value };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.SimpleBrayton_with_Three_Recuperators_Two_RC_without_ReHeating(puntero_aplicacion.luis,
                        ref cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH, puntero_aplicacion.w_dot_net2, 
                        puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2,
                        variables[2], puntero_aplicacion.p_mc_out2, variables[0],
                        variables[1], -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_mt1,
                        -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, 
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_mt2, -puntero_aplicacion.dp2_ht2,
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_mt2, puntero_aplicacion.ua_ht2,
                        puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc1, puntero_aplicacion.eta_rc2, 
                        puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.eta_thermal;
                        puntero_aplicacion.recomp_frac1 = variables[0];
                        puntero_aplicacion.recomp_frac2 = variables[1];
                        puntero_aplicacion.p_mc_in2 = variables[2];
                      
                        puntero_aplicacion.temp21 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[11];

                        puntero_aplicacion.pres21 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.PHX.Q_dot;                   

                        puntero_aplicacion.LT_Q = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac1_list.Add(puntero_aplicacion.recomp_frac1);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac1.ToString());
                        listBox9.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.ua_mt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[7] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[2];
                        double LTR_min_DT_2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[8] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[7] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[3];
                        double HTR_min_DT_2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[6] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac_1
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = puntero_aplicacion.recomp_frac1.ToString();
                        //Rec.Frac_2
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.eff.ToString();
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

                    textBox91.Text = p_mc_in2_list[maxIndex].ToString();
                    textBox90.Text = recomp_frac1_list[maxIndex].ToString();
                    textBox2.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();                   

                    //Copy results as design-point inputs
                    if (checkBox8.Checked == true)
                    {
                        puntero_aplicacion.textBox87.Text = recomp_frac1_list[maxIndex].ToString();
                        puntero_aplicacion.textBox93.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();                       
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_with_Three_Recuperators_Two_RC_without_RH" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
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
                    puntero_aplicacion.luis.core1(puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox31.Text + "," + puntero_aplicacion.comboBox6.Text + "=" + puntero_aplicacion.textBox36.Text + "," + puntero_aplicacion.comboBox7.Text + "=" + puntero_aplicacion.textBox67.Text + "," + puntero_aplicacion.comboBox12.Text + "=" + puntero_aplicacion.textBox68.Text, puntero_aplicacion.category);
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
                puntero_aplicacion.recomp_frac1 = Convert.ToDouble(puntero_aplicacion.textBox87.Text);
                puntero_aplicacion.recomp_frac2 = Convert.ToDouble(puntero_aplicacion.textBox93.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_mt2 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_mt1 = Convert.ToDouble(puntero_aplicacion.textBox82.Text);
                puntero_aplicacion.dp2_mt2 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc1 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox92.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH = new core.RecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_mt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;
                double MT_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac1_list = new List<Double>();
                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> ua_ht_list = new List<Double>();
                List<Double> ua_mt_list = new List<Double>();
                List<Double> ua_lt_list = new List<Double>();

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

                //initial_CIP_value
                if (checkBox6.Checked == true)
                {
                    initial_CIP_value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    initial_CIP_value = puntero_aplicacion.MixtureCriticalPressure;
                }

                xlWorkSheet1.Name = puntero_aplicacion.comboBox2.Text + " Mixture";

                //puntero_aplicacion.comboBox2.Text + "=" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + "=" + puntero_aplicacion.textBox61.Text
                xlWorkSheet1.Cells[1, 1] = puntero_aplicacion.comboBox2.Text + ":" + puntero_aplicacion.textBox60.Text + "," + puntero_aplicacion.comboBox1.Text + ":" + puntero_aplicacion.textBox61.Text;
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
                xlWorkSheet1.Cells[4, 4] = "MT UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "Rec.Frac_1";
                xlWorkSheet1.Cells[4, 7] = "Rec.Frac_2";
                xlWorkSheet1.Cells[4, 8] = "P_rhx_in(kPa)";
                xlWorkSheet1.Cells[4, 9] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 10] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 12] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 13] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 5, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, 0.0, initial_CIP_value, 0.0, 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 1.0, 25000.0, 1.0, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 0.05, 250.0, 0.05, 0.05 });

                    var initialValue = new[] { 0.0, 0.0, initial_CIP_value + 3000.0, 0.5, 0.25 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.SimpleBrayton_with_Three_Recup_Two_RC_without_RH_for_optimization(puntero_aplicacion.luis,
                        ref cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH, puntero_aplicacion.w_dot_net2,
                        puntero_aplicacion.t_mc_in2, puntero_aplicacion.t_t_in2, variables[2], puntero_aplicacion.p_mc_out2, 
                        variables[0], variables[1], -puntero_aplicacion.dp2_lt1, -puntero_aplicacion.dp2_mt1,
                        -puntero_aplicacion.dp2_ht1, -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1,
                        -puntero_aplicacion.dp2_lt2, -puntero_aplicacion.dp2_mt2, -puntero_aplicacion.dp2_ht2,
                        variables[3], variables[4], UA_Total, puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc1,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_t2, puntero_aplicacion.n_sub_hxrs2, 
                        puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.eta_thermal;
                        puntero_aplicacion.recomp_frac1 = variables[0];
                        puntero_aplicacion.recomp_frac2 = variables[1];
                        puntero_aplicacion.p_mc_in2 = variables[2];
                        LT_fraction = variables[3];
                        MT_fraction = variables[4];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_mt2 = UA_Total * MT_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - MT_fraction- LT_fraction);

                        puntero_aplicacion.temp21 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[0];
                        puntero_aplicacion.temp22 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[1];
                        puntero_aplicacion.temp23 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[2];
                        puntero_aplicacion.temp24 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[3];
                        puntero_aplicacion.temp25 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[4];
                        puntero_aplicacion.temp26 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[5];
                        puntero_aplicacion.temp27 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[6];
                        puntero_aplicacion.temp28 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[7];
                        puntero_aplicacion.temp29 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[8];
                        puntero_aplicacion.temp210 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[9];
                        puntero_aplicacion.temp211 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[10];
                        puntero_aplicacion.temp212 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[11];

                        puntero_aplicacion.pres21 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[0];
                        puntero_aplicacion.pres22 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[1];
                        puntero_aplicacion.pres23 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[2];
                        puntero_aplicacion.pres24 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[3];
                        puntero_aplicacion.pres25 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[4];
                        puntero_aplicacion.pres26 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[5];
                        puntero_aplicacion.pres27 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[6];
                        puntero_aplicacion.pres28 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[7];
                        puntero_aplicacion.pres29 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[8];
                        puntero_aplicacion.pres210 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[9];
                        puntero_aplicacion.pres211 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[10];
                        puntero_aplicacion.pres212 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.pres[11];

                        puntero_aplicacion.PHX_Q2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.PHX.Q_dot;

                        puntero_aplicacion.LT_Q = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.eff;

                        puntero_aplicacion.HT_Q = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac1_list.Add(puntero_aplicacion.recomp_frac1);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                        ua_ht_list.Add(puntero_aplicacion.ua_ht2);
                        ua_mt_list.Add(puntero_aplicacion.ua_mt2);
                        ua_lt_list.Add(puntero_aplicacion.ua_lt2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac1.ToString());
                        listBox9.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.ua_mt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());                        

                        double LTR_min_DT_1 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[7] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[2];
                        double LTR_min_DT_2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[8] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[1];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double HTR_min_DT_1 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[7] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[3];
                        double HTR_min_DT_2 = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[6] - cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.temp[4];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //MT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_mt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac_1
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = puntero_aplicacion.recomp_frac1.ToString();
                        //Rec.Frac_2
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac2.ToString();
                        //P_rhx_in(kPa)
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.p_rhx_in2.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = LTR_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = cicloRecompCycle_with_Three_Recuperatos_with_Two_RC_withoutRH.HT.eff.ToString();
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
                    textBox90.Text = recomp_frac1_list[maxIndex].ToString();
                    textBox2.Text = recomp_frac2_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = ua_lt_list[maxIndex].ToString();
                    textBox8.Text = ua_mt_list[maxIndex].ToString();
                    textBox83.Text = ua_ht_list[maxIndex].ToString();

                    //Copy results as design-point inputs
                    if (checkBox8.Checked == true)
                    {
                        puntero_aplicacion.textBox87.Text = recomp_frac1_list[maxIndex].ToString();
                        puntero_aplicacion.textBox93.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();

                        puntero_aplicacion.textBox17.Text = ua_lt_list[maxIndex].ToString();
                        puntero_aplicacion.textBox84.Text = ua_mt_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_ht_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_with_Three_Recuperators_Two_RC_without_RH" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }
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
    }
}
