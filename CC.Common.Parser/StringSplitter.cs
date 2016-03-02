using System.Collections.Generic;

namespace CC.Common.Parser
{
    /// <summary>
    /// This class will split a string by named ranges
    /// </summary>
    public class StringSplitter
    {
        private readonly Dictionary<string, IntRange> _items;
        private string _string;
        private int _neededLength;
        private readonly StringSplitterEnum _pad;
        private char _padChar;
        public string this[string columnName] { get { return GetColumn(columnName); } }

        public StringSplitter(StringSplitterEnum pad = StringSplitterEnum.PadNone)
        {
            _items = new Dictionary<string, IntRange>();
            _neededLength = 0;
            _pad = pad;
            _padChar = ' ';
        }

        public char PadChar
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
                    case StringSplitterEnum.PadNone:
                        _string = value;
                        break;
                    default:
                        _string = value;
                        break;
                }
            }
        }

        public Dictionary<string, int> FieldDefs
        {
            get
            {
                var ret = new Dictionary<string, int>();
                foreach (var field in _items.Keys)
                {
                    var item = Range(field);
                    ret.Add(field, item.End - item.Start);
                }
                return ret;
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
            foreach (var item in _items.Values)
            {
                _neededLength += item.End - item.Start + 1;
            }
        }

        private string GetColumn(string columnName)
        {
            var ret = string.Empty;
            if (!_items.ContainsKey(columnName)) return ret;
            Range<int> range = _items[columnName];
            var len = range.End - range.Start;
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
            catch
            {
                // ignored
            }
            return ret;
        }
    }
}
