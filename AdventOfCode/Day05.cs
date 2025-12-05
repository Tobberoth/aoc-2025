
namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly List<string> _input;
    private List<(long, long)> _freshRanges = [];
    private List<long> _ingredients = [];

    public Day05()
    {
        _input = [.. File.ReadAllLines(InputFilePath)];
        var freshDone = false;
        foreach (var line in _input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                freshDone = true;
                continue;
            }
            if (!freshDone)
            {
                var data = line.Split('-');
                _freshRanges.Add((long.Parse(data[0]), long.Parse(data[1])));
                continue;
            }
            _ingredients.Add(long.Parse(line));
        }
    }

    public override ValueTask<string> Solve_1() {
        var count = 0;
        foreach (var ingredientID in _ingredients)
        {
            foreach (var range in _freshRanges)
            {
                if (InRange(range, ingredientID)) {
                    count++;
                    break;
                }
            }
        }
        return new($"{count}");
    }

    private static bool InRange((long, long) range, long ingredientID)
    {
        return ingredientID >= range.Item1 && ingredientID <= range.Item2;
    }

    public override ValueTask<string> Solve_2() {
        RangeList rangeList = new();
        _freshRanges.ForEach(r => rangeList.AddRange(r));
        return new($"{rangeList.TotalCount}");
    }

    public class RangeList
    {
        private List<(long, long)> _ranges = [];
        public long TotalCount => _ranges.Sum(r => r.Item2 - r.Item1 + 1);
        public RangeList() {}
        public void AddRange((long, long) newRange)
        {
            (long,long) fixedRange = newRange;
            var collided = true;
            while (collided) {
                collided = false;
                foreach (var oldRange in _ranges)
                {
                    var collision = CheckCollision(fixedRange, oldRange);
                    if (collision != null)
                    {
                        collided = true;
                        fixedRange = collision.Value;
                        _ranges.Remove(oldRange);
                        break;
                    }
                }
            }
            _ranges.Add(fixedRange);
        }

        private static (long, long)? CheckCollision((long, long) newRange, (long, long) oldRange)
        {
            // Within?
            if (newRange.Item1 >= oldRange.Item1 && newRange.Item2 <= oldRange.Item2)
                return oldRange;
            // Over?
            if (oldRange.Item1 >= newRange.Item1 && oldRange.Item2 <= newRange.Item2)
                return newRange;
            // Top?
            if (newRange.Item1 >= oldRange.Item1 && newRange.Item1 <= oldRange.Item2 && newRange.Item2 > oldRange.Item2)
                return (oldRange.Item1, newRange.Item2);
            // Bottom?
            if (newRange.Item2 >= oldRange.Item1 && newRange.Item2 <= oldRange.Item2 && newRange.Item1 < oldRange.Item1)
                return (newRange.Item1, oldRange.Item2);
            return null;
        }
    }
}