using System;

namespace CC.Common.Parser
{
    public class Range<T> : IRange<T> where T : IComparable<T>
    {
        private readonly T _start;
        private readonly T _end;

        public Range(T start, T end)
        {
            if (start.CompareTo(end) <= 0)
            {
                _start = start;
                _end = end;
            }
            else
            {
                _start = end;
                _end = start;
            }
        }

        public T Start
        {
            get { return _start; }
        }

        public T End
        {
            get { return _end; }
        }

        public bool Contains(T valueToFind)
        {
            return valueToFind.CompareTo(Start) >= 0 && valueToFind.CompareTo(End) <= 0;
        }
    }
}
