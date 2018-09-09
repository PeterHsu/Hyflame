using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.ZeroCurve.Elves
{
    public class ParRateElf
    {
        public ParRateElf(int tenor, EnumTenorUnit unit, double rate, EnumRateMarket market)
        {
            Tenor = tenor;
            Unit = unit;
            Rate = rate;
            Market = market;
        }
        public int Tenor { get; }
        public EnumTenorUnit Unit { get; }
        public double Rate { get; }
        public EnumRateMarket Market { get; }
    }
 
}
