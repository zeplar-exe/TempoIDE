using System;
using System.Collections;

namespace TempoControls.Core.IntTypes
{
    public readonly struct IntRange : IEnumerable
    {
        public readonly int Start;
        public readonly int End;

        public int Size => Math.Max(Start, End) - Math.Min(Start, End);

        public IntRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(int integer)
        {
            return integer >= Start && integer < End;
        }

        public IntRange Arrange()
        {
            if (Start > End)
            {
                return new IntRange(End, Start);
            }

            return this;
        }

        public override string ToString()
        {
            return $"{Start}, {End}";
        }

        public IEnumerator GetEnumerator()
        {
            for (var integer = Start; integer < End; integer++)
                yield return integer;
        }
    }
}