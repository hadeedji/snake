using System;
using static Snake.Configuration;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Snake {
public class SnakePiece {
    public int x { get;}
    public int y { get;}

    public Direction outDirection { get; set; }

    public SnakePiece(int x, int y) {
        this.x = x;
        this.y = y;
    }

    private int xInPixels => LineWidth + x * (LineWidth + CellSize);
    private int yInPixels => LineWidth + y * (LineWidth + CellSize);

    public Rectangle rectangle =>
        new Rectangle(xInPixels, yInPixels, CellSize, CellSize);

    public Rectangle Offset(Direction offsetDirection, int pixels) =>
        offsetDirection switch {
            Direction.Up => new Rectangle(xInPixels, yInPixels - LineWidth,
                                          CellSize, LineWidth + CellSize - pixels),

            Direction.Down => new Rectangle(xInPixels, yInPixels + pixels,
                                            CellSize, LineWidth + CellSize - pixels),

            Direction.Left => new Rectangle(xInPixels - LineWidth, yInPixels,
                                            LineWidth + CellSize - pixels, CellSize),

            Direction.Right => new Rectangle(xInPixels + pixels, yInPixels,
                                             LineWidth + CellSize - pixels, CellSize),

            _ => throw new ArgumentOutOfRangeException(nameof(offsetDirection), offsetDirection, null)
        };

    public Rectangle LineBetween(SnakePiece piece) {
        var vector = piece - this;
        return vector switch {
            (1, 0) => new Rectangle(xInPixels + CellSize, yInPixels, LineWidth, CellSize),
            (-1, 0) => new Rectangle(xInPixels - LineWidth, yInPixels, LineWidth, CellSize),
            (0, 1) => new Rectangle(xInPixels, yInPixels + CellSize, CellSize, LineWidth),
            (0, -1) => new Rectangle(xInPixels, yInPixels - LineWidth, CellSize, LineWidth),
            _ => new Rectangle(0, 0, 0, 0)
        };
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
