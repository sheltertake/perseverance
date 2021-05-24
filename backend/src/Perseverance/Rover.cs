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

            if (planet[x, y].HasValue)
                throw new ArgumentException(nameof(planet));

            planet.Land(x,y);

            Planet = planet;
            X = x;
            Y = y;
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
        private bool Forward()
        {
            var target = Y;

            if (target < Planet.H)
            {
                target++;
            }
            else
            {
                target = 0;
            }

            if (Planet[X, target].HasValue)
                return false;

            Planet.Map[X, Y] = null;
            Planet.Map[X, target] = true;

            Y = target;

           

            return true;
        }
        private bool Right()
        {
            var target = X;

            if (target < Planet.W)
            {
                target++;
            }
            else
            {
                target = 0;
            }

            if (Planet[target, Y].HasValue)
                return false;


            Planet.Map[X, Y] = null;
            Planet.Map[target, Y] = true;

            X = target;

            return true;
        }

        private bool Left()
        {
            var target = X;

            if (target > 0)
            {
                target--;
            }
            else
            {
                target = Planet.W;
            }

            if (Planet[target, Y].HasValue)
                return false;


            Planet.Map[X, Y] = null;
            Planet.Map[target, Y] = true;

            X = target;

            return true;
        }

        private bool Backward()
        {
            var target = Y;

            if (target > 0)
            {
                target--;
            }
            else
            {
                target = Planet.H;
            }

            if (Planet[X, target].HasValue)
                return false;


            Planet.Map[X, Y] = null;
            Planet.Map[X, target] = true;

            Y = target;

            return true;
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
            byte planetHeight = 3,
            byte planetWidth = 3,
            Obstacle[] obstacles = null
        )
        {
            return new Rover(
                planet: new Planet(
                    h: --planetHeight,
                    w: --planetWidth,
                    obstacles: obstacles
                ),
                x: x,
                y: y);
        }

        public static string ToCommand(this string keyboardEventCode)
        {
            return keyboardEventCode switch
            {
                "ArrowRight" => "R",
                "ArrowLeft" => "L",
                "ArrowUp" => "F",
                "ArrowDown" => "B",
                _ => ""
            };
        }

        //private static class Commands
        //{
        //    internal const char F = 'F';
        //    internal const char B = 'B';
        //    internal const char L = 'L';
        //    internal const char R = 'R';
        //}

    }
}