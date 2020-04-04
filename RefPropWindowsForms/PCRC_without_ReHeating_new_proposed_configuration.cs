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

using NLoptNet;

namespace RefPropWindowsForms
{
    public partial class PCRC_without_ReHeating_new_proposed_configuration : Form
    {
        public double MixtureCriticalPressure = 0.0;
        public double MixtureCriticalTemperature = 0.0;

        public MainWindow punteroMainWindow = new MainWindow();

        public core luis = new core();

        public Net_Power Net_Power_dialog;

        public Diagrams Diagrams_dialog;

        public Final_Report Final_Report_dialog;

        public Generator Generator_dialog;

        public Estimated_Cost Estimated_Cost_Dialog;

        public Fresnel SF_PHX_LF;
        public Fresnel SF_RHX_LF;

        public PTC_Solar_Field SF_PHX;
        public PTC_Solar_Field SF_RHX;

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

        public PreeCooler Precooler_dialog1;
        public PreeCooler Precooler_dialog2;

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;
        public Radial_Turbine ReHeating_Turbine;

        //First calculate the Main Compressor Rotational speed and after send that value to the Turbines
        public Double N_design_Main_Compressor;

        public snl_compressor_tsr Main_Compressor;
        public snl_compressor_tsr ReCompressor, Pre_Compressor1;

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

        public Double LF_Main_Solar_Impinging_flowpath, LF_Main_Solar_Energy_Absorbed_flowpath, LF_Main_Energy_Loss_flowpath, LF_Main_Net_Absorbed_flowpath;
        public Double LF_Main_Net_Absorbed_SF, LF_Main_Collector_Efficiency, LF_Main_SF_Pressure_drop, LF_Main_calculated_mass_flux, LF_Main_calculated_Number_Rows;
        public Double LF_Main_calculated_Row_length;

        public Double LF_Main_SF_Pump_Calculated_Power, LF_Main_SF_Pump_isoentropic_eff, LF_Main_SF_Pump_Hydraulic_Power, LF_Main_SF_Pump_Mechanical_eff;
        public Double LF_Main_SF_Pump_Shaft_Work, LF_Main_SF_Pump_Motor_eff, LF_Main_SF_Pump_Motor_Elec_Consump, LF_Main_SF_Pump_Motor_NamePlate_Design, LF_Main_SF_Pump_Motor_NamePlate;

        //ReHeating Solar Field
        public Double PTC_ReHeating_SF_Effective_Apperture_Area;
        public Double LF_ReHeating_SF_Effective_Apperture_Area;

        public Double PTC_ReHeating_Solar_Impinging_flowpath, PTC_ReHeating_Solar_Energy_Absorbed_flowpath, PTC_ReHeating_Energy_Loss_flowpath, PTC_ReHeating_Net_Absorbed_flowpath;
        public Double PTC_ReHeating_Net_Absorbed_SF, PTC_ReHeating_Collector_Efficiency, PTC_ReHeating_SF_Pressure_drop, PTC_ReHeating_calculated_mass_flux, PTC_ReHeating_calculated_Number_Rows;
        public Double PTC_ReHeating_calculated_Row_length;

        public Double PTC_ReHeating_SF_Pump_Calculated_Power, PTC_ReHeating_SF_Pump_isoentropic_eff, PTC_ReHeating_SF_Pump_Hydraulic_Power, PTC_ReHeating_SF_Pump_Mechanical_eff;
        public Double PTC_ReHeating_SF_Pump_Shaft_Work, PTC_ReHeating_SF_Pump_Motor_eff, PTC_ReHeating_SF_Pump_Motor_Elec_Consump, PTC_ReHeating_SF_Pump_Motor_NamePlate_Design, PTC_ReHeating_SF_Pump_Motor_NamePlate;
        public Double Dual_Loop_PTC_Main_SF_Pump_Motor_Elec_Consump_1, Dual_Loop_PTC_Main_SF_Pump_Motor_Elec_Consump_2;

        public Double LF_ReHeating_Solar_Impinging_flowpath, LF_ReHeating_Solar_Energy_Absorbed_flowpath, LF_ReHeating_Energy_Loss_flowpath, LF_ReHeating_Net_Absorbed_flowpath;
        public Double LF_ReHeating_Net_Absorbed_SF, LF_ReHeating_Collector_Efficiency, LF_ReHeating_SF_Pressure_drop, LF_ReHeating_calculated_mass_flux, LF_ReHeating_calculated_Number_Rows;
        public Double LF_ReHeating_calculated_Row_length;

        public Double LF_ReHeating_SF_Pump_Calculated_Power, LF_ReHeating_SF_Pump_isoentropic_eff, LF_ReHeating_SF_Pump_Hydraulic_Power, LF_ReHeating_SF_Pump_Mechanical_eff;
        public Double LF_ReHeating_SF_Pump_Shaft_Work, LF_ReHeating_SF_Pump_Motor_eff, LF_ReHeating_SF_Pump_Motor_Elec_Consump, LF_ReHeating_SF_Pump_Motor_NamePlate_Design, LF_ReHeating_SF_Pump_Motor_NamePlate;
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

        //Main Compressor2 results
        public Double Main_Compressor_Pin, Main_Compressor_Tin, Main_Compressor_Pout, Main_Compressor_Tout;
        public Double Main_Compressor_Flow, Main_Compressor_Diameter, Main_Compressor_Rotation_Velocity;
        public Double Main_Compressor_Efficiency, Main_Compressor_Phi; //Main_Compressor_Phi is the Compressor Flow Factor
        public Boolean Main_Compressor_Surge;

        //Main Compressor1 results
        public Double Pre_Compressor1_Pin, Pre_Compressor1_Tin, Pre_Compressor1_Pout, Pre_Compressor1_Tout;
        public Double Pre_Compressor1_Flow, Pre_Compressor1_Diameter1, Pre_Compressor1_Diameter2, Pre_Compressor1_Rotation_Velocity;
        public Double Pre_Compressor1_Efficiency, Pre_Compressor1_Phi; //Pre_Compressor_Phi is the Compressor Flow Factor
        public Boolean Pre_Compressor1_Surge;

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

        public PCRC_without_ReHeating_new_proposed_configuration()
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
        public Double tol2;
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

        public Double PHX1, RHX1, PC1, COOLER1;

        //Mixtures Calculation button
        private void button20_Click(object sender, EventArgs e)
        {
            int maxIterations = 5;
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
                luis.core1(this.comboBox2.Text + "=" + textBox68.Text + "," + this.comboBox6.Text + "=" + textBox69.Text + "," + this.comboBox12.Text + "=" + textBox33.Text + "," + this.comboBox7.Text + "=" + textBox34.Text, category);
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

            //Store Input Data from Graphical User Interface GUI into variables
            Double m_w_dot_net2 = Convert.ToDouble(textBox1.Text);
            Double t_mc_in2 = Convert.ToDouble(textBox2.Text);            
            Double p_mc_in2 = Convert.ToDouble(textBox3.Text);
            Double p_mc_out2 = Convert.ToDouble(textBox28.Text);
            Double p_pc_in2 = Convert.ToDouble(textBox23.Text);
            Double p_pc_out2 = Convert.ToDouble(textBox8.Text);
            Double t_pc_in2 = Convert.ToDouble(textBox22.Text);
            Double p_rhx_in2 = p_pc_in2;
            Double t_rht_in2 = Convert.ToDouble(textBox4.Text);
            Double t_t_in2 = t_rht_in2;
            Double dp2_lt1 = Convert.ToDouble(textBox5.Text);
            Double dp2_ht1 = Convert.ToDouble(textBox12.Text);
            Double dp2_pc1 = Convert.ToDouble(textBox11.Text);
            Double dp2_phx1 = 0.0;
            Double dp2_rhx1 = Convert.ToDouble(textBox10.Text);
            Double dp2_lt2 = Convert.ToDouble(textBox26.Text);
            Double dp2_ht2 = Convert.ToDouble(textBox25.Text);
            Double dp2_cooler2 = Convert.ToDouble(textBox27.Text);
            Double ua_lt2 = Convert.ToDouble(textBox17.Text);
            Double ua_ht2 = Convert.ToDouble(textBox16.Text);
            Double m_recomp_frac2 = Convert.ToDouble(textBox15.Text);
            Double m_eta_mc2 = Convert.ToDouble(textBox14.Text);
            Double m_eta_rc2 = Convert.ToDouble(textBox13.Text);
            Double m_eta_pc2 = Convert.ToDouble(textBox24.Text);
            Double m_eta_t2 = Convert.ToDouble(textBox19.Text);
            Double m_eta_trh2 = m_eta_t2;
            long n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            Double tol2 = Convert.ToDouble(textBox21.Text);

            luis.wmm = luis.working_fluid.MolecularWeight;

            core.PCRCwithReheating cicloPCRC_withRH = new core.PCRCwithReheating();

            //PRIMERA LLAMADA
            luis.RecompCycle_PCRC_without_Reheating_newproposed(luis, ref cicloPCRC_withRH, m_w_dot_net2,
            t_mc_in2, t_t_in2, t_rht_in2, p_rhx_in2, p_mc_in2, p_mc_out2, p_pc_in2, t_pc_in2, p_pc_out2,
            ua_lt2, ua_ht2, m_eta_mc2, m_eta_rc2, m_eta_pc2, m_eta_t2, m_eta_trh2, n_sub_hxrs2,
            m_recomp_frac2, tol2, eta_thermal2, -dp2_lt1, -dp2_lt2, -dp2_ht1, -dp2_ht2,
            -dp2_pc1, -dp2_pc2, -dp2_phx1, -dp2_phx2, -dp2_rhx1, -dp2_rhx2, -dp2_cooler1, -dp2_cooler2);

            massflow2 = cicloPCRC_withRH.m_dot_turbine;
            w_dot_net2 = cicloPCRC_withRH.W_dot_net;
            eta_thermal2 = cicloPCRC_withRH.eta_thermal;

            eta_mc2 = m_eta_mc2;
            eta_rc2 = m_eta_rc2;
            eta_pc2 = m_eta_pc2;
            eta_t2 = m_eta_t2;
            eta_trh2 = m_eta_trh2;

            temp21 = cicloPCRC_withRH.temp[0];
            temp22 = cicloPCRC_withRH.temp[1];
            temp23 = cicloPCRC_withRH.temp[2];
            temp24 = cicloPCRC_withRH.temp[3];
            temp25 = cicloPCRC_withRH.temp[4];
            temp26 = cicloPCRC_withRH.temp[5];
            temp27 = cicloPCRC_withRH.temp[6];
            temp28 = cicloPCRC_withRH.temp[7];
            temp29 = cicloPCRC_withRH.temp[8];
            temp210 = cicloPCRC_withRH.temp[9];
            temp211 = cicloPCRC_withRH.temp[10];
            temp212 = cicloPCRC_withRH.temp[11];
            temp213 = cicloPCRC_withRH.temp[12];
            temp214 = cicloPCRC_withRH.temp[13];

            pres21 = cicloPCRC_withRH.pres[0];
            pres22 = cicloPCRC_withRH.pres[1];
            pres23 = cicloPCRC_withRH.pres[2];
            pres24 = cicloPCRC_withRH.pres[3];
            pres25 = cicloPCRC_withRH.pres[4];
            pres26 = cicloPCRC_withRH.pres[5];
            pres27 = cicloPCRC_withRH.pres[6];
            pres28 = cicloPCRC_withRH.pres[7];
            pres29 = cicloPCRC_withRH.pres[8];
            pres210 = cicloPCRC_withRH.pres[9];
            pres211 = cicloPCRC_withRH.pres[10];
            pres212 = cicloPCRC_withRH.pres[11];
            pres213 = cicloPCRC_withRH.pres[12];
            pres214 = cicloPCRC_withRH.pres[13];

            //SEGUNDA LLAMADA
            luis.RecompCycle_PCRC_without_Reheating_newproposed(luis, ref cicloPCRC_withRH, m_w_dot_net2,
            t_mc_in2, temp25, t_rht_in2, p_rhx_in2, p_mc_in2, p_mc_out2, p_pc_in2, t_pc_in2, p_pc_out2,
            ua_lt2, ua_ht2, m_eta_mc2, m_eta_rc2, m_eta_pc2, m_eta_t2, m_eta_trh2, n_sub_hxrs2,
            m_recomp_frac2, tol2, eta_thermal2, -dp2_lt1, -dp2_lt2, -dp2_ht1, -dp2_ht2,
            -dp2_pc1, -dp2_pc2, -dp2_phx1, -dp2_phx2, -dp2_rhx1, -dp2_rhx2, -dp2_cooler1, -dp2_cooler2);

            massflow2 = cicloPCRC_withRH.m_dot_turbine;
            w_dot_net2 = cicloPCRC_withRH.W_dot_net;
            eta_thermal2 = cicloPCRC_withRH.eta_thermal;

            eta_mc2 = m_eta_mc2;
            eta_rc2 = m_eta_rc2;
            eta_pc2 = m_eta_pc2;
            eta_t2 = m_eta_t2;
            eta_trh2 = m_eta_trh2;

            temp21 = cicloPCRC_withRH.temp[0];
            temp22 = cicloPCRC_withRH.temp[1];
            temp23 = cicloPCRC_withRH.temp[2];
            temp24 = cicloPCRC_withRH.temp[3];
            temp25 = cicloPCRC_withRH.temp[4];
            temp26 = cicloPCRC_withRH.temp[5];
            temp27 = cicloPCRC_withRH.temp[6];
            temp28 = cicloPCRC_withRH.temp[7];
            temp29 = cicloPCRC_withRH.temp[8];
            temp210 = cicloPCRC_withRH.temp[9];
            temp211 = cicloPCRC_withRH.temp[10];
            temp212 = cicloPCRC_withRH.temp[11];
            temp213 = cicloPCRC_withRH.temp[12];
            temp214 = cicloPCRC_withRH.temp[13];

            pres21 = cicloPCRC_withRH.pres[0];
            pres22 = cicloPCRC_withRH.pres[1];
            pres23 = cicloPCRC_withRH.pres[2];
            pres24 = cicloPCRC_withRH.pres[3];
            pres25 = cicloPCRC_withRH.pres[4];
            pres26 = cicloPCRC_withRH.pres[5];
            pres27 = cicloPCRC_withRH.pres[6];
            pres28 = cicloPCRC_withRH.pres[7];
            pres29 = cicloPCRC_withRH.pres[8];
            pres210 = cicloPCRC_withRH.pres[9];
            pres211 = cicloPCRC_withRH.pres[10];
            pres212 = cicloPCRC_withRH.pres[11];
            pres213 = cicloPCRC_withRH.pres[12];
            pres214 = cicloPCRC_withRH.pres[13];

            //TERCERA LLAMADA
            luis.RecompCycle_PCRC_without_Reheating_newproposed(luis, ref cicloPCRC_withRH, m_w_dot_net2,
            t_mc_in2, temp25, t_rht_in2, p_rhx_in2, p_mc_in2, p_mc_out2, p_pc_in2, t_pc_in2, p_pc_out2,
            ua_lt2, ua_ht2, m_eta_mc2, m_eta_rc2, m_eta_pc2, m_eta_t2, m_eta_trh2, n_sub_hxrs2,
            m_recomp_frac2, tol2, eta_thermal2, -dp2_lt1, -dp2_lt2, -dp2_ht1, -dp2_ht2,
            -dp2_pc1, -dp2_pc2, -dp2_phx1, -dp2_phx2, -dp2_rhx1, -dp2_rhx2, -dp2_cooler1, -dp2_cooler2);

            massflow2 = cicloPCRC_withRH.m_dot_turbine;
            w_dot_net2 = cicloPCRC_withRH.W_dot_net;
            eta_thermal2 = cicloPCRC_withRH.eta_thermal;

            eta_mc2 = m_eta_mc2;
            eta_rc2 = m_eta_rc2;
            eta_pc2 = m_eta_pc2;
            eta_t2 = m_eta_t2;
            eta_trh2 = m_eta_trh2;

            temp21 = cicloPCRC_withRH.temp[0];
            temp22 = cicloPCRC_withRH.temp[1];
            temp23 = cicloPCRC_withRH.temp[2];
            temp24 = cicloPCRC_withRH.temp[3];
            temp25 = cicloPCRC_withRH.temp[4];
            temp26 = cicloPCRC_withRH.temp[5];
            temp27 = cicloPCRC_withRH.temp[6];
            temp28 = cicloPCRC_withRH.temp[7];
            temp29 = cicloPCRC_withRH.temp[8];
            temp210 = cicloPCRC_withRH.temp[9];
            temp211 = cicloPCRC_withRH.temp[10];
            temp212 = cicloPCRC_withRH.temp[11];
            temp213 = cicloPCRC_withRH.temp[12];
            temp214 = cicloPCRC_withRH.temp[13];

            pres21 = cicloPCRC_withRH.pres[0];
            pres22 = cicloPCRC_withRH.pres[1];
            pres23 = cicloPCRC_withRH.pres[2];
            pres24 = cicloPCRC_withRH.pres[3];
            pres25 = cicloPCRC_withRH.pres[4];
            pres26 = cicloPCRC_withRH.pres[5];
            pres27 = cicloPCRC_withRH.pres[6];
            pres28 = cicloPCRC_withRH.pres[7];
            pres29 = cicloPCRC_withRH.pres[8];
            pres210 = cicloPCRC_withRH.pres[9];
            pres211 = cicloPCRC_withRH.pres[10];
            pres212 = cicloPCRC_withRH.pres[11];
            pres213 = cicloPCRC_withRH.pres[12];
            pres214 = cicloPCRC_withRH.pres[13];

            //CUARTA LLAMADA
            luis.RecompCycle_PCRC_without_Reheating_newproposed(luis, ref cicloPCRC_withRH, m_w_dot_net2,
            t_mc_in2, temp25, t_rht_in2, p_rhx_in2, p_mc_in2, p_mc_out2, p_pc_in2, t_pc_in2, p_pc_out2,
            ua_lt2, ua_ht2, m_eta_mc2, m_eta_rc2, m_eta_pc2, m_eta_t2, m_eta_trh2, n_sub_hxrs2,
            m_recomp_frac2, tol2, eta_thermal2, -dp2_lt1, -dp2_lt2, -dp2_ht1, -dp2_ht2,
            -dp2_pc1, -dp2_pc2, -dp2_phx1, -dp2_phx2, -dp2_rhx1, -dp2_rhx2, -dp2_cooler1, -dp2_cooler2);

            massflow2 = cicloPCRC_withRH.m_dot_turbine;
            w_dot_net2 = cicloPCRC_withRH.W_dot_net;
            eta_thermal2 = cicloPCRC_withRH.eta_thermal;

            eta_mc2 = m_eta_mc2;
            eta_rc2 = m_eta_rc2;
            eta_pc2 = m_eta_pc2;
            eta_t2 = m_eta_t2;
            eta_trh2 = m_eta_trh2;

            temp21 = cicloPCRC_withRH.temp[0];
            temp22 = cicloPCRC_withRH.temp[1];
            temp23 = cicloPCRC_withRH.temp[2];
            temp24 = cicloPCRC_withRH.temp[3];
            temp25 = cicloPCRC_withRH.temp[4];
            temp26 = cicloPCRC_withRH.temp[5];
            temp27 = cicloPCRC_withRH.temp[6];
            temp28 = cicloPCRC_withRH.temp[7];
            temp29 = cicloPCRC_withRH.temp[8];
            temp210 = cicloPCRC_withRH.temp[9];
            temp211 = cicloPCRC_withRH.temp[10];
            temp212 = cicloPCRC_withRH.temp[11];
            temp213 = cicloPCRC_withRH.temp[12];
            temp214 = cicloPCRC_withRH.temp[13];

            pres21 = cicloPCRC_withRH.pres[0];
            pres22 = cicloPCRC_withRH.pres[1];
            pres23 = cicloPCRC_withRH.pres[2];
            pres24 = cicloPCRC_withRH.pres[3];
            pres25 = cicloPCRC_withRH.pres[4];
            pres26 = cicloPCRC_withRH.pres[5];
            pres27 = cicloPCRC_withRH.pres[6];
            pres28 = cicloPCRC_withRH.pres[7];
            pres29 = cicloPCRC_withRH.pres[8];
            pres210 = cicloPCRC_withRH.pres[9];
            pres211 = cicloPCRC_withRH.pres[10];
            pres212 = cicloPCRC_withRH.pres[11];
            pres213 = cicloPCRC_withRH.pres[12];
            pres214 = cicloPCRC_withRH.pres[13];

            //Pressures
            textBox47.Text = Convert.ToString(temp21);
            textBox46.Text = Convert.ToString(temp22);
            textBox45.Text = Convert.ToString(temp23);
            textBox44.Text = Convert.ToString(temp24);
            textBox43.Text = Convert.ToString(temp25);
            textBox42.Text = Convert.ToString(temp26);
            textBox35.Text = Convert.ToString(temp27);
            textBox18.Text = Convert.ToString(temp28);
            textBox7.Text = Convert.ToString(temp29);
            textBox6.Text = Convert.ToString(temp210);
            textBox57.Text = Convert.ToString(temp213);
            textBox56.Text = Convert.ToString(temp214);

            //Temperatures
            textBox54.Text = Convert.ToString(pres21);
            textBox53.Text = Convert.ToString(pres22);
            textBox52.Text = Convert.ToString(pres23);
            textBox51.Text = Convert.ToString(pres24);
            textBox37.Text = Convert.ToString(pres25);
            textBox36.Text = Convert.ToString(pres26);
            textBox41.Text = Convert.ToString(pres27);
            textBox40.Text = Convert.ToString(pres28);
            textBox39.Text = Convert.ToString(pres29);
            textBox38.Text = Convert.ToString(pres210);
            textBox59.Text = Convert.ToString(pres213);
            textBox58.Text = Convert.ToString(pres214);

            String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
            String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state, point13_state, point14_state;

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

            //TOOLTIPS
            toolTip1.SetToolTip(label77, point1_state);
            toolTip2.SetToolTip(label75, point2_state);
            toolTip3.SetToolTip(label74, point3_state);
            toolTip4.SetToolTip(label71, point4_state);
            toolTip5.SetToolTip(label80, point5_state);
            toolTip6.SetToolTip(label69, point6_state);
            toolTip7.SetToolTip(label63, point7_state);
            toolTip8.SetToolTip(label65, point8_state);
            toolTip9.SetToolTip(label66, point9_state);
            toolTip10.SetToolTip(label67, point10_state);
            toolTip11.SetToolTip(label81, point13_state);
            toolTip12.SetToolTip(label82, point14_state);

            //High Temperature checkBox for not calculating the Solar Field
            if (checkBox1.Checked == false)
            {
                PHX1 = cicloPCRC_withRH.PHX.Q_dot;
                RHX1 = cicloPCRC_withRH.RHX.Q_dot;
                recomp_frac2 = cicloPCRC_withRH.recomp_frac;
                LT_Q = cicloPCRC_withRH.LT.Q_dot;
                LT_mdotc = cicloPCRC_withRH.LT.m_dot_design[0];
                LT_mdoth = cicloPCRC_withRH.LT.m_dot_design[1];
                LT_Tcin = cicloPCRC_withRH.LT.T_c_in;
                LT_Thin = cicloPCRC_withRH.LT.T_h_in;
                LT_Pcin = cicloPCRC_withRH.LT.P_c_in;
                LT_Phin = cicloPCRC_withRH.LT.P_h_in;
                LT_Pcout = cicloPCRC_withRH.LT.P_c_out;
                LT_Phout = cicloPCRC_withRH.LT.P_h_out;
                LT_Effc = cicloPCRC_withRH.LT.eff;

                HT_Q = cicloPCRC_withRH.HT.Q_dot;
                HT_mdotc = cicloPCRC_withRH.HT.m_dot_design[0];
                HT_mdoth = cicloPCRC_withRH.HT.m_dot_design[1];
                HT_Tcin = cicloPCRC_withRH.HT.T_c_in;
                HT_Thin = cicloPCRC_withRH.HT.T_h_in;
                HT_Pcin = cicloPCRC_withRH.HT.P_c_in;
                HT_Phin = cicloPCRC_withRH.HT.P_h_in;
                HT_Pcout = cicloPCRC_withRH.HT.P_c_out;
                HT_Phout = cicloPCRC_withRH.HT.P_h_out;
                HT_Effc = cicloPCRC_withRH.HT.eff;

                PC1 = -cicloPCRC_withRH.PC.Q_dot;
                COOLER1 = -cicloPCRC_withRH.COOLER.Q_dot;

                if (comboBox5.Text == "Dual-Loop")
                {
                    //ReHeating SF
                    comboBox8.Enabled = true;
                    comboBox9.Enabled = true;
                    comboBox10.Enabled = true;
                    comboBox11.Enabled = true;
                    button17.Enabled = true;
                    button13.Enabled = true;

                    button5.Enabled = false;
                }

                else if ((comboBox5.Text == "Parabolic") || (comboBox5.Text == "Fresnel"))
                {
                    //ReHeaing SF
                    comboBox8.Enabled = false;
                    comboBox9.Enabled = false;
                    comboBox10.Enabled = false;
                    comboBox11.Enabled = false;
                    button17.Enabled = false;
                    button13.Enabled = false;

                    button5.Enabled = true;
                }

                button6.Enabled = true;
                button7.Enabled = true;
                button4.Enabled = true;
                button16.Enabled = true;
                button9.Enabled = true;
                button11.Enabled = true;
                button12.Enabled = true;
                button15.Enabled = true;
            }
        }

        //Fluid Type selection ComboBox
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                comboBox6.Enabled = false;
                comboBox12.Enabled = false;
                comboBox7.Enabled = false;
                textBox68.Enabled = false;
                textBox69.Enabled = false;
                textBox33.Enabled = false;
                textBox34.Enabled = false;
                button20.Enabled = false;
                button2.Enabled = true;
            }

            else if (comboBox1.Text == "NewMixture")
            {
                comboBox6.Enabled = true;
                comboBox12.Enabled = true;
                comboBox7.Enabled = true;
                textBox68.Enabled = true;
                textBox69.Enabled = true;
                textBox33.Enabled = true;
                textBox34.Enabled = true;
                button20.Enabled = true;
                button2.Enabled = false;

                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox68.Text + "," + this.comboBox6.Text + "=" + textBox69.Text + "," + this.comboBox12.Text + "=" + textBox33.Text + "," + this.comboBox7.Text + "=" + textBox34.Text, ReferenceState.DEF);

                textBox32.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox31.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox30.Text = Convert.ToString(working_fluid.CriticalDensity);
            }
        }

        //Set Critical Conditions button
        private void button25_Click(object sender, EventArgs e)
        {
            double option1 = 0.0;
            double option2 = 0.0;
            double option3 = 0.0;
            double option4 = 0.0;

            option1 = Convert.ToDouble(this.textBox68.Text);
            option2 = Convert.ToDouble(this.textBox69.Text);
            option3 = Convert.ToDouble(this.textBox33.Text);
            option4 = Convert.ToDouble(this.textBox34.Text);

            if ((option1 == 1) || (option2 == 1) || (option3 == 1) || (option4 == 1))
            {
                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox68.Text + "," + this.comboBox6.Text + "=" + textBox69.Text + "," + this.comboBox12.Text + "=" + textBox33.Text + "," + this.comboBox7.Text + "=" + textBox34.Text, ReferenceState.DEF);
                textBox23.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox22.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox2.Text = Convert.ToString(working_fluid.CriticalTemperature);

                textBox32.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox31.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox30.Text = Convert.ToString(working_fluid.CriticalDensity);

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
                xlWorkSheet.Cells[15, 6] = this.comboBox12.Text;
                xlWorkSheet.Cells[16, 6] = this.comboBox7.Text;

                // % Compositions
                xlWorkSheet.Cells[13, 7] = this.textBox68.Text;
                xlWorkSheet.Cells[14, 7] = this.textBox69.Text;
                xlWorkSheet.Cells[15, 7] = this.textBox33.Text;
                xlWorkSheet.Cells[16, 7] = this.textBox34.Text;

                //MessageBox.Show(xlWorkSheet.get_Range("D68", "D68").Value2.ToString());
                this.textBox23.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox2.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox22.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();

                this.textBox32.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox31.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox30.Text = xlWorkSheet.get_Range("D70", "D70").Value2.ToString();

                MixtureCriticalPressure = xlWorkSheet.get_Range("D69", "D69").Value2;
                MixtureCriticalTemperature = xlWorkSheet.get_Range("D68", "D68").Value2;

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

        //Reset button
        private void button14_Click(object sender, EventArgs e)
        {
            //Clear Pressures values
            textBox47.Text = "";
            textBox46.Text = "";
            textBox45.Text = "";
            textBox44.Text = "";
            textBox43.Text = "";
            textBox42.Text = "";
            textBox35.Text = "";
            textBox18.Text = "";
            textBox7.Text = "";
            textBox6.Text = "";
            textBox57.Text = "";
            textBox56.Text = "";
           
            //Clear Temperature values
            textBox54.Text = "";
            textBox53.Text = "";
            textBox52.Text = "";
            textBox51.Text = "";
            textBox37.Text = "";
            textBox36.Text = "";
            textBox41.Text = "";
            textBox40.Text = "";
            textBox39.Text = "";
            textBox38.Text = "";
            textBox59.Text = "";
            textBox58.Text = "";
           
            //Clear Power, mass flow and efficiency
            textBox48.Text = "";
            textBox49.Text = "";
            textBox50.Text = "";

            //w_dot_net2 = Convert.ToDouble(textBox1.Text);
            textBox1.Text = "50000";
            //t_mc_in2 = Convert.ToDouble(textBox2.Text);
            textBox2.Text = "305.15";
            //t_t_in2 = Convert.ToDouble(textBox4.Text);
            comboBox4_SelectedValueChanged(this, e);
            //p_mc_in2 = Convert.ToDouble(textBox3.Text);
            textBox3.Text = "10300";
            //p_mc_out2 = Convert.ToDouble(textBox28.Text);
            textBox28.Text = "25000";
            //p_pc_in2 = Convert.ToDouble(textBox23.Text);
            textBox23.Text = "7400";
            //p_pc_out2 = Convert.ToDouble(textBox8.Text);
            textBox8.Text = "10300";
            //t_pc_in2 = Convert.ToDouble(textBox22.Text);
            textBox22.Text = "305.15";
            //p_rhx_in2 = Convert.ToDouble(textBox7.Text);
            //textBox7.Text = "17300";
            //t_rht_in2 = Convert.ToDouble(textBox6.Text);
            //comboBox7_SelectedValueChanged(this, e);
            //dp2_lt1 = Convert.ToDouble(textBox5.Text);
            textBox5.Text = "0.0";
            //dp2_ht1 = Convert.ToDouble(textBox12.Text);
            textBox12.Text = "0.0";
            //dp2_pc2 = Convert.ToDouble(textBox11.Text);
            textBox11.Text = "0.0";
            //dp2_phx1 = Convert.ToDouble(textBox10.Text);
            textBox10.Text = "0.0";
            //dp2_rhx1 = Convert.ToDouble(textBox9.Text);
            //textBox9.Text = "0.0";
            //dp2_lt2 = Convert.ToDouble(textBox26.Text);
            textBox26.Text = "0.0";
            //dp2_ht2 = Convert.ToDouble(textBox25.Text);
            textBox25.Text = "0.0";
            //dp2_cooler2 = Convert.ToDouble(textBox27.Text);
            textBox27.Text = "0.0";
            //ua_lt2 = Convert.ToDouble(textBox17.Text);
            textBox17.Text = "5000";
            //ua_ht2 = Convert.ToDouble(textBox16.Text);
            textBox16.Text = "5000";
            //recomp_frac2 = Convert.ToDouble(textBox15.Text);
            textBox15.Text = "0.1";
            //eta_mc2 = Convert.ToDouble(textBox14.Text);
            textBox14.Text = "0.89";
            //eta_rc2 = Convert.ToDouble(textBox13.Text);
            textBox13.Text = "0.89";
            //eta_pc2 = Convert.ToDouble(textBox24.Text);
            textBox24.Text = "0.89";
            //eta_t2 = Convert.ToDouble(textBox19.Text);
            textBox19.Text = "0.93";
            //eta_trh2 = Convert.ToDouble(textBox18.Text);
            //textBox18.Text = "0.93";
            //n_sub_hxrs2 = Convert.ToInt64(textBox20.Text);
            textBox20.Text = "15";
            //tol2 = Convert.ToDouble(textBox21.Text);
            textBox21.Text = "0.00001";
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Solar Salt")
            {
                textBox4.Text = "823.15";
            }

            else if (comboBox4.Text == "Hitec XL")
            {
                textBox4.Text = "793.15";
            }

            else if (comboBox4.Text == "Therminol VP1")
            {
                textBox4.Text = "663.15";
            }

            else if (comboBox4.Text == "Syltherm_800")
            {
                textBox4.Text = "663.15";
            }

            else if (comboBox4.Text == "Dowtherm_A")
            {
                textBox4.Text = "683.15";
            }

            else if (comboBox4.Text == "Therminol_75")
            {
                textBox4.Text = "648.15";
            }
        }
    }
}
