using System.Numerics;
using Spectre.Console;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly List<List<string>> _input;
    private readonly List<List<long>> _nums;
    private readonly CephData _cephData;

    public Day06()
    {
        _input = [.. File.ReadAllLines(InputFilePath).Select(l => l.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList())];
        _nums = [.. _input[..^1].Select(l => l.Select(long.Parse).ToList())];
        _cephData = new CephData([.. File.ReadAllLines(InputFilePath)]);
    }

    public override ValueTask<string> Solve_1() {
        BigInteger sum = 0;
        for (var i = 0; i < _input[0].Count; i++)
            sum += CalcColumn(i);
        return new($"{sum}");
    }

    long CalcColumn(int x)
    {
        var operation = _input.Last()[x];
        return operation switch
        {
            "+" => _nums.Select(n => n[x]).Sum(),
            "*" => _nums.Select(n => n[x]).Aggregate((long)1, (acc, n) => acc * n),
            _ => throw new InvalidOperationException("Invalid Operator")
        };
    }

    public override ValueTask<string> Solve_2() {
        BigInteger totalSum = 0;
        for (var num = 0; num < _cephData.Length; num++)
        {
            var op = _cephData.GetColumnOperator(num);
            totalSum += op switch
            {
                '+' => _cephData.GetColumnNumbers(num).Sum(),
                '*' => _cephData.GetColumnNumbers(num).Aggregate((long)1, (acc, n) => acc * n),
                _ => throw new InvalidOperationException("Invalid Operator"),
            };
        }
        return new($"{totalSum}");
    }

    public class CephData
    {
        private List<string> _inputData = [];
        private List<int> _startIndexes = [];
        public int Length => _startIndexes.Count;

        public CephData(List<string> inputData)
        {
            _inputData = inputData;
            var operatorLine = _inputData.Last();
            for (var i = 0; i < operatorLine.Length; i++)
            {
                if (operatorLine[i] != ' ')
                    _startIndexes.Add(i);
            }
        }

        public List<long> GetColumnNumbers(int col)
        {
            List<long> ret = [];
            var thisStartIndex = _startIndexes[col];
            var thisEndIndex = _startIndexes.Count > col + 1 ? _startIndexes[col+1] : _inputData[0].Length + 1;
            thisEndIndex -= 2;
            for (var currentIndex = thisEndIndex; currentIndex >= thisStartIndex; currentIndex--)
            {
                var currentNum = "";
                foreach (var line in _inputData[..^1])
                {
                    var digit = line[currentIndex];       
                    if (digit != ' ')
                        currentNum += digit;
                }
                ret.Add(long.Parse(currentNum));
            }
            return ret;
        }

        public char GetColumnOperator(int col) => _inputData.Last()[_startIndexes[col]];
    }
}