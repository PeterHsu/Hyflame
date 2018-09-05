using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula.Tester
{
    static class RateTester
    {
        internal static void Test利率()
        {
            double PV = 100;
            double Rn = 6; // 6%
            int m = 1; //(一年4次)
            int t = 10; //(3年)
            double FV = Rate.複利終值(PV, Rn, t, m);
            double FV2 = Rate.連續複利終值(PV, Rn, t);
            Console.WriteLine(Math.Round(FV, 6, MidpointRounding.AwayFromZero));
            Console.WriteLine(Math.Round(FV2, 6, MidpointRounding.AwayFromZero));
            m = 3650; //(一年4次)
            t = 10; //(3年)
            FV = Rate.複利終值(PV, Rn, m, t);
            Console.WriteLine(Math.Round(FV, 6, MidpointRounding.AwayFromZero));
        }
        internal static void Test折現因子()
        {
            double PV = 100;
            double Rn = 6; // 6%
            int m = 1; //(一年1次)
            int t = 1; //(1年)
            double FV = Rate.複利終值(PV, Rn, t, m);
            Console.WriteLine(Math.Round(FV, 6, MidpointRounding.AwayFromZero));
            double P = Rate.現值(FV, Rn, t);
            Console.WriteLine(Math.Round(P, 6, MidpointRounding.AwayFromZero));
            m = 1; //(一年1次)
            t = 5; //(3年)
            FV = Rate.複利終值(PV, Rn, t, m);
            Console.WriteLine(Math.Round(FV, 6, MidpointRounding.AwayFromZero));
            P = Rate.現值(FV, Rn, t);
            Console.WriteLine(Math.Round(P, 6, MidpointRounding.AwayFromZero));
            //# 推導回來的現值P應該要與PV相同
        }
        internal static void Test天數()
        {
            double Rn = 9.5; //# 3個月利率
            double t = 91 / 360d; //# 3個月
            double V = Rate.單利折現因子(Rn, t);
            Console.WriteLine(V);
            Rn = 9.75; //# 6個月利率
            t = 183 / 360d; //# 6個月
            V = Rate.單利折現因子(Rn, t);
            Console.WriteLine(V);
        }
        internal static void Test線性插補法()
        {
            double Rt1 = 0.976549;
            double t1 = 91;
            double Rt2 = 0.952778;
            double t2 = 183;
            double tn = 124;

            double Rtn = Rate.線性插補法(Rt1, t1, Rt2, t2, tn);
            Console.WriteLine(Rtn);
            Rtn = Rate.指數插補法(Rt1, t1, Rt2, t2, tn);
            Console.WriteLine(Rtn);
        }
        internal static void Test遠期利率()
        {
            double V1 = 0.953516;
            double V2 = 0.909091;
            int F = 2;

            double Rtn = Rate.遠期利率(V1, V2, F);
            Console.WriteLine(Rtn);

            V1 = 0.93457944;
            V2 = 0.86922423;
            F = 1;
            Rtn = Rate.遠期利率(V1, V2, F);
            Console.WriteLine(Rtn);
        }
        internal static void PeterTest()
        {
            Console.WriteLine(Rate.單利折現因子(0.259, 10 / 365d));
            Console.WriteLine(Rate.複利折現因子(0.259, 10 / 365d));
            Console.WriteLine(Rate.單利折現因子(0.340000, 20 / 365d));
            Console.WriteLine(Rate.複利折現因子(0.340000, 20 / 365d));
            //int F = 4;

            //double V1 = Rate.單利折現因子(0.6517, 0.2465753424657534);
            //double V2 = Rate.單利折現因子(0.7653, 0.4931506849315068);
            //double Rtn = Rate.遠期利率(V1, V2, F);
            //Console.WriteLine(Rtn * 100);

            //V1 = Rate.單利折現因子(0.7653, 0.4931506849315068);
            //V2 = Rate.單利折現因子(0.6975, 0.7534246575342466);
            //Rtn = Rate.遠期利率(V1, V2, F);
            //Console.WriteLine(Rtn * 100);

            //V1 = 0.9983267331;
            //V2 = 0.9965368226;
            //Rtn = Rate.遠期利率(V1, V2, F);
            //Console.WriteLine(Rtn * 100);
        }
    }
}
