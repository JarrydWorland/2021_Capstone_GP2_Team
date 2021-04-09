using System;
using static Direction;

public enum Direction
{
	North = 0,
	East  = 1,
	South = 2,
	West  = 3,
}

// TODO: unused?
namespace DirectionExtensions
{
	public static class DirectionExtensions
	{
		public static Direction Opposite(this Direction direction)
		{
			return direction switch
			{
				North => South,
				East => West,
				South => North,
				West => East,
				_ => throw new InvalidOperationException("invalid direction enum"),
			};
		}
	}
}
