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
            double FV = PV * 利率因子(Rn, t, m);
            return FV;
        }
        public static double 連續複利終值(double PV, double Rn, int t)
        {
            //# 假設m趨近無限
            //# FV = PV * e^(Rn * t)
            double FV = PV * Math.Exp((Rn / 100) * t);
            return FV;
        }
        public static double 利率因子(double Rn, int t, int m = 1)
        {
            //# V = (1 + Rn / m )^(m*t)
            double V = Math.Pow(1 + (Rn / 100) / m, m * t);
            return V;
        }
        public static double 折現因子(double Rn, int t, int m = 1)
        {
            //# V = 1 / (1 + Rn / m )^(m*t)
            //# V = 1 / 利率因子
            double V = 1 / 利率因子(Rn, t, m);
            return V;
        }
        public static double 現值(double FV, double Rn, int t, int m = 1)
        {
            //# P = S / ( 1 + Rn)^t
            //# P = S * 折現因子
            double PV = FV * 折現因子(Rn, t);
            return PV;
        }
    }
}
