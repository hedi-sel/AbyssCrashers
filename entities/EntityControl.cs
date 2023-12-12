using Godot;
using System;
using System.Linq;

public abstract partial class EntityControl : CharacterBody2D
{
    public T GetComponent<T>() where T : Node2D
    {
        return (T)GetChildren().First(node => node is T);
    }

    public abstract void TakeDamage(Vector2 knockback, float damage);

    private bool _facingRight = true;
    protected void FaceDirection(bool faceRight)
    {
        if (faceRight == _facingRight) return;
        _facingRight = faceRight;
        Scale = Scale.Reflect(Vector2.Up);
    }
}