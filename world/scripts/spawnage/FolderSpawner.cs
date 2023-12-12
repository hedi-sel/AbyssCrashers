using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class FolderSpawner : MultiplayerSpawner
{
    [Export(PropertyHint.Dir)] public string Folder;

    protected readonly List<PackedScene> PackedScenes = new();

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        base._EnterTree();
        using var directory = DirAccess.Open(Folder);
        directory.ListDirBegin();
        var file = directory.GetNext();
        while (file != "")
        {
            file = Path.Join(Folder, file);
            if (file.GetExtension() == "tscn")
            {
                AddSpawnableScene(file);
                PackedScenes.Add(ResourceLoader.Load<PackedScene>(file));
            }
            file = directory.GetNext();
        }
    }
}