using System;

namespace Perseverance
{
    /// <summary>
    /// planet is the surface where rover move
    ///  - has a certain initial dimension (x,y)
    /// </summary>
    public readonly struct Planet
    {
        public byte H { get; }
        public byte W { get; }

        public bool?[,] Map { get; }

        public Point[] Obstacles { get; }

        //public bool? this[byte y, byte x] => Map[y, x];

        public Planet(byte w, byte h, Point[] obstacles = null)
        {
            W = w;
            H = h;
            Obstacles = obstacles;

            Map = new bool?[h + 1, w + 1];

            if (obstacles == null) return;

            foreach (var obstacle in obstacles)
            {
                Map[obstacle.Y, obstacle.X] = false;
            }
        }

        public void TryLand(byte Y, byte X)
        {
            if (Map[Y, X].HasValue)
                throw new ArgumentException(nameof(Y));

            Map[Y, X] = true;
        }

        public Point? TryMove(byte Y, byte X, byte targetY, byte targetX)
        {
            // wrap corrections
            if (targetX == byte.MaxValue)
                targetX = W;
            if (targetY == byte.MaxValue)
                targetY = H;

            if (targetX > W)
                targetX = 0;
            if (targetY > H)
                targetY = 0;

            // obstacles
            if (Map[targetY, targetX].HasValue)
                return null;

            // set rover position
            Map[Y, X] = null;
            Map[targetY, targetX] = true;

            return new Point()
            {
                Y = targetY,
                X = targetX
            };
        }
    }
}
