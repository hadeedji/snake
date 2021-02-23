﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Snake.Program;

namespace Snake {
public class MainGame : Game {
    public MainGame() {
        graphics = new GraphicsDeviceManager(this);

        IsMouseVisible = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / configuration.framesPerSecond);
    }

    public GraphicsDeviceManager graphics { get; }
    public SpriteBatch spriteBatch { get; private set; }

    private State currentGameState { get; set; }

    protected override void Initialize() {
        int lineWidth = configuration.dimensions.line;
        int cellSize = configuration.dimensions.cell;
        int boardHeight = configuration.dimensions.height;
        int boardWidth = configuration.dimensions.width;

        int windowHeight = lineWidth + boardHeight * (lineWidth + cellSize);
        graphics.PreferredBackBufferHeight = windowHeight;

        int windowWidth = lineWidth + boardWidth * (lineWidth + cellSize);
        graphics.PreferredBackBufferWidth = windowWidth;

        graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        currentGameState = new GameplayState(this, spriteBatch);
    }

    protected override void Update(GameTime gameTime) {
        currentGameState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        currentGameState.Draw(gameTime);

        base.Draw(gameTime);
    }
}
}
