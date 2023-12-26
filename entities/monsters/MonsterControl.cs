using Godot;
using System;
using System.Linq;

public partial class MonsterControl : EntityControl
{
    [Export] public float Speed = 40;
    [Export] public float BumpDamage = 0.5f;
    [Export] public float BumpForce = 1f;

    protected AdvancedAnimationPlayer AnimationPlayer;
    protected IEntityLayer EntityLayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        SetInactive();

        AnimationPlayer = GetNode<AdvancedAnimationPlayer>(nameof(AnimationPlayer));

        if (Multiplayer.GetUniqueId() != 1)
        {
            return;
        }

        EntityLayer = GetParent<IEntityLayer>();

        CheckPlayersPosition();

        PlayerControl.PlayerMoveRoom += (_, _) => CheckPlayersPosition();
    }

    public void SetRoom(RoomId room)
    {
        if (CurrentRoom != default) GD.PushError($"{nameof(MonsterControl)}: Room can only be set once");
        CurrentRoom = room;
    }

    private void SetInactive()
    {
        SetPhysicsProcess(false);
        SetProcess(false);
    }

    private void SetActive()
    {
        SetPhysicsProcess(true);
        SetProcess(true);
    }

    private void CheckPlayersPosition()
    {
        if (EntityLayer.GetPlayers.Any(p => p.CurrentRoom == CurrentRoom))
            SetActive();
        else
            SetInactive();
    }

    protected PlayerControl GetClosestPlayer()
        => EntityLayer.GetPlayers.MinBy(p => p.Position.DistanceSquaredTo(Position));

    protected override void Flicker()
    {
        var tween = CreateTween();
        const float time = 0.06f;
        const int count = 3;
        for (int i = 0; i < count; i++)
        {
            tween.TweenProperty(this, "modulate:a", 0, time);
            tween.TweenProperty(this, "modulate:a", 1, time);
        }

        tween.Play();
    }

    protected void TriggerSlideCollisionBump()
    {
        for (var i = 0; i < GetSlideCollisionCount(); i++)
            if (GetSlideCollision(i).GetCollider() is PlayerControl player)
                Bump(player);
    }

    protected void Bump(PlayerControl player)
    {
        player.TakeDamage((player.Position - Position).Normalized() * BumpForce, BumpDamage);
    }

    protected override void Die()
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

    public override bool TakeDamage(Vector2 knockback, float damage)
    {
        base.TakeDamage(knockback, damage);
        Velocity = knockback * 500;
        return true;
    }
}