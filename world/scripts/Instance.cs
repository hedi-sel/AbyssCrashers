using Godot;
using System;
using System.Collections.Generic;

public partial class Instance : Node2D
{
    private static readonly Dictionary<Type, Instance> Instances = new();

    protected Instance()
    {
        Instances.Add(GetType(), this);
    }

    public static T Get<T>() where T : Instance
    {
        return (T)Instances[typeof(T)];
    }
}