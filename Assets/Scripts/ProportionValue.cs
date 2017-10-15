using System.Collections.Generic;
using Random = System.Random;
using System;

public class ProportionValue<T>
{
    public float Proportion { get; set; }
    public T Value { get; set; }
}

public static class  ProportionValue
{
    public static ProportionValue<T> Create<T>(float proportion, T value)
    {
        return new ProportionValue<T> { Proportion = proportion, Value = value };
    }

    static Random random = new Random();
    public static T ChoseByRandom<T>(this IEnumerable<ProportionValue<T>> collection)
    {
        var rnd = random.NextDouble();
        foreach (var num in collection)
        {
            if (rnd < num.Proportion)
                return num.Value;
            rnd -= num.Proportion;
        }
        throw new InvalidOperationException("The proportions in the collection do not add up to 1.");
    }
}
