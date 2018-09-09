using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.ZeroCurve.Elves
{
    public class ParRateElfEx
    {
        public ParRateElfEx(ParRateElf parRateInfo, DateTime startDate, DateTime endDate, double days, double daysActual, double zero)
        {
            this.Tenor = parRateInfo.Tenor;
            this.Unit = parRateInfo.Unit;
            this.Rate = parRateInfo.Rate;
            this.Market = parRateInfo.Market;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Days = days;
            this.DaysAct = daysActual;
            this.Zero = zero;
            this.DF = RateAx.折現因子_T_Z(this.Zero, this.DaysAct);
        }
        public int Tenor { get; set; }
        public EnumTenorUnit Unit { get; set; }
        public double Rate { get; set; }
        public EnumRateMarket Market { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Days { get; set; }
        public double DaysAct { get; set; }
        public double Zero { get; set; }
        public double DF { get; set; }
        public override string ToString()
        {
            return $"Tenor={Tenor}, Unit={Unit}, Rate={Rate}, Market={Market}, StartDate={StartDate}, EndDate={EndDate}, Days={Days}, DaysAct={DaysAct}, Zero={Zero}, DF={DF}";
        }
    }
}
