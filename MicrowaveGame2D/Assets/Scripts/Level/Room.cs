using System.Collections.Generic;
using UnityEngine;

namespace Level
{
	public class Room : MonoBehaviour
	{
		public IEnumerable<Door> Doors => GetComponentsInChildren<Door>(true);
		public Vector2Int coordinate;

		public static GameObject Make(GameObject original, Transform parent, Vector2Int coordinate)
		{
			GameObject gameObject = Object.Instantiate(original, parent, true);
			Room room = gameObject.GetComponent<Room>();
			room.coordinate = coordinate;
			return gameObject;
		}
	}
}
