using Godot;
using System;
using AbyssCrashers.objects;

public partial class OrientedProjectile : Projectile
{
    // The prefab scene need to be oriented upwards
    public override void _Ready()
    {
        base._Ready();

        if (Math.Abs(Velocity.X) > Math.Abs(Velocity.Y))
        {
            Rotation += float.Pi / 2;
            if (Velocity.X < 0)
                Scale = new Vector2(1, -1);
        }
        else
        {
            if (Velocity.Y > 0)
                Rotation += float.Pi;
        }
    }
}