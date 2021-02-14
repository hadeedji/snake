using System.Collections.Generic;
using System.Linq;
using static Snake.Configuration;

namespace Snake {
public class Snake {
    public Queue<SnakePiece> pieces { get; private set; }
    public Direction direction;
    private readonly Queue<Direction> _inputQueue;

    private SnakePiece head => pieces.ToArray().Last();

    public SnakePiece nextPiece { get; private set; }
    public int progress { get; private set; }

    public Snake() {
        pieces = new Queue<SnakePiece>();
        _inputQueue = new Queue<Direction>();
        direction = Direction.Right;

        pieces.Enqueue(new SnakePiece(boardWidth / 2 - 2, boardHeight / 2));
        head.outDirection = direction;
        pieces.Enqueue(new SnakePiece(boardWidth / 2 - 1, boardHeight / 2));
        head.outDirection = direction;
        pieces.Enqueue(new SnakePiece(boardWidth / 2 - 0, boardHeight / 2));
        head.outDirection = direction;

        nextPiece = head + direction.Vector();
    }

    public void Move(int pixels) {
        progress += pixels;
        if (progress <= cellSize + lineWidth) return;
        progress -= cellSize + lineWidth;

        pieces.Dequeue();
        pieces.Enqueue(nextPiece);

        UpdateDirection();
        nextPiece = head + direction.Vector();
        head.outDirection = direction;
        if (nextPiece.x < 0) nextPiece.x = boardWidth - 1;
        if (nextPiece.x > boardWidth - 1) nextPiece.x = 0;
        if (nextPiece.y < 0) nextPiece.y = boardHeight - 1;
        if (nextPiece.y > boardHeight - 1) nextPiece.y = 0;
    }

    private void UpdateDirection() {
        if (_inputQueue.Count != 0) {
            direction = _inputQueue.Dequeue();
        }
    }

    public void AddDirectionToQueue(Direction directionToAdd) {
        if (_inputQueue.Count == 0) {
            if (directionToAdd != direction) _inputQueue.Enqueue(directionToAdd);
        } else if (directionToAdd != _inputQueue.ToArray().Last()) _inputQueue.Enqueue(directionToAdd);
    }
}

public enum Direction {
    Up,
    Down,
    Left,
    Right
}
}
