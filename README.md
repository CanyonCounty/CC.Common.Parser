CC.Common.Parser
================

Utility Classes for parsing files.

IRange.cs - Interface for Ranges. Defines Start, End, Contains
Range.cs - Generic Class for handling ranges
IntRange.cs - Subclass or Range for int types

StringSplitterEnum.cs - Defines PadLeft, PadRight, PadNone. Determines if the return strings are padded.

StringSplitter.cs - Can be used to easily parse a column or fix width file from COBOL or legacy (pre XML/JSON) systems.

Example:
```
  // Parse DemoFile1 - Since it's fixed width we don't need to pad it
  StringSplitter split = new StringSplitter(StringSplitterEnum.PadNone);

  // Now we add our "field" definitions - zero based positioning
  split.AddRange("Date", new IntRange(0, 8));
  split.AddRange("Name", new IntRange(8, 47));
  split.AddRange("DeptNo", new IntRange(47, 53));
  split.AddRange("DoorCode", new IntRange(54, 57));
  split.AddRange("KeyCode", new IntRange(57, 60));

  // Read our file, line by line or all at once, doesn't matter
  string[] data = new string[] { };
  using (StreamReader sr = File.OpenText("DemoFile1.txt"))
  {
    string text = sr.ReadToEnd();
    // Split using UNIX EOL
    data = text.Split('\n');
    sr.Close();
  }

  // Go through each line
  foreach (string line in data)
  {
    // "stuff" the "parser"
    split.String = line;

    // Ask for the chunk back - case sensitive, but doesn't throw an error, you get a blank string
    string date = split["Date"];
    string name = split["Name"];
    string dept = split["DeptNo"];
    string code = split["DoorCode"];
    string key = split["KeyCode"];

    Console.WriteLine("Date: {0}", DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.CurrentCulture).ToLongDateString());
    // splitter does not trim the strings
    Console.WriteLine("Name: {0}", name.Trim());
    Console.WriteLine("Dept: {0}", dept.Trim());
    Console.WriteLine("Door: {0}", code.Trim());
    Console.WriteLine("Code: {0}", key.Trim());
    Console.WriteLine();
  }
```