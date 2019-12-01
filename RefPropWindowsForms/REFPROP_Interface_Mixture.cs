using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using sc.net;
using System.Diagnostics;

namespace RefPropWindowsForms
{
    public partial class REFPROP_Interface_Mixture : Form
    {
        public core luis = new core();

        //Input Data:
        public RefrigerantCategory Fluid_Category;
        public ReferenceState Fluid_Reference_State;
        public Refrigerant working_fluid;

        public REFPROP_Interface_Mixture()
        {
            InitializeComponent();
        }

        //OK Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Calculate Button
        private void button1_Click(object sender, EventArgs e)
        {
            
            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, comboBox2.Text + "=" + textBox10.Text + "," + comboBox4.Text + "=" + textBox9.Text, ReferenceState.DEF);

            //Console.WriteLine(myRefrigerant.MolecularWeight);
            //myRefrigerant.FindSaturatedStateWithTemperature(273, SaturationPoint.Dew_Point);
            //myRefrigerant.DisplayThermoDynamicState();

            working_fluid.FindStateWithTP(Convert.ToDouble(this.textBox2.Text), Convert.ToDouble(this.textBox1.Text));
            //myRefrigerant.DisplayThermoDynamicState();

            this.textBox3.Text = working_fluid.Density.ToString();
            this.textBox4.Text = working_fluid.Enthalpy.ToString();
            this.textBox5.Text = working_fluid.Entropy.ToString();
            this.textBox11.Text = working_fluid.MolecularWeight.ToString();

            this.textBox7.Text = working_fluid.CriticalTemperature.ToString();
            this.textBox6.Text = working_fluid.CriticalPressure.ToString();
            this.textBox8.Text = working_fluid.CriticalDensity.ToString();

            this.textBox12.Text = working_fluid.Cp.ToString();
            this.textBox13.Text = working_fluid.thermalconductivity.ToString();
            this.textBox14.Text = working_fluid.Viscosity.ToString();
            //this.textBox15.Text = working_fluid.Cp.ToString();
            this.textBox16.Text = working_fluid.MolecularWeight.ToString();

            //myRefrigerant.FindStateWithTD(300, 40 / myRefrigerant.MolecularWeight);
            //myRefrigerant.DisplayThermoDynamicState();

            //myRefrigerant.Display();

            //System.Console.ReadKey();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //CO2+METHANE+ETHANE
            //CO2+HELIUM+ARGON
            //CO2+CARBON_MONOXIDE+HYDROGEN_SULFIDE


            Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, "CO2=0.99,ARGON=0.01", ReferenceState.DEF);
            working_fluid.FindStateWithTP(823.15,25000);
            this.textBox3.Text = working_fluid.Density.ToString();
            this.textBox4.Text = working_fluid.Enthalpy.ToString();
            this.textBox5.Text = working_fluid.Entropy.ToString();
            this.textBox11.Text = working_fluid.MolecularWeight.ToString();

            this.textBox7.Text = working_fluid.CriticalTemperature.ToString();
            this.textBox6.Text = working_fluid.CriticalPressure.ToString();
            this.textBox8.Text = working_fluid.CriticalDensity.ToString();
        }

        //Compile_Mixture
        private void button20_Click(object sender, EventArgs e)
        {
            
        }
    }
}
