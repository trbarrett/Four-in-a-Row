using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.Game;
using FourRow.AI;
using NUnit.Framework;
using FourRow.Util;

namespace FourRow.Tests.Unit.AI
{
    [TestFixture]
    class AIHelperTest
    {

        [Test]
        public void TestFindDirectWinningPossibilities_OnHorizontalAtCorner() {
            //setup the initial condition
            var player1 = new Object();
            var player2 = new Object();
            Board board = new Board();

            board[0][0].OwningPlayer = player1;
            board[1][0].OwningPlayer = player1;
            board[2][0].OwningPlayer = player1;

            var winningPossibilites = AIHelper.FindDirectWinningPossibilities(board, player1);

            Assert.AreEqual(board[3][0], winningPossibilites[0]);
            Assert.AreEqual(1, winningPossibilites.Count);
        }

        [Test]
        public void TestFindDirectWinningPossibilities_OnHorizontalAtMiddleBottom() {
            //setup the initial condition
            var player1 = new Object();
            var player2 = new Object();
            Board board = new Board();

            board[1][0].OwningPlayer = player1;
            board[2][0].OwningPlayer = player1;
            board[3][0].OwningPlayer = player1;

            var winningPossibilites = AIHelper.FindDirectWinningPossibilities(board, player1);

            Assert.AreEqual(2, winningPossibilites.Count);
            Assert.AreEqual(board[0][0], winningPossibilites[0]);
            Assert.AreEqual(board[4][0], winningPossibilites[1]);
        }

        [Test]
        public void TestFindDirectWinningPossibilities_OnVertical() {
            //setup the initial condition
            var player1 = new Object();
            var player2 = new Object();
            Board board = new Board();

            board[4][0].OwningPlayer = player1;
            board[4][1].OwningPlayer = player1;
            board[4][2].OwningPlayer = player1;

            var winningPossibilites = AIHelper.FindDirectWinningPossibilities(board, player1);

            Assert.AreEqual(1, winningPossibilites.Count);
            Assert.AreEqual(board[4][3], winningPossibilites[0]);
        }

        [Test]
        public void TestFindDirectWinningPossibilities_OnHorizontalAtMiddleRaised()
        {
            //setup the initial condition
            var player1 = new Object();
            var player2 = new Object();
            Board board = new Board();

            board[1][0].OwningPlayer = player2;
            board[1][1].OwningPlayer = player2;
            board[1][2].OwningPlayer = player1;

            board[2][0].OwningPlayer = player2;
            board[2][1].OwningPlayer = player2;
            board[2][2].OwningPlayer = player1;

            board[3][0].OwningPlayer = player2;
            board[3][1].OwningPlayer = player2;
            board[3][2].OwningPlayer = player1;

            board[4][0].OwningPlayer = player2;
            board[4][1].OwningPlayer = player2;

            var winningPossibilites = AIHelper.FindDirectWinningPossibilities(board, player1);

            Assert.AreEqual(1, winningPossibilites.Count);
            Assert.AreEqual(board[4][2], winningPossibilites[0]); //horizontal across the middle
        }

        [Test]
        public void TestFindDirectWinningPossibilities_MultiplePossibilties()
        {
            //setup the initial condition
            var player1 = new Object();
            var player2 = new Object();
            Board board = new Board();

            board[1][0].OwningPlayer = player2;
            board[1][1].OwningPlayer = player2;
            board[1][2].OwningPlayer = player1;

            board[2][0].OwningPlayer = player2;
            board[2][1].OwningPlayer = player2;
            board[2][2].OwningPlayer = player1;

            board[3][0].OwningPlayer = player1;
            board[3][1].OwningPlayer = player1;
            board[3][2].OwningPlayer = player1;

            board[4][0].OwningPlayer = player1;
            board[4][1].OwningPlayer = player1;

            var winningPossibilites = AIHelper.FindDirectWinningPossibilities(board, player1);

            Assert.AreEqual(board[1][3], winningPossibilites[0]); //negative diagonal
            Assert.AreEqual(board[3][3], winningPossibilites[1]); //there's a vertical here as well
            Assert.AreEqual(board[4][2], winningPossibilites[2]); //horizontal across the middle
            Assert.AreEqual(3, winningPossibilites.Count);
        }

        [Test]
        public void TestFindDirectWinningPossibilities_OnDiagonal() {
            //setup the initial condition
            var player1 = new Object();
            var player2 = new Object();
            Board board = new Board();

            board[2][0].OwningPlayer = player2;
            board[2][1].OwningPlayer = player1;

            board[3][0].OwningPlayer = player2;
            board[3][1].OwningPlayer = player2;
            board[3][2].OwningPlayer = player1;

            board[4][0].OwningPlayer = player2;
            board[4][1].OwningPlayer = player2;
            board[4][2].OwningPlayer = player2;
            board[4][3].OwningPlayer = player1;

            var winningPossibilites = AIHelper.FindDirectWinningPossibilities(board, player1);

            Assert.AreEqual(1, winningPossibilites.Count);
            Assert.AreEqual(board[1][0], winningPossibilites[0]);

            //Note that 5,4 wsn't a possibility, because there are no tiles underneath it. If 
            //we put them in it should be though

            board[5][0].OwningPlayer = player1;
            board[5][1].OwningPlayer = player2;
            board[5][2].OwningPlayer = player2;
            board[5][3].OwningPlayer = player1;

            winningPossibilites = AIHelper.FindDirectWinningPossibilities(board, player1);

            Assert.AreEqual(2, winningPossibilites.Count);
            Assert.AreEqual(board[1][0], winningPossibilites[0]);
            Assert.AreEqual(board[5][4], winningPossibilites[1]);
        }

        [Test]
        public void TestFindTwoTurnCompultionWinForTile1() {
            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2);
            var board = game.Board;

            // . . . . . x
            // . . . . . t
            // . . x x . x
            // . . x t . t .
            ////////////////////

            board[2][0].OwningPlayer = player1;
            board[2][1].OwningPlayer = player1;

            board[3][0].OwningPlayer = player2;
            board[3][1].OwningPlayer = player1;

            //column 4 is empty, with which we can create a compulsion.
            //If player one places a tile in column four, it will cause a potential
            //win if they place another one (4 horizontally) so player 2 will have to 
            //block it. But that will let player 1 place a tile there to win
            //via a diagonal.

            board[5][0].OwningPlayer = player2;
            board[5][1].OwningPlayer = player1;
            board[5][2].OwningPlayer = player2;
            board[5][3].OwningPlayer = player1;

            List<Tile> compulsionWin = AIHelper.FindTwoTurnCompulsionWin(game, player1);

            Assert.AreEqual(1, compulsionWin.Count);
            Assert.AreEqual(board[4][0], compulsionWin[0]);
        }

        [Test]
        public void TestFindTwoTurnCompultionWinForTile2() {
            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2);
            var board = game.Board;

            //
            //
            //
            // . . x x . . .
            //////////////////

            board[2][0].OwningPlayer = player1;
            board[3][0].OwningPlayer = player1;

            List<Tile> compulsionWin = AIHelper.FindTwoTurnCompulsionWin(game, player1);

            Assert.AreEqual(2, compulsionWin.Count);
            Assert.AreEqual(board[1][0], compulsionWin[0]);
            Assert.AreEqual(board[4][0], compulsionWin[1]);
        }

        [Test]
        public void TestFindTwoTurnCompultionWinForTile3() {
            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2);
            var board = game.Board;

            //
            //
            //
            // . . x . x . . .
            //////////////////

            board[2][0].OwningPlayer = player1;
            board[4][0].OwningPlayer = player1;

            List<Tile> compulsionWin = AIHelper.FindTwoTurnCompulsionWin(game, player1);

            Assert.AreEqual(1, compulsionWin.Count);
            Assert.AreEqual(board[3][0], compulsionWin[0]);
        }

        [Test]
        public void TestFindTwoTurnCompultionWinForTile4() {
            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2);
            var board = game.Board;

            // x x x
            // t t t
            // t t t
            // t t t x
            // t t t x . . .
            /////////////////

            board[0][0].OwningPlayer = player2;
            board[0][1].OwningPlayer = player2;
            board[0][2].OwningPlayer = player2;
            board[0][3].OwningPlayer = player1;
            board[0][4].OwningPlayer = player1;

            board[1][0].OwningPlayer = player1;
            board[1][1].OwningPlayer = player1;
            board[1][2].OwningPlayer = player1;
            board[1][3].OwningPlayer = player2;
            board[1][4].OwningPlayer = player1;

            board[2][0].OwningPlayer = player2;
            board[2][1].OwningPlayer = player1;
            board[2][2].OwningPlayer = player2;
            board[2][3].OwningPlayer = player1;
            board[2][4].OwningPlayer = player1;

            board[3][0].OwningPlayer = player1;
            board[3][1].OwningPlayer = player1;

            List<Tile> compulsionWin = AIHelper.FindTwoTurnCompulsionWin(game, player1);

            Assert.AreEqual(1, compulsionWin.Count);
            Assert.AreEqual(board[3][2], compulsionWin[0]);
        }

        [Test]
        public void TestFindTwoTurnCompultionWinForTile_DoesntThrowGame() {
            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2);
            var board = game.Board;

            //If player one, x tries to play on 4,0 to start a compulsion
            //it would backfire, giving player two the win via a positive diagonal
            //connection. So make sure that option isn't considered

            // . . . . . x t
            // . . . . . t x
            // . . x x . x t
            // . . x t . t x
            ////////////////////

            board[2][0].OwningPlayer = player1;
            board[2][1].OwningPlayer = player1;

            board[3][0].OwningPlayer = player2;
            board[3][1].OwningPlayer = player1;

            //column four is empty

            board[5][0].OwningPlayer = player2;
            board[5][1].OwningPlayer = player1;
            board[5][2].OwningPlayer = player2;
            board[5][3].OwningPlayer = player1;

            board[6][0].OwningPlayer = player1;
            board[6][1].OwningPlayer = player2;
            board[6][2].OwningPlayer = player1;
            board[6][3].OwningPlayer = player2;

            List<Tile> compulsionWin = AIHelper.FindTwoTurnCompulsionWin(game, player1);

            Assert.AreEqual(0, compulsionWin.Count);
        }

        [Test]
        public void TestFindFourTokenConnectionPossibilties() {
            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2);
            var board = game.Board;

            board[0][0].OwningPlayer = player1;
            board[1][0].OwningPlayer = player2;

            board[4][0].OwningPlayer = player1;
            board[4][1].OwningPlayer = player1;

            board[5][0].OwningPlayer = player1;
            board[5][1].OwningPlayer = player2;


            List<TileConnection> conns = AIHelper.FindFourTokenConnectionPossibilties(board, player1);

            //Around 0,0
            Assert.AreEqual("[0,0] - [0,3]", conns[0].ToString()); //vert
            Assert.AreEqual("[0,0] - [3,3]", conns[1].ToString()); //+diag

            //Around 4,0
            //Assert.AreEqual("[4,0] - [4,3]", conns[2].ToString()); //vert subset of [4,0] - [4,4]
            //Assert.AreEqual("[2,0] - [6,0]", conns[2].ToString()); //horiz dup of [2,0] - [6,0] around 5,0
            Assert.AreEqual("[1,3] - [4,0]", conns[2].ToString()); //-diag

            //Around 4,1
            Assert.AreEqual("[4,0] - [4,4]", conns[3].ToString()); //vert
            Assert.AreEqual("[1,1] - [4,1]", conns[4].ToString()); //horiz
            Assert.AreEqual("[3,0] - [6,3]", conns[5].ToString()); //+diag
            Assert.AreEqual("[1,4] - [5,0]", conns[6].ToString()); //-diag

            //Around 5,0
            Assert.AreEqual("[2,0] - [6,0]", conns[7].ToString());
            //Assert.AreEqual("[2,3] - [5,0]", conns[10].ToString()); //-diag subset of [1,4] - [5,0]


            Assert.AreEqual(8, conns.Count);

        }

    }
}
