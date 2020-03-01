using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace RefPropWindowsForms
{
    public partial class Fresnel : Form
    {
        public core CorePointer = new core();

        //Optics Calculations
        Double DAY, HOUR, B, Egiorno, Eorario, Tsun, decl, angorario, zone;
        Double angzenit, anginc_long,anginc_trans, alt_solare, azimuth, MerSD, Lat, Lon;
        List<Double> angles = new List<Double>();
        List<Double> IAM_longitudinal = new List<Double>();
        List<Double> IAM_transversal = new List<Double>();

        List<Double[]> IAM_thermoflow25 = new List<Double[]>();

        public Double DNI;
        public Double NominalOpticalEfficiency;
        public Double CleanlinessFactor;
        public Double EndLossFactor;
        public Double Lf_ave;
        public Double IAMOverall;
        public Double IAMLongitudinal;
        public Double IAMTransversal;
        public Double CollectorApertureWidth;
        public Double Focal_distance;
        public Double SolarFieldThermalEnergy;
        public Double NumberRows;
        public Double SolarFieldInletTemperature;
        public Double SolarFieldOutputTemperature;
        public Double SolarFieldTemperatureIncrement;
        public Double CoefficientA1;
        public Double CoefficientA2;
        public Int64 Number_Of_Temperatures; //Number_Of_Temperatures = Receiver_Number_Of_Segments + 1 (number of segments for calculation - finite difference analysis)
        public Double FACTOR;
        public Double temp1;
        public Double temp2;
        public Double temp3;
        public Double ReflectorApertureArea;
        public Double ReflectorArea;
        public Double ReflectorLength;
        public Double RowLength;
        public Double temp4;
        public Double LengthIncrement;
        public Double Desired_Mass_Flux;
        public Double Actual_Mass_Flux;

        public Double[] Receiver_lengths;
        public Double[] Temperature;
        public Double[] ThermalLosses;
        public Double[] PressureDrop;
        public Double[] rho;
        public Double[] velocity;
        public Double[] Reynold_number;
        public Double[] Dynamic_viscosity;
        public Double[] Darcy;
        public Double[] Density_Viscosity;
        public Double Rugosidad;
        public Double CrossArea, Diameter_Interior;
        public Double Caudal_per_row;
        public Double Total_Pressure_Drop;

        public Double ThermalLossesTotal;

        public Double temp5;
        public Double temp6;
        public Double temp7;
        public Double ERROR;

        public Double AdmisibleError;

        public Int64 error_code;

        public Double SolarImpinging_path, SolarEnergyAbsorbed_path, Energyloss_path, NetAbsorbed_path, NeatAbsorbed_Field;
        public Double Collector_Efficiency, Field_Efficiency, Area_Efficiency;

        public Double LF_SF_Effective_Apperture_Area;

        //Input Data: N_sub_hxrs, Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out
        public Int64 N_sub_hxrs;
        public Double Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out;

        //Results: UA, min_DT
        public Double UA, min_DT, Effectiveness;
        public Double NTU, CR;

        public Double Cp_HTF;

        //Storing PTC_SF_Design dialog results in the Brayton dialog for Final Reporting or Cost Estimation
        public Double Brayton_cycle_type_variable;
        public String SF_Type_variable;

        public RC_Optimization Punterociclo_1;
        public Recompression_Brayton_Power_Cycle Punterociclo_2;
        public RC_optimal_without_ReHeating Punterociclo_3;
        public RC_without_ReHeating Punterociclo_4;
        public RC_with_Two_ReHeating Punterociclo_15;
        public RC_with_Three_ReHeating Punterociclo_16;
        public RC_without_ReHeating_new_proposed_configuration Punterociclo_21;

        public PCRC_with_ReHeating Punterociclo_5;
        public PCRC Punterociclo_6;
        public PCRC_optimal_withoutReHeating Punterociclo_7;
        public PCRC_without_ReHeating Punterociclo_8;
        public PCRC_with_Two_ReHeating Punterociclo_17;
        public PCRC_with_Three_ReHeating Punterociclo_18;

        public RCMCI_optimal Punterociclo_9;
        public RCMCI Punterociclo_10;
        public RCMCI_optimal_without_RH Punterociclo_11;
        public RCMCI_without_ReHeating Punterociclo_12;
        public RCMCI_with_Two_Reheatings Punterociclo_19;
        public RCMCI_with_Three_Reheatings Punterociclo_20;

        public PCRCMCI Punterociclo_14;

        //Solar Field Pump Design
        public Double Density_in;
        public Double Specific_Gravity;
        public Double Suction_Pressure;
        public Double Discharge_Pressure;
        public Double Calculated_Head;
        public Double Flow;
        public Double Gravity;
        public Double Calculated_Power;
        public Double Pump_isoentropic_Efficiency;
        public Double Hydraulic_Pumping_Power;
        public Double Pump_Mechanical_Efficiency;
        public Double Pump_Shaft_Work;
        public Double Motor_Efficiency;
        public Double Electrical_Consumption;
        public Double Name_Plate_Design_Point_Load;
        public Double Motor_Name_Plate_Power;
        public Double Solar_Field_AP;
        public Double HX_Hot_Side_AP;

        Double[] T_c;
        Double[] T_h;

        decimal[] T_cd;
        decimal[] T_hd;

        public Double[] UA_local_1 = new Double[25];
        public Double[] NTU_local_1 = new Double[25];
        public Double[] CR_local_1 = new Double[25];
        public Double[] Effec_local_1 = new Double[25];
        public Double[] C_dot_c_local_1 = new Double[25];
        public Double[] C_dot_h_local_1 = new Double[25];

        public Double[] UA_local_1_module = new Double[25];
        public Double[] NTU_local_1_module = new Double[25];
        public Double[] CR_local_1_module = new Double[25];
   
        public Double[] Effec_local_1_module = new Double[25];
        public Double[] C_dot_c_local_1_module = new Double[25];
        public Double[] C_dot_h_local_1_module = new Double[25];

        public Boolean CR_Calculated=true;

        public Fresnel()
        {
            InitializeComponent();
        }

        //OK Button

        public void LF_Solar_Field_uno(core HXwindow)
        {
            CorePointer = HXwindow;
        }

        // RC_Optimization_WithReHeating: Brayton_cycle_type_variable = 1
        public void LF_Solar_Field_ciclo_RC_Optimization_withReHeating(RC_Optimization Punterociclo1, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_1 = Punterociclo1;
        }

        // RC_Design_WithReHeating: Brayton_cycle_type_variable = 2
        public void LF_Solar_Field_ciclo_RC_Design_withReHeating(Recompression_Brayton_Power_Cycle Punterociclo2, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_2 = Punterociclo2;
        }

        // RC_Optimization_WithoutReHeating: Brayton_cycle_type_variable = 3
        public void LF_Solar_Field_ciclo_RC_Optimization_withoutReHeating(RC_optimal_without_ReHeating Punterociclo3, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_3 = Punterociclo3;
        }

        // RC_Design_WithoutReHeating: Brayton_cycle_type_variable = 4
        public void LF_Solar_Field_ciclo_RC_Design_withoutReHeating(RC_without_ReHeating Punterociclo4, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_4 = Punterociclo4;
        }

        // RC_Design_WithTwoReHeating: Brayton_cycle_type_variable = 15
        public void LF_Solar_Field_ciclo_RC_with_Two_ReHeating(RC_with_Two_ReHeating Punterociclo15, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_15 = Punterociclo15;
        }

        // RC_Design_WithTwoReHeating: Brayton_cycle_type_variable = 16
        public void LF_Solar_Field_ciclo_RC_with_Three_ReHeating(RC_with_Three_ReHeating Punterociclo16, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_16 = Punterociclo16;
        }

        // PCRC_Optimization_WithReHeating: Brayton_cycle_type_variable = 5
        public void LF_Solar_Field_ciclo_PCRC_Optimization_withReHeating(PCRC_with_ReHeating Punterociclo5, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_5 = Punterociclo5;
        }

        // PCRC_Design_WithReHeating: Brayton_cycle_type_variable = 6
        public void LF_Solar_Field_ciclo_PCRC_Design_withReHeating(PCRC Punterociclo6, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_6 = Punterociclo6;
        }

        // PCRC_Optimization_WithoutReHeating: Brayton_cycle_type_variable = 7
        public void LF_Solar_Field_ciclo_PCRC_Optimization_withoutReHeating(PCRC_optimal_withoutReHeating Punterociclo7, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_7 = Punterociclo7;
        }

        // PCRC_Design_WithoutReHeating: Brayton_cycle_type_variable = 8
        public void LF_Solar_Field_ciclo_PCRC_Design_withoutReHeating(PCRC_without_ReHeating Punterociclo8, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_8 = Punterociclo8;
        }

        // PCRC_Design_WithTwoReHeating: Brayton_cycle_type_variable = 17
        public void LF_Solar_Field_ciclo_PCRC_with_Two_ReHeating(PCRC_with_Two_ReHeating Punterociclo17, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_17 = Punterociclo17;
        }

        // PCRC_Design_WithThreeReHeating: Brayton_cycle_type_variable = 18
        public void LF_Solar_Field_ciclo_PCRC_with_Three_ReHeating(PCRC_with_Three_ReHeating Punterociclo18, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_18 = Punterociclo18;
        }

        // RCMCI_Optimization_WithReHeating: Brayton_cycle_type_variable = 9
        public void LF_Solar_Field_ciclo_RCMCI_Optimization_withReHeating(RCMCI_optimal Punterociclo9, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_9 = Punterociclo9;
        }

        // RCMCI_Design_WithReHeating: Brayton_cycle_type_variable = 10
        public void LF_Solar_Field_ciclo_RCMCI_Design_withReHeating(RCMCI Punterociclo10, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_10 = Punterociclo10;
        }

        // RCMCI_Optimization_WithoutReHeating: Brayton_cycle_type_variable = 11
        public void LF_Solar_Field_ciclo_RCMCI_Optimization_withoutReHeating(RCMCI_optimal_without_RH Punterociclo11, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_11 = Punterociclo11;
        }

        // RCMCI_Design_WithoutReHeating: Brayton_cycle_type_variable = 12
        public void LF_Solar_Field_ciclo_RCMCI_Design_withoutReHeating(RCMCI_without_ReHeating Punterociclo12, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_12 = Punterociclo12;
        }

        // RCMCI_Design_WithTwoReHeating: Brayton_cycle_type_variable = 19
        public void LF_Solar_Field_ciclo_RCMCI_with_Two_Reheatings(RCMCI_with_Two_Reheatings Punterociclo19, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_19 = Punterociclo19;
        }

        // RCMCI_Design_WithThreeReHeating: Brayton_cycle_type_variable = 20
        public void LF_Solar_Field_ciclo_RCMCI_with_Three_Reheatings(RCMCI_with_Three_Reheatings Punterociclo20, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_20 = Punterociclo20;
        }

        // PCRCMCI_Design_WithoutReHeating: Brayton_cycle_type_variable = 14
        public void LF_Solar_Field_ciclo_PCRCMCI_Design_withReHeating(PCRCMCI Punterociclo14, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_14 = Punterociclo14;
        }

        // RC_Design_WithoutReHeating: Brayton_cycle_type_variable = 21
        public void LF_Solar_Field_ciclo_RC_Design_withoutReHeating_new_configuration(RC_without_ReHeating_new_proposed_configuration Punterociclo21, Double Brayton_cycle_type, String SF_Type)
        {
            SF_Type_variable = SF_Type;
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_21 = Punterociclo21;
        }

        //Calculate PHX Design
        public void Calculate_PHX()
        {
            Int16 Number_Modules;

            Double Q_dot_permodule;

            chart1.Series.Clear();

            chart1.Series.Add("Cold_Side");
            chart1.Series.Add("Hot_Side");

            chart2.Series.Clear();

            chart2.Series.Add("Cold_Side");
            chart2.Series.Add("Hot_Side");

            if (this.Brayton_cycle_type_variable == 1)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_1.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_1.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_1.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_1.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_1.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_1.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 2)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_2.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_2.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_2.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_2.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_2.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_2.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }

            //RC without ReHeating OPTIMIZATION
            else if (this.Brayton_cycle_type_variable == 3)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_3.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_3.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_3.temp26 + Convert.ToDouble(textBox107.Text));
                }
            }

            //RC without ReHeating DESIGN
            else if (this.Brayton_cycle_type_variable == 4)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_4.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_4.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_4.temp26 + Convert.ToDouble(textBox107.Text));
                }
            }

            //RC_without_ReHeating_new_proposed_configuration
            else if (this.Brayton_cycle_type_variable == 21)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_21.temp27 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_21.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_21.temp27 + Convert.ToDouble(textBox107.Text));
                }
            }

            //RC with Two ReHeating DESIGN
            else if (this.Brayton_cycle_type_variable == 15)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_15.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_15.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_15.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating1_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_15.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating2_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_15.temp214 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_15.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_15.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }
            
            else if (this.Brayton_cycle_type_variable == 5)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_5.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_5.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_5.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_5.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_5.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_5.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 6)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_6.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_6.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_6.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_6.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_6.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_6.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 7)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_7.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_7.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_7.temp26 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 8)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_8.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_8.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_8.temp26 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 9)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_9.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_9.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_9.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_9.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_9.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_9.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 10)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_10.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_10.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_10.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_10.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_10.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_10.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 11)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_11.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_11.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_11.temp26 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 12)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_12.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_12.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_12.temp26 + Convert.ToDouble(textBox107.Text));
                }
            }

            else if (this.Brayton_cycle_type_variable == 14)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_14.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_14.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_14.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_14.temp216 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_14.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_14.temp216 + Convert.ToDouble(textBox107.Text));
                }
            }

            //RCMCI with Two ReHeating DESIGN
            else if (this.Brayton_cycle_type_variable == 19)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_19.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_19.PHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_19.temp26 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating1_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_19.temp212 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating2_SF")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_19.temp216 + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF1_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_19.RHX1_temp_out + Convert.ToDouble(textBox107.Text));
                }

                else if (this.SF_Type_variable == "ReHeating_SF2_Dual_Loop")
                {
                    textBox37.Text = Convert.ToString(Punterociclo_19.temp212 + Convert.ToDouble(textBox107.Text));
                }
            }

            N_sub_hxrs = Convert.ToInt64(textBox42.Text);
            Q_dot = Convert.ToDouble(textBox41.Text);
            m_dot_c = Convert.ToDouble(textBox40.Text);
            m_dot_h = Convert.ToDouble(textBox39.Text);
            T_c_in = Convert.ToDouble(textBox38.Text);
            T_h_in = Convert.ToDouble(textBox37.Text);
            P_c_in = Convert.ToDouble(textBox36.Text);
            P_c_out = Convert.ToDouble(textBox35.Text);
            P_h_in = Convert.ToDouble(textBox34.Text);
            P_h_out = Convert.ToDouble(textBox33.Text);

            Number_Modules = Convert.ToInt16(textBox80.Text);
            Q_dot_permodule = Q_dot / Number_Modules;
            textBox79.Text = Convert.ToString(Q_dot_permodule);

            //Modificar este valor a una Recta dependiente de la Temperatura: Cp(T)
            Cp_HTF = Convert.ToDouble(textBox46.Text);

            T_c = new Double[N_sub_hxrs + 1];
            T_h = new Double[N_sub_hxrs + 1];

            T_cd = new decimal[N_sub_hxrs + 1];
            T_hd = new decimal[N_sub_hxrs + 1];

            Double[] P_c = new Double[N_sub_hxrs + 1];
            Double[] P_h = new Double[N_sub_hxrs + 1];

            Double[] UA_local = new Double[N_sub_hxrs];
            Double[] NTU_local = new Double[N_sub_hxrs];
            Double[] CR_local = new Double[N_sub_hxrs];
            Double[] Effec_local = new Double[N_sub_hxrs];
            Double[] C_dot_c_local = new Double[N_sub_hxrs];
            Double[] C_dot_h_local = new Double[N_sub_hxrs];

            Double[] UA_local_module = new Double[N_sub_hxrs];
            Double[] NTU_local_module = new Double[N_sub_hxrs];
            Double[] CR_local_module = new Double[N_sub_hxrs];
            Double[] Effec_local_module = new Double[N_sub_hxrs];
            Double[] C_dot_c_local_module = new Double[N_sub_hxrs];
            Double[] C_dot_h_local_module = new Double[N_sub_hxrs];

            NTU = 0;

            if (CR_Calculated == true)
            {
                CR = 0;
            }

            else if (CR_Calculated == false)
            {

            }

            CorePointer.calculate_PHX_UA(Cp_HTF, N_sub_hxrs, Q_dot, m_dot_c, ref m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out, ref error_code, ref UA, ref min_DT, ref T_h, ref T_c,
                ref Effectiveness, ref P_h, ref P_c, ref UA_local, ref NTU, ref CR, ref NTU_local, ref CR_local, ref Effec_local, ref CR_Calculated);

            if (CR_Calculated == true)
            {
                textBox39.BackColor = Color.White;
            }

            else if (CR_Calculated == false)
            {
                textBox39.Text = Convert.ToString(m_dot_h);
                textBox39.BackColor = Color.Yellow;
            }         
            
            if (min_DT < 2)
            {
               MessageBox.Show("Pinch Point value lower than 2ºC, please review the HX Input Data");
               
               goto exit_min_pinchpoint;
            }

            for (int j = 0; j < N_sub_hxrs; j++)
            {
                UA_local_1[j] = UA_local[j];
                NTU_local_1[j] = NTU_local[j];
                CR_local_1[j] = CR_local[j];
                Effec_local_1[j] = Effec_local[j];
            }

            for (int j = 0; j < N_sub_hxrs; j++)
            {
                UA_local_1_module[j] = UA_local_module[j];
                NTU_local_1_module[j] = NTU_local_module[j];
                CR_local_1_module[j] = CR_local_module[j];
                Effec_local_1_module[j] = Effec_local_module[j];
            }

            for (int j = 0; j <= N_sub_hxrs; j++)
            {
                T_cd[j] = Convert.ToDecimal(T_c[j]);
                T_hd[j] = Convert.ToDecimal(T_h[j]);
            }

            textBox45.Text = Convert.ToString(UA);
            textBox44.Text = Convert.ToString(min_DT);
            textBox110.Text = Convert.ToString(NTU);
            textBox109.Text = Convert.ToString(CR);
            
            dataGridView1.Rows[0].Cells[0].Value = decimal.Round(T_cd[0], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[1].Value = decimal.Round(T_cd[1], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[2].Value = decimal.Round(T_cd[2], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[3].Value = decimal.Round(T_cd[3], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[4].Value = decimal.Round(T_cd[4], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[5].Value = decimal.Round(T_cd[5], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[6].Value = decimal.Round(T_cd[6], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[7].Value = decimal.Round(T_cd[7], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[8].Value = decimal.Round(T_cd[8], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[9].Value = decimal.Round(T_cd[9], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[10].Value = decimal.Round(T_cd[10], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[11].Value = decimal.Round(T_cd[11], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[12].Value = decimal.Round(T_cd[12], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[13].Value = decimal.Round(T_cd[13], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[14].Value = decimal.Round(T_cd[14], 1, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[0].Cells[15].Value = decimal.Round(T_cd[15], 1, MidpointRounding.AwayFromZero);

            dataGridView2.Rows[0].Cells[0].Value = decimal.Round(T_hd[0], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[1].Value = decimal.Round(T_hd[1], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[2].Value = decimal.Round(T_hd[2], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[3].Value = decimal.Round(T_hd[3], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[4].Value = decimal.Round(T_hd[4], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[5].Value = decimal.Round(T_hd[5], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[6].Value = decimal.Round(T_hd[6], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[7].Value = decimal.Round(T_hd[7], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[8].Value = decimal.Round(T_hd[8], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[9].Value = decimal.Round(T_hd[9], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[10].Value = decimal.Round(T_hd[10], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[11].Value = decimal.Round(T_hd[11], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[12].Value = decimal.Round(T_hd[12], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[13].Value = decimal.Round(T_hd[13], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[14].Value = decimal.Round(T_hd[14], 1, MidpointRounding.AwayFromZero);
            dataGridView2.Rows[0].Cells[15].Value = decimal.Round(T_hd[15], 1, MidpointRounding.AwayFromZero);

            // Set series chart type
            chart1.Series["Cold_Side"].ChartType = SeriesChartType.Line;
            chart1.Series["Cold_Side"].BorderWidth = 2;
            chart1.Series["Hot_Side"].ChartType = SeriesChartType.Line;
            chart1.Series["Hot_Side"].BorderWidth = 2;

            chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
            chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

            chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(T_cd[15] - 25, 1, MidpointRounding.AwayFromZero));
            chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(T_hd[0] + 25, 1, MidpointRounding.AwayFromZero));

            for (int k = 0; k <= N_sub_hxrs; k++)
            {
                chart1.Series["Cold_Side"].Points.AddY(dataGridView1.Rows[0].Cells[k].Value);
                chart1.Series["Hot_Side"].Points.AddY(dataGridView2.Rows[0].Cells[k].Value);
            }

            //Data Input for PTC SF Design
            Double T_H_in, T_H_out;
            T_H_in = Convert.ToDouble(T_hd[15]);
            textBox26.Text = Convert.ToString(T_H_in - 273.15);
            T_H_out = Convert.ToDouble(T_hd[0]);
            textBox30.Text = Convert.ToString(T_H_out - 273.15);
            textBox25.Text = Convert.ToString(Q_dot);
            textBox43.Text = Convert.ToString(Effectiveness);

            //HX Detail Design
            Double Channel_Heigh;
            Double Channel_Width;
            Double Distance_Between_Channels;
            Double Number_Channels_Heigh_perblock;
            Double Number_Channels_Width_perblock;
            Double Total_Number_Channels;
            Double Heigh_perblock;
            Double Width_perblock;

            Double Flow_cold, Flow_hot;
            Double[] Velocity_h = new Double[N_sub_hxrs];
            Double[] Velocity_c = new Double[N_sub_hxrs];

            Double Hydraulic_Diameter;

            Double[] Reynold_h = new Double[N_sub_hxrs];
            Double[] Reynold_c = new Double[N_sub_hxrs];

            Double[] Darcy_h = new Double[N_sub_hxrs];
            Double[] Darcy_c = new Double[N_sub_hxrs];

            Double[] Nusselt_h = new Double[N_sub_hxrs];
            Double[] Nusselt_c = new Double[N_sub_hxrs];

            Double[] HTC_h = new Double[N_sub_hxrs];
            Double[] HTC_c = new Double[N_sub_hxrs];

            Double[] Length_local = new Double[N_sub_hxrs];
            Double Total_Length;

            Double[] AP_h = new Double[N_sub_hxrs];
            Double[] AP_c = new Double[N_sub_hxrs];
            Double Total_AP_h;
            Double Total_AP_c;

            //1. Average Temperature
            Double[] Tave_h = new Double[N_sub_hxrs];
            Double[] Tave_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Tave_h[contador] = (T_h[contador] + T_h[contador + 1]) / 2;
                Tave_c[contador] = (T_c[contador] + T_c[contador + 1]) / 2;
            }

            //2. Densities
            Double[] Density_h = new Double[N_sub_hxrs];
            Double[] Density_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                if (comboBox1.Text == "Solar Salt")
                {
                    Density_h[contador] = (-0.636 * (Tave_h[contador] - 273.15)) + 2090;

                    if (contador == (N_sub_hxrs - 1))
                    {
                        Density_in = Density_h[0];
                    }
                }

                else if (comboBox1.Text == "Hitec XL")
                {
                    Density_h[contador] = (3e-6 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.8285 * (Tave_h[contador] - 273.15) + 2240.3;

                    if (contador == (N_sub_hxrs - 1))
                    {
                        Density_in = Density_h[0];
                    }
                }

                else if (comboBox1.Text == "Therminol VP1")
                {
                    Density_h[contador] = (-0.0008 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.6364 * (Tave_h[contador] - 273.15) + 1074;
                        //(-2.367e-6 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) + (7.8116e-4 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.90797 * (Tave_h[contador] - 273.15) + 1083.25;

                    if (contador == (N_sub_hxrs - 1))
                    {
                        Density_in = Density_h[0];
                    }
                }

                else if (comboBox1.Text == "Syltherm_800")
                {
                    Density_h[contador] = (-0.0007 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.7166 * (Tave_h[contador] - 273.15) + 946.03;
                    //(-0.0008 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.6364 * (Tave_h[contador] - 273.15) + 1074;

                    if (contador == (N_sub_hxrs - 1))
                    {
                        Density_in = Density_h[0];
                    }
                }

                else if (comboBox1.Text == "Dowtherm_A")
                {
                    Density_h[contador] = (-0.0008 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.6314 * (Tave_h[contador] - 273.15) + 1068.6;
                    //(-0.0008 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.6364 * (Tave_h[contador] - 273.15) + 1074;

                    if (contador == (N_sub_hxrs - 1))
                    {
                        Density_in = Density_h[0];
                    }
                }

                else if (comboBox1.Text == "Therminol_75")
                {
                    Density_h[contador] = (-0.0004 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.596 * (Tave_h[contador] - 273.15) + 1090.4;
                    //(-0.0008 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - 0.6364 * (Tave_h[contador] - 273.15) + 1074;

                    if (contador == (N_sub_hxrs - 1))
                    {
                        Density_in = Density_h[0];
                    }
                }

                else if (comboBox1.Text == "Liquid Sodium")
                {
                    Density_h[contador] = 219 + 275.32 * (1 - ((Tave_h[contador] - 273.15) / 2503.7)) + 511.58 * Math.Pow((1 - ((Tave_h[contador] - 273.15) / 2503.7)), 0.5);

                    if (contador == (N_sub_hxrs - 1))
                    {
                        Density_in = Density_h[0];
                    }
                }

                CorePointer.working_fluid.FindStateWithTP(Tave_c[contador], P_c[contador]);
                Density_c[contador] = CorePointer.working_fluid.Density;
            }

            //3.Viscosities
            Double[] Viscosity_h = new Double[N_sub_hxrs];
            Double[] Viscosity_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                if (comboBox1.Text == "Solar Salt")
                {
                    Viscosity_h[contador] = (-0.000000000147388 * Math.Pow((Tave_h[contador] - 273.15), 3)) + (0.000000228024134 * Math.Pow((Tave_h[contador] - 273.15), 2)) - (0.000119957203979 * (Tave_h[contador] - 273.15)) + 0.022707419662049;
                }

                else if (comboBox1.Text == "Hitec XL")
                {
                    Viscosity_h[contador] = 1000000 * Math.Pow((Tave_h[contador] - 273.15), -3.315);
                }

                else if (comboBox1.Text == "Therminol VP1")
                {
                    Viscosity_h[contador] = (0.0002 * Math.Pow((Tave_h[contador] - 273.15), -1.15)) * Density_h[contador];
                }

                else if (comboBox1.Text == "Syltherm_800")
                {
                    Viscosity_h[contador] = (1.1629 * Math.Pow((Tave_h[contador] - 273.15), -1.361)) * Density_h[contador];
                }

                else if (comboBox1.Text == "Dowtherm_A")
                {
                    Viscosity_h[contador] = (0.2222 * Math.Pow((Tave_h[contador] - 273.15), -1.216)) * Density_h[contador];
                }

                else if (comboBox1.Text == "Therminol_75")
                {
                    Viscosity_h[contador] = (24.252 * Math.Pow((Tave_h[contador] - 273.15), -1.943)) * Density_h[contador];
                }

                else if (comboBox1.Text == "Liquid Sodium")
                {
                    Viscosity_h[contador] = Math.Pow(Math.E, (-6.4406 - 0.3958 * Math.Log(Tave_h[contador] - 273.15) + (556.835 / (Tave_h[contador] - 273.15))));
                }

                CorePointer.working_fluid.FindStateWithTD(Tave_c[contador], Density_c[contador] / CorePointer.wmm);
                Viscosity_c[contador] = (CorePointer.working_fluid.viscosity) / 1000000;

                if (Viscosity_c[contador] <= 0)
                {
                    MessageBox.Show("Cold Stream viscosity lower than ZERO, please review the HX Input Data");

                    goto exit_min_pinchpoint;
                }
            }

            //4.Densities/Viscosities
            Double[] Densities_Viscosity_Ratio_h = new Double[N_sub_hxrs];
            Double[] Densities_Viscosity_Ratio_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Densities_Viscosity_Ratio_h[contador] = Density_h[contador] / Viscosity_h[contador];
                Densities_Viscosity_Ratio_c[contador] = Density_c[contador] / Viscosity_c[contador];
            }

            //5.Thermal Conductivity
            Double[] Thermal_Conductivity_h = new Double[N_sub_hxrs];
            Double[] Thermal_Conductivity_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                if (comboBox1.Text == "Solar Salt")
                {
                    Thermal_Conductivity_h[contador] = (0.0002 * (Tave_h[contador] - 273.15)) + 0.443;
                }

                else if (comboBox1.Text == "Hitec XL")
                {
                    Thermal_Conductivity_h[contador] = 0.519;
                }

                else if (comboBox1.Text == "Therminol VP1")
                {
                    Thermal_Conductivity_h[contador] = -0.0000002 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) - 0.00009 * (Tave_h[contador] - 273.15) + 0.138;
                }

                else if (comboBox1.Text == "Syltherm_800")
                {
                    Thermal_Conductivity_h[contador] = -1e-10 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) - 0.0002 * (Tave_h[contador] - 273.15) + 0.1387;
                }

                else if (comboBox1.Text == "Dowtherm_A")
                {
                    Thermal_Conductivity_h[contador] = -8e-11 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) - 0.0002 * (Tave_h[contador] - 273.15) + 0.1418;
                }

                else if (comboBox1.Text == "Therminol_75")
                {
                    Thermal_Conductivity_h[contador] = -7e-8 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) - 6e-5 * (Tave_h[contador] - 273.15) + 0.1357;
                }

                else if (comboBox1.Text == "Liquid Sodium")
                {
                    Thermal_Conductivity_h[contador] = 124.67 - 0.11381 * (Tave_h[contador] - 273.15) + 5.5226e-5 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) - 1.1842e-8 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15);
                }

                CorePointer.working_fluid.FindStateWithTD(Tave_c[contador], Density_c[contador] / CorePointer.wmm);
                Thermal_Conductivity_c[contador] = CorePointer.working_fluid.thermalconductivity;
            }

            //6.Specific Heat at constant pressure Cp
            Double[] Cp_h = new Double[N_sub_hxrs];
            Double[] Cp_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                if (comboBox1.Text == "Solar Salt")
                {
                    Cp_h[contador] = (0.000172 * (Tave_h[contador] - 273.15)) + 1.4431;
                }

                else if (comboBox1.Text == "Hitec XL")
                {
                    Cp_h[contador] = (-0.0000001 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) - (0.0003 * (Tave_h[contador] - 273.15)) + 1.5355;
                }

                else if (comboBox1.Text == "Therminol VP1")
                {
                    Cp_h[contador] = (0.0000008 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) + (0.0025 * (Tave_h[contador] - 273.15)) + 1.5089;
                }

                else if (comboBox1.Text == "Syltherm_800")
                {
                    Cp_h[contador] = (5e-9 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) + (0.0017 * (Tave_h[contador] - 273.15)) + 1.5756;
                }

                else if (comboBox1.Text == "Dowtherm_A")
                {
                    Cp_h[contador] = (1e-6 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) + (0.0024 * (Tave_h[contador] - 273.15)) + 1.5402;
                }

                else if (comboBox1.Text == "Therminol_75")
                {
                    Cp_h[contador] = (-2e-6 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15)) + (0.0034 * (Tave_h[contador] - 273.15)) + 1.4502;
                }

                else if (comboBox1.Text == "Liquid Sodium")
                {
                    Cp_h[contador] = (1658.2 - 8479e-4 * (Tave_h[contador] - 273.15) + 4454.1e-7 * (Tave_h[contador] - 273.15) * (Tave_h[contador] - 273.15) - 2992.6e3 * Math.Pow((Tave_h[contador] - 273.15), -2)) / 1000;
                }

                CorePointer.working_fluid.FindStateWithTP(Tave_c[contador], P_c[contador]);
                Cp_c[contador] = CorePointer.working_fluid.Cp;
            }

            //7.Prandtl Number: Prandtl = eta * Cpcalc / tcx / wm / 1000
            Double[] Prandtl_h = new Double[N_sub_hxrs];
            Double[] Prandtl_c = new Double[N_sub_hxrs];

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                CorePointer.working_fluid.FindStateWithTP(Tave_h[contador], P_h[contador]);
                Prandtl_h[contador] = (Viscosity_h[contador] * Cp_h[contador] / Thermal_Conductivity_h[contador] / 1000) * 1000000;
                CorePointer.working_fluid.FindStateWithTP(Tave_c[contador], P_c[contador]);
                Prandtl_c[contador] = (Viscosity_c[contador] * Cp_c[contador] / Thermal_Conductivity_c[contador] / CorePointer.wmm / 1000) * 1000000;
            }

            //Initialize the values for channels
            Channel_Heigh = Convert.ToDouble(this.textBox78.Text);
            Channel_Width = Convert.ToDouble(this.textBox77.Text);
            Distance_Between_Channels = Convert.ToDouble(this.textBox71.Text);
            Number_Channels_Width_perblock = Convert.ToDouble(this.textBox76.Text);
            Number_Channels_Heigh_perblock = Convert.ToDouble(this.textBox75.Text);
            Total_Number_Channels = Number_Channels_Heigh_perblock * Number_Channels_Width_perblock;
            Heigh_perblock = Number_Channels_Heigh_perblock * (Channel_Heigh + Channel_Width + Distance_Between_Channels * 2);
            Width_perblock = Number_Channels_Heigh_perblock * (Channel_Heigh + Channel_Width + Distance_Between_Channels * 2);
            textBox73.Text = Convert.ToString(Heigh_perblock);
            textBox74.Text = Convert.ToString(Width_perblock);
            textBox72.Text = Convert.ToString(Total_Number_Channels);

            Double MassFlow_cold_permodule, MassFlow_hot_permodule;
            MassFlow_cold_permodule = Convert.ToDouble(textBox40.Text) / Number_Modules;
            MassFlow_hot_permodule = Convert.ToDouble(textBox39.Text) / Number_Modules;

            Flow_cold = MassFlow_cold_permodule / Total_Number_Channels;
            Flow_hot = MassFlow_hot_permodule / Total_Number_Channels;

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Velocity_c[contador] = Flow_cold / (((3.1416 * Channel_Width * Channel_Width) / 2) * Density_c[contador]);
                Velocity_h[contador] = Flow_hot / (((3.1416 * Channel_Width * Channel_Width) / 2) * Density_h[contador]);
            }

            Hydraulic_Diameter = 4 * (3.1416 * Channel_Width * Channel_Width / 2) / ((Channel_Width + Channel_Width + 3.1416 * Channel_Width));

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Reynold_c[contador] = Densities_Viscosity_Ratio_c[contador] * Velocity_c[contador] * Hydraulic_Diameter;
                Reynold_h[contador] = Densities_Viscosity_Ratio_h[contador] * Velocity_h[contador] * Hydraulic_Diameter;
            }

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Darcy_c[contador] = Math.Pow(((0.79 * Math.Log(Reynold_c[contador])) - 1.64), -2);
                Darcy_h[contador] = Math.Pow(((0.79 * Math.Log(Reynold_h[contador])) - 1.64), -2);
            }

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Nusselt_c[contador] = ((Darcy_c[contador] / 8) * (Reynold_c[contador] - 1000) * Prandtl_c[contador]) / (1 + 12.7 * (Math.Pow((Darcy_c[contador] / 8), 0.5)) * (Math.Pow(Prandtl_c[contador], 0.6666666666666) - 1));
                Nusselt_h[contador] = 0.023 * (Math.Pow(Reynold_h[contador], 0.8)) * (Math.Pow(Prandtl_h[contador], 0.3));
            }

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                HTC_c[contador] = Nusselt_c[contador] * Thermal_Conductivity_c[contador] / Hydraulic_Diameter;
                HTC_h[contador] = Nusselt_h[contador] * Thermal_Conductivity_h[contador] / Hydraulic_Diameter;
            }

            Total_Length = 0;

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                UA_local[contador] = UA_local[contador] / Number_Modules;
            }

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Length_local[contador] = UA_local[contador] * (1 / (HTC_h[contador] / 1000) + 1 / (HTC_c[contador] / 1000)) / (2 * Total_Number_Channels * Channel_Width * 2);
                Total_Length = Total_Length + Length_local[contador];
            }

            textBox70.Text = Convert.ToString(Total_Length);

            Total_AP_c = 0;
            Total_AP_h = 0;

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                AP_c[contador] = (Length_local[contador] * Darcy_c[contador] * Density_c[contador] * Velocity_c[contador] * Velocity_c[contador] / (2 * Hydraulic_Diameter)) / 1000000;
                Total_AP_c = Total_AP_c + AP_c[contador];
                AP_h[contador] = (Length_local[contador] * Darcy_h[contador] * Density_h[contador] * Velocity_h[contador] * Velocity_h[contador] / (2 * Hydraulic_Diameter)) / 1000000;
                Total_AP_h = Total_AP_h + AP_h[contador];
            }

            textBox69.Text = Convert.ToString(Total_AP_c * 10);
            textBox52.Text = Convert.ToString(Total_AP_h * 10);
            HX_Hot_Side_AP = Total_AP_h * 10;

            //Graph Detail-Design Results
            decimal[] HTC_cd = new decimal[N_sub_hxrs];
            decimal[] HTC_hd = new decimal[N_sub_hxrs];

            for (int j = 0; j < N_sub_hxrs; j++)
            {
                HTC_cd[j] = Convert.ToDecimal(HTC_c[j]);
                HTC_hd[j] = Convert.ToDecimal(HTC_h[j]);
            }

            dataGridView4.Rows[0].Cells[0].Value = decimal.Round(HTC_cd[0], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[1].Value = decimal.Round(HTC_cd[1], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[2].Value = decimal.Round(HTC_cd[2], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[3].Value = decimal.Round(HTC_cd[3], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[4].Value = decimal.Round(HTC_cd[4], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[5].Value = decimal.Round(HTC_cd[5], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[6].Value = decimal.Round(HTC_cd[6], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[7].Value = decimal.Round(HTC_cd[7], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[8].Value = decimal.Round(HTC_cd[8], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[9].Value = decimal.Round(HTC_cd[9], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[10].Value = decimal.Round(HTC_cd[10], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[11].Value = decimal.Round(HTC_cd[11], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[12].Value = decimal.Round(HTC_cd[12], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[13].Value = decimal.Round(HTC_cd[13], 1, MidpointRounding.AwayFromZero);
            dataGridView4.Rows[0].Cells[14].Value = decimal.Round(HTC_cd[14], 1, MidpointRounding.AwayFromZero);

            dataGridView3.Rows[0].Cells[0].Value = decimal.Round(HTC_hd[0], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[1].Value = decimal.Round(HTC_hd[1], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[2].Value = decimal.Round(HTC_hd[2], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[3].Value = decimal.Round(HTC_hd[3], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[4].Value = decimal.Round(HTC_hd[4], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[5].Value = decimal.Round(HTC_hd[5], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[6].Value = decimal.Round(HTC_hd[6], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[7].Value = decimal.Round(HTC_hd[7], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[8].Value = decimal.Round(HTC_hd[8], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[9].Value = decimal.Round(HTC_hd[9], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[10].Value = decimal.Round(HTC_hd[10], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[11].Value = decimal.Round(HTC_hd[11], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[12].Value = decimal.Round(HTC_hd[12], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[13].Value = decimal.Round(HTC_hd[13], 1, MidpointRounding.AwayFromZero);
            dataGridView3.Rows[0].Cells[14].Value = decimal.Round(HTC_hd[14], 1, MidpointRounding.AwayFromZero);

            // Set series chart type
            chart2.Series["Cold_Side"].ChartType = SeriesChartType.Line;
            chart2.Series["Cold_Side"].BorderWidth = 2;
            chart2.Series["Hot_Side"].ChartType = SeriesChartType.Line;
            chart2.Series["Hot_Side"].BorderWidth = 2;

            chart2.ChartAreas["ChartArea1"].AxisY.Interval = 100;
            chart2.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

            textBox47.Text = Convert.ToString(HTC_cd[14] - 250);

            chart2.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(textBox47.Text), 1, MidpointRounding.AwayFromZero));
            chart2.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(HTC_hd[14] + 500, 1, MidpointRounding.AwayFromZero));

            for (int k = 0; k < N_sub_hxrs; k++)
            {
                chart2.Series["Cold_Side"].Points.AddY(dataGridView4.Rows[0].Cells[k].Value);
                chart2.Series["Hot_Side"].Points.AddY(dataGridView3.Rows[0].Cells[k].Value);
            }

            //IAM Table Loading
            for (int angles1 = 0; angles1 <= 90; angles1++)
            {
                angles.Add(Convert.ToDouble(angles1));
            }

            Double number_dataGridView6_rows = 0;
            number_dataGridView6_rows = dataGridView6.Rows.Count;

            if (comboBox7.Text == "Thermoflow 21, Novatec Biosol, Fresnel")
            {
                for (int a = 1; a <= number_dataGridView6_rows-2; a++)
                {
                    IAM_longitudinal.Add(Convert.ToDouble(dataGridView6.Rows[a].Cells[1].Value));
                    IAM_transversal.Add(Convert.ToDouble(dataGridView6.Rows[a].Cells[2].Value));
                }
            }

            else if (comboBox7.Text == "Thermoflow 25, Novatec - Superheater (Fresnel)")
            {
                double[] IAM_values_thermoflow25;

                for (int a = 1; a <= number_dataGridView6_rows - 2; a++)
                {
                    IAM_values_thermoflow25 = new double[] { Convert.ToDouble(dataGridView6.Rows[a].Cells[0].Value), Convert.ToDouble(dataGridView6.Rows[a].Cells[1].Value), Convert.ToDouble(dataGridView6.Rows[a].Cells[2].Value) };
                    IAM_thermoflow25.Add(IAM_values_thermoflow25);
                }
            }

            exit_min_pinchpoint:

            return;
        }

        public Double rad(Double angolo)
        {
            Double rad1;
            rad1 = angolo * Math.PI / 180;
            return rad1;
        }

        public Double gradi(Double angolo1)
        {
            Double gradi1;
            gradi1 = angolo1 * 180 / Math.PI;
            return gradi1;
        }


        //Calculate PHX and Solar Field Design
        public void button3_Click(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                CR_Calculated = true;
                textBox109.ReadOnly = true;
                textBox109.BackColor = Color.Yellow;
                textBox39.ReadOnly = false;
            }
            else if (radioButton5.Checked == true)
            {
                CR_Calculated = false;
                textBox109.ReadOnly = false;
                textBox109.BackColor=Color.White;
                CR = Convert.ToDouble(textBox109.Text);
                textBox39.ReadOnly = true;
            }

            //If the solar power plant configuration is Direct sHTF (without PHX)
            //Punterociclo_4(RC without ReHeating)
            if (Punterociclo_4 != null && Punterociclo_4.checkBox3.Checked == false)
            {
                Calculate_PHX();
            }

            else if (Punterociclo_4 == null)
            {
                Calculate_PHX();
            }

            else if (Punterociclo_4 != null && Punterociclo_4.checkBox3.Checked == true)
            {

            }

            //Optics Calculations
            zone = Convert.ToDouble(textBox140.Text);
            MerSD = 15 * (-zone);
            Lon = Convert.ToDouble(textBox141.Text);
            Lat = Convert.ToDouble(textBox137.Text);
            DNI = Convert.ToDouble(textBox142.Text);
            DAY = Convert.ToDouble(textBox139.Text);
            HOUR = Convert.ToDouble(textBox138.Text);
            B = (DAY - 1);
            B = (B * 360) / 365;
            Egiorno = 229.18 * (0.000075 + 0.001868 * Math.Cos(rad(B)) - 0.032077 * Math.Sin(rad(B)) - 0.014615 * Math.Cos(rad(2 * B)) - 0.04089 * Math.Sin(rad(2 * B)));
            Eorario = Egiorno;
            Tsun = (HOUR) + (MerSD + Lon) / 15 + Eorario / 60;
            decl = 23.45 * Math.Sin(rad(360 * (284 + DAY) / 365));
            angorario = (Tsun - 12) * 15;
            angzenit = gradi(Math.Acos(Math.Cos(rad(decl)) * Math.Cos(rad(Lat)) * Math.Cos(rad(angorario)) + Math.Sin(rad(decl)) * Math.Sin(rad(Lat))));
            //anginc = gradi(Math.Acos(Math.Pow(Math.Pow(Math.Cos(rad(angzenit)), 2) + Math.Pow(Math.Cos(rad(decl)), 2) * (Math.Pow(Math.Sin(rad(angorario)), 2)), 0.5)));
            
            alt_solare = 90 - angzenit;
            azimuth = 180 - (gradi(Math.Asin(-Math.Cos(rad(decl)) * Math.Sin(rad(angorario)) / Math.Cos(rad(alt_solare)))));

            anginc_long = gradi(Math.Acos(Math.Pow(1 - (Math.Pow(Math.Cos(rad(azimuth)), 2) * Math.Pow(Math.Cos(rad(alt_solare)), 2)), 0.5)));
            anginc_trans = gradi(Math.Atan(Math.Sin(rad(azimuth))/Math.Tan(rad(alt_solare))));

            textBox64.Text = Convert.ToString(anginc_long);
            textBox143.Text = Convert.ToString(anginc_trans);
            textBox144.Text = Convert.ToString(azimuth);
            textBox145.Text = Convert.ToString(angzenit);
            textBox146.Text = Convert.ToString(alt_solare);

            Double IAM_longitudinal_temp1 = 0; 
            Double IAM_transversal_temp1 = 0;
            Double IAM_longitudinal_temp2 = 0;
            Double IAM_transversal_temp2 = 0;
            Double angle1_temp, angle2_temp;

            for (int loop = 0; loop <= 90; loop++)
            {
                if (angles[loop] > anginc_long)
                {
                    angle1_temp = angles[loop - 1];
                    angle2_temp = angles[loop];

                    if (comboBox7.Text == "Thermoflow 21, Novatec Biosol, Fresnel")
                    {
                        IAM_longitudinal_temp1 = IAM_longitudinal[loop - 1];
                        IAM_longitudinal_temp2 = IAM_longitudinal[loop];
                        IAMLongitudinal = interpMethod(angle1_temp, IAM_longitudinal_temp1, angle2_temp, IAM_longitudinal_temp2, anginc_long);
                        textBox63.Text = Convert.ToString(IAMLongitudinal);
                        goto transversal;
                    }

                    else if (comboBox7.Text == "Thermoflow 25, Novatec - Superheater (Fresnel)")
                    {
                       for (int loop2 = 0; loop2 <= 9; loop2++)
                       {
                           if (Convert.ToDouble(IAM_thermoflow25[loop2].GetValue(0)) > anginc_long)
                           {
                               IAM_longitudinal_temp1 = Convert.ToDouble(IAM_thermoflow25[loop2-1].GetValue(1));
                               IAM_longitudinal_temp2 = Convert.ToDouble(IAM_thermoflow25[loop2].GetValue(1));
                               IAMLongitudinal = interpMethod(Convert.ToDouble(IAM_thermoflow25[loop2 - 1].GetValue(0)), IAM_longitudinal_temp1, Convert.ToDouble(IAM_thermoflow25[loop2].GetValue(0)), IAM_longitudinal_temp2, anginc_long);
                               textBox63.Text = Convert.ToString(IAMLongitudinal);
                               goto transversal;
                           }
                       }                       
                    }
                }
            }

           transversal:

            for (int loop1 = 0; loop1 <= 90; loop1++)
            {
                if (angles[loop1] > anginc_trans)
                {
                    angle1_temp = angles[loop1 - 1];
                    angle2_temp = angles[loop1];

                    if (comboBox7.Text == "Thermoflow 21, Novatec Biosol, Fresnel")
                    {
                        IAM_transversal_temp1 = IAM_transversal[loop1 - 1];
                        IAM_transversal_temp2 = IAM_transversal[loop1];
                        IAMTransversal = interpMethod(angle1_temp, IAM_transversal_temp1, angle2_temp, IAM_transversal_temp2, anginc_trans);
                        textBox32.Text = Convert.ToString(IAMTransversal);
                        goto salida;
                    }

                    else if (comboBox7.Text == "Thermoflow 25, Novatec - Superheater (Fresnel)")
                    {
                        for (int loop2 = 0; loop2 <= 9; loop2++)
                        {
                            if (Convert.ToDouble(IAM_thermoflow25[loop2].GetValue(0)) > anginc_long)
                            {
                                IAM_transversal_temp1 = Convert.ToDouble(IAM_thermoflow25[loop2 - 1].GetValue(2));
                                IAM_transversal_temp2 = Convert.ToDouble(IAM_thermoflow25[loop2].GetValue(2));
                                IAMTransversal = interpMethod(Convert.ToDouble(IAM_thermoflow25[loop2 - 1].GetValue(0)), IAM_transversal_temp1, Convert.ToDouble(IAM_thermoflow25[loop2].GetValue(0)), IAM_transversal_temp2, anginc_trans);
                                textBox32.Text = Convert.ToString(IAMTransversal);
                                goto salida;
                            } 
                        }
                    }
                }
            }

            salida:

            AdmisibleError = 1;

            //Solar Field Design Calculations
            //Copy mass flow from HX to SF
            textBox12.Text = textBox39.Text;

            DNI = Convert.ToDouble(textBox142.Text);
            NominalOpticalEfficiency = Convert.ToDouble(textBox9.Text);
            CleanlinessFactor = Convert.ToDouble(textBox20.Text);
            EndLossFactor = Convert.ToDouble(textBox21.Text);
            //IAMLongitudinal = Convert.ToDouble(textBox23.Text);
            //IAMTransversal = Convert.ToDouble(textBox24.Text);
            CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
            SolarFieldThermalEnergy = Convert.ToDouble(textBox25.Text);
            NumberRows = 50;
            SolarFieldInletTemperature = Convert.ToDouble(textBox26.Text);
            SolarFieldOutputTemperature = Convert.ToDouble(textBox30.Text);
            CoefficientA1 = Convert.ToDouble(textBox27.Text);
            CoefficientA2 = Convert.ToDouble(textBox28.Text);

            Number_Of_Temperatures = Convert.ToInt64(textBox29.Text);
            Receiver_lengths = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            Temperature = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            ThermalLosses = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            PressureDrop = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            rho = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            velocity = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            Reynold_number = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            Dynamic_viscosity = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            Darcy = new Double[Convert.ToInt64(Number_Of_Temperatures)];
            Density_Viscosity = new Double[Convert.ToInt64(Number_Of_Temperatures)];

            Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
            Focal_distance = Convert.ToDouble(textBox131.Text);
            Diameter_Interior = Convert.ToDouble(textBox81.Text) - 2 * (Convert.ToDouble(textBox82.Text));
            CrossArea = Math.PI * Math.Pow(Diameter_Interior / 1000, 2) / 4;

            Actual_Mass_Flux = (m_dot_h / NumberRows) / CrossArea;

            if (Actual_Mass_Flux < Desired_Mass_Flux)
            {
                do
                {
                    if (Actual_Mass_Flux < Desired_Mass_Flux)
                    {
                        NumberRows = NumberRows - 1;
                    }

                    Actual_Mass_Flux = (m_dot_h / NumberRows) / CrossArea;

                } while ((Desired_Mass_Flux - Actual_Mass_Flux) > 1);
            }

            if (Actual_Mass_Flux > Desired_Mass_Flux)
            {
                do
                {
                    if (Actual_Mass_Flux > Desired_Mass_Flux)
                    {
                        NumberRows = NumberRows + 1;
                    }

                    Actual_Mass_Flux = (m_dot_h / NumberRows) / CrossArea;

                } while ((-Desired_Mass_Flux + Actual_Mass_Flux) > 1);
            }

            textBox86.Text = Convert.ToString(Actual_Mass_Flux);
            textBox87.Text = Convert.ToString(NumberRows);

            double contador_bucle = 0;

        begining:

            IAMOverall = IAMLongitudinal * IAMTransversal;
            FACTOR = NominalOpticalEfficiency * CleanlinessFactor * EndLossFactor * IAMOverall;
            SolarFieldTemperatureIncrement = (SolarFieldOutputTemperature - SolarFieldInletTemperature) / (Number_Of_Temperatures - 1);

            //First Step
            temp1 = DNI * FACTOR;

            //Second Step and Third Step
            temp2 = (SolarFieldThermalEnergy * 1000) / temp1;

            //Fourth Step
            temp3 = (temp2 / CollectorApertureWidth) / NumberRows;

            ReflectorApertureArea = temp2;

            // Loop Begin
            do
            {
                ReflectorArea = ReflectorApertureArea;
                ReflectorLength = ReflectorApertureArea / CollectorApertureWidth;
                RowLength = ReflectorLength / NumberRows;
                temp4 = ReflectorApertureArea * FACTOR;

                Temperature[0] = SolarFieldInletTemperature;
                Temperature[1] = Temperature[0] + SolarFieldTemperatureIncrement;
                Temperature[2] = Temperature[1] + SolarFieldTemperatureIncrement;
                Temperature[3] = Temperature[2] + SolarFieldTemperatureIncrement;
                Temperature[4] = Temperature[3] + SolarFieldTemperatureIncrement;
                Temperature[5] = Temperature[4] + SolarFieldTemperatureIncrement;
                Temperature[6] = Temperature[5] + SolarFieldTemperatureIncrement;
                Temperature[7] = Temperature[6] + SolarFieldTemperatureIncrement;
                Temperature[8] = Temperature[7] + SolarFieldTemperatureIncrement;
                Temperature[9] = Temperature[8] + SolarFieldTemperatureIncrement;

                LengthIncrement = RowLength / (Number_Of_Temperatures - 1);

                ThermalLosses[0] = LengthIncrement * (((CoefficientA1 * Temperature[1]) + (CoefficientA2 * Temperature[1] * Temperature[1] * Temperature[1] * Temperature[1])) / 1000);
                ThermalLosses[1] = LengthIncrement * (((CoefficientA1 * Temperature[2]) + (CoefficientA2 * Temperature[2] * Temperature[2] * Temperature[2] * Temperature[2])) / 1000);
                ThermalLosses[2] = LengthIncrement * (((CoefficientA1 * Temperature[3]) + (CoefficientA2 * Temperature[3] * Temperature[3] * Temperature[3] * Temperature[3])) / 1000);
                ThermalLosses[3] = LengthIncrement * (((CoefficientA1 * Temperature[4]) + (CoefficientA2 * Temperature[4] * Temperature[4] * Temperature[4] * Temperature[4])) / 1000);
                ThermalLosses[4] = LengthIncrement * (((CoefficientA1 * Temperature[5]) + (CoefficientA2 * Temperature[5] * Temperature[5] * Temperature[5] * Temperature[5])) / 1000);
                ThermalLosses[5] = LengthIncrement * (((CoefficientA1 * Temperature[6]) + (CoefficientA2 * Temperature[6] * Temperature[6] * Temperature[6] * Temperature[6])) / 1000);
                ThermalLosses[6] = LengthIncrement * (((CoefficientA1 * Temperature[7]) + (CoefficientA2 * Temperature[7] * Temperature[7] * Temperature[7] * Temperature[7])) / 1000);
                ThermalLosses[7] = LengthIncrement * (((CoefficientA1 * Temperature[8]) + (CoefficientA2 * Temperature[8] * Temperature[8] * Temperature[8] * Temperature[8])) / 1000);

                ThermalLossesTotal = ThermalLosses[0] + ThermalLosses[1] + ThermalLosses[2] + ThermalLosses[3] + ThermalLosses[4] + ThermalLosses[5] + ThermalLosses[6] + ThermalLosses[7];
                //ThermalLossesTotal = ThermalLossesTotal * 1.01;

                temp5 = (temp4 * DNI / NumberRows) / 1000;
                temp6 = temp5 - ThermalLossesTotal;
                temp7 = temp6 * NumberRows;
                ERROR = SolarFieldThermalEnergy - temp7;

                ReflectorApertureArea = ReflectorApertureArea + 1;

            } while (ERROR > AdmisibleError);

            //END-LOSSES FACTOR calculation: a=Focal_distance, w=CollectorApertureWidth/2 
            EndLossFactor = 1 - (Math.Tan(rad(anginc_long)) * (Focal_distance / RowLength));

            contador_bucle = contador_bucle + 1;

            if (contador_bucle < 10)
            {
                goto begining;
            }

            else
            {

            }
            textBox21.Text = Convert.ToString(EndLossFactor);

            //SOLAR FIELD PRESSURE DROP CALCULATIONS
            //Reynolds Number calculation
            for (int j = 0; j < 10; j++)
            {
                Caudal_per_row = Convert.ToDouble(textBox39.Text) / NumberRows;

                //Density calculation
                if (comboBox1.Text == "Solar Salt")
                {
                    rho[j] = (-0.636 * (Temperature[j])) + 2090;
                }

                else if (comboBox1.Text == "Hitec XL")
                {
                    rho[j] = (3e-6 * (Temperature[j]) * (Temperature[j])) - 0.8285 * (Temperature[j]) + 2240.3;
                }

                else if (comboBox1.Text == "Therminol VP1")
                {
                    rho[j] = (-0.0008 * (Temperature[j]) * (Temperature[j])) - 0.6364 * (Temperature[j]) + 1074;
                }

                else if (comboBox1.Text == "Syltherm_800")
                {
                    rho[j] = (-0.0007 * (Temperature[j]) * (Temperature[j])) - 0.7166 * (Temperature[j]) + 946.03;
                }

                else if (comboBox1.Text == "Dowtherm_A")
                {
                    rho[j] = (-0.0008 * (Temperature[j]) * (Temperature[j])) - 0.6314 * (Temperature[j]) + 1068.6;
                }

                else if (comboBox1.Text == "Therminol_75")
                {
                    rho[j] = (-0.0004 * (Temperature[j]) * (Temperature[j])) - 0.596 * (Temperature[j]) + 1090.4;
                }

                else if (comboBox1.Text == "Liquid Sodium")
                {
                    rho[j] = 219 + 275.32 * (1 - ((Temperature[j]) / 2503.7)) + 511.58 * Math.Pow((1 - ((Temperature[j]) / 2503.7)), 0.5);
                }

                //Velocity calculation v = Q/(rho x A)
                velocity[j] = Caudal_per_row / (rho[j] * CrossArea);
                
                //Dynamic_Viscosity calculation
                if (comboBox1.Text == "Solar Salt")
                {
                    Dynamic_viscosity[j] = (-0.000000000147388 * Math.Pow((Temperature[j]), 3)) + (0.000000228024134 * Math.Pow((Temperature[j]), 2)) - (0.000119957203979 * (Temperature[j])) + 0.022707419662049;
                }

                else if (comboBox1.Text == "Hitec XL")
                {
                    Dynamic_viscosity[j] = 1000000 * Math.Pow((Temperature[j]), -3.315);
                }

                else if (comboBox1.Text == "Therminol VP1")
                {
                    Dynamic_viscosity[j] = (0.0002 * Math.Pow((Temperature[j]), -1.15)) * rho[j];     
                }

                else if (comboBox1.Text == "Syltherm_800")
                {
                    Dynamic_viscosity[j] = (1.1629 * Math.Pow((Temperature[j]), -1.361));
                }

                else if (comboBox1.Text == "Dowtherm_A")
                {
                    Dynamic_viscosity[j] = (0.2222 * Math.Pow((Temperature[j]), -1.216));
                }

                else if (comboBox1.Text == "Therminol_75")
                {
                    Dynamic_viscosity[j] = (24.252 * Math.Pow((Temperature[j]), -1.943));
                }

                else if (comboBox1.Text == "Liquid Sodium")
                {
                    Dynamic_viscosity[j] = Math.Pow(Math.E, (-6.4406 - 0.3958 * Math.Log(Temperature[j]) + (556.835 / (Temperature[j]))));
                }

                //Density_Viscority
                Density_Viscosity[j] = rho[j] / Dynamic_viscosity[j];

                //Reynold_number
                Reynold_number[j] = Density_Viscosity[j] * velocity[j] * (Diameter_Interior / 1000);

                //Rugosidad
                Rugosidad = Convert.ToDouble(textBox83.Text);

                double to, e1;

                int i = 0;
                Console.Write("Ingresar el valor estimado de x = ");

                Darcy[j] = 0.001;
                do
                {
                    to = Darcy[j];
                    Darcy[j] = Darcy[j] - funcion(Darcy[j], Rugosidad, Diameter_Interior, Reynold_number[j]) / derivada(Darcy[j], Rugosidad, Diameter_Interior, Reynold_number[j]);
                    e1 = Math.Abs((Darcy[j] - to) / Darcy[j]);
                    i = i + 1;

                } while (e1 > 0.00000000001 && i < 100);

                PressureDrop[j] = ((Darcy[j] * (rho[j] / 2) * (Math.Pow(velocity[j], 2) / (Diameter_Interior / 1000))) * LengthIncrement) / 100000;
            }

            Total_Pressure_Drop = 0;
            for (int z = 0; z < 10; z++)
            {
                Total_Pressure_Drop = Total_Pressure_Drop + PressureDrop[z];
            }

            textBox23.Text = Convert.ToString(IAMLongitudinal);
            textBox24.Text = Convert.ToString(IAMTransversal);
            textBox6.Text = Convert.ToString(IAMOverall);

            textBox84.Text = Convert.ToString(Total_Pressure_Drop);
            Solar_Field_AP = Total_Pressure_Drop;

            textBox16.Text = Convert.ToString(ReflectorApertureArea);

            SolarImpinging_path = ((ReflectorApertureArea * DNI*(Convert.ToDouble(textBox99.Text))) / NumberRows) / 1000;
            textBox49.Text = Convert.ToString(SolarImpinging_path);

            SolarEnergyAbsorbed_path = ((ReflectorApertureArea * FACTOR * DNI) / NumberRows) / 1000;
            textBox48.Text = Convert.ToString(SolarEnergyAbsorbed_path);

            Energyloss_path = ThermalLossesTotal;
            textBox50.Text = Convert.ToString(Energyloss_path);

            NetAbsorbed_path = SolarEnergyAbsorbed_path - Energyloss_path;
            textBox51.Text = Convert.ToString(NetAbsorbed_path);

            NeatAbsorbed_Field = NetAbsorbed_path * NumberRows;
            textBox5.Text = Convert.ToString(NeatAbsorbed_Field);

            //Collector_Efficiency, Field_Efficiency, Area_Efficiency
            Collector_Efficiency = (NetAbsorbed_path / SolarImpinging_path) * 100;
            textBox22.Text = Convert.ToString(Collector_Efficiency);

            LF_SF_Effective_Apperture_Area = Convert.ToDouble(textBox16.Text);

            textBox19.Text = Convert.ToString(RowLength);

            //Solar Field Pump Design
            textBox98.Text = Convert.ToString(Density_in / 1000);
            Specific_Gravity = Convert.ToDouble(textBox98.Text);
            Discharge_Pressure = P_h_in;
            Suction_Pressure = (Discharge_Pressure / 100) - (Solar_Field_AP + HX_Hot_Side_AP);
            textBox97.Text = Convert.ToString(Suction_Pressure);
            textBox65.Text = Convert.ToString(Discharge_Pressure / 100);
            Calculated_Head = 10.2 * (((Discharge_Pressure / 100) - Suction_Pressure) / Specific_Gravity);
            textBox66.Text = Convert.ToString(Calculated_Head);
            textBox67.Text = Convert.ToString(m_dot_h);
            Gravity = Convert.ToDouble(textBox68.Text);
            Calculated_Power = m_dot_h * Gravity * Calculated_Head / 1000;
            textBox88.Text = Convert.ToString(Calculated_Power);
            Pump_isoentropic_Efficiency = Convert.ToDouble(textBox89.Text);
            Hydraulic_Pumping_Power = Calculated_Power / (Pump_isoentropic_Efficiency / 100);
            textBox90.Text = Convert.ToString(Hydraulic_Pumping_Power);
            Pump_Mechanical_Efficiency = Convert.ToDouble(textBox91.Text);
            Pump_Shaft_Work = Hydraulic_Pumping_Power / (Pump_Mechanical_Efficiency / 100);
            textBox92.Text = Convert.ToString(Pump_Shaft_Work);
            Motor_Efficiency = Convert.ToDouble(textBox93.Text);
            Electrical_Consumption = Pump_Shaft_Work / (Motor_Efficiency / 100);
            textBox94.Text = Convert.ToString(Electrical_Consumption);
            Name_Plate_Design_Point_Load = Convert.ToDouble(textBox95.Text);
            textBox96.Text = Convert.ToString(Electrical_Consumption * Name_Plate_Design_Point_Load);

            if (this.Brayton_cycle_type_variable == 1)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_1.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_1.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_1.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_1.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_1.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_1.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_1.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 2)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_2.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_2.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_2.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_2.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_2.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_2.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_2.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 3)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_3.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_3.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_3.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_3.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_3.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_3.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_3.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 4)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_4.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_4.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_4.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_4.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_4.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_4.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_4.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 15)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_15.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating1_SF")
                {
                    Punterociclo_15.ReHeating1_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating2_SF")
                {
                    Punterociclo_15.ReHeating2_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_15.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_15.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_15.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_15.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_15.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 5)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_5.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_5.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_5.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_5.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_5.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_5.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_5.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 6)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_6.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_6.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_6.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_6.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_6.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_6.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_6.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 7)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_7.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_7.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_7.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_7.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_7.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_7.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_7.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 8)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_8.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_8.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_8.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_8.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_8.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_8.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_8.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 9)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_9.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_9.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_9.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_9.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_9.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_9.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_9.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 10)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_10.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_10.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_10.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_10.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_10.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_10.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_10.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 11)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_11.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_11.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_11.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_11.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_11.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_11.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_11.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 12)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_12.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_12.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_12.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_12.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_12.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_12.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_12.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 14)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_14.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_14.ReHeating_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_14.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_14.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_14.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_14.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_14.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 19)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_19.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating1_SF")
                {
                    Punterociclo_19.ReHeating1_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "ReHeating2_SF")
                {
                    Punterociclo_19.ReHeating2_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_19.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_19.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_19.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_19.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_19.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            else if (this.Brayton_cycle_type_variable == 21)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_21.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                }            

                else if (this.SF_Type_variable == "Main_SF1_Dual_Loop")
                {
                    Punterociclo_21.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption;
                    Punterociclo_21.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1 = Electrical_Consumption;
                }

                else if (this.SF_Type_variable == "Main_SF2_Dual_Loop")
                {
                    Punterociclo_21.Main_SF_Pump_Electrical_Consumption = Electrical_Consumption + Punterociclo_21.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_1;
                    Punterociclo_21.Dual_Loop_LF_Main_SF_Pump_Motor_Elec_Consump_2 = Electrical_Consumption;
                }
            }

            Name_Plate_Design_Point_Load = Convert.ToDouble(textBox95.Text);
            textBox96.Text = Convert.ToString(Electrical_Consumption * Name_Plate_Design_Point_Load);

            comboBox8_SelectedValueChanged(this, e);
            comboBox9_SelectedValueChanged(this, e);
        }

        //Función para calcular el Coeficiente de Darcy
        public static double funcion(double x, double rugosidad1, double diametro1, double reynold1)
        {
            double a = rugosidad1 / (3.7 * diametro1);
            double b = 2.51 / reynold1;
            return -2 * Math.Log10(a + b / Math.Pow(x, 0.5)) - 1.0 / Math.Pow(x, 0.5);
        }

        public static double derivada(double x, double rugosidad2, double diametro2, double reynold2)
        {
            double a = rugosidad2 / (3.7 * diametro2);
            double b = 2.51 / reynold2;
            return (b / (a * Math.Pow(x, 1.5) + b * x)) + (1.0 / (2 * Math.Pow(x, 1.5)));
        }

        //Interpolation Example for IAM values calculation
        //Double value;
        //value = interpMethod(10, 0.9766, 11, 0.9723, 10.5);
        Double interpMethod(Double x0, Double y0, Double x1, Double y1, Double x)
        {
            return y0 * (x - x1) / (x0 - x1) + y1 * (x - x0) / (x1 - x0);
        }

        //Button OK
        public void button77_Click(object sender, EventArgs e)
        {
            //Storing the PTC_SF_Design results in the Brayton cycle dialog for Final Reporting or Cost Estimation

            //RC_Optimization_withReHeating
            if (this.Brayton_cycle_type_variable == 1)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_1.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_1.LF_ReHeating_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //RC_Design_withReHeating
            else if (this.Brayton_cycle_type_variable == 2)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_2.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_2.LF_ReHeating_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //RC_Optimization_withoutReHeating
            else if (this.Brayton_cycle_type_variable == 3)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_3.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //RC_Design_withoutReHeating
            else if (this.Brayton_cycle_type_variable == 4)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_4.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //RC_Design_withTwoReHeating
            else if (this.Brayton_cycle_type_variable == 15)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_15.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating1_SF")
                {
                    Punterociclo_15.LF_ReHeating1_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating2_SF")
                {
                    Punterociclo_15.LF_ReHeating2_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Optimization_withReHeating
            else if (this.Brayton_cycle_type_variable == 5)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_5.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_5.LF_ReHeating_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Design_withReHeating
            else if (this.Brayton_cycle_type_variable == 6)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_6.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_6.LF_ReHeating_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Optimization_withReHeating
            else if (this.Brayton_cycle_type_variable == 7)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_7.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Design_withReHeating
            else if (this.Brayton_cycle_type_variable == 8)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_8.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Optimization_withReHeating
            else if (this.Brayton_cycle_type_variable == 9)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_9.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_9.LF_ReHeating_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Design_withReHeating
            else if (this.Brayton_cycle_type_variable == 10)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_10.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_10.LF_ReHeating_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Optimization_withReHeating
            else if (this.Brayton_cycle_type_variable == 11)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_11.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //PCRC_Design_withReHeating
            else if (this.Brayton_cycle_type_variable == 12)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_12.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            // PCRCMCI_Design_withReHeating: Brayton_cycle_type_variable = 14
            else if (this.Brayton_cycle_type_variable == 14)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_14.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating_SF")
                {
                    Punterociclo_14.LF_ReHeating_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            //RCMCI_Design_withTwoReHeating
            else if (this.Brayton_cycle_type_variable == 19)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_19.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating1_SF")
                {
                    Punterociclo_19.LF_ReHeating1_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }

                else if (this.SF_Type_variable == "ReHeating2_SF")
                {
                    Punterociclo_19.LF_ReHeating2_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }
            }

            else if (this.Brayton_cycle_type_variable == 21)
            {
                if (this.SF_Type_variable == "Main_SF")
                {
                    Punterociclo_21.LF_Main_SF_Effective_Apperture_Area = LF_SF_Effective_Apperture_Area;
                }              
            }

            this.Dispose();
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            dataGridView6.Rows.Clear();

            if (comboBox7.Text == "Thermoflow 21, Novatec Biosol, Fresnel")
            {

                string[] IAM_values;
                IAM_values = new string[] { "Degress", "Longitudinal", "Transverse" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "0", "1", "1" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "1", "0.999", "0.994" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "2", "0.998", "0.983" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "3", "0.997", "0.973" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "4", "0.995", "0.971" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "5", "0.993", "0.971" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "6", "0.991", "0.977" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "7", "0.988", "0.988" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "8", "0.985", "0.996" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "9", "0.982", "0.992" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "10", "0.978", "0.98" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "11", "0.974", "0.97" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "12", "0.97", "0.967" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "13", "0.965", "0.965" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "14", "0.96", "0.97" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "15", "0.955", "0.981" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "16", "0.949", "0.986" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "17", "0.943", "0.979" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "18", "0.936", "0.965" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "19", "0.929", "0.958" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "20", "0.922", "0.956" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "21", "0.915", "0.955" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "22", "0.907", "0.963" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "23", "0.899", "0.97" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "24", "0.89", "0.967" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "25", "0.88", "0.952" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "26", "0.871", "0.945" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "27", "0.862", "0.943" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "28", "0.852", "0.941" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "29", "0.841", "0.951" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "30", "0.831", "0.951" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "31", "0.819", "0.942" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "32", "0.808", "0.932" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "33", "0.796", "0.928" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "34", "0.783", "0.925" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "35", "0.771", "0.932" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "36", "0.758", "0.93" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "37", "0.744", "0.919" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "38", "0.731", "0.914" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "39", "0.716", "0.91" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "40", "0.702", "0.912" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "41", "0.687", "0.911" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "42", "0.672", "0.904" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "43", "0.656", "0.899" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "44", "0.64", "0.895" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "45", "0.623", "0.893" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "46", "0.606", "0.884" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "47", "0.589", "0.874" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "48", "0.571", "0.864" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "49", "0.553", "0.864" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "50", "0.535", "0.863" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "51", "0.515", "0.85" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "52", "0.496", "0.837" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "53", "0.476", "0.822" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "54", "0.456", "0.807" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "55", "0.435", "0.792" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "56", "0.414", "0.776" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "57", "0.392", "0.759" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "58", "0.37", "0.741" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "59", "0.348", "0.724" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "60", "0.325", "0.705" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "61", "0.303", "0.685" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "62", "0.28", "0.666" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "63", "0.258", "0.645" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "64", "0.236", "0.624" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "65", "0.214", "0.602" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "66", "0.193", "0.58" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "67", "0.173", "0.557" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "68", "0.153", "0.533" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "69", "0.135", "0.51" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "70", "0.117", "0.486" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "71", "0.1", "0.461" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "72", "0.085", "0.437" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "73", "0.071", "0.412" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "74", "0.059", "0.387" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "75", "0.047", "0.362" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "76", "0.037", "0.338" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "77", "0.029", "0.312" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "78", "0.022", "0.287" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "79", "0.016", "0.262" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "80", "0.011", "0.237" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "81", "0.007", "0.212" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "82", "0.004", "0.186" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "83", "0.002", "0.16" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "84", "0", "0.133" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "85", "0", "0.105" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "86", "0", "0.076" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "87", "0", "0.048" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "88", "0", "0.022" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "89", "0", "0.004" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "90", "0", "0" };
                dataGridView6.Rows.Add(IAM_values);
            }

            else if (comboBox7.Text == "Thermoflow 25, Novatec - Superheater (Fresnel)")
            {
                string[] IAM_values;
                double[] IAM_values_thermoflow25;
                IAM_values = new string[] { "Degress", "Longitudinal", "Transverse" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "0", "1", "1" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "10", "0.977", "0.979" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "20", "0.92", "0.959" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "30", "0.825", "0.953" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "40", "0.694", "0.912" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "50", "0.522", "0.858" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "60", "0.312", "0.7" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "70", "0.109", "0.48" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "80", "0.001", "0.232" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "90", "0", "0" };
                dataGridView6.Rows.Add(IAM_values);
            }
        }

        public void Load_ComboBox7()
        {
            dataGridView6.Rows.Clear();

            if (comboBox7.Text == "Thermoflow 21, Novatec Biosol, Fresnel")
            {

                string[] IAM_values;
                IAM_values = new string[] { "Degress", "Longitudinal", "Transverse" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "0", "1", "1" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "1", "0.999", "0.994" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "2", "0.998", "0.983" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "3", "0.997", "0.973" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "4", "0.995", "0.971" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "5", "0.993", "0.971" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "6", "0.991", "0.977" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "7", "0.988", "0.988" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "8", "0.985", "0.996" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "9", "0.982", "0.992" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "10", "0.978", "0.98" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "11", "0.974", "0.97" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "12", "0.97", "0.967" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "13", "0.965", "0.965" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "14", "0.96", "0.97" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "15", "0.955", "0.981" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "16", "0.949", "0.986" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "17", "0.943", "0.979" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "18", "0.936", "0.965" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "19", "0.929", "0.958" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "20", "0.922", "0.956" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "21", "0.915", "0.955" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "22", "0.907", "0.963" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "23", "0.899", "0.97" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "24", "0.89", "0.967" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "25", "0.88", "0.952" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "26", "0.871", "0.945" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "27", "0.862", "0.943" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "28", "0.852", "0.941" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "29", "0.841", "0.951" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "30", "0.831", "0.951" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "31", "0.819", "0.942" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "32", "0.808", "0.932" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "33", "0.796", "0.928" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "34", "0.783", "0.925" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "35", "0.771", "0.932" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "36", "0.758", "0.93" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "37", "0.744", "0.919" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "38", "0.731", "0.914" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "39", "0.716", "0.91" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "40", "0.702", "0.912" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "41", "0.687", "0.911" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "42", "0.672", "0.904" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "43", "0.656", "0.899" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "44", "0.64", "0.895" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "45", "0.623", "0.893" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "46", "0.606", "0.884" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "47", "0.589", "0.874" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "48", "0.571", "0.864" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "49", "0.553", "0.864" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "50", "0.535", "0.863" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "51", "0.515", "0.85" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "52", "0.496", "0.837" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "53", "0.476", "0.822" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "54", "0.456", "0.807" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "55", "0.435", "0.792" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "56", "0.414", "0.776" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "57", "0.392", "0.759" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "58", "0.37", "0.741" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "59", "0.348", "0.724" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "60", "0.325", "0.705" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "61", "0.303", "0.685" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "62", "0.28", "0.666" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "63", "0.258", "0.645" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "64", "0.236", "0.624" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "65", "0.214", "0.602" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "66", "0.193", "0.58" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "67", "0.173", "0.557" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "68", "0.153", "0.533" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "69", "0.135", "0.51" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "70", "0.117", "0.486" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "71", "0.1", "0.461" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "72", "0.085", "0.437" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "73", "0.071", "0.412" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "74", "0.059", "0.387" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "75", "0.047", "0.362" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "76", "0.037", "0.338" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "77", "0.029", "0.312" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "78", "0.022", "0.287" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "79", "0.016", "0.262" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "80", "0.011", "0.237" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "81", "0.007", "0.212" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "82", "0.004", "0.186" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "83", "0.002", "0.16" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "84", "0", "0.133" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "85", "0", "0.105" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "86", "0", "0.076" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "87", "0", "0.048" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "88", "0", "0.022" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "89", "0", "0.004" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "90", "0", "0" };
                dataGridView6.Rows.Add(IAM_values);
            }

            else if (comboBox7.Text == "Thermoflow 25, Novatec - Superheater (Fresnel)")
            {
                string[] IAM_values;
                IAM_values = new string[] { "Degress", "Longitudinal", "Transverse" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "0", "1", "1" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "10", "0.977", "0.979" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "20", "0.92", "0.959" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "30", "0.825", "0.953" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "40", "0.694", "0.912" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "50", "0.522", "0.858" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "60", "0.312", "0.7" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "70", "0.109", "0.48" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "80", "0.001", "0.232" };
                dataGridView6.Rows.Add(IAM_values);

                IAM_values = new string[] { "90", "0", "0" };
                dataGridView6.Rows.Add(IAM_values);
            }
        }

        //HTF Selection change
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Solar Salt")
            {
                textBox46.Text = "1.515";
            }
            if (comboBox1.Text == "Caloria")
            {
                textBox46.Text = "2.77";
            }
            if (comboBox1.Text == "Hitec XL")
            {
                textBox46.Text = "1.375";
            }
            if (comboBox1.Text == "Therminol VP1")
            {
                textBox46.Text = "2.634";
            }
            if (comboBox1.Text == "Syltherm_800")
            {
                textBox46.Text = "2.304";
            }
            if (comboBox1.Text == "Dowtherm_A")
            {
                textBox46.Text = "2.855";
            }
            if (comboBox1.Text == "Therminol_75")
            {
                textBox46.Text = "2.445";
            }
            if (comboBox1.Text == "Hitec")
            {
                textBox46.Text = "1.56";
            }
            if (comboBox1.Text == "Dowtherm Q")
            {
                textBox46.Text = "2.587";
            }
            if (comboBox1.Text == "Dowtherm RP")
            {
                textBox46.Text = "2.602";
            }
        }

        private void comboBox8_SelectedValueChanged(object sender, EventArgs e)
        {
            //Temperatures
            if (comboBox8.Text == "Temperature")
            {
                chart1.Series.Clear();
                chart1.Series.Add("Cold_Side");
                chart1.Series.Add("Hot_Side");

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();

                dataGridView1.Rows[0].Cells[0].Value = decimal.Round(T_cd[0], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[1].Value = decimal.Round(T_cd[1], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[2].Value = decimal.Round(T_cd[2], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[3].Value = decimal.Round(T_cd[3], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[4].Value = decimal.Round(T_cd[4], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[5].Value = decimal.Round(T_cd[5], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[6].Value = decimal.Round(T_cd[6], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[7].Value = decimal.Round(T_cd[7], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[8].Value = decimal.Round(T_cd[8], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[9].Value = decimal.Round(T_cd[9], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[10].Value = decimal.Round(T_cd[10], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[11].Value = decimal.Round(T_cd[11], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[12].Value = decimal.Round(T_cd[12], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[13].Value = decimal.Round(T_cd[13], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[14].Value = decimal.Round(T_cd[14], 1, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[15].Value = decimal.Round(T_cd[15], 1, MidpointRounding.AwayFromZero);

                dataGridView2.Rows[0].Cells[0].Value = decimal.Round(T_hd[0], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[1].Value = decimal.Round(T_hd[1], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[2].Value = decimal.Round(T_hd[2], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[3].Value = decimal.Round(T_hd[3], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[4].Value = decimal.Round(T_hd[4], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[5].Value = decimal.Round(T_hd[5], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[6].Value = decimal.Round(T_hd[6], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[7].Value = decimal.Round(T_hd[7], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[8].Value = decimal.Round(T_hd[8], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[9].Value = decimal.Round(T_hd[9], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[10].Value = decimal.Round(T_hd[10], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[11].Value = decimal.Round(T_hd[11], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[12].Value = decimal.Round(T_hd[12], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[13].Value = decimal.Round(T_hd[13], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[14].Value = decimal.Round(T_hd[14], 1, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[15].Value = decimal.Round(T_hd[15], 1, MidpointRounding.AwayFromZero);

                // Set series chart type
                chart1.Series["Cold_Side"].ChartType = SeriesChartType.Line;
                chart1.Series["Cold_Side"].BorderWidth = 2;
                chart1.Series["Hot_Side"].ChartType = SeriesChartType.Line;
                chart1.Series["Hot_Side"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 16;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(T_cd[15] - 25, 1, MidpointRounding.AwayFromZero));
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(T_hd[0] + 25, 1, MidpointRounding.AwayFromZero));

                for (int k = 0; k <= N_sub_hxrs; k++)
                {
                    chart1.Series["Cold_Side"].Points.AddY(dataGridView1.Rows[0].Cells[k].Value);
                    chart1.Series["Hot_Side"].Points.AddY(dataGridView2.Rows[0].Cells[k].Value);
                }

                label36.Text = "Temperature_Cold_Side (K):";
                label35.Text = "Temperature_Hot_Side (K):";
            }

            //Conductance (UA)
            else if (comboBox8.Text == "Conductance (UA)")
            {
                //UA_local
                chart1.Series.Clear();
                chart1.Series.Add("UA_Local");

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();

                dataGridView1.Rows[0].Cells[0].Value = decimal.Round(Convert.ToDecimal(UA_local_1[0]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[1].Value = decimal.Round(Convert.ToDecimal(UA_local_1[1]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[2].Value = decimal.Round(Convert.ToDecimal(UA_local_1[2]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[3].Value = decimal.Round(Convert.ToDecimal(UA_local_1[3]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[4].Value = decimal.Round(Convert.ToDecimal(UA_local_1[4]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[5].Value = decimal.Round(Convert.ToDecimal(UA_local_1[5]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[6].Value = decimal.Round(Convert.ToDecimal(UA_local_1[6]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[7].Value = decimal.Round(Convert.ToDecimal(UA_local_1[7]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[8].Value = decimal.Round(Convert.ToDecimal(UA_local_1[8]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[9].Value = decimal.Round(Convert.ToDecimal(UA_local_1[9]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[10].Value = decimal.Round(Convert.ToDecimal(UA_local_1[10]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[11].Value = decimal.Round(Convert.ToDecimal(UA_local_1[11]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[12].Value = decimal.Round(Convert.ToDecimal(UA_local_1[12]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[13].Value = decimal.Round(Convert.ToDecimal(UA_local_1[13]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[14].Value = decimal.Round(Convert.ToDecimal(UA_local_1[14]), 2, MidpointRounding.AwayFromZero);

                // Set series chart type
                chart1.Series["UA_Local"].ChartType = SeriesChartType.Line;
                chart1.Series["UA_Local"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 15;

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(0, 1, MidpointRounding.AwayFromZero));
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(UA_local_1[0]) + decimal.Round(Convert.ToDecimal(UA_local_1[N_sub_hxrs - 1] / 4), 1, MidpointRounding.AwayFromZero)));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum) / 10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["UA_Local"].Points.AddXY(k + 1, UA_local_1[k]);
                }

                label36.Text = "UA (kWth/K)";
                label35.Text = " ";
            }

            //Number of Transfer Units (NTU)
            else if (comboBox8.Text == "Nº Transfer Units (NTU)")
            {
                chart1.Series.Clear();
                chart1.Series.Add("NTU_Local");

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();

                dataGridView2.Rows[0].Cells[0].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[0]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[1].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[1]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[2].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[2]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[3].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[3]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[4].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[4]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[5].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[5]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[6].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[6]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[7].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[7]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[8].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[8]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[9].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[9]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[10].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[10]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[11].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[11]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[12].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[12]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[13].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[13]), 2, MidpointRounding.AwayFromZero);
                dataGridView2.Rows[0].Cells[14].Value = decimal.Round(Convert.ToDecimal(NTU_local_1[14]), 2, MidpointRounding.AwayFromZero);

                // Set series chart type
                chart1.Series["NTU_Local"].ChartType = SeriesChartType.Line;
                chart1.Series["NTU_Local"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 15;

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(0, 1, MidpointRounding.AwayFromZero));
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(NTU_local_1[0]) + Convert.ToDecimal(NTU_local_1[N_sub_hxrs - 1] / 2), 1, MidpointRounding.AwayFromZero));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum) / 10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["NTU_Local"].Points.AddXY(k + 1, NTU_local_1[k]);
                }

                label36.Text = "";
                label35.Text = "NTU";
            }

            //Capacity Ratio (CR)
            else if (comboBox8.Text == "Capacity Ratio (CR)")
            {
                chart1.Series.Clear();
                chart1.Series.Add("CR_Local");

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();

                dataGridView1.Rows[0].Cells[0].Value = decimal.Round(Convert.ToDecimal(CR_local_1[0]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[1].Value = decimal.Round(Convert.ToDecimal(CR_local_1[1]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[2].Value = decimal.Round(Convert.ToDecimal(CR_local_1[2]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[3].Value = decimal.Round(Convert.ToDecimal(CR_local_1[3]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[4].Value = decimal.Round(Convert.ToDecimal(CR_local_1[4]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[5].Value = decimal.Round(Convert.ToDecimal(CR_local_1[5]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[6].Value = decimal.Round(Convert.ToDecimal(CR_local_1[6]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[7].Value = decimal.Round(Convert.ToDecimal(CR_local_1[7]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[8].Value = decimal.Round(Convert.ToDecimal(CR_local_1[8]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[9].Value = decimal.Round(Convert.ToDecimal(CR_local_1[9]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[10].Value = decimal.Round(Convert.ToDecimal(CR_local_1[10]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[11].Value = decimal.Round(Convert.ToDecimal(CR_local_1[11]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[12].Value = decimal.Round(Convert.ToDecimal(CR_local_1[12]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[13].Value = decimal.Round(Convert.ToDecimal(CR_local_1[13]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[14].Value = decimal.Round(Convert.ToDecimal(CR_local_1[14]), 2, MidpointRounding.AwayFromZero);

                // Set series chart type
                chart1.Series["CR_Local"].ChartType = SeriesChartType.Line;
                chart1.Series["CR_Local"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 15;

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(0, 1, MidpointRounding.AwayFromZero));
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(CR_local_1[0]) + Convert.ToDecimal(CR_local_1[N_sub_hxrs - 1] / 2), 1, MidpointRounding.AwayFromZero));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum) / 10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["CR_Local"].Points.AddXY(k + 1, CR_local_1[k]);
                }

                label36.Text = "CR";
                label35.Text = "";
            }

            //Effectiveness
            else if (comboBox8.Text == "Effectiveness")
            {
                chart1.Series.Clear();
                chart1.Series.Add("Eff_Local");

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();

                dataGridView1.Rows[0].Cells[0].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[0]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[1].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[1]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[2].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[2]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[3].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[3]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[4].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[4]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[5].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[5]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[6].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[6]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[7].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[7]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[8].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[8]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[9].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[9]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[10].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[10]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[11].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[11]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[12].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[12]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[13].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[13]), 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[14].Value = decimal.Round(Convert.ToDecimal(Effec_local_1[14]), 2, MidpointRounding.AwayFromZero);

                // Set series chart type
                chart1.Series["Eff_Local"].ChartType = SeriesChartType.Line;
                chart1.Series["Eff_Local"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 15;

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(0, 1, MidpointRounding.AwayFromZero));
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Effec_local_1[0]) + Convert.ToDecimal(Effec_local_1[N_sub_hxrs - 1] / 2), 1, MidpointRounding.AwayFromZero));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum) / 10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["Eff_Local"].Points.AddXY(k + 1, Effec_local_1[k]);
                }

                label36.Text = "Effectiveness";
                label35.Text = "";
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                CR_Calculated = true;
                textBox109.ReadOnly = true;
                textBox109.BackColor = Color.Yellow;
            }
            else if (radioButton5.Checked == true)
            {
                CR_Calculated = false;
                textBox109.ReadOnly = false;
                textBox109.BackColor = Color.White;
                textBox39.BackColor = Color.Yellow;
                textBox39.ReadOnly = true;
            }          
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                CR_Calculated = true;
                textBox109.ReadOnly = true;
                textBox109.BackColor = Color.Yellow;
                textBox39.BackColor = Color.White;
                textBox39.ReadOnly = false;
            }
            else if (radioButton5.Checked == true)
            {
                CR_Calculated = false;
                textBox109.ReadOnly = false;
                textBox109.BackColor = Color.White;
            } 
        }

        //OK Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //
        public void comboBox9_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox9.Text == "Temperature")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("HTF_Temperature");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView5.Columns.Add("HTF_T" + Convert.ToString(counter44 + 1), "HTF_T" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Temperature[counter45]), 1, MidpointRounding.AwayFromZero);
                }

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView7.Columns.Add("Length" + Convert.ToString(counter44 + 1), "Length" + Convert.ToString(counter44 + 1));
                    dataGridView7.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {

                    Receiver_lengths[counter45] = LengthIncrement * counter45;
                    dataGridView7.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Receiver_lengths[counter45]), 1, MidpointRounding.AwayFromZero);
                }

                chart3.Series["HTF_Temperature"].ChartType = SeriesChartType.Line;
                chart3.Series["HTF_Temperature"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Length (m)";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Temperature (ºC)";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Receiver_lengths[Number_Of_Temperatures - 1];
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(LengthIncrement), 1, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                double yinterval = (((Temperature[Number_Of_Temperatures - 1]) - (Temperature[0])) / (Number_Of_Temperatures - 1));
                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(yinterval), 1, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Decimal Interval = Convert.ToDecimal(chart3.ChartAreas["ChartArea1"].AxisY.Interval);

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Temperature[0]) - Interval, 2, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures + 1));

                for (int k = 0; k < Number_Of_Temperatures; k++)
                {
                    chart3.Series["HTF_Temperature"].Points.AddXY(Receiver_lengths[k], dataGridView5.Rows[0].Cells[k].Value);
                }
            }

            else if (comboBox9.Text == "Heat Losses")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("Receiver_ThermalLosses");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();

                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures - 2; counter44++)
                {
                    dataGridView5.Columns.Add("Q_Loss" + Convert.ToString(counter44 + 1), "Q_Loss" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures - 2; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(ThermalLosses[counter45]), 1, MidpointRounding.AwayFromZero);
                }

                chart3.Series["Receiver_ThermalLosses"].ChartType = SeriesChartType.Line;
                chart3.Series["Receiver_ThermalLosses"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Number of Segment";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Thermal Losses (kWth)";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Number_Of_Temperatures - 2;

                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToInt64(((ThermalLosses[Number_Of_Temperatures - 3]) - (ThermalLosses[0])) / (Number_Of_Temperatures - 2));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Decimal Interval = Convert.ToDecimal(chart3.ChartAreas["ChartArea1"].AxisY.Interval);

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(ThermalLosses[0]) - Interval, 1, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures));

                for (int k = 0; k < Number_Of_Temperatures - 2; k++)
                {
                    chart3.Series["Receiver_ThermalLosses"].Points.AddXY(k + 1, dataGridView5.Rows[0].Cells[k].Value);
                }
            }

            else if (comboBox9.Text == "Velocities")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("HTF_Velocities");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView5.Columns.Add("HTF_v" + Convert.ToString(counter44 + 1), "HTF_v" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(velocity[counter45]), 3, MidpointRounding.AwayFromZero);
                }

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView7.Columns.Add("Length" + Convert.ToString(counter44 + 1), "Length" + Convert.ToString(counter44 + 1));
                    dataGridView7.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    Receiver_lengths[counter45] = LengthIncrement * counter45;
                    dataGridView7.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Receiver_lengths[counter45]), 2, MidpointRounding.AwayFromZero);
                }

                chart3.Series["HTF_Velocities"].ChartType = SeriesChartType.Line;
                chart3.Series["HTF_Velocities"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Length (m)";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Velocity (m/s)";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Receiver_lengths[Number_Of_Temperatures - 1];
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(LengthIncrement), 2, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Convert.ToDouble(((velocity[Number_Of_Temperatures - 1]) - (velocity[0])) / (Number_Of_Temperatures - 1))), 3, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Decimal Interval = Convert.ToDecimal(chart3.ChartAreas["ChartArea1"].AxisY.Interval);

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(velocity[0]) - Interval, 3, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures + 1));

                for (int k = 0; k < Number_Of_Temperatures; k++)
                {
                    chart3.Series["HTF_Velocities"].Points.AddXY(Receiver_lengths[k], dataGridView5.Rows[0].Cells[k].Value);
                }
            }

            else if (comboBox9.Text == "Pressuer Drop")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("HTF_Pressure_Drop");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures - 1; counter44++)
                {
                    dataGridView5.Columns.Add("HTF_AP" + Convert.ToString(counter44 + 1), "HTF_AP" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures - 1; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(PressureDrop[counter45]), 6, MidpointRounding.AwayFromZero);
                }

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView7.Columns.Add("Length" + Convert.ToString(counter44 + 1), "Length" + Convert.ToString(counter44 + 1));
                    dataGridView7.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    Receiver_lengths[counter45] = LengthIncrement * counter45;
                    dataGridView7.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Receiver_lengths[counter45]), 2, MidpointRounding.AwayFromZero);
                }

                chart3.Series["HTF_Pressure_Drop"].ChartType = SeriesChartType.Line;
                chart3.Series["HTF_Pressure_Drop"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Length (m)";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Pressure Drop (kPa)";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Receiver_lengths[Number_Of_Temperatures - 1];
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(LengthIncrement), 2, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                Double temp = (PressureDrop.Max()) - (PressureDrop.Min());
                Double temp1 = temp / (Number_Of_Temperatures - 1);

                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(temp1), 6, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Double Interval = chart3.ChartAreas["ChartArea1"].AxisY.Interval;

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(PressureDrop.Min() - Interval), 6, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures + 1));

                for (int k = 0; k < Number_Of_Temperatures - 1; k++)
                {
                    chart3.Series["HTF_Pressure_Drop"].Points.AddXY(Receiver_lengths[k + 1], dataGridView5.Rows[0].Cells[k].Value);
                }
            }

            else if (comboBox9.Text == "Densities")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("HTF_Densities");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView5.Columns.Add("HTF_rho" + Convert.ToString(counter44 + 1), "HTF_rho" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(rho[counter45]), 3, MidpointRounding.AwayFromZero);
                }

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView7.Columns.Add("Length" + Convert.ToString(counter44 + 1), "Length" + Convert.ToString(counter44 + 1));
                    dataGridView7.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    Receiver_lengths[counter45] = LengthIncrement * counter45;
                    dataGridView7.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Receiver_lengths[counter45]), 2, MidpointRounding.AwayFromZero);
                }

                chart3.Series["HTF_Densities"].ChartType = SeriesChartType.Line;
                chart3.Series["HTF_Densities"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Length (m)";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Density (kg/m3)";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Receiver_lengths[Number_Of_Temperatures - 1];
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(LengthIncrement), 2, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Convert.ToDouble(((rho[0]) - (rho[Number_Of_Temperatures - 1])) / (Number_Of_Temperatures - 1))), 3, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Decimal Interval = Convert.ToDecimal(chart3.ChartAreas["ChartArea1"].AxisY.Interval);

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(rho[Number_Of_Temperatures - 1]) - Interval, 3, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures + 1));

                for (int k = 0; k < Number_Of_Temperatures; k++)
                {
                    chart3.Series["HTF_Densities"].Points.AddXY(Receiver_lengths[k], dataGridView5.Rows[0].Cells[k].Value);
                }
            }

            else if (comboBox9.Text == "Reynold Number")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("HTF_Reynold_Number");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView5.Columns.Add("HTF_Re" + Convert.ToString(counter44 + 1), "HTF_Re" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Reynold_number[counter45]), 0, MidpointRounding.AwayFromZero);
                }

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView7.Columns.Add("Length" + Convert.ToString(counter44 + 1), "Length" + Convert.ToString(counter44 + 1));
                    dataGridView7.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    Receiver_lengths[counter45] = LengthIncrement * counter45;
                    dataGridView7.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Receiver_lengths[counter45]), 2, MidpointRounding.AwayFromZero);
                }

                chart3.Series["HTF_Reynold_Number"].ChartType = SeriesChartType.Line;
                chart3.Series["HTF_Reynold_Number"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Length (m)";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Reynolds_Number";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Receiver_lengths[Number_Of_Temperatures - 1];
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(LengthIncrement), 2, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Convert.ToDouble(((Reynold_number[Number_Of_Temperatures - 1]) - (Reynold_number[0])) / (Number_Of_Temperatures - 1))), 0, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Decimal Interval = decimal.Round(Convert.ToDecimal(chart3.ChartAreas["ChartArea1"].AxisY.Interval), 0, MidpointRounding.AwayFromZero);

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Reynold_number[0]) - Interval, 0, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures + 1));

                for (int k = 0; k < Number_Of_Temperatures; k++)
                {
                    chart3.Series["HTF_Reynold_Number"].Points.AddXY(Receiver_lengths[k], dataGridView5.Rows[0].Cells[k].Value);
                }
            }

            else if (comboBox9.Text == "Darcy")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("HTF_Darcy");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView5.Columns.Add("HTF_f" + Convert.ToString(counter44 + 1), "HTF_f" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Darcy[counter45]), 5, MidpointRounding.AwayFromZero);
                }

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView7.Columns.Add("Length" + Convert.ToString(counter44 + 1), "Length" + Convert.ToString(counter44 + 1));
                    dataGridView7.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    Receiver_lengths[counter45] = LengthIncrement * counter45;
                    dataGridView7.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Receiver_lengths[counter45]), 2, MidpointRounding.AwayFromZero);
                }

                chart3.Series["HTF_Darcy"].ChartType = SeriesChartType.Line;
                chart3.Series["HTF_Darcy"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Length (m)";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Darcy_Coefficient";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Receiver_lengths[Number_Of_Temperatures - 1];
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(LengthIncrement), 2, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Convert.ToDouble(((Darcy[0]) - (Darcy[Number_Of_Temperatures - 1])) / (Number_Of_Temperatures - 1))), 5, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Decimal Interval = decimal.Round(Convert.ToDecimal(chart3.ChartAreas["ChartArea1"].AxisY.Interval), 0, MidpointRounding.AwayFromZero);

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Darcy[Number_Of_Temperatures - 1]) - Interval, 5, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures + 1));

                for (int k = 0; k < Number_Of_Temperatures; k++)
                {
                    chart3.Series["HTF_Darcy"].Points.AddXY(Receiver_lengths[k], dataGridView5.Rows[0].Cells[k].Value);
                }
            }

            else if (comboBox9.Text == "Dynamic_Viscosity")
            {
                //UA_local
                chart3.Series.Clear();
                chart3.Series.Add("HTF_Dynamic_Viscosity");

                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView7.Rows.Clear();
                dataGridView7.Columns.Clear();

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView5.Columns.Add("HTF_vis" + Convert.ToString(counter44 + 1), "HTF_vis" + Convert.ToString(counter44 + 1));
                    dataGridView5.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    dataGridView5.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Dynamic_viscosity[counter45]), 5, MidpointRounding.AwayFromZero);
                }

                for (int counter44 = 0; counter44 < Number_Of_Temperatures; counter44++)
                {
                    dataGridView7.Columns.Add("Length" + Convert.ToString(counter44 + 1), "Length" + Convert.ToString(counter44 + 1));
                    dataGridView7.Columns[counter44].Width = 60;
                }

                for (int counter45 = 0; counter45 < Number_Of_Temperatures; counter45++)
                {
                    Receiver_lengths[counter45] = LengthIncrement * counter45;
                    dataGridView7.Rows[0].Cells[counter45].Value = decimal.Round(Convert.ToDecimal(Receiver_lengths[counter45]), 2, MidpointRounding.AwayFromZero);
                }

                chart3.Series["HTF_Dynamic_Viscosity"].ChartType = SeriesChartType.Line;
                chart3.Series["HTF_Dynamic_Viscosity"].BorderWidth = 2;

                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Length (m)";
                chart3.ChartAreas["ChartArea1"].AxisY.Title = "Darcy_Coefficient";

                chart3.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart3.ChartAreas["ChartArea1"].AxisX.Maximum = Receiver_lengths[Number_Of_Temperatures - 1];
                chart3.ChartAreas["ChartArea1"].AxisX.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(LengthIncrement), 2, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart3.ChartAreas["ChartArea1"].AxisY.Interval = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Convert.ToDouble(((Dynamic_viscosity[0]) - (Dynamic_viscosity[Number_Of_Temperatures - 1])) / (Number_Of_Temperatures - 1))), 5, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                Decimal Interval = decimal.Round(Convert.ToDecimal(chart3.ChartAreas["ChartArea1"].AxisY.Interval), 0, MidpointRounding.AwayFromZero);

                chart3.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Dynamic_viscosity[Number_Of_Temperatures - 1]) - Interval, 5, MidpointRounding.AwayFromZero));
                chart3.ChartAreas["ChartArea1"].AxisY.Maximum = chart3.ChartAreas["ChartArea1"].AxisY.Minimum + (chart3.ChartAreas["ChartArea1"].AxisY.Interval * (Number_Of_Temperatures + 1));

                for (int k = 0; k < Number_Of_Temperatures; k++)
                {
                    chart3.Series["HTF_Dynamic_Viscosity"].Points.AddXY(Receiver_lengths[k], dataGridView5.Rows[0].Cells[k].Value);
                }
            }
        }
    }
}
