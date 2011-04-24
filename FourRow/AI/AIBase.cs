using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourRow.AI
{
    class AIBase
    {
        private UI.GameController _gameController;
        private Object _player;

        public AIBase(UI.GameController gameController, Object player) {
            _gameController = gameController;
            _player = player;
        }

        public void ProcessGameTurn() {

            //We basically need to consider each move based on the impact to the game, and how many turns
            //ahead it will need to be. Often it will be better to look at the best multi-turn move combination
            //rather than the single best move for this turn.
            //We could give each turn possibility a ranking, then decide from that. But that means we'll have
            //to process every option before we take one, which will take more time. If we can be certain
            //of the order in which we will want to implement each option then we don't need to process the
            //options. We jst need to consider them in order. I'm not sure method I'll take yet, but I'm
            //going to go ahead with the assumption that we can just do each check in a linear order unless
            //I encounter something that proves otherwise.

            //==== End Moves ====

            //This turn compulsions
            //---------------------

            //Look for a winning possibility and complete it
            var potentialWinningTiles = AIHelper.FindDirectWinningPossibilities(
                _gameController.Game.Board, 
                _player);

            if (potentialWinningTiles.Count != 0) {
                PlaceTokenInOneOfTheseTiles(potentialWinningTiles);
                return;
            }

            //Look to see if the opponent will be able to complete his move next turn, and block it.
            var potentialLosingTiles = AIHelper.FindDirectWinningPossibilities(
                _gameController.Game.Board,
                _gameController.Game.GetNonCurrentPlayer());

            if (potentialLosingTiles.Count != 0) {
                PlaceTokenInOneOfTheseTiles(potentialLosingTiles);
                return;
            }

            //Two turn compulstions
            //---------------------

            //Look to see if we have a move that will force the other player into giving us the win
            //in the next turn, and do it.
            var compulsionTiles = AIHelper.FindTwoTurnCompulsionWin(_gameController.Game, _player);

            if (compulsionTiles.Count != 0) {
                PlaceTokenInOneOfTheseTiles(compulsionTiles);
                return;
            }

            //Look for the above thing for the opponent. A move which will force us to make a defensive blocking 
            //move, and give him the winn on the next turn.

            //Three plus turn compulsions
            //---------------------------

            //Three turn compulsions are unlikley, so I'm not going to consider them at this point. Though
            //doing so should just be a modification and additional search based on the two-turn compulsion

            //==== Mid And Early Game Moves ====

            //In the mid game we are trying to create general opportunities to head into the end game by
            //creating potential token connections. At this point we're not really interested in blocking
            //the opponent. Though if in doing so we can further our own ends we should.

            //1) We want to look at where we have potential connections, and try to build that area up
            //to generate a compulsion in the other player.


            //2) We need to look for places where there is enough space for four tokens in all four 
            //types of connections.

            //We want to discount putting down tokens that would force the other player to place a blocking
            //token. That would just be a waste of a potential connection. 
            //i.e. Building two staight up is fine. But building a third one would just force them to block us.

            //We want to preference positions which are close to our other tokens. That will give us
            //more opportunities later on.

            //We need to consider connections that we need to build towards. Such as diagonal ones and vertical
            //ones supported by platforms. We want to place supports for those, but not supports that would
            //give the other player a pre-emtive blocking move.

            //When building up want to try and build in such places that can force a compulsion later on. They
            //are less obvious to the opposing player.


            //3) If we have no directly obvious moves we should look for places where the opposing player
            //has space enough for four tokens, and is aready half way complete in making that connection.
            //Then we want to block him if possible.

            //4) If there are no good options, then place a token as low and centeral as possible.
            // (I'm not actually sure if this is a very optimial strategy. I would have to do some testing
            //with different options (random, low and sides, high and central, high and sides, third poins, etc))



            //For testing, just randomly place the token.

        }

        public void PlaceTokenInOneOfTheseTiles(List<Game.Tile> tiles) {
            if (tiles.Count == 1) {
                _gameController.Game.DropTokenOnColumn(tiles[0].ColumnNo);
            } else {
                var rand = new Random();
                var randomTile = tiles[
                    rand.Next(tiles.Count - 1)];
                _gameController.Game.DropTokenOnColumn(tiles[0].ColumnNo);
            }
        }


    }
}
