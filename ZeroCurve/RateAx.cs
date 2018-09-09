using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.ZeroCurve
{
    /// <summary>
    /// 利率常用的函式庫, Ax表示僅提供最單純的Static Function
    /// </summary>
    public static class RateAx
    {
        #region 已驗證過的函式
        public static double 單利終值_T(double PV, double r, double t)
        {
            double FV = PV * (1 + r * t);
            return FV;
        }
        public static double 單利終值_D(double PV, double r, double d, double actual = 365)
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
        public static double 現值_D_零息利率(double FV, double z, double d, double actual = 365)
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
        public static double 終值_D_零息利率(double PV, double z, double d, double actual = 365)
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
        public static double 折現因子_D_零息利率(double z, double d, double actual = 365)
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
        public static double 零息利率_D_折現因子(double DF, double d, double actual = 365)
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
        public static double 零息利率_D_R(double r, double d, double actual = 365)
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
            double z = Math.Log(1 + r * t) / t;
            return z;
        }
        /// <summary>
        /// TN專用零息利率
        /// </summary>
        /// <param name="DF1d"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static double TN零息利率(double DF1d, double r, double d, double actual = 365)
        {
            double t = d / actual;
            double t2 = (d - 1) / actual;
            double z = -1 * Math.Log(DF1d / (1 + r * t2)) / t;
            return z;
        }
        /// <summary>
        /// 折現因子 By Days and Par Rate
        /// </summary>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static double 折現因子_D_R(double r, double d, double actual = 365)
        {
            double t = d / actual;
            double DF = 折現因子_T_R(r, t);
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
            double DF = 1 / (1 + r * t);
            return DF;
        }
        /// <summary>
        /// 折現因子 By Days and Zero Rate
        /// </summary>
        /// <param name="z"></param>
        /// <param name="d"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static double 折現因子_D_Z(double z, double d, double actual = 365)
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
            double DF = Math.Exp(-1 * z * t);
            return DF;
        }
        #endregion 已驗證過的函式
        public static double 遠期利率1(double Rt, double t, double Rts, double ts)
        {
            double A = Math.Pow(1 + Rts, ts / 365d);
            double B = Math.Pow(1 + Rt, t / 365d);
            double FRA = Math.Pow(A / B, 365d / (ts - t)) - 1;
            return FRA;
        }

        public static double 遠期利率2(double Rt1zero, double t1, double Rt, double s)
        {
            double FR = (Rt * (t1 + s / 365d) - Rt1zero * t1) / (s / 365d);
            return FR;
        }
    }
}
