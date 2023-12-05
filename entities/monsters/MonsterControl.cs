using Godot;
using System;
using System.Linq;

public partial class MonsterControl : RigidBody2D, IEntityControl
{
    public EntityType Type => EntityType.Monster;

    [Export] public float Hp = 1;
    [Export] public float Speed = 40;

    protected EntityLayer EntityLayer;
    protected float HpLeft;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        HpLeft = Hp;
        EntityLayer = GetParent<EntityLayer>();
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
        ApplyImpulse(impulse * 100);

        HpLeft -= damage;
        if (HpLeft <= 0) Destroy();
    }
}