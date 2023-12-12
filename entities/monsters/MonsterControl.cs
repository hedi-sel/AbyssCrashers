using Godot;
using System;
using System.Linq;

public partial class MonsterControl : CharacterBody2D, IEntityControl
{
    public EntityType Type => EntityType.Monster;

    [Export] public float Hp = 1;
    [Export] public float Speed = 40;

    protected AdvancedAnimationPlayer AnimationPlayer;
    protected EntityLayer EntityLayer;
    protected float HpLeft;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        AnimationPlayer = GetNode<AdvancedAnimationPlayer>(nameof(AnimationPlayer));
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
        SetPhysicsProcess(false);
        GetNode<CollisionShape2D>(nameof(CollisionShape2D)).SetDeferred(
            nameof(CollisionShape2D.Disabled).ToLower(), true);
        AnimationPlayer.PlayEvent("die", false);
        AnimationPlayer.AnimationFinished += _ => FadeOut();
    }

    private void FadeOut()
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, nameof(Modulate).ToLower(), Colors.Transparent, 1.2)
            .SetTrans(Tween.TransitionType.Quad);
        tween.Finished += QueueFree;
    }

    public void TakeDamage(Vector2 knockback, float damage)
    {
        Velocity = knockback * 500;

        HpLeft -= damage;
        if (HpLeft <= 0) Destroy();
    }
}