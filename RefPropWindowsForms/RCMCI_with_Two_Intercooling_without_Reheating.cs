﻿using System;
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

using Excel = Microsoft.Office.Interop.Excel;

namespace RefPropWindowsForms
{
    public partial class RCMCI_with_Two_Intercooling_without_Reheating : Form
    {
        public core luis = new core();

        public Net_Power Net_Power_dialog;

        public Final_Report Final_Report_dialog;

        public Generator Generator_dialog;

        public Diagrams Diagrams_dialog;

        public Estimated_Cost Estimated_Cost_Dialog;

        public Fresnel SF_PHX_LF;

        public PTC_Solar_Field SF_PHX;

        //Dual-Loop option
        public Double PHX1_temp_out = 0;
        public Double PHX1_Q = 0;
        public Double PHX1_pres_out = 0;
        public Double DP_PHX1 = 0;
        public Double PHX1_h_in = 0;
        public Double PHX1_h_out = 0;
        public Double PHX2_Q = 0;
        public Double PHX2_pres_out = 0;
        public Double DP_PHX2 = 0;
        public Double PHX2_h_in = 0;
        public Double PHX2_h_out = 0;

        public PreeCooler Precooler_dialog1;
        public PreeCooler Precooler_dialog2;
        public PreeCooler Precooler_dialog3;

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;

        //First calculate the Main Compressor Rotational speed and after send that value to the Turbines
        public Double N_design_Main_Compressor;

        public snl_compressor_tsr Compressor1, Compressor2, Compressor3;
        public snl_compressor_tsr ReCompressor;

        //Input Data:
        public RefrigerantCategory category;
        public ReferenceState referencestate;
        public Int64 Error_code;
        public core.RecompCycle recomp_cycle = new core.RecompCycle();

        public Double wmm;

        //STORING RESULTS

        //"Parabolic" or "Fresnel"
        public String Collector_Type_Main_SF;
        public String Collector_Type_ReHeating_SF;

        //Main Solar Field 
        public Double PTC_Main_SF_Effective_Apperture_Area;
        public Double LF_Main_SF_Effective_Apperture_Area;

        public Double PTC_Main_Solar_Impinging_flowpath, PTC_Main_Solar_Energy_Absorbed_flowpath, PTC_Main_Energy_Loss_flowpath, PTC_Main_Net_Absorbed_flowpath;
        public Double PTC_Main_Net_Absorbed_SF, PTC_Main_Collector_Efficiency, PTC_Main_SF_Pressure_drop, PTC_Main_calculated_mass_flux, PTC_Main_calculated_Number_Rows;
        public Double PTC_Main_calculated_Row_length;

        public Double PTC_Main_SF_Pump_Calculated_Power, PTC_Main_SF_Pump_isoentropic_eff, PTC_Main_SF_Pump_Hydraulic_Power, PTC_Main_SF_Pump_Mechanical_eff;
        public Double PTC_Main_SF_Pump_Shaft_Work, PTC_Main_SF_Pump_Motor_eff, PTC_Main_SF_Pump_Motor_Elec_Consump, PTC_Main_SF_Pump_Motor_NamePlate_Design, PTC_Main_SF_Pump_Motor_NamePlate;
        public Double Dual_Loop_PTC_Main_SF_Pump_Motor_Elec_Consump_1, Dual_Loop_PTC_Main_SF_Pump_Motor_Elec_Consump_2;

        public Double LF_Main_Solar_Impinging_flowpath, LF_Main_Solar_Energy_Absorbed_flowpath, LF_Main_Energy_Loss_flowpath, LF_Main_Net_Absorbed_flowpath;
        public Double LF_Main_Net_Absorbed_SF, LF_Main_Collector_Efficiency, LF_Main_SF_Pressure_drop, LF_Main_calculated_mass_flux, LF_Main_calculated_Number_Rows;
        public Double LF_Main_calculated_Row_length;

        public Double LF_Main_SF_Pump_Calculated_Power, LF_Main_SF_Pump_isoentropic_eff, LF_Main_SF_Pump_Hydraulic_Power, LF_Main_SF_Pump_Mechanical_eff;
        public Double LF_Main_SF_Pump_Shaft_Work, LF_Main_SF_Pump_Motor_eff, LF_Main_SF_Pump_Motor_Elec_Consump, LF_Main_SF_Pump_Motor_NamePlate_Design, LF_Main_SF_Pump_Motor_NamePlate;
        public Double Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1, Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2;
       
        //Primary Heat Exchanger (PHX) 
        public Double PHX_Qdot, PHX_Num_HXs, PHX_mdot_c, PHX_mdot_h, PHX_cold_Pin, PHX_cold_Tin, PHX_cold_Pout, PHX_cold_Tout;
        public Double PHX_hot_Pin, PHX_hot_Tin, PHX_hot_Pout, PHX_hot_Tout;
        public Double PHX_UA, PHX_NTU, PHX_CR, PHX_min_DT, PHX_Effectiveness, PHX_Q_per_module, PHX_number_modules, PHX_min_DT_input;
        //public Double PHX_mdot_h_module, PHX_mdot_c_module, PHX_UA_module, PHX_NTU_module, PHX_CR_module, PHX_min_DT_module, PHX_Effectiveness_module;

        public Double[] PHX_T_cold = new Double[25];
        public Double[] PHX_T_hot = new Double[25];

        public Double[] PHX_UA_local = new Double[25];
        public Double[] PHX_NTU_local = new Double[25];
        public Double[] PHX_CR_local = new Double[25];
        public Double[] PHX_Effec_local = new Double[25];
        public Double[] PHX_C_dot_c_local = new Double[25];
        public Double[] PHX_C_dot_h_local = new Double[25];

        public Double[] PHX_UA_local_module = new Double[25];
        public Double[] PHX_NTU_local_module = new Double[25];
        public Double[] PHX_CR_local_module = new Double[25];
        public Double[] PHX_Effec_local_module = new Double[25];
        public Double[] PHX_C_dot_c_local_module = new Double[25];
        public Double[] PHX_C_dot_h_local_module = new Double[25];
       
        //Generator Information
        public Double Generator_Name_Plate_Power, Generator_Power_Output, Generator_Total_Loss;
        public Double Generator_Shaft_Power, Gear_Efficiency, Rating_Point_Efficiency;
        public Double Generator_Mechanical_Loss, Generator_Electrical_Loss, Rating_Design_Point_Load;

        //SF and ACHE Pumps electrical consumption
        public Double Main_SF_Pump_Electrical_Consumption;
        public Double UHS_Water_Pump, ACHE_fans;      

        //Main Compressor2 results
        public Double Main_Compressor_Pin, Main_Compressor_Tin, Main_Compressor_Pout, Main_Compressor_Tout;
        public Double Main_Compressor_Flow, Main_Compressor_Diameter, Main_Compressor_Rotation_Velocity;
        public Double Main_Compressor_Efficiency, Main_Compressor_Phi; //Main_Compressor_Phi is the Compressor Flow Factor
        public Boolean Main_Compressor_Surge;

        //Main Compressor1 results
        public Double Main_Compressor1_Pin, Main_Compressor1_Tin, Main_Compressor1_Pout, Main_Compressor1_Tout;
        public Double Main_Compressor1_Flow, Main_Compressor1_Diameter1, Main_Compressor1_Diameter2, Main_Compressor1_Rotation_Velocity;
        public Double Main_Compressor1_Efficiency, Main_Compressor1_Phi; //Main_Compressor_Phi is the Compressor Flow Factor
        public Boolean Main_Compressor1_Surge;

        //Main Compressor3 results
        public Double Main_Compressor3_Pin, Main_Compressor3_Tin, Main_Compressor3_Pout, Main_Compressor3_Tout;
        public Double Main_Compressor3_Flow, Main_Compressor3_Diameter1, Main_Compressor3_Diameter2, Main_Compressor3_Rotation_Velocity;
        public Double Main_Compressor3_Efficiency, Main_Compressor3_Phi; //Main_Compressor_Phi is the Compressor Flow Factor
        public Boolean Main_Compressor3_Surge;

        //Recompressor results
        public Double ReCompressor_Pin, ReCompressor_Tin, ReCompressor_Pout, ReCompressor_Tout;
        public Double ReCompressor_Flow, ReCompressor_Diameter1, ReCompressor_Diameter2, ReCompressor_Rotation_Velocity;
        public Double ReCompressor_Efficiency, ReCompressor_Phi; //Main_Compressor_Phi is the Compressor Flow Factor
        public Boolean ReCompressor_Surge;

        //Main Turbine results
        public Double Main_Turbine_Pin, Main_Turbine_Tin, Main_Turbine_Pout, Main_Turbine_Tout;
        public Double Main_Turbine_Flow, Main_Turbine_Rotary_Velocity, Main_Turbine_Diameter, Main_Turbine_Efficiency, Main_Turbine_Anozzle;
        public Double Main_Turbine_nu, Main_Turbine_w_Tip_Ratio;
               
        //LTR results 
        public Double LTR_Qdot, LTR_Num_HXs, LTR_mdot_c, LTR_mdot_h, LTR_cold_Pin, LTR_cold_Tin, LTR_cold_Pout, LTR_cold_Tout;
        public Double LTR_hot_Pin, LTR_hot_Tin, LTR_hot_Pout, LTR_hot_Tout;
        public Double LTR_UA, LTR_NTU, LTR_CR, LTR_min_DT, LTR_Effectiveness, LTR_Q_per_module, LTR_number_modules;

        public Double LTR_mdot_h_module, LTR_mdot_c_module, LTR_UA_module, LTR_NTU_module, LTR_CR_module, LTR_min_DT_module, LTR_Effectiveness_module;

        public Double[] LTR_T_cold = new Double[25];
        public Double[] LTR_T_hot = new Double[25];

        public Double[] LTR_UA_local = new Double[25];
        public Double[] LTR_NTU_local = new Double[25];
        public Double[] LTR_CR_local = new Double[25];
        public Double[] LTR_Effec_local = new Double[25];
        public Double[] LTR_C_dot_c_local = new Double[25];
        public Double[] LTR_C_dot_h_local = new Double[25];

        public Double[] LTR_UA_local_module = new Double[25];
        public Double[] LTR_NTU_local_module = new Double[25];
        public Double[] LTR_CR_local_module = new Double[25];
        public Double[] LTR_Effec_local_module = new Double[25];
        public Double[] LTR_C_dot_c_local_module = new Double[25];
        public Double[] LTR_C_dot_h_local_module = new Double[25];

        //HTR results
        public Double HTR_Qdot, HTR_Num_HXs, HTR_mdot_c, HTR_mdot_h, HTR_cold_Pin, HTR_cold_Tin, HTR_cold_Pout, HTR_cold_Tout;

        public Double HTR_hot_Pin, HTR_hot_Tin, HTR_hot_Pout, HTR_hot_Tout;
        public Double HTR_UA, HTR_NTU, HTR_CR, HTR_min_DT, HTR_Effectiveness, HTR_Q_per_module, HTR_number_modules;
        public Double HTR_mdot_h_module, HTR_mdot_c_module, HTR_UA_module, HTR_NTU_module, HTR_CR_module, HTR_min_DT_module, HTR_Effectiveness_module;

        public Double[] HTR_T_cold = new Double[25];
        public Double[] HTR_T_hot = new Double[25];

        public Double[] HTR_UA_local = new Double[25];
        public Double[] HTR_NTU_local = new Double[25];
        public Double[] HTR_CR_local = new Double[25];
        public Double[] HTR_Effec_local = new Double[25];
        public Double[] HTR_C_dot_c_local = new Double[25];
        public Double[] HTR_C_dot_h_local = new Double[25];

        public Double[] HTR_UA_local_module = new Double[25];
        public Double[] HTR_NTU_local_module = new Double[25];
        public Double[] HTR_CR_local_module = new Double[25];
        public Double[] HTR_Effec_local_module = new Double[25];
        public Double[] HTR_C_dot_c_local_module = new Double[25];
        public Double[] HTR_C_dot_h_local_module = new Double[25];

        public Double specific_work_main_turbine = 0;
        public Double specific_work_compressor1 = 0;
        public Double specific_work_compressor2 = 0;
        public Double specific_work_compressor3 = 0;
        public Double Miscellanous_Auxiliaries = 0;
        public Double Total_Auxiliaries = 0;

        public Double w_dot_net2;
        public Double t_mc1_in2, t_mc2_in2, t_mc3_in2;
        public Double t_t_in2;
        public Double ua_lt2, ua_ht2;
        public Double eta1_mc2;
        public Double eta2_mc2;
        public Double eta3_mc2;
        public Double eta_rc2;
        public Double eta_t2;
        public Int64 n_sub_hxrs2;
        public Double p_mc1_in2;
        public Double p_mc1_out2;
        public Double p_mc2_in2;
        public Double p_mc2_out2;
        public Double p_mc3_in2;
        public Double p_mc3_out2;
        public Double recomp_frac2;
        public Double tol2;
        public Double eta_thermal2;

        public Double dp2_lt1, dp2_lt2;
        public Double dp2_ht1, dp2_ht2;

        public Double dp1_pc1, dp1_pc2;
        public Double dp2_pc1, dp2_pc2;
        public Double dp3_pc1, dp3_pc2;

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
        public Double temp211;
        public Double temp212;
        public Double temp213;
        public Double temp214;

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
        public Double pres211;
        public Double pres212;
        public Double pres213;
        public Double pres214;

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
        public Double enth211;
        public Double enth212;
        public Double enth213;
        public Double enth214;

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
        public Double entr211;
        public Double entr212;
        public Double entr213;
        public Double entr214;

        public Double massflow2;
        public Double LT_mdoth, LT_mdotc, LT_Tcin, LT_Thin, LT_Pcin, LT_Phin;
        public Double LT_Pcout, LT_Phout, LT_Q, HT_mdoth, HT_mdotc, HT_Tcin, HT_Thin;
        public Double HT_Pcin, HT_Phin, HT_Pcout, HT_Phout, HT_Q, LT_UA, HT_UA;
        public Double LT_Effc, HT_Effc;
        public Double PHX, PC1, PC2, PC3;

        public RCMCI_with_Two_Intercooling_without_Reheating()
        {
            InitializeComponent();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            int maxIterations = 5;
            int numIterations = 0;
            
            //PureFluid
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                luis.core1(this.comboBox1.Text, category);
            }

            //NewMixture
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                luis.core1(this.comboBox2.Text + "=" + textBox33.Text + "," + this.comboBox6.Text + "=" + textBox34.Text, category);
            }

            if (comboBox1.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
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

            luis.working_fluid.Category = category;
            luis.working_fluid.reference = referencestate;

            luis.wmm = luis.working_fluid.MolecularWeight;

            w_dot_net2 = Convert.ToDouble(textBox1.Text);
            t_mc1_in2 = Convert.ToDouble(textBox2.Text);
            t_mc2_in2 = Convert.ToDouble(textBox28.Text);
            t_mc3_in2 = Convert.ToDouble(textBox85.Text);
            t_t_in2 = Convert.ToDouble(textBox4.Text);
            p_mc1_in2 = Convert.ToDouble(textBox3.Text);
            p_mc1_out2 = Convert.ToDouble(textBox8.Text);
            p_mc2_in2 = Convert.ToDouble(textBox23.Text);
            p_mc2_out2 = Convert.ToDouble(textBox22.Text);
            p_mc3_in2 = Convert.ToDouble(textBox84.Text);
            p_mc3_out2 = Convert.ToDouble(textBox83.Text);
            ua_lt2 = Convert.ToDouble(textBox17.Text);
            ua_ht2 = Convert.ToDouble(textBox16.Text);
            recomp_frac2 = Convert.ToDouble(textBox15.Text);
            eta1_mc2 = Convert.ToDouble(textBox14.Text);
            eta2_mc2 = Convert.ToDouble(textBox27.Text);
            eta3_mc2 = Convert.ToDouble(textBox86.Text);
            eta_rc2 = Convert.ToDouble(textBox13.Text);
            eta_t2 = Convert.ToDouble(textBox19.Text);
            n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            tol2 = Convert.ToDouble(textBox21.Text);
            dp2_lt1 = Convert.ToDouble(textBox5.Text);
            dp2_ht1 = Convert.ToDouble(textBox12.Text);
            dp1_pc1 = Convert.ToDouble(textBox11.Text);
            dp2_pc2 = Convert.ToDouble(textBox24.Text);
            dp3_pc2 = Convert.ToDouble(textBox87.Text);
            dp2_phx1 = Convert.ToDouble(textBox10.Text);
            dp2_lt2 = Convert.ToDouble(textBox26.Text);
            dp2_ht2 = Convert.ToDouble(textBox25.Text);

            core.RCMCIwithTwoIntercoolingwithoutReheating cicloRCMCI__withTwoIntercooling_withoutRH = new core.RCMCIwithTwoIntercoolingwithoutReheating();

            luis.RecompCycle_RCMCI_with_Two_Intercooling_without_Reheating(luis, ref cicloRCMCI__withTwoIntercooling_withoutRH, w_dot_net2,
            t_mc2_in2, t_t_in2, p_mc2_in2, p_mc2_out2, p_mc1_in2, t_mc1_in2, p_mc1_out2, p_mc3_in2, t_mc3_in2, p_mc3_out2,
            ua_lt2, ua_ht2, eta2_mc2, eta_rc2, eta1_mc2, eta3_mc2, eta_t2, n_sub_hxrs2, recomp_frac2, tol2, eta_thermal2, -dp2_lt1, -dp2_lt2, -dp2_ht1, -dp2_ht2,
            -dp1_pc1, -dp1_pc2, -dp2_phx1, -dp2_phx2, -dp2_pc2, -dp2_pc2, -dp3_pc2, -dp3_pc2);

            massflow2 = cicloRCMCI__withTwoIntercooling_withoutRH.m_dot_turbine;
            w_dot_net2 = cicloRCMCI__withTwoIntercooling_withoutRH.W_dot_net;
            eta_thermal2 = cicloRCMCI__withTwoIntercooling_withoutRH.eta_thermal;
            recomp_frac2 = cicloRCMCI__withTwoIntercooling_withoutRH.recomp_frac;

            temp21 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[0];
            temp22 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[1];
            temp23 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[2];
            temp24 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[3];
            temp25 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[4];
            temp26 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[5];
            temp27 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[6];
            temp28 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[7];
            temp29 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[8];
            temp210 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[9];
            temp211 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[10];
            temp212 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[11];
            temp213 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[12];
            temp214 = cicloRCMCI__withTwoIntercooling_withoutRH.temp[13];

            pres21 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[0];
            pres22 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[1];
            pres23 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[2];
            pres24 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[3];
            pres25 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[4];
            pres26 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[5];
            pres27 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[6];
            pres28 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[7];
            pres29 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[8];
            pres210 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[9];
            pres211 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[10];
            pres212 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[11];
            pres213 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[12];
            pres214 = cicloRCMCI__withTwoIntercooling_withoutRH.pres[13];

            textBox47.Text = Convert.ToString(temp21); //Point 1
            textBox46.Text = Convert.ToString(temp22); //Point 2
            textBox45.Text = Convert.ToString(temp23); //Point 3
            textBox44.Text = Convert.ToString(temp24); //Point 4
            textBox43.Text = Convert.ToString(temp25); //Point 5
            textBox42.Text = Convert.ToString(temp26); //Point 6
            textBox35.Text = Convert.ToString(temp27); //Point 7
            textBox18.Text = Convert.ToString(temp28); //Point 8
            textBox7.Text = Convert.ToString(temp29); //Point 9
            textBox6.Text = Convert.ToString(temp210); //Point 10
            textBox57.Text = Convert.ToString(temp211); //Point 11
            textBox56.Text = Convert.ToString(temp212); //Point 12
            textBox70.Text = Convert.ToString(temp213); //Point 13
            textBox81.Text = Convert.ToString(temp214); //Point 14

            textBox54.Text = Convert.ToString(pres21);
            textBox53.Text = Convert.ToString(pres22);
            textBox52.Text = Convert.ToString(pres23);
            textBox9.Text = Convert.ToString(pres24);
            textBox37.Text = Convert.ToString(pres25);
            textBox36.Text = Convert.ToString(pres26);
            textBox41.Text = Convert.ToString(pres27);
            textBox40.Text = Convert.ToString(pres28);
            textBox39.Text = Convert.ToString(pres29);
            textBox38.Text = Convert.ToString(pres210);
            textBox59.Text = Convert.ToString(pres211);
            textBox58.Text = Convert.ToString(pres212);
            textBox80.Text = Convert.ToString(pres213);
            textBox82.Text = Convert.ToString(pres214);

            String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
            String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state;
            String point13_state, point14_state;

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

            luis.working_fluid.FindStateWithTP(temp211, pres211);
            enth211 = luis.working_fluid.Enthalpy;
            entr211 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp212, pres212);
            enth212 = luis.working_fluid.Enthalpy;
            entr212 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp213, pres213);
            enth213 = luis.working_fluid.Enthalpy;
            entr213 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp214, pres214);
            enth214 = luis.working_fluid.Enthalpy;
            entr214 = luis.working_fluid.Entropy;

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

            point11_state = "Pressure (kPa):" + Convert.ToString(pres211) + Environment.NewLine +
                     "Temperature (K):" + Convert.ToString(temp211) + Environment.NewLine +
                     "Entalphy (kJ/kg):" + Convert.ToString(enth211) + Environment.NewLine +
                     "Entrophy (kJ/kg K):" + Convert.ToString(entr211) + Environment.NewLine;

            point12_state = "Pressure (kPa):" + Convert.ToString(pres212) + Environment.NewLine +
                      "Temperature (K):" + Convert.ToString(temp212) + Environment.NewLine +
                      "Entalphy (kJ/kg):" + Convert.ToString(enth212) + Environment.NewLine +
                      "Entrophy (kJ/kg K):" + Convert.ToString(entr212) + Environment.NewLine;

            point13_state = "Pressure (kPa):" + Convert.ToString(pres213) + Environment.NewLine +
                     "Temperature (K):" + Convert.ToString(temp213) + Environment.NewLine +
                     "Entalphy (kJ/kg):" + Convert.ToString(enth213) + Environment.NewLine +
                     "Entrophy (kJ/kg K):" + Convert.ToString(entr213) + Environment.NewLine;

            point14_state = "Pressure (kPa):" + Convert.ToString(pres214) + Environment.NewLine +
                     "Temperature (K):" + Convert.ToString(temp214) + Environment.NewLine +
                     "Entalphy (kJ/kg):" + Convert.ToString(enth214) + Environment.NewLine +
                     "Entrophy (kJ/kg K):" + Convert.ToString(entr214) + Environment.NewLine;

            textBox48.Text = Convert.ToString(w_dot_net2);
            textBox49.Text = Convert.ToString(massflow2);
            textBox50.Text = Convert.ToString(eta_thermal2 * 100);

            if (checkBox1.Checked == false)
            {
                PHX = cicloRCMCI__withTwoIntercooling_withoutRH.PHX.Q_dot;

                LT_Q = cicloRCMCI__withTwoIntercooling_withoutRH.LT.Q_dot;
                LT_mdotc = cicloRCMCI__withTwoIntercooling_withoutRH.LT.m_dot_design[0];
                LT_mdoth = cicloRCMCI__withTwoIntercooling_withoutRH.LT.m_dot_design[1];
                LT_Tcin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.T_c_in;
                LT_Thin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.T_h_in;
                LT_Pcin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_c_in;
                LT_Phin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_h_in;
                LT_Pcout = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_c_out;
                LT_Phout = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_h_out;
                LT_Effc = cicloRCMCI__withTwoIntercooling_withoutRH.LT.eff;

                HT_Q = cicloRCMCI__withTwoIntercooling_withoutRH.HT.Q_dot;
                HT_mdotc = cicloRCMCI__withTwoIntercooling_withoutRH.HT.m_dot_design[0];
                HT_mdoth = cicloRCMCI__withTwoIntercooling_withoutRH.HT.m_dot_design[1];
                HT_Tcin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.T_c_in;
                HT_Thin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.T_h_in;
                HT_Pcin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_c_in;
                HT_Phin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_h_in;
                HT_Pcout = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_c_out;
                HT_Phout = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_h_out;
                HT_Effc = cicloRCMCI__withTwoIntercooling_withoutRH.HT.eff;

                PC1 = -cicloRCMCI__withTwoIntercooling_withoutRH.PC1.Q_dot;
                PC2 = -cicloRCMCI__withTwoIntercooling_withoutRH.PC2.Q_dot;
                PC3 = -cicloRCMCI__withTwoIntercooling_withoutRH.COOLER.Q_dot;

                //if (comboBox4.Text == "Dual-Loop")
                //{
                //    //Main SF
                //    comboBox9.Enabled = true;
                //    comboBox11.Enabled = true;
                //    comboBox8.Enabled = true;
                //    comboBox10.Enabled = true;
                //    button17.Enabled = true;
                //    button13.Enabled = true;

                //    button15.Enabled = false;
                //}

                //else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                //{
                //    //Main SF
                //    comboBox9.Enabled = false;
                //    comboBox11.Enabled = false;
                //    comboBox8.Enabled = false;
                //    comboBox10.Enabled = false;
                //    button17.Enabled = false;
                //    button13.Enabled = false;

                //    button15.Enabled = true;
                //}             
            }

            else 
            {
                PHX = cicloRCMCI__withTwoIntercooling_withoutRH.PHX.Q_dot;

                LT_Q = cicloRCMCI__withTwoIntercooling_withoutRH.LT.Q_dot;
                LT_mdotc = cicloRCMCI__withTwoIntercooling_withoutRH.LT.m_dot_design[0];
                LT_mdoth = cicloRCMCI__withTwoIntercooling_withoutRH.LT.m_dot_design[1];
                LT_Tcin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.T_c_in;
                LT_Thin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.T_h_in;
                LT_Pcin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_c_in;
                LT_Phin = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_h_in;
                LT_Pcout = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_c_out;
                LT_Phout = cicloRCMCI__withTwoIntercooling_withoutRH.LT.P_h_out;
                LT_Effc = cicloRCMCI__withTwoIntercooling_withoutRH.LT.eff;

                HT_Q = cicloRCMCI__withTwoIntercooling_withoutRH.HT.Q_dot;
                HT_mdotc = cicloRCMCI__withTwoIntercooling_withoutRH.HT.m_dot_design[0];
                HT_mdoth = cicloRCMCI__withTwoIntercooling_withoutRH.HT.m_dot_design[1];
                HT_Tcin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.T_c_in;
                HT_Thin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.T_h_in;
                HT_Pcin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_c_in;
                HT_Phin = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_h_in;
                HT_Pcout = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_c_out;
                HT_Phout = cicloRCMCI__withTwoIntercooling_withoutRH.HT.P_h_out;
                HT_Effc = cicloRCMCI__withTwoIntercooling_withoutRH.HT.eff;

                PC1 = -cicloRCMCI__withTwoIntercooling_withoutRH.PC1.Q_dot;
                PC2 = -cicloRCMCI__withTwoIntercooling_withoutRH.PC2.Q_dot;
                PC3 = -cicloRCMCI__withTwoIntercooling_withoutRH.COOLER.Q_dot;
            }

            button7.Enabled = true;
            button6.Enabled = true;
            button28.Enabled = true;
            button4.Enabled = true;
            button8.Enabled = true;
            button12.Enabled = true;
            button5.Enabled = true;
            button29.Enabled = true;
            button9.Enabled = true;
            button15.Enabled = true;
        }


        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                comboBox6.Enabled = false;
                comboBox12.Enabled = false;
                comboBox7.Enabled = false;
                textBox33.Enabled = false;
                textBox34.Enabled = false;
                textBox68.Enabled = false;
                textBox69.Enabled = false;
                button20.Enabled = false;
                button2.Enabled = true;
            }

            else if (comboBox1.Text == "NewMixture")
            {
                comboBox6.Enabled = true;
                comboBox12.Enabled = true;
                comboBox7.Enabled = true;
                textBox33.Enabled = true;
                textBox34.Enabled = true;
                textBox68.Enabled = true;
                textBox69.Enabled = true;
                button20.Enabled = true;
                button2.Enabled = false;

                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox33.Text + "," + this.comboBox6.Text + "=" + textBox34.Text + "," + this.comboBox12.Text + "=" + textBox68.Text + "," + this.comboBox7.Text + "=" + textBox69.Text, ReferenceState.DEF);

                textBox32.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox51.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox31.Text = Convert.ToString(working_fluid.CriticalDensity);
            }
        }

        //NLopt Optimization window
        private void Button36_Click(object sender, EventArgs e)
        {

        }

        //Compressor C1 calculation button
        private void button5_Click(object sender, EventArgs e)
        {
            ReCompressor = new snl_compressor_tsr();
            ReCompressor.textBox1.Text = Convert.ToString(pres211);
            ReCompressor.textBox2.Text = Convert.ToString(temp211);
            ReCompressor.textBox6.Text = Convert.ToString(pres212);
            ReCompressor.textBox5.Text = Convert.ToString(temp212);

            ReCompressor.textBox9.Text = Convert.ToString(massflow2);
            ReCompressor.textBox8.Text = Convert.ToString(recomp_frac2);

            ReCompressor.button2.Enabled = false;
            ReCompressor.button4.Enabled = false;

            ReCompressor.Show();
        }

        //Compressor C2 calculation button
        private void button29_Click(object sender, EventArgs e)
        {
            ReCompressor = new snl_compressor_tsr();
            ReCompressor.textBox1.Text = Convert.ToString(pres213);
            ReCompressor.textBox2.Text = Convert.ToString(temp213);
            ReCompressor.textBox6.Text = Convert.ToString(pres214);
            ReCompressor.textBox5.Text = Convert.ToString(temp214);

            ReCompressor.textBox9.Text = Convert.ToString(massflow2);
            ReCompressor.textBox8.Text = Convert.ToString(recomp_frac2);

            ReCompressor.button2.Enabled = false;
            ReCompressor.button4.Enabled = false;

            ReCompressor.Show();
        }

        //Compressor C3 calculation button
        private void button9_Click(object sender, EventArgs e)
        {
            button10.Enabled = true;

            Compressor1 = new snl_compressor_tsr();
            Compressor1.textBox1.Text = Convert.ToString(pres212);
            Compressor1.textBox2.Text = Convert.ToString(temp212);
            Compressor1.textBox6.Text = Convert.ToString(pres22);
            Compressor1.textBox5.Text = Convert.ToString(temp22);

            Compressor1.textBox9.Text = Convert.ToString(massflow2);
            Compressor1.textBox8.Text = Convert.ToString(recomp_frac2);

            Compressor1.button3.Enabled = false;
            Compressor1.button5.Enabled = false;
            Compressor1.button6.Enabled = false;
            Compressor1.button7.Enabled = false;

            Compressor1.Calculate_Main_Compressor();

            N_design_Main_Compressor = Convert.ToDouble(Compressor1.textBox11.Text);

            Compressor1.Show();
        }

        //Turbine calculation button
        private void button10_Click(object sender, EventArgs e)
        {
            Main_Turbine = new Radial_Turbine();
            Main_Turbine.textBox1.Text = Convert.ToString(pres26);
            Main_Turbine.textBox6.Text = Convert.ToString(pres27);
            Main_Turbine.textBox2.Text = Convert.ToString(temp26);
            Main_Turbine.textBox5.Text = Convert.ToString(temp27);
            Main_Turbine.textBox9.Text = Convert.ToString(massflow2);
            Main_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            Main_Turbine.calculate_Radial_Turbine();
            Main_Turbine.Show();
        }

        //Recompressor calculation button
        private void button12_Click(object sender, EventArgs e)
        {
            ReCompressor = new snl_compressor_tsr();
            ReCompressor.textBox1.Text = Convert.ToString(pres29);
            ReCompressor.textBox2.Text = Convert.ToString(temp29);
            ReCompressor.textBox6.Text = Convert.ToString(pres210);
            ReCompressor.textBox5.Text = Convert.ToString(temp210);

            ReCompressor.textBox9.Text = Convert.ToString(massflow2);
            ReCompressor.textBox8.Text = Convert.ToString(recomp_frac2);

            ReCompressor.button2.Enabled = false;
            ReCompressor.button4.Enabled = false;

            ReCompressor.Show();
        }

        //Pre-Cooler_3 calculation button
        private void button8_Click(object sender, EventArgs e)
        {
            Precooler_dialog2 = new PreeCooler();
            Precooler_dialog2.textBox2.Text = Convert.ToString(-PC3);
            Precooler_dialog2.textBox4.Text = Convert.ToString(massflow2 * (1 - recomp_frac2));
            Precooler_dialog2.textBox6.Text = Convert.ToString(temp214);
            Precooler_dialog2.textBox12.Text = Convert.ToString(pres214);
            Precooler_dialog2.textBox8.Text = Convert.ToString(pres21);
            Precooler_dialog2.PreeCooler1(luis);
            Precooler_dialog2.Calculate_Cooler();
            Precooler_dialog2.Show();
        }

        //Pre-Cooler_2 calculation button
        private void button4_Click(object sender, EventArgs e)
        {
            Precooler_dialog2 = new PreeCooler();
            Precooler_dialog2.textBox2.Text = Convert.ToString(-PC2);
            Precooler_dialog2.textBox4.Text = Convert.ToString(massflow2 * (1 - recomp_frac2));
            Precooler_dialog2.textBox6.Text = Convert.ToString(temp212);
            Precooler_dialog2.textBox12.Text = Convert.ToString(pres212);
            Precooler_dialog2.textBox8.Text = Convert.ToString(pres213);
            Precooler_dialog2.PreeCooler1(luis);
            Precooler_dialog2.Calculate_Cooler();
            Precooler_dialog2.Show();
        }

        //Pre-Cooler_1 calculation button
        private void button28_Click(object sender, EventArgs e)
        {
            Precooler_dialog1 = new PreeCooler();
            Precooler_dialog1.textBox2.Text = Convert.ToString(-PC1);
            Precooler_dialog1.textBox4.Text = Convert.ToString(massflow2 * (1 - recomp_frac2));
            Precooler_dialog1.textBox6.Text = Convert.ToString(temp29);
            Precooler_dialog1.textBox12.Text = Convert.ToString(pres29);
            Precooler_dialog1.textBox8.Text = Convert.ToString(pres211);
            Precooler_dialog1.PreeCooler1(luis);
            Precooler_dialog1.Calculate_Cooler();
            Precooler_dialog1.Show();
        }

        //LTR calculation button
        private void button6_Click(object sender, EventArgs e)
        {
            LT_Recuperator = new HeatExchangerUA();
            LT_Recuperator.textBox2.Text = Convert.ToString(LT_Q);
            LT_Recuperator.textBox3.Text = Convert.ToString(LT_mdotc);
            LT_Recuperator.textBox4.Text = Convert.ToString(LT_mdoth);
            LT_Recuperator.textBox7.Text = Convert.ToString(LT_Tcin);
            LT_Recuperator.textBox6.Text = Convert.ToString(LT_Thin);
            LT_Recuperator.textBox5.Text = Convert.ToString(LT_Pcin);
            LT_Recuperator.textBox8.Text = Convert.ToString(LT_Phin);
            LT_Recuperator.textBox9.Text = Convert.ToString(LT_Pcout);
            LT_Recuperator.textBox12.Text = Convert.ToString(LT_Phout);
            LT_Recuperator.textBox13.Text = Convert.ToString(LT_Effc);

            LT_Recuperator.HeatExchangerUA1(luis);
            LT_Recuperator.Calculate_HX();
            LT_Recuperator.Show();
        }

        //HTR calculation button
        private void button7_Click(object sender, EventArgs e)
        {
            HT_Recuperator = new HeatExchangerUA();
            HT_Recuperator.textBox2.Text = Convert.ToString(HT_Q);
            HT_Recuperator.textBox3.Text = Convert.ToString(HT_mdotc);
            HT_Recuperator.textBox4.Text = Convert.ToString(HT_mdoth);
            HT_Recuperator.textBox7.Text = Convert.ToString(HT_Tcin);
            HT_Recuperator.textBox6.Text = Convert.ToString(HT_Thin);
            HT_Recuperator.textBox5.Text = Convert.ToString(HT_Pcin);
            HT_Recuperator.textBox8.Text = Convert.ToString(HT_Phin);
            HT_Recuperator.textBox9.Text = Convert.ToString(HT_Pcout);
            HT_Recuperator.textBox12.Text = Convert.ToString(HT_Phout);
            HT_Recuperator.textBox13.Text = Convert.ToString(HT_Effc);

            HT_Recuperator.HeatExchangerUA1(luis);
            HT_Recuperator.Calculate_HX();
            HT_Recuperator.Show();
        }
    }
}
