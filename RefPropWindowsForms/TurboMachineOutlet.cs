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
    public partial class TurboMachineOutlet : Form
    {
        public core luis = new core();
        public Int64 error_code;

        //Input Data:
        public RefrigerantCategory Fluid_Category;
        public ReferenceState Fluid_Reference_State;

        //Input variables: T_in, P_in, P_out, eta
        public Double T_in, P_in, P_out, eta;
        public Boolean is_comp;
        
        //Output variables: enth_in, entr_in, dens_in,  temp_out,  enth_out,  entr_out,  dens_out, spec_work
        public Double enth_in, entr_in, dens_in,  temp_out,  enth_out,  entr_out,  dens_out, spec_work;

        public TurboMachineOutlet()
        {
            InitializeComponent();
        }

        //Calculate Button
        private void button1_Click(object sender, EventArgs e)
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

            luis.core1(this.comboBox2.Text, Fluid_Category);
            luis.working_fluid.Category = Fluid_Category;
            luis.working_fluid.reference = Fluid_Reference_State;

            T_in = Convert.ToDouble(textBox1.Text);
            P_in = Convert.ToDouble(textBox3.Text);
            P_out = Convert.ToDouble(textBox2.Text);
            eta = Convert.ToDouble(textBox4.Text);

            if (comboBox1.Text == "True")
            {
                is_comp = true;
            }

            if (comboBox1.Text == "False")
            {
                is_comp = false;
            }

            luis.calculate_turbomachine_outlet(luis,T_in,P_in,P_out,eta,is_comp,ref error_code,ref enth_in,ref entr_in, ref dens_in, ref temp_out, ref enth_out, ref entr_out, ref dens_out, ref spec_work);

            textBox10.Text = Convert.ToString(enth_in);
            textBox8.Text = Convert.ToString(entr_in);
            textBox9.Text = Convert.ToString(dens_in);
            textBox7.Text = Convert.ToString(temp_out);
            textBox6.Text = Convert.ToString(enth_out);
            textBox12.Text = Convert.ToString(entr_out);
            textBox11.Text = Convert.ToString(dens_out);
            textBox13.Text = Convert.ToString(spec_work);
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
