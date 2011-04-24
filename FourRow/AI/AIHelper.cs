using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.Util;
using FourRow.Game;

namespace FourRow.AI
{
    class AIHelper
    {

        static public List<Game.Tile> FindDirectWinningPossibilities(Game.Board board, Object player) {
            var allPotentialWinningTiles = new List<Game.Tile>();
            var placeable = board.GetAllPlaceableTiles();
            foreach (Tile t in placeable) {
                if (IsDirectlyWinnableOnTileForPlayer(board, t, player)) {
                    allPotentialWinningTiles.Add(t);
                }
            }
            return allPotentialWinningTiles;
        }

        static public bool IsDirectlyWinnableOnTileForPlayer(Game.Board board, Game.Tile tile, Object player) {
            //Get all the tile connections on this given tile.
            var connections = board.GetAllTileConnectionsForTile(tile.ColumnNo, tile.RowNo);

            //check each of them for a win, and return true if any of them is a winning connection
            var directlyWinnable = new List<Tile>();
            foreach (TileConnection tc in connections) {
                if (tc.WillTokenPlacedWinGameForPlayer(player,tile)) {
                    return true;
                }
            }

            return false;
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
            //Create a copy of the game to test with, ensuring that it is the player we are checking's turn
            var testGame = game.Copy(player);

            //Drop a token on the column
            testGame.DropTokenOnColumn(tile.Column.ColumnNo);

            //Check if the other player will compulsed to make a blocking move, which will be the case if
            //we have a winning move
            var winPossibilities = FindDirectWinningPossibilities(testGame.Board, player);

            //Check that we can make a winning move
            var won = false;
            winPossibilities.ForEach(winingTile => {
                var blockedGame = testGame.Copy();
                blockedGame.DropTokenOnColumn(winingTile.Column.ColumnNo); //simulate the opponent dropping a blocking tile
                var finishingMovePosibilities = FindDirectWinningPossibilities(blockedGame.Board, player);
                if (finishingMovePosibilities.Count > 0) {
                    //make sure that we don't give the game away though
                    var opponentWins = FindDirectWinningPossibilities(testGame.Board, game.GetOtherPlayer(player));
                    if (opponentWins.Count() == 0) {
                        won = true; //c# question: how do we return out of the parent function here?
                    }
                }
            });

            return won;
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

        static public List<TileConnection> FindFourTokenConnectionPossibilties(Game.Board board, Object player) {
            //Go through each player token in the board
            var playerTiles = GetPlayerTokenTiles(board, player);

            var potentialConnections = new List<TileConnection>();
            playerTiles.ForEach(tile => {
                potentialConnections.AddRange(
                    FindAllPossibleWinnableConnectionsForPlayerTile(board, tile));
            });

            potentialConnections = CleanDuplicatesFromTileCollectionList(potentialConnections);

            return potentialConnections;
        }

        static private List<Game.Tile> GetPlayerTokenTiles(Game.Board board, Object player) {
            var playerTiles = new List<Game.Tile>();
            board.Columns.ForEach(col => {
                col.Tiles.ForEach(tile => {
                    if (tile.OwningPlayer == player) {
                        playerTiles.Add(tile);
                    }
                });
            });
            return playerTiles;
        }


        //ToDo: ???TestME
        /// <summary>
        /// Method looks for all connections that include the given tile which would could possibly
        /// be used to win. So this doesn't include any opponent tiles, but will include empty
        /// and own tiles.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="tile"></param>
        static private List<TileConnection> FindAllPossibleWinnableConnectionsForPlayerTile(Game.Board board, Game.Tile tile) {
            var allTileConnections = new List<List<Game.Tile>> { 
                board.Columns[tile.ColumnNo].Tiles, //Check vertical
                board.GetRow(tile.RowNo), //Check horizontal
                board.GetPositiveDiagonalTilesFromStartingRow( //Check positive diagonal
                    tile.GetPositiveDiagonalStartingRowNo()),
                board.GetNegativeDiagonalTilesFromStartingRow( //Check negative diagonal
                    tile.GetNegativeDiagonalStartingRowNo())

            };

            var winnableConnections = new List<TileConnection>();
            allTileConnections.ForEach(tileSet => {
                var winnableConnection = (new TileConnection(tileSet)).GetWinnableConnectionAround(tile);
                if (winnableConnection != null) {
                    winnableConnections.Add(winnableConnection);
                }
                
            });
            return winnableConnections;
        }

        /// <summary>
        /// This method removes duplicates from tile connections. This includes connections
        /// which are a subset of another connection.
        /// </summary>
        /// <param name="tileConnections"></param>
        /// <returns></returns>
        static public List<TileConnection> CleanDuplicatesFromTileCollectionList(List<TileConnection> tileConnections) {
            var dupes = new HashSet<TileConnection>();
            
            //compare every tile connection against every other to find the duplicates. This is slow.
            for (var tci = 0; tci < tileConnections.Count; tci++) {
                var tc = tileConnections[tci];
                for (var tci2 = tci + 1; tci2 < tileConnections.Count; tci2++) {
                    var tc2 = tileConnections[tci2];
                    if (!dupes.Contains(tc2) && tc2.IsDuplicateOrSubsetOf(tc)) {
                        dupes.Add(tc2);

                    } else if (!dupes.Contains(tc) && tc.IsDuplicateOrSubsetOf(tc2)) {
                        //By using else if we ensure that we don't add both tileconnections
                        //if they are exact duplicates
                        dupes.Add(tc);
                    }
                }
            }

            //remove all the duplicates
            dupes.ToList().ForEach( dup => {
                tileConnections.Remove(dup);
            });

            return tileConnections;
        }

    }
}
