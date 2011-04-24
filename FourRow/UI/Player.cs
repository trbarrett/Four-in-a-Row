using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourRow.UI
{

    public class Player : IPlayer
    {

        public static readonly Color Player1TokenColour = Color.Red;
        public static readonly Color Player1HighlightColour = Color.Pink;

        public static readonly Color Player2TokenColour = Color.Blue;
        public static readonly Color Player2HighlightColour = Color.LightBlue;

        private string _name;
        private Color _tokenColour;
        private Color _highlightColour;
        private IAI _ai;

        public string PlayerName { get {return _name;}}
        public Color TokenColour { get { return _tokenColour; } }
        public Color HighlightColour { get { return _highlightColour; } }
        public IAI AI { get { return _ai; } }

        public bool IsInteractive() {
            if (AI != null) {
                return AI.IsInteractive();
            } else {
                return true;
            }
        }

        public void PlayTurn(IGameController gameController) {
            if (_ai != null) {
                _ai.PlayTurn(gameController, this);
            } else {
                //The the human player take their turn as they may
            }
        }

        public Player(string name, Color tokenColour, Color highlightColour, IAI ai) : 
                this(name, tokenColour, highlightColour) {
            _ai = ai;
        }

        public Player(string name, Color tokenColour, Color highlightColour) {
            _name = name;
            _tokenColour = tokenColour;
            _highlightColour = highlightColour;
        }

        public static Player NewPlayer1(IAI ai) {
            return new Player("Player 1", Player1TokenColour, Player1HighlightColour, ai);
        }

        public static Player NewPlayer2(IAI ai) {
            return new Player("Player 2", Player2TokenColour, Player2HighlightColour, ai);
        }
    }
}
