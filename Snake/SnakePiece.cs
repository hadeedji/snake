using System;
using System.Drawing;
using static Snake.Configuration;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Snake {
public class SnakePiece {
    public int x;
    public int y;
    public Direction outDirection;

    public SnakePiece(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Rectangle rectangle {
        get {
            return new Rectangle(lineWidth + x * (lineWidth + cellSize),
                                 lineWidth + y * (lineWidth + cellSize),
                                 cellSize, cellSize);
        }
    }

    public Rectangle Offset(Direction offsetDirection, int pixels) {
        switch (offsetDirection) {
            case Direction.Up:
                return new Rectangle(lineWidth + x * (lineWidth+cellSize), lineWidth + y * (lineWidth + cellSize), cellSize,
                                     cellSize - pixels);
            case Direction.Down:
                return new Rectangle(lineWidth + x * (lineWidth+cellSize), lineWidth + y * (lineWidth+cellSize) + pixels, cellSize,
                                     cellSize - pixels);
            case Direction.Left:
                return new Rectangle(lineWidth + x * (lineWidth+cellSize), lineWidth + y * (lineWidth+cellSize), cellSize - pixels,
                                     cellSize);
            case Direction.Right:
                return new Rectangle(lineWidth + x * (lineWidth+cellSize) + pixels, lineWidth + y * (lineWidth+cellSize), cellSize - pixels,
                                     cellSize);
            default:
                throw new ArgumentOutOfRangeException(nameof(offsetDirection), offsetDirection, null);
        }
    }

    public static SnakePiece operator +(SnakePiece piece, (int x, int y) vector) {
        return new SnakePiece(piece.x + vector.x, piece.y + vector.y);
    }
}
}
