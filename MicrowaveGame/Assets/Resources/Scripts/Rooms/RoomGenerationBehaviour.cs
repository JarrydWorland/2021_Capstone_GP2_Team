using UnityEngine;

namespace Scripts.Rooms
{
	[RequireComponent(typeof(RoomConnectionBehaviour))]
	public class RoomGenerationBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Whether this room prefab should be used in level generation.
		/// </summary>
		public bool Enabled = true;
	}
}
