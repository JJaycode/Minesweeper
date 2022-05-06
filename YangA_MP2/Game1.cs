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
        private static StreamReader fileReader;
        private static StreamWriter fileWriter;

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

        private static int currentTime;
        private static int highTime = 1000;

        public static string resultFile = "results.txt";

        Texture2D easyBoard;
        Rectangle easyBoardRect;

        Texture2D medBoard;
        Rectangle medBoardRect;

        Texture2D hardBoard;
        Rectangle hardBoardRect;

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

        Texture2D exit;
        Rectangle exitRect;

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

        Song winMusic;
        Song loseMusic;

        SoundEffect clearFlag;
        SoundEffect mine;
        SoundEffect placeFlag;
        public static SoundEffect bigClear;
        public static SoundEffect smallClear;

        Texture2D mineExplode;
        Animation explodeAnim;
        Vector2 explodePos;

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

            ReadFile("results.txt");

            switch (gameDiff)
            {
                case EASY:
                    gameboardRow = EASY_ROWS;
                    gameboardColumn = EASY_COLUMN;
                    gameMines = EASY_MINES;
                    gameTileSize = EASY_TILE_SIZE;
                    break;
                case MEDIUM:
                    gameboardRow = MEDIUM_ROWS;
                    gameboardColumn = MEDIUM_COLUMN;
                    gameMines = MEDIUM_MINES;
                    gameTileSize = MEDIUM_TILE_SIZE;
                    break;
                case HARD:
                    gameboardRow = HARD_ROWS;
                    gameboardColumn = HARD_COLUMN;
                    gameMines = HARD_MINES;
                    gameTileSize = HARD_TILE_SIZE;
                    break;
            }

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
            medBoard = Content.Load<Texture2D>("Images/Sprites/board_med");
            medBoardRect = new Rectangle(0, HUD_HEIGHT, medBoard.Width, medBoard.Height);
            hardBoard = Content.Load<Texture2D>("Images/Sprites/board_hard");
            hardBoardRect = new Rectangle(0, HUD_HEIGHT, hardBoard.Width, hardBoard.Height);

            clock = Content.Load<Texture2D>("Images/Sprites/Watch");
            flag = Content.Load<Texture2D>("Images/Sprites/flag");
            hud = Content.Load<Texture2D>("Images/Sprites/HUDBar");
            soundOn = Content.Load<Texture2D>("Images/Sprites/SoundOn");
            soundOff = Content.Load<Texture2D>("Images/Sprites/SoundOff");

            one = Content.Load<Texture2D>("Images/Sprites/1");
            two = Content.Load<Texture2D>("Images/Sprites/2");
            three = Content.Load<Texture2D>("Images/Sprites/3");
            four = Content.Load<Texture2D>("Images/Sprites/4");
            five = Content.Load<Texture2D>("Images/Sprites/5");
            six = Content.Load<Texture2D>("Images/Sprites/6");
            seven = Content.Load<Texture2D>("Images/Sprites/7");
            eight = Content.Load<Texture2D>("Images/Sprites/8");

            clearDark = Content.Load<Texture2D>("Images/Sprites/Clear_Dark");
            clearLight = Content.Load<Texture2D>("Images/Sprites/Clear_Light");

            mine1 = Content.Load<Texture2D>("Images/Sprites/Mine1");
            mine2 = Content.Load<Texture2D>("Images/Sprites/Mine2");
            mine3 = Content.Load<Texture2D>("Images/Sprites/Mine3");
            mine4 = Content.Load<Texture2D>("Images/Sprites/Mine4");
            mine5 = Content.Load<Texture2D>("Images/Sprites/Mine5");
            mine6 = Content.Load<Texture2D>("Images/Sprites/Mine6");
            mine7 = Content.Load<Texture2D>("Images/Sprites/Mine7");
            mine8 = Content.Load<Texture2D>("Images/Sprites/Mine8");

            exit = Content.Load<Texture2D>("Images/Sprites/Exit");

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

            winMusic = Content.Load<Song>("Sounds/Win");
            loseMusic = Content.Load<Song>("Sounds/Lose");
            mine = Content.Load<SoundEffect>("Sounds/Mine");
            clearFlag = Content.Load<SoundEffect>("Sounds/ClearFlag");
            placeFlag = Content.Load<SoundEffect>("Sounds/PlaceFlag");
            smallClear = Content.Load<SoundEffect>("Sounds/SmallClear");
            bigClear = Content.Load<SoundEffect>("Sounds/LargeClear");

            mineExplode = Content.Load<Texture2D>("Images/Sprites/explode2");

            explodeAnim = new Animation(mineExplode,               //The sprite sheet image
                                                 5,                         //The number of frames wide the sprite sheet is
                                                 5,                         //The number of frames high the sprite sheet is
                                                 23,                        //The total number of frames in the animation
                                                 0,                         //The starting frame number to draw first
                                                 0,                         //The frame number to draw when the animation is not drawing, Animation.NO_IDLE will prevent drawing
                                                 1,                         //The repetition option, this can be infinite, 1 or any other option other than 0 or a negative value
                                                 1,                         //The number of times to repeat the same frame before the frame is changed, for smoothness purposes
                                                 explodePos,               //The beginning draw location
                                                 0.5f,                      //The scaling amount of the frame
                                                 false);                     //Whether to begin animating immediately or not
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

            if (isMuted == true)
            {
                MediaPlayer.Volume = 0.0f;
                SoundEffect.MasterVolume = 0.0f;
            }
            else
            {
                MediaPlayer.Volume = 0.6f;
                SoundEffect.MasterVolume = 0.6f;
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

                    MediaPlayer.Stop();

                    gameTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                    explodeAnim.Update(gameTime);

                    if (gameTimer.GetTimePassed() > MAX_TIME)
                    {
                        gameState = LOSE;
                    }

                    if (CheckWin(gameboardRow, gameboardColumn, gameMines) == true)
                    {
                        gameState = WIN;
                    }

                    if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                    {
                        SetFlag(mousePosition, gameboardRow, gameboardColumn, gameTileSize);
                    }

                    if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT)
                        {
                            gameState = SWITCH;
                        }
                        else if (mousePosition.X > gameboardColumn * gameTileSize - 85 && mousePosition.Y > 12 && mousePosition.X < gameboardColumn * gameTileSize - 85 + 30 && mousePosition.Y < 12 + HUD_SPRITE_HEIGHT)
                        {
                            if (isMuted == true)
                            {
                                isMuted = false;
                            }
                            else
                            {
                                isMuted = true;
                            }
                        }
                        else if (mousePosition.X > gameboardColumn * gameTileSize - HUD_SPRITE_HEIGHT - 3 && mousePosition.Y > 12 && mousePosition.X < gameboardColumn * gameTileSize - 3 && mousePosition.Y < 12 + HUD_SPRITE_HEIGHT)
                        {
                            Exit();
                            exitRect = new Rectangle(gameboardColumn * gameTileSize - HUD_SPRITE_HEIGHT - 3, 12, HUD_SPRITE_HEIGHT, HUD_SPRITE_HEIGHT);
                        }
                        else
                        {
                            RevealTiles(mousePosition, gameboardRow, gameboardColumn, gameTileSize);
                        }
                    }

                    //switch (gameDiff)
                    //{
                    //    case EASY:
                    //        if (CheckWin(EASY_ROWS, EASY_COLUMN, EASY_MINES) == true)
                    //        {
                    //            gameState = WIN;
                    //        }

                    //        if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                    //        {
                    //            SetFlag(mousePosition, EASY_ROWS, EASY_COLUMN, EASY_TILE_SIZE);
                    //        }

                    //        if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    //        {
                    //            if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT)
                    //            {
                    //                gameState = SWITCH;
                    //            }
                    //            else if (mousePosition.X > EASY_COLUMN * EASY_TILE_SIZE - 85 && mousePosition.Y > 12 && mousePosition.X < EASY_COLUMN * EASY_TILE_SIZE - 85 + 30 && mousePosition.Y < 12 + HUD_SPRITE_HEIGHT)
                    //            {
                    //                if (isMuted == true)
                    //                {
                    //                    isMuted = false;
                    //                }
                    //                else
                    //                {
                    //                    isMuted = true;
                    //                }
                    //            }
                    //            else if (mousePosition.X > EASY_COLUMN * EASY_TILE_SIZE - HUD_SPRITE_HEIGHT - 3 && mousePosition.Y > 12 && mousePosition.X < EASY_COLUMN * EASY_TILE_SIZE - 3 && mousePosition.Y < 12 + HUD_SPRITE_HEIGHT)
                    //            {
                    //                Exit();
                    //                exitRect = new Rectangle(EASY_COLUMN * EASY_TILE_SIZE - HUD_SPRITE_HEIGHT - 3, 12, HUD_SPRITE_HEIGHT, HUD_SPRITE_HEIGHT);
                    //            }
                    //            else
                    //            {
                    //                RevealTiles(mousePosition, EASY_ROWS, EASY_COLUMN, EASY_TILE_SIZE);
                    //            }
                    //        }
                    //        break;
                    //    case MEDIUM:
                    //        if (CheckWin(MEDIUM_ROWS, MEDIUM_COLUMN, MEDIUM_MINES) == true)
                    //        {
                    //            gameState = WIN;
                    //        }

                    //        if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                    //        {
                    //            SetFlag(mousePosition, MEDIUM_ROWS, MEDIUM_COLUMN, MEDIUM_TILE_SIZE);
                    //        }

                    //        if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    //        {
                    //            if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT)
                    //            {
                    //                gameState = SWITCH;
                    //            }
                    //            else
                    //            {
                    //                RevealTiles(mousePosition, MEDIUM_ROWS, MEDIUM_COLUMN, MEDIUM_TILE_SIZE);
                    //            }
                    //        }
                    //        break;
                    //    case HARD:
                    //        if (CheckWin(HARD_ROWS, HARD_COLUMN, HARD_MINES) == true)
                    //        {
                    //            gameState = WIN;
                    //        }

                    //        if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                    //        {
                    //            SetFlag(mousePosition, HARD_ROWS, HARD_COLUMN, HARD_TILE_SIZE);
                    //        }

                    //        if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    //        {
                    //            if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT)
                    //            {
                    //                gameState = SWITCH;
                    //            }
                    //            else
                    //            {
                    //                RevealTiles(mousePosition, HARD_ROWS, HARD_COLUMN, HARD_TILE_SIZE);
                    //            }
                    //        }
                    //        break;
                    //}
                    break;

                case SWITCH:
                    if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        switch (gameDiff)
                        {
                            case EASY:
                                if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = MEDIUM;
                                    gameboardRow = MEDIUM_ROWS;
                                    gameboardColumn = MEDIUM_COLUMN;
                                    gameMines = MEDIUM_MINES;
                                    gameTileSize = MEDIUM_TILE_SIZE;
                                    ResetGame();
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT)
                                {
                                    gameDiff = HARD;
                                    gameboardRow = HARD_ROWS;
                                    gameboardColumn = HARD_COLUMN;
                                    gameMines = HARD_MINES;
                                    gameTileSize = HARD_TILE_SIZE;
                                    ResetGame();
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = EASY;
                                    gameboardRow = EASY_ROWS;
                                    gameboardColumn = EASY_COLUMN;
                                    gameMines = EASY_MINES;
                                    gameTileSize = EASY_TILE_SIZE;
                                }
                                break;
                            case MEDIUM:
                                if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = MEDIUM;
                                    gameboardRow = MEDIUM_ROWS;
                                    gameboardColumn = MEDIUM_COLUMN;
                                    gameMines = MEDIUM_MINES;
                                    gameTileSize = MEDIUM_TILE_SIZE;
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT)
                                {
                                    gameDiff = HARD;
                                    gameboardRow = HARD_ROWS;
                                    gameboardColumn = HARD_COLUMN;
                                    gameMines = HARD_MINES;
                                    gameTileSize = HARD_TILE_SIZE;
                                    ResetGame();
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = EASY;
                                    gameboardRow = EASY_ROWS;
                                    gameboardColumn = EASY_COLUMN;
                                    gameMines = EASY_MINES;
                                    gameTileSize = EASY_TILE_SIZE;
                                    ResetGame();
                                }
                                break;
                            case HARD:
                                if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = MEDIUM;
                                    gameboardRow = MEDIUM_ROWS;
                                    gameboardColumn = MEDIUM_COLUMN;
                                    gameMines = MEDIUM_MINES;
                                    gameTileSize = MEDIUM_TILE_SIZE;
                                    ResetGame();
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3 && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT)
                                {
                                    gameDiff = HARD;
                                    gameboardRow = HARD_ROWS;
                                    gameboardColumn = HARD_COLUMN;
                                    gameMines = HARD_MINES;
                                    gameTileSize = HARD_TILE_SIZE;
                                }
                                else if (mousePosition.X > DIFF_BUTTON_LOC && mousePosition.Y > DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT && mousePosition.X < DIFF_BUTTON_LOC + DIFF_BUTTON_LENGTH && mousePosition.Y < DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3)
                                {
                                    gameDiff = EASY;
                                    gameboardRow = EASY_ROWS;
                                    gameboardColumn = EASY_COLUMN;
                                    gameMines = EASY_MINES;
                                    gameTileSize = EASY_TILE_SIZE;
                                    ResetGame();
                                }
                                break;
                        }
                        gameState = GAMEPLAY;
                    }
                    break;

                case LOSE:

                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(loseMusic);
                    }

                    WriteFile();

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
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(winMusic);
                    }

                    currentTime = Convert.ToInt32(gameTimer.GetTimePassed() / 1000);

                    if (currentTime < highTime)
                    {
                        highTime = currentTime;
                    }

                    WriteFile();

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
                    if (gameDiff == EASY)
                    {
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        DrawBoard(easyBoard.Width, 260, 20);

                        Rectangle easyButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(easyButton, easyButtonRect, Color.White);

                        Vector2 flagLoc = new Vector2(180, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLoc, Color.White);
                    }
                    else if (gameDiff == MEDIUM)
                    {
                        spriteBatch.Draw(medBoard, medBoardRect, Color.White);
                        DrawBoard(medBoard.Width, MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 190, 20);

                        Rectangle medButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(mediumButton, medButtonRect, Color.White);

                        Vector2 flagLocMed = new Vector2(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 265, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocMed, Color.White);
                    }
                    else if (gameDiff == HARD)
                    {
                        spriteBatch.Draw(hardBoard, hardBoardRect, Color.White);
                        DrawBoard(hardBoard.Width, HARD_COLUMN * HARD_TILE_SIZE - 190, 20);

                        Rectangle hardButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(hardButton, hardButtonRect, Color.White);

                        Vector2 flagLocHard = new Vector2(HARD_COLUMN * HARD_TILE_SIZE - 270, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocHard, Color.White);
                    }
                    DrawInstruction();
                    spriteBatch.End();
                    break;
                case GAMEPLAY:
                    spriteBatch.Begin();
                    if (gameDiff == EASY)
                    {
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        DrawBoard(easyBoard.Width, 260, 20);

                        Rectangle easyButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(easyButton, easyButtonRect, Color.White);

                        Vector2 flagLoc = new Vector2(180, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLoc, Color.White);
                    }
                    else if (gameDiff == MEDIUM)
                    {
                        spriteBatch.Draw(medBoard, medBoardRect, Color.White);
                        DrawBoard(medBoard.Width, MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 190, 20);

                        Rectangle medButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(mediumButton, medButtonRect, Color.White);

                        Vector2 flagLocMed = new Vector2(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 265, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocMed, Color.White);
                    }
                    else if (gameDiff == HARD)
                    {
                        spriteBatch.Draw(hardBoard, hardBoardRect, Color.White);
                        DrawBoard(hardBoard.Width, HARD_COLUMN * HARD_TILE_SIZE - 190, 20);

                        Rectangle hardButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(hardButton, hardButtonRect, Color.White);

                        Vector2 flagLocHard = new Vector2(HARD_COLUMN * HARD_TILE_SIZE - 270, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocHard, Color.White);
                    }
                    spriteBatch.End();
                    break;
                case WIN:
                    spriteBatch.Begin();
                    if (gameDiff == EASY)
                    {
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        DrawBoard(easyBoard.Width, 260, 20);

                        Rectangle easyButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(easyButton, easyButtonRect, Color.White);

                        Vector2 flagLoc = new Vector2(180, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLoc, Color.White);
                    }
                    else if (gameDiff == MEDIUM)
                    {
                        spriteBatch.Draw(medBoard, medBoardRect, Color.White);
                        DrawBoard(medBoard.Width, MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 190, 20);

                        Rectangle medButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(mediumButton, medButtonRect, Color.White);

                        Vector2 flagLocMed = new Vector2(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 265, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocMed, Color.White);
                    }
                    else if (gameDiff == HARD)
                    {
                        spriteBatch.Draw(hardBoard, hardBoardRect, Color.White);
                        DrawBoard(hardBoard.Width, HARD_COLUMN * HARD_TILE_SIZE - 190, 20);

                        Rectangle hardButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(hardButton, hardButtonRect, Color.White);

                        Vector2 flagLocHard = new Vector2(HARD_COLUMN * HARD_TILE_SIZE - 270, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocHard, Color.White);
                    }
                    DrawWin();
                    spriteBatch.End();
                    break;
                case LOSE:
                    spriteBatch.Begin();
                    if (gameDiff == EASY)
                    {
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        DrawBoard(easyBoard.Width, 260, 20);

                        Rectangle easyButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(easyButton, easyButtonRect, Color.White);

                        Vector2 flagLoc = new Vector2(180, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLoc, Color.White);
                    }
                    else if (gameDiff == MEDIUM)
                    {
                        spriteBatch.Draw(medBoard, medBoardRect, Color.White);
                        DrawBoard(medBoard.Width, MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 190, 20);

                        Rectangle medButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(mediumButton, medButtonRect, Color.White);

                        Vector2 flagLocMed = new Vector2(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 265, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocMed, Color.White);
                    }
                    else if (gameDiff == HARD)
                    {
                        spriteBatch.Draw(hardBoard, hardBoardRect, Color.White);
                        DrawBoard(hardBoard.Width, HARD_COLUMN * HARD_TILE_SIZE - 190, 20);

                        Rectangle hardButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(hardButton, hardButtonRect, Color.White);

                        Vector2 flagLocHard = new Vector2(HARD_COLUMN * HARD_TILE_SIZE - 270, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocHard, Color.White);
                    }
                    DrawLose();
                    spriteBatch.End();
                    break;
                case SWITCH:
                    spriteBatch.Begin();
                    if (gameDiff == EASY)
                    {
                        spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
                        DrawBoard(easyBoard.Width, 260, 20);

                        Rectangle easyButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(easyButton, easyButtonRect, Color.White);

                        Vector2 flagLoc = new Vector2(180, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLoc, Color.White);

                        Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 - 7, 5, 5);
                        spriteBatch.Draw(check, checkRect, Color.White);
                    }
                    else if (gameDiff == MEDIUM)
                    {
                        spriteBatch.Draw(medBoard, medBoardRect, Color.White);
                        DrawBoard(medBoard.Width, MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 190, 20);

                        Rectangle medButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(mediumButton, medButtonRect, Color.White);

                        Vector2 flagLocMed = new Vector2(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 265, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocMed, Color.White);

                        Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + 4, 5, 5);
                        spriteBatch.Draw(check, checkRect, Color.White);
                    }
                    else if (gameDiff == HARD)
                    {
                        spriteBatch.Draw(hardBoard, hardBoardRect, Color.White);
                        DrawBoard(hardBoard.Width, HARD_COLUMN * HARD_TILE_SIZE - 190, 20);

                        Rectangle hardButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
                        spriteBatch.Draw(hardButton, hardButtonRect, Color.White);

                        Vector2 flagLocHard = new Vector2(HARD_COLUMN * HARD_TILE_SIZE - 270, 20);
                        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocHard, Color.White);

                        Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3, 5, 5);
                        spriteBatch.Draw(check, checkRect, Color.White);
                    }

                    Rectangle dropDownRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT, DIFF_BUTTON_LENGTH, DROPDOWN_HEIGHT);
                    spriteBatch.Draw(dropDown, dropDownRect, Color.White);

                    //if (gameDiff == EASY)
                    //{
                    //    Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 - 7, 5, 5);
                    //    spriteBatch.Draw(check, checkRect, Color.White);
                    //}
                    //else if (gameDiff == MEDIUM)
                    //{
                    //    Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + 4, 5, 5);
                    //    spriteBatch.Draw(check, checkRect, Color.White);
                    //}
                    //else if (gameDiff == HARD)
                    //{
                    //    Rectangle checkRect = new Rectangle(DIFF_BUTTON_LOC + 10, DIFF_BUTTON_LOC + HUD_SPRITE_HEIGHT + DROPDOWN_HEIGHT / 3 + DROPDOWN_HEIGHT / 3, 5, 5);
                    //    spriteBatch.Draw(check, checkRect, Color.White);
                    //}
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawBoard(int hudWidth, int timerLocX, int timerLocY)
        {
            exitRect = new Rectangle(gameboardColumn * gameTileSize - HUD_SPRITE_HEIGHT - 3, 12, HUD_SPRITE_HEIGHT, HUD_SPRITE_HEIGHT);
            offRect = new Rectangle(gameboardColumn * gameTileSize - 85, 12, 30, HUD_SPRITE_HEIGHT);
            onRect = new Rectangle(gameboardColumn * gameTileSize - 85, 12, 30, HUD_SPRITE_HEIGHT);
            clockRect = new Rectangle(gameboardColumn * gameTileSize - 230, 12, 30, HUD_SPRITE_HEIGHT);
            flagRect = new Rectangle(gameboardColumn * gameTileSize - 310, 12, 35, HUD_SPRITE_HEIGHT);
            hudRect = new Rectangle(0, 0, hudWidth, hud.Height);

            spriteBatch.Draw(hud, hudRect, Color.White);
            spriteBatch.Draw(clock, clockRect, Color.White);
            spriteBatch.Draw(flag, flagRect, Color.White);
            spriteBatch.Draw(exit, exitRect, Color.White);

            if (isMuted == true)
            {
                spriteBatch.Draw(soundOff, offRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(soundOn, onRect, Color.White);
            }

            if (gameTimer.IsActive())
            {
                double timePassed = gameTimer.GetTimePassed();
                int secPassed = 0;

                if (timePassed > 1000)
                {
                    secPassed = Convert.ToInt32(timePassed / 1000);
                }

                Vector2 timerLoc = new Vector2(timerLocX, timerLocY);
                spriteBatch.DrawString(gameFont, secPassed.ToString("000"), timerLoc, Color.White);
            }

            for (int i = 0; i < gameboardRow; i++)
            {
                for (int j = 0; j < gameboardColumn; j++)
                {
                    if (Tiles[i, j].GetState() == FLAG)
                    {
                        Rectangle flagTileRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                        spriteBatch.Draw(flag, flagTileRect, Color.White);
                    }

                    if (Tiles[i, j].GetState() == REVEALED)
                    {
                        //Different places are different sprites on board
                        if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 == 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 != 0)
                        {
                            Rectangle clearLightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                            spriteBatch.Draw(clearLight, clearLightRect, Color.White);
                        }
                        else if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 != 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 == 0)
                        {
                            Rectangle clearDarkRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                            spriteBatch.Draw(clearDark, clearDarkRect, Color.White);
                        }

                        switch (Tiles[i, j].BombCount(Bombs))
                        {
                            case 1:
                                Rectangle oneRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(one, oneRect, Color.White);
                                break;
                            case 2:
                                Rectangle twoRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(two, twoRect, Color.White);
                                break;
                            case 3:
                                Rectangle threeRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(three, threeRect, Color.White);
                                break;
                            case 4:
                                Rectangle fourRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(four, fourRect, Color.White);
                                break;
                            case 5:
                                Rectangle fiveRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(five, fiveRect, Color.White);
                                break;
                            case 6:
                                Rectangle sixRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(six, sixRect, Color.White);
                                break;
                            case 7:
                                Rectangle sevenRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(seven, sevenRect, Color.White);
                                break;
                            case 8:
                                Rectangle eightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
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
                                Rectangle mine1Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine1, mine1Rec, Color.White);
                                break;
                            case 2:
                                Rectangle mine2Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine2, mine2Rec, Color.White);
                                break;
                            case 3:
                                Rectangle mine3Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine3, mine3Rec, Color.White);
                                break;
                            case 4:
                                Rectangle mine4Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine4, mine4Rec, Color.White);
                                break;
                            case 5:
                                Rectangle mine5Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine5, mine5Rec, Color.White);
                                break;
                            case 6:
                                Rectangle mine6Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine6, mine6Rec, Color.White);
                                break;
                            case 7:
                                Rectangle mine7Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine7, mine7Rec, Color.White);
                                break;
                            case 8:
                                Rectangle mine8Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), gameTileSize, gameTileSize);
                                spriteBatch.Draw(mine8, mine8Rec, Color.White);
                                break;
                        }

                        //explodePos = new Vector2(Tiles[i, j].GetX(), Tiles[i, j].GetY());

                        //explodeAnim = new Animation(mineExplode,               //The sprite sheet image
                        //                 5,                         //The number of frames wide the sprite sheet is
                        //                 5,                         //The number of frames high the sprite sheet is
                        //                 23,                        //The total number of frames in the animation
                        //                 0,                         //The starting frame number to draw first
                        //                 0,                         //The frame number to draw when the animation is not drawing, Animation.NO_IDLE will prevent drawing
                        //                 1,                         //The repetition option, this can be infinite, 1 or any other option other than 0 or a negative value
                        //                 1,                         //The number of times to repeat the same frame before the frame is changed, for smoothness purposes
                        //                 explodePos,               //The beginning draw location
                        //                 0.5f,                      //The scaling amount of the frame
                        //                 true);                     //Whether to begin animating immediately or not

                        //explodeAnim.isAnimating = true;

                        explodeAnim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                    }
                }
            }
            //switch (gameDiff)
            //{
            //    case EASY:
            //        exitRect = new Rectangle(EASY_COLUMN * EASY_TILE_SIZE - HUD_SPRITE_HEIGHT - 3, 12, HUD_SPRITE_HEIGHT, HUD_SPRITE_HEIGHT);
            //        offRect = new Rectangle(EASY_COLUMN * EASY_TILE_SIZE - 85, 12, 30, HUD_SPRITE_HEIGHT);
            //        onRect = new Rectangle(EASY_COLUMN * EASY_TILE_SIZE - 85, 12, 30, HUD_SPRITE_HEIGHT);
            //        clockRect = new Rectangle(EASY_COLUMN * EASY_TILE_SIZE - 230, 12, 30, HUD_SPRITE_HEIGHT);
            //        flagRect = new Rectangle(EASY_COLUMN * EASY_TILE_SIZE - 310, 12, 35, HUD_SPRITE_HEIGHT);
            //        hudRect = new Rectangle(0, 0, easyBoard.Width, hud.Height);

            //        //spriteBatch.Draw(easyBoard, easyBoardRect, Color.White);
            //        spriteBatch.Draw(hud, hudRect, Color.White);
            //        spriteBatch.Draw(clock, clockRect, Color.White);
            //        spriteBatch.Draw(flag, flagRect, Color.White);
            //        spriteBatch.Draw(exit, exitRect, Color.White);

            //        if (isMuted == true)
            //        {
            //            spriteBatch.Draw(soundOff, offRect, Color.White);
            //        }
            //        else
            //        {
            //            spriteBatch.Draw(soundOn, onRect, Color.White);
            //        }

            //        Rectangle easyButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
            //        spriteBatch.Draw(easyButton, easyButtonRect, Color.White);

            //        Vector2 flagLoc = new Vector2(180, 20);
            //        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLoc, Color.White);

            //        if (gameTimer.IsActive())
            //        {
            //            double timePassed = gameTimer.GetTimePassed();
            //            int secPassed = 0;

            //            if (timePassed > 1000)
            //            {
            //                secPassed = Convert.ToInt32(timePassed / 1000);
            //            }

            //            Vector2 timerLoc = new Vector2(260, 20);
            //            spriteBatch.DrawString(gameFont, secPassed.ToString("000"), timerLoc, Color.White);
            //        }

            //        for (int i = 0; i < EASY_ROWS; i++)
            //        {
            //            for (int j = 0; j < EASY_COLUMN; j++)
            //            {
            //                if (Tiles[i, j].GetState() == FLAG)
            //                {
            //                    Rectangle flagTileRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                    spriteBatch.Draw(flag, flagTileRect, Color.White);
            //                }

            //                if (Tiles[i, j].GetState() == REVEALED)
            //                {
            //                    //Different places are different sprites on board
            //                    if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 == 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 != 0)
            //                    {
            //                        Rectangle clearLightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                        spriteBatch.Draw(clearLight, clearLightRect, Color.White);
            //                    }
            //                    else if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 != 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 == 0)
            //                    {
            //                        Rectangle clearDarkRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                        spriteBatch.Draw(clearDark, clearDarkRect, Color.White);
            //                    }

            //                    switch (Tiles[i, j].BombCount(Bombs))
            //                    {
            //                        case 1:
            //                            Rectangle oneRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(one, oneRect, Color.White);
            //                            break;
            //                        case 2:
            //                            Rectangle twoRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(two, twoRect, Color.White);
            //                            break;
            //                        case 3:
            //                            Rectangle threeRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(three, threeRect, Color.White);
            //                            break;
            //                        case 4:
            //                            Rectangle fourRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(four, fourRect, Color.White);
            //                            break;
            //                        case 5:
            //                            Rectangle fiveRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(five, fiveRect, Color.White);
            //                            break;
            //                        case 6:
            //                            Rectangle sixRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(six, sixRect, Color.White);
            //                            break;
            //                        case 7:
            //                            Rectangle sevenRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(seven, sevenRect, Color.White);
            //                            break;
            //                        case 8:
            //                            Rectangle eightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(eight, eightRect, Color.White);
            //                            break;
            //                        default:
            //                            break;
            //                    }
            //                }

            //                if (Tiles[i, j].GetBombColor() == -1)
            //                {
            //                    int num = rnd.Next(1, 9);

            //                    Tiles[i, j].SetBombColor(num);
            //                }

            //                if (Tiles[i, j].GetState() == BOMB)
            //                {
            //                    switch (Tiles[i, j].GetBombColor())
            //                    {
            //                        case 1:
            //                            Rectangle mine1Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine1, mine1Rec, Color.White);
            //                            break;
            //                        case 2:
            //                            Rectangle mine2Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine2, mine2Rec, Color.White);
            //                            break;
            //                        case 3:
            //                            Rectangle mine3Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine3, mine3Rec, Color.White);
            //                            break;
            //                        case 4:
            //                            Rectangle mine4Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine4, mine4Rec, Color.White);
            //                            break;
            //                        case 5:
            //                            Rectangle mine5Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine5, mine5Rec, Color.White);
            //                            break;
            //                        case 6:
            //                            Rectangle mine6Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine6, mine6Rec, Color.White);
            //                            break;
            //                        case 7:
            //                            Rectangle mine7Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine7, mine7Rec, Color.White);
            //                            break;
            //                        case 8:
            //                            Rectangle mine8Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), EASY_TILE_SIZE, EASY_TILE_SIZE);
            //                            spriteBatch.Draw(mine8, mine8Rec, Color.White);
            //                            break;
            //                    }

            //                    explodePos = new Vector2(Tiles[i, j].GetX(), Tiles[i, j].GetY());

            //                    explodeAnim = new Animation(mineExplode,               //The sprite sheet image
            //                                     5,                         //The number of frames wide the sprite sheet is
            //                                     5,                         //The number of frames high the sprite sheet is
            //                                     23,                        //The total number of frames in the animation
            //                                     0,                         //The starting frame number to draw first
            //                                     0,                         //The frame number to draw when the animation is not drawing, Animation.NO_IDLE will prevent drawing
            //                                     1,                         //The repetition option, this can be infinite, 1 or any other option other than 0 or a negative value
            //                                     1,                         //The number of times to repeat the same frame before the frame is changed, for smoothness purposes
            //                                     explodePos,               //The beginning draw location
            //                                     0.5f,                      //The scaling amount of the frame
            //                                     true);                     //Whether to begin animating immediately or not

            //                    explodeAnim.isAnimating = true;

            //                    explodeAnim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
            //                }
            //            }
            //        }
            //        break;
            //    case MEDIUM:
            //        offRect = new Rectangle(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 85, 12, 30, HUD_SPRITE_HEIGHT);
            //        onRect = new Rectangle(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 85, 12, 30, HUD_SPRITE_HEIGHT);
            //        clockRect = new Rectangle(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 230, 12, 30, HUD_SPRITE_HEIGHT);
            //        flagRect = new Rectangle(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 310, 12, 35, HUD_SPRITE_HEIGHT);
            //        hudRect = new Rectangle(0, 0, medBoard.Width, hud.Height);

            //        spriteBatch.Draw(medBoard, medBoardRect, Color.White);
            //        spriteBatch.Draw(hud, hudRect, Color.White);
            //        spriteBatch.Draw(clock, clockRect, Color.White);
            //        spriteBatch.Draw(flag, flagRect, Color.White);
            //        spriteBatch.Draw(soundOn, onRect, Color.White);
            //        spriteBatch.Draw(soundOff, offRect, Color.White);

            //        Rectangle medButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
            //        spriteBatch.Draw(mediumButton, medButtonRect, Color.White);

            //        Vector2 flagLocMed = new Vector2(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 265, 20);
            //        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocMed, Color.White);

            //        if (gameTimer.IsActive())
            //        {
            //            double timePassed = gameTimer.GetTimePassed();
            //            int secPassed = 0;

            //            if (timePassed > 1000)
            //            {
            //                secPassed = Convert.ToInt32(timePassed / 1000);
            //            }

            //            Vector2 timerLoc = new Vector2(MEDIUM_COLUMN * MEDIUM_TILE_SIZE - 190, 20);
            //            spriteBatch.DrawString(gameFont, secPassed.ToString("000"), timerLoc, Color.White);
            //        }

            //        for (int i = 0; i < MEDIUM_ROWS; i++)
            //        {
            //            for (int j = 0; j < MEDIUM_COLUMN; j++)
            //            {
            //                if (Tiles[i, j].GetState() == FLAG)
            //                {
            //                    Rectangle flagTileRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                    spriteBatch.Draw(flag, flagTileRect, Color.White);
            //                }

            //                if (Tiles[i, j].GetState() == REVEALED)
            //                {
            //                    //Different places are different sprites on board
            //                    if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 == 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 != 0)
            //                    {
            //                        Rectangle clearLightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                        spriteBatch.Draw(clearLight, clearLightRect, Color.White);
            //                    }
            //                    else if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 != 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 == 0)
            //                    {
            //                        Rectangle clearDarkRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                        spriteBatch.Draw(clearDark, clearDarkRect, Color.White);
            //                    }

            //                    switch (Tiles[i, j].BombCount(Bombs))
            //                    {
            //                        case 1:
            //                            Rectangle oneRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(one, oneRect, Color.White);
            //                            break;
            //                        case 2:
            //                            Rectangle twoRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(two, twoRect, Color.White);
            //                            break;
            //                        case 3:
            //                            Rectangle threeRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(three, threeRect, Color.White);
            //                            break;
            //                        case 4:
            //                            Rectangle fourRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(four, fourRect, Color.White);
            //                            break;
            //                        case 5:
            //                            Rectangle fiveRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(five, fiveRect, Color.White);
            //                            break;
            //                        case 6:
            //                            Rectangle sixRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(six, sixRect, Color.White);
            //                            break;
            //                        case 7:
            //                            Rectangle sevenRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(seven, sevenRect, Color.White);
            //                            break;
            //                        case 8:
            //                            Rectangle eightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(eight, eightRect, Color.White);
            //                            break;
            //                        default:
            //                            break;
            //                    }
            //                }

            //                if (Tiles[i, j].GetBombColor() == -1)
            //                {
            //                    int num = rnd.Next(1, 9);

            //                    Tiles[i, j].SetBombColor(num);
            //                }

            //                if (Tiles[i, j].GetState() == BOMB)
            //                {
            //                    switch (Tiles[i, j].GetBombColor())
            //                    {
            //                        case 1:
            //                            Rectangle mine1Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine1, mine1Rec, Color.White);
            //                            break;
            //                        case 2:
            //                            Rectangle mine2Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine2, mine2Rec, Color.White);
            //                            break;
            //                        case 3:
            //                            Rectangle mine3Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine3, mine3Rec, Color.White);
            //                            break;
            //                        case 4:
            //                            Rectangle mine4Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine4, mine4Rec, Color.White);
            //                            break;
            //                        case 5:
            //                            Rectangle mine5Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine5, mine5Rec, Color.White);
            //                            break;
            //                        case 6:
            //                            Rectangle mine6Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine6, mine6Rec, Color.White);
            //                            break;
            //                        case 7:
            //                            Rectangle mine7Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine7, mine7Rec, Color.White);
            //                            break;
            //                        case 8:
            //                            Rectangle mine8Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), MEDIUM_TILE_SIZE, MEDIUM_TILE_SIZE);
            //                            spriteBatch.Draw(mine8, mine8Rec, Color.White);
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //        break;
            //    case HARD:
            //        offRect = new Rectangle(HARD_COLUMN * HARD_TILE_SIZE - 85, 12, 30, HUD_SPRITE_HEIGHT);
            //        onRect = new Rectangle(HARD_COLUMN * HARD_TILE_SIZE - 85, 12, 30, HUD_SPRITE_HEIGHT);
            //        clockRect = new Rectangle(HARD_COLUMN * HARD_TILE_SIZE - 230, 12, 30, HUD_SPRITE_HEIGHT);
            //        flagRect = new Rectangle(HARD_COLUMN * HARD_TILE_SIZE - 310, 12, 35, HUD_SPRITE_HEIGHT);
            //        hudRect = new Rectangle(0, 0, hardBoard.Width, hud.Height);

            //        spriteBatch.Draw(hardBoard, hardBoardRect, Color.White);
            //        spriteBatch.Draw(hud, hudRect, Color.White);
            //        spriteBatch.Draw(clock, clockRect, Color.White);
            //        spriteBatch.Draw(flag, flagRect, Color.White);
            //        spriteBatch.Draw(soundOn, onRect, Color.White);
            //        spriteBatch.Draw(soundOff, offRect, Color.White);

            //        Rectangle hardButtonRect = new Rectangle(DIFF_BUTTON_LOC, DIFF_BUTTON_LOC, DIFF_BUTTON_LENGTH, HUD_SPRITE_HEIGHT);
            //        spriteBatch.Draw(hardButton, hardButtonRect, Color.White);

            //        Vector2 flagLocHard = new Vector2(HARD_COLUMN * HARD_TILE_SIZE - 270, 20);
            //        spriteBatch.DrawString(gameFont, flagNum.ToString(), flagLocHard, Color.White);

            //        if (gameTimer.IsActive())
            //        {
            //            double timePassed = gameTimer.GetTimePassed();
            //            int secPassed = 0;

            //            if (timePassed > 1000)
            //            {
            //                secPassed = Convert.ToInt32(timePassed / 1000);
            //            }

            //            Vector2 timerLoc = new Vector2(HARD_COLUMN * HARD_TILE_SIZE - 190, 20);
            //            spriteBatch.DrawString(gameFont, secPassed.ToString("000"), timerLoc, Color.White);
            //        }

            //        for (int i = 0; i < HARD_ROWS; i++)
            //        {
            //            for (int j = 0; j < HARD_COLUMN; j++)
            //            {
            //                if (Tiles[i, j].GetState() == FLAG)
            //                {
            //                    Rectangle flagTileRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                    spriteBatch.Draw(flag, flagTileRect, Color.White);
            //                }

            //                if (Tiles[i, j].GetState() == REVEALED)
            //                {
            //                    //Different places are different sprites on board
            //                    if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 == 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 != 0)
            //                    {
            //                        Rectangle clearLightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                        spriteBatch.Draw(clearLight, clearLightRect, Color.White);
            //                    }
            //                    else if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 != 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 == 0)
            //                    {
            //                        Rectangle clearDarkRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                        spriteBatch.Draw(clearDark, clearDarkRect, Color.White);
            //                    }

            //                    switch (Tiles[i, j].BombCount(Bombs))
            //                    {
            //                        case 1:
            //                            Rectangle oneRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(one, oneRect, Color.White);
            //                            break;
            //                        case 2:
            //                            Rectangle twoRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(two, twoRect, Color.White);
            //                            break;
            //                        case 3:
            //                            Rectangle threeRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(three, threeRect, Color.White);
            //                            break;
            //                        case 4:
            //                            Rectangle fourRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(four, fourRect, Color.White);
            //                            break;
            //                        case 5:
            //                            Rectangle fiveRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(five, fiveRect, Color.White);
            //                            break;
            //                        case 6:
            //                            Rectangle sixRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(six, sixRect, Color.White);
            //                            break;
            //                        case 7:
            //                            Rectangle sevenRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(seven, sevenRect, Color.White);
            //                            break;
            //                        case 8:
            //                            Rectangle eightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(eight, eightRect, Color.White);
            //                            break;
            //                        default:
            //                            break;
            //                    }
            //                }

            //                if (Tiles[i, j].GetBombColor() == -1)
            //                {
            //                    int num = rnd.Next(1, 9);

            //                    Tiles[i, j].SetBombColor(num);
            //                }

            //                if (Tiles[i, j].GetState() == BOMB)
            //                {
            //                    switch (Tiles[i, j].GetBombColor())
            //                    {
            //                        case 1:
            //                            Rectangle mine1Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine1, mine1Rec, Color.White);
            //                            break;
            //                        case 2:
            //                            Rectangle mine2Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine2, mine2Rec, Color.White);
            //                            break;
            //                        case 3:
            //                            Rectangle mine3Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine3, mine3Rec, Color.White);
            //                            break;
            //                        case 4:
            //                            Rectangle mine4Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine4, mine4Rec, Color.White);
            //                            break;
            //                        case 5:
            //                            Rectangle mine5Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine5, mine5Rec, Color.White);
            //                            break;
            //                        case 6:
            //                            Rectangle mine6Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine6, mine6Rec, Color.White);
            //                            break;
            //                        case 7:
            //                            Rectangle mine7Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine7, mine7Rec, Color.White);
            //                            break;
            //                        case 8:
            //                            Rectangle mine8Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), HARD_TILE_SIZE, HARD_TILE_SIZE);
            //                            spriteBatch.Draw(mine8, mine8Rec, Color.White);
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //        break;
            //}
        }

        private void DrawLose()
        {
            switch (gameDiff)
            {
                case EASY:
                    Rectangle gameLoseRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, (easyBoard.Height - gameLose.Height + HUD_HEIGHT) / 2, gameLose.Width, gameLose.Height);
                    spriteBatch.Draw(gameLose, gameLoseRec, Color.White);
                    Rectangle gameLoseNoScoreRec = new Rectangle(135, 200, gameLoseNoScore.Width, gameLoseNoScore.Height);
                    spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec, Color.White);
                    Rectangle gameLoseRetryRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, (easyBoard.Height - gameLose.Height + HUD_HEIGHT) / 2 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
                    spriteBatch.Draw(gameLoseRetry, gameLoseRetryRec, Color.White);

                    if (highTime == 1000)
                    {
                        Rectangle gameLoseNoScoreRec3 = new Rectangle(265, 200, gameLoseNoScore.Width, gameLoseNoScore.Height);
                        spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec3, Color.White);
                    }
                    else
                    {
                        Vector2 highScoreLoc = new Vector2(275, 200);
                        spriteBatch.DrawString(gameFont, highTime.ToString("000"), highScoreLoc, Color.White);
                    }
                    break;
                case MEDIUM:
                    Rectangle gameLoseRec2 = new Rectangle((medBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2, gameLose.Width, gameLose.Height);
                    spriteBatch.Draw(gameLose, gameLoseRec2, Color.White);
                    Rectangle gameLoseNoScoreRec2 = new Rectangle(180, 235, gameLoseNoScore.Width, gameLoseNoScore.Height);
                    spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec2, Color.White);
                    Rectangle gameLoseRetryRec2 = new Rectangle((medBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
                    spriteBatch.Draw(gameLoseRetry, gameLoseRetryRec2, Color.White);

                    if (highTime == 1000)
                    {
                        Rectangle gameLoseNoScoreRec5 = new Rectangle(310, 235, gameLoseNoScore.Width, gameLoseNoScore.Height);
                        spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec5, Color.White);
                    }
                    else
                    {
                        Vector2 highScoreLoc = new Vector2(310, 235);
                        spriteBatch.DrawString(gameFont, highTime.ToString("000"), highScoreLoc, Color.White);
                    }
                    break;
                case HARD:
                    Rectangle gameLoseRec4 = new Rectangle((hardBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2, gameLose.Width, gameLose.Height);
                    spriteBatch.Draw(gameLose, gameLoseRec4, Color.White);
                    Rectangle gameLoseNoScoreRec4 = new Rectangle(210, 235, gameLoseNoScore.Width, gameLoseNoScore.Height);
                    spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec4, Color.White);
                    Rectangle gameLoseRetryRec4 = new Rectangle((hardBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
                    spriteBatch.Draw(gameLoseRetry, gameLoseRetryRec4, Color.White);

                    if (highTime == 1000)
                    {
                        Rectangle gameLoseNoScoreRec6 = new Rectangle(340, 235, gameLoseNoScore.Width, gameLoseNoScore.Height);
                        spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec6, Color.White);
                    }
                    else
                    {
                        Vector2 highScoreLoc = new Vector2(340, 235);
                        spriteBatch.DrawString(gameFont, highTime.ToString("000"), highScoreLoc, Color.White);
                    }
                    break;
            }
        }

        private void DrawWin()
        {
            switch (gameDiff)
            {
                case EASY:
                    Rectangle gameWinRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, (easyBoard.Height - gameLose.Height + HUD_HEIGHT) / 2, gameLose.Width, gameLose.Height);
                    spriteBatch.Draw(gameWin, gameWinRec, Color.White);
                    Rectangle gameWinRetryRec = new Rectangle((easyBoard.Width - gameLose.Width) / 2, (easyBoard.Height - gameLose.Height + HUD_HEIGHT) / 2 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
                    spriteBatch.Draw(gameWinRetry, gameWinRetryRec, Color.White);
                    Vector2 currentScorLoc = new Vector2(135, 200);
                    spriteBatch.DrawString(gameFont, currentTime.ToString("000"), currentScorLoc, Color.White);

                    if (highTime == 1000)
                    {
                        Rectangle gameLoseNoScoreRec3 = new Rectangle(265, 200, gameLoseNoScore.Width, gameLoseNoScore.Height);
                        spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec3, Color.White);
                    }
                    else
                    {
                        Vector2 highScoreLoc = new Vector2(275, 200);
                        spriteBatch.DrawString(gameFont, highTime.ToString("000"), highScoreLoc, Color.White);
                    }
                    break;
                case MEDIUM:
                    Rectangle gameWinRec2 = new Rectangle((medBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2, gameLose.Width, gameLose.Height);
                    spriteBatch.Draw(gameWin, gameWinRec2, Color.White);
                    Rectangle gameWinRetryRec2 = new Rectangle((medBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
                    spriteBatch.Draw(gameWinRetry, gameWinRetryRec2, Color.White);
                    Vector2 currentScorLoc2 = new Vector2(180, 235);
                    spriteBatch.DrawString(gameFont, currentTime.ToString("000"), currentScorLoc2, Color.White);

                    if (highTime == 1000)
                    {
                        Rectangle gameLoseNoScoreRec5 = new Rectangle(310, 235, gameLoseNoScore.Width, gameLoseNoScore.Height);
                        spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec5, Color.White);
                    }
                    else
                    {
                        Vector2 highScoreLoc2 = new Vector2(310, 235);
                        spriteBatch.DrawString(gameFont, highTime.ToString("000"), highScoreLoc2, Color.White);
                    }
                    break;
                case HARD:
                    Rectangle gameWinRec3 = new Rectangle((hardBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2, gameLose.Width, gameLose.Height);
                    spriteBatch.Draw(gameWin, gameWinRec3, Color.White);
                    Rectangle gameWinRetryRec3 = new Rectangle((hardBoard.Width - gameLose.Width) / 2, (medBoard.Height - gameLose.Height + HUD_HEIGHT) / 2 + gameLose.Height, gameLose.Width, gameLoseRetry.Height);
                    spriteBatch.Draw(gameWinRetry, gameWinRetryRec3, Color.White);
                    Vector2 currentScorLoc3 = new Vector2(210, 235);
                    spriteBatch.DrawString(gameFont, currentTime.ToString("000"), currentScorLoc3, Color.White);

                    if (highTime == 1000)
                    {
                        Rectangle gameLoseNoScoreRec6 = new Rectangle(340, 235, gameLoseNoScore.Width, gameLoseNoScore.Height);
                        spriteBatch.Draw(gameLoseNoScore, gameLoseNoScoreRec6, Color.White);
                    }
                    else
                    {
                        Vector2 highScoreLoc3 = new Vector2(340, 235);
                        spriteBatch.DrawString(gameFont, highTime.ToString("000"), highScoreLoc3, Color.White);
                    }
                    break;
            }
        }


        private void DrawInstruction()
        {
            Rectangle inst1Rect = new Rectangle((gameboardColumn * gameTileSize) / 2 - 100, (gameboardRow * gameTileSize) / 2 - 50, 100, 100);
            spriteBatch.Draw(inst1, inst1Rect, Color.White);

            Rectangle inst2Rect = new Rectangle((gameboardColumn * gameTileSize) / 2, (gameboardRow * gameTileSize) / 2 - 50, 100, 100);
            spriteBatch.Draw(inst2, inst2Rect, Color.White);
        }

        private void ResetGame()
        {
            //reset tile revealed, reset bombs, reset flags, reset timer, reset bomb count
            Bombs.Clear();

            int bombLocation;
            Random randomLocation = new Random();

            gameTimer.ResetTimer(true);

            graphics.PreferredBackBufferWidth = gameTileSize * gameboardColumn;
            graphics.PreferredBackBufferHeight = gameTileSize * gameboardRow + HUD_HEIGHT;
            graphics.ApplyChanges();

            flagNum = gameMines;

            Tiles = new Tile[gameboardRow, gameboardColumn];

            while (Bombs.Count < gameMines)
            {
                bombLocation = randomLocation.Next(0, gameboardColumn * gameboardRow + 1);

                if (!Bombs.Contains(bombLocation))
                {
                    Bombs.Add(bombLocation);
                }
            }
            //switch (gameDiff)
            //{
            //    case EASY:
            //        graphics.PreferredBackBufferWidth = EASY_TILE_SIZE * EASY_COLUMN;
            //        graphics.PreferredBackBufferHeight = EASY_TILE_SIZE * EASY_ROWS + HUD_HEIGHT;
            //        graphics.ApplyChanges();

            //        flagNum = EASY_MINES;

            //        Tiles = new Tile[EASY_ROWS, EASY_COLUMN];

            //        while (Bombs.Count < EASY_MINES)
            //        {
            //            bombLocation = randomLocation.Next(0, EASY_COLUMN * EASY_ROWS + 1);

            //            if (!Bombs.Contains(bombLocation))
            //            {
            //                Bombs.Add(bombLocation);
            //            }
            //        }
            //        break;
            //    case MEDIUM:
            //        graphics.PreferredBackBufferWidth = MEDIUM_TILE_SIZE * MEDIUM_COLUMN;
            //        graphics.PreferredBackBufferHeight = MEDIUM_TILE_SIZE * MEDIUM_ROWS + HUD_HEIGHT;
            //        graphics.ApplyChanges();

            //        flagNum = MEDIUM_MINES;

            //        Tiles = new Tile[MEDIUM_ROWS, MEDIUM_COLUMN];

            //        while (Bombs.Count < MEDIUM_MINES)
            //        {
            //            bombLocation = randomLocation.Next(0, MEDIUM_COLUMN * MEDIUM_ROWS + 1);

            //            if (!Bombs.Contains(bombLocation))
            //            {
            //                Bombs.Add(bombLocation);
            //            }
            //        }
            //        break;
            //    case HARD:
            //        graphics.PreferredBackBufferWidth = HARD_TILE_SIZE * HARD_COLUMN;
            //        graphics.PreferredBackBufferHeight = HARD_TILE_SIZE * HARD_ROWS + HUD_HEIGHT;
            //        graphics.ApplyChanges();

            //        flagNum = HARD_MINES;

            //        Tiles = new Tile[HARD_ROWS, HARD_COLUMN];

            //        while (Bombs.Count < HARD_MINES)
            //        {
            //            bombLocation = randomLocation.Next(0, HARD_COLUMN * HARD_ROWS + 1);

            //            if (!Bombs.Contains(bombLocation))
            //            {
            //                Bombs.Add(bombLocation);
            //            }
            //        }
            //        break;
            //}

            SetTiles();

            isReset = false;
        }

        private void SetTiles()
        {
            for (int i = 0; i < gameboardRow; i++)
            {
                for (int j = 0; j < gameboardColumn; j++)
                {
                    Tiles[i, j] = new Tile(j * gameTileSize, i * gameTileSize + HUD_HEIGHT, i, j);
                    Tiles[i, j].BombCount(Bombs);
                    Tiles[i, j].SetBombColor(-1);
                }
            }
            for (int i = 0; i < gameboardRow; i++)
            {
                for (int j = 0; j < gameboardColumn; j++)
                {
                    Tiles[i, j].SetAdj(Tiles);
                }
            }
            //switch (gameDiff)
            //{
            //    case EASY:
            //        for (int i = 0; i < EASY_ROWS; i++)
            //        {
            //            for (int j = 0; j < EASY_COLUMN; j++)
            //            {
            //                Tiles[i, j] = new Tile(j * EASY_TILE_SIZE, i * EASY_TILE_SIZE + HUD_HEIGHT, i, j);
            //                Tiles[i, j].BombCount(Bombs);
            //                Tiles[i, j].SetBombColor(-1);
            //            }
            //        }
            //        for (int i = 0; i < EASY_ROWS; i++)
            //        {
            //            for (int j = 0; j < EASY_COLUMN; j++)
            //            {
            //                Tiles[i, j].SetAdj(Tiles);
            //            }
            //        }
            //        break;
            //    case MEDIUM:
            //        for (int i = 0; i < MEDIUM_ROWS; i++)
            //        {
            //            for (int j = 0; j < MEDIUM_COLUMN; j++)
            //            {
            //                Tiles[i, j] = new Tile(j * MEDIUM_TILE_SIZE, i * MEDIUM_TILE_SIZE + HUD_HEIGHT, i, j);
            //                Tiles[i, j].BombCount(Bombs);
            //                Tiles[i, j].SetBombColor(-1);
            //            }
            //        }
            //        for (int i = 0; i < MEDIUM_ROWS; i++)
            //        {
            //            for (int j = 0; j < MEDIUM_COLUMN; j++)
            //            {
            //                Tiles[i, j].SetAdj(Tiles);
            //            }
            //        }
            //        break;
            //    case HARD:
            //        for (int i = 0; i < HARD_ROWS; i++)
            //        {
            //            for (int j = 0; j < HARD_COLUMN; j++)
            //            {
            //                Tiles[i, j] = new Tile(j * HARD_TILE_SIZE, i * HARD_TILE_SIZE + HUD_HEIGHT, i, j);
            //                Tiles[i, j].BombCount(Bombs);
            //                Tiles[i, j].SetBombColor(-1);
            //            }
            //        }
            //        for (int i = 0; i < HARD_ROWS; i++)
            //        {
            //            for (int j = 0; j < HARD_COLUMN; j++)
            //            {
            //                Tiles[i, j].SetAdj(Tiles);
            //            }
            //        }
            //        break;
            //}
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

                        explodePos = new Vector2(Tiles[k, l].GetX(), Tiles[k, l].GetY());

                        explodeAnim = new Animation(mineExplode,               //The sprite sheet image
                                         5,                         //The number of frames wide the sprite sheet is
                                         5,                         //The number of frames high the sprite sheet is
                                         33,                        //The total number of frames in the animation
                                         0,                         //The starting frame number to draw first
                                         0,                         //The frame number to draw when the animation is not drawing, Animation.NO_IDLE will prevent drawing
                                         3,                         //The repetition option, this can be infinite, 1 or any other option other than 0 or a negative value
                                         2,                         //The number of times to repeat the same frame before the frame is changed, for smoothness purposes
                                         explodePos,               //The beginning draw location
                                         0.5f,                      //The scaling amount of the frame
                                         true);                     //Whether to begin animating immediately or not

                        explodeAnim.isAnimating = true;

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
                    if (mousePosition.X > Tiles[i, j].GetX() && mousePosition.X < Tiles[i, j].GetX() + tileSize && mousePosition.Y > Tiles[i, j].GetY() && mousePosition.Y < Tiles[i, j].GetY() + tileSize && Tiles[i, j].GetState() == HIDDEN)
                    {
                        if (Tiles[i, j].BombCount(Bombs) > 0 && Tiles[i, j].IsBomb(Bombs) == false)
                        {
                            Game1.smallClear.CreateInstance().Play();
                        }
                        else if (Tiles[i, j].BombCount(Bombs) == 0 && Tiles[i, j].IsBomb(Bombs) == false)
                        {
                            Game1.bigClear.CreateInstance().Play();
                        }

                        Tiles[i, j].RevealTiles();

                        if (Tiles[i, j].IsBomb(Bombs) == true)
                        {
                            mine.CreateInstance().Play();



                            //explodePos = new Vector2(Tiles[i, j].GetX(), Tiles[i, j].GetY());

                            //explodeAnim = new Animation(mineExplode,               //The sprite sheet image
                            //                 5,                         //The number of frames wide the sprite sheet is
                            //                 5,                         //The number of frames high the sprite sheet is
                            //                 23,                        //The total number of frames in the animation
                            //                 0,                         //The starting frame number to draw first
                            //                 0,                         //The frame number to draw when the animation is not drawing, Animation.NO_IDLE will prevent drawing
                            //                 1,                         //The repetition option, this can be infinite, 1 or any other option other than 0 or a negative value
                            //                 1,                         //The number of times to repeat the same frame before the frame is changed, for smoothness purposes
                            //                 explodePos,               //The beginning draw location
                            //                 0.5f,                      //The scaling amount of the frame
                            //                 true);                     //Whether to begin animating immediately or not

                            //explodeAnim.isAnimating = true;

                            //explodeAnim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);







                            ShowAllBomb(row, column);

                            //gameState = LOSE;
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
                            placeFlag.CreateInstance().Play();

                            Tiles[i, j].SetState(FLAG);
                            flagNum--;
                        }
                        else if (Tiles[i, j].GetState() == FLAG)
                        {
                            clearFlag.CreateInstance().Play();

                            Tiles[i, j].SetState(HIDDEN);
                            flagNum++;
                        }
                    }
                }
            }
        }

        private static void WriteFile()
        {
            try
            {
                fileWriter = File.CreateText(resultFile);
                fileWriter.WriteLine(Convert.ToString(highTime));
                fileWriter.Write(Convert.ToString(gameDiff));
            }
            //Catch exceptions and errors
            catch (FormatException fe)
            {
                //Catch exceptions and errors
                Console.WriteLine(fe.Message);
            }
            catch (FileNotFoundException fnf)
            {
                //Catch exceptions and errors
                Console.WriteLine(fnf.Message);
            }
            catch (Exception e)
            {
                //Catch exceptions and errors
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Close the file
                if (fileWriter != null)
                    fileWriter.Close();
            }

        }

        private static void ReadFile(string fileName)
        {
            try
            {
                //Don't run this method if the file does not exist
                if (File.Exists(fileName) == false)
                    return;

                //Open the file
                fileReader = File.OpenText(fileName);

                while (!fileReader.EndOfStream)
                {
                    int highTimeFile = Convert.ToInt32(fileReader.ReadLine());
                    int gameDiffFile = Convert.ToInt32(fileReader.ReadLine());

                    if (highTimeFile != 0 && highTimeFile != 1000)
                    {
                        highTime = highTimeFile;
                    }

                    gameDiff = gameDiffFile;
                }
            }
            //Catch exceptions and errors
            catch (FormatException fe)
            {
                //Display error message
                Console.WriteLine(fe.Message);
            }
            catch (FileNotFoundException fnf)
            {
                //Display error message
                Console.WriteLine(fnf.Message);
            }
            catch (Exception e)
            {
                //Display error message
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Close the file
                if (fileReader != null)
                    fileReader.Close();
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
