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
                             where item > date
                             orderby item
                             select item).First();
            return tradeDate;
        }

    }
}
