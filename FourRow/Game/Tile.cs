using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourRow.Game
{
    public class Tile
    {
        private int _rowNo;
        private Column _column;

        public Column Column { get { return _column; } }

        public Object OwningPlayer { get; set; }

        public int RowNo { get { return _rowNo; } }
        public int ColumnNo { get { return _column.ColumnNo; } }

        public Tile(Column column, int rowNo, Object owningPlayer) : this(column, rowNo) {
            OwningPlayer = owningPlayer;
        }

        public Tile(Column column, int rowNo) {
            _column = column;
            _rowNo = rowNo;
        }
    }
}
