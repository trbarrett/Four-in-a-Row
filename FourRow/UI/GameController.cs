using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourRow.UI
{
    public class GameController
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

        public void InitializeGame() {
            _player1 = new Player("Player 1", Player.Player1TokenColour, Player.Player1HighlightColour);
            _player2 = new Player("Player 2", Player.Player2TokenColour, Player.Player2HighlightColour);
            _game = new Game.GameCore(_player1, _player2);

            _game.GameStateChanged += new EventHandler(
                (sender, e) => {
                    _dropBoard.UpdateState();
                    if (_game.IsGameFinished()) {
                        _mainForm.SetFinishedState(_game);
                        _dropBoard.SetFinishedState();
                    }
                });
        }

        public GameController(MainForm mainForm, DropBoard dropBoard) {
            _mainForm = mainForm;
            _dropBoard = dropBoard;
            InitializeGame();
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
                _game.DropTokenOnColumn(uiColumn.GameColumn);
            }
        }


    }
}
