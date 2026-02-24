using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoLand
{
    internal class AnimSprite
    {
        Texture2D texture { get; set; }

        int rows { get; set; }

        int cols { get; set; }

        int currentFrame;
        int totalFrame;

        int timeSinceLastFrame = 0;
        int millisecPerFrame = 100;
        public AnimSprite(Texture2D texture, int rows, int cols)
        {
            this.texture = texture;
            this.rows = rows;
            this.cols = cols;

            currentFrame = 0;
            totalFrame = rows * cols;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            //have we waited long enough?

            if (timeSinceLastFrame > millisecPerFrame)
            {
                timeSinceLastFrame -= millisecPerFrame;

                currentFrame++;
                timeSinceLastFrame = 0;

                if (currentFrame == totalFrame) currentFrame = 0;

                if (currentFrame == 8)
                {
                    currentFrame = 0;
                }
            }


        }
        // make a new draw method that does all the things that the current draw method does and add a scale parameter
      

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color c)
        {
            int width = texture.Width / cols;
            int height = texture.Height / rows;

            int row = currentFrame / cols;
            int col = currentFrame % cols;
            Rectangle sourceRect = new Rectangle(width * col, height * row, width, height);

            Rectangle destRect = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(texture, destRect, sourceRect, c);
        }
    }
}
