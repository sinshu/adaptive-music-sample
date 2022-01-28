using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdaptiveMusicSample
{
    public class Button
    {
        private Texture2D texture;
        private Rectangle sourceRectangle;
        private Vector2 position;
        private int number;

        private bool state;

        public Button(Texture2D texture, Rectangle sourceRectangle, Vector2 position, int number)
        {
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;
            this.position = position;
            this.number = number;

            state = true;
        }

        public bool Contains(int x, int y)
        {
            var h = position.X <= x && x < position.X + sourceRectangle.Width;
            var v = position.Y <= y && y < position.Y + sourceRectangle.Height;
            return h && v;
        }

        public void Draw(SpriteBatch sprite)
        {
            if (state)
            {
                sprite.Draw(texture, position, sourceRectangle, Color.White);
            }
            else
            {
                sprite.Draw(texture, position, sourceRectangle, 0.25F * Color.White);
            }
        }

        public bool ToggleState()
        {
            state = !state;
            return state;
        }

        public int Number => number;
    }
}
