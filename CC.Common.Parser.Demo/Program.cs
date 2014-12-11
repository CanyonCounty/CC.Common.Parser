using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace CC.Common.Parser.Demo
{
  class Program
  {
    private const string DATE = "Date";
    private const string NAME = "Name";
    private const string DEPT = "DeptNo";
    private const string DOOR = "DoorCode";
    private const string CODE = "KeyCode";

    static void Main(string[] args)
    {
      // Parse DemoFile1 - Since it's fixed width we don't need to pad it
      StringSplitter split = new StringSplitter(StringSplitterEnum.PadNone);

      // Now we add our "field" definitions - zero based positioning
      split.AddRange(DATE, new IntRange(0, 8));
      split.AddRange(NAME, new IntRange(8, 47));
      split.AddRange(DEPT, new IntRange(47, 53));
      split.AddRange(DOOR, new IntRange(54, 57));
      split.AddRange(CODE, new IntRange(57, 60));
      // you can also have overlaping fields
      split.AddRange("New", new IntRange(54, 60));

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
        string date = split[DATE];
        string name = split[NAME];
        string dept = split[DEPT];
        string code = split[DOOR];
        string key = split[CODE];

        Console.WriteLine("Date: {0}", DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.CurrentCulture).ToLongDateString());
        // splitter does not trim the strings, it returns the "width" you asked for
        Console.WriteLine("Name: {0}", name.Trim());
        Console.WriteLine("Dept: {0}", dept.Trim());
        Console.WriteLine("Door: {0}", code.Trim());
        Console.WriteLine("Code: {0}", key.Trim());
        // And just for giggles
        Console.WriteLine("New : {0}", split["New"].Trim());
        Console.WriteLine();
      }

      foreach (var item in split.FieldDefs)
      {
        Console.WriteLine(item.Key + ": " + item.Value.ToString());
      }
      // Wait for us
      Console.ReadLine();
    }
  }
}
