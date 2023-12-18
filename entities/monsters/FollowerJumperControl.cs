using Godot;
using System;

public partial class FollowerJumperControl : FollowerControl
{
    [Export] public float ViewRange = 25;
    [Export] public float PreparationTime = 0.8f;
    [Export] public float JumpPower = 50;
    [Export] public float CooldownTime = 4;

    private float ViewRangeSquared => ViewRange * ViewRange;

    private Timer _preparationTimer;
    private Timer _cooldownTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        _preparationTimer = new Timer
        {
            Autostart = false, OneShot = true, WaitTime = PreparationTime
        };
        _preparationTimer.Timeout += TriggerJump;

        _cooldownTimer = new Timer
        {
            Autostart = false, OneShot = true, WaitTime = CooldownTime
        };
        _cooldownTimer.Timeout += () => _currentState = State.Idle;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        switch (_currentState)
        {
            case State.Idle:
                if ((ClosestPlayer.Position - Position).LengthSquared() < ViewRangeSquared)
                {
                    _currentState = State.PreparingJump;
                    _preparationTimer.Start();
                }

                break;
            case State.Jumping:
                Velocity.MoveToward(Vector2.Zero, (float)delta);
                MoveAndSlide();
                break;
        }
    }

    private void TriggerJump()
    {
        Velocity = (ClosestPlayer.Position - Position).Normalized() * JumpPower;
    }

    private State _currentState = State.Idle;
    protected override bool CanMove() => _currentState != State.PreparingJump && _currentState != State.Jumping;

    public enum State
    {
        Idle,
        PreparingJump,
        Jumping,
        Cooldown,
    }
}