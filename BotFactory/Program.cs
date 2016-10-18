using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotFactory.Models;
using BotFactory.Factories;
using BotFactory.Common.Tools;

namespace BotFactory
{
    class Program
    {
        static void Main( string[ ] args )
        {
            bool t = true;
            UnitFactory factory = new UnitFactory(7,7);
            factory.AddWorkableUnitToQueue((new R2D2()).GetType(), "R2D2", new Coordinates(3,3), new Coordinates( 6, 6 ) );
            while (t == true) 
            {
                factory.createRobot();
                if (factory.Queue.Count() <= factory.QueueCapacity)
                    factory.AddWorkableUnitToQueue( ( new R2D2() ).GetType(), "R2D2", new Coordinates( 3, 3 ), new Coordinates( 6, 6 ) );
                if(factory.Queue.Count() >= factory.QueueCapacity && factory.Storage.Count >= factory.StorageCapacity )
                    t = false;
            }
            /*BaseUnit t = new TestClasse("Toto", 2);
            t.Move( new Common.Tools.Coordinates( 2, 2 ));
            t.DoSomethingElse();*/
        }
    }
}
