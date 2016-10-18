using BotFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BotFactory
{
    namespace Models
    {
        public abstract class BuildableUnit
        {
            public double BuildTime { get; set; }
            public string Model { get; set; }
            public BuildableUnit(double buildTime = 5, string name ="No name" )
            {
                BuildTime =  buildTime;
                Model = name;
            }
        }
    }
}