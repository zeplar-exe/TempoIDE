using System;

namespace TempoControls.Core.IntTypes
{
    public readonly struct IntVector
    {
        public readonly int X;
        public readonly int Y;

        public IntVector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static IntVector operator +(IntVector a, IntVector b) => new(a.X + b.X, a.Y + b.Y);
        public static IntVector operator -(IntVector a, IntVector b) => new(a.X - b.X, a.Y - b.Y);
        
        public static bool operator ==(IntVector left, IntVector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntVector left, IntVector right)
        {
            return !left.Equals(right);
        }
        
        public bool Equals(IntVector other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is IntVector other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        
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