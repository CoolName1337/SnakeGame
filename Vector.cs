namespace snake
{
    public struct Vector
    {
        private static int Abs(int a) => a < 0 ? -a : a;
        public int X { get; set; }
        public int Y { get; set; }
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector(Vector other)
        {
            X = other.X;
            Y = other.Y;
        }
        public static Vector operator -(Vector self) => new(-self.X, -self.Y);
        public static Vector operator -(Vector v1, Vector v2) => new(v1.X - v2.X, v1.Y - v2.Y);
        public static Vector operator +(Vector v1, Vector v2) => new(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector Up { get => new(0, 1); }
        public static Vector Down { get => new(0, -1); }
        public static Vector Left { get => new(-1, 0); }
        public static Vector Right { get => new(1, 0); }
        public static Vector Random(int x, int y) => new(new Random().Next(x), new Random().Next(y));
        public static Vector Random(int x, int x1, int y, int y1) => new(new Random().Next(x, x1), new Random().Next(y, y1));

        public Vector Normalized() => new(Abs(X) >= Abs(Y) ? X / Abs(X) : 0, Abs(Y) >= Abs(X) ? Y / Abs(Y) : 0);

    }
}
