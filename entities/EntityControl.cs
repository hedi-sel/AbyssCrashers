using Godot;
using System;
using System.Linq;

public interface IEntityControl
{
    public EntityType Type { get; }

    public T GetComponent<T>() where T : Node2D
    {
        var thisNode = this as Node ?? throw new Exception(nameof(IEntityControl) + " is not a node");
        return (T)thisNode.GetChildren().First(node => node is T);
    }

    public void TakeDamage(Vector2 origin, float damage);
}

public enum EntityType
{
    None,
    Player,
    Monster,
    Static,
}