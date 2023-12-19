using System;

public static class RandomGenerator
{
    private static readonly Random Random = new Random();

    public static int GetInt() => Random.Next();

    public static int GetSign() => Random.Next() >= 0 ? 1 : -1;
}