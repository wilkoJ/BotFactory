﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotFactory.Common.Tools;

namespace BotFactory
{
    namespace Models
    {
        public abstract class BaseUnit : ReportingUnit
        {
            public string Name { get; set; }
            public double Time { get; set; }
            public Coordinates CurrentPos { get; set; }
            public BaseUnit(double buildTime, string name = "No Name", double time = 1)
                : base(buildTime, name)
            {
                Name = name;
                Time = time;
                CurrentPos = new Coordinates();
            }
            public async Task<bool> Move(Coordinates end)
            {
                StatusChangedEventArgs s = new StatusChangedEventArgs();
                s.NewStatus = String.Format( "{0} is currently moving", Name );
                OnStatusChanged( this, s );
                Vector direction = Vector.FromCoordinates( CurrentPos, end );
                int time = (int)direction.Length * (int)Time;
                await Task.Delay( time * 100 );
                CurrentPos = end;
                return true;
            }
            public void DoSomethingElse()
            {
            }
    
        }
    }
}
