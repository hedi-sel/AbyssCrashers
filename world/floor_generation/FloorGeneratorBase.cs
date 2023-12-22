using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using AbyssCrashers.world.scripts.helpers;

public abstract partial class FloorGeneratorBase : TileMap, IEntityLayer
{
    public FloorGeneratorBase() => InstanceHolder.Register<IEntityLayer>(this);
    public List<PlayerControl> Players { get; } = new();

    protected void GenerateBaseRoom(Rect2I roomRect, int floor, List<CardinalDirection> connections)
    {
        for (var x = roomRect.Position.X; x < roomRect.End.X; x++)
        {
            for (var y = roomRect.Position.Y; y < roomRect.End.Y; y++)
            {
                SetCell(0, new Vector2I(x, y), floor,
                    atlasCoords: TileLibrary.Floor.GetRandom());
            }
        }

        for (var y = roomRect.Position.Y; y < roomRect.End.Y - 1; y++)
        {
            SetCell(1, new Vector2I(roomRect.Position.X, y), floor, GetVertTile(false));
            SetCell(1, new Vector2I(roomRect.End.X - 1, y), floor, GetVertTile(true));
        }

        for (var x = roomRect.Position.X + 1; x < roomRect.End.X - 1; x++)
        {
            SetCell(1, new Vector2I(x, roomRect.Position.Y), floor, TileLibrary.WallHorizUp.GetRandom());
            SetCell(1, new Vector2I(x, roomRect.End.Y - 1), floor, TileLibrary.WallHorizBottom.GetRandom());
        }

        SetCell(1, new Vector2I(roomRect.Position.X, roomRect.End.Y - 1), floor, GetVertTileBottom(false));
        SetCell(1, new Vector2I(roomRect.End.X - 1, roomRect.End.Y - 1), floor, GetVertTileBottom(true));

        foreach (var connection in connections)
        {
            var center = roomRect.Position - RoomId.RoomStartTile + Vector2I.Left + Vector2I.Up;
            switch (connection)
            {
                case CardinalDirection.Up:
                    SetCell(1, new Vector2I(center.X, roomRect.Position.Y), floor, TileLibrary.DoorHorizBottom);
                    break;
                case CardinalDirection.Down:
                    SetCell(1, new Vector2I(center.X, roomRect.End.Y - 1), floor, TileLibrary.DoorHorizUp);
                    break;
                case CardinalDirection.Left:
                    SetCell(2, new Vector2I(roomRect.Position.X, center.Y), floor, TileLibrary.DoorVertUp);
                    SetCell(1, new Vector2I(roomRect.Position.X, center.Y), floor,
                        TileLibrary.DoorVertUpOpen.GetRandom());
                    SetCell(1, new Vector2I(roomRect.Position.X, center.Y + 1), floor, TileLibrary.DoorVertBottom);
                    break;
                case CardinalDirection.Right:
                    SetCell(2, new Vector2I(roomRect.End.X - 1, center.Y), floor, TileLibrary.DoorVertUp);
                    SetCell(1, new Vector2I(roomRect.End.X - 1, center.Y), floor,
                        TileLibrary.DoorVertUpOpen.GetRandom());
                    SetCell(1, new Vector2I(roomRect.End.X - 1, center.Y + 1), floor, TileLibrary.DoorVertBottom);
                    break;
            }
        }

        if (!connections.Contains(CardinalDirection.Down))
        {
            for (var x = roomRect.Position.X; x < roomRect.End.X; x++)
                SetCell(0, new Vector2I(x, roomRect.End.Y), floor, TileLibrary.Black);
        }

        if (!connections.Contains(CardinalDirection.Up))
        {
            for (var x = roomRect.Position.X; x < roomRect.End.X; x++)
                SetCell(0, new Vector2I(x, roomRect.Position.Y - 1), floor, TileLibrary.Black);
        }

        Vector2I GetVertTile(bool isRight)
            => isRight && !connections.Contains(CardinalDirection.Right) ? TileLibrary.WallVertRight.GetRandom()
                : !isRight && !connections.Contains(CardinalDirection.Left) ? TileLibrary.WallVertLeft.GetRandom()
                : TileLibrary.WallVertRyoho.GetRandom();

        Vector2I GetVertTileBottom(bool isRight)
            => isRight && !connections.Contains(CardinalDirection.Right) ? TileLibrary.WallVertBottomRight.GetRandom()
                : !isRight && !connections.Contains(CardinalDirection.Left) ? TileLibrary.WallVertBottomLeft.GetRandom()
                : TileLibrary.WallVertBottomRyoho.GetRandom();
    }
}