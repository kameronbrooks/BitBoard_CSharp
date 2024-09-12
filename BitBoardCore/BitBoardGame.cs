using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace BitBoardCore
{
    public class BitBoardGame
    {
        const uint INITIAL_PLAYER_1_STATE = 0x0000_0FFFu;   // Player 1 Initial State bits
        const uint INITIAL_PLAYER_2_STATE = 0xFFF0_0000u;   // Player 2 Initial State bits   

        public const int PLAYER_1 = 0;
        public const int PLAYER_2 = 1;

        public const int CELL_SIZE = 60;

        public const int BOARD_COLUMNS = 8;
        public const int BOARD_ROWS = 8;

        uint[] _boards;
        int _currentTurn;

        public BitBoardGame()
        {
            _boards = new uint[2];
            _currentTurn = 0;
            
        }

        public void InitializeBoard()
        {
            _boards[PLAYER_1] = INITIAL_PLAYER_1_STATE;
            _boards[PLAYER_2] = INITIAL_PLAYER_2_STATE;
        }

        public int GetOtherPlayerID(int playerID)
        {
            return ((playerID + 1) % 2);
        }

        public void MovePiece(int playerID, int fromBit, int toBit)
        {
            // Move the bit to the new position from the old position
            _boards[playerID] = BitUtility.SwapBit(_boards[playerID], fromBit, toBit);
            // Get the id of the other player
            int otherPlayerID = GetOtherPlayerID(playerID);

            // Check to see if the cell is occupied by a piece on the other team
            if (BitUtility.CheckBit(_boards[otherPlayerID], toBit) == 1)
            {
                // If so, capture it
                CapturePiece(otherPlayerID, toBit);
            }
        }

        public bool IsLegalMove(int playerID, int fromBit, int toBit)
        {
            return true;
        }

        public void CapturePiece(int playerID, int bit)
        {

        }

        public override string ToString()
        {
            return ToString("b");
        }

        public string ToString(string format)
        {
            return "Board";
        }

        public void UpdateGameState()
        {

        }

        public void Render(System.Drawing.Graphics graphics)
        {

            graphics.Clear(Color.Black);

            RenderBoard(graphics);

            RenderPieces(graphics);
            

            //graphics.FillRectangle(darkBrush, new Rectangle() { X = 0, Y = 0, Height = CELL_SIZE, Width = CELL_SIZE});
        }
        
        protected void RenderBoard(System.Drawing.Graphics graphics)
        {
            Brush darkBrush = new SolidBrush(Color.Brown);
            Brush lightBrush = new SolidBrush(Color.Tan);

            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (((i + j) % 2) == 1)
                    {
                        graphics.FillRectangle(darkBrush, new Rectangle() { X = CELL_SIZE * j, Y = CELL_SIZE * i, Height = CELL_SIZE, Width = CELL_SIZE });
                    }
                    else
                    {
                        graphics.FillRectangle(lightBrush, new Rectangle() { X = CELL_SIZE * j, Y = CELL_SIZE * i, Height = CELL_SIZE, Width = CELL_SIZE });
                    }
                }
            }
        }

        protected void RenderPiece(System.Drawing.Graphics graphics, Brush brush, int cx, int cy)
        {
            int size = (int)(CELL_SIZE * 0.75f);
            graphics.FillEllipse(brush, new Rectangle() { X = CELL_SIZE * cx + size / 5, Y = (CELL_SIZE*7) - (CELL_SIZE * cy) + size / 5, Height = size, Width = size });
        }

        protected void RenderPieces(System.Drawing.Graphics graphics)
        {
            Brush redBrush = new SolidBrush(Color.Red);
            Brush blackBrush = new SolidBrush(Color.Black);

            for(int i = 0; i < sizeof(uint)*8; i++)
            {
                int boardIndex = i * 2;
                int y = boardIndex / BOARD_COLUMNS;
                int x = boardIndex - (y * BOARD_COLUMNS) + (y % 2);     // Get the x position based on the cell id


                if(BitUtility.CheckBit(_boards[PLAYER_1], i) == 1)
                {
                    RenderPiece(graphics, redBrush, x, y);
                }
                if (BitUtility.CheckBit(_boards[PLAYER_2], i) == 1)
                {
                    RenderPiece(graphics, blackBrush, x, y);
                }
            }
        }
    }
}
