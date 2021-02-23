using System;
using System.Collections.Generic;
using System.Linq;
using static Snake.Program;

namespace Snake {
public class Snake {
    public Snake() {
        inputQueue = new Queue<Direction>();
        random = new Random();

        pieces = new Queue<SnakePiece>();
        direction = Direction.Right;

        for (int i = 2; i >= 0; i--) {
            var piece = new SnakePiece(boardWidth / 2 - i, boardHeight / 2);
            piece.outDirection = direction;
            pieces.Enqueue(piece);
        }

        lastPiece = pieces.Peek();
        apple = new SnakePiece(boardWidth / 2 + 3, boardHeight / 2);

        score = 0;
        currentSpeed = configuration.speed.starting;
        progress = cellSize + lineWidth;
        drawTrailing = true;
    }

    public event Action OnSnakeDeath;

    public Queue<Direction> inputQueue { get; }
    public Random random { get; }

    public Queue<SnakePiece> pieces { get; private set; }
    public Direction direction { get; private set; }
    public SnakePiece lastPiece { get; private set; }
    public SnakePiece apple { get; private set; }

    public int score { get; private set; }
    public int currentSpeed { get; private set; }
    public int progress { get; private set; }
    public bool drawTrailing { get; private set; }

    private int lineWidth => configuration.dimensions.line;
    private int cellSize => configuration.dimensions.cell;
    private int boardHeight => configuration.dimensions.height;
    private int boardWidth => configuration.dimensions.width;

    private SnakePiece head => pieces.ToArray().Last();


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
            if (currentSpeed < configuration.speed.maximum)
                currentSpeed += configuration.speed.increment;
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

        apple = possibilities.ToArray()[random.Next(possibilities.Count)];
    }

    private SnakePiece AssignNewPiece() {
        head.outDirection = direction;
        var nextPiece = head + direction.Vector();

        Direction? wallDirection = (nextPiece.x, nextPiece.y) switch {
            (var x, _) when x < 0 => Direction.Left,
            (_, var y) when y < 0 => Direction.Up,
            (var x, _) when x >= boardWidth => Direction.Right,
            (_, var y) when y >= boardHeight => Direction.Down,
            _ => null
        };

        if (wallDirection.HasValue) {
            if (configuration.wallsPresent) Die();

            nextPiece = wallDirection.Value switch {
                Direction.Up => new SnakePiece(nextPiece.x, boardHeight - 1),
                Direction.Down => new SnakePiece(nextPiece.x, 0),
                Direction.Left => new SnakePiece(boardWidth - 1, nextPiece.y),
                Direction.Right => new SnakePiece(0, nextPiece.y),
                _ => throw new ArgumentOutOfRangeException()
            };
        }


        return nextPiece;
    }

    private void Die() {
        OnSnakeDeath?.Invoke();
    }

    private void UpdateDirection() {
        if (inputQueue.Count != 0) {
            direction = inputQueue.Dequeue();
        }
    }

    public void AddDirectionToQueue(Direction directionToAdd) {
        var current = inputQueue.Count == 0 ? direction : inputQueue.ToArray().Last();
        if (directionToAdd != current && directionToAdd != current.Opposite()) {
            inputQueue.Enqueue(directionToAdd);
        }
    }
}
}
