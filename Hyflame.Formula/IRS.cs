using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula
{
    public static class IRS
    {
        /// <summary>
        /// 折現因子(單利)
        /// </summary>
        /// <param name="Zk">零息利率%</param>
        /// <param name="Tk">天期</param>
        /// <returns></returns>
        public static double Vk1(double Zk, double Tk)
        {
            double ret = 1 / (1 + Zk / 100 * Tk);
            return ret;
        }
        /// <summary>
        /// 折現因子(複利)
        /// </summary>
        /// <param name="Zk">零息利率%</param>
        /// <param name="Tk">天期</param>
        /// <returns></returns>
        public static double Vk(double Zk, double Tk)
        {
            return 1 / Math.Pow(1 + Zk / 100, Tk);
        }
        /// <summary>
        /// 遠期利率, FR By Day, 用365天計算
        /// </summary>
        /// <param name="Rt">上期利率</param>
        /// <param name="t">上期利率天期</param>
        /// <param name="Rts">下期利率</param>
        /// <param name="ts">下期利率天期</param>
        /// <returns>遠期利率</returns>
        public static double FR8Day(double Rt, int t, double Rts, int ts)
        {
            double tempA = Math.Pow(1 + Rts , ts / 365d);
            double tempB = Math.Pow(1 + Rt , t / 365d);
            return Math.Pow(tempA / tempB, 365d / (ts - t)) - 1;
        }
        /// <summary>
        /// 遠期利率, FR By Year
        /// </summary>
        /// <param name="Rt">上期利率</param>
        /// <param name="t">上期利率天期</param>
        /// <param name="Rts">下期利率</param>
        /// <param name="ts">下期利率天期</param>
        /// <returns>遠期利率</returns>
        public static double FR8Year(double Rt, int t, double Rts, int ts)
        {
            double tempA = Math.Pow(1 + Rts, ts);
            double tempB = Math.Pow(1 + Rt, t);
            return Math.Pow(tempA / tempB, ts - t) - 1;
        }
        /// <summary>
        /// 單利公式, 用365天計算
        /// </summary>
        /// <param name="P">本金</param>
        /// <param name="r">年利率</param>
        /// <param name="t">天數</param>
        /// <returns></returns>
        public static double InterestByDays365(double P, double r, int t)
        {
            return P * r * (t / 365d);
        }
        public static double InterestByYear(double P, double r, int t)
        {
            return P * r * (t / 365d);
        }
    }
}
