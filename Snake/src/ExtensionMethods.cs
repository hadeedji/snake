using System;
using Microsoft.Xna.Framework;

namespace Snake {
public static class ExtensionMethods {
    public static Color ToXna(this System.Drawing.Color color) => new Color(color.R, color.G, color.B);

    public static (int x, int y) Vector(this Direction direction) {
        switch (direction) {
            case Direction.Up:
                return (0, -1);
            case Direction.Down:
                return (0, 1);
            case Direction.Left:
                return (-1, 0);
            case Direction.Right:
                return (1, 0);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public static Direction Opposite(this Direction direction) {
        switch (direction) {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
}