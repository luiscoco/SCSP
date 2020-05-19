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
    public partial class PCRCMCI : Form
    {
        //Input Data:
        public RefrigerantCategory category;
        public ReferenceState referencestate;
        public Int64 Error_code;

        public Double wmm;

        public double MixtureCriticalPressure = 0.0;
        public double MixtureCriticalTemperature = 0.0;

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

        //Store Input Data from Graphical User Interface GUI into variables
        public Double w_dot_net;

        public Double t_mc1_in;
        public Double t_mc2_in;

        public Double t_t_in;

        public Double p_mc1_in;
        public Double p_mc1_out;

        public Double p_mc2_in;
        public Double p_mc2_out;

        public Double p_pc_in;
        public Double p_pc_out;
        public Double t_pc_in;

        public Double p_rhx_in;
        public Double t_rht_in;

        public Double dp_lt1;
        public Double dp_ht1;

        public Double dp_pc1;
        public Double dp_pc2;
        public Double dp_pc3;

        public Double dp_phx;
        public Double dp_rhx;

        public Double dp_lt2;
        public Double dp_ht2;

        public Double ua_lt;
        public Double ua_ht;

        public Double m_recomp_frac;

        public Double m_eta_pc;
        public Double m_eta_mc1;
        public Double m_eta_mc2;
        public Double m_eta_rc;
        public Double m_eta_t;
        public Double m_eta_trh;

        public long n_sub_hxrs;

        public Double tol;

        public Double eta_thermal;

        public Double PHX1;
        public Double RHX1;
        public Double recomp_frac2;
        public Double LT_Q;
        public Double LT_mdotc;
        public Double LT_mdoth;
        public Double LT_Tcin;
        public Double LT_Thin;
        public Double LT_Pcin;
        public Double LT_Phin;
        public Double LT_Pcout;
        public Double LT_Phout;
        public Double LT_Effc;

        public Double HT_Q;
        public Double HT_mdotc;
        public Double HT_mdoth;
        public Double HT_Tcin;
        public Double HT_Thin;
        public Double HT_Pcin;
        public Double HT_Phin;
        public Double HT_Pcout;
        public Double HT_Phout;
        public Double HT_Effc;

        public Double PC1;
        public Double PC2;
        public Double PC3;

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;
        public Radial_Turbine ReHeating_Turbine;

        public PreeCooler Precooler_dialog1;
        public PreeCooler Precooler_dialog2;
        public PreeCooler Precooler_dialog3;

        public Double massflow2;
        public Double w_dot_net2;
        public Double eta_thermal2;

        public Double eta_mc1;
        public Double eta_rc2;
        public Double eta_pc2;
        public Double eta_t2;
        public Double eta_trh2;

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

        public snl_compressor_tsr Main_Compressor1;
        public snl_compressor_tsr Main_Compressor2;

        public snl_compressor_tsr PreCompressor;

        public Double N_design_Main_Compressor;

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

        //Generator Information
        public Double Generator_Name_Plate_Power, Generator_Power_Output, Generator_Total_Loss;
        public Double Generator_Shaft_Power, Gear_Efficiency, Rating_Point_Efficiency;
        public Double Generator_Mechanical_Loss, Generator_Electrical_Loss, Rating_Design_Point_Load;

        //SF and ACHE Pumps electrical consumption
        public Double Main_SF_Pump_Electrical_Consumption, ReHeating_SF_Pump_Electrical_Consumption;
        public Double UHS_Water_Pump, ACHE_fans;

        public PCRCMCI()
        {
            InitializeComponent();
        }

        //Brayton cycle calculation
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
                luis.core1(this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, category);
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
            w_dot_net = Convert.ToDouble(textBox48.Text);

            t_mc1_in = Convert.ToDouble(textBox2.Text);
            t_mc2_in = Convert.ToDouble(textBox28.Text);

            t_t_in = Convert.ToDouble(textBox4.Text);

            p_mc1_in = Convert.ToDouble(textBox3.Text);
            p_mc1_out = Convert.ToDouble(textBox8.Text);

            p_mc2_in = Convert.ToDouble(textBox23.Text);
            p_mc2_out = Convert.ToDouble(textBox22.Text);

            p_pc_in = Convert.ToDouble(textBox103.Text);
            p_pc_out = Convert.ToDouble(textBox104.Text);
            t_pc_in = Convert.ToDouble(textBox102.Text);

            p_rhx_in = Convert.ToDouble(textBox7.Text);
            t_rht_in = Convert.ToDouble(textBox6.Text);

            dp_lt1 = Convert.ToDouble(textBox5.Text);
            dp_ht1 = Convert.ToDouble(textBox12.Text);

            dp_pc1 = Convert.ToDouble(textBox11.Text);
            dp_pc2 = Convert.ToDouble(textBox24.Text);
            dp_pc3 = Convert.ToDouble(textBox107.Text);

            dp_phx = Convert.ToDouble(textBox10.Text);
            dp_rhx = Convert.ToDouble(textBox9.Text);

            dp_lt2 = Convert.ToDouble(textBox26.Text);
            dp_ht2 = Convert.ToDouble(textBox25.Text);

            ua_lt = Convert.ToDouble(textBox17.Text);
            ua_ht = Convert.ToDouble(textBox16.Text);

            m_recomp_frac = Convert.ToDouble(textBox15.Text);

            m_eta_pc = Convert.ToDouble(textBox106.Text);
            m_eta_mc1 = Convert.ToDouble(textBox14.Text);
            m_eta_mc2 = Convert.ToDouble(textBox27.Text);
            m_eta_rc = Convert.ToDouble(textBox13.Text);
            m_eta_t = Convert.ToDouble(textBox19.Text);
            m_eta_trh = Convert.ToDouble(textBox18.Text);

            n_sub_hxrs = Convert.ToInt64(textBox20.Text);

            tol = Convert.ToDouble(textBox21.Text);

            eta_thermal = 0.0;

            luis.wmm = luis.working_fluid.MolecularWeight;

            core.PCRCMCIwithReheating cicloPCRCMCI_withRH = new core.PCRCMCIwithReheating();

            luis.RecompCycle_PCRCMCI_with_Reheating(luis, ref cicloPCRCMCI_withRH, w_dot_net,
            t_pc_in, t_mc1_in, t_mc2_in, t_t_in, t_rht_in, p_rhx_in,
            p_pc_in, p_pc_out, p_mc2_in, p_mc2_out, p_mc1_in, p_mc1_out, ua_lt, ua_ht,
            m_eta_mc2, m_eta_pc, m_eta_rc, m_eta_mc1, m_eta_t, m_eta_trh, n_sub_hxrs, m_recomp_frac,
            tol, eta_thermal, -dp_lt1, -dp_lt2, -dp_ht1, -dp_ht2, -dp_pc1, -dp_pc2, -dp_pc3, -dp_phx, -dp_rhx);

            massflow2 = cicloPCRCMCI_withRH.m_dot_turbine;
            w_dot_net2 = cicloPCRCMCI_withRH.W_dot_net;
            eta_thermal2 = cicloPCRCMCI_withRH.eta_thermal;

            eta_mc1 = m_eta_mc1;
            eta_rc2 = m_eta_rc;
            eta_pc2 = m_eta_pc;
            eta_t2 = m_eta_t;
            eta_trh2 = m_eta_trh;

            temp21 = cicloPCRCMCI_withRH.temp[0];
            temp22 = cicloPCRCMCI_withRH.temp[1];
            temp23 = cicloPCRCMCI_withRH.temp[2];
            temp24 = cicloPCRCMCI_withRH.temp[3];
            temp25 = cicloPCRCMCI_withRH.temp[4];
            temp26 = cicloPCRCMCI_withRH.temp[5];
            temp27 = cicloPCRCMCI_withRH.temp[6];
            temp28 = cicloPCRCMCI_withRH.temp[7];
            temp29 = cicloPCRCMCI_withRH.temp[8];
            temp210 = cicloPCRCMCI_withRH.temp[9];
            temp211 = cicloPCRCMCI_withRH.temp[10];
            temp212 = cicloPCRCMCI_withRH.temp[11];
            temp213 = cicloPCRCMCI_withRH.temp[12];
            temp214 = cicloPCRCMCI_withRH.temp[13];
            temp215 = cicloPCRCMCI_withRH.temp[14];
            temp216 = cicloPCRCMCI_withRH.temp[15];

            pres21 = cicloPCRCMCI_withRH.pres[0];
            pres22 = cicloPCRCMCI_withRH.pres[1];
            pres23 = cicloPCRCMCI_withRH.pres[2];
            pres24 = cicloPCRCMCI_withRH.pres[3];
            pres25 = cicloPCRCMCI_withRH.pres[4];
            pres26 = cicloPCRCMCI_withRH.pres[5];
            pres27 = cicloPCRCMCI_withRH.pres[6];
            pres28 = cicloPCRCMCI_withRH.pres[7];
            pres29 = cicloPCRCMCI_withRH.pres[8];
            pres210 = cicloPCRCMCI_withRH.pres[9];
            pres211 = cicloPCRCMCI_withRH.pres[10];
            pres212 = cicloPCRCMCI_withRH.pres[11];
            pres213 = cicloPCRCMCI_withRH.pres[12];
            pres214 = cicloPCRCMCI_withRH.pres[13];
            pres215 = cicloPCRCMCI_withRH.pres[14];
            pres216 = cicloPCRCMCI_withRH.pres[15];

            textBox47.Text = Convert.ToString(temp21);
            textBox46.Text = Convert.ToString(temp22);
            textBox45.Text = Convert.ToString(temp23);
            textBox44.Text = Convert.ToString(temp24);
            textBox43.Text = Convert.ToString(temp25);
            textBox42.Text = Convert.ToString(temp26);
            textBox39.Text = Convert.ToString(temp27);
            textBox38.Text = Convert.ToString(temp28);
            textBox37.Text = Convert.ToString(temp29);
            textBox29.Text = Convert.ToString(temp210);
            textBox57.Text = Convert.ToString(temp211);
            textBox56.Text = Convert.ToString(temp212);
            textBox65.Text = Convert.ToString(temp213);
            textBox64.Text = Convert.ToString(temp214);
            textBox98.Text = Convert.ToString(temp215);
            textBox100.Text = Convert.ToString(temp216);

            textBox63.Text = Convert.ToString(pres21);
            textBox62.Text = Convert.ToString(pres22);
            textBox61.Text = Convert.ToString(pres23);
            textBox60.Text = Convert.ToString(pres24);
            textBox55.Text = Convert.ToString(pres25);
            textBox54.Text = Convert.ToString(pres26);
            textBox53.Text = Convert.ToString(pres27);
            textBox52.Text = Convert.ToString(pres28);
            textBox41.Text = Convert.ToString(pres29);
            textBox40.Text = Convert.ToString(pres210);
            textBox59.Text = Convert.ToString(pres211);
            textBox58.Text = Convert.ToString(pres212);
            textBox67.Text = Convert.ToString(pres213);
            textBox66.Text = Convert.ToString(pres214);
            textBox99.Text = Convert.ToString(pres215);
            textBox101.Text = Convert.ToString(pres216);

            String point1_state, point2_state, point3_state, point4_state, point5_state, point6_state;
            String point7_state, point8_state, point9_state, point10_state, point11_state, point12_state,
                point13_state, point14_state, point15_state, point16_state;

            luis.working_fluid.FindStateWithTP(temp21, pres21);
            Double enth21 = luis.working_fluid.Enthalpy;
            Double entr21 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp22, pres22);
            Double enth22 = luis.working_fluid.Enthalpy;
            Double entr22 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp23, pres23);
            Double enth23 = luis.working_fluid.Enthalpy;
            Double entr23 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp24, pres24);
            Double enth24 = luis.working_fluid.Enthalpy;
            Double entr24 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp25, pres25);
            Double enth25 = luis.working_fluid.Enthalpy;
            Double entr25 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp26, pres26);
            Double enth26 = luis.working_fluid.Enthalpy;
            Double entr26 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp27, pres27);
            Double enth27 = luis.working_fluid.Enthalpy;
            Double entr27 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp28, pres28);
            Double enth28 = luis.working_fluid.Enthalpy;
            Double entr28 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp29, pres29);
            Double enth29 = luis.working_fluid.Enthalpy;
            Double entr29 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp210, pres210);
            Double enth210 = luis.working_fluid.Enthalpy;
            Double entr210 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp211, pres211);
            Double enth211 = luis.working_fluid.Enthalpy;
            Double entr211 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp212, pres212);
            Double enth212 = luis.working_fluid.Enthalpy;
            Double entr212 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp213, pres213);
            Double enth213 = luis.working_fluid.Enthalpy;
            Double entr213 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp214, pres214);
            Double enth214 = luis.working_fluid.Enthalpy;
            Double entr214 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp215, pres215);
            Double enth215 = luis.working_fluid.Enthalpy;
            Double entr215 = luis.working_fluid.Entropy;

            luis.working_fluid.FindStateWithTP(temp216, pres216);
            Double enth216 = luis.working_fluid.Enthalpy;
            Double entr216 = luis.working_fluid.Entropy;

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

            point15_state = "Pressure (kPa):" + Convert.ToString(pres215) + Environment.NewLine +
                     "Temperature (K):" + Convert.ToString(temp215) + Environment.NewLine +
                     "Entalphy (kJ/kg):" + Convert.ToString(enth215) + Environment.NewLine +
                     "Entrophy (kJ/kg K):" + Convert.ToString(entr215) + Environment.NewLine;

            point16_state = "Pressure (kPa):" + Convert.ToString(pres216) + Environment.NewLine +
                     "Temperature (K):" + Convert.ToString(temp216) + Environment.NewLine +
                     "Entalphy (kJ/kg):" + Convert.ToString(enth216) + Environment.NewLine +
                     "Entrophy (kJ/kg K):" + Convert.ToString(entr216) + Environment.NewLine;

            textBox48.Text = Convert.ToString(w_dot_net2);
            textBox49.Text = Convert.ToString(massflow2);
            textBox50.Text = Convert.ToString(eta_thermal2 * 100);

            //toolTip1.SetToolTip(label55, point1_state);
            //toolTip2.SetToolTip(label57, point2_state);
            //toolTip3.SetToolTip(label59, point3_state);
            //toolTip4.SetToolTip(label60, point4_state);
            //toolTip5.SetToolTip(label56, point5_state);
            //toolTip6.SetToolTip(label58, point6_state);
            //toolTip7.SetToolTip(label68, point7_state);
            //toolTip8.SetToolTip(label69, point8_state);
            //toolTip9.SetToolTip(label70, point9_state);
            //toolTip10.SetToolTip(label33, point10_state);
            //toolTip11.SetToolTip(label72, point11_state);
            //toolTip12.SetToolTip(label73, point12_state);
            //toolTip11.SetToolTip(label71, point13_state);
            //toolTip12.SetToolTip(label32, point14_state);

            if (checkBox1.Checked == false)
            {
                PHX1 = cicloPCRCMCI_withRH.PHX.Q_dot;
                RHX1 = cicloPCRCMCI_withRH.RHX.Q_dot;
                recomp_frac2 = cicloPCRCMCI_withRH.recomp_frac;
                LT_Q = cicloPCRCMCI_withRH.LT.Q_dot;
                LT_mdotc = cicloPCRCMCI_withRH.LT.m_dot_design[0];
                LT_mdoth = cicloPCRCMCI_withRH.LT.m_dot_design[1];
                LT_Tcin = cicloPCRCMCI_withRH.LT.T_c_in;
                LT_Thin = cicloPCRCMCI_withRH.LT.T_h_in;
                LT_Pcin = cicloPCRCMCI_withRH.LT.P_c_in;
                LT_Phin = cicloPCRCMCI_withRH.LT.P_h_in;
                LT_Pcout = cicloPCRCMCI_withRH.LT.P_c_out;
                LT_Phout = cicloPCRCMCI_withRH.LT.P_h_out;
                LT_Effc = cicloPCRCMCI_withRH.LT.eff;

                HT_Q = cicloPCRCMCI_withRH.HT.Q_dot;
                HT_mdotc = cicloPCRCMCI_withRH.HT.m_dot_design[0];
                HT_mdoth = cicloPCRCMCI_withRH.HT.m_dot_design[1];
                HT_Tcin = cicloPCRCMCI_withRH.HT.T_c_in;
                HT_Thin = cicloPCRCMCI_withRH.HT.T_h_in;
                HT_Pcin = cicloPCRCMCI_withRH.HT.P_c_in;
                HT_Phin = cicloPCRCMCI_withRH.HT.P_h_in;
                HT_Pcout = cicloPCRCMCI_withRH.HT.P_c_out;
                HT_Phout = cicloPCRCMCI_withRH.HT.P_h_out;
                HT_Effc = cicloPCRCMCI_withRH.HT.eff;

                PC1 = -cicloPCRCMCI_withRH.PC1.Q_dot;
                PC2 = -cicloPCRCMCI_withRH.PC2.Q_dot;
                PC3 = -cicloPCRCMCI_withRH.PC3.Q_dot;

                //    if (comboBox4.Text == "Dual-Loop")
                //    {
                //        //Main SF
                //        comboBox9.Enabled = true;
                //        comboBox11.Enabled = true;
                //        comboBox8.Enabled = true;
                //        comboBox10.Enabled = true;
                //        button24.Enabled = true;
                //        button25.Enabled = true;

                //        button13.Enabled = false;
                //    }

                //    else if ((comboBox4.Text == "Parabolic") || (comboBox4.Text == "Fresnel"))
                //    {
                //        //Main SF
                //        comboBox9.Enabled = false;
                //        comboBox11.Enabled = false;
                //        comboBox8.Enabled = false;
                //        comboBox10.Enabled = false;
                //        button24.Enabled = false;
                //        button25.Enabled = false;

                //        button13.Enabled = true;
                //    }

                //    if (comboBox5.Text == "Dual-Loop")
                //    {
                //        //ReHeating SF
                //        comboBox12.Enabled = true;
                //        comboBox13.Enabled = true;
                //        comboBox14.Enabled = true;
                //        comboBox15.Enabled = true;
                //        button22.Enabled = true;
                //        button23.Enabled = true;

                //        button8.Enabled = false;
                //    }

                //    else if ((comboBox5.Text == "Parabolic") || (comboBox5.Text == "Fresnel"))
                //    {
                //        //ReHeaing SF
                //        comboBox12.Enabled = false;
                //        comboBox13.Enabled = false;
                //        comboBox14.Enabled = false;
                //        comboBox15.Enabled = false;
                //        button22.Enabled = false;
                //        button23.Enabled = false;

                //        button8.Enabled = true;
                //    }

                    button6.Enabled = true;
                    button8.Enabled = true;
                    button13.Enabled = true;
                    button4.Enabled = true;
                    button7.Enabled = true;
                    button9.Enabled = true;
                    button15.Enabled = true;
                    button10.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                    button14.Enabled = true;                   
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                comboBox16.Enabled = false;
                comboBox18.Enabled = false;
                comboBox17.Enabled = false;
                textBox35.Enabled = false;
                textBox36.Enabled = false;
                textBox69.Enabled = false;
                textBox70.Enabled = false;
                button20.Enabled = false;              
            }

            else if (comboBox1.Text == "NewMixture")
            {
                comboBox16.Enabled = true;
                comboBox18.Enabled = true;
                comboBox17.Enabled = true;
                textBox35.Enabled = true;
                textBox36.Enabled = true;
                textBox69.Enabled = true;
                textBox70.Enabled = true;
                button20.Enabled = true;
             
                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

                textBox34.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox68.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox51.Text = Convert.ToString(working_fluid.CriticalDensity);

                MixtureCriticalPressure = working_fluid.CriticalPressure;
                MixtureCriticalTemperature = working_fluid.CriticalTemperature;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        //Simple brayton cycle
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //SIMPLE Brayton Options
            if (checkBox2.Checked == true)
            {
                textBox15.Text = "0.0";
                textBox16.Text = "0.0";
            }

            //SIMPLE Brayton Options
            else if (checkBox2.Checked == false)
            {
                textBox15.Text = "0.25";
                textBox16.Text = "5000";
            }
        }

        //LTR calculation
        private void button4_Click(object sender, EventArgs e)
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

        //HTR calculation
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

        //SF and PHX Design
        private void button15_Click(object sender, EventArgs e)
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

                SF_PHX.textBox41.Text = Convert.ToString(PHX1);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(temp25);
                SF_PHX.textBox36.Text = Convert.ToString(pres25);
                SF_PHX.textBox35.Text = Convert.ToString(pres26);
                SF_PHX.PTC_Solar_Field_uno(luis);
                //SF_PHX.SF_Type_variable = "Main_SF";
                //SF_PHX.textBox37.Text = Convert.ToString(temp26 + Convert.ToDouble(SF_PHX.textBox107.Text));
                SF_PHX.PTC_Solar_Field_ciclo_PCRCMCI_Design_withReHeating(this, 14, "Main_SF");
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

                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX1);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(temp25);
                SF_PHX_LF.textBox36.Text = Convert.ToString(pres25);
                SF_PHX_LF.textBox35.Text = Convert.ToString(pres26);
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.LF_Solar_Field_ciclo_PCRCMCI_Design_withReHeating(this, 14, "Main_SF");
                //SF_PHX_LF.SF_Type_variable = "Main_SF";
                SF_PHX_LF.Load_ComboBox7();
                //SF_PHX_LF.textBox37.Text = Convert.ToString(temp26 + Convert.ToDouble(SF_PHX_LF.textBox107.Text));
                SF_PHX_LF.button3_Click(this, e);
                SF_PHX_LF.Show();
            }
        }

        //SF and RHX Design
        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox5.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();

                if (comboBox6.Text == "Solar Salt")
                {
                    SF_PHX.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox6.Text == "Hitec XL")
                {
                    SF_PHX.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox6.Text == "Therminol VP1")
                {
                    SF_PHX.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox6.Text == "Syltherm_800")
                {
                    SF_PHX.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox6.Text == "Dowtherm_A")
                {
                    SF_PHX.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox6.Text == "Therminol_75")
                {
                    SF_PHX.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox6.Text == "Liquid Sodium")
                {
                    SF_PHX.comboBox1.Text = "Liquid Sodium";
                }

                SF_PHX.textBox41.Text = Convert.ToString(RHX1);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(temp215);
                SF_PHX.textBox36.Text = Convert.ToString(pres215);
                SF_PHX.textBox35.Text = Convert.ToString(pres216);
                SF_PHX.PTC_Solar_Field_uno(luis);
                //SF_PHX.SF_Type_variable = "ReHeating_SF";
                //SF_PHX.textBox37.Text = Convert.ToString(temp216 + Convert.ToDouble(SF_PHX.textBox107.Text));
                SF_PHX.PTC_Solar_Field_ciclo_PCRCMCI_Design_withReHeating(this, 14, "ReHeating_SF");
                SF_PHX.button3_Click(this, e);
                SF_PHX.Show();
            }

            else if (comboBox5.Text == "Fresnel")
            {
                SF_RHX_LF = new Fresnel();

                if (comboBox6.Text == "Solar Salt")
                {
                    SF_RHX_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox6.Text == "Hitec XL")
                {
                    SF_RHX_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox6.Text == "Therminol VP1")
                {
                    SF_RHX_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox6.Text == "Syltherm_800")
                {
                    SF_RHX_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox6.Text == "Dowtherm_A")
                {
                    SF_RHX_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox6.Text == "Therminol_75")
                {
                    SF_RHX_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox6.Text == "Liquid Sodium")
                {
                    SF_RHX_LF.comboBox1.Text = "Liquid Sodium";
                }

                SF_RHX_LF.textBox41.Text = Convert.ToString(RHX1);
                SF_RHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_RHX_LF.textBox38.Text = Convert.ToString(temp215);
                SF_RHX_LF.textBox36.Text = Convert.ToString(pres215);
                SF_RHX_LF.textBox35.Text = Convert.ToString(pres216);
                SF_RHX_LF.LF_Solar_Field_uno(luis);
                //SF_RHX_LF.SF_Type_variable = "ReHeating_SF";
                //SF_RHX_LF.textBox37.Text = Convert.ToString(temp216 + Convert.ToDouble(SF_RHX_LF.textBox107.Text));
                SF_PHX_LF.LF_Solar_Field_ciclo_PCRCMCI_Design_withReHeating(this, 14, "ReHeating_SF");
                SF_RHX_LF.Load_ComboBox7();
                SF_RHX_LF.button3_Click(this, e);
                SF_RHX_LF.Show();
            }
        }

        //PreCooler1
        private void button6_Click(object sender, EventArgs e)
        {
            Precooler_dialog1 = new PreeCooler();
            Precooler_dialog1.textBox2.Text = Convert.ToString(-PC1);
            Precooler_dialog1.textBox4.Text = Convert.ToString(massflow2);
            Precooler_dialog1.textBox6.Text = Convert.ToString(temp29);
            Precooler_dialog1.textBox12.Text = Convert.ToString(pres29);
            Precooler_dialog1.textBox8.Text = Convert.ToString(pres211);
            Precooler_dialog1.PreeCooler1(luis);
            Precooler_dialog1.Calculate_Cooler();
            Precooler_dialog1.Show();
        }

        //PreCooler2
        private void button8_Click(object sender, EventArgs e)
        {
            Precooler_dialog2 = new PreeCooler();
            Precooler_dialog2.textBox2.Text = Convert.ToString(-PC2);
            Precooler_dialog2.textBox4.Text = Convert.ToString(massflow2 * (1 - recomp_frac2));
            Precooler_dialog2.textBox6.Text = Convert.ToString(temp212);
            Precooler_dialog2.textBox12.Text = Convert.ToString(pres212);
            Precooler_dialog2.textBox8.Text = Convert.ToString(pres21);
            Precooler_dialog2.PreeCooler1(luis);
            Precooler_dialog2.Calculate_Cooler();
            Precooler_dialog2.Show();
        }

        //PreCooler3
        private void button13_Click(object sender, EventArgs e)
        {
            Precooler_dialog3 = new PreeCooler();
            Precooler_dialog3.textBox2.Text = Convert.ToString(-PC3);
            Precooler_dialog3.textBox4.Text = Convert.ToString(massflow2 * (1 - recomp_frac2));
            Precooler_dialog3.textBox6.Text = Convert.ToString(temp22);
            Precooler_dialog3.textBox12.Text = Convert.ToString(pres22);
            Precooler_dialog3.textBox8.Text = Convert.ToString(pres213);
            Precooler_dialog3.PreeCooler1(luis);
            Precooler_dialog3.Calculate_Cooler();
            Precooler_dialog3.Show();
        }

        //Main Compressor1
        private void button10_Click(object sender, EventArgs e)
        {
            button19.Enabled = true;
            button31.Enabled = true;
            Main_Compressor1 = new snl_compressor_tsr();
            Main_Compressor1.textBox1.Text = Convert.ToString(pres21);
            Main_Compressor1.textBox2.Text = Convert.ToString(temp21);
            Main_Compressor1.textBox6.Text = Convert.ToString(pres22);
            Main_Compressor1.textBox5.Text = Convert.ToString(temp22);
            Main_Compressor1.textBox9.Text = Convert.ToString(massflow2);
            Main_Compressor1.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Compressor1.button3.Enabled = false;
            Main_Compressor1.button5.Enabled = false;
            Main_Compressor1.button6.Enabled = false;
            Main_Compressor1.button7.Enabled = false;
            Main_Compressor1.Calculate_Main_Compressor();
            N_design_Main_Compressor = Convert.ToDouble(Main_Compressor1.textBox11.Text);

            Main_Compressor1.Show();
        }

        //Main Compressor2
        private void button11_Click(object sender, EventArgs e)
        {
            button19.Enabled = true;
            button31.Enabled = true;
            Main_Compressor2 = new snl_compressor_tsr();
            Main_Compressor2.textBox1.Text = Convert.ToString(pres213);
            Main_Compressor2.textBox2.Text = Convert.ToString(temp213);
            Main_Compressor2.textBox6.Text = Convert.ToString(pres214);
            Main_Compressor2.textBox5.Text = Convert.ToString(temp214);
            Main_Compressor2.textBox9.Text = Convert.ToString(massflow2);
            Main_Compressor2.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Compressor2.button3.Enabled = false;
            Main_Compressor2.button5.Enabled = false;
            Main_Compressor2.button6.Enabled = false;
            Main_Compressor2.button7.Enabled = false;
            Main_Compressor2.Calculate_Main_Compressor();
            N_design_Main_Compressor = Convert.ToDouble(Main_Compressor2.textBox11.Text);

            Main_Compressor2.Show();
        }

        //PreCompressor
        private void button14_Click(object sender, EventArgs e)
        {
            PreCompressor = new snl_compressor_tsr();
            PreCompressor.textBox1.Text = Convert.ToString(pres211);
            PreCompressor.textBox2.Text = Convert.ToString(temp211);
            PreCompressor.textBox6.Text = Convert.ToString(pres212);
            PreCompressor.textBox5.Text = Convert.ToString(temp212);
            PreCompressor.textBox9.Text = Convert.ToString(massflow2);
            PreCompressor.textBox8.Text = Convert.ToString(1.0);
            PreCompressor.button2.Enabled = false;
            PreCompressor.button4.Enabled = false;

            PreCompressor.Show();
        }

        //Main Turbine
        private void button19_Click(object sender, EventArgs e)
        {
            Main_Turbine = new Radial_Turbine();
            Main_Turbine.textBox1.Text = Convert.ToString(pres26);
            Main_Turbine.textBox6.Text = Convert.ToString(pres215);
            Main_Turbine.textBox2.Text = Convert.ToString(temp26);
            Main_Turbine.textBox5.Text = Convert.ToString(temp215);
            Main_Turbine.textBox9.Text = Convert.ToString(massflow2);
            Main_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            Main_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            Main_Turbine.calculate_Radial_Turbine();
            Main_Turbine.Show();
        }

        //ReHeating Turbine
        private void button31_Click(object sender, EventArgs e)
        {
            ReHeating_Turbine = new Radial_Turbine();
            ReHeating_Turbine.textBox1.Text = Convert.ToString(pres216);
            ReHeating_Turbine.textBox6.Text = Convert.ToString(pres27);
            ReHeating_Turbine.textBox2.Text = Convert.ToString(temp216);
            ReHeating_Turbine.textBox5.Text = Convert.ToString(temp27);
            ReHeating_Turbine.textBox9.Text = Convert.ToString(massflow2);
            ReHeating_Turbine.textBox8.Text = Convert.ToString(recomp_frac2);
            ReHeating_Turbine.textBox3.Text = Convert.ToString(N_design_Main_Compressor);
            ReHeating_Turbine.calculate_Radial_Turbine();
            ReHeating_Turbine.Show();
        }

        //ReCompressor
        private void button12_Click(object sender, EventArgs e)
        {
            PreCompressor = new snl_compressor_tsr();
            PreCompressor.textBox1.Text = Convert.ToString(pres212);
            PreCompressor.textBox2.Text = Convert.ToString(temp212);
            PreCompressor.textBox6.Text = Convert.ToString(pres210);
            PreCompressor.textBox5.Text = Convert.ToString(temp210);
            PreCompressor.textBox9.Text = Convert.ToString(massflow2);
            PreCompressor.textBox8.Text = Convert.ToString(recomp_frac2);
            PreCompressor.button2.Enabled = false;
            PreCompressor.button4.Enabled = false;

            PreCompressor.Show();
        }

        //PHX1_SF1_Dual_Loop
        private void button25_Click(object sender, EventArgs e)
        {
            //if (comboBox11.Text == "Parabolic")
            //{
            //    SF_PHX = new PTC_Solar_Field();

            //    if (comboBox9.Text == "Solar Salt")
            //    {
            //        SF_PHX.comboBox1.Text = "Solar Salt";
            //    }
            //    else if (comboBox9.Text == "Hitec XL")
            //    {
            //        SF_PHX.comboBox1.Text = "Hitec XL";
            //    }
            //    else if (comboBox9.Text == "Therminol VP1")
            //    {
            //        SF_PHX.comboBox1.Text = "Therminol VP1";
            //    }
            //    else if (comboBox9.Text == "Syltherm_800")
            //    {
            //        SF_PHX.comboBox1.Text = "Syltherm_800";
            //    }
            //    else if (comboBox9.Text == "Dowtherm_A")
            //    {
            //        SF_PHX.comboBox1.Text = "Dowtherm_A";
            //    }
            //    else if (comboBox9.Text == "Therminol_75")
            //    {
            //        SF_PHX.comboBox1.Text = "Therminol_75";
            //    }
            //    else if (comboBox9.Text == "Liquid Sodium")
            //    {
            //        SF_PHX.comboBox1.Text = "Liquid Sodium";
            //    }

            //    luis.working_fluid.FindStateWithTP(temp25, pres25); // properties at the inlet conditions
            //    PHX1_h_in = luis.working_fluid.Enthalpy;
            //    PHX1_temp_out = Convert.ToDouble(textBox33.Text);
            //    DP_PHX1 = Convert.ToDouble(textBox32.Text);
            //    PHX1_pres_out = pres25 - pres25 * DP_PHX1;
            //    luis.working_fluid.FindStateWithTP(PHX1_temp_out, PHX1_pres_out); // properties at the inlet conditions
            //    PHX1_h_out = luis.working_fluid.Enthalpy;
            //    PHX1_Q = massflow2 * (PHX1_h_out - PHX1_h_in);

            //    SF_PHX.textBox41.Text = Convert.ToString(PHX1_Q);
            //    SF_PHX.textBox40.Text = Convert.ToString(massflow2);
            //    SF_PHX.textBox38.Text = Convert.ToString(temp25);
            //    SF_PHX.textBox36.Text = Convert.ToString(pres25);
            //    SF_PHX.textBox35.Text = Convert.ToString(PHX1_pres_out);
            //    SF_PHX.PTC_Solar_Field_uno(luis);
            //    SF_PHX.PTC_Solar_Field_ciclo_PCRC_Design_withReHeating(this, 6, "Main_SF1_Dual_Loop");
            //    SF_PHX.button3_Click(this, e);
            //    SF_PHX.Show();
            //}

            //else if (comboBox11.Text == "Fresnel")
            //{
            //    SF_PHX_LF = new Fresnel();

            //    if (comboBox9.Text == "Solar Salt")
            //    {
            //        SF_PHX_LF.comboBox1.Text = "Solar Salt";
            //    }
            //    else if (comboBox9.Text == "Hitec XL")
            //    {
            //        SF_PHX_LF.comboBox1.Text = "Hitec XL";
            //    }
            //    else if (comboBox9.Text == "Therminol VP1")
            //    {
            //        SF_PHX_LF.comboBox1.Text = "Therminol VP1";
            //    }
            //    else if (comboBox9.Text == "Syltherm_800")
            //    {
            //        SF_PHX_LF.comboBox1.Text = "Syltherm_800";
            //    }
            //    else if (comboBox9.Text == "Dowtherm_A")
            //    {
            //        SF_PHX_LF.comboBox1.Text = "Dowtherm_A";
            //    }
            //    else if (comboBox9.Text == "Therminol_75")
            //    {
            //        SF_PHX_LF.comboBox1.Text = "Therminol_75";
            //    }
            //    else if (comboBox9.Text == "Liquid Sodium")
            //    {
            //        SF_PHX_LF.comboBox1.Text = "Liquid Sodium";
            //    }

            //    luis.working_fluid.FindStateWithTP(temp25, pres25); // properties at the inlet conditions
            //    PHX1_h_in = luis.working_fluid.Enthalpy;
            //    PHX1_temp_out = Convert.ToDouble(textBox33.Text);
            //    DP_PHX1 = Convert.ToDouble(textBox32.Text);
            //    PHX1_pres_out = pres25 - pres25 * DP_PHX1;
            //    luis.working_fluid.FindStateWithTP(PHX1_temp_out, PHX1_pres_out); // properties at the inlet conditions
            //    PHX1_h_out = luis.working_fluid.Enthalpy;
            //    PHX1_Q = massflow2 * (PHX1_h_out - PHX1_h_in);

            //    SF_PHX_LF.textBox41.Text = Convert.ToString(PHX1_Q);
            //    SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
            //    SF_PHX_LF.textBox38.Text = Convert.ToString(temp25);
            //    SF_PHX_LF.textBox36.Text = Convert.ToString(pres25);
            //    SF_PHX_LF.textBox35.Text = Convert.ToString(PHX1_pres_out);
            //    SF_PHX_LF.LF_Solar_Field_uno(luis);
            //    SF_PHX_LF.LF_Solar_Field_ciclo_PCRC_Design_withReHeating(this, 6, "Main_SF1_Dual_Loop");
            //    SF_PHX_LF.Load_ComboBox7();
            //    SF_PHX_LF.button3_Click(this, e);
            //    SF_PHX_LF.Show();
            //}
        }

        //Recomp. Fraction sensing analysis
        private void button29_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "Recomp. Frac.";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature);

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
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //Recomp.Fraction variation
                for (double i = Convert.ToDouble(textBox82.Text); i <= Convert.ToDouble(textBox81.Text); i = i + Convert.ToDouble(textBox80.Text))
                {
                    textBox15.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox15.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

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
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";             

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_without_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }               

        //CIT1 sensing analysis
        private void button33_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "CIT1(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT1(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIT(K)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //CIT1 variation
                for (double i = Convert.ToDouble(textBox79.Text); i <= Convert.ToDouble(textBox78.Text); i = i + Convert.ToDouble(textBox77.Text))
                {
                    //Set CIT1
                    textBox102.Text = Convert.ToString(Convert.ToDecimal(i));
                   
                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox102.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

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
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //CIT2 sensing analysis
        private void button27_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "CIT2(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT2(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIT(K)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //CIT2 variation
                for (double i = Convert.ToDouble(textBox97.Text); i <= Convert.ToDouble(textBox96.Text); i = i + Convert.ToDouble(textBox95.Text))
                {
                    //Set CIT2
                    textBox2.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox2.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

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
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //CIT3 sensing analysis
        private void button34_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "CIT3(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature - 273.15);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT3(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIT(K)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //CIT3 variation
                for (double i = Convert.ToDouble(textBox79.Text); i <= Convert.ToDouble(textBox78.Text); i = i + Convert.ToDouble(textBox77.Text))
                {
                    //Set CIT3
                    textBox28.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox28.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres21;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox28.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox28.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //CIP1 sensing analysis
        private void button32_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "CIP1(kPa)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP1(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //CIP1 variation
                for (double i = Convert.ToDouble(textBox76.Text); i <= Convert.ToDouble(textBox75.Text); i = i + Convert.ToDouble(textBox74.Text))
                {
                    //Set CIP1
                    textBox103.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox103.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres211;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox3.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //CIP2 sensing analysis
        private void button26_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "CIP2(kPa)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP2(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //CIP2 variation
                for (double i = Convert.ToDouble(textBox94.Text); i <= Convert.ToDouble(textBox93.Text); i = i + Convert.ToDouble(textBox92.Text))
                {
                    //Set CIP2
                    textBox3.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox3.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

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
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //CIP3 sensing analysis
        private void button35_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "CIP3(kPa)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature);

            xlWorkSheet1.Cells[3, 1] = "";
            xlWorkSheet1.Cells[3, 2] = "";
            xlWorkSheet1.Cells[4, 3] = "";

            xlWorkSheet1.Cells[4, 1] = "CIP3(kPa)";
            xlWorkSheet1.Cells[4, 2] = "CIT(K)";
            xlWorkSheet1.Cells[4, 3] = "LT UA(kW/K)";
            xlWorkSheet1.Cells[4, 4] = "HT UA(kW/K)";
            xlWorkSheet1.Cells[4, 5] = "CIP(kPa)";
            xlWorkSheet1.Cells[4, 6] = "Eff.(%)";
            xlWorkSheet1.Cells[4, 7] = "LTR Eff.(%)";
            xlWorkSheet1.Cells[4, 8] = "LTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 9] = "HTR Eff.(%)";
            xlWorkSheet1.Cells[4, 10] = "HTR Pinch(ºC)";
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //CIP1 variation
                for (double i = Convert.ToDouble(textBox112.Text); i <= Convert.ToDouble(textBox111.Text); i = i + Convert.ToDouble(textBox110.Text))
                {
                    //Set CIP1
                    textBox23.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox23.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres213;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox3.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //TIP sensing analysis
        private void button21_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "TIT(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature);

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
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //TIP variation
                for (double i = Convert.ToDouble(textBox85.Text); i <= Convert.ToDouble(textBox84.Text); i = i + Convert.ToDouble(textBox83.Text))
                {
                    //Set TIP
                    textBox22.Text = Convert.ToString(Convert.ToDecimal(i));
                    
                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox22.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

                    //if (comboBox4.Text == "Parabolic")
                    //{
                    //    listBox6.Items.Add(PTC_Main_SF_Effective_Apperture_Area.ToString());
                    //}
                    //else if (comboBox4.Text == "Fresnel")
                    //{
                    //    listBox6.Items.Add(LF_Main_SF_Effective_Apperture_Area.ToString());
                    //}

                    xlWorkSheet1.Cells[counter + 1, 1] = pres214;
                    xlWorkSheet1.Cells[counter + 1, 2] = Convert.ToString(Convert.ToDouble(textBox2.Text) - 273.15);
                    xlWorkSheet1.Cells[counter + 1, 3] = textBox17.Text;
                    xlWorkSheet1.Cells[counter + 1, 4] = textBox16.Text;
                    xlWorkSheet1.Cells[counter + 1, 5] = textBox3.Text;
                    xlWorkSheet1.Cells[counter + 1, 6] = (eta_thermal2 * 100).ToString();
                    xlWorkSheet1.Cells[counter + 1, 7] = LT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 8] = LTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 9] = HT_Effc.ToString();
                    xlWorkSheet1.Cells[counter + 1, 10] = HTR_min_DT_paper.ToString();
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
        }

        //TIT sensing analysis
        private void button28_Click(object sender, EventArgs e)
        {
            PCRCMCI_Sensing_Analysis_Results PCRCMCI_Sensing_Analysis_Results_window = new PCRCMCI_Sensing_Analysis_Results();
            PCRCMCI_Sensing_Analysis_Results_window.Show();

            PCRCMCI_Sensing_Analysis_Results_window.label96.Text = "TIT(K)";

            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

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

            xlWorkSheet1.Name = this.comboBox2.Text + "_Mixture";

            xlWorkSheet1.Cells[1, 1] = this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text;
            xlWorkSheet1.Cells[1, 2] = "Pcrit(kPa)";
            xlWorkSheet1.Cells[1, 3] = "Tcrit(ºC)";

            xlWorkSheet1.Cells[2, 1] = "";
            xlWorkSheet1.Cells[2, 2] = Convert.ToString(working_fluid.CriticalPressure);
            xlWorkSheet1.Cells[2, 3] = Convert.ToString(working_fluid.CriticalTemperature);

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
            xlWorkSheet1.Cells[4, 11] = "PTC_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 12] = "PTC_Pressure_Drop(bar)";
            xlWorkSheet1.Cells[4, 13] = "LF_Apperture_Area(m2)";
            xlWorkSheet1.Cells[4, 14] = "LF_Pressure_Drop(bar)";

            //UA variation
            for (double j = Convert.ToDouble(textBox73.Text); j <= Convert.ToDouble(textBox72.Text); j = j + Convert.ToDouble(textBox71.Text))
            {
                textBox17.Text = Convert.ToString(j);
                textBox16.Text = Convert.ToString(j);

                //TIT variation
                for (double i = Convert.ToDouble(textBox91.Text); i <= Convert.ToDouble(textBox90.Text); i = i + Convert.ToDouble(textBox89.Text))
                {
                    //Set TIT
                    textBox4.Text = Convert.ToString(Convert.ToDecimal(i));
                    textBox6.Text = Convert.ToString(Convert.ToDecimal(i));

                    //Call Subplex Button
                    button20_Click(this, e);

                    //Final Report
                    double LTR_min_DT_1 = temp28 - temp23;
                    double LTR_min_DT_2 = temp29 - temp22;
                    double LTR_min_DT_paper = Math.Min(LTR_min_DT_1, LTR_min_DT_2);

                    double HTR_min_DT_1 = temp28 - temp24;
                    double HTR_min_DT_2 = temp27 - temp25;
                    double HTR_min_DT_paper = Math.Min(HTR_min_DT_1, HTR_min_DT_2);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox1.Items.Add(textBox4.Text);

                    PCRCMCI_Sensing_Analysis_Results_window.listBox2.Items.Add((eta_thermal2 * 100).ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox3.Items.Add((LT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox4.Items.Add(LTR_min_DT_paper.ToString());

                    PCRCMCI_Sensing_Analysis_Results_window.listBox5.Items.Add((HT_Effc * 100).ToString());
                    PCRCMCI_Sensing_Analysis_Results_window.listBox6.Items.Add(HTR_min_DT_paper.ToString());

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
                    xlWorkSheet1.Cells[counter + 1, 11] = PTC_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 12] = PTC_Main_SF_Pressure_drop.ToString();
                    xlWorkSheet1.Cells[counter + 1, 13] = LF_Main_SF_Effective_Apperture_Area.ToString();
                    xlWorkSheet1.Cells[counter + 1, 14] = LF_Main_SF_Pressure_drop.ToString();

                    //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(2);
                    //xlWorkSheet2.Cells[1, 1] = "hola_Irene";              

                    counter++;
                }
            }

            xlWorkBook1.SaveAs("c:\\Users\\luisc\\Desktop\\Paper_Results_PCRCMCI_with_ReHeating_" + xlWorkSheet1.Name + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue1, misValue1, misValue1, misValue1, Excel.XlSaveAsAccessMode.xlExclusive, misValue1, misValue1, misValue1, misValue1, misValue1);

            xlWorkBook1.Close(true, misValue1, misValue1);
            xlApp1.Quit();

            releaseObject(xlWorkSheet1);
            //releaseObject(xlWorkSheet2);
            releaseObject(xlWorkBook1);
            releaseObject(xlApp1);
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

        //Generator design
        private void button30_Click(object sender, EventArgs e)
        {
            Generator_dialog = new Generator();
            Generator_dialog.Generator_Shaft_Power = w_dot_net2;
            Generator_dialog.PCRCMCI_Design_withReHeating(this, 14);
            Generator_dialog.button2_Click(this, e);
            Generator_dialog.Show();
        }

        //Set critical conditions
        private void button36_Click(object sender, EventArgs e)
        {
            double option1 = 0.0;
            double option2 = 0.0;
            double option3 = 0.0;
            double option4 = 0.0;

            option1 = Convert.ToDouble(this.textBox35.Text);
            option2 = Convert.ToDouble(this.textBox36.Text);
            option3 = Convert.ToDouble(this.textBox69.Text);
            option4 = Convert.ToDouble(this.textBox70.Text);

            if ((option1 == 1) || (option2 == 1) || (option3 == 1) || (option4 == 1))
            {
                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, ReferenceState.DEF);

                textBox34.Text = Convert.ToString(working_fluid.CriticalPressure);
                textBox68.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox51.Text = Convert.ToString(working_fluid.CriticalDensity);

                textBox2.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox28.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox102.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox103.Text = Convert.ToString(working_fluid.CriticalPressure);

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
                xlWorkSheet.Cells[14, 6] = this.comboBox16.Text;
                xlWorkSheet.Cells[15, 6] = this.comboBox18.Text;
                xlWorkSheet.Cells[16, 6] = this.comboBox17.Text;

                // % Compositions
                xlWorkSheet.Cells[13, 7] = this.textBox35.Text;
                xlWorkSheet.Cells[14, 7] = this.textBox36.Text;
                xlWorkSheet.Cells[15, 7] = this.textBox69.Text;
                xlWorkSheet.Cells[16, 7] = this.textBox70.Text;

                //MessageBox.Show(xlWorkSheet.get_Range("D68", "D68").Value2.ToString());
                this.textBox103.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox2.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox28.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox102.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();

                this.textBox34.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox68.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox51.Text = xlWorkSheet.get_Range("D70", "D70").Value2.ToString();

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

        //Optimization analysis
        private void button37_Click(object sender, EventArgs e)
        {

        }
    }
}
