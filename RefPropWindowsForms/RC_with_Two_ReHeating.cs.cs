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
    public partial class RC_with_Two_ReHeating : Form
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
        public Fresnel SF_PHX_LF;
        public Fresnel SF_RHX1_LF;
        public Fresnel SF_RHX2_LF;

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

        //ReHeating Heat Exchanger1 (RHX2)
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

        //Generator Information
        public Double Generator_Name_Plate_Power, Generator_Power_Output, Generator_Total_Loss;
        public Double Generator_Shaft_Power, Gear_Efficiency, Rating_Point_Efficiency;
        public Double Generator_Mechanical_Loss, Generator_Electrical_Loss, Rating_Design_Point_Load;

        //SF and ACHE Pumps electrical consumption
        public Double Main_SF_Pump_Electrical_Consumption, ReHeating1_SF_Pump_Electrical_Consumption, ReHeating2_SF_Pump_Electrical_Consumption;
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
        public Double ua_lt2, ua_ht2;
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_t2;
        public Double eta_trh12;
        public Double eta_trh22;
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
        public Double PHX_Q2, RHX1_Q2, RHX2_Q2, PC_Q2;
        public Double LT_Effc, HT_Effc;
        public Double N_design2 = 0;

        public Double dp2_rhx1;
        public Double dp2_rhx2;

        public Double recomp_frac;
        public Double eta_mc;
        public Double eta_rc;
        public Double eta_t;
        public Double eta_trh1;
        public Double eta_trh2;

        public RC_with_Two_ReHeating()
        {
            InitializeComponent();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            int maxIterations = 100;
            int numIterations = 0;

            //PureFluid
            if (comboBox16.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                luis.core1(this.comboBox1.Text, category);
            }

            //NewMixture
            if (comboBox16.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                luis.core1(this.comboBox2.Text + "=" + textBox60.Text + "," +
                    this.comboBox1.Text + "=" + textBox61.Text + "," +
                    this.comboBox18.Text + "=" + textBox51.Text + "," +
                    this.comboBox17.Text + "=" + textBox80.Text, category);
            }

            if (comboBox16.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
            }

            if (comboBox16.Text == "PseudoPureFluid")
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

            w_dot_net2 = Convert.ToDouble(textBox1.Text);
            t_mc_in2 = Convert.ToDouble(textBox2.Text);
            t_t_in2 = Convert.ToDouble(textBox4.Text);
            p_mc_in2 = Convert.ToDouble(textBox3.Text);
            p_mc_out2 = Convert.ToDouble(textBox8.Text);
            p_rhx1_in2 = Convert.ToDouble(textBox7.Text);
            t_rht1_in2 = Convert.ToDouble(textBox6.Text);
            p_rhx2_in2 = Convert.ToDouble(textBox63.Text);
            t_rht2_in2 = Convert.ToDouble(textBox62.Text);           
            ua_lt2 = Convert.ToDouble(textBox17.Text);
            ua_ht2 = Convert.ToDouble(textBox16.Text);

            dp2_lt1 = Convert.ToDouble(textBox5.Text);
            dp2_lt2 = Convert.ToDouble(textBox26.Text);
            dp2_ht1 = Convert.ToDouble(textBox12.Text);
            dp2_ht2 = Convert.ToDouble(textBox25.Text);
            dp2_pc2 = Convert.ToDouble(textBox11.Text);
            dp2_phx1 = Convert.ToDouble(textBox10.Text);
            dp2_rhx1 = Convert.ToDouble(textBox9.Text);
            dp2_rhx2 = Convert.ToDouble(textBox83.Text);

            recomp_frac = Convert.ToDouble(textBox15.Text);
            eta_mc = Convert.ToDouble(textBox14.Text);
            eta_rc = Convert.ToDouble(textBox13.Text);
            eta_t = Convert.ToDouble(textBox19.Text);
            eta_trh1 = Convert.ToDouble(textBox18.Text);
            eta_trh2 = Convert.ToDouble(textBox88.Text);
            n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            tol2 = Convert.ToDouble(textBox21.Text);

            luis.wmm = luis.working_fluid.MolecularWeight;

            core.RecompCycleTwoReheating cicloRC = new core.RecompCycleTwoReheating();            

            luis.RecompCycledesign_withTwoReheating(luis, ref cicloRC, w_dot_net2, t_mc_in2, t_t_in2, p_mc_in2, p_mc_out2,
                           p_rhx1_in2, t_rht1_in2, p_rhx2_in2, t_rht2_in2, -dp2_lt1, -dp2_ht1, -dp2_pc2, -dp2_phx1, -dp2_rhx1, -dp2_rhx2, -dp2_lt2, -dp2_ht2,
                           ua_lt2, ua_ht2, recomp_frac, eta_mc, eta_rc, eta_t, eta_trh1, eta_trh2, n_sub_hxrs2, tol2);

            massflow2 = cicloRC.m_dot_turbine;
            w_dot_net2 = cicloRC.W_dot_net;
            eta_thermal2 = cicloRC.eta_thermal;
            recomp_frac2 = cicloRC.recomp_frac;
            eta_t2 = eta_t;
            eta_trh12 = eta_trh1;
            eta_trh22 = eta_trh2;
            eta_mc2 = eta_mc;
            eta_rc2 = eta_rc;

            temp21 = cicloRC.temp[0];
            temp22 = cicloRC.temp[1];
            temp23 = cicloRC.temp[2];
            temp24 = cicloRC.temp[3];
            temp25 = cicloRC.temp[4];
            temp26 = cicloRC.temp[5];
            temp27 = cicloRC.temp[6];
            temp28 = cicloRC.temp[7];
            temp29 = cicloRC.temp[8];
            temp210 = cicloRC.temp[9];
            temp211 = cicloRC.temp[10];
            temp212 = cicloRC.temp[11];
            temp213 = cicloRC.temp[12];
            temp214 = cicloRC.temp[13];

            pres21 = cicloRC.pres[0];
            pres22 = cicloRC.pres[1];
            pres23 = cicloRC.pres[2];
            pres24 = cicloRC.pres[3];
            pres25 = cicloRC.pres[4];
            pres26 = cicloRC.pres[5];
            pres27 = cicloRC.pres[6];
            pres28 = cicloRC.pres[7];
            pres29 = cicloRC.pres[8];
            pres210 = cicloRC.pres[9];
            pres211 = cicloRC.pres[10];
            pres212 = cicloRC.pres[11];
            pres213 = cicloRC.pres[12];
            pres214 = cicloRC.pres[13];

            textBox50.Text = Convert.ToString(cicloRC.eta_thermal);

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
            textBox37.Text = Convert.ToString(pres211);
            textBox36.Text = Convert.ToString(pres212);
            textBox85.Text = Convert.ToString(pres213);
            textBox87.Text = Convert.ToString(pres214);

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
            textBox31.Text = Convert.ToString(temp211);
            textBox30.Text = Convert.ToString(temp212);
            textBox84.Text = Convert.ToString(temp213);
            textBox86.Text = Convert.ToString(temp214);

            textBox48.Text = Convert.ToString(w_dot_net2);
            textBox49.Text = Convert.ToString(massflow2);
            textBox50.Text = Convert.ToString(eta_thermal2 * 100);

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

            String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
            String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state,
                point13_state, point14_state;

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

            //High temperature check box
            if (checkBox1.Checked == false)
            {
                PHX_Q2 = cicloRC.PHX.Q_dot;
                RHX1_Q2 = cicloRC.RHX1.Q_dot;
                RHX2_Q2 = cicloRC.RHX2.Q_dot;

                LT_Q = cicloRC.LT.Q_dot;
                LT_mdotc = cicloRC.LT.m_dot_design[0];
                LT_mdoth = cicloRC.LT.m_dot_design[1];
                LT_Tcin = cicloRC.LT.T_c_in;
                LT_Thin = cicloRC.LT.T_h_in;
                LT_Pcin = cicloRC.LT.P_c_in;
                LT_Phin = cicloRC.LT.P_h_in;
                LT_Pcout = cicloRC.LT.P_c_out;
                LT_Phout = cicloRC.LT.P_h_out;
                LT_Effc = cicloRC.LT.eff;

                HT_Q = cicloRC.HT.Q_dot;
                HT_mdotc = cicloRC.HT.m_dot_design[0];
                HT_mdoth = cicloRC.HT.m_dot_design[1];
                HT_Tcin = cicloRC.HT.T_c_in;
                HT_Thin = cicloRC.HT.T_h_in;
                HT_Pcin = cicloRC.HT.P_c_in;
                HT_Phin = cicloRC.HT.P_h_in;
                HT_Pcout = cicloRC.HT.P_c_out;
                HT_Phout = cicloRC.HT.P_h_out;
                HT_Effc = cicloRC.HT.eff;

                PC_Q2 = cicloRC.PC.Q_dot;

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button4.Enabled = false;
                }

                else
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button4.Enabled = true;
                }

                if (comboBox5.Text == "Dual-Loop")
                {
                    //ReHeating SF
                    comboBox12.Enabled = true;
                    comboBox13.Enabled = true;
                    comboBox14.Enabled = true;
                    comboBox15.Enabled = true;
                    button20.Enabled = true;
                    button21.Enabled = true;

                    button8.Enabled = false;
                    button23.Enabled = false;

                }

                else
                {
                    //ReHeaing SF
                    comboBox12.Enabled = false;
                    comboBox13.Enabled = false;
                    comboBox14.Enabled = false;
                    comboBox15.Enabled = false;
                    button20.Enabled = false;
                    button21.Enabled = false;

                    button8.Enabled = true;
                    button23.Enabled = true;
                }

                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button15.Enabled = true;
            }
        }

        private void comboBox16_SelectedValueChanged(object sender, EventArgs e)
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
            }
        }

        //Primary Heat Exchanger and Primary Solar Field
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();

                if (comboBox7.Text == "Solar Salt")
                {
                    SF_PHX.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox7.Text == "Hitec XL")
                {
                    SF_PHX.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox7.Text == "Therminol VP1")
                {
                    SF_PHX.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox7.Text == "Syltherm_800")
                {
                    SF_PHX.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox7.Text == "Dowtherm_A")
                {
                    SF_PHX.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox7.Text == "Therminol_75")
                {
                    SF_PHX.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox7.Text == "Liquid Sodium")
                {
                    SF_PHX.comboBox1.Text = "Liquid Sodium";
                }

                SF_PHX.textBox41.Text = Convert.ToString(PHX_Q2);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(temp25);
                SF_PHX.textBox36.Text = Convert.ToString(pres25);
                SF_PHX.textBox35.Text = Convert.ToString(pres26);
                SF_PHX.textBox37.Text = Convert.ToString(temp26);
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.PTC_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "Main_SF");
                SF_PHX.button3_Click(this, e);
                SF_PHX.Show();
            }

            else if (comboBox4.Text == "Fresnel")
            {
                SF_PHX_LF = new Fresnel();

                if (comboBox7.Text == "Solar Salt")
                {
                    SF_PHX_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox7.Text == "Hitec XL")
                {
                    SF_PHX_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox7.Text == "Therminol VP1")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox7.Text == "Syltherm_800")
                {
                    SF_PHX_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox7.Text == "Dowtherm_A")
                {
                    SF_PHX_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox7.Text == "Therminol_75")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox7.Text == "Liquid Sodium")
                {
                    SF_PHX_LF.comboBox1.Text = "Liquid Sodium";
                }

                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX_Q2);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(temp25);
                SF_PHX_LF.textBox36.Text = Convert.ToString(pres25);
                SF_PHX_LF.textBox35.Text = Convert.ToString(pres26);
                SF_PHX_LF.textBox37.Text = Convert.ToString(temp26);
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.LF_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "Main_SF");
                SF_PHX_LF.Load_ComboBox7();
                SF_PHX_LF.button3_Click(this, e);
                SF_PHX_LF.Show();
            }
        }

        //ReHeating1 Heat Exchanger and ReHeating1 Solar Field
        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox5.Text == "Parabolic")
            {
                SF_RHX1 = new PTC_Solar_Field();

                if (comboBox6.Text == "Solar Salt")
                {
                    SF_RHX1.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox6.Text == "Hitec XL")
                {
                    SF_RHX1.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox6.Text == "Therminol VP1")
                {
                    SF_RHX1.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox6.Text == "Syltherm_800")
                {
                    SF_RHX1.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox6.Text == "Dowtherm_A")
                {
                    SF_RHX1.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox6.Text == "Therminol_75")
                {
                    SF_RHX1.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox6.Text == "Liquid Sodium")
                {
                    SF_RHX1.comboBox1.Text = "Liquid Sodium";
                }

                SF_RHX1.textBox41.Text = Convert.ToString(RHX1_Q2);
                SF_RHX1.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX1.textBox38.Text = Convert.ToString(temp211);
                SF_RHX1.textBox36.Text = Convert.ToString(pres211);
                SF_RHX1.textBox35.Text = Convert.ToString(pres212);
                SF_RHX1.PTC_Solar_Field_uno(luis);
                SF_RHX1.PTC_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating1_SF");
                SF_RHX1.button3_Click(this, e);
                SF_RHX1.Show();
            }

            else if (comboBox5.Text == "Fresnel")
            {
                SF_RHX1_LF = new Fresnel();

                if (comboBox6.Text == "Solar Salt")
                {
                    SF_RHX1_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox6.Text == "Hitec XL")
                {
                    SF_RHX1_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox6.Text == "Therminol VP1")
                {
                    SF_RHX1_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox6.Text == "Syltherm_800")
                {
                    SF_RHX1_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox6.Text == "Dowtherm_A")
                {
                    SF_RHX1_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox6.Text == "Therminol_75")
                {
                    SF_RHX1_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox6.Text == "Liquid Sodium")
                {
                    SF_RHX1_LF.comboBox1.Text = "Liquid Sodium";
                }

                SF_RHX1_LF.textBox41.Text = Convert.ToString(RHX1_Q2);
                SF_RHX1_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX1_LF.textBox38.Text = Convert.ToString(temp211);
                SF_RHX1_LF.textBox36.Text = Convert.ToString(pres211);
                SF_RHX1_LF.textBox35.Text = Convert.ToString(pres212);
                SF_RHX1_LF.LF_Solar_Field_uno(luis);
                SF_RHX1_LF.LF_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating1_SF");
                SF_RHX1_LF.Load_ComboBox7();
                SF_RHX1_LF.button3_Click(this, e);
                SF_RHX1_LF.Show();
            }
        }

        //ReHeating2 Heat Exchanger and ReHeating2 Solar Field
        private void button23_Click(object sender, EventArgs e)
        {
            if (comboBox19.Text == "Parabolic")
            {
                SF_RHX2 = new PTC_Solar_Field();

                if (comboBox20.Text == "Solar Salt")
                {
                    SF_RHX2.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox20.Text == "Hitec XL")
                {
                    SF_RHX2.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox20.Text == "Therminol VP1")
                {
                    SF_RHX2.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox20.Text == "Syltherm_800")
                {
                    SF_RHX2.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox20.Text == "Dowtherm_A")
                {
                    SF_RHX2.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox20.Text == "Therminol_75")
                {
                    SF_RHX2.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox20.Text == "Liquid Sodium")
                {
                    SF_RHX2.comboBox1.Text = "Liquid Sodium";
                }

                SF_RHX2.textBox41.Text = Convert.ToString(RHX2_Q2);
                SF_RHX2.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX2.textBox38.Text = Convert.ToString(temp213);
                SF_RHX2.textBox36.Text = Convert.ToString(pres213);
                SF_RHX2.textBox35.Text = Convert.ToString(pres214);
                SF_RHX2.PTC_Solar_Field_uno(luis);
                SF_RHX2.PTC_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating2_SF");
                SF_RHX2.button3_Click(this, e);
                SF_RHX2.Show();
            }

            else if (comboBox19.Text == "Fresnel")
            {
                SF_RHX2_LF = new Fresnel();

                if (comboBox20.Text == "Solar Salt")
                {
                    SF_RHX2_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox20.Text == "Hitec XL")
                {
                    SF_RHX2_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox20.Text == "Therminol VP1")
                {
                    SF_RHX2_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox20.Text == "Syltherm_800")
                {
                    SF_RHX2_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox20.Text == "Dowtherm_A")
                {
                    SF_RHX2_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox20.Text == "Therminol_75")
                {
                    SF_RHX2_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox20.Text == "Liquid Sodium")
                {
                    SF_RHX2_LF.comboBox1.Text = "Liquid Sodium";
                }

                SF_RHX2_LF.textBox41.Text = Convert.ToString(RHX2_Q2);
                SF_RHX2_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX2_LF.textBox38.Text = Convert.ToString(temp213);
                SF_RHX2_LF.textBox36.Text = Convert.ToString(pres213);
                SF_RHX2_LF.textBox35.Text = Convert.ToString(pres214);
                SF_RHX2_LF.LF_Solar_Field_uno(luis);
                SF_RHX2_LF.LF_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating2_SF");
                SF_RHX2_LF.Load_ComboBox7();
                SF_RHX2_LF.button3_Click(this, e);
                SF_RHX2_LF.Show();
            }
        }

        //Low Temperature Recuperator design
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

        //Hight Temperature Recuperator design
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

        //PreCooler design
        private void button5_Click(object sender, EventArgs e)
        {
            Precooler_dialog = new PreeCooler();
            Precooler_dialog.textBox2.Text = Convert.ToString(PC_Q2);
            Precooler_dialog.textBox4.Text = Convert.ToString(massflow2 * (1 - recomp_frac2));
            Precooler_dialog.textBox6.Text = Convert.ToString(temp29);
            Precooler_dialog.textBox12.Text = Convert.ToString(pres29);
            Precooler_dialog.textBox8.Text = Convert.ToString(pres21);
            Precooler_dialog.PreeCooler1(luis);
            Precooler_dialog.Calculate_Cooler();
            Precooler_dialog.Show();
        }

        //Main Compressor
        private void button9_Click(object sender, EventArgs e)
        {
            button10.Enabled = true;
            button11.Enabled = true;
            button31.Enabled = true;

            Main_Compressor = new snl_compressor_tsr();
            Main_Compressor.textBox1.Text = Convert.ToString(pres21);
            Main_Compressor.textBox2.Text = Convert.ToString(temp21);
            Main_Compressor.textBox6.Text = Convert.ToString(pres22);
            Main_Compressor.textBox5.Text = Convert.ToString(temp22);
            Main_Compressor.textBox9.Text = Convert.ToString(massflow2);
            Main_Compressor.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Compressor.button3.Enabled = false;
            Main_Compressor.button5.Enabled = false;
            Main_Compressor.button6.Enabled = false;
            Main_Compressor.button7.Enabled = false;
            Main_Compressor.Calculate_Main_Compressor();
            N_design_Main_Compressor = Convert.ToDouble(Main_Compressor.textBox11.Text);
            Main_Compressor.Show();
        }

        //Recompressor design
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
            ReCompressor.button3_Click(this, e);
            ReCompressor.Show();
        }

        //Turbine3 design
        private void button31_Click(object sender, EventArgs e)
        {
            ReHeating2_Turbine = new Radial_Turbine();
            ReHeating2_Turbine.textBox1.Text = Convert.ToString(pres214);
            ReHeating2_Turbine.textBox6.Text = Convert.ToString(pres27);
            ReHeating2_Turbine.textBox2.Text = Convert.ToString(temp214);
            ReHeating2_Turbine.textBox5.Text = Convert.ToString(temp27);
            ReHeating2_Turbine.textBox9.Text = Convert.ToString(massflow2);
            ReHeating2_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            ReHeating2_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            // MessageBox.Show("Not forget to set the Turbine Rotation speed (rpm)");
            ReHeating2_Turbine.calculate_Radial_Turbine();
            ReHeating2_Turbine.Show();
        }

        //Turbine2 design
        private void button11_Click(object sender, EventArgs e)
        {
            ReHeating1_Turbine = new Radial_Turbine();
            ReHeating1_Turbine.textBox1.Text = Convert.ToString(pres212);
            ReHeating1_Turbine.textBox6.Text = Convert.ToString(pres213);
            ReHeating1_Turbine.textBox2.Text = Convert.ToString(temp212);
            ReHeating1_Turbine.textBox5.Text = Convert.ToString(temp213);
            ReHeating1_Turbine.textBox9.Text = Convert.ToString(massflow2);
            ReHeating1_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            ReHeating1_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            // MessageBox.Show("Not forget to set the Turbine Rotation speed (rpm)");
            ReHeating1_Turbine.calculate_Radial_Turbine();
            ReHeating1_Turbine.Show();
        }

        //Turbine1 design
        private void button10_Click(object sender, EventArgs e)
        {
            Main_Turbine = new Radial_Turbine();
            Main_Turbine.textBox1.Text = Convert.ToString(pres26);
            Main_Turbine.textBox6.Text = Convert.ToString(pres211);
            Main_Turbine.textBox2.Text = Convert.ToString(temp26);
            Main_Turbine.textBox5.Text = Convert.ToString(temp211);
            Main_Turbine.textBox9.Text = Convert.ToString(massflow2);
            Main_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            // MessageBox.Show("Not forget to set the Turbine Rotation speed (rpm)");
            Main_Turbine.calculate_Radial_Turbine();
            Main_Turbine.Show();
        }

        //Net Power, Net Efficiency
        private void button15_Click(object sender, EventArgs e)
        {
            Net_Power_dialog = new Net_Power();
            Net_Power_dialog.RC_Design_withTwoReHeating(this, 15);

            //Turbines and Compressors Power calculation

            //Main Turbine
            long error_code_main_turbine = 0;
            Double ental_in_main_turbine = 0;
            Double entrop_in_main_turbine = 0;
            Double dens_in_main_turbine = 0;
            Double temp_out_main_turbine = 0;
            Double ental_out_main_turbine = 0;
            Double entrop_out_main_turbine = 0;
            Double dens_out_main_turbine = 0;

            luis.calculate_turbomachine_outlet(luis, temp26, pres26, pres211, eta_t2, false, ref error_code_main_turbine, ref ental_in_main_turbine, ref entrop_in_main_turbine,
                                              ref dens_in_main_turbine, ref temp_out_main_turbine, ref ental_out_main_turbine, ref entrop_out_main_turbine, ref dens_out_main_turbine,
                                              ref specific_work_main_turbine);

            Net_Power_dialog.specific_work_main_turbine = specific_work_main_turbine;

            //ReHeating1 Turbine
            long error_code_reheating1_turbine = 0;
            Double ental_in_reheating1_turbine = 0;
            Double entrop_in_reheating1_turbine = 0;
            Double dens_in_reheating1_turbine = 0;
            Double temp_out_reheating1_turbine = 0;
            Double ental_out_reheating1_turbine = 0;
            Double entrop_out_reheating1_turbine = 0;
            Double dens_out_reheating1_turbine = 0;

            luis.calculate_turbomachine_outlet(luis, temp212, pres212, pres213, eta_trh12, false, ref error_code_reheating1_turbine, ref ental_in_reheating1_turbine, ref entrop_in_reheating1_turbine,
                                              ref dens_in_reheating1_turbine, ref temp_out_reheating1_turbine, ref ental_out_reheating1_turbine, ref entrop_out_reheating1_turbine, ref dens_out_reheating1_turbine,
                                              ref specific_work_reheating1_turbine);

            Net_Power_dialog.specific_work_reheating1_turbine = specific_work_reheating1_turbine;

            //ReHeating1 Turbine
            long error_code_reheating2_turbine = 0;
            Double ental_in_reheating2_turbine = 0;
            Double entrop_in_reheating2_turbine = 0;
            Double dens_in_reheating2_turbine = 0;
            Double temp_out_reheating2_turbine = 0;
            Double ental_out_reheating2_turbine = 0;
            Double entrop_out_reheating2_turbine = 0;
            Double dens_out_reheating2_turbine = 0;

            luis.calculate_turbomachine_outlet(luis, temp214, pres214, pres27, eta_trh22, false, ref error_code_reheating2_turbine, ref ental_in_reheating2_turbine, ref entrop_in_reheating2_turbine,
                                              ref dens_in_reheating2_turbine, ref temp_out_reheating2_turbine, ref ental_out_reheating2_turbine, ref entrop_out_reheating2_turbine, ref dens_out_reheating2_turbine,
                                              ref specific_work_reheating2_turbine);

            Net_Power_dialog.specific_work_reheating2_turbine = specific_work_reheating2_turbine;

            //Compressor Power calculation
            //Compressor1 (Main Compressor)
            long error_code_compressor1 = 0;
            Double ental_in_compressor1 = 0;
            Double entrop_in_compressor1 = 0;
            Double dens_in_compressor1 = 0;
            Double temp_out_compressor1 = 0;
            Double ental_out_compressor1 = 0;
            Double entrop_out_compressor1 = 0;
            Double dens_out_compressor1 = 0;

            luis.calculate_turbomachine_outlet(luis, temp21, pres21, pres22, eta_mc2, true, ref error_code_compressor1, ref ental_in_compressor1, ref entrop_in_compressor1,
                                              ref dens_in_compressor1, ref temp_out_compressor1, ref ental_out_compressor1, ref entrop_out_compressor1, ref dens_out_compressor1,
                                              ref specific_work_compressor1);

            Net_Power_dialog.specific_work_compressor1 = specific_work_compressor1;

            //Compressor2 (Recompressor)
            long error_code_compressor2 = 0;
            Double ental_in_compressor2 = 0;
            Double entrop_in_compressor2 = 0;
            Double dens_in_compressor2 = 0;
            Double temp_out_compressor2 = 0;
            Double ental_out_compressor2 = 0;
            Double entrop_out_compressor2 = 0;
            Double dens_out_compressor2 = 0;

            luis.calculate_turbomachine_outlet(luis, temp29, pres29, pres210, eta_rc2, true, ref error_code_compressor2, ref ental_in_compressor2, ref entrop_in_compressor2,
                                              ref dens_in_compressor2, ref temp_out_compressor2, ref ental_out_compressor2, ref entrop_out_compressor2, ref dens_out_compressor2,
                                              ref specific_work_compressor2);

            Net_Power_dialog.specific_work_compressor2 = specific_work_compressor2;

            //Generator dialogue 
            button19_Click(this, e);
            Generator_dialog.button1_Click(this, e);

            //PHX and Main SF dialogue
            if (comboBox4.Text == "Parabolic")
            {
                button4_Click(this, e);
                SF_PHX.button2_Click(this, e);
                Main_SF_Pump_Electrical_Consumption = SF_PHX.Electrical_Consumption;
            }

            else if (comboBox4.Text == "Fresnel")
            {
                button4_Click(this, e);
                SF_PHX_LF.button77_Click(this, e);
                Main_SF_Pump_Electrical_Consumption = SF_PHX_LF.Electrical_Consumption;
            }

            //RHX1 and ReHeating SF dialogue
            if (comboBox5.Text == "Parabolic")
            {
                button8_Click(this, e);
                SF_RHX1.button2_Click(this, e);
                ReHeating1_SF_Pump_Electrical_Consumption = SF_RHX1.Electrical_Consumption;
            }

            else if (comboBox5.Text == "Fresnel")
            {
                button8_Click(this, e);
                SF_RHX1_LF.button77_Click(this, e);
                ReHeating1_SF_Pump_Electrical_Consumption = SF_RHX1_LF.Electrical_Consumption;
            }

            //RHX2 and ReHeating SF dialogue
            if (comboBox19.Text == "Parabolic")
            {
                button23_Click(this, e);
                SF_RHX2.button2_Click(this, e);
                ReHeating2_SF_Pump_Electrical_Consumption = SF_RHX2.Electrical_Consumption;
            }

            else if (comboBox19.Text == "Fresnel")
            {
                button23_Click(this, e);
                SF_RHX2_LF.button77_Click(this, e);
                ReHeating2_SF_Pump_Electrical_Consumption = SF_RHX2_LF.Electrical_Consumption;
            }

            Net_Power_dialog.button2_Click(this, e);
            Net_Power_dialog.Show();
        }

        //Generator Dialog
        private void button19_Click(object sender, EventArgs e)
        {
            Generator_dialog = new Generator();
            Generator_dialog.Generator_Shaft_Power = w_dot_net2;
            Generator_dialog.RC_Design_withTwoReHeating(this, 15);
            Generator_dialog.button2_Click(this, e);
            Generator_dialog.Show();
        }

        //Heat_Balance results.
        
        private void button3_Click(object sender, EventArgs e)
        {
            //Turbines and Compressors Power calculation

            //Main Turbine
            long error_code_main_turbine = 0;
            Double ental_in_main_turbine = 0;
            Double entrop_in_main_turbine = 0;
            Double dens_in_main_turbine = 0;
            Double temp_out_main_turbine = 0;
            Double ental_out_main_turbine = 0;
            Double entrop_out_main_turbine = 0;
            Double dens_out_main_turbine = 0;

            luis.calculate_turbomachine_outlet(luis, temp26, pres26, pres211, eta_t2, false, ref error_code_main_turbine, ref ental_in_main_turbine, ref entrop_in_main_turbine,
                                              ref dens_in_main_turbine, ref temp_out_main_turbine, ref ental_out_main_turbine, ref entrop_out_main_turbine, ref dens_out_main_turbine,
                                              ref specific_work_main_turbine);

            //ReHeating1 Turbine
            long error_code_reheating1_turbine = 0;
            Double ental_in_reheating1_turbine = 0;
            Double entrop_in_reheating1_turbine = 0;
            Double dens_in_reheating1_turbine = 0;
            Double temp_out_reheating1_turbine = 0;
            Double ental_out_reheating1_turbine = 0;
            Double entrop_out_reheating1_turbine = 0;
            Double dens_out_reheating1_turbine = 0;

            luis.calculate_turbomachine_outlet(luis, temp212, pres212, pres213, eta_trh12, false, ref error_code_reheating1_turbine, ref ental_in_reheating1_turbine, ref entrop_in_reheating1_turbine,
                                              ref dens_in_reheating1_turbine, ref temp_out_reheating1_turbine, ref ental_out_reheating1_turbine, ref entrop_out_reheating1_turbine, ref dens_out_reheating1_turbine,
                                              ref specific_work_reheating1_turbine);

            //ReHeating1 Turbine
            long error_code_reheating2_turbine = 0;
            Double ental_in_reheating2_turbine = 0;
            Double entrop_in_reheating2_turbine = 0;
            Double dens_in_reheating2_turbine = 0;
            Double temp_out_reheating2_turbine = 0;
            Double ental_out_reheating2_turbine = 0;
            Double entrop_out_reheating2_turbine = 0;
            Double dens_out_reheating2_turbine = 0;

            luis.calculate_turbomachine_outlet(luis, temp214, pres214, pres27, eta_trh22, false, ref error_code_reheating2_turbine, ref ental_in_reheating2_turbine, ref entrop_in_reheating2_turbine,
                                              ref dens_in_reheating2_turbine, ref temp_out_reheating2_turbine, ref ental_out_reheating2_turbine, ref entrop_out_reheating2_turbine, ref dens_out_reheating2_turbine,
                                              ref specific_work_reheating2_turbine);

            //Compressor Power calculation
            //Compressor1 (Main Compressor)
            long error_code_compressor1 = 0;
            Double ental_in_compressor1 = 0;
            Double entrop_in_compressor1 = 0;
            Double dens_in_compressor1 = 0;
            Double temp_out_compressor1 = 0;
            Double ental_out_compressor1 = 0;
            Double entrop_out_compressor1 = 0;
            Double dens_out_compressor1 = 0;

            luis.calculate_turbomachine_outlet(luis, temp21, pres21, pres22, eta_mc2, true, ref error_code_compressor1, ref ental_in_compressor1, ref entrop_in_compressor1,
                                              ref dens_in_compressor1, ref temp_out_compressor1, ref ental_out_compressor1, ref entrop_out_compressor1, ref dens_out_compressor1,
                                              ref specific_work_compressor1);

            //Compressor2 (Recompressor)
            long error_code_compressor2 = 0;
            Double ental_in_compressor2 = 0;
            Double entrop_in_compressor2 = 0;
            Double dens_in_compressor2 = 0;
            Double temp_out_compressor2 = 0;
            Double ental_out_compressor2 = 0;
            Double entrop_out_compressor2 = 0;
            Double dens_out_compressor2 = 0;

            luis.calculate_turbomachine_outlet(luis, temp29, pres29, pres210, eta_rc2, true, ref error_code_compressor2, ref ental_in_compressor2, ref entrop_in_compressor2,
                                              ref dens_in_compressor2, ref temp_out_compressor2, ref ental_out_compressor2, ref entrop_out_compressor2, ref dens_out_compressor2,
                                              ref specific_work_compressor2);
            //Main Compressor Dialogue 
            button10.Enabled = true;
            button11.Enabled = true;
            Main_Compressor = new snl_compressor_tsr();
            Main_Compressor.textBox1.Text = Convert.ToString(pres21);
            Main_Compressor.textBox2.Text = Convert.ToString(temp21);
            Main_Compressor.textBox6.Text = Convert.ToString(pres22);
            Main_Compressor.textBox5.Text = Convert.ToString(temp22);
            Main_Compressor.textBox9.Text = Convert.ToString(massflow2);
            Main_Compressor.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Compressor.button3.Enabled = false;
            Main_Compressor.button5.Enabled = false;
            Main_Compressor.button6.Enabled = false;
            Main_Compressor.button7.Enabled = false;
            Main_Compressor.Calculate_Main_Compressor();
            N_design_Main_Compressor = Convert.ToDouble(Main_Compressor.textBox11.Text);

            Main_Compressor_Pin = Convert.ToDouble(Main_Compressor.textBox1.Text);
            Main_Compressor_Tin = Convert.ToDouble(Main_Compressor.textBox2.Text);
            Main_Compressor_Pout = Convert.ToDouble(Main_Compressor.textBox6.Text);
            Main_Compressor_Tout = Convert.ToDouble(Main_Compressor.textBox5.Text);
            Main_Compressor_Flow = Convert.ToDouble(Main_Compressor.textBox9.Text);
            Main_Compressor_Diameter = Convert.ToDouble(Main_Compressor.textBox12.Text);
            Main_Compressor_Rotation_Velocity = Convert.ToDouble(Main_Compressor.textBox11.Text);
            Main_Compressor_Efficiency = Convert.ToDouble(Main_Compressor.textBox10.Text);
            Main_Compressor_Phi = Convert.ToDouble(Main_Compressor.textBox13.Text);
            Main_Compressor_Surge = Convert.ToBoolean(Main_Compressor.textBox14.Text);
            Main_Compressor.Dispose();

            //ReCompressor Dialogue 
            ReCompressor = new snl_compressor_tsr();
            ReCompressor.textBox1.Text = Convert.ToString(pres29);
            ReCompressor.textBox2.Text = Convert.ToString(temp29);
            ReCompressor.textBox6.Text = Convert.ToString(pres210);
            ReCompressor.textBox5.Text = Convert.ToString(temp210);
            ReCompressor.textBox9.Text = Convert.ToString(massflow2);
            ReCompressor.textBox8.Text = Convert.ToString(recomp_frac2);
            ReCompressor.button2.Enabled = false;
            ReCompressor.button4.Enabled = false;
            ReCompressor.button3_Click(this, e);

            ReCompressor_Pin = Convert.ToDouble(ReCompressor.textBox1.Text);
            ReCompressor_Tin = Convert.ToDouble(ReCompressor.textBox2.Text);
            ReCompressor_Pout = Convert.ToDouble(ReCompressor.textBox6.Text);
            ReCompressor_Tout = Convert.ToDouble(ReCompressor.textBox5.Text);
            ReCompressor_Flow = Convert.ToDouble(ReCompressor.textBox9.Text);
            ReCompressor_Diameter1 = Convert.ToDouble(ReCompressor.textBox12.Text);
            ReCompressor_Diameter2 = Convert.ToDouble(ReCompressor.textBox3.Text);
            ReCompressor_Rotation_Velocity = Convert.ToDouble(ReCompressor.textBox11.Text);
            ReCompressor_Efficiency = Convert.ToDouble(ReCompressor.textBox10.Text);
            ReCompressor_Phi = Convert.ToDouble(ReCompressor.textBox13.Text);
            ReCompressor_Surge = Convert.ToBoolean(ReCompressor.textBox14.Text);
            ReCompressor.Dispose();

            //Main Turbine
            Main_Turbine = new Radial_Turbine();
            Main_Turbine.textBox1.Text = Convert.ToString(pres26);
            Main_Turbine.textBox6.Text = Convert.ToString(pres211);
            Main_Turbine.textBox2.Text = Convert.ToString(temp26);
            Main_Turbine.textBox5.Text = Convert.ToString(temp211);
            Main_Turbine.textBox9.Text = Convert.ToString(massflow2);
            Main_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            // MessageBox.Show("Not forget to set the Turbine Rotation speed (rpm)");
            Main_Turbine.calculate_Radial_Turbine();

            Main_Turbine_Pin = Convert.ToDouble(Main_Turbine.textBox1.Text);
            Main_Turbine_Tin = Convert.ToDouble(Main_Turbine.textBox2.Text);
            Main_Turbine_Pout = Convert.ToDouble(Main_Turbine.textBox6.Text);
            Main_Turbine_Tout = Convert.ToDouble(Main_Turbine.textBox5.Text);
            Main_Turbine_Rotary_Velocity = Convert.ToDouble(Main_Turbine.textBox3.Text);
            Main_Turbine_Flow = Convert.ToDouble(Main_Turbine.textBox9.Text);
            Main_Turbine_Diameter = Convert.ToDouble(Main_Turbine.textBox12.Text);
            Main_Turbine_Efficiency = Convert.ToDouble(Main_Turbine.textBox10.Text);
            Main_Turbine_Anozzle = Convert.ToDouble(Main_Turbine.textBox13.Text);
            Main_Turbine_nu = Convert.ToDouble(Main_Turbine.textBox14.Text);
            Main_Turbine_w_Tip_Ratio = Convert.ToDouble(Main_Turbine.textBox16.Text);
            Main_Turbine.Dispose();

            //ReHeating1 Turbine
            ReHeating1_Turbine = new Radial_Turbine();
            ReHeating1_Turbine.textBox1.Text = Convert.ToString(pres212);
            ReHeating1_Turbine.textBox6.Text = Convert.ToString(pres213);
            ReHeating1_Turbine.textBox2.Text = Convert.ToString(temp212);
            ReHeating1_Turbine.textBox5.Text = Convert.ToString(temp213);
            ReHeating1_Turbine.textBox9.Text = Convert.ToString(massflow2);
            ReHeating1_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            ReHeating1_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            // MessageBox.Show("Not forget to set the Turbine Rotation speed (rpm)");
            ReHeating1_Turbine.calculate_Radial_Turbine();

            ReHeating1_Turbine_Pin = Convert.ToDouble(ReHeating1_Turbine.textBox1.Text);
            ReHeating1_Turbine_Tin = Convert.ToDouble(ReHeating1_Turbine.textBox2.Text);
            ReHeating1_Turbine_Pout = Convert.ToDouble(ReHeating1_Turbine.textBox6.Text);
            ReHeating1_Turbine_Tout = Convert.ToDouble(ReHeating1_Turbine.textBox5.Text);
            ReHeating1_Turbine_Rotary_Velocity = Convert.ToDouble(ReHeating1_Turbine.textBox3.Text);
            ReHeating1_Turbine_Flow = Convert.ToDouble(ReHeating1_Turbine.textBox9.Text);
            ReHeating1_Turbine_Diameter = Convert.ToDouble(ReHeating1_Turbine.textBox12.Text);
            ReHeating1_Turbine_Efficiency = Convert.ToDouble(ReHeating1_Turbine.textBox10.Text);
            ReHeating1_Turbine_Anozzle = Convert.ToDouble(ReHeating1_Turbine.textBox13.Text);
            ReHeating1_Turbine_nu = Convert.ToDouble(ReHeating1_Turbine.textBox14.Text);
            ReHeating1_Turbine_w_Tip_Ratio = Convert.ToDouble(ReHeating1_Turbine.textBox16.Text);
            ReHeating1_Turbine.Dispose();

            //ReHeating2 Turbine
            ReHeating2_Turbine = new Radial_Turbine();
            ReHeating2_Turbine.textBox1.Text = Convert.ToString(pres214);
            ReHeating2_Turbine.textBox6.Text = Convert.ToString(pres27);
            ReHeating2_Turbine.textBox2.Text = Convert.ToString(temp214);
            ReHeating2_Turbine.textBox5.Text = Convert.ToString(temp27);
            ReHeating2_Turbine.textBox9.Text = Convert.ToString(massflow2);
            ReHeating2_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            ReHeating2_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            // MessageBox.Show("Not forget to set the Turbine Rotation speed (rpm)");
            ReHeating2_Turbine.calculate_Radial_Turbine();

            ReHeating2_Turbine_Pin = Convert.ToDouble(ReHeating2_Turbine.textBox1.Text);
            ReHeating2_Turbine_Tin = Convert.ToDouble(ReHeating2_Turbine.textBox2.Text);
            ReHeating2_Turbine_Pout = Convert.ToDouble(ReHeating2_Turbine.textBox6.Text);
            ReHeating2_Turbine_Tout = Convert.ToDouble(ReHeating2_Turbine.textBox5.Text);
            ReHeating2_Turbine_Rotary_Velocity = Convert.ToDouble(ReHeating2_Turbine.textBox3.Text);
            ReHeating2_Turbine_Flow = Convert.ToDouble(ReHeating2_Turbine.textBox9.Text);
            ReHeating2_Turbine_Diameter = Convert.ToDouble(ReHeating2_Turbine.textBox12.Text);
            ReHeating2_Turbine_Efficiency = Convert.ToDouble(ReHeating2_Turbine.textBox10.Text);
            ReHeating2_Turbine_Anozzle = Convert.ToDouble(ReHeating2_Turbine.textBox13.Text);
            ReHeating2_Turbine_nu = Convert.ToDouble(ReHeating2_Turbine.textBox14.Text);
            ReHeating2_Turbine_w_Tip_Ratio = Convert.ToDouble(ReHeating2_Turbine.textBox16.Text);
            ReHeating2_Turbine.Dispose();

            //Low Temperature Recuperator (LTR) 
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

            LTR_cold_Pin = Convert.ToDouble(LT_Recuperator.textBox5.Text);
            LTR_cold_Tin = Convert.ToDouble(LT_Recuperator.textBox7.Text);
            LTR_hot_Pin = Convert.ToDouble(LT_Recuperator.textBox8.Text);
            LTR_hot_Tin = Convert.ToDouble(LT_Recuperator.textBox6.Text);
            LTR_cold_Pout = Convert.ToDouble(LT_Recuperator.textBox9.Text);
            LTR_hot_Pout = Convert.ToDouble(LT_Recuperator.textBox12.Text);
            LTR_mdot_c = Convert.ToDouble(LT_Recuperator.textBox3.Text);
            LTR_mdot_h = Convert.ToDouble(LT_Recuperator.textBox4.Text);
            LTR_Qdot = Convert.ToDouble(LT_Recuperator.textBox2.Text);
            LTR_Num_HXs = Convert.ToDouble(LT_Recuperator.textBox1.Text);
            LTR_UA = Convert.ToDouble(LT_Recuperator.textBox11.Text);
            LTR_NTU = Convert.ToDouble(LT_Recuperator.textBox31.Text);
            //LTR_CR = Convert.ToDouble(LT_Recuperator.textBox32.Text);
            LTR_Effectiveness = Convert.ToDouble(LT_Recuperator.textBox13.Text);
            LTR_min_DT = Convert.ToDouble(LT_Recuperator.textBox10.Text);
            LTR_number_modules = Convert.ToDouble(LT_Recuperator.textBox27.Text);
            LTR_Q_per_module = Convert.ToDouble(LT_Recuperator.textBox28.Text);
            LTR_mdot_c_module = Convert.ToDouble(LT_Recuperator.textBox36.Text);
            LTR_mdot_h_module = Convert.ToDouble(LT_Recuperator.textBox35.Text);
            LTR_UA_module = Convert.ToDouble(LT_Recuperator.textBox39.Text);
            LTR_NTU_module = Convert.ToDouble(LT_Recuperator.textBox34.Text);
            //LTR_CR_module = Convert.ToDouble(LT_Recuperator.textBox33.Text);
            LTR_Effectiveness_module = Convert.ToDouble(LT_Recuperator.textBox37.Text);
            LTR_min_DT_module = Convert.ToDouble(LT_Recuperator.textBox38.Text);
            LT_Recuperator.Dispose();

            //High Temperature Recuperator (HTR)
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

            HTR_cold_Pin = Convert.ToDouble(HT_Recuperator.textBox5.Text);
            HTR_cold_Tin = Convert.ToDouble(HT_Recuperator.textBox7.Text);
            HTR_hot_Pin = Convert.ToDouble(HT_Recuperator.textBox8.Text);
            HTR_hot_Tin = Convert.ToDouble(HT_Recuperator.textBox6.Text);
            HTR_cold_Pout = Convert.ToDouble(HT_Recuperator.textBox9.Text);
            HTR_hot_Pout = Convert.ToDouble(HT_Recuperator.textBox12.Text);
            HTR_mdot_c = Convert.ToDouble(HT_Recuperator.textBox3.Text);
            HTR_mdot_h = Convert.ToDouble(HT_Recuperator.textBox4.Text);
            HTR_Qdot = Convert.ToDouble(HT_Recuperator.textBox2.Text);
            HTR_Num_HXs = Convert.ToDouble(HT_Recuperator.textBox1.Text);
            HTR_UA = Convert.ToDouble(HT_Recuperator.textBox11.Text);
            HTR_NTU = Convert.ToDouble(HT_Recuperator.textBox31.Text);
            //HTR_CR = Convert.ToDouble(HT_Recuperator.textBox32.Text);
            HTR_Effectiveness = Convert.ToDouble(HT_Recuperator.textBox13.Text);
            HTR_min_DT = Convert.ToDouble(HT_Recuperator.textBox10.Text);
            HTR_number_modules = Convert.ToDouble(HT_Recuperator.textBox27.Text);
            HTR_Q_per_module = Convert.ToDouble(HT_Recuperator.textBox28.Text);
            HTR_mdot_c_module = Convert.ToDouble(HT_Recuperator.textBox36.Text);
            HTR_mdot_h_module = Convert.ToDouble(HT_Recuperator.textBox35.Text);
            HTR_UA_module = Convert.ToDouble(HT_Recuperator.textBox39.Text);
            HTR_NTU_module = Convert.ToDouble(HT_Recuperator.textBox34.Text);
            //HTR_CR_module = Convert.ToDouble(HT_Recuperator.textBox33.Text);
            HTR_Effectiveness_module = Convert.ToDouble(HT_Recuperator.textBox37.Text);
            HTR_min_DT_module = Convert.ToDouble(HT_Recuperator.textBox38.Text);
            HT_Recuperator.Dispose();

            //Main SF Solar Collector Type
            if (comboBox4.Text == "Parabolic")
            {
                Collector_Type_Main_SF = "Parabolic";
            }

            else if (comboBox4.Text == "Fresnel")
            {
                Collector_Type_Main_SF = "Fresnel";
            }

            //Reheating1 SF Collector Type
            if (comboBox5.Text == "Parabolic")
            {
                Collector_Type_ReHeating1_SF = "Parabolic";
            }

            else if (comboBox5.Text == "Fresnel")
            {
                Collector_Type_ReHeating1_SF = "Fresnel";
            }

            //Reheating2 SF Collector Type
            if (comboBox19.Text == "Parabolic")
            {
                Collector_Type_ReHeating2_SF = "Parabolic";
            }

            else if (comboBox19.Text == "Fresnel")
            {
                Collector_Type_ReHeating2_SF = "Fresnel";
            }

            //Primary Heat Exchanger (PHX)
            if (comboBox4.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();
                SF_PHX.textBox41.Text = Convert.ToString(PHX_Q2);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(temp25);
                SF_PHX.textBox36.Text = Convert.ToString(pres25);
                SF_PHX.textBox35.Text = Convert.ToString(pres26);
                SF_PHX.textBox37.Text = Convert.ToString(temp26);
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.PTC_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "Main_SF");
                SF_PHX.button3_Click(this, e);
            }

            else if (comboBox4.Text == "Fresnel")
            {
                SF_PHX_LF = new Fresnel();
                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX_Q2);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(temp25);
                SF_PHX_LF.textBox36.Text = Convert.ToString(pres25);
                SF_PHX_LF.textBox35.Text = Convert.ToString(pres26);
                SF_PHX_LF.textBox37.Text = Convert.ToString(temp26);
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.LF_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "Main_SF");
                SF_PHX_LF.Load_ComboBox7();
                SF_PHX_LF.button3_Click(this, e);
            }

            //Main Solar Field 
            if (comboBox4.Text == "Parabolic")
            {
                PHX_cold_Pin = Convert.ToDouble(SF_PHX.textBox36.Text);
                PHX_cold_Tin = Convert.ToDouble(SF_PHX.textBox38.Text);
                PHX_hot_Pin = Convert.ToDouble(SF_PHX.textBox34.Text);
                PHX_hot_Tin = Convert.ToDouble(SF_PHX.textBox37.Text);
                PHX_cold_Pout = Convert.ToDouble(SF_PHX.textBox35.Text);
                PHX_hot_Pout = Convert.ToDouble(SF_PHX.textBox33.Text);
                PHX_mdot_c = Convert.ToDouble(SF_PHX.textBox40.Text);
                PHX_mdot_h = Convert.ToDouble(SF_PHX.textBox39.Text);
                PHX_Qdot = Convert.ToDouble(SF_PHX.textBox41.Text);
                PHX_Num_HXs = Convert.ToDouble(SF_PHX.textBox42.Text);
                PHX_UA = Convert.ToDouble(SF_PHX.textBox45.Text);
                PHX_NTU = Convert.ToDouble(SF_PHX.textBox110.Text);
                PHX_CR = Convert.ToDouble(SF_PHX.textBox109.Text);
                PHX_Effectiveness = Convert.ToDouble(SF_PHX.textBox43.Text);
                PHX_min_DT = Convert.ToDouble(SF_PHX.textBox44.Text);
                PHX_number_modules = Convert.ToDouble(SF_PHX.textBox80.Text);
                PHX_Q_per_module = Convert.ToDouble(SF_PHX.textBox79.Text);
                PHX_min_DT_input = Convert.ToDouble(SF_PHX.textBox107.Text);
                SF_PHX.Dispose();
            }

            //Main Solar Field 
            else if (comboBox4.Text == "Fresnel")
            {
                PHX_cold_Pin = Convert.ToDouble(SF_PHX_LF.textBox36.Text);
                PHX_cold_Tin = Convert.ToDouble(SF_PHX_LF.textBox38.Text);
                PHX_hot_Pin = Convert.ToDouble(SF_PHX_LF.textBox34.Text);
                PHX_hot_Tin = Convert.ToDouble(SF_PHX_LF.textBox37.Text);
                PHX_cold_Pout = Convert.ToDouble(SF_PHX_LF.textBox35.Text);
                PHX_hot_Pout = Convert.ToDouble(SF_PHX_LF.textBox33.Text);
                PHX_mdot_c = Convert.ToDouble(SF_PHX_LF.textBox40.Text);
                PHX_mdot_h = Convert.ToDouble(SF_PHX_LF.textBox39.Text);
                PHX_Qdot = Convert.ToDouble(SF_PHX_LF.textBox41.Text);
                PHX_Num_HXs = Convert.ToDouble(SF_PHX_LF.textBox42.Text);
                PHX_UA = Convert.ToDouble(SF_PHX_LF.textBox45.Text);
                PHX_NTU = Convert.ToDouble(SF_PHX_LF.textBox110.Text);
                PHX_CR = Convert.ToDouble(SF_PHX_LF.textBox109.Text);
                PHX_Effectiveness = Convert.ToDouble(SF_PHX_LF.textBox43.Text);
                PHX_min_DT = Convert.ToDouble(SF_PHX_LF.textBox44.Text);
                PHX_number_modules = Convert.ToDouble(SF_PHX_LF.textBox80.Text);
                PHX_Q_per_module = Convert.ToDouble(SF_PHX_LF.textBox79.Text);
                PHX_min_DT_input = Convert.ToDouble(SF_PHX_LF.textBox107.Text);
                SF_PHX_LF.Dispose();
            }

            //ReHeating1 Heat Exchanger (RHX1)
            if (comboBox5.Text == "Parabolic")
            {
                SF_RHX1 = new PTC_Solar_Field();
                SF_RHX1.textBox41.Text = Convert.ToString(RHX1_Q2);
                SF_RHX1.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX1.textBox38.Text = Convert.ToString(temp211);
                SF_RHX1.textBox36.Text = Convert.ToString(pres211);
                SF_RHX1.textBox35.Text = Convert.ToString(pres212);
                SF_RHX1.PTC_Solar_Field_uno(luis);
                SF_RHX1.PTC_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating1_SF");
                SF_RHX1.button3_Click(this, e);
            }

            else if (comboBox5.Text == "Fresnel")
            {
                SF_RHX1_LF = new Fresnel();
                SF_RHX1_LF.textBox41.Text = Convert.ToString(RHX1_Q2);
                SF_RHX1_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX1_LF.textBox38.Text = Convert.ToString(temp211);
                SF_RHX1_LF.textBox36.Text = Convert.ToString(pres211);
                SF_RHX1_LF.textBox35.Text = Convert.ToString(pres212);
                SF_RHX1_LF.LF_Solar_Field_uno(luis);
                SF_RHX1_LF.LF_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating1_SF");
                SF_RHX1_LF.Load_ComboBox7();
                SF_RHX1_LF.button3_Click(this, e);
            }

            if (comboBox5.Text == "Parabolic")
            {
                RHX1_cold_Pin = Convert.ToDouble(SF_RHX1.textBox36.Text);
                RHX1_cold_Tin = Convert.ToDouble(SF_RHX1.textBox38.Text);
                RHX1_hot_Pin = Convert.ToDouble(SF_RHX1.textBox34.Text);
                RHX1_hot_Tin = Convert.ToDouble(SF_RHX1.textBox37.Text);
                RHX1_cold_Pout = Convert.ToDouble(SF_RHX1.textBox35.Text);
                RHX1_hot_Pout = Convert.ToDouble(SF_RHX1.textBox33.Text);
                RHX1_mdot_c = Convert.ToDouble(SF_RHX1.textBox40.Text);
                RHX1_mdot_h = Convert.ToDouble(SF_RHX1.textBox39.Text);
                RHX1_Qdot = Convert.ToDouble(SF_RHX1.textBox41.Text);
                RHX1_Num_HXs = Convert.ToDouble(SF_RHX1.textBox42.Text);
                RHX1_UA = Convert.ToDouble(SF_RHX1.textBox45.Text);
                RHX1_NTU = Convert.ToDouble(SF_RHX1.textBox110.Text);
                RHX1_CR = Convert.ToDouble(SF_RHX1.textBox109.Text);
                RHX1_Effectiveness = Convert.ToDouble(SF_RHX1.textBox43.Text);
                RHX1_min_DT = Convert.ToDouble(SF_RHX1.textBox44.Text);
                RHX1_number_modules = Convert.ToDouble(SF_RHX1.textBox80.Text);
                RHX1_Q_per_module = Convert.ToDouble(SF_RHX1.textBox79.Text);
                RHX1_min_DT_input = Convert.ToDouble(SF_RHX1.textBox107.Text);
                SF_RHX1.Dispose();
            }

            else if (comboBox5.Text == "Fresnel")
            {
                RHX1_cold_Pin = Convert.ToDouble(SF_RHX1_LF.textBox36.Text);
                RHX1_cold_Tin = Convert.ToDouble(SF_RHX1_LF.textBox38.Text);
                RHX1_hot_Pin = Convert.ToDouble(SF_RHX1_LF.textBox34.Text);
                RHX1_hot_Tin = Convert.ToDouble(SF_RHX1_LF.textBox37.Text);
                RHX1_cold_Pout = Convert.ToDouble(SF_RHX1_LF.textBox35.Text);
                RHX1_hot_Pout = Convert.ToDouble(SF_RHX1_LF.textBox33.Text);
                RHX1_mdot_c = Convert.ToDouble(SF_RHX1_LF.textBox40.Text);
                RHX1_mdot_h = Convert.ToDouble(SF_RHX1_LF.textBox39.Text);
                RHX1_Qdot = Convert.ToDouble(SF_RHX1_LF.textBox41.Text);
                RHX1_Num_HXs = Convert.ToDouble(SF_RHX1_LF.textBox42.Text);
                RHX1_UA = Convert.ToDouble(SF_RHX1_LF.textBox45.Text);
                RHX1_NTU = Convert.ToDouble(SF_RHX1_LF.textBox110.Text);
                RHX1_CR = Convert.ToDouble(SF_RHX1_LF.textBox109.Text);
                RHX1_Effectiveness = Convert.ToDouble(SF_RHX1_LF.textBox43.Text);
                RHX1_min_DT = Convert.ToDouble(SF_RHX1_LF.textBox44.Text);
                RHX1_number_modules = Convert.ToDouble(SF_RHX1_LF.textBox80.Text);
                RHX1_Q_per_module = Convert.ToDouble(SF_RHX1_LF.textBox79.Text);
                RHX1_min_DT_input = Convert.ToDouble(SF_RHX1_LF.textBox107.Text);
                SF_RHX1_LF.Dispose();
            }

            //ReHeating2 SF
            if (comboBox19.Text == "Parabolic")
            {
                SF_RHX2 = new PTC_Solar_Field();
                SF_RHX2.textBox41.Text = Convert.ToString(RHX1_Q2);
                SF_RHX2.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX2.textBox38.Text = Convert.ToString(temp211);
                SF_RHX2.textBox36.Text = Convert.ToString(pres211);
                SF_RHX2.textBox35.Text = Convert.ToString(pres212);
                SF_RHX2.PTC_Solar_Field_uno(luis);
                SF_RHX2.PTC_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating2_SF");
                SF_RHX2.button3_Click(this, e);
            }

            else if (comboBox19.Text == "Fresnel")
            {
                SF_RHX2_LF = new Fresnel();
                SF_RHX2_LF.textBox41.Text = Convert.ToString(RHX2_Q2);
                SF_RHX2_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX2_LF.textBox38.Text = Convert.ToString(temp211);
                SF_RHX2_LF.textBox36.Text = Convert.ToString(pres211);
                SF_RHX2_LF.textBox35.Text = Convert.ToString(pres212);
                SF_RHX2_LF.LF_Solar_Field_uno(luis);
                SF_RHX2_LF.LF_Solar_Field_ciclo_RC_with_Two_ReHeating(this, 15, "ReHeating2_SF");
                SF_RHX2_LF.Load_ComboBox7();
                SF_RHX2_LF.button3_Click(this, e);
            }

            if (comboBox19.Text == "Parabolic")
            {
                RHX2_cold_Pin = Convert.ToDouble(SF_RHX2.textBox36.Text);
                RHX2_cold_Tin = Convert.ToDouble(SF_RHX2.textBox38.Text);
                RHX2_hot_Pin = Convert.ToDouble(SF_RHX2.textBox34.Text);
                RHX2_hot_Tin = Convert.ToDouble(SF_RHX2.textBox37.Text);
                RHX2_cold_Pout = Convert.ToDouble(SF_RHX2.textBox35.Text);
                RHX2_hot_Pout = Convert.ToDouble(SF_RHX2.textBox33.Text);
                RHX2_mdot_c = Convert.ToDouble(SF_RHX2.textBox40.Text);
                RHX2_mdot_h = Convert.ToDouble(SF_RHX2.textBox39.Text);
                RHX2_Qdot = Convert.ToDouble(SF_RHX2.textBox41.Text);
                RHX2_Num_HXs = Convert.ToDouble(SF_RHX2.textBox42.Text);
                RHX2_UA = Convert.ToDouble(SF_RHX2.textBox45.Text);
                RHX2_NTU = Convert.ToDouble(SF_RHX2.textBox110.Text);
                RHX2_CR = Convert.ToDouble(SF_RHX2.textBox109.Text);
                RHX2_Effectiveness = Convert.ToDouble(SF_RHX2.textBox43.Text);
                RHX2_min_DT = Convert.ToDouble(SF_RHX2.textBox44.Text);
                RHX2_number_modules = Convert.ToDouble(SF_RHX2.textBox80.Text);
                RHX2_Q_per_module = Convert.ToDouble(SF_RHX2.textBox79.Text);
                RHX2_min_DT_input = Convert.ToDouble(SF_RHX2.textBox107.Text);
                SF_RHX2.Dispose();
            }

            else if (comboBox19.Text == "Fresnel")
            {
                RHX2_cold_Pin = Convert.ToDouble(SF_RHX2_LF.textBox36.Text);
                RHX2_cold_Tin = Convert.ToDouble(SF_RHX2_LF.textBox38.Text);
                RHX2_hot_Pin = Convert.ToDouble(SF_RHX2_LF.textBox34.Text);
                RHX2_hot_Tin = Convert.ToDouble(SF_RHX2_LF.textBox37.Text);
                RHX2_cold_Pout = Convert.ToDouble(SF_RHX2_LF.textBox35.Text);
                RHX2_hot_Pout = Convert.ToDouble(SF_RHX2_LF.textBox33.Text);
                RHX2_mdot_c = Convert.ToDouble(SF_RHX2_LF.textBox40.Text);
                RHX2_mdot_h = Convert.ToDouble(SF_RHX2_LF.textBox39.Text);
                RHX2_Qdot = Convert.ToDouble(SF_RHX2_LF.textBox41.Text);
                RHX2_Num_HXs = Convert.ToDouble(SF_RHX2_LF.textBox42.Text);
                RHX2_UA = Convert.ToDouble(SF_RHX2_LF.textBox45.Text);
                RHX2_NTU = Convert.ToDouble(SF_RHX2_LF.textBox110.Text);
                RHX2_CR = Convert.ToDouble(SF_RHX2_LF.textBox109.Text);
                RHX2_Effectiveness = Convert.ToDouble(SF_RHX2_LF.textBox43.Text);
                RHX2_min_DT = Convert.ToDouble(SF_RHX2_LF.textBox44.Text);
                RHX2_number_modules = Convert.ToDouble(SF_RHX2_LF.textBox80.Text);
                RHX2_Q_per_module = Convert.ToDouble(SF_RHX2_LF.textBox79.Text);
                RHX2_min_DT_input = Convert.ToDouble(SF_RHX2_LF.textBox107.Text);
                SF_RHX2_LF.Dispose();
            }

            //Main Solar Field 
            if (comboBox4.Text == "Parabolic")
            {
                PTC_Main_Solar_Impinging_flowpath = Convert.ToDouble(SF_PHX.textBox49.Text);
                PTC_Main_Solar_Energy_Absorbed_flowpath = Convert.ToDouble(SF_PHX.textBox48.Text);
                PTC_Main_Energy_Loss_flowpath = Convert.ToDouble(SF_PHX.textBox50.Text);
                PTC_Main_Net_Absorbed_flowpath = Convert.ToDouble(SF_PHX.textBox51.Text);
                PTC_Main_Net_Absorbed_SF = Convert.ToDouble(SF_PHX.textBox5.Text);
                PTC_Main_Collector_Efficiency = Convert.ToDouble(SF_PHX.textBox22.Text);
                PTC_Main_SF_Effective_Apperture_Area = Convert.ToDouble(SF_PHX.textBox16.Text);
                PTC_Main_SF_Pressure_drop = Convert.ToDouble(SF_PHX.textBox84.Text);
                PTC_Main_calculated_mass_flux = Convert.ToDouble(SF_PHX.textBox86.Text);
                PTC_Main_calculated_Number_Rows = Convert.ToDouble(SF_PHX.textBox87.Text);
                PTC_Main_calculated_Row_length = Convert.ToDouble(SF_PHX.textBox32.Text);

                PTC_Main_SF_Pump_Calculated_Power = Convert.ToDouble(SF_PHX.textBox88.Text);
                PTC_Main_SF_Pump_isoentropic_eff = Convert.ToDouble(SF_PHX.textBox89.Text);
                PTC_Main_SF_Pump_Hydraulic_Power = Convert.ToDouble(SF_PHX.textBox90.Text);
                PTC_Main_SF_Pump_Mechanical_eff = Convert.ToDouble(SF_PHX.textBox91.Text);
                PTC_Main_SF_Pump_Shaft_Work = Convert.ToDouble(SF_PHX.textBox92.Text);
                PTC_Main_SF_Pump_Motor_eff = Convert.ToDouble(SF_PHX.textBox93.Text);
                PTC_Main_SF_Pump_Motor_Elec_Consump = Convert.ToDouble(SF_PHX.textBox94.Text);
                PTC_Main_SF_Pump_Motor_NamePlate_Design = Convert.ToDouble(SF_PHX.textBox95.Text);
                PTC_Main_SF_Pump_Motor_NamePlate = Convert.ToDouble(SF_PHX.textBox96.Text);
            }

            else if (comboBox4.Text == "Fresnel")
            {
                LF_Main_Solar_Impinging_flowpath = Convert.ToDouble(SF_PHX_LF.textBox49.Text);
                LF_Main_Solar_Energy_Absorbed_flowpath = Convert.ToDouble(SF_PHX_LF.textBox48.Text);
                LF_Main_Energy_Loss_flowpath = Convert.ToDouble(SF_PHX_LF.textBox50.Text);
                LF_Main_Net_Absorbed_flowpath = Convert.ToDouble(SF_PHX_LF.textBox51.Text);
                LF_Main_Net_Absorbed_SF = Convert.ToDouble(SF_PHX_LF.textBox5.Text);
                LF_Main_Collector_Efficiency = Convert.ToDouble(SF_PHX_LF.textBox22.Text);
                LF_Main_SF_Effective_Apperture_Area = Convert.ToDouble(SF_PHX_LF.textBox16.Text);
                LF_Main_SF_Pressure_drop = Convert.ToDouble(SF_PHX_LF.textBox84.Text);
                LF_Main_calculated_mass_flux = Convert.ToDouble(SF_PHX_LF.textBox86.Text);
                LF_Main_calculated_Number_Rows = Convert.ToDouble(SF_PHX_LF.textBox87.Text);
                LF_Main_calculated_Row_length = Convert.ToDouble(SF_PHX_LF.textBox19.Text);

                LF_Main_SF_Pump_Calculated_Power = Convert.ToDouble(SF_PHX_LF.textBox88.Text);
                LF_Main_SF_Pump_isoentropic_eff = Convert.ToDouble(SF_PHX_LF.textBox89.Text);
                LF_Main_SF_Pump_Hydraulic_Power = Convert.ToDouble(SF_PHX_LF.textBox90.Text);
                LF_Main_SF_Pump_Mechanical_eff = Convert.ToDouble(SF_PHX_LF.textBox91.Text);
                LF_Main_SF_Pump_Shaft_Work = Convert.ToDouble(SF_PHX_LF.textBox92.Text);
                LF_Main_SF_Pump_Motor_eff = Convert.ToDouble(SF_PHX_LF.textBox93.Text);
                LF_Main_SF_Pump_Motor_Elec_Consump = Convert.ToDouble(SF_PHX_LF.textBox94.Text);
                LF_Main_SF_Pump_Motor_NamePlate_Design = Convert.ToDouble(SF_PHX_LF.textBox95.Text);
                LF_Main_SF_Pump_Motor_NamePlate = Convert.ToDouble(SF_PHX_LF.textBox96.Text);
            }

            //ReHeating1 Solar Field
            if (comboBox5.Text == "Parabolic")
            {
                PTC_ReHeating1_Solar_Impinging_flowpath = Convert.ToDouble(SF_RHX1.textBox49.Text);
                PTC_ReHeating1_Solar_Energy_Absorbed_flowpath = Convert.ToDouble(SF_RHX1.textBox48.Text);
                PTC_ReHeating1_Energy_Loss_flowpath = Convert.ToDouble(SF_RHX1.textBox50.Text);
                PTC_ReHeating1_Net_Absorbed_flowpath = Convert.ToDouble(SF_RHX1.textBox51.Text);
                PTC_ReHeating1_Net_Absorbed_SF = Convert.ToDouble(SF_RHX1.textBox5.Text);
                PTC_ReHeating1_Collector_Efficiency = Convert.ToDouble(SF_RHX1.textBox22.Text);
                PTC_ReHeating1_SF_Effective_Apperture_Area = Convert.ToDouble(SF_RHX1.textBox16.Text);
                PTC_ReHeating1_SF_Pressure_drop = Convert.ToDouble(SF_RHX1.textBox84.Text);
                PTC_ReHeating1_calculated_mass_flux = Convert.ToDouble(SF_RHX1.textBox86.Text);
                PTC_ReHeating1_calculated_Number_Rows = Convert.ToDouble(SF_RHX1.textBox87.Text);
                PTC_ReHeating1_calculated_Row_length = Convert.ToDouble(SF_RHX1.textBox32.Text);

                PTC_ReHeating1_SF_Pump_Calculated_Power = Convert.ToDouble(SF_RHX1.textBox88.Text);
                PTC_ReHeating1_SF_Pump_isoentropic_eff = Convert.ToDouble(SF_RHX1.textBox89.Text);
                PTC_ReHeating1_SF_Pump_Hydraulic_Power = Convert.ToDouble(SF_RHX1.textBox90.Text);
                PTC_ReHeating1_SF_Pump_Mechanical_eff = Convert.ToDouble(SF_RHX1.textBox91.Text);
                PTC_ReHeating1_SF_Pump_Shaft_Work = Convert.ToDouble(SF_RHX1.textBox92.Text);
                PTC_ReHeating1_SF_Pump_Motor_eff = Convert.ToDouble(SF_RHX1.textBox93.Text);
                PTC_ReHeating1_SF_Pump_Motor_Elec_Consump = Convert.ToDouble(SF_RHX1.textBox94.Text);
                PTC_ReHeating1_SF_Pump_Motor_NamePlate_Design = Convert.ToDouble(SF_RHX1.textBox95.Text);
                PTC_ReHeating1_SF_Pump_Motor_NamePlate = Convert.ToDouble(SF_RHX1.textBox96.Text);
            }

            else if (comboBox5.Text == "Fresnel")
            {
                LF_ReHeating1_Solar_Impinging_flowpath = Convert.ToDouble(SF_RHX1_LF.textBox49.Text);
                LF_ReHeating1_Solar_Energy_Absorbed_flowpath = Convert.ToDouble(SF_RHX1_LF.textBox48.Text);
                LF_ReHeating1_Energy_Loss_flowpath = Convert.ToDouble(SF_RHX1_LF.textBox50.Text);
                LF_ReHeating1_Net_Absorbed_flowpath = Convert.ToDouble(SF_RHX1_LF.textBox51.Text);
                LF_ReHeating1_Net_Absorbed_SF = Convert.ToDouble(SF_RHX1_LF.textBox5.Text);
                LF_ReHeating1_Collector_Efficiency = Convert.ToDouble(SF_RHX1_LF.textBox22.Text);
                LF_ReHeating1_SF_Effective_Apperture_Area = Convert.ToDouble(SF_RHX1_LF.textBox16.Text);
                LF_ReHeating1_SF_Pressure_drop = Convert.ToDouble(SF_RHX1_LF.textBox84.Text);
                LF_ReHeating1_calculated_mass_flux = Convert.ToDouble(SF_RHX1_LF.textBox86.Text);
                LF_ReHeating1_calculated_Number_Rows = Convert.ToDouble(SF_RHX1_LF.textBox87.Text);
                LF_ReHeating1_calculated_Row_length = Convert.ToDouble(SF_RHX1_LF.textBox19.Text);

                LF_ReHeating1_SF_Pump_Calculated_Power = Convert.ToDouble(SF_RHX1_LF.textBox88.Text);
                LF_ReHeating1_SF_Pump_isoentropic_eff = Convert.ToDouble(SF_RHX1_LF.textBox89.Text);
                LF_ReHeating1_SF_Pump_Hydraulic_Power = Convert.ToDouble(SF_RHX1_LF.textBox90.Text);
                LF_ReHeating1_SF_Pump_Mechanical_eff = Convert.ToDouble(SF_RHX1_LF.textBox91.Text);
                LF_ReHeating1_SF_Pump_Shaft_Work = Convert.ToDouble(SF_RHX1_LF.textBox92.Text);
                LF_ReHeating1_SF_Pump_Motor_eff = Convert.ToDouble(SF_RHX1_LF.textBox93.Text);
                LF_ReHeating1_SF_Pump_Motor_Elec_Consump = Convert.ToDouble(SF_RHX1_LF.textBox94.Text);
                LF_ReHeating1_SF_Pump_Motor_NamePlate_Design = Convert.ToDouble(SF_RHX1_LF.textBox95.Text);
                LF_ReHeating1_SF_Pump_Motor_NamePlate = Convert.ToDouble(SF_RHX1_LF.textBox96.Text);
            }

            //ReHeating2 Solar Field
            if (comboBox19.Text == "Parabolic")
            {
                PTC_ReHeating2_Solar_Impinging_flowpath = Convert.ToDouble(SF_RHX2.textBox49.Text);
                PTC_ReHeating2_Solar_Energy_Absorbed_flowpath = Convert.ToDouble(SF_RHX2.textBox48.Text);
                PTC_ReHeating2_Energy_Loss_flowpath = Convert.ToDouble(SF_RHX2.textBox50.Text);
                PTC_ReHeating2_Net_Absorbed_flowpath = Convert.ToDouble(SF_RHX2.textBox51.Text);
                PTC_ReHeating2_Net_Absorbed_SF = Convert.ToDouble(SF_RHX2.textBox5.Text);
                PTC_ReHeating2_Collector_Efficiency = Convert.ToDouble(SF_RHX2.textBox22.Text);
                PTC_ReHeating2_SF_Effective_Apperture_Area = Convert.ToDouble(SF_RHX2.textBox16.Text);
                PTC_ReHeating2_SF_Pressure_drop = Convert.ToDouble(SF_RHX2.textBox84.Text);
                PTC_ReHeating2_calculated_mass_flux = Convert.ToDouble(SF_RHX2.textBox86.Text);
                PTC_ReHeating2_calculated_Number_Rows = Convert.ToDouble(SF_RHX2.textBox87.Text);
                PTC_ReHeating2_calculated_Row_length = Convert.ToDouble(SF_RHX2.textBox32.Text);

                PTC_ReHeating2_SF_Pump_Calculated_Power = Convert.ToDouble(SF_RHX2.textBox88.Text);
                PTC_ReHeating2_SF_Pump_isoentropic_eff = Convert.ToDouble(SF_RHX2.textBox89.Text);
                PTC_ReHeating2_SF_Pump_Hydraulic_Power = Convert.ToDouble(SF_RHX2.textBox90.Text);
                PTC_ReHeating2_SF_Pump_Mechanical_eff = Convert.ToDouble(SF_RHX2.textBox91.Text);
                PTC_ReHeating2_SF_Pump_Shaft_Work = Convert.ToDouble(SF_RHX2.textBox92.Text);
                PTC_ReHeating2_SF_Pump_Motor_eff = Convert.ToDouble(SF_RHX2.textBox93.Text);
                PTC_ReHeating2_SF_Pump_Motor_Elec_Consump = Convert.ToDouble(SF_RHX2.textBox94.Text);
                PTC_ReHeating2_SF_Pump_Motor_NamePlate_Design = Convert.ToDouble(SF_RHX2.textBox95.Text);
                PTC_ReHeating2_SF_Pump_Motor_NamePlate = Convert.ToDouble(SF_RHX2.textBox96.Text);
            }

            else if (comboBox19.Text == "Fresnel")
            {
                LF_ReHeating2_Solar_Impinging_flowpath = Convert.ToDouble(SF_RHX2_LF.textBox49.Text);
                LF_ReHeating2_Solar_Energy_Absorbed_flowpath = Convert.ToDouble(SF_RHX2_LF.textBox48.Text);
                LF_ReHeating2_Energy_Loss_flowpath = Convert.ToDouble(SF_RHX2_LF.textBox50.Text);
                LF_ReHeating2_Net_Absorbed_flowpath = Convert.ToDouble(SF_RHX2_LF.textBox51.Text);
                LF_ReHeating2_Net_Absorbed_SF = Convert.ToDouble(SF_RHX2_LF.textBox5.Text);
                LF_ReHeating2_Collector_Efficiency = Convert.ToDouble(SF_RHX2_LF.textBox22.Text);
                LF_ReHeating2_SF_Effective_Apperture_Area = Convert.ToDouble(SF_RHX2_LF.textBox16.Text);
                LF_ReHeating2_SF_Pressure_drop = Convert.ToDouble(SF_RHX2_LF.textBox84.Text);
                LF_ReHeating2_calculated_mass_flux = Convert.ToDouble(SF_RHX2_LF.textBox86.Text);
                LF_ReHeating2_calculated_Number_Rows = Convert.ToDouble(SF_RHX2_LF.textBox87.Text);
                LF_ReHeating2_calculated_Row_length = Convert.ToDouble(SF_RHX2_LF.textBox19.Text);

                LF_ReHeating2_SF_Pump_Calculated_Power = Convert.ToDouble(SF_RHX2_LF.textBox88.Text);
                LF_ReHeating2_SF_Pump_isoentropic_eff = Convert.ToDouble(SF_RHX2_LF.textBox89.Text);
                LF_ReHeating2_SF_Pump_Hydraulic_Power = Convert.ToDouble(SF_RHX2_LF.textBox90.Text);
                LF_ReHeating2_SF_Pump_Mechanical_eff = Convert.ToDouble(SF_RHX2_LF.textBox91.Text);
                LF_ReHeating2_SF_Pump_Shaft_Work = Convert.ToDouble(SF_RHX2_LF.textBox92.Text);
                LF_ReHeating2_SF_Pump_Motor_eff = Convert.ToDouble(SF_RHX2_LF.textBox93.Text);
                LF_ReHeating2_SF_Pump_Motor_Elec_Consump = Convert.ToDouble(SF_RHX2_LF.textBox94.Text);
                LF_ReHeating2_SF_Pump_Motor_NamePlate_Design = Convert.ToDouble(SF_RHX2_LF.textBox95.Text);
                LF_ReHeating2_SF_Pump_Motor_NamePlate = Convert.ToDouble(SF_RHX2_LF.textBox96.Text);
            }

            //Generator 
            Generator_dialog = new Generator();
            Generator_dialog.Generator_Shaft_Power = w_dot_net2;
            Generator_dialog.RC_Design_withTwoReHeating(this, 15);
            Generator_dialog.button2_Click(this, e);

            Generator_Name_Plate_Power = Generator_dialog.Generator_Name_Plate_Power;
            Rating_Design_Point_Load = Generator_dialog.Rating_Design_Point_Load;
            Generator_Power_Output = Generator_dialog.Generator_Power_Output;
            Generator_Shaft_Power = Generator_dialog.Generator_Shaft_Power;
            Generator_Total_Loss = Generator_dialog.Generator_Total_Loss;
            Generator_Electrical_Loss = Generator_dialog.Generator_Electrical_Loss;
            Generator_Mechanical_Loss = Generator_dialog.Generator_Mechanical_Loss;
            Rating_Point_Efficiency = Generator_dialog.Rating_Point_Efficiency;

            //Auxiliary Energy Losses Dialog
            Net_Power_dialog = new Net_Power();
            Net_Power_dialog.RC_Design_withTwoReHeating(this, 15);
            ACHE_fans = Convert.ToDouble(Net_Power_dialog.textBox2.Text);
            Miscellanous_Auxiliaries = Convert.ToDouble(Net_Power_dialog.textBox13.Text);
            UHS_Water_Pump = Convert.ToDouble(Net_Power_dialog.textBox11.Text);

            if (comboBox4.Text == "Parabolic")
            {
                Main_SF_Pump_Electrical_Consumption = PTC_Main_SF_Pump_Motor_Elec_Consump;
            }

            else if (comboBox4.Text == "Fresnel")
            {
                Main_SF_Pump_Electrical_Consumption = LF_Main_SF_Pump_Motor_Elec_Consump;
            }

            if (comboBox5.Text == "Parabolic")
            {
                ReHeating1_SF_Pump_Electrical_Consumption = PTC_ReHeating1_SF_Pump_Motor_Elec_Consump;
            }

            else if (comboBox5.Text == "Fresnel")
            {
                ReHeating1_SF_Pump_Electrical_Consumption = LF_ReHeating1_SF_Pump_Motor_Elec_Consump;
            }

            if (comboBox19.Text == "Parabolic")
            {
                ReHeating2_SF_Pump_Electrical_Consumption = PTC_ReHeating2_SF_Pump_Motor_Elec_Consump;
            }

            else if (comboBox19.Text == "Fresnel")
            {
                ReHeating2_SF_Pump_Electrical_Consumption = LF_ReHeating2_SF_Pump_Motor_Elec_Consump;
            }

            //Final_Report Dialog Show
            Final_Report_dialog = new Final_Report();
            Final_Report_dialog.Ciclo_RC_Design_withTwoReHeating(this, 15);
            Final_Report_dialog.Show();
        }

        //Clear Lists
        private void button27_Click(object sender, EventArgs e)
        {
            RC_with_Two_ReHeating_Sensing_Analysis_Results RC_with_Two_ReHeating_Sensing_Analysis_Results_window = new RC_with_Two_ReHeating_Sensing_Analysis_Results();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.Show();

            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox1.Items.Clear();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox2.Items.Clear();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox3.Items.Clear();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox4.Items.Clear();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox5.Items.Clear();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox6.Items.Clear();
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

        //Recompression Fract. sensing analysis
        private void button26_Click(object sender, EventArgs e)
        {
            RC_with_Two_ReHeating_Sensing_Analysis_Results RC_with_Two_ReHeating_Sensing_Analysis_Results_window = new RC_with_Two_ReHeating_Sensing_Analysis_Results();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.Show();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.label96.Text = "Recomp. Frac.";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text, ReferenceState.DEF);

            int counter = 4;

            //Escritura de Archivo de Excel
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

            xlWorkSheet1.Name = this.comboBox2.Text + " Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

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

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //Recomp.Fraction variation
                for (double i = Convert.ToDouble(textBox82.Text); i <= Convert.ToDouble(textBox81.Text); i = i + Convert.ToDouble(textBox70.Text))
                {
                    textBox15.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button22_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox15.Text);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres21;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox15.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_RC_with_Two_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //CIT sensing analysis
        private void button25_Click(object sender, EventArgs e)
        {
            RC_with_Two_ReHeating_Sensing_Analysis_Results RC_with_Two_ReHeating_Sensing_Analysis_Results_window = new RC_with_Two_ReHeating_Sensing_Analysis_Results();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.Show();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.label96.Text = "CIT(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text, ReferenceState.DEF);

            int counter = 4;

            //Escritura de Archivo de Excel
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

            xlWorkSheet1.Name = this.comboBox2.Text + " Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIT(K)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //Recomp.Fraction variation
                for (double i = Convert.ToDouble(textBox69.Text); i <= Convert.ToDouble(textBox68.Text); i = i + Convert.ToDouble(textBox67.Text))
                {
                    textBox2.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button22_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox2.Text);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres21;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox2.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_RC_with_Two_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //CIP sensing analysis
        private void button24_Click(object sender, EventArgs e)
        {
            RC_with_Two_ReHeating_Sensing_Analysis_Results RC_with_Two_ReHeating_Sensing_Analysis_Results_window = new RC_with_Two_ReHeating_Sensing_Analysis_Results();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.Show();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.label96.Text = "CIP(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text, ReferenceState.DEF);

            int counter = 4;

            //Escritura de Archivo de Excel
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

            xlWorkSheet1.Name = this.comboBox2.Text + " Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //Recomp.Fraction variation
                for (double i = Convert.ToDouble(textBox66.Text); i <= Convert.ToDouble(textBox65.Text); i = i + Convert.ToDouble(textBox64.Text))
                {
                    textBox3.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button22_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox3.Text);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres21;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox3.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_RC_with_Two_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //TIP_Main Turbine sensing analysis
        private void button28_Click(object sender, EventArgs e)
        {
            RC_with_Two_ReHeating_Sensing_Analysis_Results RC_with_Two_ReHeating_Sensing_Analysis_Results_window = new RC_with_Two_ReHeating_Sensing_Analysis_Results();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.Show();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.label96.Text = "TIP(kPa)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text, ReferenceState.DEF);

            int counter = 4;

            //Escritura de Archivo de Excel
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

            xlWorkSheet1.Name = this.comboBox2.Text + " Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "TIP(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //TIP variation
                for (double i = Convert.ToDouble(textBox76.Text); i <= Convert.ToDouble(textBox75.Text); i = i + Convert.ToDouble(textBox74.Text))
                {
                    textBox8.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button22_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox8.Text);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres26;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox3.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_RC_with_Two_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //TIT_Main Turbine sensing analysis
        private void button29_Click(object sender, EventArgs e)
        {
            RC_with_Two_ReHeating_Sensing_Analysis_Results RC_with_Two_ReHeating_Sensing_Analysis_Results_window = new RC_with_Two_ReHeating_Sensing_Analysis_Results();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.Show();
            RC_with_Two_ReHeating_Sensing_Analysis_Results_window.label96.Text = "TIT(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text, ReferenceState.DEF);

            int counter = 4;

            //Escritura de Archivo de Excel
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

            xlWorkSheet1.Name = this.comboBox2.Text + " Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox60.Text + "," + this.comboBox1.Text + "=" + textBox61.Text + "," + this.comboBox18.Text + "=" + textBox51.Text + "," + this.comboBox17.Text + "=" + textBox80.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "TIT(K)";
            xlWorkSheet1.Cells[4, 2] = "CIT(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //TIT variation
                for (double i = Convert.ToDouble(textBox79.Text); i <= Convert.ToDouble(textBox78.Text); i = i + Convert.ToDouble(textBox77.Text))
                {
                    //Main TIT
                    textBox4.Text = Convert.ToString(Convert.ToDecimal(i));
                    //ReHeating TIT
                    //textBox6.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button22_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox4.Text);

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    RC_with_Two_ReHeating_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = temp26;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox3.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_RC_with_Two_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //NLopt Optimizaton button
        private void Button36_Click(object sender, EventArgs e)
        {
            RC_with_Two_ReHeating_Optimization_Analysis_Results RC_with_Two_ReHeating_Optimization_Analysis_Results_window = new RC_with_Two_ReHeating_Optimization_Analysis_Results(this);
            RC_with_Two_ReHeating_Optimization_Analysis_Results_window.Show();
        }

        //Sensing analysis
        private void Button37_Click(object sender, EventArgs e)
        {

        }

        //Set Critical Conditions
        private void Button30_Click(object sender, EventArgs e)
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
    }
}
