using Godot;
using System;

public partial class DiagonalStriderControl : MonsterControl
{
    Vector2 _currentDirection = new(RandomGenerator.GetSign(), RandomGenerator.GetSign());

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        AnimationPlayer.PlayIfNeeded("run");

        FaceDirection(_currentDirection.X > 0);

        Velocity = Velocity.MoveToward(_currentDirection * Speed, 100);
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null)
            _currentDirection = -_currentDirection.Reflect(collision.GetNormal());
        if (collision?.GetCollider() is PlayerControl player)
            Bump(player);
    }
}