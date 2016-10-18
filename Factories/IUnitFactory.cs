using BotFactory.Common.Tools;
using BotFactory.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotFactory.Factories
{
    public interface IUnitFactory
    {
        TimeSpan QueueTime { get; set; }
        int QueueCapacity { get; set; }
        int StorageCapacity { get; set; }
        int QueueFreeSlots { get; set; }
        int StorageFreeSlots { get; set; }
        List<FactoryQueueElement> Queue { get; set; }
        List<ITestingUnit> Storage { get; set; }
        event FactoryProgress  FactoryProgress;
        bool AddWorkableUnitToQueue( Type model, string name, Coordinates parkPosition, Coordinates workPosition );
        void createRobot();
        object FactoryLogger( object model, string message );
        void ThreadRun();
    }
}
