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
    public partial class Dual_Loop_SF_Calculation : Form
    {
        public PTC_Solar_Field SF_PHX;
        public Fresnel SF_PHX_LF;

        public core luis = new core();

        public RefrigerantCategory category;
        public ReferenceState referencestate;

        Double PHX1_Q;
        Double massflow2;

        Double PHX1_temp_out;
        Double PHX1_pres_out;

        public Dual_Loop_SF_Calculation()
        {
            InitializeComponent();
        }

        //Ok button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Dual_Loop_Solar_Field_1
        private void button17_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                luis.core1(this.comboBox13.Text, category);
            }

            //NewMixture
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                luis.core1(this.comboBox13.Text + "=" + textBox31.Text + "," + this.comboBox14.Text + "=" + textBox36.Text, category);
            }

            if (comboBox11.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();

                if (comboBox9.Text == "Solar Salt")
                {
                    SF_PHX.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox9.Text == "Hitec XL")
                {
                    SF_PHX.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox9.Text == "Therminol VP1")
                {
                    SF_PHX.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox9.Text == "Syltherm_800")
                {
                    SF_PHX.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox9.Text == "Dowtherm_A")
                {
                    SF_PHX.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox9.Text == "Therminol_75")
                {
                    SF_PHX.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox9.Text == "Liquid Sodium")
                {
                    SF_PHX.comboBox1.Text = "Liquid Sodium";
                }

                Double temp25 = Convert.ToDouble(textBox1.Text);
                Double pres25 = Convert.ToDouble(textBox2.Text);
                luis.working_fluid.FindStateWithTP(temp25, pres25); // properties at the inlet conditions

                Double PHX1_h_in = luis.working_fluid.Enthalpy;
                       PHX1_temp_out = Convert.ToDouble(textBox6.Text);
                Double DP_PHX1 = Convert.ToDouble(textBox3.Text);
                       PHX1_pres_out = pres25 - pres25 * DP_PHX1;

                luis.working_fluid.FindStateWithTP(PHX1_temp_out, PHX1_pres_out); // properties at the inlet conditions
                Double PHX1_h_out = luis.working_fluid.Enthalpy;
                massflow2 = Convert.ToDouble(textBox4.Text);
                PHX1_Q = massflow2 * (PHX1_h_out - PHX1_h_in);

                SF_PHX.textBox41.Text = Convert.ToString(PHX1_Q);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(temp25);
                SF_PHX.textBox36.Text = Convert.ToString(pres25);
                SF_PHX.textBox37.Text = Convert.ToString(Convert.ToDouble(textBox6.Text) + Convert.ToDouble(textBox107.Text));
                SF_PHX.textBox35.Text = Convert.ToString(PHX1_pres_out);                
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.SF_Type_variable = "Main_SF1_Dual_Loop";            
                SF_PHX.button3_Click(this, e);
                SF_PHX.Show();
            }

            else if (comboBox11.Text == "Fresnel")
            {
                SF_PHX_LF = new Fresnel();

                if (comboBox9.Text == "Solar Salt")
                {
                    SF_PHX_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox9.Text == "Hitec XL")
                {
                    SF_PHX_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox9.Text == "Therminol VP1")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox9.Text == "Syltherm_800")
                {
                    SF_PHX_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox9.Text == "Dowtherm_A")
                {
                    SF_PHX_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox9.Text == "Therminol_75")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox9.Text == "Liquid Sodium")
                {
                    SF_PHX_LF.comboBox1.Text = "Liquid Sodium";
                }

                Double temp25 = Convert.ToDouble(textBox1.Text);
                Double pres25 = Convert.ToDouble(textBox2.Text);
                luis.working_fluid.FindStateWithTP(temp25, pres25); // properties at the inlet conditions

                Double PHX1_h_in = luis.working_fluid.Enthalpy;
                PHX1_temp_out = Convert.ToDouble(textBox6.Text);
                Double DP_PHX1 = Convert.ToDouble(textBox3.Text);
                PHX1_pres_out = pres25 - pres25 * DP_PHX1;

                luis.working_fluid.FindStateWithTP(PHX1_temp_out, PHX1_pres_out); // properties at the inlet conditions
                Double PHX1_h_out = luis.working_fluid.Enthalpy;
                massflow2 = Convert.ToDouble(textBox4.Text);
                PHX1_Q = massflow2 * (PHX1_h_out - PHX1_h_in);

                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX1_Q);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(temp25);
                SF_PHX_LF.textBox36.Text = Convert.ToString(pres25);
                SF_PHX_LF.textBox37.Text = Convert.ToString(Convert.ToDouble(textBox6.Text) + Convert.ToDouble(textBox107.Text));
                SF_PHX_LF.textBox35.Text = Convert.ToString(PHX1_pres_out);
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.SF_Type_variable = "Main_SF1_Dual_Loop";
                SF_PHX_LF.Load_ComboBox7();
                SF_PHX_LF.button3_Click(this, e);
                SF_PHX_LF.Show();
            }
        }

        //Dual_Loop_Solar_Field_2
        private void button18_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                luis.core1(this.comboBox13.Text, category);
            }

            //NewMixture
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                luis.core1(this.comboBox13.Text + "=" + textBox31.Text + "," + this.comboBox14.Text + "=" + textBox36.Text, category);
            }

            if (comboBox10.Text == "Parabolic")
            {
                SF_PHX = new PTC_Solar_Field();

                if (comboBox8.Text == "Solar Salt")
                {
                    SF_PHX.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox8.Text == "Hitec XL")
                {
                    SF_PHX.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox8.Text == "Therminol VP1")
                {
                    SF_PHX.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox8.Text == "Syltherm_800")
                {
                    SF_PHX.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox8.Text == "Dowtherm_A")
                {
                    SF_PHX.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox8.Text == "Therminol_75")
                {
                    SF_PHX.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox8.Text == "Liquid Sodium")
                {
                    SF_PHX.comboBox1.Text = "Liquid Sodium";
                }

                Double PHX_Q2 = Convert.ToDouble(textBox5.Text);

                Double PHX2_Q = PHX_Q2 - PHX1_Q;

                Double pres26 = Convert.ToDouble(textBox7.Text);

                SF_PHX.textBox41.Text = Convert.ToString(PHX2_Q);
                SF_PHX.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX.textBox38.Text = Convert.ToString(PHX1_temp_out);
                SF_PHX.textBox36.Text = Convert.ToString(PHX1_pres_out);
                SF_PHX.textBox35.Text = Convert.ToString(pres26);
                SF_PHX.textBox37.Text = Convert.ToString(Convert.ToDouble(textBox8.Text) + Convert.ToDouble(textBox107.Text));
                SF_PHX.PTC_Solar_Field_uno(luis);
                SF_PHX.SF_Type_variable = "Main_SF2_Dual_Loop";                
                SF_PHX.button3_Click(this, e);
                SF_PHX.Show();
            }

            else if (comboBox10.Text == "Fresnel")
            {
                SF_PHX_LF = new Fresnel();

                if (comboBox8.Text == "Solar Salt")
                {
                    SF_PHX_LF.comboBox1.Text = "Solar Salt";
                }
                else if (comboBox8.Text == "Hitec XL")
                {
                    SF_PHX_LF.comboBox1.Text = "Hitec XL";
                }
                else if (comboBox8.Text == "Therminol VP1")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol VP1";
                }
                else if (comboBox8.Text == "Syltherm_800")
                {
                    SF_PHX_LF.comboBox1.Text = "Syltherm_800";
                }
                else if (comboBox8.Text == "Dowtherm_A")
                {
                    SF_PHX_LF.comboBox1.Text = "Dowtherm_A";
                }
                else if (comboBox8.Text == "Therminol_75")
                {
                    SF_PHX_LF.comboBox1.Text = "Therminol_75";
                }
                else if (comboBox8.Text == "Liquid Sodium")
                {
                    SF_PHX_LF.comboBox1.Text = "Liquid Sodium";
                }

                Double PHX_Q2 = Convert.ToDouble(textBox5.Text);

                Double PHX2_Q = PHX_Q2 - PHX1_Q;

                Double pres26 = Convert.ToDouble(textBox7.Text);

                SF_PHX_LF.textBox41.Text = Convert.ToString(PHX2_Q);
                SF_PHX_LF.textBox40.Text = Convert.ToString(massflow2);
                SF_PHX_LF.textBox38.Text = Convert.ToString(PHX1_temp_out);
                SF_PHX_LF.textBox36.Text = Convert.ToString(PHX1_pres_out);
                SF_PHX_LF.textBox35.Text = Convert.ToString(pres26);
                SF_PHX_LF.textBox37.Text = Convert.ToString(Convert.ToDouble(textBox8.Text) + Convert.ToDouble(textBox107.Text));
                SF_PHX_LF.LF_Solar_Field_uno(luis);
                SF_PHX_LF.SF_Type_variable = "Main_SF2_Dual_Loop";                
                SF_PHX_LF.Load_ComboBox7();
                SF_PHX_LF.button3_Click(this, e);
                SF_PHX_LF.Show();
            }
        }

        private void Dual_Loop_SF_Calculation_Load(object sender, EventArgs e)
        {
            comboBox9.Enabled = true;
            comboBox11.Enabled = true;
            comboBox8.Enabled = true;
            comboBox10.Enabled = true;
            button17.Enabled = true;
            button18.Enabled = true;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PureFluid")
            {
                comboBox14.Enabled = false;
                textBox31.Enabled = false;
                textBox36.Enabled = false;
            }

            else if (comboBox1.Text == "NewMixture")
            {
                comboBox14.Enabled = true;
                textBox31.Enabled = true;
                textBox36.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
