using System;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Snake.Program;

namespace Snake {
public class MainGame : Game {
    public MainGame() {
        graphics = new GraphicsDeviceManager(this);
        windowHeight = lineWidth + boardHeight * (lineWidth + cellSize);
        windowWidth = lineWidth + boardWidth * (lineWidth + cellSize);
        snake = new Snake();
        
        IsMouseVisible = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / configuration.framesPerSecond);
    }
    
    public GraphicsDeviceManager graphics { get; }
    public SpriteBatch spriteBatch { get; private set; }

    private int lineWidth => configuration.dimensions.line;
    private int cellSize => configuration.dimensions.cell;
    private int boardHeight => configuration.dimensions.height;
    private int boardWidth => configuration.dimensions.width;
    
    private int windowHeight { get; }
    private int windowWidth { get; }
    private Snake snake { get; }
    
    private Texture2D pixelTexture { get; set; }
    private double pixelsToMove { get; set; }

    protected override void Initialize() {
        graphics.PreferredBackBufferHeight = windowHeight;
        graphics.PreferredBackBufferWidth = windowWidth;
        graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] {Color.White});
    }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        // TODO: Fix Input handling.
        if (Keyboard.GetState().IsKeyDown(Keys.Up)) snake.AddDirectionToQueue(Direction.Up);
        else if (Keyboard.GetState().IsKeyDown(Keys.Down)) snake.AddDirectionToQueue(Direction.Down);
        else if (Keyboard.GetState().IsKeyDown(Keys.Left)) snake.AddDirectionToQueue(Direction.Left);
        else if (Keyboard.GetState().IsKeyDown(Keys.Right)) snake.AddDirectionToQueue(Direction.Right);

        pixelsToMove += (gameTime.ElapsedGameTime.TotalSeconds * snake.currentSpeed);

        if (pixelsToMove > 1) {
            var round = (int) Math.Round(pixelsToMove, 0);
            snake.Move(round);
            pixelsToMove -= round;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(configuration.colors.background);
        var fps = Math.Round(1.0 / gameTime.ElapsedGameTime.TotalSeconds, 0).ToString(CultureInfo.CurrentCulture);
        this.Window.Title = $"Snake - {fps} FPS - Score: {snake.score}";

        var cells = snake.pieces.ToArray();

        spriteBatch.Begin();
        if (snake.drawTrailing)
            spriteBatch.Draw(pixelTexture, snake.lastPiece.Offset(snake.lastPiece.outDirection, snake.progress),
                             configuration.colors.snake);

        for (int i = 0; i < cells.Length - 1; i++) {
            spriteBatch.Draw(pixelTexture, cells[i].rectangle, configuration.colors.snake);
            if (i != cells.Length - 2) {
                spriteBatch.Draw(pixelTexture, cells[i].LineBetween(cells[i + 1]), configuration.colors.snake);
            }
        }

        spriteBatch.Draw(pixelTexture, cells.Last().Offset(snake.direction.Opposite(), cellSize - snake.progress),
                         configuration.colors.snake);

        spriteBatch.Draw(pixelTexture, snake.apple.rectangle, configuration.colors.apple);

        if (configuration.wallsPresent) {
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, windowWidth, lineWidth), configuration.colors.wall);
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, lineWidth, windowHeight), configuration.colors.wall);
            spriteBatch.Draw(pixelTexture, new Rectangle(windowWidth - lineWidth, 0, lineWidth, windowHeight),
                             configuration.colors.wall);
            spriteBatch.Draw(pixelTexture, new Rectangle(0, windowHeight - lineWidth, windowWidth, lineWidth),
                             configuration.colors.wall);
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
}
