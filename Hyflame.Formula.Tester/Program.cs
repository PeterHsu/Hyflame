using System;

namespace Hyflame.Formula.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestWarrant();
            //TestWarrant2();
            //TestIRS();
            //IRSTester.TestFR3();
            //RateTester.Test利率();
            //RateTester.Test折現因子();
            //RateTester.Test天數();
            //RateTester.Test線性插補法();
            //RateTester.PeterTest();
            //RateTester.Test遠期利率合約();
            //RateTester.Test線性插補法2();
            RateTester.TestIRS遠期利率();
            Console.ReadLine();
        }
        static void TestWarrant()
        {
            //#範例說明
            //#假設收盤價為20
            //#換股比率0.8, 每一仟股換發800股, 倒數為 1/0.8=1.25
            //#每股退2元
            //#履約價原為22元
            //#行使比例原為0.5

            //#調整後的履約價為24.75
            //#調整後的行使比例為0.444
            (double fs, double fk, double fn) = Warrant.Reduction(20, 1.25, 2, 22, 0.5);
            Console.WriteLine($"新參考價={fs}, 新履約價={fk}, 新行使比例={fn}");
        }
        static void TestWarrant2()
        {
            //# 範例說明(3532台勝科)
            double s = 107.5; //# 假設收盤價為107.5
            double a = 0.5; //# 換股比率0.5
            double d = 5; //# 每股退5元
            double k = 119.15;//# 履約價原為119.15元
            double n = 0.041;//# 行使比例原為0.041

            //#調整後的履約價為24.75
            //#調整後的行使比例為0.444
            (double fs, double fk, double fn) = Warrant.ReductionEx(s, a, d, k, n);
            Console.WriteLine($"新參考價={fs}, 新履約價={fk}, 新行使比例={fn}");
        }
        static void TestIRS()
        {
            Console.WriteLine(IRS.Vk(10 / 100, 1));
        }
    }
}
