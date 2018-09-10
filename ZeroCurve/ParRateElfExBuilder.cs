using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyflame.ZeroCurve.Elves;

namespace Hyflame.ZeroCurve
{
    internal class ParRateElfExBuilder
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<ParRateElfEx> ParRateList { get; } = new List<ParRateElfEx>();
        public DateTime TradeDate { get; }
        public DateTime SpotDate { get; set; }
        private TradeDateAx m_tradeDateAx;
        const double ACTUAL = 365; //# 預設採用ACT/365
        public ParRateElfExBuilder(DateTime tradeDate, DateTime spotDate, TradeDateAx tradeDateAx)
        {
            //# 因為SpotDate可以由交易日強制修改,所以不能用TradeDate去推算,所以要帶入實際的spotDate
            this.TradeDate = tradeDate;
            this.SpotDate = spotDate;
            this.m_tradeDateAx = tradeDateAx;
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
            zero = RateAx.零息利率_T_R(parRateInfo.Rate, daysAct);
            #endregion 取零息利率
            this.ParRateList.Add(new ParRateElfEx(parRateInfo, this.TradeDate, endDate, days, daysAct, zero));
        }
        private void AddParRateForCOSMOS(DateTime startDate, ParRateElf parRateInfo)
        {
            (DateTime endDate, double days, double daysAct) = GetEndDate(startDate, parRateInfo);
            double zero = 1;
            #region 取零息利率
            //# 先加入新的COSMOS利率, zero先預設為1
            this.ParRateList.Add(new ParRateElfEx(parRateInfo, this.TradeDate, endDate, days, daysAct, zero));
            //# 先取出已經存在的COSMOS利率資料重算
            List<ParRateElfEx> cosmosList = this.ParRateList.Where(p => p.Market == EnumRateMarket.COSMOS).ToList();
            //# 為防萬一先排序一下
            cosmosList = (from item in cosmosList
                          orderby item.Days
                          select item).ToList();

            //# 取最後的Taibor利率
            var maxTaibor = (from item in this.ParRateList
                             where item.Market == EnumRateMarket.Taibor
                             orderby item.Days descending
                             select item).First();
            GetZero(maxTaibor, cosmosList);
            logger.Info("ParRateExList={@parRateExList}", this.ParRateList);
            #endregion
        }
        private void GetZero(ParRateElfEx maxTaibor, List<ParRateElfEx> cosmosList)
        {
            ParRateElfEx lastRateElfEx = maxTaibor;
            for (int swap = 0; swap < cosmosList.Count; swap++)
            {
                //# 先給定一個外插的斜率
                double slope1 = (cosmosList[swap].Rate - lastRateElfEx.Zero) / (cosmosList[swap].DaysAct - lastRateElfEx.DaysAct);
                double slope2;
                //# 牛頓法設定
                double fixedRate = 0.0001;
                double epsilon = 0.000_000_000_001;
                int maxIter = 10;
                double zero = 1;

                //# 檢驗用不同的slope帶入後, 最得決定斜率為何
                for (int i = 0; i <= maxIter; i++)
                {
                    //# 檢驗用不同的slope帶入後, 算出的IRS的NPV
                    double value1 = YieldCurveLinearBootstrap1(cosmosList[swap].DaysAct, lastRateElfEx, slope1, cosmosList[swap].DaysAct, cosmosList[swap].Rate);
                    //# 用線性插補法算出 Zero Rate
                    zero = lastRateElfEx.Zero + slope1 * (cosmosList[swap].DaysAct - lastRateElfEx.DaysAct);
                    //# 調整斜率
                    slope2 = slope1 - fixedRate;
                    //# 檢驗用不同的slope帶入後, 算出的IRS的NPV
                    double value2 = YieldCurveLinearBootstrap1(cosmosList[swap].DaysAct, lastRateElfEx, slope2, cosmosList[swap].DaysAct, cosmosList[swap].Rate);
                    //# 以下不知
                    double dx = (value2 - value1) / fixedRate;
                    //logger.Info($"year={cosmosList[swap].DaysAct}, Rate={cosmosList[swap].Rate}, Swaps={swap + 1}, slope1={slope1}, value1={value1}, zeroRate={zero}, slope2={slope2}, value2={value2}, dx={dx}");
                    if (Math.Abs(dx) < epsilon) break;
                    slope1 = slope1 - (0 - value1) / dx;
                }
                //# 得到Zero Rate
                cosmosList[swap].Zero = zero;
                lastRateElfEx = cosmosList[swap];
            }
        }
        public double YieldCurveLinearBootstrap1(double _yearTime, ParRateElfEx lastRateElfEx, double slope, double daysAct, double irsRate)
        {
            //# 展開此COSMOS的利率, 總共有多少次計息, 每3個月計息一次, 1年就有4次
            IRSRateElf[] irsList = new IRSRateElf[Convert.ToInt32(Math.Round(_yearTime)) * 4];
            //# 預設NPV為-100, 不知道為什麼
            double npv = -100;

            for (int row = 0; row < irsList.Length; row++)
            {
                //# 每3個月計息一次, 取得計息日, 換算出天數相關資料
                irsList[row].EndDate = m_tradeDateAx.AdjustTradeDate(SpotDate.AddMonths((row + 1) * 3));
                irsList[row].Days = (irsList[row].EndDate - TradeDate).TotalDays;
                irsList[row].DaysAct = irsList[row].Days / ACTUAL;
                if (row == 0)
                {
                    irsList[row].TenorDays = (irsList[row].EndDate - SpotDate).TotalDays;
                }
                else
                {
                    irsList[row].TenorDays = (irsList[row].EndDate - irsList[row - 1].EndDate).TotalDays;
                }
                irsList[row].TenorDaysAct = irsList[row].TenorDays / ACTUAL;
                //# 如果還在Taibor的利率範圍內, 直接用線性插補法取得Zero Rate
                if (irsList[row].Days < lastRateElfEx.Days)
                {
                    //# 取得已經完成Zero Rate的資料
                    var readlyRateList = from item in this.ParRateList
                                     where item.Zero != 1
                                     orderby item.Days
                                     select item;

                    var interpolate = MathNet.Numerics.Interpolate.Linear(readlyRateList.Select(p => p.DaysAct), readlyRateList.Select(p => p.Zero));
                    irsList[row].Zero = interpolate.Interpolate(irsList[row].DaysAct);
                }
                else //# 如果超過了Taibor的利率範圍, 還是用線性插補法取得, 只是改用了COSMOS和Taibor的斜率
                {
                    irsList[row].Zero = lastRateElfEx.Zero + slope * (irsList[row].DaysAct - lastRateElfEx.DaysAct);
                }
                //# 如果是此IRS最後一個Tenor了
                if (row == irsList.Length - 1)
                {
                    //# 不知道為什麼最後一個利息要加100元
                    irsList[row].Interest = irsRate * irsList[row].TenorDaysAct + 100;
                }
                else
                {
                    irsList[row].Interest = irsRate * irsList[row].TenorDaysAct;
                }
                //# 算折現因子
                var DFx = 1 / Math.Exp(irsList[row].Zero * 0.01 * irsList[row].DaysAct);
                //# 取得TN利率
                var tnRate = this.ParRateList.Where(p => p.Market == EnumRateMarket.Interbank && p.Tenor == 2).First();
                var DFy = Math.Exp(tnRate.Zero * 0.01 * tnRate.DaysAct);
                irsList[row].DF = DFx * DFy;
                //# 算NPV
                irsList[row].NPV = irsList[row].Interest * irsList[row].DF;
                //# 總計NPV
                npv += irsList[row].NPV;
            }
            return npv;
        }
        private (DateTime, double, double) GetEndDate(DateTime startDate, ParRateElf parRateInfo)
        {
            DateTime endDate = default(DateTime); //# 到期的實際日期
            double days = 0; //# 從交易日至到期日總天數
            double daysAct = 0; //# 總天數 / Actual

            //# 表示驗證用, 會改寫days及daysAct
            if (parRateInfo.FakeDays > 0)
            {
                days = parRateInfo.FakeDays;
                daysAct = days / ACTUAL;
                endDate = m_tradeDateAx.AdjustTradeDate(startDate.AddDays(parRateInfo.FakeDays));
            }
            else
            {
                #region 取到期日
                if (parRateInfo.Unit == EnumTenorUnit.Day)
                    endDate = m_tradeDateAx.AdjustTradeDate(startDate.AddDays(parRateInfo.Tenor));
                if (parRateInfo.Unit == EnumTenorUnit.Month)
                    endDate = m_tradeDateAx.AdjustTradeDate(startDate.AddMonths(parRateInfo.Tenor));
                if (parRateInfo.Unit == EnumTenorUnit.Year)
                    endDate = m_tradeDateAx.AdjustTradeDate(startDate.AddYears(parRateInfo.Tenor));
                #endregion 取到期日
                days = (endDate - this.TradeDate).TotalDays;
                daysAct = days / ACTUAL;
            }
            return (endDate, days, daysAct);
        }
    }
}
