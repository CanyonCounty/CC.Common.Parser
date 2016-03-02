using System;

namespace CC.Common.Parser
{
    public interface IRange<T> where T : IComparable<T>
    {
        T Start { get; }
        T End { get; }
        bool Contains(T valueToFind);
    }
}
