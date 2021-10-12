using UnityEngine;
using Scripts.Levels;

namespace Scripts.Rooms
{
	[ExecuteAlways]
	public class RoomEditorBehaviour : MonoBehaviour
	{
		private void Awake()
		{
			// remove this component when the game is running
			if (Application.IsPlaying(gameObject)) Destroy(this);
		}

		private void Update()
		{
			SnapToRoomGrid();
		}

		private void SnapToRoomGrid()
		{
			float offsetX = LevelGenerator.RoomPlacementOffset.x;
			float offsetY = LevelGenerator.RoomPlacementOffset.y;
			transform.position = new Vector3
			{
				x = Mathf.Round(transform.position.x / offsetX) * offsetX,
				y = Mathf.Round(transform.position.y / offsetY) * offsetY,
				z = 0,
			};
		}
	}
}
