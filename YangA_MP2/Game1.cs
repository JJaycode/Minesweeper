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
using Helper;

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
        public const int WIN = 2;
        public const int LOSE = 3;
        public const int SWITCH = 4;

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

        private const int HUD_HEIGHT = 60;
        private const int HUD_SPRITE_HEIGHT = 35;
        private const int DIFF_BUTTON_LENGTH = 70;
        private const int DIFF_BUTTON_LOC = 10;
        private const int DROPDOWN_HEIGHT = 60;

        public const int HIDDEN = 0;
        public const int REVEALED = 1;
        public const int BOMB = 2;
        public const int FLAG = 3;

        public static int gameState = INSTRUCTIONS;
        public static int gameDiff = EASY;
        public static int gameboardRow = EASY_ROWS;
        public static int gameboardColumn = EASY_COLUMN;
        public static int gameMines = EASY_MINES;
        public static int gameTileSize = EASY_TILE_SIZE;

        public int MAX_TIME = 1000000;

        public static double INST_TIME = 3000;

        public static List<int> Bombs = new List<int>();

        public static Tile[,] Tiles;

        private bool isReset = true;

        private int flagNum;

        private Random rnd = new Random();

        private bool isMuted = false;

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

        Texture2D mine1;
        Rectangle mine1Rect;

        Texture2D mine2;
        Rectangle mine2Rect;

        Texture2D mine3;
        Rectangle mine3Rect;

        Texture2D mine4;
        Rectangle mine4Rect;

        Texture2D mine5;
        Rectangle mine5Rect;

        Texture2D mine6;
        Rectangle mine6Rect;

        Texture2D mine7;
        Rectangle mine7Rect;

        Texture2D mine8;
        Rectangle mine8Rect;

        Texture2D easyButton;
        Texture2D mediumButton;
        Texture2D hardButton;

        Texture2D dropDown;
        Texture2D check;

        Texture2D inst1;
        Texture2D inst2;

        Texture2D gameLose;
        Texture2D gameLoseRetry;
        Texture2D gameLoseNoScore;

        Texture2D gameWin;
        Texture2D gameWinRetry;

        MouseState currentMouseState;
        MouseState lastMouseState;

        SpriteFont gameFont;

        Timer gameTimer = new Timer(Timer.INFINITE_TIMER, true);

        Timer instTimer;

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
            ResetGame();

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
            easyBoardRect = new Rectangle(0, HUD_HEIGHT, easyBoard.Width, easyBoard.Height);
            clock = Content.Load<Texture2D>("Images/Sprites/Watch");
            clockRect = new Rectangle(220, 12, 30, HUD_SPRITE_HEIGHT);
            flag = Content.Load<Texture2D>("Images/Sprites/flag");
            flagRect = new Rectangle(140, 12, 35, HUD_SPRITE_HEIGHT);
            hud = Content.Load<Texture2D>("Images/Sprites/HUDBar");
            hudRect = new Rectangle(0, 0, easyBoard.Width, hud.Height);
            soundOn = Content.Load<Texture2D>("Images/Sprites/SoundOn");
            onRect = new Rectangle(365, 12, 30, HUD_SPRITE_HEIGHT);
            soundOff = Content.Load<Texture2D>("Images/Sprites/SoundOff");
            offRect = new Rectangle(365, 12, 30, HUD_SPRITE_HEIGHT);

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

            mine1 = Content.Load<Texture2D>("Images/Sprites/Mine1");
            mine2 = Content.Load<Texture2D>("Images/Sprites/Mine2");
            mine3 = Content.Load<Texture2D>("Images/Sprites/Mine3");
            mine4 = Content.Load<Texture2D>("Images/Sprites/Mine4");
            mine5 = Content.Load<Texture2D>("Images/Sprites/Mine5");
            mine6 = Content.Load<Texture2D>("Images/Sprites/Mine6");
            mine7 = Content.Load<Texture2D>("Images/Sprites/Mine7");
            mine8 = Content.Load<Texture2D>("Images/Sprites/Mine8");

            gameFont = Content.Load<SpriteFont>("Fonts/SpriteFont");

            easyButton = Content.Load<Texture2D>("Images/Sprites/EasyButton");
            mediumButton = Content.Load<Texture2D>("Images/Sprites/MedButton");
            hardButton = Content.Load<Texture2D>("Images/Sprites/hardButton");

            dropDown = Content.Load<Texture2D>("Images/Sprites/DropDown");
            check = Content.Load<Texture2D>("Images/Sprites/Check");

            inst1 = Content.Load<Texture2D>("Images/Sprites/Instructions1");
            inst2 = Content.Load<Texture2D>("Images/Sprites/Instructions2");

            gameLose = Content.Load<Texture2D>("Images/Sprites/GameOver_Results");
            gameLoseNoScore = Content.Load<Texture2D>("Images/Sprites/GameOver_NoTime");
            gameLoseRetry = Content.Load<Texture2D>("Images/Sprites/GameOver_TryAgain");

            gameWin = Content.Load<Texture2D>("Images/Sprites/GameOver_WinResults");
            gameWinRetry = Content.Load<Texture2D>("Images/Sprites/GameOver_PlayAgain");

            instTimer = new Timer(Timer.INFINITE_TIMER, true);
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

            switch (gameState)
            {
                case INSTRUCTIONS:

                    instTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                    if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released || currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        gameState = GAMEPLAY;
                    }
                    break;
                case GAMEPLAY:

                    gameTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);


                    if (gameTimer.GetTimePassed() > MAX_TIME)
                    {
                        gameState = LOSE;
                    }

                    switch (gameDiff)
                    {
                        case EASY:
                            if (CheckWin(EASY_ROWS, EASY_COLUMN, EASY_MINES) == true)
                            {
                                gameState = WIN;
                            }

                            if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                            {
                                SetFlag(mousePosition, EASY_ROWS, EASY_COLUMN, EASY_TILE_SIZE);
                            }

                            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                            {
                                if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT)
                                {
                                    gameState = SWITCH;
                                }

                                RevealTiles(mousePosition, EASY_ROWS, EASY_COLUMN, EASY_TILE_SIZE);
                            }
                            break;
                        case MEDIUM:
                            if (CheckWin(MEDIUM_ROWS, MEDIUM_COLUMN, MEDIUM_MINES) == true)
                            {
                                gameState = WIN;
                            }

                            if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                            {
                                SetFlag(mousePosition, MEDIUM_ROWS, MEDIUM_COLUMN, MEDIUM_TILE_SIZE);
                            }

                            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                            {
                                if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT)
                                {
                                    gameState = SWITCH;
                                }

                                RevealTiles(mousePosition, MEDIUM_ROWS, MEDIUM_COLUMN, MEDIUM_TILE_SIZE);
                            }
                            break;
                        case HARD:
                            if (CheckWin(HARD_ROWS, HARD_COLUMN, HARD_MINES) == true)
                            {
                                gameState = WIN;
                            }

                            if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                            {
                                SetFlag(mousePosition, HARD_ROWS, HARD_COLUMN, HARD_TILE_SIZE);
                            }

                            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                            {
                                if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT)
                                {
                                    gameState = SWITCH;
                                }

                                RevealTiles(mousePosition, HARD_ROWS, HARD_COLUMN, HARD_TILE_SIZE);
                            }
                                break;
                    }    
                    break;
                case SWITCH:
                    if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        switch (gameState)
                        {
                            case EASY:
                                if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = MEDIUM;
                                    ResetGame();
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT)
                                {
                                    gameDiff = HARD;
                                    ResetGame();
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = EASY;
                                }
                                break;
                            case MEDIUM:
                                break;
                            case HARD:
                                break;
                        }
                        gameState = GAMEPLAY;
                    }
                    break;
                case LOSE:
                    if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        if (mousePosition.X > (easyBoard.Width - gameLose.Width) / 2 && mousePosition.Y > 100 + gameLose.Height && mousePosition.X < (easyBoard.Width - gameLose.Width) / 2 + gameLoseRetry.Width && mousePosition.Y < 100 + gameLose.Height + gameLoseRetry.Height)
                        {
                            gameState = GAMEPLAY;
                            ResetGame();
                        }
                    }
                    break;
                case WIN:
                    if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        if (mousePosition.X > (easyBoard.Width - gameLose.Width) / 2 && mousePosition.Y > 100 + gameLose.Height && mousePosition.X < (easyBoard.Width - gameLose.Width) / 2 + gameLoseRetry.Width && mousePosition.Y < 100 + gameLose.Height + gameLoseRetry.Height)
                        {
                            gameState = GAMEPLAY;
                            ResetGame();
                        }
                    }
                    break;
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
                    spriteBatch.Begin();
                    DrawBoard();
                    DrawInstruction();
                    spriteBatch.End();
                    break;
                case GAMEPLAY:
                    spriteBatch.Begin();
                    DrawBoard();
                    spriteBatch.End();
                    break;
                case WIN:
                    spriteBatch.Begin();
                    DrawBoard();
                    DrawWin();
                    spriteBatch.End();
                    break;
                case LOSE:
                    spriteBatch.Begin();
                    DrawBoard();
                    DrawLose();
                    spriteBatch.End();
                    break;
                case SWITCH:
                    spriteBatch.Begin();
                    DrawBoard();
                    Rectangle dropDownRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT, DIFF_BUTTON_LENGTH, DROPDOWN_HEIGHT);
                    spriteBatch.Draw(dropDown, dropDownRect, Color.White);

                    if (gameDiff == EASY)
                    {
                        Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 - 9, 5, 5);
                        spriteBatch.Draw(check, checkRect, Color.White);
                    }
                    else if (gameDiff == MEDIUM)
                    {
                        Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 - 6, 5, 5);
                        spriteBatch.Draw(check, checkRect, Color.White);
                    }
                    else if (gameDiff == HARD)
                    {
                        Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 - 3, 5, 5);
                        spriteBatch.Draw(check, checkRect, Color.White);
                    }
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawBoard()
        {
            switch (gameDiff)
            {
                case EASY:
                    spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                    spriteBatch.Draw(hud, hudRect, Color.White);

                    Rectangle easyButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                    spriteBatch.Draw(easyButton, easyButtonRect, Color.White);

                    Vector2 flagLoc = new Vector2(180, 20);
                    spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLoc, Color.White);

                    if (gameTimer.IsActive())
                    {
                        double timePassed = gameTimer.GetTimePassed();
                        int secPassed = 0;

                        if (timePassed > 1000)
                        {
                            secPassed = Convert.ToInt32(timePassed / 1000);
                        }

                        Vector2 timerLoc = new Vector2(260, 20);
                        spriteBatch.DrawString(gameFont, secPassed.ToString("000"), timerLoc, Color.White);
                    }

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
                                //Different places are different sprites on board
                                if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 == 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 != 0)
                                {
                                    Rectangle clearLightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                    spriteBatch.Draw(clearLight, clearLightRect, Color.White);
                                }
                                else if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 != 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 == 0)
                                {
                                    Rectangle clearDarkRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                    spriteBatch.Draw(clearDark, clearDarkRect, Color.White);
                                }

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

                            if (Tiles[i, j].GetBombColor() == -1)
                            {
                                int num = rnd.Next(1, 9);

                                Tiles[i, j].SetBombColor(num);
                            }

                            if (Tiles[i, j].GetState() == BOMB)
                            {
                                switch (Tiles[i, j].GetBombColor())
                                {
                                    case 1:
                                        Rectangle mine1Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine1, mine1Rec, Color.White);
                                        break;
                                    case 2:
                                        Rectangle mine2Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine2, mine2Rec, Color.White);
                                        break;
                                    case 3:
                                        Rectangle mine3Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine3, mine3Rec, Color.White);
                                        break;
                                    case 4:
                                        Rectangle mine4Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine4, mine4Rec, Color.White);
                                        break;
                                    case 5:
                                        Rectangle mine5Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine5, mine5Rec, Color.White);
                                        break;
                                    case 6:
                                        Rectangle mine6Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine6, mine6Rec, Color.White);
                                        break;
                                    case 7:
                                        Rectangle mine7Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine7, mine7Rec, Color.White);
                                        break;
                                    case 8:
                                        Rectangle mine8Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
                                        spriteBatch.Draw(mine8, mine8Rec, Color.White);
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

        }

        private void DrawLose()
        {
            Rectangle gameLoseRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, 100, gameLose.Width, gameLose.Height);
            spriteBatch.Draw(gameLose, gameLoseRec, Color.White);
            Rectangle gameLoseNoScoreRec = new Rectangle(100, 250, gameLoseNoScore.Width, gameLoseNoScore.Height);
            spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec, Color.White);
            Rectangle gameLoseRetryRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, 100 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
            spriteBatch.Draw(gameLoseRetry, gameLoseRetryRec, Color.White);
        }

        private void DrawWin()
        {
            Rectangle gameWinRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, 100, gameLose.Width, gameLose.Height);
            spriteBatch.Draw(gameWin, gameWinRec, Color.White); ;
            Rectangle gameWinRetryRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, 100 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
            spriteBatch.Draw(gameWinRetry, gameWinRetryRec, Color.White);
        }


        private void DrawInstruction()
        {
            Rectangle inst1Rect = new Rectangle(100, 100, 100, 100);
            spriteBatch.Draw(inst1, inst1Rect, Color.White);

            Rectangle inst2Rect = new Rectangle(100, 100, 100, 100);
            spriteBatch.Draw(inst2, inst2Rect, Color.White);
        }

        private void ResetGame()
        {
            //reset tile revealed, reset bombs, reset flags, reset timer, reset bomb count
            Bombs.Clear();

            int bombLocation;
            Random randomLocation = new Random();

            gameTimer.ResetTimer(true);
            //instTimer.ResetTimer(true);

            switch (gameDiff)
            {
                case EASY:
                    graphics.PreferredBackBufferWidth = EASY_TILE_SIZE * EASY_COLUMN;
                    graphics.PreferredBackBufferHeight = EASY_TILE_SIZE * EASY_ROWS + HUD_HEIGHT;
                    graphics.ApplyChanges();

                    flagNum = EASY_MINES;

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
                    graphics.PreferredBackBufferWidth = MEDIUM_TILE_SIZE * MEDIUM_COLUMN;
                    graphics.PreferredBackBufferHeight = MEDIUM_TILE_SIZE * MEDIUM_ROWS + HUD_HEIGHT;
                    graphics.ApplyChanges();

                    flagNum = MEDIUM_MINES;

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
                    graphics.PreferredBackBufferWidth = HARD_TILE_SIZE * HARD_COLUMN;
                    graphics.PreferredBackBufferHeight = HARD_TILE_SIZE * HARD_ROWS + HUD_HEIGHT;
                    graphics.ApplyChanges();

                    flagNum = HARD_MINES;

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
                            Tiles[i, j] = new Tile(j * EASY_TILE_SIZE, i * EASY_TILE_SIZE + HUD_HEIGHT, i, j);
                            Tiles[i, j].BombCount(Bombs);
                            Tiles[i, j].SetBombColor(-1);
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

        private void ShowAllBomb(int row, int column)
        {
            for (int k = 0; k < row; k++)
            {
                for (int l = 0; l < column; l++)
                {
                    if (Tiles[k, l].IsBomb(Bombs) == true)
                    {
                        Tiles[k, l].SetState(BOMB);
                    }
                }
            }
        }

        private void RevealTiles(Point mousePosition, int row, int column, int tileSize)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (mousePosition.X > Tiles[i, j].GetX() && mousePosition.X < Tiles[i, j].GetX() + tileSize && mousePosition.Y > Tiles[i, j].GetY() && mousePosition.Y < Tiles[i, j].GetY() + tileSize)
                    {
                        Tiles[i, j].RevealTiles();

                        if (Tiles[i, j].IsBomb(Bombs) == true)
                        {
                            ShowAllBomb(row, column);

                            gameState = LOSE;
                        }
                    }
                }
            }
        }

        private void SetFlag(Point mousePosition, int row, int column, int tileSize)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (mousePosition.X > Tiles[i, j].GetX() && mousePosition.X < Tiles[i, j].GetX() + tileSize && mousePosition.Y > Tiles[i, j].GetY() && mousePosition.Y < Tiles[i, j].GetY() + tileSize)
                    {
                        if (Tiles[i, j].GetState() != FLAG && Tiles[i, j].GetState() == HIDDEN)
                        {
                            Tiles[i, j].SetState(FLAG);
                            flagNum--;
                        }
                        else if (Tiles[i, j].GetState() == FLAG)
                        {
                            Tiles[i, j].SetState(HIDDEN);
                            flagNum++;
                        }
                    }
                }
            }
        }

        private bool CheckWin(int row, int column, int bombCount)
        {
            int count = 0;

            for (int k = 0; k < row; k++)
            {
                for (int l = 0; l < column; l++)
                {
                    if (Tiles[k, l].IsBomb(Bombs) == false)
                    {
                        if (Tiles[k, l].GetState() == REVEALED)
                        {
                            count++;
                        }
                    }
                }
            }

            if (count == row * column - bombCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
