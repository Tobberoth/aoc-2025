using System.Numerics;

namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly List<List<char>> _input;

    public Day07()
    {
        _input = [.. File.ReadAllLines(InputFilePath).Select(s => s.ToList())];
    }

    public override ValueTask<string> Solve_1() {
        var timesSplit = 0;
        for (var y = 1; y < _input.Count; y++)
        {
            for (var x = 0; x < _input[0].Count; x++)
            {
                if (_input[y-1][x] == 'S' || _input[y-1][x] == '|')
                {
                    if (_input[y][x] == '^')
                    {
                        timesSplit++;
                        _input[y][x-1] = '|';
                        _input[y][x+1] = '|';
                    }
                    else
                        _input[y][x] = '|';
                }
            }
        }
        return new($"{timesSplit}");
    }

    public override ValueTask<string> Solve_2() {
        BigInteger sum = 0;
        for (var x = 0; x < _input[0].Count; x++)
            if (_input[0][x] == 'S')
                sum = GetSplitterValue(0, x);

        return new($"{sum}");
    }

    private readonly Dictionary<(int, int), BigInteger> MemoMap = [];
    private BigInteger GetSplitterValue(int y, int x)
    {
        if (MemoMap.ContainsKey((y, x))) return MemoMap[(y, x)];
        if (y == _input.Count - 2)
            return 2;
        (int, int)? leftPos = null;
        (int, int)? rightPos = null;
        for (var newY = y+1; newY < _input.Count; newY++)
        {
            if (!leftPos.HasValue && _input[newY][x-1] == '^') {
                leftPos = (newY, x-1);
            }
            if (!rightPos.HasValue && _input[newY][x+1] == '^') {
                rightPos = (newY, x+1);
            }
        }
        var leftValue = leftPos.HasValue ? GetSplitterValue(leftPos.Value.Item1, leftPos.Value.Item2) : 1;
        var rightValue = rightPos.HasValue ? GetSplitterValue(rightPos.Value.Item1, rightPos.Value.Item2) : 1;
        if (!MemoMap.ContainsKey((y, x)))
            MemoMap.Add((y, x), leftValue + rightValue);
        return MemoMap[(y, x)];
    }
}
