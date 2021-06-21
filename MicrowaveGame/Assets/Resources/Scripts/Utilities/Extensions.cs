using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Utilities
{
	public static class Extensions
	{
		/// <summary>
		/// Clamps a given value between a given minimum and maximum.
		/// </summary>
		/// <param name="value">The value to be clamped.</param>
		/// <param name="minimum">The minimum value.</param>
		/// <param name="maximum">The maximum value.</param>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <returns>Returns the value if it is inside the bounds, otherwise returns minimum if it was smaller, or maximum if it was larger.</returns>
		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T> =>
			value.CompareTo(minimum) < 0 ? minimum : value.CompareTo(maximum) > 0 ? maximum : value;

		/// <summary>
		/// Checks if the given game object has a given component.
		/// </summary>
		/// <param name="gameObject">The game object.</param>
		/// <typeparam name="T">The component type being checked.</typeparam>
		/// <returns>Returns true if the game object has the component, otherwise returns false.</returns>
		public static bool HasComponent<T>(this GameObject gameObject) => gameObject.GetComponent<T>() != null;

		/// <summary>
		/// Given a name, find the corresponding inactive object.
		/// </summary>
		/// <param name="name">The name of the object.</param>
		/// <returns>Returns the game object that has the given name.</returns>
		public static GameObject FindInactiveObjectByName(string name)
		{
			foreach (var transform in Resources.FindObjectsOfTypeAll<Transform>())
			{
				if (transform.hideFlags == HideFlags.None && transform.name == name) return transform.gameObject;
			}

			return null;
		}

		/// <summary>
		/// Given a collection, return an element at random.
		/// </summary>
		public static TSource GetRandomElement<TSource>(this IEnumerable<TSource> source) =>
			source.ElementAt(UnityEngine.Random.Range(0, source.Count()));

		/// <summary>
		/// Given a collection, only performs a Where() call with the given predicate if the given condition is met.
		/// </summary>
		public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition,
			Func<TSource, bool> predicate) => condition ? source.Where(predicate) : source;

		/// <summary>
		/// Given a collection, only performs a Where() call with the given predicate if the given condition is met.
		/// </summary>
		public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition,
			Func<TSource, int, bool> predicate) => condition ? source.Where(predicate) : source;

		/// <summary>
		/// Given a collection, create a hash set containing the elements from the collection.
		/// </summary>
		public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source) =>
			new HashSet<TSource>(source);
	}
}