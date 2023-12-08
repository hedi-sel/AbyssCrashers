using Godot;
using System;
using System.Linq;

public partial class MonsterControl : CharacterBody2D, IEntityControl
{
    public EntityType Type => EntityType.Monster;

    [Export] public float Hp = 1;
    [Export] public float Speed = 40;

    protected AnimationPlayer AnimationPlayer;
    protected EntityLayer EntityLayer;
    protected float HpLeft;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        AnimationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
        EntityLayer = GetParent<EntityLayer>();

        if (Multiplayer.GetUniqueId() != 1)
        {
            SetPhysicsProcess(false);
            SetProcess(false);
            return;
        }

        HpLeft = Hp;
    }

    protected PlayerControl GetClosestPlayer()
        => EntityLayer.Players.MinBy(p => p.Position.DistanceSquaredTo(Position));

    private void Destroy()
    {
        var time = new Timer
        {
            Autostart = true,
            WaitTime = 0.5,
        };
        AddChild(time);
        time.Timeout += QueueFree;
    }

    public void TakeDamage(Vector2 origin, float damage)
    {
        var impulse = (GlobalPosition - origin).Normalized();
        Velocity = impulse * 1000;

        HpLeft -= damage;
        if (HpLeft <= 0) Destroy();
    }
}