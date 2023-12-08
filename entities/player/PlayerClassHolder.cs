using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerClassHolder : Node2D
{
    [Export] public PlayerClass[] PlayerClassList;

    private Dictionary<PlayerClass.Id, PlayerClass> _playerClassDict;

    void LoadClassDict() => _playerClassDict = PlayerClassList.ToDictionary(c => c.ClassId);

    public PlayerClass Get(PlayerClass.Id id)
    {
        if (_playerClassDict == null) LoadClassDict();
        return _playerClassDict![id];
    }
}