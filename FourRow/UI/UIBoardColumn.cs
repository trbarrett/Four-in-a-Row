using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourRow.UI
{
    public class UIBoardColumn: UIComponent
    {

        private readonly int ColumnWidth = 40;

        private DropBoard _dropBoard;
        private Game.Column _column;
        private List<UIBoardTile> _tiles = new List<UIBoardTile>();

        public UIBoardTile this[int row] {
            get { return _tiles[row]; }
        }

        public Game.Column GameColumn { get { return _column; } }

        public int ColumnNo { get { return _column.ColumnNo; } }

        public UIBoardColumn(DropBoard dropBoard, Game.Column column) {
            _dropBoard = dropBoard;
            _column = column;
            var columnHeight = _column.Tiles.Count * UIBoardTile.TileSize.Height;
            var currentY = columnHeight - UIBoardTile.TileSize.Height; //We start from the bottom and work upwards adding tiles
            _column.Tiles.ForEach( tile => {
                UIBoardTile uiTile = new UIBoardTile(this, tile);
                _tiles.Add(uiTile);
                uiTile.Position = new Point(0, currentY);
                currentY -= uiTile.Height;
            });

            this.Size = new Size(ColumnWidth, columnHeight);
        }

        public void DrawColumn(Graphics g) {
            try {

                g.TranslateTransform(this.Position.X, this.Position.Y);

                //Draw each of the tiles
                _tiles.ForEach( uiTile => {
                    uiTile.DrawTile(g);
                });
            } finally {
                g.ResetTransform();
            }
        }
    }
}
