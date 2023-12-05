using Godot;
using System;
using System.IO;
using System.Linq;

public partial class MonsterSpawner : MultiplayerSpawner
{
    [Export(PropertyHint.Dir)] public string MonsterScenesFolder;

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        base._EnterTree();
        using var directory = DirAccess.Open(MonsterScenesFolder);
        directory.ListDirBegin();
        var file = directory.GetNext();
        while (file != "")
        {
            if(file.GetExtension() == "tscn"){
                AddSpawnableScene(file);
            }
            file = directory.GetNext();
        }
    }
}