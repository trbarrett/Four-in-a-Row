using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourRow.UI
{
    public class Player
    {

        public static readonly Color Player1TokenColour = Color.Red;
        public static readonly Color Player1HighlightColour = Color.Pink;

        public static readonly Color Player2TokenColour = Color.Blue;
        public static readonly Color Player2HighlightColour = Color.LightBlue;

        private string _name;
        private Color _tokenColour;
        private Color _highlightColour;

        public string PlayerName { get {return _name;}}
        public Color TokenColour { get { return _tokenColour; } }
        public Color HighlightColour { get { return _highlightColour; } }

        public Player(string name, Color tokenColour, Color highlightColour) {
            _name = name;
            _tokenColour = tokenColour;
            _highlightColour = highlightColour;
        }
    }
}
