namespace TempoIDE.Classes
{
    public struct IntRange
    {
        public readonly int Start;
        public readonly int End;

        public int Size => End - Start;

        public IntRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(int integer)
        {
            return integer >= Start && integer < End;
        }
    }
}