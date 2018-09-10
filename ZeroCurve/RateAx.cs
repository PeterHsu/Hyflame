using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.ZeroCurve
{
    /// <summary>
    /// 利率常用的函式庫
    /// </summary>
    public static class RateAx
    {
        const double ACTUAL = 365;

        #region 已驗證過的函式
        public static double 單利終值_T(double PV, double r, double t)
        {
            double FV = PV * (1 + r * t);
            return FV;
        }
        public static double 單利終值_D(double PV, double r, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double FV = 單利終值_T(PV, r, t);
            return FV;
        }
        public static double 終值_折現因子(double PV, double DF)
        {
            double FV = PV / DF;
            return FV;
        }
        public static double 現值_折現因子(double FV, double DF)
        {
            double PV = FV * DF;
            return PV;
        }
        public static double 現值_D_零息利率(double FV, double z, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double PV = 現值_T_零息利率(FV, z, t);
            return PV;
        }
        public static double 現值_T_零息利率(double FV, double z, double t)
        {
            double PV = FV * Math.Exp(-1 * z * t);
            return PV;
        }
        public static double 終值_D_零息利率(double PV, double z, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double FV = 終值_T_零息利率(PV, z, t);
            return FV;
        }
        public static double 終值_T_零息利率(double PV, double z, double t)
        {
            double FV = PV * Math.Exp(z * t);
            return FV;
        }
        public static double 折現因子_D_零息利率(double z, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double DF = 折現因子_T_零息利率(z, t);
            return DF;
        }
        public static double 折現因子_T_零息利率(double z, double t)
        {
            double DF = Math.Exp(-1 * z * t);
            return DF;
        }
        public static double 零息利率_D_折現因子(double DF, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double z = 零息利率_T_折現因子(DF, t);
            return z;
        }
        public static double 零息利率_T_折現因子(double DF, double t)
        {
            double z = -1 * Math.Log(DF) / t;
            return z;
        }
        /// <summary>
        /// 零息利率 By Days and Par Rate
        /// </summary>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static double 零息利率_D_R(double r, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double z = 零息利率_T_R(r, t);
            return z;
        }
        /// <summary>
        /// 零息利率 By Days/Actual and Par Rate
        /// </summary>
        /// <param name="r"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double 零息利率_T_R(double r, double t)
        {
            double z = Math.Log(1 + r / 100 * t) / t;
            return z * 100;
        }
        /// <summary>
        /// TN專用零息利率
        /// </summary>
        /// <param name="DF1d"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static double TN零息利率(double DF1d, double r, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double t2 = (d - 1) / actual;
            double z = -1 * Math.Log(DF1d / (1 + r / 100 * t2)) / t;
            return z * 100;
        }
        /// <summary>
        /// 折現因子 By Days and Par Rate
        /// </summary>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static double 折現因子_D_R(double r, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double DF = 折現因子_T_R(r / 100, t);
            return DF;
        }
        /// <summary>
        /// 折現因子 By Days/Actual and Par Rate
        /// </summary>
        /// <param name="r"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double 折現因子_T_R(double r, double t)
        {
            double DF = 1 / (1 + r / 100 * t);
            return DF;
        }
        /// <summary>
        /// 折現因子 By Days and Zero Rate
        /// </summary>
        /// <param name="z"></param>
        /// <param name="d"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static double 折現因子_D_Z(double z, double d, double actual = ACTUAL)
        {
            double t = d / actual;
            double DF = 折現因子_T_Z(z, t);
            return DF;
        }
        /// <summary>
        /// 折現因子 By Days/Actual and Zero Rate
        /// </summary>
        /// <param name="z"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double 折現因子_T_Z(double z, double t)
        {
            double DF = Math.Exp(-1 * z / 100 * t);
            return DF;
        }
        #endregion 已驗證過的函式
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rt"></param>
        /// <param name="t"></param>
        /// <param name="Rts"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static double 遠期利率合約_T(double Rt, double t, double Rts, double ts)
        {
            double A = Math.Pow(1 + Rts, ts);
            double B = Math.Pow(1 + Rt, t);
            double FRA = Math.Pow(A / B, ACTUAL / (ts - t)) - 1;
            return FRA;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rt1zero">t天的Zero Rate</param>
        /// <param name="t1">t天的DaysAct</param>
        /// <param name="Rt">n天後的Zero Rate</param>
        /// <param name="n">相隔n天</param>
        /// <returns></returns>
        public static double 遠期利率2(double Rt1zero, double t1, double Rt, double n)
        {
            double FR = (Rt / 100 * (t1 + n / ACTUAL) - Rt1zero / 100 * t1) / (n / ACTUAL);
            return FR;
        }
    }
}
