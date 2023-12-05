using System;
using Godot;

namespace AbyssCrashers.objects;

public partial class Projectile : Area2D
{
    public Vector2 Velocity { get; private set; }
    public float Range { get; private set; }
    public float TraversedRange { get; private set; }

    private float _velocityLength;

    public AnimationPlayer AnimationPlayer { get; private set; }

    public void Initialize(Vector2 velocity, float range)
    {
        Velocity = velocity;
        _velocityLength = Velocity.Length();
        Range = range;
        TraversedRange = 0;
    }

    public override void _Ready()
    {
        if (!IsMultiplayerAuthority())
        {
            SetProcess(false);
            return;
        }

        base._Ready();
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer?.Play("projectile");

        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not IEntityControl entity)
        {
            GD.PrintErr("No EntityControl found");
            return;
        }

        entity.TakeDamage(this.GlobalPosition, 1);
        Destroy();
    }

    private void Destroy()
    {
        QueueFree();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        var floatDelta = (float)delta;
        this.Position += Velocity * floatDelta;

        TraversedRange += _velocityLength * floatDelta;
        if (TraversedRange >= Range) Destroy();
    }
}