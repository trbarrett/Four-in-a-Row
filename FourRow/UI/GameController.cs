using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourRow.UI
{
    public class GameController : IGameController
    {
        private Player _player1;
        private Player _player2;
        private Game.GameCore _game;

        //I'm not sure if the UI game controller should have a reference to the ui forms. Perhaps
        //it should just comunicate to it through events? At least it should use an Iinterface.
        private MainForm _mainForm;
        private DropBoard _dropBoard;

        public Player Player1 {
            get { return _player1; }
        }

        public Player Player2 {
            get { return _player2; }
        }

        public Game.GameCore Game {
            get { return _game; }
        }

        public IPlayer CurrentPlayer {
            get { return (Player)_game.CurrentPlayer; }
        }

        public GameController(MainForm mainForm, DropBoard dropBoard, Player player1, Player player2) {
            _mainForm = mainForm;
            _dropBoard = dropBoard;
            _player1 = player1;
            _player2 = player2;

            _game = new Game.GameCore(_player1, _player2);
            _game.GameStateChanged += new EventHandler(OnGameStateChanged);
        }

        public void OnGameStateChanged(Object sender, EventArgs e) {
            _dropBoard.UpdateState();
            if (_game.IsGameFinished()) {
                _mainForm.SetFinishedState(_game);
                _dropBoard.SetFinishedState();

            } else {
                IPlayer player = (IPlayer) _game.CurrentPlayer;
                //Let the dropboard know what player's turn it is so that it
                //can display that information, and if the player is an AI,
                //give it control.
                _dropBoard.SetCurrentPlayer(player);

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

        public void DropTokenOnColumn(UIBoardColumn uiColumn) {
            if (!uiColumn.GameColumn.IsFull) {
                _game.DropTokenOnColumn(uiColumn.GameColumn.ColumnNo);
            }
        }


    }
}
