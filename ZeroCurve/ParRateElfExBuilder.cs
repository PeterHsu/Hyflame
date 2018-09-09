using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyflame.ZeroCurve.Elves;

namespace Hyflame.ZeroCurve
{
    class ParRateElfExBuilder
    {
        public List<ParRateElfEx> ParRateList { get; } = new List<ParRateElfEx>();
        public DateTime TradeDate { get; }
        public DateTime SpotDate { get; set; }
        private TradeDateAx m_tradeDateHelper;
        const double ACTUAL = 365; //# 預設採用ACT/365
        public ParRateElfExBuilder(DateTime tradeDate, DateTime spotDate, DateTime endDate, List<DateTime> holidays)
        {
            //# 因為SpotDate可以由交易日強制修改,所以不能用TradeDate去推算,所以要帶入實際的spotDate
            this.TradeDate = tradeDate;
            this.SpotDate = spotDate;
            this.m_tradeDateHelper = new TradeDateAx(tradeDate, endDate, holidays);
        }
        public void AddParRate(ParRateElf parRateInfo)
        {
            switch (parRateInfo.Market)
            {
                case EnumRateMarket.Interbank:
                    AddParRateForInterbank(this.TradeDate, parRateInfo); //# 啟算日從交易日
                    break;
                case EnumRateMarket.Taibor:
                    AddParRateForTaibor(this.TradeDate, parRateInfo); //# 啟算日從交易日
                    break;
                case EnumRateMarket.COSMOS:
                    AddParRateForCOSMOS(this.SpotDate, parRateInfo); //# 啟算日從即期日
                    break;
            }
        }
        private void AddParRateForInterbank(DateTime startDate, ParRateElf parRateInfo)
        {
            //# 一定是ON或TN, 央行沒有報TN, 所以會用ON的利率帶進來
            (DateTime endDate, double days, double daysAct) = GetEndDate(startDate, parRateInfo);
            double zero = 0; //# 零息利率

            #region 取零息利率
            if (parRateInfo.Unit == EnumTenorUnit.Day && parRateInfo.Tenor == 2) //# TN
            {
                ParRateElfEx ON = ParRateList.SingleOrDefault(p => p.Tenor == 1 && p.Unit == EnumTenorUnit.Day);
                if (ON == null)
                {
                    throw new Exception("沒有O/N無法計算T/N的零息利率");
                }
                zero = RateAx.TN零息利率(ON.DF, parRateInfo.Rate, days);
            }
            else
                zero = RateAx.零息利率_T_R(parRateInfo.Rate, daysAct);
            #endregion 取零息利率
            this.ParRateList.Add(new ParRateElfEx(parRateInfo, this.TradeDate, endDate, days, daysAct, zero));
        }
        private void AddParRateForTaibor(DateTime startDate, ParRateElf parRateInfo)
        {
            (DateTime endDate, double days, double daysAct) = GetEndDate(startDate, parRateInfo);
            double zero = 0; //# 零息利率

            #region 取零息利率
            if (parRateInfo.Unit == EnumTenorUnit.Day && parRateInfo.Tenor == 2) //# TN
            {
                ParRateElfEx ON = ParRateList.SingleOrDefault(p => p.Tenor == 1 && p.Unit == EnumTenorUnit.Day);
                if (ON == null)
                {
                    throw new Exception("沒有O/N無法計算T/N的零息利率");
                }
                zero = RateAx.TN零息利率(ON.DF, parRateInfo.Rate, days);
            }
            else
                zero = RateAx.零息利率_T_R(parRateInfo.Rate, daysAct);
            #endregion 取零息利率
            this.ParRateList.Add(new ParRateElfEx(parRateInfo, this.TradeDate, endDate, days, daysAct, zero));
        }
        private void AddParRateForCOSMOS(DateTime startDate, ParRateElf parRateInfo)
        {
            (DateTime endDate, double days, double daysAct) = GetEndDate(startDate, parRateInfo);
        }
        private (DateTime, double, double) GetEndDate(DateTime startDate, ParRateElf parRateInfo)
        {
            DateTime endDate = default(DateTime); //# 到期的實際日期
            double days = 0; //# 從交易日至到期日總天數
            double daysAct = 0; //# 總天數 / Actual

            #region 取到期日
            if (parRateInfo.Unit == EnumTenorUnit.Day)
                endDate = m_tradeDateHelper.AdjustTradeDate(startDate.AddDays(parRateInfo.Tenor));
            if (parRateInfo.Unit == EnumTenorUnit.Month)
                endDate = m_tradeDateHelper.AdjustTradeDate(startDate.AddMonths(parRateInfo.Tenor));
            if (parRateInfo.Unit == EnumTenorUnit.Year)
                endDate = m_tradeDateHelper.AdjustTradeDate(startDate.AddYears(parRateInfo.Tenor));
            #endregion 取到期日
            days = (endDate - this.TradeDate).TotalDays;
            daysAct = days / ACTUAL;
            return (endDate, days, daysAct);
        }
    }
}
