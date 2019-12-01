using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Accord;
using Accord.Math.Optimization;

using NUnit.Framework;

namespace Accord_Subplex_Validation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConstructorTest4();
        }

        public void ConstructorTest4()
        {
            //Weak version of Rosenbrock's problem.
            var function = new NonlinearObjectiveFunction(2, x =>
                Math.Pow(x[0] * x[0] - x[1], 2.0) + Math.Pow(1.0 + x[0], 2.0));

            Subplex solver = new Subplex(function);

            Assert.IsTrue(solver.Minimize());
            double minimum = solver.Value;
            double[] solution = solver.Solution;

            Assert.AreEqual(2, solution.Length);
            Assert.AreEqual(0, minimum, 1e-10);
            Assert.AreEqual(-1, solution[0], 1e-5);
            Assert.AreEqual(1, solution[1], 1e-4);

            double expectedMinimum = function.Function(solver.Solution);
            Assert.AreEqual(expectedMinimum, minimum);
        }
    }
}
