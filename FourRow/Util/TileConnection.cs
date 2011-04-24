using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.Game;

namespace FourRow.Util
{

    public class TileConnection
    {
        private List<Tile> _tiles;

        public List<Tile> Tiles {
            get { return _tiles; }
        }

        public Tile this[int row] {
            get { return Tiles[row]; }
        }

        public TileConnection(List<Tile> tiles) {
            _tiles = tiles;
        }

        public bool HasImmediatlyWinnableConnectionForPlayer(Object player) {
            foreach (Tile t in _tiles) {
                if (t.OwningPlayer == null && WillTokenPlacedWinGameForPlayer(player, t)) {
                    return true;
                }
            }
            return false;
        }

        
        public bool WillTokenPlacedWinGameForPlayer(
                Object player,
                Game.Tile tile) {

            int directlyConnectingCount = 0;

            //Count Backwards
            int currentIndex = _tiles.IndexOf(tile) - 1;
            while (currentIndex >= 0 && _tiles[currentIndex].OwningPlayer == player) {
                directlyConnectingCount++;
                currentIndex--;
            }

            //Count Forwards
            currentIndex = _tiles.IndexOf(tile) + 1;
            while (currentIndex < _tiles.Count && _tiles[currentIndex].OwningPlayer == player) {
                directlyConnectingCount++;
                currentIndex++;
            }

            return directlyConnectingCount >= 3;
        }


        /// <summary>
        /// Given a tile, this method checks if there is a possible
        /// eventually winnable connection around it.
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public TileConnection GetWinnableConnectionAround(Tile tile) {
            var tileIndex = _tiles.IndexOf(tile);

            //Check Backwards
            int backwardsIndex = tileIndex;
            int currentIndex = tileIndex - 1;
            while (currentIndex >= 0 &&
                currentIndex >= (tileIndex - 3) && //once we pass 3 tiles away, they are no longer contributing to this tiles winnablility
                    (_tiles[currentIndex].OwningPlayer == tile.OwningPlayer || 
                    _tiles[currentIndex].OwningPlayer == null)) {
                backwardsIndex = currentIndex;
                currentIndex--;
            }

            //Count Forwards
            var forwardsIndex = tileIndex;
            currentIndex = tileIndex + 1;
            while (currentIndex < _tiles.Count &&
                currentIndex <= (tileIndex + 3) && //once we pass 3 tiles away, they are no longer contributing to this tiles winnablility
                    (_tiles[currentIndex].OwningPlayer == tile.OwningPlayer || 
                    _tiles[currentIndex].OwningPlayer == null)) {
                forwardsIndex = currentIndex;
                currentIndex++;
            }

            if ((forwardsIndex - backwardsIndex + 1) >= 4) {
                return new TileConnection(
                    _tiles.GetRange(backwardsIndex, forwardsIndex - backwardsIndex + 1));

            } else {
                return null;
            }
        }

        public bool IsDuplicateOrSubsetOf(TileConnection tc) {
            if (_tiles.Count == 0) { return true; }
            //check the if the first and last tile are within the given tc. If so we are the same
            //or a subset
            return (tc.Tiles.Contains(_tiles[0]) 
                && tc.Tiles.Contains(_tiles[_tiles.Count - 1]));
        }

        public override string ToString() { 
            return _tiles[0].ToStringBasic() + " - " + _tiles[_tiles.Count - 1].ToStringBasic();
        }

        public override bool Equals(object obj) {
            if (obj == null) { return false; }
            return this.ToString().Equals(obj.ToString());
        }
    }
}
