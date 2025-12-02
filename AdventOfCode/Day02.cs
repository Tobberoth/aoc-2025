using System.Numerics;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly List<(long start, long end)> _input;

    public Day02()
    {
        _input = [.. File.ReadAllText(InputFilePath).Split(",").Select(l => {
            var data = l.Split('-');
            return (long.Parse(data[0]), long.Parse(data[1]));
        })];
    }

    public override ValueTask<string> Solve_1() {
        long sum = 0;
        foreach (var range in _input)
        {
            for (var i = range.start; i <= range.end; i++)
            {
                if (IsNumberDouble(i))
                    sum += i;
            }
        }

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2() {
        BigInteger sum = 0;
        foreach (var range in _input)
        {
            for (var i = range.start; i <= range.end; i++)
            {
                if (IsNumberDoubleRecursive(i))
                    sum += i;
            }
        }

        return new(sum.ToString());
    }

    public bool IsNumberDouble(long number)
    {
        var numString = number.ToString();
        var stringLength = numString.Length;
        return numString[0..(stringLength / 2)] == numString[(stringLength / 2)..];
    }

    public bool IsNumberDoubleRecursive(long number, int level = 0)
    {
        var numString = number.ToString();
        if (level == 0)
            level = numString.Length;
        if (level == 1) return false;
        // Split into level parts
        var chunks = numString.Chunk(numString.Length / level).Select(ca => new string(ca)).ToList();
        // Check if all parts are the same.
        if (chunks.Count > 1 && chunks.Distinct().Count() == 1)
            return true;
        // If not, find the next reasonable split and try again
        return IsNumberDoubleRecursive(number, --level);
    }
}
