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
    public partial class Generator : Form
    {
        public Double Rows_Count;

        public List<Double> Generator_Load_Rating_list_Air = new List<Double>();
        public List<Double> Generator_Losses_PF_1_list_Air = new List<Double>();
        public List<Double> Generator_Losses_PF_2_list_Air = new List<Double>();
        public List<Double> Generator_Losses_PF_3_list_Air = new List<Double>();

        public List<Double> Generator_Load_Rating_list_H2 = new List<Double>();
        public List<Double> Generator_Losses_PF_1_list_H2 = new List<Double>();
        public List<Double> Generator_Losses_PF_2_list_H2 = new List<Double>();
        public List<Double> Generator_Losses_PF_3_list_H2 = new List<Double>();

        public List<Double> Generator_Load_Rating_list_TEWAC = new List<Double>();
        public List<Double> Generator_Losses_PF_1_list_TEWAC = new List<Double>();
        public List<Double> Generator_Losses_PF_2_list_TEWAC = new List<Double>();
        public List<Double> Generator_Losses_PF_3_list_TEWAC = new List<Double>();

        //Generator calculated performance parameters
        public Double Generator_Name_Plate_Power, Name_Plate_Efficiency;
        public Double Generator_Power_Output, Generator_Shaft_Power;
        public Double Generator_Efficiency;
        public Double Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss;

        public Double Rating_Point_Efficiency;
        public Double Design_Point_Power_Factor;
        public Double Map_Type_selected, Power_Factor_selected;
        public Double Rating_Design_Point_Load;
        public Double Number_Units_per_Station;
        public Double Number_Generator_Poles;
        public Double Shaft_Number;
        public Double Mechanical_Loss_Percentage_Total_Loss;
        public Double Gear_Efficiency;
        public Double Load_RatedLoad;
        public Double Loss_LossAtRating;
       
        //Storing PTC_SF_Design dialog results in the Brayton dialog for Final Reporting or Cost Estimation
        public Double Brayton_cycle_type_variable;
        public String SF_Type_variable;

        public RC_Optimization Punterociclo_1;
        public Recompression_Brayton_Power_Cycle Punterociclo_2;
        public RC_optimal_without_ReHeating Punterociclo_3;
        public RC_without_ReHeating Punterociclo_4;
        public RC_with_Two_ReHeating Punterociclo_15;
        public RC_with_Three_ReHeating Punterociclo_16;

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

        public Generator()
        {
            InitializeComponent();
        }

        // RC_Optimization_WithReHeating: Brayton_cycle_type_variable = 1
        public void RC_Optimization_withReHeating(RC_Optimization Punterociclo1, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_1 = Punterociclo1;
        }

        // RC_Design_WithReHeating: Brayton_cycle_type_variable = 2
        public void RC_Design_withReHeating(Recompression_Brayton_Power_Cycle Punterociclo2, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_2 = Punterociclo2;
        }

        // RC_Optimization_WithoutReHeating: Brayton_cycle_type_variable = 3
        public void RC_Optimization_withoutReHeating(RC_optimal_without_ReHeating Punterociclo3, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_3 = Punterociclo3;
        }

        // RC_Design_WithoutReHeating: Brayton_cycle_type_variable = 4
        public void RC_Design_withoutReHeating(RC_without_ReHeating Punterociclo4, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_4 = Punterociclo4;
        }

        // RC_Design_WithoutReHeating: Brayton_cycle_type_variable = 15
        public void RC_Design_withTwoReHeating(RC_with_Two_ReHeating Punterociclo15, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_15 = Punterociclo15;
        }

        // PCRC_Optimization_WithReHeating: Brayton_cycle_type_variable = 5
        public void PCRC_Optimization_withReHeating(PCRC_with_ReHeating Punterociclo5, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_5 = Punterociclo5;
        }

        // PCRC_Design_WithReHeating: Brayton_cycle_type_variable = 6
        public void PCRC_Design_withReHeating(PCRC Punterociclo6, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_6 = Punterociclo6;
        }

        // PCRC_Optimization_WithoutReHeating: Brayton_cycle_type_variable = 7
        public void PCRC_Optimization_withoutReHeating(PCRC_optimal_withoutReHeating Punterociclo7, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_7 = Punterociclo7;
        }

        // PCRC_Design_WithoutReHeating: Brayton_cycle_type_variable = 8
        public void PCRC_Design_withoutReHeating(PCRC_without_ReHeating Punterociclo8, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_8 = Punterociclo8;
        }

        // PCRC_Optimization_WithReHeating: Brayton_cycle_type_variable = 9
        public void RCMCI_Optimization_withReHeating(RCMCI_optimal Punterociclo9, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_9 = Punterociclo9;
        }

        // PCRC_Design_WithReHeating: Brayton_cycle_type_variable = 10
        public void RCMCI_Design_withReHeating(RCMCI Punterociclo10, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_10 = Punterociclo10;
        }

        // PCRC_Optimization_WithoutReHeating: Brayton_cycle_type_variable = 11
        public void RCMCI_Optimization_withoutReHeating(RCMCI_optimal_without_RH Punterociclo11, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_11 = Punterociclo11;
        }

        // PCRC_Design_WithoutReHeating: Brayton_cycle_type_variable = 12
        public void RCMCI_Design_withoutReHeating(RCMCI_without_ReHeating Punterociclo12, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_12 = Punterociclo12;
        }

        // PCRCMCI_Design_WithReHeating: Brayton_cycle_type_variable = 14
        public void PCRCMCI_Design_withReHeating(PCRCMCI Punterociclo14, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_14 = Punterociclo14;
        }

        // RCMCI_Design_WithoutTwoReHeating: Brayton_cycle_type_variable = 19
        public void RCMCI_Design_withTwoReHeating(RCMCI_with_Two_Reheatings Punterociclo19, Double Brayton_cycle_type)
        {
            Brayton_cycle_type_variable = Brayton_cycle_type;
            Punterociclo_19 = Punterociclo19;
        }

        //Ok Button
        public void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Calculate Generator Losses Button
        public void button2_Click(object sender, EventArgs e)
        {

            //Automatic Method Rating_Point_Efficiency
            if (checkBox1.Checked == false)
            {
                textBox1.Enabled = true;
                textBox1.BackColor = Color.White;
                Rating_Design_Point_Load = Convert.ToDouble(textBox3.Text);
                Rating_Point_Efficiency = Convert.ToDouble(textBox1.Text);
            }

            else if (checkBox1.Checked == true)
            {
                textBox1.Enabled = false;
                textBox1.BackColor = Color.DarkGray;
                Rating_Design_Point_Load = Convert.ToDouble(textBox3.Text);
                Rating_Point_Efficiency = 0.01 * ((Generator_Shaft_Power * Rating_Design_Point_Load) / 1000) + 97.6;
            }

            //Gear Box Efficiency
            if (checkBox2.Checked == true)
            {
                textBox8.Enabled = true;
                textBox8.BackColor = Color.White;
                Gear_Efficiency = Convert.ToDouble(textBox8.Text)/100;
            }

            else if (checkBox2.Checked == false)
            {
                textBox8.Enabled = false;
                textBox8.BackColor = Color.DarkGray;
                Gear_Efficiency = 1.0;
            }

            Design_Point_Power_Factor = Convert.ToDouble(textBox2.Text);
            Number_Units_per_Station = Convert.ToDouble(textBox4.Text);
            Number_Generator_Poles = Convert.ToDouble(textBox5.Text);
            Shaft_Number = Convert.ToDouble(textBox6.Text);
            Mechanical_Loss_Percentage_Total_Loss = Convert.ToDouble(textBox7.Text)/100;
            
            //User-defined
            if (Map_Type_selected == 0)
            { 
            
            }

            //Air Cooled
            else if (Map_Type_selected == 1)
            {
                if (Power_Factor_selected == 0.8)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_Air[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_Air[i - 1], Generator_Losses_PF_1_list_Air[i - 1], Generator_Load_Rating_list_Air[i], Generator_Losses_PF_1_list_Air[i], (1 / Rating_Design_Point_Load));
                        }
                    }
                }

                else if (Power_Factor_selected == 0.9)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_Air[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_Air[i - 1], Generator_Losses_PF_2_list_Air[i - 1], Generator_Load_Rating_list_Air[i], Generator_Losses_PF_2_list_Air[i], (1 / Rating_Design_Point_Load));

                            goto loop_exit;
                        }
                    }
                }

                else if (Power_Factor_selected == 1.0)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_Air[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_Air[i - 1], Generator_Losses_PF_3_list_Air[i - 1], Generator_Load_Rating_list_Air[i], Generator_Losses_PF_3_list_Air[i], (1 / Rating_Design_Point_Load));
                        }
                    }
                }
            }

            //H2 Cooled
            else if (Map_Type_selected == 2)
            {
                if (Power_Factor_selected == 0.8)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_H2[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_H2[i - 1], Generator_Losses_PF_1_list_H2[i - 1], Generator_Load_Rating_list_H2[i], Generator_Losses_PF_1_list_H2[i], (1 / Rating_Design_Point_Load));
                        }
                    }
                }

                else if (Power_Factor_selected == 0.9)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_H2[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_H2[i - 1], Generator_Losses_PF_2_list_H2[i - 1], Generator_Load_Rating_list_H2[i], Generator_Losses_PF_2_list_H2[i], (1 / Rating_Design_Point_Load));

                            goto loop_exit;
                        }
                    }
                }

                else if (Power_Factor_selected == 1.0)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_H2[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_H2[i - 1], Generator_Losses_PF_3_list_H2[i - 1], Generator_Load_Rating_list_H2[i], Generator_Losses_PF_3_list_H2[i], (1 / Rating_Design_Point_Load));
                        }
                    }
                }
            }

            else if (Map_Type_selected == 3)
            {
                if (Power_Factor_selected == 0.8)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_H2[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_H2[i - 1], Generator_Losses_PF_1_list_H2[i - 1], Generator_Load_Rating_list_H2[i], Generator_Losses_PF_1_list_H2[i], (1 / Rating_Design_Point_Load));
                        }
                    }
                }

                //TEWAC Totally enclosed Water Air Cooled
                else if (Power_Factor_selected == 0.9)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_TEWAC[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_TEWAC[i - 1], Generator_Losses_PF_2_list_TEWAC[i - 1], Generator_Load_Rating_list_TEWAC[i], Generator_Losses_PF_2_list_TEWAC[i], (1 / Rating_Design_Point_Load));

                            goto loop_exit;
                        }
                    }
                }

                else if (Power_Factor_selected == 1.0)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        if (Generator_Load_Rating_list_TEWAC[i] > Design_Point_Power_Factor)
                        {
                            Loss_LossAtRating = interpMethod_generator(Generator_Load_Rating_list_TEWAC[i - 1], Generator_Losses_PF_3_list_TEWAC[i - 1], Generator_Load_Rating_list_TEWAC[i], Generator_Losses_PF_3_list_TEWAC[i], (1 / Rating_Design_Point_Load));
                        }
                    }
                }
            }

        loop_exit:

            Generator_Power_Output = Generator_Shaft_Power * (Rating_Point_Efficiency/100) * Gear_Efficiency;
            Generator_Name_Plate_Power = Generator_Shaft_Power * Rating_Design_Point_Load;
            Generator_Total_Loss = Generator_Shaft_Power-Generator_Power_Output;
            Generator_Mechanical_Loss = Generator_Total_Loss*Mechanical_Loss_Percentage_Total_Loss;
            Generator_Electrical_Loss = Generator_Total_Loss-Generator_Mechanical_Loss;

            //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss

            textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                              Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                              Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                              Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                              Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                              Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

            //Storing Generator Dialogue results in the Power cycle dialogue.
            if (this.Brayton_cycle_type_variable == 1)
            {
                Punterociclo_1.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_1.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_1.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_1.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_1.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_1.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_1.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_1.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_1.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 2)
            {
                Punterociclo_2.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_2.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_2.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_2.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_2.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_2.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_2.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_2.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_2.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 3)
            {
                Punterociclo_3.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_3.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_3.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_3.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_3.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_3.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_3.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_3.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_3.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 4)
            {
                Punterociclo_4.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_4.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_4.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_4.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_4.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_4.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_4.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_4.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_4.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 15)
            {
                Punterociclo_15.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_15.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_15.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_15.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_15.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_15.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_15.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_15.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_15.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 5)
            {
                Punterociclo_5.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_5.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_5.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_5.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_5.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_5.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_5.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_5.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_5.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 6)
            {
                Punterociclo_6.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_6.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_6.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_6.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_6.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_6.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_6.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_6.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_6.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 7)
            {
                Punterociclo_7.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_7.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_7.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_7.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_7.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_7.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_7.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_7.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_7.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 8)
            {
                Punterociclo_8.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_8.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_8.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_8.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_8.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_8.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_8.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_8.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_8.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 9)
            {
                Punterociclo_9.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_9.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_9.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_9.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_9.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_9.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_9.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_9.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_9.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 10)
            {
                Punterociclo_10.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_10.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_10.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_10.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_10.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_10.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_10.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_10.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_10.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 11)
            {
                Punterociclo_11.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_11.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_11.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_11.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_11.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_11.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_11.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_11.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_11.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 12)
            {
                Punterociclo_12.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_12.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_12.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_12.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_12.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_12.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_12.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_12.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_12.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 14)
            {
                Punterociclo_14.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_14.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_14.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_14.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_14.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_14.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_14.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_14.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_14.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }

            else if (this.Brayton_cycle_type_variable == 19)
            {
                Punterociclo_19.Generator_Name_Plate_Power = Generator_Name_Plate_Power;
                Punterociclo_19.Generator_Shaft_Power = Generator_Shaft_Power;
                Punterociclo_19.Rating_Point_Efficiency = Rating_Point_Efficiency;
                Punterociclo_19.Gear_Efficiency = Gear_Efficiency;
                Punterociclo_19.Generator_Total_Loss = Generator_Total_Loss;
                Punterociclo_19.Generator_Power_Output = Generator_Power_Output;
                Punterociclo_19.Generator_Electrical_Loss = Generator_Electrical_Loss;
                Punterociclo_19.Generator_Mechanical_Loss = Generator_Mechanical_Loss;
                Punterociclo_19.Rating_Design_Point_Load = Rating_Design_Point_Load;
            }
        }

        //Loading the Generator Lossing Values in the an array
        private void Generator_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;

            chart1.Series.Clear();

            chart1.Series.Add("PF=0.8");
            chart1.Series.Add("PF=0.9");
            chart1.Series.Add("PF=1.0");

            Generator_Load_Rating_list_Air.Add(0.0);
            Generator_Load_Rating_list_Air.Add(0.2);
            Generator_Load_Rating_list_Air.Add(0.4);
            Generator_Load_Rating_list_Air.Add(0.6);
            Generator_Load_Rating_list_Air.Add(0.8);
            Generator_Load_Rating_list_Air.Add(1.0);

            Generator_Load_Rating_list_H2.Add(0.0);
            Generator_Load_Rating_list_H2.Add(0.2);
            Generator_Load_Rating_list_H2.Add(0.4);
            Generator_Load_Rating_list_H2.Add(0.6);
            Generator_Load_Rating_list_H2.Add(0.8);
            Generator_Load_Rating_list_H2.Add(1.0);

            Generator_Load_Rating_list_TEWAC.Add(0.0);
            Generator_Load_Rating_list_TEWAC.Add(0.2);
            Generator_Load_Rating_list_TEWAC.Add(0.4);
            Generator_Load_Rating_list_TEWAC.Add(0.6);
            Generator_Load_Rating_list_TEWAC.Add(0.8);
            Generator_Load_Rating_list_TEWAC.Add(1.0);

            Generator_Losses_PF_1_list_Air.Add(0.6);
            Generator_Losses_PF_1_list_Air.Add(0.62);
            Generator_Losses_PF_1_list_Air.Add(0.681);
            Generator_Losses_PF_1_list_Air.Add(0.782);
            Generator_Losses_PF_1_list_Air.Add(0.924);
            Generator_Losses_PF_1_list_Air.Add(1.106);

            Generator_Losses_PF_1_list_H2.Add(0.4);
            Generator_Losses_PF_1_list_H2.Add(0.43);
            Generator_Losses_PF_1_list_H2.Add(0.521);
            Generator_Losses_PF_1_list_H2.Add(0.673);
            Generator_Losses_PF_1_list_H2.Add(0.886);
            Generator_Losses_PF_1_list_H2.Add(1.159);

            Generator_Losses_PF_1_list_TEWAC.Add(0.5);
            Generator_Losses_PF_1_list_TEWAC.Add(0.525);
            Generator_Losses_PF_1_list_TEWAC.Add(0.601);
            Generator_Losses_PF_1_list_TEWAC.Add(0.728);
            Generator_Losses_PF_1_list_TEWAC.Add(0.905);
            Generator_Losses_PF_1_list_TEWAC.Add(1.132);

            Generator_Losses_PF_2_list_Air.Add(0.6);
            Generator_Losses_PF_2_list_Air.Add(0.616);
            Generator_Losses_PF_2_list_Air.Add(0.664);
            Generator_Losses_PF_2_list_Air.Add(0.744);
            Generator_Losses_PF_2_list_Air.Add(0.856);
            Generator_Losses_PF_2_list_Air.Add(1.0);

            Generator_Losses_PF_2_list_H2.Add(0.4);
            Generator_Losses_PF_2_list_H2.Add(0.424);
            Generator_Losses_PF_2_list_H2.Add(0.496);
            Generator_Losses_PF_2_list_H2.Add(0.616);
            Generator_Losses_PF_2_list_H2.Add(0.784);
            Generator_Losses_PF_2_list_H2.Add(1.0);

            Generator_Losses_PF_2_list_TEWAC.Add(0.5);
            Generator_Losses_PF_2_list_TEWAC.Add(0.52);
            Generator_Losses_PF_2_list_TEWAC.Add(0.58);
            Generator_Losses_PF_2_list_TEWAC.Add(0.68);
            Generator_Losses_PF_2_list_TEWAC.Add(0.82);
            Generator_Losses_PF_2_list_TEWAC.Add(1.0);

            Generator_Losses_PF_3_list_Air.Add(0.6);
            Generator_Losses_PF_3_list_Air.Add(0.613);
            Generator_Losses_PF_3_list_Air.Add(0.652);
            Generator_Losses_PF_3_list_Air.Add(0.717);
            Generator_Losses_PF_3_list_Air.Add(0.807);
            Generator_Losses_PF_3_list_Air.Add(0.924);

            Generator_Losses_PF_3_list_H2.Add(0.4);
            Generator_Losses_PF_3_list_H2.Add(0.419);
            Generator_Losses_PF_3_list_H2.Add(0.478);
            Generator_Losses_PF_3_list_H2.Add(0.575);
            Generator_Losses_PF_3_list_H2.Add(0.711);
            Generator_Losses_PF_3_list_H2.Add(0.886);

            Generator_Losses_PF_3_list_TEWAC.Add(0.5);
            Generator_Losses_PF_3_list_TEWAC.Add(0.516);
            Generator_Losses_PF_3_list_TEWAC.Add(0.565);
            Generator_Losses_PF_3_list_TEWAC.Add(0.646);
            Generator_Losses_PF_3_list_TEWAC.Add(0.759);
            Generator_Losses_PF_3_list_TEWAC.Add(0.905);

            comboBox1_SelectedValueChanged(this, e);
        }

        //Performance Map Button
        private void button3_Click(object sender, EventArgs e)
        {
            TabPage t = tabControl1.TabPages[1];
            tabControl1.SelectedTab = t; //go to tab
        }

        //Interpolation Example for IAM values calculation
        //Double value;
        //value = interpMethod(10, 0.9766, 11, 0.9723, 10.5);
        Double interpMethod_generator(Double x0, Double y0, Double x1, Double y1, Double x)
        {
            return y0 * (x - x1) / (x0 - x1) + y1 * (x - x0) / (x1 - x0);
        }

        //Automatic Method
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                textBox1.Enabled = true;
                textBox1.BackColor = Color.White;
                Rating_Point_Efficiency = Convert.ToDouble(textBox1.Text);
            }

            else if (checkBox1.Checked == true)
            {
                textBox1.Enabled = false;
                textBox1.BackColor = Color.DarkGray;
            }
        }

        //Gear Box Efficiency
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox8.Enabled = true;
                textBox8.BackColor = Color.White;
                Gear_Efficiency = Convert.ToDouble(textBox8.Text) / 100;
            }

            else if (checkBox2.Checked == false)
            {
                textBox8.Enabled = false;
                textBox8.BackColor = Color.DarkGray;
                Gear_Efficiency = 1.0;
            }
        }

        //Selected Button
        private void button4_Click(object sender, EventArgs e)
        {
            //Map Type Selected
            if (radioButton1.Checked == true)
            {
                Map_Type_selected = 0;
            }

            else if (radioButton2.Checked == true)
            {
                Map_Type_selected = 1;
            }

            else if (radioButton3.Checked == true)
            {
                Map_Type_selected = 2;
            }

            else if (radioButton4.Checked == true)
            {
                Map_Type_selected = 3;
            }

            //Power Factor selected (PF)
            if (radioButton1.Checked == true)
            {
                Power_Factor_selected = 0.8;
            }

            else if (radioButton2.Checked == true)
            {
                Power_Factor_selected = 0.9;
            }

            else if (radioButton3.Checked == true)
            {
                Power_Factor_selected = 1.0;
            }

            TabPage t = tabControl1.TabPages[0];
            tabControl1.SelectedTab = t; //go to tab
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //User-defined values
            if (radioButton1.Checked == true)
            {

            }

            //Defaulf Air-Cooled option
            else if (radioButton2.Checked == true)
            {
                if (comboBox1.Text == "PF = 0.8")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[0]), Convert.ToString(Generator_Losses_PF_1_list_Air[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[1]), Convert.ToString(Generator_Losses_PF_1_list_Air[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[2]), Convert.ToString(Generator_Losses_PF_1_list_Air[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[3]), Convert.ToString(Generator_Losses_PF_1_list_Air[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[4]), Convert.ToString(Generator_Losses_PF_1_list_Air[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[5]), Convert.ToString(Generator_Losses_PF_1_list_Air[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                else if (comboBox1.Text == "PF = 0.9")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[0]), Convert.ToString(Generator_Losses_PF_2_list_Air[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[1]), Convert.ToString(Generator_Losses_PF_2_list_Air[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[2]), Convert.ToString(Generator_Losses_PF_2_list_Air[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[3]), Convert.ToString(Generator_Losses_PF_2_list_Air[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[4]), Convert.ToString(Generator_Losses_PF_2_list_Air[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[5]), Convert.ToString(Generator_Losses_PF_2_list_Air[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                else if (comboBox1.Text == "PF = 1.0")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[0]), Convert.ToString(Generator_Losses_PF_3_list_Air[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[1]), Convert.ToString(Generator_Losses_PF_3_list_Air[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[2]), Convert.ToString(Generator_Losses_PF_3_list_Air[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[3]), Convert.ToString(Generator_Losses_PF_3_list_Air[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[4]), Convert.ToString(Generator_Losses_PF_3_list_Air[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_Air[5]), Convert.ToString(Generator_Losses_PF_3_list_Air[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.5;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.3;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_1_list_Air);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_2_list_Air);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_3_list_Air);
            }

            //Defaulf H2-Cooled option
            else if (radioButton3.Checked == true)
            {
                if (comboBox1.Text == "PF = 0.8")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[0]), Convert.ToString(Generator_Losses_PF_1_list_H2[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[1]), Convert.ToString(Generator_Losses_PF_1_list_H2[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[2]), Convert.ToString(Generator_Losses_PF_1_list_H2[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[3]), Convert.ToString(Generator_Losses_PF_1_list_H2[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[4]), Convert.ToString(Generator_Losses_PF_1_list_H2[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[5]), Convert.ToString(Generator_Losses_PF_1_list_H2[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                else if (comboBox1.Text == "PF = 0.9")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[0]), Convert.ToString(Generator_Losses_PF_2_list_H2[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[1]), Convert.ToString(Generator_Losses_PF_2_list_H2[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[2]), Convert.ToString(Generator_Losses_PF_2_list_H2[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[3]), Convert.ToString(Generator_Losses_PF_2_list_H2[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[4]), Convert.ToString(Generator_Losses_PF_2_list_H2[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[5]), Convert.ToString(Generator_Losses_PF_2_list_H2[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                else if (comboBox1.Text == "PF = 1.0")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[0]), Convert.ToString(Generator_Losses_PF_3_list_H2[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[1]), Convert.ToString(Generator_Losses_PF_3_list_H2[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[2]), Convert.ToString(Generator_Losses_PF_3_list_H2[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[3]), Convert.ToString(Generator_Losses_PF_3_list_H2[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[4]), Convert.ToString(Generator_Losses_PF_3_list_H2[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_H2[5]), Convert.ToString(Generator_Losses_PF_3_list_H2[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.3;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_1_list_H2);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_2_list_H2);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_3_list_H2);
            }

            //Default TEWAC
            else if (radioButton4.Checked == true)
            {
                if (comboBox1.Text == "PF = 0.8")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[0]), Convert.ToString(Generator_Losses_PF_1_list_TEWAC[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[1]), Convert.ToString(Generator_Losses_PF_1_list_TEWAC[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[2]), Convert.ToString(Generator_Losses_PF_1_list_TEWAC[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[3]), Convert.ToString(Generator_Losses_PF_1_list_TEWAC[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[4]), Convert.ToString(Generator_Losses_PF_1_list_TEWAC[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[5]), Convert.ToString(Generator_Losses_PF_1_list_TEWAC[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                else if (comboBox1.Text == "PF = 0.9")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[0]), Convert.ToString(Generator_Losses_PF_2_list_TEWAC[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[1]), Convert.ToString(Generator_Losses_PF_2_list_TEWAC[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[2]), Convert.ToString(Generator_Losses_PF_2_list_TEWAC[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[3]), Convert.ToString(Generator_Losses_PF_2_list_TEWAC[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[4]), Convert.ToString(Generator_Losses_PF_2_list_TEWAC[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[5]), Convert.ToString(Generator_Losses_PF_2_list_TEWAC[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                else if (comboBox1.Text == "PF = 1.0")
                {
                    Rows_Count = dataGridView1.Rows.Count;

                    if (Rows_Count > 2)
                    {
                        dataGridView1.Rows.Clear();
                    }

                    string[] row1 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[0]), Convert.ToString(Generator_Losses_PF_3_list_TEWAC[0]) };
                    string[] row2 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[1]), Convert.ToString(Generator_Losses_PF_3_list_TEWAC[1]) };
                    string[] row3 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[2]), Convert.ToString(Generator_Losses_PF_3_list_TEWAC[2]) };
                    string[] row4 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[3]), Convert.ToString(Generator_Losses_PF_3_list_TEWAC[3]) };
                    string[] row5 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[4]), Convert.ToString(Generator_Losses_PF_3_list_TEWAC[4]) };
                    string[] row6 = new string[] { Convert.ToString(Generator_Load_Rating_list_TEWAC[5]), Convert.ToString(Generator_Losses_PF_3_list_TEWAC[5]) };

                    dataGridView1.Rows.Add(row1);
                    dataGridView1.Rows.Add(row2);
                    dataGridView1.Rows.Add(row3);
                    dataGridView1.Rows.Add(row4);
                    dataGridView1.Rows.Add(row5);
                    dataGridView1.Rows.Add(row6);
                }

                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.45;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_1_list_TEWAC);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_2_list_TEWAC);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_3_list_TEWAC);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            chart1.Series.Clear();

            chart1.Series.Add("PF=0.8");
            chart1.Series.Add("PF=0.9");
            chart1.Series.Add("PF=1.0");

            //User-defined values
            if (radioButton1.Checked == true)
            {

            }

            //Defaulf Air-Cooled option
            else if (radioButton2.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.5;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.3;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_1_list_Air);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_2_list_Air);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_3_list_Air);
            }

            //Defaulf H2-Cooled option
            else if (radioButton3.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.3;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_1_list_H2);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_2_list_H2);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_3_list_H2);
            }

            //Default TEWAC
            else if (radioButton4.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.45;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_1_list_TEWAC);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_2_list_TEWAC);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_3_list_TEWAC);
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            chart1.Series.Add("PF=0.8");
            chart1.Series.Add("PF=0.9");
            chart1.Series.Add("PF=1.0");

            //User-defined values
            if (radioButton1.Checked == true)
            {

            }

            //Defaulf Air-Cooled option
            else if (radioButton2.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.5;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.3;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_1_list_Air);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_2_list_Air);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_3_list_Air);
            }

            //Defaulf H2-Cooled option
            else if (radioButton3.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.3;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_1_list_H2);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_2_list_H2);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_3_list_H2);
            }

            //Default TEWAC
            else if (radioButton4.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.45;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_1_list_TEWAC);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_2_list_TEWAC);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_3_list_TEWAC);
            }


        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            chart1.Series.Add("PF=0.8");
            chart1.Series.Add("PF=0.9");
            chart1.Series.Add("PF=1.0");

            //User-defined values
            if (radioButton1.Checked == true)
            {

            }

            //Defaulf Air-Cooled option
            else if (radioButton2.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.5;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.3;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_1_list_Air);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_2_list_Air);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_3_list_Air);
            }

            //Defaulf H2-Cooled option
            else if (radioButton3.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.3;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_1_list_H2);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_2_list_H2);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_3_list_H2);
            }

            //Default TEWAC
            else if (radioButton4.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.45;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_1_list_TEWAC);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_2_list_TEWAC);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_3_list_TEWAC);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            chart1.Series.Add("PF=0.8");
            chart1.Series.Add("PF=0.9");
            chart1.Series.Add("PF=1.0");

            //User-defined values
            if (radioButton1.Checked == true)
            {

            }

            //Defaulf Air-Cooled option
            else if (radioButton2.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.5;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.3;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_1_list_Air);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_2_list_Air);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_Air, Generator_Losses_PF_3_list_Air);
            }

            //Defaulf H2-Cooled option
            else if (radioButton3.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.3;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_1_list_H2);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_2_list_H2);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_H2, Generator_Losses_PF_3_list_H2);
            }

            //Default TEWAC
            else if (radioButton4.Checked == true)
            {
                // Set series chart type
                chart1.Series["PF=0.8"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.8"].BorderWidth = 2;
                chart1.Series["PF=0.9"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=0.9"].BorderWidth = 2;
                chart1.Series["PF=1.0"].ChartType = SeriesChartType.Line;
                chart1.Series["PF=1.0"].BorderWidth = 2;

                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 1.2;

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.1;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0.45;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 1.5;

                chart1.Series["PF=0.8"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_1_list_TEWAC);
                chart1.Series["PF=0.9"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_2_list_TEWAC);
                chart1.Series["PF=1.0"].Points.DataBindXY(Generator_Load_Rating_list_TEWAC, Generator_Losses_PF_3_list_TEWAC);
            }
        }
    }
}
