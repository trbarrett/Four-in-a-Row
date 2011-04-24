using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.Game;
using NUnit.Framework;

namespace FourRow.Tests.Unit.GameTest
{
    [TestFixture]
    class ColumnTest {

        [Test]
        public void TestGetFirstEmptyTile() {
            var player = new Object();
            var col = new Column(0);

            Assert.AreEqual(col[0], col.GetFirstEmptyTile());

            col[0].OwningPlayer = player;
            Assert.AreEqual(col[1], col.GetFirstEmptyTile());

            col[1].OwningPlayer = player;
            Assert.AreEqual(col[2], col.GetFirstEmptyTile());

            col[2].OwningPlayer = player;
            Assert.AreEqual(col[3], col.GetFirstEmptyTile());

            col[3].OwningPlayer = player;
            Assert.AreEqual(col[4], col.GetFirstEmptyTile());

            col[4].OwningPlayer = player;
            Assert.AreEqual(col[5], col.GetFirstEmptyTile());

            col[5].OwningPlayer = player;
            Assert.AreEqual(null, col.GetFirstEmptyTile());
        }

        [Test]
        public void TestIsFull() {
            var player = new Object();
            var col = new Column(0);

            Assert.AreEqual(false, col.IsFull);

            col[0].OwningPlayer = player;
            col[1].OwningPlayer = player;
            col[2].OwningPlayer = player;
            col[3].OwningPlayer = player;
            Assert.AreEqual(false, col.IsFull);

            col[4].OwningPlayer = player;
            col[5].OwningPlayer = player;
            Assert.AreEqual(true, col.IsFull);
        }
    }
}
