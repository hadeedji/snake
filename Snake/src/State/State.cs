using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake {
public abstract class State {
    public State(Game game, SpriteBatch spriteBatch) =>
        (this.game, this.spriteBatch) = (game, spriteBatch);

    protected Action<State> stateChanged { get; private set; }

    protected Game game { get; }
    protected SpriteBatch spriteBatch { get; }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);

    public void AddStateChanger(Action<State> stateChanger) => stateChanged += stateChanger;
}
}
