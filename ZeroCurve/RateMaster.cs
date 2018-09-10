using Hyflame.ZeroCurve.Elves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyflame.ZeroCurve
{
    public class RateMaster
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private TradeDateAx m_tradeDateAx;
        private List<ParRateElf> m_parRateElfList;
        public List<ParRateElfEx> ParRateList { get; private set; }
        public DateTime TradeDate { get; }
        public DateTime SpotDate { get; }

        public RateMaster(DateTime tradeDate, DateTime spotDate, DateTime endDate, List<ParRateElf> parRateElfList, List<DateTime> holidays)
            : this(tradeDate, endDate, parRateElfList, holidays)
        {
            this.SpotDate = spotDate;
        }
        public RateMaster(DateTime tradeDate, DateTime endDate, List<ParRateElf> parRateElfList, List<DateTime> holidays)
        {
            this.TradeDate = tradeDate;
            this.m_parRateElfList = parRateElfList;
            this.m_tradeDateAx = new TradeDateAx(tradeDate, endDate, holidays);
            this.SpotDate = m_tradeDateAx.GetSettlementDate(TradeDate, 0).Item1;
        }
        public void Run()
        {
            ParRateElfExBuilder parRateBuilder = new ParRateElfExBuilder(TradeDate, SpotDate, m_tradeDateAx);
            foreach (var parRateElf in m_parRateElfList)
            {
                parRateBuilder.AddParRate(parRateElf);
            }
            this.ParRateList = parRateBuilder.ParRateList;
        }
        public double GetZeroRate_D(double d)
        {
            double t = d / 365d;
            var interpolate = MathNet.Numerics.Interpolate.Linear(this.ParRateList.Select(p => p.DaysAct), this.ParRateList.Select(p => p.Zero));
            double zero = interpolate.Interpolate(t);
            return zero;
        }
        public (double, double, double, double, double) GetZeroRate_T(double t, double t1)
        {
            //# 零息利率, 遠期利率, 折現因子, 驗證零息利率, Swap
            var interpolate = MathNet.Numerics.Interpolate.Linear(this.ParRateList.Select(p => p.DaysAct), this.ParRateList.Select(p => p.Zero));
            double zero2 = interpolate.Interpolate(t1 + 90 / 365d);
            double zero1 = interpolate.Interpolate(t1);
            double zero = interpolate.Interpolate(t);
            
            double FR = RateAx.遠期利率2(zero1, t1, zero2, 90); //# 算上一期到加90天的利率
            double DF = RateAx.折現因子_T_零息利率(zero, t);
            double newZero = RateAx.零息利率_T_折現因子(DF, t);

            var interpolate2 = MathNet.Numerics.Interpolate.Linear(this.ParRateList.Select(p => p.DaysAct), this.ParRateList.Select(p => p.Rate));
            double rate = interpolate2.Interpolate(t);

            return (zero, FR, DF, newZero, rate);
        }
    }
}
