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
            GetName(tenor, unit);
        }
        private void GetName(int tenor, EnumTenorUnit unit)
        {
            switch (unit)
            {
                case EnumTenorUnit.Day:
                    if (tenor == 1)
                    {
                        TenorName = "O/N";
                    }
                    else if (tenor == 2)
                    {
                        TenorName = "T/N";
                    }
                    else
                    {
                        TenorName = $"{tenor}天";
                    }
                    break;
                case EnumTenorUnit.Month:
                    int year = tenor / 12;
                    int month = tenor % 12;
                    if (year == 0)
                    {
                        TenorName = $"{month}月";
                    }
                    else if (month == 0)
                    {
                        TenorName = $"{year}年";
                    }
                    else
                    {
                        TenorName = $"{year}年{month}月";
                    }
                    break;
                case EnumTenorUnit.Year:
                    TenorName = $"{tenor}年";
                    break;
            }
        }
        public string TenorName { get; private set; }
        public int Tenor { get; }
        public EnumTenorUnit Unit { get; }
        public double Rate { get; }
        public EnumRateMarket Market { get; }
        public double FakeDays { get; set; }
    }

}
