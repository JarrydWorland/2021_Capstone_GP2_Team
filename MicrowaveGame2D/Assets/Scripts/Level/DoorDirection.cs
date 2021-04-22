using System;
using UnityEngine;

namespace Level
{
	public enum DoorDirection
	{
		North,
		East,
		South,
		West
	}

	public static class DoorDirectionExtensions
	{
		public static DoorDirection Opposite(this DoorDirection direction)
		{
			return direction switch
			{
				DoorDirection.North => DoorDirection.South,
				DoorDirection.East => DoorDirection.West,
				DoorDirection.South => DoorDirection.North,
				DoorDirection.West => DoorDirection.East,
				_ => throw new InvalidOperationException("Invalid DoorDirection enum!")
			};
		}

		public static Vector2 ToVector2(this DoorDirection direction)
		{
			return direction switch
			{
				DoorDirection.North => Vector2.up,
				DoorDirection.East => Vector2.right,
				DoorDirection.South => Vector2.down,
				DoorDirection.West => Vector2.left,
				_ => throw new InvalidOperationException("Invalid DoorDirection enum!")
			};
		}

		public static Vector2Int ToVector2Int(this DoorDirection direction)
		{
			return direction switch
			{
				DoorDirection.North => Vector2Int.up,
				DoorDirection.East => Vector2Int.right,
				DoorDirection.South => Vector2Int.down,
				DoorDirection.West => Vector2Int.left,
				_ => throw new InvalidOperationException("Invalid DoorDirection enum!")
			};
		}
	}
}
