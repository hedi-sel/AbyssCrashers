using Godot;
using System;
using System.Linq;

public partial class MonsterSpawner : FolderSpawner
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Multiplayer.IsServer())
        {
            var rat = PackedScenes.First().Instantiate<FollowerControl>();
            rat.Position = new Vector2(200, 100);
            GetNode(SpawnPath).AddChild(rat);
        }
    }
}