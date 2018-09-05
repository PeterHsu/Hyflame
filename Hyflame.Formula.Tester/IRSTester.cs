using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula.Tester
{
    static class IRSTester
    {
        internal static void TestFR1()
        {
            //# 案例:
            //# 90 天利率為0.65118
            //# 180天利率為0.76386
            double r90 = 0.65118;
            int t90 = 60;
            double r180 = 0.76386;
            int t180 = 180;
            double fr = IRS.FR8Day(r90, t90, r180, t180);
            Console.WriteLine($"FR={fr}");
            //# 假設本金為100元
            //# 存放90天利息為
            double P = 100;
            double I90 = IRS.InterestByDays365(P, r180, t180);
            Console.WriteLine($"90天利息為{Math.Round(I90, 6, MidpointRounding.AwayFromZero)}");
            double I60 = IRS.InterestByDays365(P, r90, t90);
            Console.WriteLine($"60天利息為{Math.Round(I60, 6, MidpointRounding.AwayFromZero)}");
            double IFR = IRS.InterestByDays365(P, fr, t180 - t90);
            Console.WriteLine($"遠期利息為{Math.Round(IFR, 6, MidpointRounding.AwayFromZero)}");
            Console.WriteLine($"60天利息+遠期利息={Math.Round(I60 + IFR, 6, MidpointRounding.AwayFromZero)}");
        }
        internal static void TestFR2()
        {
            //# 案例:
            //# 2年利率為0.5
            //# 3年利率為0.6
            double r2 = 0.5;
            int t2 = 2;
            double r3 = 0.6;
            int t3 = 3;
            double fr = IRS.FR8Year(r2, t2, r3, t3);
            Console.WriteLine($"FR={fr}");
            //# 假設本金為100元
            //# 存放90天利息為
            double P = 100;
            double I2 = IRS.InterestByDays365(P, r2, t2);
            Console.WriteLine($"2年利息為{Math.Round(I2, 6, MidpointRounding.AwayFromZero)}");
            double I3 = IRS.InterestByDays365(P, r3, t3);
            Console.WriteLine($"3年利息為{Math.Round(I3, 6, MidpointRounding.AwayFromZero)}");
            double IFR = IRS.InterestByDays365(P, fr, t3 - t2);
            Console.WriteLine($"遠期利息為{Math.Round(IFR, 6, MidpointRounding.AwayFromZero)}");
            Console.WriteLine($"2年利息+遠期利息={Math.Round(I3 + IFR, 6, MidpointRounding.AwayFromZero)}");
        }
        internal static void TestFR3()
        {
            //# 案例:
            //# 1年利率為2
            //# 2年利率為2.5
            double r1 = 2;
            int t1 = 1;
            double r2 = 2.5;
            int t2 = 2;
            double fr = IRS.FR8Year(r1, t1, r2, t2);
            Console.WriteLine($"FR={fr}");
            //# 假設本金為100元
            //# 存放90天利息為
            double P = 100;
            double I2 = IRS.InterestByDays365(P, r1, t1);
            Console.WriteLine($"1年利息為{Math.Round(I2, 6, MidpointRounding.AwayFromZero)}");
            double I3 = IRS.InterestByDays365(P, r2, t2);
            Console.WriteLine($"2年利息為{Math.Round(I3, 6, MidpointRounding.AwayFromZero)}");
            double IFR = IRS.InterestByDays365(P, fr, t2 - t1);
            Console.WriteLine($"遠期利息為{Math.Round(IFR, 6, MidpointRounding.AwayFromZero)}");
            Console.WriteLine($"1年利息+遠期利息={Math.Round(I3 + IFR, 6, MidpointRounding.AwayFromZero)}");
        }

    }
}
