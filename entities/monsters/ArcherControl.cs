using Godot;
using System;
using System.Net;
using AbyssCrashers.world.scripts.helpers;

public partial class ArcherControl : MonsterControl
{
    [Export] public float IdealDistance = 150;

    private ProjectileLauncher _projectileLauncher;

    public override void _Ready()
    {
        base._Ready();
        _projectileLauncher = GetNode<ProjectileLauncher>(nameof(ProjectileLauncher));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (CanMove())
        {
            AnimationPlayer.PlayIfNeeded("run");
            var playerPos = GetClosestPlayer().Position;
            var relativePosition = Position - playerPos;
            var alignement = relativePosition.ToCardinalDirection().ToVector();

            var idealMovement = playerPos + alignement * IdealDistance - Position;

            // var distancing = (playerPos + alignement * IdealDistance).Normalized();
            var movement = (idealMovement - 0.7f * idealMovement.Dot(alignement) * alignement).Normalized();

            FaceDirection(movement.X > 0);

            Velocity = Velocity.MoveToward(movement * Speed, 100);
            MoveAndSlide();
            TriggerSlideCollisionBump();

            if (Math.Abs(alignement.Cross(idealMovement)) < 3f)
            {
                if (_projectileLauncher.KeepActive(-alignement))
                    AnimationPlayer.PlayEvent("attack");
                if (alignement.X != 0)
                    FaceDirection(alignement.X < 0);
            }
        }
    }

    private bool CanMove()
    {
        return AnimationPlayer.CurrentAnimation != "attack";
    }
}