using System;

namespace BotFactory.Common.Tools
{
    public class Vector
    {
        public double x { get; set; }
        public double y { get; set; }
        public double Length { get; set; }
        public Vector(double X, double Y, double length)
        {
            x = X;
            y = Y;
            Length = length;
        }
        static public Vector FromCoordinates( Coordinates begin, Coordinates end )
        {
            double X = begin.x - end.x;
            double Y = begin.y - end.y;
            double length = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            Vector result = new Vector( X, Y, length );
            return ( result );
        }
    }
}