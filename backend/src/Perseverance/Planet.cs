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

        public bool[,] Map { get; }

        public Obstacle[] Obstacles { get; }

        public bool this[byte x, byte y] => Map[x, y];

        public Planet(byte h, byte w, Obstacle[] obstacles = null)
        {
            H = h;
            W = w;
            Obstacles = obstacles;

            Map = new bool[w + 1, h + 1];

            if (obstacles == null) return;

            foreach (var obstacle in obstacles)
            {
                Map[obstacle.X, obstacle.Y] = true;
            }
        }
    }
}
