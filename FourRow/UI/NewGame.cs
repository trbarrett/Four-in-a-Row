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
    public partial class NewGame : Form
    {

        public enum PlayerOptions {
            Human,
            AI
        }

        private IAI _player1AI = null;
        private IAI _player2AI = null;
        private bool _updateingPlayerAI = false;

        public IAI Player1AI {
            get { return _player1AI; }
            set { _player1AI = value; }
        }

        public IAI Player2AI {
            get { return _player2AI; }
            set { 
                _player2AI = value;
                UpdatePlayerComboValues(_player2AI, cmbPlayer2Option, cmbPlayer2AIDifficulty);
            }
        }

        public NewGame() {
            InitializeComponent();
            SetupComboBoxPlayerOptions(cmbPlayer2Option, cmbPlayer2AIDifficulty);
        }

        public void SetupComboBoxPlayerOptions(ComboBox playerCombo, ComboBox aiDifficultyCombo) {
            foreach (String option in Enum.GetNames(typeof(PlayerOptions))) {
                playerCombo.Items.Add(option);
            }
            playerCombo.Text = PlayerOptions.AI.ToString();

            foreach (String option in Enum.GetNames(typeof(AI.AIDifficulty))) {
                aiDifficultyCombo.Items.Add(option);
            }
            aiDifficultyCombo.Text = AI.AIDifficulty.Medium.ToString();
        }

        private void UpdatePlayerComboValues(IAI playerAI, ComboBox playerCombo, ComboBox aiDifficultyCombo) {
            _updateingPlayerAI = true;
            if (playerAI == null) {
                playerCombo.Text = PlayerOptions.Human.ToString();
                aiDifficultyCombo.Visible = false;
            } else {
                playerCombo.Text = PlayerOptions.AI.ToString();
                aiDifficultyCombo.Text = playerAI.Difficulty.ToString();
                aiDifficultyCombo.Visible = true;
            }
            _updateingPlayerAI = false;
        }

        private void PlayerOrAIOptionChanged(ref IAI playerAI, ComboBox playerCombo, ComboBox aiDifficultyCombo) {
            if (_updateingPlayerAI) {
                return;
            }

            if (playerCombo.Text == PlayerOptions.Human.ToString()) {
                playerAI = null;
                aiDifficultyCombo.Visible = false;

            } else if (playerCombo.Text == PlayerOptions.AI.ToString()) {
                AI.AIDifficulty diff = FourRow.AI.AIDifficulty.Medium;
                if (!string.IsNullOrEmpty(aiDifficultyCombo.Text)) {
                    diff = (AI.AIDifficulty)Enum.Parse(typeof(AI.AIDifficulty), aiDifficultyCombo.Text);
                }
                playerAI = new AI.AIBase(diff);
                cmbPlayer2AIDifficulty.Visible = true;
            }
        }

        private void cmbPlayer2Option_SelectedIndexChanged(object sender, EventArgs e) {
            PlayerOrAIOptionChanged(ref _player2AI, cmbPlayer2Option, cmbPlayer2AIDifficulty);
        }

        private void cmbPlayer2AIDifficulty_SelectedIndexChanged(object sender, EventArgs e) {
            PlayerOrAIOptionChanged(ref _player2AI, cmbPlayer2Option, cmbPlayer2AIDifficulty);
        }

    }
}
