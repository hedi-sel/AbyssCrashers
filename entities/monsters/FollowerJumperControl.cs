using Godot;
using System;

public partial class FollowerJumperControl : FollowerControl
{
    [Export] public float ViewRange = 25;
    [Export] public float PreparationTime = 0.8f;
    [Export] public float CooldownTime = 4;
    [Export] public float JumpPower = 500;

    private float ViewRangeSquared => ViewRange * ViewRange;


    private State _currentState = State.Idle;
    private Vector2 _jumpVelocity;

    private Timer _cooldownTimer;

    private bool _isServer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        if (!Multiplayer.IsServer()) return;

        _isServer = true;

        _cooldownTimer = new Timer
        {
            Autostart = false, OneShot = true, WaitTime = CooldownTime
        };
        _cooldownTimer.Timeout += () => _currentState = State.Idle;

        AddChild(_cooldownTimer);
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
                    _jumpVelocity = (ClosestPlayer.Position - Position).Normalized() * JumpPower;
                    AnimationPlayer.PlayEvent("prepare");
                }

                break;
            case State.Jumping:
                MoveAndSlide();
                TriggerSlideCollisionBump();
                break;
        }
    }

    public void TriggerJump()
    {
        AnimationPlayer.PlayEvent("jump");

        var tween = GetTree().CreateTween();
        Velocity = _jumpVelocity;
        tween.TweenProperty(this, "velocity", Vector2.Zero, AnimationPlayer.CurrentAnimationLength)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Quad);
        _currentState = State.Jumping;
    }

    public void FinishJump()
    {
        if (!_isServer)
            return;
        _currentState = State.Cooldown;
        _cooldownTimer.Start();
    }

    protected override bool CanMove() => _currentState != State.PreparingJump && _currentState != State.Jumping;

    public enum State
    {
        Idle,
        PreparingJump,
        Jumping,
        Cooldown,
    }
}