using System;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake {
public class MainGame : Game {
    public static Configuration configuration { get; set; }
    private int lineWidth => configuration.dimensions.line;
    private int cellSize => configuration.dimensions.cell;
    private int boardHeight => configuration.dimensions.height;
    private int boardWidth => configuration.dimensions.width;

    private int _windowHeight;
    private int _windowWidth;
    private Texture2D _texture;
    Snake _snake;
    public static int speed;
    private double _pixelsToMove;

    public GraphicsDeviceManager graphics { get; }
    public SpriteBatch spriteBatch { get; private set; }

    public MainGame(Configuration inputConfiguration) {
        configuration = inputConfiguration;
        graphics = new GraphicsDeviceManager(this);
        IsMouseVisible = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / configuration.framesPerSecond);

        _snake = new Snake();
        speed = configuration.speed.starting;
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

        // TODO: Fix Input handling.
        if (Keyboard.GetState().IsKeyDown(Keys.Up)) _snake.AddDirectionToQueue(Direction.Up);
        else if (Keyboard.GetState().IsKeyDown(Keys.Down)) _snake.AddDirectionToQueue(Direction.Down);
        else if (Keyboard.GetState().IsKeyDown(Keys.Left)) _snake.AddDirectionToQueue(Direction.Left);
        else if (Keyboard.GetState().IsKeyDown(Keys.Right)) _snake.AddDirectionToQueue(Direction.Right);

        _pixelsToMove += (gameTime.ElapsedGameTime.TotalSeconds * speed);

        if (_pixelsToMove > 1) {
            var round = (int) Math.Round(_pixelsToMove, 0);
            _snake.Move(round);
            _pixelsToMove -= round;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(configuration.colors.background);
        var fps = Math.Round(1.0 / gameTime.ElapsedGameTime.TotalSeconds, 0).ToString(CultureInfo.CurrentCulture);
        this.Window.Title = $"Snake - {fps} FPS - Score: {_snake.score}";

        var cells = _snake.pieces.ToArray();

        spriteBatch.Begin();
        if (_snake.drawTrailing)
            spriteBatch.Draw(_texture, _snake.lastPiece.Offset(_snake.lastPiece.outDirection, _snake.progress),
                             configuration.colors.snake);

        for (int i = 0; i < cells.Length - 1; i++) {
            spriteBatch.Draw(_texture, cells[i].rectangle, configuration.colors.snake);
            if (i != cells.Length - 2) {
                spriteBatch.Draw(_texture, cells[i].LineBetween(cells[i + 1]), configuration.colors.snake);
            }
        }

        spriteBatch.Draw(_texture, cells.Last().Offset(_snake.direction.Opposite(), cellSize - _snake.progress),
                         configuration.colors.snake);

        spriteBatch.Draw(_texture, _snake.apple.rectangle, configuration.colors.apple);

        if (configuration.wallsPresent) {
            spriteBatch.Draw(_texture, new Rectangle(0, 0, _windowWidth, lineWidth), configuration.colors.wall);
            spriteBatch.Draw(_texture, new Rectangle(0, 0, lineWidth, _windowHeight), configuration.colors.wall);
            spriteBatch.Draw(_texture, new Rectangle(_windowWidth - lineWidth, 0, lineWidth, _windowHeight),
                             configuration.colors.wall);
            spriteBatch.Draw(_texture, new Rectangle(0, _windowHeight - lineWidth, _windowWidth, lineWidth),
                             configuration.colors.wall);
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
}
