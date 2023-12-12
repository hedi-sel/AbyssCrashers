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

public static class PlayerClassLoader
{
    public static void LoadPlayerClass(this PlayerControl player)
    {
        var playerClassData = player.GetNode<PlayerClassHolder>(nameof(PlayerClassHolder)).Get(player.PlayerClass);

        var animPlayer = player.GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
        animPlayer.SpriteFrames = playerClassData.Frames;

        var projectileLauncher = player.GetNode<ProjectileLauncher>(nameof(ProjectileLauncher));
        projectileLauncher.Projectile = playerClassData.BaseProjectile;

        player.RunSpeed = playerClassData.RunSpeed;
    }
}