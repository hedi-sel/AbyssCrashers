using System.Collections.Generic;
using Godot;

public class RoomTemplate
{
    private RoomTemplate(params RoomElement[] elements)
    {
        Elements = elements;
    }

    public readonly RoomElement[] Elements;

    public static RoomTemplate StartingRoom =>
        new(new RoomElement(RoomElementType.PlayerSpawner, Vector2I.Zero));

    public static RoomTemplate MonsterRoom =>
        new(
            new MonsterRoomElement(MonsterType.Archer, new Vector2I(-8, -4)),
            new MonsterRoomElement(MonsterType.Follower, new Vector2I(8,-4)),
            new MonsterRoomElement(MonsterType.Diagonal, new Vector2I(-8, 4)),
            new MonsterRoomElement(MonsterType.Jumper, new Vector2I(8, 4))
        );
}