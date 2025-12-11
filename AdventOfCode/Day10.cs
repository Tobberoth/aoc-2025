using System.Numerics;

namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly List<string> _input;
    private readonly List<Machine> _machines;

    public Day10()
    {
        _input = [.. File.ReadAllLines(InputFilePath)];
        _machines = [.. _input.Select(l => new Machine(l))];
    }

    public override ValueTask<string> Solve_1() {
        BigInteger sum = _machines.Select(m => m.MinimumStepsToSolveLights()).Sum();
        return new($"{sum}");
    }

    public override ValueTask<string> Solve_2() {
        BigInteger sum = 0;
        var count = 0;
        foreach (var mach in _machines) {
            sum += mach.MinimumStepsToSolveJoltage();
            Console.WriteLine($"{count}");
        }
        return new($"{sum}");
    }
}

public class Machine
{
    public List<bool> IndicatorLightsTarget { get; private set; } = [];
    public List<List<int>> Buttons { get; private set; } = [];
    public List<int> JoltageRequirements { get; private set; } = [];

    public Machine(string line)
    {
        var data = line.Split(' ');
        IndicatorLightsTarget = [.. data[0][1..^1].Select(c => c != '.')];
        Buttons = [.. data[1..^1].Select(b => b[1..^1].Split(',').Select(int.Parse).ToList())];
        JoltageRequirements = [.. data[^1][1..^1].Split(',').Select(int.Parse)];
    }

    public List<bool> PressButtonLights(int whichButton, List<bool> indicatorLights = null)
    {
        indicatorLights ??= [.. IndicatorLightsTarget.Select(b => false)];
        bool[] ret = new bool[indicatorLights.Count];
        indicatorLights.CopyTo(ret);
        if (whichButton >= Buttons.Count)
            throw new InvalidOperationException($"{whichButton} is not a valid button for {this}");
        var button = Buttons[whichButton];
        foreach (var index in button)
            ret[index] = !indicatorLights[index];
        return [.. ret];
    }

    public List<int> PressButtonJoltage(int whichButton, List<int> currentJoltage = null)
    {
        currentJoltage ??= [.. JoltageRequirements.Select(i => 0)];
        int[] ret = new int[currentJoltage.Count];
        currentJoltage.CopyTo(ret);
        if (whichButton >= Buttons.Count)
            throw new InvalidOperationException($"{whichButton} is not a valid button for {this}");
        var button = Buttons[whichButton];
        foreach (var index in button)
            ret[index] = currentJoltage[index] + 1;
        return [.. ret];
    }

    Dictionary<string, long> memoLights = [];
    public long MinimumStepsToSolveLights(List<bool> indicatorLights = null)
    {
        indicatorLights ??= [.. IndicatorLightsTarget.Select(b => false)];

        var key = string.Join("", indicatorLights);
        if (memoLights.ContainsKey(key)) return memoLights[key];
        if (indicatorLights.SequenceEqual(IndicatorLightsTarget)) {
            return 0;
        }
        memoLights[key] = long.MaxValue;
        var minVal = long.MaxValue;
        for (var i = 0; i < Buttons.Count; i++)
        {
            var newIndicatorLights = PressButtonLights(i, indicatorLights);
            var val = MinimumStepsToSolveLights(newIndicatorLights);
            if (val == long.MaxValue) continue;
            val += 1;
            if (val < minVal)
                minVal = val;
        }
        memoLights[key] = minVal;
        return minVal;
    }

    Dictionary<string, long> memoJoltage = [];
    public long MinimumStepsToSolveJoltage(List<int> currentJoltage = null)
    {
        currentJoltage ??= [.. JoltageRequirements.Select(i => 0)];
        var key = string.Join(",", currentJoltage);
        if (memoJoltage.ContainsKey(key)) return memoJoltage[key];
        if (currentJoltage.SequenceEqual(JoltageRequirements)) {
            return 0;
        }
        for (var i = 0; i < currentJoltage.Count; i++)
        {
            if (currentJoltage[i] > JoltageRequirements[i]) {
                memoJoltage[key] = long.MaxValue;
                return long.MaxValue;
            }
        }

        var minVal = long.MaxValue;
        for (var i = 0; i < Buttons.Count; i++)
        {
            var newJoltage = PressButtonJoltage(i, currentJoltage);
            var val = MinimumStepsToSolveJoltage(newJoltage);
            if (val == long.MaxValue) continue;
            val += 1;
            if (val < minVal)
                minVal = val;
        }
        memoJoltage[key] = minVal;
        return minVal;
    }

    public override string ToString() =>
        $"[{string.Join("", IndicatorLightsTarget.Select(l => l ? '#' : '.'))}] {string.Join(" ", Buttons.Select(b => $"({string.Join(",", b)})"))} {{{string.Join(",", JoltageRequirements)}}}";
}