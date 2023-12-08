using Godot;
using System;

public partial class FollowerControl : MonsterControl
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        AnimationPlayer.Play("run");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        var playerPos = GetClosestPlayer().Position;
        var movement = (playerPos - Position).Normalized();
        Velocity = Velocity.MoveToward(movement * Speed, 100);
        MoveAndCollide(Velocity * (float)delta);
    }
}