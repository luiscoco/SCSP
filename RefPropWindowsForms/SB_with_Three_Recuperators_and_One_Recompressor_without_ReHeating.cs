using System;
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
    public partial class SB_with_Three_Recuperators_and_One_Recompressor_without_ReHeating : Form
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
        public Double ua_lt2, ua_ht2;
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_t2;
        public Double eta_trh2;
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

        public Double massflow2;
        public Double LT_mdoth, LT_mdotc, LT_Tcin, LT_Thin, LT_Pcin, LT_Phin;
        public Double LT_Pcout, LT_Phout, LT_Q, HT_mdoth, HT_mdotc, HT_Tcin, HT_Thin;
        public Double HT_Pcin, HT_Phin, HT_Pcout, HT_Phout, HT_Q, LT_UA, HT_UA;
        public Double PHX_Q2, RHX_Q2, PC_Q2;
        public Double LT_Effc, HT_Effc;
        public Double N_design2 = 0;


        public SB_with_Three_Recuperators_and_One_Recompressor_without_ReHeating(MainWindow punteroMainWindow1)
        {
            punteroMainWindow = punteroMainWindow1;
            InitializeComponent();
        }

        //Mixtures calculations
        private void button11_Click(object sender, EventArgs e)
        {
            int maxIterations = 100;
            int numIterations = 0;

            //PureFluid
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                luis.core1(this.comboBox2.Text, category);
            }

            //NewMixture
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                luis.core1(this.comboBox2.Text + "=" + textBox31.Text + "," + this.comboBox6.Text + "=" + textBox36.Text + "," + this.comboBox7.Text + "=" + textBox67.Text + "," + this.comboBox12.Text + "=" + textBox68.Text, category);
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

            Double w_dot_net2 = Convert.ToDouble(textBox1.Text);
            Double t_mc_in2 = Convert.ToDouble(textBox2.Text);
            Double t_t_in2 = Convert.ToDouble(textBox4.Text);
            Double p_mc_in2 = Convert.ToDouble(textBox3.Text);
            Double p_mc_out2 = Convert.ToDouble(textBox8.Text);
            Double ua_lt2 = Convert.ToDouble(textBox17.Text);
            Double ua_mt2 = Convert.ToDouble(textBox84.Text);
            Double ua_ht2 = Convert.ToDouble(textBox16.Text);

            Double dp2_lt1 = Convert.ToDouble(textBox5.Text);
            Double dp2_lt2 = Convert.ToDouble(textBox26.Text);
            Double dp2_mt1 = Convert.ToDouble(textBox82.Text);
            Double dp2_mt2 = Convert.ToDouble(textBox83.Text);
            Double dp2_ht1 = Convert.ToDouble(textBox12.Text);
            Double dp2_ht2 = Convert.ToDouble(textBox25.Text);
            Double dp2_pc2 = Convert.ToDouble(textBox11.Text);
            Double dp2_phx1 = Convert.ToDouble(textBox10.Text);

            Double eta_mc2 = Convert.ToDouble(textBox14.Text);
            Double eta_t2 = Convert.ToDouble(textBox19.Text);
            long n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            Double tol2 = Convert.ToDouble(textBox21.Text);

            luis.wmm = luis.working_fluid.MolecularWeight;

            core.RecompCycle_with_Three_Recuperatos_withoutRH cicloRC = new core.RecompCycle_with_Three_Recuperatos_withoutRH();

            luis.SimpleBrayton_with_Three_Recuperators_One_RC_without_ReHeating(luis, ref cicloRC,
            w_dot_net2, t_mc_in2, t_t_in2, p_mc_in2, p_mc_out2, -dp2_lt1, -dp2_mt1,
            -dp2_ht1, -dp2_pc2, -dp2_phx1, -dp2_lt2, -dp2_mt2, -dp2_ht2, ua_lt2, ua_mt2, ua_ht2,
            eta_mc2, eta_t2, n_sub_hxrs2, tol2);

            massflow2 = cicloRC.m_dot_turbine;
            w_dot_net2 = cicloRC.W_dot_net;
            eta_thermal2 = cicloRC.eta_thermal;
            recomp_frac2 = cicloRC.recomp_frac;

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

            textBox50.Text = Convert.ToString(cicloRC.eta_thermal);

            textBox22.Text = Convert.ToString(pres21);
            textBox23.Text = Convert.ToString(pres22);
            textBox27.Text = Convert.ToString(pres23);
            textBox24.Text = Convert.ToString(pres24);
            textBox29.Text = Convert.ToString(pres25);
            textBox28.Text = Convert.ToString(pres26);
            textBox41.Text = Convert.ToString(pres27);
            textBox38.Text = Convert.ToString(pres28);
            textBox34.Text = Convert.ToString(pres29);
            textBox40.Text = Convert.ToString(pres210);

            textBox47.Text = Convert.ToString(temp21);
            textBox46.Text = Convert.ToString(temp22);
            textBox45.Text = Convert.ToString(temp23);
            textBox44.Text = Convert.ToString(temp24);
            textBox43.Text = Convert.ToString(temp25);
            textBox42.Text = Convert.ToString(temp26);
            textBox35.Text = Convert.ToString(temp27);
            textBox33.Text = Convert.ToString(temp28);
            textBox32.Text = Convert.ToString(temp29);
            textBox39.Text = Convert.ToString(temp210);

            textBox48.Text = Convert.ToString(w_dot_net2);
            textBox49.Text = Convert.ToString(massflow2);
            textBox50.Text = Convert.ToString(eta_thermal2 * 100);
        }

        //Set critical conditions
        private void button13_Click(object sender, EventArgs e)
        {
            double option1 = 0.0;
            double option2 = 0.0;
            double option3 = 0.0;
            double option4 = 0.0;

            option1 = Convert.ToDouble(this.textBox31.Text);
            option2 = Convert.ToDouble(this.textBox36.Text);
            option3 = Convert.ToDouble(this.textBox67.Text);
            option4 = Convert.ToDouble(this.textBox68.Text);

            if ((option1 == 1) || (option2 == 1) || (option3 == 1) || (option4 == 1))
            {
                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture,
                    this.comboBox2.Text + "=" + textBox31.Text + "," +
                    this.comboBox6.Text + "=" + textBox36.Text + "," +
                    this.comboBox7.Text + "=" + textBox67.Text + "," +
                    this.comboBox12.Text + "=" + textBox68.Text, ReferenceState.DEF);

                textBox37.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox51.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox52.Text = Convert.ToString(working_fluid.CriticalDensity);

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
                xlWorkSheet.Cells[14, 6] = this.comboBox6.Text;
                xlWorkSheet.Cells[15, 6] = this.comboBox7.Text;
                xlWorkSheet.Cells[16, 6] = this.comboBox12.Text;

                // % Compositions
                xlWorkSheet.Cells[13, 7] = this.textBox31.Text;
                xlWorkSheet.Cells[14, 7] = this.textBox36.Text;
                xlWorkSheet.Cells[15, 7] = this.textBox67.Text;
                xlWorkSheet.Cells[16, 7] = this.textBox68.Text;

                //MessageBox.Show(xlWorkSheet.get_Range("D68", "D68").Value2.ToString());
                this.textBox3.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox2.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();

                MixtureCriticalPressure = xlWorkSheet.get_Range("D69", "D69").Value;
                MixtureCriticalTemperature = xlWorkSheet.get_Range("D68", "D68").Value2;

                this.textBox37.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox51.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox52.Text = xlWorkSheet.get_Range("D70", "D70").Value2.ToString();

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


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                comboBox1.Enabled = false;
                textBox60.Enabled = false;
                textBox61.Enabled = false;
                textBox51.Enabled = false;
                textBox80.Enabled = false;
                button22.Enabled = false;
                button2.Enabled = true;
            }

            else if (comboBox1.Text == "NewMixture")
            {
                comboBox2.Enabled = true;
                comboBox6.Enabled = true;
                comboBox7.Enabled = true;
                comboBox12.Enabled = true;

                textBox31.Enabled = true;
                textBox36.Enabled = true;
                textBox67.Enabled = true;
                textBox68.Enabled = true;

                button2.Enabled = true;
                button11.Enabled = true;

                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox31.Text + "," + this.comboBox6.Text + "=" + textBox36.Text + "," + this.comboBox7.Text + "=" + textBox67.Text + "," + this.comboBox12.Text + "=" + textBox68.Text, ReferenceState.DEF);

                textBox59.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox58.Text = Convert.ToString(working_fluid.CriticalTemperature - 273.15);
                textBox55.Text = Convert.ToString(working_fluid.CriticalDensity);

                MixtureCriticalPressure = working_fluid.CriticalPressure;
                MixtureCriticalTemperature = working_fluid.CriticalTemperature;
            }
        }

        //Optimization Analysis
        private void button34_Click(object sender, EventArgs e)
        {

        }
    }
}
