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

        var rat = PackedScenes[1].Instantiate<MonsterControl>();
        rat.Position = new Vector2(200, 100);
        GetNode(SpawnPath).AddChild(rat);

        var slime = PackedScenes[0].Instantiate<MonsterControl>();
        slime.Position = new Vector2(200, 25);
        GetNode(SpawnPath).AddChild(slime);

        var crab = PackedScenes[2].Instantiate<MonsterControl>();
        crab.Position = new Vector2(32, 25);
        GetNode(SpawnPath).AddChild(crab);
    }
}