//Rushi Patel
//DinoLand
//2024-03-07
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;

namespace DinoLand
{
    public class DinoLand : Game
    {
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        const float SKYRATIO = 2f / 3f;
        float screenWidth;
        float screenHeight;
        //the grass texture
        Texture2D grass;

        //new spriteclass
        SpriteClass dino;
        SpriteClass broccoli;
        SpriteClass dog;
        SpriteClass cheese;
        //bool and floats 
        bool spaceDown;
        private bool gameOver;
        bool gameStarted;
        private bool isColliding = false;
        float cheeseSpeedMulti;
        float dogSpeedMulti;
        float broccoliSpeedMultiplier;
        float gravitySpeed;
        float dinoSpeedX;
        float dinoJumpY;
        float score;
        //more textures 
        Texture2D startGameSplash;
        SpriteFont scoreFont;
        SpriteFont stateFont;
        AnimSprite Cat;

        Random random;

        Texture2D gameOverTexture;

       
        public DinoLand()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = true;

            screenHeight = (float)_graphics.PreferredBackBufferHeight;
            screenWidth = (float)_graphics.PreferredBackBufferWidth;

            this.IsMouseVisible = false;

            broccoliSpeedMultiplier = 0.5f;
            spaceDown = false;
            gameStarted = false;
            score = 0;
            random = new Random();
            dinoSpeedX = ScaleToHighDPI(1000f);
            dinoJumpY = ScaleToHighDPI(-1200f);
            gravitySpeed = ScaleToHighDPI(30f);

            gameOver = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            grass = Content.Load<Texture2D>("grass");

            //loads the sprites
            dino = new SpriteClass(GraphicsDevice, "Content/jerry.png", ScaleToHighDPI(.5f));
            broccoli = new SpriteClass(GraphicsDevice, "Content/tom.png", ScaleToHighDPI(0.2f));
            dog = new SpriteClass(GraphicsDevice, "Content/Dog.png", ScaleToHighDPI(.7f));
            cheese = new SpriteClass(GraphicsDevice, "Content/cheese.png", ScaleToHighDPI(.2f));
            Texture2D CatTexture = Content.Load<Texture2D>("Cat");

            Cat = new AnimSprite(CatTexture, 2, 5);
            startGameSplash = Content.Load<Texture2D>("start-splash");
            gameOverTexture = Content.Load<Texture2D>("game-over");
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            // TODO: Add your update logic here

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardHandler(); // Handle keyboard input
                               // Update animated SpriteClass objects based on their current rates of change
            if (gameOver)
            {
                dino.dX = 0;
                dino.dY=0;
                broccoli.dX = 0;
                broccoli.dY = 0;
                broccoli.dA = 0;
                dog.dX= 0;
                dog.dY= 0;
                dog.dA= 0;
                cheese.dX = 0;
                cheese.dY = 0;
                cheese.dA = 0;
            }
            //updates the sprites 
            dino.Update(elapsedTime);
            broccoli.Update(elapsedTime);
            dog.Update(elapsedTime);
            cheese.Update(elapsedTime);
            Cat.Update(gameTime);

            // Accelerate the dino downward each frame to simulate gravity.
            dino.dY += gravitySpeed;

            // Set game floor so the player does not fall through it
            if (dino.y > screenHeight * SKYRATIO)
            {
                dino.dY = 0;
                dino.y = screenHeight * SKYRATIO;
            }

            // Set game edges to prevent the player from moving offscreen
            if (dino.x > screenWidth - dino.texture.Width / 5)
            {
                dino.x = screenWidth - dino.texture.Width / 5;
                dino.dX = 0;
            }
            if (dino.x < 0 + dino.texture.Width / 5)
            {
                dino.x = 0 + dino.texture.Width / 5;
                dino.dX = 0;
            }

            // If the broccoli goes offscreen, spawn a new one and increase the score
            if (broccoli.y > screenHeight + 100 || broccoli.y < -100 || broccoli.x > screenWidth + 100 || broccoli.x < -100)
            {   
                SpawnBroccoli();//spawns tom
                score++;
            }

            if (dog.y > screenHeight + 100 || dog.y < -100 || dog.x > screenWidth + 100 || dog.x < -100)
            {
                SpawnDog();//spawns a dog
                score-=1;
            }
            if (cheese.y > screenHeight + 100 || cheese.y < -100 || cheese.x > screenWidth + 100 || cheese.x < -100)
            {
                SpawnCheese();//spawns a cheese
            }
            if (dino.RectangleCollision(broccoli)|| dino.RectangleCollision(dog))
            {
                gameOver = true;//games over when dog and tom collide into jerry
            }
            if(!isColliding)
            {
                if (dino.RectangleCollision(cheese))
                {
                    score += 1;//when cheese collides score increases
                    isColliding = true;
                }
            }
            else
            {
                if(!dino.RectangleCollision(cheese))//the score wont go up once jerry and cheese are not colliding
                {
                    isColliding = false;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(grass, new Rectangle(0, (int)(screenHeight * SKYRATIO),
              (int)screenWidth, (int)screenHeight), Color.White);
            broccoli.Draw(_spriteBatch);
            dino.Draw(_spriteBatch);
            dog.Draw(_spriteBatch);
            cheese.Draw(_spriteBatch);
            _spriteBatch.DrawString(scoreFont, score.ToString(),
            new Vector2(screenWidth - 100, 50), Color.Black);
            if (gameOver)
            {
                // Draw game over texture
                _spriteBatch.Draw(gameOverTexture, new Vector2(screenWidth / 2 - gameOverTexture.Width / 2, screenHeight / 4 - gameOverTexture.Width / 2), Color.White);

                String pressEnter = "Press Enter to restart!";

                // Measure the size of text in the given font
                Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);

                // Draw the text horizontally centered
                _spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, screenHeight - 200), Color.White);

                Cat.Draw(_spriteBatch, new Vector2(30, 50), Color.White);

            }
            if (!gameStarted)
            {
                // Fill the screen with black before the game starts
                _spriteBatch.Draw(startGameSplash, new Rectangle(0, 0,
                (int)screenWidth, (int)screenHeight), Color.White);

                String title = "VEGGIE JUMP";
                String pressSpace = "Press Space to start";

                // Measure the size of text in the given font
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                _spriteBatch.DrawString(stateFont, title,
                new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3),
                Color.ForestGreen);
                _spriteBatch.DrawString(stateFont, pressSpace,
                new Vector2(screenWidth / 2 - pressSpaceSize.X / 2,
                screenHeight / 2), Color.White);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        public float ScaleToHighDPI(float f)
        {
            //Lines from original tutorial code need reference i can't find
            //Used static value of 0.5 instead
            //DisplayInformation d = DisplayInformation.GetForCurrentView();
            //f *= (float)d.RawPixelsPerViewPixel;
            f *= (float)0.5;
            return f;
        }
        public void SpawnBroccoli()
        {
            int direction = random.Next(1, 5);
            switch (direction)//determines the sprite location
            {
                case 1:
                    broccoli.x = -100;
                    broccoli.y = random.Next(0, (int)screenHeight);
                    break;
                case 2:
                    broccoli.y = -100;
                    broccoli.x = random.Next(0, (int)screenWidth);
                    break;
                case 3:
                    broccoli.x = screenWidth + 100;
                    broccoli.y = random.Next(0, (int)screenHeight);
                    break;
                case 4:
                    broccoli.y = screenHeight + 100;
                    broccoli.x = random.Next(0, (int)screenWidth);
                    break;
            }

            if (score % 2 == 0)
            {
                broccoliSpeedMultiplier += 0.7f; 
            }

            broccoli.dX = (dino.x - broccoli.x) * broccoliSpeedMultiplier;
            broccoli.dY = (dino.y - broccoli.y) * broccoliSpeedMultiplier;
            broccoli.dA = 7f;

        }
        public void SpawnDog()
        {
            int direction2 = random.Next(1, 5);
            switch (direction2)//determines the sprite location
            {
                case 1:
                    dog.x = -100;
                    dog.y = random.Next(0, (int)screenHeight);
                    break;
                case 2:
                    dog.y = -100;
                    dog.x = random.Next(0, (int)screenWidth);
                    break;
                case 3:
                    dog.x = screenWidth + 100;
                    dog.y = random.Next(0, (int)screenHeight);
                    break;
                case 4:
                    dog.y = screenHeight + 100;
                    dog.x = random.Next(0, (int)screenWidth);
                    break;
            }

            if (score % 2 == 0)
            {
                 dogSpeedMulti+= 0.7f;
            }

            dog.dX = (dino.x - dog.x) * dogSpeedMulti;
            dog.dY = (dino.y - dog.y) * dogSpeedMulti;
            dog.dA = 7f;
        }
        public void SpawnCheese()
        {
            int direction3 = random.Next(1, 5);
            switch (direction3)//determines the sprite location
            {
                case 1:
                    cheese.x = -100;
                    cheese.y = random.Next(0, (int)screenHeight);
                    break;
                case 2:
                    cheese.y = -100;
                    cheese.x = random.Next(0, (int)screenWidth);
                    break;
                case 3:
                    cheese.x = screenWidth + 100;
                    cheese.y = random.Next(0, (int)screenHeight);
                    break;
                case 4:
                    cheese.y = screenHeight + 100;
                    cheese.x = random.Next(0, (int)screenWidth);
                    break;
            }

            if (score % 2 == 0)
            {
                cheeseSpeedMulti += 0.7f;
            }

            cheese.dX = (dino.x - cheese.x) * cheeseSpeedMulti;
            cheese.dY = (dino.y - cheese.y) * cheeseSpeedMulti;
            cheese.dA = 7f;
        }
        public void StartGame()
        {
            //spawns all the sprites with their speed
            dino.x = screenWidth / 2;
            dino.y = screenHeight * SKYRATIO;
            broccoliSpeedMultiplier = 0.5f;
            SpawnBroccoli();
            dogSpeedMulti = .5f;
            SpawnDog();
            cheeseSpeedMulti = .5f;
            SpawnCheese();
            score = 0;
            
        }
        void KeyboardHandler()
        {
            KeyboardState state = Keyboard.GetState();

            // Quit the game if Escape is pressed.
            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Start the game if Space is pressed.
            if (!gameStarted)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    StartGame();
                    gameStarted = true;
                    spaceDown = true;
                    gameOver = false;
                }
                return;
            }
            // Jump if Space is pressed
            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
            {
                // Jump if the Space is pressed but not held and the dino is on the floor
                if (!spaceDown && dino.y >= screenHeight * SKYRATIO - 1) dino.dY = dinoJumpY;

                spaceDown = true;
            }
            else spaceDown = false;

            // Handle left and right
            if (state.IsKeyDown(Keys.Left)) dino.dX = dinoSpeedX * -1;

            else if (state.IsKeyDown(Keys.Right)) dino.dX = dinoSpeedX;
            else dino.dX = 0;
            
            if(gameOver&&state.IsKeyDown(Keys.Enter))
            {
                StartGame();
                gameOver = false;
            }
        }
        //collision detection
       
    }
}
