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

        //These control the status of the ai in the new game form. They are updated
        //with the current details when the user creates a new game.
        private IAI _newGamePlayer1AIStatus = null; //we start of with player1 being human
        private IAI _newGamePlayer2AIStatus = new AI.AIBase(AI.AIDifficulty.Medium); //and player2 being a medium ai

        public MainForm() {
            InitializeComponent();
            ClearState();
        }

        public void SetFinishedState(Game.GameCore game) {
            if (game.IsStatemate) {
                lblStateDescription.Text = "The game is a stalemate";
            } else {
                lblStateDescription.Text = string.Format("{0} has won the game", _gameController.GetWinningPlayer().PlayerName);
                lblStateDescription.BackColor = _gameController.GetWinningPlayer().HighlightColour;
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e) {
            using (var newGameForm = new NewGame()) {
                newGameForm.Player1AI = _newGamePlayer1AIStatus;
                newGameForm.Player2AI = _newGamePlayer2AIStatus;

                if (newGameForm.ShowDialog() == DialogResult.OK) {

                    _newGamePlayer1AIStatus = newGameForm.Player1AI;
                    _newGamePlayer2AIStatus = newGameForm.Player2AI;

                    Player player1 = new Player("Player 1", 
                        Player.Player1TokenColour, 
                        Player.Player1HighlightColour, 
                        newGameForm.Player1AI);
                    Player player2 = new Player("Player 2", 
                        Player.Player2TokenColour, 
                        Player.Player2HighlightColour, 
                        newGameForm.Player2AI);

                    _gameController = new GameController(this, dropBoard, player1, player2);
                    dropBoard.InitializeGameState(_gameController);
                }
            }
        }

        private void ClearState() {
            lblStateDescription.Text = "";
            lblStateDescription.BackColor = SystemColors.Control;
        }

    }
}
