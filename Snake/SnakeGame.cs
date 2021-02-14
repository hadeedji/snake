using System;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Snake.Configuration;

namespace Snake {
public class SnakeGame : Game {
    private int _windowHeight;
    private int _windowWidth;
    private Texture2D _texture;
    private Snake _snake;
    private int _speed;
    private double _pixelsToMove;

    public GraphicsDeviceManager graphics { get; }
    public SpriteBatch spriteBatch { get; private set; }

    public SnakeGame() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / fps);
        _snake = new Snake();
        _speed = 100;
    }

    protected override void Initialize() {
        _windowHeight = lineWidth + boardHeight * (lineWidth + cellSize);
        graphics.PreferredBackBufferHeight = _windowHeight;

        _windowWidth = lineWidth + boardWidth * (lineWidth + cellSize);
        graphics.PreferredBackBufferWidth = _windowWidth;

        graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] {Color.White});
    }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
        if (Keyboard.GetState().IsKeyDown(Keys.Up)) _snake.AddDirectionToQueue(Direction.Up);
        else if (Keyboard.GetState().IsKeyDown(Keys.Down)) _snake.AddDirectionToQueue(Direction.Down);
        else if (Keyboard.GetState().IsKeyDown(Keys.Left)) _snake.AddDirectionToQueue(Direction.Left);
        else if (Keyboard.GetState().IsKeyDown(Keys.Right)) _snake.AddDirectionToQueue(Direction.Right);

        _pixelsToMove += (gameTime.ElapsedGameTime.TotalSeconds * _speed);

        if (_pixelsToMove > 1) {
            var round = (int) Math.Round(_pixelsToMove, 0);
            _snake.Move(round);
            _pixelsToMove -= round;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(backgroundColor);
        var fps = Math.Round(1.0 / gameTime.ElapsedGameTime.TotalSeconds, 0).ToString(CultureInfo.CurrentCulture);
        this.Window.Title = $"Snake - {fps} FPS";

        var cells = _snake.pieces.ToArray();

        spriteBatch.Begin();
        spriteBatch.Draw(_texture, _snake.lastPiece.Offset(_snake.lastPiece.outDirection, _snake.progress), snakeColor);

        for (int i = 0; i < cells.Length - 1; i++) {
            spriteBatch.Draw(_texture, cells[i].rectangle, snakeColor);
            if (i != cells.Length - 2) {
                try {
                    spriteBatch.Draw(_texture, cells[i].LineBetween(cells[i + 1]), snakeColor);
                } catch (ArgumentException e) {
                    Console.WriteLine(e);
                }
            }
        }

        spriteBatch.Draw(_texture, cells.Last().Offset(_snake.direction.Opposite(), cellSize - _snake.progress),
                         snakeColor);
        
        spriteBatch.Draw(_texture, _snake.apple.rectangle, appleColor);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
}
