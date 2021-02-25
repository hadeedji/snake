using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake {
public class GameOver : State {
    public GameOver(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch) {
        font = game.Content.Load<SpriteFont>("Font");
    }

    public int score { get; init; }

    private SpriteFont font { get; }

    public override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Enter)) stateChanged(new GameplayState(game, spriteBatch));
    }

    public override void Draw(GameTime gameTime) {
        game.GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();
        spriteBatch.DrawString(font, $"Score: {score}", new Vector2(300, 300), Color.White);
        spriteBatch.End();
    }
}
}
