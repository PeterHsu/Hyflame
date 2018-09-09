using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula.Tester
{
    static class OtherRateTester
    {
        public static void Test1()
        {
            //double t = 0.5;
            //double r = 0.022718;
            //double z = Math.Log(1 + r * t) / t;
            //Console.WriteLine(z);
            //double m = 1 / t;
            //double z2 = m * Math.Log(1 + r / m);
            //Console.WriteLine(z2);
            //double DF = Math.Exp(-1* z2 * t);
            //Console.WriteLine(DF);
            
            double r2 = 0.0214;
            double t2 = 1;
            double n = 4;
            double P = 1;
            double S = P * Math.Pow(1 + r2 * 0.5 / n, t2 * n * 0.5);
            Console.WriteLine(S);
            n = 365;
             S = P * Math.Pow(1 + r2 * 0.5 / n, t2 * n * 0.5);
            Console.WriteLine(S);
            Console.WriteLine(P * Math.Exp(r2 * 0.25));
            Console.Write(0.02065 * 100 / 4);
         
        }
    }
}
