using Godot;
using System;
using System.Collections.Generic;

public static class InstanceHolder
{
    private static readonly Dictionary<Type, object> Instances = new();

    public static void Register(object instance)
    {
        Instances.Add(instance.GetType(), instance);
    }

    public static T Get<T>()
    {
        return (T)Instances[typeof(T)];
    }
}