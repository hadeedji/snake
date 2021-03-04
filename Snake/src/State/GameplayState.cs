using System;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Snake.Program;

namespace Snake {
public class GameplayState : State {
    public GameplayState(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch) {
        snake = new Snake();
        snake.OnSnakeDeath += () => stateChanged(new GameOver(game, spriteBatch) {score = snake.score});

        pixelTexture = new Texture2D(game.GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] {Color.White});

        pixelsToMove = 0;

        windowHeight = lineWidth + boardHeight * (lineWidth + cellSize);
        windowWidth = lineWidth + boardWidth * (lineWidth + cellSize);
    }

    private Snake snake { get; }
    private Texture2D pixelTexture { get; }
    
    private double pixelsToMove { get; set; }
    private bool isPressed { get; set; }

    private int lineWidth => configuration.dimensions.line;
    private int cellSize => configuration.dimensions.cell;
    private int boardHeight => configuration.dimensions.height;
    private int boardWidth => configuration.dimensions.width;

    private int windowHeight { get; }
    private int windowWidth { get; }

    public override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) game.Exit();
        if (!Keyboard.GetState().IsKeyDown(Keys.P)) isPressed = false;
        if (!isPressed && Keyboard.GetState().IsKeyDown(Keys.P)) stateChanged(new Pause(game, spriteBatch, this));

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
    }

    public override void Draw(GameTime gameTime) {
        game.GraphicsDevice.Clear(configuration.colors.background);
        var fps = Math.Round(1.0 / gameTime.ElapsedGameTime.TotalSeconds, 0).ToString(CultureInfo.CurrentCulture);
        game.Window.Title = $"Snake - {fps} FPS - Score: {snake.score}";

        var cells = snake.pieces.ToArray();

        spriteBatch.Begin();
        if (snake.drawTrailing)
            spriteBatch.Draw(pixelTexture, snake.lastPiece.Offset(snake.lastPiece.outDirection, snake.progress),
                             configuration.colors.snake);

        for (int i = 0; i < cells.Length - 1; i++) {
            spriteBatch.Draw(pixelTexture, cells[i].rectangle, configuration.colors.snake);
            if (i != cells.Length - 2) {
                var line = cells[i].LineBetween(cells[i + 1]);
                if (line.HasValue)
                    spriteBatch.Draw(pixelTexture, line.Value, configuration.colors.snake);
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
    }

    protected override void ChangedInto() {
        isPressed = true;
    }
}
}
