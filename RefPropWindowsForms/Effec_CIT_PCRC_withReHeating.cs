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

using System.Windows.Forms.DataVisualization.Charting;

using sc.net;

namespace RefPropWindowsForms
{
    public partial class Effec_CIT_PCRC_withReHeating : Form
    {
        public core luis = new core();

        public PTC_SF_PHX_Data_Input Main_PTC_SF_PHX_Data_Input_Dialogue;
        public PTC_SF_PHX_Data_Input ReHeating_PTC_SF_PHX_Data_Input_Dialogue;

        public LF_SF_PHX_Data_Input Main_LF_SF_PHX_Data_Input_Dialogue;
        public LF_SF_PHX_Data_Input ReHeating_LF_SF_PHX_Data_Input_Dialogue;

        //PTC Data Input Dialogue
        public Double m_dot_h_PHX = 1200;
        public long N_sub_hxrs_PHX = 15;
        public Double P_h_in_PHX = 0;
        public Double P_h_out_PHX = 0;
        public Double Main_SF_Cp_HTF = 0;
        public Double AT_Main_SF = 10;

        public Double m_dot_h_RHX = 1200;
        public long N_sub_hxrs_RHX = 15;
        public Double P_h_in_RHX = 0;
        public Double P_h_out_RHX = 0;
        public Double ReHeating_SF_Cp_HTF = 0;
        public Double AT_ReHeating_SF = 10;

        //PTC_SF Variables
        public Double Main_SF_zone = -8;
        public Double Main_SF_Lon = -116.8;
        public Double Main_SF_Lat = 34.86;
        public Double Main_SF_DNI = 986;
        public Double Main_SF_DAY = 172;
        public Double Main_SF_HOUR = 11.5;

        public String Main_SF_HTF;
        public Double Main_SF_NominalOpticalEfficiency = 0.75;
        public Double Main_SF_CleanlinessFactor = 0.96;
        public Double Main_SF_EndLossFactor = 0.999;
        public Double Main_SF_CollectorApertureWidth = 5.77;
        public Double Main_SF_SolarFieldThermalEnergy = 0;
        public Double Main_SF_NumberRows = 100;
        public Double Main_SF_InletTemperature = 0;
        public Double Main_SF_OutputTemperature = 823.2;
        public Double Main_SF_CoefficientA1 = 0.141;
        public Double Main_SF_CoefficientA2 = 6.48e-9;
        public Double Main_SF_NumberOfSegments = 10;
        public Double Main_SF_Desired_Mass_Flux = 3000;
        public Double Main_SF_Focal_length = 1.71;
        public Double Main_SF_Exterior_Diameter = 70;
        public Double Main_SF_Receiver_Thickness = 4.191;
        public Double Main_SF_Diameter_Interior;
        public Double Main_SF_m_dot_h = 600;
        public Double Main_SF_Rugosidad = 0.0457;
        public Double Main_SF_anginc = 0;
        public Double Main_SF_anginc_long = 0;
        public Double Main_SF_anginc_trans = 0;
        public Double Main_SF_azimuth = 0;
        public Double Main_SF_angzenit = 0;
        public Double Main_SF_alt_solare = 0;
        public Double Main_SF_IAMLongitudinal = 0;
        public Double Main_SF_IAMTransversal = 0;
        public Double Main_SF_IAMOverall = 0;
        public Double Main_SF_ReflectorApertureArea = 0;
        public Double Main_SF_Total_Pressure_Drop = 0;
        public String Main_SF_IAM_Table_Name = "Thermoflow 25, Novatec - Superheater (Fresnel)";

        public Double ReHeating_SF_zone = -8;
        public Double ReHeating_SF_Lon = -116.8;
        public Double ReHeating_SF_Lat = 34.86;
        public Double ReHeating_SF_DNI = 986;
        public Double ReHeating_SF_DAY = 172;
        public Double ReHeating_SF_HOUR = 11.5;

        public String ReHeating_SF_HTF;
        public Double ReHeating_SF_NominalOpticalEfficiency = 0.75;
        public Double ReHeating_SF_CleanlinessFactor = 0.96;
        public Double ReHeating_SF_EndLossFactor = 0;
        public Double ReHeating_SF_CollectorApertureWidth = 5.77;
        public Double ReHeating_SF_SolarFieldThermalEnergy = 0;
        public Double ReHeating_SF_NumberRows = 100;
        public Double ReHeating_SF_InletTemperature = 0;
        public Double ReHeating_SF_OutputTemperature = 823.2;
        public Double ReHeating_SF_CoefficientA1 = 0.141;
        public Double ReHeating_SF_CoefficientA2 = 6.48e-9;
        public Double ReHeating_SF_NumberOfSegments = 10;
        public Double ReHeating_SF_Desired_Mass_Flux = 3000;
        public Double ReHeating_SF_Focal_length = 1.71;
        public Double ReHeating_SF_Exterior_Diameter = 70;
        public Double ReHeating_SF_Receiver_Thickness = 4.191;
        public Double ReHeating_SF_Diameter_Interior;
        public Double ReHeating_SF_m_dot_h = 600;
        public Double ReHeating_SF_Rugosidad = 0.0457;
        public Double ReHeating_SF_anginc = 0;
        public Double ReHeating_SF_anginc_long = 0;
        public Double ReHeating_SF_anginc_trans = 0;
        public Double ReHeating_SF_azimuth = 0;
        public Double ReHeating_SF_angzenit = 0;
        public Double ReHeating_SF_alt_solare = 0;
        public Double ReHeating_SF_IAMLongitudinal = 0;
        public Double ReHeating_SF_IAMTransversal = 0;
        public Double ReHeating_SF_IAMOverall = 0;
        public Double ReHeating_SF_ReflectorApertureArea = 0;
        public Double ReHeating_SF_Total_Pressure_Drop = 0;
        public String ReHeating_SF_IAM_Table_Name = "Thermoflow 25, Novatec - Superheater (Fresnel)";

        public Equipments_Unitary_Cost Equipments_Unitary_Cost_Dialoge;

        public HeatExchangerUA LT_Recuperator = new HeatExchangerUA();
        public HeatExchangerUA HT_Recuperator = new HeatExchangerUA();

        public Radial_Turbine Main_Turbine = new Radial_Turbine();

        //First calculate the Main Compressor Rotational speed and after send that value to the Turbines
        public Double N_design_Main_Compressor;

        public snl_compressor_tsr Main_Compressor = new snl_compressor_tsr();
        public snl_compressor_tsr ReCompressor = new snl_compressor_tsr();

        //Input Data:
        public RefrigerantCategory category;
        public ReferenceState referencestate;

        //Thermal Efficiency
        public Double eta_optimum;

        //Graph variables
        public Double CIT_min, CIT_max, CIT_increment;
        public Double UA_Total_min, UA_Total_max, UA_Total_increment;

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
        public Double w_mc, w_rc, w_t, C_dot_min, Q_dot_max;
        public Double T9_lower_bound, T9_upper_bound, T8_lower_bound, T8_upper_bound, last_LT_residual, last_T9_guess;
        public Double last_HT_residual, last_T8_guess, secant_guess;
        public Double m_dot_t, m_dot_mc, m_dot_rc, eta_mc_isen, eta_rc_isen, eta_t_isen;
        public Double min_DT_LT, min_DT_HT, UA_LT_calc, UA_HT_calc, Q_dot_LT, Q_dot_HT, UA_HT_residual, UA_LT_residual;
        public Double[] temp = new Double[12];
        public Double[] pres = new Double[12];
        public Double[] enth = new Double[12];
        public Double[] entr = new Double[12];
        public Double[] dens = new Double[12];

        public Double wmm;

        public DataPoint eta = new DataPoint();
        public List<Double> eta_list = new List<Double>();

        //For LT Fraction
        public DataPoint LT_frac_point = new DataPoint();
        public List<Double> LT_frac_list = new List<Double>();

        //For LT Conductance_UA
        public DataPoint LT_UA_point = new DataPoint();
        public List<Double> LT_UA_list = new List<Double>();

        //For HT Conductance_UA
        public DataPoint HT_UA_point = new DataPoint();
        public List<Double> HT_UA_list = new List<Double>();

        //For PR_Pressure_Ratio
        public DataPoint PR_frac_point = new DataPoint();
        public List<Double> PR_frac_list = new List<Double>();

        //For CIP (Compressor Inlet Pressure)
        public DataPoint CIP_point = new DataPoint();
        public List<Double> CIP_list = new List<Double>();

        public DataPoint LT_HX_Eff = new DataPoint();
        public List<Double> LT_HX_Eff_list = new List<Double>();

        public DataPoint LT_HX_min_DT = new DataPoint();
        public List<Double> LT_HX_min_DT_list = new List<Double>();

        public DataPoint HT_HX_Eff = new DataPoint();
        public List<Double> HT_HX_Eff_list = new List<Double>();

        public DataPoint HT_HX_min_DT = new DataPoint();
        public List<Double> HT_HX_min_DT_list = new List<Double>();

        public DataPoint Main_SF_Effective_Aperture_Area = new DataPoint();
        public List<Double> Main_SF_Effective_Aperture_Area_list = new List<Double>();

        public DataPoint Main_SF_Pressure_Drop = new DataPoint();
        public List<Double> Main_SF_Pressure_Drop_list = new List<Double>();

        public DataPoint ReHeating_SF_Effective_Aperture_Area = new DataPoint();
        public List<Double> ReHeating_SF_Effective_Aperture_Area_list = new List<Double>();

        public DataPoint ReHeating_SF_Pressure_Drop = new DataPoint();
        public List<Double> ReHeating_SF_Pressure_Drop_list = new List<Double>();

        public DataPoint PHX_Eff = new DataPoint();
        public List<Double> PHX_Eff_list = new List<Double>();

        public DataPoint PHX_UA = new DataPoint();
        public List<Double> PHX_UA_list = new List<Double>();

        public DataPoint PHX_Q = new DataPoint();
        public List<Double> PHX_Q_list = new List<Double>();

        public DataPoint RHX_Eff = new DataPoint();
        public List<Double> RHX_Eff_list = new List<Double>();

        public DataPoint RHX_UA = new DataPoint();
        public List<Double> RHX_UA_list = new List<Double>();

        public DataPoint RHX_Q = new DataPoint();
        public List<Double> RHX_Q_list = new List<Double>();

        public DataPoint PC1_Eff = new DataPoint();
        public List<Double> PC1_Eff_list = new List<Double>();

        public DataPoint PC1_UA = new DataPoint();
        public List<Double> PC1_UA_list = new List<Double>();

        public DataPoint PC1_Q = new DataPoint();
        public List<Double> PC1_Q_list = new List<Double>();

        public DataPoint PC2_Eff = new DataPoint();
        public List<Double> PC2_Eff_list = new List<Double>();

        public DataPoint PC2_UA = new DataPoint();
        public List<Double> PC2_UA_list = new List<Double>();

        public DataPoint PC2_Q = new DataPoint();
        public List<Double> PC2_Q_list = new List<Double>();

        const string refpropDLL_path1 = "PCRC_CO2_Optimal_PCRC_with_RH_Subplex.dll";
        [DllImport(refpropDLL_path1, EntryPoint = "carbondioxidesubplex_", SetLastError = true)]
        public static extern void carbondioxidesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_rhx_in_guess1, ref Boolean fixed_P_rhx_in1,
                                                 ref Double t_rht_in1, ref Double p_pc_in1, ref Boolean fixed_pc_in1, ref Double t_pc_in1, ref Double ua_rec_total1, ref Double eta_mc1, ref Double eta_rc1,
                                                 ref Double eta_pc1, ref Double eta_t1, ref Double eta_trh1,
                                                 ref Int64 n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1, ref Boolean fixed_p_mc_out1,
                                                 ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1,
                                                 ref Double lt_frac_guess1, ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1, ref Double dp_pc2, ref Double dp_phx1,
                                                 ref Double dp_phx2, ref Double dp_rhx1, ref Double dp_rhx2, ref Double dp_cooler1, ref Double dp_cooler2, ref Double temp1,
                                                 ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7,
                                                 ref Double temp8, ref Double temp9, ref Double temp10, ref Double temp11, ref Double temp12, ref Double temp13, ref Double temp14,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6, ref Double pres7, ref Double pres8,
                                                 ref Double pres9, ref Double pres10, ref Double pres11, ref Double pres12, ref Double pres13, ref Double pres14, ref Double m_dot_turbine1,
                                                 ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin,
                                                 ref Double LT_Pcout, ref Double LT_Phout, ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin,
                                                 ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double PHX, ref Double RHX, ref Double PC, ref Double COOLER);

        const string refpropDLL_path2 = "PCRC_Ethane_Optimal_PCRC_with_RH.dll";
        [DllImport(refpropDLL_path2, EntryPoint = "ethanesubplex_", SetLastError = true)]
        public static extern void ethanesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_rhx_in_guess1, ref Boolean fixed_P_rhx_in1,
                                                 ref Double t_rht_in1, ref Double p_pc_in1, ref Boolean fixed_pc_in1, ref Double t_pc_in1, ref Double ua_rec_total1, ref Double eta_mc1, ref Double eta_rc1,
                                                 ref Double eta_pc1, ref Double eta_t1, ref Double eta_trh1,
                                                 ref Int64 n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1, ref Boolean fixed_p_mc_out1,
                                                 ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1,
                                                 ref Double lt_frac_guess1, ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1, ref Double dp_pc2, ref Double dp_phx1,
                                                 ref Double dp_phx2, ref Double dp_rhx1, ref Double dp_rhx2, ref Double dp_cooler1, ref Double dp_cooler2, ref Double temp1,
                                                 ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7,
                                                 ref Double temp8, ref Double temp9, ref Double temp10, ref Double temp11, ref Double temp12, ref Double temp13, ref Double temp14,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6, ref Double pres7, ref Double pres8,
                                                 ref Double pres9, ref Double pres10, ref Double pres11, ref Double pres12, ref Double pres13, ref Double pres14, ref Double m_dot_turbine1,
                                                 ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin,
                                                 ref Double LT_Pcout, ref Double LT_Phout, ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin,
                                                 ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double PHX, ref Double RHX, ref Double PC, ref Double COOLER);

        const string refpropDLL_path3 = "PCRC_Methane_Optimal_PCRC_with_RH.dll";
        [DllImport(refpropDLL_path3, EntryPoint = "methanesubplex_", SetLastError = true)]
        public static extern void methanesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_rhx_in_guess1, ref Boolean fixed_P_rhx_in1,
                                                 ref Double t_rht_in1, ref Double p_pc_in1, ref Boolean fixed_pc_in1, ref Double t_pc_in1, ref Double ua_rec_total1, ref Double eta_mc1, ref Double eta_rc1,
                                                 ref Double eta_pc1, ref Double eta_t1, ref Double eta_trh1,
                                                 ref Int64 n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1, ref Boolean fixed_p_mc_out1,
                                                 ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1,
                                                 ref Double lt_frac_guess1, ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1, ref Double dp_pc2, ref Double dp_phx1,
                                                 ref Double dp_phx2, ref Double dp_rhx1, ref Double dp_rhx2, ref Double dp_cooler1, ref Double dp_cooler2, ref Double temp1,
                                                 ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7,
                                                 ref Double temp8, ref Double temp9, ref Double temp10, ref Double temp11, ref Double temp12, ref Double temp13, ref Double temp14,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6, ref Double pres7, ref Double pres8,
                                                 ref Double pres9, ref Double pres10, ref Double pres11, ref Double pres12, ref Double pres13, ref Double pres14, ref Double m_dot_turbine1,
                                                 ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin,
                                                 ref Double LT_Pcout, ref Double LT_Phout, ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin,
                                                 ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double PHX, ref Double RHX, ref Double PC, ref Double COOLER);

        const string refpropDLL_path4 = "PCRC_Nitrogen_Optimal_PCRC_with_RH.dll";
        [DllImport(refpropDLL_path4, EntryPoint = "nitrogensubplex_", SetLastError = true)]
        public static extern void nitrogensubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_rhx_in_guess1, ref Boolean fixed_P_rhx_in1,
                                                 ref Double t_rht_in1, ref Double p_pc_in1, ref Boolean fixed_pc_in1, ref Double t_pc_in1, ref Double ua_rec_total1, ref Double eta_mc1, ref Double eta_rc1,
                                                 ref Double eta_pc1, ref Double eta_t1, ref Double eta_trh1,
                                                 ref Int64 n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1, ref Boolean fixed_p_mc_out1,
                                                 ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1,
                                                 ref Double lt_frac_guess1, ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1, ref Double dp_pc2, ref Double dp_phx1,
                                                 ref Double dp_phx2, ref Double dp_rhx1, ref Double dp_rhx2, ref Double dp_cooler1, ref Double dp_cooler2, ref Double temp1,
                                                 ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7,
                                                 ref Double temp8, ref Double temp9, ref Double temp10, ref Double temp11, ref Double temp12, ref Double temp13, ref Double temp14,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6, ref Double pres7, ref Double pres8,
                                                 ref Double pres9, ref Double pres10, ref Double pres11, ref Double pres12, ref Double pres13, ref Double pres14, ref Double m_dot_turbine1,
                                                 ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin,
                                                 ref Double LT_Pcout, ref Double LT_Phout, ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin,
                                                 ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double PHX, ref Double RHX, ref Double PC, ref Double COOLER);

        const string refpropDLL_path5 = "PCRC_Xenon_Optimal_PCRC_with_RH.dll";
        [DllImport(refpropDLL_path5, EntryPoint = "xenonsubplex_", SetLastError = true)]
        public static extern void xenonsubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_rhx_in_guess1, ref Boolean fixed_P_rhx_in1,
                                                 ref Double t_rht_in1, ref Double p_pc_in1, ref Boolean fixed_pc_in1, ref Double t_pc_in1, ref Double ua_rec_total1, ref Double eta_mc1, ref Double eta_rc1,
                                                 ref Double eta_pc1, ref Double eta_t1, ref Double eta_trh1,
                                                 ref Int64 n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1, ref Boolean fixed_p_mc_out1,
                                                 ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1,
                                                 ref Double lt_frac_guess1, ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1, ref Double dp_pc2, ref Double dp_phx1,
                                                 ref Double dp_phx2, ref Double dp_rhx1, ref Double dp_rhx2, ref Double dp_cooler1, ref Double dp_cooler2, ref Double temp1,
                                                 ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7,
                                                 ref Double temp8, ref Double temp9, ref Double temp10, ref Double temp11, ref Double temp12, ref Double temp13, ref Double temp14,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6, ref Double pres7, ref Double pres8,
                                                 ref Double pres9, ref Double pres10, ref Double pres11, ref Double pres12, ref Double pres13, ref Double pres14, ref Double m_dot_turbine1,
                                                 ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin,
                                                 ref Double LT_Pcout, ref Double LT_Phout, ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin,
                                                 ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double PHX, ref Double RHX, ref Double PC, ref Double COOLER);

        const string refpropDLL_path6 = "PCRC_SF6_Optimal_PCRC_with_RH.dll";
        [DllImport(refpropDLL_path6, EntryPoint = "sulfurhexafluoridesubplex_", SetLastError = true)]
        public static extern void sulfurhexafluoridesubplex_(ref Double w_dot_net1, ref Double t_mc_in1, ref Double t_t_in1, ref Double p_rhx_in_guess1, ref Boolean fixed_P_rhx_in1,
                                                 ref Double t_rht_in1, ref Double p_pc_in1, ref Boolean fixed_pc_in1, ref Double t_pc_in1, ref Double ua_rec_total1, ref Double eta_mc1, ref Double eta_rc1,
                                                 ref Double eta_pc1, ref Double eta_t1, ref Double eta_trh1,
                                                 ref Int64 n_sub_hxrs1, ref Double p_high_limit1, ref Double p_mc_out_guess1, ref Boolean fixed_p_mc_out1,
                                                 ref Double pr_mc_guess1, ref Boolean fixed_pr_mc1, ref Double recomp_frac_guess1, ref Boolean fixed_recomp_frac1,
                                                 ref Double lt_frac_guess1, ref Boolean fixed_lt_frac1, ref Double tol1, ref Double opt_tol1, ref Double eta_thermal1,
                                                 ref Double dp_lt1, ref Double dp_lt2, ref Double dp_ht1, ref Double dp_ht2, ref Double dp_pc1, ref Double dp_pc2, ref Double dp_phx1,
                                                 ref Double dp_phx2, ref Double dp_rhx1, ref Double dp_rhx2, ref Double dp_cooler1, ref Double dp_cooler2, ref Double temp1,
                                                 ref Double temp2, ref Double temp3, ref Double temp4, ref Double temp5, ref Double temp6, ref Double temp7,
                                                 ref Double temp8, ref Double temp9, ref Double temp10, ref Double temp11, ref Double temp12, ref Double temp13, ref Double temp14,
                                                 ref Double pres1, ref Double pres2, ref Double pres3, ref Double pres4, ref Double pres5, ref Double pres6, ref Double pres7, ref Double pres8,
                                                 ref Double pres9, ref Double pres10, ref Double pres11, ref Double pres12, ref Double pres13, ref Double pres14, ref Double m_dot_turbine1,
                                                 ref Double LT_mdoth, ref Double LT_mdotc, ref Double LT_Tcin, ref Double LT_Thin, ref Double LT_Pcin, ref Double LT_Phin,
                                                 ref Double LT_Pcout, ref Double LT_Phout, ref Double LT_Q, ref Double HT_mdoth, ref Double HT_mdotc, ref Double HT_Tcin, ref Double HT_Thin,
                                                 ref Double HT_Pcin, ref Double HT_Phin, ref Double HT_Pcout, ref Double HT_Phout, ref Double HT_Q, ref Double LT_UA, ref Double HT_UA, ref Double LT_Effc, ref Double HT_Effc, ref Double PHX, ref Double RHX, ref Double PC, ref Double COOLER);

        public Effec_CIT_PCRC_withReHeating()
        {
            InitializeComponent();
        }

        public Double specific_work_main_turbine = 0;
        public Double specific_work_reheating_turbine = 0;
        public Double specific_work_compressor1 = 0;
        public Double specific_work_compressor2 = 0;
        public Double specific_work_compressor3 = 0;
        public Double Miscellanous_Auxiliaries = 0;
        public Double Total_Auxiliaries = 0;

        public Double w_dot_net2;
        public Double t_mc_in2;
        public Double t_t_in2;
        public Double p_rhx_in2;
        public Double t_rht_in2;
        public Double ua_lt2, ua_ht2;
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_pc2;
        public Double eta_t2;
        public Double eta_trh2;
        public Int64 n_sub_hxrs2;
        public Double p_mc_in2;
        public Double p_mc_out2;
        public Double p_pc_in2;
        public Double t_pc_in2;
        public Double p_pc_out2;
        public Double recomp_frac2;
        public Double tol2, opt_tol2;
        public Double eta_thermal2;

        public Double dp2_lt1, dp2_lt2;
        public Double dp2_ht1, dp2_ht2;
        public Double dp2_pc1, dp2_pc2;
        public Double dp2_phx1, dp2_phx2;
        public Double dp2_rhx1, dp2_rhx2;
        public Double dp2_cooler1, dp2_cooler2;

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

        public Double p_rhx_in_guess2, ua_rec_total2, pr_mc_guess2, recomp_frac_guess2, lt_frac_guess2;
        public Double p_high_limit2, p_mc_out_guess2;
        public Boolean fixed_P_rhx_in2, fixed_p_mc_out2, fixed_pr_mc2, fixed_recomp_frac2, fixed_lt_frac2;
        public Boolean fixed_pc_in2;

        public Double PHX1, RHX1, PC1, COOLER1;

        //SUBPLEX Optimization
        private void button1_Click(object sender, EventArgs e)
        {
            Calculate_Design_Point();
        }

        public void Calculate_Design_Point()
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

            //Store Input Data from Graphical User Interface GUI into variables
            w_dot_net2 = Convert.ToDouble(textBox1.Text);
            t_mc_in2 = Convert.ToDouble(textBox2.Text);
            t_t_in2 = Convert.ToDouble(textBox4.Text);
            p_rhx_in_guess2 = Convert.ToDouble(textBox7.Text);
            fixed_P_rhx_in2 = Convert.ToBoolean(textBox3.Text);
            t_rht_in2 = Convert.ToDouble(textBox6.Text);
            p_pc_in2 = Convert.ToDouble(textBox15.Text);
            fixed_pc_in2 = Convert.ToBoolean(textBox35.Text);
            t_pc_in2 = Convert.ToDouble(textBox8.Text);
            ua_rec_total2 = Convert.ToDouble(textBox16.Text);
            p_high_limit2 = Convert.ToDouble(textBox17.Text);

            dp2_lt1 = Convert.ToDouble(textBox5.Text);
            dp2_ht1 = Convert.ToDouble(textBox12.Text);
            dp2_pc2 = Convert.ToDouble(textBox11.Text);
            dp2_phx1 = Convert.ToDouble(textBox10.Text);
            dp2_rhx1 = Convert.ToDouble(textBox9.Text);
            dp2_lt2 = Convert.ToDouble(textBox26.Text);
            dp2_ht2 = Convert.ToDouble(textBox25.Text);
            dp2_cooler2 = Convert.ToDouble(textBox27.Text);

            recomp_frac_guess2 = Convert.ToDouble(textBox32.Text);
            fixed_recomp_frac2 = Convert.ToBoolean(textBox31.Text);

            lt_frac_guess2 = Convert.ToDouble(textBox34.Text);
            fixed_lt_frac2 = Convert.ToBoolean(textBox33.Text);

            p_mc_out_guess2 = Convert.ToDouble(textBox23.Text);
            fixed_p_mc_out2 = Convert.ToBoolean(textBox22.Text);

            pr_mc_guess2 = Convert.ToDouble(textBox30.Text);

            eta_mc2 = Convert.ToDouble(textBox14.Text);
            eta_rc2 = Convert.ToDouble(textBox13.Text);
            eta_pc2 = Convert.ToDouble(textBox24.Text);
            eta_t2 = Convert.ToDouble(textBox19.Text);
            eta_trh2 = Convert.ToDouble(textBox18.Text);
            n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            tol2 = Convert.ToDouble(textBox21.Text);
            opt_tol2 = Convert.ToDouble(textBox29.Text);

            if (comboBox2.Text == "CO2")
            {
                carbondioxidesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2, ref p_rhx_in_guess2, ref fixed_P_rhx_in2,
                               ref t_rht_in2, ref p_pc_in2, ref fixed_pc_in2, ref t_pc_in2, ref ua_rec_total2, ref eta_mc2, ref eta_rc2,
                               ref eta_pc2, ref eta_t2, ref eta_trh2, ref n_sub_hxrs2, ref p_high_limit2, ref p_mc_out_guess2, ref fixed_p_mc_out2,
                               ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2, ref fixed_recomp_frac2,
                               ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                               ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1,
                               ref dp2_phx2, ref dp2_rhx1, ref dp2_rhx2, ref dp2_cooler1, ref dp2_cooler2, ref temp21,
                               ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                               ref temp28, ref temp29, ref temp210, ref temp211, ref temp212, ref temp213, ref temp214,
                               ref pres21, ref pres22, ref pres23, ref pres24, ref pres25, ref pres26, ref pres27, ref pres28,
                               ref pres29, ref pres210, ref pres211, ref pres212, ref pres213, ref pres214, ref massflow2,
                               ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin,
                               ref LT_Pcout, ref LT_Phout, ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin,
                               ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout, ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref PHX1, ref RHX1, ref PC1, ref COOLER1);

                textBox53.Text = Convert.ToString(temp21);
                textBox52.Text = Convert.ToString(temp22);
                textBox50.Text = Convert.ToString(temp23);
                textBox49.Text = Convert.ToString(temp24);
                textBox48.Text = Convert.ToString(temp25);
                textBox47.Text = Convert.ToString(temp26);
                textBox46.Text = Convert.ToString(temp27);
                textBox45.Text = Convert.ToString(temp28);
                textBox44.Text = Convert.ToString(temp29);
                textBox43.Text = Convert.ToString(temp210);
                textBox57.Text = Convert.ToString(temp211);
                textBox42.Text = Convert.ToString(temp212);
                textBox41.Text = Convert.ToString(temp213);
                textBox40.Text = Convert.ToString(temp214);

                textBox73.Text = Convert.ToString(pres21);
                textBox72.Text = Convert.ToString(pres22);
                textBox71.Text = Convert.ToString(pres23);
                textBox70.Text = Convert.ToString(pres24);
                textBox69.Text = Convert.ToString(pres25);
                textBox68.Text = Convert.ToString(pres26);
                textBox63.Text = Convert.ToString(pres27);
                textBox62.Text = Convert.ToString(pres28);
                textBox61.Text = Convert.ToString(pres29);
                textBox60.Text = Convert.ToString(pres210);
                textBox59.Text = Convert.ToString(pres211);
                textBox58.Text = Convert.ToString(pres212);
                textBox67.Text = Convert.ToString(pres213);
                textBox66.Text = Convert.ToString(pres214);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state, point13_state, point14_state;

                textBox76.Text = Convert.ToString(w_dot_net2);
                textBox76.BackColor = Color.Yellow;

                textBox75.Text = Convert.ToString(eta_thermal2);
                textBox75.BackColor = Color.Yellow;

                textBox74.Text = Convert.ToString(massflow2);
                textBox74.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_rhx_in_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox32.Text = Convert.ToString(recomp_frac_guess2);
                textBox32.BackColor = Color.Yellow;

                textBox15.Text = Convert.ToString(p_pc_in2);
                textBox15.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(p_mc_out_guess2);
                textBox30.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(pres214 / pres213);
                textBox30.BackColor = Color.Yellow;

                textBox34.Text = Convert.ToString(lt_frac_guess2);
                textBox34.BackColor = Color.Yellow;

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
            }

        else if (comboBox2.Text == "ETHANE")
        {
                ethanesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2, ref p_rhx_in_guess2, ref fixed_P_rhx_in2,
                               ref t_rht_in2, ref p_pc_in2, ref fixed_pc_in2, ref t_pc_in2, ref ua_rec_total2, ref eta_mc2, ref eta_rc2,
                               ref eta_pc2, ref eta_t2, ref eta_trh2, ref n_sub_hxrs2, ref p_high_limit2, ref p_mc_out_guess2, ref fixed_p_mc_out2,
                               ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2, ref fixed_recomp_frac2,
                               ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                               ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1,
                               ref dp2_phx2, ref dp2_rhx1, ref dp2_rhx2, ref dp2_cooler1, ref dp2_cooler2, ref temp21,
                               ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                               ref temp28, ref temp29, ref temp210, ref temp211, ref temp212, ref temp213, ref temp214,
                               ref pres21, ref pres22, ref pres23, ref pres24, ref pres25, ref pres26, ref pres27, ref pres28,
                               ref pres29, ref pres210, ref pres211, ref pres212, ref pres213, ref pres214, ref massflow2,
                               ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin,
                               ref LT_Pcout, ref LT_Phout, ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin,
                               ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout, ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref PHX1, ref RHX1, ref PC1, ref COOLER1);

                textBox53.Text = Convert.ToString(temp21);
                textBox52.Text = Convert.ToString(temp22);
                textBox50.Text = Convert.ToString(temp23);
                textBox49.Text = Convert.ToString(temp24);
                textBox48.Text = Convert.ToString(temp25);
                textBox47.Text = Convert.ToString(temp26);
                textBox46.Text = Convert.ToString(temp27);
                textBox45.Text = Convert.ToString(temp28);
                textBox44.Text = Convert.ToString(temp29);
                textBox43.Text = Convert.ToString(temp210);
                textBox57.Text = Convert.ToString(temp211);
                textBox42.Text = Convert.ToString(temp212);
                textBox41.Text = Convert.ToString(temp213);
                textBox40.Text = Convert.ToString(temp214);

                textBox73.Text = Convert.ToString(pres21);
                textBox72.Text = Convert.ToString(pres22);
                textBox71.Text = Convert.ToString(pres23);
                textBox70.Text = Convert.ToString(pres24);
                textBox69.Text = Convert.ToString(pres25);
                textBox68.Text = Convert.ToString(pres26);
                textBox63.Text = Convert.ToString(pres27);
                textBox62.Text = Convert.ToString(pres28);
                textBox61.Text = Convert.ToString(pres29);
                textBox60.Text = Convert.ToString(pres210);
                textBox59.Text = Convert.ToString(pres211);
                textBox58.Text = Convert.ToString(pres212);
                textBox67.Text = Convert.ToString(pres213);
                textBox66.Text = Convert.ToString(pres214);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state, point13_state, point14_state;

                textBox76.Text = Convert.ToString(w_dot_net2);
                textBox76.BackColor = Color.Yellow;

                textBox75.Text = Convert.ToString(eta_thermal2);
                textBox75.BackColor = Color.Yellow;

                textBox74.Text = Convert.ToString(massflow2);
                textBox74.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_rhx_in_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox32.Text = Convert.ToString(recomp_frac_guess2);
                textBox32.BackColor = Color.Yellow;

                textBox15.Text = Convert.ToString(p_pc_in2);
                textBox15.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(p_mc_out_guess2);
                textBox30.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(pres214 / pres213);
                textBox30.BackColor = Color.Yellow;

                textBox34.Text = Convert.ToString(lt_frac_guess2);
                textBox34.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "METHANE")
            {
                methanesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2, ref p_rhx_in_guess2, ref fixed_P_rhx_in2,
                               ref t_rht_in2, ref p_pc_in2, ref fixed_pc_in2, ref t_pc_in2, ref ua_rec_total2, ref eta_mc2, ref eta_rc2,
                               ref eta_pc2, ref eta_t2, ref eta_trh2, ref n_sub_hxrs2, ref p_high_limit2, ref p_mc_out_guess2, ref fixed_p_mc_out2,
                               ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2, ref fixed_recomp_frac2,
                               ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                               ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1,
                               ref dp2_phx2, ref dp2_rhx1, ref dp2_rhx2, ref dp2_cooler1, ref dp2_cooler2, ref temp21,
                               ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                               ref temp28, ref temp29, ref temp210, ref temp211, ref temp212, ref temp213, ref temp214,
                               ref pres21, ref pres22, ref pres23, ref pres24, ref pres25, ref pres26, ref pres27, ref pres28,
                               ref pres29, ref pres210, ref pres211, ref pres212, ref pres213, ref pres214, ref massflow2,
                               ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin,
                               ref LT_Pcout, ref LT_Phout, ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin,
                               ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout, ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref PHX1, ref RHX1, ref PC1, ref COOLER1);

                textBox53.Text = Convert.ToString(temp21);
                textBox52.Text = Convert.ToString(temp22);
                textBox50.Text = Convert.ToString(temp23);
                textBox49.Text = Convert.ToString(temp24);
                textBox48.Text = Convert.ToString(temp25);
                textBox47.Text = Convert.ToString(temp26);
                textBox46.Text = Convert.ToString(temp27);
                textBox45.Text = Convert.ToString(temp28);
                textBox44.Text = Convert.ToString(temp29);
                textBox43.Text = Convert.ToString(temp210);
                textBox57.Text = Convert.ToString(temp211);
                textBox42.Text = Convert.ToString(temp212);
                textBox41.Text = Convert.ToString(temp213);
                textBox40.Text = Convert.ToString(temp214);

                textBox73.Text = Convert.ToString(pres21);
                textBox72.Text = Convert.ToString(pres22);
                textBox71.Text = Convert.ToString(pres23);
                textBox70.Text = Convert.ToString(pres24);
                textBox69.Text = Convert.ToString(pres25);
                textBox68.Text = Convert.ToString(pres26);
                textBox63.Text = Convert.ToString(pres27);
                textBox62.Text = Convert.ToString(pres28);
                textBox61.Text = Convert.ToString(pres29);
                textBox60.Text = Convert.ToString(pres210);
                textBox59.Text = Convert.ToString(pres211);
                textBox58.Text = Convert.ToString(pres212);
                textBox67.Text = Convert.ToString(pres213);
                textBox66.Text = Convert.ToString(pres214);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state, point13_state, point14_state;

                textBox76.Text = Convert.ToString(w_dot_net2);
                textBox76.BackColor = Color.Yellow;

                textBox75.Text = Convert.ToString(eta_thermal2);
                textBox75.BackColor = Color.Yellow;

                textBox74.Text = Convert.ToString(massflow2);
                textBox74.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_rhx_in_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox32.Text = Convert.ToString(recomp_frac_guess2);
                textBox32.BackColor = Color.Yellow;

                textBox15.Text = Convert.ToString(p_pc_in2);
                textBox15.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(p_mc_out_guess2);
                textBox30.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(pres214 / pres213);
                textBox30.BackColor = Color.Yellow;

                textBox34.Text = Convert.ToString(lt_frac_guess2);
                textBox34.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "SF6")
            {
                sulfurhexafluoridesubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2, ref p_rhx_in_guess2, ref fixed_P_rhx_in2,
                               ref t_rht_in2, ref p_pc_in2, ref fixed_pc_in2, ref t_pc_in2, ref ua_rec_total2, ref eta_mc2, ref eta_rc2,
                               ref eta_pc2, ref eta_t2, ref eta_trh2, ref n_sub_hxrs2, ref p_high_limit2, ref p_mc_out_guess2, ref fixed_p_mc_out2,
                               ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2, ref fixed_recomp_frac2,
                               ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                               ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1,
                               ref dp2_phx2, ref dp2_rhx1, ref dp2_rhx2, ref dp2_cooler1, ref dp2_cooler2, ref temp21,
                               ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                               ref temp28, ref temp29, ref temp210, ref temp211, ref temp212, ref temp213, ref temp214,
                               ref pres21, ref pres22, ref pres23, ref pres24, ref pres25, ref pres26, ref pres27, ref pres28,
                               ref pres29, ref pres210, ref pres211, ref pres212, ref pres213, ref pres214, ref massflow2,
                               ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin,
                               ref LT_Pcout, ref LT_Phout, ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin,
                               ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout, ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref PHX1, ref RHX1, ref PC1, ref COOLER1);

                textBox53.Text = Convert.ToString(temp21);
                textBox52.Text = Convert.ToString(temp22);
                textBox50.Text = Convert.ToString(temp23);
                textBox49.Text = Convert.ToString(temp24);
                textBox48.Text = Convert.ToString(temp25);
                textBox47.Text = Convert.ToString(temp26);
                textBox46.Text = Convert.ToString(temp27);
                textBox45.Text = Convert.ToString(temp28);
                textBox44.Text = Convert.ToString(temp29);
                textBox43.Text = Convert.ToString(temp210);
                textBox57.Text = Convert.ToString(temp211);
                textBox42.Text = Convert.ToString(temp212);
                textBox41.Text = Convert.ToString(temp213);
                textBox40.Text = Convert.ToString(temp214);

                textBox73.Text = Convert.ToString(pres21);
                textBox72.Text = Convert.ToString(pres22);
                textBox71.Text = Convert.ToString(pres23);
                textBox70.Text = Convert.ToString(pres24);
                textBox69.Text = Convert.ToString(pres25);
                textBox68.Text = Convert.ToString(pres26);
                textBox63.Text = Convert.ToString(pres27);
                textBox62.Text = Convert.ToString(pres28);
                textBox61.Text = Convert.ToString(pres29);
                textBox60.Text = Convert.ToString(pres210);
                textBox59.Text = Convert.ToString(pres211);
                textBox58.Text = Convert.ToString(pres212);
                textBox67.Text = Convert.ToString(pres213);
                textBox66.Text = Convert.ToString(pres214);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state, point13_state, point14_state;

                textBox76.Text = Convert.ToString(w_dot_net2);
                textBox76.BackColor = Color.Yellow;

                textBox75.Text = Convert.ToString(eta_thermal2);
                textBox75.BackColor = Color.Yellow;

                textBox74.Text = Convert.ToString(massflow2);
                textBox74.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_rhx_in_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox32.Text = Convert.ToString(recomp_frac_guess2);
                textBox32.BackColor = Color.Yellow;

                textBox15.Text = Convert.ToString(p_pc_in2);
                textBox15.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(p_mc_out_guess2);
                textBox30.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(pres214 / pres213);
                textBox30.BackColor = Color.Yellow;

                textBox34.Text = Convert.ToString(lt_frac_guess2);
                textBox34.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "XENON")
            {
                xenonsubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2, ref p_rhx_in_guess2, ref fixed_P_rhx_in2,
                               ref t_rht_in2, ref p_pc_in2, ref fixed_pc_in2, ref t_pc_in2, ref ua_rec_total2, ref eta_mc2, ref eta_rc2,
                               ref eta_pc2, ref eta_t2, ref eta_trh2, ref n_sub_hxrs2, ref p_high_limit2, ref p_mc_out_guess2, ref fixed_p_mc_out2,
                               ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2, ref fixed_recomp_frac2,
                               ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                               ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1,
                               ref dp2_phx2, ref dp2_rhx1, ref dp2_rhx2, ref dp2_cooler1, ref dp2_cooler2, ref temp21,
                               ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                               ref temp28, ref temp29, ref temp210, ref temp211, ref temp212, ref temp213, ref temp214,
                               ref pres21, ref pres22, ref pres23, ref pres24, ref pres25, ref pres26, ref pres27, ref pres28,
                               ref pres29, ref pres210, ref pres211, ref pres212, ref pres213, ref pres214, ref massflow2,
                               ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin,
                               ref LT_Pcout, ref LT_Phout, ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin,
                               ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout, ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref PHX1, ref RHX1, ref PC1, ref COOLER1);

                textBox53.Text = Convert.ToString(temp21);
                textBox52.Text = Convert.ToString(temp22);
                textBox50.Text = Convert.ToString(temp23);
                textBox49.Text = Convert.ToString(temp24);
                textBox48.Text = Convert.ToString(temp25);
                textBox47.Text = Convert.ToString(temp26);
                textBox46.Text = Convert.ToString(temp27);
                textBox45.Text = Convert.ToString(temp28);
                textBox44.Text = Convert.ToString(temp29);
                textBox43.Text = Convert.ToString(temp210);
                textBox57.Text = Convert.ToString(temp211);
                textBox42.Text = Convert.ToString(temp212);
                textBox41.Text = Convert.ToString(temp213);
                textBox40.Text = Convert.ToString(temp214);

                textBox73.Text = Convert.ToString(pres21);
                textBox72.Text = Convert.ToString(pres22);
                textBox71.Text = Convert.ToString(pres23);
                textBox70.Text = Convert.ToString(pres24);
                textBox69.Text = Convert.ToString(pres25);
                textBox68.Text = Convert.ToString(pres26);
                textBox63.Text = Convert.ToString(pres27);
                textBox62.Text = Convert.ToString(pres28);
                textBox61.Text = Convert.ToString(pres29);
                textBox60.Text = Convert.ToString(pres210);
                textBox59.Text = Convert.ToString(pres211);
                textBox58.Text = Convert.ToString(pres212);
                textBox67.Text = Convert.ToString(pres213);
                textBox66.Text = Convert.ToString(pres214);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state, point13_state, point14_state;

                textBox76.Text = Convert.ToString(w_dot_net2);
                textBox76.BackColor = Color.Yellow;

                textBox75.Text = Convert.ToString(eta_thermal2);
                textBox75.BackColor = Color.Yellow;

                textBox74.Text = Convert.ToString(massflow2);
                textBox74.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_rhx_in_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox32.Text = Convert.ToString(recomp_frac_guess2);
                textBox32.BackColor = Color.Yellow;

                textBox15.Text = Convert.ToString(p_pc_in2);
                textBox15.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(p_mc_out_guess2);
                textBox30.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(pres214 / pres213);
                textBox30.BackColor = Color.Yellow;

                textBox34.Text = Convert.ToString(lt_frac_guess2);
                textBox34.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "NITROGEN")
            {
                nitrogensubplex_(ref w_dot_net2, ref t_mc_in2, ref t_t_in2, ref p_rhx_in_guess2, ref fixed_P_rhx_in2,
                               ref t_rht_in2, ref p_pc_in2, ref fixed_pc_in2, ref t_pc_in2, ref ua_rec_total2, ref eta_mc2, ref eta_rc2,
                               ref eta_pc2, ref eta_t2, ref eta_trh2, ref n_sub_hxrs2, ref p_high_limit2, ref p_mc_out_guess2, ref fixed_p_mc_out2,
                               ref pr_mc_guess2, ref fixed_pr_mc2, ref recomp_frac_guess2, ref fixed_recomp_frac2,
                               ref lt_frac_guess2, ref fixed_lt_frac2, ref tol2, ref opt_tol2, ref eta_thermal2,
                               ref dp2_lt1, ref dp2_lt2, ref dp2_ht1, ref dp2_ht2, ref dp2_pc1, ref dp2_pc2, ref dp2_phx1,
                               ref dp2_phx2, ref dp2_rhx1, ref dp2_rhx2, ref dp2_cooler1, ref dp2_cooler2, ref temp21,
                               ref temp22, ref temp23, ref temp24, ref temp25, ref temp26, ref temp27,
                               ref temp28, ref temp29, ref temp210, ref temp211, ref temp212, ref temp213, ref temp214,
                               ref pres21, ref pres22, ref pres23, ref pres24, ref pres25, ref pres26, ref pres27, ref pres28,
                               ref pres29, ref pres210, ref pres211, ref pres212, ref pres213, ref pres214, ref massflow2,
                               ref LT_mdoth, ref LT_mdotc, ref LT_Tcin, ref LT_Thin, ref LT_Pcin, ref LT_Phin,
                               ref LT_Pcout, ref LT_Phout, ref LT_Q, ref HT_mdoth, ref HT_mdotc, ref HT_Tcin, ref HT_Thin,
                               ref HT_Pcin, ref HT_Phin, ref HT_Pcout, ref HT_Phout, ref HT_Q, ref LT_UA, ref HT_UA, ref LT_Effc, ref HT_Effc, ref PHX1, ref RHX1, ref PC1, ref COOLER1);

                textBox53.Text = Convert.ToString(temp21);
                textBox52.Text = Convert.ToString(temp22);
                textBox50.Text = Convert.ToString(temp23);
                textBox49.Text = Convert.ToString(temp24);
                textBox48.Text = Convert.ToString(temp25);
                textBox47.Text = Convert.ToString(temp26);
                textBox46.Text = Convert.ToString(temp27);
                textBox45.Text = Convert.ToString(temp28);
                textBox44.Text = Convert.ToString(temp29);
                textBox43.Text = Convert.ToString(temp210);
                textBox57.Text = Convert.ToString(temp211);
                textBox42.Text = Convert.ToString(temp212);
                textBox41.Text = Convert.ToString(temp213);
                textBox40.Text = Convert.ToString(temp214);

                textBox73.Text = Convert.ToString(pres21);
                textBox72.Text = Convert.ToString(pres22);
                textBox71.Text = Convert.ToString(pres23);
                textBox70.Text = Convert.ToString(pres24);
                textBox69.Text = Convert.ToString(pres25);
                textBox68.Text = Convert.ToString(pres26);
                textBox63.Text = Convert.ToString(pres27);
                textBox62.Text = Convert.ToString(pres28);
                textBox61.Text = Convert.ToString(pres29);
                textBox60.Text = Convert.ToString(pres210);
                textBox59.Text = Convert.ToString(pres211);
                textBox58.Text = Convert.ToString(pres212);
                textBox67.Text = Convert.ToString(pres213);
                textBox66.Text = Convert.ToString(pres214);

                String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
                String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state, point13_state, point14_state;

                textBox76.Text = Convert.ToString(w_dot_net2);
                textBox76.BackColor = Color.Yellow;

                textBox75.Text = Convert.ToString(eta_thermal2);
                textBox75.BackColor = Color.Yellow;

                textBox74.Text = Convert.ToString(massflow2);
                textBox74.BackColor = Color.Yellow;

                textBox7.Text = Convert.ToString(p_rhx_in_guess2);
                textBox7.BackColor = Color.Yellow;

                textBox32.Text = Convert.ToString(recomp_frac_guess2);
                textBox32.BackColor = Color.Yellow;

                textBox15.Text = Convert.ToString(p_pc_in2);
                textBox15.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(p_mc_out_guess2);
                textBox30.BackColor = Color.Yellow;

                textBox30.Text = Convert.ToString(pres214 / pres213);
                textBox30.BackColor = Color.Yellow;

                textBox34.Text = Convert.ToString(lt_frac_guess2);
                textBox34.BackColor = Color.Yellow;

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
            }
        }

//Graph calculations Button
private void button2_Click(object sender, EventArgs e)
        {
            Double[] CIT;
            Double[] Eff;
            Double[] UA1;

            Double minimo = 0;
            Double maximo = 0;

            //HXs variables
            Int64 N_sub_hxrs;
            N_sub_hxrs = Convert.ToInt64(textBox20.Text);
            Double LT_Q_dot, LT_m_dot_c, LT_m_dot_h, LT_C_dot_hot, LT_C_dot_cold, LT_C_dot_min, LT_C_dot_max, LT_Q_dot_max, LT_eff;
            Double HT_Q_dot, HT_m_dot_c, HT_m_dot_h, HT_C_dot_hot, HT_C_dot_cold, HT_C_dot_min, HT_C_dot_max, HT_Q_dot_max, HT_eff;
            Double T_c_in, T_h_in, T_c_out, T_h_out, P_c_in, P_c_out, P_h_in, P_h_out;
            Double H_c_in, H_c_out, H_h_in, H_h_out;
            Double LT_UA_temp = 0;
            Double HT_UA_temp = 0;
            Double temp;
            Double HT_UA_min_DT = 0;
            Double LT_UA_min_DT = 0;
            Int64 error_code_temp = 0;

            Double[] T_c1_LT = new Double[N_sub_hxrs + 1];
            Double[] T_h1_LT = new Double[N_sub_hxrs + 1];

            Double[] T_c1_HT = new Double[N_sub_hxrs + 1];
            Double[] T_h1_HT = new Double[N_sub_hxrs + 1];

            Double[] P_c1_LT = new Double[N_sub_hxrs + 1];
            Double[] P_h1_LT = new Double[N_sub_hxrs + 1];

            Double[] P_c1_HT = new Double[N_sub_hxrs + 1];
            Double[] P_h1_HT = new Double[N_sub_hxrs + 1];

            Double[] UA_local = new Double[N_sub_hxrs];
            Double[] NTU_local = new Double[N_sub_hxrs];
            Double[] C_R_local = new Double[N_sub_hxrs];
            Double[] eff_local = new Double[N_sub_hxrs];

            Double Effec = 0;
            Double NTU = 0;
            Double CR = 0;

            T_h_out = 0;
            T_c_out = 0;      

            //Conductances values UA(kW/K)
            UA_Total_min = Convert.ToDouble(textBox51.Text);
            UA_Total_max = Convert.ToDouble(textBox55.Text);
            UA_Total_increment = Convert.ToDouble(textBox64.Text);

            Double maxiter_UA = 1;
            maxiter_UA = (UA_Total_max - UA_Total_min) / UA_Total_increment;

            Double UA_temp_value = UA_Total_min;
            Double maxiter = 1;

            double[] UA = new double[Convert.ToInt64(maxiter_UA + 1)];

            for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
            {
                textBox16.Text = Convert.ToString(UA_temp_value);

                CIT_min = Convert.ToDouble(textBox54.Text);

                if (CIT_min < 305.15)
                {
                    MessageBox.Show("Error, CIT should be above 305.15");
                }

                CIT_max = Convert.ToDouble(textBox56.Text);
                CIT_increment = Convert.ToDouble(textBox65.Text);

                maxiter = ((CIT_max - CIT_min) / CIT_increment) + 1;

                Double CIT_temp_value = CIT_min;

                for (int i = 0; i < Convert.ToInt64(maxiter); i++)
                {
                    textBox2.Text = Convert.ToString(CIT_temp_value);
                    textBox8.Text = Convert.ToString(CIT_temp_value);

                    Calculate_Design_Point();

                    //Calculate LT_HX Pinch Temperature and LT_HX Effectiveness
                    LT_Q_dot = LT_Q;
                    LT_m_dot_c = LT_mdotc;
                    LT_m_dot_h = LT_mdoth;
                    T_c_in = LT_Tcin;
                    T_h_in = LT_Thin;
                    P_c_in = LT_Pcin;
                    P_c_out = LT_Pcout;
                    P_h_in = LT_Phin;
                    P_h_out = LT_Phout;
                    N_sub_hxrs = n_sub_hxrs2;

                    //LT-Hx calculation
                    luis.calculate_hxr_UA(N_sub_hxrs, LT_Q_dot, LT_m_dot_c, LT_m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out,
                        ref error_code_temp, ref LT_UA_temp, ref LT_UA_min_DT, ref T_h1_LT, ref T_c1_LT, ref Effec, ref P_h1_LT, ref P_c1_LT, ref UA_local,
                        ref NTU, ref CR, ref NTU_local, ref C_R_local, ref eff_local);

                    //Calculate HT_HX Pinch Temperature and HT_HX Effectiveness
                    HT_Q_dot = HT_Q;
                    HT_m_dot_c = HT_mdotc;
                    HT_m_dot_h = HT_mdoth;
                    T_c_in = HT_Tcin;
                    T_h_in = HT_Thin;
                    P_c_in = HT_Pcin;
                    P_c_out = HT_Pcout;
                    P_h_in = HT_Phin;
                    P_h_out = HT_Phout;
                    N_sub_hxrs = n_sub_hxrs2;

                    //HT-Hx calculation
                    luis.calculate_hxr_UA(N_sub_hxrs, HT_Q_dot, HT_m_dot_c, HT_m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out,
                        ref error_code_temp, ref HT_UA_temp, ref HT_UA_min_DT, ref T_h1_HT, ref T_c1_HT, ref Effec, ref P_h1_HT, ref P_c1_HT, ref UA_local,
                        ref NTU, ref CR, ref NTU_local, ref C_R_local, ref eff_local);

                    //Calculate PHX                         
                    Double Q_dot_PHX = PHX1;
                    Double m_dot_c_PHX = massflow2;
                    Double T_c_in_PHX = temp25;

                    //Important fix the value AT_Main_SF=10K
                    Double T_h_in_PHX = temp26 + AT_Main_SF;
                    Double P_c_in_PHX, P_c_out_PHX;

                    P_c_in_PHX = pres25;
                    P_c_out_PHX = pres26;

                    long error_code_PHX = 0;
                    Double UA_PHX = 0;
                    Double min_DT_PHX = 0;
                    Double[] T_c_PHX = new Double[N_sub_hxrs + 1];
                    Double[] T_h_PHX = new Double[N_sub_hxrs + 1];
                    Double Effectiveness_PHX = 0;
                    Double NTU_Total_PHX = 0;
                    Double CR_Total_PHX = 1;
                    bool CR_Calculated_PHX = false;
                    Double[] P_c_PHX = new Double[N_sub_hxrs + 1];
                    Double[] P_h_PHX = new Double[N_sub_hxrs + 1];
                    Double[] UA_local_PHX = new Double[N_sub_hxrs];
                    Double[] NTU_local_PHX = new Double[N_sub_hxrs];
                    Double[] CR_local_PHX = new Double[N_sub_hxrs];
                    Double[] Effec_local_PHX = new Double[N_sub_hxrs];

                    if (Main_SF_Cp_HTF == 0)
                    {
                        if (comboBox7.Text == "Solar Salt")
                        {
                            Main_SF_Cp_HTF = 1.53;
                        }
                        if (comboBox7.Text == "Caloria")
                        {
                            Main_SF_Cp_HTF = 2.77;
                        }
                        if (comboBox7.Text == "Hitec XL")
                        {
                            Main_SF_Cp_HTF = 1.375;
                        }
                        if (comboBox7.Text == "Therminol VP1")
                        {
                            Main_SF_Cp_HTF = 2.634;
                        }
                        if (comboBox7.Text == "Syltherm_800")
                        {
                            Main_SF_Cp_HTF = 2.304;
                        }
                        if (comboBox7.Text == "Dowtherm_A")
                        {
                            Main_SF_Cp_HTF = 2.855;
                        }
                        if (comboBox7.Text == "Therminol_75")
                        {
                            Main_SF_Cp_HTF = 2.445;
                        }
                        if (comboBox7.Text == "Hitec")
                        {
                            Main_SF_Cp_HTF = 1.56;
                        }
                        if (comboBox7.Text == "Dowtherm Q")
                        {
                            Main_SF_Cp_HTF = 2.587;
                        }
                        if (comboBox7.Text == "Dowtherm RP")
                        {
                            Main_SF_Cp_HTF = 2.602;
                        }
                    }

                    if ((P_h_in_PHX == 0) || (P_h_out_PHX == 0))
                    {
                        if (comboBox7.Text == "Solar Salt")
                        {
                            P_h_in_PHX = 1500;
                            P_h_out_PHX = 1500;
                        }
                        else if (comboBox7.Text == "Caloria")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox7.Text == "Hitec XL")
                        {
                            P_h_in_PHX = 15000;
                            P_h_out_PHX = 15000;
                        }
                        else if (comboBox7.Text == "Therminol VP1")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox7.Text == "Syltherm_800")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox7.Text == "Dowtherm_A")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox7.Text == "Therminol_75")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox7.Text == "Hitec")
                        {
                            P_h_in_PHX = 1500;
                            P_h_out_PHX = 1500;
                        }
                        else if (comboBox7.Text == "Dowtherm Q")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox7.Text == "Dowtherm RP")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                    }

                    m_dot_h_PHX = 1200;

                    luis.calculate_PHX_UA(Main_SF_Cp_HTF, N_sub_hxrs_PHX, Q_dot_PHX, m_dot_c_PHX, ref m_dot_h_PHX, T_c_in_PHX, T_h_in_PHX, P_c_in_PHX, P_c_out_PHX, P_h_in_PHX, P_h_out_PHX, ref error_code_PHX, ref UA_PHX, ref min_DT_PHX, ref T_h_PHX, ref T_c_PHX,
                                                 ref Effectiveness_PHX, ref P_h_PHX, ref P_c_PHX, ref UA_local_PHX, ref NTU_Total_PHX, ref CR_Total_PHX, ref NTU_local_PHX, ref CR_local_PHX, ref Effec_local_PHX, ref CR_Calculated_PHX);

                    Main_SF_InletTemperature = T_h_PHX[N_sub_hxrs] - 273.15;
                    Main_SF_OutputTemperature = T_h_PHX[0] - 273.15;
                    Main_SF_SolarFieldThermalEnergy = Q_dot_PHX;
                    Main_SF_m_dot_h = m_dot_h_PHX;
                    Main_SF_Diameter_Interior = Main_SF_Exterior_Diameter - (2 * Main_SF_Receiver_Thickness);
                    Main_SF_HTF = comboBox7.Text;

                    if (comboBox4.Text == "Parabolic")
                    {
                        luis.PTC_Solar_Field_Design(Main_SF_HTF, Main_SF_zone, Main_SF_Lon, Main_SF_Lat, Main_SF_DNI, Main_SF_DAY, Main_SF_HOUR, Main_SF_NominalOpticalEfficiency, Main_SF_CleanlinessFactor, ref Main_SF_EndLossFactor, Main_SF_CollectorApertureWidth,
                                               Main_SF_SolarFieldThermalEnergy, ref Main_SF_NumberRows, Main_SF_InletTemperature, Main_SF_OutputTemperature, Main_SF_CoefficientA1, Main_SF_CoefficientA2, Main_SF_NumberOfSegments, Main_SF_Desired_Mass_Flux,
                                               Main_SF_Focal_length, Main_SF_Diameter_Interior, Main_SF_m_dot_h, Main_SF_Rugosidad, ref Main_SF_anginc, ref Main_SF_azimuth, ref Main_SF_angzenit, ref Main_SF_alt_solare,
                                               ref Main_SF_IAMLongitudinal, ref Main_SF_IAMTransversal, ref Main_SF_IAMOverall, ref Main_SF_ReflectorApertureArea, ref Main_SF_Total_Pressure_Drop);
                    }

                    else if (comboBox4.Text == "Fresnel")
                    {
                        //Thermoflow 21, Novatec Biosol, Fresnel
                        //Thermoflow 25, Novatec - Superheater (Fresnel)                       

                        luis.LF_Solar_Field_Design(Main_SF_HTF, Main_SF_zone, Main_SF_Lon, Main_SF_Lat, Main_SF_DNI, Main_SF_DAY, Main_SF_HOUR, Main_SF_NominalOpticalEfficiency, Main_SF_CleanlinessFactor, ref Main_SF_EndLossFactor, Main_SF_CollectorApertureWidth,
                                               Main_SF_SolarFieldThermalEnergy, ref Main_SF_NumberRows, Main_SF_InletTemperature, Main_SF_OutputTemperature, Main_SF_CoefficientA1, Main_SF_CoefficientA2, Main_SF_NumberOfSegments, Main_SF_Desired_Mass_Flux,
                                               Main_SF_Focal_length, Main_SF_Diameter_Interior, Main_SF_m_dot_h, Main_SF_Rugosidad, ref Main_SF_anginc_long, ref Main_SF_anginc_trans, ref Main_SF_azimuth, ref Main_SF_angzenit, ref Main_SF_alt_solare,
                                               ref Main_SF_IAMLongitudinal, ref Main_SF_IAMTransversal, ref Main_SF_IAMOverall, ref Main_SF_ReflectorApertureArea, ref Main_SF_Total_Pressure_Drop, Main_SF_IAM_Table_Name);
                    }

                    else if (comboBox4.Text == "Dual-Loop")
                    {

                    }

                    //Calculate RHX
                    Double Q_dot_RHX = RHX1;
                    Double m_dot_c_RHX = massflow2;
                    Double T_c_in_RHX = temp211;

                    //Important fix the value AT_Main_SF=10K
                    Double T_h_in_RHX = temp212 + AT_ReHeating_SF;
                    Double P_c_in_RHX, P_c_out_RHX;

                    P_c_in_RHX = pres211;
                    P_c_out_RHX = pres212;

                    long error_code_RHX = 0;
                    Double UA_RHX = 0;
                    Double min_DT_RHX = 0;
                    Double[] T_c_RHX = new Double[N_sub_hxrs + 1];
                    Double[] T_h_RHX = new Double[N_sub_hxrs + 1];
                    Double Effectiveness_RHX = 0;
                    Double NTU_Total_RHX = 0;
                    Double CR_Total_RHX = 1;
                    bool CR_Calculated_RHX = false;
                    Double[] P_c_RHX = new Double[N_sub_hxrs + 1];
                    Double[] P_h_RHX = new Double[N_sub_hxrs + 1];
                    Double[] UA_local_RHX = new Double[N_sub_hxrs];
                    Double[] NTU_local_RHX = new Double[N_sub_hxrs];
                    Double[] CR_local_RHX = new Double[N_sub_hxrs];
                    Double[] Effec_local_RHX = new Double[N_sub_hxrs];

                    if (ReHeating_SF_Cp_HTF == 0)
                    {
                        if (comboBox6.Text == "Solar Salt")
                        {
                            ReHeating_SF_Cp_HTF = 1.53;
                        }
                        if (comboBox6.Text == "Caloria")
                        {
                            ReHeating_SF_Cp_HTF = 2.77;
                        }
                        if (comboBox6.Text == "Hitec XL")
                        {
                            ReHeating_SF_Cp_HTF = 1.375;
                        }
                        if (comboBox6.Text == "Therminol VP1")
                        {
                            ReHeating_SF_Cp_HTF = 2.634;
                        }
                        if (comboBox6.Text == "Syltherm_800")
                        {
                            ReHeating_SF_Cp_HTF = 2.304;
                        }
                        if (comboBox6.Text == "Dowtherm_A")
                        {
                            ReHeating_SF_Cp_HTF = 2.855;
                        }
                        if (comboBox6.Text == "Therminol_75")
                        {
                            ReHeating_SF_Cp_HTF = 2.445;
                        }
                        if (comboBox6.Text == "Hitec")
                        {
                            ReHeating_SF_Cp_HTF = 1.56;
                        }
                        if (comboBox6.Text == "Dowtherm Q")
                        {
                            ReHeating_SF_Cp_HTF = 2.587;
                        }
                        if (comboBox6.Text == "Dowtherm RP")
                        {
                            ReHeating_SF_Cp_HTF = 2.602;
                        }
                    }

                    if ((P_h_in_PHX == 0) || (P_h_out_PHX == 0))
                    {
                        if (comboBox6.Text == "Solar Salt")
                        {
                            P_h_in_RHX = 1500;
                            P_h_out_RHX = 1500;
                        }
                        else if (comboBox6.Text == "Caloria")
                        {
                            P_h_in_RHX = 2500;
                            P_h_out_RHX = 2500;
                        }
                        else if (comboBox6.Text == "Hitec XL")
                        {
                            P_h_in_RHX = 15000;
                            P_h_out_RHX = 15000;
                        }
                        else if (comboBox6.Text == "Therminol VP1")
                        {
                            P_h_in_RHX = 2500;
                            P_h_out_RHX = 2500;
                        }
                        else if (comboBox6.Text == "Syltherm_800")
                        {
                            P_h_in_RHX = 2500;
                            P_h_out_RHX = 2500;
                        }
                        else if (comboBox6.Text == "Dowtherm_A")
                        {
                            P_h_in_RHX = 2500;
                            P_h_out_RHX = 2500;
                        }
                        else if (comboBox6.Text == "Therminol_75")
                        {
                            P_h_in_RHX = 2500;
                            P_h_out_RHX = 2500;
                        }
                        else if (comboBox6.Text == "Hitec")
                        {
                            P_h_in_RHX = 1500;
                            P_h_out_RHX = 1500;
                        }
                        else if (comboBox6.Text == "Dowtherm Q")
                        {
                            P_h_in_RHX = 2500;
                            P_h_out_RHX = 2500;
                        }
                        else if (comboBox6.Text == "Dowtherm RP")
                        {
                            P_h_in_RHX = 2500;
                            P_h_out_RHX = 2500;
                        }
                    }

                    m_dot_h_RHX = 1200;

                    luis.calculate_PHX_UA(ReHeating_SF_Cp_HTF, N_sub_hxrs_RHX, Q_dot_RHX, m_dot_c_RHX, ref m_dot_h_RHX, T_c_in_RHX, T_h_in_RHX, P_c_in_RHX, P_c_out_RHX, P_h_in_RHX, P_h_out_RHX, ref error_code_RHX, ref UA_RHX, ref min_DT_RHX, ref T_h_RHX, ref T_c_RHX,
                                                 ref Effectiveness_RHX, ref P_h_RHX, ref P_c_RHX, ref UA_local_RHX, ref NTU_Total_RHX, ref CR_Total_RHX, ref NTU_local_RHX, ref CR_local_RHX, ref Effec_local_RHX, ref CR_Calculated_RHX);

                    ReHeating_SF_InletTemperature = T_h_RHX[N_sub_hxrs] - 273.15;
                    ReHeating_SF_OutputTemperature = T_h_RHX[0] - 273.15;
                    ReHeating_SF_SolarFieldThermalEnergy = Q_dot_RHX;
                    ReHeating_SF_m_dot_h = m_dot_h_RHX;
                    ReHeating_SF_Diameter_Interior = ReHeating_SF_Exterior_Diameter - (2 * ReHeating_SF_Receiver_Thickness);
                    ReHeating_SF_HTF = comboBox6.Text;

                    if (comboBox5.Text == "Parabolic")
                    {
                        luis.PTC_Solar_Field_Design(ReHeating_SF_HTF, ReHeating_SF_zone, ReHeating_SF_Lon, ReHeating_SF_Lat, ReHeating_SF_DNI, ReHeating_SF_DAY, ReHeating_SF_HOUR, ReHeating_SF_NominalOpticalEfficiency, ReHeating_SF_CleanlinessFactor, ref ReHeating_SF_EndLossFactor, ReHeating_SF_CollectorApertureWidth,
                                           ReHeating_SF_SolarFieldThermalEnergy, ref ReHeating_SF_NumberRows, ReHeating_SF_InletTemperature, ReHeating_SF_OutputTemperature, ReHeating_SF_CoefficientA1, ReHeating_SF_CoefficientA2, ReHeating_SF_NumberOfSegments, ReHeating_SF_Desired_Mass_Flux,
                                           ReHeating_SF_Focal_length, ReHeating_SF_Diameter_Interior, ReHeating_SF_m_dot_h, ReHeating_SF_Rugosidad, ref ReHeating_SF_anginc, ref ReHeating_SF_azimuth, ref ReHeating_SF_angzenit, ref ReHeating_SF_alt_solare,
                                           ref ReHeating_SF_IAMLongitudinal, ref ReHeating_SF_IAMTransversal, ref ReHeating_SF_IAMOverall, ref ReHeating_SF_ReflectorApertureArea, ref ReHeating_SF_Total_Pressure_Drop);
                    }
                    
                    else if (comboBox5.Text == "Fresnel")
                    {
                        luis.LF_Solar_Field_Design(ReHeating_SF_HTF, ReHeating_SF_zone, ReHeating_SF_Lon, ReHeating_SF_Lat, ReHeating_SF_DNI, ReHeating_SF_DAY, ReHeating_SF_HOUR, ReHeating_SF_NominalOpticalEfficiency, ReHeating_SF_CleanlinessFactor, ref ReHeating_SF_EndLossFactor, ReHeating_SF_CollectorApertureWidth,
                                               ReHeating_SF_SolarFieldThermalEnergy, ref ReHeating_SF_NumberRows, ReHeating_SF_InletTemperature, ReHeating_SF_OutputTemperature, ReHeating_SF_CoefficientA1, ReHeating_SF_CoefficientA2, ReHeating_SF_NumberOfSegments, ReHeating_SF_Desired_Mass_Flux,
                                               ReHeating_SF_Focal_length, ReHeating_SF_Diameter_Interior, Main_SF_m_dot_h, ReHeating_SF_Rugosidad, ref ReHeating_SF_anginc_long, ref ReHeating_SF_anginc_trans, ref ReHeating_SF_azimuth, ref ReHeating_SF_angzenit, ref ReHeating_SF_alt_solare,
                                               ref ReHeating_SF_IAMLongitudinal, ref ReHeating_SF_IAMTransversal, ref ReHeating_SF_IAMOverall, ref ReHeating_SF_ReflectorApertureArea, ref ReHeating_SF_Total_Pressure_Drop, ReHeating_SF_IAM_Table_Name);
                    }

                    else if (comboBox4.Text == "Dual-Loop")
                    {

                    }

                    //Calculate Pre-Cooler (PC1)
                    //Water 25ºC Cp=4.18
                    Double Cp_HTF_PC1 = 4.18;
                    long N_sub_hxrs_PC1 = 15;
                    Double Q_dot_PC1 = -PC1;
                    Double m_dot_h_PC1 = massflow2;
                    Double m_dot_c_PC1 = 100000;
                    Double T_c_in_PC1 = 298.15;

                    //Important fix the value AT_Main_SF=10K
                    Double T_h_in_PC1 = temp29;
                    Double P_c_in_PC1, P_c_out_PC1;
                    P_c_in_PC1 = 500;
                    P_c_out_PC1 = 500;
                    Double P_h_in_PC1 = 0;
                    Double P_h_out_PC1 = 0;
                    P_h_in_PC1 = pres29;
                    P_h_out_PC1 = pres213;

                    long error_code_PC1 = 0;
                    Double UA_PC1 = 0;
                    Double min_DT_PC1 = 0;
                    Double[] T_c_PC1 = new Double[N_sub_hxrs + 1];
                    Double[] T_h_PC1 = new Double[N_sub_hxrs + 1];
                    Double Effectiveness_PC1 = 0;
                    Double NTU_Total_PC1 = 0;
                    Double CR_Total_PC1 = 1;
                    bool CR_Calculated_PC1 = false;
                    Double[] P_c_PC1 = new Double[N_sub_hxrs + 1];
                    Double[] P_h_PC1 = new Double[N_sub_hxrs + 1];
                    Double[] UA_local_PC1 = new Double[N_sub_hxrs];
                    Double[] NTU_local_PC1 = new Double[N_sub_hxrs];
                    Double[] CR_local_PC1 = new Double[N_sub_hxrs];
                    Double[] Effec_local_PC1 = new Double[N_sub_hxrs];

                    luis.calculate_Precooler_UA(Cp_HTF_PC1, N_sub_hxrs_PC1, Q_dot_PC1, ref m_dot_c_PC1, m_dot_h_PC1, T_c_in_PC1, T_h_in_PC1, P_c_in_PC1,
                                                P_c_out_PC1, P_h_in_PC1, P_h_out_PC1, ref error_code_PC1, ref UA_PC1, ref min_DT_PC1, ref T_h_PC1, ref T_c_PC1,
                                                ref Effectiveness_PC1, ref P_h_PC1, ref P_c_PC1, ref UA_local_PC1, ref NTU_Total_PC1, ref CR_Total_PC1,
                                                ref CR_Calculated_PC1);

                    //Calculate Pre-Cooler2
                    //Water 25ºC Cp=4.18
                    Double Cp_HTF_cooler1 = 4.18;
                    long N_sub_hxrs_cooler1 = 15;
                    Double Q_dot_cooler1 = -COOLER1;
                    Double m_dot_h_cooler1 = massflow2 * (1 - recomp_frac_guess2);
                    Double m_dot_c_cooler1 = 100000;
                    Double T_c_in_cooler1 = 298.15;

                    //Important fix the value AT_Main_SF=10K
                    Double T_h_in_cooler1 = temp214;
                    Double P_c_in_cooler1, P_c_out_cooler1;
                    P_c_in_cooler1 = 500;
                    P_c_out_cooler1 = 500;
                    Double P_h_in_cooler1 = 0;
                    Double P_h_out_cooler1 = 0;
                    P_h_in_cooler1 = pres214;
                    P_h_out_cooler1 = pres21;

                    long error_code_cooler1 = 0;
                    Double UA_cooler1 = 0;
                    Double min_DT_cooler1 = 0;
                    Double[] T_c_cooler1 = new Double[N_sub_hxrs + 1];
                    Double[] T_h_cooler1 = new Double[N_sub_hxrs + 1];
                    Double Effectiveness_cooler1 = 0;
                    Double NTU_Total_cooler1 = 0;
                    Double CR_Total_cooler1 = 1;
                    Double[] P_c_cooler1 = new Double[N_sub_hxrs + 1];
                    Double[] P_h_cooler1 = new Double[N_sub_hxrs + 1];
                    Double[] UA_local_cooler1 = new Double[N_sub_hxrs];
                    Double[] NTU_local_cooler1 = new Double[N_sub_hxrs];
                    Double[] CR_local_cooler1 = new Double[N_sub_hxrs];
                    Double[] Effec_local_cooler1 = new Double[N_sub_hxrs];

                    luis.calculate_Precooler_UA(Cp_HTF_cooler1, N_sub_hxrs_cooler1, Q_dot_cooler1, ref m_dot_c_cooler1, m_dot_h_cooler1, T_c_in_cooler1, T_h_in_cooler1, P_c_in_cooler1,
                                                P_c_out_PC1, P_h_in_cooler1, P_h_out_cooler1, ref error_code_cooler1, ref UA_cooler1, ref min_DT_cooler1, ref T_h_cooler1, ref T_c_cooler1,
                                                ref Effectiveness_cooler1, ref P_h_cooler1, ref P_c_cooler1, ref UA_local_cooler1, ref NTU_Total_cooler1, ref CR_Total_cooler1,
                                                ref CR_Calculated_PC1);

                    eta.SetValueXY(CIT_temp_value, eta_thermal2);
                    eta_list.Add(eta.XValue);
                    eta_list.Add(eta.YValues[0]);
                    eta_list.Add(UA_temp_value);

                    //LT Fraction & CIT
                    LT_frac_point.SetValueXY(CIT_temp_value, lt_frac_guess2);
                    LT_frac_list.Add(LT_frac_point.XValue);
                    LT_frac_list.Add(LT_frac_point.YValues[0]);
                    LT_frac_list.Add(UA_temp_value);

                    //LT_UA & CIT
                    LT_UA_point.SetValueXY(CIT_temp_value, LT_UA);
                    LT_UA_list.Add(LT_UA_point.XValue);
                    LT_UA_list.Add(LT_UA_point.YValues[0]);
                    LT_UA_list.Add(UA_temp_value);

                    //HT UA & CIT
                    HT_UA_point.SetValueXY(CIT_temp_value, HT_UA);
                    HT_UA_list.Add(HT_UA_point.XValue);
                    HT_UA_list.Add(HT_UA_point.YValues[0]);
                    HT_UA_list.Add(UA_temp_value);

                    //PR_Fraction & CIT
                    PR_frac_point.SetValueXY(CIT_temp_value, pr_mc_guess2);
                    PR_frac_list.Add(PR_frac_point.XValue);
                    PR_frac_list.Add(PR_frac_point.YValues[0]);
                    PR_frac_list.Add(UA_temp_value);

                    //CIP & CIT
                    CIP_point.SetValueXY(CIT_temp_value, pres21);
                    CIP_list.Add(PR_frac_point.XValue);
                    CIP_list.Add(PR_frac_point.YValues[0]);
                    CIP_list.Add(UA_temp_value);

                    //LT_Effectiveness & CIT
                    LT_HX_Eff.SetValueXY(CIT_temp_value, LT_Effc);
                    LT_HX_Eff_list.Add(LT_HX_Eff.XValue);
                    LT_HX_Eff_list.Add(LT_HX_Eff.YValues[0]);
                    LT_HX_Eff_list.Add(UA_temp_value);

                    //LT_Pinch_Point & CIT
                    LT_HX_min_DT.SetValueXY(CIT_temp_value, LT_UA_min_DT);
                    LT_HX_min_DT_list.Add(LT_HX_min_DT.XValue);
                    LT_HX_min_DT_list.Add(LT_HX_min_DT.YValues[0]);
                    LT_HX_min_DT_list.Add(UA_temp_value);

                    //HT Hx Pinch Point and Effectiveness & CIT
                    HT_HX_Eff.SetValueXY(CIT_temp_value, HT_Effc);
                    HT_HX_Eff_list.Add(HT_HX_Eff.XValue);
                    HT_HX_Eff_list.Add(HT_HX_Eff.YValues[0]);
                    HT_HX_Eff_list.Add(UA_temp_value);

                    //HT_Pinch_Point & CIT
                    HT_HX_min_DT.SetValueXY(CIT_temp_value, HT_UA_min_DT);
                    HT_HX_min_DT_list.Add(HT_HX_min_DT.XValue);
                    HT_HX_min_DT_list.Add(HT_HX_min_DT.YValues[0]);
                    HT_HX_min_DT_list.Add(UA_temp_value);

                    //Main SF_Effective_Aperture_Area & CIT
                    Main_SF_Effective_Aperture_Area.SetValueXY(CIT_temp_value, Main_SF_ReflectorApertureArea);
                    Main_SF_Effective_Aperture_Area_list.Add(Main_SF_Effective_Aperture_Area.XValue);
                    Main_SF_Effective_Aperture_Area_list.Add(Main_SF_Effective_Aperture_Area.YValues[0]);
                    Main_SF_Effective_Aperture_Area_list.Add(UA_temp_value);

                    //Main SF_Pressure_Drop & CIT
                    Main_SF_Pressure_Drop.SetValueXY(CIT_temp_value, Main_SF_Total_Pressure_Drop);
                    Main_SF_Pressure_Drop_list.Add(Main_SF_Pressure_Drop.XValue);
                    Main_SF_Pressure_Drop_list.Add(Main_SF_Pressure_Drop.YValues[0]);
                    Main_SF_Pressure_Drop_list.Add(UA_temp_value);

                    //PHX_Efficiency & CIT
                    PHX_Eff.SetValueXY(CIT_temp_value, Effectiveness_PHX);
                    PHX_Eff_list.Add(PHX_Eff.XValue);
                    PHX_Eff_list.Add(PHX_Eff.YValues[0]);
                    PHX_Eff_list.Add(UA_temp_value);

                    //PHX_UA & CIT
                    PHX_UA.SetValueXY(CIT_temp_value, UA_PHX);
                    PHX_UA_list.Add(PHX_UA.XValue);
                    PHX_UA_list.Add(PHX_UA.YValues[0]);
                    PHX_UA_list.Add(UA_temp_value);

                    //PHX_Q & CIT
                    PHX_Q.SetValueXY(CIT_temp_value, Q_dot_PHX);
                    PHX_Q_list.Add(PHX_Q.XValue);
                    PHX_Q_list.Add(PHX_Q.YValues[0]);
                    PHX_Q_list.Add(UA_temp_value);

                    //ReHeating SF_Effective_Aperture_Area & CIT
                    ReHeating_SF_Effective_Aperture_Area.SetValueXY(CIT_temp_value, ReHeating_SF_ReflectorApertureArea);
                    ReHeating_SF_Effective_Aperture_Area_list.Add(ReHeating_SF_Effective_Aperture_Area.XValue);
                    ReHeating_SF_Effective_Aperture_Area_list.Add(ReHeating_SF_Effective_Aperture_Area.YValues[0]);
                    ReHeating_SF_Effective_Aperture_Area_list.Add(UA_temp_value);

                    //ReHeating SF_Pressure_Drop & CIT
                    ReHeating_SF_Pressure_Drop.SetValueXY(CIT_temp_value, ReHeating_SF_Total_Pressure_Drop);
                    ReHeating_SF_Pressure_Drop_list.Add(ReHeating_SF_Pressure_Drop.XValue);
                    ReHeating_SF_Pressure_Drop_list.Add(ReHeating_SF_Pressure_Drop.YValues[0]);
                    ReHeating_SF_Pressure_Drop_list.Add(UA_temp_value);

                    //RHX_Efficiency & CIT
                    RHX_Eff.SetValueXY(CIT_temp_value, Effectiveness_RHX);
                    RHX_Eff_list.Add(RHX_Eff.XValue);
                    RHX_Eff_list.Add(RHX_Eff.YValues[0]);
                    RHX_Eff_list.Add(UA_temp_value);

                    //RHX_UA & CIT
                    RHX_UA.SetValueXY(CIT_temp_value, UA_RHX);
                    RHX_UA_list.Add(RHX_UA.XValue);
                    RHX_UA_list.Add(RHX_UA.YValues[0]);
                    RHX_UA_list.Add(UA_temp_value);

                    //RHX_Q & CIT
                    RHX_Q.SetValueXY(CIT_temp_value, Q_dot_RHX);
                    RHX_Q_list.Add(RHX_Q.XValue);
                    RHX_Q_list.Add(RHX_Q.YValues[0]);
                    RHX_Q_list.Add(UA_temp_value);

                    //PC1_Efficiency
                    PC1_Eff.SetValueXY(CIT_temp_value, Effectiveness_PC1);
                    PC1_Eff_list.Add(PC1_Eff.XValue);
                    PC1_Eff_list.Add(PC1_Eff.YValues[0]);
                    PC1_Eff_list.Add(UA_temp_value);

                    //PC1_UA
                    PC1_UA.SetValueXY(CIT_temp_value, UA_PC1);
                    PC1_UA_list.Add(PC1_UA.XValue);
                    PC1_UA_list.Add(PC1_UA.YValues[0]);
                    PC1_UA_list.Add(UA_temp_value);

                    //PC1_Q
                    PC1_Q.SetValueXY(CIT_temp_value, Q_dot_PC1);
                    PC1_Q_list.Add(PC1_Q.XValue);
                    PC1_Q_list.Add(PC1_Q.YValues[0]);
                    PC1_Q_list.Add(UA_temp_value);

                    //PC2_Efficiency
                    PC2_Eff.SetValueXY(CIT_temp_value, Effectiveness_cooler1);
                    PC2_Eff_list.Add(PC2_Eff.XValue);
                    PC2_Eff_list.Add(PC2_Eff.YValues[0]);
                    PC2_Eff_list.Add(UA_temp_value);

                    //PC2_UA
                    PC2_UA.SetValueXY(CIT_temp_value, UA_cooler1);
                    PC2_UA_list.Add(PC2_UA.XValue);
                    PC2_UA_list.Add(PC2_UA.YValues[0]);
                    PC2_UA_list.Add(UA_temp_value);

                    //PC2_Q
                    PC2_Q.SetValueXY(CIT_temp_value, Q_dot_cooler1);
                    PC2_Q_list.Add(PC2_Q.XValue);
                    PC2_Q_list.Add(PC2_Q.YValues[0]);
                    PC2_Q_list.Add(UA_temp_value);

                    CIT_temp_value = CIT_temp_value + CIT_increment;

                    //RESET Button
                    button14_Click(this, e);
                }

                UA[j] = UA_temp_value;
                UA_temp_value = UA_Total_increment + UA_temp_value;
            }

            int element1 = 1;
            int element2 = 2;

            comboBox8.Items.Clear();

            int contador = 0;

            //Net Plant Efficiency & CIT
            if (radioButton1.Checked == true)
            {
                CIT = new double[eta_list.Count / 3];
                Eff = new double[eta_list.Count / 3];
                UA1 = new double[eta_list.Count / 3];

                for (int element = 0; element < eta_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + eta_list[element] + " Effic.: " + eta_list[element1] + " UA: " + eta_list[element2]);
                    CIT[contador] = eta_list[element];
                    Eff[contador] = eta_list[element1];
                    UA1[contador] = eta_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_Fraction & CIT
            else if (radioButton2.Checked == true)
            {
                CIT = new double[LT_frac_list.Count / 3];
                Eff = new double[LT_frac_list.Count / 3];
                UA1 = new double[LT_frac_list.Count / 3];

                for (int element = 0; element < LT_frac_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + LT_frac_list[element] + " Effic.: " + LT_frac_list[element1] + " UA: " + LT_frac_list[element2]);
                    CIT[contador] = LT_frac_list[element];
                    Eff[contador] = LT_frac_list[element1];
                    UA1[contador] = LT_frac_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_UA & CIT
            else if (radioButton4.Checked == true)
            {
                CIT = new double[LT_UA_list.Count / 3];
                Eff = new double[LT_UA_list.Count / 3];
                UA1 = new double[LT_UA_list.Count / 3];

                for (int element = 0; element < LT_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + LT_UA_list[element] + " Effic.: " + LT_UA_list[element1] + " UA: " + LT_UA_list[element2]);
                    CIT[contador] = LT_UA_list[element];
                    Eff[contador] = LT_UA_list[element1];
                    UA1[contador] = LT_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //HT_UA & CIT
            else if (radioButton3.Checked == true)
            {
                CIT = new double[HT_UA_list.Count / 3];
                Eff = new double[HT_UA_list.Count / 3];
                UA1 = new double[HT_UA_list.Count / 3];

                for (int element = 0; element < HT_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + HT_UA_list[element] + " Effic.: " + HT_UA_list[element1] + " UA: " + HT_UA_list[element2]);
                    CIT[contador] = HT_UA_list[element];
                    Eff[contador] = HT_UA_list[element1];
                    UA1[contador] = HT_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PR_frac & CIT
            else if (radioButton5.Checked == true)
            {
                CIT = new double[PR_frac_list.Count / 3];
                Eff = new double[PR_frac_list.Count / 3];
                UA1 = new double[PR_frac_list.Count / 3];

                for (int element = 0; element < PR_frac_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PR_frac_list[element] + " Effic.: " + PR_frac_list[element1] + " UA: " + PR_frac_list[element2]);
                    CIT[contador] = PR_frac_list[element];
                    Eff[contador] = PR_frac_list[element1];
                    UA1[contador] = PR_frac_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //CIP & CIT
            else if (radioButton7.Checked == true)
            {
                CIT = new double[CIP_list.Count / 3];
                Eff = new double[CIP_list.Count / 3];
                UA1 = new double[CIP_list.Count / 3];

                for (int element = 0; element < CIP_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + CIP_list[element] + " Effic.: " + CIP_list[element1] + " UA: " + CIP_list[element2]);
                    CIT[contador] = CIP_list[element];
                    Eff[contador] = CIP_list[element1];
                    UA1[contador] = CIP_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_HX_Eff & CIT
            else if (radioButton8.Checked == true)
            {
                CIT = new double[LT_HX_Eff_list.Count / 3];
                Eff = new double[LT_HX_Eff_list.Count / 3];
                UA1 = new double[LT_HX_Eff_list.Count / 3];

                for (int element = 0; element < LT_HX_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + LT_HX_Eff_list[element] + " Effic.: " + LT_HX_Eff_list[element1] + " UA: " + LT_HX_Eff_list[element2]);
                    CIT[contador] = LT_HX_Eff_list[element];
                    Eff[contador] = LT_HX_Eff_list[element1];
                    UA1[contador] = LT_HX_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_HX_min_DT & CIT
            else if (radioButton9.Checked == true)
            {
                CIT = new double[LT_HX_min_DT_list.Count / 3];
                Eff = new double[LT_HX_min_DT_list.Count / 3];
                UA1 = new double[LT_HX_min_DT_list.Count / 3];

                for (int element = 0; element < LT_HX_min_DT_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + LT_HX_min_DT_list[element] + " Effic.: " + LT_HX_min_DT_list[element1] + " UA: " + LT_HX_min_DT_list[element2]);
                    CIT[contador] = LT_HX_min_DT_list[element];
                    Eff[contador] = LT_HX_min_DT_list[element1];
                    UA1[contador] = LT_HX_min_DT_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //HT_HX_Eff & CIT
            else if (radioButton11.Checked == true)
            {
                CIT = new double[HT_HX_Eff_list.Count / 3];
                Eff = new double[HT_HX_Eff_list.Count / 3];
                UA1 = new double[HT_HX_Eff_list.Count / 3];

                for (int element = 0; element < HT_HX_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + HT_HX_Eff_list[element] + " Effic.: " + HT_HX_Eff_list[element1] + " UA: " + HT_HX_Eff_list[element2]);
                    CIT[contador] = HT_HX_Eff_list[element];
                    Eff[contador] = HT_HX_Eff_list[element1];
                    UA1[contador] = HT_HX_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //HT_HX_min_DT & CIT
            else if (radioButton10.Checked == true)
            {
                CIT = new double[HT_HX_min_DT_list.Count / 3];
                Eff = new double[HT_HX_min_DT_list.Count / 3];
                UA1 = new double[HT_HX_min_DT_list.Count / 3];

                for (int element = 0; element < HT_HX_min_DT_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + HT_HX_min_DT_list[element] + " Effic.: " + HT_HX_min_DT_list[element1] + " UA: " + HT_HX_min_DT_list[element2]);
                    CIT[contador] = HT_HX_min_DT_list[element];
                    Eff[contador] = HT_HX_min_DT_list[element1];
                    UA1[contador] = HT_HX_min_DT_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //Main_SF_Effective_Aperture_Area & CIT
            else if (radioButton20.Checked == true)
            {
                CIT = new double[Main_SF_Effective_Aperture_Area_list.Count / 3];
                Eff = new double[Main_SF_Effective_Aperture_Area_list.Count / 3];
                UA1 = new double[Main_SF_Effective_Aperture_Area_list.Count / 3];

                for (int element = 0; element < Main_SF_Effective_Aperture_Area_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + Main_SF_Effective_Aperture_Area_list[element] + " Effic.: " + Main_SF_Effective_Aperture_Area_list[element1] + " UA: " + Main_SF_Effective_Aperture_Area_list[element2]);
                    CIT[contador] = Main_SF_Effective_Aperture_Area_list[element];
                    Eff[contador] = Main_SF_Effective_Aperture_Area_list[element1];
                    UA1[contador] = Main_SF_Effective_Aperture_Area_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //Main_SF_Pressure_Drop & CIT
            else if (radioButton15.Checked == true)
            {
                CIT = new double[Main_SF_Pressure_Drop_list.Count / 3];
                Eff = new double[Main_SF_Pressure_Drop_list.Count / 3];
                UA1 = new double[Main_SF_Pressure_Drop_list.Count / 3];

                for (int element = 0; element < Main_SF_Pressure_Drop_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + Main_SF_Pressure_Drop_list[element] + " Effic.: " + Main_SF_Pressure_Drop_list[element1] + " UA: " + Main_SF_Pressure_Drop_list[element2]);
                    CIT[contador] = Main_SF_Pressure_Drop_list[element];
                    Eff[contador] = Main_SF_Pressure_Drop_list[element1];
                    UA1[contador] = Main_SF_Pressure_Drop_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PHX_Eff & CIT
            else if (radioButton13.Checked == true)
            {
                CIT = new double[PHX_Eff_list.Count / 3];
                Eff = new double[PHX_Eff_list.Count / 3];
                UA1 = new double[PHX_Eff_list.Count / 3];

                for (int element = 0; element < PHX_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PHX_Eff_list[element] + " Effic.: " + PHX_Eff_list[element1] + " UA: " + PHX_Eff_list[element2]);
                    CIT[contador] = PHX_Eff_list[element];
                    Eff[contador] = PHX_Eff_list[element1];
                    UA1[contador] = PHX_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PHX_UA & CIT
            else if (radioButton6.Checked == true)
            {
                CIT = new double[PHX_UA_list.Count / 3];
                Eff = new double[PHX_UA_list.Count / 3];
                UA1 = new double[PHX_UA_list.Count / 3];

                for (int element = 0; element < PHX_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PHX_UA_list[element] + " Effic.: " + PHX_UA_list[element1] + " UA: " + PHX_UA_list[element2]);
                    CIT[contador] = PHX_UA_list[element];
                    Eff[contador] = PHX_UA_list[element1];
                    UA1[contador] = PHX_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PHX_Q & CIT
            else if (radioButton17.Checked == true)
            {
                CIT = new double[PHX_Q_list.Count / 3];
                Eff = new double[PHX_Q_list.Count / 3];
                UA1 = new double[PHX_Q_list.Count / 3];

                for (int element = 0; element < PHX_Q_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PHX_Q_list[element] + " Effic.: " + PHX_Q_list[element1] + " UA: " + PHX_Q_list[element2]);
                    CIT[contador] = PHX_Q_list[element];
                    Eff[contador] = PHX_Q_list[element1];
                    UA1[contador] = PHX_Q_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //ReHeating_SF_Effective_Aperture_Area & CIT
            else if (radioButton19.Checked == true)
            {
                CIT = new double[ReHeating_SF_Effective_Aperture_Area_list.Count / 3];
                Eff = new double[ReHeating_SF_Effective_Aperture_Area_list.Count / 3];
                UA1 = new double[ReHeating_SF_Effective_Aperture_Area_list.Count / 3];

                for (int element = 0; element < ReHeating_SF_Effective_Aperture_Area_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + ReHeating_SF_Effective_Aperture_Area_list[element] + " Effic.: " + ReHeating_SF_Effective_Aperture_Area_list[element1] + " UA: " + ReHeating_SF_Effective_Aperture_Area_list[element2]);
                    CIT[contador] = ReHeating_SF_Effective_Aperture_Area_list[element];
                    Eff[contador] = ReHeating_SF_Effective_Aperture_Area_list[element1];
                    UA1[contador] = ReHeating_SF_Effective_Aperture_Area_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //ReHeating_SF_Pressure_Drop & CIT
            else if (radioButton16.Checked == true)
            {
                CIT = new double[ReHeating_SF_Pressure_Drop_list.Count / 3];
                Eff = new double[ReHeating_SF_Pressure_Drop_list.Count / 3];
                UA1 = new double[ReHeating_SF_Pressure_Drop_list.Count / 3];

                for (int element = 0; element < ReHeating_SF_Pressure_Drop_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + ReHeating_SF_Pressure_Drop_list[element] + " Effic.: " + ReHeating_SF_Pressure_Drop_list[element1] + " UA: " + ReHeating_SF_Pressure_Drop_list[element2]);
                    CIT[contador] = ReHeating_SF_Pressure_Drop_list[element];
                    Eff[contador] = ReHeating_SF_Pressure_Drop_list[element1];
                    UA1[contador] = ReHeating_SF_Pressure_Drop_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //RHX_Eff & CIT
            else if (radioButton14.Checked == true)
            {
                CIT = new double[RHX_Eff_list.Count / 3];
                Eff = new double[RHX_Eff_list.Count / 3];
                UA1 = new double[RHX_Eff_list.Count / 3];

                for (int element = 0; element < RHX_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + RHX_Eff_list[element] + " Effic.: " + RHX_Eff_list[element1] + " UA: " + RHX_Eff_list[element2]);
                    CIT[contador] = RHX_Eff_list[element];
                    Eff[contador] = RHX_Eff_list[element1];
                    UA1[contador] = RHX_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //RHX_UA & CIT
            else if (radioButton12.Checked == true)
            {
                CIT = new double[RHX_UA_list.Count / 3];
                Eff = new double[RHX_UA_list.Count / 3];
                UA1 = new double[RHX_UA_list.Count / 3];

                for (int element = 0; element < RHX_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + RHX_UA_list[element] + " Effic.: " + RHX_UA_list[element1] + " UA: " + RHX_UA_list[element2]);
                    CIT[contador] = RHX_UA_list[element];
                    Eff[contador] = RHX_UA_list[element1];
                    UA1[contador] = RHX_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //RHX_Q & CIT
            else if (radioButton8.Checked == true)
            {
                CIT = new double[RHX_Q_list.Count / 3];
                Eff = new double[RHX_Q_list.Count / 3];
                UA1 = new double[RHX_Q_list.Count / 3];

                for (int element = 0; element < RHX_Q_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + RHX_Q_list[element] + " Effic.: " + RHX_Q_list[element1] + " UA: " + RHX_Q_list[element2]);
                    CIT[contador] = RHX_Q_list[element];
                    Eff[contador] = RHX_Q_list[element1];
                    UA1[contador] = RHX_Q_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC1_Eff & CIT
            else if (radioButton42.Checked == true)
            {
                CIT = new double[PC1_Eff_list.Count / 3];
                Eff = new double[PC1_Eff_list.Count / 3];
                UA1 = new double[PC1_Eff_list.Count / 3];

                for (int element = 0; element < PC1_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PC1_Eff_list[element] + " Effic.: " + PC1_Eff_list[element1] + " UA: " + PC1_Eff_list[element2]);
                    CIT[contador] = PC1_Eff_list[element];
                    Eff[contador] = PC1_Eff_list[element1];
                    UA1[contador] = PC1_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC1_UA & CIT
            else if (radioButton44.Checked == true)
            {
                CIT = new double[PC1_UA_list.Count / 3];
                Eff = new double[PC1_UA_list.Count / 3];
                UA1 = new double[PC1_UA_list.Count / 3];

                for (int element = 0; element < PC1_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PC1_UA_list[element] + " Effic.: " + PC1_UA_list[element1] + " UA: " + PC1_UA_list[element2]);
                    CIT[contador] = PC1_UA_list[element];
                    Eff[contador] = PC1_UA_list[element1];
                    UA1[contador] = PC1_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC1_Q & CIT
            else if (radioButton43.Checked == true)
            {
                CIT = new double[PC1_Q_list.Count / 3];
                Eff = new double[PC1_Q_list.Count / 3];
                UA1 = new double[PC1_Q_list.Count / 3];

                for (int element = 0; element < PC1_Q_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PC1_Q_list[element] + " Effic.: " + PC1_Q_list[element1] + " UA: " + PC1_Q_list[element2]);
                    CIT[contador] = PC1_Q_list[element];
                    Eff[contador] = PC1_Q_list[element1];
                    UA1[contador] = PC1_Q_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC2_Eff & CIT
            else if (radioButton45.Checked == true)
            {
                CIT = new double[PC2_Eff_list.Count / 3];
                Eff = new double[PC2_Eff_list.Count / 3];
                UA1 = new double[PC2_Eff_list.Count / 3];

                for (int element = 0; element < PC2_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PC2_Eff_list[element] + " Effic.: " + PC2_Eff_list[element1] + " UA: " + PC2_Eff_list[element2]);
                    CIT[contador] = PC2_Eff_list[element];
                    Eff[contador] = PC2_Eff_list[element1];
                    UA1[contador] = PC2_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC2_UA & CIT
            else if (radioButton47.Checked == true)
            {
                CIT = new double[PC2_UA_list.Count / 3];
                Eff = new double[PC2_UA_list.Count / 3];
                UA1 = new double[PC2_UA_list.Count / 3];

                for (int element = 0; element < PC2_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PC2_UA_list[element] + " Effic.: " + PC2_UA_list[element1] + " UA: " + PC2_UA_list[element2]);
                    CIT[contador] = PC2_UA_list[element];
                    Eff[contador] = PC2_UA_list[element1];
                    UA1[contador] = PC2_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC2_Q & CIT
            else if (radioButton46.Checked == true)
            {
                CIT = new double[PC2_Q_list.Count / 3];
                Eff = new double[PC2_Q_list.Count / 3];
                UA1 = new double[PC2_Q_list.Count / 3];

                for (int element = 0; element < PC2_Q_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PC2_Q_list[element] + " Effic.: " + PC2_Q_list[element1] + " UA: " + PC2_Q_list[element2]);
                    CIT[contador] = PC2_Q_list[element];
                    Eff[contador] = PC2_Q_list[element1];
                    UA1[contador] = PC2_Q_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("ChartArea1");

            //Net Plant Efficiency & CIT
            if (radioButton1.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.05), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.05), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < eta_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(eta_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(eta_list[counter2]) + "kW/K"].Points.AddXY(eta_list[counter], eta_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //LT_Fraction & CIT
            else if (radioButton2.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.05), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.05), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();                
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < LT_frac_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(LT_frac_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(LT_frac_list[counter2]) + "kW/K"].Points.AddXY(LT_frac_list[counter], LT_frac_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //LT_UA & CIT
            else if (radioButton4.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 500), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 500), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < LT_UA_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(LT_UA_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(LT_UA_list[counter2]) + "kW/K"].Points.AddXY(LT_UA_list[counter], LT_UA_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //HT_UA & CIT
            else if (radioButton3.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 500), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 500), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < HT_UA_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(HT_UA_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(HT_UA_list[counter2]) + "kW/K"].Points.AddXY(HT_UA_list[counter], HT_UA_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PR_frac & CIT
            else if (radioButton5.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PR_frac_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PR_frac_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PR_frac_list[counter2]) + "kW/K"].Points.AddXY(PR_frac_list[counter], PR_frac_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //CIP & CIT
            else if (radioButton7.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 100), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 100), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < CIP_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(CIP_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(CIP_list[counter2]) + "kW/K"].Points.AddXY(CIP_list[counter], CIP_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //LT_HX_Eff & CIT
            else if (radioButton8.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < LT_HX_Eff_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(LT_HX_Eff_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(LT_HX_Eff_list[counter2]) + "kW/K"].Points.AddXY(LT_HX_Eff_list[counter], LT_HX_Eff_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //LT_HX_min_DT & CIT
            else if (radioButton9.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < LT_HX_min_DT_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(LT_HX_min_DT_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(LT_HX_min_DT_list[counter2]) + "kW/K"].Points.AddXY(LT_HX_min_DT_list[counter], LT_HX_min_DT_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //HT_HX_Eff & CIT
            else if (radioButton11.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < HT_HX_Eff_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(HT_HX_Eff_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(HT_HX_Eff_list[counter2]) + "kW/K"].Points.AddXY(HT_HX_Eff_list[counter], HT_HX_Eff_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //HT_HX_min_DT & CIT
            else if (radioButton10.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < HT_HX_min_DT_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(HT_HX_min_DT_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(HT_HX_min_DT_list[counter2]) + "kW/K"].Points.AddXY(HT_HX_min_DT_list[counter], HT_HX_min_DT_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //Main_SF_Effective_Aperture_Area & CIT
            else if (radioButton20.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < Main_SF_Effective_Aperture_Area_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(Main_SF_Effective_Aperture_Area_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(Main_SF_Effective_Aperture_Area_list[counter2]) + "kW/K"].Points.AddXY(Main_SF_Effective_Aperture_Area_list[counter], Main_SF_Effective_Aperture_Area_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //Main_SF_Pressure_Drop & CIT
            else if (radioButton15.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < Main_SF_Pressure_Drop_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(Main_SF_Pressure_Drop_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(Main_SF_Pressure_Drop_list[counter2]) + "kW/K"].Points.AddXY(Main_SF_Pressure_Drop_list[counter], Main_SF_Pressure_Drop_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PHX_Eff & CIT
            else if (radioButton13.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();             
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PHX_Eff_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PHX_Eff_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PHX_Eff_list[counter2]) + "kW/K"].Points.AddXY(PHX_Eff_list[counter], PHX_Eff_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PHX_UA & CIT
            else if (radioButton6.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();                
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PHX_UA_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PHX_UA_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PHX_UA_list[counter2]) + "kW/K"].Points.AddXY(PHX_UA_list[counter], PHX_UA_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PHX_Q & CIT
            else if (radioButton17.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PHX_Q_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PHX_Q_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PHX_Q_list[counter2]) + "kW/K"].Points.AddXY(PHX_Q_list[counter], PHX_Q_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //ReHeating_SF_Effective_Aperture_Area & CIT
            else if (radioButton19.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < ReHeating_SF_Effective_Aperture_Area_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(ReHeating_SF_Effective_Aperture_Area_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(ReHeating_SF_Effective_Aperture_Area_list[counter2]) + "kW/K"].Points.AddXY(ReHeating_SF_Effective_Aperture_Area_list[counter], ReHeating_SF_Effective_Aperture_Area_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //ReHeating_SF_Pressure_Drop & CIT
            else if (radioButton16.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < ReHeating_SF_Pressure_Drop_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(ReHeating_SF_Pressure_Drop_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(ReHeating_SF_Pressure_Drop_list[counter2]) + "kW/K"].Points.AddXY(ReHeating_SF_Pressure_Drop_list[counter], ReHeating_SF_Pressure_Drop_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //RHX_Eff & CIT
            else if (radioButton14.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < RHX_Eff_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(RHX_Eff_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(RHX_Eff_list[counter2]) + "kW/K"].Points.AddXY(RHX_Eff_list[counter], RHX_Eff_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //RHX_UA & CIT
            else if (radioButton12.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < RHX_UA_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(RHX_UA_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(RHX_UA_list[counter2]) + "kW/K"].Points.AddXY(RHX_UA_list[counter], RHX_UA_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //RHX_Q & CIT
            else if (radioButton18.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();                           
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < RHX_Q_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(RHX_Q_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(RHX_Q_list[counter2]) + "kW/K"].Points.AddXY(RHX_Q_list[counter], RHX_Q_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PC1_Eff & CIT
            else if (radioButton42.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();             
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PC1_Eff_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PC1_Eff_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PC1_Eff_list[counter2]) + "kW/K"].Points.AddXY(PC1_Eff_list[counter], PC1_Eff_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PC1_UA & CIT
            else if (radioButton44.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PC1_UA_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PC1_UA_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PC1_UA_list[counter2]) + "kW/K"].Points.AddXY(PC1_UA_list[counter], PC1_UA_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PC1_Q & CIT
            else if (radioButton43.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                    chart1.Series.Clear();                
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PC1_Q_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PC1_Q_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PC1_Q_list[counter2]) + "kW/K"].Points.AddXY(PC1_Q_list[counter], PC1_Q_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PC2_Eff & CIT
            else if (radioButton45.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                chart1.Series.Clear();
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PC2_Eff_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PC2_Eff_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PC2_Eff_list[counter2]) + "kW/K"].Points.AddXY(PC2_Eff_list[counter], PC2_Eff_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PC2_UA & CIT
            else if (radioButton47.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                chart1.Series.Clear();
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PC2_UA_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PC2_UA_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PC2_UA_list[counter2]) + "kW/K"].Points.AddXY(PC2_UA_list[counter], PC2_UA_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            //PC2_Q & CIT
            else if (radioButton46.Checked == true)
            {
                //User-defined axis scales
                if (checkBox3.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox39.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox38.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox37.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox36.Text);
                }

                //User-defined axis scales
                else if (checkBox3.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = CIT_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = CIT_max;
                }
                chart1.Series.Clear();
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = CIT_increment;

                int counter1 = 1;
                int counter2 = 2;

                for (int j = 0; j < Convert.ToInt64(maxiter_UA); j++)
                {
                    chart1.Series.Add("UA=" + Convert.ToString(UA[j]) + "kW/K");
                    chart1.Series["UA=" + Convert.ToString(UA[j]) + "kW/K"].ChartType = SeriesChartType.Line;
                }

                for (int counter = 0; counter < PC2_Q_list.Count - 2; counter = counter + 3)
                {
                    chart1.Series["UA=" + Convert.ToString(PC2_Q_list[counter2]) + "kW/K"].BorderWidth = 2;
                    chart1.Series["UA=" + Convert.ToString(PC2_Q_list[counter2]) + "kW/K"].Points.AddXY(PC2_Q_list[counter], PC2_Q_list[counter1]);
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;
                }
            }

            eta_list.Clear();
            LT_frac_list.Clear();
            LT_UA_list.Clear();
            HT_UA_list.Clear();
            PR_frac_list.Clear();
            CIP_list.Clear();
            LT_HX_Eff_list.Clear();
            LT_HX_min_DT_list.Clear();
            HT_HX_Eff_list.Clear();
            HT_HX_min_DT_list.Clear();
            Main_SF_Effective_Aperture_Area_list.Clear();
            Main_SF_Pressure_Drop_list.Clear();
            PHX_Eff_list.Clear();
            PHX_UA_list.Clear();
            PHX_Q_list.Clear();
            ReHeating_SF_Effective_Aperture_Area_list.Clear();
            ReHeating_SF_Pressure_Drop_list.Clear();
            RHX_Eff_list.Clear();
            RHX_UA_list.Clear();
            RHX_Q_list.Clear();
            PC1_Eff_list.Clear();
            PC1_UA_list.Clear();
            PC1_Q_list.Clear();
            PC2_Eff_list.Clear();
            PC2_UA_list.Clear();
            PC2_Q_list.Clear();
        }

        //RESET Button
        private void button14_Click(object sender, EventArgs e)
        {
            //w_dot_net2 = Convert.ToDouble(textBox1.Text);
            textBox1.Text = "50000";
            //t_mc_in2 = Convert.ToDouble(textBox2.Text);
            //textBox2.Text = "305.15";
            //t_t_in2 = Convert.ToDouble(textBox4.Text);
            //textBox4.Text = "823.15";
            comboBox7_SelectedValueChanged(this, e);
            //p_rhx_in_guess2 = Convert.ToDouble(textBox7.Text);
            textBox7.Text = "11000";
            //fixed_P_rhx_in2 = Convert.ToBoolean(textBox3.Text);
            textBox3.Text = "False";
            //t_rht_in2 = Convert.ToDouble(textBox6.Text);
            //textBox6.Text = "823.15";
            comboBox6_SelectedValueChanged(this, e);
            //p_pc_in2 = Convert.ToDouble(textBox15.Text);
            textBox15.Text = "7400";
            //fixed_pc_in2 = Convert.ToBoolean(textBox35.Text);
            textBox35.Text = "False";
            //t_pc_in2 = Convert.ToDouble(textBox8.Text);
            //textBox8.Text = "305.15";
            //ua_rec_total2 = Convert.ToDouble(textBox16.Text);
            //textBox16.Text = "10000";
            //p_high_limit2 = Convert.ToDouble(textBox17.Text);
            textBox17.Text = "25000";

            //dp2_lt1 = Convert.ToDouble(textBox5.Text);
            //textBox5.Text = "0.0";
            //dp2_ht1 = Convert.ToDouble(textBox12.Text);
            //textBox12.Text = "0.0";
            //dp2_pc2 = Convert.ToDouble(textBox11.Text);
            //textBox11.Text = "0.0";
            //dp2_phx1 = Convert.ToDouble(textBox10.Text);
            //textBox10.Text = "0.0";
            //dp2_rhx1 = Convert.ToDouble(textBox9.Text);
            //textBox9.Text = "0.0";
            //dp2_lt2 = Convert.ToDouble(textBox26.Text);
            //textBox26.Text = "0.0";
            //dp2_ht2 = Convert.ToDouble(textBox25.Text);
            //textBox25.Text = "0.0";
            //dp2_cooler2 = Convert.ToDouble(textBox27.Text);
            //textBox27.Text = "0.0";

            //recomp_frac_guess2 = Convert.ToDouble(textBox32.Text);
            textBox32.Text = "0.1";
            //fixed_recomp_frac2 = Convert.ToBoolean(textBox31.Text);
            textBox31.Text = "False";
            //lt_frac_guess2 = Convert.ToDouble(textBox34.Text);
            textBox34.Text = "0.1";
            //fixed_lt_frac2 = Convert.ToBoolean(textBox33.Text);
            textBox33.Text = "False";
            //p_mc_out_guess2 = Convert.ToDouble(textBox23.Text);
            textBox23.Text = "25000";
            //fixed_p_mc_out2 = Convert.ToBoolean(textBox22.Text);
            textBox22.Text = "True";

            if (Convert.ToDouble(textBox16.Text) < 10000)
            {
                //pr_mc_guess2 = Convert.ToDouble(textBox30.Text);
                textBox30.Text = "2.0";
            }
            else if (Convert.ToDouble(textBox16.Text) > 10000)
            {
                //pr_mc_guess2 = Convert.ToDouble(textBox30.Text);
                textBox30.Text = "1.0";
            }

            //eta_mc2 = Convert.ToDouble(textBox14.Text);
            //textBox14.Text = "0.89";
            //eta_rc2 = Convert.ToDouble(textBox13.Text);
            //textBox13.Text = "0.89";
            //eta_pc2 = Convert.ToDouble(textBox24.Text);
            //textBox24.Text = "0.89";
            //eta_t2 = Convert.ToDouble(textBox19.Text);
            //textBox19.Text = "0.93";
            //eta_trh2 = Convert.ToDouble(textBox18.Text);
            //textBox18.Text = "0.93";

            //n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            textBox20.Text = "15";
            //tol2 = Convert.ToDouble(textBox21.Text);
            textBox21.Text = "0.00001";
            //opt_tol2 = Convert.ToDouble(textBox29.Text);
            textBox29.Text = "0.000001";
        }

        //OK Button
        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox7.Text == "Solar Salt")
            {
                textBox4.Text = "823.15";
            }

            else if (comboBox7.Text == "Hitec XL")
            {
                textBox4.Text = "793.15";
            }

            else if (comboBox7.Text == "Therminol VP1")
            {
                textBox4.Text = "663.15";
            }

            else if (comboBox7.Text == "Syltherm_800")
            {
                textBox4.Text = "663.15";
            }

            else if (comboBox7.Text == "Dowtherm_A")
            {
                textBox4.Text = "683.15";
            }

            else if (comboBox7.Text == "Therminol_75")
            {
                textBox4.Text = "648.15";
            }
        }

        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox6.Text == "Solar Salt")
            {
                textBox6.Text = "823.15";
            }

            else if (comboBox6.Text == "Hitec XL")
            {
                textBox6.Text = "793.15";
            }

            else if (comboBox6.Text == "Therminol VP1")
            {
                textBox6.Text = "663.15";
            }

            else if (comboBox6.Text == "Syltherm_800")
            {
                textBox6.Text = "663.15";
            }

            else if (comboBox6.Text == "Dowtherm_A")
            {
                textBox6.Text = "683.15";
            }

            else if (comboBox6.Text == "Therminol_75")
            {
                textBox6.Text = "648.15";
            }
        }

        //Main_SF_Input 
        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Parabolic")
            {
                Main_PTC_SF_PHX_Data_Input_Dialogue = new PTC_SF_PHX_Data_Input();
                Main_PTC_SF_PHX_Data_Input_Dialogue.pointer_function_CIT_PCRC_WithReHeating(this, "Main_SF");
                Main_PTC_SF_PHX_Data_Input_Dialogue.comboBox7.Text = comboBox7.Text;
                Main_PTC_SF_PHX_Data_Input_Dialogue.Show();
            }

            else if (comboBox4.Text == "Fresnel")
            {
                Main_LF_SF_PHX_Data_Input_Dialogue = new LF_SF_PHX_Data_Input();
                Main_LF_SF_PHX_Data_Input_Dialogue.pointer_function_CIT_PCRC_WithReHeating(this, "Main_SF");
                Main_LF_SF_PHX_Data_Input_Dialogue.comboBox1.Text = comboBox7.Text;
                Main_LF_SF_PHX_Data_Input_Dialogue.Show();
            }
        }

        //ReHeating_SF_Input
        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox5.Text == "Parabolic")
            {
                ReHeating_PTC_SF_PHX_Data_Input_Dialogue = new PTC_SF_PHX_Data_Input();
                ReHeating_PTC_SF_PHX_Data_Input_Dialogue.pointer_function_CIT_PCRC_WithReHeating(this, "ReHeating_SF");
                ReHeating_PTC_SF_PHX_Data_Input_Dialogue.comboBox7.Text = comboBox6.Text;
                ReHeating_PTC_SF_PHX_Data_Input_Dialogue.Show();
            }

            else if (comboBox5.Text == "Fresnel")
            {
                ReHeating_LF_SF_PHX_Data_Input_Dialogue = new LF_SF_PHX_Data_Input();
                ReHeating_LF_SF_PHX_Data_Input_Dialogue.pointer_function_CIT_PCRC_WithReHeating(this, "ReHeating_SF");
                ReHeating_LF_SF_PHX_Data_Input_Dialogue.comboBox1.Text = comboBox6.Text;
                ReHeating_LF_SF_PHX_Data_Input_Dialogue.Show();
            }
        }
    }
}
