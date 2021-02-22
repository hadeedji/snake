using System;

namespace Snake {
public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions {
    public static (int x, int y) Vector(this Direction direction) =>
        direction switch {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    public static Direction Opposite(this Direction direction) =>
        direction switch {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
}
}
