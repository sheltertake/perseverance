using System;
using System.Collections.Generic;
using System.Linq;

namespace Perseverance.Proxy.Host.Models
{
    public class PerseveranceState
    {
        public byte X { get; init; }
        public byte Y { get; init; }
        public bool?[,] Map { get; init; }
        public byte W { get; init; }
        public byte H { get; init; }
        public ICollection<Obstacle> Obstacles { get; set; }
        public Guid Guid { get; internal set; }
    }

    public static class PerseveranceStateExtensions
    {
        public static PerseveranceState ToState(this Rover rover, Guid guid)
        {
            return new()
            {
                Guid = guid,
                X = rover.X,
                Y = rover.Y,
                H = rover.Planet.H,
                W = rover.Planet.W,
                Map = rover.Planet.Map,
                Obstacles = rover.Planet.Obstacles
            };
        }
        public static Rover ToRover(this PerseveranceState state)
        {
            var planet = new Planet(h: state.H, w: state.W, obstacles: state.Obstacles?.ToArray());
            var rover = new Rover(x: state.X, y: state.Y, planet: planet);
            return rover;
        }
    }
}
