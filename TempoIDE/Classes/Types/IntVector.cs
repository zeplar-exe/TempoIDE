namespace TempoIDE.Classes.Types
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

        public static IntVector operator +(IntVector a, IntVector b) => new IntVector(a.X + b.X, a.Y + b.Y);
        public static IntVector operator -(IntVector a, IntVector b) => new IntVector(a.X - b.X, a.Y - b.Y);

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}