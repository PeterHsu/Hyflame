using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula
{
    public static class Rate
    {
        public static double 複利終值(double PV, double Rn, int t, int m = 1)
        {
            //# FV = PV * (1 + Rn / m)^(m*t)
            //# FV = PV * 利率因子
            double FV = PV * 複利利率因子(Rn, t, m);
            return FV;
        }
        public static double 連續複利終值(double PV, double Rn, int t)
        {
            //# 假設m趨近無限
            //# FV = PV * e^(Rn * t)
            double FV = PV * Math.Exp((Rn / 100) * t);
            return FV;
        }
        public static double 複利利率因子(double Rn, double t, int m = 1)
        {
            //# V = (1 + Rn / m )^(m*t)
            double V = Math.Pow(1 + (Rn / 100) / m, m * t);
            return V;
        }
        public static double 複利折現因子(double Rn, double t, int m = 1)
        {
            //# V = 1 / (1 + Rn / m )^(m*t)
            //# V = 1 / 利率因子
            double V = 1 / 複利利率因子(Rn, t, m);
            return V;
        }
        public static double 現值(double FV, double Rn, int t, int m = 1)
        {
            //# P = S / ( 1 + Rn)^t
            //# P = S * 折現因子
            double PV = FV * 複利折現因子(Rn, t);
            return PV;
        }
        public static double 單利利率因子(double Rn, double t)
        {
            //# V = 1 + Rn * t
            double V = 1 + (Rn / 100) * t;
            return V;
        }
        public static double 單利折現因子(double Rn, double t)
        {
            //# V = 1 / (1 + Rn * t)
            //# V = 1 / 利率因子
            double V = 1 / 單利利率因子(Rn, t);
            return V;
        }
        public static decimal 單利利率因子D(double Rn, double t)
        {
            //# V = 1 + Rn * t
            decimal V = 1 + ((decimal)Rn / 100) * (decimal)t;
            return V;
        }
        public static decimal 單利折現因子D(double Rn, double t)
        {
            //# V = 1 / (1 + Rn * t)
            //# V = 1 / 利率因子
            decimal V = 1 / 單利利率因子D(Rn, t);
            return V;
        }
        public static double 線性插補法(double Rt1, double t1, double Rt2, double t2, double tn)
        {
            double Rtn = Rt1 + (Rt2 - Rt1) / (t2 - t1) * (tn - t1);
            return Rtn;
        }
        public static double 線性插補法Ex(double[] points, double[] values, double point)
        {
            var interpolate = MathNet.Numerics.Interpolate.Linear(points, values);
            double Rtn = interpolate.Interpolate(point);
            return Rtn;
        }
        public static double 指數插補法(double Rt1, double t1, double Rt2, double t2, double tn)
        {
            //# 算出來完全不對, 不要用
            double Rt1Index = (tn / t1) * (t2 - tn) / (t2 - t1);
            double Rt2Index = (tn / t2) * (tn - t1) / (t2 - t1);
            double Rtn = Math.Log(Rt1, Rt1Index) * Math.Log(Rt2, Rt2Index);
            return Rtn;
        }
        public static double 遠期利率(double V1, double V2, int F)
        {
            double f = ((V1 / V2) - 1) * F;
            return f;
        }
        public static double 遠期利率合約(double Rt, double t, double Rts, double ts)
        {
            double A = Math.Pow(1 + Rts, ts / 365d);
            double B = Math.Pow(1 + Rt, t / 365d);
            double FRA = Math.Pow(A / B, 365d / (ts - t)) - 1;
            return FRA;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rt1zero">上個Tenor的Zero Rate</param>
        /// <param name="t1">上一期的天數/365</param>
        /// <param name="Rt">這期用插補法算出的利率</param>
        /// <param name="s">t-t1</param>
        /// <returns></returns>
        public static double IRS遠期利率(double Rt1zero, double t1, double Rt, double s)
        {
            //#不知道理論是什麼, 這樣算出來的結果是t1到t的遠期利率
            //#看起來是假設本金為1
            //#Rt*t = 本期利息
            //$Rt1*Rt1365 = 上期利息
            //遠期利率=(本期利息-上期利息)/90天
            double FR = (Rt * (t1 + s / 365d) - Rt1zero * t1) / (s / 365d);
            return FR;
        }

    }
}
