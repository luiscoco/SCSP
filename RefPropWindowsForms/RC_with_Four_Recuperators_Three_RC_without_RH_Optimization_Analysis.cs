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
    public partial class RC_with_Four_Recuperators_Three_RC_without_RH_Optimization_Analysis : Form
    {
        RC_with_Four_Recuperators_and_Three_Recompressors_without_ReHeating puntero_aplicacion;

        public RC_with_Four_Recuperators_Three_RC_without_RH_Optimization_Analysis(RC_with_Four_Recuperators_and_Three_Recompressors_without_ReHeating puntero1)
        {
            puntero_aplicacion = puntero1;
            InitializeComponent();
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
                puntero_aplicacion.recomp_frac3 = Convert.ToDouble(puntero_aplicacion.textBox105.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_mt1 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.ua_mt2 = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_mt11 = Convert.ToDouble(puntero_aplicacion.textBox82.Text);
                puntero_aplicacion.dp2_mt12 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.dp2_mt21 = Convert.ToDouble(puntero_aplicacion.textBox94.Text);
                puntero_aplicacion.dp2_mt22 = Convert.ToDouble(puntero_aplicacion.textBox95.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc1 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox92.Text);
                puntero_aplicacion.eta_rc3 = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH = new core.RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_mt1 + puntero_aplicacion.ua_mt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;
                double MT1_fraction = 0.1;
                double MT2_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac1_list = new List<Double>();
                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> recomp_frac3_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 4] = "MT1 UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "MT2 UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "Rec.Frac_1";
                xlWorkSheet1.Cells[4, 8] = "Rec.Frac_2";
                xlWorkSheet1.Cells[4, 9] = "Rec.Frac_3";
                xlWorkSheet1.Cells[4, 10] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 13] = "MTR1 Eff.(%)";
                xlWorkSheet1.Cells[4, 14] = "MTR1 Pinch(ºC)";
                xlWorkSheet1.Cells[4, 15] = "MTR2 Eff.(%)";
                xlWorkSheet1.Cells[4, 16] = "MTR2 Pinch(ºC)";
                xlWorkSheet1.Cells[4, 17] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 18] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 4, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, 0.0, 0.0, initial_CIP_value });
                    solver.SetUpperBounds(new[] { 1.0, 1.0, 1.0, 25000.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 0.05, 0.05, 250.0 });

                    var initialValue = new[] { 0.0, 0.0, 0.0, initial_CIP_value };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.SimpleBrayton_with_Four_Recup_Three_RC_without_RH(puntero_aplicacion.luis, 
                        ref ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH, 
                        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2, 
                        puntero_aplicacion.t_t_in2, variables[3], puntero_aplicacion.p_mc_out2,
                        variables[0], variables[1], variables[2], -puntero_aplicacion.dp2_lt1, 
                        -puntero_aplicacion.dp2_mt11, -puntero_aplicacion.dp2_mt21, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2,
                        -puntero_aplicacion.dp2_mt12, -puntero_aplicacion.dp2_mt22, -puntero_aplicacion.dp2_ht2,
                        puntero_aplicacion.ua_lt2, puntero_aplicacion.ua_mt1, puntero_aplicacion.ua_mt2, 
                        puntero_aplicacion.ua_ht2, puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc1, 
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_rc3, puntero_aplicacion.eta_t2, 
                        puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.eta_thermal;
                        puntero_aplicacion.recomp_frac1 = variables[0];
                        puntero_aplicacion.recomp_frac2 = variables[1];
                        puntero_aplicacion.recomp_frac3 = variables[2];
                        puntero_aplicacion.p_mc_in2 = variables[3];

                        puntero_aplicacion.temp21 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[0];
                        puntero_aplicacion.temp22 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[1];
                        puntero_aplicacion.temp23 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[2];
                        puntero_aplicacion.temp24 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[3];
                        puntero_aplicacion.temp25 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[4];
                        puntero_aplicacion.temp26 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[5];
                        puntero_aplicacion.temp27 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[6];
                        puntero_aplicacion.temp28 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[7];
                        puntero_aplicacion.temp29 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[8];
                        puntero_aplicacion.temp210 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[9];
                        puntero_aplicacion.temp211 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[10];
                        puntero_aplicacion.temp212 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[11];
                        puntero_aplicacion.temp213 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[12];
                        puntero_aplicacion.temp214 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[13];
                        puntero_aplicacion.temp215 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[14];
                        puntero_aplicacion.temp216 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[15];
                        puntero_aplicacion.temp217 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[16];
                        puntero_aplicacion.temp218 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[17];
                        
                        puntero_aplicacion.pres21 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[0];
                        puntero_aplicacion.pres22 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[1];
                        puntero_aplicacion.pres23 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[2];
                        puntero_aplicacion.pres24 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[3];
                        puntero_aplicacion.pres25 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[4];
                        puntero_aplicacion.pres26 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[5];
                        puntero_aplicacion.pres27 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[6];
                        puntero_aplicacion.pres28 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[7];
                        puntero_aplicacion.pres29 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[8];
                        puntero_aplicacion.pres210 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[9];
                        puntero_aplicacion.pres211 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[10];
                        puntero_aplicacion.pres212 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[11];
                        puntero_aplicacion.pres213 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[12];
                        puntero_aplicacion.pres214 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[13];
                        puntero_aplicacion.pres215 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[14];
                        puntero_aplicacion.pres216 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[15];
                        puntero_aplicacion.pres217 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[16];
                        puntero_aplicacion.pres218 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[17];

                        puntero_aplicacion.PHX_Q2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.PHX.Q_dot;

                        puntero_aplicacion.LT_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.eff;

                        puntero_aplicacion.MT1_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.Q_dot;
                        puntero_aplicacion.MT1_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.m_dot_design[0];
                        puntero_aplicacion.MT1_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.m_dot_design[1];
                        puntero_aplicacion.MT1_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.T_c_in;
                        puntero_aplicacion.MT1_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.T_h_in;
                        puntero_aplicacion.MT1_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_c_in;
                        puntero_aplicacion.MT1_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_h_in;
                        puntero_aplicacion.MT1_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_c_out;
                        puntero_aplicacion.MT1_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_h_out;
                        puntero_aplicacion.MT1_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.eff;

                        puntero_aplicacion.MT2_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.Q_dot;
                        puntero_aplicacion.MT2_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.m_dot_design[0];
                        puntero_aplicacion.MT2_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.m_dot_design[1];
                        puntero_aplicacion.MT2_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.T_c_in;
                        puntero_aplicacion.MT2_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.T_h_in;
                        puntero_aplicacion.MT2_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_c_in;
                        puntero_aplicacion.MT2_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_h_in;
                        puntero_aplicacion.MT2_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_c_out;
                        puntero_aplicacion.MT2_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_h_out;
                        puntero_aplicacion.MT2_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.eff;

                        puntero_aplicacion.HT_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac1_list.Add(puntero_aplicacion.recomp_frac1);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        recomp_frac3_list.Add(puntero_aplicacion.recomp_frac3);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac1.ToString());
                        listBox9.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.recomp_frac3.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.ua_mt1.ToString());
                        listBox22.Items.Add(puntero_aplicacion.ua_mt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[15] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[2];
                        double LTR_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[14] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[3];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double MTR1_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[14] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[4];
                        double MTR1_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[13] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[5];
                        double MTR1_min_DT_paper = Math.Min(MTR1_min_DT_1, MTR1_min_DT_2);

                        double MTR2_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[13] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[6];
                        double MTR2_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[12] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[7];
                        double MTR2_min_DT_paper = Math.Min(MTR2_min_DT_1, MTR2_min_DT_2);

                        double HTR_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[12] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[8];
                        double HTR_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[11] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[9];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //MT1 UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_mt1);
                        //MT2 UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_mt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac_1
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac1.ToString();
                        //Rec.Frac_2
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.recomp_frac2.ToString();
                        //Rec.Frac_3
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.recomp_frac3.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                        //MTR1 Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.eff.ToString();
                        //MTR1 Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = MTR1_min_DT_paper.ToString();
                        //MTR2 Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 15] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.eff.ToString();
                        //MTR2 Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 16] = MTR2_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 17] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 18] = HTR_min_DT_paper.ToString();

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
                    textBox9.Text = recomp_frac3_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox8.Text = puntero_aplicacion.ua_mt1.ToString();
                    textBox10.Text = puntero_aplicacion.ua_mt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();

                    //Copy results as design-point inputs
                    if (checkBox8.Checked == true)
                    {
                        puntero_aplicacion.textBox87.Text = recomp_frac1_list[maxIndex].ToString();
                        puntero_aplicacion.textBox93.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox105.Text = recomp_frac3_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_with_Four_Recuperators_Three_RC_without_RH" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

                    xlWorkBook1.Close(true, misValue1, misValue1);
                    xlApp1.Quit();

                    releaseObject(xlWorkSheet1);
                    //releaseObject(xlWorkSheet2);
                    releaseObject(xlWorkBook1);
                    releaseObject(xlApp1);
                }
            }

            //Not optimized UA
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
                puntero_aplicacion.recomp_frac3 = Convert.ToDouble(puntero_aplicacion.textBox105.Text);
                puntero_aplicacion.ua_lt2 = Convert.ToDouble(puntero_aplicacion.textBox17.Text);
                puntero_aplicacion.ua_mt1 = Convert.ToDouble(puntero_aplicacion.textBox84.Text);
                puntero_aplicacion.ua_mt2 = Convert.ToDouble(puntero_aplicacion.textBox104.Text);
                puntero_aplicacion.ua_ht2 = Convert.ToDouble(puntero_aplicacion.textBox16.Text);

                puntero_aplicacion.dp2_lt1 = Convert.ToDouble(puntero_aplicacion.textBox5.Text);
                puntero_aplicacion.dp2_lt2 = Convert.ToDouble(puntero_aplicacion.textBox26.Text);
                puntero_aplicacion.dp2_mt11 = Convert.ToDouble(puntero_aplicacion.textBox82.Text);
                puntero_aplicacion.dp2_mt12 = Convert.ToDouble(puntero_aplicacion.textBox83.Text);
                puntero_aplicacion.dp2_mt21 = Convert.ToDouble(puntero_aplicacion.textBox94.Text);
                puntero_aplicacion.dp2_mt22 = Convert.ToDouble(puntero_aplicacion.textBox95.Text);
                puntero_aplicacion.dp2_ht1 = Convert.ToDouble(puntero_aplicacion.textBox12.Text);
                puntero_aplicacion.dp2_ht2 = Convert.ToDouble(puntero_aplicacion.textBox25.Text);
                puntero_aplicacion.dp2_pc2 = Convert.ToDouble(puntero_aplicacion.textBox11.Text);
                puntero_aplicacion.dp2_phx1 = Convert.ToDouble(puntero_aplicacion.textBox10.Text);

                puntero_aplicacion.eta_mc2 = Convert.ToDouble(puntero_aplicacion.textBox14.Text);
                puntero_aplicacion.eta_rc1 = Convert.ToDouble(puntero_aplicacion.textBox86.Text);
                puntero_aplicacion.eta_rc2 = Convert.ToDouble(puntero_aplicacion.textBox92.Text);
                puntero_aplicacion.eta_rc3 = Convert.ToDouble(puntero_aplicacion.textBox106.Text);
                puntero_aplicacion.eta_t2 = Convert.ToDouble(puntero_aplicacion.textBox19.Text);
                puntero_aplicacion.n_sub_hxrs2 = Convert.ToInt64(puntero_aplicacion.textBox20.Text);
                puntero_aplicacion.tol2 = Convert.ToDouble(puntero_aplicacion.textBox21.Text);

                puntero_aplicacion.luis.wmm = puntero_aplicacion.luis.working_fluid.MolecularWeight;

                core.RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH = new core.RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH();

                double UA_Total = puntero_aplicacion.ua_lt2 + puntero_aplicacion.ua_mt1 + puntero_aplicacion.ua_mt2 + puntero_aplicacion.ua_ht2;

                double LT_fraction = 0.1;
                double MT1_fraction = 0.1;
                double MT2_fraction = 0.1;

                int counter = 0;

                List<Double> recomp_frac1_list = new List<Double>();
                List<Double> recomp_frac2_list = new List<Double>();
                List<Double> recomp_frac3_list = new List<Double>();
                List<Double> p_mc_in2_list = new List<Double>();
                List<Double> eta_thermal2_list = new List<Double>();
                List<Double> ua_ht_list = new List<Double>();
                List<Double> ua_mt1_list = new List<Double>();
                List<Double> ua_mt2_list = new List<Double>();
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
                xlWorkSheet1.Cells[4, 4] = "MT1 UA(kW/K)";
                xlWorkSheet1.Cells[4, 5] = "MT2 UA(kW/K)";
                xlWorkSheet1.Cells[4, 6] = "HT UA(kW/K)";
                xlWorkSheet1.Cells[4, 7] = "Rec.Frac_1";
                xlWorkSheet1.Cells[4, 8] = "Rec.Frac_2";
                xlWorkSheet1.Cells[4, 9] = "Rec.Frac_3";
                xlWorkSheet1.Cells[4, 10] = "Eff.(%)";
                xlWorkSheet1.Cells[4, 11] = "LTR Eff.(%)";
                xlWorkSheet1.Cells[4, 12] = "LTR Pinch(ºC)";
                xlWorkSheet1.Cells[4, 13] = "MTR1 Eff.(%)";
                xlWorkSheet1.Cells[4, 14] = "MTR1 Pinch(ºC)";
                xlWorkSheet1.Cells[4, 15] = "MTR2 Eff.(%)";
                xlWorkSheet1.Cells[4, 16] = "MTR2 Pinch(ºC)";
                xlWorkSheet1.Cells[4, 17] = "HTR Eff.(%)";
                xlWorkSheet1.Cells[4, 18] = "HTR Pinch(ºC)";

                using (var solver = new NLoptSolver(algorithm_type, 7, 0.000001, 10000))
                {
                    solver.SetLowerBounds(new[] { 0.0, 0.0, 0.0, initial_CIP_value, 0.0, 0.0, 0.0 });
                    solver.SetUpperBounds(new[] { 1.0, 1.0, 1.0, 25000.0, 1.0, 1.0, 1.0 });

                    solver.SetInitialStepSize(new[] { 0.05, 0.05, 0.05, 250.0, 0.05, 0.05, 0.05 });

                    var initialValue = new[] { 0.0, 0.0, 0.0, initial_CIP_value, 0.25, 0.25, 0.25 };

                    Func<double[], double> funcion = delegate (double[] variables)
                    {
                        puntero_aplicacion.luis.SimpleBrayton_with_Four_Recup_Three_RC_without_RH_for_optimization(puntero_aplicacion.luis,
                        ref ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH,
                        puntero_aplicacion.w_dot_net2, puntero_aplicacion.t_mc_in2,
                        puntero_aplicacion.t_t_in2, variables[3], puntero_aplicacion.p_mc_out2,
                        variables[0], variables[1], variables[2], -puntero_aplicacion.dp2_lt1,
                        -puntero_aplicacion.dp2_mt11, -puntero_aplicacion.dp2_mt21, -puntero_aplicacion.dp2_ht1,
                        -puntero_aplicacion.dp2_pc2, -puntero_aplicacion.dp2_phx1, -puntero_aplicacion.dp2_lt2,
                        -puntero_aplicacion.dp2_mt12, -puntero_aplicacion.dp2_mt22, -puntero_aplicacion.dp2_ht2,
                         variables[4], variables[5], variables[6], UA_Total,
                        puntero_aplicacion.eta_mc2, puntero_aplicacion.eta_rc1,
                        puntero_aplicacion.eta_rc2, puntero_aplicacion.eta_rc3, puntero_aplicacion.eta_t2,
                        puntero_aplicacion.n_sub_hxrs2, puntero_aplicacion.tol2);

                        counter++;

                        puntero_aplicacion.massflow2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.m_dot_turbine;
                        puntero_aplicacion.w_dot_net2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.W_dot_net;
                        puntero_aplicacion.eta_thermal2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.eta_thermal;
                        puntero_aplicacion.recomp_frac1 = variables[0];
                        puntero_aplicacion.recomp_frac2 = variables[1];
                        puntero_aplicacion.recomp_frac3 = variables[2];
                        puntero_aplicacion.p_mc_in2 = variables[3];
                        LT_fraction = variables[4];
                        MT1_fraction = variables[5];
                        MT2_fraction = variables[6];
                        puntero_aplicacion.ua_lt2 = UA_Total * LT_fraction;
                        puntero_aplicacion.ua_mt1 = UA_Total * MT1_fraction;
                        puntero_aplicacion.ua_mt2 = UA_Total * MT2_fraction;
                        puntero_aplicacion.ua_ht2 = UA_Total * (1 - MT1_fraction - MT2_fraction - LT_fraction);

                        puntero_aplicacion.temp21 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[0];
                        puntero_aplicacion.temp22 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[1];
                        puntero_aplicacion.temp23 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[2];
                        puntero_aplicacion.temp24 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[3];
                        puntero_aplicacion.temp25 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[4];
                        puntero_aplicacion.temp26 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[5];
                        puntero_aplicacion.temp27 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[6];
                        puntero_aplicacion.temp28 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[7];
                        puntero_aplicacion.temp29 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[8];
                        puntero_aplicacion.temp210 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[9];
                        puntero_aplicacion.temp211 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[10];
                        puntero_aplicacion.temp212 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[11];
                        puntero_aplicacion.temp213 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[12];
                        puntero_aplicacion.temp214 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[13];
                        puntero_aplicacion.temp215 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[14];
                        puntero_aplicacion.temp216 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[15];
                        puntero_aplicacion.temp217 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[16];
                        puntero_aplicacion.temp218 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[17];

                        puntero_aplicacion.pres21 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[0];
                        puntero_aplicacion.pres22 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[1];
                        puntero_aplicacion.pres23 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[2];
                        puntero_aplicacion.pres24 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[3];
                        puntero_aplicacion.pres25 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[4];
                        puntero_aplicacion.pres26 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[5];
                        puntero_aplicacion.pres27 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[6];
                        puntero_aplicacion.pres28 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[7];
                        puntero_aplicacion.pres29 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[8];
                        puntero_aplicacion.pres210 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[9];
                        puntero_aplicacion.pres211 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[10];
                        puntero_aplicacion.pres212 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[11];
                        puntero_aplicacion.pres213 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[12];
                        puntero_aplicacion.pres214 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[13];
                        puntero_aplicacion.pres215 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[14];
                        puntero_aplicacion.pres216 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[15];
                        puntero_aplicacion.pres217 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[16];
                        puntero_aplicacion.pres218 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.pres[17];

                        puntero_aplicacion.PHX_Q2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.PHX.Q_dot;

                        puntero_aplicacion.LT_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.Q_dot;
                        puntero_aplicacion.LT_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.m_dot_design[0];
                        puntero_aplicacion.LT_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.m_dot_design[1];
                        puntero_aplicacion.LT_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.T_c_in;
                        puntero_aplicacion.LT_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.T_h_in;
                        puntero_aplicacion.LT_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_c_in;
                        puntero_aplicacion.LT_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_h_in;
                        puntero_aplicacion.LT_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_c_out;
                        puntero_aplicacion.LT_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.P_h_out;
                        puntero_aplicacion.LT_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.eff;

                        puntero_aplicacion.MT1_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.Q_dot;
                        puntero_aplicacion.MT1_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.m_dot_design[0];
                        puntero_aplicacion.MT1_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.m_dot_design[1];
                        puntero_aplicacion.MT1_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.T_c_in;
                        puntero_aplicacion.MT1_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.T_h_in;
                        puntero_aplicacion.MT1_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_c_in;
                        puntero_aplicacion.MT1_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_h_in;
                        puntero_aplicacion.MT1_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_c_out;
                        puntero_aplicacion.MT1_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.P_h_out;
                        puntero_aplicacion.MT1_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.eff;

                        puntero_aplicacion.MT2_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.Q_dot;
                        puntero_aplicacion.MT2_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.m_dot_design[0];
                        puntero_aplicacion.MT2_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.m_dot_design[1];
                        puntero_aplicacion.MT2_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.T_c_in;
                        puntero_aplicacion.MT2_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.T_h_in;
                        puntero_aplicacion.MT2_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_c_in;
                        puntero_aplicacion.MT2_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_h_in;
                        puntero_aplicacion.MT2_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_c_out;
                        puntero_aplicacion.MT2_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.P_h_out;
                        puntero_aplicacion.MT2_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.eff;

                        puntero_aplicacion.HT_Q = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.Q_dot;
                        puntero_aplicacion.HT_mdotc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.m_dot_design[0];
                        puntero_aplicacion.HT_mdoth = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.m_dot_design[1];
                        puntero_aplicacion.HT_Tcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.T_c_in;
                        puntero_aplicacion.HT_Thin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.T_h_in;
                        puntero_aplicacion.HT_Pcin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_c_in;
                        puntero_aplicacion.HT_Phin = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_h_in;
                        puntero_aplicacion.HT_Pcout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_c_out;
                        puntero_aplicacion.HT_Phout = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.P_h_out;
                        puntero_aplicacion.HT_Effc = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.eff;

                        puntero_aplicacion.PC_Q2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.PC.Q_dot;

                        eta_thermal2_list.Add(puntero_aplicacion.eta_thermal2);
                        recomp_frac1_list.Add(puntero_aplicacion.recomp_frac1);
                        recomp_frac2_list.Add(puntero_aplicacion.recomp_frac2);
                        recomp_frac3_list.Add(puntero_aplicacion.recomp_frac3);
                        p_mc_in2_list.Add(puntero_aplicacion.p_mc_in2);
                        ua_ht_list.Add(puntero_aplicacion.ua_ht2);
                        ua_mt1_list.Add(puntero_aplicacion.ua_mt1);
                        ua_mt2_list.Add(puntero_aplicacion.ua_mt2);
                        ua_lt_list.Add(puntero_aplicacion.ua_lt2);

                        listBox1.Items.Add(counter.ToString());
                        listBox2.Items.Add(puntero_aplicacion.eta_thermal2.ToString());
                        listBox3.Items.Add(puntero_aplicacion.recomp_frac1.ToString());
                        listBox9.Items.Add(puntero_aplicacion.recomp_frac2.ToString());
                        listBox21.Items.Add(puntero_aplicacion.recomp_frac3.ToString());
                        listBox4.Items.Add(puntero_aplicacion.p_mc_in2.ToString());
                        listBox5.Items.Add(puntero_aplicacion.ua_lt2.ToString());
                        listBox19.Items.Add(puntero_aplicacion.ua_mt1.ToString());
                        listBox22.Items.Add(puntero_aplicacion.ua_mt2.ToString());
                        listBox6.Items.Add(puntero_aplicacion.ua_ht2.ToString());
                        listBox7.Items.Add(puntero_aplicacion.temp27.ToString());
                        listBox8.Items.Add(puntero_aplicacion.temp28.ToString());

                        double LTR_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[15] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[2];
                        double LTR_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[14] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[3];
                        double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                        double MTR1_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[14] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[4];
                        double MTR1_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[13] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[5];
                        double MTR1_min_DT_paper = Math.Min(MTR1_min_DT_1, MTR1_min_DT_2);

                        double MTR2_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[13] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[6];
                        double MTR2_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[12] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[7];
                        double MTR2_min_DT_paper = Math.Min(MTR2_min_DT_1, MTR2_min_DT_2);

                        double HTR_min_DT_1 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[12] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[8];
                        double HTR_min_DT_2 = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[11] - ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.temp[9];
                        double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                        //CIP
                        xlWorkSheet1.Cells[counter_Excel + 1, 1] = Convert.ToString(puntero_aplicacion.p_mc_in2);
                        //CIT
                        xlWorkSheet1.Cells[counter_Excel + 1, 2] = Convert.ToString(puntero_aplicacion.t_mc_in2 - 273.15);
                        //LT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 3] = Convert.ToString(puntero_aplicacion.ua_lt2);
                        //MT1 UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 4] = Convert.ToString(puntero_aplicacion.ua_mt1);
                        //MT2 UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 5] = Convert.ToString(puntero_aplicacion.ua_mt2);
                        //HT UA(kW/K)
                        xlWorkSheet1.Cells[counter_Excel + 1, 6] = Convert.ToString(puntero_aplicacion.ua_ht2);
                        //Rec.Frac_1
                        xlWorkSheet1.Cells[counter_Excel + 1, 7] = puntero_aplicacion.recomp_frac1.ToString();
                        //Rec.Frac_2
                        xlWorkSheet1.Cells[counter_Excel + 1, 8] = puntero_aplicacion.recomp_frac2.ToString();
                        //Rec.Frac_3
                        xlWorkSheet1.Cells[counter_Excel + 1, 9] = puntero_aplicacion.recomp_frac3.ToString();
                        //Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 10] = (puntero_aplicacion.eta_thermal2 * 100).ToString();
                        //LTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 11] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.LT.eff.ToString();
                        //LTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 12] = LTR_min_DT_paper.ToString();
                        //MTR1 Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 13] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT1.eff.ToString();
                        //MTR1 Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 14] = MTR1_min_DT_paper.ToString();
                        //MTR2 Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 15] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.MT2.eff.ToString();
                        //MTR2 Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 16] = MTR2_min_DT_paper.ToString();
                        //HTR Eff.(%)
                        xlWorkSheet1.Cells[counter_Excel + 1, 17] = ciclo_RecompCycle_with_Four_Recuperatos_with_Three_RC_withoutRH.HT.eff.ToString();
                        //HTR Pinch(ºC)
                        xlWorkSheet1.Cells[counter_Excel + 1, 18] = HTR_min_DT_paper.ToString();

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
                    textBox9.Text = recomp_frac3_list[maxIndex].ToString();
                    textBox86.Text = eta_thermal2_list[maxIndex].ToString();
                    textBox82.Text = puntero_aplicacion.ua_lt2.ToString();
                    textBox8.Text = puntero_aplicacion.ua_mt1.ToString();
                    textBox10.Text = puntero_aplicacion.ua_mt2.ToString();
                    textBox83.Text = puntero_aplicacion.ua_ht2.ToString();

                    //Copy results as design-point inputs
                    if (checkBox8.Checked == true)
                    {
                        puntero_aplicacion.textBox87.Text = recomp_frac1_list[maxIndex].ToString();
                        puntero_aplicacion.textBox93.Text = recomp_frac2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox105.Text = recomp_frac3_list[maxIndex].ToString();
                        puntero_aplicacion.textBox3.Text = p_mc_in2_list[maxIndex].ToString();
                        
                        puntero_aplicacion.textBox17.Text = ua_lt_list[maxIndex].ToString();
                        puntero_aplicacion.textBox84.Text = ua_mt1_list[maxIndex].ToString();
                        puntero_aplicacion.textBox104.Text = ua_mt2_list[maxIndex].ToString();
                        puntero_aplicacion.textBox16.Text = ua_ht_list[maxIndex].ToString();
                    }

                    //Closing Excel Book
                    xlWorkBook1.SaveAs(textBox3.Text + "RC_with_Four_Recuperators_Three_RC_without_RH" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

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

        //Run CIT Optimization
        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
