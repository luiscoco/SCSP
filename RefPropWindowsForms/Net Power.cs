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
    public partial class Net_Power : Form
    {
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

        public Double specific_work_main_turbine = 0;
        public Double specific_work_reheating_turbine = 0;
        public Double specific_work_reheating1_turbine = 0;
        public Double specific_work_reheating2_turbine = 0;

        public Double specific_work_compressor1 = 0;
        public Double specific_work_compressor2 = 0;
        public Double specific_work_compressor3 = 0;

        public Net_Power()
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

        //OK Button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Calculate Power production and Energy Efficiency
        public void button2_Click(object sender, EventArgs e)
        {
            //Storing Generator Dialogue results in the Power cycle dialogue.
            if (this.Brayton_cycle_type_variable == 1)
            {

                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_1.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_1.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.PHX_Q2), 4, MidpointRounding.AwayFromZero)),"kWth" };
                dataGridView4.Rows.Add(row);

                string[] row1 = new string[] { Convert.ToString("ReHeating Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.RHX_Q2), 4, MidpointRounding.AwayFromZero)),"kWth" };
                dataGridView4.Rows.Add(row1);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.PC_Q2), 4, MidpointRounding.AwayFromZero)),"kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.PHX_Q2 + Punterociclo_1.RHX_Q2), 4, MidpointRounding.AwayFromZero)),"kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_1.Generator_Power_Output) / (Punterociclo_1.PHX_Q2 + Punterociclo_1.RHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double ReHeating_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_1.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_1.massflow2;

                string[] row7 = new string[] { Convert.ToString("ReHeating Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating_turbine * Punterociclo_1.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row7);
                ReHeating_Turbine_Power = specific_work_reheating_turbine * Punterociclo_1.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_1.massflow2 * (1-Punterociclo_1.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_1.massflow2 * (1 - Punterociclo_1.recomp_frac_guess2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_1.massflow2 * Punterociclo_1.recomp_frac_guess2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_1.massflow2 * Punterociclo_1.recomp_frac_guess2;

                Total_Turbomachines_Power = Main_Turbine_Power + ReHeating_Turbine_Power + Compressor1_Power + Compressor2_Power;
                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_1.Rating_Point_Efficiency/100)*Punterociclo_1.Gear_Efficiency)*100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_1.Rating_Point_Efficiency / 100) * Punterociclo_1.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row14 = new string[] { Convert.ToString("ReHeating SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.ReHeating_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row14);

                Punterociclo_1.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_1.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_1.Generator_Power_Output * (Punterociclo_1.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.Generator_Power_Output * (Punterociclo_1.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));
                
                Punterociclo_1.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_1.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_1.Total_Auxiliaries = Punterociclo_1.ACHE_fans + Punterociclo_1.Main_SF_Pump_Electrical_Consumption + Punterociclo_1.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_1.Generator_Power_Output * (Punterociclo_1.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_1.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_1.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_1.Rating_Point_Efficiency / 100) * Punterociclo_1.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_1.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_1.Rating_Point_Efficiency / 100) * Punterociclo_1.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_1.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row21 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_1.Rating_Point_Efficiency / 100) * Punterociclo_1.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_1.Total_Auxiliaries) / (Punterociclo_1.PHX_Q2 + Punterociclo_1.RHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row21);
            }

            else if (this.Brayton_cycle_type_variable == 2)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_2.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_2.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.PHX_Q2), 4, MidpointRounding.AwayFromZero)),"kWth" };
                dataGridView4.Rows.Add(row);

                string[] row1 = new string[] { Convert.ToString("ReHeating Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.RHX_Q2), 4, MidpointRounding.AwayFromZero)),"kWth" };
                dataGridView4.Rows.Add(row1);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.PC_Q2), 4, MidpointRounding.AwayFromZero)),"kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.PHX_Q2 + Punterociclo_2.RHX_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_2.Generator_Power_Output) / (Punterociclo_2.PHX_Q2 + Punterociclo_2.RHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double ReHeating_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine*Punterociclo_2.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_2.massflow2;

                string[] row7 = new string[] { Convert.ToString("ReHeating Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating_turbine * Punterociclo_2.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row7);
                ReHeating_Turbine_Power = specific_work_reheating_turbine * Punterociclo_2.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_2.massflow2 * (1-Punterociclo_2.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_2.massflow2 * (1 - Punterociclo_2.recomp_frac2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_2.massflow2 * Punterociclo_2.recomp_frac2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_2.massflow2 * Punterociclo_2.recomp_frac2;

                Total_Turbomachines_Power = Main_Turbine_Power + ReHeating_Turbine_Power + Compressor1_Power + Compressor2_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_2.Rating_Point_Efficiency / 100) * Punterociclo_2.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_2.Rating_Point_Efficiency / 100) * Punterociclo_2.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row14 = new string[] { Convert.ToString("ReHeating SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.ReHeating_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row14);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_2.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_2.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_2.Generator_Power_Output * (Punterociclo_2.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.Generator_Power_Output * (Punterociclo_2.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_2.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_2.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_2.Total_Auxiliaries = Punterociclo_2.ACHE_fans + Punterociclo_2.Main_SF_Pump_Electrical_Consumption + Punterociclo_2.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_2.Generator_Power_Output * (Punterociclo_2.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_2.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_2.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_2.Rating_Point_Efficiency / 100) * Punterociclo_2.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_2.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_2.Rating_Point_Efficiency / 100) * Punterociclo_2.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_2.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row21 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_2.Rating_Point_Efficiency / 100) * Punterociclo_2.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_2.Total_Auxiliaries) / (Punterociclo_2.PHX_Q2 + Punterociclo_2.RHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row21);
            }

            else if (this.Brayton_cycle_type_variable == 3)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_3.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_3.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);
                
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.PHX_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.PC_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.PHX_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_3.Generator_Power_Output) / (Punterociclo_3.PHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_3.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_3.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_3.massflow2 * (1 - Punterociclo_3.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_3.massflow2 * (1 - Punterociclo_3.recomp_frac_guess2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_3.massflow2 * Punterociclo_3.recomp_frac_guess2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_3.massflow2 * Punterociclo_3.recomp_frac_guess2;

                Total_Turbomachines_Power = Main_Turbine_Power + Compressor1_Power + Compressor2_Power;
                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_3.Rating_Point_Efficiency / 100) * Punterociclo_3.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_3.Rating_Point_Efficiency / 100) * Punterociclo_3.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_3.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_3.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_3.Generator_Power_Output * (Punterociclo_3.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.Generator_Power_Output * (Punterociclo_3.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_3.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_3.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_3.Total_Auxiliaries = Punterociclo_3.ACHE_fans + Punterociclo_3.Main_SF_Pump_Electrical_Consumption + Punterociclo_3.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_3.Generator_Power_Output * (Punterociclo_3.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_3.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_3.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_3.Rating_Point_Efficiency / 100) * Punterociclo_3.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_3.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_3.Rating_Point_Efficiency / 100) * Punterociclo_3.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_3.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row21 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_3.Rating_Point_Efficiency / 100) * Punterociclo_3.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_3.Total_Auxiliaries) / (Punterociclo_3.PHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row21);
            }

            else if (this.Brayton_cycle_type_variable == 4)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_4.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_4.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.PHX_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.PC_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.PHX_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_4.Generator_Power_Output) / (Punterociclo_4.PHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_4.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_4.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_4.massflow2 * (1 - Punterociclo_4.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_4.massflow2 * (1 - Punterociclo_4.recomp_frac2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_4.massflow2 * Punterociclo_4.recomp_frac2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_4.massflow2 * Punterociclo_4.recomp_frac2;

                Total_Turbomachines_Power = Main_Turbine_Power + Compressor1_Power + Compressor2_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_4.Rating_Point_Efficiency / 100) * Punterociclo_4.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_4.Rating_Point_Efficiency / 100) * Punterociclo_4.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_4.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_4.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_4.Generator_Power_Output * (Punterociclo_4.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.Generator_Power_Output * (Punterociclo_4.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_4.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_4.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_4.Total_Auxiliaries = Punterociclo_4.ACHE_fans + Punterociclo_4.Main_SF_Pump_Electrical_Consumption + Punterociclo_4.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_4.Generator_Power_Output * (Punterociclo_4.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_4.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_4.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_4.Rating_Point_Efficiency / 100) * Punterociclo_4.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_4.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_4.Rating_Point_Efficiency / 100) * Punterociclo_4.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_4.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row21 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_4.Rating_Point_Efficiency / 100) * Punterociclo_4.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_4.Total_Auxiliaries) / (Punterociclo_4.PHX_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row21);
            }

            else if (this.Brayton_cycle_type_variable == 15)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_15.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_15.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.PHX_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row11 = new string[] { Convert.ToString("ReHeating1 Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.RHX1_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("ReHeating2 Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.RHX2_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row12);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.PC_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.PHX_Q2 + Punterociclo_15.RHX1_Q2 + Punterociclo_15.RHX2_Q2), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_15.Generator_Power_Output) / (Punterociclo_15.PHX_Q2 + Punterociclo_15.RHX1_Q2 + Punterociclo_15.RHX2_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double ReHeating1_Turbine_Power = 0;
                Double ReHeating2_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_15.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_15.massflow2;

                //Reheating1 turbine Power
                string[] row71 = new string[] { Convert.ToString("ReHeating1 Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating1_turbine * Punterociclo_15.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row71);
                ReHeating1_Turbine_Power = specific_work_reheating1_turbine * Punterociclo_15.massflow2;

                //Reheating2 turbine power
                string[] row72 = new string[] { Convert.ToString("ReHeating2 Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating2_turbine * Punterociclo_15.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row72);
                ReHeating2_Turbine_Power = specific_work_reheating2_turbine * Punterociclo_15.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_15.massflow2 * (1 - Punterociclo_15.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_15.massflow2 * (1 - Punterociclo_15.recomp_frac2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_15.massflow2 * Punterociclo_15.recomp_frac2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_15.massflow2 * Punterociclo_15.recomp_frac2;

                Total_Turbomachines_Power = Main_Turbine_Power + ReHeating1_Turbine_Power + ReHeating2_Turbine_Power + Compressor1_Power + Compressor2_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row111 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_15.Rating_Point_Efficiency / 100) * Punterociclo_15.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row111);

                string[] row112 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_15.Rating_Point_Efficiency / 100) * Punterociclo_15.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row112);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row141 = new string[] { Convert.ToString("ReHeating1 SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.ReHeating1_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row141);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.ReHeating1_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row142 = new string[] { Convert.ToString("ReHeating2 SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.ReHeating2_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row142);

                textBox4.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.ReHeating2_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_15.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_15.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_15.Generator_Power_Output * (Punterociclo_15.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.Generator_Power_Output * (Punterociclo_15.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                Punterociclo_15.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_15.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_15.Total_Auxiliaries = Punterociclo_15.ACHE_fans + Punterociclo_15.Main_SF_Pump_Electrical_Consumption + Punterociclo_15.ReHeating1_SF_Pump_Electrical_Consumption + Punterociclo_15.ReHeating2_SF_Pump_Electrical_Consumption + (Punterociclo_15.Generator_Power_Output * (Punterociclo_15.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_15.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_15.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_15.Rating_Point_Efficiency / 100) * Punterociclo_15.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_15.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_15.Rating_Point_Efficiency / 100) * Punterociclo_15.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_15.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row21 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_15.Rating_Point_Efficiency / 100) * Punterociclo_15.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_15.Total_Auxiliaries) / (Punterociclo_15.PHX_Q2 + Punterociclo_15.RHX1_Q2 + Punterociclo_15.RHX2_Q2)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row21);
            }

            else if (this.Brayton_cycle_type_variable == 5)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_5.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_5.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row1 = new string[] { Convert.ToString("ReHeating Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row1);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.PC1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.COOLER1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.PHX1 + Punterociclo_5.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_5.Generator_Power_Output) / (Punterociclo_5.PHX1 + Punterociclo_5.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double ReHeating_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_5.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_5.massflow2;

                string[] row7 = new string[] { Convert.ToString("ReHeating Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating_turbine * Punterociclo_5.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row7);
                ReHeating_Turbine_Power = specific_work_reheating_turbine * Punterociclo_5.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_5.massflow2 * (1 - Punterociclo_5.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_5.massflow2 * (1 - Punterociclo_5.recomp_frac_guess2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_5.massflow2 * Punterociclo_5.recomp_frac_guess2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_5.massflow2 * Punterociclo_5.recomp_frac_guess2;

                string[] row91 = new string[] { Convert.ToString("PreCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_5.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_5.massflow2;

                Total_Turbomachines_Power = Main_Turbine_Power + ReHeating_Turbine_Power + Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_5.Rating_Point_Efficiency / 100) * Punterociclo_5.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_5.Rating_Point_Efficiency / 100) * Punterociclo_5.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row14 = new string[] { Convert.ToString("ReHeating SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.ReHeating_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row14);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_5.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_5.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_5.Generator_Power_Output * (Punterociclo_5.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.Generator_Power_Output * (Punterociclo_5.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_5.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_5.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_5.Total_Auxiliaries = Punterociclo_5.ACHE_fans + Punterociclo_5.Main_SF_Pump_Electrical_Consumption + Punterociclo_5.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_5.Generator_Power_Output * (Punterociclo_5.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_5.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_5.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_5.Rating_Point_Efficiency / 100) * Punterociclo_5.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_5.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_5.Rating_Point_Efficiency / 100) * Punterociclo_5.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_5.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_5.Rating_Point_Efficiency / 100) * Punterociclo_5.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_5.Total_Auxiliaries) / (Punterociclo_5.PHX1 + Punterociclo_5.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);
            }

            else if (this.Brayton_cycle_type_variable == 6)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_6.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_6.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row1 = new string[] { Convert.ToString("ReHeating Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row1);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.PC1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.COOLER1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.PHX1 + Punterociclo_6.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_6.Generator_Power_Output) / (Punterociclo_6.PHX1 + Punterociclo_6.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double ReHeating_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_6.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_6.massflow2;

                string[] row7 = new string[] { Convert.ToString("ReHeating Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating_turbine * Punterociclo_6.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row7);
                ReHeating_Turbine_Power = specific_work_reheating_turbine * Punterociclo_6.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_6.massflow2 * (1 - Punterociclo_6.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_6.massflow2 * (1 - Punterociclo_6.recomp_frac2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_6.massflow2 * Punterociclo_6.recomp_frac2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_6.massflow2 * Punterociclo_6.recomp_frac2;

                string[] row91 = new string[] { Convert.ToString("PreCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_6.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_6.massflow2;

                Total_Turbomachines_Power = Main_Turbine_Power + ReHeating_Turbine_Power + Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_6.Rating_Point_Efficiency / 100) * Punterociclo_6.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_6.Rating_Point_Efficiency / 100) * Punterociclo_6.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row14 = new string[] { Convert.ToString("ReHeating SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.ReHeating_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row14);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_6.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_6.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_6.Generator_Power_Output * (Punterociclo_6.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.Generator_Power_Output * (Punterociclo_6.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_6.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_6.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_6.Total_Auxiliaries = Punterociclo_6.ACHE_fans + Punterociclo_6.Main_SF_Pump_Electrical_Consumption + Punterociclo_6.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_6.Generator_Power_Output * (Punterociclo_6.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_6.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_6.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_6.Rating_Point_Efficiency / 100) * Punterociclo_6.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_6.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_6.Rating_Point_Efficiency / 100) * Punterociclo_6.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_6.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_6.Rating_Point_Efficiency / 100) * Punterociclo_6.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_6.Total_Auxiliaries) / (Punterociclo_6.PHX1 + Punterociclo_6.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);
            }

            else if (this.Brayton_cycle_type_variable == 7)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_7.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_7.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);
                
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.PC11), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.PC21), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_7.Generator_Power_Output) / (Punterociclo_7.PHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_7.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_7.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_7.massflow2 * (1 - Punterociclo_7.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_7.massflow2 * (1 - Punterociclo_7.recomp_frac_guess2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_7.massflow2 * Punterociclo_7.recomp_frac_guess2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_7.massflow2 * Punterociclo_7.recomp_frac_guess2;

                string[] row91 = new string[] { Convert.ToString("PreCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_7.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_7.massflow2;

                Total_Turbomachines_Power = Main_Turbine_Power + Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_7.Rating_Point_Efficiency / 100) * Punterociclo_7.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_7.Rating_Point_Efficiency / 100) * Punterociclo_7.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_7.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_7.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_7.Generator_Power_Output * (Punterociclo_7.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.Generator_Power_Output * (Punterociclo_7.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_7.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_7.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_7.Total_Auxiliaries = Punterociclo_7.ACHE_fans + Punterociclo_7.Main_SF_Pump_Electrical_Consumption + Punterociclo_7.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_7.Generator_Power_Output * (Punterociclo_7.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_7.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_7.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_7.Rating_Point_Efficiency / 100) * Punterociclo_7.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_7.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_7.Rating_Point_Efficiency / 100) * Punterociclo_7.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_7.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_7.Rating_Point_Efficiency / 100) * Punterociclo_7.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_7.Total_Auxiliaries) / (Punterociclo_7.PHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);
            }

            else if (this.Brayton_cycle_type_variable == 8)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_8.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_8.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.PC11), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.PC21), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_8.Generator_Power_Output) / (Punterociclo_8.PHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_8.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_8.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_8.massflow2 * (1 - Punterociclo_8.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_8.massflow2 * (1 - Punterociclo_8.recomp_frac2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_8.massflow2 * Punterociclo_8.recomp_frac2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_8.massflow2 * Punterociclo_8.recomp_frac2;

                string[] row91 = new string[] { Convert.ToString("PreCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_8.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_8.massflow2;

                Total_Turbomachines_Power = Main_Turbine_Power + Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_8.Rating_Point_Efficiency / 100) * Punterociclo_8.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_8.Rating_Point_Efficiency / 100) * Punterociclo_8.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_8.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_8.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_8.Generator_Power_Output * (Punterociclo_8.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.Generator_Power_Output * (Punterociclo_8.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_8.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_8.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_8.Total_Auxiliaries = Punterociclo_8.ACHE_fans + Punterociclo_8.Main_SF_Pump_Electrical_Consumption + Punterociclo_8.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_8.Generator_Power_Output * (Punterociclo_8.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_8.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_8.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_8.Rating_Point_Efficiency / 100) * Punterociclo_8.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_8.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_8.Rating_Point_Efficiency / 100) * Punterociclo_8.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_8.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_8.Rating_Point_Efficiency / 100) * Punterociclo_8.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_8.Total_Auxiliaries) / (Punterociclo_8.PHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);            
            }

            else if (this.Brayton_cycle_type_variable == 9)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_9.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_9.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row1 = new string[] { Convert.ToString("ReHeating Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row1);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.PC11), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.PC21), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.PHX1 + Punterociclo_9.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_9.Generator_Power_Output) / (Punterociclo_9.PHX1 + Punterociclo_9.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double ReHeating_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_9.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_9.massflow2;

                string[] row7 = new string[] { Convert.ToString("ReHeating Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating_turbine * Punterociclo_9.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row7);
                ReHeating_Turbine_Power = specific_work_reheating_turbine * Punterociclo_9.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_9.massflow2 * (1 - Punterociclo_9.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_9.massflow2 * (1-Punterociclo_9.recomp_frac_guess2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_9.massflow2 * Punterociclo_9.recomp_frac_guess2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_9.massflow2 * Punterociclo_9.recomp_frac_guess2;

                string[] row91 = new string[] { Convert.ToString("Compressor 2 : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_9.massflow2 * (1 - Punterociclo_9.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_9.massflow2 * (1-Punterociclo_9.recomp_frac_guess2);

                Total_Turbomachines_Power = Main_Turbine_Power + ReHeating_Turbine_Power + Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_9.Rating_Point_Efficiency / 100) * Punterociclo_9.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_9.Rating_Point_Efficiency / 100) * Punterociclo_9.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row14 = new string[] { Convert.ToString("ReHeating SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.ReHeating_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row14);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_9.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_9.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_9.Generator_Power_Output * (Punterociclo_9.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.Generator_Power_Output * (Punterociclo_9.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_9.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_9.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_9.Total_Auxiliaries = Punterociclo_9.ACHE_fans + Punterociclo_9.Main_SF_Pump_Electrical_Consumption + Punterociclo_9.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_9.Generator_Power_Output * (Punterociclo_9.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_9.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_9.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_9.Rating_Point_Efficiency / 100) * Punterociclo_9.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_9.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_9.Rating_Point_Efficiency / 100) * Punterociclo_9.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_9.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_9.Rating_Point_Efficiency / 100) * Punterociclo_9.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_9.Total_Auxiliaries) / (Punterociclo_9.PHX1 + Punterociclo_9.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);
            }

            else if (this.Brayton_cycle_type_variable == 10)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_10.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_10.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);
                
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row1 = new string[] { Convert.ToString("ReHeating Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row1);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.PC11), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.PC21), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.PHX1 + Punterociclo_10.RHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_10.Generator_Power_Output) / (Punterociclo_10.PHX1 + Punterociclo_10.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double ReHeating_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_10.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_10.massflow2;

                string[] row7 = new string[] { Convert.ToString("ReHeating Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_reheating_turbine * Punterociclo_10.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row7);
                ReHeating_Turbine_Power = specific_work_reheating_turbine * Punterociclo_10.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_10.massflow2 * (1 - Punterociclo_10.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_10.massflow2 * (1 - Punterociclo_10.recomp_frac2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_10.massflow2 * Punterociclo_10.recomp_frac2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_10.massflow2 * Punterociclo_10.recomp_frac2;

                string[] row91 = new string[] { Convert.ToString("Compressor 2 : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_10.massflow2 * (1 - Punterociclo_10.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_10.massflow2 * (1 - Punterociclo_10.recomp_frac2);

                Total_Turbomachines_Power = Main_Turbine_Power + ReHeating_Turbine_Power + Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_10.Rating_Point_Efficiency / 100) * Punterociclo_10.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_10.Rating_Point_Efficiency / 100) * Punterociclo_10.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                string[] row14 = new string[] { Convert.ToString("ReHeating SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.ReHeating_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row14);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_10.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_10.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_10.Generator_Power_Output * (Punterociclo_10.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.Generator_Power_Output * (Punterociclo_10.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_10.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_10.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_10.Total_Auxiliaries = Punterociclo_10.ACHE_fans + Punterociclo_10.Main_SF_Pump_Electrical_Consumption + Punterociclo_10.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_10.Generator_Power_Output * (Punterociclo_10.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_10.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_10.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_10.Rating_Point_Efficiency / 100) * Punterociclo_10.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_10.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_10.Rating_Point_Efficiency / 100) * Punterociclo_10.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_10.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_10.Rating_Point_Efficiency / 100) * Punterociclo_10.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_10.Total_Auxiliaries) / (Punterociclo_10.PHX1 + Punterociclo_10.RHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);
            }

            else if (this.Brayton_cycle_type_variable == 11)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_11.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_11.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.PC11), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.PC21), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.PHX1), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_11.Generator_Power_Output) / (Punterociclo_11.PHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_11.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_11.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_11.massflow2 * (1 - Punterociclo_11.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_11.massflow2 * (1 - Punterociclo_11.recomp_frac_guess2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_11.massflow2 * Punterociclo_11.recomp_frac_guess2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_11.massflow2 * Punterociclo_11.recomp_frac_guess2;

                string[] row91 = new string[] { Convert.ToString("Compressor 2 : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_11.massflow2 * (1 - Punterociclo_11.recomp_frac_guess2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_11.massflow2 * (1 - Punterociclo_11.recomp_frac_guess2);

                Total_Turbomachines_Power = Main_Turbine_Power+ Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_11.Rating_Point_Efficiency / 100) * Punterociclo_11.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_11.Rating_Point_Efficiency / 100) * Punterociclo_11.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_11.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_11.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_11.Generator_Power_Output * (Punterociclo_11.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.Generator_Power_Output * (Punterociclo_11.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_11.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_11.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_11.Total_Auxiliaries = Punterociclo_11.ACHE_fans + Punterociclo_11.Main_SF_Pump_Electrical_Consumption + Punterociclo_11.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_11.Generator_Power_Output * (Punterociclo_11.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_11.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_11.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_11.Rating_Point_Efficiency / 100) * Punterociclo_11.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_11.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_11.Rating_Point_Efficiency / 100) * Punterociclo_11.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_11.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_11.Rating_Point_Efficiency / 100) * Punterociclo_11.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_11.Total_Auxiliaries) / (Punterociclo_11.PHX1)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);
            }

            else if (this.Brayton_cycle_type_variable == 12)
            {
                //Generator_Total_Loss, Generator_Electrical_Loss, Generator_Mechanical_Loss
                textBox9.Text = "Generator nameplate power = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Name_Plate_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Nameplate / Design point load = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Rating_Design_Point_Load), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + " Shaft Power = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Power Output = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Power_Output), 2, MidpointRounding.AwayFromZero) + "=" + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Shaft_Power), 2, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal((Punterociclo_12.Rating_Point_Efficiency / 100)), 4, MidpointRounding.AwayFromZero) + " * " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Gear_Efficiency), 4, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator total loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Total_Loss), 2, MidpointRounding.AwayFromZero) + " Electrical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Electrical_Loss), 2, MidpointRounding.AwayFromZero) + " Mechanical loss = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Mechanical_Loss), 2, MidpointRounding.AwayFromZero) +
                                  Environment.NewLine + "Generator Efficiency = " + decimal.Round(Convert.ToDecimal(Punterociclo_12.Rating_Point_Efficiency), 4, MidpointRounding.AwayFromZero);

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();

                string[] row = new string[] { Convert.ToString("Main Solar Field: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.PHX), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row);

                string[] row2 = new string[] { Convert.ToString("Pre-Cooler1: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.PC11), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row2);

                string[] row21 = new string[] { Convert.ToString("Pre-Cooler2: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.PC21), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView4.Rows.Add(row21);

                string[] row3 = new string[] { Convert.ToString("Net Total Heat Input: "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.PHX), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row3);

                string[] row4 = new string[] { Convert.ToString("Gross Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Power_Output), 4, MidpointRounding.AwayFromZero)), "kWth" };
                dataGridView2.Rows.Add(row4);

                string[] row5 = new string[] { Convert.ToString("Gross Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_12.Generator_Power_Output) / (Punterociclo_12.PHX)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row5);

                //Power Generated in Turbomachines
                Double Main_Turbine_Power = 0;
                Double Compressor1_Power = 0;
                Double Compressor2_Power = 0;
                Double Compressor3_Power = 0;
                Double Total_Turbomachines_Power = 0;

                //Turbines Power
                string[] row6 = new string[] { Convert.ToString("Main Turbine : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_main_turbine * Punterociclo_12.massflow2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row6);
                Main_Turbine_Power = specific_work_main_turbine * Punterociclo_12.massflow2;

                //Compressors Power
                string[] row8 = new string[] { Convert.ToString("Main Compressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor1 * Punterociclo_12.massflow2 * (1 - Punterociclo_12.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row8);
                Compressor1_Power = specific_work_compressor1 * Punterociclo_12.massflow2 * (1 - Punterociclo_12.recomp_frac2);

                string[] row9 = new string[] { Convert.ToString("ReCompressor : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor2 * Punterociclo_12.massflow2 * Punterociclo_12.recomp_frac2), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row9);
                Compressor2_Power = specific_work_compressor2 * Punterociclo_12.massflow2 * Punterociclo_12.recomp_frac2;

                string[] row91 = new string[] { Convert.ToString("Compressor 2 : "), Convert.ToString(decimal.Round(Convert.ToDecimal(specific_work_compressor3 * Punterociclo_12.massflow2 * (1 - Punterociclo_12.recomp_frac2)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row91);
                Compressor3_Power = specific_work_compressor3 * Punterociclo_12.massflow2 * (1 - Punterociclo_12.recomp_frac2);

                Total_Turbomachines_Power = Main_Turbine_Power + Compressor1_Power + Compressor2_Power + Compressor3_Power;

                string[] row10 = new string[] { Convert.ToString("Total TurboMachines Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row10);

                string[] row11 = new string[] { Convert.ToString("Generator Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_12.Rating_Point_Efficiency / 100) * Punterociclo_12.Gear_Efficiency) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView1.Rows.Add(row11);

                string[] row12 = new string[] { Convert.ToString("Generator Output Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(((Punterociclo_12.Rating_Point_Efficiency / 100) * Punterociclo_12.Gear_Efficiency) * Total_Turbomachines_Power), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row12);

                //Solar Field Pumps Electrical Consumption
                string[] row13 = new string[] { Convert.ToString("Main SF Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.Main_SF_Pump_Electrical_Consumption), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row13);

                textBox15.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.Main_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_12.UHS_Water_Pump = Convert.ToDouble(textBox11.Text);
                string[] row15 = new string[] { Convert.ToString("UHS Water Pump Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.UHS_Water_Pump), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row15);

                Punterociclo_12.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                textBox3.Text = Convert.ToString(Punterociclo_12.Generator_Power_Output * (Punterociclo_12.Miscellanous_Auxiliaries / 100));
                string[] row16 = new string[] { Convert.ToString("Miscellanous_Auxiliaries : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.Generator_Power_Output * (Punterociclo_12.Miscellanous_Auxiliaries / 100)), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row16);

                textBox14.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.ReHeating_SF_Pump_Electrical_Consumption), 2, MidpointRounding.AwayFromZero));

                Punterociclo_12.ACHE_fans = Convert.ToDouble(textBox2.Text);

                Punterociclo_12.Miscellanous_Auxiliaries = Convert.ToDouble(textBox13.Text);
                string[] row17 = new string[] { Convert.ToString("ACHE Fans Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.ACHE_fans), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row17);

                Punterociclo_12.Total_Auxiliaries = Punterociclo_12.ACHE_fans + Punterociclo_12.Main_SF_Pump_Electrical_Consumption + Punterociclo_12.ReHeating_SF_Pump_Electrical_Consumption + (Punterociclo_12.Generator_Power_Output * (Punterociclo_12.Miscellanous_Auxiliaries / 100));
                textBox1.Text = Convert.ToString(Punterociclo_12.Total_Auxiliaries);

                string[] row18 = new string[] { Convert.ToString("Total_Auxiliaries Power : "), Convert.ToString(decimal.Round(Convert.ToDecimal(Punterociclo_12.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView3.Rows.Add(row18);

                string[] row20 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_12.Rating_Point_Efficiency / 100) * Punterociclo_12.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_12.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView2.Rows.Add(row20);

                string[] row19 = new string[] { Convert.ToString("Net Power Output : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((Punterociclo_12.Rating_Point_Efficiency / 100) * Punterociclo_12.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_12.Total_Auxiliaries), 4, MidpointRounding.AwayFromZero)), "kW" };
                dataGridView1.Rows.Add(row19);

                string[] row25 = new string[] { Convert.ToString("Net Efficiency : "), Convert.ToString(decimal.Round(Convert.ToDecimal((((((Punterociclo_12.Rating_Point_Efficiency / 100) * Punterociclo_12.Gear_Efficiency) * Total_Turbomachines_Power) - Punterociclo_12.Total_Auxiliaries) / (Punterociclo_12.PHX)) * 100), 4, MidpointRounding.AwayFromZero)), "%" };
                dataGridView2.Rows.Add(row25);
            }
        }
    }
}
