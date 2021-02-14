using System;
using System.Collections.Generic;
using System.Linq;
using static Snake.Configuration;

namespace Snake {
public class Snake {
    private readonly Queue<Direction> _inputQueue;
    
    public Direction direction;
    public SnakePiece lastPiece;
    
    private SnakePiece head => pieces.ToArray().Last();

    public Queue<SnakePiece> pieces { get; private set; }
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

        progress = cellSize + lineWidth;
    }

    public void Move(int pixels) {
        progress += pixels;
        if (progress <= cellSize + lineWidth) return;
        progress -= cellSize + lineWidth;

        lastPiece = pieces.Dequeue();
        UpdateDirection();
        var nextPiece = AssignNewPiece();
        pieces.Enqueue(nextPiece);
    }

    private SnakePiece AssignNewPiece() {
        head.outDirection = direction;
        var nextPiece = head + direction.Vector();
        
        if (nextPiece.x < 0) nextPiece.x = boardWidth - 1;
        if (nextPiece.x > boardWidth - 1) nextPiece.x = 0;
        if (nextPiece.y < 0) nextPiece.y = boardHeight - 1;
        if (nextPiece.y > boardHeight - 1) nextPiece.y = 0;

        return nextPiece;
    }

    private void UpdateDirection() {
        Console.Clear();
        for (int i = 0; i < _inputQueue.Count; i++) {
            Console.WriteLine(_inputQueue.ToArray()[i]); 
        }
        if (_inputQueue.Count != 0) {
            direction = _inputQueue.Dequeue();
        }
    }

    public void AddDirectionToQueue(Direction directionToAdd) {
        var current = _inputQueue.Count == 0 ? direction : _inputQueue.ToArray().Last();
        if (directionToAdd != current && directionToAdd != current.Opposite()) {
            _inputQueue.Enqueue(directionToAdd);
        }
    }
}

public enum Direction {
    Up,
    Down,
    Left,
    Right
}
}
