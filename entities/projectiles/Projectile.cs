using System;
using Godot;

namespace AbyssCrashers.objects;

public partial class Projectile : Area2D
{
    [Export] public float Range = 100;
    [Export] public float Speed { get; protected set; } = 130;
    [Export] public float Cooldown { get; set; } = 0.4f;

    [Export] public float Damage { get; set; } = 0.5f;
    [Export] public float KnockbackMultiplier { get; set; } = 1;

    protected float TraversedRange { get; private set; }

    protected Vector2 Velocity { get; private set; }

    private float _velocityLength;

    public void Initialize(Vector2 direction)
    {
        Velocity = direction * Speed;
        _velocityLength = Velocity.Length();
        TraversedRange = 0;
    }

    public override void _Ready()
    {
        base._Ready();
        if (!IsMultiplayerAuthority())
            SetProcess(false);
        else
            BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not EntityControl entity)
        {
            GD.PrintErr("No EntityControl found");
            return;
        }

        var knockback = (body.GlobalPosition - this.GlobalPosition).Normalized();
        knockback += Velocity.Normalized();
        entity.TakeDamage(knockback * KnockbackMultiplier, Damage);
        Destroy();
    }

    protected virtual void Destroy()
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