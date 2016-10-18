using BotFactory.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotFactory.Factories
{
    public class FactoryQueueElement
    {
        public FactoryQueueElement(string name, Type model, Coordinates parkingPos, Coordinates workingPos)
        {
            Name = name;
            Model = model;
            ParkingPos = parkingPos;
            WorkingPos = workingPos;
        }
        public string Name { get; set; }
        public Type Model { get; set; }
        public Coordinates ParkingPos { get; set; }
        public Coordinates WorkingPos { get; set; }
    }
}
