using Godot;
using System;
using AbyssCrashers.objects;
using AbyssCrashers.world.scripts.helpers;

public partial class ProjectileAxe : Projectile
{
    private AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        base._Ready();
        AnimationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer.Play("projectile");

        var dir = Velocity.ToCardinalDirection();

        if (dir.IsHorizontal())
            AnimationPlayer.Advance(0.1);
        if (dir == CardinalDirection.Right)
        {
            AnimationPlayer.PlayBackwards("projectile");
            Scale = new Vector2(-1, 1);
        }
        else if(dir == CardinalDirection.Up)
            AnimationPlayer.Advance(0.75);
        else if(dir == CardinalDirection.Down)
            AnimationPlayer.Advance(0.25);
    }
}