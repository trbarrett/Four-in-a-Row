using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FourRow.UI
{
    public partial class MainForm : Form
    {
        private GameController _gameController;

        public MainForm() {
            InitializeComponent();
            ClearState();
        }

        public void SetFinishedState(Game.GameCore game) {
            lblStateDescription.Text = string.Format("{0} has won the game", _gameController.GetWinningPlayer().PlayerName);
            lblStateDescription.BackColor = _gameController.GetWinningPlayer().HighlightColour;
        }

        private void btnNewGame_Click(object sender, EventArgs e) {
            _gameController = new GameController(this, dropBoard);
            dropBoard.InitializeGameState(_gameController);
        }

        private void StartNewGame() {
            ClearState();
            _gameController = new GameController(this, dropBoard);
            dropBoard.InitializeGameState(_gameController);
        }

        private void ClearState() {
            lblStateDescription.Text = "";
            lblStateDescription.BackColor = SystemColors.Control;
        }

    }
}
