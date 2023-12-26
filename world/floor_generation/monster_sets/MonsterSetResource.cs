using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class MonsterSetResource : Resource
{
    [Export] public int Floor;
    [Export] public PackedScene Follower;
    [Export] public PackedScene Jumper;
    [Export] public PackedScene Archer;
    [Export] public PackedScene Diagonal;

    public PackedScene GetMonsterPrefab(MonsterType monsterType)
        => monsterType switch
        {
            MonsterType.Follower => Follower,
            MonsterType.Jumper => Jumper,
            MonsterType.Archer => Archer,
            MonsterType.Diagonal => Diagonal,
            _ => throw new Exception("Monster prefab not found"),
        };

    public static Dictionary<int, MonsterSetResource> LoadMonsterSets(string dirPath)
    {
        var directory = DirAccess.Open(dirPath);
        var resources = directory.GetResources<MonsterSetResource>("tres");
        return resources.ToDictionary(resource => resource.Floor);
    }
}