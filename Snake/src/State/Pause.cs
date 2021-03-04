using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake {
public class Pause : State{
    public Pause(Game game, SpriteBatch spriteBatch, State returnState) : base(game, spriteBatch) {
        this.returnState = returnState;
        isPressed = true;
    }
    
    private State returnState { get; }
    private bool isPressed { get; set; }
    
    public override void Update(GameTime gameTime) {
        if (!Keyboard.GetState().IsKeyDown(Keys.P)) isPressed = false;

        if (!isPressed && Keyboard.GetState().IsKeyDown(Keys.P)) stateChanged(returnState);
    }

    public override void Draw(GameTime gameTime) {
        returnState.Draw(gameTime);
    }
}
}
