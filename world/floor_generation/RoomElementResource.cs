using Godot;

public partial class RoomElementResource : Resource
{
    [Export] public RoomElementType Type;
    [Export] public PackedScene Prefab;
}