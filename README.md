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

Sometimes you get field definitions that look like this:

RECORD LENGTH:  200   
    
FIELD NAME|POSITION|LENGTH
--------------------------
DRIVERS LICENSE NUMBER|1|9
SOCIAL SECURITY NUMBER|10|9
COUNTY OF RESIDENCE|19|2
DATE OF BIRTH|21|8
NAME|29|26
ADDRESS|55|20
SECOND LINE OF ADDRESS|75|20
CITY|95|15
STATE|110|2
ZIP CODE|112|9
FILLER|121|1
MAILING ADDRESS|122|20
SECOND LINE OF MAILING ADDRESS|142|20
MAILING CITY|162|15
MAILING STATE|177|2
MAILING ZIP CODE|179|9
FILLER|188|13

Which can easily be coded as follows

```
  int pos = 0;
  split.AddRange("DLN", new IntRange(pos, pos += 9));
  split.AddRange("SSN", new IntRange(pos, pos += 9));
  split.AddRange("County", new IntRange(pos, pos += 2));
  split.AddRange("DOB", new IntRange(pos, pos += 8));
  split.AddRange("Name", new IntRange(pos, pos += 26));
  split.AddRange("Address", new IntRange(pos, pos += 20));
  split.AddRange("Address2", new IntRange(pos, pos += 20));
  split.AddRange("City", new IntRange(pos, pos += 15));
  split.AddRange("State", new IntRange(pos, pos += 2));
  split.AddRange("Zip", new IntRange(pos, pos += 9));
  split.AddRange("Filler1", new IntRange(pos, pos += 1));
  split.AddRange("MailAddress", new IntRange(pos, pos += 20));
  split.AddRange("MailAddress2", new IntRange(pos, pos += 20));
  split.AddRange("MailCity", new IntRange(pos, pos += 15));
  split.AddRange("MailState", new IntRange(pos, pos += 2));
  split.AddRange("MailZip", new IntRange(pos, pos += 9));
  split.AddRange("Filler", new IntRange(pos, pos += 13));
```