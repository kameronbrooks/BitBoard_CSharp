using BitBoardCore;
using System;
using System.Diagnostics;

namespace BitBoard_CSharp
{
    public partial class Form1 : Form
    {
        BitBoardCore.BitBoardGame _game;
        int _mouseX;
        int _mouseY;
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();

            _game = new BitBoardCore.BitBoardGame();
            _game.onBoardChange = OnBoardUpdate;
            _game.onTurnChange = OnTurnUpdate;
            _game.onGameOver = OnGameOver;

            LoadResources();


        }

        private void LoadResources()
        {
            Image checker_icon = null;
            Image king_icon = null;
            Image move_icon = null;

            using (MemoryStream stream = new MemoryStream(Properties.Resources.checker_icon))
            {
                checker_icon = Image.FromStream(stream);
            }
            using (MemoryStream stream = new MemoryStream(Properties.Resources.king))
            {
                king_icon = Image.FromStream(stream);
            }
            using (MemoryStream stream = new MemoryStream(Properties.Resources.move_icon))
            {
                move_icon = Image.FromStream(stream);
            }

            _game.SetResources(checker_icon, king_icon, move_icon);
        }

        private void OnGameOver(int gameState)
        {

        }

        private void OnTurnUpdate(int turn)
        {
            turnDataLabel.Text = turn.ToString() + "  (" + ((turn % 2) == 0 ? "Red" : "Black") + ")";
        }

        private void OnBoardUpdate(uint player1Board, uint player2Board)
        {
            player1BinaryDataLabel.Text = BitUtility.ToBinaryString(player1Board);
            player1HexDataLabel.Text = BitUtility.ToHexString(player1Board);

            player2BinaryDataLabel.Text = BitUtility.ToBinaryString(player2Board);
            player2HexDataLabel.Text = BitUtility.ToHexString(player2Board);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _game.InitializeBoard();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            _game.SetMousePosition(e.X, e.Y);
            doubleBufferedPanel1.Invalidate();


        }

        private void doubleBufferedPanel1_Paint(object sender, PaintEventArgs e)
        {
            _game.Render(e.Graphics);
        }

        private void doubleBufferedPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            _game.SetMousePosition(e.X, e.Y);
            doubleBufferedPanel1.Invalidate();

            hoverBitMaskDataLabel.Text = BitUtility.ToBinaryString(_game.hoverBitMask);
        }

        private void doubleBufferedPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            _game.OnMouseDown(e.X, e.Y);
            doubleBufferedPanel1.Invalidate();
        }
    }
}
