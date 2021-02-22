using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Snake {
public class Snake {
    private Configuration configuration { get; }
    private int lineWidth => configuration.dimensions.line;
    private int cellSize => configuration.dimensions.cell;
    private int boardHeight => configuration.dimensions.height;
    private int boardWidth => configuration.dimensions.width;

    private readonly Queue<Direction> _inputQueue;
    private readonly Random _random = new Random();

    public Direction direction;
    public SnakePiece lastPiece;
    public SnakePiece apple;
    public bool drawTrailing;

    private SnakePiece head => pieces.ToArray().Last();

    public Queue<SnakePiece> pieces { get; private set; }
    public int progress { get; private set; }
    public int score { get; set; }

    public Snake() {
        this.configuration = MainGame.configuration;
        pieces = new Queue<SnakePiece>();
        score = 0;
        _inputQueue = new Queue<Direction>();
        direction = Direction.Right;

        pieces.Enqueue(new SnakePiece(boardWidth / 2 - 2, boardHeight / 2));
        head.outDirection = direction;
        lastPiece = head;
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
            Die();
        }

        pieces.Enqueue(nextPiece);
        if (nextPiece == apple) {
            SpawnApple();
            score++;
            if (MainGame.speed < configuration.speed.maximum)
                MainGame.speed += configuration.speed.increment;
            drawTrailing = false;
        } else {
            lastPiece = pieces.Dequeue();
            drawTrailing = true;
        }
    }

    private void SpawnApple() {
        List<SnakePiece> possibilities = new List<SnakePiece>();
        for (int i = 0; i < boardWidth; i++) {
            for (int j = 0; j < boardHeight; j++) {
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

        Direction? wallDirection = (nextPiece.x, nextPiece.y) switch {
            (int x, _) when x < 0 => Direction.Left,
            (_, int y) when y < 0 => Direction.Up,
            (int x, _) when x >= boardWidth => Direction.Right,
            (_, int y) when y >= boardHeight => Direction.Down,
            _ => null
        };

        if (wallDirection.HasValue) {
            if (configuration.wallsPresent) Die();

            nextPiece = wallDirection.Value switch {
                Direction.Up => new SnakePiece(nextPiece.x, boardHeight - 1),
                Direction.Down => new SnakePiece(nextPiece.x, 0),
                Direction.Left => new SnakePiece(boardWidth - 1, nextPiece.y),
                Direction.Right => new SnakePiece(0, nextPiece.y),
            };
        }


        return nextPiece;
    }

    private void Die() {
        Console.WriteLine($"Score: {score}");
        Environment.Exit(0);
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
}
