﻿using System;
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
    public int score { get; set; }

    public Snake() {
        pieces = new Queue<SnakePiece>();
        score = 0;
        _inputQueue = new Queue<Direction>();
        direction = Direction.Right;

        pieces.Enqueue(new SnakePiece(BoardWidth / 2 - 2, BoardHeight / 2));
        head.outDirection = direction;
        pieces.Enqueue(new SnakePiece(BoardWidth / 2 - 1, BoardHeight / 2));
        head.outDirection = direction;
        pieces.Enqueue(new SnakePiece(BoardWidth / 2 - 0, BoardHeight / 2));
        head.outDirection = direction;

        apple = new SnakePiece(BoardWidth / 2 + 3, BoardHeight / 2);
        drawTrailing = true;

        progress = CellSize + LineWidth;
    }

    public void Move(int pixels) {
        progress += pixels;
        if (progress <= CellSize + LineWidth) return;
        progress -= CellSize + LineWidth;

        UpdateDirection();
        var nextPiece = AssignNewPiece();

        if (pieces.Contains(nextPiece)) {
            Die();
        }

        pieces.Enqueue(nextPiece);
        if (nextPiece == apple) {
            SpawnApple();
            score++;
            if (SnakeGame.speed < MaxSpeed)
                SnakeGame.speed += SpeedIncrement;
            drawTrailing = false;
        } else {
            lastPiece = pieces.Dequeue();
            drawTrailing = true;
        }
    }

    private void SpawnApple() {
        List<SnakePiece> possibilities = new List<SnakePiece>();
        for (int i = 0; i < BoardWidth; i++) {
            for (int j = 0; j < BoardWidth; j++) {
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
            || nextPiece.x > BoardWidth - 1
            || nextPiece.y < 0
            || nextPiece.y > BoardHeight - 1) Die();

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

public enum Direction {
    Up,
    Down,
    Left,
    Right
}
}