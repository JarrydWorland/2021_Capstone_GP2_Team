using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Utilities
{
	public class TagBehaviour : MonoBehaviour
	{
		private static Dictionary<string, List<GameObject>> _taggedGameObjects = new Dictionary<string, List<GameObject>>();

		[SerializeField]
		private List<string> _tags;

		private void OnEnable() => _tags.ForEach(tag => _taggedGameObjects.GetOrAdd(tag).Add(gameObject));
		private void OnDisable() => _tags.ForEach(tag => _taggedGameObjects[tag].Remove(gameObject));

		/// <summary>
		/// Checks whether the TagBehaviour has a particular tag.
		/// </summary>
		/// <param name="tag">The tag to check.</param>
		/// <returns>Returns true if it has the tag, otherwise returns false.</returns>
		public bool HasTag(string tag) => _tags.Contains(tag);

		/// <summary>
		/// Add a tag to the TagBehaviour.
		/// </summary>
		/// <param name="tag">The tag to add.</param>
		public void AddTag(string tag)
		{ 
			if (!HasTag(tag))
			{
				_tags.Add(tag);
				_taggedGameObjects.GetOrAdd(tag).Add(gameObject);
			}
		}

		/// <summary>
		/// Remove a tag from the TagBehaviour.
		/// </summary>
		/// <param name="tag">The tag to remove.</param>
		/// <returns>Returns true if the tag existed and was removed, otherwise returns false.</returns>
		public bool RemoveTag(string tag)
		{
			if (HasTag(tag))
			{
				_tags.Remove(tag);
				_taggedGameObjects[tag].Remove(gameObject);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Find all GameObjects with a given tag.
		/// </summary>
		/// <param name="tag">The tag to look for.</param>
		/// <returns>An IEnumerable of all the GameObjects with the given tag.</returns>
		public static IEnumerable<GameObject> FindWithTag(string tag) => _taggedGameObjects.GetOrAdd(tag);

		/// <summary>
		/// Find all GameObjects that have the given tags.
		/// </summary>
		/// <param name="tags">The tags to look for.</param>
		/// <returns>An IEnumerable of all the GameObjects with the given tags.</returns>
		public static IEnumerable<GameObject> FindWithTags(params string[] tags) => tags
			.Select(TagBehaviour.FindWithTag)
			.Aggregate((acc,val) => acc.Intersect(val));

		/// <summary>
		/// Get a type of component from all GameObjects that have both the
		/// component and a given tag.
		/// </summary>
		/// <param name="tag">The tag to look for.</param>
		/// <typeparam name="T">The type of component.</typeparam>
		/// <returns>An IEnumerable of all the components from GameObjects with the given tag.</returns>
		public static IEnumerable<T> GetComponentsWithTag<T>(string tag) => FindWithTag(tag)
			.Select(gameObject => gameObject.GetComponent<T>())
			.Where(component => component != null);

		/// <summary>
		/// Get a type of component from all GameObjects that have both the
		/// component and the given tags.
		/// </summary>
		/// <param name="tags">The tags to look for.</param>
		/// <typeparam name="T">The type of component.</typeparam>
		/// <returns>An IEnumerable of all the components from GameObjects with the given tags.</returns>
		public static IEnumerable<T> GetComponentsWithTags<T>(params string[] tags) => FindWithTags(tags)
			.Select(gameObject => gameObject.GetComponent<T>())
			.Where(component => component != null);
	}
}
