using System.Numerics;

namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly List<List<int>> _input;

    public Day03()
    {
        _input = [.. File.ReadAllLines(InputFilePath).Select(l => l.Select(c => c - '0').ToList())];
    }

    public override ValueTask<string> Solve_1() => new($"{CalculateSum(_input, 2)}");

    public override ValueTask<string> Solve_2() => new($"{CalculateSum(_input, 12)}");

    public static BigInteger CalculateSum(List<List<int>> numberList, int numLength)
    {
        BigInteger sum = 0;
        foreach (var input in numberList)
        {
            string digits = "";
            var start = 0;
            for (var turn = numLength; turn > 0; turn--)
            {
                var (digit, index) = GetNextDigit(input, turn, start);
                digits += digit.ToString();
                start = index + 1;
            }
            var outNum = long.Parse(digits);
            sum += outNum;
        }
        return sum;
    }

    public static (int, int) GetNextDigit(List<int> numbers, int pos = 12, int start = 0)
    {
        int maxVal1 = 0;
        int maxIndex = -1;
        for (var i = start; i < numbers.Count - pos + 1; i++)
        {
            if (numbers[i] > maxVal1)
            {
                maxVal1 = numbers[i];
                maxIndex = i;
            }
        }
        return (maxVal1, maxIndex);
    }
}