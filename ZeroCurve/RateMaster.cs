using Hyflame.ZeroCurve.Elves;
using System;
using System.Collections.Generic;
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
    }
}
