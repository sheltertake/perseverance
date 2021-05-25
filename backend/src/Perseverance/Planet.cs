namespace Perseverance
{
    /// <summary>
    /// planet is the surface where rover move
    ///  - has a certain initial dimension (x,y)
    /// </summary>
    public struct Planet
    {
        public byte H { get; }
        public byte W { get; }

        public bool?[,] Map { get; set; }

        public Obstacle[] Obstacles { get; }

        public bool? this[byte y, byte x] => Map[y, x];

        public Planet(byte w, byte h, Obstacle[] obstacles = null)
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

    }
}
