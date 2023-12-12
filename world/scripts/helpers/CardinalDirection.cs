using System;
using Godot;

namespace AbyssCrashers.world.scripts.helpers;

public enum CardinalDirection
{
    Null,
    Up,
    Down,
    Left,
    Right,
}

public static class CardinalDirectionExtensions
{
    public static CardinalDirection ToCardinalDirection(this Vector2 vect, double threashold = 0.01)
    {
        if (vect.LengthSquared() < threashold) return CardinalDirection.Null;

        if (Math.Abs(vect.X) >= Math.Abs(vect.Y))
        {
            if (vect.X > 0) return CardinalDirection.Right;
            else return CardinalDirection.Left;
        }
        else
        {
            if (vect.Y > 0) return CardinalDirection.Down;
            else return CardinalDirection.Up;
        }
    }

    public static Vector2 ToVector(this CardinalDirection dir)
        => dir switch
        {
            CardinalDirection.Right => Vector2.Right,
            CardinalDirection.Left => Vector2.Left,
            CardinalDirection.Up => Vector2.Up,
            CardinalDirection.Down => Vector2.Down,
            _ => Vector2.Zero,
        };

    public static bool IsHorizontal(this CardinalDirection dir)
        => dir == CardinalDirection.Left || dir == CardinalDirection.Right;

    public static bool IsVertical(this CardinalDirection dir)
        => dir == CardinalDirection.Up || dir == CardinalDirection.Down;
}