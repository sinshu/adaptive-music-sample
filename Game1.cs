using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AdaptiveMusicSample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private Texture2D texture;
        private SpriteBatch sprite;

        private Button[] buttons;

        private ButtonState previous;

        private AdaptiveMidiPlayer midiPlayer;
        private AdaptiveMidiFile midiFile;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            texture = Texture2D.FromFile(GraphicsDevice, "numbers.png");
            sprite = new SpriteBatch(GraphicsDevice);

            buttons = new Button[16];
            for (var i = 0; i < buttons.Length; i++)
            {
                var srcRow = i / 4;
                var srcCol = i % 4;
                var dstRow = i / 8;
                var dstCol = i % 8;
                var sourceRectangle = new Rectangle(64 * srcCol, 64 * srcRow, 64, 64);
                var position = new Vector2(16 + 96 * dstCol, 16 + 96 * dstRow);
                buttons[i] = new Button(texture, sourceRectangle, position, i);
            }

            midiPlayer = new AdaptiveMidiPlayer("TimGM6mb.sf2");
            midiFile = new AdaptiveMidiFile("test.mid");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            midiPlayer.Dispose();

            sprite.Dispose();
            texture.Dispose();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (midiPlayer.State == SoundState.Stopped)
            {
                midiPlayer.Play(midiFile, true);
            }

            var state = Mouse.GetState();
            var current = state.LeftButton;
            if (current == ButtonState.Pressed && previous == ButtonState.Released)
            {
                foreach (var button in buttons)
                {
                    if (button.Contains(state.X, state.Y))
                    {
                        var enableTrack = button.ToggleState();
                        if (enableTrack)
                        {
                            midiPlayer.EnableTrack(button.Number);
                        }
                        else
                        {
                            midiPlayer.DisableTrack(button.Number);
                        }
                    }
                }
            }
            previous = current;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sprite.Begin();
            foreach (var button in buttons)
            {
                button.Draw(sprite);
            }
            sprite.End();

            base.Draw(gameTime);
        }
    }
}
