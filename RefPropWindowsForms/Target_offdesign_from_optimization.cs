using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using System.Data.Common;
using System.Threading;
using System.Text;

using sc.net;

namespace RefPropWindowsForms
{
    public partial class Target_offdesign_from_optimization : Form
    {
        public core luis = new core();

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;
        public Radial_Turbine ReHeating_Turbine;

        //First calculate the Main Compressor Rotational speed and after send that value to the Turbines
        public Double N_design_Main_Compressor;

        public snl_compressor_tsr Main_Compressor;
        public snl_compressor_tsr ReCompressor;

        //Input Data:
        public RefrigerantCategory category;
        public ReferenceState referencestate;

        //Thermal Efficiency
        public Double eta_optimum;

        //Input Data:
        public Double W_dot_net7, T_mc_in7, T_t_in7, P_mc_in7, P_mc_out_guess7, p_high_limit7;
        public Boolean Fixed_recomp_frac7, fixed_LT_frac_guess7, fixed_P_mc_out_guess7, fixed_PR_mc_guess7;
        public Double UA_Total7, recomp_frac_guess7, LT_frac_guess7, eta_mc7, eta_rc7, eta_t7, PR_mc_guess7, opt_tol7;
        public Double DP_LT_c7;
        public Double DP_HT_c7;
        public Double DP_PC7;
        public Double DP_PHX7;
        public Double DP_LT_h7;
        public Double DP_HT_h7;
        public Int64 N_sub_hxrs7;
        public Double tol7;
        public Int64 Error_code;
        public core.RecompCycle recomp_cycle = new core.RecompCycle();

        //Parameters
        public Int64 max_iter = 10;
        public Double temperature_tolerance = 1.0e-6;  // temperature differences below this are considered zero

        //Local Variables
        public Int64 error_code, index;
        public Double w_mc, w_rc, w_t, w_trh, C_dot_min, Q_dot_max;
        public Double T9_lower_bound, T9_upper_bound, T8_lower_bound, T8_upper_bound, last_LT_residual, last_T9_guess;
        public Double last_HT_residual, last_T8_guess, secant_guess;
        public Double m_dot_t, m_dot_mc, m_dot_rc, eta_mc_isen, eta_rc_isen, eta_t_isen;
        public Double min_DT_LT, min_DT_HT, UA_LT_calc, UA_HT_calc, Q_dot_LT, Q_dot_HT, UA_HT_residual, UA_LT_residual;
        public Double[] temp = new Double[10];
        public Double[] pres = new Double[10];
        public Double[] enth = new Double[10];
        public Double[] entr = new Double[10];
        public Double[] dens = new Double[10];

        public Double wmm;

        const string refpropDLL_path1 = "CO2_optimal_withoutReHeat.dll";
        [DllImport(refpropDLL_path1, EntryPoint = "carbondioxide_", SetLastError = true)]
        public static extern void carbondioxide_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref Int64 n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design);

        const string refpropDLL_path2 = "CO2_target_offdesign.dll";
        [DllImport(refpropDLL_path2, EntryPoint = "carbondioxidetarget_", SetLastError = true)]
        public static extern void carbondioxidetarget_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_mc_in1, ref Double p_mc_out1,
                                                 ref Double ua_lt1, ref Double ua_ht1, ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1,
                                                 ref Int64 n_sub_hxrs1, ref Double recomp_frac1, ref Double tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1, ref Double dp_pc2,
                                                 ref Double dp_phx1, ref Double dp_phx2, ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4,
                                                 ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9, ref Double temp10,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6, ref Double pres7,
                                                 ref Double pres8, ref Double pres9, ref Double pres10, ref Double m_dot_turbine1, ref Double LT_mdoth, ref Double LT_mdotc,
                                                 ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                 ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin,
                                                 ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA,
                                                 ref Double LT_Effc, ref Double HT_Effc, ref Double N_mc_design, ref Double t_mc_in_off, ref Double t_t_in_off,
                                                 ref Double recomp_frac_off, ref Double n_mc_off, ref Double n_t_off, ref Double target_off, ref Int64 target_code_off,
                                                 ref Double lowest_pressure_off, ref Double highest_pressure_off);


        public Target_offdesign_from_optimization()
        {
            InitializeComponent();
        }

        public Double w_dot_net2;
        public Double t_mc_in2;
        public Double p_mc_in2;
        public Double p_mc_out2;
        public Double t_t_in2;
        public Double ua_rec_total2;
        public Double ua_lt2, ua_ht2;
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_t2;
        public Int64 n_sub_hxrs2;
        public Double p_high_limit2;
        public Double p_mc_out_guess2;
        public Boolean fixed_p_mc_out2;
        public Double pr_mc_guess2;
        public Boolean fixed_pr_mc2;
        public Double recomp_frac_guess2;
        public Double recomp_frac2;
        public Boolean fixed_recomp_frac2;
        public Double lt_frac_guess2;
        public Boolean fixed_lt_frac2;
        public Double tol2;
        public Double opt_tol2;
        public Double eta_thermal2;

        public Double dp2_lt1, dp2_lt2;
        public Double dp2_ht1, dp2_ht2;
        public Double dp2_pc1, dp2_pc2;
        public Double dp2_phx1, dp2_phx2;

        public Double t_mc_in_off2, t_t_in_off2;
        public Double p_mc_in_off2, recomp_frac_off2;
        public Double n_mc_off2, n_t_off2;
        public Double N_mc_design2;

        public Double temp21;
        public Double temp22;
        public Double temp23;
        public Double temp24;
        public Double temp25;
        public Double temp26;
        public Double temp27;
        public Double temp28;
        public Double temp29;
        public Double temp210;

        public Double pres21;
        public Double pres22;
        public Double pres23;
        public Double pres24;
        public Double pres25;
        public Double pres26;
        public Double pres27;
        public Double pres28;
        public Double pres29;
        public Double pres210;

        public Double enth21;
        public Double enth22;
        public Double enth23;
        public Double enth24;
        public Double enth25;
        public Double enth26;
        public Double enth27;
        public Double enth28;
        public Double enth29;
        public Double enth210;

        public Double entr21;
        public Double entr22;
        public Double entr23;
        public Double entr24;
        public Double entr25;
        public Double entr26;
        public Double entr27;
        public Double entr28;
        public Double entr29;
        public Double entr210;

        public Double massflow2;
        public Double LT_mdoth, LT_mdotc, LT_Tcin, LT_Thin, LT_Pcin, LT_Phin;
        public Double LT_Pcout, LT_Phout, LT_Q, HT_mdoth, HT_mdotc, HT_Tcin, HT_Thin;
        public Double HT_Pcin, HT_Phin, HT_Pcout, HT_Phout, HT_Q, LT_UA, HT_UA;
        public Double LT_Effc, HT_Effc;

        public Double target_off2;
        public Int64 target_code_off2;
        public Double lowest_pressure_off2;
        public Double highest_pressure_off2;

        public Double N_design2;


        // Optimization Button
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
            }
            if (comboBox1.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
            }
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
            }
            if (comboBox1.Text == "PseudoPureFluid")
            {
                category = RefrigerantCategory.PseudoPureFluid;
            }

            if (comboBox3.Text == "DEF")
            {
                referencestate = ReferenceState.DEF;
            }
            if (comboBox3.Text == "ASH")
            {
                referencestate = ReferenceState.ASH;
            }
            if (comboBox3.Text == "IIR")
            {
                referencestate = ReferenceState.IIR;
            }
            if (comboBox3.Text == "NBP")
            {
                referencestate = ReferenceState.NBP;
            }

            luis.core1(this.comboBox2.Text, category);
            luis.working_fluid.Category = category;
            luis.working_fluid.reference = referencestate;

            w_dot_net2 = Convert.ToDouble(textBox1.Text);
            t_mc_in2 = Convert.ToDouble(textBox2.Text);
            t_t_in2 = Convert.ToDouble(textBox4.Text);
            ua_rec_total2 = Convert.ToDouble(textBox17.Text);
            eta_mc2 = Convert.ToDouble(textBox14.Text);
            eta_rc2 = Convert.ToDouble(textBox13.Text);
            eta_t2 = Convert.ToDouble(textBox19.Text);
            n_sub_hxrs2 = Convert.ToInt16(textBox20.Text);
            p_high_limit2 = Convert.ToDouble(textBox57.Text);
            p_mc_out_guess2 = Convert.ToDouble(textBox7.Text);
            fixed_p_mc_out2 = Convert.ToBoolean(textBox58.Text);
            pr_mc_guess2 = Convert.ToDouble(textBox60.Text);
            fixed_pr_mc2 = Convert.ToBoolean(textBox59.Text);
            recomp_frac_guess2 = Convert.ToDouble(textBox16.Text);
            fixed_recomp_frac2 = Convert.ToBoolean(textBox15.Text);
            lt_frac_guess2 = Convert.ToDouble(textBox62.Text);
            fixed_lt_frac2 = Convert.ToBoolean(textBox61.Text);
            tol2 = Convert.ToDouble(textBox21.Text);
            opt_tol2 = Convert.ToDouble(textBox63.Text);

            dp2_lt1 = Convert.ToDouble(textBox5.Text);
            dp2_lt2 = Convert.ToDouble(textBox26.Text);
            dp2_ht1 = Convert.ToDouble(textBox12.Text);
            dp2_ht2 = Convert.ToDouble(textBox25.Text);
            dp2_pc1 = 0.0;
            dp2_pc2 = Convert.ToDouble(textBox11.Text);
            dp2_phx1 = Convert.ToDouble(textBox10.Text);
            dp2_phx2 = 0.0;

            if (comboBox2.Text == "CO2")
            {
                carbondioxide_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_mc_design2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                textBox16.Text = Convert.ToString(recomp_frac_guess2);
                textBox16.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(pres26);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

                textBox52.Text = Convert.ToString(LT_UA);
                textBox52.BackColor = Color.Yellow;

                textBox53.Text = Convert.ToString(HT_UA);
                textBox53.BackColor = Color.Yellow;

                textBox31.Text = Convert.ToString(N_mc_design2);
                textBox31.BackColor = Color.Yellow;

                textBox50.Text = Convert.ToString(eta_thermal2);
                textBox47.Text = Convert.ToString(temp21);
                textBox46.Text = Convert.ToString(temp22);
                textBox45.Text = Convert.ToString(temp23);
                textBox44.Text = Convert.ToString(temp24);
                textBox43.Text = Convert.ToString(temp25);
                textBox42.Text = Convert.ToString(temp26);
                textBox35.Text = Convert.ToString(temp27);
                textBox34.Text = Convert.ToString(temp28);
                textBox33.Text = Convert.ToString(temp29);
                textBox32.Text = Convert.ToString(temp210);

                textBox22.Text = Convert.ToString(pres21);
                textBox23.Text = Convert.ToString(pres22);
                textBox27.Text = Convert.ToString(pres23);
                textBox24.Text = Convert.ToString(pres24);
                textBox29.Text = Convert.ToString(pres25);
                textBox28.Text = Convert.ToString(pres26);
                textBox41.Text = Convert.ToString(pres27);
                textBox40.Text = Convert.ToString(pres28);
                textBox39.Text = Convert.ToString(pres29);
                textBox38.Text = Convert.ToString(pres210);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state;

                luis.working_fluid.FindStateWithTP(temp21, pres21);
                enth21 = luis.working_fluid.Enthalpy;
                entr21 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp22, pres22);
                enth22 = luis.working_fluid.Enthalpy;
                entr22 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp23, pres23);
                enth23 = luis.working_fluid.Enthalpy;
                entr23 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp24, pres24);
                enth24 = luis.working_fluid.Enthalpy;
                entr24 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp25, pres25);
                enth25 = luis.working_fluid.Enthalpy;
                entr25 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp26, pres26);
                enth26 = luis.working_fluid.Enthalpy;
                entr26 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp27, pres27);
                enth27 = luis.working_fluid.Enthalpy;
                entr27 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp28, pres28);
                enth28 = luis.working_fluid.Enthalpy;
                entr28 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp29, pres29);
                enth29 = luis.working_fluid.Enthalpy;
                entr29 = luis.working_fluid.Entropy;

                luis.working_fluid.FindStateWithTP(temp210, pres210);
                enth210 = luis.working_fluid.Enthalpy;
                entr210 = luis.working_fluid.Entropy;

                point1_state = "Pressure (kPa):" + Convert.ToString(pres21) + Environment.NewLine +
                              "Temperature (K):" + Convert.ToString(temp21) + Environment.NewLine +
                              "Entalphy (kJ/kg):" + Convert.ToString(enth21) + Environment.NewLine +
                              "Entrophy (kJ/kg K):" + Convert.ToString(entr21) + Environment.NewLine;

                point2_state = "Pressure (kPa):" + Convert.ToString(pres22) + Environment.NewLine +
                             "Temperature (K):" + Convert.ToString(temp22) + Environment.NewLine +
                             "Entalphy (kJ/kg):" + Convert.ToString(enth22) + Environment.NewLine +
                             "Entrophy (kJ/kg K):" + Convert.ToString(entr22) + Environment.NewLine;

                point3_state = "Pressure (kPa):" + Convert.ToString(pres23) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp23) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth23) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr23) + Environment.NewLine;

                point4_state = "Pressure (kPa):" + Convert.ToString(pres24) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp24) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth24) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr24) + Environment.NewLine;

                point5_state = "Pressure (kPa):" + Convert.ToString(pres25) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp25) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth25) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr25) + Environment.NewLine;

                point6_state = "Pressure (kPa):" + Convert.ToString(pres26) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp26) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth26) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr26) + Environment.NewLine;

                point7_state = "Pressure (kPa):" + Convert.ToString(pres27) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp27) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth27) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr27) + Environment.NewLine;

                point8_state = "Pressure (kPa):" + Convert.ToString(pres28) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp28) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth28) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr28) + Environment.NewLine;

                point9_state = "Pressure (kPa):" + Convert.ToString(pres29) + Environment.NewLine +
                         "Temperature (K):" + Convert.ToString(temp29) + Environment.NewLine +
                         "Entalphy (kJ/kg):" + Convert.ToString(enth29) + Environment.NewLine +
                         "Entrophy (kJ/kg K):" + Convert.ToString(entr29) + Environment.NewLine;

                point10_state = "Pressure (kPa):" + Convert.ToString(pres210) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp210) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth210) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr210) + Environment.NewLine;

                toolTip1.SetToolTip(label55, point1_state);
                toolTip2.SetToolTip(label57, point2_state);
                toolTip3.SetToolTip(label59, point3_state);
                toolTip4.SetToolTip(label60, point4_state);
                toolTip5.SetToolTip(label56, point5_state);
                toolTip6.SetToolTip(label58, point6_state);
                toolTip7.SetToolTip(label68, point7_state);
                toolTip8.SetToolTip(label69, point8_state);
                toolTip9.SetToolTip(label70, point9_state);
                toolTip10.SetToolTip(label71, point10_state);

                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                //button13.Enabled = true;
            }
        }

        //Off_Desing Button
        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
            }
            if (comboBox1.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
            }
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
            }
            if (comboBox1.Text == "PseudoPureFluid")
            {
                category = RefrigerantCategory.PseudoPureFluid;
            }

            if (comboBox3.Text == "DEF")
            {
                referencestate = ReferenceState.DEF;
            }
            if (comboBox3.Text == "ASH")
            {
                referencestate = ReferenceState.ASH;
            }
            if (comboBox3.Text == "IIR")
            {
                referencestate = ReferenceState.IIR;
            }
            if (comboBox3.Text == "NBP")
            {
                referencestate = ReferenceState.NBP;
            }

            luis.core1(this.comboBox2.Text, category);
            //luis.working_fluid.FluidsPath_LCE = punteroMainWindow.Fluids_Path_LCE;
            luis.working_fluid.Category = category;
            luis.working_fluid.reference = referencestate;

            //Store Input Data from Graphical User Interface GUI into variables
            w_dot_net2 = Convert.ToDouble(textBox1.Text);
            t_mc_in2 = Convert.ToDouble(textBox2.Text);
            t_t_in2 = Convert.ToDouble(textBox4.Text);
            p_mc_in2 = Convert.ToDouble(textBox22.Text);
            p_mc_out2 = Convert.ToDouble(textBox23.Text);
            ua_lt2 = Convert.ToDouble(textBox17.Text) * Convert.ToDouble(textBox62.Text);
            ua_ht2 = Convert.ToDouble(textBox17.Text) * (1-Convert.ToDouble(textBox62.Text));

            dp2_lt1 = Convert.ToDouble(textBox5.Text);
            dp2_lt2 = Convert.ToDouble(textBox26.Text);
            dp2_ht1 = Convert.ToDouble(textBox12.Text);
            dp2_ht2 = Convert.ToDouble(textBox25.Text);
            dp2_pc1 = Convert.ToDouble(textBox11.Text);
            dp2_phx2 = Convert.ToDouble(textBox10.Text);

            recomp_frac2 = Convert.ToDouble(textBox16.Text);
            eta_mc2 = Convert.ToDouble(textBox14.Text);
            eta_rc2 = Convert.ToDouble(textBox13.Text);
            eta_t2 = Convert.ToDouble(textBox19.Text);
            n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            tol2 = Convert.ToDouble(textBox21.Text);

            t_mc_in_off2 = Convert.ToDouble(textBox56.Text);
            t_t_in_off2 = Convert.ToDouble(textBox55.Text);
            recomp_frac_off2 = Convert.ToDouble(textBox54.Text);
            n_mc_off2 = Convert.ToDouble(textBox8.Text);
            n_t_off2 = Convert.ToDouble(textBox3.Text);
            target_off2 = Convert.ToDouble(textBox30.Text);
            target_code_off2 = Convert.ToInt64(textBox30.Text);
            lowest_pressure_off2 = Convert.ToDouble(textBox37.Text);
            highest_pressure_off2 = Convert.ToDouble(textBox36.Text);

            if (comboBox2.Text == "CO2")
            {
                carbondioxidetarget_(ref  w_dot_net2, ref  t_mc_in2, ref  t_t_in2, ref  p_mc_in2, ref  p_mc_out2, ref  ua_lt2,
                              ref  ua_ht2, ref  eta_mc2, ref  eta_rc2, ref  eta_t2, ref n_sub_hxrs2, ref  recomp_frac2, ref  tol2, ref  eta_thermal2,
                              ref  dp2_lt1, ref  dp2_lt2, ref  dp2_ht1, ref  dp2_ht2, ref  dp2_pc1, ref  dp2_pc2, ref  dp2_phx1, ref  dp2_phx2,
                              ref  temp21, ref  temp22, ref  temp23, ref  temp24, ref  temp25, ref  temp26, ref  temp27, ref  temp28, ref  temp29, ref  temp210,
                              ref  pres21, ref  pres22, ref  pres23, ref  pres24, ref  pres25, ref  pres26, ref  pres27, ref  pres28, ref  pres29, ref  pres210,
                              ref  massflow2, ref  LT_mdoth, ref  LT_mdotc, ref  LT_Tcin, ref  LT_Thin, ref  LT_Pcin, ref  LT_Phin, ref  LT_Pcout, ref  LT_Phout,
                              ref  LT_Q, ref  HT_mdoth, ref  HT_mdotc, ref  HT_Tcin, ref  HT_Thin, ref  HT_Pcin, ref  HT_Phin, ref  HT_Pcout, ref  HT_Phout,
                              ref  HT_Q, ref  LT_UA, ref  HT_UA, ref  LT_Effc, ref  HT_Effc, ref N_design2,
                              ref  t_mc_in_off2, ref  t_t_in_off2, ref  recomp_frac_off2, ref  n_mc_off2, ref  n_t_off2, ref  target_off2, ref  target_code_off2,
                              ref  lowest_pressure_off2, ref  highest_pressure_off2);

                textBox22.Text = Convert.ToString(pres21);
                textBox23.Text = Convert.ToString(pres22);
                textBox27.Text = Convert.ToString(pres23);
                textBox24.Text = Convert.ToString(pres24);
                textBox29.Text = Convert.ToString(pres25);
                textBox28.Text = Convert.ToString(pres26);
                textBox41.Text = Convert.ToString(pres27);
                textBox40.Text = Convert.ToString(pres28);
                textBox39.Text = Convert.ToString(pres29);
                textBox38.Text = Convert.ToString(pres210);

                textBox47.Text = Convert.ToString(temp21);
                textBox46.Text = Convert.ToString(temp22);
                textBox45.Text = Convert.ToString(temp23);
                textBox44.Text = Convert.ToString(temp24);
                textBox43.Text = Convert.ToString(temp25);
                textBox42.Text = Convert.ToString(temp26);
                textBox35.Text = Convert.ToString(temp27);
                textBox34.Text = Convert.ToString(temp28);
                textBox33.Text = Convert.ToString(temp29);
                textBox32.Text = Convert.ToString(temp210);

                textBox48.Text = Convert.ToString(w_dot_net2);
                textBox49.Text = Convert.ToString(massflow2);
                textBox50.Text = Convert.ToString(eta_thermal2 * 100);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state;

                point1_state = "Pressure (kPa):" + Convert.ToString(pres21) + Environment.NewLine +
                              "Temperature (K):" + Convert.ToString(temp21) + Environment.NewLine +
                              "Entalphy (kJ/kg):" + Convert.ToString(enth21) + Environment.NewLine +
                              "Entrophy (kJ/kg K):" + Convert.ToString(entr21) + Environment.NewLine;

                point2_state = "Pressure (kPa):" + Convert.ToString(pres22) + Environment.NewLine +
                             "Temperature (K):" + Convert.ToString(temp22) + Environment.NewLine +
                             "Entalphy (kJ/kg):" + Convert.ToString(enth22) + Environment.NewLine +
                             "Entrophy (kJ/kg K):" + Convert.ToString(entr22) + Environment.NewLine;

                point3_state = "Pressure (kPa):" + Convert.ToString(pres23) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp23) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth23) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr23) + Environment.NewLine;

                point4_state = "Pressure (kPa):" + Convert.ToString(pres24) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp24) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth24) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr24) + Environment.NewLine;

                point5_state = "Pressure (kPa):" + Convert.ToString(pres25) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp25) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth25) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr25) + Environment.NewLine;

                point6_state = "Pressure (kPa):" + Convert.ToString(pres26) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp26) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth26) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr26) + Environment.NewLine;

                point7_state = "Pressure (kPa):" + Convert.ToString(pres27) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp27) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth27) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr27) + Environment.NewLine;

                point8_state = "Pressure (kPa):" + Convert.ToString(pres28) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp28) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth28) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr28) + Environment.NewLine;

                point9_state = "Pressure (kPa):" + Convert.ToString(pres29) + Environment.NewLine +
                         "Temperature (K):" + Convert.ToString(temp29) + Environment.NewLine +
                         "Entalphy (kJ/kg):" + Convert.ToString(enth29) + Environment.NewLine +
                         "Entrophy (kJ/kg K):" + Convert.ToString(entr29) + Environment.NewLine;

                point10_state = "Pressure (kPa):" + Convert.ToString(pres210) + Environment.NewLine +
                          "Temperature (K):" + Convert.ToString(temp210) + Environment.NewLine +
                          "Entalphy (kJ/kg):" + Convert.ToString(enth210) + Environment.NewLine +
                          "Entrophy (kJ/kg K):" + Convert.ToString(entr210) + Environment.NewLine;

                toolTip1.SetToolTip(label55, point1_state);
                toolTip2.SetToolTip(label57, point2_state);
                toolTip3.SetToolTip(label59, point3_state);
                toolTip4.SetToolTip(label60, point4_state);
                toolTip5.SetToolTip(label61, point5_state);
                toolTip6.SetToolTip(label62, point6_state);
                toolTip7.SetToolTip(label63, point7_state);
                toolTip8.SetToolTip(label65, point8_state);
                toolTip9.SetToolTip(label66, point9_state);
                toolTip10.SetToolTip(label67, point10_state);

                button5.Enabled = true;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
