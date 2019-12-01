using System;
using NLoptNet;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Validation with CsNumerics library!!!

            //LN_COBYLA Algorithm Example (COBYLA)
            //LN_BOBYQA Algorithm Example (BOBYQA)
            //LN_SBPLX Algotithm Example (SUBPLEX)
            using (var solver = new NLoptSolver(NLoptAlgorithm.LN_SBPLX, 1, 0.001, 100))
            {
                solver.SetLowerBounds(new[] { -10.0 });
                solver.SetUpperBounds(new[] { 100.0 });

                solver.SetInitialStepSize(new[] {0.01});

                //solver.SetMinObjective(variables => Math.Pow(variables[0] - 3.0, 2.0) + 4.0);

                //Func<double[], double> funcion = variables => Math.Pow(variables[0] - 3.0, 2.0) + 4.0;

                Func<double[], double> funcion = delegate (double [] variables)
                {

                    return Math.Pow(variables[0] - 3.0, 2.0) + 4.0;

                };

                solver.SetMinObjective(funcion);

                double? finalScore;
                var initialValue = new[] { 2.0 };
                var result = solver.Optimize(initialValue, out finalScore);

                Console.WriteLine(result.ToString());
                Console.WriteLine(initialValue[0].ToString());
                Console.WriteLine(finalScore.GetValueOrDefault().ToString());

                Console.ReadLine();
                //Assert.AreEqual(NloptResult.XTOL_REACHED, result);
                //Assert.AreEqual(3.0, initialValue[0], 0.1);
                //Assert.AreEqual(4.0, finalScore.GetValueOrDefault(), 0.1);
            }

            //Console.WriteLine("Hello World!");
        }
    }
}
