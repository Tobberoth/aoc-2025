using System.Numerics;

namespace AdventOfCode;

public class Day09: BaseDay
{
    private readonly List<Point> _input;
    private readonly List<Tuple<Point, Point>> _horizontalEdges = [];
    private readonly List<Tuple<Point, Point>> _verticalEdges = [];

    public Day09()
    {
        _input = [.. File.ReadAllLines(InputFilePath).Select(l => {
            var data = l.Split(",").Select(s => int.Parse(s)).ToList();
            return new Point(data[0], data[1]);
        })];
        var loopedInput = (List<Point>)[.. _input[1..], _input[0]];
        List<Tuple<Point, Point>> edges = [];
        for (var i = 0; i < _input.Count; i++)
        {
            var edge = Tuple.Create(_input[i], loopedInput[i]);
            if (edge.Item1.Y == edge.Item2.Y)
                _horizontalEdges.Add(edge);
            else if (edge.Item1.X == edge.Item2.X)
                _verticalEdges.Add(edge);
        }
    }

    public override ValueTask<string> Solve_1() {
        var combinations = _input
            .SelectMany(x => _input, (x, y) => Tuple.Create(x, y))
            .ToList();
        BigInteger maxArea = 0;
        foreach (var pair in combinations)
        {
            var area = CalcArea(pair.Item1, pair.Item2);
            if (area > maxArea)
                maxArea = area;
        }
        return new($"{maxArea}");
    }

    public override ValueTask<string> Solve_2() {
        var sortedPoints = _input.OrderBy(p => p.Y);
        var topToBottomRecs = sortedPoints
            .SelectMany((b, i) => sortedPoints.Skip(i + 1), (a, b) => b.X > a.X && b.Y > a.Y ? Tuple.Create(a, b) : null)
            .Where(x => x != null);
        var bottomToTopRecs = sortedPoints.Reverse()
            .SelectMany((b, i) => sortedPoints.Reverse().Skip(i + 1), (a, b) => b.X > a.X && b.Y < a.Y ? Tuple.Create(a, b) : null)
            .Where(x => x != null);
        var recsBySize = topToBottomRecs
            .Concat(bottomToTopRecs)
            .OrderByDescending(p => CalcArea(p.Item1, p.Item2))
            .ToList();
        foreach (var rec in recsBySize)
        {
            if (!AnyCollision(rec))
                return new($"{CalcArea(rec.Item1, rec.Item2)}");
        }
        return new("Couldn't compute");
    }

    private bool AnyCollision(Tuple<Point, Point> rec)
    {
        foreach (var edge in _horizontalEdges)
        {
            if (edge.Item1.Y > Math.Min(rec.Item1.Y, rec.Item2.Y) && edge.Item1.Y < Math.Max(rec.Item1.Y, rec.Item2.Y))
            {
                var recLeftX = Math.Min(rec.Item1.X, rec.Item2.X);
                if (Math.Min(edge.Item1.X, edge.Item2.X) <= recLeftX && Math.Max(edge.Item1.X, edge.Item2.X) > recLeftX)
                    return true;
                var recRightX = Math.Max(rec.Item1.X, rec.Item2.X);
                if (Math.Min(edge.Item1.X, edge.Item2.X) < recRightX && Math.Max(edge.Item1.X, edge.Item2.X) >= recRightX)
                    return true;
            }
        }

        foreach (var edge in _verticalEdges)
        {
            if (edge.Item1.X > Math.Min(rec.Item1.X, rec.Item2.X) && edge.Item1.X < Math.Max(rec.Item1.X, rec.Item2.X))
            {
                var recTopY = Math.Min(rec.Item1.Y, rec.Item2.Y);
                if (Math.Min(edge.Item1.Y, edge.Item2.Y) <= recTopY && Math.Max(edge.Item1.Y, edge.Item2.Y) > recTopY)
                    return true;
                var recBottomY = Math.Max(rec.Item1.Y, rec.Item2.Y);
                if (Math.Min(edge.Item1.Y, edge.Item2.Y) < recBottomY && Math.Max(edge.Item1.Y, edge.Item2.Y) >= recBottomY)
                    return true;
            }
        }

        return false;
    }

    public BigInteger CalcArea(Point p1, Point p2) =>
        (BigInteger)(Math.Abs(p1.X - p2.X) + 1) * (BigInteger)(Math.Abs(p1.Y - p2.Y) + 1);
}

public record Point(int X, int Y);