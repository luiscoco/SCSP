using System;
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

using sc.net;

using Excel = Microsoft.Office.Interop.Excel;

namespace RefPropWindowsForms
{
    public partial class RC_with_Five_ReHeating : Form
    {
        public double MixtureCriticalPressure = 0.0;
        public double MixtureCriticalTemperature = 0.0;

        public RefrigerantCategory category;
        public ReferenceState referencestate;

        public core luis = new core();


        public Final_Report Final_Report_dialog;

        public Net_Power Net_Power_dialog;

        public Diagrams Diagrams_dialog;

        public Generator Generator_dialog;

        public Estimated_Cost Estimated_Cost_Dialog;

        public PTC_Solar_Field SF_PHX;
        public PTC_Solar_Field SF_RHX1;
        public PTC_Solar_Field SF_RHX2;
        public PTC_Solar_Field SF_RHX3;
        public PTC_Solar_Field SF_RHX4;
        public PTC_Solar_Field SF_RHX5;

        public Fresnel SF_PHX_LF;
        public Fresnel SF_RHX1_LF;
        public Fresnel SF_RHX2_LF;
        public Fresnel SF_RHX3_LF;
        public Fresnel SF_RHX4_LF;
        public Fresnel SF_RHX5_LF;

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
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;
        public Radial_Turbine ReHeating1_Turbine;
        public Radial_Turbine ReHeating2_Turbine;
        public Radial_Turbine ReHeating3_Turbine;
        public Radial_Turbine ReHeating4_Turbine;
        public Radial_Turbine ReHeating5_Turbine;

        //First calculate the Main Compressor Rotational speed and after send that value to the Turbines
        public Double N_design_Main_Compressor;

        public snl_compressor_tsr Main_Compressor;
        public snl_compressor_tsr ReCompressor;

        public Double wmm;

        //STORING RESULTS

        //"Parabolic" or "Fresnel"
        public String Collector_Type_Main_SF;
        public String Collector_Type_ReHeating1_SF;
        public String Collector_Type_ReHeating2_SF;
        public String Collector_Type_ReHeating3_SF;
        public String Collector_Type_ReHeating4_SF;
        public String Collector_Type_ReHeating5_SF;

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

        //ReHeating1 Solar Field
        public Double PTC_ReHeating1_SF_Effective_Apperture_Area;
        public Double LF_ReHeating1_SF_Effective_Apperture_Area;

        public Double PTC_ReHeating1_Solar_Impinging_flowpath, PTC_ReHeating1_Solar_Energy_Absorbed_flowpath, PTC_ReHeating1_Energy_Loss_flowpath, PTC_ReHeating1_Net_Absorbed_flowpath;
        public Double PTC_ReHeating1_Net_Absorbed_SF, PTC_ReHeating1_Collector_Efficiency, PTC_ReHeating1_SF_Pressure_drop, PTC_ReHeating1_calculated_mass_flux, PTC_ReHeating1_calculated_Number_Rows;
        public Double PTC_ReHeating1_calculated_Row_length;

        public Double PTC_ReHeating1_SF_Pump_Calculated_Power, PTC_ReHeating1_SF_Pump_isoentropic_eff, PTC_ReHeating1_SF_Pump_Hydraulic_Power, PTC_ReHeating1_SF_Pump_Mechanical_eff;
        public Double PTC_ReHeating1_SF_Pump_Shaft_Work, PTC_ReHeating1_SF_Pump_Motor_eff, PTC_ReHeating1_SF_Pump_Motor_Elec_Consump, PTC_ReHeating1_SF_Pump_Motor_NamePlate_Design, PTC_ReHeating1_SF_Pump_Motor_NamePlate;

        public Double LF_ReHeating1_Solar_Impinging_flowpath, LF_ReHeating1_Solar_Energy_Absorbed_flowpath, LF_ReHeating1_Energy_Loss_flowpath, LF_ReHeating1_Net_Absorbed_flowpath;
        public Double LF_ReHeating1_Net_Absorbed_SF, LF_ReHeating1_Collector_Efficiency, LF_ReHeating1_SF_Pressure_drop, LF_ReHeating1_calculated_mass_flux, LF_ReHeating1_calculated_Number_Rows;
        public Double LF_ReHeating1_calculated_Row_length;

        public Double LF_ReHeating1_SF_Pump_Calculated_Power, LF_ReHeating1_SF_Pump_isoentropic_eff, LF_ReHeating1_SF_Pump_Hydraulic_Power, LF_ReHeating1_SF_Pump_Mechanical_eff;
        public Double LF_ReHeating1_SF_Pump_Shaft_Work, LF_ReHeating1_SF_Pump_Motor_eff, LF_ReHeating1_SF_Pump_Motor_Elec_Consump, LF_ReHeating1_SF_Pump_Motor_NamePlate_Design, LF_ReHeating1_SF_Pump_Motor_NamePlate;

        //ReHeating2 Solar Field
        public Double PTC_ReHeating2_SF_Effective_Apperture_Area;
        public Double LF_ReHeating2_SF_Effective_Apperture_Area;

        public Double PTC_ReHeating2_Solar_Impinging_flowpath, PTC_ReHeating2_Solar_Energy_Absorbed_flowpath, PTC_ReHeating2_Energy_Loss_flowpath, PTC_ReHeating2_Net_Absorbed_flowpath;
        public Double PTC_ReHeating2_Net_Absorbed_SF, PTC_ReHeating2_Collector_Efficiency, PTC_ReHeating2_SF_Pressure_drop, PTC_ReHeating2_calculated_mass_flux, PTC_ReHeating2_calculated_Number_Rows;
        public Double PTC_ReHeating2_calculated_Row_length;

        public Double PTC_ReHeating2_SF_Pump_Calculated_Power, PTC_ReHeating2_SF_Pump_isoentropic_eff, PTC_ReHeating2_SF_Pump_Hydraulic_Power, PTC_ReHeating2_SF_Pump_Mechanical_eff;
        public Double PTC_ReHeating2_SF_Pump_Shaft_Work, PTC_ReHeating2_SF_Pump_Motor_eff, PTC_ReHeating2_SF_Pump_Motor_Elec_Consump, PTC_ReHeating2_SF_Pump_Motor_NamePlate_Design, PTC_ReHeating2_SF_Pump_Motor_NamePlate;

        public Double LF_ReHeating2_Solar_Impinging_flowpath, LF_ReHeating2_Solar_Energy_Absorbed_flowpath, LF_ReHeating2_Energy_Loss_flowpath, LF_ReHeating2_Net_Absorbed_flowpath;
        public Double LF_ReHeating2_Net_Absorbed_SF, LF_ReHeating2_Collector_Efficiency, LF_ReHeating2_SF_Pressure_drop, LF_ReHeating2_calculated_mass_flux, LF_ReHeating2_calculated_Number_Rows;
        public Double LF_ReHeating2_calculated_Row_length;

        public Double LF_ReHeating2_SF_Pump_Calculated_Power, LF_ReHeating2_SF_Pump_isoentropic_eff, LF_ReHeating2_SF_Pump_Hydraulic_Power, LF_ReHeating2_SF_Pump_Mechanical_eff;
        public Double LF_ReHeating2_SF_Pump_Shaft_Work, LF_ReHeating2_SF_Pump_Motor_eff, LF_ReHeating2_SF_Pump_Motor_Elec_Consump, LF_ReHeating2_SF_Pump_Motor_NamePlate_Design, LF_ReHeating2_SF_Pump_Motor_NamePlate;

        //ReHeating3 Solar Field
        public Double PTC_ReHeating3_SF_Effective_Apperture_Area;
        public Double LF_ReHeating3_SF_Effective_Apperture_Area;

        public Double PTC_ReHeating3_Solar_Impinging_flowpath, PTC_ReHeating3_Solar_Energy_Absorbed_flowpath, PTC_ReHeating3_Energy_Loss_flowpath, PTC_ReHeating3_Net_Absorbed_flowpath;
        public Double PTC_ReHeating3_Net_Absorbed_SF, PTC_ReHeating3_Collector_Efficiency, PTC_ReHeating3_SF_Pressure_drop, PTC_ReHeating3_calculated_mass_flux, PTC_ReHeating3_calculated_Number_Rows;
        public Double PTC_ReHeating3_calculated_Row_length;

        public Double PTC_ReHeating3_SF_Pump_Calculated_Power, PTC_ReHeating3_SF_Pump_isoentropic_eff, PTC_ReHeating3_SF_Pump_Hydraulic_Power, PTC_ReHeating3_SF_Pump_Mechanical_eff;
        public Double PTC_ReHeating3_SF_Pump_Shaft_Work, PTC_ReHeating3_SF_Pump_Motor_eff, PTC_ReHeating3_SF_Pump_Motor_Elec_Consump, PTC_ReHeating3_SF_Pump_Motor_NamePlate_Design, PTC_ReHeating3_SF_Pump_Motor_NamePlate;

        public Double LF_ReHeating3_Solar_Impinging_flowpath, LF_ReHeating3_Solar_Energy_Absorbed_flowpath, LF_ReHeating3_Energy_Loss_flowpath, LF_ReHeating3_Net_Absorbed_flowpath;
        public Double LF_ReHeating3_Net_Absorbed_SF, LF_ReHeating3_Collector_Efficiency, LF_ReHeating3_SF_Pressure_drop, LF_ReHeating3_calculated_mass_flux, LF_ReHeating3_calculated_Number_Rows;
        public Double LF_ReHeating3_calculated_Row_length;

        public Double LF_ReHeating3_SF_Pump_Calculated_Power, LF_ReHeating3_SF_Pump_isoentropic_eff, LF_ReHeating3_SF_Pump_Hydraulic_Power, LF_ReHeating3_SF_Pump_Mechanical_eff;
        public Double LF_ReHeating3_SF_Pump_Shaft_Work, LF_ReHeating3_SF_Pump_Motor_eff, LF_ReHeating3_SF_Pump_Motor_Elec_Consump, LF_ReHeating3_SF_Pump_Motor_NamePlate_Design, LF_ReHeating3_SF_Pump_Motor_NamePlate;

        //ReHeating4 Solar Field
        public Double PTC_ReHeating4_SF_Effective_Apperture_Area;
        public Double LF_ReHeating4_SF_Effective_Apperture_Area;

        public Double PTC_ReHeating4_Solar_Impinging_flowpath, PTC_ReHeating4_Solar_Energy_Absorbed_flowpath, PTC_ReHeating4_Energy_Loss_flowpath, PTC_ReHeating4_Net_Absorbed_flowpath;
        public Double PTC_ReHeating4_Net_Absorbed_SF, PTC_ReHeating4_Collector_Efficiency, PTC_ReHeating4_SF_Pressure_drop, PTC_ReHeating4_calculated_mass_flux, PTC_ReHeating4_calculated_Number_Rows;
        public Double PTC_ReHeating4_calculated_Row_length;

        public Double PTC_ReHeating4_SF_Pump_Calculated_Power, PTC_ReHeating4_SF_Pump_isoentropic_eff, PTC_ReHeating4_SF_Pump_Hydraulic_Power, PTC_ReHeating4_SF_Pump_Mechanical_eff;
        public Double PTC_ReHeating4_SF_Pump_Shaft_Work, PTC_ReHeating4_SF_Pump_Motor_eff, PTC_ReHeating4_SF_Pump_Motor_Elec_Consump, PTC_ReHeating4_SF_Pump_Motor_NamePlate_Design, PTC_ReHeating4_SF_Pump_Motor_NamePlate;

        public Double LF_ReHeating4_Solar_Impinging_flowpath, LF_ReHeating4_Solar_Energy_Absorbed_flowpath, LF_ReHeating4_Energy_Loss_flowpath, LF_ReHeating4_Net_Absorbed_flowpath;
        public Double LF_ReHeating4_Net_Absorbed_SF, LF_ReHeating4_Collector_Efficiency, LF_ReHeating4_SF_Pressure_drop, LF_ReHeating4_calculated_mass_flux, LF_ReHeating4_calculated_Number_Rows;
        public Double LF_ReHeating4_calculated_Row_length;

        public Double LF_ReHeating4_SF_Pump_Calculated_Power, LF_ReHeating4_SF_Pump_isoentropic_eff, LF_ReHeating4_SF_Pump_Hydraulic_Power, LF_ReHeating4_SF_Pump_Mechanical_eff;
        public Double LF_ReHeating4_SF_Pump_Shaft_Work, LF_ReHeating4_SF_Pump_Motor_eff, LF_ReHeating4_SF_Pump_Motor_Elec_Consump, LF_ReHeating4_SF_Pump_Motor_NamePlate_Design, LF_ReHeating4_SF_Pump_Motor_NamePlate;

        //ReHeating5 Solar Field
        public Double PTC_ReHeating5_SF_Effective_Apperture_Area;
        public Double LF_ReHeating5_SF_Effective_Apperture_Area;

        public Double PTC_ReHeating5_Solar_Impinging_flowpath, PTC_ReHeating5_Solar_Energy_Absorbed_flowpath, PTC_ReHeating5_Energy_Loss_flowpath, PTC_ReHeating5_Net_Absorbed_flowpath;
        public Double PTC_ReHeating5_Net_Absorbed_SF, PTC_ReHeating5_Collector_Efficiency, PTC_ReHeating5_SF_Pressure_drop, PTC_ReHeating5_calculated_mass_flux, PTC_ReHeating5_calculated_Number_Rows;
        public Double PTC_ReHeating5_calculated_Row_length;

        public Double PTC_ReHeating5_SF_Pump_Calculated_Power, PTC_ReHeating5_SF_Pump_isoentropic_eff, PTC_ReHeating5_SF_Pump_Hydraulic_Power, PTC_ReHeating5_SF_Pump_Mechanical_eff;
        public Double PTC_ReHeating5_SF_Pump_Shaft_Work, PTC_ReHeating5_SF_Pump_Motor_eff, PTC_ReHeating5_SF_Pump_Motor_Elec_Consump, PTC_ReHeating5_SF_Pump_Motor_NamePlate_Design, PTC_ReHeating5_SF_Pump_Motor_NamePlate;

        public Double LF_ReHeating5_Solar_Impinging_flowpath, LF_ReHeating5_Solar_Energy_Absorbed_flowpath, LF_ReHeating5_Energy_Loss_flowpath, LF_ReHeating5_Net_Absorbed_flowpath;
        public Double LF_ReHeating5_Net_Absorbed_SF, LF_ReHeating5_Collector_Efficiency, LF_ReHeating5_SF_Pressure_drop, LF_ReHeating5_calculated_mass_flux, LF_ReHeating5_calculated_Number_Rows;
        public Double LF_ReHeating5_calculated_Row_length;

        public Double LF_ReHeating5_SF_Pump_Calculated_Power, LF_ReHeating5_SF_Pump_isoentropic_eff, LF_ReHeating5_SF_Pump_Hydraulic_Power, LF_ReHeating5_SF_Pump_Mechanical_eff;
        public Double LF_ReHeating5_SF_Pump_Shaft_Work, LF_ReHeating5_SF_Pump_Motor_eff, LF_ReHeating5_SF_Pump_Motor_Elec_Consump, LF_ReHeating5_SF_Pump_Motor_NamePlate_Design, LF_ReHeating5_SF_Pump_Motor_NamePlate;


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

        //ReHeating Heat Exchanger1 (RHX1)
        public Double RHX1_Qdot, RHX1_Num_HXs, RHX1_mdot_c, RHX1_mdot_h, RHX1_cold_Pin, RHX1_cold_Tin, RHX1_cold_Pout, RHX1_cold_Tout;
        public Double RHX1_hot_Pin, RHX1_hot_Tin, RHX1_hot_Pout, RHX1_hot_Tout;
        public Double RHX1_UA, RHX1_NTU, RHX1_CR, RHX1_min_DT, RHX1_Effectiveness, RHX1_Q_per_module, RHX1_number_modules, RHX1_min_DT_input;
        public Double RHX1_mdot_h_module, RHX1_mdot_c_module, RHX1_UA_module, RHX1_NTU_module, RHX1_CR_module, RHX1_min_DT_module, RHX1_Effectiveness_module;

        public Double[] RHX1_T_cold = new Double[25];
        public Double[] RHX1_T_hot = new Double[25];

        public Double[] RHX1_UA_local = new Double[25];
        public Double[] RHX1_NTU_local = new Double[25];
        public Double[] RHX1_CR_local = new Double[25];
        public Double[] RHX1_Effec_local = new Double[25];
        public Double[] RHX1_C_dot_c_local = new Double[25];
        public Double[] RHX1_C_dot_h_local = new Double[25];

        public Double[] RHX1_UA_local_module = new Double[25];
        public Double[] RHX1_NTU_local_module = new Double[25];
        public Double[] RHX1_CR_local_module = new Double[25];
        public Double[] RHX1_Effec_local_module = new Double[25];
        public Double[] RHX1_C_dot_c_local_module = new Double[25];
        public Double[] RHX1_C_dot_h_local_module = new Double[25];

        //ReHeating Heat Exchanger2 (RHX2)
        public Double RHX2_Qdot, RHX2_Num_HXs, RHX2_mdot_c, RHX2_mdot_h, RHX2_cold_Pin, RHX2_cold_Tin, RHX2_cold_Pout, RHX2_cold_Tout;
        public Double RHX2_hot_Pin, RHX2_hot_Tin, RHX2_hot_Pout, RHX2_hot_Tout;
        public Double RHX2_UA, RHX2_NTU, RHX2_CR, RHX2_min_DT, RHX2_Effectiveness, RHX2_Q_per_module, RHX2_number_modules, RHX2_min_DT_input;
        public Double RHX2_mdot_h_module, RHX2_mdot_c_module, RHX2_UA_module, RHX2_NTU_module, RHX2_CR_module, RHX2_min_DT_module, RHX2_Effectiveness_module;

        public Double[] RHX2_T_cold = new Double[25];
        public Double[] RHX2_T_hot = new Double[25];

        public Double[] RHX2_UA_local = new Double[25];
        public Double[] RHX2_NTU_local = new Double[25];
        public Double[] RHX2_CR_local = new Double[25];
        public Double[] RHX2_Effec_local = new Double[25];
        public Double[] RHX2_C_dot_c_local = new Double[25];
        public Double[] RHX2_C_dot_h_local = new Double[25];

        public Double[] RHX2_UA_local_module = new Double[25];
        public Double[] RHX2_NTU_local_module = new Double[25];
        public Double[] RHX2_CR_local_module = new Double[25];
        public Double[] RHX2_Effec_local_module = new Double[25];
        public Double[] RHX2_C_dot_c_local_module = new Double[25];
        public Double[] RHX2_C_dot_h_local_module = new Double[25];

        //ReHeating Heat Exchanger3 (RHX3)
        public Double RHX3_Qdot, RHX3_Num_HXs, RHX3_mdot_c, RHX3_mdot_h, RHX3_cold_Pin, RHX3_cold_Tin, RHX3_cold_Pout, RHX3_cold_Tout;
        public Double RHX3_hot_Pin, RHX3_hot_Tin, RHX3_hot_Pout, RHX3_hot_Tout;
        public Double RHX3_UA, RHX3_NTU, RHX3_CR, RHX3_min_DT, RHX3_Effectiveness, RHX3_Q_per_module, RHX3_number_modules, RHX3_min_DT_input;

        public Double RHX3_mdot_h_module, RHX3_mdot_c_module, RHX3_UA_module, RHX3_NTU_module, RHX3_CR_module, RHX3_min_DT_module, RHX3_Effectiveness_module;

        public Double[] RHX3_T_cold = new Double[25];
        public Double[] RHX3_T_hot = new Double[25];

        public Double[] RHX3_UA_local = new Double[25];
        public Double[] RHX3_NTU_local = new Double[25];

        public Double[] RHX3_CR_local = new Double[25];

        public Double[] RHX3_Effec_local = new Double[25];
        public Double[] RHX3_C_dot_c_local = new Double[25];

        public Double[] RHX3_C_dot_h_local = new Double[25];

        public Double[] RHX3_UA_local_module = new Double[25];

        public Double[] RHX3_NTU_local_module = new Double[25];

        public Double[] RHX3_CR_local_module = new Double[25];
        public Double[] RHX3_Effec_local_module = new Double[25];

        public Double[] RHX3_C_dot_c_local_module = new Double[25];

        public Double[] RHX3_C_dot_h_local_module = new Double[25];

        //ReHeating Heat Exchanger4 (RHX4)
        public Double RHX4_Qdot, RHX4_Num_HXs, RHX4_mdot_c, RHX4_mdot_h, RHX4_cold_Pin, RHX4_cold_Tin, RHX4_cold_Pout, RHX4_cold_Tout;
        public Double RHX4_hot_Pin, RHX4_hot_Tin, RHX4_hot_Pout, RHX4_hot_Tout;
        public Double RHX4_UA, RHX4_NTU, RHX4_CR, RHX4_min_DT, RHX4_Effectiveness, RHX4_Q_per_module, RHX4_number_modules, RHX4_min_DT_input;

        public Double RHX4_mdot_h_module, RHX4_mdot_c_module, RHX4_UA_module, RHX4_NTU_module, RHX4_CR_module, RHX4_min_DT_module, RHX4_Effectiveness_module;

        public Double[] RHX4_T_cold = new Double[25];
        public Double[] RHX4_T_hot = new Double[25];

        public Double[] RHX4_UA_local = new Double[25];
        public Double[] RHX4_NTU_local = new Double[25];

        public Double[] RHX4_CR_local = new Double[25];

        public Double[] RHX4_Effec_local = new Double[25];
        public Double[] RHX4_C_dot_c_local = new Double[25];

        public Double[] RHX4_C_dot_h_local = new Double[25];

        public Double[] RHX4_UA_local_module = new Double[25];

        public Double[] RHX4_NTU_local_module = new Double[25];

        public Double[] RHX4_CR_local_module = new Double[25];
        public Double[] RHX4_Effec_local_module = new Double[25];

        public Double[] RHX4_C_dot_c_local_module = new Double[25];
        public Double[] RHX4_C_dot_h_local_module = new Double[25];

        //ReHeating Heat Exchanger5 (RHX5)
        public Double RHX5_Qdot, RHX5_Num_HXs, RHX5_mdot_c, RHX5_mdot_h, RHX5_cold_Pin, RHX5_cold_Tin, RHX5_cold_Pout, RHX5_cold_Tout;
        public Double RHX5_hot_Pin, RHX5_hot_Tin, RHX5_hot_Pout, RHX5_hot_Tout;
        public Double RHX5_UA, RHX5_NTU, RHX5_CR, RHX5_min_DT, RHX5_Effectiveness, RHX5_Q_per_module, RHX5_number_modules, RHX5_min_DT_input;

        public Double RHX5_mdot_h_module, RHX5_mdot_c_module, RHX5_UA_module, RHX5_NTU_module, RHX5_CR_module, RHX5_min_DT_module, RHX5_Effectiveness_module;

        public Double[] RHX5_T_cold = new Double[25];
        public Double[] RHX5_T_hot = new Double[25];

        public Double[] RHX5_UA_local = new Double[25];
        public Double[] RHX5_NTU_local = new Double[25];

        public Double[] RHX5_CR_local = new Double[25];

        public Double[] RHX5_Effec_local = new Double[25];
        public Double[] RHX5_C_dot_c_local = new Double[25];

        public Double[] RHX5_C_dot_h_local = new Double[25];

        public Double[] RHX5_UA_local_module = new Double[25];

        public Double[] RHX5_NTU_local_module = new Double[25];

        public Double[] RHX5_CR_local_module = new Double[25];
        public Double[] RHX5_Effec_local_module = new Double[25];

        public Double[] RHX5_C_dot_c_local_module = new Double[25];
        public Double[] RHX5_C_dot_h_local_module = new Double[25];

        //Generator Information
        public Double Generator_Name_Plate_Power, Generator_Power_Output, Generator_Total_Loss;

        public Double Generator_Shaft_Power, Gear_Efficiency, Rating_Point_Efficiency;
        public Double Generator_Mechanical_Loss, Generator_Electrical_Loss, Rating_Design_Point_Load;

        //SF and ACHE Pumps electrical consumption
        public Double Main_SF_Pump_Electrical_Consumption, ReHeating1_SF_Pump_Electrical_Consumption, ReHeating2_SF_Pump_Electrical_Consumption, ReHeating3_SF_Pump_Electrical_Consumption;
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

        //ReHeating1 Turbine results
        public Double ReHeating1_Turbine_Pin, ReHeating1_Turbine_Tin, ReHeating1_Turbine_Pout, ReHeating1_Turbine_Tout;
        public Double ReHeating1_Turbine_Flow, ReHeating1_Turbine_Rotary_Velocity, ReHeating1_Turbine_Diameter, ReHeating1_Turbine_Efficiency, ReHeating1_Turbine_Anozzle;
        public Double ReHeating1_Turbine_nu, ReHeating1_Turbine_w_Tip_Ratio;

        //ReHeating2 Turbine results
        public Double ReHeating2_Turbine_Pin, ReHeating2_Turbine_Tin, ReHeating2_Turbine_Pout, ReHeating2_Turbine_Tout;
        public Double ReHeating2_Turbine_Flow, ReHeating2_Turbine_Rotary_Velocity, ReHeating2_Turbine_Diameter, ReHeating2_Turbine_Efficiency, ReHeating2_Turbine_Anozzle;
        public Double ReHeating2_Turbine_nu, ReHeating2_Turbine_w_Tip_Ratio;

        //ReHeating3 Turbine results
        public Double ReHeating3_Turbine_Pin, ReHeating3_Turbine_Tin, ReHeating3_Turbine_Pout, ReHeating3_Turbine_Tout;
        public Double ReHeating3_Turbine_Flow, ReHeating3_Turbine_Rotary_Velocity, ReHeating3_Turbine_Diameter, ReHeating3_Turbine_Efficiency, ReHeating3_Turbine_Anozzle;
        public Double ReHeating3_Turbine_nu, ReHeating3_Turbine_w_Tip_Ratio;

        //ReHeating4 Turbine results
        public Double ReHeating4_Turbine_Pin, ReHeating4_Turbine_Tin, ReHeating4_Turbine_Pout, ReHeating4_Turbine_Tout;
        public Double ReHeating4_Turbine_Flow, ReHeating4_Turbine_Rotary_Velocity, ReHeating4_Turbine_Diameter, ReHeating4_Turbine_Efficiency, ReHeating4_Turbine_Anozzle;
        public Double ReHeating4_Turbine_nu, ReHeating4_Turbine_w_Tip_Ratio;

        //ReHeating5 Turbine results
        public Double ReHeating5_Turbine_Pin, ReHeating5_Turbine_Tin, ReHeating5_Turbine_Pout, ReHeating5_Turbine_Tout;
        public Double ReHeating5_Turbine_Flow, ReHeating5_Turbine_Rotary_Velocity, ReHeating5_Turbine_Diameter, ReHeating5_Turbine_Efficiency, ReHeating5_Turbine_Anozzle;
        public Double ReHeating5_Turbine_nu, ReHeating5_Turbine_w_Tip_Ratio;

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
        public Double specific_work_reheating1_turbine = 0;
        public Double specific_work_reheating2_turbine = 0;
        public Double specific_work_reheating3_turbine = 0;
        public Double specific_work_reheating4_turbine = 0;
        public Double specific_work_reheating5_turbine = 0;
        public Double specific_work_compressor1 = 0;
        public Double specific_work_compressor2 = 0;

        public Double Miscellanous_Auxiliaries = 0;
        public Double Total_Auxiliaries = 0;

        public Double w_dot_net2;
        public Double t_mc_in2;
        public Double t_t_in2;
        public Double p_rhx1_in2;
        public Double t_rht1_in2;
        public Double p_rhx2_in2;
        public Double t_rht2_in2;
        public Double p_rhx3_in2;
        public Double t_rht3_in2;
        public Double p_rhx4_in2;
        public Double t_rht4_in2;
        public Double p_rhx5_in2;
        public Double t_rht5_in2;
        public Double ua_lt2, ua_ht2;
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_t2;
        public Double eta_trh12;
        public Double eta_trh22;
        public Double eta_trh32;
        public Double eta_trh42;
        public Double eta_trh52;
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
        public Double dp2_rhx11, dp2_rhx12;
        public Double dp2_rhx21, dp2_rhx22;
        public Double dp2_rhx31, dp2_rhx32;
        public Double dp2_rhx41, dp2_rhx42;
        public Double dp2_rhx51, dp2_rhx52;

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
        public Double temp215;
        public Double temp216;
        public Double temp217;
        public Double temp218;
        public Double temp219;
        public Double temp220;

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
        public Double pres215;
        public Double pres216;
        public Double pres217;
        public Double pres218;
        public Double pres219;
        public Double pres220;

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
        public Double enth215;
        public Double enth216;
        public Double enth217;
        public Double enth218;
        public Double enth219;
        public Double enth220;

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
        public Double entr215;
        public Double entr216;
        public Double entr217;
        public Double entr218;
        public Double entr219;
        public Double entr220;

        public Double massflow2;
        public Double LT_mdoth, LT_mdotc, LT_Tcin, LT_Thin, LT_Pcin, LT_Phin;
        public Double LT_Pcout, LT_Phout, LT_Q, HT_mdoth, HT_mdotc, HT_Tcin, HT_Thin;
        public Double HT_Pcin, HT_Phin, HT_Pcout, HT_Phout, HT_Q, LT_UA, HT_UA;
        public Double PHX_Q2, RHX1_Q2, RHX2_Q2, RHX3_Q2, RHX4_Q2, RHX5_Q2, PC_Q2;
        public Double LT_Effc, HT_Effc;
        public Double N_design2 = 0;

        public Double dp2_rhx1;
        public Double dp2_rhx2;
        public Double dp2_rhx3;
        public Double dp2_rhx4;
        public Double dp2_rhx5;

        public Double recomp_frac;
        public Double eta_mc;
        public Double eta_rc;
        public Double eta_t;
        public Double eta_trh1;
        public Double eta_trh2;
        public Double eta_trh3;
        public Double eta_trh4;
        public Double eta_trh5;

        public RC_with_Five_ReHeating()
        {
            InitializeComponent();
        }

        //Set Critical conditions
        private void button30_Click(object sender, EventArgs e)
        {
            double option1 = 0.0;
            double option2 = 0.0;
            double option3 = 0.0;
            double option4 = 0.0;

            option1 = Convert.ToDouble(this.textBox60.Text);
            option2 = Convert.ToDouble(this.textBox61.Text);
            option3 = Convert.ToDouble(this.textBox51.Text);
            option4 = Convert.ToDouble(this.textBox80.Text);

            if ((option1 == 1) || (option2 == 1) || (option3 == 1) || (option4 == 1))
            {
                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture,
                    this.comboBox2.Text + "=" + textBox60.Text + "," +
                    this.comboBox1.Text + "=" + textBox61.Text + "," +
                    this.comboBox18.Text + "=" + textBox51.Text + "," +
                    this.comboBox17.Text + "=" + textBox80.Text, ReferenceState.DEF);

                textBox59.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox58.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox55.Text = Convert.ToString(working_fluid.CriticalDensity);

                textBox3.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox2.Text = Convert.ToString(working_fluid.CriticalTemperature);

                MixtureCriticalPressure = working_fluid.CriticalPressure;
                MixtureCriticalTemperature = working_fluid.CriticalTemperature;
            }

            else
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();

                xlWorkBook = xlApp.Workbooks.Open("C:\\SCSP\\RefPropWindowsForms\\bin\\Debug\\REFPROP.xls");

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(9);

                //Fluids selection
                xlWorkSheet.Cells[13, 6] = this.comboBox2.Text;
                xlWorkSheet.Cells[14, 6] = this.comboBox1.Text;
                xlWorkSheet.Cells[15, 6] = this.comboBox18.Text;
                xlWorkSheet.Cells[16, 6] = this.comboBox17.Text;

                // % Compositions
                xlWorkSheet.Cells[13, 7] = this.textBox60.Text;
                xlWorkSheet.Cells[14, 7] = this.textBox61.Text;
                xlWorkSheet.Cells[15, 7] = this.textBox51.Text;
                xlWorkSheet.Cells[16, 7] = this.textBox80.Text;

                //MessageBox.Show(xlWorkSheet.get_Range("D68", "D68").Value2.ToString());
                this.textBox3.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox2.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();

                MixtureCriticalPressure = xlWorkSheet.get_Range("D69", "D69").Value;
                MixtureCriticalTemperature = xlWorkSheet.get_Range("D68", "D68").Value2;

                this.textBox59.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox58.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox55.Text = xlWorkSheet.get_Range("D70", "D70").Value2.ToString();

                //xlWorkBook.SaveAs("C:\\SCSP_Gitlab\\RefPropWindowsForms\\Copia de REFPROP.xlS", 
                //Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, 
                //Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, 
                //misValue);

                xlWorkBook.Close(false, misValue, misValue);

                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox16.Text == "PureFluid")
            {
                comboBox1.Enabled = false;
                comboBox18.Enabled = false;
                comboBox17.Enabled = false;
                textBox60.Enabled = false;
                textBox61.Enabled = false;
                textBox51.Enabled = false;
                textBox80.Enabled = false;
                button22.Enabled = false;
                button2.Enabled = true;
            }

            else if (comboBox16.Text == "NewMixture")
            {
                comboBox1.Enabled = true;
                comboBox18.Enabled = true;
                comboBox17.Enabled = true;
                textBox60.Enabled = true;
                textBox61.Enabled = true;
                textBox51.Enabled = true;
                textBox80.Enabled = true;
                button22.Enabled = true;
                button2.Enabled = false;

                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text, ReferenceState.DEF);

                textBox59.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox58.Text = Convert.ToString(working_fluid.CriticalTemperature - 273.15);
                textBox55.Text = Convert.ToString(working_fluid.CriticalDensity);

                MixtureCriticalPressure = working_fluid.CriticalPressure;
                MixtureCriticalTemperature = working_fluid.CriticalTemperature;
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

        //Mixtures calculations
        private void button22_Click(object sender, EventArgs e)
        {

        }

        //Optimziation analysis
        private void button36_Click(object sender, EventArgs e)
        {

        }
    }   
}
