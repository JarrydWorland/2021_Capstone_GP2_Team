using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helpers
{
	public static class Extensions
	{
		public static TSource RandomElement<TSource>(this IEnumerable<TSource> source)
		{
			int index = UnityEngine.Random.Range(0, source.Count());
			return source.ElementAt(index);
		}

		public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition,
			Func<TSource, bool> predicate) => condition ? source.Where(predicate) : source;

		public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition,
			Func<TSource, int, bool> predicate) => condition ? source.Where(predicate) : source;

		public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source) =>
			new HashSet<TSource>(source);

		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T> =>
			value.CompareTo(minimum) < 0 ? minimum : value.CompareTo(maximum) > 0 ? maximum : value;

		public static bool HasComponent<T>(this GameObject gameObject) => gameObject.GetComponent<T>() != null;
	}
}