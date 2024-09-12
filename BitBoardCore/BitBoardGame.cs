using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BitBoardCore
{
    public class BitBoardGame
    {
        const uint INITIAL_PLAYER_1_STATE = 0x0000_0FFFu;   // Player 1 Initial State bits
        const uint INITIAL_PLAYER_2_STATE = 0xFFF0_0000u;   // Player 2 Initial State bits   

        public const int PLAYER_1 = 0;
        public const int PLAYER_2 = 1;

        uint[] _boards;

        public BitBoardGame()
        {
            _boards = new uint[2];
            
        }

        public void InitializeBoard()
        {
            _boards[0] = INITIAL_PLAYER_1_STATE;
            _boards[1] = INITIAL_PLAYER_2_STATE;
        }

        public void MovePiece(int playerID, int fromBit, int toBit)
        {

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
    }
}
