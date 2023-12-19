using Godot;
using System;
using System.Linq;

public abstract partial class EntityControl : CharacterBody2D
{
    [Export] public float MaxHealth = 3;
    private float _missingHealth = 0;

    protected float Health => MaxHealth - _missingHealth;

    public RoomId CurrentRoom { get; protected set; }

    public event Action<float> HealthUpdate;

    public virtual void AddHealth(float value)
    {
        _missingHealth -= value;
        HealthUpdate?.Invoke(Health);
        if (Health <= 0) Die();
        else if (value < 0) Flicker();
    }

    public virtual bool TakeDamage(Vector2 knockback, float damage)
    {
        AddHealth(-damage);
        return true;
    }

    protected abstract void Flicker();

    protected abstract void Die();

    private bool _facingRight = true;

    protected void FaceDirection(bool faceRight)
    {
        if (faceRight == _facingRight) return;
        _facingRight = faceRight;
        Scale = Scale.Reflect(Vector2.Up);
    }
}