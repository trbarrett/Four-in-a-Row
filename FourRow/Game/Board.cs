using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourRow.Game
{

    public class Board
    {
        public static int BoardWidth = 7;
        public static int BoardHeight = 6;

        public Column this[int column] {
            get { return Columns[column]; }
        }

        public List<Column> Columns { get; set; }

        public Board() {
            Columns = new List<Column>();
            for (int column = 0; column < BoardWidth; column++) {
                Columns.Add(new Column(column));
            }
        }

        public List<List<Tile>> GetAllRows() {
            var rows = new List<List<Tile>>();
            for (int rowNo = 0; rowNo < BoardHeight; rowNo++) {
                rows.Add(GetRow(rowNo));
            }
            return rows;
        }

        public List<Tile> GetRow(int rowNo) {
            var rowTiles = new List<Tile>();
            for (int colNo = 0; colNo < BoardWidth; colNo++) {
                rowTiles.Add( this[colNo][rowNo]);
            }
            return rowTiles;
        }

        public List<List<Tile>> GetAllPositiveDiagonals() {
            //Each positive diagonal can be described by the row we start on.
            //This does mean that we have to move into negatives to account
            //for those diagonals that start at the bottom of the board. We just
            //clip to the board in that case so that we don't pickup up
            //imaginary tiles.

            /* 
             * 5  x x x o x x x
             * 4  x   o       x
             * 3  x o         o
             * 2  o         o x
             * 1  x       o   x
             * 0  x x x o x x x
             * -1     o  
             * -2   o
             * -3 o = (0 - width(7) + 4)
            */

            //We also ignore the first and last 3 diagonals since they
            //don't have enough tiles to make a connection.
            int startingDiagRow = 0 - BoardWidth + 4; //-3
            int endingDiagRow = (BoardHeight - 1) - 3; //2

            List<List<Tile>> positiveDiagonals = new List<List<Tile>>();
            for (int diagRow = startingDiagRow; diagRow <= endingDiagRow; diagRow++) {
                positiveDiagonals.Add(
                    GetPositiveDiagonalTilesFromStartingRow(diagRow));
            }

            return positiveDiagonals;
        }

        public List<Tile> GetPositiveDiagonalTilesFromStartingRow(int diagRow) {
            int startingRowNo;
            int startingColumnNo;
            if (diagRow < 0) {
                startingRowNo = 0;
                startingColumnNo = 0 + Math.Abs(diagRow);
            } else {
                startingRowNo = diagRow;
                startingColumnNo = 0;
            }

            List<Tile> diagonalTiles = new List<Tile>();
            int columnNo = startingColumnNo;
            int rowNo = startingRowNo;
            while (columnNo < Board.BoardWidth && rowNo < Board.BoardHeight) {
                diagonalTiles.Add(this[columnNo][rowNo]);
                columnNo++;
                rowNo++;
            }

            return diagonalTiles;
        }

        public List<List<Tile>> GetAllNegativeDiagonals() {
            //Each negative diagonal can be described by the row we start on.
            //This does mean that we have to move past the board height to account
            //for those diagonals that start at the top/right of the board. We just
            //clip to the board in that case so that we don't pickup up
            //imaginary tiles.

            /*
             * 8  o = hieght(7) - 1 + width(6) - 1 - 3 = 6 + 5 - 3
             * 7    o
             * 6      o
             * 5  x x x o x x x
             * 4  x       o   x
             * 3  o         o x
             * 2  x o         o
             * 1  x   o       x
             * 0  x x x o x x x
            */

            //We also ignore the first and last 3 diagonals since they
            //don't have enough tiles to make a connection.
            int startingDiagRow = 3; //fourth row when you include row 0
            int endingDiagRow = (BoardHeight - 1) + BoardWidth - 4; 

            List<List<Tile>> negativeDiagonals = new List<List<Tile>>();
            for (int diagRow = startingDiagRow; diagRow <= endingDiagRow; diagRow++) {
                negativeDiagonals.Add(
                    GetNegativeDiagonalTilesFromStartingRow(diagRow));
            }

            return negativeDiagonals;
        }

        public List<Tile> GetNegativeDiagonalTilesFromStartingRow(int diagRow) {
            int startingRowNo;
            int startingColumnNo;
            if (diagRow > (BoardHeight - 1)) {
                startingRowNo = BoardHeight - 1;
                startingColumnNo = diagRow - (BoardHeight - 1);
            } else {
                startingRowNo = diagRow;
                startingColumnNo = 0;
            }

            List<Tile> diagonalTiles = new List<Tile>();
            int columnNo = startingColumnNo;
            int rowNo = startingRowNo;
            while (columnNo < BoardWidth && rowNo >= 0) {
                diagonalTiles.Add(this[columnNo][rowNo]);
                columnNo++;
                rowNo--;
            }

            return diagonalTiles;
        }

        /// <summary>
        /// Gets all tiles that connect in a winnable way, verticals, horizontals, 
        /// positive diagonals and negative diagonals.
        /// </summary>
        /// <returns></returns>
        public List<List<Tile>> GetAllTileConnections() {
            var allTileConnections = new List<List<Tile>>();

            allTileConnections.AddRange(
                (from col in Columns
                 select col.Tiles).ToList());

            allTileConnections.AddRange(
                (from row in this.GetAllRows()
                 select row).ToList());

            allTileConnections.AddRange(
                (from diag in this.GetAllPositiveDiagonals()
                     select diag).ToList());

            allTileConnections.AddRange(
                (from diag in this.GetAllNegativeDiagonals()
                     select diag).ToList());

            return allTileConnections;
        }
    }
}
