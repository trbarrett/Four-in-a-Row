using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.UI;

namespace FourRow.Tests.Unit.AI
{
    class AIBattleGameController : IGameController
    {
        private Player _player1;
        private Player _player2;
        private Game.GameCore _game;

        public FourRow.Game.GameCore Game {
            get { return _game; }
        }

        public IPlayer CurrentPlayer {
            get { return (IPlayer)_game.CurrentPlayer; }
        }

        public AIBattleGameController(Player player1, Player player2) {
            _player1 = player1;
            _player2 = player2;

            _game = new Game.GameCore(_player1, _player2);
        }

        public void PlayAIGame() {
            while (!_game.IsGameFinished()) {
                CurrentPlayer.PlayTurn(this);
            }
        }

        public Player GetWinningPlayer() {
            if (!_game.IsGameFinished()) { return null; }
            return DeterminePlayer(_game.WinningPlayer);
        }

        private Player DeterminePlayer(Object player) {
            if (player == _player1) {
                return _player1;
            } else if (player == _player2) {
                return _player2;
            } else {
                throw new ApplicationException(string.Format("Unknown player object: {0}", player.ToString()));
            }
        }
    }
}
