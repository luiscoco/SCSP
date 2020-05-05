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
    public partial class LF_SF_Calculation : Form
    {
        public double ReflectorApertureAreaResult;
        public double Total_Pressure_DropResult;

        public core luis = new core();

        public Fresnel SF_PHX = new Fresnel();

        public bool calledForSensingAnalysis = false;

        //Input Data:
        public RefrigerantCategory category;
        public ReferenceState referencestate;
        public LF_SF_Calculation()
        {
            InitializeComponent();

            SF_PHX.SF_Type_variable = "Main_SF";
        }

        public void button1_Click(object sender, EventArgs e)
        {

            if (comboBox2.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                luis.core1(this.comboBox13.Text, category);
            }

            //NewMixture
            if (comboBox2.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                luis.core1(this.comboBox13.Text + "=" + textBox31.Text + "," + this.comboBox14.Text + "=" + textBox36.Text, category);
            }           

            if (comboBox1.Text == "Solar Salt")
            {
                SF_PHX.comboBox1.Text = "Solar Salt";
            }
            else if (comboBox1.Text == "Hitec XL")
            {
                SF_PHX.comboBox1.Text = "Hitec XL";
            }
            else if (comboBox1.Text == "Therminol VP1")
            {
                SF_PHX.comboBox1.Text = "Therminol VP1";
            }
            else if (comboBox1.Text == "Syltherm_800")
            {
                SF_PHX.comboBox1.Text = "Syltherm_800";
            }
            else if (comboBox1.Text == "Dowtherm_A")
            {
                SF_PHX.comboBox1.Text = "Dowtherm_A";
            }
            else if (comboBox1.Text == "Therminol_75")
            {
                SF_PHX.comboBox1.Text = "Therminol_75";
            }
            else if (comboBox1.Text == "Liquid Sodium")
            {
                SF_PHX.comboBox1.Text = "Liquid Sodium";
            }

            SF_PHX.textBox41.Text = textBox1.Text;
            SF_PHX.textBox40.Text = textBox2.Text;
            SF_PHX.textBox38.Text = textBox3.Text;
            SF_PHX.textBox36.Text = textBox4.Text;
            SF_PHX.textBox35.Text = textBox5.Text;
            SF_PHX.LF_Solar_Field_uno(luis);
            SF_PHX.textBox37.Text = Convert.ToString(Convert.ToDouble(textBox6.Text) + Convert.ToDouble(textBox107.Text));
            SF_PHX.Load_ComboBox7();
            SF_PHX.button3_Click(this, e);
            ReflectorApertureAreaResult = SF_PHX.ReflectorApertureArea;
            Total_Pressure_DropResult = SF_PHX.Total_Pressure_Drop;

            if (!calledForSensingAnalysis)
            {
                SF_PHX.Show();
            }
        }

        //OK Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "PureFluid")
            {
                comboBox14.Enabled = false;
                textBox31.Enabled = false;
                textBox36.Enabled = false;
            }

            else if (comboBox2.Text == "NewMixture")
            {
                comboBox14.Enabled = true;
                textBox31.Enabled = true;
                textBox36.Enabled = true;
            }
        }
    }
}
