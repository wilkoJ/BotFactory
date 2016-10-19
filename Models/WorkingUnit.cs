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
            StatusChangedEventArgs s = new StatusChangedEventArgs();
            s.NewStatus = String.Format( "{0} is going to work!! Tireeeedd", Name );
            OnStatusChanged( this, s );
            IsWorking = true;
            var result = await Move( WorkingPos );
            if( IsWorking )
            {
                s.NewStatus = String.Format( "{0} start working", Name );
                OnStatusChanged( this, s );
            }
            return result;
        }
        public virtual async Task<bool> WorkEnds()
        {
            StatusChangedEventArgs s = new StatusChangedEventArgs();
            s.NewStatus = String.Format( "{0} on his way home", Name );
            OnStatusChanged( this, s );
            IsWorking = false;
            var result =await Move( ParkingPos );
            if( !IsWorking )
            {
                s.NewStatus = String.Format( "{0} stop working", Name );
                OnStatusChanged( this, s );
            }
            return result;
        }    
    }
}
