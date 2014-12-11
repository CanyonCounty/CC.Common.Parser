using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CC.Common.Parser
{
  /// <summary>
  /// This class will split a string by named ranges
  /// </summary>
  public class StringSplitter
  {
    private Dictionary<string, IntRange> _items;
    private string _string;
    private int _neededLength;
    private StringSplitterEnum _pad;
    private char _padChar;
    public string this[string columnName] { get { return GetColumn(columnName); } }

    public StringSplitter(StringSplitterEnum pad = StringSplitterEnum.PadNone)
    {
      _items = new Dictionary<string, IntRange>();
      _neededLength = 0;
      _pad = pad;
      _padChar = ' ';
    }

    public Char PadChar
    {
      get { return _padChar; }
      set { _padChar = value; }
    }

    public string String
    {
      get { return _string; }
      set
      {
        switch (_pad)
        {
          case StringSplitterEnum.PadLeft:
            _string = value.PadLeft(_neededLength, _padChar);
            break;
          case StringSplitterEnum.PadRight:
            _string = value.PadRight(_neededLength, _padChar);
            break;
          default:
            _string = value;
            break;
        }
      }
    }

    public Range<int> Range(string columnName)
    {
      return _items[columnName];
    }

    public void Range(string columnName, IntRange range)
    {
      if (_items.ContainsKey(columnName))
      {
        _items[columnName] = range;
      }
    }

    public void AddRange(string columnName, IntRange range)
    {
      if (!_items.ContainsKey(columnName))
      {
        _items.Add(columnName, range);
      }
      else
      {
        _items[columnName] = range;
      }

      CalculateLength();
    }

    private void CalculateLength()
    {
      _neededLength = 0;
      foreach (Range<int> item in _items.Values)
      {
        _neededLength += item.End - item.Start + 1;
      }
    }

    private string GetColumn(string columnName)
    {
      string ret = String.Empty;
      if (_items.ContainsKey(columnName))
      {
        Range<int> range = _items[columnName];
        int len = range.End - range.Start;
        if (len > _string.Length) len = _string.Length;

        // Check to see if the range is currently in the string
        // if not, then return what we can
        // If we do that then the data returned it WRONG
        /*
        while (_string.Length < range.Start + len)
        {
          len -= 1;
        }
        */
        try
        {
          ret = _string.Substring(range.Start, len);
        }
        catch { }
      }
      return ret;
    }
  }
}
