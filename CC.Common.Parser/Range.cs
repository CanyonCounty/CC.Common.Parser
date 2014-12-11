using System;

namespace CC.Common.Parser
{
public class Range<T> : IRange<T> where T : IComparable<T>
  {
    private readonly T start;
    private readonly T end;

    public Range(T start, T end)
    {
      if (start.CompareTo(end) <= 0)
      {
        this.start = start;
        this.end = end;
      }
      else
      {
        this.start = end;
        this.end = start;
      }
    }

    public T Start
    {
      get { return this.start; }
    }

    public T End
    {
      get { return this.end; }
    }

    public bool Contains(T valueToFind)
    {
      return valueToFind.CompareTo(Start) >= 0 && valueToFind.CompareTo(End) <= 0;
    }
}
