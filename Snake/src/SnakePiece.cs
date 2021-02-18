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

    private int xInPixels => LineWidth + x * (LineWidth + CellSize);
    private int yInPixels => LineWidth + y * (LineWidth + CellSize);

    public Rectangle rectangle {
        get {
            return new Rectangle(xInPixels,
                                 yInPixels,
                                 CellSize, CellSize);
        }
    }

    public Rectangle Offset(Direction offsetDirection, int pixels) {
        switch (offsetDirection) {
            case Direction.Up:
                return new Rectangle(LineWidth + x * (LineWidth + CellSize), y * (LineWidth + CellSize), CellSize,
                                     LineWidth + CellSize - pixels);
            case Direction.Down:
                return new Rectangle(LineWidth + x * (LineWidth + CellSize),
                                     LineWidth + y * (LineWidth + CellSize) + pixels, CellSize,
                                     LineWidth + CellSize - pixels);
            case Direction.Left:
                return new Rectangle(x * (LineWidth + CellSize), LineWidth + y * (LineWidth + CellSize),
                                     LineWidth + CellSize - pixels,
                                     CellSize);
            case Direction.Right:
                return new Rectangle(LineWidth + x * (LineWidth + CellSize) + pixels,
                                     LineWidth + y * (LineWidth + CellSize), LineWidth + CellSize - pixels,
                                     CellSize);
            default:
                throw new ArgumentOutOfRangeException(nameof(offsetDirection), offsetDirection, null);
        }
    }

    public Rectangle LineBetween(SnakePiece piece) {
        var vector = piece - this;
        switch (vector) {
            case (1, 0):
                return new Rectangle(xInPixels + CellSize, yInPixels, LineWidth, CellSize);
            case (-1, 0):
                return new Rectangle(xInPixels - LineWidth, yInPixels, LineWidth, CellSize);
            case (0, 1):
                return new Rectangle(xInPixels, yInPixels + CellSize, CellSize, LineWidth);
            case (0, -1):
                return new Rectangle(xInPixels, yInPixels - LineWidth, CellSize, LineWidth);
            default:
                return new Rectangle(0, 0, 0, 0);
        }
    }

    protected bool Equals(SnakePiece other) {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SnakePiece) obj);
    }

    public override int GetHashCode() {
        unchecked {
            return (x * 397) ^ y;
        }
    }

    public static bool operator ==(SnakePiece left, SnakePiece right) {
        return Equals(left, right);
    }

    public static bool operator !=(SnakePiece left, SnakePiece right) {
        return !Equals(left, right);
    }

    public static SnakePiece operator +(SnakePiece piece, (int x, int y) vector) {
        return new SnakePiece(piece.x + vector.x, piece.y + vector.y);
    }

    public static (int x, int y) operator -(SnakePiece piece1, SnakePiece piece2) {
        return (piece1.x - piece2.x, piece1.y - piece2.y);
    }
}
}
