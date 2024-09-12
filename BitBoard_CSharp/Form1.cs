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
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            _game.Render(e.Graphics);
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
            this.Invalidate();

        }
    }
}
