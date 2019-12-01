﻿using System;
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
    public partial class IsoentropicEfficiency : Form
    {
        public core luis = new core();

        //Input Data:
        public RefrigerantCategory Fluid_Category;
        public ReferenceState Fluid_Reference_State;

        public Int64 error_code;
        public Double T_in, P_in, P_out, poly_eta, isen_eta; 
        public Boolean is_comp;

        public IsoentropicEfficiency()
        {
            InitializeComponent();
        }

        //Button "Calculate"
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

            T_in=Convert.ToDouble(textBox1.Text);
            P_in = Convert.ToDouble(textBox2.Text);
            P_out = Convert.ToDouble(textBox3.Text);
            poly_eta = Convert.ToDouble(textBox4.Text);
           
            string var;
            var = comboBox1.Text;
			
            //var item = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            //MessageBox.Show(var);

            if (var == "True")
            {
                is_comp = true;
            }

            if (var == "False")
            {
                is_comp = false;
            }

            luis.isen_eta_from_poly_eta(luis,T_in, P_in, P_out, poly_eta, is_comp, ref error_code, ref isen_eta);

            textBox6.Text = Convert.ToString(isen_eta);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        } 
    }
}
