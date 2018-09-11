using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.ZeroCurve.Elves
{
    public class YieldElf
    {
        public YieldElf(ParRateElfEx parRateElfEx, double rf)
        {
            this.Tenor = parRateElfEx.Tenor;
            this.Unit = parRateElfEx.Unit;
            this.Rate = parRateElfEx.Rate;
            this.Market = parRateElfEx.Market;
            this.IsParRate = true;
            this.StartDate = parRateElfEx.StartDate;
            this.EndDate = parRateElfEx.EndDate;
            this.Days = parRateElfEx.Days;
            this.DaysAct = parRateElfEx.DaysAct;
            this.Zero = parRateElfEx.Zero;
            this.DF = parRateElfEx.DF;
            this.FR = rf;
            GetName(parRateElfEx.Tenor, parRateElfEx.Unit);
        }
        public YieldElf(int tenor, EnumTenorUnit unit, double rate, EnumRateMarket market, DateTime startDate, DateTime endDate, double days, double daysAct, double zero, double df, double fr)
        {
            this.Tenor = tenor;
            this.Unit = unit;
            this.Rate = rate;
            this.Market = market;
            this.IsParRate = false;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Days = days;
            this.DaysAct = daysAct;
            this.Zero = zero;
            this.DF = df;
            this.FR = fr;
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
                    else if(month == 0)
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
        public int Tenor { get; set; }
        public EnumTenorUnit Unit { get; set; }
        public string TenorName { get; set; }
        public double Rate { get; set; }
        public EnumRateMarket Market { get; set; }
        public bool IsParRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Days { get; set; }
        public double DaysAct { get; set; }
        public double Zero { get; set; }
        public double DF { get; set; }
        public double FR { get; set; }
    }
}
