namespace BitBoard_CSharp
{
    public partial class Form1 : Form
    {
        BitBoardCore.BitBoardGame _game;
        public Form1()
        {
            InitializeComponent();
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
    }
}
