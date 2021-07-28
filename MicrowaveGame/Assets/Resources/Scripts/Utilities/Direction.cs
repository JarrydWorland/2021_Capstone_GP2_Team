using System;
using UnityEngine;

namespace Scripts.Utilities
{
	/// <summary>
	/// Represents a cardinal direction.
	/// </summary>
	public enum Direction
	{
		North,
		East,
		South,
		West
	}

	public static class DirectionExtensions
	{
		/// <summary>
		/// Get the opposite direction.
		/// </summary>
		/// <returns>Returns the Direction value representing the opposite of the current direction.</returns>
		public static Direction Opposite(this Direction direction)
		{
			return direction switch
			{
				Direction.North => Direction.South,
				Direction.East => Direction.West,
				Direction.South => Direction.North,
				Direction.West => Direction.East,
				_ => throw new InvalidOperationException("Invalid Direction enum!")
			};
		}

		/// <summary>
		/// Gets a Vector2 from the given direction.
		/// </summary>
		/// <returns>A normalized Vector2 in the same direction.</returns>
		public static Vector2 ToVector2(this Direction direction)
		{
			return direction switch
			{
				Direction.North => Vector2.up,
				Direction.East => Vector2.right,
				Direction.South => Vector2.down,
				Direction.West => Vector2.left,
				_ => throw new InvalidOperationException("Invalid Direction enum!")
			};
		}

		/// <summary>
		/// Gets a Vector3 from the given direction.
		/// </summary>
		/// <returns>A normalized Vector3 in the same direction.</returns>
		public static Vector3 ToVector3(this Direction direction)
		{
			return direction switch
			{
				Direction.North => Vector3.up,
				Direction.East => Vector3.right,
				Direction.South => Vector3.down,
				Direction.West => Vector3.left,
				_ => throw new InvalidOperationException("Invalid Direction enum!")
			};
		}

		/// <summary>
		/// Gets a Vector2Int from the given direction.
		/// </summary>
		/// <returns>A normalized Vector2Int in the same direction.</returns>
		public static Vector2Int ToVector2Int(this Direction direction)
		{
			return direction switch
			{
				Direction.North => Vector2Int.up,
				Direction.East => Vector2Int.right,
				Direction.South => Vector2Int.down,
				Direction.West => Vector2Int.left,
				_ => throw new InvalidOperationException("Invalid Direction enum!")
			};
		}
	}
}
