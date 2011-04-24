using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.Game;
using FourRow.AI;
using NUnit.Framework;

namespace FourRow.Tests.Unit.AI
{
    [TestFixture]
    class AIHelperTest
    {

        [Test]
        public void TestDoesTileHaveThreeDirectlyConnectingPlayerTokens_FilledOnOneEnd1() {
            var player = new Object();
            var tileConnection = new List<Tile>();
            tileConnection.Add( new Tile(new Column(0), 0, player));
            tileConnection.Add( new Tile(new Column(1), 0, player));
            tileConnection.Add( new Tile(new Column(2), 0, player));
            tileConnection.Add( new Tile(new Column(3), 0, null));
            tileConnection.Add( new Tile(new Column(4), 0, null));
            tileConnection.Add( new Tile(new Column(5), 0, null));
            tileConnection.Add( new Tile(new Column(6), 0, null));

            Assert.IsTrue(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[3]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[4]));
        }

        [Test]
        public void TestDoesTileHaveThreeDirectlyConnectingPlayerTokens_FilledOnOneEnd2() {
            var player = new Object();
            var tileConnection = new List<Tile>();
            tileConnection.Add(new Tile(new Column(0), 0, null));
            tileConnection.Add(new Tile(new Column(1), 0, null));
            tileConnection.Add(new Tile(new Column(2), 0, null));
            tileConnection.Add(new Tile(new Column(3), 0, null));
            tileConnection.Add(new Tile(new Column(4), 0, player));
            tileConnection.Add(new Tile(new Column(5), 0, player));
            tileConnection.Add(new Tile(new Column(6), 0, player));

            Assert.IsTrue(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[3]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[2]));
        }

        [Test]
        public void TestDoesTileHaveThreeDirectlyConnectingPlayerTokens_FilledInMiddle1() {
            var player = new Object();
            var tileConnection = new List<Tile>();
            tileConnection.Add(new Tile(new Column(0), 0, null));
            tileConnection.Add(new Tile(new Column(1), 0, player));
            tileConnection.Add(new Tile(new Column(2), 0, player));
            tileConnection.Add(new Tile(new Column(3), 0, player));
            tileConnection.Add(new Tile(new Column(4), 0, null));
            tileConnection.Add(new Tile(new Column(5), 0, null));
            tileConnection.Add(new Tile(new Column(6), 0, null));

            Assert.IsTrue(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[0]));

            Assert.IsTrue(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[4]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[5]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[6]));
        }

        [Test]
        public void TestDoesTileHaveThreeDirectlyConnectingPlayerTokens_FilledInMiddle2() {
            var player = new Object();
            var tileConnection = new List<Tile>();
            tileConnection.Add(new Tile(new Column(0), 0, null));
            tileConnection.Add(new Tile(new Column(1), 0, null));
            tileConnection.Add(new Tile(new Column(2), 0, null));
            tileConnection.Add(new Tile(new Column(3), 0, player));
            tileConnection.Add(new Tile(new Column(4), 0, player));
            tileConnection.Add(new Tile(new Column(5), 0, player));
            tileConnection.Add(new Tile(new Column(6), 0, null));

            Assert.IsTrue(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[2]));

            Assert.IsTrue(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[6]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[0]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[1]));
        }

        [Test]
        public void TestDoesTileHaveThreeDirectlyConnectingPlayerTokens_TileInCentre() {
            var player = new Object();
            var tileConnection = new List<Tile>();
            tileConnection.Add(new Tile(new Column(0), 0, null));
            tileConnection.Add(new Tile(new Column(1), 0, null));
            tileConnection.Add(new Tile(new Column(2), 0, player));
            tileConnection.Add(new Tile(new Column(3), 0, null));
            tileConnection.Add(new Tile(new Column(4), 0, player));
            tileConnection.Add(new Tile(new Column(5), 0, player));
            tileConnection.Add(new Tile(new Column(6), 0, null));

            Assert.IsTrue(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[3]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[0]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[1]));

            Assert.IsFalse(
                AIHelper.DoesTileHaveThreeDirectlyConnectingPlayerTokens(
                    player, tileConnection, tileConnection[6]));
        }

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

            Assert.AreEqual(1, winningPossibilites.Count);
            Assert.AreEqual(board[3][0], winningPossibilites[0]);
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
        public void TestFindDirectWinningPossibilities_OnHorizontalAtMiddleRaised() {
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

            Assert.AreEqual(2, winningPossibilites.Count);
            Assert.AreEqual(board[4][3], winningPossibilites[0]);
            Assert.AreEqual(board[3][3], winningPossibilites[1]); //there's a vertical here as well
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
            Assert.AreEqual(board[5][4], winningPossibilites[0]);
        }
    }
}
