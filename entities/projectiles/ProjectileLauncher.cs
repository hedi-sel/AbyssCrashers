using Godot;
using System;
using AbyssCrashers.objects;

public partial class ProjectileLauncher : Node2D
{
    [Export] public PackedScene Projectile;

    private Random _random = new();

    private Timer _delay = new()
    {
        Autostart = false,
        OneShot = true,
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddChild(_delay);
    }

    public bool KeepActive(Vector2 direction) //Call every frame whule active
    {
        if (!_delay.IsStopped()) return false;

        Launch(direction);
        return true;
    }

    private void Launch(Vector2 direction)
    {
        // Position = direction.Normalized() * Position.Length();
        var projectileScene = Projectile.Instantiate<Projectile>();
        projectileScene.Initialize(direction);
        projectileScene.Name = "Projectile_" + _random.Next();

        Instance.Get<ProjectileLayer>().AddChild(projectileScene);
        projectileScene.GlobalPosition = GlobalPosition;

        _delay.WaitTime = projectileScene.Cooldown;
        _delay.Start();
    }
}