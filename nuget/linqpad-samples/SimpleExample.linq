<Query Kind="Program">
  <Reference Relative="DemoFile1.txt">&lt;MyDocuments&gt;\LINQPad Queries\CC.Common.Parser\DemoFile1.txt</Reference>
  <NuGetReference>CC.Common.Parser</NuGetReference>
  <Namespace>CC.Common.Parser</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

// naming our "fields"
private const string Date = "Date";
private const string Name = "Name";
private const string Dept = "DeptNo";
private const string Door = "DoorCode";
private const string Code = "KeyCode";

void Main()
{
  // Parse DemoFile1 - Since it's fixed width we don't need to pad it
  var split = new StringSplitter();

  // Now we add our "field" definitions - zero based positioning
  split.AddRange(Date, new IntRange(0, 8));
  split.AddRange(Name, new IntRange(8, 47));
  split.AddRange(Dept, new IntRange(47, 53));
  split.AddRange(Door, new IntRange(54, 57));
  split.AddRange(Code, new IntRange(57, 60));
  // you can also have overlaping fields
  split.AddRange("New", new IntRange(54, 60));

  // Read our file, line by line or all at once, doesn't matter
  string[] data;
  // Util.GetFullPath is just needed for running in LINQPad
  using (var sr = File.OpenText(Util.GetFullPath("DemoFile1.txt")))
  {
    var text = sr.ReadToEnd();
    // Split using EOL
    data = text.Split('\n');
    sr.Close();
  }

  // Go through each line
  foreach (var line in data)
  {
    // "stuff" the "parser"
    split.String = line;

    // Ask for the chunk back - case sensitive, but doesn't throw an error, you get a blank string
    var date = split[Date];
    var name = split[Name];
    var dept = split[Dept];
    var code = split[Door];
    var key = split[Code];

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

  Console.WriteLine("Field Definitions");
  foreach (var item in split.FieldDefs)
  {
    Console.WriteLine(item.Key + ": " + item.Value.ToString());
  }  
}
