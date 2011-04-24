using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourRow.UI
{
    public class UIBoardTile: UIComponent
    {
        public static readonly Size TileSize = new Size(40, 40);

        private UIBoardColumn _uiColumn;
        private Game.Tile _tile;
        private bool _tileHighlightOn;

        public UIBoardTile(UIBoardColumn uiColumn, Game.Tile tile) {
            _uiColumn = uiColumn;
            _tile = tile;
            this.Size = TileSize;
        }

        public void UpdateHighlightState(bool highlightOn) {
            _tileHighlightOn = highlightOn;
        }


        public void DrawTile(Graphics g) {
            Pen p = null;
            Brush b = null;
            try {
                //creating and disposing of the pen for each tile is quite wasteful
                p = new Pen(Color.Black, 1);
                g.DrawRectangle(p, this.Bounds);

                //We should fill it based on the player colour
                var uiPlayer = _tile.OwningPlayer as Player;
                if (uiPlayer != null) {
                    b = new SolidBrush(GetTokenColour(uiPlayer));
                    g.FillRectangle(b, this.Bounds);
                }

            } finally {
                if (b != null) { b.Dispose(); }
                if (p != null) { p.Dispose(); }
            }
        }

        private Color GetTokenColour(Player uiPlayer) {
            if (_tileHighlightOn) {
                return uiPlayer.HighlightColour;
            } else {
                return uiPlayer.TokenColour;
            }
        }
    }
}
