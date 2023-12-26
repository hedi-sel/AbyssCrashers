using Godot;
using System;
using System.Linq;

public partial class MonsterSpawner : FolderSpawner
{
    public MonsterSpawner()
    {
        InstanceHolder.Register<MonsterSpawner>(this);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    public void SpawnOne(PackedScene prefab, Vector2I tilePosition, RoomId room)
    {
        var mon = prefab.Instantiate<MonsterControl>();
        mon.Position = room.TileToGlobalPosition(tilePosition);
        mon.Name += "_" + RandomGenerator.GetInt();
        mon.SetRoom(room);
        GetNode(SpawnPath).AddChild(mon);
    }
}