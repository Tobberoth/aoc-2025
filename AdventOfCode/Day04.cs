
namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly List<List<char>> _input;
    private readonly int _height;
    private readonly int _width;

    public Day04()
    {
        _input = [..File.ReadAllLines(InputFilePath).ToList().Select(l => l.ToList())];
        _height = _input.Count;
        _width = _input[0].Count;
    }

    public override ValueTask<string> Solve_1() {
        var count = 0;
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (_input[y][x] == '@' && CanAccess(y, x))
                    count++;
            }
        }
        return new($"{count}");
    }

    public override ValueTask<string> Solve_2() {
        var count = 0;
        var clearedThisTurn = true;
        while (clearedThisTurn) {
            clearedThisTurn = false;
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    if (_input[y][x] == '@' && CanAccess(y, x)) {
                        count++;
                        _input[y][x] = '.';
                        clearedThisTurn = true;
                    }
                }
            }
        }
        return new($"{count}");
    }

    private bool CanAccess(int y, int x)
    {
        var rolls = 0;
        // Top
        if ((y-1) >= 0 && (x-1) >= 0 && _input[y-1][x-1] == '@')
            rolls++;
        if ((y-1) >= 0 && _input[y-1][x] == '@')
            rolls++;
        if ((y-1) >= 0 && (x+1) < _width && _input[y-1][x+1] == '@')
            rolls++;
        // Mid
        if ((x-1) >= 0 && _input[y][x-1] == '@')
            rolls++;
        if ((x+1) < _width && _input[y][x+1] == '@')
            rolls++;
        // Bottom
        if ((y+1) < _height && (x-1) >= 0 && _input[y+1][x-1] == '@')
            rolls++;
        if ((y+1) < _height && _input[y+1][x] == '@')
            rolls++;
        if ((y+1) < _height && (x+1) < _width && _input[y+1][x+1] == '@')
            rolls++;
        
        return rolls < 4;
    }
}