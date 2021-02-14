using System;
using System.Collections.Generic;
using System.Linq;
using static Snake.Configuration;

namespace Snake {
public class Snake {
    private readonly Queue<Direction> _inputQueue;
    private readonly Random _random = new Random();

    public Direction direction;
    public SnakePiece lastPiece;
    public SnakePiece apple;
    public bool drawTrailing;

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

        apple = new SnakePiece(boardWidth / 2 + 3, boardHeight / 2);
        drawTrailing = true;

        progress = cellSize + lineWidth;
    }

    public void Move(int pixels) {
        progress += pixels;
        if (progress <= cellSize + lineWidth) return;
        progress -= cellSize + lineWidth;

        UpdateDirection();
        var nextPiece = AssignNewPiece();

        if (pieces.Contains(nextPiece)) {
            Environment.Exit(0);
        }

        pieces.Enqueue(nextPiece);
        if (nextPiece == apple) {
            SpawnApple();
            if (SnakeGame.speed < maxSpeed)
                SnakeGame.speed += speedIncrement;
            Console.WriteLine(SnakeGame.speed);
            drawTrailing = false;
        } else {
            lastPiece = pieces.Dequeue();
            drawTrailing = true;
        }
    }

    private void SpawnApple() {
        List<SnakePiece> possibilities = new List<SnakePiece>();
        for (int i = 0; i < boardWidth; i++) {
            for (int j = 0; j < boardWidth; j++) {
                var possibility = new SnakePiece(i, j);
                var acceptable = true;
                foreach (SnakePiece snakePiece in pieces)
                    if (possibility == snakePiece)
                        acceptable = false;
                if (acceptable) possibilities.Add(possibility);
            }
        }

        apple = possibilities.ToArray()[_random.Next(possibilities.Count)];
    }

    private SnakePiece AssignNewPiece() {
        head.outDirection = direction;
        var nextPiece = head + direction.Vector();

        if (nextPiece.x < 0
         || nextPiece.x > boardWidth - 1
         || nextPiece.y < 0
         || nextPiece.y > boardHeight - 1) Environment.Exit(0);

        return nextPiece;
    }

    private void UpdateDirection() {
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
