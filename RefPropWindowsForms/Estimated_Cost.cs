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
    public partial class Estimated_Cost : Form
    {
        public Double PTC_Main_SF_area;
        public Double PTC_ReHeating_SF_area;
        public Double LF_Main_SF_area;
        public Double LF_ReHeating_SF_area;
        public Double Fossil_Backup_Gross_Power;
        public Double Power_Plant_Gross_Power;
        public Double Balance_Of_Plant_Gross_Power;
        //Convert from m2 to acres
        public Double PTC_Solar_Field_Area;
        public Double LF_Solar_Field_Area;
        public Double Non_Solar_Field_Land_Area_Multiplier;
        public Double PTC_Rows_Distance_Multiplier;
        public Double Land_Area;

        public Double PTC_Main_SF_Unitary_Cost;
        public Double PTC_ReHeating_SF_Unitary_Cost;
        public Double PTC_HTF_System_Unitary_Cost;
        public Double LF_Main_SF_Unitary_Cost;
        public Double LF_ReHeating_SF_Unitary_Cost;
        public Double LF_HTF_System_Unitary_Cost;
        public Double Site_Improvements_Unitary_Cost;
        public Double Fossil_Backup_Unitary_Cost;
        public Double Power_Plant_Unitary_Cost;
        public Double Balance_Of_Plant_Unitary_Cost;
        public Double Contingency;

        public Double PTC_Main_SF_Cost;
        public Double PTC_ReHeating_SF_Cost;
        public Double PTC_HTF_System_Cost;
        public Double Site_Improvements_Cost;
        public Double Fossil_Backup_Cost;
        public Double Power_Plant_Cost;
        public Double Balance_Of_Plant_Cost;
        public Double Total_Direct_Capital_Cost_before_Contingency;
        public Double Total_Direct_Capital_cost;


        public Estimated_Cost()
        {
            InitializeComponent();
        }

        //OK Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Cost Etimation calculations
        public void button1_Click(object sender, EventArgs e)
        {
            PTC_Main_SF_area = Convert.ToDouble(textBox32.Text);
            PTC_ReHeating_SF_area = Convert.ToDouble(textBox3.Text);
            PTC_Solar_Field_Area = (PTC_Main_SF_area + PTC_ReHeating_SF_area)/4046.86;
            textBox7.Text = Convert.ToString(PTC_Solar_Field_Area);
            textBox63.Text = Convert.ToString(PTC_Main_SF_area + PTC_ReHeating_SF_area);
            textBox17.Text = Convert.ToString(PTC_Main_SF_area + PTC_ReHeating_SF_area);
            Fossil_Backup_Gross_Power = Convert.ToDouble(textBox64.Text);
            Power_Plant_Gross_Power = Convert.ToDouble(textBox65.Text); ;
            Balance_Of_Plant_Gross_Power = Convert.ToDouble(textBox66.Text); ;

            //PTC: AISI T22 SS Receiver Pipes, DSG as HTF 
            //PTC: AISI 347 SS Receiver Pipes, Solar Salt as HTF
            //PTC: CS Receiver Pipes, Thermal Oil as HTF
            //LF: AISI T22 SS Receiver Pipes, DSG as HTF
            //LF: AISI 347 SS Receiver Pipes, Solar Salt as HTF
            //LF: CS Receiver Pipes, Thermal Oil as HTF

            if (comboBox1.Text == "User-defined...")
            {
              
            }

            else if (comboBox1.Text == "PTC: CS Receiver Pipes, Thermal Oil as HTF")
            {
                textBox91.Text = Convert.ToString(30.0);
                textBox90.Text = Convert.ToString(270.0);
                textBox2.Text = Convert.ToString(270.0);
                textBox8.Text = Convert.ToString(1.4);
                textBox10.Text = Convert.ToString(3.0);
            }

            else if (comboBox1.Text == "LF: AISI T22 SS Receiver Pipes, DSG as HTF")
            {
                textBox91.Text = Convert.ToString(20.0);
                textBox90.Text = Convert.ToString(180.0);
                textBox2.Text = Convert.ToString(180.0);
                textBox8.Text = Convert.ToString(1.5);
                textBox10.Text = Convert.ToString(1.0);
            }

            Non_Solar_Field_Land_Area_Multiplier = Convert.ToDouble(textBox8.Text);
            PTC_Rows_Distance_Multiplier = Convert.ToDouble(textBox10.Text);

            Land_Area = PTC_Solar_Field_Area * Non_Solar_Field_Land_Area_Multiplier * PTC_Rows_Distance_Multiplier;
            textBox9.Text = Convert.ToString(Land_Area);

            Site_Improvements_Unitary_Cost = Convert.ToDouble(textBox91.Text);
            PTC_Main_SF_Unitary_Cost = Convert.ToDouble(textBox90.Text);
            PTC_ReHeating_SF_Unitary_Cost = Convert.ToDouble(textBox2.Text);
            PTC_HTF_System_Unitary_Cost = Convert.ToDouble(textBox89.Text);
            Fossil_Backup_Unitary_Cost = Convert.ToDouble(textBox88.Text);
            Power_Plant_Unitary_Cost = Convert.ToDouble(textBox68.Text);
            Balance_Of_Plant_Unitary_Cost = Convert.ToDouble(textBox67.Text);
            Contingency = Convert.ToDouble(textBox92.Text);

            PTC_Main_SF_Cost = PTC_Main_SF_area * PTC_Main_SF_Unitary_Cost;
            PTC_ReHeating_SF_Cost = PTC_ReHeating_SF_area * PTC_ReHeating_SF_Unitary_Cost;
            PTC_HTF_System_Cost = (PTC_Main_SF_area + PTC_ReHeating_SF_area) * PTC_HTF_System_Unitary_Cost;
            Site_Improvements_Cost = (PTC_Main_SF_area + PTC_ReHeating_SF_area) * Site_Improvements_Unitary_Cost;
            Fossil_Backup_Cost = Fossil_Backup_Gross_Power*1000 * Fossil_Backup_Unitary_Cost;
            Power_Plant_Cost = Power_Plant_Gross_Power*1000 * Power_Plant_Unitary_Cost;
            Balance_Of_Plant_Cost = Balance_Of_Plant_Gross_Power*1000 * Balance_Of_Plant_Unitary_Cost;
            Total_Direct_Capital_Cost_before_Contingency = (Contingency / 100) * (Site_Improvements_Cost + PTC_Main_SF_Cost + PTC_ReHeating_SF_Cost + PTC_HTF_System_Cost + Fossil_Backup_Cost + Power_Plant_Cost + Balance_Of_Plant_Cost);
            Total_Direct_Capital_cost = Site_Improvements_Cost + PTC_Main_SF_Cost + PTC_ReHeating_SF_Cost + PTC_HTF_System_Cost + Fossil_Backup_Cost + Power_Plant_Cost + Balance_Of_Plant_Cost + Total_Direct_Capital_Cost_before_Contingency;

            textBox98.Text = Convert.ToString(PTC_Main_SF_Cost);
            textBox1.Text = Convert.ToString(PTC_ReHeating_SF_Cost);
            textBox97.Text = Convert.ToString(PTC_HTF_System_Cost);
            textBox99.Text = Convert.ToString(Site_Improvements_Cost);
            textBox96.Text = Convert.ToString(Fossil_Backup_Cost);
            textBox95.Text = Convert.ToString(Power_Plant_Cost);
            textBox94.Text = Convert.ToString(Balance_Of_Plant_Cost);
            textBox93.Text = Convert.ToString(Total_Direct_Capital_Cost_before_Contingency);
            textBox100.Text = Convert.ToString(Total_Direct_Capital_cost);

            textBox121.Text = Convert.ToString(Land_Area);
            textBox108.Text = Convert.ToString(Total_Direct_Capital_cost);
        }
    }
}
