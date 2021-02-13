using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Snake.Configuration;

namespace Snake {
public class SnakeGame : Game {
    private int _windowHeight;
    private int _windowWidth;
    private Texture2D _texture;

    public GraphicsDeviceManager graphics { get; }
    public SpriteBatch spriteBatch { get; private set; }

    public SnakeGame() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(backgroundColor);

        base.Draw(gameTime);
    }
}
}
