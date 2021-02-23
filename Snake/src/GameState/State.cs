using Microsoft.Xna.Framework;

namespace Snake {
public abstract class State {
    private ChangeState changeState { get; set; }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}

public delegate void ChangeState(State state);
}
