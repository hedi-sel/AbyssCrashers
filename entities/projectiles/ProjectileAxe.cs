using Godot;
using System;
using AbyssCrashers.objects;

public partial class ProjectileAxe : Projectile
{
    private AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        base._Ready();
        AnimationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer.Play("projectile");

        if (Math.Abs(Velocity.X) > Math.Abs(Velocity.Y))
        {
            if (Velocity.X > 0)
            {
                AnimationPlayer.PlayBackwards("projectile");
                Scale = new Vector2(-1, 1);
            }

            AnimationPlayer.Advance(0.1);
        }
        else
        {
            AnimationPlayer.Advance(Velocity.Y > 0 ? 0.25 : 0.75);
        }
    }
}