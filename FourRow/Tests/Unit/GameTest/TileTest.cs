using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.Game;
using NUnit.Framework;

namespace FourRow.Tests.Unit.GameTest
{
    [TestFixture]
    class TileTest
    {

        [Test]
        public void TestGetPositiveDiagonalStartingRowNo() {
            var tile = new Tile(new Column(0), 4);
            Assert.AreEqual(4, tile.GetPositiveDiagonalStartingRowNo());

            tile = new Tile(new Column(3), 5);
            Assert.AreEqual(2, tile.GetPositiveDiagonalStartingRowNo());

            tile = new Tile(new Column(6), 5);
            Assert.AreEqual(-1, tile.GetPositiveDiagonalStartingRowNo());

            tile = new Tile(new Column(6), 0);
            Assert.AreEqual(-6, tile.GetPositiveDiagonalStartingRowNo());
        }

        [Test]
        public void TestGetNegativeDiagonalStartingRowNo() {
            var tile = new Tile(new Column(0), 4);
            Assert.AreEqual(4, tile.GetNegativeDiagonalStartingRowNo());

            tile = new Tile(new Column(3), 4);
            Assert.AreEqual(7, tile.GetNegativeDiagonalStartingRowNo());

            tile = new Tile(new Column(5), 0);
            Assert.AreEqual(5, tile.GetNegativeDiagonalStartingRowNo());

            tile = new Tile(new Column(6), 2);
            Assert.AreEqual(8, tile.GetNegativeDiagonalStartingRowNo());
        }
    }
}
