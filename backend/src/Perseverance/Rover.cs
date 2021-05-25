using System;

namespace Perseverance
{
    public class Rover
    {
        private static class Commands
        {
            internal const char F = 'F';
            internal const char B = 'B';
            internal const char L = 'L';
            internal const char R = 'R';
        }

        public byte X { get; private set; }
        public byte Y { get; private set; }
        public Planet Planet { get; }


        public Rover(
            Planet planet,
            byte x = 0,
            byte y = 0)
        {
            if (planet.W < x)
                throw new ArgumentException(nameof(x));
            if (planet.H < y)
                throw new ArgumentException(nameof(y));
            
            Planet = planet;
            X = x;
            Y = y;

            Planet.TryLand(Y, X);
        }

        public Rover Move(string input)
        {
            foreach (var c in input.ToUpper())
            {
                if (!Next(c))
                    break;
            }

            return this;
        }

        private bool Next(char input)
        {
            return input switch
            {
                Commands.F => Forward(),
                Commands.B => Backward(),
                Commands.L => Left(),
                Commands.R => Right(),
                _ => false,
            };
        }

        private bool Right()
        {
            byte target = (byte) (X + 1);
            var point = Planet.TryMove(Y, X, Y, target);
            if (point.HasValue)
                X = point.Value.X;

            return point.HasValue;
        }

        private bool Left()
        {
            byte target = (byte)(X - 1);
            var point = Planet.TryMove(Y, X, Y, target);
            if (point.HasValue)
                X = point.Value.X;

            return point.HasValue;
        }

        private bool Backward()
        {
            byte target = (byte)(Y - 1);
            var point = Planet.TryMove(Y, X, target, X);
            if (point.HasValue)
                Y = point.Value.Y;

            return point.HasValue;
        }

        private bool Forward()
        {
            byte target = (byte)(Y + 1);
            var point = Planet.TryMove(Y, X, target, X);
            if (point.HasValue)
                Y = point.Value.Y;

            return point.HasValue;
        }
    }

    public static class RoverFactory
    {
        /// <summary>
        /// Create Rover
        /// </summary>
        /// <returns></returns>
        public static Rover Create(
            byte x = 1,
            byte y = 1,
            byte w = 3,
            byte h = 3,
            Point[] obstacles = null
        )
        {
            return new Rover(
                planet: new Planet(
                    w: --w,
                    h: --h,
                    obstacles: obstacles
                ),
                x: x,
                y: y);
        }

    }
}