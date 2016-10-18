using System;

namespace BotFactory.Common.Tools
{
    public class Coordinates
    {
        public double x { get; set; }
        public double y { get; set; }
        public Coordinates(double X = 0, double Y = 0)
        {
            x = X;
            y = Y;
        }
        public override bool Equals(object coord)
        {
            if( coord == null )
                return false;
            Coordinates p = coord as Coordinates;
            if (p == null)
                return false;
            if( x == p.x && y == p.y )
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (int)x ^ (int)y;
        }
    }
}