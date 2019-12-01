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

using Excel = Microsoft.Office.Interop.Excel;



namespace RefPropWindowsForms
{
    public partial class RC_optimal_without_ReHeating : Form, IDisposable
    {
        public core luis = new core();

        public TOPGEN_V3 TOPGEN_V3_dialog;

        public Net_Power Net_Power_dialog;

        public Diagrams Diagrams_dialog;

        public Final_Report Final_Report_dialog;

        public Generator Generator_dialog;

        public Estimated_Cost Estimated_Cost_Dialog;      

        public PTC_Solar_Field SF_PHX;
        public Fresnel SF_PHX_LF;

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

        public PreeCooler Precooler_dialog;

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public snl_radial_turbine Main_Turbine;
        //public snl_radial_turbine ReHeating_Turbine;

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

        //STORING RESULTS

        //"Parabolic" or "Fresnel"
        public String Collector_Type_Main_SF;

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

        private void RC_optimal_without_ReHeating_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }        

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

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
              
        //public IntPtr pDll;

        //public IntPtr pAddressOfFunctionToCall;
     
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void carbondioxidemixturesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
        //                                        ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
        //                                        ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
        //                                        ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
        //                                        ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
        //                                        ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
        //                                        ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
        //                                        ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
        //                                        ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
        //                                        ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
        //                                        ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        const string refpropDLL_path1 = "RC_CO2_Optimal_Subplex.dll";
        [DllImport(refpropDLL_path1, EntryPoint = "carbondioxidesubplex_", SetLastError = true)]
        public static extern void carbondioxidesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);


        const string refpropDLL_path2 = "RC_CO2_Optimal_Newuoa.dll";
        [DllImport(refpropDLL_path2, EntryPoint = "carbondioxidenewuoa_", SetLastError = true)]
        public static extern void carbondioxidenewuoa_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path3 = "RC_CO2_Optimal_Uobyqa.dll";
        [DllImport(refpropDLL_path3, EntryPoint = "carbondioxideuobyqa_", SetLastError = true)]
        public static extern void carbondioxideuobyqa_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //EHTANE PURE FLUID
        const string refpropDLL_path4 = "RC_Ethane_Optimal.dll";
        [DllImport(refpropDLL_path4, EntryPoint = "ethanesubplex_", SetLastError = true)]
        public static extern void ethanesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);
        //SF6 PURE FLUID
        const string refpropDLL_path5 = "RC_SF6_Optimal.dll";
        [DllImport(refpropDLL_path5, EntryPoint = "sulfurhexafluoridesubplex_", SetLastError = true)]
        public static extern void sulfurhexafluoridesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);
        //XENON PURE FLUID
        const string refpropDLL_path6 = "RC_Xenon_Optimal.dll";
        [DllImport(refpropDLL_path6, EntryPoint = "xenonsubplex_", SetLastError = true)]
        public static extern void xenonsubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);
        //NITROGEN PURE FLUID
        const string refpropDLL_path7 = "RC_Nitrogen_Optimal.dll";
        [DllImport(refpropDLL_path7, EntryPoint = "nitrogensubplex_", SetLastError = true)]
        public static extern void nitrogensubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);
        //METHANE PURE FLUID
        const string refpropDLL_path8 = "RC_Methane_Optimal.dll";
        [DllImport(refpropDLL_path8, EntryPoint = "methanesubplex_", SetLastError = true)]
        public static extern void methanesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+HELIUM (1%, 2.5%, 5%, 10%)
        const string refpropDLL_path9 = "RC_95_CO2_5_He__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path9, EntryPoint = "carbondioxideheliumcincosubplex_", SetLastError = true)]
        public static extern void carbondioxideheliumcincosubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path11 = "RC_975_CO2_25_He__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path11, EntryPoint = "carbondioxideheliumdossubplex_", SetLastError = true)]
        public static extern void carbondioxideheliumdossubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path13 = "RC_90_CO2_10_He__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path13, EntryPoint = "carbondioxideheliumdiezsubplex_", SetLastError = true)]
        public static extern void carbondioxideheliumdiezsubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path14 = "RC_99_CO2_1_He__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path14, EntryPoint = "carbondioxideheliumunosubplex_", SetLastError = true)]
        public static extern void carbondioxideheliumunosubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+ARGON (1%, 2.5%, 5%, 10%)
        const string refpropDLL_path15 = "RC_99_CO2_1_Ar__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path15, EntryPoint = "carbondioxideargonunosubplex_", SetLastError = true)]
        public static extern void carbondioxideargonunosubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path16 = "RC_95_CO2_5_Ar__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path16, EntryPoint = "carbondioxideargoncincosubplex_", SetLastError = true)]
        public static extern void carbondioxideargoncincosubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path17 = "RC_975_CO2_25_Ar__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path17, EntryPoint = "carbondioxideargondossubplex_", SetLastError = true)]
        public static extern void carbondioxideargondossubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path18 = "RC_90_CO2_10_Ar__Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path18, EntryPoint = "carbondioxideargondiezsubplex_", SetLastError = true)]
        public static extern void carbondioxideargondiezsubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        const string refpropDLL_path19 = "RC_99_CO2_1_Butane_Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path19, EntryPoint = "carbondioxidebutaneunosubplex_", SetLastError = true)]
        public static extern void carbondioxidebutaneunosubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path20 = "RC_975_CO2_25_Butane_Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path20, EntryPoint = "carbondioxidebutanedossubplex_", SetLastError = true)]
        public static extern void carbondioxidebutanedossubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path21 = "RC_95_CO2_5_Butane_Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path21, EntryPoint = "carbondioxidebutanecincosubplex_", SetLastError = true)]
        public static extern void carbondioxidebutanecincosubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path22 = "RC_90_CO2_10_Butane_Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path22, EntryPoint = "carbondioxidebutanediezsubplex_", SetLastError = true)]
        public static extern void carbondioxidebutanediezsubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path24 = "RC_99_CO2_1_Methane_Optimal_without_RH_Subplex.dll";
        [DllImport(refpropDLL_path24, EntryPoint = "carbondioxidemethaneunosubplex_", SetLastError = true)]
        public static extern void carbondioxidemethaneunosubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        const string refpropDLL_path23 = "RC_CO2_Mixture_Subplex.dll";
        [DllImport(refpropDLL_path23, EntryPoint = "carbondioxidemixturesubplex_", SetLastError = true)]
        public static extern void carbondioxidemixturesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double ua_rec_total1,
                                                ref Double eta_mc1, ref Double eta_rc1, ref Double eta_t1, ref int n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1,
                                                ref Boolean fixed_p_mc_out1, ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1, ref Double lt_frac_guess1,
                                                ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1, ref Double dp1_lt1, ref Double dp1_lt2, ref Double dp1_ht1,
                                                ref Double dp1_ht2, ref Double dp1_pc1, ref Double dp1_pc2, ref Double dp1_phx1, ref Double dp1_phx2,
                                                ref Double temp1, ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7, ref Double temp8, ref Double temp9,
                                                ref Double temp10, ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6,
                                                ref Double pres7, ref Double pres8, ref Double pres9, ref Double pres10, ref Double massflow,
                                                ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin, ref Double LT_Pcout, ref Double LT_Phout,
                                                ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin, ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout,
                                                ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double N_design, ref Double PHX_Q1, ref Double PC_Q1);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+PENTANE (1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+HEXANE (1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+BENZENE(1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+SF6 (1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+NITROGEN (1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+OXYGEN (1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+CO (1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+XENON (1%, 2.5%, 5%, 10%)



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //MIXTURE sCO2+NEON (1%, 2.5%, 5%, 10%)        



        public RC_optimal_without_ReHeating()
        {
            InitializeComponent();
        }

        public Double specific_work_main_turbine = 0;
        public Double specific_work_reheating_turbine = 0;
        public Double specific_work_compressor1 = 0;
        public Double specific_work_compressor2 = 0;
        public Double Miscellanous_Auxiliaries = 0;
        public Double Total_Auxiliaries = 0;

        public Double w_dot_net2;
        public Double t_mc_in2;
        public Double t_t_in2;
        public Double ua_rec_total2;
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_t2;
        public int n_sub_hxrs2;
        public Double p_high_limit2;
        public Double p_mc_out_guess2;
        public Boolean fixed_p_mc_out2;
        public Double pr_mc_guess2;
        public Boolean fixed_pr_mc2;
        public Double recomp_frac_guess2;
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

        public Double PHX_Q2, PC_Q2;

        //SUBPLEX Optimization
        private void button2_Click(object sender, EventArgs e)
        {



            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Loading the required DLL
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //pDll = NativeMethods.LoadLibrary("RC_CO2_Mixture_Subplex.dll");
            //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "carbondioxidemixturesubplex_");

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //PureFluid
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                luis.core1(this.comboBox13.Text, category);
            }

            //NewMixture
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                luis.core1(this.comboBox13.Text + "=" + textBox31.Text + "," + this.comboBox14.Text + "=" + textBox36.Text, category);
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
            recomp_frac_guess2 = Convert.ToDouble(numericUpDown1.Value);
                //Convert.ToDouble(textBox16.Text);
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

            if ((comboBox13.Text == "CO2") && (category== RefrigerantCategory.PureFluid))
            {
                carbondioxidesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic")|| (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }


            else if (comboBox13.Text == "ETHANE")
            {
                ethanesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "SF6")
            {
                sulfurhexafluoridesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "XENON")
            {
                xenonsubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "NITROGEN")
            {

                nitrogensubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "METHANE")
            {

                methanesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.95,HELIUM=0.05")
            {

                carbondioxideheliumcincosubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }
                      
            else if (comboBox13.Text == "CO2=0.975,HELIUM=0.025")
            {

                carbondioxideheliumdossubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.90,HELIUM=0.10")
            {

                carbondioxideheliumdiezsubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.99,HELIUM=0.01")
            {

                carbondioxideheliumunosubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.99,ARGON=0.01")
            {

                carbondioxideargonunosubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.95,ARGON=0.05")
            {

                carbondioxideargoncincosubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.975,ARGON=0.025")
            {

                carbondioxideargondossubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.90,ARGON=0.10")
            {

                carbondioxideargondiezsubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.99,BUTANE=0.01")
            {
                carbondioxidebutaneunosubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.975,BUTANE=0.025")
            {
                carbondioxidebutanedossubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.95,BUTANE=0.05")
            {
                carbondioxidebutanecincosubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (comboBox13.Text == "CO2=0.90,BUTANE=0.10")
            {
                carbondioxidebutanediezsubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }

            else if (category == RefrigerantCategory.NewMixture)
            {
                //carbondioxidemixturesubplex_ carbondioxidemixturesubplex_ = (carbondioxidemixturesubplex_)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(carbondioxidemixturesubplex_));

                carbondioxidemixturesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;                
            }

            else if (comboBox13.Text == "CO2=0.99,METHANE=0.01")
            {
                carbondioxidemethaneunosubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
            base.Dispose();
        }

        //Main Compressor Detail Design
        private void button9_Click(object sender, EventArgs e)
        {
            button10.Enabled = true;

            Main_Compressor = new snl_compressor_tsr();
            Main_Compressor.textBox1.Text = Convert.ToString(pres21);
            Main_Compressor.textBox2.Text = Convert.ToString(temp21);
            Main_Compressor.textBox6.Text = Convert.ToString(pres22);
            Main_Compressor.textBox5.Text = Convert.ToString(temp22);

            Main_Compressor.textBox9.Text = Convert.ToString(massflow2);
            Main_Compressor.textBox8.Text = Convert.ToString(recomp_frac_guess2);

            Main_Compressor.button3.Enabled = false;
            Main_Compressor.button5.Enabled = false;
            Main_Compressor.button6.Enabled = false;
            Main_Compressor.button7.Enabled = false;

            Main_Compressor.Calculate_Main_Compressor();

            N_design_Main_Compressor = Convert.ToDouble(Main_Compressor.textBox11.Text);

            Main_Compressor.Show();
        }

        //Recompressor Detail Design
        private void button12_Click(object sender, EventArgs e)
        {
            ReCompressor = new snl_compressor_tsr();
            ReCompressor.textBox1.Text = Convert.ToString(pres29);
            ReCompressor.textBox2.Text = Convert.ToString(temp29);
            ReCompressor.textBox6.Text = Convert.ToString(pres210);
            ReCompressor.textBox5.Text = Convert.ToString(temp210);

            ReCompressor.textBox9.Text = Convert.ToString(massflow2);
            ReCompressor.textBox8.Text = Convert.ToString(recomp_frac_guess2);

            ReCompressor.button2.Enabled = false;
            ReCompressor.button4.Enabled = false;

            ReCompressor.Show();
        }

        //Main Turbine Detail Design
        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox16.Text == "Dyreby_Turbine (SNL)")
            {
                Main_Turbine = new snl_radial_turbine();
                Main_Turbine.textBox1.Text = Convert.ToString(pres26);
                Main_Turbine.textBox6.Text = Convert.ToString(pres27);
                Main_Turbine.textBox2.Text = Convert.ToString(temp26);
                Main_Turbine.textBox5.Text = Convert.ToString(temp27);

                Main_Turbine.textBox9.Text = Convert.ToString(massflow2);
                Main_Turbine.textBox8.Text = Convert.ToString(recomp_frac_guess2);

                Main_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);

                // MessageBox.Show("Not forget to set the Turbine Rotation speed (rpm)");

                Main_Turbine.calculate_snl_Radial_Turbine();
                Main_Turbine.Show();
            }

            else if (comboBox16.Text == "Jianhui Qi (Radial)")
            {
                //Main Turbine
                long error_code_main_turbine = 0;
                Double ental_in_main_turbine = 0;
                Double entrop_in_main_turbine = 0;
                Double dens_in_main_turbine = 0;
                Double temp_out_main_turbine = 0;
                Double ental_out_main_turbine = 0;
                Double entrop_out_main_turbine = 0;
                Double dens_out_main_turbine = 0;

                luis.calculate_turbomachine_outlet(luis, temp26, pres26, pres27, eta_t2, false, ref error_code_main_turbine, ref ental_in_main_turbine, ref entrop_in_main_turbine,
                                                  ref dens_in_main_turbine, ref temp_out_main_turbine, ref ental_out_main_turbine, ref entrop_out_main_turbine, ref dens_out_main_turbine,
                                                  ref specific_work_main_turbine);

                TOPGEN_V3_dialog = new TOPGEN_V3();
                TOPGEN_V3_dialog.textBox1.Text = Convert.ToString(pres26);
                TOPGEN_V3_dialog.textBox2.Text = Convert.ToString(temp26);
                TOPGEN_V3_dialog.textBox18.Text = Convert.ToString(pres27);
                TOPGEN_V3_dialog.textBox17.Text = Convert.ToString(temp27);
                TOPGEN_V3_dialog.textBox20.Text = Convert.ToString(specific_work_main_turbine*massflow2);
                TOPGEN_V3_dialog.Sending_Pointer_RC_Optimization_withoutReHeating(this,3);
                TOPGEN_V3_dialog.Show();
            }
        }

        //HTR Detail Design
        private void button13_Click(object sender, EventArgs e)
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

        //HTR Detail Design
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

        //NEWUOA Optimization
        private void button6_Click(object sender, EventArgs e)
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

            luis.core1(this.comboBox13.Text, category);
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
            recomp_frac_guess2 = Convert.ToDouble(numericUpDown1.Value);
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

            if (comboBox13.Text == "CO2")
            {
                carbondioxidenewuoa_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }
        }

        //UOBYQA Optimization
        private void button4_Click(object sender, EventArgs e)
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

            luis.core1(this.comboBox13.Text, category);
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
            recomp_frac_guess2 = Convert.ToDouble(numericUpDown1.Value);
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

            if (comboBox13.Text == "CO2")
            {
                carbondioxideuobyqa_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2,
                     ref ua_rec_total2, ref eta_mc2, ref eta_rc2, ref eta_t2, ref n_sub_hxrs2, ref p_high_limit2,
                     ref p_mc_out_guess2, ref fixed_p_mc_out2, ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2,
                     ref fixed_recomp_frac2, ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                     ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1, ref dp2_phx2,
                     ref temp21, ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                     ref temp28, ref temp29, ref temp210, ref pres21, ref pres22, ref pres23, ref pres24,
                     ref pres25, ref pres26, ref pres27, ref pres28, ref pres29, ref pres210, ref massflow2,
                     ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin, ref LT_Pcout, ref LT_Phout,
                     ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin, ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout,
                     ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref N_design2, ref PHX_Q2, ref PC_Q2);

                textBox49.Text = Convert.ToString(massflow2);
                textBox49.BackColor = Color.Yellow;

                numericUpDown1.Value = Convert.ToDecimal(recomp_frac_guess2);
                numericUpDown1.BackColor = Color.Yellow;

                textBox62.Text = Convert.ToString(lt_frac_guess2);
                textBox62.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_mc_out_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox60.Text = Convert.ToString(pr_mc_guess2);
                textBox60.BackColor = Color.Yellow;

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

                if (comboBox4.Text == "Dual-Loop")
                {
                    //Main SF
                    comboBox9.Enabled = true;
                    comboBox11.Enabled = true;
                    comboBox8.Enabled = true;
                    comboBox10.Enabled = true;
                    button17.Enabled = true;
                    button18.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                {
                    //Main SF
                    comboBox9.Enabled = false;
                    comboBox11.Enabled = false;
                    comboBox8.Enabled = false;
                    comboBox10.Enabled = false;
                    button17.Enabled = false;
                    button18.Enabled = false;

                    button5.Enabled = true;
                }

                button8.Enabled = true;
                button7.Enabled = true;
                button13.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button16.Enabled = true;
            }
        }

        //Heat Balance Results
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

            luis.calculate_turbomachine_outlet(luis, temp26, pres26, pres27, eta_t2, false, ref error_code_main_turbine, ref ental_in_main_turbine, ref entrop_in_main_turbine,
                                              ref dens_in_main_turbine, ref temp_out_main_turbine, ref ental_out_main_turbine, ref entrop_out_main_turbine, ref dens_out_main_turbine,
                                              ref specific_work_main_turbine);

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

            //Main Compressor
            button10.Enabled = true;
            Main_Compressor = new snl_compressor_tsr();
            Main_Compressor.textBox1.Text = Convert.ToString(pres21);
            Main_Compressor.textBox2.Text = Convert.ToString(temp21);
            Main_Compressor.textBox6.Text = Convert.ToString(pres22);
            Main_Compressor.textBox5.Text = Convert.ToString(temp22);
            Main_Compressor.textBox9.Text = Convert.ToString(massflow2);
            Main_Compressor.textBox8.Text = Convert.ToString(recomp_frac_guess2);
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

            //Recompressor
            ReCompressor = new snl_compressor_tsr();
            ReCompressor.textBox1.Text = Convert.ToString(pres29);
            ReCompressor.textBox2.Text = Convert.ToString(temp29);
            ReCompressor.textBox6.Text = Convert.ToString(pres210);
            ReCompressor.textBox5.Text = Convert.ToString(temp210);
            ReCompressor.textBox9.Text = Convert.ToString(massflow2);
            ReCompressor.textBox8.Text = Convert.ToString(recomp_frac_guess2);
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
            
            //Turbine
            Main_Turbine = new snl_radial_turbine();
            Main_Turbine.textBox1.Text = Convert.ToString(pres26);
            Main_Turbine.textBox6.Text = Convert.ToString(pres27);
            Main_Turbine.textBox2.Text = Convert.ToString(temp26);
            Main_Turbine.textBox5.Text = Convert.ToString(temp27);
            Main_Turbine.textBox9.Text = Convert.ToString(massflow2);
            Main_Turbine.textBox8.Text = Convert.ToString(recomp_frac_guess2);
            Main_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            Main_Turbine.calculate_snl_Radial_Turbine();

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

            //LTR
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

            //HTR
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

            //Solar Collector Type
            if (comboBox4.Text == "Parabolic")
            {
                Collector_Type_Main_SF = "Parabolic";
            }

            else if (comboBox4.Text == "Fresnel")
            {
                Collector_Type_Main_SF = "Fresnel";
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
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.PTC_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF");
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
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.LF_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF");
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

            //Generator 
            Generator_dialog = new Generator();
            Generator_dialog.Generator_Shaft_Power = w_dot_net2;
            Generator_dialog.RC_Optimization_withoutReHeating(this, 3);
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
            Net_Power_dialog.RC_Optimization_withoutReHeating(this, 3);
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

            //Final_Report Dialog Show
            Final_Report_dialog = new Final_Report();
            Final_Report_dialog.Ciclo_RC_Optimization_withoutReHeating(this, 3);
            Final_Report_dialog.Show();   
        }

        //RESET Button
        private void button14_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            //w_dot_net2 = Convert.ToDouble(textBox1.Text);
            textBox1.Text = "50000";
            //t_mc_in2 = Convert.ToDouble(textBox2.Text);
            textBox2.Text = "305.15";
            //t_t_in2 = Convert.ToDouble(textBox4.Text);
            comboBox5_SelectedValueChanged(this,e);
            //ua_rec_total2 = Convert.ToDouble(textBox17.Text);
            textBox17.Text = "10000";
            //eta_mc2 = Convert.ToDouble(textBox14.Text);
            textBox14.Text = "0.89";
            //eta_rc2 = Convert.ToDouble(textBox13.Text);
            textBox13.Text = "0.89";
            //eta_t2 = Convert.ToDouble(textBox19.Text);
            textBox19.Text = "0.93";
            //n_sub_hxrs2 = Convert.ToInt16(textBox20.Text);
            textBox20.Text = "15";
            //p_high_limit2 = Convert.ToDouble(textBox57.Text);
            textBox57.Text = "25000";
            //p_mc_out_guess2 = Convert.ToDouble(textBox7.Text);
            textBox7.Text = "25000";
            //fixed_p_mc_out2 = Convert.ToBoolean(textBox58.Text);
            textBox58.Text = "True";
            //pr_mc_guess2 = Convert.ToDouble(textBox60.Text);
            textBox60.Text = "1.0";
            //fixed_pr_mc2 = Convert.ToBoolean(textBox59.Text);
            textBox59.Text = "False";
            //recomp_frac_guess2 = Convert.ToDouble(textBox16.Text);
            numericUpDown1.Value = 0.1M;
            //fixed_recomp_frac2 = Convert.ToBoolean(textBox15.Text);
            textBox15.Text = "False";
            //lt_frac_guess2 = Convert.ToDouble(textBox62.Text);
            textBox62.Text = "0.1";
            //fixed_lt_frac2 = Convert.ToBoolean(textBox61.Text);
            textBox61.Text = "False";
            //tol2 = Convert.ToDouble(textBox21.Text);
            textBox21.Text = "0.00001";
            //opt_tol2 = Convert.ToDouble(textBox63.Text);
            textBox63.Text = "0.000001";
            //dp2_lt1 = Convert.ToDouble(textBox5.Text);
            textBox5.Text = "0.0";
            //dp2_lt2 = Convert.ToDouble(textBox26.Text);
            textBox26.Text = "0.0";
            //dp2_ht1 = Convert.ToDouble(textBox12.Text);
            textBox12.Text = "0.0";
            //dp2_ht2 = Convert.ToDouble(textBox25.Text);
            textBox25.Text = "0.0";
            //dp2_pc1 = 0.0;
            
            //dp2_pc2 = Convert.ToDouble(textBox11.Text);
            textBox11.Text = "0.0";
            //dp2_phx1 = Convert.ToDouble(textBox10.Text);
            textBox10.Text = "0.0";
            //dp2_phx2 = 0.0;
            

            textBox47.Text = "";
            textBox46.Text = "";
            textBox45.Text = "";
            textBox44.Text = "";
            textBox43.Text = "";
            textBox42.Text = "";
            textBox35.Text = "";
            textBox34.Text = "";
            textBox33.Text = "";
            textBox32.Text = "";

            textBox22.Text = "";
            textBox23.Text = "";
            textBox27.Text = "";
            textBox24.Text = "";
            textBox29.Text = "";
            textBox28.Text = "";
            textBox41.Text = "";
            textBox40.Text = "";
            textBox39.Text = "";
            textBox38.Text = "";

            textBox49.Text = "";
            textBox50.Text = "";
        }

        //SF and PHX Design
        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();

                if (comboBox5.Text == "Solar Salt")
                {
                    SF_PHX.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox5.Text == "Hitec XL")
                {
                    SF_PHX.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox5.Text == "Therminol VP1")
                {
                    SF_PHX.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox5.Text == "Syltherm_800")
                {
                    SF_PHX.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox5.Text == "Dowtherm_A")
                {
                    SF_PHX.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox5.Text == "Therminol_75")
                {
                    SF_PHX.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox5.Text == "Liquid Sodium")
                {
                    SF_PHX.comboBox1.Text = "Liquid Sodium";
                }

                SF_PHX.textBox41.Text = Convert.ToString(PHX_Q2);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(temp25);
                SF_PHX.textBox36.Text = Convert.ToString(pres25);
                SF_PHX.textBox35.Text = Convert.ToString(pres26);
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.PTC_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF");
                SF_PHX.button3_Click(this, e);
                SF_PHX.Show();
            }

            else if (comboBox4.Text == "Fresnel")
            {
                SF_PHX_LF = new Fresnel();

                if (comboBox5.Text == "Solar Salt")
                {
                    SF_PHX_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox5.Text == "Hitec XL")
                {
                    SF_PHX_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox5.Text == "Therminol VP1")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox5.Text == "Syltherm_800")
                {
                    SF_PHX_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox5.Text == "Dowtherm_A")
                {
                    SF_PHX_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox5.Text == "Therminol_75")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox5.Text == "Liquid Sodium")
                {
                    SF_PHX_LF.comboBox1.Text = "Liquid Sodium";
                }

                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX_Q2);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(temp25);
                SF_PHX_LF.textBox36.Text = Convert.ToString(pres25);
                SF_PHX_LF.textBox35.Text = Convert.ToString(pres26);
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.LF_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF");
                SF_PHX_LF.Load_ComboBox7();
                SF_PHX_LF.button3_Click(this, e);
                SF_PHX_LF.Show();
            }
        }

        //PreCooler Design
        private void button8_Click(object sender, EventArgs e)
        {
            Precooler_dialog = new PreeCooler();
            Precooler_dialog.textBox2.Text = Convert.ToString(PC_Q2);
            Precooler_dialog.textBox4.Text = Convert.ToString(massflow2 * (1 - recomp_frac_guess2));
            Precooler_dialog.textBox6.Text = Convert.ToString(temp29);
            Precooler_dialog.textBox12.Text = Convert.ToString(pres29);
            Precooler_dialog.textBox8.Text = Convert.ToString(pres21);
            Precooler_dialog.PreeCooler1(luis);
            Precooler_dialog.Calculate_Cooler();
            Precooler_dialog.Show();
        }

        //Cost Estimation
        private void button11_Click(object sender, EventArgs e)
        {
            Estimated_Cost_Dialog = new Estimated_Cost();
            Estimated_Cost_Dialog.textBox32.Text = Convert.ToString(PTC_Main_SF_Effective_Apperture_Area);
            Estimated_Cost_Dialog.textBox64.Text = Convert.ToString(w_dot_net2 / 1000);
            Estimated_Cost_Dialog.textBox65.Text = Convert.ToString(w_dot_net2 / 1000);
            Estimated_Cost_Dialog.textBox66.Text = Convert.ToString(w_dot_net2 / 1000);
            Estimated_Cost_Dialog.button1_Click(this, e);
            Estimated_Cost_Dialog.Show();
        }

        //Generator Energy Losses calculation
        private void button19_Click(object sender, EventArgs e)
        {
            Generator_dialog = new Generator();
            Generator_dialog.Generator_Shaft_Power = w_dot_net2;
            Generator_dialog.RC_Optimization_withoutReHeating(this,3);
            Generator_dialog.button2_Click(this, e);
            Generator_dialog.Show();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Net_Power_dialog = new Net_Power();
            Net_Power_dialog.RC_Optimization_withoutReHeating(this, 3);

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
         
            luis.calculate_turbomachine_outlet(luis, temp26, pres26, pres27, eta_t2, false, ref error_code_main_turbine, ref ental_in_main_turbine, ref entrop_in_main_turbine,
                                              ref dens_in_main_turbine, ref temp_out_main_turbine, ref ental_out_main_turbine, ref entrop_out_main_turbine, ref dens_out_main_turbine,
                                              ref specific_work_main_turbine);

            Net_Power_dialog.specific_work_main_turbine = specific_work_main_turbine;

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
            Net_Power_dialog.button2_Click(this, e);
            Net_Power_dialog.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Diagrams_dialog = new Diagrams();
            Diagrams_dialog.RC_Optimization_withoutReHeating(this, 3);
            Diagrams_dialog.button2_Click(this, e);
            Diagrams_dialog.Show();
        }

        //SIMPLE Brayton Options
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //SIMPLE Brayton Options
            if (checkBox2.Checked == true)
            {
                textBox15.Text = "True";
                numericUpDown1.Value = 0.0M;

                textBox61.Text = "True";
                textBox62.Text = "1.0";
            }

            //SIMPLE Brayton Options
            else if (checkBox2.Checked == false)
            {
                textBox15.Text = "False";
                numericUpDown1.Value = 0.1M;

                textBox61.Text = "False";
                textBox62.Text = "0.1";
            }
        }

        //Main SF
        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox5.Text == "Solar Salt")
            {
                textBox4.Text = "823.15";
            }

            else if (comboBox5.Text == "Hitec XL")
            {
                textBox4.Text = "793.15";
            }

            else if (comboBox5.Text == "Therminol VP1")
            {
                textBox4.Text = "663.15";
            }

            else if (comboBox5.Text == "Syltherm_800")
            {
                textBox4.Text = "663.15";
            }

            else if (comboBox5.Text == "Dowtherm_A")
            {
                textBox4.Text = "683.15";
            }

            else if (comboBox5.Text == "Therminol_75")
            {
                textBox4.Text = "648.15";
            }
        }

        //Dual-Loop SF1
        private void button17_Click(object sender, EventArgs e)
        {
            if (comboBox11.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();

                if (comboBox9.Text == "Solar Salt")
                {
                    SF_PHX.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox9.Text == "Hitec XL")
                {
                    SF_PHX.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox9.Text == "Therminol VP1")
                {
                    SF_PHX.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox9.Text == "Syltherm_800")
                {
                    SF_PHX.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox9.Text == "Dowtherm_A")
                {
                    SF_PHX.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox9.Text == "Therminol_75")
                {
                    SF_PHX.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox9.Text == "Liquid Sodium")
                {
                    SF_PHX.comboBox1.Text = "Liquid Sodium";
                }

                luis.working_fluid.FindStateWithTP(temp25, pres25); // properties at the inlet conditions
                PHX1_h_in = luis.working_fluid.Enthalpy;
                PHX1_temp_out = Convert.ToDouble(textBox6.Text);
                DP_PHX1 = Convert.ToDouble(textBox3.Text);
                PHX1_pres_out = pres25 - pres25 * DP_PHX1;
                luis.working_fluid.FindStateWithTP(PHX1_temp_out, PHX1_pres_out); // properties at the inlet conditions
                PHX1_h_out = luis.working_fluid.Enthalpy;
                PHX1_Q = massflow2 * (PHX1_h_out - PHX1_h_in);

                SF_PHX.textBox41.Text = Convert.ToString(PHX1_Q);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(temp25);
                SF_PHX.textBox36.Text = Convert.ToString(pres25);
                SF_PHX.textBox35.Text = Convert.ToString(PHX1_pres_out);
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.PTC_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF1_Dual_Loop");
                SF_PHX.button3_Click(this, e);
                SF_PHX.Show();
            }

            else if (comboBox11.Text == "Fresnel")
            {
                SF_PHX_LF = new Fresnel();

                if (comboBox9.Text == "Solar Salt")
                {
                    SF_PHX_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox9.Text == "Hitec XL")
                {
                    SF_PHX_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox9.Text == "Therminol VP1")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox9.Text == "Syltherm_800")
                {
                    SF_PHX_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox9.Text == "Dowtherm_A")
                {
                    SF_PHX_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox9.Text == "Therminol_75")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox9.Text == "Liquid Sodium")
                {
                    SF_PHX_LF.comboBox1.Text = "Liquid Sodium";
                }

                luis.working_fluid.FindStateWithTP(temp25, pres25); // properties at the inlet conditions
                PHX1_h_in = luis.working_fluid.Enthalpy;
                PHX1_temp_out = Convert.ToDouble(textBox6.Text);
                DP_PHX1 = Convert.ToDouble(textBox3.Text);
                PHX1_pres_out = pres25 - pres25 * DP_PHX1;
                luis.working_fluid.FindStateWithTP(PHX1_temp_out, PHX1_pres_out); // properties at the inlet conditions
                PHX1_h_out = luis.working_fluid.Enthalpy;
                PHX1_Q = massflow2 * (PHX1_h_out - PHX1_h_in);

                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX1_Q);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(temp25);
                SF_PHX_LF.textBox36.Text = Convert.ToString(pres25);
                SF_PHX_LF.textBox35.Text = Convert.ToString(PHX1_pres_out);
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.LF_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF1_Dual_Loop");
                SF_PHX_LF.Load_ComboBox7();
                SF_PHX_LF.button3_Click(this, e);
                SF_PHX_LF.Show();
            }
        }
            
        //Dual-Loop SF2
        private void button18_Click(object sender, EventArgs e)
        {
            if (comboBox10.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();

                if (comboBox8.Text == "Solar Salt")
                {
                    SF_PHX.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox8.Text == "Hitec XL")
                {
                    SF_PHX.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox8.Text == "Therminol VP1")
                {
                    SF_PHX.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox8.Text == "Syltherm_800")
                {
                    SF_PHX.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox8.Text == "Dowtherm_A")
                {
                    SF_PHX.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox8.Text == "Therminol_75")
                {
                    SF_PHX.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox8.Text == "Liquid Sodium")
                {
                    SF_PHX.comboBox1.Text = "Liquid Sodium";
                }

                PHX2_Q = PHX_Q2 - PHX1_Q;

                SF_PHX.textBox41.Text = Convert.ToString(PHX2_Q);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(PHX1_temp_out);
                SF_PHX.textBox36.Text = Convert.ToString(PHX1_pres_out);
                SF_PHX.textBox35.Text = Convert.ToString(pres26);
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.PTC_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF2_Dual_Loop");
                SF_PHX.button3_Click(this, e);
                SF_PHX.Show();
            }

            else if (comboBox10.Text == "Fresnel")
            {
                SF_PHX_LF = new Fresnel();

                if (comboBox8.Text == "Solar Salt")
                {
                    SF_PHX_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox8.Text == "Hitec XL")
                {
                    SF_PHX_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox8.Text == "Therminol VP1")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox8.Text == "Syltherm_800")
                {
                    SF_PHX_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox8.Text == "Dowtherm_A")
                {
                    SF_PHX_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox8.Text == "Therminol_75")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox8.Text == "Liquid Sodium")
                {
                    SF_PHX_LF.comboBox1.Text = "Liquid Sodium";
                }

                PHX2_Q = PHX_Q2 - PHX1_Q;

                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX2_Q);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(PHX1_temp_out);
                SF_PHX_LF.textBox36.Text = Convert.ToString(PHX1_pres_out);
                SF_PHX_LF.textBox35.Text = Convert.ToString(pres26);
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.LF_Solar_Field_ciclo_RC_Optimization_withoutReHeating(this, 3, "Main_SF2_Dual_Loop");
                SF_PHX_LF.Load_ComboBox7();
                SF_PHX_LF.button3_Click(this, e);
                SF_PHX_LF.Show();
            }
        }

        //Compile_Mixture
        private void button20_Click(object sender, EventArgs e)
        {
            

        }

        //Paper_Button
        private void button21_Click(object sender, EventArgs e)
        {
            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox13.Text + "=" + textBox31.Text + "," + this.comboBox14.Text + "=" + textBox36.Text, ReferenceState.DEF);
            this.textBox2.Text = (working_fluid.CriticalTemperature).ToString();
            this.textBox60.Text = Convert.ToString ((Convert.ToDouble(this.textBox7.Text) / working_fluid.CriticalPressure));
        }

        //Type Fluid Selected Change: Pure, PreDefinedMixture, NewMixture
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                comboBox14.Enabled = false;
                textBox31.Enabled = false;
                textBox36.Enabled = false;
            }

            else if (comboBox1.Text == "NewMixture")
            {
                comboBox14.Enabled = true;
                textBox31.Enabled = true;
                textBox36.Enabled = true;
                button24.Enabled = true;

                //numericUpDown1.Value = 0.35M;
                textBox15.Text = "True";
                textBox62.Text = "0.5";
                textBox61.Text = "True";
                textBox60.Text = "3.5";
                textBox59.Text = "True";
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

        //Generate_Paper Button
        private void button22_Click(object sender, EventArgs e)
        {
            int counter = 1;


            //Escritura de Archivo de Excel
            Excel.Application xlApp1;
            Excel.Workbook xlWorkBook1;
            Excel.Worksheet xlWorkSheet1;
            Excel.Worksheet xlWorkSheet2;
            object misValue1 = System.Reflection.Missing.Value;

            xlApp1 = new Excel.Application();
            xlApp1.DisplayAlerts = false;
            xlWorkBook1 = xlApp1.Workbooks.Add(misValue1);
            xlWorkSheet1 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(1);

            xlWorkSheet1.Cells[1, 1] = "Rec.Frac.";
            xlWorkSheet1.Cells[1, 2] = "Eff.(%)";
            xlWorkSheet1.Cells[1, 3] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[1, 4] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[1, 5] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[1, 6] = "HTR Pinch(ºC)";

            for (double j = 5000; j <= 15000; j = j + 5000)
            {
                textBox17.Text = Convert.ToString(j);

                for (double i = 0.1; i <= 0.40; i = i + 0.05)
                {
                    numericUpDown1.Value = Convert.ToDecimal(i);

                    //Call Subplex Button
                    button2_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    listBox1.Items.Add(numericUpDown1.Value.ToString());

                    listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    listBox3.Items.Add((LT_Effc * 100).ToString());
                    listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    listBox5.Items.Add((HT_Effc * 100).ToString());
                    listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = numericUpDown1.Value.ToString();
                    xlWorkSheet1.Cells[counter + 1, 2] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 3] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 4] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 5] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 6] = HTR_min_DT_paper.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }

            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_Mixtures_FINAL.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //Clear List Content
        private void button23_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
        }

        //Compile Mixture Button 
        private void button24_Click(object sender, EventArgs e)
        {

            File.Delete("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\RC_CO2_Mixture_Subplex.dll");

            StreamWriter fl6;
            fl6 = new StreamWriter("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90");
            fl6.WriteLine("module CO2_properties");
            fl6.WriteLine("");
            fl6.WriteLine("implicit double precision (a-h,o-z)");
            fl6.WriteLine("implicit integer (i-k,m,n)");
            fl6.WriteLine("");
            fl6.Close();

            StreamWriter fl1;
            fl1 = new StreamWriter("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\mixtureFluidsNumber.f90");
            fl1.WriteLine("integer,  parameter :: nc = " + Convert.ToString(2));
            fl1.Close();

            StreamWriter fl2;
            fl2 = new StreamWriter("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\first_fluid.f90");
            string fluid1 = comboBox13.Text + ".fld'";
            fl2.WriteLine("hf(1)='C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\fluids\\" + fluid1);
            fl2.Close();

            StreamWriter fl3;
            fl3 = new StreamWriter("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\second_fluid.f90");
            string fluid2 = comboBox14.Text + ".fld'";
            fl3.WriteLine("      hf(2)='C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\fluids\\" + fluid2);
            fl3.Close();

            StreamWriter fl4;
            fl4 = new StreamWriter("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\first_fluid_mole_fraction.f90");
            string fluid1MoleFraction = "      x(1)=" + textBox31.Text + "d0";
            fl4.WriteLine(fluid1MoleFraction);
            fl4.Close();

            StreamWriter fl5;
            fl5 = new StreamWriter("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\second_fluid_mole_fraction.f90");
            string fluid2MoleFraction = "      x(2)=" + textBox36.Text + "d0";
            fl5.WriteLine(fluid2MoleFraction);
            fl5.Close();

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\mixtureFluidsNumber.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_2.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\first_fluid.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\second_fluid.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_3.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\first_fluid_mole_fraction.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\second_fluid_mole_fraction.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_4.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            //CO2_RP_module
            StreamWriter fl7;
            fl7 = new StreamWriter("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\CO2_RP_module1.f90");
            fl7.WriteLine("");
            fl7.Close();

            using (Stream input = File.OpenRead("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90"))
            using (Stream output = new FileStream("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\CO2_RP_module1.f90", FileMode.Append,
                                                  FileAccess.Write, FileShare.None))
            {
                input.CopyTo(output); // Using .NET 4
            }

            File.Delete("C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\file_1.f90");

            Process p = new System.Diagnostics.Process();
            //p.StartInfo.Arguments = "argument1 argument2 argument3";
            p.StartInfo.FileName = "C:\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\sCO2 Heat Balances C#\\RefPropWindowsForms\\RefPropWindowsForms\\bin\\Debug\\CompileCO2_optimal_REFPROP_library.bat";
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

        }

        //Run Phyton File
        private void button20_Click_1(object sender, EventArgs e)
        {
            // full path of python interpreter  
            string python = @"C:\Python34\python.exe";

            // python app to call  
            string myPythonApp = "sum.py";

            // dummy parameters to send Python script  
            int x = 2;
            int y = 5;

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            //myProcessStartInfo.CreateNoWindow = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            myProcessStartInfo.Arguments = myPythonApp;

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;

            // start process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            // wait exit signal from the app we called 
            myProcess.WaitForExit();

            // close the process 
            myProcess.Close();

            MessageBox.Show(myString);
        }
    }
}

