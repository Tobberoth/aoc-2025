namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly List<Junction> _input = [];
    private readonly IOrderedEnumerable<KeyValuePair<double, (Junction, Junction)>> _distances;

    public Day08()
    {
        foreach (var line in File.ReadAllLines(InputFilePath))
        {
            var data = line.Split(',');
            _input.Add(new Junction(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2])));
        }
        HashSet<(Junction, Junction)> AvailablePairs = [];
        for (var i = 0; i < _input.Count; i++)
        {
            for (var j = i+1; j < _input.Count; j++)
            {
                AvailablePairs.Add((_input[i], _input[j]));
            }
        }
        _distances = AvailablePairs.ToDictionary(p => CalculateDistance(p.Item1, p.Item2)).OrderBy(k => k.Key);
    }

    public override ValueTask<string> Solve_1() {
        Circuits circuits = new();
        foreach (var pair in _distances.Take(1000))
            circuits.Add(pair.Value.Item1, pair.Value.Item2);
        return new($"{circuits.CalculateValue}");
    }

    public override ValueTask<string> Solve_2() {
        Circuits circuits = new();
        foreach (var pair in _distances)
        {
            circuits.Add(pair.Value.Item1, pair.Value.Item2);
            if (circuits.IsDone(_input.Count))
            return new($"{(long)pair.Value.Item1.X * (long)pair.Value.Item2.X}");
        }
        throw new InvalidOperationException("Should not get here");
    }

    private static double CalculateDistance(Junction point1, Junction point2)
    {
        return Math.Sqrt(
            Math.Pow(point1.X - point2.X, 2) +
            Math.Pow(point1.Y - point2.Y, 2) +
            Math.Pow(point1.Z - point2.Z, 2)
        );
    }

    public record Junction(int X, int Y, int Z);

    public class Circuits
    {
        private List<HashSet<Junction>> _circuitList { get; set; } = [];
        public bool IsDone(int targetJunctions) => _circuitList.First().Count == targetJunctions;

        public long CalculateValue => _circuitList
            .Select(c => c.Count)
            .OrderByDescending(c => c)
            .Take(3)
            .Aggregate(1, (acc, n) => acc * n);

        public void Add(Junction junction1, Junction junction2)
        {
            if (_circuitList.Any(h => h.Contains(junction1) && h.Contains(junction2)))
                return;

            var junction1InCircuit = _circuitList.FirstOrDefault(h => h.Contains(junction1));
            var junction2InCircuit = _circuitList.FirstOrDefault(h => h.Contains(junction2));
            if (junction1InCircuit == null && junction2InCircuit == null) {
                _circuitList.Add([junction1, junction2]);
                return;
            }
            if (junction1InCircuit != null && junction2InCircuit == null) {
                junction1InCircuit.Add(junction2);
                return;
            }
            if (junction1InCircuit == null && junction2InCircuit != null) {
                junction2InCircuit.Add(junction1);
                return;
            }
            HashSet<Junction> newCircuit = [.. junction1InCircuit, .. junction2InCircuit];
            _circuitList.Remove(junction1InCircuit);
            _circuitList.Remove(junction2InCircuit);
            _circuitList.Add([.. junction1InCircuit, .. junction2InCircuit]);
        }
    }
}