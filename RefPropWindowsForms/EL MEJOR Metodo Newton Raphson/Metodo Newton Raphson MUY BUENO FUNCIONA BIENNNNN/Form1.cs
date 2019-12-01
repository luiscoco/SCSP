using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NumericalMethods;
using NumericalMethods.FourthBlog;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Parameter E1 = new Parameter(1.0);
            Parameter E2 = new Parameter(1.0);
            //Parameter E3 = new Parameter(0.0);
            //Parameter E4 = new Parameter(0.0);

            //double a = Convert.ToDouble(textBox5.Text);
            //double b = Convert.ToDouble(textBox6.Text);
            //double c = Convert.ToDouble(textBox7.Text);
            //double d = Convert.ToDouble(textBox8.Text);

            //Func<double>[] functions = new Func<double>[]
            //{
            //    () => a * (100.0 - E1 - 2.0 * E2) * (1.0 - E1 - E3) - 100.0 * E1,
            //    () => b * Math.Pow(100.0 - E1 - 2.0 * E2, 2.0) - 100.0 * E2,
            //    () => 0.5 * (100.0 - E1 - E3 - 2.0 * E4) - c * E3,
            //    () => d * Math.Pow(100.0 * E3 - 2.0 * E4, 2.0) - 100.0 * E4
            //};

            Func<double>[] functions = new Func<double>[]
            {
                () => (Math.Pow(E1, 2))+E1*E2-10,
                () => E2+(3*E1*(Math.Pow(E2, 2)))-57
            };

            //Parameter[] parameters = new Parameter[] { E1, E2, E3, E4 };

            Parameter[] parameters = new Parameter[] { E1, E2 };

            NewtonRaphson nr = new NewtonRaphson(parameters, functions);
            for (int i = 0; i < 15; i++)
            {
                nr.Iterate();
            }

            textBox1.Text = Convert.ToString(E1.Value);
            textBox2.Text = Convert.ToString(E2.Value);
            //textBox3.Text = Convert.ToString(E3.Value);
            //textBox4.Text = Convert.ToString(E4.Value);
        }
    }
}
