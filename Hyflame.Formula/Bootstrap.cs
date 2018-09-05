using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula
{
    public class Bootstrap
    {
        public List<MarketRate> MarketRate { get; set; }
        public void AddMarketRate(MarketRate marketRate)
        {
            this.MarketRate.Add(marketRate);
        }
        private void Step1()
        {

        }
        private void Interpolation()
        {

        }
    }
    public struct MarketRate
    {
        public EnumRateUnit RateUnit { get; set; }
        public int RateTime { get; set; }
        public double Rate { get; set; }
    }
    public enum EnumRateUnit
    {
        Day,
        Month,
        Year
    }
}
