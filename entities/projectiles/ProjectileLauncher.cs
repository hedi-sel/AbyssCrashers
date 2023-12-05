using Godot;
using System;
using AbyssCrashers.objects;

public partial class ProjectileLauncher : Node2D
{
    [Export] public PackedScene Projectile;

    [Export] public float Range = 1;
    [Export] public float ProjectileSpeed = 100;

    [Export] public float LaunchDelay = 0.3f;

    private Random _random = new();

    private Timer _delay = new Timer()
    {
        Autostart = false,
        OneShot = true,
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _delay.WaitTime = LaunchDelay;
        AddChild(_delay);
    }

    public bool KeepActive(Vector2 direction) //Call every frame whule active
    {
        if (_delay.IsStopped())
        {
            Launch(direction);
            _delay.Start();
            return true;
        }

        return false;
    }

    private void Launch(Vector2 direction)
    {
        var projectileScene = Projectile.Instantiate<Projectile>();
        projectileScene.Initialize(direction * ProjectileSpeed, Range);
        projectileScene.Name = "Projectile_" + _random.Next();

        Instance.Get<ProjectileLayer>().AddChild(projectileScene);
        projectileScene.GlobalPosition = this.GlobalPosition;
    }
}