namespace BitBoard_CSharp
{
    public partial class Form1 : Form
    {
        BitBoardCore.BitUtility _bitUtility;
        public Form1()
        {
            InitializeComponent();
            _bitUtility = new BitBoardCore.BitUtility();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
