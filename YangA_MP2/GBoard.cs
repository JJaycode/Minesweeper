using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace YangA_MP2
{
    internal class GBoard
    {
        //const int HIDDEN = 0;
        //const int REVEALED = 1;
        //const int BOMB = 2;
        //const int FLAG = 3;

        internal int Row;
        internal int Col;
        internal int MineCount;
        internal int TileSize;

        internal Tile[,] Tiles;
        internal List<int> Bombs;

        int flagNum;

        private Random rnd = new Random();

        internal Texture2D bkBoard;
        internal Rectangle boardRect;

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

        SpriteFont gameFont;

        MouseState currentMouseState;
        MouseState lastMouseState;


        internal GBoard(int row, int col, int minecnt, int tileSize, Texture2D bkBrd, Rectangle bRec)
        {
            this.Row = row;
            this.Col = col;
            this.MineCount = minecnt;
            this.TileSize = tileSize;
            this.bkBoard = bkBrd;
            this.boardRect = bRec;
            this.flagNum = minecnt;

            //this.Bombs = new List<int>;
            //this.Tiles = new Tile[col, row](); 
        }

        //private void InitItems()
        //{
        //    Texture2D clock;
        //    Rectangle clockRect;

        //    Texture2D flag;
        //    Rectangle flagRect;

        //    Texture2D hud;
        //    Rectangle hudRect;

        //    Texture2D soundOn;
        //    Rectangle onRect;

        //    Texture2D soundOff;
        //    Rectangle offRect;

        //    Texture2D clearLight;
        //    Rectangle clearLightRect;

        //    Texture2D clearDark;
        //    Rectangle clearDarkRect;

        //    Texture2D one;
        //    Rectangle oneRect;

        //    Texture2D two;
        //    Rectangle twoRect;

        //    Texture2D three;
        //    Rectangle threeRect;

        //    Texture2D four;
        //    Rectangle fourRect;

        //    Texture2D five;
        //    Rectangle fiveRect;

        //    Texture2D six;
        //    Rectangle sixRect;

        //    Texture2D seven;
        //    Rectangle sevenRect;

        //    Texture2D eight;
        //    Rectangle eightRect;

        //    Texture2D mine1;
        //    Rectangle mine1Rect;

        //    Texture2D mine2;
        //    Rectangle mine2Rect;

        //    Texture2D mine3;
        //    Rectangle mine3Rect;

        //    Texture2D mine4;
        //    Rectangle mine4Rect;

        //    Texture2D mine5;
        //    Rectangle mine5Rect;

        //    Texture2D mine6;
        //    Rectangle mine6Rect;

        //    Texture2D mine7;
        //    Rectangle mine7Rect;

        //    Texture2D mine8;
        //    Rectangle mine8Rect;
        //}

        internal void LoadContent(SpriteBatch SpriteBatch, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            //easyBoard = Content.Load<Texture2D>("Images/Backgrounds/board_easy");
            //easyBoardRect = new Rectangle(0, HUD_HEIGHT, easyBoard.Width, easyBoard.Height);
            //medBoard = Content.Load<Texture2D>("Images/Sprites/board_med");
            //medBoardRect = new Rectangle(0, HUD_HEIGHT, medBoard.Width, medBoard.Height);
            //hardBoard = Content.Load<Texture2D>("Images/Sprites/board_hard");
            //hardBoardRect = new Rectangle(0, HUD_HEIGHT, hardBoard.Width, hardBoard.Height);

            clock = Content.Load<Texture2D>("Images/Sprites/Watch");
            flag = Content.Load<Texture2D>("Images/Sprites/flag");
            hud = Content.Load<Texture2D>("Images/Sprites/HUDBar");
            soundOn = Content.Load<Texture2D>("Images/Sprites/SoundOn");
            soundOff = Content.Load<Texture2D>("Images/Sprites/SoundOff");

            //soundEffects.

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

            //instTimer = new Timer(Timer.INFINITE_TIMER, true);
        }

        private void ResetBombs()
        {
            if (Bombs == null)
                Bombs = new List<int>();
            else
                Bombs.Clear();

            int bombLocation;
            Random randomLocation = new Random();

            while (Bombs.Count < this.MineCount)
            {
                bombLocation = randomLocation.Next(0, this.Col * this.Row + 1);

                if (!Bombs.Contains(bombLocation))
                {
                    Bombs.Add(bombLocation);
                }
            }
        }

        private void ResetTiles()
        {
            Tiles = new Tile[this.Row, this.Col];

            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    Tiles[i, j] = new Tile(j * this.TileSize, i * this.TileSize + Game1.HUD_HEIGHT, i, j);
                    Tiles[i, j].BombCount(Bombs);
                    Tiles[i, j].SetBombColor(-1);
                }
            }
            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    Tiles[i, j].SetAdj(Tiles);
                }
            }
        }

        internal void Reset()
        {
            ResetBombs();
            ResetTiles();
        }

        private bool CheckWin()
        {
            int count = 0;

            for (int k = 0; k < this.Row; k++)
            {
                for (int l = 0; l < this.Col; l++)
                {
                    if (Tiles[k, l].IsBomb(Bombs) == false)
                    {
                        if (Tiles[k, l].GetState() == Game1.REVEALED)
                        {
                            count++;
                        }
                    }
                }
            }

            if (count == this.Row * this.Col - this.MineCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SetFlag(Point mousePosition)
        {
            bool hasSetAFlag = false;  

            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    if ((mousePosition.X > Tiles[i, j].GetX()) && (mousePosition.X < Tiles[i, j].GetX() + this.TileSize)
                        && (mousePosition.Y > Tiles[i, j].GetY()) && (mousePosition.Y < Tiles[i, j].GetY() + this.TileSize))
                    {
                        if (Tiles[i, j].GetState() != Game1.FLAG && Tiles[i, j].GetState() == Game1.HIDDEN)
                        {
                            Tiles[i, j].SetState(Game1.FLAG);
                            hasSetAFlag = true; //total flag count will decrease 1 in the screen
                        }
                        else if (Tiles[i, j].GetState() == Game1.FLAG)
                        {
                            Tiles[i, j].SetState(Game1.HIDDEN);
                            hasSetAFlag = true; //total flag count will increase 1 in the screen
                        }
                    }
                }
            }

            return hasSetAFlag;
        }

        private void ShowAllBomb()
        {
            for (int k = 0; k < this.Row; k++)
            {
                for (int l = 0; l < this.Col; l++)
                {
                    if (Tiles[k, l].IsBomb(Bombs) == true)
                    {
                        Tiles[k, l].SetState(Game1.BOMB);
                    }
                }
            }
        }

        private int RevealTiles(Point mousePosition)
        {
            int gameState = Game1.GAMEPLAY;

            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    if (mousePosition.X > Tiles[i, j].GetX() && mousePosition.X < Tiles[i, j].GetX() + this.TileSize 
                        && mousePosition.Y > Tiles[i, j].GetY() && mousePosition.Y < Tiles[i, j].GetY() + this.TileSize)
                    {
                        if (Tiles[i, j].IsBomb(Bombs) == true)
                        {
                            ShowAllBomb();

                            //instTimer.ResetTimer(true);

                            //double timePassed = instTimer.GetTimePassed();
                            ////int secPassed = 0;

                            //if (timePassed >= 2000)
                            {
                                gameState = Game1.LOSE;
                            }
                        }
                        else
                        {
                            Tiles[i, j].RevealTiles(this.Bombs);
                        }
                    }
                }
            }

            return gameState;
        }

        internal int Update(Point mousePosition, MouseState currentMouseState, MouseState lastMouseState)
        {
            int gameState = Game1.GAMEPLAY;

            if (CheckWin() == true)
            {
                gameState = Game1.WIN;
            }

            if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
            {
                bool flag = SetFlag(mousePosition);

                if (flag)
                    flagNum--;
                else
                    flagNum++; 
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                if (mousePosition.X > Game1.DIFF_BUTTON_LOC && mousePosition.Y > Game1.DIFF_BUTTON_LOC 
                    && mousePosition.X < Game1.DIFF_BUTTON_LOC + Game1.DIFF_BUTTON_LENGTH && mousePosition.Y < Game1.DIFF_BUTTON_LOC + Game1.HUD_SPRITE_HEIGHT)
                {
                    gameState = Game1.SWITCH;
                }
                else
                {
                    gameState = RevealTiles(mousePosition);
                }
            }

            return gameState; 
        }

        internal void DrawBoard(SpriteBatch spriteBatch, Timer gameTimer)
        {
            offRect = new Rectangle(this.Col * this.TileSize - 85, 12, 30, Game1.HUD_SPRITE_HEIGHT);
            onRect = new Rectangle(this.Col * this.TileSize - 85, 12, 30, Game1.HUD_SPRITE_HEIGHT);
            clockRect = new Rectangle(this.Col * this.TileSize - 230, 12, 30, Game1.HUD_SPRITE_HEIGHT);
            flagRect = new Rectangle(this.Col * this.TileSize - 310, 12, 35, Game1.HUD_SPRITE_HEIGHT);
            hudRect = new Rectangle(0, 0, this.bkBoard.Width, hud.Height);

            spriteBatch.Draw(this.bkBoard, this.boardRect, Color.White);
            spriteBatch.Draw(hud, hudRect, Color.White);
            spriteBatch.Draw(clock, clockRect, Color.White);
            spriteBatch.Draw(flag, flagRect, Color.White);
            spriteBatch.Draw(soundOn, onRect, Color.White);
            spriteBatch.Draw(soundOff, offRect, Color.White);

            Rectangle easyButtonRect = new Rectangle(Game1.DIFF_BUTTON_LOC, Game1.DIFF_BUTTON_LOC, Game1.DIFF_BUTTON_LENGTH, Game1.HUD_SPRITE_HEIGHT);
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

            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    if (Tiles[i, j].GetState() == Game1.FLAG)
                    {
                        Rectangle flagTileRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                        spriteBatch.Draw(flag, flagTileRect, Color.White);
                    }

                    if (Tiles[i, j].GetState() == Game1.REVEALED)
                    {
                        //Different places are different sprites on board
                        if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 == 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 != 0)
                        {
                            Rectangle clearLightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                            spriteBatch.Draw(clearLight, clearLightRect, Color.White);
                        }
                        else if (Tiles[i, j].GetColumn() % 2 == 0 && Tiles[i, j].GetRow() % 2 != 0 || Tiles[i, j].GetColumn() % 2 != 0 && Tiles[i, j].GetRow() % 2 == 0)
                        {
                            Rectangle clearDarkRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                            spriteBatch.Draw(clearDark, clearDarkRect, Color.White);
                        }

                        switch (Tiles[i, j].BombCount(Bombs))
                        {
                            case 1:
                                Rectangle oneRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(one, oneRect, Color.White);
                                break;
                            case 2:
                                Rectangle twoRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(two, twoRect, Color.White);
                                break;
                            case 3:
                                Rectangle threeRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(three, threeRect, Color.White);
                                break;
                            case 4:
                                Rectangle fourRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(four, fourRect, Color.White);
                                break;
                            case 5:
                                Rectangle fiveRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(five, fiveRect, Color.White);
                                break;
                            case 6:
                                Rectangle sixRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(six, sixRect, Color.White);
                                break;
                            case 7:
                                Rectangle sevenRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(seven, sevenRect, Color.White);
                                break;
                            case 8:
                                Rectangle eightRect = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
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

                    if (Tiles[i, j].GetState() == Game1.BOMB)
                    {
                        switch (Tiles[i, j].GetBombColor())
                        {
                            case 1:
                                Rectangle mine1Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine1, mine1Rec, Color.White);
                                break;
                            case 2:
                                Rectangle mine2Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine2, mine2Rec, Color.White);
                                break;
                            case 3:
                                Rectangle mine3Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine3, mine3Rec, Color.White);
                                break;
                            case 4:
                                Rectangle mine4Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine4, mine4Rec, Color.White);
                                break;
                            case 5:
                                Rectangle mine5Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine5, mine5Rec, Color.White);
                                break;
                            case 6:
                                Rectangle mine6Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine6, mine6Rec, Color.White);
                                break;
                            case 7:
                                Rectangle mine7Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine7, mine7Rec, Color.White);
                                break;
                            case 8:
                                Rectangle mine8Rec = new Rectangle(Tiles[i, j].GetX(), Tiles[i, j].GetY(), this.TileSize, this.TileSize);
                                spriteBatch.Draw(mine8, mine8Rec, Color.White);
                                break;
                        }
                    }
                }
            }

        }



    }
}
