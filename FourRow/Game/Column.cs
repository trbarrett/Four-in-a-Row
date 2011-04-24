using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourRow.Game
{
    public class Column
    {
        private int _columnNo;
        public int ColumnNo { get { return _columnNo; } }

        public List<Tile> Tiles { get; set; }

        public Tile this[int row] {
            get { return Tiles[row]; }
        }

        public bool IsFull {
            get { return GetFirstEmptyTile() == null; }
        }

        public Tile GetFirstEmptyTile() {
            return Tiles.Find(delegate(Tile t) {
                return t.OwningPlayer == null;
            });
        }

        public Column(int columnNo) {
            _columnNo = columnNo;
            this.Tiles = new List<Tile>();
            for (int row = 0; row < Board.BoardHeight; row++) {
                Tiles.Add(new Tile(this, row));
            }
        }
    }
}
