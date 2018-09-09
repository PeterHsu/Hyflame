using Hyflame.ZeroCurve;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula.Tester
{
    static class TradeDateAxTester
    {
        public static void Test()
        {
            List<DateTime> holidays = new List<DateTime>();
            holidays.Add(new DateTime(2018, 9, 10));
            holidays.Add(new DateTime(2018, 9, 9));
            holidays.Add(new DateTime(2018, 9, 11, 11,23,35));
            TradeDateAx ax = new TradeDateAx(DateTime.Today, new DateTime(2018, 10, 10), holidays);
            DateTime newDate = ax.AdjustTradeDate(new DateTime(2018, 9, 10));
            Console.WriteLine(newDate);
        }
    }
}
