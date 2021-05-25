using System;
using System.Collections.Generic;
using System.Linq;

namespace Perseverance.Proxy.Host.Models
{
    public class LandOptions
    {
        public byte H { get; set; } = 3;
        public byte W { get; set; } = 3;
        public byte X { get; set; } = 1;
        public byte Y { get; set; } = 1;
        public byte O { get; set; } = 4;
    }

    public static class LandOptionsExtensions
    {
        public static Point[] RandomObstacles(this LandOptions options)
        {
            if (options.O <= 0)
                return null;

            bool?[,] bools = new bool?[options.H, options.W];
            bools[options.Y, options.X] = true;

            int tot = options.H * options.W - 1;
            Point[] points = new Point[tot];
            var i = 0;
            for (int y = 0; y < bools.GetLength(0); y++)
            {
                for (int x = 0; x < bools.GetLength(1); x++)
                {
                    if (!bools[y, x].HasValue)
                    {
                        points[i] = new Point() { Y = (byte)y, X = (byte)x };
                        i++;
                    }
                }
            }

            var randomPoints = new List<Point>();
            var pointsToExtract = points.ToList();
            while (randomPoints.Count < options.O)
            {
                var randomValue = new Random().Next(pointsToExtract.Count);
                var extracted = points[randomValue];
                randomPoints.Add(extracted);
                pointsToExtract.Remove(extracted);
            }

            return randomPoints.ToArray();

        }
    }
}
