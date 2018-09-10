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
            parRateElfList.Add(new ParRateElf(1, EnumTenorUnit.Day, 0.5257, EnumRateMarket.Interbank, 1));
            parRateElfList.Add(new ParRateElf(2, EnumTenorUnit.Day, 0.5257, EnumRateMarket.Interbank, 2));
            parRateElfList.Add(new ParRateElf(30, EnumTenorUnit.Day, 0.5603, EnumRateMarket.Taibor, 31));
            parRateElfList.Add(new ParRateElf(60, EnumTenorUnit.Day, 0.6013, EnumRateMarket.Taibor, 60));
            parRateElfList.Add(new ParRateElf(90, EnumTenorUnit.Day, 0.6517, EnumRateMarket.Taibor, 91));
            parRateElfList.Add(new ParRateElf(180, EnumTenorUnit.Day, 0.7653, EnumRateMarket.Taibor, 182));
            parRateElfList.Add(new ParRateElf(1, EnumTenorUnit.Year, 0.6975, EnumRateMarket.COSMOS, 367));
            parRateElfList.Add(new ParRateElf(2, EnumTenorUnit.Year, 0.7475, EnumRateMarket.COSMOS, 735));
            parRateElfList.Add(new ParRateElf(3, EnumTenorUnit.Year, 0.825, EnumRateMarket.COSMOS, 1099));
            parRateElfList.Add(new ParRateElf(4, EnumTenorUnit.Year, 0.845, EnumRateMarket.COSMOS, 1463));
            parRateElfList.Add(new ParRateElf(5, EnumTenorUnit.Year, 0.915, EnumRateMarket.COSMOS, 1828));
            RateMaster rateMaster = new RateMaster(tradeDate, tradeDate.AddYears(10), parRateElfList, holidays);
            rateMaster.Run();
            Console.WriteLine(rateMaster.SpotDate);
            foreach (var item in rateMaster.ParRateList)
                Console.WriteLine(item);
        }
    }
}
