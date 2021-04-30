using System.Collections.Generic;
using System.Linq;
using System;

public static class Extensions
{
	public static TSource RandomElement<TSource>(this IEnumerable<TSource> source)
	{
		int index = UnityEngine.Random.Range(0, source.Count());
		return source.ElementAt(index);
	}

	public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
	{
		return condition ? source.Where(predicate) : source;
	}

	public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
	{
		return condition ? source.Where(predicate) : source;
	}

	public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
	{
		return new HashSet<TSource>(source);
	}

	public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
	{
		if (val.CompareTo(min) < 0) return min;
		else if (val.CompareTo(max) > 0) return max;
		else return val;
	}
}