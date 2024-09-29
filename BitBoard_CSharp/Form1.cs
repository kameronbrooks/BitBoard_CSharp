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

        Image? _checkerIcon = null;
        Image? _kingIcon = null;
        Image? _moveIcon = null;
        bool _resourcesLoaded = false;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();

            LoadResources();

            StartGame();


            Debug.WriteLine(BitUtility.ClearBit(0xFFF, 13));
            Debug.WriteLine(BitUtility.SetBit(0x0, 13));


        }

        private void LoadResources()
        {

            using (MemoryStream stream = new MemoryStream(Properties.Resources.checker_icon))
            {
                _checkerIcon = Image.FromStream(stream);
            }
            using (MemoryStream stream = new MemoryStream(Properties.Resources.king))
            {
                _kingIcon = Image.FromStream(stream);
            }
            using (MemoryStream stream = new MemoryStream(Properties.Resources.move_icon))
            {
                _moveIcon = Image.FromStream(stream);
            }
            _resourcesLoaded = true;

        }

        private void OnGameOver(int gameState)
        {
            gameOverLabel.Visible = true;
            winnerLabel.Visible = true;
            if (gameState == BitBoardGame.PLAYER_1)
                winnerLabel.Text = "Red Wins!";
            else if (gameState == BitBoardGame.PLAYER_2)
                winnerLabel.Text = "Black Wins!";
            else
            {
                winnerLabel.Text = "Tie Game!";
            }

        }

        private void StartGame()
        {
            // Load the images if they have not been loaded yet
            if (!_resourcesLoaded)
            {
                LoadResources();
            }

            // Hide the labels
            gameOverLabel.Visible = false;
            winnerLabel.Visible = false;

            _game = new BitBoardGame();
            _game.onBoardChange = OnBoardUpdate;
            _game.onTurnChange = OnTurnUpdate;
            _game.onGameOver = OnGameOver;

            // Set the images used in the game rendering
            _game.SetResources(_checkerIcon, _kingIcon, _moveIcon);

            _game.InitializeBoard();
        }

        private void OnTurnUpdate(int turn)
        {
            turnDataLabel.Text = turn.ToString() + "  (" + ((turn & 1) == 0 ? "Red" : "Black") + ")";
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

        private void restartButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }
    }
}
