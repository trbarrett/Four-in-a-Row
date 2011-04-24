using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FourRow.UI
{
    public partial class DropBoard: UserControl
    {
        private readonly int StartTop = 2;
        private readonly int StartLeft = 2;

        private GameController _currentGame;
        private bool _gameFinished = false;

        private Timer flashTimer;
        private bool _flashOn = false;

        protected List<UIBoardColumn> _uiBoardColumns = new List<UIBoardColumn>();

        public DropBoard() {
            InitializeComponent();
            //this.DoubleBuffered = true;
        }

        public void InitializeGameState(GameController gameController) {
            _gameFinished = false;
            _currentGame = gameController;
            SetupBoard();
            this.Invalidate();
        }

        public void UpdateState() {
            this.Invalidate();
        }

        public void SetFinishedState() {
            _gameFinished = true;

            flashTimer = new Timer();
            flashTimer.Interval = 500;
            flashTimer.Tick += new EventHandler(flashTimer_Tick);
            flashTimer.Start();

            _flashOn = true;
            UpdateFlashState(_flashOn);
        }

        void flashTimer_Tick(object sender, EventArgs e) {
            _flashOn = !_flashOn;
            UpdateFlashState(_flashOn);
            this.Invalidate();
        }

        public void UpdateFlashState(bool flashOn) {
            List<Game.Tile> tiles = _currentGame.Game.GetWinningTiles();
            IEnumerable<UIBoardTile> winningUITiles =
                from tile in tiles
                select _uiBoardColumns[tile.ColumnNo][tile.RowNo];

            winningUITiles.ToList().ForEach(uiTile => {
                uiTile.UpdateHighlightState(flashOn);
            });
        }

        public void SetupBoard() {
            if (flashTimer != null) {
                flashTimer.Dispose();
                flashTimer = null;
            }
            _flashOn = false;

            _uiBoardColumns = new List<UIBoardColumn>();

            if (_currentGame == null || _currentGame.Game == null || _currentGame.Game.Board == null) {
                return;
            }

            var left = StartLeft;
            _currentGame.Game.Board.Columns.ForEach(col => {
                var uiColumn = new UIBoardColumn(this, col);
                uiColumn.Position = new Point(left, StartTop);
                _uiBoardColumns.Add(uiColumn);
                left += uiColumn.Width;
            });
        }

        public void ResetBoard() {
            this.Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            //once the game is finished stop responding to mouse clicks
            if (_gameFinished) {
                return;
            }

            //Find which column we've clicked on so we can tell the game controller
            //to drop a tile on it.
            _uiBoardColumns.ForEach( uiColumn => {
                if (uiColumn.ContainsPoint(e.Location)) {
                    _currentGame.DropTokenOnColumn(uiColumn);
                }
            });
        }

        protected override void OnPaint(PaintEventArgs e) {
            //base.OnPaint(e);
            Graphics g = null;
            try {
                g = this.CreateGraphics();
                g.Clear(this.BackColor);
                DrawGameBoard(g);

            } finally {
                if (g != null) { g.Dispose(); }
            }
        }

        private void DrawGameBoard(Graphics g) {
             if (this.DesignMode) {
                 DrawDesignMode(g);
             }

            if (_uiBoardColumns != null ) {
                _uiBoardColumns.ForEach( uiColumn => {
                    uiColumn.DrawColumn(g);
                });
            }
        }

        private void DrawDesignMode(Graphics g) {
            Pen p = new Pen(Brushes.Gray);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(p, new Rectangle(0, 0, this.Width - 1, this.Height - 1)); //-1 size or the bottom and right edges won't show
            p.Dispose();
        }

    }
}
