using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FourRow.Util;
using FourRow.Game;

namespace FourRow.Tests.Unit.Util
{
    [TestFixture]
    class TileConnectionTest
    {
        [Test]
        public void TestToString() {
            var tc = new TileConnection(new List<Tile>() {
                new Tile(new Column(0), 1),
                new Tile(new Column(1), 2),
                new Tile(new Column(2), 3)
            });

            Assert.AreEqual("[0,1] - [2,3]", tc.ToString());


            tc = new TileConnection(new List<Tile>() {
                new Tile(new Column(2), 1),
                new Tile(new Column(2), 2),
                new Tile(new Column(2), 3)
            });

            Assert.AreEqual("[2,1] - [2,3]", tc.ToString());


            tc = new TileConnection(new List<Tile>() {
                new Tile(new Column(2), 1)
            });

            Assert.AreEqual("[2,1] - [2,1]", tc.ToString());
        }

        [Test]
        public void GetWinnableConnectionAround_Simple() {
            object player1 = new object();
            object player2 = new object();

            var tc = new TileConnection(new List<Tile>() {
                new Tile(new Column(0), 1, player2),
                new Tile(new Column(1), 1),
                new Tile(new Column(2), 1, player1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
                new Tile(new Column(5), 1),
                new Tile(new Column(6), 1)
            });

            var winnable = tc.GetWinnableConnectionAround(tc.Tiles[2]);

            //0,1 is out since it's the wrong player
            //6,1 is out since it's too far away
            Assert.AreEqual(5, winnable.Tiles.Count);
            Assert.AreEqual("[1,1] - [5,1]", winnable.ToString());
        }

        [Test]
        public void GetWinnableConnectionAround_OnEdge() {
            object player1 = new object();
            object player2 = new object();

            var tc = new TileConnection(new List<Tile>() {
                new Tile(new Column(0), 1, player1),
                new Tile(new Column(1), 1, player1),
                new Tile(new Column(2), 1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
                new Tile(new Column(5), 1),
                new Tile(new Column(6), 1)
            });

            var winnable = tc.GetWinnableConnectionAround(tc.Tiles[0]);

            //4,1 + is out since it's too far away
            Assert.AreEqual(4, winnable.Tiles.Count);
            Assert.AreEqual("[0,1] - [3,1]", winnable.ToString());
        }

        [Test]
        public void TestIsDuplicateOrSubsetOf() {
            var tc1 = new TileConnection(new List<Tile>() {
                new Tile(new Column(1), 1),
                new Tile(new Column(2), 1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
            });

            var tc2 = new TileConnection(new List<Tile>() {
                new Tile(new Column(0), 1),
                new Tile(new Column(1), 1),
                new Tile(new Column(2), 1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
                new Tile(new Column(5), 1),
                new Tile(new Column(6), 1)
            });

            Assert.IsTrue(tc1.IsDuplicateOrSubsetOf(tc2));
            Assert.IsTrue(!tc2.IsDuplicateOrSubsetOf(tc1));

            tc1 = new TileConnection(new List<Tile>() {
                new Tile(new Column(1), 1),
                new Tile(new Column(2), 1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
            });

            tc2 = new TileConnection(new List<Tile>() {
                new Tile(new Column(1), 1),
                new Tile(new Column(2), 1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
            });

            Assert.IsTrue(tc1.IsDuplicateOrSubsetOf(tc2));
            Assert.IsTrue(tc2.IsDuplicateOrSubsetOf(tc1));



            tc1 = new TileConnection(new List<Tile>() {
                new Tile(new Column(0), 1),
                new Tile(new Column(1), 1),
                new Tile(new Column(2), 1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
            });

            tc2 = new TileConnection(new List<Tile>() {
                new Tile(new Column(1), 1),
                new Tile(new Column(2), 1),
                new Tile(new Column(3), 1),
                new Tile(new Column(4), 1),
                new Tile(new Column(5), 1),
            });

            Assert.IsTrue(!tc1.IsDuplicateOrSubsetOf(tc2));
            Assert.IsTrue(!tc2.IsDuplicateOrSubsetOf(tc1));
        }

        [Test]
        public void TestWillTokenPlacedWinGameForPlayer_FilledOnOneEnd1() {
            var player = new Object();
            var tiles = new List<Tile>();
            tiles.Add(new Tile(new Column(0), 0, player));
            tiles.Add(new Tile(new Column(1), 0, player));
            tiles.Add(new Tile(new Column(2), 0, player));
            tiles.Add(new Tile(new Column(3), 0, null));
            tiles.Add(new Tile(new Column(4), 0, null));
            tiles.Add(new Tile(new Column(5), 0, null));
            tiles.Add(new Tile(new Column(6), 0, null));
            var tileConnection = new TileConnection(tiles);


            Assert.IsTrue(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[3]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[4]));

            Assert.IsTrue(tileConnection.HasImmediatlyWinnableConnectionForPlayer(player));
        }

        [Test]
        public void TestWillTokenPlacedWinGameForPlayer_FilledOnOneEnd2() {
            var player = new Object();
            var tiles = new List<Tile>();
            tiles.Add(new Tile(new Column(0), 0, null));
            tiles.Add(new Tile(new Column(1), 0, null));
            tiles.Add(new Tile(new Column(2), 0, null));
            tiles.Add(new Tile(new Column(3), 0, null));
            tiles.Add(new Tile(new Column(4), 0, player));
            tiles.Add(new Tile(new Column(5), 0, player));
            tiles.Add(new Tile(new Column(6), 0, player));
            var tileConnection = new TileConnection(tiles);


            Assert.IsTrue(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[3]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[2]));

            Assert.IsTrue(tileConnection.HasImmediatlyWinnableConnectionForPlayer(player));
        }

        [Test]
        public void TestWillTokenPlacedWinGameForPlayer_FilledInMiddle1() {
            var player = new Object();
            var tiles = new List<Tile>();
            tiles.Add(new Tile(new Column(0), 0, null));
            tiles.Add(new Tile(new Column(1), 0, player));
            tiles.Add(new Tile(new Column(2), 0, player));
            tiles.Add(new Tile(new Column(3), 0, player));
            tiles.Add(new Tile(new Column(4), 0, null));
            tiles.Add(new Tile(new Column(5), 0, null));
            tiles.Add(new Tile(new Column(6), 0, null));
            var tileConnection = new TileConnection(tiles);


            Assert.IsTrue(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[0]));
            Assert.IsTrue(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[4]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[5]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[6]));

            Assert.IsTrue(tileConnection.HasImmediatlyWinnableConnectionForPlayer(player));
        }

        [Test]
        public void TestWillTokenPlacedWinGameForPlayer_FilledInMiddle2() {
            var player = new Object();
            var tiles = new List<Tile>();
            tiles.Add(new Tile(new Column(0), 0, null));
            tiles.Add(new Tile(new Column(1), 0, null));
            tiles.Add(new Tile(new Column(2), 0, null));
            tiles.Add(new Tile(new Column(3), 0, player));
            tiles.Add(new Tile(new Column(4), 0, player));
            tiles.Add(new Tile(new Column(5), 0, player));
            tiles.Add(new Tile(new Column(6), 0, null));
            var tileConnection = new TileConnection(tiles);
            

            Assert.IsTrue(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[2]));
            Assert.IsTrue(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[6]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[0]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[1]));

            Assert.IsTrue(tileConnection.HasImmediatlyWinnableConnectionForPlayer(player));
        }

        [Test]
        public void TestWillTokenPlacedWinGameForPlayer_TileInCentre() {
            var player = new Object();
            var tiles = new List<Tile>();
            tiles.Add(new Tile(new Column(0), 0, null));
            tiles.Add(new Tile(new Column(1), 0, null));
            tiles.Add(new Tile(new Column(2), 0, player));
            tiles.Add(new Tile(new Column(3), 0, null));
            tiles.Add(new Tile(new Column(4), 0, player));
            tiles.Add(new Tile(new Column(5), 0, player));
            tiles.Add(new Tile(new Column(6), 0, null));
            var tileConnection = new TileConnection(tiles);


            Assert.IsTrue(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[3]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[0]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[1]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[6]));

            Assert.IsTrue(tileConnection.HasImmediatlyWinnableConnectionForPlayer(player));
        }

        [Test]
        public void TestWillTokenPlacedWinGameForPlayer_NotWinnable() {
            var player = new Object();
            var tiles = new List<Tile>();
            tiles.Add(new Tile(new Column(0), 0, null));
            tiles.Add(new Tile(new Column(1), 0, null));
            tiles.Add(new Tile(new Column(2), 0, null));
            tiles.Add(new Tile(new Column(3), 0, player));
            tiles.Add(new Tile(new Column(4), 0, player));
            tiles.Add(new Tile(new Column(5), 0, null));
            tiles.Add(new Tile(new Column(6), 0, null));
            var tileConnection = new TileConnection(tiles);

            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[2]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[5]));
            Assert.IsFalse(tileConnection.WillTokenPlacedWinGameForPlayer(player, tileConnection[6]));

            Assert.IsFalse(tileConnection.HasImmediatlyWinnableConnectionForPlayer(player));
        }
    }
}
