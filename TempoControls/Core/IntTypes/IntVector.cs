namespace TempoControls.Core.IntTypes
{
    public struct IntVector
    {
        public int X;
        public int Y;

        public IntVector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static IntVector operator +(IntVector a, IntVector b) => new(a.X + b.X, a.Y + b.Y);
        public static IntVector operator -(IntVector a, IntVector b) => new(a.X - b.X, a.Y - b.Y);

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
    
    public struct DoubleVector
    {
        public double X;
        public double Y;

        public DoubleVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static DoubleVector operator +(DoubleVector a, DoubleVector b) => new(a.X + b.X, a.Y + b.Y);
        public static DoubleVector operator -(DoubleVector a, DoubleVector b) => new(a.X - b.X, a.Y - b.Y);

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}