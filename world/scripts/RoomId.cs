using System;
using AbyssCrashers.world.scripts.helpers;
using Godot;

public record struct RoomId(int X, int Y)
{
    public static readonly Vector2 RoomSize = new(19 * 16, 11 * 16);
    public static readonly Vector2 HalfRoomSize = RoomSize / 2;

    public RoomId Next(CardinalDirection dir)
        => dir switch
        {
            CardinalDirection.Left => new RoomId(X - 1, Y),
            CardinalDirection.Right => new RoomId(X + 1, Y),
            CardinalDirection.Up => new RoomId(X, Y - 1),
            CardinalDirection.Down => new RoomId(X, Y + 1),
            _ => throw new Exception(),
        };

    public Vector2 ToPosition() => new Vector2(RoomSize.X * X, RoomSize.Y * Y);

    public static RoomId FromPosition(Vector2 position)
        => new(
            (int)Math.Floor((position.X + HalfRoomSize.X) / RoomSize.X),
            (int)Math.Floor((position.Y + HalfRoomSize.Y) / RoomSize.Y));
}