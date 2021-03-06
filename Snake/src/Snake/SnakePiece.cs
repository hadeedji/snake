﻿using System;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using static Snake.Program;

namespace Snake {
public class SnakePiece {
    public SnakePiece(int x, int y) {
        this.x = x;
        this.y = y;


        xInPixels = lineWidth + x * (lineWidth + cellSize);
        yInPixels = lineWidth + y * (lineWidth + cellSize);

        rectangle = new Rectangle(xInPixels, yInPixels, cellSize, cellSize);
    }

    public int x { get; }
    public int y { get; }
    public Rectangle rectangle { get; }

    public Direction outDirection { get; set; }

    private int xInPixels { get; }
    private int yInPixels { get; }

    private int lineWidth => configuration.dimensions.line;
    private int cellSize => configuration.dimensions.cell;

    public Rectangle Offset(Direction offsetDirection, int pixels) =>
        offsetDirection switch {
            Direction.Up => new Rectangle(xInPixels, yInPixels - lineWidth,
                                          cellSize, lineWidth + cellSize - pixels),

            Direction.Down => new Rectangle(xInPixels, yInPixels + pixels,
                                            cellSize, lineWidth + cellSize - pixels),

            Direction.Left => new Rectangle(xInPixels - lineWidth, yInPixels,
                                            lineWidth + cellSize - pixels, cellSize),

            Direction.Right => new Rectangle(xInPixels + pixels, yInPixels,
                                             lineWidth + cellSize - pixels, cellSize),

            _ => throw new ArgumentOutOfRangeException()
        };

    public Rectangle? LineBetween(SnakePiece piece) {
        var vector = piece - this;
        return vector switch {
            (1, 0) => new Rectangle(xInPixels + cellSize, yInPixels, lineWidth, cellSize),
            (-1, 0) => new Rectangle(xInPixels - lineWidth, yInPixels, lineWidth, cellSize),
            (0, 1) => new Rectangle(xInPixels, yInPixels + cellSize, cellSize, lineWidth),
            (0, -1) => new Rectangle(xInPixels, yInPixels - lineWidth, cellSize, lineWidth),
            _ => null
        };
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

    public override int GetHashCode() {
        unchecked {
            return (x * 397) ^ y;
        }
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SnakePiece) obj);
    }

    protected bool Equals(SnakePiece other) {
        return x == other.x && y == other.y;
    }
}
}
