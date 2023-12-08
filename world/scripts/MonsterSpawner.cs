using Godot;
using System;
using System.Linq;

public partial class MonsterSpawner : FolderSpawner
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var rat = PackedScenes.First().Instantiate<FollowerControl>();
		GetNode(SpawnPath).AddChild(rat);
		rat.Position = new Vector2(200, 100);
	}
}
