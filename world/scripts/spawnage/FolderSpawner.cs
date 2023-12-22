using Godot;
using System;
using System.Collections.Generic;

public partial class FolderSpawner : MultiplayerSpawner
{
    [Export(PropertyHint.Dir)] public string Folder;

    protected readonly List<PackedScene> PackedScenes = new();

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        base._EnterTree();
        using var directory = DirAccess.Open(Folder);
        foreach (var res in directory.GetResources<PackedScene>())
        {
            AddSpawnableScene(res.ResourcePath);
            PackedScenes.Add(res);
        }
    }
}