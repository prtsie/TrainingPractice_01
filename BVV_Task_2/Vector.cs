using System.Diagnostics.CodeAnalysis;

namespace BVV_Task_2;

public struct Vector(int x, int y)
{
    public int X { get; set; } = x;

    public int Y { get; set; } = y;

    public override string ToString() => $"{X}, {Y}";

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vector vector && vector.X == X && vector.Y == Y;
    }
}