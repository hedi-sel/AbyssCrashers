using System;

public static class RandomGenerator
{
    private static readonly Random Random = new Random();

    public static int GetInt() => Random.Next();

    public static int GetInt(int maxValue) => Math.Abs(Random.Next()) % maxValue;

    public static int GetSign() => Random.Next() >= 0 ? 1 : -1;

    public static T GetRandom<T>(this T[] array) => array[GetInt(array.Length)];
}