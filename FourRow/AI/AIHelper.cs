using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourRow.AI
{
    class AIHelper
    {

        static public List<Game.Tile> FindDirectWinningPossibilities(Game.Board board, Object player) {
            //Get a list of each of the possibilities
            List<List<Game.Tile>> tileConnections = board.GetAllTileConnections();

            var allPotentialWinningTiles = new List<Game.Tile>();
            tileConnections.ForEach(tileConnection => {
                var potentialWinningTiles = CheckForPotentialWinningTiles(board, player, tileConnection);
                allPotentialWinningTiles.AddRange(potentialWinningTiles);
            });

            return allPotentialWinningTiles;
        }

        static public List<Game.Tile> FindTwoTurnCompulsionWin(
                Game.GameCore game,
                Object player) {

            //Look through the playable tiles for each column. 
            //Consider the case where if it was filled, the player would be forced to make
            //a follow-up move that would allow us to make a winning followup move.

            var twoTurnCompulsionWinTiles = new List<Game.Tile>();
            game.Board.Columns.ForEach(column => {
                if (!column.IsFull) {
                    if (CheckTwoTurnCompulsionWinForTile(game, player, column.GetFirstEmptyTile())) {
                        twoTurnCompulsionWinTiles.Add(column.GetFirstEmptyTile());
                    }
                }
            });
            return twoTurnCompulsionWinTiles;
        }

        static private bool CheckTwoTurnCompulsionWinForTile(Game.GameCore game, Object player, Game.Tile tile) {
            //Create a copy of the game to test with
            var testGame = game.Copy();

            //Drop a token on the column
            game.DropTokenOnColumn(tile.Column);

            //Check if the other player will compulsed to make a blocking move, which will be the case if
            //we have a winning move
            var winPossibilities = FindDirectWinningPossibilities(game.Board, player);

            //Check that we can make a winning move
            var won = false;
            winPossibilities.ForEach(winingTile => {
                var blockedGame = game.Copy();
                blockedGame.DropTokenOnColumn(tile.Column); //simulate the opponent dropping a blocking tile
                var finishingMovePosibilities = FindDirectWinningPossibilities(blockedGame.Board, player);
                if (finishingMovePosibilities.Count > 0) {
                    won = true; //how do we return out of the parent function here?
                }
            });

            return won;
        }


        static public List<Game.Tile> CheckForPotentialWinningTiles(
                Game.Board board, 
                Object player, 
                List<Game.Tile> tileConnection) {

            var potentialWinningTiles = new List<Game.Tile>();
            //step throught the connection, looking for a placable empty tile with three player tiles adjacent
            tileConnection.ForEach(tile => {
                if (IsTileImmediatleyPlacable(board, tile)) {
                    if (DoesTileHaveThreeDirectlyConnectingPlayerTokens(player, tileConnection, tile)) {
                        potentialWinningTiles.Add(tile);
                    }
                }
            });

            return potentialWinningTiles;
        }

        static public bool DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                Object player,
                List<Game.Tile> tileConnection,
                Game.Tile tile) {

            int directlyConnectingCount = 0;
            
            //Count Backwards
            int currentIndex = tileConnection.IndexOf(tile) - 1;
            while (currentIndex >= 0 && tileConnection[currentIndex].OwningPlayer == player) {
                directlyConnectingCount++;
                currentIndex--;
            }
         
            //Count Forwards
            currentIndex = tileConnection.IndexOf(tile) + 1;
            while (currentIndex < tileConnection.Count && tileConnection[currentIndex].OwningPlayer == player) {
                directlyConnectingCount++;
                currentIndex++;
            }

            return directlyConnectingCount >= 3;
        }

        /// <summary>
        /// An immediatley placable tile is a tile that would be filled by the next token dropped on that column
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        static public bool IsTileImmediatleyPlacable(Game.Board board, Game.Tile tile) {
            if (tile.OwningPlayer != null) {
                //if there is already a token on the tile then it can't be placeable
                return false;
            } else if (tile.RowNo == 0) {
                //if we are on the bottom row, then we must be placable
                return true;
            } else if (board[tile.ColumnNo][tile.RowNo - 1].OwningPlayer != null) {
                //else if the tile below us is filled, then we are placable
                return true;
            } else {
                //otherwise we are not
                return false;
            }
        }



    }
}
