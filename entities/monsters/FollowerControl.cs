using Godot;
using System;

public partial class FollowerControl : MonsterControl
{
    protected PlayerControl ClosestPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!CanMove()) return;

        AnimationPlayer.PlayIfNeeded("run");

        ClosestPlayer = GetClosestPlayer();
        var playerPos = ClosestPlayer.Position;
        var movement = (playerPos - Position).Normalized();
        FaceDirection(movement.X > 0);

        Velocity = Velocity.MoveToward(movement * Speed, 100);
        MoveAndCollide(Velocity * (float)delta);
    }

    protected virtual bool CanMove() => true;
}