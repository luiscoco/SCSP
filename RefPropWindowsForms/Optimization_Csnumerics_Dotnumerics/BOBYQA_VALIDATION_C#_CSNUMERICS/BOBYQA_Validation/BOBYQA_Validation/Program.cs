using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cureos.Numerics.Optimizers;

namespace BOBYQA_Validation
{
    class AddDemo
    {
        public const double TOL = 1.0e-5;

        public void FindMinimum_IntermedRosenbrock_ReturnsValidMinimum()
        {
            var optimizer = new Bobyqa(
                2,
                (n, x) => 10.0 * Math.Pow(x[0] * x[0] - x[1], 2.0) + Math.Pow(1.0 + x[0], 2.0));
            var summary = optimizer.FindMinimum(new[] { 1.0, -1.0 });

            //Assert.AreEqual(-1.0, summary.X[0], TOL);
            //Assert.AreEqual(1.0, summary.X[1], TOL);
        }

        //[Test]
        public void FindMinimum_HS04_ReturnsValidMinimum()
        {
            var optimizer = new Bobyqa(2, (n, x) => Math.Pow(x[0] + 1.0, 3.0) / 3.0 + x[1], new[] { 1.0, 0.0 }, null)
            {
                InterpolationConditions = 4
            };
            var result = optimizer.FindMinimum(new[] { 1.125, 0.125 });

           // Assert.AreEqual(1.0, result.X[0], TOL);
           // Assert.AreEqual(0.0, result.X[1], TOL);
        }

        //[Test]
        public void FindMinimum_HS05_ReturnsValidMinimum()
        {
            var optimizer = new Bobyqa(
                2,
                (n, x) => Math.Sin(x[0] + x[1]) + Math.Pow(x[0] - x[1], 2.0) - 1.5 * x[0] + 2.5 * x[1] + 1,
                new[] { -1.5, -3.0 },
                new[] { 4.0, 3.0 });
            var result = optimizer.FindMinimum(new[] { 0.0, 0.0 });

          //  Assert.AreEqual(0.5 - Math.PI / 3.0, result.X[0], TOL);
          //  Assert.AreEqual(-0.5 - Math.PI / 3.0, result.X[1], TOL);
        }

        //THIS IS THE OFICIAL VALIDATION PROBLEMS:
        //[TestCase(5, 16)]
        //[TestCase(5, 21)]
        //[TestCase(10, 26)]
        //[TestCase(10, 41)]
        public void FindMinimum_BobyqaTestCase_ReturnStatusNormal(int m, int npt)
        {
            //     Test problem for BOBYQA, the objective function being the sum of
            //     the reciprocals of all pairwise distances between the points P_I,
            //     I=1,2,...,M in two dimensions, where M=N/2 and where the components
            //     of P_I are X(2*I-1) and X(2*I). Thus each vector X of N variables
            //     defines the M points P_I. The initial X gives equally spaced points
            //     on a circle. Four different choices of the pairs (N,NPT) are tried,
            //     namely (10,16), (10,21), (20,26) and (20,41). Convergence to a local
            //     minimum that is not global occurs in both the N=10 cases. The details
            //     of the results are highly sensitive to computer rounding errors. The
            //     choice IPRINT=2 provides the current X and optimal F so far whenever
            //     RHO is reduced. The bound constraints of the problem require every
            //     component of X to be in the interval [-1,1].

            //IMPORTANT TO MODIFY "n": THE NUMBER OF VARIABLES IN X ARRAYS (X0, XL, XU)
            var n = 2 * m;

            var x0 = new double[n];
            var xl = new double[n];
            var xu = new double[n];


            //UPPER AND LOWER BOUNDS IN ARRAYS XL[] AND XU[]
            const double bdl = -1.0;
            const double bdu = 1.0;
            for (var i = 0; i < n; ++i)
            {
                xl[i] = bdl;
                xu[i] = bdu;
            }

            Console.WriteLine("{0}2D output with M ={1,4},  N ={2,4}  and  NPT ={3,4}", Environment.NewLine, m, n, npt);

            //INITIAL CONDITIONS IN ARRAY X0[]
            for (var j = 1; j <= m; ++j)
            {
                var temp = 2.0 * Math.PI * j / m;
                x0[2 * j - 2] = Math.Cos(temp);
                x0[2 * j - 1] = Math.Sin(temp);
            }

            const int iprint = 2;
            const int maxfun = 500000;
            const double rhobeg = 1.0E-1;
            const double rhoend = 1.0E-6;

            var optimizer = new Bobyqa(n, this.BobyqaTestCalfun, xl, xu)
            {
                InterpolationConditions = npt,
                TrustRegionRadiusStart = rhobeg,
                TrustRegionRadiusEnd = rhoend,
                PrintLevel = iprint,
                MaximumFunctionCalls = maxfun,
                Logger = Console.Out
            };

            const OptimizationStatus expected = OptimizationStatus.Normal;
            var actual = optimizer.FindMinimum(x0).Status;
        }

        public double BobyqaTestCalfun(int n, double[] x)
        {
            var f = 0.0;
            for (var i = 4; i <= n; i += 2)
            {
                for (var j = 2; j <= i - 2; j += 2)
                {
                    var temp = Math.Max(Math.Pow(x[i - 2] - x[j - 2], 2.0) + Math.Pow(x[i - 1] - x[j - 1], 2.0), 1.0e-6);
                    f += 1.0 / Math.Sqrt(temp);
                }
            }
            return f;
        }

        //[TestCase(13, 78)]
        public void FindMinimum_ConstrainedRosenWithAdditionalInterpolationPoints_ReturnsValidMinimum(int n, int maxAdditionalPoints)
        {
            var xl = Enumerable.Repeat(-1.0, n).ToArray();
            var xu = Enumerable.Repeat(2.0, n).ToArray();
            var expected = Enumerable.Repeat(1.0, n).ToArray();

            var optimizer = new Bobyqa(n, this.Rosen, xl, xu)
            {
                TrustRegionRadiusEnd = 1.0e-8,
                MaximumFunctionCalls = 3000,
                Logger = Console.Out
            };
            for (var num = 1; num <= maxAdditionalPoints; ++num)
            {
                Console.WriteLine("\nNumber of additional points = {0}", num);
                optimizer.InterpolationConditions = 2 * n + 1 + num;
                var result = optimizer.FindMinimum(Enumerable.Repeat(0.1, n).ToArray());

          //      CollectionAssert.AreEqual(expected, result.X, new DoubleComparer(1.0e-6));
            }
        }

        //[Test]
        //public void FindMinimum_LogOutput_OutputNonEmpty()
        //{
        //    const int n = 9;
        //    var optimizer = new Bobyqa(n, this.Rosen) { Logger = Console.Out };
        //    using (var logger = new StringWriter())
        //    {
        //        optimizer.Logger = logger;
        //        optimizer.FindMinimum(Enumerable.Repeat(0.1, n).ToArray());
            //    Assert.Greater(logger.ToString().Length, 0);
        //    }
        //}

        //[Test]
        public void FindMinimum_LogOutputToConsole_OutputNonEmpty()
        {
            const int n = 9;
            var optimizer = new Bobyqa(n, this.Rosen) { Logger = Console.Out };
            optimizer.FindMinimum(Enumerable.Repeat(0.1, n).ToArray());
        }

        public double Rosen(int n, double[] x)
        {
            var f = 0.0;
            for (var i = 0; i < n - 1; ++i)
                f += 1e2 * (x[i] * x[i] - x[i + 1]) * (x[i] * x[i] - x[i + 1]) + (x[i] - 1.0) * (x[i] - 1.0);
            return f;
        }


    }

        class Program
    {
        static void Main(string[] args)
        {
            AddDemo prueba1 = new AddDemo();
            prueba1.FindMinimum_BobyqaTestCase_ReturnStatusNormal(5, 16);

            Console.WriteLine("");

            AddDemo prueba2 = new AddDemo();
            prueba2.FindMinimum_BobyqaTestCase_ReturnStatusNormal(5, 21);

            Console.WriteLine("");

            AddDemo prueba3 = new AddDemo();
            prueba3.FindMinimum_BobyqaTestCase_ReturnStatusNormal(10, 26);

            Console.WriteLine("");

            AddDemo prueba4 = new AddDemo();
            prueba4.FindMinimum_BobyqaTestCase_ReturnStatusNormal(10, 41);

            Console.WriteLine("");

            int luisprueba = 67;
            Console.WriteLine("Prueba Realizada Luis: {0}", luisprueba);
            Console.ReadLine();

        }
    }
}
