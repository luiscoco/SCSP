using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using sc.net;

namespace RefPropWindowsForms
{
    public partial class snl_radial_turbine : Form
    {
        public core luis11 = new core();
        public core.Turbine Turbine_design = new core.Turbine();

        //Input Data:
        public RefrigerantCategory Fluid_Category;
        public ReferenceState Fluid_Reference_State;

        //Input data
        public Double p_in;
        public Double t_in;

        public Double T_out;
        public Double P_out;

        public Double m_dot_turbine;
        public Double recomp_frac;

        public Double D_turbine;
        public Double N_design;
        public Double N;
        public Double eta;
        public Double A_nozzle;
        public Double nu;
        public Double w_tip_ratio;

        //Off-Design Point Data Input and Output
        public Double p_in_offdesign;
        public Double t_in_offdesign;
        public Double p_out_offdesign;
        public Double N_offdesign;
        public Double error_code_offdesign;

        public Double m_dot_offdesign;
        public Double t_out_offdesign;
        
        public snl_radial_turbine()
        {
            InitializeComponent();
        }

        // Radial_Turbine Design-Point Performance
        public void button2_Click(object sender, EventArgs e)
        {
            calculate_snl_Radial_Turbine();
        }

        // SNL_Turbine_Turbine Design-Point detail design
        public void calculate_snl_Radial_Turbine()
        {
            if (comboBox1.Text == "PureFluid")
            {
                Fluid_Category = RefrigerantCategory.PureFluid;
            }
            if (comboBox1.Text == "PredefinedMixture")
            {
                Fluid_Category = RefrigerantCategory.PredefinedMixture;
            }
            if (comboBox1.Text == "NewMixture")
            {
                Fluid_Category = RefrigerantCategory.NewMixture;
            }
            if (comboBox1.Text == "PseudoPureFluid")
            {
                Fluid_Category = RefrigerantCategory.PseudoPureFluid;
            }

            if (comboBox3.Text == "DEF")
            {
                Fluid_Reference_State = ReferenceState.DEF;
            }
            if (comboBox3.Text == "ASH")
            {
                Fluid_Reference_State = ReferenceState.ASH;
            }
            if (comboBox3.Text == "IIR")
            {
                Fluid_Reference_State = ReferenceState.IIR;
            }
            if (comboBox3.Text == "NBP")
            {
                Fluid_Reference_State = ReferenceState.NBP;
            }

            luis11.core1(this.comboBox2.Text, Fluid_Category);
            luis11.working_fluid.Category = Fluid_Category;
            luis11.working_fluid.reference = Fluid_Reference_State;

            p_in = Convert.ToDouble(textBox1.Text);
            t_in = Convert.ToDouble(textBox2.Text);
            P_out = Convert.ToDouble(textBox6.Text);
            T_out = Convert.ToDouble(textBox5.Text);
            N_design = Convert.ToDouble(textBox3.Text);
            m_dot_turbine = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.snl_radial_turbine(luis11, p_in, t_in, P_out, T_out, m_dot_turbine, N_design, ref D_turbine, ref A_nozzle,
                                      ref eta, ref N, ref nu, ref w_tip_ratio);

            textBox12.Text = Convert.ToString(D_turbine);
            textBox11.Text = Convert.ToString(N);
            textBox10.Text = Convert.ToString(eta);
            textBox13.Text = Convert.ToString(A_nozzle);
        }

        // SNL_Turbine_Turbine Off-Design performance
        private void button3_Click(object sender, EventArgs e)
        {
            // First Design-Point conditions are calculated 

            if (comboBox1.Text == "PureFluid")
            {
                Fluid_Category = RefrigerantCategory.PureFluid;
            }
            if (comboBox1.Text == "PredefinedMixture")
            {
                Fluid_Category = RefrigerantCategory.PredefinedMixture;
            }
            if (comboBox1.Text == "NewMixture")
            {
                Fluid_Category = RefrigerantCategory.NewMixture;
            }
            if (comboBox1.Text == "PseudoPureFluid")
            {
                Fluid_Category = RefrigerantCategory.PseudoPureFluid;
            }

            if (comboBox3.Text == "DEF")
            {
                Fluid_Reference_State = ReferenceState.DEF;
            }
            if (comboBox3.Text == "ASH")
            {
                Fluid_Reference_State = ReferenceState.ASH;
            }
            if (comboBox3.Text == "IIR")
            {
                Fluid_Reference_State = ReferenceState.IIR;
            }
            if (comboBox3.Text == "NBP")
            {
                Fluid_Reference_State = ReferenceState.NBP;
            }

            luis11.core1(this.comboBox2.Text, Fluid_Category);
            luis11.working_fluid.Category = Fluid_Category;
            luis11.working_fluid.reference = Fluid_Reference_State;

            p_in = Convert.ToDouble(textBox1.Text);
            t_in = Convert.ToDouble(textBox2.Text);
            P_out = Convert.ToDouble(textBox6.Text);
            T_out = Convert.ToDouble(textBox5.Text);
            N_design = Convert.ToDouble(textBox3.Text);
            m_dot_turbine = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.snl_radial_turbine(luis11, p_in, t_in, P_out, T_out, m_dot_turbine, N_design,
                                     ref D_turbine, ref A_nozzle, ref eta, ref N, ref nu, ref w_tip_ratio);

            textBox12.Text = Convert.ToString(D_turbine);
            textBox11.Text = Convert.ToString(N);
            textBox10.Text = Convert.ToString(eta);
            textBox13.Text = Convert.ToString(A_nozzle);
            textBox14.Text = Convert.ToString(nu);
            textBox16.Text = Convert.ToString(w_tip_ratio);

            Turbine_design.A_nozzle = A_nozzle;
            Turbine_design.N_design = N;
            Turbine_design.eta_design = eta;
            Turbine_design.D_rotor = D_turbine;
            Turbine_design.nu = nu;
            Turbine_design.w_tip_ratio = w_tip_ratio;

            //Second with the Design-Point results we calculate the Off-Design Point conditions
            p_in_offdesign = Convert.ToDouble(textBox15.Text);
            t_in_offdesign = Convert.ToDouble(textBox7.Text);
            p_out_offdesign = Convert.ToDouble(textBox4.Text);
            N_offdesign = Convert.ToDouble(textBox20.Text);

            luis11.SNL_Turbine_OffDesign(luis11, ref Turbine_design, p_in_offdesign, t_in_offdesign, p_out_offdesign,
                                           N_offdesign, ref error_code_offdesign, ref m_dot_offdesign, ref t_out_offdesign);

            textBox19.Text = Convert.ToString(m_dot_offdesign);
            textBox18.Text = Convert.ToString(t_out_offdesign);
            textBox17.Text = Convert.ToString(Turbine_design.eta);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
