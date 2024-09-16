using BitBoardCore;
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
        }
    }
}
