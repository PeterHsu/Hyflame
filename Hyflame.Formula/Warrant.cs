using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.Formula
{
    public static class Warrant
    {
        /// <summary>
        /// 標的減資, 重新計算履約價及行使比例
        /// </summary>
        /// <param name="s">減資或合併前最後營業日標的證券收盤價</param>
        /// <param name="b">減資或換股比例, 每a股舊股換一股新股</param>
        /// <param name="d">每一股舊股退還股款</param>
        /// <param name="k">調整前之履約價格</param>
        /// <param name="n">調整前之行使比例</param>
        /// <returns>(fs:調整後的參考價, fk:調整後的履約價, fn:調整後的行使比例)</returns>
        public static (double fs, double fk, double fn) Reduction(double s, double b, double d, double k, double n)
        {
            double fs = 0; //# 調整後的參考價
            double fk = 0; //# 調整後的履約價
            double fn = 0; //# 調整後的行使比例

            fs = (s - d) * b;
            fk = Math.Round(k * fs / s, 2, MidpointRounding.AwayFromZero); //# 新履約價取小數點第二位(四捨五入)
            fn = Math.Round(n * s / fs, 3, MidpointRounding.AwayFromZero); //# 新行使比例取小數點第三位(四捨五入)

            return (fs, fk, fn);
        }
        /// <summary>
        /// 標的減資, 重新計算履約價及行使比例
        /// </summary>
        /// <param name="s">減資或合併前最後營業日標的證券收盤價</param>
        /// <param name="a">換股比率, 每一仟股換發a股</param>
        /// <param name="d">每一股舊股退還股款</param>
        /// <param name="k">調整前之履約價格</param>
        /// <param name="n">調整前之行使比例</param>
        /// <returns>(fs:調整後的參考價, fk:調整後的履約價, fn:調整後的行使比例)</returns>
        public static (double fs, double fk, double fn) ReductionEx(double s, double a, double d, double k, double n)
        {
            //#減資或換股比例, 每a股舊股換一股新股 = 換股比率, 每一仟股換發n股的倒數
            double b = 1 / a;

            double fs = 0; //# 調整後的參考價
            double fk = 0; //# 調整後的履約價
            double fn = 0; //# 調整後的行使比例

            (fs, fk, fn) = Reduction(s, b, d, k, n);

            return (fs, fk, fn);
        }
    }
}
