using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FourRow.Game;

namespace FourRow.Tests.Unit.GameTest
{
    [TestFixture]
    class BoardTest
    {

        [Test]
        public void TestGetAllRows() {
            Board board = new Board();

            var rows = board.GetAllRows();

            Assert.AreEqual(rows[0][0], board[0][0]);
            Assert.AreEqual(rows[0][1], board[1][0]);
            Assert.AreEqual(rows[0][2], board[2][0]);
            Assert.AreEqual(rows[0][3], board[3][0]);
            Assert.AreEqual(rows[0][4], board[4][0]);
            Assert.AreEqual(rows[0][5], board[5][0]);
            Assert.AreEqual(rows[0][6], board[6][0]);

            Assert.AreEqual(rows[1][0], board[0][1]);
            Assert.AreEqual(rows[1][1], board[1][1]);
            Assert.AreEqual(rows[1][2], board[2][1]);
            Assert.AreEqual(rows[1][3], board[3][1]);
            Assert.AreEqual(rows[1][4], board[4][1]);
            Assert.AreEqual(rows[1][5], board[5][1]);
            Assert.AreEqual(rows[1][6], board[6][1]);

            Assert.AreEqual(rows[2][0], board[0][2]);
            Assert.AreEqual(rows[2][1], board[1][2]);
            Assert.AreEqual(rows[2][2], board[2][2]);
            Assert.AreEqual(rows[2][3], board[3][2]);
            Assert.AreEqual(rows[2][4], board[4][2]);
            Assert.AreEqual(rows[2][5], board[5][2]);
            Assert.AreEqual(rows[2][6], board[6][2]);

            Assert.AreEqual(rows[3][0], board[0][3]);
            Assert.AreEqual(rows[3][1], board[1][3]);
            Assert.AreEqual(rows[3][2], board[2][3]);
            Assert.AreEqual(rows[3][3], board[3][3]);
            Assert.AreEqual(rows[3][4], board[4][3]);
            Assert.AreEqual(rows[3][5], board[5][3]);
            Assert.AreEqual(rows[3][6], board[6][3]);

            Assert.AreEqual(rows[4][0], board[0][4]);
            Assert.AreEqual(rows[4][1], board[1][4]);
            Assert.AreEqual(rows[4][2], board[2][4]);
            Assert.AreEqual(rows[4][3], board[3][4]);
            Assert.AreEqual(rows[4][4], board[4][4]);
            Assert.AreEqual(rows[4][5], board[5][4]);
            Assert.AreEqual(rows[4][6], board[6][4]);

            Assert.AreEqual(rows[5][0], board[0][5]);
            Assert.AreEqual(rows[5][1], board[1][5]);
            Assert.AreEqual(rows[5][2], board[2][5]);
            Assert.AreEqual(rows[5][3], board[3][5]);
            Assert.AreEqual(rows[5][4], board[4][5]);
            Assert.AreEqual(rows[5][5], board[5][5]);
            Assert.AreEqual(rows[5][6], board[6][5]);
        }

        [Test]
        public void TestGetAllPositiveDiagonals() {
            Board board = new Board();

            var connections = board.GetAllPositiveDiagonals();

            Assert.AreEqual(connections[0][0], board[3][0]);
            Assert.AreEqual(connections[0][1], board[4][1]);
            Assert.AreEqual(connections[0][2], board[5][2]);
            Assert.AreEqual(connections[0][3], board[6][3]);

            Assert.AreEqual(connections[1][0], board[2][0]);
            Assert.AreEqual(connections[1][1], board[3][1]);
            Assert.AreEqual(connections[1][2], board[4][2]);
            Assert.AreEqual(connections[1][3], board[5][3]);
            Assert.AreEqual(connections[1][4], board[6][4]);

            Assert.AreEqual(connections[2][0], board[1][0]);
            Assert.AreEqual(connections[2][1], board[2][1]);
            Assert.AreEqual(connections[2][2], board[3][2]);
            Assert.AreEqual(connections[2][3], board[4][3]);
            Assert.AreEqual(connections[2][4], board[5][4]);
            Assert.AreEqual(connections[2][5], board[6][5]);

            Assert.AreEqual(connections[3][0], board[0][0]);
            Assert.AreEqual(connections[3][1], board[1][1]);
            Assert.AreEqual(connections[3][2], board[2][2]);
            Assert.AreEqual(connections[3][3], board[3][3]);
            Assert.AreEqual(connections[3][4], board[4][4]);
            Assert.AreEqual(connections[3][5], board[5][5]);

            Assert.AreEqual(connections[4][0], board[0][1]);
            Assert.AreEqual(connections[4][1], board[1][2]);
            Assert.AreEqual(connections[4][2], board[2][3]);
            Assert.AreEqual(connections[4][3], board[3][4]);
            Assert.AreEqual(connections[4][4], board[4][5]);

            Assert.AreEqual(connections[5][0], board[0][2]);
            Assert.AreEqual(connections[5][1], board[1][3]);
            Assert.AreEqual(connections[5][2], board[2][4]);
            Assert.AreEqual(connections[5][3], board[3][5]);
        }

        [Test]
        public void TestGetAllNegativeDiagonals() {
            Board board = new Board();

            var connections = board.GetAllNegativeDiagonals();

            Assert.AreEqual(connections[0][0], board[0][3]);
            Assert.AreEqual(connections[0][1], board[1][2]);
            Assert.AreEqual(connections[0][2], board[2][1]);
            Assert.AreEqual(connections[0][3], board[3][0]);

            Assert.AreEqual(connections[1][0], board[0][4]);
            Assert.AreEqual(connections[1][1], board[1][3]);
            Assert.AreEqual(connections[1][2], board[2][2]);
            Assert.AreEqual(connections[1][3], board[3][1]);
            Assert.AreEqual(connections[1][4], board[4][0]);

            Assert.AreEqual(connections[2][0], board[0][5]);
            Assert.AreEqual(connections[2][1], board[1][4]);
            Assert.AreEqual(connections[2][2], board[2][3]);
            Assert.AreEqual(connections[2][3], board[3][2]);
            Assert.AreEqual(connections[2][4], board[4][1]);
            Assert.AreEqual(connections[2][5], board[5][0]);

            Assert.AreEqual(connections[3][0], board[1][5]);
            Assert.AreEqual(connections[3][1], board[2][4]);
            Assert.AreEqual(connections[3][2], board[3][3]);
            Assert.AreEqual(connections[3][3], board[4][2]);
            Assert.AreEqual(connections[3][4], board[5][1]);
            Assert.AreEqual(connections[3][5], board[6][0]);

            Assert.AreEqual(connections[4][0], board[2][5]);
            Assert.AreEqual(connections[4][1], board[3][4]);
            Assert.AreEqual(connections[4][2], board[4][3]);
            Assert.AreEqual(connections[4][3], board[5][2]);
            Assert.AreEqual(connections[4][4], board[6][1]);

            Assert.AreEqual(connections[5][0], board[3][5]);
            Assert.AreEqual(connections[5][1], board[4][4]);
            Assert.AreEqual(connections[5][2], board[5][3]);
            Assert.AreEqual(connections[5][3], board[6][2]);
        }

    }
}
