using Godot;
using AbyssCrashers.objects;
using AbyssCrashers.world.scripts.helpers;

public partial class OrientedProjectile : Projectile
{
    // The prefab scene need to be oriented upwards
    public override void _Ready()
    {
        base._Ready();

        var dir = Velocity.ToCardinalDirection();
        if (dir.IsHorizontal())
            Rotation += float.Pi / 2;
        if (dir == CardinalDirection.Left)
            Scale = new Vector2(1, -1);
        else if (dir == CardinalDirection.Down)
            Rotation += float.Pi;
    }
}