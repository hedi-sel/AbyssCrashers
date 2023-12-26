using Godot;

public record RoomElement(RoomElementType Type, Vector2I TilePosition);

public enum RoomElementType
{
    None,
    PlayerSpawner,
    Monster,
}

public record class MonsterRoomElement(MonsterType Monster, Vector2I TilePosition)
    : RoomElement(RoomElementType.Monster, TilePosition);


public enum MonsterType
{
    Jumper,
    Follower,
    Archer,
    Diagonal
}