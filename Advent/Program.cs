using Advent.Days;

var dayObjectList = GetAllEntities();
ProcessDays(dayObjectList);

static void ProcessDays(List<string> days)
{
    foreach (var day in days)
    {
        var dayType = Type.GetType(day);
        IDay dayObj = (IDay)Activator.CreateInstance(dayType);
        var dayNum = day.Replace("Advent.Days.Day", "");
        var lines = File.ReadAllLines($"./Inputs/Day{dayNum}.txt");
        Console.WriteLine($"Day #{dayNum}");
        Console.WriteLine($"    Part 1: {dayObj.Part1(lines)}");
        Console.WriteLine($"    Part 2: {dayObj.Part2(lines)}");
        Console.WriteLine("------------------------");
    }
}

static List<string> GetAllEntities()
{
    return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
          .Where(x => typeof(IDay).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
          .Select(x => x.FullName).ToList();
}