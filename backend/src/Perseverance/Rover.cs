using System;
using System.Diagnostics;

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

            if (planet[y, x].HasValue)
                throw new ArgumentException(nameof(planet));


            Planet = planet;
            X = x;
            Y = y;

            Planet.Map[Y, X] = true;
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

            Console.WriteLine($"Y from {Y} to {target}");
            if (Planet[target,X].HasValue)
                return false;

            Planet.Map[Y, X] = null;
            Planet.Map[target, X] = true;
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
            Console.WriteLine($"X from {X} to {target}");
            if (Planet[Y, target].HasValue)
                return false;


            Planet.Map[Y, X] = null;
            Planet.Map[Y, target] = true;


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

            Console.WriteLine($"X from {X} to {target}");
            if (Planet[Y, target].HasValue)
                return false;


            Planet.Map[Y, X] = null;
            Planet.Map[Y, target] = true;

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

            Console.WriteLine($"Y from {Y} to {target}");
            if (Planet[target, X].HasValue)
                return false;
            
            Planet.Map[Y, X] = null;
            Planet.Map[target,X] = true;

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
            byte w = 3,
            byte h = 3,
            Obstacle[] obstacles = null
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

        //public static string ToCommand(this string keyboardEventCode)
        //{
        //    return keyboardEventCode switch
        //    {
        //        "ArrowRight" => "R",
        //        "ArrowLeft" => "L",
        //        "ArrowUp" => "F",
        //        "ArrowDown" => "B",
        //        _ => ""
        //    };
        //}

        //private static class Commands
        //{
        //    internal const char F = 'F';
        //    internal const char B = 'B';
        //    internal const char L = 'L';
        //    internal const char R = 'R';
        //}

    }
}