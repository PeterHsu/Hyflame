using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyflame.ZeroCurve
{
    public class TradeDateAx
    {
        private List<DateTime> m_tradeDateList = new List<DateTime>();
        private DateTime m_startDate;
        private DateTime m_endDate;
        public TradeDateAx(DateTime startDate, DateTime endDate, List<DateTime> holidays)
        {
            this.m_startDate = startDate;
            this.m_endDate = endDate;
            for(var dt = startDate;dt<= endDate;dt = dt.AddDays(1))
            {
                m_tradeDateList.Add(dt);
            }
            //# 怕有人傻傻的傳了時間進來
            List<DateTime> newHolidays = new List<DateTime>();
            foreach (var dt in holidays)
            {
                newHolidays.Add(dt.Date);
            }
            m_tradeDateList = m_tradeDateList.Except(newHolidays).ToList();
        }
        /// <summary>
        /// 當輸入日期為假日時, 找到下一個交易日
        /// </summary>
        /// <param name="date">要調整的日期</param>
        /// <returns>調整後的交易日</returns>
        public DateTime AdjustTradeDate(DateTime date)
        {
            var tradeDate = (from item in m_tradeDateList
                             where item >= date
                             orderby item
                             select item).First();
            return tradeDate;
        }
        /// <summary>
        /// 取得第幾期的清算日, tenor為0時為即期日
        /// </summary>
        /// <param name="tradeDate">交易日T</param>
        /// <param name="tenor">第幾期</param>
        /// <param name="month">一個Tenor是幾個月, 預設為3個月</param>
        /// <returns>(清算日, 即期日是否為月底, 即期日到清算日有幾天)</returns>
        public (DateTime, bool, int) GetSettlementDate(DateTime tradeDate, int tenor, int month = 3)
        {
            DateTime spotDate = AddTradeDate(tradeDate.Date, 2); //# 即期日為 T+2交易日
            DateTime lastDate = GetLastDateForMonth(spotDate.Date); //# 當月最後一個交易日
            bool isLastDate = false; //# 是不是月底
            //#當即期日為月底日時, 未來清算日皆為月底日
            if (spotDate.Date == lastDate.Date)
            {
                isLastDate = true;
            }
            if (tenor == 0) //# 回傳即期日
            {
                return (spotDate, isLastDate, 0);
            }
            //# 清算日邏輯如下
            DateTime settlementDay = spotDate.AddMonths(tenor * month);
            if (isLastDate) //# 如果即期日是月底, 清算日即為月底
            {
                settlementDay = GetLastDateForMonth(settlementDay);
            }
            else
            {
                settlementDay = AdjustSettlementDay(settlementDay); //# 調整清算日邏輯
            }
            int totalDays = (int)(settlementDay - spotDate).TotalDays;
            return (settlementDay, isLastDate, totalDays);
        }
        /// <summary>
        /// 加幾個交易日的日期
        /// </summary>
        /// <param name="date">起算日</param>
        /// <param name="days">加幾個交易日</param>
        /// <returns>日期</returns>
        public DateTime AddTradeDate(DateTime date, int days)
        {
            var tradeDate = (from item in m_tradeDateList
                             where item > date
                             orderby item
                             select item).Take(days).Max();
            return tradeDate;
        }
        /// <summary>
        /// 取得當月的最後一個交易日
        /// </summary>
        /// <param name="date">當月日期</param>
        /// <returns>最後交易日</returns>
        private DateTime GetLastDateForMonth(DateTime date)
        {
            DateTime firstDate = new DateTime(date.Year, date.Month, 1);
            DateTime lastDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            var tradeDate = (from item in m_tradeDateList
                             where item >= firstDate 
                             && item <= lastDay
                             select item).Max();
            return tradeDate;
        }
        /// <summary>
        /// 清算日, 遞延不跨月
        /// </summary>
        /// <param name="date">預計的清算日</param>
        /// <returns>實際的清算日</returns>
        public DateTime AdjustSettlementDay(DateTime date)
        {
            //# 先調整交易日
            var tradeDate = AdjustTradeDate(date);
            if(tradeDate.Month != date.Month) //# 跨月了
            {
                tradeDate = (from item in m_tradeDateList
                                 where item <= date
                                 orderby item descending
                                 select item).Max();
            }
            return tradeDate;
        }
    }
}
