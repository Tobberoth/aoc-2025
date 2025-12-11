using System.Numerics;

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

    public BigInteger CalculateRoutes(string startServer, string endServer)
    {
        Dictionary<string, BigInteger> memo = [];
        BigInteger CalculateRoutesRec(string currentServer)
        {
            if (currentServer == endServer)
                return 1;
            if (currentServer == "out")
                return 0;
            if (memo.ContainsKey(currentServer))
                return memo[currentServer];
            BigInteger routes = 0;
            foreach (var next in _serverRacks[currentServer])
                routes += CalculateRoutesRec(next);
            memo[currentServer] = routes;
            return routes;
        }
        return CalculateRoutesRec(startServer);
    }

    public override ValueTask<string> Solve_1() {
        return new($"{CalculateRoutes("you","out")}");
    }

    public override ValueTask<string> Solve_2() {
        var dacToOut = CalculateRoutes("dac", "out");
        var fftToOut = CalculateRoutes("fft", "out");
        var dacToFft = CalculateRoutes("dac", "fft");
        var fftToDac = CalculateRoutes("fft", "dac");
        var srvToDac = CalculateRoutes("svr", "dac");
        var srvToFft = CalculateRoutes("svr", "fft");
        BigInteger sum = (dacToOut * fftToDac * srvToFft) + (fftToOut * dacToFft * srvToDac);
        return new($"{sum}");
    }
}