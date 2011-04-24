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

        public override string ToString() {
            var owningPlayerStr = "";
            if (OwningPlayer == null) {
                owningPlayerStr = "none";
            } else { 
                owningPlayerStr = OwningPlayer.ToString();
            }
            return string.Format("Tile: [{0},{1}] - {2}", ColumnNo.ToString(), RowNo.ToString(), owningPlayerStr);
        }

        public string ToStringBasic() {
            return string.Format("[{0},{1}]", ColumnNo.ToString(), RowNo.ToString());
        }

        //Note this doesn't take into account the fact that the diagonal may be less than 4 tiles across
        public int GetPositiveDiagonalStartingRowNo() {
            return RowNo - ColumnNo;
        }

        //Note this doesn't take into account the fact that the diagonal may be less than 4 tiles across
        public int GetNegativeDiagonalStartingRowNo() {
            return RowNo + ColumnNo;
        }

        public override bool Equals(object obj) {
            if (obj == null) { return false; }
            return this.ToString().Equals(obj.ToString());
        }
    }
}
