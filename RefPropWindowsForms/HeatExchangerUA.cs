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
    public partial class HeatExchangerUA : Form
    {

      //private ZedGraph.ZedGraphControl z1;

      // VERY IMPORTANT the dataBase functions REFPROP in this programs return the following units. 
        
      //T = temperature [K]
      //P = pressure [kPa]
      //D = density [mol/L]  L=dm^3
      //E = internal energy [J/mol]
      //H = enthalpy [J/mol]
      //S = entropy [J/mol-K]
      //Q = vapor quality [moles vapor/total moles]
      //or [kg vapor/total kg] depending on the
      //value of the input flag kq

        public core CorePointer = new core();

        public void HeatExchangerUA1(core HXwindow)
        {
            CorePointer = HXwindow;
        }

        public Int64 error_code;

        //Input Data: N_sub_hxrs, Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out
        public Int64 N_sub_hxrs;
        public Double Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out;
        public Double Effec = 0;
        public Double Effec_module;


        //Input Data per HX Module
        public Int16 Number_Modules;
        public Double Q_dot_permodule;
        public Double m_dot_c_module;
        public Double m_dot_h_module;

        //Results: UA, min_DT
        public Double UA, min_DT, NTU,CR;
        public Double UA_module, min_DT_module, NTU_module, CR_module;

        public Double[] T_c;
        public Double[] T_h;
        public decimal[] T_cd;
        public decimal[] T_hd;

        public Double[] P_c;
        public Double[] P_h;

        public Double[] UA_local_1= new Double[25];
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

        public HeatExchangerUA()
        {
            InitializeComponent();
        }

        public void Calculate_HX()
        {           
            chart1.Series.Clear();

            chart1.Series.Add("Cold_Side");
            chart1.Series.Add("Hot_Side");

            chart2.Series.Clear();

            chart2.Series.Add("Cold_Side");
            chart2.Series.Add("Hot_Side");

            N_sub_hxrs = Convert.ToInt64(textBox1.Text);
            Q_dot = Convert.ToDouble(textBox2.Text);
            m_dot_c = Convert.ToDouble(textBox3.Text);
            m_dot_h = Convert.ToDouble(textBox4.Text);
            T_c_in = Convert.ToDouble(textBox7.Text);
            T_h_in = Convert.ToDouble(textBox6.Text);
            P_c_in = Convert.ToDouble(textBox5.Text);
            P_c_out = Convert.ToDouble(textBox9.Text);
            P_h_in = Convert.ToDouble(textBox8.Text);
            P_h_out = Convert.ToDouble(textBox12.Text);

            Number_Modules = Convert.ToInt16(textBox27.Text);
            Q_dot_permodule = Q_dot / Number_Modules;
            textBox28.Text = Convert.ToString(Q_dot_permodule);
            textBox36.Text = Convert.ToString(m_dot_c/Number_Modules);
            m_dot_c_module = m_dot_c / Number_Modules;
            textBox35.Text = Convert.ToString(m_dot_h / Number_Modules);
            m_dot_h_module = m_dot_h / Number_Modules;

            T_c = new Double[N_sub_hxrs + 1];
            T_h = new Double[N_sub_hxrs + 1];

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

            T_cd = new decimal[N_sub_hxrs + 1];
            T_hd = new decimal[N_sub_hxrs + 1];

            NTU = 0;
            CR = 0;

            //Total HX calculation
            CorePointer.calculate_hxr_UA(N_sub_hxrs, Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out,
            ref error_code, ref UA, ref min_DT, ref T_h, ref T_c,ref Effec, ref P_h, ref P_c, ref UA_local, ref NTU, ref CR,
            ref NTU_local, ref CR_local, ref Effec_local);

            NTU_module = 0;
            CR_module = 0;

            //HX_Module calculation
            CorePointer.calculate_hxr_UA(N_sub_hxrs, Q_dot_permodule, m_dot_c_module, m_dot_h_module, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out,
            ref error_code, ref UA_module, ref min_DT_module, ref T_h, ref T_c, ref Effec_module, ref P_h, ref P_c, ref UA_local_module, ref NTU_module, ref CR_module,
            ref NTU_local_module, ref CR_local_module, ref Effec_local_module);

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

            textBox11.Text = Convert.ToString(UA);
            textBox10.Text = Convert.ToString(min_DT);
            textBox31.Text = Convert.ToString(NTU);
            //textBox32.Text = Convert.ToString(CR);

            textBox39.Text = Convert.ToString(UA_module);
            textBox34.Text = Convert.ToString(NTU_module);
            //textBox33.Text = Convert.ToString(CR_module);
            textBox38.Text = Convert.ToString(min_DT_module);
            textBox37.Text = Convert.ToString(Effec_module);       

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
                CorePointer.working_fluid.FindStateWithTP(Tave_h[contador], P_h[contador]);
                Density_h[contador] = CorePointer.working_fluid.Density;
                CorePointer.working_fluid.FindStateWithTP(Tave_c[contador], P_c[contador]);
                Density_c[contador] = CorePointer.working_fluid.Density;
            }

            //3.Viscosities
            Double[] Viscosity_h = new Double[N_sub_hxrs];
            Double[] Viscosity_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                CorePointer.working_fluid.FindStateWithTD(Tave_h[contador], Density_h[contador] / CorePointer.wmm);
                Viscosity_h[contador] = (CorePointer.working_fluid.viscosity) / 1000000;
                CorePointer.working_fluid.FindStateWithTD(Tave_c[contador], Density_c[contador] / CorePointer.wmm);
                Viscosity_c[contador] = (CorePointer.working_fluid.viscosity) / 1000000;
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
                CorePointer.working_fluid.FindStateWithTD(Tave_h[contador], Density_h[contador] / CorePointer.wmm);
                Thermal_Conductivity_h[contador] = CorePointer.working_fluid.thermalconductivity;
                CorePointer.working_fluid.FindStateWithTD(Tave_c[contador], Density_c[contador] / CorePointer.wmm);
                Thermal_Conductivity_c[contador] = CorePointer.working_fluid.thermalconductivity;
            }

            //6.Specific Heat at constant pressure Cp
            Double[] Cp_h = new Double[N_sub_hxrs];
            Double[] Cp_c = new Double[N_sub_hxrs];
            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                CorePointer.working_fluid.FindStateWithTP(Tave_h[contador], P_h[contador]);
                Cp_h[contador] = CorePointer.working_fluid.Cp;
                CorePointer.working_fluid.FindStateWithTP(Tave_c[contador], P_c[contador]);
                Cp_c[contador] = CorePointer.working_fluid.Cp;
            }

            //7.Prandtl Number: Prandtl = eta * Cpcalc / tcx / wm / 1000
            Double[] Prandtl_h = new Double[N_sub_hxrs];
            Double[] Prandtl_c = new Double[N_sub_hxrs];

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                CorePointer.working_fluid.FindStateWithTP(Tave_h[contador], P_h[contador]);
                Prandtl_h[contador] = (Viscosity_h[contador] * Cp_h[contador] / Thermal_Conductivity_h[contador] / CorePointer.wmm / 1000) * 1000000;
                CorePointer.working_fluid.FindStateWithTP(Tave_c[contador], P_c[contador]);
                Prandtl_c[contador] = (Viscosity_c[contador] * Cp_c[contador] / Thermal_Conductivity_c[contador] / CorePointer.wmm / 1000) * 1000000;
            }
            
            //Initialize the values for channels
            Channel_Heigh = Convert.ToDouble(this.textBox14.Text);
            Channel_Width = Convert.ToDouble(this.textBox15.Text);
            Distance_Between_Channels = Convert.ToDouble(this.textBox21.Text);
            Number_Channels_Heigh_perblock = Convert.ToDouble(this.textBox17.Text);
            Number_Channels_Width_perblock = Convert.ToDouble(this.textBox16.Text);
            Total_Number_Channels = Number_Channels_Heigh_perblock * Number_Channels_Width_perblock;
            Heigh_perblock = Number_Channels_Heigh_perblock * (Channel_Heigh + Channel_Width + Distance_Between_Channels * 2);
            Width_perblock = Number_Channels_Heigh_perblock * (Channel_Heigh + Channel_Width + Distance_Between_Channels * 2);
            textBox18.Text = Convert.ToString(Heigh_perblock);
            textBox19.Text = Convert.ToString(Width_perblock);
            textBox20.Text = Convert.ToString(Total_Number_Channels);

            Double MassFlow_cold_permodule, MassFlow_hot_permodule;
            MassFlow_cold_permodule = Convert.ToDouble(textBox3.Text) / Number_Modules;
            MassFlow_hot_permodule = Convert.ToDouble(textBox4.Text) / Number_Modules;

            Flow_cold = MassFlow_cold_permodule / Total_Number_Channels;
            Flow_hot = MassFlow_hot_permodule / Total_Number_Channels;

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Velocity_c[contador] = Flow_cold/(((3.1416*Channel_Width*Channel_Width)/2)*Density_c[contador]);
                Velocity_h[contador] = Flow_hot/(((3.1416*Channel_Width*Channel_Width)/2)*Density_h[contador]);
            }

            Hydraulic_Diameter = 4 * (3.1416 * Channel_Width * Channel_Width / 2) / ((Channel_Width + Channel_Width + 3.1416 * Channel_Width));

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Reynold_c[contador] = Densities_Viscosity_Ratio_c[contador] * Velocity_c[contador] * Hydraulic_Diameter;
                Reynold_h[contador] = Densities_Viscosity_Ratio_h[contador] * Velocity_h[contador] * Hydraulic_Diameter;
            }

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
               Darcy_c[contador] = Math.Pow(((0.79*Math.Log(Reynold_c[contador]))-1.64),-2);
               Darcy_h[contador] = Math.Pow(((0.79*Math.Log(Reynold_h[contador]))-1.64),-2);
            }

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                Nusselt_c[contador]= ((Darcy_c[contador]/8)*(Reynold_c[contador]-1000)*Prandtl_c[contador])/(1+12.7*(Math.Pow((Darcy_c[contador]/8),0.5))*(Math.Pow(Prandtl_c[contador],0.6666666666666)-1));
                Nusselt_h[contador] = ((Darcy_h[contador]/8)*(Reynold_h[contador]-1000)*Prandtl_h[contador])/(1 +12.7*(Math.Pow((Darcy_h[contador]/8),0.5))*(Math.Pow(Prandtl_h[contador],0.6666666666666)-1));
            }

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
               HTC_c [contador] = Nusselt_c[contador] * Thermal_Conductivity_c[contador]/Hydraulic_Diameter;
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

            textBox22.Text = Convert.ToString(Total_Length);

            Total_AP_c=0;
            Total_AP_h=0;

            for (int contador = 0; contador < N_sub_hxrs; contador++)
            {
                AP_c[contador] = (Length_local[contador] * Darcy_c[contador] * Density_c[contador] * Velocity_c[contador] * Velocity_c[contador] / (2 * Hydraulic_Diameter)) / 1000000;
                Total_AP_c = Total_AP_c + AP_c[contador];
                AP_h[contador] = (Length_local[contador] * Darcy_h[contador] * Density_h[contador] * Velocity_h[contador] * Velocity_h[contador] / (2 * Hydraulic_Diameter)) / 1000000;
                Total_AP_h = Total_AP_h + AP_h[contador];
            }

            textBox23.Text = Convert.ToString(Total_AP_c*10);
            textBox24.Text = Convert.ToString(Total_AP_h*10);

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

            textBox25.Text = Convert.ToString(HTC_hd[0]-250);

            chart2.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(textBox25.Text), 1, MidpointRounding.AwayFromZero));
            chart2.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(HTC_cd[14] + 500, 1, MidpointRounding.AwayFromZero));

            for (int k = 0; k < N_sub_hxrs; k++)
            {
                chart2.Series["Cold_Side"].Points.AddY(dataGridView4.Rows[0].Cells[k].Value);
                chart2.Series["Hot_Side"].Points.AddY(dataGridView3.Rows[0].Cells[k].Value);
            }

            //Prueba llamada a función 
            //PCHE_Detail_Design(long N_sub_hxrs, Double Q_dot, Double m_dot_c, Double m_dot_h, Double T_c_in, Double T_h_in,
            //                           Double P_c_in, Double P_c_out, Double P_h_in, Double P_h_out, Double Number_Modules,
            //                           Double Channel_Heigh, Double Channel_Width,Double Distance_Between_Channels, Double Number_Channels_Heigh_perblock,
            //                           Double Number_Channels_Width_perblock,ref Double UA, ref Double min_DT,ref Int64 error_code,
            //                           ref Double Total_Length, ref Double Total_AP_h, ref Double Total_AP_c)

            Double UA_PCHE = 0;
            Double min_DT_PCHE = 0;
            long error_code_PCHE = 0;
            Double Total_Length_PCHE = 0;
            Double Total_AP_h_PCHE = 0;
            Double Total_AP_c_PCHE = 0;

            CorePointer.Recuperators_PCHE_Detail_Design(N_sub_hxrs, Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out, Number_Modules, Channel_Heigh, Channel_Width, Distance_Between_Channels,
                                           Number_Channels_Width_perblock, Number_Channels_Heigh_perblock, ref UA_PCHE, ref min_DT_PCHE, ref error_code_PCHE, ref Total_Length_PCHE, ref Total_AP_h_PCHE, ref Total_AP_c_PCHE);

            textBox30.Text = Convert.ToString(Total_Length_PCHE);
            textBox29.Text = Convert.ToString(Total_AP_c_PCHE);
            textBox26.Text = Convert.ToString(Total_AP_h_PCHE);
        }

        //Calculate Button
        private void button1_Click(object sender, EventArgs e)
        {
            Calculate_HX();

            comboBox5_SelectedValueChanged(this, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {           

            //Temperatures
            if (comboBox5.Text == "Temperature")
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

                label20.Text = "Temperature_Cold_Side (K):";
                label19.Text = "Temperature_Hot_Side (K):";
            }

            //Conductance (UA)
            else if (comboBox5.Text == "Conductance (UA)")
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
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(UA_local_1[N_sub_hxrs-1])+decimal.Round(Convert.ToDecimal(UA_local_1[N_sub_hxrs-1]/4), 1, MidpointRounding.AwayFromZero)));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum)/10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["UA_Local"].Points.AddXY(k+1,UA_local_1[k]);
                }                

                label20.Text = "UA (kWth/K)";
                label19.Text = " ";
            }

            //Number of Transfer Units (NTU)
            else if (comboBox5.Text == "Nº Transfer Units (NTU)")
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
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(NTU_local_1[N_sub_hxrs - 1]) + Convert.ToDecimal(NTU_local_1[N_sub_hxrs - 1]/2), 1, MidpointRounding.AwayFromZero));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum) / 10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["NTU_Local"].Points.AddXY(k+1, NTU_local_1[k]);
                }

                label20.Text = "";
                label19.Text = "NTU";
            }

            //Capacity Ratio (CR)
            else if (comboBox5.Text == "Capacity Ratio (CR)")
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
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(CR_local_1[N_sub_hxrs - 1]) + Convert.ToDecimal(CR_local_1[N_sub_hxrs - 1] / 2), 1, MidpointRounding.AwayFromZero));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum) / 10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["CR_Local"].Points.AddXY(k+1, CR_local_1[k]);
                }

                label20.Text = "CR";
                label19.Text = "";
            }

            //Effectiveness
            else if (comboBox5.Text == "Effectiveness")
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
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(decimal.Round(Convert.ToDecimal(Effec_local_1[N_sub_hxrs - 1]) + Convert.ToDecimal(Effec_local_1[N_sub_hxrs - 1] / 2), 1, MidpointRounding.AwayFromZero));

                chart1.ChartAreas["ChartArea1"].AxisY.Interval = (chart1.ChartAreas["ChartArea1"].AxisY.Minimum + chart1.ChartAreas["ChartArea1"].AxisY.Maximum) / 10;
                chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();

                for (int k = 0; k < N_sub_hxrs; k++)
                {
                    chart1.Series["Eff_Local"].Points.AddXY(k+1, Effec_local_1[k]);
                }

                label20.Text = "Effectiveness";
                label19.Text = "";
            }
        }
    }
}
