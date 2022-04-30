using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace YangA_MP2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public const int EASY = 0;
        public const int MEDIUM = 1;
        public const int HARD = 2;

        public const int INSTRUCTIONS = 0;
        public const int GAMEPLAY = 1;
        public const int ENDGAME = 2;

        public const int EASY_ROWS = 8;
        public const int EASY_COLUMN = 10;
        public const int MEDIUM_ROWS = 14;
        public const int MEDIUM_COLUMN = 18;
        public const int HARD_ROWS = 20;
        public const int HARD_COLUMN = 24;

        public const int EASY_MINES = 10;
        public const int MEDIUM_MINES = 40;
        public const int HARD_MINES = 99;

        public const int EASY_TILE_SIZE = 45;
        public const int MEDIUM_TILE_SIZE = 30;
        public const int HARD_TILE_SIZE = 25;

        public const int HIDDEN = 0;
        public const int REVEALED = 1;
        public const int BOMB = 2;
        public const int FLAG = 3;

        public static int gameState = 1;
        public static int gameDiff = EASY;
        public static int gameboardRow = EASY_ROWS;
        public static int gameboardColumn = EASY_COLUMN;
        public static int gameMines = EASY_MINES;
        public static int gameTileSize = EASY_TILE_SIZE;

        public static List<int> Bombs = new List<int>();

        public static Tile[,] Tiles;

        private bool isReset = true;

        Texture2D easyBoard;
        Rectangle easyBoardRect;

        Texture2D clock;
        Rectangle clockRect;

        Texture2D flag;
        Rectangle flagRect;

        Texture2D hud;
        Rectangle hudRect;

        Texture2D soundOn;
        Rectangle onRect;

        Texture2D soundOff;
        Rectangle offRect;

        Texture2D clearLight;
        Rectangle clearLightRect;

        Texture2D clearDark;
        Rectangle clearDarkRect;

        Texture2D one;
        Rectangle oneRect;

        Texture2D two;
        Rectangle twoRect;

        Texture2D three;
        Rectangle threeRect;

        Texture2D four;
        Rectangle fourRect;

        Texture2D five;
        Rectangle fiveRect;

        Texture2D six;
        Rectangle sixRect;

        Texture2D seven;
        Rectangle sevenRect;

        Texture2D eight;
        Rectangle eightRect;


        MouseState currentMouseState;
        MouseState lastMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 450;
            graphics.PreferredBackBufferHeight = 420;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            easyBoard = Content.Load<Texture2D>("Images/Backgrounds/board_easy");
            easyBoardRect = new Rectangle(0, 60, easyBoard.Width, easyBoard.Height);
            clock = Content.Load<Texture2D>("Images/Sprites/Watch");
            clockRect = new Rectangle(220, 12, 30, 35);
            flag = Content.Load<Texture2D>("Images/Sprites/flag");
            flagRect = new Rectangle(140, 12, 35, 35);
            hud = Content.Load<Texture2D>("Images/Sprites/HUDBar");
            hudRect = new Rectangle(0, 0, easyBoard.Width, hud.Height);
            soundOn = Content.Load<Texture2D>("Images/Sprites/SoundOn");
            onRect = new Rectangle(365, 12, 30, 30);
            soundOff = Content.Load<Texture2D>("Images/Sprites/SoundOff");
            offRect = new Rectangle(365, 12, 30, 30);

            one = Content.Load<Texture2D>("Images/Sprites/1");
            //oneRect = new Rectangle(365, 12, 30, 30);
            two = Content.Load<Texture2D>("Images/Sprites/2");
            //twoRect = new Rectangle(365, 12, 30, 30);
            three = Content.Load<Texture2D>("Images/Sprites/3");
            //threeRect = new Rectangle(365, 12, 30, 30);
            four = Content.Load<Texture2D>("Images/Sprites/4");
            //fourRect = new Rectangle(365, 12, 30, 30);
            five = Content.Load<Texture2D>("Images/Sprites/5");
            //fiveRect = new Rectangle(365, 12, 30, 30);
            six = Content.Load<Texture2D>("Images/Sprites/6");
            //sixRect = new Rectangle(365, 12, 30, 30);
            seven = Content.Load<Texture2D>("Images/Sprites/7");
            //sevenRect = new Rectangle(365, 12, 30, 30);
            eight = Content.Load<Texture2D>("Images/Sprites/8");
            //eightRect = new Rectangle(365, 12, 30, 30);

            clearDark = Content.Load<Texture2D>("Images/Sprites/Clear_Dark");
            clearLight = Content.Load<Texture2D>("Images/Sprites/Clear_Light");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (isReset == true)
            {
                ResetGame();
            }

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();


            Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
            {
                switch (gameDiff)
                {
                    case EASY:

                        for (int i = 0; i < EASY_ROWS; i++)
                        {
                            for (int j = 0; j < EASY_COLUMN; j++)
                            {
                                if (mousePosition.X > Tiles[i, j].GetX() && mousePosition.X < Tiles[i, j].GetX() + EASY_TILE_SIZE && mousePosition.Y > Tiles[i, j].GetY() && mousePosition.Y < Tiles[i, j].GetY() + EASY_TILE_SIZE)
                                {
                                    if (Tiles[i, j].GetState() != FLAG)
                                    {
                                        Tiles[i, j].SetState(FLAG);
                                    }
                                    else
                                    {
                                        Tiles[i, j].SetState(HIDDEN);
                                    }
                                }
                            }
                        }
                        break;
                    case MEDIUM:
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        spriteBatch.Draw(hud, hudRect, Color.White);
                        break;
                    case HARD:
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        spriteBatch.Draw(hud, hudRect, Color.White);
                        break;
                }
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                switch (gameDiff)
                {
                    case EASY:

                        for (int i = 0; i < EASY_ROWS; i++)
                        {
                            for (int j = 0; j < EASY_COLUMN; j++)
                            {
                                if (mousePosition.X > Tiles[i, j].GetX() && mousePosition.X < Tiles[i, j].GetX() + EASY_TILE_SIZE && mousePosition.Y > Tiles[i, j].GetY() && mousePosition.Y < Tiles[i, j].GetY() + EASY_TILE_SIZE)
                                {
                                    Tiles[i, j].RevealTiles();
                                    break;
                                }
                            }
                        }
                        break;
                    case MEDIUM:
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        spriteBatch.Draw(hud, hudRect, Color.White);
                        break;
                    case HARD:
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        spriteBatch.Draw(hud, hudRect, Color.White);
                        break;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            IsMouseVisible = true;

            switch (gameState)
            {
                case INSTRUCTIONS:
                    DrawInstruction();
                    break;
                case GAMEPLAY:
                    DrawBoard();
                    break;
                case ENDGAME:
                    DrawEndgame();
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawBoard()
        {
            spriteBatch.Begin();

            switch (gameDiff)
            {
                case EASY:
                    spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                    spriteBatch.Draw(hud, hudRect, Color.White);

                    for (int i = 0; i < EASY_ROWS; i++)
                    {
                        for (int j = 0; j < EASY_COLUMN; j++)
                        {
                            if (Tiles[i, j].GetState() == FLAG)
                            {
                                Rectangle flagTileRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                spriteBatch.Draw(flag, flagTileRect, Color.White);
                            }
                            if (Tiles[i, j].GetState() == REVEALED)
                            {
                                Rectangle clearDarkRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                spriteBatch.Draw(clearDark, clearDarkRect, Color.White);

                                //Different places are different sprites on board
                                //if (Tiles[i, j].GetColumn )
                                //{

                                //}
                                //else
                                //{

                                //}

                                switch (Tiles[i, j].BombCount(Bombs))
                                {
                                    case 1:
                                        Rectangle oneRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(one, oneRect, Color.White);
                                        break;
                                    case 2:
                                        Rectangle twoRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(two, twoRect, Color.White);
                                        break;
                                    case 3:
                                        Rectangle threeRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(three, threeRect, Color.White);
                                        break;
                                    case 4:
                                        Rectangle fourRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(four, fourRect, Color.White);
                                        break;
                                    case 5:
                                        Rectangle fiveRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(five, fiveRect, Color.White);
                                        break;
                                    case 6:
                                        Rectangle sixRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(six, sixRect, Color.White);
                                        break;
                                    case 7:
                                        Rectangle sevenRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(seven, sevenRect, Color.White);
                                        break;
                                    case 8:
                                        Rectangle eightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(eight, eightRect, Color.White);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case MEDIUM:
                    spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                    spriteBatch.Draw(hud, hudRect, Color.White);
                    break;
                case HARD:
                    spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                    spriteBatch.Draw(hud, hudRect, Color.White);
                    break;
            }

            spriteBatch.Draw(clock, clockRect, Color.White);
            spriteBatch.Draw(flag, flagRect, Color.White);
            spriteBatch.Draw(soundOn, onRect, Color.White);
            spriteBatch.Draw(soundOff, offRect, Color.White);
            spriteBatch.End();

        }

        private void DrawEndgame()
        {

        }

        private void DrawInstruction()
        {

        }

        private void ResetGame()
        {
            //reset tile revealed, reset bombs, reset flags, reset timer, reset bomb count
            int bombLocation;
            Random randomLocation = new Random();

            switch (gameDiff)
            {
                case EASY:
                    graphics.PreferredBackBufferWidth = 450;
                    graphics.PreferredBackBufferHeight = 420;
                    graphics.ApplyChanges();

                    Tiles = new Tile[EASY_ROWS, EASY_COLUMN];
                    while (Bombs.Count < EASY_MINES)
                    {
                        bombLocation = randomLocation.Next(0, EASY_COLUMN * EASY_ROWS + 1);

                        if (!Bombs.Contains(bombLocation))
                        {
                            Bombs.Add(bombLocation);
                        }
                    }

                    break;
                case MEDIUM:
                    graphics.PreferredBackBufferWidth = 450;
                    graphics.PreferredBackBufferHeight = 420;
                    graphics.ApplyChanges();

                    while (Bombs.Count < MEDIUM_MINES)
                    {
                        bombLocation = randomLocation.Next(0, MEDIUM_COLUMN * MEDIUM_ROWS + 1);

                        if (!Bombs.Contains(bombLocation))
                        {
                            Bombs.Add(bombLocation);
                        }
                    }

                    break;
                case HARD:
                    graphics.PreferredBackBufferWidth = 450;
                    graphics.PreferredBackBufferHeight = 420;
                    graphics.ApplyChanges();

                    while (Bombs.Count < HARD_MINES)
                    {
                        bombLocation = randomLocation.Next(0, HARD_COLUMN * HARD_ROWS + 1);

                        if (!Bombs.Contains(bombLocation))
                        {
                            Bombs.Add(bombLocation);
                        }
                    }

                    break;
            }

            SetTiles();

            isReset = false;
        }

        private void SetTiles()
        {
            switch (gameDiff)
            {
                case EASY:
                    for (int i = 0; i < EASY_ROWS; i++)
                    {
                        for (int j = 0; j < EASY_COLUMN; j++)
                        {
                            Tiles[i, j] = new Tile(j * EASY_TILE_SIZE, i * EASY_TILE_SIZE + 60, i, j);
                            Tiles[i, j].BombCount(Bombs);
                        }
                    }

                    for (int i = 0; i < EASY_ROWS; i++)
                    {
                        for (int j = 0; j < EASY_COLUMN; j++)
                        {
                            Tiles[i, j].SetAdj(Tiles);
                        }
                    }

                    break;
                case MEDIUM:
                    for (int i = 0; i < MEDIUM_ROWS; i++)
                    {
                        for (int j = 0; j < MEDIUM_COLUMN; j++)
                        {
                            Tiles[i, j] = new Tile(j * MEDIUM_TILE_SIZE, i * MEDIUM_TILE_SIZE, i, j);
                            Tiles[i, j].BombCount(Bombs);
                        }
                    }
                    for (int i = 0; i < MEDIUM_ROWS; i++)
                    {
                        for (int j = 0; j < MEDIUM_COLUMN; j++)
                        {
                            Tiles[i, j].SetAdj(Tiles);
                        }
                    }
                    break;
                case HARD:
                    for (int i = 0; i < HARD_ROWS; i++)
                    {
                        for (int j = 0; j < HARD_COLUMN; j++)
                        {
                            Tiles[i, j] = new Tile(j * HARD_TILE_SIZE, i * HARD_TILE_SIZE, i, j);
                            Tiles[i, j].BombCount(Bombs);
                        }
                    }
                    for (int i = 0; i < HARD_ROWS; i++)
                    {
                        for (int j = 0; j < HARD_COLUMN; j++)
                        {
                            Tiles[i, j].SetAdj(Tiles);
                        }
                    }
                    break;
            }
        }
    }
}
