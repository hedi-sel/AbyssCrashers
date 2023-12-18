using Godot;
using System;
using System.Linq;

public partial class MonsterSpawner : FolderSpawner
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        if (!Multiplayer.IsServer()) return;

        var mon = PackedScenes[1].Instantiate<MonsterControl>();
        mon.Position = new Vector2(-50, -50);
        GetNode(SpawnPath).AddChild(mon);

        mon = PackedScenes[0].Instantiate<MonsterControl>();
        mon.Position = new Vector2(-50, 25);
        GetNode(SpawnPath).AddChild(mon);

        mon = PackedScenes[2].Instantiate<MonsterControl>();
        mon.Position = new Vector2(50, 50);
        GetNode(SpawnPath).AddChild(mon);

        mon = PackedScenes[3].Instantiate<MonsterControl>();
        mon.Position = new Vector2(80, -50);
        GetNode(SpawnPath).AddChild(mon);
    }
}