namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly Dictionary<string, List<string>> _serverRacks = [];

    public Day11()
    {
        File.ReadAllLines(InputFilePath)
            .Select(l => {
                var data = l.Split(':', StringSplitOptions.TrimEntries);
                var key = data[0];
                var outputs = data[1].Split(' ', StringSplitOptions.TrimEntries).ToList();
                return Tuple.Create(key, outputs);
            })
            .ToList()
            .ForEach(t => _serverRacks.Add(t.Item1, t.Item2));
    }

    Dictionary<string, long> memo = [];
    public long RoutesToOut(string currentServer)
    {
        if (currentServer == "out")
            return 1;
        if (memo.ContainsKey(currentServer))
            return memo[currentServer];
        long routes = 0;
        foreach (var next in _serverRacks[currentServer])
            routes += RoutesToOut(next);
        memo[currentServer] = routes;
        return routes;
    }

    public override ValueTask<string> Solve_1() {
        return new($"{RoutesToOut("you")}");
    }

    public override ValueTask<string> Solve_2() {
        return new("Step 2");
    }
}