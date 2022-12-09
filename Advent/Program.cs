using Advent.Days;

var dayObjectList = GetAllEntities();
ProcessDays(dayObjectList);

static void ProcessDays(List<(int, string)> days)
{
    foreach (var day in days.OrderBy(x => x.Item1))
    {
        var dayType = Type.GetType(day.Item2);
        IDay dayObj = (IDay)Activator.CreateInstance(dayType);
        var dayNum = day.Item1;
        var lines = File.ReadAllLines($"./Inputs/Day{dayNum}.txt");
        Console.WriteLine($"Day #{dayNum}");
        Console.WriteLine($"    Part 1: {dayObj.Part1(lines)}");
        Console.WriteLine($"    Part 2: {dayObj.Part2(lines)}");
        Console.WriteLine("------------------------");
    }
}

static List<(int, string)> GetAllEntities()
{
    List<(int, string)> returnValue = new();
    var assemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
          .Where(x => typeof(IDay).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
          .Select(x => x.FullName).ToList();
    foreach(var asm in assemblies)
    {
        var dayNum = asm.Replace("Advent.Days.Day", "");
        (int, string) entry = (Convert.ToInt32(dayNum), asm);
        returnValue.Add(entry);
    }
    return returnValue;
}