using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YangA_MP2
{
    public class Tile
    {

        private int state;
        private int x;
        private int y;
        private bool isChecked = false;
        private int row;
        private int column;

        List<Tile> adjescantTiles = new List<Tile> { };

        public Tile(int x, int y, int row, int column)
        {
            this.row = row;
            this.column = column;
            this.x = x;
            this.y = y;
        }

        public int GetRow()
        {
            return row;
        }

        public int GetColumn()
        {
            return column;
        }

        public bool IsBomb(List<int> bombs)
        {
            switch (Game1.gameDiff)
            {
                case Game1.EASY:
                    for (int i = 0; i < bombs.Count; i++)
                    {
                        if (bombs[i] == (column + Game1.EASY_COLUMN * row))
                        {
                            return true;
                        }
                    }
                    break;
                case Game1.MEDIUM:
                    for (int i = 0; i < bombs.Count; i++)
                    {
                        if (bombs[i] == (x + Game1.MEDIUM_COLUMN * y))
                        {
                            return true;
                        }
                    }
                    break;
                case Game1.HARD:
                    for (int i = 0; i < bombs.Count; i++)
                    {
                        if (bombs[i] == (x + Game1.HARD_COLUMN * y))
                        {
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public int GetState()
        {
            return state;
        }

        public void SetState(int state)
        {
            this.state = state;
        }

        public bool GetChecked()
        {
            return isChecked;
        }

        public void SetChecked(bool check)
        {
            isChecked = check;
        }

        public void SetAdj(Tile[,] tiles)
        {
            if (adjescantTiles.Count == 0)
            {
                adjescantTiles.Add(GetTile(tiles, row, column - 1));
                adjescantTiles.Add(GetTile(tiles, row, column + 1));

                adjescantTiles.Add(GetTile(tiles, row - 1, column - 1));
                adjescantTiles.Add(GetTile(tiles, row - 1, column));
                adjescantTiles.Add(GetTile(tiles, row - 1, column + 1));

                adjescantTiles.Add(GetTile(tiles, row + 1, column - 1));
                adjescantTiles.Add(GetTile(tiles, row + 1, column));
                adjescantTiles.Add(GetTile(tiles, row + 1, column + 1));
            }
        }

        public List<Tile> GetAdj()
        {
            return adjescantTiles;
        }

        public int BombCount(List<int> bombs)
        {
            int bombCount = 0;

            for (int i = 0; i < adjescantTiles.Count; i++)
            {
                if (adjescantTiles[i] != null && adjescantTiles[i].IsBomb(bombs) == true)
                {
                    bombCount++;
                }
            }
            return bombCount;
        }

        private Tile GetTile(Tile[,] tiles, int row, int column)
        {
            Tile tile = null;

            switch (Game1.gameDiff)
            {
                case Game1.EASY:
                    if (column >= 0 && column < Game1.EASY_COLUMN && row >= 0 && row < Game1.EASY_ROWS)
                    {
                        tile = tiles[row, column];
                    }
                    break;
                case Game1.MEDIUM:
                    if (column >= 0 && column < Game1.MEDIUM_COLUMN && row >= 0 && row < Game1.MEDIUM_ROWS)
                    {
                        tile = tiles[row, column];
                    }
                    break;
                case Game1.HARD:
                    if (column >= 0 && column < Game1.HARD_COLUMN && row >= 0 && row < Game1.HARD_ROWS)
                    {
                        tile = tiles[row, column];
                    }
                    break;
            }
            return tile;
        }

        public void RevealTiles()
        {
            if (IsBomb(Game1.Bombs) == false && GetChecked() == false && BombCount(Game1.Bombs) == 0)
            {
                //to do: reveal tile
                SetState(Game1.REVEALED);
                SetChecked(true);

                for (int i = 0; i < adjescantTiles.Count; i++)
                {
                    if ((adjescantTiles[i] != null) )
                    {
                        adjescantTiles[i].RevealTiles();
                    }
                }
            }
            else if (IsBomb(Game1.Bombs) == false && GetChecked() == false)
            {
                SetState(Game1.REVEALED);
                SetChecked(true);
            }
            else if (IsBomb(Game1.Bombs) == true && GetChecked() == false)
            {
                SetState(Game1.BOMB);
                SetChecked(true);
            }
        }
    }
}
