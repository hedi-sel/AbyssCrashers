using Godot;

public static class TileLibrary
{
    public static readonly Vector2I[] Floor = { new(0, 0), new(6, 0) };
    public static readonly Vector2I[] Moss = { new(1, 0), new(7, 0) };
    public static readonly Vector2I[] Burned = { new(3, 0), new(9, 0) };
    public static readonly Vector2I PlayerSpawn = new(12, 0);

    public static readonly Vector2I[] Grass =
    {
        new(2, 0), new(8, 0), new(2, 4), new(5, 4), new(10, 7), new(13, 7),
    };

    public static readonly Vector2I[] WallVertRyoho =
    {
        new(6, 9), new(7, 9), new(6, 10), new(7, 10),
        new(14, 9), new(15, 9), new(14, 10), new(15, 10),
    };

    public static readonly Vector2I[] WallVertLeft = { new(2, 9), new(2, 10), new(3, 9), new(3, 10) };
    public static readonly Vector2I[] WallVertRight = { new(12, 9), new(12, 10) };

    public static readonly Vector2I[] WallVertBottomRyoho = { new(9, 9), new(9, 10), };
    public static readonly Vector2I[] WallVertBottomLeft = { new(1, 9), new(1, 10), };
    public static readonly Vector2I[] WallVertBottomRight = { new(8, 9), new(8, 10), };

    public static readonly Vector2I[] WallHorizUp =
    {
        new(0, 5), new(1, 5), new(2, 5), new(3, 5),
        new(0, 6), new(1, 6), new(2, 6), new(3, 6),
        // Decor Wall
        new(4, 5), new(4, 6),
    };

    public static readonly Vector2I[] WallHorizBottom = { new(0, 12), new(4, 12), };

    public static readonly Vector2I DoorVertUp = new(3, 14);
    public static readonly Vector2I DoorVertBottom = new(7, 13);
    public static readonly Vector2I DoorHorizUp = new(0, 14);
    public static readonly Vector2I DoorHorizBottom = new(0, 7);

    public static readonly Vector2I[] DoorVertUpOpen =
    {
        new(8, 5), new(9, 5), new(10, 5), new(11, 5),
        new(8, 6), new(9, 6), new(10, 6), new(11, 6)
    };

    public static readonly Vector2I DoorVertBottomOpen = new(3, 13);
    public static readonly Vector2I DoorHorizUpOpen = new(1, 14);
    public static readonly Vector2I DoorHorizBottomOpen = new(1, 7);

    public static readonly Vector2I Black = new(0, 9);
}