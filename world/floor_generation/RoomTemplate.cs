using System.Collections.Generic;
using System.Numerics;

public class RoomTemplate
{
    private RoomTemplate(params RoomElement[] elements)
    {
        Elements = elements;
    }

    public RoomElement[] Elements;

    public static RoomTemplate StartingRoom =>
        new(new RoomElement(RoomElementType.PlayerSpawner, new Vector2(-10, -10)));
}

public record RoomElement(RoomElementType Type, Vector2 Position);

public enum RoomElementType
{
    None,
    PlayerSpawner,
}