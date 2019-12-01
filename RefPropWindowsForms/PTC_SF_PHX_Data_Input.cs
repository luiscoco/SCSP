using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RefPropWindowsForms
{
    public partial class PTC_SF_PHX_Data_Input : Form
    { 
        //CIT_RC_WithoutReHeating = 1
        Effec_CIT_withoutReHeating puntero_CIT_RC_Without_ReHeating;
        //TIP_RC_WithoutReHeating = 2
        Effec_TIP_withoutReHeating puntero_TIP_RC_Without_ReHeating;
        //Flow_Fraction_RC_WithoutReHeating = 3
        Effec_Recomp_Fract_withoutReHeating puntero_Recomp_Fract_RC_Without_ReHeating;

        //CIT_RC_WithReHeating = 4
        Effec_CIT puntero_CIT_RC_WithReHeating;
        //TIP_RC_WithReHeating = 5
        Effec_TIP puntero_TIP_RC_WithReHeating;
        //Flow_Fraction_RC_WithReHeating = 6
        Effec_Recomp_Fract puntero_Recomp_Fract_RC_WithReHeating;

        //CIT_PCRC_WithoutReHeating = 7
        Effec_CIT_PCRC_withoutReHeating puntero_CIT_PCRC_Without_ReHeating;
        //TIP_PCRC_WithoutReHeating = 8
        Effec_TIP_PCRC_withoutReHeating puntero_TIP_PCRC_Without_ReHeating;
        //Flow_Fraction_PCRC_WithoutReHeating = 9
        Effec_Recomp_Fract_PCRC_withoutReHeating puntero_Recomp_Fract_PCRC_Without_ReHeating;
        
        //CIT_PCRC_WithReHeating = 10
        Effec_CIT_PCRC_withReHeating puntero_CIT_PCRC_With_ReHeating;
        //TIP_PCRC_WithReHeating = 11
        Effec_TIP_PCRC_withReHeating puntero_TIP_PCRC_With_ReHeating;
        //Flow_Fraction_PCRC_WithReHeating = 12
        Effec_Recomp_Fract_PCRC_withReHeating puntero_Recomp_Fract_PCRC_With_ReHeating;

        //CIT_RCMCI_WithoutReHeating = 13
        Effec_CIT_RCMCI_withoutReHeating puntero_CIT_RCMCI_Without_ReHeating;
        //TIP_RCMCI_WithoutReHeating = 14
        Effec_TIP_RCMCI_withoutReHeating puntero_TIP_RCMCI_Without_ReHeating;
        //Flow_Fraction_PCRC_WithoutReHeating = 15
        Effec_Recomp_Fract_RCMCI_withoutReHeating puntero_Recomp_Fract_RCMCI_Without_ReHeating;

        //CIT_RCMCI_WithReHeating = 16
        Effec_CIT_RCMCI_withReHeating puntero_CIT_RCMCI_With_ReHeating;
        //TIP_RCMCI_WithReHeating = 17
        Effec_TIP_RCMCI_withReHeating puntero_TIP_RCMCI_With_ReHeating;
        //Flow_Fraction_RCMCI_WithReHeating = 18
        Effec_Recomp_Fract_RCMCI_withReHeating puntero_Recomp_Fract_RCMCI_With_ReHeating;

        //TIT_RC_WithoutReHeating = 19
        Effec_TIT puntero_TIT_RC_With_ReHeating;
        //TIT_RC_WithReHeating = 20
        Effec_TIT_withoutReHeating puntero_TIT_RC_WithoutReHeating;
        //TIT_PCRC_WithoutReHeating = 21
        //Effec_CIT_PCRC_withoutReHeating puntero_TIT_PCRC_Without_ReHeating;
        //TIT_PCRC_WithReHeating = 22
        //Effec_CIT_PCRC_withReHeating puntero_TIT_PCRC_With_ReHeating;
        //TIT_RCMCI_WithoutReHeating = 23
        //Effec_CIT_RCMCI_withoutReHeating puntero_TIT_RCMCI_Without_ReHeating;
        //TIT_RCMCI_WithReHeating = 24
        //Effec_CIT_RCMCI_withReHeating puntero_TIT_RCMCI_With_ReHeating;

        public int tipo_sensing_dialogue;
        public string Solar_Field_Type;

        public PTC_SF_PHX_Data_Input()
        {
            InitializeComponent();
        }

        //SF_Type: Main_SF or ReHeating_SF

        //CIT_RC_WithoutReHeating = 1
        public void pointer_function_CIT_RC_WithoutReHeating(Effec_CIT_withoutReHeating puntero, string SF_Type)
        {
            puntero_CIT_RC_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 1;
            Solar_Field_Type = SF_Type;
        }

        //TIP_RC_WithoutReHeating = 2
        public void pointer_function_TIP_RC_WithoutReHeating(Effec_TIP_withoutReHeating puntero, string SF_Type)
        {
            puntero_TIP_RC_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 2;
            Solar_Field_Type = SF_Type;
        }

        //Flow_Fraction_RC_RC_WithoutReHeating = 3
        public void pointer_function_Recomp_Fract_RC_WithoutReHeating(Effec_Recomp_Fract_withoutReHeating puntero, string SF_Type)
        {
            puntero_Recomp_Fract_RC_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 3;
            Solar_Field_Type = SF_Type;
        }

        //CIT_RC_WithoutReHeating = 4
        public void pointer_function_CIT_RC_WithReHeating(Effec_CIT puntero,string SF_Type)
        {
            puntero_CIT_RC_WithReHeating = puntero;
            tipo_sensing_dialogue = 4;
            Solar_Field_Type = SF_Type;
        }

        //TIP_RC_WithoutReHeating = 5
        public void pointer_function_TIP_RC_WithReHeating(Effec_TIP puntero, string SF_Type)
        {
            puntero_TIP_RC_WithReHeating = puntero;
            tipo_sensing_dialogue = 5;
            Solar_Field_Type = SF_Type;
        }
        
        //Flow_Fraction_RC_WithoutReHeating = 6        
        public void pointer_function_Recomp_Fract_RC_WithReHeating(Effec_Recomp_Fract puntero, string SF_Type)
        {
            puntero_Recomp_Fract_RC_WithReHeating = puntero;
            tipo_sensing_dialogue = 6;
            Solar_Field_Type = SF_Type;
        }

        //CIT_PCRC_WithoutReHeating = 7  
        public void pointer_function_CIT_PCRC_Without_ReHeating(Effec_CIT_PCRC_withoutReHeating puntero, string SF_Type)
        {
            puntero_CIT_PCRC_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 7;
            Solar_Field_Type = SF_Type;
        }

        //TIP_PCRC_WithoutReHeating = 8
        public void pointer_function_TIP_PCRC_Without_ReHeating(Effec_TIP_PCRC_withoutReHeating puntero, string SF_Type)
        {
            puntero_TIP_PCRC_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 8;
            Solar_Field_Type = SF_Type;
        }

        //Flow_Fraction_PCRC_WithoutReHeating = 9
        public void pointer_function_Recomp_Fract_PCRC_Without_ReHeating(Effec_Recomp_Fract_PCRC_withoutReHeating puntero, string SF_Type)
        {
            puntero_Recomp_Fract_PCRC_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 9;
            Solar_Field_Type = SF_Type;
        }

        //CIT_PCRC_WithReHeating = 10 
        public void pointer_function_CIT_PCRC_WithReHeating(Effec_CIT_PCRC_withReHeating puntero, string SF_Type)
        {
            puntero_CIT_PCRC_With_ReHeating = puntero;
            tipo_sensing_dialogue = 10;
            Solar_Field_Type = SF_Type;
        }

        //TIP_PCRC_WithReHeating = 11
        public void pointer_function_TIP_PCRC_WithReHeating(Effec_TIP_PCRC_withReHeating puntero, string SF_Type)
        {
            puntero_TIP_PCRC_With_ReHeating = puntero;
            tipo_sensing_dialogue = 11;
            Solar_Field_Type = SF_Type;
        }

        //Flow_Fraction_PCRC_WithReHeating = 12
        public void pointer_function_Recomp_Fract_PCRC_WithReHeating(Effec_Recomp_Fract_PCRC_withReHeating puntero, string SF_Type)
        {
            puntero_Recomp_Fract_PCRC_With_ReHeating = puntero;
            tipo_sensing_dialogue = 12;
            Solar_Field_Type = SF_Type;
        }

        //CIT_RCMCI_WithoutReHeating = 13  
        public void pointer_function_CIT_RCMCI_Without_ReHeating(Effec_CIT_RCMCI_withoutReHeating puntero, string SF_Type)
        {
            puntero_CIT_RCMCI_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 13;
            Solar_Field_Type = SF_Type;
        }

        //TIP_RCMCI_WithoutReHeating = 14
        public void pointer_function_TIP_RCMCI_Without_ReHeating(Effec_TIP_RCMCI_withoutReHeating puntero, string SF_Type)
        {
            puntero_TIP_RCMCI_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 14;
            Solar_Field_Type = SF_Type;
        }

        //Flow_Fraction_RCMCI_WithoutReHeating = 15
        public void pointer_function_Recomp_Fract_RCMCI_Without_ReHeating(Effec_Recomp_Fract_RCMCI_withoutReHeating puntero, string SF_Type)
        {
            puntero_Recomp_Fract_RCMCI_Without_ReHeating = puntero;
            tipo_sensing_dialogue = 15;
            Solar_Field_Type = SF_Type;
        }

        //CIT_RCMCI_WithReHeating = 16 
        public void pointer_function_CIT_RCMCI_WithReHeating(Effec_CIT_RCMCI_withReHeating puntero, string SF_Type)
        {
            puntero_CIT_RCMCI_With_ReHeating = puntero;
            tipo_sensing_dialogue = 16;
            Solar_Field_Type = SF_Type;
        }

        //TIP_RCMCI_WithReHeating = 17
        public void pointer_function_TIP_RCMCI_WithReHeating(Effec_TIP_RCMCI_withReHeating puntero, string SF_Type)
        {
            puntero_TIP_RCMCI_With_ReHeating = puntero;
            tipo_sensing_dialogue = 17;
            Solar_Field_Type = SF_Type;
        }

        //Flow_Fraction_RCMCI_WithReHeating = 18
        public void pointer_function_Recomp_Fract_RCMCI_WithReHeating(Effec_Recomp_Fract_RCMCI_withReHeating puntero, string SF_Type)
        {
            puntero_Recomp_Fract_RCMCI_With_ReHeating = puntero;
            tipo_sensing_dialogue = 18;
            Solar_Field_Type = SF_Type;
        }

        //TIT_RC_Without_ReHeating = 19
        public void pointer_function_TIT_RC_Without_ReHeating(Effec_TIT puntero, string SF_Type)
        {
            puntero_TIT_RC_With_ReHeating = puntero;
            tipo_sensing_dialogue = 19;
            Solar_Field_Type = SF_Type;
        }

        //TIT_RC_Without_ReHeating = 20
        public void pointer_function_TIT_RC_With_ReHeating(Effec_TIT_withoutReHeating puntero, string SF_Type)
        {
            puntero_TIT_RC_WithoutReHeating = puntero;
            tipo_sensing_dialogue = 19;
            Solar_Field_Type = SF_Type;
        }

        //OK Button
        private void button2_Click(object sender, EventArgs e)
        {
            if (Solar_Field_Type == "Main_SF")
            {
                if (tipo_sensing_dialogue == 1)
                {
                    //HX Input Data
                    puntero_CIT_RC_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_RC_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_RC_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_CIT_RC_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_RC_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_RC_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_RC_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //TIP_RC_WithoutReHeating = 2
                else if (tipo_sensing_dialogue == 2)
                {
                    //HX Input Data
                    puntero_TIP_RC_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_RC_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_RC_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIP_RC_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_RC_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_RC_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_RC_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 3)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_RC_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_RC_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //CIT_RC_WithReHeating = 4
                else if (tipo_sensing_dialogue == 4)
                {
                    //HX Input Data
                    puntero_CIT_RC_WithReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_RC_WithReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_RC_WithReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_RC_WithReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_RC_WithReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_CIT_RC_WithReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_RC_WithReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_RC_WithReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_RC_WithReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RC_WithReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 15000;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                }

                //TIP_RC_WithReHeating = 5
                else if (tipo_sensing_dialogue == 5)
                {
                    //HX Input Data
                    puntero_TIP_RC_WithReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_RC_WithReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_RC_WithReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_RC_WithReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_RC_WithReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIP_RC_WithReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_RC_WithReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_RC_WithReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_RC_WithReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RC_WithReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 15000;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                }

                //Flow_Fraction_RC_WithReHeating = 6
                else if (tipo_sensing_dialogue == 6)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_RC_WithReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_RC_WithReHeating.comboBox4.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 15000;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_PHX = 2500;
                    }
                }

                //CIT_PCRC_WithoutReHeating = 7
                else if (tipo_sensing_dialogue == 7)
                {
                    //HX Input Data
                    puntero_CIT_PCRC_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_PCRC_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_PCRC_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_CIT_PCRC_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_PCRC_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //TIP_PCRC_WithoutReHeating = 8
                else if (tipo_sensing_dialogue == 8)
                {
                    //HX Input Data
                    puntero_TIP_PCRC_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_PCRC_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_PCRC_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIP_PCRC_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_PCRC_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //Flow_Fraction_PCRC_WithoutReHeating = 9
                else if (tipo_sensing_dialogue == 9)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //CIT_PCRC_WithReHeating = 10 
                else if (tipo_sensing_dialogue == 10)
                {
                    //HX Input Data
                    puntero_CIT_PCRC_With_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_PCRC_With_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_PCRC_With_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_CIT_PCRC_With_ReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_PCRC_With_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 15000;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //TIP_PCRC_WithReHeating = 11
                else if (tipo_sensing_dialogue == 11)
                {
                    //HX Input Data
                    puntero_TIP_PCRC_With_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_PCRC_With_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_PCRC_With_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIP_PCRC_With_ReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_PCRC_With_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 15000;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //Flow_Fraction_PCRC_WithReHeating = 12
                else if (tipo_sensing_dialogue == 12)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_PCRC_With_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_PCRC_With_ReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 15000;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //CIT_RCMCI_WithoutReHeating = 13  
                else if (tipo_sensing_dialogue == 13)
                {
                    //HX Input Data
                    puntero_CIT_RCMCI_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_CIT_RCMCI_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 14)
                {
                    //HX Input Data
                    puntero_TIP_RCMCI_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIP_RCMCI_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //Flow_Fraction_RCMCI_WithoutReHeating = 15
                else if (tipo_sensing_dialogue == 15)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 15000;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_Without_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //CIT_RCMCI_WithReHeating = 16 
                else if (tipo_sensing_dialogue == 16)
                {
                    //HX Input Data
                    puntero_CIT_RCMCI_With_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_RCMCI_With_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_RCMCI_With_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_CIT_RCMCI_With_ReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_RCMCI_With_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 15000;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                //TIP_RCMCI_WithReHeating = 17
                else if (tipo_sensing_dialogue == 17)
                {
                    //HX Input Data
                    puntero_TIP_RCMCI_With_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_RCMCI_With_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_RCMCI_With_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIP_RCMCI_With_ReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_RCMCI_With_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 15000;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 18)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 15000;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 19)
                {
                    //HX Input Data
                    puntero_TIT_RC_With_ReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIT_RC_With_ReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIT_RC_With_ReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIT_RC_With_ReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIT_RC_With_ReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIT_RC_With_ReHeating.comboBox7.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIT_RC_With_ReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);
                    
                    //Main SF Input Data
                    puntero_TIT_RC_With_ReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIT_RC_With_ReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIT_RC_With_ReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 15000;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 1500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_PHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 20)
                {
                    //HX Input Data
                    puntero_TIT_RC_WithoutReHeating.m_dot_h_PHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIT_RC_WithoutReHeating.N_sub_hxrs_PHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIT_RC_WithoutReHeating.AT_Main_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_HTF = comboBox7.Text;
                    puntero_TIT_RC_WithoutReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIT_RC_WithoutReHeating.Main_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIT_RC_WithoutReHeating.Main_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIT_RC_WithoutReHeating.Main_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIT_RC_WithoutReHeating.Main_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 1500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 15000;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 1500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIT_RC_WithoutReHeating.P_h_in_PHX = 2500;
                        puntero_TIT_RC_WithoutReHeating.P_h_out_PHX = 2500;
                    }
                }
            }

            else if (Solar_Field_Type == "ReHeating_SF")
            {
                if (tipo_sensing_dialogue == 1)
                {


                }

                else if (tipo_sensing_dialogue == 2)
                {


                }

                else if (tipo_sensing_dialogue == 3)
                {


                }

                //CIT_RC_WithReHeating = 4
                else if (tipo_sensing_dialogue == 4)
                {
                    //HX Input Data
                    puntero_CIT_RC_WithReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_RC_WithReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_RC_WithReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_RC_WithReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_RC_WithReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_CIT_RC_WithReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_RC_WithReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 1500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 15000;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 1500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                }

                //TIP_RC_WithReHeating = 5
                else if (tipo_sensing_dialogue == 5)
                {
                    //HX Input Data
                    puntero_TIP_RC_WithReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_RC_WithReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_RC_WithReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_RC_WithReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_RC_WithReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_TIP_RC_WithReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //ReHeating SF Input Data
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_RC_WithReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 1500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 15000;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 1500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                }

                //Flow_Fraction_RC_WithReHeating = 6
                else if (tipo_sensing_dialogue == 6)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_RC_WithReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_RC_WithReHeating.comboBox5.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //ReHeating SF Input Data
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 1500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 15000;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 1500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RC_WithReHeating.P_h_out_RHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 7)
                {


                }

                else if (tipo_sensing_dialogue == 8)
                {


                }

                else if (tipo_sensing_dialogue == 9)
                {


                }

                else if (tipo_sensing_dialogue == 10)
                {
                    //HX Input Data
                    puntero_CIT_PCRC_With_ReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_PCRC_With_ReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_PCRC_With_ReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_CIT_PCRC_With_ReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 15000;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                }

                //TIP_PCRC_WithReHeating = 11
                else if (tipo_sensing_dialogue == 11)
                {
                    //HX Input Data
                    puntero_TIP_PCRC_With_ReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_PCRC_With_ReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_PCRC_With_ReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_TIP_PCRC_With_ReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 15000;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 12)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_PCRC_With_ReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_PCRC_With_ReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 15000;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_PCRC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 13)
                {


                }

                else if (tipo_sensing_dialogue == 14)
                {


                }

                else if (tipo_sensing_dialogue == 15)
                {


                }

                //CIT_RCMCI_WithReHeating = 16 
                else if (tipo_sensing_dialogue == 16)
                {
                    //HX Input Data
                    puntero_CIT_RCMCI_With_ReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_CIT_RCMCI_With_ReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_CIT_RCMCI_With_ReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_CIT_RCMCI_With_ReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 15000;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_CIT_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_CIT_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 17)
                {
                    //HX Input Data
                    puntero_TIP_RCMCI_With_ReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIP_RCMCI_With_ReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIP_RCMCI_With_ReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_TIP_RCMCI_With_ReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 15000;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIP_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIP_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 18)
                {
                    //HX Input Data
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 15000;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_Recomp_Fract_RCMCI_With_ReHeating.P_h_out_RHX = 2500;
                    }
                }

                else if (tipo_sensing_dialogue == 19)
                {
                    //HX Input Data
                    puntero_TIT_RC_With_ReHeating.m_dot_h_RHX = Convert.ToDouble(textBox39.Text);
                    puntero_TIT_RC_With_ReHeating.N_sub_hxrs_RHX = Convert.ToInt64(textBox42.Text);
                    puntero_TIT_RC_With_ReHeating.P_h_in_RHX = Convert.ToDouble(textBox34.Text);
                    puntero_TIT_RC_With_ReHeating.P_h_out_RHX = Convert.ToDouble(textBox33.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = Convert.ToDouble(textBox46.Text);
                    puntero_TIT_RC_With_ReHeating.AT_ReHeating_SF = Convert.ToDouble(textBox107.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_HTF = comboBox7.Text;
                    puntero_TIT_RC_With_ReHeating.comboBox6.Text = comboBox7.Text;

                    //Location and Meteorological conditions
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_zone = Convert.ToDouble(textBox140.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Lon = Convert.ToDouble(textBox141.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Lat = Convert.ToDouble(textBox137.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_DNI = Convert.ToDouble(textBox142.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_DAY = Convert.ToDouble(textBox139.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_HOUR = Convert.ToDouble(textBox138.Text);

                    //Main SF Input Data
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_NominalOpticalEfficiency = Convert.ToDouble(textBox19.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_CleanlinessFactor = Convert.ToDouble(textBox20.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_CollectorApertureWidth = Convert.ToDouble(textBox31.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Focal_length = Convert.ToDouble(textBox131.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_CoefficientA1 = Convert.ToDouble(textBox27.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_CoefficientA2 = Convert.ToDouble(textBox28.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_NumberOfSegments = Convert.ToDouble(textBox29.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Desired_Mass_Flux = Convert.ToDouble(textBox85.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Exterior_Diameter = Convert.ToDouble(textBox81.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Receiver_Thickness = Convert.ToDouble(textBox82.Text);
                    puntero_TIT_RC_With_ReHeating.ReHeating_SF_Rugosidad = Convert.ToDouble(textBox83.Text);

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.53;
                    }
                    if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.77;
                    }
                    if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.375;
                    }
                    if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.634;
                    }
                    if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.304;
                    }
                    if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.855;
                    }
                    if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.445;
                    }
                    if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 1.56;
                    }
                    if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.587;
                    }
                    if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIT_RC_With_ReHeating.ReHeating_SF_Cp_HTF = 2.602;
                    }

                    if (comboBox7.Text == "Solar Salt")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Caloria")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec XL")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 15000;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 15000;
                    }
                    else if (comboBox7.Text == "Therminol VP1")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Syltherm_800")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm_A")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Therminol_75")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Hitec")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 1500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 1500;
                    }
                    else if (comboBox7.Text == "Dowtherm Q")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                    else if (comboBox7.Text == "Dowtherm RP")
                    {
                        puntero_TIT_RC_With_ReHeating.P_h_in_RHX = 2500;
                        puntero_TIT_RC_With_ReHeating.P_h_out_RHX = 2500;
                    }
                }
            }
                      
            this.Dispose();
        }

        //AT variations then varies T_hot_in 
        private void textBox107_TextChanged(object sender, EventArgs e)
        {
            textBox37.Text = Convert.ToString(Convert.ToDouble(textBox107.Text)+Convert.ToDouble(puntero_CIT_RC_WithReHeating.temp26));
        }
    }
}
