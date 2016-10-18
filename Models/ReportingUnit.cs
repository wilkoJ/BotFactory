﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotFactory.Models
{
    public abstract class ReportingUnit : BuildableUnit
    {
        public event UnitStatusChanged UnitStatusChanged;
        public ReportingUnit(double buildTime, string name)
            :base(buildTime, name)
        {
        }
        public virtual void OnStatusChanged(object sender, StatusChangedEventArgs args )
        {
            UnitStatusChanged( sender, args );
        }
    }
}
