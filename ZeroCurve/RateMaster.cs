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
        public (double, double, double, double, double) GetYield_T(double t)
        {
            //# 零息利率, 遠期利率, 折現因子, 驗證零息利率, Swap
            var interpolate = MathNet.Numerics.Interpolate.Linear(this.ParRateList.Select(p => p.DaysAct), this.ParRateList.Select(p => p.Zero));
            //# 從此天+90為遠期利率
            double t2 = t + 90 / 365d;
            double zero2 = interpolate.Interpolate(t2);
            double zero = interpolate.Interpolate(t);

            double FR = RateAx.遠期利率e(zero, t ,zero2, t2); //# 算上一期到加90天的利率
            double DF = RateAx.折現因子_T_Z(zero, t);
            double newZero = RateAx.零息利率_T_折現因子(DF, t);

            var interpolate2 = MathNet.Numerics.Interpolate.Linear(this.ParRateList.Select(p => p.DaysAct), this.ParRateList.Select(p => p.Rate));
            double rate = interpolate2.Interpolate(t);

            return (zero, FR, DF, newZero, rate);
        }

        public YieldElf[] GetCurve(int year)
        {
            List<YieldElf> yieldList = new List<YieldElf>();
            //foreach (var item in this.ParRateList)
            //{
            //    (double zero, double FR, double DF, double newZero, double rate) = GetYield_T(item.DaysAct, item.DaysAct - 90 / 365d);
            //    yieldList.Add(new YieldElf(item, FR));
            //}
            for (int i = 0; i < year; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    int tenor = i * 12 + j * 3; //#每3個月
                    EnumTenorUnit unit = EnumTenorUnit.Month;
                    double rate = 0;
                    EnumRateMarket market = EnumRateMarket.None;
                    DateTime startDate = DateTime.Today;
                    (DateTime endDate, bool isLastDay, double days) = m_tradeDateAx.GetSettlementDate(startDate, (i * 4 ) + j );
                    double daysAct = days / 365d;
                    (double zero, double fr, double df, double newZero, double swap) = GetYield_T(daysAct);
                    YieldElf yieldElf = new YieldElf(tenor, unit, rate, market, startDate, endDate, days, daysAct, zero, df, fr);
                    bool exist = (from item in yieldList
                                  where item.Days == days
                                  select item).Any();
                    if(!exist) //# 如果已經存在就不要加入了
                        yieldList.Add(yieldElf);
                }
            }
            var query = (from item in yieldList
                         orderby item.Days
                         select item).ToArray();
            return query;
        }

    }
}
