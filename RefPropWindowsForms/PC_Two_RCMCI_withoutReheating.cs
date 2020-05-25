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
    public partial class PC_Two_RCMCI_withoutReheating : Form
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
        public Double t_mc2_in;

        public Double t_t_in;

        public Double p_mc4_in;
        public Double p_mc4_out;

        public Double p_mc3_in;
        public Double p_mc3_out;

        public Double p_mc2_in;
        public Double p_mc2_out;

        public Double p_pc_in;
        public Double p_pc_out;
        public Double t_pc_in;

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

        public Double m_eta_pc;
        public Double m_eta_mc4;
        public Double m_eta_mc3;
        public Double m_eta_mc2;
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
        public Double eta_mc2;
        public Double eta_rc2;
        public Double eta_pc;
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
        public snl_compressor_tsr Main_Compressor2;
        public snl_compressor_tsr Recompressor;
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

        //Generator Information
        public Double Generator_Name_Plate_Power, Generator_Power_Output, Generator_Total_Loss;
        public Double Generator_Shaft_Power, Gear_Efficiency, Rating_Point_Efficiency;
        public Double Generator_Mechanical_Loss, Generator_Electrical_Loss, Rating_Design_Point_Load;

        //SF and ACHE Pumps electrical consumption
        public Double Main_SF_Pump_Electrical_Consumption;
        public Double UHS_Water_Pump, ACHE_fans;

        public PC_Two_RCMCI_withoutReheating()
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
            w_dot_net = Convert.ToDouble(textBox1.Text);

            t_mc4_in = Convert.ToDouble(textBox2.Text);
            t_mc3_in = Convert.ToDouble(textBox28.Text);
            t_mc2_in = Convert.ToDouble(textBox102.Text);

            t_pc_in = Convert.ToDouble(textBox86.Text);

            t_t_in = Convert.ToDouble(textBox4.Text);

            p_pc_in = Convert.ToDouble(textBox103.Text);
            p_pc_out = Convert.ToDouble(textBox104.Text);

            p_mc4_in = Convert.ToDouble(textBox3.Text);
            p_mc4_out = Convert.ToDouble(textBox8.Text);

            p_mc3_in = Convert.ToDouble(textBox23.Text);
            p_mc3_out = Convert.ToDouble(textBox22.Text);

            p_mc2_in = Convert.ToDouble(textBox88.Text);
            p_mc2_out = Convert.ToDouble(textBox87.Text);          

            ua_lt = Convert.ToDouble(textBox17.Text);
            ua_ht = Convert.ToDouble(textBox16.Text);

            m_recomp_frac = Convert.ToDouble(textBox15.Text);

            m_eta_pc = Convert.ToDouble(textBox106.Text);
            m_eta_mc4 = Convert.ToDouble(textBox14.Text);
            m_eta_mc3 = Convert.ToDouble(textBox27.Text);
            m_eta_mc2 = Convert.ToDouble(textBox98.Text);
            m_eta_rc = Convert.ToDouble(textBox13.Text);
            m_eta_t = Convert.ToDouble(textBox19.Text);

            n_sub_hxrs = Convert.ToInt64(textBox20.Text);

            tol = Convert.ToDouble(textBox21.Text);

            dp_lt1 = Convert.ToDouble(textBox5.Text);
            dp_ht1 = Convert.ToDouble(textBox12.Text);

            dp_lt2 = Convert.ToDouble(textBox26.Text);
            dp_ht2 = Convert.ToDouble(textBox25.Text);

            dp_pc1 = Convert.ToDouble(textBox11.Text);
            dp_pc2 = Convert.ToDouble(textBox24.Text);
            dp_pc3 = Convert.ToDouble(textBox107.Text);
            dp_pc4 = Convert.ToDouble(textBox99.Text);

            dp_phx = Convert.ToDouble(textBox10.Text);          

            eta_thermal = 0.0;

            luis.wmm = luis.working_fluid.MolecularWeight;

            core.PC_Two_RCMCI_withoutReheating cicloPC_Two_RCMCI_withoutReheating = new core.PC_Two_RCMCI_withoutReheating();

            luis.RecompCycle_PC_Two_RCMCI_withoutReheating(luis, ref cicloPC_Two_RCMCI_withoutReheating,
            w_dot_net,  t_pc_in,  t_mc4_in,  t_mc3_in,  t_mc2_in,  t_t_in,
            p_pc_in,  p_pc_out,  p_mc4_in,  p_mc4_out,  p_mc3_in,  p_mc3_out,
            p_mc2_in, p_mc2_out,  ua_lt,  ua_ht,  m_eta_mc4,  m_eta_pc,  m_eta_rc,
            m_eta_mc3,  m_eta_mc2,  m_eta_t, n_sub_hxrs,  m_recomp_frac,  tol,  eta_thermal,
            dp_lt1,  dp_lt2,  dp_ht1,  dp_ht2,  dp_pc1,  dp_pc4,  dp_pc3,  dp_pc2,
            dp_phx);

            massflow2 = cicloPC_Two_RCMCI_withoutReheating.m_dot_turbine;
            w_dot_net2 = cicloPC_Two_RCMCI_withoutReheating.W_dot_net;
            eta_thermal2 = cicloPC_Two_RCMCI_withoutReheating.eta_thermal;

            eta_mc4 = m_eta_mc4;
            eta_mc3 = m_eta_mc3;
            eta_mc2 = m_eta_mc2;
            eta_rc2 = m_eta_rc;
            eta_pc = m_eta_pc;
            eta_t2 = m_eta_t;

            temp21 = cicloPC_Two_RCMCI_withoutReheating.temp[0];
            temp22 = cicloPC_Two_RCMCI_withoutReheating.temp[1];
            temp23 = cicloPC_Two_RCMCI_withoutReheating.temp[2];
            temp24 = cicloPC_Two_RCMCI_withoutReheating.temp[3];
            temp25 = cicloPC_Two_RCMCI_withoutReheating.temp[4];
            temp26 = cicloPC_Two_RCMCI_withoutReheating.temp[5];
            temp27 = cicloPC_Two_RCMCI_withoutReheating.temp[6];
            temp28 = cicloPC_Two_RCMCI_withoutReheating.temp[7];
            temp29 = cicloPC_Two_RCMCI_withoutReheating.temp[8];
            temp210 = cicloPC_Two_RCMCI_withoutReheating.temp[9];
            temp211 = cicloPC_Two_RCMCI_withoutReheating.temp[10];
            temp212 = cicloPC_Two_RCMCI_withoutReheating.temp[11];
            temp213 = cicloPC_Two_RCMCI_withoutReheating.temp[12];
            temp214 = cicloPC_Two_RCMCI_withoutReheating.temp[13];
            temp215 = cicloPC_Two_RCMCI_withoutReheating.temp[14];
            temp216 = cicloPC_Two_RCMCI_withoutReheating.temp[15];

            pres21 = cicloPC_Two_RCMCI_withoutReheating.pres[0];
            pres22 = cicloPC_Two_RCMCI_withoutReheating.pres[1];
            pres23 = cicloPC_Two_RCMCI_withoutReheating.pres[2];
            pres24 = cicloPC_Two_RCMCI_withoutReheating.pres[3];
            pres25 = cicloPC_Two_RCMCI_withoutReheating.pres[4];
            pres26 = cicloPC_Two_RCMCI_withoutReheating.pres[5];
            pres27 = cicloPC_Two_RCMCI_withoutReheating.pres[6];
            pres28 = cicloPC_Two_RCMCI_withoutReheating.pres[7];
            pres29 = cicloPC_Two_RCMCI_withoutReheating.pres[8];
            pres210 = cicloPC_Two_RCMCI_withoutReheating.pres[9];
            pres211 = cicloPC_Two_RCMCI_withoutReheating.pres[10];
            pres212 = cicloPC_Two_RCMCI_withoutReheating.pres[11];
            pres213 = cicloPC_Two_RCMCI_withoutReheating.pres[12];
            pres214 = cicloPC_Two_RCMCI_withoutReheating.pres[13];
            pres215 = cicloPC_Two_RCMCI_withoutReheating.pres[14];
            pres216 = cicloPC_Two_RCMCI_withoutReheating.pres[15];

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
            textBox7.Text = Convert.ToString(temp215);
            textBox6.Text = Convert.ToString(temp216);

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
            textBox18.Text = Convert.ToString(pres215);
            textBox9.Text = Convert.ToString(pres216);

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

            if (checkBox1.Checked == false)
            {
                PHX1 = cicloPC_Two_RCMCI_withoutReheating.PHX.Q_dot;
                recomp_frac2 = cicloPC_Two_RCMCI_withoutReheating.recomp_frac;
                LT_Q = cicloPC_Two_RCMCI_withoutReheating.LT.Q_dot;
                LT_mdotc = cicloPC_Two_RCMCI_withoutReheating.LT.m_dot_design[0];
                LT_mdoth = cicloPC_Two_RCMCI_withoutReheating.LT.m_dot_design[1];
                LT_Tcin = cicloPC_Two_RCMCI_withoutReheating.LT.T_c_in;
                LT_Thin = cicloPC_Two_RCMCI_withoutReheating.LT.T_h_in;
                LT_Pcin = cicloPC_Two_RCMCI_withoutReheating.LT.P_c_in;
                LT_Phin = cicloPC_Two_RCMCI_withoutReheating.LT.P_h_in;
                LT_Pcout = cicloPC_Two_RCMCI_withoutReheating.LT.P_c_out;
                LT_Phout = cicloPC_Two_RCMCI_withoutReheating.LT.P_h_out;
                LT_Effc = cicloPC_Two_RCMCI_withoutReheating.LT.eff;

                HT_Q = cicloPC_Two_RCMCI_withoutReheating.HT.Q_dot;
                HT_mdotc = cicloPC_Two_RCMCI_withoutReheating.HT.m_dot_design[0];
                HT_mdoth = cicloPC_Two_RCMCI_withoutReheating.HT.m_dot_design[1];
                HT_Tcin = cicloPC_Two_RCMCI_withoutReheating.HT.T_c_in;
                HT_Thin = cicloPC_Two_RCMCI_withoutReheating.HT.T_h_in;
                HT_Pcin = cicloPC_Two_RCMCI_withoutReheating.HT.P_c_in;
                HT_Phin = cicloPC_Two_RCMCI_withoutReheating.HT.P_h_in;
                HT_Pcout = cicloPC_Two_RCMCI_withoutReheating.HT.P_c_out;
                HT_Phout = cicloPC_Two_RCMCI_withoutReheating.HT.P_h_out;
                HT_Effc = cicloPC_Two_RCMCI_withoutReheating.HT.eff;

                PC1 = -cicloPC_Two_RCMCI_withoutReheating.PC1.Q_dot;
                PC2 = -cicloPC_Two_RCMCI_withoutReheating.PC2.Q_dot;
                PC3 = -cicloPC_Two_RCMCI_withoutReheating.PC3.Q_dot;

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
            }

            button6.Enabled = true;
            button8.Enabled = true;
            button13.Enabled = true;
            button4.Enabled = true;
            button7.Enabled = true;
            button15.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button14.Enabled = true;
        }

        //Optimization Analysis
        private void button37_Click(object sender, EventArgs e)
        {

        }

        //OK button
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //Reset button
        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
