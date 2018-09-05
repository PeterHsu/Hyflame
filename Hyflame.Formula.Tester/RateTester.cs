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
    }
}
