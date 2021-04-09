using System.Collections.Generic;
using System.Linq;

using UnityEngine;

// TODO: unused?
namespace GameObjectExtensions
{
	public static class GameObjectExtensions
	{
		public static Bounds GetBounds(this GameObject obj)
		{
			Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
			SpriteRenderer test = obj.GetComponent<SpriteRenderer>();
			if (test) bounds = test.bounds;

			/* Bounds bounds = obj.GetComponent<SpriteRenderer>()?.bounds */
			/* 	         ?? new Bounds(obj.transform.position, Vector3.zero); */

			foreach (GameObject child in obj.GetChildren())
			{
				bounds.Encapsulate(child.GetBounds());
			}

			return bounds;

		}

		public static IEnumerable<GameObject> GetChildren(this GameObject obj) => obj
			.GetComponent<Transform>()
			.Cast<Transform>()
			.Select(transform => transform.gameObject);
	}
}
