using System;

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
}