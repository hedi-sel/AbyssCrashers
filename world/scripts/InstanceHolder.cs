using Godot;
using System;
using System.Collections.Generic;

public static class InstanceHolder
{
    private static readonly Dictionary<Type, object> Instances = new();

    public static void Register<T>(T instance)
    {
        Instances.Add(typeof(T), instance);
    }

    public static T Get<T>()
    {
        return (T)Instances[typeof(T)];
    }
}