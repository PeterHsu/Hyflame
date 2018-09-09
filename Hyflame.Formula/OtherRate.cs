using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula
{
    public static class OtherRate
    {   
        public static double DaysAct(int days)
        {
            return days / 365d;
        }
        public static double daysAct2Days(double actDays)
        {
            return actDays * 365d;
        }
    }
}
