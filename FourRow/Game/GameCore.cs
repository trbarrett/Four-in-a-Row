using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourRow.Game
{

    //We should unit test this!
    public class GameCore
    {
        private Board _board;
        private Object _player1;
        private Object _player2;
        private Object _currentPlayer;

        private Tile _winningTile;
        private List<List<Tile>> _winningSets;
        private Object _winningPlayer;

        public event EventHandler GameStateChanged;

        public Board Board {
            get { return _board; }
        }

        public Object CurrentPlayer { get { return _currentPlayer; } }

        public Object WinningPlayer {
            get { return _winningPlayer; }
        }

        public GameCore(Object player1, Object player2) {
            _player1 = player1;
            _player2 = player2;
            _currentPlayer = _player1; //we should randomize this
            _board = new Board();
        }

        public bool IsGameFinished() {
            return _winningPlayer != null;
        }

        public List<Tile> GetWinningTiles() {
            HashSet<Tile> winningTiles = new HashSet<Tile>();

            IEnumerable<Tile> tiles =
                from set in _winningSets
                from tile in set
                select tile;

            //make sure we have a unique list
            IEnumerable<Tile> unique = tiles.TakeWhile(tile => {
                if (winningTiles.Contains(tile)) {
                    return false;
                } else {
                    winningTiles.Add(tile);
                    return true;
                }
            });

            return unique.ToList();
        }

        public GameCore Copy() {
            var copyGame = new GameCore(_player1, _player2);
            copyGame._currentPlayer = this._currentPlayer;

            Board.Columns.ForEach(column => {
                column.Tiles.ForEach(tile => {
                    copyGame.Board[tile.ColumnNo][tile.RowNo].OwningPlayer = tile.OwningPlayer;
                });
            });

            return copyGame;
        }

        public void DropTokenOnColumn(int columnNo) {
            DropTokenOnColumn(_board[columnNo]);
        }


        public void DropTokenOnColumn(Column column) {
            if (column.IsFull) {
                throw new ApplicationException(string.Format("Cannot drop token on column {0} because it is full!", column.ColumnNo));
            }

            Tile tokenTile = column.GetFirstEmptyTile();
            tokenTile.OwningPlayer = _currentPlayer;

            //To-Do: We should probably check for a win condition now
            List<List<Tile>> winningSets = CheckForWin(tokenTile);
            if (winningSets.Count != 0) {
                //We have a winner
                _winningTile = tokenTile;
                _winningSets = winningSets;
                _winningPlayer = _currentPlayer;
                _currentPlayer = null;

            } else {
                //after a token has been dropped we update the current player
                _currentPlayer = GetNonCurrentPlayer();
            }

            //and let any subscribers know that the game state has changed
            if (GameStateChanged != null) {GameStateChanged(this, new EventArgs());} 
        }

        public Object GetNonCurrentPlayer() {
            return (_currentPlayer == _player1) ? _player2 : _player1;
        }

        private List<List<Tile>> CheckForWin(Tile lastPlayedTile) {
            //Check in each of the four directions for four or more in a row.

            //To do this we could just map each directions into a linear list, and do the same check for each

           List<List<Tile>> winningSets = new List<List<Tile>>();

            //Check Horizontal
            CheckInlineTilesAndAddToWinningSetIfMatching(
                   GetRowTilesForTile(lastPlayedTile),
                   winningSets);

            //Check Vertical
            CheckInlineTilesAndAddToWinningSetIfMatching(
                   _board[lastPlayedTile.ColumnNo].Tiles,
                   winningSets);

            //Check Positive Diagonal
            CheckInlineTilesAndAddToWinningSetIfMatching(
                   GetPoistiveDiagonalTilesForTile(lastPlayedTile),
                   winningSets);

            //Check Negative Diagonal
            CheckInlineTilesAndAddToWinningSetIfMatching(
                   GetNegativeDiagonalTilesForTile(lastPlayedTile),
                   winningSets);

            return winningSets;
        }

        private void CheckInlineTilesAndAddToWinningSetIfMatching(List<Tile> inlineTiles, List<List<Tile>> winningSets) {
            List<Tile> winningTiles = GetWinningTilesFromTileList(inlineTiles);
            if (winningTiles != null) {
                winningSets.Add(winningTiles);
            }
        }

        private List<Tile> GetWinningTilesFromTileList(List<Tile> inlineTiles) {
            //We are looking for four or more in a row
            List<Tile> winningTiles = new List<Tile>();
            bool foundWinning = false;

            foreach (Tile tile in inlineTiles) {
                //Always add the first non-empty tile we come across
                if (winningTiles.Count == 0 && tile.OwningPlayer != null) {
                    winningTiles.Add(tile);
                } else {
                    if (tile.OwningPlayer == null) {
                        if (foundWinning) { return winningTiles; } //we have a winning connection already, so return it.
                        winningTiles.Clear();

                    } else if (tile.OwningPlayer != winningTiles.Last().OwningPlayer) {
                        if (foundWinning) { return winningTiles; } //we have a winning connection already, so return it.
                        winningTiles.Clear();
                        winningTiles.Add(tile); //We've changed owner
                    } else {
                        //The owner has continued
                        winningTiles.Add(tile);
                        if (winningTiles.Count >= 4) {
                            //once we've got more than 4 we know we have a winning connection. 
                            foundWinning = true; 
                        }
                    }
                }
            }

            if (foundWinning) { //this case happens if the last tile is part of a winning connection
                return winningTiles;
            } else {
                return null;
            }
        }

        private List<Tile> GetRowTilesForTile(Tile tile) {
            int rowNo = tile.RowNo;
            var tiles = new List<Tile>();
            for (int columnNo = 0; columnNo < Board.BoardWidth; columnNo++) {
                tiles.Add(_board[columnNo][rowNo]);
            }
            return tiles;
        }

        private List<Tile> GetPoistiveDiagonalTilesForTile(Tile tile) {
            int rowNo = tile.RowNo;
            int columnNo = tile.ColumnNo;

            //work out the start point. Farthest bottom left point diagonally inline with the tile
            while (columnNo > 0 && rowNo > 0) {
                columnNo--;
                rowNo--;
            }

            //move diagonally up and rightwatds grabbing each tile
            var tiles = new List<Tile>();
            while ( columnNo < Board.BoardWidth && rowNo < Board.BoardHeight) {
                tiles.Add(_board[columnNo][rowNo]);
                columnNo++;
                rowNo++;
            }
            return tiles;
        }

        private List<Tile> GetNegativeDiagonalTilesForTile(Tile tile) {
            int rowNo = tile.RowNo;
            int columnNo = tile.ColumnNo;

            //work out the start point. Farthest top left point diagonally inline with the tile
            while (columnNo > 0 && rowNo < Board.BoardHeight - 1) {
                columnNo--;
                rowNo++;
            }

            //move diagonally down and rightwatds grabbing each tile
            var tiles = new List<Tile>();
            while (columnNo < Board.BoardWidth && rowNo >= 0) {
                tiles.Add(_board[columnNo][rowNo]);
                columnNo++;
                rowNo--;
            }
            return tiles;
        }

    }

}
