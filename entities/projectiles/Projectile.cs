using System;
using Godot;

namespace AbyssCrashers.objects;

public partial class Projectile : Area2D
{
    [Export] public Vector2 Velocity { get; private set; }
    [Export] public float Range { get; private set; }

    public float TraversedRange { get; private set; }

    private float _velocityLength;

    protected AnimationPlayer AnimationPlayer { get; private set; }

    public void Initialize(Vector2 velocity, float range)
    {
        Velocity = velocity;
        _velocityLength = Velocity.Length();
        Range = range;
        TraversedRange = 0;
    }

    public override void _Ready()
    {
        base._Ready();
        AnimationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer?.Play("projectile");
        if (!IsMultiplayerAuthority())
            SetProcess(false);
        else
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