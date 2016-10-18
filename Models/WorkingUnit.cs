using BotFactory.Common.Tools;
using BotFactory.Factories;
using BotFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotFactory.Models
{
    public class WorkingUnit : BaseUnit, IWorkingUnit, ITestingUnit
    {
            public WorkingUnit(double buildTime, string name, double speed)
                : base(buildTime, name, speed)
            {
                //ParkingPos = new Coordinates( 5, 5 );
                //WorkingPos = new Coordinates( 5, 12 );
            }
            public Coordinates ParkingPos { get; set; }
            public Coordinates WorkingPos { get; set; }
            public bool IsWorking { get; set; }
        public virtual async Task<bool> WorkBegins()
        {
            var result = await Move( WorkingPos );
            IsWorking = true;
            return result;
        }
        public virtual async Task<bool> WorkEnds()
        {
            var result =await Move( ParkingPos );
            IsWorking = false;
            return result;
        }    
    }
}
