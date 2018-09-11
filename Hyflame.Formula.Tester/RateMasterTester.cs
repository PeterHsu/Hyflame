using Hyflame.ZeroCurve;
using Hyflame.ZeroCurve.Elves;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hyflame.Formula.Tester
{
    static class RateMasterTester
    {
        public static void Test()
        {
            DateTime tradeDate = DateTime.Today;
            //DateTime tradeDate = new DateTime(2018, 9, 7);
            List<DateTime> holidays = new List<DateTime>();
            using (StreamReader sr = new StreamReader("holidays.txt"))
            {
                while(sr.Peek() >= 0)
                {
                    holidays.Add(DateTime.Parse(sr.ReadLine()));
                }
            }
            //# 準備Par Rate的資料
            List<ParRateElf> parRateElfList = new List<ParRateElf>();
            parRateElfList.Add(new ParRateElf(1, EnumTenorUnit.Day, 0.5257, EnumRateMarket.Interbank));
            parRateElfList.Add(new ParRateElf(2, EnumTenorUnit.Day, 0.5257, EnumRateMarket.Interbank));
            parRateElfList.Add(new ParRateElf(30, EnumTenorUnit.Day, 0.5603, EnumRateMarket.Taibor));
            parRateElfList.Add(new ParRateElf(60, EnumTenorUnit.Day, 0.6013, EnumRateMarket.Taibor));
            parRateElfList.Add(new ParRateElf(90, EnumTenorUnit.Day, 0.6517, EnumRateMarket.Taibor));
            parRateElfList.Add(new ParRateElf(180, EnumTenorUnit.Day, 0.7653, EnumRateMarket.Taibor));
            parRateElfList.Add(new ParRateElf(1, EnumTenorUnit.Year, 0.6975, EnumRateMarket.COSMOS));
            parRateElfList.Add(new ParRateElf(2, EnumTenorUnit.Year, 0.7475, EnumRateMarket.COSMOS));
            parRateElfList.Add(new ParRateElf(3, EnumTenorUnit.Year, 0.825, EnumRateMarket.COSMOS));
            parRateElfList.Add(new ParRateElf(4, EnumTenorUnit.Year, 0.845, EnumRateMarket.COSMOS));
            parRateElfList.Add(new ParRateElf(5, EnumTenorUnit.Year, 0.915, EnumRateMarket.COSMOS));
            RateMaster rateMaster = new RateMaster(tradeDate, tradeDate.AddYears(10), parRateElfList, holidays);
            rateMaster.Run();
            var list = rateMaster.GetCurve(5);
            foreach(var item in list)
            {
                Console.WriteLine($@"{item.TenorName}, {item.IsParRate}, {item.EndDate.ToString("yyyy-MM-dd")}, {item.Days}, {item.DaysAct}, {item.Rate}, {item.Zero}");
            }
            //Console.WriteLine(rateMaster.SpotDate);
            //foreach (var item in rateMaster.ParRateList)
            //    Console.WriteLine(item);
            //Console.WriteLine(rateMaster.GetYield_T(0.2493150684931507, 0));
            //Console.WriteLine(rateMaster.GetYield_T(0.4986301369863014, 0.2493150684931507));
            //Console.WriteLine(rateMaster.GetYield_T(0.7534246575342466, 0.4986301369863014));
            //Console.WriteLine(rateMaster.GetYield_T(1.0054794520547945, 0.7534246575342466));
            //Console.WriteLine(rateMaster.GetYield_T(1.2547945205479452, 1.0054794520547945));
            //Console.WriteLine(rateMaster.GetYield_T(1.5041095890410958, 1.2547945205479452));
            //Console.WriteLine(rateMaster.GetYield_T(1.7561643835616438, 1.5041095890410958));
            //Console.WriteLine(rateMaster.GetYield_T(2.0136986301369864, 1.7561643835616438));
            //Console.WriteLine(rateMaster.GetYield_T(2.263013698630137, 2.0136986301369864));
            //Console.WriteLine(rateMaster.GetYield_T(2.504109589041096, 2.263013698630137));
            //Console.WriteLine(rateMaster.GetYield_T(2.7616438356164386, 2.504109589041096));
            //Console.WriteLine(rateMaster.GetYield_T(3.010958904109589, 2.7616438356164386));
            //Console.WriteLine(rateMaster.GetYield_T(3.26027397260274, 3.010958904109589));
            //Console.WriteLine(rateMaster.GetYield_T(3.5095890410958903, 3.26027397260274));
            //Console.WriteLine(rateMaster.GetYield_T(3.758904109589041, 3.5095890410958903));
        }
    }
}
