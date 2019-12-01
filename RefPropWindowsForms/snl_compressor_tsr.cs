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
    public partial class snl_compressor_tsr : Form
    {
        public core luis11 = new core();
        public core.Compressor One_Stage_Compressor_design = new core.Compressor();
        public core.Compressor Two_Stage_Compressor_design = new core.Compressor();

        //Input Data:
        public RefrigerantCategory Fluid_Category;
        public ReferenceState Fluid_Reference_State;

        public snl_compressor_tsr()
        {
            InitializeComponent();
        }
        
        //Auxiliar variables
        public Double psi_design = 0;
        public Double m_dot = 0;

        //Input data
        public Double p_in;
        public Double t_in;
        public Double rho_in;
        public Double h_in;
        public Double s_in;

        public Double T_out;
        public Double P_out;
        public Double h_out;
        public Double rho_out;

        public Double m_dot_compressor;
        public Double recomp_frac;

        //Main Compressor Results outputs
        public Double D_rotor;
        public Double N;
        public Double eta;
        public Double phi;
        public Double phi_min;
        public Double phi_max;
        public Boolean surge;

        //Recompressor (Type 2 stages) Results outputs
        public Double D1_rotor;
        public Double D2_rotor;
        public Double N1;
        public Double eta1;
        public Double phi1;
        public Double phi1_min;
        public Double phi1_max;
        public Boolean surge1;

        //Off-Design Point Data Input and Output
        public Double p_in_offdesign;
        public Double t_in_offdesign;
        public Double p_out_offdesign;
        public Double N_offdesign;
        public Double error_code_offdesign;

        public Double m_dot_offdesign;
        public Double t_out_offdesign;

        public void Calculate_Main_Compressor()
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
            m_dot_compressor = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.Main_Compressor_Detail_Design(luis11, p_in, t_in, P_out, T_out, m_dot_compressor,
            recomp_frac, ref D_rotor, ref N, ref eta, ref surge, ref phi_min, ref phi_max, ref phi);

            textBox12.Text = Convert.ToString(D_rotor);
            textBox11.Text = Convert.ToString(N);
            textBox10.Text = Convert.ToString(eta);
            textBox13.Text = Convert.ToString(phi);
            textBox14.Text = Convert.ToString(surge);

            One_Stage_Compressor_design.D_rotor = D_rotor;
            One_Stage_Compressor_design.N_design = N;
            One_Stage_Compressor_design.eta_design = eta;
            One_Stage_Compressor_design.phi_design = phi;
            One_Stage_Compressor_design.phi_min = phi_min;
            One_Stage_Compressor_design.phi_max = phi_max;
            One_Stage_Compressor_design.surge = surge;
        
        }


        //Calculate Button for Main Compressor Design-Point detail design (Type SNL_compressor_tsr.f90 or SNL_compressor.f90)
        private void button2_Click(object sender, EventArgs e)
        {
            Calculate_Main_Compressor();
        }

        //Calculate Button for ReCompressor-TWo Stages Design-Point detail design (Type SNL_compressor_tsr.f90)
        public void button3_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            button6.Enabled = false;

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
            m_dot_compressor = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.ReCompressor_TWO_Stages_Detail_Design(luis11, p_in, t_in, P_out, T_out, m_dot_compressor,
            recomp_frac, ref D1_rotor, ref D2_rotor, ref N1, ref eta1, ref surge1, ref phi1_min, ref phi1_max, ref phi1);

            textBox12.Text = Convert.ToString(D1_rotor);
            textBox3.Text = Convert.ToString(D2_rotor);
            textBox11.Text = Convert.ToString(N1);
            textBox10.Text = Convert.ToString(eta1);
            textBox13.Text = Convert.ToString(phi1);
            textBox14.Text = Convert.ToString(surge1);

            Two_Stage_Compressor_design.D_rotor = D1_rotor;
            Two_Stage_Compressor_design.D_rotor_2 = D2_rotor;
            Two_Stage_Compressor_design.N_design = N1;
            Two_Stage_Compressor_design.eta_design = eta1;
            Two_Stage_Compressor_design.phi_design = phi1;
            Two_Stage_Compressor_design.phi_min = phi1_min;
            Two_Stage_Compressor_design.phi_max = phi1_max;
        }

        //Main Compressor Off-Design performance (Type snl_compressor.f90)
        private void button4_Click(object sender, EventArgs e)
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
            m_dot_compressor = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.Main_Compressor_Detail_Design(luis11, p_in, t_in, P_out, T_out, m_dot_compressor,
            recomp_frac, ref D_rotor, ref N, ref eta, ref surge,ref phi_min,ref phi_max,ref phi);

            textBox12.Text = Convert.ToString(D_rotor);
            textBox11.Text = Convert.ToString(N);
            textBox10.Text = Convert.ToString(eta);
            textBox13.Text = Convert.ToString(phi);
            textBox14.Text = Convert.ToString(surge);

            One_Stage_Compressor_design.D_rotor = D_rotor;
            One_Stage_Compressor_design.N_design = N;
            One_Stage_Compressor_design.eta_design = eta;
            One_Stage_Compressor_design.phi_design = phi;
            One_Stage_Compressor_design.phi_min = phi_min;
            One_Stage_Compressor_design.phi_max = phi_max;
            One_Stage_Compressor_design.surge = surge;

            p_in_offdesign = Convert.ToDouble(textBox15.Text);
            t_in_offdesign = Convert.ToDouble(textBox7.Text);
            p_out_offdesign = Convert.ToDouble(textBox4.Text);
            N_offdesign = Convert.ToDouble(textBox20.Text);

            m_dot_compressor = Convert.ToDouble(textBox21.Text);
            recomp_frac = Convert.ToDouble(textBox16.Text);
            m_dot_compressor=(m_dot_compressor-(m_dot_compressor * recomp_frac));

            luis11.SNL_Compressor_OffDesign(luis11, ref One_Stage_Compressor_design, p_in_offdesign, t_in_offdesign, p_out_offdesign,
                                       N_offdesign, ref error_code_offdesign, ref m_dot_compressor, ref t_out_offdesign);

            textBox19.Text = Convert.ToString(m_dot_compressor);
            textBox18.Text = Convert.ToString(t_out_offdesign);
            textBox17.Text = Convert.ToString(One_Stage_Compressor_design.eta);
        }

        //Recompressor ONE-Stage Off-Design performance (Type snl_compressor.f90)
        public void button6_Click(object sender, EventArgs e)
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
            m_dot_compressor = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.ReCompressor_Detail_Design(luis11, p_in, t_in, P_out, T_out, m_dot_compressor,
            recomp_frac, ref D_rotor, ref N, ref eta, ref surge, ref phi_min, ref phi_max, ref phi);

            textBox12.Text = Convert.ToString(D_rotor);
            textBox11.Text = Convert.ToString(N);
            textBox10.Text = Convert.ToString(eta);
            textBox13.Text = Convert.ToString(phi);
            textBox14.Text = Convert.ToString(surge);

            One_Stage_Compressor_design.D_rotor = D_rotor;
            One_Stage_Compressor_design.N_design = N;
            One_Stage_Compressor_design.eta_design = eta;
            One_Stage_Compressor_design.phi_design = phi;
            One_Stage_Compressor_design.phi_min = phi_min;
            One_Stage_Compressor_design.phi_max = phi_max;
            One_Stage_Compressor_design.surge = surge;

            p_in_offdesign = Convert.ToDouble(textBox15.Text);
            t_in_offdesign = Convert.ToDouble(textBox7.Text);
            p_out_offdesign = Convert.ToDouble(textBox4.Text);
            N_offdesign = Convert.ToDouble(textBox20.Text);

            m_dot_compressor = Convert.ToDouble(textBox21.Text);
            recomp_frac = Convert.ToDouble(textBox16.Text);

            m_dot_compressor = (m_dot_compressor * recomp_frac);

            luis11.SNL_ReCompressor_OffDesign(luis11, ref One_Stage_Compressor_design, p_in_offdesign, t_in_offdesign, p_out_offdesign,
                                       N_offdesign, ref error_code_offdesign, ref m_dot_compressor, ref t_out_offdesign);

            textBox19.Text = Convert.ToString(m_dot_compressor);
            textBox18.Text = Convert.ToString(t_out_offdesign);
            textBox17.Text = Convert.ToString(One_Stage_Compressor_design.eta);
        }

        //Recompressor TWO-Stage Off-Design performance (Type snl_compressor_tsr.f90)
        private void button5_Click(object sender, EventArgs e)
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
            m_dot_compressor = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.ReCompressor_TWO_Stages_Detail_Design(luis11, p_in, t_in, P_out, T_out, m_dot_compressor,
            recomp_frac, ref D1_rotor, ref D2_rotor, ref N, ref eta, ref surge, ref phi_min, ref phi_max, ref phi);

            textBox12.Text = Convert.ToString(D1_rotor);
            textBox3.Text = Convert.ToString(D2_rotor);
            textBox11.Text = Convert.ToString(N1);
            textBox10.Text = Convert.ToString(eta1);
            textBox13.Text = Convert.ToString(phi1);
            textBox14.Text = Convert.ToString(surge1);

            Two_Stage_Compressor_design.D_rotor = D1_rotor;
            Two_Stage_Compressor_design.D_rotor_2 = D2_rotor;
            Two_Stage_Compressor_design.N_design = N1;
            Two_Stage_Compressor_design.eta_design = eta1;
            Two_Stage_Compressor_design.phi_design = phi1;
            Two_Stage_Compressor_design.phi_min = phi1_min;
            Two_Stage_Compressor_design.phi_max = phi1_max;

            p_in_offdesign = Convert.ToDouble(textBox15.Text);
            t_in_offdesign = Convert.ToDouble(textBox7.Text);
            p_out_offdesign = Convert.ToDouble(textBox4.Text);
            N_offdesign = Convert.ToDouble(textBox20.Text);

            m_dot_compressor = Convert.ToDouble(textBox21.Text);
            recomp_frac = Convert.ToDouble(textBox16.Text);

            m_dot_compressor = (m_dot_compressor * recomp_frac);

            luis11.SNL_ReCompressor_TWO_Stages_OffDesign(luis11, ref Two_Stage_Compressor_design, p_in_offdesign, t_in_offdesign,
                                          p_out_offdesign, N_offdesign, ref error_code_offdesign, ref m_dot_compressor, ref T_out);

            textBox19.Text = Convert.ToString(m_dot_compressor);
            textBox18.Text = Convert.ToString(T_out);
            textBox17.Text = Convert.ToString(Two_Stage_Compressor_design.eta);

        }

        //Recompressor ONE-Stage Design-Point detail design 
        public void button7_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button6.Enabled = true;

            textBox3.Text = "N/A";
            
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
            m_dot_compressor = Convert.ToDouble(textBox9.Text);
            recomp_frac = Convert.ToDouble(textBox8.Text);

            luis11.ReCompressor_Detail_Design(luis11, p_in, t_in, P_out, T_out, m_dot_compressor,
            recomp_frac, ref D_rotor, ref N, ref eta, ref surge, ref phi_min, ref phi_max, ref phi);

            textBox12.Text = Convert.ToString(D_rotor);
            textBox11.Text = Convert.ToString(N);
            textBox10.Text = Convert.ToString(eta);
            textBox13.Text = Convert.ToString(phi);
            textBox14.Text = Convert.ToString(surge);

            One_Stage_Compressor_design.D_rotor = D_rotor;
            One_Stage_Compressor_design.N_design = N;
            One_Stage_Compressor_design.eta_design = eta;
            One_Stage_Compressor_design.phi_design = phi;
            One_Stage_Compressor_design.phi_min = phi_min;
            One_Stage_Compressor_design.phi_max = phi_max;
            One_Stage_Compressor_design.surge = surge;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
