using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.ZeroCurve.Elves
{
    public class ParRateElf
    {
        public ParRateElf(int tenor, EnumTenorUnit unit, double rate, EnumRateMarket market, double fakeDays = 0)
        {
            this.Tenor = tenor;
            this.Unit = unit;
            this.Rate = rate;
            this.Market = market;
            this.FakeDays = fakeDays;
        }
        public int Tenor { get; }
        public EnumTenorUnit Unit { get; }
        public double Rate { get; }
        public EnumRateMarket Market { get; }
        public double FakeDays { get; set; }
    }
 
}
