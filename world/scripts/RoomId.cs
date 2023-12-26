using System;
using AbyssCrashers.world.scripts.helpers;
using Godot;

public record struct RoomId(int X, int Y)
{
    private static readonly Vector2I RoomTileSize = new(19, 11);
    public static readonly Vector2I RoomPixelSize = RoomTileSize * 16;

    public static readonly Vector2I RoomStartTile = new(-10, -6);
    private static readonly Vector2I RoomFullTileSize = new(20, 11);

    private static readonly Vector2 HalfRoomPixelSize = RoomPixelSize / 2;

    public RoomId Next(CardinalDirection dir)
        => dir switch
        {
            CardinalDirection.Left => new RoomId(X - 1, Y),
            CardinalDirection.Right => new RoomId(X + 1, Y),
            CardinalDirection.Up => new RoomId(X, Y - 1),
            CardinalDirection.Down => new RoomId(X, Y + 1),
            _ => throw new Exception(),
        };

    public Vector2 GetPosition() => new(RoomPixelSize.X * X, RoomPixelSize.Y * Y);

    public Vector2 TileToGlobalPosition(Vector2I tilePosition) => GetPosition() + tilePosition * 16;

    public static RoomId FromPosition(Vector2 position) => new(
        (int)Math.Floor((position.X + HalfRoomPixelSize.X) / RoomPixelSize.X),
        (int)Math.Floor((position.Y + HalfRoomPixelSize.Y) / RoomPixelSize.Y));

    public Rect2I GetTileRect()
        => new(RoomTileSize * new Vector2I(X, Y) + RoomStartTile, RoomFullTileSize);
}