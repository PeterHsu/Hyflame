using System;
using System.Collections.Generic;
using System.Text;
using RateAx = Hyflame.ZeroCurve.RateAx;


namespace Hyflame.Formula.Tester
{
    static class RateTester
    {
        internal static void Test單複利()
        {
            double r1 = 3;
            double d = 50;
            double FV1 = RateAx.單利終值_D(1, r1, d);
            Console.WriteLine(FV1);

        }
        internal static void Test遠期利率()
        {
            double r1 = 0.6916437211111013;
            double t1 = 2375;
            double n = 90;

            double r2 = 1.0334337150205943;
            double t2 = t1 + n;
            double rf = RateAx.遠期利率e(r1, t1 / 365d, r2, t2 / 365d);
            Console.WriteLine(rf);
            double FV = RateAx.終值_遠期利率e(r1, t1 / 365d, rf, (t2 - t1) / 365d);
            Console.WriteLine(FV);
            double FV2 = RateAx.終值_D_零息利率(1, r2, t2);
            Console.WriteLine(FV2);

        }
        //internal static void Test遠期利率()
        //{
        //    double V1 = 0.953516;
        //    double V2 = 0.909091;
        //    int F = 2;

        //    double Rtn = Rate.遠期利率(V1, V2, F);
        //    Console.WriteLine(Rtn);

        //    V1 = 0.93457944;
        //    V2 = 0.86922423;
        //    F = 1;
        //    Rtn = Rate.遠期利率(V1, V2, F);
        //    Console.WriteLine(Rtn);
        //}
        internal static void Test單利()
        {
            //# 單利公式
            double PV = 1; //# 本金
            double R = 0.5;　//# 年利率
            double d = 1; //# 天數
            double FV = RateAx.單利終值_D(PV, R, d);
            Console.WriteLine($"單利終值:{FV}");
            double DF = RateAx.折現因子_D_R(R, d);
            Console.WriteLine($"折現因子為:{DF}");
            double newPV = RateAx.現值_折現因子(FV, DF);
            Console.WriteLine($"驗證折現因子推現值:{newPV}");
            double newFV = RateAx.終值_折現因子(PV, DF);
            Console.WriteLine($"驗證折現因子推終值:{newFV}");
            Console.WriteLine("".PadRight(10, '-'));
            //# 零息利率公式
            double zero = RateAx.零息利率_D_R(R, d);
            Console.WriteLine($"零息利率為:{zero}", zero);
            newPV = RateAx.現值_D_零息利率(FV, zero, d);
            Console.WriteLine($"驗證零息利率推現值:{newPV}");
            newFV = RateAx.終值_D_零息利率(PV, zero, d);
            Console.WriteLine($"驗證零息利率推終值:{newFV}");
            double newDF = RateAx.折現因子_D_Z(zero, d);
            Console.WriteLine($"從零息利率找折現因子:{newDF}");
            double newZero = RateAx.零息利率_D_折現因子(DF, d);
            Console.WriteLine($"從折現因子找零息利率:{newZero}");

        }
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
        internal static void Test線性插補法2()
        {
            double[] points = { -1, 0.0027397260273972603, 0.0054794520547945206, 0.0821917808219178, 0.16712328767123288, 0.24657534246575341, 0.49315068493150682, 1.010958904109589, 2.0082191780821916, 3.0082191780821916, 4.0082191780821921, 5.0082191780821921 };
            double[] values = { -1, 0.0052569621427417676, 0.0052569621427456057, 0.0056017102476400753, 0.0060099807536333491, 0.0065117694163945028, 0.0076385947076219683, 0.0069587557786794478, 0.0074636883696238433, 0.008244073174368172, 0.0084455100303834282, 0.0091560853421900189 };
            double point = 0.24657534246575341 + 90 / 365d;
            point = 0.49315068493150682;
            double Rtn = Rate.線性插補法Ex(points, values, point);
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
        internal static void Test遠期利率合約()
        {
            double FRA = Rate.遠期利率合約(0.6013, 60, 0.6517, 90);
            Console.WriteLine(FRA);
        }
        internal static void TestIRS遠期利率()
        {
            //double R1Zero = 0.0065117694163945;
            //double R1t365 = 0.246575342465753;
            //double Rt = 0.00763859470762197;
            //double Rtn = Rate.IRS遠期利率(R1Zero, R1t365, Rt);
            //Console.WriteLine(Rtn);
            //double FRA = Rate.遠期利率合約(0.65117694163945, 0.246575342465753 * 365 , 0.763859470762197, 0.493150684931507 * 365);
            //Console.WriteLine(FRA);            //R1Zero = 0.00897698142798124;
            // R1t365 = 4.75616438356164;
            // Rt = 0.00915219177883765;
            // Rtn = Rate.IRS遠期利率(R1Zero, R1t365, Rt);
            //Console.WriteLine(Rtn);

        }
    }
}
