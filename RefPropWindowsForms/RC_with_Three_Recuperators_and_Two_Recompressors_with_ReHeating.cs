﻿using System;
using System.Collections;
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

using NLoptNet;

using sc.net;

using Excel = Microsoft.Office.Interop.Excel;

using System.Diagnostics;

namespace RefPropWindowsForms
{
    public partial class RC_with_Three_Recuperators_and_Two_Recompressors_with_ReHeating : Form
    {
        public double MixtureCriticalPressure = 0.0;
        public double MixtureCriticalTemperature = 0.0;

        public MainWindow punteroMainWindow = new MainWindow();

        public core luis = new core();

        public Final_Report Final_Report_dialog;

        public Net_Power Net_Power_dialog;

        public Diagrams Diagrams_dialog;

        public Generator Generator_dialog;

        public Estimated_Cost Estimated_Cost_Dialog;

        public PTC_Solar_Field SF_PHX;
        public PTC_Solar_Field SF_RHX;
        public Fresnel SF_PHX_LF;
        public Fresnel SF_RHX_LF;

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

        public Double RHX1_temp_out = 0;
        public Double RHX1_Q = 0;
        public Double RHX1_pres_out = 0;
        public Double DP_RHX1 = 0;
        public Double RHX1_h_in = 0;
        public Double RHX1_h_out = 0;
        public Double RHX2_Q = 0;
        public Double RHX2_pres_out = 0;
        public Double DP_RHX2 = 0;
        public Double RHX2_h_in = 0;
        public Double RHX2_h_out = 0;

        public PreeCooler Precooler_dialog;

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA MT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;
        public Radial_Turbine ReHeating_Turbine;

        //First calculate the Main Compressor Rotational speed and after send that value to the Turbines
        public Double N_design_Main_Compressor;

        public snl_compressor_tsr Main_Compressor;
        public snl_compressor_tsr ReCompressor1;
        public snl_compressor_tsr ReCompressor2;

        //Input Data:
        public RefrigerantCategory category;
        public ReferenceState referencestate;

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

        //ReHeating Solar Field
        public Double PTC_ReHeating_SF_Effective_Apperture_Area;
        public Double LF_ReHeating_SF_Effective_Apperture_Area;

        public Double PTC_ReHeating_Solar_Impinging_flowpath, PTC_ReHeating_Solar_Energy_Absorbed_flowpath, PTC_ReHeating_Energy_Loss_flowpath, PTC_ReHeating_Net_Absorbed_flowpath;
        public Double PTC_ReHeating_Net_Absorbed_SF, PTC_ReHeating_Collector_Efficiency, PTC_ReHeating_SF_Pressure_drop, PTC_ReHeating_calculated_mass_flux, PTC_ReHeating_calculated_Number_Rows;
        public Double PTC_ReHeating_calculated_Row_length;

        public Double PTC_ReHeating_SF_Pump_Calculated_Power, PTC_ReHeating_SF_Pump_isoentropic_eff, PTC_ReHeating_SF_Pump_Hydraulic_Power, PTC_ReHeating_SF_Pump_Mechanical_eff;
        public Double PTC_ReHeating_SF_Pump_Shaft_Work, PTC_ReHeating_SF_Pump_Motor_eff, PTC_ReHeating_SF_Pump_Motor_Elec_Consump, PTC_ReHeating_SF_Pump_Motor_NamePlate_Design, PTC_ReHeating_SF_Pump_Motor_NamePlate;

        public Double LF_ReHeating_Solar_Impinging_flowpath, LF_ReHeating_Solar_Energy_Absorbed_flowpath, LF_ReHeating_Energy_Loss_flowpath, LF_ReHeating_Net_Absorbed_flowpath;
        public Double LF_ReHeating_Net_Absorbed_SF, LF_ReHeating_Collector_Efficiency, LF_ReHeating_SF_Pressure_drop, LF_ReHeating_calculated_mass_flux, LF_ReHeating_calculated_Number_Rows;
        public Double LF_ReHeating_calculated_Row_length;

        public Double LF_ReHeating_SF_Pump_Calculated_Power, LF_ReHeating_SF_Pump_isoentropic_eff, LF_ReHeating_SF_Pump_Hydraulic_Power, LF_ReHeating_SF_Pump_Mechanical_eff;
        public Double LF_ReHeating_SF_Pump_Shaft_Work, LF_ReHeating_SF_Pump_Motor_eff, LF_ReHeating_SF_Pump_Motor_Elec_Consump, LF_ReHeating_SF_Pump_Motor_NamePlate_Design, LF_ReHeating_SF_Pump_Motor_NamePlate;

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

        //ReHeating Heat Exchanger (RHX)
        public Double RHX_Qdot, RHX_Num_HXs, RHX_mdot_c, RHX_mdot_h, RHX_cold_Pin, RHX_cold_Tin, RHX_cold_Pout, RHX_cold_Tout;
        public Double RHX_hot_Pin, RHX_hot_Tin, RHX_hot_Pout, RHX_hot_Tout;
        public Double RHX_UA, RHX_NTU, RHX_CR, RHX_min_DT, RHX_Effectiveness, RHX_Q_per_module, RHX_number_modules, RHX_min_DT_input;
        public Double RHX_mdot_h_module, RHX_mdot_c_module, RHX_UA_module, RHX_NTU_module, RHX_CR_module, RHX_min_DT_module, RHX_Effectiveness_module;

        public Double[] RHX_T_cold = new Double[25];
        public Double[] RHX_T_hot = new Double[25];

        public Double[] RHX_UA_local = new Double[25];
        public Double[] RHX_NTU_local = new Double[25];
        public Double[] RHX_CR_local = new Double[25];
        public Double[] RHX_Effec_local = new Double[25];
        public Double[] RHX_C_dot_c_local = new Double[25];
        public Double[] RHX_C_dot_h_local = new Double[25];

        public Double[] RHX_UA_local_module = new Double[25];
        public Double[] RHX_NTU_local_module = new Double[25];
        public Double[] RHX_CR_local_module = new Double[25];
        public Double[] RHX_Effec_local_module = new Double[25];
        public Double[] RHX_C_dot_c_local_module = new Double[25];
        public Double[] RHX_C_dot_h_local_module = new Double[25];

        //Generator Information
        public Double Generator_Name_Plate_Power, Generator_Power_Output, Generator_Total_Loss;
        public Double Generator_Shaft_Power, Gear_Efficiency, Rating_Point_Efficiency;
        public Double Generator_Mechanical_Loss, Generator_Electrical_Loss, Rating_Design_Point_Load;

        //SF and ACHE Pumps electrical consumption
        public Double Main_SF_Pump_Electrical_Consumption, ReHeating_SF_Pump_Electrical_Consumption;
        public Double UHS_Water_Pump, ACHE_fans;

        //Main Compressor results
        public Double Main_Compressor_Pin, Main_Compressor_Tin, Main_Compressor_Pout, Main_Compressor_Tout;
        public Double Main_Compressor_Flow, Main_Compressor_Diameter, Main_Compressor_Rotation_Velocity;
        public Double Main_Compressor_Efficiency, Main_Compressor_Phi; //Main_Compressor_Phi is the Compressor Flow Factor
        public Boolean Main_Compressor_Surge;

        //Recompressor results
        public Double ReCompressor_Pin, ReCompressor_Tin, ReCompressor_Pout, ReCompressor_Tout;
        public Double ReCompressor_Flow, ReCompressor_Diameter1, ReCompressor_Diameter2, ReCompressor_Rotation_Velocity;
        public Double ReCompressor_Efficiency, ReCompressor_Phi; //Main_Compressor_Phi is the Compressor Flow Factor
        public Boolean ReCompressor_Surge;

        //Main Turbine results
        public Double Main_Turbine_Pin, Main_Turbine_Tin, Main_Turbine_Pout, Main_Turbine_Tout;
        public Double Main_Turbine_Flow, Main_Turbine_Rotary_Velocity, Main_Turbine_Diameter, Main_Turbine_Efficiency, Main_Turbine_Anozzle;

        public Double Main_Turbine_nu, Main_Turbine_w_Tip_Ratio;

        //ReHeating Turbine results
        public Double ReHeating_Turbine_Pin, ReHeating_Turbine_Tin, ReHeating_Turbine_Pout, ReHeating_Turbine_Tout;
        public Double ReHeating_Turbine_Flow, ReHeating_Turbine_Rotary_Velocity, ReHeating_Turbine_Diameter, ReHeating_Turbine_Efficiency, ReHeating_Turbine_Anozzle;
        public Double ReHeating_Turbine_nu, ReHeating_Turbine_w_Tip_Ratio;

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

        //Brayton Power Cycle Results
        public Double specific_work_main_turbine = 0;
        public Double specific_work_reheating_turbine = 0;
        public Double specific_work_compressor1 = 0;
        public Double specific_work_compressor2 = 0;

        public Double Miscellanous_Auxiliaries = 0;
        public Double Total_Auxiliaries = 0;

        public Double w_dot_net2;
        public Double t_mc_in2;
        public Double t_t_in2;
        public Double p_rhx_in2;
        public Double t_rht_in2;
        public Double ua_lt2, ua_mt2, ua_ht2;
        public Double eta_mc2;
        public Double eta_rc1;
        public Double eta_rc2;
        public Double eta_t2;
        public Double eta_trh2;
        public Int64 n_sub_hxrs2;
        public Double p_mc_in2;
        public Double p_mc_out2;
        public Double recomp_frac1;
        public Double recomp_frac2;
        public Double tol2;
        public Double eta_thermal2;
        public Double recompFrac1;
        public Double recompFrac2;

        public Double dp2_lt1, dp2_lt2;
        public Double dp2_mt1, dp2_mt2;
        public Double dp2_ht1, dp2_ht2;
        public Double dp2_pc1, dp2_pc2;
        public Double dp2_phx1, dp2_phx2;
        public Double dp2_rhx1, dp2_rhx2;

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
        public Double MT_mdoth, MT_mdotc, MT_Tcin, MT_Thin, MT_Pcin, MT_Phin;
        public Double LT_Pcout, LT_Phout, LT_Q, HT_mdoth, HT_mdotc, HT_Tcin, HT_Thin;
        public Double MT_Pcout, MT_Phout, MT_Q;
        public Double HT_Pcin, HT_Phin, HT_Pcout, HT_Phout, HT_Q, LT_UA, MT_UA, HT_UA;
        public Double PHX_Q2, PC_Q2;
        public Double LT_Effc, MT_Effc, HT_Effc;
        public Double N_design2 = 0;

        public RC_with_Three_Recuperators_and_Two_Recompressors_with_ReHeating(MainWindow punteroMainWindow1)
        {
            punteroMainWindow = punteroMainWindow1;
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

        //LTR
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

        //HTR
        private void button12_Click(object sender, EventArgs e)
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

        //MTR
        private void button7_Click(object sender, EventArgs e)
        {
            MT_Recuperator = new HeatExchangerUA();
            MT_Recuperator.textBox2.Text = Convert.ToString(MT_Q);
            MT_Recuperator.textBox3.Text = Convert.ToString(MT_mdotc);
            MT_Recuperator.textBox4.Text = Convert.ToString(MT_mdoth);
            MT_Recuperator.textBox7.Text = Convert.ToString(MT_Tcin);
            MT_Recuperator.textBox6.Text = Convert.ToString(MT_Thin);
            MT_Recuperator.textBox5.Text = Convert.ToString(MT_Pcin);
            MT_Recuperator.textBox8.Text = Convert.ToString(MT_Phin);
            MT_Recuperator.textBox9.Text = Convert.ToString(MT_Pcout);
            MT_Recuperator.textBox12.Text = Convert.ToString(MT_Phout);
            MT_Recuperator.textBox13.Text = Convert.ToString(MT_Effc);
            MT_Recuperator.HeatExchangerUA1(luis);
            MT_Recuperator.Calculate_HX();
            MT_Recuperator.Show();
        }
    }
}
