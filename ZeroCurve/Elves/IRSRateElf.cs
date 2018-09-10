using System;
using System.Collections.Generic;
using System.Text;

namespace Hyflame.ZeroCurve.Elves
{
    struct IRSRateElf
    {
        public double Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Days { get; set; }
        public double DaysAct { get; set; }
        public double TenorDays { get; set; }
        public double TenorDaysAct { get; set; }
        public double Zero { get; set; }
        public double Interest { get; set; }
        public double DF { get; set; }
        public double NPV { get; set; }
    }
}
