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
    public partial class Target_off_design_without_ReHeating : Form
    {
        public core luis = new core();

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;

        //First calculate the Main Compressor Rotational speed and after send that value to the Turbines
        public Double N_design_Main_Compressor;

        public snl_compressor_tsr Main_Compressor;
        public snl_compressor_tsr ReCompressor;

        //Input Data:
        public RefrigerantCategory category;
        public ReferenceState referencestate;

        public Double wmm;

        const string refpropDLL_path1 = "CO2_design_withoutReHeat.dll";
        [DllImport(refpropDLL_path1, EntryPoint = "carbondioxide_", SetLastError = true)]
        public static extern void carbondioxide_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_mc_in1,
                                                 ref Double p_mc_out1, ref Double ua_lt1, ref Double ua_ht1, ref Double eta_mc1, ref Double eta_rc1,
                                                 ref Double eta_t1, ref Int64 n_sub_hxrs1, ref Double recomp_frac1, ref Double tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1,
                                                 ref Double dp_pc2, ref Double dp_phx1, ref Double dp_phx2, ref Double temp1, ref Double temp2, ref Double temp3,
                                                 ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9, ref Double temp10,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                 ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double m_dot_turbine1, ref Double LT_mdoth,
                                                 ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout,
                                                 ref Double LT_Phout, ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin,
                                                 ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA,
                                                 ref Double LT_Effc, ref Double HT_Effc, ref Double N_design);

        const string refpropDLL_path2 = "CO2_target_offdesign.dll";
        [DllImport(refpropDLL_path2, EntryPoint = "carbondioxidetarget_", SetLastError = true)]
        public static extern void carbondioxidetarget_(ref Double w_dot_net1,ref Double t_mc_in1,ref Double t_t_in1,ref Double p_mc_in1,ref Double p_mc_out1,
                                                 ref Double ua_lt1,ref Double ua_ht1,ref Double eta_mc1,ref Double eta_rc1,ref Double eta_t1,
                                                 ref Int64 n_sub_hxrs1, ref Double recomp_frac1, ref Double tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1,ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1,ref Double dp_pc2,
                                                 ref Double dp_phx1,ref Double dp_phx2,ref Double temp1,ref Double temp2,ref Double temp3,ref Double temp4,
                                                 ref Double temp5,ref Double temp6,ref Double temp7,ref Double temp8,ref Double temp9,ref Double temp10,
                                                 ref Double pres1,ref Double pres2,ref Double pres3,ref Double pres4,ref Double pres5,ref Double pres6,ref Double pres7,
                                                 ref Double pres8,ref Double pres9,ref Double pres10,ref Double m_dot_turbine1,ref Double LT_mdoth,ref Double LT_mdotc,
                                                 ref Double LT_Tcin,ref Double LT_Thin,ref Double LT_Pcin,ref Double LT_Phin,ref Double LT_Pcout,ref Double LT_Phout,
                                                 ref Double LT_Q, ref Double HT_mdoth,ref Double HT_mdotc,ref Double HT_Tcin,ref Double HT_Thin,ref Double HT_Pcin,
                                                 ref Double HT_Phin,ref Double HT_Pcout,ref Double HT_Phout,ref Double HT_Q,ref Double LT_UA,ref Double HT_UA,
                                                 ref Double LT_Effc,ref Double HT_Effc,ref Double N_mc_design,ref Double t_mc_in_off,ref Double t_t_in_off,
                                                 ref Double recomp_frac_off,ref Double n_mc_off,ref Double n_t_off,ref Double target_off,ref Int64 target_code_off,
                                                 ref Double lowest_pressure_off, ref Double highest_pressure_off);

        public Target_off_design_without_ReHeating()
        {
            InitializeComponent();
        }

        public Double w_dot_net2;
        public Double t_mc_in2;
        public Double t_t_in2;
        public Double ua_lt2, ua_ht2;
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_t2;
        public Int64 n_sub_hxrs2;
        public Double p_mc_in2;
        public Double p_mc_out2;
        public Double recomp_frac2;
        public Double tol2;
        public Double eta_thermal2;

        public Double dp2_lt1, dp2_lt2;
        public Double dp2_ht1, dp2_ht2;
        public Double dp2_pc1, dp2_pc2;
        public Double dp2_phx1, dp2_phx2;

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
        public Double LT_Effc, HT_Effc, N_design2;
        public Double N_mc_design2 = 0;
        public Double t_mc_in_off2, t_t_in_off2, recomp_frac_off2, n_mc_off2, n_t_off2;
        public Double target_off2, lowest_pressure_off2, highest_pressure_off2;
        public Int64 target_code_off2;


        // Design-Point Button
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
            //luis.working_fluid.FluidsPath_LCE = punteroMainWindow.Fluids_Path_LCE;
            luis.working_fluid.Category = category;
            luis.working_fluid.reference = referencestate;

            //Store Input Data from Graphical User Interface GUI into variables
            w_dot_net2 = Convert.ToDouble(textBox1.Text);
            t_mc_in2 = Convert.ToDouble(textBox2.Text);
            t_t_in2 = Convert.ToDouble(textBox4.Text);
            p_mc_in2 = Convert.ToDouble(textBox3.Text);
            p_mc_out2 = Convert.ToDouble(textBox8.Text);
            ua_lt2 = Convert.ToDouble(textBox17.Text);
            ua_ht2 = Convert.ToDouble(textBox16.Text);

            dp2_lt1 = Convert.ToDouble(textBox5.Text);
            dp2_lt2 = Convert.ToDouble(textBox26.Text);
            dp2_ht1 = Convert.ToDouble(textBox12.Text);
            dp2_ht2 = Convert.ToDouble(textBox25.Text);
            dp2_pc1 = Convert.ToDouble(textBox11.Text);
            dp2_phx2 = Convert.ToDouble(textBox10.Text);

            recomp_frac2 = Convert.ToDouble(textBox15.Text);
            eta_mc2 = Convert.ToDouble(textBox14.Text);
            eta_rc2 = Convert.ToDouble(textBox13.Text);
            eta_t2 = Convert.ToDouble(textBox19.Text);
            n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            tol2 = Convert.ToDouble(textBox21.Text);

            if (comboBox2.Text == "CO2")
            {
                carbondioxide_(ref  w_dot_net2, ref  t_mc_in2, ref  t_t_in2, ref  p_mc_in2, ref  p_mc_out2, ref  ua_lt2,
                               ref  ua_ht2, ref  eta_mc2, ref  eta_rc2, ref  eta_t2, ref  n_sub_hxrs2, ref  recomp_frac2, ref  tol2, ref  eta_thermal2,
                               ref  dp2_lt1, ref  dp2_lt2, ref  dp2_ht1, ref  dp2_ht2, ref  dp2_pc1, ref  dp2_pc2, ref  dp2_phx1, ref  dp2_phx2,
                               ref  temp21, ref  temp22, ref  temp23, ref  temp24, ref  temp25, ref  temp26, ref  temp27, ref  temp28, ref  temp29, ref  temp210,
                               ref  pres21, ref  pres22, ref  pres23, ref  pres24, ref  pres25, ref  pres26, ref  pres27, ref  pres28, ref  pres29, ref  pres210,
                               ref  massflow2, ref  LT_mdoth, ref  LT_mdotc, ref  LT_Tcin, ref  LT_Thin, ref  LT_Pcin, ref  LT_Phin, ref  LT_Pcout, ref  LT_Phout,
                               ref  LT_Q, ref  HT_mdoth, ref  HT_mdotc, ref  HT_Tcin, ref  HT_Thin, ref  HT_Pcin, ref  HT_Phin, ref  HT_Pcout, ref  HT_Phout,
                               ref  HT_Q, ref  LT_UA, ref  HT_UA, ref  LT_Effc, ref  HT_Effc, ref N_design2);

                //luis.Design_Point_RC(luis, ref recomp_cycle, W_dot_net, T_mc_in, T_t_in, P_mc_in, P_mc_out, P_rhx_in, T_rht_in, DP_LT_c, DP_HT_c, DP_PC, DP_PHX, DP_RHX, DP_LT_h, DP_HT_h, UA_LT, UA_HT, recomp_frac, eta_mc, eta_rc, eta_t, eta_trh, N_sub_hxrs, tol);

                //Fill results in the Graphical User Interface (GUI)

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

                textBox7.Text = Convert.ToString(N_design2);
                textBox7.BackColor = Color.Yellow;
            }
        }

        // Target-Off-Design Button
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
            p_mc_in2 = Convert.ToDouble(textBox3.Text);
            p_mc_out2 = Convert.ToDouble(textBox8.Text);
            ua_lt2 = Convert.ToDouble(textBox17.Text);
            ua_ht2 = Convert.ToDouble(textBox16.Text);

            dp2_lt1 = Convert.ToDouble(textBox5.Text);
            dp2_lt2 = Convert.ToDouble(textBox26.Text);
            dp2_ht1 = Convert.ToDouble(textBox12.Text);
            dp2_ht2 = Convert.ToDouble(textBox25.Text);
            dp2_pc1 = Convert.ToDouble(textBox11.Text);
            dp2_phx2 = Convert.ToDouble(textBox10.Text);

            recomp_frac2 = Convert.ToDouble(textBox15.Text);
            eta_mc2 = Convert.ToDouble(textBox14.Text);
            eta_rc2 = Convert.ToDouble(textBox13.Text);
            eta_t2 = Convert.ToDouble(textBox19.Text);
            n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            tol2 = Convert.ToDouble(textBox21.Text);

            t_mc_in_off2 = Convert.ToDouble(textBox56.Text);
            t_t_in_off2 = Convert.ToDouble(textBox55.Text);
            recomp_frac_off2 = Convert.ToDouble(textBox9.Text);
            n_mc_off2 = Convert.ToDouble(textBox57.Text);
            n_t_off2 = Convert.ToDouble(textBox6.Text);
            target_off2 = Convert.ToDouble(textBox31.Text);
            target_code_off2 = Convert.ToInt64(textBox30.Text);
            lowest_pressure_off2 = Convert.ToDouble(textBox37.Text);
            highest_pressure_off2 = Convert.ToDouble(textBox36.Text);

            if (comboBox2.Text == "CO2")
            {
                carbondioxidetarget_(ref  w_dot_net2, ref  t_mc_in2, ref  t_t_in2, ref  p_mc_in2, ref  p_mc_out2, ref  ua_lt2,
                              ref  ua_ht2, ref  eta_mc2, ref  eta_rc2, ref  eta_t2,ref n_sub_hxrs2, ref  recomp_frac2, ref  tol2, ref  eta_thermal2,
                              ref  dp2_lt1, ref  dp2_lt2, ref  dp2_ht1, ref  dp2_ht2, ref  dp2_pc1, ref  dp2_pc2, ref  dp2_phx1, ref  dp2_phx2,
                              ref  temp21, ref  temp22, ref  temp23, ref  temp24, ref  temp25, ref  temp26, ref  temp27, ref  temp28, ref  temp29, ref  temp210,
                              ref  pres21, ref  pres22, ref  pres23, ref  pres24, ref  pres25, ref  pres26, ref  pres27, ref  pres28, ref  pres29, ref  pres210,
                              ref  massflow2, ref  LT_mdoth, ref  LT_mdotc, ref  LT_Tcin, ref  LT_Thin, ref  LT_Pcin, ref  LT_Phin, ref  LT_Pcout, ref  LT_Phout,
                              ref  LT_Q, ref  HT_mdoth, ref  HT_mdotc, ref  HT_Tcin, ref  HT_Thin, ref  HT_Pcin, ref  HT_Phin, ref  HT_Pcout, ref  HT_Phout,
                              ref  HT_Q, ref  LT_UA, ref  HT_UA, ref  LT_Effc, ref  HT_Effc, ref N_design2,
                              ref  t_mc_in_off2,ref  t_t_in_off2, ref  recomp_frac_off2,ref  n_mc_off2,ref  n_t_off2,ref  target_off2,ref  target_code_off2,
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

                textBox7.Text = Convert.ToString(N_design2);
                textBox7.BackColor = Color.Yellow;
            }
        }
    }
}
