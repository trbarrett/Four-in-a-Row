using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.Game;
using FourRow.Util;
using System.Diagnostics;

namespace FourRow.AI
{

    public enum AIDifficulty {
        VeryEasy,
        Easy,
        Medium,
        Hard,
        VeryHard
    }

    public class AIBase : IAI
    {

        private Random Rand = new Random();

        //private IGameController _gameController;
        //private Object _player;
        private bool _isInteractive = false;

        private AIDifficulty _difficulty;

        private bool _tryToBlockOpponentsGoodConnections = false;

        //These checks provide a bounds on checking for compulsions. Even on the easy
        //settings we should be checking for compulsions when the tile count is low
        private int _blockOpponentCompulsionsCount = 7;
        private int _createCompulsionsCount = 7;

        private bool _UseFuzzyLogicOnRatings = false;
        //We alter all the ratings by this value before normalizing them
        //for the fuzzy logic desision.
        private int _FuzzyDistortion = 0;

        //get the percentage chance we will miss a win with 30 tokens on the board
        //this percentage reduces for each tile less.
        private readonly Dictionary<AIDifficulty, double> WinMissFor30Tokens = 
            new Dictionary<AIDifficulty, double>() {
                {AIDifficulty.VeryEasy, 0.4},
                {AIDifficulty.Easy, 0.2},
                {AIDifficulty.Medium, 0.1},
                {AIDifficulty.Hard, 0.0},
                {AIDifficulty.VeryHard, 0.0}};

        //get the percentage chance we will miss an opponents win with 30 tokens on 
        //the board this percentage reduces for each tile less.
        private readonly Dictionary<AIDifficulty, double> LoseMissFor30Tokens =
            new Dictionary<AIDifficulty, double>() {
                {AIDifficulty.VeryEasy, 0.6},
                {AIDifficulty.Easy, 0.4},
                {AIDifficulty.Medium, 0.2},
                {AIDifficulty.Hard, 0.1},
                {AIDifficulty.VeryHard, 0.0}};

        public AIDifficulty Difficulty { get { return _difficulty; } }

        public AIBase() : this(AIDifficulty.Medium) {}

        public AIBase(AIDifficulty difficulty) {
            _difficulty = difficulty;

            if (_difficulty <= AIDifficulty.Hard) {
                _UseFuzzyLogicOnRatings = true;
            }

            if (_difficulty == AIDifficulty.VeryEasy) { _FuzzyDistortion = 16; }
            if (_difficulty == AIDifficulty.Easy) { _FuzzyDistortion = 8; } 
            if (_difficulty == AIDifficulty.Medium) { _FuzzyDistortion = 4; }
            if (_difficulty == AIDifficulty.Hard) { _FuzzyDistortion = -4; }

            if (_difficulty >= AIDifficulty.Medium) {
                _tryToBlockOpponentsGoodConnections = true;
                _createCompulsionsCount = 12;
            }

            if (_difficulty >= AIDifficulty.Hard) {
                _createCompulsionsCount = int.MaxValue;
                _blockOpponentCompulsionsCount = 12;
            }

            if (_difficulty >= AIDifficulty.VeryHard) {
                _blockOpponentCompulsionsCount = int.MaxValue;
            }
        }

        public bool IsInteractive() {
            return _isInteractive;
        }

        private bool CanCreateCompulsion(Board b) {
            return _createCompulsionsCount > b.GetAllTokenTiles().Count;
        }

        private bool CanBlockOpponentCompulsion(Board b) {
            return _blockOpponentCompulsionsCount > b.GetAllTokenTiles().Count;
        }

        private bool WillSeeImmediateLoss(int possibilities, Board board) {
            return CheckMissOpportunity(LoseMissFor30Tokens, possibilities, board);

        }

        private bool WillSeeImmediateWin(int possibilities, Board board) {
            return CheckMissOpportunity(WinMissFor30Tokens, possibilities, board);
        }

        private bool CheckMissOpportunity(
                Dictionary<AIDifficulty, double> missFor30Tokens, int possibilities, Board board) {
            if (missFor30Tokens[_difficulty] == 0.0) {
                return true;

            } else {
                //work out a modifier from the default miss chance at 30 tokens. 
                double chanceModifier = 1.0;
                int tokenTiles = board.GetAllTokenTiles().Count;
                if (tokenTiles < 30) {
                    chanceModifier = Convert.ToDouble(board.GetAllTokenTiles().Count) / 30.0;
                }
                
                //Chance is reduced for the amount of winn posibilities there are
                var missChance = (missFor30Tokens[_difficulty] * chanceModifier) / possibilities;
                return Rand.NextDouble() > missChance;
            }

        }

        public void PlayTurn(IGameController gameController, Object player) {
            _isInteractive = false;

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
            //===================

            //This turn compulsions
            //---------------------

            //Look for a winning possibility and complete it
            var potentialWinningTiles = AIHelper.FindDirectWinningPossibilities(
                gameController.Game.Board, 
                player);

            if (potentialWinningTiles.Count != 0 
                    && WillSeeImmediateWin(potentialWinningTiles.Count, gameController.Game.Board)) {
                //Depending on how easy the difficulty is, and how many tokens are on the board
                //the AI should be able to miss a chance to win. There should be more chance
                //to see the win the more potential winning tiles there are.
                PlaceTokenInOneOfTheseTiles(gameController, potentialWinningTiles);
                return;
            }

            //Look to see if the opponent will be able to complete his move next turn, and block it.
            var potentialLosingTiles = AIHelper.FindDirectWinningPossibilities(
                gameController.Game.Board,
                gameController.Game.GetOtherPlayer(player));

            if (potentialLosingTiles.Count != 0
                    && WillSeeImmediateLoss(potentialLosingTiles.Count, gameController.Game.Board)) {
                //Depending on how easy the difficulty is, and how many tokens are on the board
                //the AI should be able to miss a chance to stop the opponent. There should
                //be less chance to miss the more potential losing tiles there are.
                PlaceTokenInOneOfTheseTiles(gameController, potentialLosingTiles);
                return;
            }

            //Two turn compulstions
            //---------------------

            if (CanCreateCompulsion(gameController.Game.Board)) {
                //Look to see if we have a move that will force the other player into giving us the win
                //in the next turn, and do it.
            
                var compulsionTiles = AIHelper.FindTwoTurnCompulsionWin(gameController.Game, player);
                if (compulsionTiles.Count != 0) {
                    PlaceTokenInOneOfTheseTiles(gameController, compulsionTiles);
                    return;
                }
            }

            if (CanBlockOpponentCompulsion(gameController.Game.Board)) { //when there's a small amount of 
                //Look for the above thing for the opponent. A move which will force us to make a defensive blocking 
                //move, and give him the win on the next turn.
                //Note that FindCompulsionWinBlockingTiles() will not return any blocks if there is a compulsion
                //and it can't be blocked
                var compulsionBlocks = FindCompulsionWinBlockingTiles(gameController, player);
                if (compulsionBlocks.Count != 0) {
                    PlaceTokenInOneOfTheseTiles(gameController, compulsionBlocks);
                    return;
                }
            }
            

            //Three plus turn compulsions
            //---------------------------

            //Three turn compulsions are less likley to occur in the game, so I'm not going to consider them at 
            //this point. Though doing so should just be a modification and additional search based on the 
            //two-turn compulsion


            //==== Mid And Early Game Moves ====
            //==================================

            //Now go through and rate each of the playable tiles
            var ratings = RatePosibilePlacements(gameController.Game, player);

            if (ratings.Count != 0) {
                List<Tile> placementOptions = null;
                if (_UseFuzzyLogicOnRatings) {
                    placementOptions = ChoosePlacementBasedOnFuzzyLogic(ratings);
                } else {
                    placementOptions = ChoosePlacementTilesBasedOnHighestRating(ratings);
                }

                PlaceTokenInOneOfTheseTiles(gameController, placementOptions);
                return;
            }


            // If there are no good options, then place a token as low and centeral as possible.
            // (I'm not actually sure if this is a very optimial strategy. I would have to do some testing
            //with different options (random, low and sides, high and central, high and sides, third poins, etc))


            //For testing, just randomly place the token.

            //For testing we are going to make it interactive.
            _isInteractive = true;

        }

        public List<Tile> FindCompulsionWinBlockingTiles(IGameController gameController, Object player) {
            var opponentCompulsionTiles = AIHelper.FindTwoTurnCompulsionWin(gameController.Game, 
                gameController.Game.GetOtherPlayer(player));

            //We may or may not be able to block this compulsion... We'll need to check each
            //column to see if placing a tile wouldn't create a direct win or compulsion. If we can find
            //one that doesn't we should drop a token there.
            var compulsionBlockingTiles = new List<Tile>();
            opponentCompulsionTiles.ForEach(tile => {
                var copyGame = gameController.Game.Copy();
                copyGame.DropTokenOnColumn(tile.ColumnNo);

                var winningPossiblities =
                    AIHelper.FindDirectWinningPossibilities(copyGame.Board, copyGame.CurrentPlayer);

                var twoTurnCompulsions = AIHelper.FindTwoTurnCompulsionWin(copyGame, copyGame.CurrentPlayer);

                if (winningPossiblities.Count == 0 && twoTurnCompulsions.Count == 0) {
                    compulsionBlockingTiles.Add(tile);
                }
            });

            return compulsionBlockingTiles;

            //Hmm this method is a bit brute forced. I wonder if there's different situations and cases we can
            //look at to block the compulsion... In many cases where the other player must place a tile to
            //start a compulsion, we could block by placing one there first. That's because it depends on
            //us building the final tile, which we can prematuraley stop. Other cases it doesn't matter
            //what we do, because placing the blocking tile gives them the opportunity to win straight away.

            //
            //
            //
            // . . x x . . .
            //////////////////
            //In the above case placing a token to either side of the x's will block the compulsion

            // x x x
            // t t t
            // t t t
            // t t t x
            // t t t x . . .
            /////////////////
            //In this case if we place a token above the 4th column we will block a compulsion. If
            //we don't then the user will force a compulsion on us 

            //   
            // x x x 
            // t x t
            // x t t t . . .
            ////////////////
            //In this case there is nothing we can do to stop a compulsion. If we place a token in the
            //4th column the opponent will win in the next turn. If they place one there then we will
            //have to block it and they will win on the diagonal.

            //In all the above cases where we can block the compulsion, we do it by placing a token
            //on the same column that the opponent would. Are there situations where would need
            //to place a token elsewhere to stop the compulsion? None that I can think of...

        }

        public void PlaceTokenInOneOfTheseTiles(IGameController gameController, List<Game.Tile> tiles) {
            if (tiles.Count == 1) {
                gameController.Game.DropTokenOnColumn(tiles[0].ColumnNo);
            } else {
                var rand = new Random();
                var randomTile = tiles[ rand.Next(tiles.Count - 1) ];
                gameController.Game.DropTokenOnColumn(randomTile.ColumnNo);
            }
        }

        public List<Tile> ChoosePlacementBasedOnFuzzyLogic(Dictionary<int, List<Tile>> ratings) {
            //Apply the fuzzy distortion to each rating
            var distoredList = new Dictionary<int, List<Tile>>();
            foreach (int value in ratings.Keys) {
                distoredList.Add(value + _FuzzyDistortion, ratings[value]);
            }

            //work out what the positive total is for normalization.
            var positiveTotal = 0;
            foreach (int value in ratings.Keys) {
                if (value > 0) {
                    positiveTotal += value * ratings[value].Count;
                }
            }

            if (positiveTotal == 0) {
                return ChoosePlacementTilesBasedOnHighestRating(ratings);
            }

            //create a list of the normalized values
            var normalizedItems = new List<KeyValuePair<double, Tile>>();
            foreach (int value in ratings.Keys) {
                if (value > 0) {
                    var normalizedValue = Convert.ToDouble(value) / positiveTotal;
                    foreach(Tile t in ratings[value]) {
                        normalizedItems.Add(new KeyValuePair<double,Tile>(normalizedValue, t));
                    }
                }
            }

            //now randomly pick an item
            var r = Rand.NextDouble();
            double currentTotal = 0;
            foreach (var pair in normalizedItems) {
                currentTotal += pair.Key;
                if (r < currentTotal) {
                    return new List<Tile>() { pair.Value };
                }
            }

            Debug.Assert(false, "We shouldn't have got here!");

            return new List<Tile>() { normalizedItems[normalizedItems.Count - 1].Value };
        }

        public List<Tile> ChoosePlacementTilesBasedOnHighestRating(Dictionary<int, List<Tile>> ratings) {
            var sortedRatings = ratings.Keys.ToList();
            sortedRatings.Sort();
            return ratings[sortedRatings[sortedRatings.Count() - 1]];
        }

        //Test Me
        public Dictionary<int, List<Tile>> RatePosibilePlacements(Game.GameCore gameCore, Object player) {
            //Assumptions: If we could win, or the other player could win we
            //would have placed a tile already and not reached this step.

            //Get all the placeable tiles on the board, then go through each of them and rate how
            //good each placement would be.

            //After doing all this we need to make sure we're not placing a token where we would be giving the
            //opponent a winning position.
            var board = gameCore.Board;
            var placeableTiles = gameCore.Board.GetAllPlaceableTiles();

            var ratings = new Dictionary<int, List<Tile>>();

            foreach (Tile pt in placeableTiles) {
                int rating = GetRatingForTile(gameCore, pt, player);
                if (!ratings.ContainsKey(rating)) {
                    ratings.Add(rating, new List<Tile>());
                }
                ratings[rating].Add(pt);
            }

            return ratings;
        }


        //Test Me
        public int GetRatingForTile(GameCore gameCore, Tile tile, Object player) {

            int rating = 0;

            //First check if placing a tile here will force the opponent to block us, or give the
            //opponent a wining move. We shouldn't consider those tiles.
            var copy = gameCore.Copy();
            copy.DropTokenOnColumn(tile.ColumnNo);
            var otherPlayerDirectWinnable = AIHelper.FindDirectWinningPossibilities(copy.Board, gameCore.GetOtherPlayer(player));
            if (otherPlayerDirectWinnable.Count > 0) {

                //lower skilled players can miss an opponents winning move, especially if they are
                //focused on their own win
                if (_difficulty == AIDifficulty.VeryEasy) {  rating -= 2; }
                else if (_difficulty == AIDifficulty.Easy) {  rating -= 4; }
                else if (_difficulty == AIDifficulty.Medium) {  rating -= 6; }
                else {
                    //the other player can win if we play here, so don't play
                    return -1000;
                }
            }

            var usDirectWinnable = AIHelper.FindDirectWinningPossibilities(copy.Board, player);
            if (usDirectWinnable.Count > 0) {
                //we can win next turn if we place here, forcing the other player to make a blocking move.
                //It may be worth making the move anyway if it gives us a good position, and there isn't
                //anything better, so don't completley discount it.
                if (CanCreateCompulsion(copy.Board)) {
                    rating -= 3;
                } else {
                    //if the player can't create compulsions we shouldn't penalize them for
                    //trying to win

                    if (_difficulty < AIDifficulty.Medium) {
                        //lower skilled players will chase a win
                        rating += 2;
                    }
                }
            }
            
            //We are trying to create general opportunities to head into the end game by creating
            //potential token connections. So we want to look at where we have potential connections 
            //and try to build that area up to generate a compulsion.
            rating += (FindTilesInConnection(gameCore, tile, player).Count * 2);
            

            //Future ToDos
            //Now check the opponents tile connections. Add points based on how many they have, sine we're
            //blocking their position
            if (_tryToBlockOpponentsGoodConnections) {
                rating += FindTilesInConnection(gameCore, tile, gameCore.GetOtherPlayer(player)).Count;
            }

            //Check what will happen once we play our tile. Does it open up any good positions?

            //Do some planning stuff now:

            //If placing a tile here will start a compulsion for us, add points
            //If placing a tile here will block an opponents compulsion then add some points.
            //Check the tile connections for the tile above this one. If the opponent would have good
            //connections then subtract points. If we would have a good connections subtract points
            //(since it alows the opponent to block us.

            return rating;
        }

        /// <summary>
        /// Method finds the amount of player tokens around the tile which are in a good connection
        /// to it.
        /// </summary>
        public List<Tile> FindTilesInConnection(GameCore gameCore, Tile tile, Object player) {
            //To do this we get all the potential tile connections that contain this placeable tile.
            var possibleConnections = AIHelper.FindFourTokenConnectionPossibilties(gameCore.Board, player);
            var containingConnections = (from pc in possibleConnections 
                                        where pc.Tiles.Contains(tile) 
                                        select pc).ToList();

            //And add points based on how many of our tokens in tile connections it is part of. More
            //is better
            var playerTilesInConnection = (from tc in containingConnections
                                           from t in tc.Tiles
                                           where t.OwningPlayer == player
                                           select t).ToList();

            return playerTilesInConnection;
        }
    }
}
