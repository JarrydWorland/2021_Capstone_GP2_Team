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
		/// Gets a value from the dictionary if one already exists, otherwise it
		/// will add a new value to the dictionary and return that instead.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key to lookup in the dictionary.</param>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <returns>Returns an existing value if one exists, otherwise adds a new value and return it.</returns>
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
			where TValue : new()
		{
			if (!dictionary.TryGetValue(key, out TValue val))
			{
				val = new TValue();
				dictionary.Add(key, val);
			}
			return val;
		}

		/// <summary>
		/// Gets a value from the dictionary if one exists, otherwise it will
		/// return a default TValue.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key to lookup in the dictionary.</param>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <returns>Returns an existing value if one exists, otherwise returns a default value.</returns>
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary.TryGetValue(key, out TValue val)) return val;
			return default;
		}

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
		/// Convert a Vector2 into a Direction
		/// </summary>
		/// <param name="vector2">The Vector2.</param>
		/// <returns>Returns the Vector2 represented as a Direction.</returns>
		public static Direction ToDirection(this Vector2 vector2)
		{
			Vector2 normalized = vector2.normalized;
			return Math.Abs(normalized.y) > Math.Abs(normalized.x)
				? normalized.y >= 0
					? Direction.North
					: Direction.South
				: normalized.x >= 0
					? Direction.East
					: Direction.West;
		}

		/// <summary>
		/// Convert a Vector3 into a Direction
		/// </summary>
		/// <param name="vector2">The Vector3.</param>
		/// <returns>Returns the Vector3 represented as a Direction.</returns>
		public static Direction ToDirection(this Vector3 vector3)
		{
			Vector3 normalized = vector3.normalized;
			return Math.Abs(normalized.y) > Math.Abs(normalized.x)
				? normalized.y >= 0
					? Direction.North
					: Direction.South
				: normalized.x >= 0
					? Direction.East
					: Direction.West;
		}

		/// <summary>
		/// Given a collection, return an element at random. Will throw an
		/// exception if the collection is empty.
		/// </summary>
		public static TSource GetRandomElement<TSource>(this IEnumerable<TSource> source) =>
			source.ElementAt(UnityEngine.Random.Range(0, source.Count()));

		/// <summary>
		/// Given a collection, return an element at random where each element
		/// is given a probability via a probability predicate. Will throw an
		/// exception if the collection is empty or the sum of the probabilities
		/// is zero.
		/// </summary>
		/// <param name="probabilityPredicate">A function which takes a element from the collection and produces a probability for that element.</param>
		/// <returns>A randomly selected element from the collection.</returns>
		public static TSource GetRandomElementWithProbability<TSource>(this IEnumerable<TSource> source, Func<TSource, float> probabilityPredicate)
		{
			float[] probabilities = source.Select(probabilityPredicate).ToArray();
			float probabilitySum = probabilities.Sum();
			
			int selectedIndex = 0;
			float random = UnityEngine.Random.Range(0.0f, probabilitySum);
			for (int i=0; i<probabilities.Length; i++)
			{
				if (random < probabilities[i])
				{
					selectedIndex = i;
					break;
				}
				random -= probabilities[i];
			}
			return source.ElementAt(selectedIndex);
		}

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
