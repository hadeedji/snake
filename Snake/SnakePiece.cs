using System;
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

    private int xInPixels => lineWidth + x * (lineWidth + cellSize);
    private int yInPixels => lineWidth + y * (lineWidth + cellSize);

    public Rectangle rectangle {
        get {
            return new Rectangle(xInPixels,
                                 yInPixels,
                                 cellSize, cellSize);
        }
    }

    public Rectangle Offset(Direction offsetDirection, int pixels) {
        switch (offsetDirection) {
            case Direction.Up:
                return new Rectangle(lineWidth + x * (lineWidth + cellSize), y * (lineWidth + cellSize), cellSize,
                                     lineWidth + cellSize - pixels);
            case Direction.Down:
                return new Rectangle(lineWidth + x * (lineWidth + cellSize),
                                     lineWidth + y * (lineWidth + cellSize) + pixels, cellSize,
                                     lineWidth + cellSize - pixels);
            case Direction.Left:
                return new Rectangle(x * (lineWidth + cellSize), lineWidth + y * (lineWidth + cellSize),
                                     lineWidth + cellSize - pixels,
                                     cellSize);
            case Direction.Right:
                return new Rectangle(lineWidth + x * (lineWidth + cellSize) + pixels,
                                     lineWidth + y * (lineWidth + cellSize), lineWidth + cellSize - pixels,
                                     cellSize);
            default:
                throw new ArgumentOutOfRangeException(nameof(offsetDirection), offsetDirection, null);
        }
    }

    public Rectangle LineBetween(SnakePiece piece) {
        var vector = piece - this;
        switch (vector) {
            case (1, 0):
                return new Rectangle(xInPixels + cellSize, yInPixels, lineWidth, cellSize);
            case (-1, 0):
                return new Rectangle(xInPixels - lineWidth, yInPixels, lineWidth, cellSize);
            case (0, 1):
                return new Rectangle(xInPixels, yInPixels + cellSize, cellSize, lineWidth);
            case (0, -1):
                return new Rectangle(xInPixels, yInPixels - lineWidth, cellSize, lineWidth);
            default:
                throw new ArgumentException("Pieces are not adjacent.");
        }
    }


    public static SnakePiece operator +(SnakePiece piece, (int x, int y) vector) {
        return new SnakePiece(piece.x + vector.x, piece.y + vector.y);
    }

    public static (int x, int y) operator -(SnakePiece piece1, SnakePiece piece2) {
        return (piece1.x - piece2.x, piece1.y - piece2.y);
    }
}
}
