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
    public partial class Effec_Recomp_Fract_withoutReHeating : Form
    {
        public core luis = new core();

        public PTC_SF_PHX_Data_Input Main_PTC_SF_PHX_Data_Input_Dialogue;
        public LF_SF_PHX_Data_Input Main_LF_SF_PHX_Data_Input_Dialogue;

        //PTC Data Input Dialogue
        public Double m_dot_h_PHX = 1200;
        public long N_sub_hxrs_PHX = 15;
        public Double P_h_in_PHX = 0;
        public Double P_h_out_PHX = 0;
        public Double Main_SF_Cp_HTF = 0;
        public Double AT_Main_SF = 10;

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
        public Double recomp_min, recomp_max, recomp_increment;
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

        public DataPoint Main_SF_Cost = new DataPoint();
        public List<Double> Main_SF_Cost_list = new List<Double>();

        public DataPoint Main_SF_Pressure_Drop = new DataPoint();
        public List<Double> Main_SF_Pressure_Drop_list = new List<Double>();

        public DataPoint PHX_Eff = new DataPoint();
        public List<Double> PHX_Eff_list = new List<Double>();

        public DataPoint PHX_UA = new DataPoint();
        public List<Double> PHX_UA_list = new List<Double>();

        public DataPoint PHX_Q = new DataPoint();
        public List<Double> PHX_Q_list = new List<Double>();

        public DataPoint PC1_Eff = new DataPoint();
        public List<Double> PC1_Eff_list = new List<Double>();

        public DataPoint PC1_UA = new DataPoint();
        public List<Double> PC1_UA_list = new List<Double>();

        public DataPoint PC1_Q = new DataPoint();
        public List<Double> PC1_Q_list = new List<Double>();

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

        const string refpropDLL_path2 = "RC_Ethane_Optimal.dll";
        [DllImport(refpropDLL_path2, EntryPoint = "ethanesubplex_", SetLastError = true)]
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

        const string refpropDLL_path3 = "RC_SF6_Optimal.dll";
        [DllImport(refpropDLL_path3, EntryPoint = "sulfurhexafluoridesubplex_", SetLastError = true)]
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

        const string refpropDLL_path4 = "RC_Xenon_Optimal.dll";
        [DllImport(refpropDLL_path4, EntryPoint = "xenonsubplex_", SetLastError = true)]
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

        const string refpropDLL_path5 = "RC_Nitrogen_Optimal.dll";
        [DllImport(refpropDLL_path5, EntryPoint = "nitrogensubplex_", SetLastError = true)]
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

        const string refpropDLL_path6 = "RC_Methane_Optimal.dll";
        [DllImport(refpropDLL_path6, EntryPoint = "methanesubplex_", SetLastError = true)]
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

        public Effec_Recomp_Fract_withoutReHeating()
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

                textBox16.Text = Convert.ToString(recomp_frac_guess2);
                textBox16.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "Ethane")
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

                textBox16.Text = Convert.ToString(recomp_frac_guess2);
                textBox16.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "SF6")
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

                textBox16.Text = Convert.ToString(recomp_frac_guess2);
                textBox16.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "Xenon")
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

                textBox16.Text = Convert.ToString(recomp_frac_guess2);
                textBox16.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "CH4")
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

                textBox16.Text = Convert.ToString(recomp_frac_guess2);
                textBox16.BackColor = Color.Yellow;

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
            }

            else if (comboBox2.Text == "N2")
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

                textBox16.Text = Convert.ToString(recomp_frac_guess2);
                textBox16.BackColor = Color.Yellow;

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

            }
        }

        //Optimization Button
        private void button1_Click(object sender, EventArgs e)
        {            
            Calculate_Design_Point();
        }

        //Dialog Close Button
        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Graph Design Button
        private void button2_Click(object sender, EventArgs e)
        {
            Double[] Recomp_Fract;
            Double[] Eff;
            Double[] UA1;

            Double minimo = 0;
            Double maximo = 0;

            //HXs variables
            Int64 N_sub_hxrs;
            N_sub_hxrs = Convert.ToInt64(textBox20.Text);
            Double LT_Q_dot, LT_m_dot_c, LT_m_dot_h, LT_C_dot_hot, LT_C_dot_cold, LT_C_dot_min, LT_C_dot_max, LT_Q_dot_max, LT_eff;
            Double HT_Q_dot, HT_m_dot_c, HT_m_dot_h, HT_C_dot_hot, HT_C_dot_cold, HT_C_dot_min, HT_C_dot_max, HT_Q_dot_max, HT_eff;
            Double T_c_in, T_h_in,T_c_out,T_h_out, P_c_in, P_c_out, P_h_in, P_h_out;
            Double H_c_in, H_c_out, H_h_in, H_h_out;
            Double LT_UA_temp =0;
            Double HT_UA_temp = 0;
            Double temp;
            Double HT_UA_min_DT=0;
            Double LT_UA_min_DT = 0;
            Int64 error_code_temp = 0 ;
            
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

            for (int j = 0; j< Convert.ToInt64(maxiter_UA); j++)
            {

            textBox17.Text = Convert.ToString(UA_temp_value);

            //Recompression Fraction True
            textBox15.Text = "True";
            recomp_min= Convert.ToDouble(textBox54.Text);

            if (recomp_min>0.75)
            {
                MessageBox.Show("Error, please recompresion fraction is better to be lower or equeal to 0.75");
            }

            recomp_max = Convert.ToDouble(textBox56.Text);
            recomp_increment = Convert.ToDouble(textBox65.Text);

            maxiter = ((recomp_max - recomp_min) / recomp_increment)+1;

            Double recomp_temp_value = recomp_min;
               
            for (int i = 0; i < Convert.ToInt64(maxiter); i++)
            {
               textBox16.Text = Convert.ToString(recomp_temp_value);

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
                    Double Q_dot_PHX = PHX_Q2;
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
                        if (comboBox5.Text == "Solar Salt")
                        {
                            Main_SF_Cp_HTF = 1.53;
                        }
                        if (comboBox5.Text == "Caloria")
                        {
                            Main_SF_Cp_HTF = 2.77;
                        }
                        if (comboBox5.Text == "Hitec XL")
                        {
                            Main_SF_Cp_HTF = 1.375;
                        }
                        if (comboBox5.Text == "Therminol VP1")
                        {
                            Main_SF_Cp_HTF = 2.634;
                        }
                        if (comboBox5.Text == "Syltherm_800")
                        {
                            Main_SF_Cp_HTF = 2.304;
                        }
                        if (comboBox5.Text == "Dowtherm_A")
                        {
                            Main_SF_Cp_HTF = 2.855;
                        }
                        if (comboBox5.Text == "Therminol_75")
                        {
                            Main_SF_Cp_HTF = 2.445;
                        }
                        if (comboBox5.Text == "Hitec")
                        {
                            Main_SF_Cp_HTF = 1.56;
                        }
                        if (comboBox5.Text == "Dowtherm Q")
                        {
                            Main_SF_Cp_HTF = 2.587;
                        }
                        if (comboBox5.Text == "Dowtherm RP")
                        {
                            Main_SF_Cp_HTF = 2.602;
                        }
                    }

                    if ((P_h_in_PHX == 0) || (P_h_out_PHX == 0))
                    {
                        if (comboBox5.Text == "Solar Salt")
                        {
                            P_h_in_PHX = 1500;
                            P_h_out_PHX = 1500;
                        }
                        else if (comboBox5.Text == "Caloria")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox5.Text == "Hitec XL")
                        {
                            P_h_in_PHX = 15000;
                            P_h_out_PHX = 15000;
                        }
                        else if (comboBox5.Text == "Therminol VP1")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox5.Text == "Syltherm_800")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox5.Text == "Dowtherm_A")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox5.Text == "Therminol_75")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox5.Text == "Hitec")
                        {
                            P_h_in_PHX = 1500;
                            P_h_out_PHX = 1500;
                        }
                        else if (comboBox5.Text == "Dowtherm Q")
                        {
                            P_h_in_PHX = 2500;
                            P_h_out_PHX = 2500;
                        }
                        else if (comboBox5.Text == "Dowtherm RP")
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
                    Main_SF_HTF = comboBox5.Text;

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

                    //Calculate Pre-Cooler1
                    //Water 25ºC Cp=4.18
                    Double Cp_HTF_cooler1 = 4.18;
                    long N_sub_hxrs_PC1 = 15;
                    Double Q_dot_PC1 = PC_Q2;
                    Double m_dot_h_PC1 = massflow2 * (1 - recomp_frac_guess2);
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
                    P_h_out_PC1 = pres21;

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

                    luis.calculate_Precooler_UA(Cp_HTF_cooler1, N_sub_hxrs_PC1, Q_dot_PC1, ref m_dot_c_PC1, m_dot_h_PC1, T_c_in_PC1, T_h_in_PC1, P_c_in_PC1,
                                                P_c_out_PC1, P_h_in_PC1, P_h_out_PC1, ref error_code_PC1, ref UA_PC1, ref min_DT_PC1, ref T_h_PC1, ref T_c_PC1,
                                                ref Effectiveness_PC1, ref P_h_PC1, ref P_c_PC1, ref UA_local_PC1, ref NTU_Total_PC1, ref CR_Total_PC1,
                                                ref CR_Calculated_PC1);

                    //Net_Efficiency & Recomp_Frac
                    eta.SetValueXY(recomp_temp_value, eta_thermal2);
                    eta_list.Add(eta.XValue);
                    eta_list.Add(eta.YValues[0]);
                    eta_list.Add(UA_temp_value);

                    //LT Fraction & Recomp_Frac
                    LT_frac_point.SetValueXY(recomp_temp_value, lt_frac_guess2);
                    LT_frac_list.Add(LT_frac_point.XValue);
                    LT_frac_list.Add(LT_frac_point.YValues[0]);
                    LT_frac_list.Add(UA_temp_value);

                    //LT_UA & Recomp_Frac
                    LT_UA_point.SetValueXY(recomp_temp_value, LT_UA);
                    LT_UA_list.Add(LT_UA_point.XValue);
                    LT_UA_list.Add(LT_UA_point.YValues[0]);
                    LT_UA_list.Add(UA_temp_value);

                    //HT UA & Recomp_Frac
                    HT_UA_point.SetValueXY(recomp_temp_value, HT_UA);
                    HT_UA_list.Add(HT_UA_point.XValue);
                    HT_UA_list.Add(HT_UA_point.YValues[0]);
                    HT_UA_list.Add(UA_temp_value);

                    //PR_Fraction & Recomp_Frac
                    PR_frac_point.SetValueXY(recomp_temp_value, pr_mc_guess2);
                    PR_frac_list.Add(PR_frac_point.XValue);
                    PR_frac_list.Add(PR_frac_point.YValues[0]);
                    PR_frac_list.Add(UA_temp_value);

                    //CIP & Recomp_Frac
                    CIP_point.SetValueXY(recomp_temp_value, pres21);
                    CIP_list.Add(PR_frac_point.XValue);
                    CIP_list.Add(PR_frac_point.YValues[0]);
                    CIP_list.Add(UA_temp_value);

                    //LT_Effectiveness & Recomp_Frac
                    LT_HX_Eff.SetValueXY(recomp_temp_value, LT_Effc);
                    LT_HX_Eff_list.Add(LT_HX_Eff.XValue);
                    LT_HX_Eff_list.Add(LT_HX_Eff.YValues[0]);
                    LT_HX_Eff_list.Add(UA_temp_value);

                    //LT_Pinch_Point & Recomp_Frac
                    LT_HX_min_DT.SetValueXY(recomp_temp_value, LT_UA_min_DT);
                    LT_HX_min_DT_list.Add(LT_HX_min_DT.XValue);
                    LT_HX_min_DT_list.Add(LT_HX_min_DT.YValues[0]);
                    LT_HX_min_DT_list.Add(UA_temp_value);

                    //HT Hx Pinch Point and Effectiveness & Recomp_Frac
                    HT_HX_Eff.SetValueXY(recomp_temp_value, HT_Effc);
                    HT_HX_Eff_list.Add(HT_HX_Eff.XValue);
                    HT_HX_Eff_list.Add(HT_HX_Eff.YValues[0]);
                    HT_HX_Eff_list.Add(UA_temp_value);

                    //HT_Pinch_Point & Recomp_Frac
                    HT_HX_min_DT.SetValueXY(recomp_temp_value, HT_UA_min_DT);
                    HT_HX_min_DT_list.Add(HT_HX_min_DT.XValue);
                    HT_HX_min_DT_list.Add(HT_HX_min_DT.YValues[0]);
                    HT_HX_min_DT_list.Add(UA_temp_value);

                    //Main SF_Effective_Aperture_Area & Recomp_Frac
                    Main_SF_Effective_Aperture_Area.SetValueXY(recomp_temp_value, Main_SF_ReflectorApertureArea);
                    Main_SF_Effective_Aperture_Area_list.Add(Main_SF_Effective_Aperture_Area.XValue);
                    Main_SF_Effective_Aperture_Area_list.Add(Main_SF_Effective_Aperture_Area.YValues[0]);
                    Main_SF_Effective_Aperture_Area_list.Add(UA_temp_value);

                    //Main SF_Pressure_Drop & Recomp_Frac
                    Main_SF_Pressure_Drop.SetValueXY(recomp_temp_value, Main_SF_Total_Pressure_Drop);
                    Main_SF_Pressure_Drop_list.Add(Main_SF_Pressure_Drop.XValue);
                    Main_SF_Pressure_Drop_list.Add(Main_SF_Pressure_Drop.YValues[0]);
                    Main_SF_Pressure_Drop_list.Add(UA_temp_value);

                    //PHX_Efficiency & Recomp_Frac
                    PHX_Eff.SetValueXY(recomp_temp_value, Effectiveness_PHX);
                    PHX_Eff_list.Add(PHX_Eff.XValue);
                    PHX_Eff_list.Add(PHX_Eff.YValues[0]);
                    PHX_Eff_list.Add(UA_temp_value);

                    //PHX_UA & Recomp_Frac
                    PHX_UA.SetValueXY(recomp_temp_value, UA_PHX);
                    PHX_UA_list.Add(PHX_UA.XValue);
                    PHX_UA_list.Add(PHX_UA.YValues[0]);
                    PHX_UA_list.Add(UA_temp_value);

                    //PHX_Q & Recomp_Frac
                    PHX_Q.SetValueXY(recomp_temp_value, Q_dot_PHX);
                    PHX_Q_list.Add(PHX_Q.XValue);
                    PHX_Q_list.Add(PHX_Q.YValues[0]);
                    PHX_Q_list.Add(UA_temp_value);

                    //PC1_Efficiency & Recomp_Frac
                    PC1_Eff.SetValueXY(recomp_temp_value, Effectiveness_PC1);
                    PC1_Eff_list.Add(PC1_Eff.XValue);
                    PC1_Eff_list.Add(PC1_Eff.YValues[0]);
                    PC1_Eff_list.Add(UA_temp_value);

                    //PC1_UA & Recomp_Frac
                    PC1_UA.SetValueXY(recomp_temp_value, UA_PC1);
                    PC1_UA_list.Add(PC1_UA.XValue);
                    PC1_UA_list.Add(PC1_UA.YValues[0]);
                    PC1_UA_list.Add(UA_temp_value);

                    //PC1_Q & Recomp_Frac
                    PC1_Q.SetValueXY(recomp_temp_value, Q_dot_PC1);
                    PC1_Q_list.Add(PC1_Q.XValue);
                    PC1_Q_list.Add(PC1_Q.YValues[0]);
                    PC1_Q_list.Add(UA_temp_value);

                    recomp_temp_value = recomp_temp_value + recomp_increment;
               
               //RESET Button
               button14_Click(this, e);
               //Recompression Fraction fixed set to "True"
               textBox15.Text = "True";
            }
                UA[j] = UA_temp_value;
                UA_temp_value = UA_Total_increment + UA_temp_value;
        }

            int element1 = 1;
            int element2 = 2;

            comboBox8.Items.Clear();

            int contador = 0;

            //Net Plant Efficiency & Recomp_Fract
            if (radioButton1.Checked == true)
            {
                Recomp_Fract = new double[eta_list.Count / 3];
                Eff = new double[eta_list.Count / 3];
                UA1 = new double[eta_list.Count / 3];

                for (int element = 0; element < eta_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + eta_list[element] + " Effic.: " + eta_list[element1] + " UA: " + eta_list[element2]);
                    Recomp_Fract[contador] = eta_list[element];
                    Eff[contador] = eta_list[element1];
                    UA1[contador] = eta_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_Fraction & Recomp_Fract
            else if (radioButton2.Checked == true)
            {
                Recomp_Fract = new double[LT_frac_list.Count / 3];
                Eff = new double[LT_frac_list.Count / 3];
                UA1 = new double[LT_frac_list.Count / 3];

                for (int element = 0; element < LT_frac_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + LT_frac_list[element] + " Effic.: " + LT_frac_list[element1] + " UA: " + LT_frac_list[element2]);
                    Recomp_Fract[contador] = LT_frac_list[element];
                    Eff[contador] = LT_frac_list[element1];
                    UA1[contador] = LT_frac_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_UA & Recomp_Fract
            else if (radioButton4.Checked == true)
            {
                Recomp_Fract = new double[LT_UA_list.Count / 3];
                Eff = new double[LT_UA_list.Count / 3];
                UA1 = new double[LT_UA_list.Count / 3];

                for (int element = 0; element < LT_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + LT_UA_list[element] + " Effic.: " + LT_UA_list[element1] + " UA: " + LT_UA_list[element2]);
                    Recomp_Fract[contador] = LT_UA_list[element];
                    Eff[contador] = LT_UA_list[element1];
                    UA1[contador] = LT_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //HT_UA & Recomp_Fract
            else if (radioButton3.Checked == true)
            {
                Recomp_Fract = new double[HT_UA_list.Count / 3];
                Eff = new double[HT_UA_list.Count / 3];
                UA1 = new double[HT_UA_list.Count / 3];

                for (int element = 0; element < HT_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + HT_UA_list[element] + " Effic.: " + HT_UA_list[element1] + " UA: " + HT_UA_list[element2]);
                    Recomp_Fract[contador] = HT_UA_list[element];
                    Eff[contador] = HT_UA_list[element1];
                    UA1[contador] = HT_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PR_frac & Recomp_Fract
            else if (radioButton5.Checked == true)
            {
                Recomp_Fract = new double[PR_frac_list.Count / 3];
                Eff = new double[PR_frac_list.Count / 3];
                UA1 = new double[PR_frac_list.Count / 3];

                for (int element = 0; element < PR_frac_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + PR_frac_list[element] + " Effic.: " + PR_frac_list[element1] + " UA: " + PR_frac_list[element2]);
                    Recomp_Fract[contador] = PR_frac_list[element];
                    Eff[contador] = PR_frac_list[element1];
                    UA1[contador] = PR_frac_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //CIP & Recomp_Fract
            else if (radioButton7.Checked == true)
            {
                Recomp_Fract = new double[CIP_list.Count / 3];
                Eff = new double[CIP_list.Count / 3];
                UA1 = new double[CIP_list.Count / 3];

                for (int element = 0; element < CIP_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + CIP_list[element] + " Effic.: " + CIP_list[element1] + " UA: " + CIP_list[element2]);
                    Recomp_Fract[contador] = CIP_list[element];
                    Eff[contador] = CIP_list[element1];
                    UA1[contador] = CIP_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_HX_Eff & Recomp_Fract
            else if (radioButton8.Checked == true)
            {
                Recomp_Fract = new double[LT_HX_Eff_list.Count / 3];
                Eff = new double[LT_HX_Eff_list.Count / 3];
                UA1 = new double[LT_HX_Eff_list.Count / 3];

                for (int element = 0; element < LT_HX_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + LT_HX_Eff_list[element] + " Effic.: " + LT_HX_Eff_list[element1] + " UA: " + LT_HX_Eff_list[element2]);
                    Recomp_Fract[contador] = LT_HX_Eff_list[element];
                    Eff[contador] = LT_HX_Eff_list[element1];
                    UA1[contador] = LT_HX_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //LT_HX_min_DT & Recomp_Fract
            else if (radioButton9.Checked == true)
            {
                Recomp_Fract = new double[LT_HX_min_DT_list.Count / 3];
                Eff = new double[LT_HX_min_DT_list.Count / 3];
                UA1 = new double[LT_HX_min_DT_list.Count / 3];

                for (int element = 0; element < LT_HX_min_DT_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + LT_HX_min_DT_list[element] + " Effic.: " + LT_HX_min_DT_list[element1] + " UA: " + LT_HX_min_DT_list[element2]);
                    Recomp_Fract[contador] = LT_HX_min_DT_list[element];
                    Eff[contador] = LT_HX_min_DT_list[element1];
                    UA1[contador] = LT_HX_min_DT_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //HT_HX_Eff & Recomp_Fract
            else if (radioButton11.Checked == true)
            {
                Recomp_Fract = new double[HT_HX_Eff_list.Count / 3];
                Eff = new double[HT_HX_Eff_list.Count / 3];
                UA1 = new double[HT_HX_Eff_list.Count / 3];

                for (int element = 0; element < HT_HX_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + HT_HX_Eff_list[element] + " Effic.: " + HT_HX_Eff_list[element1] + " UA: " + HT_HX_Eff_list[element2]);
                    Recomp_Fract[contador] = HT_HX_Eff_list[element];
                    Eff[contador] = HT_HX_Eff_list[element1];
                    UA1[contador] = HT_HX_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //HT_HX_min_DT & Recomp_Fract
            else if (radioButton10.Checked == true)
            {
                Recomp_Fract = new double[HT_HX_min_DT_list.Count / 3];
                Eff = new double[HT_HX_min_DT_list.Count / 3];
                UA1 = new double[HT_HX_min_DT_list.Count / 3];

                for (int element = 0; element < HT_HX_min_DT_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + HT_HX_min_DT_list[element] + " Effic.: " + HT_HX_min_DT_list[element1] + " UA: " + HT_HX_min_DT_list[element2]);
                    Recomp_Fract[contador] = HT_HX_min_DT_list[element];
                    Eff[contador] = HT_HX_min_DT_list[element1];
                    UA1[contador] = HT_HX_min_DT_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //Main_SF_Effective_Aperture_Area & Recomp_Fract
            else if (radioButton20.Checked == true)
            {
                Recomp_Fract = new double[Main_SF_Effective_Aperture_Area_list.Count / 3];
                Eff = new double[Main_SF_Effective_Aperture_Area_list.Count / 3];
                UA1 = new double[Main_SF_Effective_Aperture_Area_list.Count / 3];

                for (int element = 0; element < Main_SF_Effective_Aperture_Area_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + Main_SF_Effective_Aperture_Area_list[element] + " Effic.: " + Main_SF_Effective_Aperture_Area_list[element1] + " UA: " + Main_SF_Effective_Aperture_Area_list[element2]);
                    Recomp_Fract[contador] = Main_SF_Effective_Aperture_Area_list[element];
                    Eff[contador] = Main_SF_Effective_Aperture_Area_list[element1];
                    UA1[contador] = Main_SF_Effective_Aperture_Area_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //Main_SF_Pressure_Drop & Recomp_Fract
            else if (radioButton15.Checked == true)
            {
                Recomp_Fract = new double[Main_SF_Pressure_Drop_list.Count / 3];
                Eff = new double[Main_SF_Pressure_Drop_list.Count / 3];
                UA1 = new double[Main_SF_Pressure_Drop_list.Count / 3];

                for (int element = 0; element < Main_SF_Pressure_Drop_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("CIT: " + Main_SF_Pressure_Drop_list[element] + " Effic.: " + Main_SF_Pressure_Drop_list[element1] + " UA: " + Main_SF_Pressure_Drop_list[element2]);
                    Recomp_Fract[contador] = Main_SF_Pressure_Drop_list[element];
                    Eff[contador] = Main_SF_Pressure_Drop_list[element1];
                    UA1[contador] = Main_SF_Pressure_Drop_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PHX_Eff & Recomp_Fract
            else if (radioButton13.Checked == true)
            {
                Recomp_Fract = new double[PHX_Eff_list.Count / 3];
                Eff = new double[PHX_Eff_list.Count / 3];
                UA1 = new double[PHX_Eff_list.Count / 3];

                for (int element = 0; element < PHX_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + PHX_Eff_list[element] + " Effic.: " + PHX_Eff_list[element1] + " UA: " + PHX_Eff_list[element2]);
                    Recomp_Fract[contador] = PHX_Eff_list[element];
                    Eff[contador] = PHX_Eff_list[element1];
                    UA1[contador] = PHX_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PHX_UA & Recomp_Fract
            else if (radioButton6.Checked == true)
            {
                Recomp_Fract = new double[PHX_UA_list.Count / 3];
                Eff = new double[PHX_UA_list.Count / 3];
                UA1 = new double[PHX_UA_list.Count / 3];

                for (int element = 0; element < PHX_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + PHX_UA_list[element] + " Effic.: " + PHX_UA_list[element1] + " UA: " + PHX_UA_list[element2]);
                    Recomp_Fract[contador] = PHX_UA_list[element];
                    Eff[contador] = PHX_UA_list[element1];
                    UA1[contador] = PHX_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PHX_Q & Recomp_Fract
            else if (radioButton17.Checked == true)
            {
                Recomp_Fract = new double[PHX_Q_list.Count / 3];
                Eff = new double[PHX_Q_list.Count / 3];
                UA1 = new double[PHX_Q_list.Count / 3];

                for (int element = 0; element < PHX_Q_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + PHX_Q_list[element] + " Effic.: " + PHX_Q_list[element1] + " UA: " + PHX_Q_list[element2]);
                    Recomp_Fract[contador] = PHX_Q_list[element];
                    Eff[contador] = PHX_Q_list[element1];
                    UA1[contador] = PHX_Q_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }


            //PC1_Eff & Recomp_Fract
            else if (radioButton42.Checked == true)
            {
                Recomp_Fract = new double[PC1_Eff_list.Count / 3];
                Eff = new double[PC1_Eff_list.Count / 3];
                UA1 = new double[PC1_Eff_list.Count / 3];

                for (int element = 0; element < PC1_Eff_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + PC1_Eff_list[element] + " Effic.: " + PC1_Eff_list[element1] + " UA: " + PC1_Eff_list[element2]);
                    Recomp_Fract[contador] = PC1_Eff_list[element];
                    Eff[contador] = PC1_Eff_list[element1];
                    UA1[contador] = PC1_Eff_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC1_UA & Recomp_Fract
            else if (radioButton44.Checked == true)
            {
                Recomp_Fract = new double[PC1_UA_list.Count / 3];
                Eff = new double[PC1_UA_list.Count / 3];
                UA1 = new double[PC1_UA_list.Count / 3];

                for (int element = 0; element < PC1_UA_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + PC1_UA_list[element] + " Effic.: " + PC1_UA_list[element1] + " UA: " + PC1_UA_list[element2]);
                    Recomp_Fract[contador] = PC1_UA_list[element];
                    Eff[contador] = PC1_UA_list[element1];
                    UA1[contador] = PC1_UA_list[element2];

                    element1 = element1 + 3;
                    element2 = element2 + 3;
                    contador++;
                }

                minimo = Eff.Min();
                maximo = Eff.Max();
            }

            //PC1_Q & Recomp_Fract
            else if (radioButton43.Checked == true)
            {
                Recomp_Fract = new double[PC1_Q_list.Count / 3];
                Eff = new double[PC1_Q_list.Count / 3];
                UA1 = new double[PC1_Q_list.Count / 3];

                for (int element = 0; element < PC1_Q_list.Count - 2; element = element + 3)
                {
                    comboBox8.Items.Add("TIP: " + PC1_Q_list[element] + " Effic.: " + PC1_Q_list[element1] + " UA: " + PC1_Q_list[element2]);
                    Recomp_Fract[contador] = PC1_Q_list[element];
                    Eff[contador] = PC1_Q_list[element1];
                    UA1[contador] = PC1_Q_list[element2];

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

            //Net Plant Efficiency & TIP
            if (radioButton1.Checked == true)
            {
                //User-defined axis scales
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.05), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.05), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }

                chart1.Series.Clear();               
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //LT_Fraction & TIP
            else if (radioButton2.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.05), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.05), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }

                chart1.Series.Clear();              
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //LT_UA & TIP
            else if (radioButton4.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 500), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 500), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }

                    chart1.Series.Clear();                             
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //HT_UA & TIP
            else if (radioButton3.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 500), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 500), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //PR_frac & TIP
            else if (radioButton5.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //CIP & TIP
            else if (radioButton7.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 100), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 100), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //LT_HX_Eff & TIP
            else if (radioButton8.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //LT_HX_min_DT & TIP
            else if (radioButton9.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();            
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //HT_HX_Eff & TIP
            else if (radioButton11.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //HT_HX_min_DT & TIP
            else if (radioButton10.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //Main_SF_Effective_Aperture_Area & TIP
            else if (radioButton20.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();             
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //Main_SF_Pressure_Drop & TIP
            else if (radioButton15.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //PHX_Eff & TIP
            else if (radioButton13.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();                
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //PHX_UA & TIP
            else if (radioButton6.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //PHX_Q & TIP
            else if (radioButton17.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();               
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //PC1_Eff & TIP
            else if (radioButton42.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 0.1), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 0.1), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();              
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //PC1_UA & TIP
            else if (radioButton44.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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

            //PC1_Q & TIP
            else if (radioButton43.Checked == true)
            {
                if (checkBox1.Checked == true)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDouble(textBox6.Text);
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDouble(textBox3.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(textBox9.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(textBox8.Text);
                }

                //User-defined axis scales
                else if (checkBox1.Checked == false)
                {

                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(minimo - 1000), 2, MidpointRounding.AwayFromZero));
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(maximo + 1000), 2, MidpointRounding.AwayFromZero));

                    chart1.ChartAreas["ChartArea1"].AxisX.Minimum = recomp_min;
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = recomp_max;
                }
                    chart1.Series.Clear();
                    chart1.ChartAreas["ChartArea1"].AxisX.Interval = recomp_increment;

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
            PC1_Eff_list.Clear();
            PC1_UA_list.Clear();
            PC1_Q_list.Clear();
        }

        //RESET Button
        public void button14_Click(object sender, EventArgs e)
        {
            //w_dot_net2 = Convert.ToDouble(textBox1.Text);
            textBox1.Text = "50000.0";
            //t_mc_in2 = Convert.ToDouble(textBox2.Text);
            textBox2.Text = "305.15";
            //t_t_in2 = Convert.ToDouble(textBox4.Text);
            //textBox4.Text = "823.15";
            comboBox5_SelectedValueChanged(this, e);
            //ua_rec_total2 = Convert.ToDouble(textBox17.Text);
            //textBox17.Text = "10000";

            //eta_mc2 = Convert.ToDouble(textBox14.Text);
            //textBox14.Text = "0.89";
            //eta_rc2 = Convert.ToDouble(textBox13.Text);
            //textBox13.Text = "0.89";
            //eta_t2 = Convert.ToDouble(textBox19.Text);
            //textBox19.Text = "0.93";

            //n_sub_hxrs2 = Convert.ToInt16(textBox20.Text);
            textBox20.Text = "15";
            //p_high_limit2 = Convert.ToDouble(textBox57.Text);
            textBox57.Text = "25000";
            //p_mc_out_guess2 = Convert.ToDouble(textBox7.Text);
            textBox7.Text = "25000";
            //fixed_p_mc_out2 = Convert.ToBoolean(textBox58.Text);
            textBox58.Text = "True";
            //pr_mc_guess2 = Convert.ToDouble(textBox60.Text);
            textBox60.Text = "3.0";
            //fixed_pr_mc2 = Convert.ToBoolean(textBox59.Text);
            textBox59.Text = "False";

            //recomp_frac_guess2 = Convert.ToDouble(textBox16.Text);
            //textBox16.Text = "0.1";
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
            //textBox5.Text = "0.0";
            //dp2_lt2 = Convert.ToDouble(textBox26.Text);
            //textBox26.Text = "0.0";
            //dp2_ht1 = Convert.ToDouble(textBox12.Text);
            //textBox12.Text = "0.0";
            //dp2_ht2 = Convert.ToDouble(textBox25.Text);
            //textBox25.Text = "0.0";
            //dp2_pc1 = 0.0;
            //dp2_pc2 = Convert.ToDouble(textBox11.Text);
            //textBox11.Text = "0.0";
            //dp2_phx1 = Convert.ToDouble(textBox10.Text);
            //textBox10.Text = "0.0";
            //dp2_phx2 = 0.0;  

            //Power
            textBox48.Text = "0.0";
            //Mass Flow
            textBox49.Text = "0.0";
            //Net Plant efficiency
            textBox50.Text = "0.0";         
        }

        //SF_Data_Input_for_Sensing_Analysis
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Parabolic")
            {
                Main_PTC_SF_PHX_Data_Input_Dialogue = new PTC_SF_PHX_Data_Input();
                Main_PTC_SF_PHX_Data_Input_Dialogue.pointer_function_Recomp_Fract_RC_WithoutReHeating(this, "Main_SF");
                Main_PTC_SF_PHX_Data_Input_Dialogue.comboBox7.Text = comboBox5.Text;
                Main_PTC_SF_PHX_Data_Input_Dialogue.Show();
            }

            else if (comboBox4.Text == "Fresnel")
            {
                Main_LF_SF_PHX_Data_Input_Dialogue = new LF_SF_PHX_Data_Input();
                Main_LF_SF_PHX_Data_Input_Dialogue.pointer_function_Recomp_Fract_RC_WithoutReHeating(this, "Main_SF");
                Main_LF_SF_PHX_Data_Input_Dialogue.comboBox7.Text = comboBox5.Text;
                Main_LF_SF_PHX_Data_Input_Dialogue.Show();
            }
        }

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
    }
}
