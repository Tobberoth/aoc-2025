namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;
    private int zeroclicks { get; set; }

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() {
        var count = 0;
        var combination = 50;
        var lines = _input.Split(Environment.NewLine);
        foreach (var line in lines) {
            combination = Spin(combination, line[0], int.Parse(line[1..]));
            if (combination == 0)
                count++;
        }
        return new(count.ToString());
    }

    public override ValueTask<string> Solve_2() {
        return new(zeroclicks.ToString());
    }
    
    public int Spin(int combination, char direction, int steps) {
        var startedAtZero = combination == 0;
        if (direction == 'R') {
            combination += steps;
            if (combination > 99) {
                zeroclicks += Math.Abs(combination / 100);
            }
            combination = combination % 100;
        } else {
            combination -= steps;
            var counted = false;
            if (combination < 0) {
                counted = true;
                zeroclicks += Math.Abs(combination / 100) + 1;
                if (startedAtZero) zeroclicks -= 1;
            }
            combination = combination % 100;
            if (combination < 0) {
                combination += 100;
            } else if (combination == 0) {
                if (!counted)
                    zeroclicks++;
            }
        }
        return combination;
    }
}
