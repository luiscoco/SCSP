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

namespace RefPropWindowsForms
{
    public partial class Two_PC_RCMCI_with_Two_Reheating : Form
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

        public PTC_Solar_Field SF_PHX;

        //Store Input Data from Graphical User Interface GUI into variables
        public Double w_dot_net;

        public Double t_mc4_in;
        public Double t_mc3_in;

        public Double t_t_in;

        public Double p_mc4_in;
        public Double p_mc4_out;

        public Double p_mc3_in;
        public Double p_mc3_out;

        public Double p_pc1_in;
        public Double p_pc1_out;
        public Double t_pc1_in;

        public Double p_pc2_in;
        public Double p_pc2_out;
        public Double t_pc2_in;

        public Double dp_lt1;
        public Double dp_ht1;

        public Double dp_pc1;
        public Double dp_pc2;
        public Double dp_pc3;
        public Double dp_pc4;

        public Double dp_phx;

        public Double dp_lt2;
        public Double dp_ht2;

        public Double ua_lt;
        public Double ua_ht;

        public Double m_recomp_frac;

        public Double m_eta_pc1;
        public Double m_eta_pc2;
        public Double m_eta_mc4;
        public Double m_eta_mc3;
        public Double m_eta_rc;
        public Double m_eta_t;

        public long n_sub_hxrs;

        public Double tol;

        public Double eta_thermal;

        public Double PHX1;
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
        public Double PC4;

        public HeatExchangerUA LT_Recuperator;
        public HeatExchangerUA HT_Recuperator;

        public Radial_Turbine Main_Turbine;

        public PreeCooler Precooler_dialog1;
        public PreeCooler Precooler_dialog2;
        public PreeCooler Precooler_dialog3;
        public PreeCooler Precooler_dialog4;

        public Double massflow2;
        public Double w_dot_net2;
        public Double eta_thermal2;

        public Double eta_mc4;
        public Double eta_mc3;
        public Double eta_rc2;
        public Double eta_pc1;
        public Double eta_pc2;
        public Double eta_t2;

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

        public snl_compressor_tsr Main_Compressor4;
        public snl_compressor_tsr Main_Compressor3;
        public snl_compressor_tsr Recompressor;
        public snl_compressor_tsr PreCompressor1;
        public snl_compressor_tsr PreCompressor2;

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

        //Generator Information
        public Double Generator_Name_Plate_Power, Generator_Power_Output, Generator_Total_Loss;
        public Double Generator_Shaft_Power, Gear_Efficiency, Rating_Point_Efficiency;
        public Double Generator_Mechanical_Loss, Generator_Electrical_Loss, Rating_Design_Point_Load;

        //SF and ACHE Pumps electrical consumption
        public Double Main_SF_Pump_Electrical_Consumption;
        public Double UHS_Water_Pump, ACHE_fans;


        public Two_PC_RCMCI_with_Two_Reheating()
        {
            InitializeComponent();
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

        //Set Critical Conditions
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
                textBox88.Text = Convert.ToString(working_fluid.CriticalTemperature);
                textBox87.Text = Convert.ToString(working_fluid.CriticalPressure);

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
                this.textBox87.Text = xlWorkSheet.get_Range("D69", "D69").Value2.ToString();
                this.textBox2.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox28.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox102.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();
                this.textBox88.Text = xlWorkSheet.get_Range("D68", "D68").Value2.ToString();

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

        //Mixtures Calculation
        private void button20_Click(object sender, EventArgs e)
        {

        }

        //Optimization Analysis
        private void button37_Click(object sender, EventArgs e)
        {

        }
    }
}
