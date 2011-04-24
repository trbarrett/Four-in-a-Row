using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FourRow.Game;
using FourRow.AI;
using Rhino.Mocks;
using FourRow.UI;

namespace FourRow.Tests.Unit.AI
{
    [TestFixture]
    class AIBaseTest
    {
        [Test]
        public void TestFindAndSetupWinningCompulsion() {
            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2);
            var board = game.Board;

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

            var mocks = new MockRepository();
            var gc = mocks.StrictMock<IGameController>();
            Expect.Call(gc.Game).Return(game).Repeat.Any();
            mocks.ReplayAll();

            var ai = new AIBase();
            ai.PlayTurn(gc, player1);

            Assert.AreEqual(player1, board[4][0].OwningPlayer);
            mocks.VerifyAll();
        }

        [Test]
        public void TestFindAndBlockOpponentCompulsion() {

            //setup the initial condition
            var player1 = "Player 1";
            var player2 = "Player 2";
            var game = new GameCore(player1, player2, player2);
            var board = game.Board;

            // x x x
            // t t t
            // t t t
            // t t t x
            // t t t x t . .
            /////////////////

            board[0][0].OwningPlayer = player2;
            board[0][1].OwningPlayer = player2;
            board[0][2].OwningPlayer = player2;
            board[0][3].OwningPlayer = player1;
            board[0][4].OwningPlayer = player1;
            board[0][5].OwningPlayer = player2;

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

            board[4][0].OwningPlayer = player2;
            board[4][1].OwningPlayer = player2;

            var mocks = new MockRepository();
            var gc = mocks.StrictMock<IGameController>();
            Expect.Call(gc.Game).Return(game).Repeat.Any();
            mocks.ReplayAll();

            var ai = new AIBase(AIDifficulty.VeryHard);
            ai.PlayTurn(gc, player2);

            Assert.AreEqual(player2, board[3][2].OwningPlayer);
            mocks.VerifyAll();
        }

        [Test, Ignore]
        public void TestAIFight() {

            int stalemateCount = 0;
            int player1WinCount = 0;
            int player2WinCount = 0;

            Player player1 = Player.NewPlayer1(new AIBase(AIDifficulty.Medium));
            Player player2 = Player.NewPlayer2(new AIBase(AIDifficulty.VeryHard));

            for (int i = 0; i < 100; i++) {

                var gc = new AIBattleGameController(player1, player2);

                gc.PlayAIGame();
                if (gc.Game.IsStatemate) {
                    stalemateCount++;
                } else if (gc.Game.WinningPlayer == player1) {
                    player1WinCount++;
                } else {
                    player2WinCount++;
                }
            }

            Console.WriteLine("Stalemate Count: " + stalemateCount.ToString());
            Console.WriteLine("Player 1 Win Count: " + player1WinCount.ToString());
            Console.WriteLine("Player 2 Win Count: " + player2WinCount.ToString());


        }
    }
}
