﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBoardCore
{
    public static class BoardBitStates
    {
        // ░░::░░::░░::░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░::░░::░░::░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░::░░::░░::░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░::░░::░░::░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        public const uint EVEN_ROWS = 0x0F0F0F0Fu;

        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░::░░::░░::░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░::░░::░░::░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░::░░::░░::░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░::░░::░░::░░
        public const uint ODD_ROWS = 0xF0F0F0F0u;

        // =-=-=-=-=-=-=-=-=-=-=-=-= PLAYER 1 =-=-=-=-=-=-=-=-=-=-=-=-=

        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        // ░░::░░::░░::░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        public const uint INITIAL_PLAYER_1_STATE = 0x0000_0FFFu;   // Player 1 Initial State bits
         
        // ░░::░░::░░::░░::
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        public const uint PLAYER1_LMOVE_MASK = 0xEFEFEFEu;

        // ░░::░░::░░::░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        public const uint PLAYER1_RMOVE_MASK = 0xF7F7F7Fu;

        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        // ░░::░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░::░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░::░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        public const uint PLAYER1_LJUMP_MASK = 0xEEEEEEu;

        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░::░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░::░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░::░░
        public const uint PLAYER1_RJUMP_MASK = 0x777777u;

        // =-=-=-=-=-=-=-=-=-=-=-=-= PLAYER 2 =-=-=-=-=-=-=-=-=-=-=-=-=

        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░::░░::░░::░░
        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        public const uint INITIAL_PLAYER_2_STATE = 0xFFF0_0000u;   // Player 2 Initial State bits  

        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░▉▉
        // ::░░::░░::░░::░░
        public const uint PLAYER2_LMOVE_MASK = 0xFEFEFEF0u;

        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░▉▉░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ::░░::░░::░░::░░
        public const uint PLAYER2_RMOVE_MASK = 0x7F7F7F70u;

        // ░░::░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░::░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░::░░▉▉░░▉▉░░▉▉
        // ::░░▉▉░░▉▉░░▉▉░░
        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        public const uint PLAYER2_LJUMP_MASK = 0xEEEEEE00u;

        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░::░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░::░░
        // ░░▉▉░░▉▉░░▉▉░░::
        // ▉▉░░▉▉░░▉▉░░::░░
        // ░░::░░::░░::░░::
        // ::░░::░░::░░::░░
        public const uint PLAYER2_RJUMP_MASK = 0x77777700u;

    }
}
