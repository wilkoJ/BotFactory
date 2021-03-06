﻿using BotFactory.Common.Tools;
using BotFactory.Models;
using System.Threading.Tasks;
namespace BotFactory.Interface
{
    public interface IWorkingUnit
    {
        Coordinates ParkingPos { get; set; }
        Coordinates WorkingPos { get; set; }
        event UnitStatusChanged UnitStatusChanged;
        bool IsWorking { get; set; }
        Task<bool> WorkBegins();
        Task<bool> WorkEnds();
        string Name { get; set; }
        double Time { get; set; }
        double BuildTime { get; set; }
        Coordinates CurrentPos { get; set; }
        Task<bool> Move( Coordinates end );
        void DoSomethingElse();
    }
}